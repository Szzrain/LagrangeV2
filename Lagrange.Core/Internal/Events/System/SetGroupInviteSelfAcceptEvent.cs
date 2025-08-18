namespace Lagrange.Core.Internal.Events.System;

internal class SetGroupInviteSelfAcceptEventReq(long groupUin, long sequence) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public long Sequence { get; } = sequence;
}

internal class SetGroupInviteSelfAcceptEventResp : ProtocolEvent
{
    public static readonly SetGroupInviteSelfAcceptEventResp Default = new();
}