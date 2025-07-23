using System.Text.Json.Serialization;

namespace Lagrange.Milky.Entity.Event;


public class GroupInvitationEvent(long time, long selfId, GroupInvitationEventData data) : EventBase<GroupInvitationEventData>(time, selfId, "group_invitation", data) { }

public class GroupInvitationEventData
{
    [JsonPropertyName("request_id")]
    public string RequestID { get; }
    [JsonPropertyName("time")]
    public long Time { get; }
    [JsonPropertyName("is_filtered")]
    public bool IsFiltered { get; }
    [JsonPropertyName("initiator_id")]
    public long? InitiatorID { get; }
    [JsonPropertyName("state")]
    public string State { get; }
    [JsonPropertyName("group_id")]
    public long GroupID { get; }

    public GroupInvitationEventData(string requestId, long time, bool isFiltered, long? initiatorId, string state, long groupId)
    {
        RequestID = requestId;
        Time = time;
        IsFiltered = isFiltered;
        InitiatorID = initiatorId;
        State = state;
        GroupID = groupId;
    }
}