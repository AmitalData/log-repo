using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Simplog.Global.Data.GlobalModel;

namespace WebFreight.Web.Helpers.ExportServer
{
    public class ExportServerService
    {
        private static TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        private static TenantManagementService tenantManagementService = new TenantManagementService(GlobalContext.GetContext());            

        public static ExportServerSettings GetSettings(int tenant)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            return new ExportServerSettings()
            {
                exportLoginCredential = tenantManagementPM.ExportLoginCredintial,
                exportTenant = tenantManagementPM.ExportTenant,
            };
        }

        public static void UpdateSettings(int tenant, ExportServerSettings exportServerSettings)
        {
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);
            tenantManagementPM.ExportLoginCredintial = exportServerSettings.exportLoginCredential;
            tenantManagementPM.ExportTenant = exportServerSettings.exportTenant;
            tenantManagementService.Update(tenantManagementPM, true);
        }
    }

    public class ExportServerSettings
    {
        public string exportLoginCredential { get; set; }
        public int? exportTenant { get; set; }
    }
}