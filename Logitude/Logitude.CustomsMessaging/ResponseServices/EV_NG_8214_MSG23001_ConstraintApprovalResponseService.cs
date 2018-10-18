using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.ConstraintApprovalRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class EV_NG_8214_MSG23001_ConstraintApprovalResponseService
        : ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, ConstraintApprovalRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(INF_MSG_Generic customResponse, ConstraintApprovalRequestParams requestParams)
        {
            bool succeeded = false;
            string applicationId = null;
            bool hasException = false;
            string exceptionMessage = null;
            if (customResponse.ResponseContentHeader.Exception == null)
            {
                succeeded = true;
                applicationId = customResponse.ResponseContentHeader.ApplicationID.ToString();
                hasException = false;
            }
            else
            {
                string ExceptionDescription = "";
                var ExeptionDescription = "";
                if (customResponse.ResponseContentHeader.Exception != null)
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
                    hasException = true;
                    exceptionMessage = ExceptionDescription;
                }
            }

            SendDeclarationStatus(customResponse, requestParams); 

            INF_MSG_GenericResponseData responseData = new INF_MSG_GenericResponseData() { Succeeded = succeeded, ApplicationID = applicationId, HasException = hasException, UserMessage = exceptionMessage };
            return responseData;
        }


        public void SendDeclarationStatus(INF_MSG_Generic customResponse, ConstraintApprovalRequestParams requestParams) // moran 29.12.14 - Task 9792
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(dbContext);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
            if (declarationPM != null)
            {
                try
                {
                    DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
                    {
                        LoggingEnabled = true,

                        CustomFileNo = declarationPM.CustomFileNo,
                        DeclarationNumber = declarationPM.DeclarationNumber,
                        Tenant = declarationPM.Tenant,
                        RequestName = "Declaration Status Search",
                        ResponseName = "Declaration Status Search",
                        SuppressSplitWR = true
                    };

                    searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
                    var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
                    if (!resData.Succeeded)
                    {
                        LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                        return;
                    }
                    LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
                }
                catch
                {
                    LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed !!!");
                }
            }

            
        }

        public override void Update(INF_MSG_Generic customResponse, ConstraintApprovalRequestParams requestParams)
        {
            DeclarationConstraintPM declarationConstraintPM;

            if(customResponse.ResponseContentHeader.Exception == null)
            {
                ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                var declarationQueryService = new DeclarationQueryService(dbContext);

                DeclarationPM declarationPM = declarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
                if (declarationPM != null)
                {
                    var declarationConstraintQueryService = new DeclarationConstraintQueryService(dbContext);
                    var declarationConstraintUpdateService = new DeclarationConstraintUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

                    declarationConstraintPM = declarationConstraintQueryService.GetSingle(declarationPM.Id, requestParams.ConstraintNumber, true, false);
                    if (declarationConstraintPM != null)
                    {
                        declarationConstraintPM.ChangeSetOp = ChangeSetOperation.Update;
                        declarationConstraintPM.ConstraintStatusCode = "2";

                        var myInsertEventContextTagModel = new EventContextTagModel() 
                        {
                            CallProccessID = EventContextTagModel.ProccessEnum.EV_NG_8214_MSG23001_ConstraintApproval,
                            EventCode = "RCA",
                            EventRemarks = "Constraint Sent to Custom"
                        };
                        declarationConstraintPM.CurrentContextTag = myInsertEventContextTagModel;                          
                        declarationConstraintUpdateService.Update(declarationConstraintPM, true);
                    }
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = requestParams.ConstraintNumber,
                Succeeded = true,
                HasException = false,
            };
            // moran 22.1.15 - Task 9659 <--
        }
    }
}
