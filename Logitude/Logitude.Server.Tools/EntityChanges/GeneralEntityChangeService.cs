using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Server.Tools.EntityChanges
{
    public class GeneralEntityChangeService
    {

        public GeneralEntityChangeService()
        {


        }

        public string GetLoggedContactId(int tenant)
        {
            string loggedContactId = string.Empty;
            string email = ("system@tenant" + tenant.ToString() + ".com");
            ContactRepository contactRepository = new ContactRepository(tenant);
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                email = HttpContext.Current.User.Identity.Name;
            }
            var contact = contactRepository.GetSingleContactByEmail(email, tenant, true);
            if (contact != null) loggedContactId = contact.Id;

            return loggedContactId;
        }

        public DateTime? GetAutomationLastUpdateDate(string automationObjectTableId , int tenant)
        {
            string result = string.Empty;
            AutomationLastUpdateRepository automationLastUpdateRepository = new AutomationLastUpdateRepository(tenant);
            var automationLastUpdate = automationLastUpdateRepository.GetSingleAutomationLastUpdate(automationObjectTableId, tenant);
            return automationLastUpdate != null ? automationLastUpdate.LastUpdateDate : null;

        }

        public AutomationObjectTableClass GetAutomationObjectTableByName(string objectTableName , int tenant)
        {
            AutomationObjectTableClass automationObjectTableClass = null;
            if (!string.IsNullOrEmpty(objectTableName))
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                var objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, tenant, true);
                if (objectTable != null)
                {
                    automationObjectTableClass = new AutomationObjectTableClass() { Name = objectTable.Name, Id = objectTable.Id, OriginalId = objectTable.Id, OriginalName = objectTable.Name };
                    if (objectTableName == "Master")
                    {
                        var shipmentObjectTable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
                        automationObjectTableClass.OriginalId = shipmentObjectTable.Id;
                        automationObjectTableClass.OriginalName = shipmentObjectTable.Name;
                    }
                }

                var automationLastUpdate = GetAutomationLastUpdateDate(automationObjectTableClass.Id , tenant);
                if (automationLastUpdate != null)
                {
                    automationObjectTableClass.AutomationLastUpdate = automationLastUpdate != null ? automationLastUpdate.ToString() : "";
                    automationObjectTableClass.AutomationLastUpdateDate = automationLastUpdate;

                }
            }

            return automationObjectTableClass;
        }


        public bool CheckIfEntityHaveAutomation(string objectTableName, string type, int tenant)
        {
            bool result = false;

            string tableName = objectTableName;
            if (objectTableName == "MasterAndHouse") tableName = "Shipment";

            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName(tableName, 0, true);

            if (objecttable != null)
            {
                AutomationRepository automationRepository = new AutomationRepository(tenant);
                result = automationRepository.CheckIfEntityHaveAutomation(objecttable.Id, type, tenant);
                if (!result && objectTableName == "MasterAndHouse")
                {
                    objecttable = objecttableRepository.GetObjectTableByName("Master", 0, true);
                    result = automationRepository.CheckIfEntityHaveAutomation(objecttable.Id, type, tenant);
                }
            }

            return result;
        }

        public static bool IsShowLogBoxAutomationFields()
        {
            bool result = false;
            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxpre" || LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2" || LogitudeSettings.LogitudeURL == "http://localhost:9996"))
            {
                result = true;
            }
            return result;
        }

        public EntityDetails GetEntityDetails(string entityId, string objectTableName, int tenant)
        {
            object entityPM = GetEntityPMByIdAndObjectTableName(entityId, objectTableName, tenant);
            if (objectTableName != "Shipment") return new EntityDetails { EntityPM = entityPM, ObjectTableName = objectTableName, CombinedObjectTableName = objectTableName };

            string shipmentTableName = GetShipmentTableName(entityPM);
            if (shipmentTableName != "MasterAndHouse") return new EntityDetails { EntityPM = entityPM, ObjectTableName = shipmentTableName, CombinedObjectTableName = objectTableName };

            return new EntityDetails { EntityPM = entityPM, ObjectTableName = "Master", OtherObjectTableName = "Shipment", CombinedObjectTableName = shipmentTableName };
        }

        private string GetShipmentTableName(object entityPM)
        {
            string shipmentLevelCode = entityPM.GetType().GetProperty("ShipmentLevelCode")?.GetValue(entityPM)?.ToString();
            string shipmentTableName = shipmentLevelCode == "C" ? "Master" : shipmentLevelCode == "H" ? "Shipment" : "MasterAndHouse";
            return shipmentTableName;
        }

        private object GetEntityPMByIdAndObjectTableName(string entityId, string objectTableName, int tenant)
        {
            object entityPM = null;

            if (!string.IsNullOrEmpty(entityId))
            {
                entityPM = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(objectTableName, entityId, tenant);
            }

            return entityPM;
        }
    }

    public class EntityDetails
    {
        public object EntityPM { get; set; }
        public string ObjectTableName { get; set; }
        public string OtherObjectTableName { get; set; }
        public string CombinedObjectTableName { get; set; }
    }
}
