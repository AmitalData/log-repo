using Logitude.AmitalMessaging.Utils;
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
using System.Linq;
using System.Collections.Generic;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;
using Logitude.Customs.BL.TraceEvents;
using System.Configuration;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.AmitalMessaging.Customs.CustomFile;
using System.Globalization;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Messaging.Customs;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class DF_NG_8251_Web02_DeclarationStatus_ResponseService
        : ResponseServiceBase<DeclarationStatusResponseData,
        DF_NG_8251_Web02_DeclarationStatus_Response,
        DeclarationStatusRequestParams>
    {

        public override DeclarationStatusResponseData GetResponse(
            DF_NG_8251_Web02_DeclarationStatus_Response customResponse,
            DeclarationStatusRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DF_NG_8251_Web02_DeclarationStatus_Response customResponse,
            DeclarationStatusRequestParams requestParams)
        {
            
            
            


            this.MyResponseData = new DeclarationStatusResponseData();
            if (!String.IsNullOrWhiteSpace(requestParams.TesterSendOption))
            {
                TesterSendOption(requestParams);
                return;

            }
            string declarationStatusCodeName = "";
            string declarationStatusCode = "";
            string warningMess = "";
            bool isAutoPayment= false;
            DeclarationStatusTypePM declarationStatusTypePM = null;
            AmitalContext _AmitalContext = AmitalContext.GetContext(requestParams.Tenant);

            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var useSuppressNewConcurrencyGUID = true;// ConfigurationManager.AppSettings["20180130.TestFeatures"] == "1";
            if (useSuppressNewConcurrencyGUID)
            {
                declarationUpdateService.SuppressNewConcurrencyGUID = true;
            }
            
            foreach (var declarationStatus_ResponseDeclarationStatusAnswer in customResponse.DeclarationStatusAnswer)
            {
                DeclarationPM declarationPM = null;
                if (declarationStatus_ResponseDeclarationStatusAnswer.ExceptionPerQuery != null)
                {
                    //var errMess = declarationStatus_ResponseDeclarationStatusAnswer.ExceptionPerQuery;
                    var errMess = XmlGenericUtil<DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswer>.SerializeObject(declarationStatus_ResponseDeclarationStatusAnswer);
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    MyResponseData.ResponseStatusXML = errMess;
                    MyResponseData.Succeeded = true;
                    MyResponseData.HasException = true;
                    MyResponseData.UserMessage = declarationStatus_ResponseDeclarationStatusAnswer.ExceptionPerQuery;
                    return;
                }
                var declarationNumber = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationID;
                LogMessagingUtil.Instance.AppendLine("DeclarationNumber=" + declarationNumber);

                var declarationId = declarationQueryService.GetIdByDeclarationNumber(declarationNumber, requestParams.Tenant);
                if (string.IsNullOrWhiteSpace(declarationId))
                {
                    warningMess = "DeclarationNumber '" + declarationNumber + "' not found in DB.";
                    LogMessagingUtil.Instance.AppendLine(warningMess);
                    warningMess = warningMess + "\n";
                }
                else
                {
                    declarationStatusCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode.ToString();
                    if (!string.IsNullOrWhiteSpace(declarationStatusCode)) //Yuval Chalup 06.01.2014 TASK 10144 (Changed from specific status codes only to all codes)
                    {
                        declarationPM = declarationQueryService.GetSingle(declarationId, true, false);
                        if (declarationPM != null) // moran 31.12.15 - Task 19428 - change handle -->
                        {
                            if (
                                (
                                requestParams.RequestVIA == SendRequestVIA.WebServiceBatch ||
                                requestParams.RequestVIA == SendRequestVIA.DCABatch)
                                &&
                                declarationPM.ProcedureCurrentCode == "4070001" //"ProcedureCurrentCode":"4070001","ProcedureCurrentName":"יבוא מסחרי-שח\"מ"
                                && !string.IsNullOrWhiteSpace(declarationPM.CustomFileNo)
                                )
                            {
                                //using (var amitalContext = AmitalContext.GetContext(declarationPM.Tenant))
                                var amitalContext = AmitalContext.GetContext(declarationPM.Tenant);
                                //{
                                //AmitalContext.SetOracleMonitor();
                                var myCCUFILEMQueryService = new CCUFILEMQueryService(amitalContext);

                                myCCUFILEMQueryService.VirtualCCUQUELOCK_LockNOWAIT(declarationPM.Tenant, declarationPM.CustomFileNo);
                                //}
                            }
                            LogMessagingUtil.Instance.AppendLine("Declaration found (id =" + declarationPM.Id + ")");
                            Boolean paymentDateUpdated = false;
                            bool courierStatusUpdated = false;

                            if (declarationPM.PaymentDate.HasValue)
                            {
                                var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(dbContext);
                                var declarationPaymentsPM = myDeclarationPaymentQueryService.GetSingle(declarationPM.Id, true, false);
                                if (declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue)
                                {
                                    if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTimeSpecified == true)
                                    {
                                        if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.HasValue && declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.Value != declarationPaymentsPM.PaymentDate.Value)
                                        {
                                            //TimeSpan span = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.Value.Subtract(declarationPaymentsPM.PaymentDate.Value);
                                            //if (span.Minutes > 0)
                                            if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.Value.Hour != declarationPaymentsPM.PaymentDate.Value.Hour)
                                            {

                                                declarationPaymentsPM.PaymentDate = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime;
                                                DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
                                                declarationPaymentsPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                                declarationPaymentUpdateService.Update(declarationPaymentsPM, true);
                                                declarationPM.PaymentDate = declarationPaymentsPM.PaymentDate;
                                                paymentDateUpdated = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(dbContext);
                                var declarationPaymentsPM = myDeclarationPaymentQueryService.GetSingle(declarationPM.Id, true, false);
                                if (declarationPaymentsPM != null && declarationPaymentsPM.PaymentDate.HasValue && declarationPaymentsPM.PaymentDate < DateTime.Now)
                                {
                                    if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTimeSpecified == true)
                                    {
                                        declarationPM.PaymentDate = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime;
                                        declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                        declarationUpdateService.ToUpdateWithPaymentDate = true;
                                        declarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst;
                                        declarationUpdateService.Update(declarationPM, true);
                                    }
                                }
                            }

                                    string availableStatus = null;
                            if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.LogisticStatusCode == "4")
                            {
                                availableStatus = "SMG";
                            }
                            else if (new[] { "2", "5", "9" }.Contains(declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode))
                            {
                                int? cargoQuantity = 0, declarationQuantity = 0;
                                if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog != null
                               && declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities != null
                                   && declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities.Length > 0)
                                {
                                    foreach (var availabiltyItem in declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities)
                                    {
                                        if (availabiltyItem.CargoPackageQuantitySpecified == true)
                                        {
                                            cargoQuantity += availabiltyItem.CargoPackageQuantity;
                                        }
                                        if (availabiltyItem.DeclarationPackgeQuantitySpecified == true)
                                        {
                                            declarationQuantity += availabiltyItem.DeclarationPackgeQuantity;
                                        }
                                    }
                                }
                                if (declarationQuantity == cargoQuantity)
                                {
                                    availableStatus = "SMG";
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(availableStatus))
                            {
                                string site = null;
                                if (declarationPM.Consignments != null && declarationPM.Consignments.Count() > 0)
                                {
                                    site = declarationPM.Consignments.FirstOrDefault().StorageSiteCode;
                                }
                                switch (site)
                                {
                                    case "ILMMN":
                                        availableStatus = "SMK";
                                        break;
                                    case " ILSWS":
                                        availableStatus = "SMP";
                                        break;
                                    case "ILTNL":
                                        availableStatus = "SMA";
                                        break;
                                    default:
                                        availableStatus = "SMG";
                                        break;
                                }

                              //  if (availableStatus == "SMG" && declarationPM.TransportModeId == "O") availableStatus = "SST";

                                LogMessagingUtil.Instance.AppendLine("HAWB Received");

                                if (!string.IsNullOrWhiteSpace(availableStatus) && declarationPM.AvailabilityDate==null)
                                {
                                    if (availableStatus == "SMG" && declarationPM.TransportModeId == "A")
                                    {
                                        RaiseStatus(declarationPM, "", availableStatus);

                                    }
                                    else if (availableStatus == "SMG" && declarationPM.TransportModeId == "O")
                                    {
                                        RaiseStatus(declarationPM, "", "SST");

                                    }


                                    if (availableStatus != "SMG" && declarationPM.TransportModeId=="A")
                                    {
                                        RaiseStatus(declarationPM, "", "SMG");
                                        //declarationPM.AvailabilityDate = DateTime.Now;
                                        isAutoPayment = true;
                                    }

                                    else if ((availableStatus != "SST" && availableStatus != "SMG") &&  declarationPM.TransportModeId == "O")
                                    {
                                        RaiseStatus(declarationPM, "", "SST");
                                       // declarationPM.AvailabilityDate = DateTime.Now;
                                        isAutoPayment = true;
                                    }

                                  else   if(availableStatus=="SMG" || availableStatus == "SST")
                                    {
                                       // declarationPM.AvailabilityDate = DateTime.Now;
                                        isAutoPayment = true;
                                    }
                                }


                                //if(availableStatus == "SMG")
                                //{
                                 
                                //}

                            }

                            if (declarationPM.IsCourierDeclaration)
                            {
                                //declarationPM.CourierSuspentionReasonCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;
                                //declarationPM.CourierSuspentionCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;//Eitan H 31/12/18//Task 49319
                                switch (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode)
                                {
                                    case "3":
                                        declarationPM.CourierCustomStatusCode = "1";
                                        courierStatusUpdated = true;

                                        LogMessagingUtil.Instance.AppendLine("Pre Clearance");
                                        var myEventContextTagModelPRS = new EventContextTagModel()
                                        {
                                            CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_8251_Web02_DeclarationStatusResponseServicePreClearance,
                                        };                                       
                                        myEventContextTagModelPRS.EventCode = "PRS";
                                        declarationPM.CurrentContextTag = myEventContextTagModelPRS;
                                        RaiseStatus(declarationPM, "", myEventContextTagModelPRS.EventCode);
                                        break;
                                    case "25":
                                    case "30":
                                    case "31":
                                    case "32":
                                    case "33":
                                    case "34":
                                    case "35":
                                        declarationPM.CourierCustomStatusCode = "2";
                                        courierStatusUpdated = true;
                                        declarationPM.CourierSuspentionCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;//Eitan H 4/3/2019 Task 49319

                                        var myEventContextTagModel = new EventContextTagModel()
                                        {
                                            CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_8251_Web02_DeclarationStatusResponseServicePreClearance,
                                        };
                                        switch (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode)
                                        {
                                            case "25":
                                                LogMessagingUtil.Instance.AppendLine("Event VCI");
                                                myEventContextTagModel.EventCode = "VCI";
                                                break;
                                            case "30":
                                                LogMessagingUtil.Instance.AppendLine("Event VCS");
                                                myEventContextTagModel.EventCode = "VCS";
                                                break;
                                            case "31":
                                                LogMessagingUtil.Instance.AppendLine("Event VCD");
                                                myEventContextTagModel.EventCode = "VCD";
                                                break;
                                            case "32":
                                                LogMessagingUtil.Instance.AppendLine("Event VCE");
                                                myEventContextTagModel.EventCode = "VCE";
                                                break;
                                            case "33":
                                                LogMessagingUtil.Instance.AppendLine("Event VCA");
                                                myEventContextTagModel.EventCode = "VCA";
                                                break;
                                            case "34":
                                                LogMessagingUtil.Instance.AppendLine("Event VCG");
                                                myEventContextTagModel.EventCode = "VCG";
                                                break;
                                            case "35":
                                                LogMessagingUtil.Instance.AppendLine("Event VCT");
                                                myEventContextTagModel.EventCode = "VCT";
                                                break;
                                        }
                                        declarationPM.CurrentContextTag = myEventContextTagModel;
                                        RaiseStatus(declarationPM, "", myEventContextTagModel.EventCode);
                                        break;
                                    case "13":
                                        if (declarationPM.PaymentDate.HasValue)
                                        {
                                            declarationPM.DeclarationStatusTypeCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;
                                            declarationPM.PaymentDate = null;
                                            declarationPM.PaymentOrderNumber = null;
                                            declarationPM.PaymentStatusCode = null;
                                            declarationPM.CourierCustomStatusCode = null;
                                            declarationPM.CourierSuspentionCode = null;
                                            declarationPM.CourierSuspentionReasonCode = null;

                                            //Delete 
                                            var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(dbContext);
                                            var declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(declarationPM.Id, true, false);
                                            if (declarationPaymentPM != null)
                                            {
                                                declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Delete;
                                                if (declarationPaymentPM.DeclarationPaymentMethods.Any())
                                                {
                                                    foreach (var item in declarationPaymentPM.DeclarationPaymentMethods)
                                                    {
                                                        item.ChangeSetOp = ChangeSetOperation.Delete;
                                                    }
                                                }
                                                DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
                                                declarationPaymentUpdateService.Update(declarationPaymentPM, true);
                                            }
                                            courierStatusUpdated = true;
                                        }
                                        break;


                                }
                            }

                            if (requestParams.RequestOrigin == "DeclarationStatusRequestViewModel") // moran 20.1.16 - Task 19428 add RequestOrigin = "DeclarationStatusRequestViewModel" check
                            {
                                if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode == "1")
                                {
                                    declarationPM.DeclarationStatusTypeCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;
                                    var myEventContextTagModel = new EventContextTagModel()
                                    {
                                        CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_8251_Web02_DeclarationStatusResponseServiceCancel,
                                    };

                                    LogMessagingUtil.Instance.AppendLine("Canceled");
                                    myEventContextTagModel.EventCode = "DCN";
                                    declarationPM.CurrentContextTag = myEventContextTagModel;
                                    declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationUpdateService.Update(declarationPM, true);
                                    //if (isAutoPayment)
                                    //    SendPayment(declarationPM, dbContext, requestParams);
                                }
                                else if ((declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationVersion == declarationPM.VersionId && declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime.HasValue))
                                {
                                    declarationPM.DeclarationStatusTypeCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;

                                    if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime.HasValue && declarationPM.HatraDate == null)
                                    {
                                        var myEventContextTagModel = new EventContextTagModel()
                                        {
                                            CallProccessID = EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate,
                                        };

                                        // released
                                        LogMessagingUtil.Instance.AppendLine("released");
                                        myEventContextTagModel.EventCode = "RSG";
                                        myEventContextTagModel.StatusDateTime = (DateTime)declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime;
                                        declarationPM.HatraDate = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime;
                                        LogMessagingUtil.Instance.AppendLine("declarationPM.HatraDate" + (declarationPM.HatraDate.HasValue ? declarationPM.HatraDate.Value.ToString() : ""));
                                        declarationPM.CurrentContextTag = myEventContextTagModel;
                                    }
                                    declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationUpdateService.Update(declarationPM, true);
                                    //if (isAutoPayment)
                                    //    SendPayment(declarationPM, dbContext, requestParams);
                                }
                                else
                                {
                                    MyResponseData.WarningMessage = warningMess = "סטטוס ההצהרה לא עודכן , יש לבצע בקשה לשחזור נתוני הצהרה " + " (" + declarationNumber + ")";
                                }
                            }
                            else // moran 18.12.16 - Bug 25294
                            {
                                if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationVersion == declarationPM.VersionId)
                                {
                                    declarationPM.DeclarationStatusTypeCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;
                                    declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                    declarationUpdateService.Update(declarationPM, true);
                                    //if (isAutoPayment)
                                    //    SendPayment(declarationPM, dbContext, requestParams);
                                }
                                else
                                {
                                    warningMess = "Status was not updated, Version in the message(" + declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationVersion + ") is different than the version(" + declarationPM.VersionId + ") in the declaration " + declarationNumber + " with ID " + declarationId;
                                    LogMessagingUtil.Instance.AppendLine(warningMess);
                                    warningMess = warningMess + "\n";
                                    MyResponseData.WarningMessage = warningMess = "סטטוס ההצהרה לא עודכן , יש לבצע בקשה לשחזור נתוני הצהרה " + " (" + declarationNumber + ")";
                                }
                            }
                            if ((paymentDateUpdated || courierStatusUpdated) && declarationPM.ChangeSetOp != ChangeSetOperation.Update)
                            {
                                declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                declarationUpdateService.Update(declarationPM, true);
                                //if (isAutoPayment)
                                //    SendPayment(declarationPM, dbContext, requestParams);
                            }
                        }
                        else
                        {
                            warningMess = "Couldn't retrieve Declaration Number " + declarationNumber + " with ID " + declarationId;
                            LogMessagingUtil.Instance.AppendLine(warningMess);
                            warningMess = warningMess + "\n";
                        } // moran 31.12.15 - Task 19428 - change handle <--
                    }
                }

                if (isAutoPayment)
                    SendPayment(declarationPM, dbContext, requestParams);

                try
                {
                    declarationStatusCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode.ToString();
                    //if (!String.IsNullOrWhiteSpace(declarationStatusCode) && (declarationStatusCode == "1" || declarationStatusCode == "8" || declarationStatusCode == "17" || declarationStatusCode == "18"))
                    if (!string.IsNullOrWhiteSpace(declarationStatusCode)) //Yuval Chalup 06.01.2014 TASK 10144 (Changed from specific status codes only to all codes)
                    {
                        var QueryService = new DeclarationStatusTypeQueryService(requestParams.Tenant);
                        declarationStatusTypePM = QueryService.GetSingle(declarationStatusCode, true, false);
                        if (declarationStatusTypePM == null)
                        {
                            declarationStatusCodeName = declarationStatusCode + " (Status does not exist)";
                        }
                        else
                        {
                            declarationStatusCodeName = declarationStatusTypePM.LocalName;
                            if (declarationStatusCodeName == null)
                            {
                                declarationStatusCodeName = declarationStatusTypePM.EnglishName;
                            }
                            if (declarationStatusCodeName == null)
                            {
                                declarationStatusCodeName = declarationStatusCode + " (No status description)";
                            }
                        }
                    }


                    //var errMess = warningMess + "Declaration - " + declarationNumber + ":\n" + "Succeeded - Status is " +   declarationStatusCodeName;
                    var errMess = warningMess + "Declaration - " + declarationNumber + ":\n" + "Succeeded - Status is " +
                        declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode + " - " +
                        declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusText;

                    var xml = XmlGenericUtil<DF_NG_8251_Web02_DeclarationStatus_ResponseDeclarationStatusAnswerDeclarationStatusDetails>.SerializeObject(declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails);
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    MyResponseData.ResponseStatusXML = xml;
                    MyResponseData.DeclarationStatusColor = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusColorName;
                    MyResponseData.DeclarationID = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationID;
                    MyResponseData.DeclarationVersion = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationVersion;
                    MyResponseData.DeclarationStatusCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode;
                    MyResponseData.DeclarationStatusText = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusText;
                    MyResponseData.LogisticStatusCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.LogisticStatusCode;
                    MyResponseData.LogisticStatusText = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.LogisticStatusText;
                    MyResponseData.DeclarationOfficeID = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationOfficeID;
                    MyResponseData.DeclarationOfficeText = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationOfficeText;
                    MyResponseData.FinancialStatusCode = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.FinancialStatusCode;
                    MyResponseData.FinancialStatusText = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.FinancialStatusText;
                    MyResponseData.TaxationDateTime = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.TaxationDateTime.Date.ToString("dd/MM/yyyy");
                    if(declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.TaxationDateTime.TimeOfDay.Hours != 0)
                    {
                        MyResponseData.TaxationDateTime = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.TaxationDateTime.TimeOfDay.ToString("hh:mm") + "   " + MyResponseData.TaxationDateTime;
                    }

                    MyResponseData.HandeledWroker = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.HandledWorker;

                    if(declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime.HasValue)
                    {
                        MyResponseData.ReleaseDateTime = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime.Value.Date.ToString("dd/MM/yyyy");
                        if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime.Value.TimeOfDay.Hours != 0)
                        {
                            MyResponseData.ReleaseDateTime = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.ReleaseDateTime.Value.TimeOfDay.ToString("hh':'mm") + "   " + MyResponseData.ReleaseDateTime; 
                        }
                    }
                    if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTimeSpecified == true)
                    {
                        MyResponseData.SubmitDateTime = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.Value.Date.ToString("dd/MM/yyyy");
                        if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.Value.TimeOfDay.Hours != 0)
                        {
                            MyResponseData.SubmitDateTime = declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.SubmitDateTime.Value.TimeOfDay.ToString("hh':'mm") + "   " + MyResponseData.SubmitDateTime;
                        }
                    }

                    if (declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog != null
                            && declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities != null
                                && declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities.Length > 0)
                    {
                        MyResponseData.AvailabiltyQuantitiesList = new List<AvailabiltyLogDeclarationCargoQuantities>();
                        foreach (var availabiltyItem in declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationAvailabilityLog.AvailabiltyLogDeclarationCargoQuantities)
                        {
                            AvailabiltyLogDeclarationCargoQuantities availabiltyLogDeclarationCargoQuantities = new AvailabiltyLogDeclarationCargoQuantities();
                            availabiltyLogDeclarationCargoQuantities.CargoIdentifierTypeCode = availabiltyItem.CargoIdentifierTypeCode.ToString();
                            availabiltyLogDeclarationCargoQuantities.CargoIdentifierTypeText = availabiltyItem.CargoIdentifierTypeText;
                            availabiltyLogDeclarationCargoQuantities.CargoIdentifierKey1 = availabiltyItem.CargoIdentifierKey1;
                            availabiltyLogDeclarationCargoQuantities.CargoIdentifierKey2 = availabiltyItem.CargoIdentifierKey2;
                            availabiltyLogDeclarationCargoQuantities.CargoIdentifierKey3 = availabiltyItem.CargoIdentifierKey3;
                            if (availabiltyItem.IsSecondRoundSpecified == true)
                            {
                                availabiltyLogDeclarationCargoQuantities.IsSecondRound = (bool)availabiltyItem.IsSecondRound;
                            }
                            availabiltyLogDeclarationCargoQuantities.CargoPackageTypeCode = availabiltyItem.CargoPackageTypeCode.ToString();
                            availabiltyLogDeclarationCargoQuantities.CargoPackageTypeText = availabiltyItem.CargoPackageTypeText;
                            if (availabiltyItem.CargoPackageQuantitySpecified == true)
                            {
                                availabiltyLogDeclarationCargoQuantities.CargoPackageQuantity = availabiltyItem.CargoPackageQuantity.ToString();
                            }
                            if (availabiltyItem.CargoPackageWeightSpecified == true)
                            {
                                availabiltyLogDeclarationCargoQuantities.CargoPackageWeight = availabiltyItem.CargoPackageWeight.ToString();
                            }
                            availabiltyLogDeclarationCargoQuantities.CargoWeightMeasurementUnitCode = availabiltyItem.CargoWeightMeasurementUnitCode.ToString();
                            availabiltyLogDeclarationCargoQuantities.CargoWeightMeasurementUnitText = availabiltyItem.CargoWeightMeasurementUnitText;
                            availabiltyLogDeclarationCargoQuantities.DeclarationPackageTypeCode = availabiltyItem.DeclarationPackageTypeCode.ToString();
                            availabiltyLogDeclarationCargoQuantities.DeclarationPackageTypeText = availabiltyItem.DeclarationPackageTypeText;
                            if (availabiltyItem.DeclarationPackgeQuantitySpecified == true)
                            {
                                availabiltyLogDeclarationCargoQuantities.DeclarationPackgeQuantity = availabiltyItem.DeclarationPackgeQuantity.ToString();
                            }
                            if (availabiltyItem.DeclarationPackageWeightSpecified == true)
                            {
                                availabiltyLogDeclarationCargoQuantities.DeclarationPackageWeight = availabiltyItem.DeclarationPackageWeight.ToString();
                            }
                            availabiltyLogDeclarationCargoQuantities.DeclarationWeightMeasurementUnitCode = availabiltyItem.DeclarationWeightMeasurementUnitCode.ToString();
                            availabiltyLogDeclarationCargoQuantities.DeclarationWeightMeasurementUnitText = availabiltyItem.DeclarationWeightMeasurementUnitText;
                            availabiltyLogDeclarationCargoQuantities.ComparisonResult = availabiltyItem.ComparisonResult;
                            MyResponseData.AvailabiltyQuantitiesList.Add(availabiltyLogDeclarationCargoQuantities);
                        }
                    }

                    LogMessagingUtil.Instance.AppendLine("declarationUpdateService.UpdateD");
                    MyResponseData.Succeeded = true;

                    // moran 13.1.15 - Task 10089 -->
                    if (!string.IsNullOrWhiteSpace(declarationId))
                    {
                        if (this.MyRequestSheetParam == null)
                        {
                            this.MyRequestSheetParam = new RequestSheetParam();
                        }
                        this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                        this.MyRequestSheetParam.EntityId1 = declarationId;
                    }
                    // moran 13.1.15 - Task 10089 <--
                }
                catch
                {
                    var errMess = warningMess + "Declaration - " + declarationNumber + ":\n" + "Failed update - Status is " + declarationStatus_ResponseDeclarationStatusAnswer.DeclarationStatusDetails.DeclarationStatusCode.ToString();
                    LogMessagingUtil.Instance.AppendLine(errMess);
                    MyResponseData.ResponseStatusXML = errMess;
                    MyResponseData.HasException = true;
                }
            }
        }

        private void SendPayment(DeclarationPM declarationPM,ICustomContext dbContext, DeclarationStatusRequestParams requestParams)
        {
            if (declarationPM.AvailabilityDate != null) return;
            var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(dbContext);
            var myDeclarationPaymentUpdateService = new DeclarationPaymentUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant); ;

            var myDeclarationQueryService = new DeclarationQueryService(dbContext);
            var myDeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
              var _declarationPM = myDeclarationQueryService.GetSingle(declarationPM.Id, true, false);
            _declarationPM.AvailabilityDate = DateTime.Now;
            _declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.Update(_declarationPM, true);

            var declarationPaymentPM = myDeclarationPaymentQueryService.GetSingle(_declarationPM.Id, true, false);
            

            if (declarationPaymentPM != null)
            {
                if (declarationPaymentPM.AutomaticPayment == 1)
                {
                    if (!CheckFileCredit(declarationPM, declarationPaymentPM, requestParams.LoggingUserId))
                    {

                        var MyUnifreightEventParam = new UnifreightEventParam()
                        {
                            Code = "APAYF",
                            Mode = UnifreightEventMode.@new,
                            EventDateTime = DateTime.Now,
                            Entname = "CFIFILEM",
                            PrimaryNum = declarationPM.CustomFileNo,
                            EventRemarks = "לא אושר בבקרת אשראי",
                        };
                        LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                        var myOpenUnifreighTask = new UnifreightEventTaskService();
                        myOpenUnifreighTask.UpsertEventLE2U(
                            declarationPM.Tenant,
                           requestParams.LoggingUserId,
                            MyUnifreightEventParam);
                    }
                    else
                    {
                        try
                        {
                            DateTime requestDate = CheckIfBlockTime(declarationPM, declarationPaymentPM);

                            using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                            {
                                declarationPaymentPM.PaymentDate = DateTime.Now;
                                declarationPaymentPM.ChangeSetOp = ChangeSetOperation.Update;

                                var requestParams2755 = new GenericRequestParams()
                                {
                                    Tenant = requestParams.Tenant,
                                    LoggingEnabled = true,
                                    LoggingObjectTableId = requestParams.LoggingObjectTableId,
                                    LoggingEntityId = declarationPM.Id,
                                    AppicationId = declarationPM.Id,
                                    InterfaceTypeCode = "2755",
                                    LoggingUserId = requestParams.LoggingUserId,
                                    RequestVIA = SendRequestVIA.WebServiceBatch,
                                  
                                };
                                if (requestDate != DateTime.MinValue)
                                {
                                    requestDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, requestDate.Hour, requestDate.Minute, requestDate.Second);
                                    declarationPaymentPM.PaymentDate = requestDate;

                                    requestParams2755.RequestVIAChangeDue = string.Concat("נרשמה בקשה מתוזמנת לתאריך ", requestDate.ToShortDateString(), " שעה ", requestDate.ToShortTimeString());// "הבקשה תשלח בעתיד";
                                    requestParams2755.FutureSendDateTime = requestDate;
 
                                    SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755, false, requestDate);

                                }
                                else
                                {
                                     SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams2755, false);
                                }



                                scopeNewCRS.Complete();
                            }

                        }
                        catch (System.Exception)
                        {
                            var MyUnifreightEventParam = new UnifreightEventParam()
                            {
                                Code = "APAYF",
                                Mode = UnifreightEventMode.@new,
                                EventDateTime = DateTime.Now,
                                Entname = "CFIFILEM",
                                PrimaryNum = declarationPM.CustomFileNo,
                                EventRemarks = "כשלון בשליחת הגשת תשלום",
                            };
                            LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                            var myOpenUnifreighTask = new UnifreightEventTaskService();
                            myOpenUnifreighTask.UpsertEventLE2U(
                                declarationPM.Tenant,
                               requestParams.LoggingUserId,
                                MyUnifreightEventParam);

                            throw;
                        }
                    }
                }
                myDeclarationPaymentUpdateService.Update(declarationPaymentPM, true);

            }


        }

        private DateTime CheckIfBlockTime(DeclarationPM declarationPM, DeclarationPaymentPM declarationPaymentPM)
        {
            var declarationQS = new DeclarationQueryService(declarationPaymentPM.Tenant);
            string timesCompany = declarationQS.GetDefault("ISRAEL", "CGG_PAY_BLK_RNG", "NON", "NON", declarationPaymentPM.Tenant);
            TimeSpan toTimeCurrent = new TimeSpan();
            TimeSpan toTime2Current = new TimeSpan();
            TimeSpan toTime = new TimeSpan();
            if (timesCompany != null && timesCompany != "")
            {
                List<string> times= GetTimesFromDefault(timesCompany);

                TimeSpan fromTime = DateTime.ParseExact(times[0], "HH:mm",
                                        CultureInfo.InvariantCulture).TimeOfDay;


                  toTime = DateTime.ParseExact(times[1], "HH:mm",
                                    CultureInfo.InvariantCulture).TimeOfDay;

                if (DateTime.Now.TimeOfDay > fromTime && DateTime.Now.TimeOfDay < toTime)
                {
                    toTimeCurrent = toTime;
                //    return new DateTime(toTime.Ticks).AddMinutes(5);
                }

            }
            string timesCustomer = declarationQS.GetDefault("ISRAEL", "CIM_PAY_BLK_RNG", "NON", declarationPM.CustomerCode, declarationPaymentPM.Tenant);

            if (timesCustomer != null && timesCustomer != "")
            {
                List<string> times = GetTimesFromDefault(timesCustomer);

                TimeSpan fromTime = DateTime.ParseExact(times[0], "HH:mm",
                                        CultureInfo.InvariantCulture).TimeOfDay;


                  toTime = DateTime.ParseExact(times[1], "HH:mm",
                                    CultureInfo.InvariantCulture).TimeOfDay;

                if (DateTime.Now.TimeOfDay > fromTime && DateTime.Now.TimeOfDay < toTime)
                {
                    toTime2Current = toTime;
                   // return new DateTime(toTime.Ticks).AddMinutes(5);
                }

            }

            if(toTimeCurrent> toTime2Current)
            {
                return new DateTime(toTimeCurrent.Ticks).AddMinutes(5);
            }
            else if(toTime2Current > toTimeCurrent)
            {
                return new DateTime(toTime2Current.Ticks).AddMinutes(5);

            }
            //else if(toTime!= new TimeSpan())
            //{
            //    return new DateTime(toTime.Ticks).AddMinutes(5);

            //}

            return  DateTime.MinValue;
 
        }

        private List<string> GetTimesFromDefault(string times)
        {
          var arr = times.Split('-');
            return new List<string>()
            {
                 arr[0].TrimEnd() ,  arr[1].TrimStart()
            };
        }

        private bool CheckFileCredit(DeclarationPM declarationPM, DeclarationPaymentPM declarationPaymentPM, string user)
        {
            CustomFileCreditRequestParams requestParamsCredit = new CustomFileCreditRequestParams()
            {
                Tenant = declarationPM.Tenant,
                AppicationId = declarationPaymentPM.DeclarationId,
                LoggingEnabled = true,
                LoggingEntityId = declarationPM.Id,
                InterfaceTypeCode = "2755",
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingEntityReference = declarationPM.DeclarationNumber,
                LoggingUserId = user,
                RequestName = "Send to check credit request",
                ResponseName = "Get check credit Response",
                Mode = "Check",
                RequestVIA = SendRequestVIA.WebServiceBatch,
            };
            var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
            CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
             if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
            {
                return false;
            }

            return true;
        }

        private static void TesterSendOption(DeclarationStatusRequestParams requestParams)
        {
            string testerSendOption = requestParams.TesterSendOption??"";
            testerSendOption = testerSendOption.ToUpper();
            var context = CustomContext.GetContext(requestParams.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);


            LogMessagingUtil.Instance.AppendLine(testerSendOption);
            switch (testerSendOption)
            {
                case "NOTHING"://this._SendOptionList.push(new KeyValuePair
                    {
                        return;
                    }
                    break;
                case "READ"://this._SendOptionList.push(new KeyValuePair("Read".toUpperCase(), "Read"));
                    {

                        var declarationId = declarationQueryService.GetIdByDeclarationNumber(requestParams.DeclarationNumber, requestParams.Tenant);
                        LogMessagingUtil.Instance.AppendLine("declarationQueryService.GetIdByDeclarationNumber");
                        return;
                    }
                    break;
                case "UPDATE"://this._SendOptionList.push(new KeyValuePair("Update".toUpperCase(), "Update"));
                    {
                        var declarationId = declarationQueryService.GetIdByDeclarationNumber(requestParams.DeclarationNumber, requestParams.Tenant);
                        LogMessagingUtil.Instance.AppendLine("declarationQueryService.GetIdByDeclarationNumber");
                        var pm=declarationQueryService.GetSingle(declarationId, false, false);
                        LogMessagingUtil.Instance.AppendLine("declarationQueryService..GetSingle(declarationId, false, false);");
                        pm.ChangeSetOp = ChangeSetOperation.Update;
                        pm.UpdateDateTime = DateTime.UtcNow;
                        declarationUpdateService.Update(pm, true);
                        LogMessagingUtil.Instance.AppendLine("declarationUpdateService.Update(pm, true);");
                    }
                    break;

                default:
                    break;
            }

        }

        public static void RaiseStatus(DeclarationPM dirtyDeclarationPM, string loggingUserId, string statusId)
        {
            
            try 
            {
                if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = statusId,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + statusId + " from Logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = statusId,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };
                if (!dirtyDeclarationPM.IsConnectedToUnifreight) myAmitalEventTracerModel.NotConnectedToUniface = true;

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent Status " + statusId + "  CustomFileNo = " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
    }
}
