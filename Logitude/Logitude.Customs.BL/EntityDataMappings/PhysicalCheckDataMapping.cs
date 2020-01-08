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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityLists;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class PhysicalCheckDataMapping: IMapping<PhysicalCheckPM, PhysicalCheck>
   {

        public void CustomPMToPOCO(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
             
                entityPOCO.Id = entityPM.Id;             
                entityPOCO.Tenant = entityPM.Tenant;

            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;    
        }

        public void CustomPOCOToPM(PhysicalCheckPM entityPM, PhysicalCheck entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CargoIdentifierTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.CheckSiteName);
            CustomMappedPMProperties.Add(PMPropertyNames.OperationName);
            CustomMappedPMProperties.Add(PMPropertyNames.QueueTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.StorageSiteName);
            CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            CustomMappedPMProperties.Add(PMPropertyNames.StatusMessageName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);

            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();

            //if (entityPOCO.CargoIdentifierKey1 != null)
            //{
            //    DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPOCO.Tenant);
            //    DeclarationPM declaration = declarationQueryService.GetSingle(entityPOCO.DeclarationId, false, false);
            // //   entityPM.CargoIdentifierKey1 = declaration.c;
            //}

            if (entityPOCO.CargoIdentifierTypeCode != null)
            {
                CargoIdentifireTypeQueryService cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPOCO.Tenant);
                CargoIdentifireTypePM cargoIdentifireType = cargoIdentifireTypeQueryService.GetSingle(entityPOCO.CargoIdentifierTypeCode, false, true);
                entityPM.CargoIdentifierTypeName = cargoIdentifireType.LocalName;
            }

            if (entityPOCO.CheckSiteCode != null)
            {
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPOCO.Tenant);
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(entityPOCO.CheckSiteCode, false, true);
                entityPM.CheckSiteName = siteLookup != null ? siteLookup.LocalName : null;
            }


            if (entityPOCO.OperationCode != null)
            {
                PhysicalCheckOperationQueryService physicalCheckOperationQueryService = new PhysicalCheckOperationQueryService(entityPOCO.Tenant);
                PhysicalCheckOperationPM physicalCheckOperation = physicalCheckOperationQueryService.GetSingle(entityPOCO.OperationCode, false, true);
                entityPM.OperationName = physicalCheckOperation.LocalName;
            }

            if (entityPOCO.QueueTypeCode != null)
            {
                CheckQueueTypeQueryService checkQueueTypeQueryService = new CheckQueueTypeQueryService(entityPOCO.Tenant);
                CheckQueueTypePM checkQueueType = checkQueueTypeQueryService.GetSingle(entityPOCO.QueueTypeCode, false, true);
                entityPM.QueueTypeName = checkQueueType.LocalName;
            }

            if (entityPOCO.StorageSiteCode != null)
            {
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPOCO.Tenant);
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(entityPOCO.StorageSiteCode, false, true);
                entityPM.StorageSiteName = siteLookup.LocalName;
            }


            if (entityPOCO.DeclarationId != null)
            {
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPOCO.Tenant);
                DeclarationPM declaration = declarationQueryService.GetSingle(entityPOCO.DeclarationId, false, true);
                entityPM.CustomerName = declaration.CustomerName;
            }

            if (entityPOCO.StatusMessageCode != null)
            {
                PhysicalCheckStatusMessageQueryService physicalCheckStatusMessageQueryService = new PhysicalCheckStatusMessageQueryService(entityPOCO.Tenant);
                PhysicalCheckStatusMessagePM physicalCheckStatusMessage = physicalCheckStatusMessageQueryService.GetSingle(entityPOCO.StatusMessageCode, false, true);
                entityPM.StatusMessageName = physicalCheckStatusMessage.LocalName;
            }





        }

        private static void BuildSearchFields(PhysicalCheckPM entityPM, PhysicalCheck poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.CheckId))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CheckId : result + "," + entityPM.CheckId;

            }


            if (!string.IsNullOrEmpty(entityPM.ContainerNubmer))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ContainerNubmer : result + "," + entityPM.ContainerNubmer;

            }

            if (!string.IsNullOrEmpty(entityPM.CargoIdentifierKey1))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CargoIdentifierKey1 : result + "," + entityPM.CargoIdentifierKey1;

            }

            if (!string.IsNullOrEmpty(entityPM.CargoIdentifierKey2))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CargoIdentifierKey2 : result + "," + entityPM.CargoIdentifierKey2;

            }

            if (!string.IsNullOrEmpty(entityPM.CargoIdentifierKey3))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CargoIdentifierKey3 : result + "," + entityPM.CargoIdentifierKey3;

            }

             
            DeclarationQueryService declarationQuery = new DeclarationQueryService(poco.Tenant);

            DeclarationPM declaration = declarationQuery.GetSingle(entityPM.DeclarationId, false, false);

            if (declaration != null)
            {
                if (!string.IsNullOrEmpty(declaration.DeclarationNumber))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.DeclarationNumber : result + "," + declaration.DeclarationNumber;
                }

                if (!string.IsNullOrEmpty(declaration.CustomFileNo))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.CustomFileNo : result + "," + declaration.CustomFileNo;
                }

            }



            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public static void CustomXmlToPM(List<CommunicationLogStepList> communicationLogStepList, PhysicalCheckPM entityPM)
        {

            string documentData = communicationLogStepList[0].DocumentData;
            dynamic data = JObject.Parse(documentData);
            string cargoIdentifierType = data.CheckEntity.cargoIdentifier.cargoIdentifierType;
            if (cargoIdentifierType != null)
            {
                CargoIdentifireTypeQueryService cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPM.Tenant);
                CargoIdentifireTypePM cargoIdentifireType = cargoIdentifireTypeQueryService.GetSingle(cargoIdentifierType, false, true);
                entityPM.CargoIdentifierTypeName = cargoIdentifireType.LocalName;
            }
            string checkSiteNumber = data.NoticeToClient.checkSiteNumber;
            if (checkSiteNumber != null)
            {
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPM.Tenant);
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(checkSiteNumber, false, true);
                entityPM.CheckSiteName = siteLookup != null ? siteLookup.LocalName : null;
            }
            string OperationCode = data.NoticeToClient.operationCode;
            if (OperationCode != null)
            {
                PhysicalCheckOperationQueryService physicalCheckOperationQueryService = new PhysicalCheckOperationQueryService(entityPM.Tenant);
                PhysicalCheckOperationPM physicalCheckOperation = physicalCheckOperationQueryService.GetSingle(OperationCode, false, true);
                entityPM.OperationName = physicalCheckOperation.LocalName;
            }
            string QueueTypeCode = data.NoticeToClient.CheckType;
            if (QueueTypeCode != null)
            {
                CheckQueueTypeQueryService checkQueueTypeQueryService = new CheckQueueTypeQueryService(entityPM.Tenant);
                CheckQueueTypePM checkQueueType = checkQueueTypeQueryService.GetSingle(QueueTypeCode, false, true);
                entityPM.QueueTypeCode = checkQueueType.Code;
                entityPM.QueueTypeName = checkQueueType.LocalName;
            }
            string storageSiteNumber = data.NoticeToClient.storageSiteNumber;
            if (storageSiteNumber != null)
            {
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPM.Tenant);
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(storageSiteNumber, false, true);
                entityPM.StorageSiteName = siteLookup.LocalName;
            }
            string importerNumber = data.NoticeToClient.importerNumber;
            string declarationNumber = data.NoticeToClient.declarationID;

            if (importerNumber != null)
            {
               PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(entityPM.Tenant);
               entityPM.CustomerName = physicalCheckQueryService.GetCustomerNameByChecKId(declarationNumber, entityPM.Tenant);
            }
            
            string statusMessage = data.NoticeToClient.statusMessage;
            if (statusMessage != null)
            {
                PhysicalCheckStatusMessageQueryService physicalCheckStatusMessageQueryService = new PhysicalCheckStatusMessageQueryService(entityPM.Tenant);
                PhysicalCheckStatusMessagePM physicalCheckStatusMessage = physicalCheckStatusMessageQueryService.GetSingle(statusMessage, false, true);
                entityPM.StatusMessageName = physicalCheckStatusMessage.LocalName;
            }
            entityPM.CargoIdentifierKey1 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey1;
            entityPM.CargoIdentifierKey2 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey2;
            entityPM.ContainerNubmer = data.CheckEntity.containerNumer;
            entityPM.CheckId = data.NoticeToClient.checkId;
            entityPM.OpenDate = data.NoticeToClient.openDate;
            entityPM.DeclarationId = declarationNumber;






        }
    }


}
   