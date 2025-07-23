using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;

namespace Lagrange.Milky.Api.Handler.Request;

[Api("accept_group_invite_request")]
public class AcceptGroupInviteRequestHandler(BotContext bot) : IEmptyResultApiHandler<AcceptGroupInviteRequestParameter>
{
    private readonly BotContext _bot = bot;

    public async Task HandleAsync(AcceptGroupInviteRequestParameter parameter, CancellationToken token)
    {
        await _bot.SetGroupInviteAccept(parameter.GroupId, long.Parse(parameter.RequestID));
    }
}

public class AcceptGroupInviteRequestParameter(long groupId, string requestId)
{
    [JsonRequired]
    [JsonPropertyName("group_id")]
    public long GroupId { get; init; } = groupId;

    [JsonRequired]
    [JsonPropertyName("request_id")]
    public string RequestID { get; init; } = requestId;
}