using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;
using Logitude.Server.Tools.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TSH_MSG7_AgentPaymentReplyResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, TSH_MSG7_AgentPaymentReply, GenericRequestParams>
    {

        private PaymentOrderPM _PaymentOrderPM;

        public bool _UNIQUEFILINGPOFeatureExist { get; private set; }

        public override INF_MSG_GenericResponseData GetResponse(TSH_MSG7_AgentPaymentReply customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(TSH_MSG7_AgentPaymentReply customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 3052- Answer to the agent request for changing existing payment


            //אם ה Feature מוגדר, אז יש לתייק את המסמך שהגיע כחלק מהמסר, כאשר לפני כן יש לנסות לאתר אם כבר קיים מסמך כזה ואז רק ליצור גרסה חדשה.
            _UNIQUEFILINGPOFeatureExist = ProxyUtil.SecurityUtilityCheckFeature("Customs.PaymentOrder", "UNIQUEFILINGPO", requestParams.Tenant);///

            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var paymentOrderQueryService = new PaymentOrderQueryService(dbContext);
            var PaymentOrderUpdateService = new PaymentOrderUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            string declarationId = null;

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    Succeeded = true,
                    ApplicationID = requestParams.AppicationId,
                    HasException = true,
                    UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription,
                };
                if (customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionType == 6599)
                {
                    this.MyResponseData.UserMessage = string.Concat(this.MyResponseData.UserMessage, "\n", "על מנת לשלם את ההוראה העדכנית יש לבצע לפני כן שחזור הוראת תשלום");
                }
                LogMessagingUtil.Instance.AppendLine("Exception for payment " + requestParams.AppicationId + ". " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                return;
            }

            string userMessage = "תשלום הוראה " + customResponse.AgentPaymentReply.paymentID;
            if (this.MyRequestSheetParam == null || string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = " משוב לתשלום הוראת תשלום" + customResponse.AgentPaymentReply.paymentID;
            }

            if (string.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                requestParams.AppicationId = paymentOrderQueryService.GetIdByPaymentNumber(customResponse.AgentPaymentReply.paymentID.ToString(), requestParams.Tenant);
            }

            _PaymentOrderPM = paymentOrderQueryService.GetSingle(requestParams.AppicationId, true, true);
            if (_PaymentOrderPM == null)
            {
                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    Succeeded = false,
                    ApplicationID = requestParams.AppicationId,
                    HasException = true,
                    UserMessage = "Cann't find Payment Order" + requestParams.AppicationId,
                };
                LogMessagingUtil.Instance.AppendLine("Cann't find Payment Order " + customResponse.AgentPaymentReply.paymentID);
                return; //if not found exit  
            }

            bool isAddConnectionToAccountingFile = false;
            if (!string.IsNullOrWhiteSpace(_PaymentOrderPM.AccountingCustomFile))
            {
                isAddConnectionToAccountingFile = true;
                declarationId = declarationQueryService.GetIdByCustomFileNo(_PaymentOrderPM.AccountingCustomFile, _PaymentOrderPM.Tenant);
                if (_PaymentOrderPM.PaymentOrderConnectionTables != null && !string.IsNullOrWhiteSpace(declarationId))
                {
                    foreach (var connectedItem in _PaymentOrderPM.PaymentOrderConnectionTables)
                    {
                        if (connectedItem.ConnectedEntityCode == "D" && connectedItem.ConnectedEntityId == declarationId)
                        {
                            isAddConnectionToAccountingFile = false;
                            break;
                        }
                    }
                }
            }
            else if (_PaymentOrderPM.PaymentOrderConnectionTables != null)
            {
                foreach (var connectedItem in _PaymentOrderPM.PaymentOrderConnectionTables)
                {
                    if (connectedItem.ConnectedEntityCode == "D")
                    {
                        declarationId = connectedItem.ConnectedEntityId;
                        //_PaymentOrderPM.CustomsRequestsDeclarationId = connectedItem.ConnectedEntityId;
                        break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(declarationId) && isAddConnectionToAccountingFile == true)
            {
                if (_PaymentOrderPM.PaymentOrderConnectionTables == null)
                {
                    _PaymentOrderPM.PaymentOrderConnectionTables = new List<PaymentOrderConnectionTablePM>();
                }
                PaymentOrderConnectionTablePM paymentOrderConnectionTablePM = new PaymentOrderConnectionTablePM();
                paymentOrderConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                paymentOrderConnectionTablePM.Tenant = _PaymentOrderPM.Tenant;
                paymentOrderConnectionTablePM.PaymentOrderId = _PaymentOrderPM.Id;
                paymentOrderConnectionTablePM.ConnectedEntityCode = "D";
                paymentOrderConnectionTablePM.ConnectedEntityId = declarationId;
                _PaymentOrderPM.PaymentOrderConnectionTables.Add(paymentOrderConnectionTablePM);
            }

            if (!_PaymentOrderPM.IsClosed && customResponse.AgentPaymentReply.status == 3)
            {
                LogMessagingUtil.Instance.AppendLine("Start compring between the records in DB and the records in message");
                _PaymentOrderPM.ChangeSetOp = ChangeSetOperation.Update;
                var myInsertEventContextTagModel = new EventContextTagModel() // Indication to Create Unifreight Status "POP"
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.TSH_MSG7_AgentPaymentReplyResponseService,
                    EventCode = "POP",
                    EventRemarks = "Payment Order Paid",
                    FUStatusRemarks = "הוראת תשלום " + _PaymentOrderPM.PaymentNumber + " שולמה "
                                 + "\n" + "סטטוס הוראה:" + _PaymentOrderPM.PaymentStatusName
                                 + "\n" + "תאריך אחרון לתשלום:" + _PaymentOrderPM.LastPayDate
                                 + "\n" + "סכום לתשלום:" + _PaymentOrderPM.TotalSumToPay
                                 + "\n" + "התהליך היוצר:" + _PaymentOrderPM.PaymentProcessName,
                };
                _PaymentOrderPM.CurrentContextTag = myInsertEventContextTagModel;

                _PaymentOrderPM.IsClosed = true;
                _PaymentOrderPM.ActualPayDate = customResponse.ResponseContentHeader.TransmitionDateTime;
                _PaymentOrderPM.PaymentStatusCode = customResponse.AgentPaymentReply.status.ToString();

                var paymentOrderMethodJoin =
                (from pm in _PaymentOrderPM.PaymentOrderMethods
                 join cusMethod in customResponse.AgentPaymentMethods.ToList()

                 on new
                 {
                     pm.TypeCode,
                     pm.Amount,
                 }
                 equals
                 new
                 {
                     TypeCode = cusMethod.type.ToString(),
                     Amount = (decimal?)cusMethod.amount,
                 }

                 into outerJoin
                 from cusMethod in outerJoin.DefaultIfEmpty()
                 select new { pm, cusMethod }
                );

                bool isDoUpdate = true;
                if (paymentOrderMethodJoin.FirstOrDefault() == null || paymentOrderMethodJoin.FirstOrDefault().cusMethod == null || paymentOrderMethodJoin.Count() > _PaymentOrderPM.PaymentOrderMethods.Count())
                {
                    if (paymentOrderMethodJoin.FirstOrDefault() == null || paymentOrderMethodJoin.FirstOrDefault().cusMethod == null)
                    {
                        isDoUpdate = false;
                        LogMessagingUtil.Instance.AppendLine("No record found!!! Cann't compre between DB to message");
                    }
                    else
                    {
                        foreach (var paymentOrderMethodItem in paymentOrderMethodJoin)
                        {
                            foreach (var checkItem in paymentOrderMethodJoin)
                            {
                                if (paymentOrderMethodItem.cusMethod.type == checkItem.cusMethod.type && paymentOrderMethodItem.cusMethod.amount == checkItem.cusMethod.amount && paymentOrderMethodItem.cusMethod.paymentMethodStatus != checkItem.cusMethod.paymentMethodStatus)
                                {
                                    isDoUpdate = false;
                                    LogMessagingUtil.Instance.AppendLine("Cann't compre between DB to message- there are similar records with different status code");
                                    break;
                                }
                            }
                        }
                    }
                }
                if (isDoUpdate)
                {
                    foreach (var method in paymentOrderMethodJoin)
                    {
                        if (method != null && method.cusMethod != null)
                        {
                            method.pm.ChangeSetOp = ChangeSetOperation.Update;
                            method.pm.PaymentMethodStatusCode = method.cusMethod.paymentMethodStatus.ToString();
                            method.pm.Amount = method.cusMethod.amount;
                            //method.pm.TypeCode = method.cusMethod.type.ToString();
                        }
                    }
                    userMessage = "תשלום הוראה " + _PaymentOrderPM.PaymentNumber + " בוצע בהצלחה";
                }
                else
                {
                    userMessage = "תשלום הוראה " + _PaymentOrderPM.PaymentNumber + "\n" + "לא ניתן לאתר שורות תשלום במסר";
                }
                PaymentOrderUpdateService.Update(_PaymentOrderPM, true);
            }
            bool task44020 = true;
            if (!task44020)
            {
                if (!string.IsNullOrWhiteSpace(declarationId))
                {
                    UpdatePaymentDocument(declarationId, requestParams.Tenant, requestParams.LoggingUserId);
                }
            }
            else
            {
                DeclarationPM myDeclarationPM = null;
                var myDeclarationQueryService = new DeclarationQueryService(requestParams.Tenant);
                if (!string.IsNullOrWhiteSpace(declarationId))
                {
                    myDeclarationPM = myDeclarationQueryService.GetSingle(declarationId, false, false);
                }

                var myAnalyzePaymentDocumentManager = new AnalyzePaymentDocumentManager(null, _PaymentOrderPM, myDeclarationPM);
                

                bool AttachmentExistInGDMFILING = true;
                if (_UNIQUEFILINGPOFeatureExist)
                {
                    myAnalyzePaymentDocumentManager.AnalyzePaymentDocument(
                        customResponse.PrintedPaymentForm.PrintedPaymentForm.content
                        , requestParams);
                }
                else
                {
                    
                    DocumentsFilingPM documentsFilingPM = myAnalyzePaymentDocumentManager.GetDocumentsFiling(requestParams);
                    if (documentsFilingPM==null)
                    {
                        // eitan : if get first time Attach add contents
                        myAnalyzePaymentDocumentManager.CreatePaymentDocument(
                            customResponse.PrintedPaymentForm.PrintedPaymentForm.content
                            , requestParams);
                    }
                    else
                    {
                        //CreateUnfreigtFiling()
                        // eitan : if already have filing - update metadata only !!
                        myAnalyzePaymentDocumentManager.UpdatePaymentDocument(documentsFilingPM, 
                            /*content =*/ null,//do not send data only if _UNIQUEFILINGPOFeatureExist !!
                            requestParams);
                    }
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                Succeeded = true,
                ApplicationID = _PaymentOrderPM.Id,
                HasException = false,
                UserMessage = userMessage,
            };

            this.MyRequestSheetParam.EntityId1 = _PaymentOrderPM.Id;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
            this.MyRequestSheetParam.EntityReference = _PaymentOrderPM.PaymentNumber;
            if (!string.IsNullOrWhiteSpace(declarationId))
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = declarationId;
            }
        }

        private void UpdatePaymentDocument(string DeclarationId, int Tenant, string LoggedUserId)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, Tenant);
            var documentTypeQuery = new DocumentTypeQuery(Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(Tenant);

            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("POR", _PaymentOrderPM.Tenant);
            DocumentsFilingPM documentsFilingPM = documentsFilingQuery.GetDocumentsFilingByChild(documentType.Id, _PaymentOrderPM.PaymentNumber, Tenant);
            if (documentsFilingPM != null && documentsFilingPM.EntityId != DeclarationId)
            {
                documentsFilingPM.EntityId = DeclarationId;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                documentsFilingPM.ChildEntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                documentsFilingPM.ExternalEntityReference = _PaymentOrderPM.AccountingCustomFile;

                documentsFilingService.Update(documentsFilingPM, null, LoggedUserId);
                LogMessagingUtil.Instance.AppendLine("Connect payment document " + documentsFilingPM.Code + " to AccountingCustomFile" + _PaymentOrderPM.AccountingCustomFile);
            }

        }
    }
}
