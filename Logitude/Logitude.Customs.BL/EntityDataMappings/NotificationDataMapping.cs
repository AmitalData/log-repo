
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class NotificationDataMapping: IMapping<NotificationPM, Notification>
   {

        public void CustomPMToPOCO(NotificationPM entityPM, Notification entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
          

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.BadjCount = entityPM.BadjCount;

                
            }


            entityPM.SearchFields = entityPM.SearchFields ?? "";
            if (entityPM.SearchFields.Length > 1999)
            {
                entityPM.SearchFields = entityPM.SearchFields.Substring(0, 1999);
            }
            entityPM.Description = entityPM.Description ?? "";
            if (entityPM.Description.Length > 1999)
            {
                entityPM.Description = entityPM.Description.Substring(0, 1999);
            }


            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(NotificationPM entityPM, Notification entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.NotificationDefinitionName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ObjectTableName);



            if (entityPOCO.NotificationDefinitionCode != null)
            {
                NotificationDefinitionQueryService notificationDefinitionQueryService = new NotificationDefinitionQueryService(entityPOCO.Tenant);
                NotificationDefinitionPM notificationDefinition = notificationDefinitionQueryService.GetSingle(entityPOCO.NotificationDefinitionCode, false, true);
                entityPM.NotificationDefinitionName = notificationDefinition.LocalName;
            }

            if (entityPOCO.ObjectTableId != null)
            {
                ObjectTableRepository rep = new ObjectTableRepository(0);
                ObjectTable objectTable = rep.GetObjectTableById(entityPOCO.ObjectTableId, entityPOCO.Tenant);
                entityPM.ObjectTableName = objectTable.Name;
            }

            if (entityPOCO.EntityId != null)
            {
                ObjectTableRepository rep = new ObjectTableRepository(0);
                ObjectTable objectTable = rep.GetObjectTableById(entityPOCO.ObjectTableId, entityPOCO.Tenant);
                if (objectTable != null && objectTable.Name == "Customs.Declaration")
                {
                    DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPOCO.Tenant);
                    DeclarationPM declaration = declarationQuery.GetSingle(entityPOCO.EntityId, false, false);
             //       entityPM.CustomerName = declaration != null ? declaration.CustomerName : null;
                   
                }
            }

            Card customerCard = CardRepository.GetSingleCard(entityPOCO.CustomerId, entityPOCO.Tenant, true);
            if (customerCard != null)
            {
                entityPM.CustomerName = customerCard.LocalName != null ? customerCard.LocalName : customerCard.EnglishName;
             
            }
       


        }

        private static void BuildSearchFields(NotificationPM entityPM, Notification poco)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.Reference1Number))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.Reference1Number : result + "," + entityPM.Reference1Number;

            }

            if (!string.IsNullOrEmpty(entityPM.Description))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.Description : result + "," + entityPM.Description;

            }

            if (!string.IsNullOrEmpty(entityPM.NotificationDefinitionName))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.NotificationDefinitionName : result + "," + entityPM.NotificationDefinitionName;

            }

            if (!string.IsNullOrEmpty(entityPM.ObjectTableId))
            {
                ObjectTableRepository rep = new ObjectTableRepository(0);
                ObjectTable objectTable = rep.GetObjectTableByName("Customs.Declaration", 0, false);
                DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPM.Tenant);

                if (entityPM.ObjectTableId == objectTable.Id)
                {
                    var declaration = declarationQuery.GetSingle(entityPM.EntityId, false, false) ?? new DeclarationPM();
                    result = string.IsNullOrEmpty(result) ? declaration.CustomerName : result + "," + declaration.CustomerName;
                }
            }






            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
            poco.SearchFields = poco.SearchFields ?? "";
            if (poco.SearchFields.Length > 1999)
            {
                poco.SearchFields = poco.SearchFields.Substring(0, 1999);
            }
        }
   }


}
   