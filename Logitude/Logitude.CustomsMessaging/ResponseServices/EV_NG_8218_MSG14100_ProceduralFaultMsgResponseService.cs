using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Fault;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class EV_NG_8218_MSG14100_ProceduralFaultMsgResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, EV_NG_8218_MSG14100_ProceduralFaultMsg, GenericRequestParams>
    {
        ProceduralFaultPM _MyProceduralFaultPM;
        public override void Update(EV_NG_8218_MSG14100_ProceduralFaultMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 8218- New/Update Fault (DCA)
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var proceduralFaultQueryService = new ProceduralFaultQueryService(requestParams.Tenant);
            var proceduralFaultUpdateService = new ProceduralFaultUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            foreach (var proceduralFaultItem in customResponse.ProceduralFaultDetails)
            {
                string faultId = proceduralFaultQueryService.GetFaultIdByFaultNumber(proceduralFaultItem.proceduralFaultNumber.ToString(), requestParams.Tenant);
                _MyProceduralFaultPM = new ProceduralFaultPM();
                DeclarationPM myDeclarationPM = null;

                EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
                myInsertEventContextTagModel.MyNotificationPM = new NotificationPM();

                if (!string.IsNullOrWhiteSpace(faultId))
                {
                    this._MyProceduralFaultPM = proceduralFaultQueryService.GetSingle(faultId,true,false);
                    this._MyProceduralFaultPM.ChangeSetOp = ChangeSetOperation.Update;
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgUpdate;
                    DeleteFaultsConnectedEntity(proceduralFaultUpdateService);
                }
                else
                {
                    this._MyProceduralFaultPM.ChangeSetOp = ChangeSetOperation.Insert;
                    this._MyProceduralFaultPM.Tenant = requestParams.Tenant;
                    this._MyProceduralFaultPM.ProceduralFaultNumber = proceduralFaultItem.proceduralFaultNumber.ToString();
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgInsert;
                }
                
                this._MyProceduralFaultPM.ProceduralFaultStatusCode = proceduralFaultItem.proceduralFaultStatus.ToString();
                this._MyProceduralFaultPM.CreateDate = proceduralFaultItem.createDate;
                this._MyProceduralFaultPM.InputTypeCode = proceduralFaultItem.inputType.ToString();
                this._MyProceduralFaultPM.InspectionTypeCode = proceduralFaultItem.inspectionType.ToString();
                this._MyProceduralFaultPM.ProceduralFaultCode = proceduralFaultItem.proceduralFaultCode.ToString();
                this._MyProceduralFaultPM.ProceduralFaultInputProcesCode = proceduralFaultItem.proceduralFaultInputProcess.ToString();
                if (proceduralFaultItem.ransomViolationTypeSpecified == true)
                {
                    this._MyProceduralFaultPM.RansomViolationTypeCode = proceduralFaultItem.ransomViolationType.ToString();
                }
                if (proceduralFaultItem.ransomViolationSumSpecified == true)
                {
                    this._MyProceduralFaultPM.RansomViolationSum = proceduralFaultItem.ransomViolationSum;
                }
                this._MyProceduralFaultPM.Remarks = GetProceduralFaultRemarks(proceduralFaultItem, requestParams.Tenant);
                this._MyProceduralFaultPM.IsCustomerResponsibility = proceduralFaultItem.isCustomerResponsibility;
                this._MyProceduralFaultPM.IsAgentProceduralFaultCountabl = proceduralFaultItem.isAgentProceduralFaultCountable;
                this._MyProceduralFaultPM.IsCustProceduralFaultCountabl = proceduralFaultItem.isCustomerProceduralFaultCountable;
                this._MyProceduralFaultPM.IsAgentResponsibility = proceduralFaultItem.isAgentResponsibility;
                if (proceduralFaultItem.updateDateSpecified == true)
                {
                    this._MyProceduralFaultPM.UpdateDate = proceduralFaultItem.updateDate;
                }
                this._MyProceduralFaultPM.LeadingDocumentVersion = proceduralFaultItem.leadingDocumentVersion;
                if (proceduralFaultItem.ConnectedEntity != null)
                {
                    foreach (var conectedEntityItem in proceduralFaultItem.ConnectedEntity)
                    {
                        ProceduralFaultsConnEntityPM proceduralFaultsConnectedEntityPM = new ProceduralFaultsConnEntityPM();
                        proceduralFaultsConnectedEntityPM.ChangeSetOp = ChangeSetOperation.Insert;
                        proceduralFaultsConnectedEntityPM.Tenant = requestParams.Tenant;
                        proceduralFaultsConnectedEntityPM.EntityType = conectedEntityItem.entityType.ToString();
                        proceduralFaultsConnectedEntityPM.EntityIdKey1 = conectedEntityItem.entityIdKey1;
                        proceduralFaultsConnectedEntityPM.EntityIdKey2 = conectedEntityItem.entityIdKey2;
                        proceduralFaultsConnectedEntityPM.EntityIdKey3 = conectedEntityItem.entityIdKey3;
                        proceduralFaultsConnectedEntityPM.EntityPath = conectedEntityItem.entityPath;

                        if (conectedEntityItem.entityType == 1055)
                        {
                            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                            myDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(conectedEntityItem.entityIdKey1, requestParams.Tenant);
                            if (myDeclarationPM != null && !string.IsNullOrWhiteSpace(myDeclarationPM.Id))
                            {
                                this._MyProceduralFaultPM.DeclarationId = myDeclarationPM.Id;
                                this._MyProceduralFaultPM.CustomFileNo = myDeclarationPM.CustomFileNo;
                                this._MyProceduralFaultPM.DeclarationNumber = myDeclarationPM.DeclarationNumber;

                                //Event data initialization
                                myInsertEventContextTagModel.EventCode = "LIK";
                                myInsertEventContextTagModel.EventRemarks = "New ProceduralFault";
                                if (this._MyProceduralFaultPM.ChangeSetOp == ChangeSetOperation.Update)
                                {
                                    myInsertEventContextTagModel.EventRemarks = "ProceduralFault Updated";
                                }
                                myInsertEventContextTagModel.StatusObjectTable = "Customs.Declaration";
                                myInsertEventContextTagModel.StatusEntityId = myDeclarationPM.Id;
                                myInsertEventContextTagModel.StatusCustomFileNo = myDeclarationPM.CustomFileNo;
                                //Notification data initialization
                                myInsertEventContextTagModel.MyNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                                myInsertEventContextTagModel.MyNotificationPM.EntityId = myDeclarationPM.Id;
                                myInsertEventContextTagModel.MyNotificationPM.DeclarationOfficeCode = myDeclarationPM.DeclarationOfficeCode;
                                myInsertEventContextTagModel.MyNotificationPM.DepartmentId = myDeclarationPM.DepartmentId;
                                myInsertEventContextTagModel.MyNotificationPM.Reference1Number = myDeclarationPM.ReferentUserId;
                                myInsertEventContextTagModel.MyNotificationPM.CustomerId = myDeclarationPM.CustomerId; // moran 13.9.16 - Bug 22397 change from MyNotificationPM.AssigneToId to MyNotificationPM.CustomerId
                                if (this._MyProceduralFaultPM.ChangeSetOp == ChangeSetOperation.Insert)
                                {
                                    myInsertEventContextTagModel.MyNotificationPM.Description = "תיק " + myDeclarationPM.CustomFileNo + "- התקבל ליקוי מכס";
                                }
                                else
                                {
                                    myInsertEventContextTagModel.MyNotificationPM.Description = "תיק " + myDeclarationPM.CustomFileNo + "- עודכן ליקוי מכס";
                                }
                            }
                        }
                        this._MyProceduralFaultPM.ProceduralFaultsConnEntities.Add(proceduralFaultsConnectedEntityPM);
                    }
                }
                this._MyProceduralFaultPM.CurrentContextTag = myInsertEventContextTagModel;
                proceduralFaultUpdateService.Update(this._MyProceduralFaultPM,true);

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.ProceduralFault");
                this.MyRequestSheetParam.EntityId1 = this._MyProceduralFaultPM.Id;
                this.MyRequestSheetParam.RequestDescription = "התקבל ליקוי מכס " + this._MyProceduralFaultPM.ProceduralFaultNumber;
                if (myDeclarationPM != null)
                {
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = myDeclarationPM.Id;
                    this.MyRequestSheetParam.CustomFileNo = myDeclarationPM.CustomFileNo;
                    this.MyRequestSheetParam.RequestDescription = "תיק " + myDeclarationPM.CustomFileNo + "- התקבל ליקוי מכס";
                    if (myDeclarationPM.IsConvertedDeclaration)
                    {
                        this.MyRequestSheetParam.RequestDescription = string.Concat(MyRequestSheetParam.RequestDescription, "\n", myDeclarationPM.UserNotes);
                    }
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = this._MyProceduralFaultPM.Id,
                Succeeded = true,
                UserMessage = "התקבל ליקוי מכס " + this._MyProceduralFaultPM.ProceduralFaultNumber,
                HasException = false,
            };
        }

        private string GetProceduralFaultRemarks(ProceduralFaultDetails proceduralFaultItem, int tenant)
        {
            string remarks = "";
            string faultUpdateWorkerRemarks = "";
            string organizationUnitTypeNameUpdate = "";
            string customsHouseTypeNameUpdate = "";
            string faultDiscovererWorkerRemarks = "";
            string organizationUnitTypeNameDiscoverer = "";
            string customsHouseTypeNameDiscoverer = "";
          
            if (proceduralFaultItem.FaultUpdateWorker != null)
            {
                if (proceduralFaultItem.FaultUpdateWorker.organizationUnitType > 0)
                {
                    OrganizationUnitTypeQueryService organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(tenant);
                    OrganizationUnitTypePM organizationUnitTypePM = organizationUnitTypeQueryService.GetSingle(proceduralFaultItem.FaultUpdateWorker.organizationUnitType.ToString(), false, true);
                    organizationUnitTypeNameUpdate = organizationUnitTypePM.LocalName;
                }
                if (proceduralFaultItem.FaultUpdateWorker.customsHouse > 0)
                {
                    CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(tenant);
                    CustomsHouseTypePM customsHouseTypePM = customsHouseTypeQueryService.GetSingle(proceduralFaultItem.FaultUpdateWorker.customsHouse.ToString(), false, true);
                    if (customsHouseTypePM != null)
                    {
                        customsHouseTypeNameUpdate = customsHouseTypePM.LocalName;
                    }
                }

                faultUpdateWorkerRemarks = "עודכן ע''י:" + "\n" +
                    "יחידה מקצועית: " + organizationUnitTypeNameUpdate + "\n" +
                    "בית מכס: " + customsHouseTypeNameUpdate + "\n" +
                    "עובד:" + proceduralFaultItem.FaultUpdateWorker.workerName;
            }

            if (proceduralFaultItem.FaultDiscovererWorker != null)
            {
                if (proceduralFaultItem.FaultDiscovererWorker.organizationUnitType > 0)
                {
                    if (proceduralFaultItem.FaultUpdateWorker.organizationUnitType.Equals(proceduralFaultItem.FaultDiscovererWorker.organizationUnitType))
                    {
                        organizationUnitTypeNameDiscoverer = organizationUnitTypeNameUpdate;
                    }
                    else
                    {
                        OrganizationUnitTypeQueryService organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(tenant);
                        OrganizationUnitTypePM organizationUnitTypePM = organizationUnitTypeQueryService.GetSingle(proceduralFaultItem.FaultDiscovererWorker.organizationUnitType.ToString(), false, true);
                        organizationUnitTypeNameDiscoverer = organizationUnitTypePM.LocalName;
                    }
                }
                if (proceduralFaultItem.FaultDiscovererWorker.customsHouse > 0)
                {
                    if (proceduralFaultItem.FaultUpdateWorker.organizationUnitType.Equals(proceduralFaultItem.FaultDiscovererWorker.organizationUnitType))
                    {
                        customsHouseTypeNameDiscoverer = customsHouseTypeNameUpdate;
                    }
                    else
                    {
                        CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(tenant);
                        CustomsHouseTypePM customsHouseTypePM = customsHouseTypeQueryService.GetSingle(proceduralFaultItem.FaultDiscovererWorker.customsHouse.ToString(), false, true);
                        customsHouseTypeNameDiscoverer = customsHouseTypePM.LocalName;
                    }
                }

                faultDiscovererWorkerRemarks = "אותר ע''י:" + "\n" +
                    "יחידה מקצועית: " + organizationUnitTypeNameDiscoverer + "\n" +
                    "בית מכס: " + customsHouseTypeNameDiscoverer + "\n" +
                    "עובד:" + proceduralFaultItem.FaultDiscovererWorker.workerName;
            }

            remarks = proceduralFaultItem.remarks + "\n" + faultUpdateWorkerRemarks + "\n" + faultDiscovererWorkerRemarks;
            return remarks;

        }

        public override INF_MSG_GenericResponseData GetResponse(EV_NG_8218_MSG14100_ProceduralFaultMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void DeleteFaultsConnectedEntity(ProceduralFaultUpdateService proceduralFaultUpdateService)
        {
            if(this._MyProceduralFaultPM.ProceduralFaultsConnEntities == null)
            {
                return;
            }

            foreach (var item in this._MyProceduralFaultPM.ProceduralFaultsConnEntities)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
                this._MyProceduralFaultPM.DeletedProceduralFaultsConnEntities.Add(item);
            }
        }
    }
}
