using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;

namespace Lagrange.Milky.Api.Handler.Request;

[Api("accept_group_invitation ")]
public class AcceptGroupInvitationRequestHandler(BotContext bot) : IEmptyResultApiHandler<AcceptGroupInvitationRequestParameter>
{
    private readonly BotContext _bot = bot;

    public async Task HandleAsync(AcceptGroupInvitationRequestParameter parameter, CancellationToken token)
    {
        await _bot.SetGroupInviteSelfAccept(parameter.GroupId, parameter.InvitationSeq);
    }
}

public class AcceptGroupInvitationRequestParameter(long groupId, long invitationSeq)
{
    [JsonRequired]
    [JsonPropertyName("group_id")]
    public long GroupId { get; } = groupId;

    [JsonRequired]
    [JsonPropertyName("invitation_seq")]
    public long InvitationSeq { get; } = invitationSeq;
}