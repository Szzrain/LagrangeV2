namespace Lagrange.Core.Internal.Events.System;

internal class SetGroupInviteAcceptEventReq(long groupUin, long sequence) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public long Sequence { get; } = sequence;
}

internal class SetGroupInviteAcceptEventResp : ProtocolEvent
{
    public static readonly SetGroupInviteAcceptEventResp Default = new();
}