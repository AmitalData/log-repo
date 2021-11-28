using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class CrmWebServicesValidator
    {
        public static bool IsDisabled(int tenant)
        {
            return (tenant == 0 || tenant == 341) && LogitudeSettings.WorkEnvironment != "cloud";
        }
    }
}