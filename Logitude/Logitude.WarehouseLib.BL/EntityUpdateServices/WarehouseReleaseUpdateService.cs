using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.Helpers;
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

                SetPartnerContactField(entityPM);
                new MainEntityChangeService(new EntityChangeArgs()
                {
                    EntityPM = entityPM,
                    ProcessType = "OnCreate",
                    ObjectTableName = "WarehouseRelease",
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    StartDate = DateTime.Now,
                    EntityReference = entityPM.ReleaseNumber
                }).AddEntityChange();
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

      
        protected override void OnUpdating(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO)
        {

            AddTraceEvents(entityPM, entityPOCO);


            base.OnUpdating(entityPM, entityPOCO);

            if (entityPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update) return;

            if (!entityPM.IsUpdateByAutomation)
            {
                SetPartnerContactField(entityPM);
                new MainEntityChangeService(new EntityChangeArgs() {
                    EntityPM = entityPM,
                    OldEntityPM = this.OldEntityPM, 
                    ProcessType = "OnUpdate", 
                    EntityChangeFieldXml = this.EntityChangeFieldXml, 
                    ObjectTableName = "WarehouseRelease", 
                    EntityId = entityPM.Id, 
                    Tenant = entityPM.Tenant, 
                    EntityReference = entityPM.ReleaseNumber 
                }).AddEntityChange();
            }
        }

        public void SetPartnerContactField(WarehouseReleasePM entityPM)
        {
            entityPM.CustomerPrimaryContactId = GetPrimaryContactId(entityPM.CustomerId, entityPM.Tenant);
        }

        private string GetPrimaryContactId(string cardId, int tenant)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            CardPM cardPM = cardQuery.GetSinglePM(cardId, tenant);
            if (cardPM == null) return null;
            return cardPM.PrimaryContactId;
        }

        private void AddTraceEvents(WarehouseReleasePM entityPM, WarehouseRelease entityPOCO)
        {
            List<string> eventCodeLists = new List<string>();
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert) eventCodeLists.Add("CRRE");
            else eventCodeLists.Add("UPRE");
            if (entityPM.ExpectedReleaseDate != entityPOCO.ExpectedReleaseDate) eventCodeLists.Add("EXRE");
            if (entityPM.ActualReleaseDate != entityPOCO.ActualReleaseDate) eventCodeLists.Add("ENRE");

            var eventTracerArgs =  new EventTracerArgs(){Tenant = entityPM.Tenant,UserId = entityPM.UpdatedByUserId,EntityId = entityPM.Id,ObjectTableName = "WarehouseRelease",};
            WarehouseEntryReleaseHelper warehouseEntryReleaseHelper = new WarehouseEntryReleaseHelper();
            warehouseEntryReleaseHelper.AddTraceEvents(eventCodeLists, eventTracerArgs);

        }


        private void BuildActivityLog(string typeCode, WarehouseReleasePM entityPM)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("WarehouseRelease", 0, true);
            string email = GetLoggedUserEmail(entityPM);
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, typeCode, loggedContact.Id);
                if (typeCode == "U") entityPM.UpdatedByUserId = loggedContact.Id;


            }
        }
        private string GetLoggedUserEmail(WarehouseReleasePM entityPM)
        {
            if (HttpContext.Current != null) return HttpContext.Current.User.Identity.Name;
            UserRepository userRepository = new UserRepository(entityPM.Tenant);
            User loggedUser = userRepository.GetSingleUserById(entityPM.UpdatedByUserId);
            return loggedUser?.Contact?.Email;
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
