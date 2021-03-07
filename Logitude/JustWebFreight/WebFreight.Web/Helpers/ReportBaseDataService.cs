using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ReportBaseDataService
    {

        public string GetCompanyName(int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            return tenantQuery.GetCompanyNameById(tenant);
        }

    }
}