using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ChangingTimeServiceReference;
using UnifreightIIG.Common.MessageLib.Unifreight.FuStatus;
using UnifreightIIG.Common.MessageLib.Unifreight.Transmission;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using UnifreightIIG.Common.TheGateway;
using Logitude.Customs.Data.EntityMapping;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class CH_NG_192_MSG3_ApproveChangeTimeResponseService : ResponseServiceBase<CH_NG_192_MSG3_ApproveChangeTimeResponseData, CH_NG_192_MSG3_ApproveChangeTimeRequest, CH_NG_191_MSG2_ChangingTimeRequestParams>
    {
        public override CH_NG_192_MSG3_ApproveChangeTimeResponseData GetResponse(CH_NG_192_MSG3_ApproveChangeTimeRequest customResponse,CH_NG_191_MSG2_ChangingTimeRequestParams  requestParams)
        {
            CH_NG_192_MSG3_ApproveChangeTimeRequestApproveChangeTimeRequest approveChangeTimeRequest = customResponse.ApproveChangeTimeRequest;
            CH_NG_192_MSG3_ApproveChangeTimeResponseData customCheckData = new CH_NG_192_MSG3_ApproveChangeTimeResponseData();

            /*int.TryParse(requestParams.RequestType, out requestType);
            if (approveChangeTimeRequest != null)
            {
                if (requestType == 1)
                {
                    switch (approveChangeTimeRequest.queueType)
                    {
                        case 1:
                            customCheckData.XrayItems = approveChangeTimeRequest.availableTimeForXrayCheck.ToList();
                            break;
                        case 2:
                            customCheckData.XrayItems = approveChangeTimeRequest.availableTimeForStandardCheck.ToList();
                            break;
                        case 3:
                            customCheckData.XrayItems = null;
                            customCheckData.XrayItems = approveChangeTimeRequest.availableTimeForDualCheck.ToList();
                            break;
                    }
                    //if (customCheckData.PiscalCheckItems != null)
                    //{
                    //    customCheckData.PiscalCheckItems = myApproveChangeTimeRequest.availableTimeForStandardCheck.ToList();
                    //}
                    //else
                    //{
                    //    customCheckData.PiscalCheckItems = null;
                    //}
                }
            }*/

            if (approveChangeTimeRequest != null)
            {
                if (approveChangeTimeRequest.requestType == 1 && approveChangeTimeRequest.availableTime != null)
                {
                    // list of available dates & times
                    customCheckData.XrayItems = approveChangeTimeRequest.availableTime.ToList();
                }
            }

            string ExceptionDescription = "";
            var ExeptionDescription = "";
            if (customResponse?.ResponseContentHeader?.Exception != null)
            {
                foreach (var rec in customResponse.ResponseContentHeader.Exception)
                {
                 
                    if (!String.IsNullOrWhiteSpace(ExeptionDescription))
                    {
                        ExeptionDescription += Environment.NewLine;
                    }
                    ExeptionDescription += rec.ExeptionDescription;

                }


                ExceptionDescription = ExeptionDescription;
                customCheckData.HasException = true;
                customCheckData.UserMessage = ExceptionDescription;
            }
            if (!customCheckData.HasException)
            {
                if (requestParams.IsAngularClient)
                {
                    customCheckData.Succeeded = true;
                    customCheckData.UserMessage = "נשלח בהצלחה";
                }

            }
            return customCheckData;
        }

        public override void Update(
            CH_NG_192_MSG3_ApproveChangeTimeRequest customResponse,
            CH_NG_191_MSG2_ChangingTimeRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            //var CreateCommunicationService
            PhysicalCheckQueryService physicalCheckQueryService = new PhysicalCheckQueryService(requestParams.Tenant);
            PhysicalCheckUpdateService physicalCheckUpdateService = new PhysicalCheckUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            PhysicalCheckPM phsicalCheckPM = physicalCheckQueryService.GetSingle(requestParams.PhysicalCheckId, false, false);

            if (phsicalCheckPM == null)
            {
                phsicalCheckPM= physicalCheckQueryService.GetPhysicalCheckByCheckId(customResponse?.ApproveChangeTimeRequest?.checkId.ToString());
                if (phsicalCheckPM == null)
                {
                    return;
                }
            }


            CH_NG_192_MSG3_ApproveChangeTimeRequestApproveChangeTimeRequest approveChangeTimeRequest = customResponse.ApproveChangeTimeRequest;
            int requestType = approveChangeTimeRequest.requestType;

            switch (requestType)
            {
                case 1: // In .Case of list of available dates & times 
                    break;
                case 2: // In case of approval / deny of a requested date
                case 3: // In Case of automatic update
                    if (approveChangeTimeRequest.newDateSpecified && approveChangeTimeRequest.newDate != null)
                    {
                        DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
                        var declarationPM = declarationQueryService.GetSingle(phsicalCheckPM.DeclarationId, false, false); // Get declaration number for raising event
                        phsicalCheckPM.ChangeSetOp = ChangeSetOperation.Update;
                        phsicalCheckPM.LimitDate = approveChangeTimeRequest.newDate.Value; // Update date of the phsical Check

                        var myUpdateEventContextTagModel = new EventContextTagModel() // Raise event PUI
                        {
                            CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_190_MSG1_NoticeToClientResponseServiceUpdate,
                            EventCode = "PUI",
                            EventRemarks = "Physical Checks Updated , Check Id:" + phsicalCheckPM.CheckId,
                            FUStatusRemarks = "Check Id:" + phsicalCheckPM.CheckId + ", Limit Date:" + phsicalCheckPM.LimitDate + ", Queue Type:" + phsicalCheckPM.QueueTypeCode + ", Check Instruction:" + phsicalCheckPM.CheckEssence + ", Container Number:" + phsicalCheckPM.ContainerNubmer,
                        };
                        phsicalCheckPM.CurrentContextTag = myUpdateEventContextTagModel;

                        physicalCheckUpdateService.Update(phsicalCheckPM, true);
                    }
                    break;
                case 4:
                    phsicalCheckPM.BringQueueForwardIndicatorS = "4";
                    phsicalCheckPM.ChangeSetOp = ChangeSetOperation.Update;
                    physicalCheckUpdateService.Update(phsicalCheckPM, true);

                    break;
                case 5:
                    phsicalCheckPM.BringQueueForwardIndicatorS = "5";
                    var myDeleteEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.CH_NG_192_MSG1_QueueAdvanceDeniedResponseService,
                        EventCode = "PCB",
                        EventRemarks = "Queue advance denied",
                        FUStatusRemarks = "הקדמת תור נדחתה"
                    };
                    phsicalCheckPM.CurrentContextTag = myDeleteEventContextTagModel;
                    phsicalCheckPM.ChangeSetOp = ChangeSetOperation.Update;
                    physicalCheckUpdateService.Update(phsicalCheckPM, true);
                    break;
            }

            if(phsicalCheckPM != null && requestType != 0)
            {
                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PhysicalCheck");
                this.MyRequestSheetParam.EntityId1 = requestParams.PhysicalCheckId;

                if (requestType == 1 || requestType == 2)
                {
                    this.MyRequestSheetParam.RequestDescription = "שינוי מועד בדיקה " + phsicalCheckPM.CheckId;
                }
                else if(requestType == 4 || requestType == 5)
                {
                    this.MyRequestSheetParam.RequestDescription = "הקדמת תור" + phsicalCheckPM.CheckId;
                }
                else
                {
                    this.MyRequestSheetParam.RequestDescription = "חיפוש תורים לבדיקה " + phsicalCheckPM.CheckId;
                }
                if (phsicalCheckPM.DeclarationId != null)
                {
                    var declarationQueryService = new DeclarationQueryService(dbContext);
                    string customfileNumber = declarationQueryService.GetCustomFileNoByDeclarationId(phsicalCheckPM.DeclarationId, phsicalCheckPM.Tenant);
                    this.MyRequestSheetParam.CustomFileNo = customfileNumber;
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = phsicalCheckPM.DeclarationId;
                }

            }
        }



    }

}
