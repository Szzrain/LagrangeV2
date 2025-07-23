namespace Lagrange.Core.Events.EventArgs;

public class BotGroupInviteEvent(string requestId, Int64 initiatorUin, long groupUin) : EventBase
{
    public string RequestId { get; } = requestId;

    public Int64 InitiatorUin { get; } = initiatorUin;
    
    public long GroupUin { get; } = groupUin;

    public override string ToEventMessage()
    {
        return $"{nameof(BotGroupInviteEvent)}: RequestId: {RequestId}, " +
               $"InitiatorUin: {InitiatorUin}, " + $"GroupUin: {GroupUin}";
    }
}