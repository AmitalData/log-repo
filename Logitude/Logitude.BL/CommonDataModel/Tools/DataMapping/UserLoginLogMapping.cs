using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class UserLoginLogMapping
    {
        public static void MapEntity(UserLoginLogPM userLoginLogPm, UserLoginLog userLoginLog, bool isNewState)
        {
            userLoginLog.Tenant = userLoginLogPm.Tenant;
            userLoginLog.IP = userLoginLogPm.IP;
            userLoginLog.Browser = userLoginLogPm.Browser;
            userLoginLog.UserId = userLoginLogPm.UserId;
            userLoginLog.GMTDateTime = userLoginLogPm.GMTDateTime;
            userLoginLog.LocalDateTime = userLoginLogPm.LocalDateTime;
            userLoginLog.UserAgent = userLoginLogPm.UserAgent;
        }
    }
}