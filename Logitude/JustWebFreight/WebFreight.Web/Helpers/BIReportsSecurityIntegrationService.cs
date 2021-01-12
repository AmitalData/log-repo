
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
        private  int tenant;
        public BIReportsSecurityIntegrationService(int tenant)
        {
            this.tenant = tenant;
        }

        public void CheckBIReportDataSecurity( DataTable bIReportData , List<int> tenants =null)
        {
            if (bIReportData != null && (bIReportData.Columns.Contains("Tenant")))
            {
                var isSecure = true;
                
                if (tenants == null) isSecure = IsBIReportDataSecure(bIReportData);
                else isSecure = IsBIReportCenteralDataSecure(bIReportData , tenants);

                if (!isSecure) throw new Exception("You are not authorized to view the content. The returned data is doesn't belong to the right tenant!");
            }
        }



        private  bool IsBIReportDataSecure(DataTable bIReportData)
        {
            return !((from row in bIReportData.AsEnumerable() where row.Field<int>("Tenant") != tenant select row).Any());
        }


        private  bool IsBIReportCenteralDataSecure(DataTable bIReportData , List<int> tenants)
        {
            return !((from row in bIReportData.AsEnumerable()
                      where !tenants.Contains(row.Field<int>("Tenant"))
                      select row).Any());
        }
    }




}


