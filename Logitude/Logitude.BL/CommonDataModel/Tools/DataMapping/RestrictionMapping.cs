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
    public class RestrictionMapping
    {
        public static void MapEntity(RestrictionPM restrictionPm, Restriction restriction, bool isNewState)
        {
            restriction.ContactTenantId = restrictionPm.ContactTenantId;
            restriction.ObjectFieldId = restrictionPm.ObjectFieldId;
            restriction.ObjectTableId = restrictionPm.ObjectTableId;
            restriction.Tenant = restrictionPm.Tenant;
            restriction.Value = restrictionPm.Value;
            restriction.ObjectFieldCode = restrictionPm.ObjectFieldCode;
        }
    }
}