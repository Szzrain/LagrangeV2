using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<SetGroupInviteSelfAcceptEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x10c8_1")]
internal class SetGroupInviteAcceptService: OidbService<SetGroupInviteSelfAcceptEventReq, SetGroupInviteSelfAcceptEventResp, D10C8Req, D10C8RespBody>
{
    private protected override uint Command => 0x10c8;

    private protected override uint Service => 1;

    private protected override Task<D10C8Req> ProcessRequest(SetGroupInviteSelfAcceptEventReq inviteSelfAccept, BotContext context)
    {
        return Task.FromResult(new D10C8Req
        {
            Accept = 1,
            Body = new D10C8ReqBody
            {
                Sequence = inviteSelfAccept.Sequence,
                EventType = 2,
                GroupUin = inviteSelfAccept.GroupUin
            }
        });
    }

    private protected override Task<SetGroupInviteSelfAcceptEventResp> ProcessResponse(D10C8RespBody response, BotContext context)
    {
        return Task.FromResult(SetGroupInviteSelfAcceptEventResp.Default);
    }
}