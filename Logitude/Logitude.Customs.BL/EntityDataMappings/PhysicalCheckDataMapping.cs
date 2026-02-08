using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Server.Tools; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        public static void PhysicalCheckRequestXmlToPM(List<CommunicationLogStepList> communicationLogStepList, PhysicalCheckPM entityPM)
        {
        var swTotal = Stopwatch.StartNew();
        var sw = new Stopwatch();

        try
        {
            sw.Start();
            string documentData = communicationLogStepList[0].DocumentData;
            sw.Stop();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Get documentData (index 0) took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

            sw.Restart();
            dynamic data = JObject.Parse(documentData);
            sw.Stop();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] JObject.Parse took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

            sw.Restart();
            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(entityPM.Tenant);
            sw.Stop();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new PhysicalCheckQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

            if (data.CheckEntity.cargoIdentifier.cargoIdentifierType != null)
            {
                sw.Restart();
                CargoIdentifireTypeQueryService cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new CargoIdentifireTypeQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                CargoIdentifireTypePM cargoIdentifireType = cargoIdentifireTypeQueryService.GetSingle(Convert.ToString(data.CheckEntity.cargoIdentifier.cargoIdentifierType), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] CargoIdentifireType.GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.CargoIdentifierTypeName = cargoIdentifireType.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CargoIdentifierTypeName took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.checkSiteNumber != null)
            {
                sw.Restart();
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new SiteLookupQueryService (checkSiteNumber) took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(Convert.ToString(data.NoticeToClient.checkSiteNumber), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] SiteLookup(checkSiteNumber).GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.CheckSiteName = siteLookup != null ? siteLookup.LocalName : null;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CheckSiteName took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.entityType != null)
            {
                sw.Restart();
                CheckEntityTypeQueryService cargoIdentifireTypeQueryService = new CheckEntityTypeQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new CheckEntityTypeQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                CheckEntityTypePM checkEntityType = cargoIdentifireTypeQueryService.GetSingle(Convert.ToString(data.NoticeToClient.entityType), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] CheckEntityType.GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.CargoTypeCode = checkEntityType.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CargoTypeCode took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.operationCode != null)
            {
                sw.Restart();
                PhysicalCheckOperationQueryService physicalCheckOperationQueryService = new PhysicalCheckOperationQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new PhysicalCheckOperationQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                PhysicalCheckOperationPM physicalCheckOperation = physicalCheckOperationQueryService.GetSingle(Convert.ToString(data.NoticeToClient.operationCode), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] PhysicalCheckOperation.GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.OperationName = physicalCheckOperation.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set OperationName took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.CheckType != null)
            {
                sw.Restart();
                CheckTypeLookupQueryService checkTypeLookupQueryService = new CheckTypeLookupQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new CheckTypeLookupQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                CheckTypeLookupPM checkTypeLookup = checkTypeLookupQueryService.GetSingle(Convert.ToString(data.NoticeToClient.CheckType), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] CheckTypeLookup.GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.CheckTypeName = checkTypeLookup.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CheckTypeName took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.QueueType != null)
            {
                sw.Restart();
                CheckQueueTypeQueryService checkQueueTypeQueryService = new CheckQueueTypeQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new CheckQueueTypeQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                CheckQueueTypePM checkQueueType = checkQueueTypeQueryService.GetSingle(Convert.ToString(data.NoticeToClient.QueueType), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] CheckQueueType.GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.QueueTypeCode = checkQueueType.Code;
                entityPM.QueueTypeName = checkQueueType.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set QueueTypeCode/Name took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.storageSiteNumber != null)
            {
                sw.Restart();
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new SiteLookupQueryService (storageSiteNumber) took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(Convert.ToString(data.NoticeToClient.storageSiteNumber), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] SiteLookup(storageSiteNumber).GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.StorageSiteName = siteLookup.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set StorageSiteName took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.declarationID != null)
            {
                sw.Restart();
                var result = physicalCheckQueryService.GetCustomerNameByDeclartionNo(
                    Convert.ToString(data.NoticeToClient.declarationID),
                    entityPM.Tenant
                );
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] GetCustomerNameByDeclartionNo took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                CardPM card = (CardPM)result.Item1;
                string customFileNo = (string)result.Item2;
                entityPM.CustomerName = card.LocalName;
                entityPM.DeclarationId = data.NoticeToClient.declarationID;
                entityPM.CustomFileNo = customFileNo;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CustomerName/DeclarationId/CustomFileNo took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.statusMessage != null)
            {
                sw.Restart();
                PhysicalCheckStatusMessageQueryService physicalCheckStatusMessageQueryService = new PhysicalCheckStatusMessageQueryService(entityPM.Tenant);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] new PhysicalCheckStatusMessageQueryService took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                PhysicalCheckStatusMessagePM physicalCheckStatusMessage = physicalCheckStatusMessageQueryService.GetSingle(Convert.ToString(data.NoticeToClient.statusMessage), false, true);
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] PhysicalCheckStatusMessage.GetSingle took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");

                sw.Restart();
                entityPM.StatusMessageName = physicalCheckStatusMessage.LocalName;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set StatusMessageName took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.checkId != null)
            {
                sw.Restart();
                entityPM.CheckId = data.NoticeToClient.checkId;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CheckId took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.limitDate != null)
            {
                sw.Restart();
                entityPM.LimitDate = data.NoticeToClient.limitDate;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set LimitDate took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.CheckEntity.cargoIdentifier.cargoIdentifierKey1 != null)
            {
                sw.Restart();
                entityPM.CargoIdentifierKey1 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey1;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CargoIdentifierKey1 took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.CheckEntity.cargoIdentifier.cargoIdentifierKey2 != null)
            {
                sw.Restart();
                entityPM.CargoIdentifierKey2 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey2;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set CargoIdentifierKey2 took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.CheckEntity.containerNumber != null)
            {
                sw.Restart();
                entityPM.ContainerNubmer = data.CheckEntity.containerNumber;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set ContainerNubmer took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            if (data.NoticeToClient.openDate != null)
            {
                sw.Restart();
                entityPM.OpenDate = data.NoticeToClient.openDate;
                sw.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] Set OpenDate took {sw.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
            }

            swTotal.Stop();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[PhysicalCheckMap] TOTAL took {swTotal.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}");
        }
        catch (Exception ex)
        {
            swTotal.Stop();
            NetCommonHelper.Logger.DevLog.Instance.WriteError($"[PhysicalCheckMap] FAILED after {swTotal.ElapsedMilliseconds} ms. tenant={entityPM.Tenant}. ex={ex}");
            throw;
        }
    }

    public static void ClosedPhysicalCheckXmlToPM(List<CommunicationLogStepList> communicationLogStepList, PhysicalCheckPM entityPM)
        {
            string documentData = communicationLogStepList[0].DocumentData;
            dynamic data = JObject.Parse(documentData);
            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(entityPM.Tenant);
            if (data.generalDetails.checkId != null)
            {
                entityPM.CheckId = data.generalDetails.checkId;
            }
            if (data.generalDetails.endDate != null)
            {
                entityPM.EndDate = data.generalDetails.endDate;
            }
            if (data.generalDetails.declarationID != null)
            {
                Card card = physicalCheckQueryService.GetCustomerNameByDeclartionNo(Convert.ToString(data.generalDetails.declarationID), entityPM.Tenant);
                entityPM.CustomerName = card.LocalName;
                entityPM.DeclarationId = data.generalDetails.declarationID;
                entityPM.CustomFileNo = physicalCheckQueryService.GetCustomFileNoByCheckId(entityPM.DeclarationId, entityPM.Tenant);

            }
            if (data.generalDetails.entityType != null && data.generalDetails.entityType!= "0")
            {
                CheckEntityTypeQueryService cargoIdentifireTypeQueryService = new CheckEntityTypeQueryService(entityPM.Tenant);
                CheckEntityTypePM checkEntityType = cargoIdentifireTypeQueryService.GetSingle(Convert.ToString(data.generalDetails.entityType), false, true);
                entityPM.CargoTypeCode = checkEntityType.LocalName;
            }
            if (data.generalDetails.checkSiteNumber != null)
            {
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPM.Tenant);
                SiteLookupPM siteLookup = siteLookupQueryService.GetSingle(Convert.ToString(data.generalDetails.checkSiteNumber), false, true);
                entityPM.CheckSiteName = siteLookup?.LocalName;

            }
            if (data.CheckEntity.cargoIdentifier.cargoIdentifierType != null)
            {
                CargoIdentifireTypeQueryService cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPM.Tenant);
                CargoIdentifireTypePM cargoIdentifireType = cargoIdentifireTypeQueryService.GetSingle(Convert.ToString(data.CheckEntity.cargoIdentifier.cargoIdentifierType), false, true);
                entityPM.CargoIdentifierTypeName = cargoIdentifireType.LocalName;
            }
            if (data.CheckEntity.cargoIdentifier.cargoIdentifierKey1 != null)
            {
                entityPM.CargoIdentifierKey1 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey1;
            }
            if (data.CheckEntity.cargoIdentifier.cargoIdentifierKey2 != null)
            {
                entityPM.CargoIdentifierKey2 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey2;
            }
            if (data.CheckEntity.cargoIdentifier.cargoIdentifierKey3 != null)
            {
                entityPM.CargoIdentifierKey3 = data.CheckEntity.cargoIdentifier.cargoIdentifierKey3;
            }
            if (data.CheckEntity.containerNumber != null)
            {
                entityPM.ContainerNubmer = data.CheckEntity.containerNumber;
            }

        }

    }


}
   