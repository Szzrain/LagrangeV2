namespace Lagrange.Core.Events.EventArgs;

public class BotFriendRequestEvent(string requestId, Int64 initiatorUin, string message, string source) : EventBase
{
    public string RequestId { get; } = requestId;

    public Int64 InitiatorUin { get; } = initiatorUin;
    
    public string Message { get; } = message;
    
    public string Source { get; } = source;

    public override string ToEventMessage()
    {
        return $"{nameof(BotFriendRequestEvent)}: RequestId: {RequestId}, " +
               $"InitiatorUin: {InitiatorUin}, " +
               $"Message: {Message}, " +
               $"Source: {Source}";
    }
}
