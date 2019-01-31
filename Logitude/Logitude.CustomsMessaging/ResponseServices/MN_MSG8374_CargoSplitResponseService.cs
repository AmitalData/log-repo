using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using UnifreightIIG.Common.CargoSplitSaveServiceReference;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityUpdateServices;
using Logitude.Server.Tools.Models;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data.EntityKeys;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class MN_MSG8374_CargoSplitResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, MN_MSG8374_CargoSplitRequestFeedBack_Message, CargoSplitRequestParams>
    {

        public override void Update(MN_MSG8374_CargoSplitRequestFeedBack_Message customResponse, CargoSplitRequestParams requestParams)
        {
            bool succeeded = false;
            string applicationId = null;
            bool hasException = false;
            string exceptionMessage = null;
            string message = null;

            if (customResponse == null)
            {
                message = "customResponse is empty";
                LogMessagingUtil.Instance.AppendLine(message);
                exceptionMessage = message;
                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    Succeeded = succeeded,
                    ApplicationID = applicationId,
                    HasException = hasException,
                    UserMessage = exceptionMessage,
                };
                return;
            }

            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationCargoSplitQueryService = new DeclarationCargoSplitQueryService(dbContext);
            DeclarationCargoSplitPM _DeclarationCargoSplitPM;
            string declarationCargoSplitID = null;
            string cargoSpllitRequestNumber = null;

            if (customResponse.ResponseContentHeader.Exception == null)
            {

                if (customResponse != null && customResponse.CargoSplitRequestResponse != null && customResponse.CargoSplitRequestResponse.CargoSpllitRequestNumberSpecified)
                {
                    cargoSpllitRequestNumber = customResponse.CargoSplitRequestResponse.CargoSpllitRequestNumber.ToString();
                    declarationCargoSplitID = myDeclarationCargoSplitQueryService.GetIdByDeclarationCargoSplitRequestNumber(cargoSpllitRequestNumber, requestParams.Tenant);
                }
                if (string.IsNullOrWhiteSpace(declarationCargoSplitID))
                {
                    if (customResponse != null && customResponse.CargoSplitRequestResponse != null && customResponse.CargoSplitRequestResponse.CargoIdentifier != null && customResponse.CargoSplitRequestResponse.CargoIdentifier.Count() > 0)
                    {
                        declarationCargoSplitID = myDeclarationCargoSplitQueryService.GetIdByCargoIdentifiers(customResponse.CargoSplitRequestResponse.CargoIdentifier[0].cargoIdentifierKey1, customResponse.CargoSplitRequestResponse.CargoIdentifier[0].cargoIdentifierKey2, customResponse.CargoSplitRequestResponse.CargoIdentifier[0].cargoIdentifierKey3, customResponse.CargoSplitRequestResponse.CargoIdentifier[0].cargoIdentifierType, requestParams.Tenant);
                    }
                }

                if (string.IsNullOrWhiteSpace(declarationCargoSplitID) && !string.IsNullOrWhiteSpace(requestParams.DeclarationCargoSplit))
                {
                    declarationCargoSplitID = requestParams.DeclarationCargoSplit;
                }

                if (string.IsNullOrWhiteSpace(declarationCargoSplitID))
                {
                    message = "Declaration Cargo Split ID is missing (External Number: " + cargoSpllitRequestNumber + ")";
                    LogMessagingUtil.Instance.AppendLine(message);
                    hasException = true;
                    exceptionMessage = message;
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        Succeeded = succeeded,
                        ApplicationID = applicationId,
                        HasException = hasException,
                        UserMessage = exceptionMessage,
                    };

                    return;
                }

                _DeclarationCargoSplitPM = myDeclarationCargoSplitQueryService.GetSingle(declarationCargoSplitID, true, false);

                if (_DeclarationCargoSplitPM == null)
                {
                    message = "Declaration Cargo Split with ID " + declarationCargoSplitID + " does not exist";
                    LogMessagingUtil.Instance.AppendLine(message);
                    exceptionMessage = message;
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        Succeeded = succeeded,
                        ApplicationID = applicationId,
                        HasException = hasException,
                        UserMessage = exceptionMessage,
                    };
                    return;
                }

                if(this.MyRequestSheetParam == null)this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.DeclarationCargoSplit");

                if (!string.IsNullOrEmpty(_DeclarationCargoSplitPM.DeclarationId))
                {
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = _DeclarationCargoSplitPM.DeclarationId;
                }

                succeeded = true;
                applicationId = customResponse.ResponseContentHeader.ApplicationID.ToString();
                hasException = false;
                exceptionMessage = "מענה לבקשת פיצול מטען נשלח בהצלחה";
                string userMess = null;

                if ((customResponse.CargoSplitRequestResponse.CargoSpllitRequestNumberSpecified && string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.RequestNumber))
                 || (customResponse.CargoSplitRequestResponse.responseStatus != _DeclarationCargoSplitPM.ResponseStatusCode))
                {
                    var DeclarationCargoSplitUpdateService = new DeclarationCargoSplitUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                    _DeclarationCargoSplitPM.ChangeSetOp = ChangeSetOperation.Update;
                    if (customResponse.CargoSplitRequestResponse.CargoSpllitRequestNumberSpecified && string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.RequestNumber))
                    {
                        _DeclarationCargoSplitPM.RequestNumber = customResponse.CargoSplitRequestResponse.CargoSpllitRequestNumber.ToString();
                    }
                    if (customResponse.CargoSplitRequestResponse.responseStatus != _DeclarationCargoSplitPM.ResponseStatusCode)
                    {
                        _DeclarationCargoSplitPM.ResponseStatusCode = customResponse.CargoSplitRequestResponse.responseStatus;
                        userMess = RaiseDeclarationCargoSplitResponseStatus(_DeclarationCargoSplitPM, customResponse, requestParams);
                        if (userMess != null) exceptionMessage = userMess;
                    }
                    if (customResponse.CargoSplitRequestResponse.CargoIdentifier != null)
                    {
                        if (_DeclarationCargoSplitPM.DecCargoSplitCargoIdentifiers != null)
                        {
                            var myDecCargoSplitCargoIdentifierUpdateService = new DecCargoSplitCargoIdentifierUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                            var myDeclarationCargoSplitKeys = new DeclarationCargoSplitKeys { Id = _DeclarationCargoSplitPM.Id };
                            myDecCargoSplitCargoIdentifierUpdateService.FastDeleteComposition(myDeclarationCargoSplitKeys);
                        }
                        var myDecCargoSplitCargoIdentifierPMList = new List<DecCargoSplitCargoIdentifierPM>();
                        int Counter = 0;
                        foreach (var item in customResponse.CargoSplitRequestResponse.CargoIdentifier.Skip(1))
                        {
                            var myDecCargoSplitCargoIdentifierPM = new DecCargoSplitCargoIdentifierPM();
                            myDecCargoSplitCargoIdentifierPM.DeclarationCargoSplitId = _DeclarationCargoSplitPM.Id;
                            myDecCargoSplitCargoIdentifierPM.Tenant = _DeclarationCargoSplitPM.Tenant;
                            myDecCargoSplitCargoIdentifierPM.LineNumber = Counter++;
                            myDecCargoSplitCargoIdentifierPM.CargoIdentifierKey1 = item.cargoIdentifierKey1;
                            myDecCargoSplitCargoIdentifierPM.CargoIdentifierKey2 = item.cargoIdentifierKey2;
                            myDecCargoSplitCargoIdentifierPM.CargoIdentifierKey3 = item.cargoIdentifierKey3;
                            myDecCargoSplitCargoIdentifierPM.ChangeSetOp = ChangeSetOperation.Insert;
                            myDecCargoSplitCargoIdentifierPMList.Add(myDecCargoSplitCargoIdentifierPM);

                        }
                        if(myDecCargoSplitCargoIdentifierPMList != null)
                        {
                            _DeclarationCargoSplitPM.DecCargoSplitCargoIdentifiers = myDecCargoSplitCargoIdentifierPMList;
                        }
                    }
                    DeclarationCargoSplitUpdateService.Update(_DeclarationCargoSplitPM, true);
                }
                else
                {
                    string notificationDescription = "";
                    string customsFileNo = "";
                    DeclarationPM declaration = new DeclarationPM();

                    if (!string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.DeclarationId))
                    {
                        var declarationQueryService = new DeclarationQueryService(_DeclarationCargoSplitPM.Tenant);
                        declaration = declarationQueryService.GetSingle(_DeclarationCargoSplitPM.DeclarationId, false, true);
                        if (declaration != null && !string.IsNullOrWhiteSpace(declaration.CustomFileNo))
                        {
                            customsFileNo = declaration.CustomFileNo;
                        }
                    }
                    switch (_DeclarationCargoSplitPM.ResponseStatusCode)
                    {
                        case "1":
                            notificationDescription = "בקשת פיצול מטען מס' " + _DeclarationCargoSplitPM.RequestNumber + " אושרה " + customsFileNo;
                            break;
                        case "2":
                            notificationDescription = "בקשת פיצול מטען מס' " + _DeclarationCargoSplitPM.RequestNumber + " נדחתה " + customsFileNo;
                            break;
                        case "4":
                        case "7":
                            notificationDescription = "בקשת פיצול מטען מס' " + _DeclarationCargoSplitPM.RequestNumber + " בוטלה " + customsFileNo;
                            break;
                        case "6":
                            notificationDescription = "בוצע פיצול מטען  " + customsFileNo;
                            break;
                        default:
                            notificationDescription = "התקבל משוב לפיצול מטען " + customsFileNo;
                            break;

                    }
                    this.MyRequestSheetParam.RequestDescription = notificationDescription;
                }
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

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                Succeeded = succeeded,
                ApplicationID = applicationId,
                HasException = hasException,
                UserMessage = exceptionMessage,
            };

        }


        private string RaiseDeclarationCargoSplitResponseStatus(DeclarationCargoSplitPM _DeclarationCargoSplitPM, MN_MSG8374_CargoSplitRequestFeedBack_Message customResponse, CargoSplitRequestParams requestParams)
        {
            string userMessage = null;
            string status = null;
            string notificationDefinitionCode = "";
            string notificationDescription = "";
            string notificationStatusCode = "";
            string assigneToNotificationTypeCode = "I";
            string customsFileNo = "";
            DeclarationPM declaration = new DeclarationPM();

            if (!string.IsNullOrWhiteSpace(_DeclarationCargoSplitPM.DeclarationId))
            {
                var declarationQueryService = new DeclarationQueryService(_DeclarationCargoSplitPM.Tenant);
                declaration = declarationQueryService.GetSingle(_DeclarationCargoSplitPM.DeclarationId, false, true);
                if (declaration != null && !string.IsNullOrWhiteSpace(declaration.CustomFileNo))
                {
                    customsFileNo = declaration.CustomFileNo;
                }
            }

            switch (_DeclarationCargoSplitPM.ResponseStatusCode)
            {
                case "1":
                    status = "CSA";
                    notificationDefinitionCode = "8374A";
                    notificationDescription = "בקשת פיצול מטען מס' " + _DeclarationCargoSplitPM.RequestNumber + " אושרה " + customsFileNo;
                    userMessage = "בקשת פיצול מטען " + _DeclarationCargoSplitPM.RequestNumber + " אושרה";
                    break;
                case "2":
                    status = "CSJ";
                    notificationDefinitionCode = "8374J";
                    notificationDescription = "בקשת פיצול מטען מס' " + _DeclarationCargoSplitPM.RequestNumber + " נדחתה " + customsFileNo;
                    userMessage = "בקשת פיצול מטען " + _DeclarationCargoSplitPM.RequestNumber + " נדחתה";
                    break;
                case "4":
                case "7":
                    status = "CSC";
                    notificationDefinitionCode = "8374C";
                    notificationDescription = "בקשת פיצול מטען מס' " + _DeclarationCargoSplitPM.RequestNumber + " בוטלה " + customsFileNo;
                    userMessage = "בקשת פיצול מטען " + _DeclarationCargoSplitPM.RequestNumber + " בוטלה";
                    break;
                case "6":
                    status = "CSD";
                    notificationDefinitionCode = "8374D";
                    notificationDescription = "בוצע פיצול מטען  " + customsFileNo;
                    userMessage = "בוצע פיצול מטען לבקשה " + _DeclarationCargoSplitPM.RequestNumber;
                    break;
            }

            notificationStatusCode = status;
            
            this.MyRequestSheetParam.RequestDescription = notificationDescription;

                if (!string.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
            {
                notificationDescription = notificationDescription + "\n" + customResponse.ResponseContentHeader.Remark;
            }


            if (!string.IsNullOrWhiteSpace(notificationDefinitionCode))
            {
                LogMessagingUtil.Instance.AppendLine("Start Sending Notification... ");
                DoUpdateNotification(notificationDefinitionCode, _DeclarationCargoSplitPM.Tenant, customResponse.ResponseContentHeader.Remark, notificationDescription, assigneToNotificationTypeCode, declaration, _DeclarationCargoSplitPM);
            }

            this.MyRequestSheetParam.CustomFileNo = customsFileNo;

            if (!string.IsNullOrWhiteSpace(customsFileNo))
            {
                ICustomContext dbContext = CustomContext.GetContext(_DeclarationCargoSplitPM.Tenant);
                var DeclarationCargoSplitUpdateService = new DeclarationCargoSplitUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _DeclarationCargoSplitPM.Tenant);
                DeclarationCargoSplitUpdateService.RaiseDeclarationCargoSplitEventAndStatus(status, status, _DeclarationCargoSplitPM, declaration, false, "", "");
            }
            return (userMessage);
        }



        private void DoUpdateNotification(string notificationDefinitionCode, int tenant, string responseToMessage, string description, string typeCode, DeclarationPM connectedDeclarationPM, DeclarationCargoSplitPM _DeclarationCargoSplitPM)
        {
            LogMessagingUtil.Instance.AppendLine("New Message To Agent Request Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = description;
            newNotificationPM.Reference2Number = responseToMessage;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = typeCode;
            if(_DeclarationCargoSplitPM != null)
            {
                newNotificationPM.EntityId = _DeclarationCargoSplitPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.DeclarationCargoSplit");
            }

            string customerId = null;
            string referentUserId = null;
            if (connectedDeclarationPM != null)
            {
                if (_DeclarationCargoSplitPM != null) newNotificationPM.Reference2Number = _DeclarationCargoSplitPM.Id;
                newNotificationPM.EntityId = connectedDeclarationPM.Id;
                newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                newNotificationPM.Reference1Number = connectedDeclarationPM.CustomFileNo;
                newNotificationPM.DepartmentId = connectedDeclarationPM.DepartmentId;
                newNotificationPM.DeclarationOfficeCode = connectedDeclarationPM.DeclarationOfficeCode;
                customerId = connectedDeclarationPM.CustomerId;
                referentUserId = connectedDeclarationPM.ReferentUserId;
            }
            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(connectedDeclarationPM.CustomerId)) newNotificationPM.CustomerId = connectedDeclarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, "");

            if (connectedDeclarationPM != null && !string.IsNullOrWhiteSpace(newNotificationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }

            notificationUpdateService.Update(newNotificationPM, true);
        }

        public override INF_MSG_GenericResponseData GetResponse(MN_MSG8374_CargoSplitRequestFeedBack_Message customResponse, CargoSplitRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}




