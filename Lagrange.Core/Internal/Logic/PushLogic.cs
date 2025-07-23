using System.Text;
using System.Text.Json;
using System.Web;
using Lagrange.Core.Common;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Notify;
using Lagrange.Core.Message.Entities;
using Lagrange.Core.Utility.Binary;
using ProtoHelper = Lagrange.Core.Utility.ProtoHelper;

namespace Lagrange.Core.Internal.Logic;

[EventSubscribe<PushMessageEvent>(Protocols.All)]
internal class PushLogic(BotContext context) : ILogic
{
    public async ValueTask Incoming(ProtocolEvent e)
    {
        var messageEvent = (PushMessageEvent)e;

        switch ((Type)messageEvent.MsgPush.CommonMessage.ContentHead.Type)
        {
            case Type.GroupMessage:
            case Type.PrivateMessage:
            case Type.TempMessage:
                var message = await context.EventContext.GetLogic<MessagingLogic>().Parse(messageEvent.MsgPush.CommonMessage);
                if (message.Entities[0] is LightAppEntity {AppName: "com.tencent.qun.invite"} app)
                {
                    using var document = JsonDocument.Parse(app.Payload);
                    var root = document.RootElement;

                    string url = root.GetProperty("meta").GetProperty("news").GetProperty("jumpUrl").GetString() ?? throw new Exception("sb tx! Is this 'com.tencent.qun.invite'?");
                    var query = HttpUtility.ParseQueryString(new Uri(url).Query);
                    long groupUin = uint.Parse(query["groupcode"] ?? throw new Exception("sb tx! Is this '/group/invite_join'?"));
                    ulong sequence = ulong.Parse(query["msgseq"] ?? throw new Exception("sb tx! Is this '/group/invite_join'?"));
                    context.EventInvoker.PostEvent(new BotGroupInviteEvent(
                        sequence.ToString(),
                        message.Contact.Uin,
                        groupUin
                    ));
                    break;
                }
                context.EventInvoker.PostEvent(new BotMessageEvent(message, messageEvent.Raw));
                break;
            case Type.GroupMemberDecreaseNotice when messageEvent.MsgPush.CommonMessage.MessageBody.MsgContent is { } content:
                var decrease = ProtoHelper.Deserialize<GroupChange>(content.Span);
                if (decrease.DecreaseType == 3)
                {
                    var op = ProtoHelper.Deserialize<OperatorInfo>(decrease.Operator.AsSpan());
                    context.EventInvoker.PostEvent(
                        new BotGroupMemberDecreaseEvent(
                            decrease.GroupUin,
                            context.CacheContext.ResolveCachedUin(decrease.MemberUid) ?? 0,
                            context.CacheContext.ResolveCachedUin(op.Operator.Uid ?? "") ?? 0
                        )
                    );
                }
                else
                {
                    context.EventInvoker.PostEvent(
                        new BotGroupMemberDecreaseEvent(
                            decrease.GroupUin,
                            context.CacheContext.ResolveCachedUin(decrease.MemberUid) ?? 0,
                            context.CacheContext.ResolveCachedUin(Encoding.UTF8.GetString(decrease.Operator.AsSpan())) ?? 0
                        )
                    );
                }
                break;
            // Note: There's no seq in this event, don't use.
            // case Type.GroupInviteNotice when messageEvent.MsgPush.CommonMessage.MessageBody.MsgContent is { } content:
            //     var invite = ProtoHelper.Deserialize<GroupInvite>(content.Span);
            //     context.EventInvoker.PostEvent(new BotGroupInviteEvent(
            //         messageEvent.MsgPush.CommonMessage.ContentHead.Sequence.ToString(),
            //         context.CacheContext.ResolveCachedUin(invite.InviterUid) ?? 0,
            //         invite.GroupUin
            //     ));
            //     break;
            case Type.Event0x210:
                var pkgType210 = (Event0x210SubType)messageEvent.MsgPush.CommonMessage.ContentHead.SubType;
                switch (pkgType210)
                {
                    case Event0x210SubType.FriendRequestNotice when messageEvent.MsgPush.CommonMessage.MessageBody.MsgContent is { } content:
                        var friendRequest = ProtoHelper.Deserialize<FriendRequest>(content.Span);
                        context.EventInvoker.PostEvent(new BotFriendRequestEvent(
                                friendRequest.Info!.SourceUid,
                                messageEvent.MsgPush.CommonMessage.RoutingHead.FromUin,
                                friendRequest.Info.Message,
                                friendRequest.Info.Source?? string.Empty
                            ));
                        break;
                }
                break;
            case Type.Event0x2DC:
                var pkgType = (Event0x2DCSubType)messageEvent.MsgPush.CommonMessage.ContentHead.SubType;
                switch (pkgType)
                {
                    case Event0x2DCSubType.GroupGreyTipNotice20 when messageEvent.MsgPush.CommonMessage.MessageBody.MsgContent is {} content:
                        var packet = new BinaryPacket(content);
                        Int64 groupUin = packet.Read<Int32>(); // group uin
                        _ = packet.Read<byte>(); // unknown byte
                        var proto = packet.ReadBytes(Prefix.Int16 | Prefix.LengthOnly);
                        var greyTip = ProtoHelper.Deserialize<NotifyMessageBody>(proto);
                        var templates = greyTip.GeneralGrayTip.MsgTemplParam.ToDictionary(x => x.Name, x => x.Value);

                        if (!templates.TryGetValue("action_str", out var actionStr) && !templates.TryGetValue("alt_str1", out actionStr))
                        {
                            actionStr = string.Empty;
                        }

                        if (greyTip.GeneralGrayTip.BusiType == 12) // poke
                        {
                            context.EventInvoker.PostEvent(new BotGroupNudgeEvent(
                                groupUin,
                                uint.Parse(templates["uin_str1"]),
                                uint.Parse(templates["uin_str2"]))
                            );
                        }
                        break;
                }
                break;
        }
    }

    private enum Type
    {
        PrivateMessage = 166,
        GroupMessage = 82,
        TempMessage = 141,
        Event0x210 = 528,  // friend related event
        Event0x2DC = 732,  // group related event
        
        GroupMemberDecreaseNotice = 34,
        GroupRequestJoinNotice = 84, // directly entered
        GroupInviteNotice = 87,  // the bot self is being invited
    }
    
    private enum Event0x2DCSubType
    {
        GroupMuteNotice = 12,
        SubType16 = 16,
        GroupRecallNotice = 17,
        GroupGreyTipNotice21 = 21,
        GroupGreyTipNotice20 = 20,
    }

    private enum Event0x2DCSubType16Field13
    {
        GroupMemberSpecialTitleNotice = 6,
        GroupNameChangeNotice = 12,
        GroupTodoNotice = 23,
        GroupReactionNotice = 35,
    }
    
    private enum Event0x210SubType
    {
        FriendRequestNotice = 35,
        GroupMemberEnterNotice = 38,
        FriendDeleteOrPinChangedNotice = 39,
        FriendRecallNotice = 138,
        ServicePinChanged = 199, // e.g: My computer | QQ Wallet | ...
        FriendPokeNotice = 290,
        GroupKickNotice = 212,
        FriendRecallPoke = 321,
    }
}
