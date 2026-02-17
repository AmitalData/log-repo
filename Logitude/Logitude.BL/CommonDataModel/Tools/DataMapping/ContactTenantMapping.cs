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
    public class ContactTenantMapping
    {
        public static void MapEntity(ContactTenantPM contactTenantPm, ContactTenant contactTenant, bool isNewState)
        {
            contactTenant.ContactId = contactTenantPm.ContactId;
            contactTenant.Id = contactTenantPm.Id;
            contactTenant.TenantId = contactTenantPm.TenantId;
        }
    }
}
