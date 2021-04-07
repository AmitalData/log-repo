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
    public class TermsofUseMapping
    {
        public static void MapEntity(TermsofUsePM termsofUsePm, TermsofUse termsofUse)
        {
                termsofUse.Tenant = termsofUsePm.Tenant;
                termsofUse.Date = termsofUsePm.Date;
                termsofUse.VersionNumber = termsofUsePm.VersionNumber;

        }
    }
}