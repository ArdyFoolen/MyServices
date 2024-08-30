using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigApp
{
    public static class AppSettingHelper
    {
        #region API limits

        /// <summary>
        /// Gets Limits config section.
        /// </summary>
        public static ExternalAPIConfigurationSection.LimitElementCollection Limits
        {
            get
            {
                var externalAPISection = ConfigurationManager.GetSection("externalAPI") as ExternalAPIConfigurationSection;

                return (externalAPISection == null) ? null : externalAPISection.Limits;
            }
        }

        /// <summary>
        /// Gets LimitUsers config section.
        /// </summary>
        public static ExternalAPIConfigurationSection.LimitUserElementCollection LimitUsers
        {
            get
            {
                var externalAPISection = ConfigurationManager.GetSection("externalAPI") as ExternalAPIConfigurationSection;

                return (externalAPISection == null) ? null : externalAPISection.LimitUsers;
            }
        }

        /// <summary>
        /// Gets the limit value from config by limit key.
        /// </summary>
        /// <param name="key">Limit key.</param>
        /// <returns>Limit value.</returns>
        public static int GetLimitValue(string key)
        {
            if (Limits == null)
            {
                return 0;
            }

            var limit = Limits.GetLimit(key);

            int result;
            return (limit != null && int.TryParse(limit.Value, out result)) ? result : 0;
        }

        public static int APIGroupsLimit
        {
            get
            {
                return GetLimitValue("APIGroupsLimit");
            }
        }

        public static int APIUsersLimit
        {
            get
            {
                return GetLimitValue("APIUsersLimit");
            }
        }

        public static int APILinksLimit
        {
            get
            {
                return GetLimitValue("APILinksLimit");
            }
        }

        public static int APILinksUserLimit
        {
            get
            {
                if (Limits == null)
                    return 0;

                if (LimitUsers == null || LimitUsers.DefaultLimit == 0)
                    return APILinksLimit;
                return LimitUsers.DefaultLimit;
            }
        }

        public static int[] APILinksLimitUsers
        {
            get
            {
                if (LimitUsers == null)
                    return Array.Empty<int>();

                return LimitUsers.GetLimitUsers().ToArray();
            }
        }
        //public static int APISpecificUsersLinksLimit
        //{
        //    get
        //    {
        //        return GetLimitValue("APISpecificUsersLinksLimit");
        //    }
        //}

        //public static int[] APILinksLimitSpecificUsers
        //{
        //    get
        //    {
        //        return GetLimitValue("APILinksLimitSpecificUsers");
        //    }
        //}

        public static int APIParticipantsLimit
        {
            get
            {
                return GetLimitValue("APIParticipantsLimit");
            }
        }

        public static int APISyncpointsPerCompanyLimit
        {
            get
            {
                return GetLimitValue("APISyncpointsPerCompanyLimit");
            }
        }
        public static int APISyncpointParticipantsLimit
        {
            get
            {
                int result;
                return int.TryParse(ConfigurationManager.AppSettings["APISyncpointParticipantsLimit"], out result) ? result : 0;
            }
        }

        public static int APISyncpointsPerUserLimit
        {
            get
            {
                return GetLimitValue("APISyncpointsPerUserLimit");
            }
        }

        public static int APIReportsLimit
        {
            get
            {
                return GetLimitValue("APIReportsLimit");
            }
        }

        public static int APIReportDateRangeLimit
        {
            get
            {
                return GetLimitValue("APIReportDateRangeLimitInDays");
            }
        }

        public static int APIPolicySetsLimit
        {
            get
            {
                return GetLimitValue("APIPolicySetsLimit");
            }
        }

        public static int APIGroupMembersLimit
        {
            get
            {
                return GetLimitValue("APIGroupMembersLimit");
            }
        }
        /// <summary>
        /// Gets limit size per request for SyncPoints, Groups, Links, Participans and Users
        /// </summary>
        public static int APIRequestEntriesLimit1
        {
            get
            {
                return GetLimitValue("APIRequestEntriesLimit1");
            }
        }
        /// <summary>
        /// Gets limit size per request for Files and FolderFolders
        /// </summary>
        public static int APIRequestEntriesLimit2
        {
            get
            {
                return GetLimitValue("APIRequestEntriesLimit2");
            }
        }

        #endregion
    }
}
