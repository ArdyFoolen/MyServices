using ConfigApp;

Console.WriteLine($"APIGroupsLimit : {AppSettingHelper.APIGroupsLimit}");
Console.WriteLine($"APIUsersLimit : {AppSettingHelper.APIUsersLimit}");
Console.WriteLine($"APILinksLimit : {AppSettingHelper.APILinksLimit}");
Console.WriteLine($"APIParticipantsLimit : {AppSettingHelper.APIParticipantsLimit}");
Console.WriteLine($"APISyncpointsPerCompanyLimit : {AppSettingHelper.APISyncpointsPerCompanyLimit}");
Console.WriteLine($"APISyncpointsPerUserLimit : {AppSettingHelper.APISyncpointsPerUserLimit}");
Console.WriteLine($"APIReportsLimit : {AppSettingHelper.APIReportsLimit}");
Console.WriteLine($"APIReportDateRangeLimitInDays : {AppSettingHelper.APIReportDateRangeLimit}");
Console.WriteLine($"APIPolicySetsLimit : {AppSettingHelper.APIPolicySetsLimit}");
Console.WriteLine($"APIGroupMembersLimit : {AppSettingHelper.APIGroupMembersLimit}");
Console.WriteLine($"APIRequestEntriesLimit1 : {AppSettingHelper.APIRequestEntriesLimit1}");
Console.WriteLine($"APIRequestEntriesLimit2 : {AppSettingHelper.APIRequestEntriesLimit2}");

Console.WriteLine($"APILinksUserLimit : {AppSettingHelper.APILinksUserLimit}");
var result = AppSettingHelper.APILinksLimitUsers;
foreach (var item in result)
{
    Console.WriteLine($"limituser : {item}");
}
