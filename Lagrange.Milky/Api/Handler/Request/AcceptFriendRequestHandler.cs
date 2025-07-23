using System.Text.Json.Serialization;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;

namespace Lagrange.Milky.Api.Handler.Request;

[Api("accept_friend_request")]
public class AcceptFriendRequestHandler(BotContext bot) : IEmptyResultApiHandler<AcceptFriendRequestHandlerParameter>
{
    private readonly BotContext _bot = bot;

    public async Task HandleAsync(AcceptFriendRequestHandlerParameter parameter, CancellationToken token)
    {
        await _bot.SetFriendRequestAccept(parameter.RequestID);
    }
}

public class AcceptFriendRequestHandlerParameter(string requestId)
{
    [JsonRequired]
    [JsonPropertyName("request_id")]
    public string RequestID { get; init; } = requestId;
}