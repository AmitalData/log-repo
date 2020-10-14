
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class BIReportsSecurityIntegrationService
    {
        int tenant;
        public BIReportsSecurityIntegrationService(int tenant)
        {
            this.tenant = tenant;
        }

        public void CheckBIReportDataSecurity(DataTable dataTable, List<int> tenants = null)
        {
            var iSSecureData = true;
            if (dataTable != null)
            {
                if (tenants == null)
                {
                    iSSecureData = !((from row in dataTable.AsEnumerable() where row.Field<int>("Tenant") != tenant select row).Any());
                }
                else iSSecureData = IsBIReportCenteralDataSecure(dataTable, tenants);
               
                
                if (!iSSecureData) throw new Exception("You are not authorized to view the content. The returned data is doesn't belong to the right tenant!");
            }
        }



        private static bool IsBIReportCenteralDataSecure(DataTable dataTable, List<int> tenants)
        {
            return !((from row in dataTable.AsEnumerable()
                      where !tenants.Contains(row.Field<int>("Tenant"))
                      select row).Any());
        }
    }
}


