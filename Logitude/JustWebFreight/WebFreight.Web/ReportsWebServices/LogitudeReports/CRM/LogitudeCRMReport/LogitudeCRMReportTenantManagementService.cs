using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.CRM.BL.DataContracts;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM.LogitudeCRMReport
{
    public class LogitudeCRMReportTenantManagementService
    {

        private readonly List<TenantManagementPM> tenantManagements;

        public LogitudeCRMReportTenantManagementService(List<int> tenantNumbers)
        {
            tenantManagements = new TenantManagementQuery().GetByTenantNumbers(tenantNumbers);
        }

        public TenantManagementPM GetByTenantNumber(string tenantNumber)
        {
            return tenantManagements.Where(a => a.Id.ToString() == tenantNumber).FirstOrDefault();
        }

        public int? GetNumberOfUsers(TenantManagementPM tenantManagement)
        {
            int? numberOfUsers = tenantManagement.TenantManagementLicenses.Sum(a => a.NumberOfUsers);
            if (tenantManagement.MainAdditionalPackageApplied)
            {
                return tenantManagement.IsMultiPackage ? (numberOfUsers ?? 0) + (tenantManagement.NumberOfUsers ?? 0) : tenantManagement.NumberOfUsers ?? 0;
            }
            else
            {
                return tenantManagement.IsMultiPackage ? numberOfUsers ?? 0 : tenantManagement.NumberOfUsers;
            }

        }

        public double? GetTotalPrice(TenantManagementPM tenantManagement)
        {
            double? totalPrice = tenantManagement.TenantManagementLicenses.Sum(a => a.TotalPrice);
            if (tenantManagement.MainAdditionalPackageApplied)
            {
                return tenantManagement.IsMultiPackage ? (totalPrice ?? 0) + (tenantManagement.TotalPrice ?? 0) : tenantManagement.TotalPrice ?? 0;
            }
            else
            {
                return tenantManagement.IsMultiPackage ? totalPrice ?? 0 : tenantManagement.TotalPrice;
            }

        }


    }
}