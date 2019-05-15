using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.WarehouseLib.BL.EntityUpdateServices
{
 
    public partial class WarehouseReleaseUpdateService
    {

        protected override void OnCreating(WarehouseReleasePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("WarehouseRelease", entityPM.Tenant);
                entityPM.ReleaseNumber = TableCounter.GetNumber(entityPM.Tenant, "WARC", null, null).ToString();
                this.BuildActivityLog("N", entityPM);
            }
        }

        protected override void OnUpdating(WarehouseReleasePM entityPM)
        {
            ComputeTotalQuantities(entityPM);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                string x = "";
                if (!string.IsNullOrEmpty(entityPM.CustomerRef1) && !string.IsNullOrEmpty(entityPM.CustomerRef2)) x = ",";
                entityPM.References = entityPM.CustomerRef1 + x + entityPM.CustomerRef2;

                WarehouseReleaseStatusRepository warehouseReleaseStatusRepository = new WarehouseReleaseStatusRepository(entityPM.Tenant);
                WarehouseReleaseStatus warehouseReleaseStatus = warehouseReleaseStatusRepository.GetSingle(entityPM.StatusCode);
                if (warehouseReleaseStatus != null) entityPM.StatusName = warehouseReleaseStatus.Name;
                this.BuildActivityLog("U", entityPM);
            }
        }

        protected override void UpdateComposition(WarehouseReleasePM entityPM)
        {
            WarehouseReleasePackageUpdateService warehouseReleasePackageUpdateService = new WarehouseReleasePackageUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            warehouseReleasePackageUpdateService.UpdateMulti(entityPM.WarehouseReleasePackages, entityPM.DeletedWarehouseReleasePackages, entityPM, false);
            base.UpdateComposition(entityPM);
           
        }

        private void BuildActivityLog(string typeCode, WarehouseReleasePM entityPM)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("WarehouseRelease", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, typeCode, loggedContact.Id);
                if (typeCode == "U") entityPM.UpdatedByUserId = loggedContact.Id;


            }
        }

        private void ComputeTotalQuantities(WarehouseReleasePM entityPM)
        {
            if (entityPM != null && entityPM.WarehouseReleasePackages != null && entityPM.WarehouseReleasePackages.Count() > 0)
            {
                entityPM.TotalQuantity = entityPM.WarehouseReleasePackages.Sum(a => a.Quantity);
            }
        }
    }
}
