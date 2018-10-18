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
    public class EV_NG_8219_MSG14100_ProceduralFaultCancelMsgResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, EV_NG_8219_MSG14100_ProceduralFaultCancelMsg, GenericRequestParams>
    {
        public override void Update(EV_NG_8219_MSG14100_ProceduralFaultCancelMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 8219-  Fault Cancelled (DCA)
            var context = CustomContext.GetContext(requestParams.Tenant);
            var proceduralFaultQueryService = new ProceduralFaultQueryService(requestParams.Tenant);
            var proceduralFaultUpdateService = new ProceduralFaultUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            ProceduralFaultPM proceduralFaultPM = null;
            DeclarationPM myDeclarationPM = null;
            bool notificationFlag = true;
            
            foreach (var proceduralFaultItem in customResponse.ProceduralFaultCancelMsg)
            {
                string faultId = proceduralFaultQueryService.GetFaultIdByFaultNumber(proceduralFaultItem.proceduralFaultNumber.ToString(), requestParams.Tenant);
                proceduralFaultPM = new ProceduralFaultPM();

                if (!string.IsNullOrWhiteSpace(faultId))
                {
                    proceduralFaultPM = proceduralFaultQueryService.GetSingle(faultId, true, false);
                    proceduralFaultPM.ChangeSetOp = ChangeSetOperation.Update;
                    
                    EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
                    myInsertEventContextTagModel.MyNotificationPM = new NotificationPM();
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8218_MSG14100_ProceduralFaultMsgCancel;
                    myInsertEventContextTagModel.EventCode = "LIC";
                    myInsertEventContextTagModel.EventRemarks = "ProceduralFault Cancelled";

                    proceduralFaultPM.ProceduralFaultStatusCode = proceduralFaultItem.proceduralFaultStatus.ToString();
                    proceduralFaultPM.ProceduralFaultCode = proceduralFaultItem.proceduralFaultCode.ToString();
                    proceduralFaultPM.UpdateDate = proceduralFaultItem.updateDate;

                    if (customResponse.GeneralDetails != null && notificationFlag == true)
                    {
                        var declarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.GeneralDetails.declarationId, requestParams.Tenant);
                        if (declarationId != null)
                        {
                            myDeclarationPM = myDeclarationQueryService.GetSingle(declarationId, false, false);
                            proceduralFaultPM.DeclarationNumber = myDeclarationPM.DeclarationNumber;
                            //Event data initialization
                            myInsertEventContextTagModel.StatusObjectTable = "Customs.Declaration";
                            myInsertEventContextTagModel.StatusEntityId = declarationId;
                            myInsertEventContextTagModel.StatusCustomFileNo = myDeclarationPM.CustomFileNo;
                            //Notification data initialization
                            myInsertEventContextTagModel.MyNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                            myInsertEventContextTagModel.MyNotificationPM.EntityId = declarationId;
                            myInsertEventContextTagModel.MyNotificationPM.DeclarationOfficeCode = myDeclarationPM.DeclarationOfficeCode;
                            myInsertEventContextTagModel.MyNotificationPM.DepartmentId = myDeclarationPM.DepartmentId;
                            myInsertEventContextTagModel.MyNotificationPM.Reference1Number = myDeclarationPM.ReferentUserId;
                            myInsertEventContextTagModel.MyNotificationPM.CustomerId = myDeclarationPM.CustomerId; // moran 13.9.16 - Bug 22397 change from MyNotificationPM.AssigneToId to MyNotificationPM.CustomerId
                            myInsertEventContextTagModel.MyNotificationPM.Description = "תיק " + myDeclarationPM.CustomFileNo + "- בוטל ליקוי מכס";
                            notificationFlag = false;
                        }
                    }

                    proceduralFaultPM.CurrentContextTag = myInsertEventContextTagModel;
                    proceduralFaultUpdateService.Update(proceduralFaultPM, true);
                }
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.ProceduralFault");
            this.MyRequestSheetParam.EntityId1 = proceduralFaultPM.Id;
            this.MyRequestSheetParam.RequestDescription = "בוטל ליקוי מכס" ;
            if (myDeclarationPM != null)
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = myDeclarationPM.Id;
                this.MyRequestSheetParam.CustomFileNo = myDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.RequestDescription = "תיק " + myDeclarationPM.CustomFileNo + "- בוטל ליקוי מכס";
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = proceduralFaultPM.Id,
                Succeeded = true,
                UserMessage = null,
                HasException = false,
            };
        }

        public override INF_MSG_GenericResponseData GetResponse(EV_NG_8219_MSG14100_ProceduralFaultCancelMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}
