using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

[ProtoPackable]
internal partial class D10C8Req
{
    [ProtoMember(1)] public uint Accept { get; set; } // 2 for reject, 1 for accept, 3 for ignore

    [ProtoMember(2)] public D10C8ReqBody? Body { get; set; } 
}

[ProtoPackable]
internal partial class D10C8ReqBody
{
    [ProtoMember(1)] public long Sequence { get; set; } // 1
    
    [ProtoMember(2)] public uint EventType { get; set; } // 2
    
    [ProtoMember(3)] public long GroupUin { get; set; } // 3
    
    [ProtoMember(4)] public string? Message { get; set; } // ""
}

[ProtoPackable]
internal partial class D10C8RespBody;