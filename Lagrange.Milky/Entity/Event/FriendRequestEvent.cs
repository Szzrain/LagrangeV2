using System.Text.Json.Serialization;
using Lagrange.Milky.Extension;

namespace Lagrange.Milky.Entity.Event;

public class FriendRequestEvent(long time, long selfId, FriendRequestEventData data) : EventBase<FriendRequestEventData>(time, selfId, "friend_request", data) { }

public class FriendRequestEventData
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
    [JsonPropertyName("comment")]
    public string Comment { get; }
    [JsonPropertyName("via")]
    public string Via { get; }

    public FriendRequestEventData(string requestId, long time, bool isFiltered, long? initiatorId, string state, string comment, string via)
    {
        RequestID = requestId;
        Time = time;
        IsFiltered = isFiltered;
        InitiatorID = initiatorId;
        State = state;
        Comment = comment;
        Via = via;
    }
}