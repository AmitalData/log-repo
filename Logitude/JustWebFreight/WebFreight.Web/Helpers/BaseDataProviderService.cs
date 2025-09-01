using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;
using Simplog.Data.CommonDataModel.EntityPOCOs; 

namespace WebFreight.Web.Helpers
{
    public static class BaseDataProviderService
    {
        public static void FillBaseVariableFields(dynamic dataProvider ,int tenant)
        {
            dataProvider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            dataProvider.Logo = DataProviders.General.GetLogo(tenant);
            dataProvider.GeneralAddress= GetGeneralAddress( tenant);

            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
            if (tenantPM != null)
            {
                dataProvider.CompanyName = tenantPM.Company;
                dataProvider.InvoicePrintNotes = tenantPM.InvoicePrintNotes;
                dataProvider.InvoicePrintNotesLocal = tenantPM.InvoicePrintNotesLocal;
            }
        }

        private static string GetGeneralAddress(int tenant)
        {
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            AddressRepository addressRep = new AddressRepository(currentTenant.Id);
            Address tenantAddress = addressRep.GetSingleAddress(currentTenant.AddressId, currentTenant.Id);
            return DataProviders.General.GetAddress(tenantAddress);
        }
    }
}