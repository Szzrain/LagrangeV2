﻿using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<AddGroupReactionEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x9082_1")]
internal class AddGroupReactionService : OidbService<AddGroupReactionEventReq, AddGroupReactionEventResp, SetGroupReactionRequest, SetGroupReactionResponse>
{
    private protected override uint Command => 0x9082;

    private protected override uint Service => 1;

    private protected override Task<SetGroupReactionRequest> ProcessRequest(AddGroupReactionEventReq request, BotContext context)
    {
        return Task.FromResult(new SetGroupReactionRequest
        {
            GroupUin = request.GroupUin,
            Sequence = request.Sequence,
            Code = request.Code,
            Type = request.Code.Length <= 3 ? 1ul : 2ul
        });
    }

    private protected override Task<AddGroupReactionEventResp> ProcessResponse(SetGroupReactionResponse response, BotContext context)
    {
        return Task.FromResult(AddGroupReactionEventResp.Default);
    }
}
