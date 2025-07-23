using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Notify;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class GroupInvite
{
    [ProtoMember(1)] public Int64 GroupUin { get; set; }
    
    [ProtoMember(5)] public string InviterUid { get; set; }
}
