using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifreightIIG.Common.MessageLib.Deficit;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DE_NG_5107_MSG10_AcceptanceOrRejectionMessageResponseService :
        ResponseServiceBase<
        INF_MSG_GenericResponseData,
        DE_NG_5107_MSG10_AcceptanceOrRejectionMessage,
        GenericRequestParams>
        
    {
        DeclarationPM _MyDeclarationPM;

        public override void Update(DE_NG_5107_MSG10_AcceptanceOrRejectionMessage customResponse,
            GenericRequestParams requestParams)
        {
            try
            {
                var context = CustomContext.GetContext(requestParams.Tenant);
                var myQueryService = new DeclarationQueryService(context);
                var myUpdateService = new //DeclarationUpdateService(context);
                DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                Decimal debitAmountSum = 0;
                MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = true };

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = "מסר דחיה/אישור גרעון עצמי " + customResponse.AcceptanceOrRejectionMessage.leadingFileNumber;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

                //Check if the response is OK
                if (customResponse.AcceptanceOrRejectionMessage == null)
                {
                    LogMessagingUtil.Instance.AppendLine("No Response Data - AcceptanceOrRejectionMessage empty");
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "No Response Data - AcceptanceOrRejectionMessage empty" };
                    return;
                }
                if (customResponse.DeficitFile == null)
                {
                    LogMessagingUtil.Instance.AppendLine("No Response Data - DeficitFile empty");
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "No Response Data - DeficitFile empty" };
                    return;
                }
                if (customResponse.DeficitFile.TPGIdentifier == null)
                {
                    LogMessagingUtil.Instance.AppendLine("No Response Data - TPGIdentifier empty");
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "No Response Data - TPGIdentifier empty" };
                    return;
                }
                var declaration = customResponse.DeficitFile.TPGIdentifier.fileNumber;
                if (String.IsNullOrWhiteSpace(declaration))
                {
                    LogMessagingUtil.Instance.AppendLine("No Declaration No. in TPGIdentifier");
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "No Declaration No. in TPGIdentifier" };
                    return;
                }

                //Find Declaration
                declaration = myQueryService.GetIdByDeclarationNumber(declaration, requestParams.Tenant);
                if (String.IsNullOrWhiteSpace(declaration))
                {
                    LogMessagingUtil.Instance.AppendLine("No Declaration found for declaration Number " + declaration);
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "No Declaration found for declaration Number " + declaration };
                    return;
                } 
                this._MyDeclarationPM = myQueryService.GetSingle(declaration, true, false);
                if (this._MyDeclarationPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("Cannot find declaration " + declaration);
                    MyResponseData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "Cannot find declaration " + declaration };
                    return;
                }

                this.MyRequestSheetParam.EntityId1 = _MyDeclarationPM.Id;
                this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM.CustomFileNo;

                //String the status remarks
                if (customResponse.DeficitFile.DebtAmount != null)
                {
                    foreach (var debtAmount in customResponse.DeficitFile.DebtAmount)
                    {
                        debitAmountSum = debitAmountSum + debtAmount.аmount;
                    }
                }
                string remarks = "Deficit Customs Answer" + "\n" +
                                "Decision Code: " + customResponse.AcceptanceOrRejectionMessage.decisionCode + "\n" +
                                ", Decision Note: " + customResponse.AcceptanceOrRejectionMessage.decisionNote + "\n" +
                                ", Leading file number: " + customResponse.AcceptanceOrRejectionMessage.leadingFileNumber + "\n" +
                                ", Total Debit Amount: " + debitAmountSum;

                var myUpdateEventContextTagModel = new EventContextTagModel()
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.DE_NG_5107_MSG10_AcceptanceOrRejectionMessageResponseService,
                    EventCode = "DCA",
                    EventRemarks = remarks,
                    FUStatusRemarks = remarks,
                };

                RaiseEvent(_MyDeclarationPM, requestParams.LoggingUserId, "DCA", remarks);

                _MyDeclarationPM.CurrentContextTag = myUpdateEventContextTagModel;
                _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                LogMessagingUtil.Instance.AppendLine("customResponse.DeficitFile.TPGIdentifier.fileNumber:" + customResponse.DeficitFile.TPGIdentifier.fileNumber.ToString() + " Update");

                _MyDeclarationPM.Tenant = requestParams.Tenant;
                myUpdateService.Update(_MyDeclarationPM, true);

                MyResponseData.ApplicationID = requestParams.AppicationId = declaration;

            }
            catch (System.Exception ee)
            {
                MyResponseData = null;
                ///MyResponseData = new INF_MSG_GenericResponseData() { HasException = true, ExceptionMessage = ee.ToString() };
                throw;
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(DE_NG_5107_MSG10_AcceptanceOrRejectionMessage customResponse, GenericRequestParams requestParams)
        {
            var tmpResData = MyResponseData;
            if (tmpResData == null)
            {
                tmpResData = new INF_MSG_GenericResponseData() { Succeeded = false, HasException = true, UserMessage = "Unknown ??" };
            }

            return tmpResData;
        }

        private void RaiseEvent(DeclarationPM _MyDeclarationPM, string loggingUserId, string eventCode, string remarks)
        {
            var myAmitalEventTracer = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
            {
                Tenant = _MyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                EventCode = eventCode,
                notes = remarks,
                CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                EntityId = _MyDeclarationPM.Id,
                UserId = loggingUserId,

                CommunicationSubject = "FU Status " + eventCode + " from logitude Declaration DCA",
                MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = _MyDeclarationPM.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = eventCode,
                    status_DateTime = DateTime.Now,
                    //status_place = "DCA",
                    //status_save = "no_fail",
                    comments = remarks,
                }
            };

            LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
            AmitalEventTracer.CreateTraceEvent(myAmitalEventTracer);
        }
    }
}

