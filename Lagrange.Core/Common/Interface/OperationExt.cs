﻿using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Response;
using Lagrange.Core.Internal.Logic;

namespace Lagrange.Core.Common.Interface;

public static class OperationExt
{
    public static Task<BotQrCodeInfo?> FetchQrCodeInfo(this BotContext context, byte[] k) =>
        context.EventContext.GetLogic<WtExchangeLogic>().FetchQrCodeInfo(k);

    public static Task<(bool Success, string Message)> CloseQrCode(this BotContext context, byte[] k, bool confirm) =>
        context.EventContext.GetLogic<WtExchangeLogic>().CloseQrCode(k, confirm);

    public static Task<Dictionary<string, string>> FetchCookies(this BotContext context, params List<string> domains) =>
        context.EventContext.GetLogic<OperationLogic>().FetchCookies(domains);

    public static Task<(string Key, uint Expiration)> FetchClientKey(this BotContext context) =>
        context.EventContext.GetLogic<OperationLogic>().FetchClientKey();

    public static Task<List<BotFriend>> FetchFriends(this BotContext context, bool refresh = false) =>
        context.CacheContext.GetFriendList(refresh);

    public static Task<List<BotGroup>> FetchGroups(this BotContext context, bool refresh = false) =>
        context.CacheContext.GetGroupList(refresh);

    public static Task<List<BotGroupMember>> FetchMembers(this BotContext context, long groupUin, bool refresh = false) =>
        context.CacheContext.GetMemberList(groupUin, refresh);

    public static Task<List<BotGroupNotificationBase>> FetchGroupNotifications(this BotContext context, ulong count, ulong start = 0) =>
        context.EventContext.GetLogic<OperationLogic>().FetchGroupNotifications(count, start);

    public static Task<List<BotGroupNotificationBase>> FetchFilteredGroupNotifications(this BotContext context, ulong count, ulong start = 0) =>
        context.EventContext.GetLogic<OperationLogic>().FetchFilteredGroupNotifications(count, start);

    public static Task<BotStranger> FetchStranger(this BotContext context, long uin) =>
        context.EventContext.GetLogic<OperationLogic>().FetchStranger(uin);

    public static Task SetGroupNotification(this BotContext context, long groupUin, ulong sequence, BotGroupNotificationType type, bool isFiltered, GroupNotificationOperate operate, string message = "") =>
        context.EventContext.GetLogic<OperationLogic>().SetGroupNotification(groupUin, sequence, type, isFiltered, operate, message);

    public static Task SetGroupReaction(this BotContext context, long groupUin, ulong sequence, string code, bool isAdd) =>
        context.EventContext.GetLogic<OperationLogic>().SetGroupReaction(groupUin, sequence, code, isAdd);
}