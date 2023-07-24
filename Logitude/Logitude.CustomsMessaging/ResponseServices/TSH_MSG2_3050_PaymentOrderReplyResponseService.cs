using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
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
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;
using UnifreightIIG.Common.MessageLib.Unifreight.FuStatus;
using UnifreightIIG.Common.MessageLib.Unifreight.Transmission;
using Logitude.Customs.BL.Models;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity.Validation;
using System.Configuration;
using System.Globalization;
using Logitude.Server.Tools.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class TSH_MSG2_3050_PaymentOrderReplyResponseService :
        ResponseServiceBase<PaymentOrderReplyResponseData, TSH_MSG2_PaymentOrderReply, NewPaymentRequestParams>
    {
        private PaymentOrderPM _PaymentOrderPM;
        private DeclarationPM _DeclarationPM;
        private string _ReturnMessage;
        private ICommonDataContext _CommonContext;

        public override PaymentOrderReplyResponseData GetResponse(TSH_MSG2_PaymentOrderReply customResponse, NewPaymentRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        public override Action<TSH_MSG2_PaymentOrderReply> GetActionShrinkCustomResponse()
        {
            //customResponse.PaymentOrderReply.PrintedPaymentForm

            return (customResponse) =>
            {

                if (customResponse == null) return;
                if (customResponse.PaymentOrderReply == null) return;
                if (customResponse.PaymentOrderReply.PrintedPaymentForm == null) return;


                var MD5Hash = MD5HashUtil.GetMD5Hash(customResponse.PaymentOrderReply.PrintedPaymentForm.content);
                customResponse.PaymentOrderReply.PrintedPaymentForm.content = System.Text.UTF8Encoding.UTF8.GetBytes(MD5Hash);


            };
        }


    
        public override void Update(TSH_MSG2_PaymentOrderReply customResponse, NewPaymentRequestParams requestParams)
        {
            //Analyze message 3050- Insert\Update payment order
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            _CommonContext = CommonDataContext.GetContext(requestParams.Tenant);
            var paymentOrderQueryService = new PaymentOrderQueryService(dbContext);
            var paymentOrderUpdateService = new PaymentOrderUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var paymentOrderLinesUpdateService = new PaymentOrderLineUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var paymentOrderConnectionTableUpdateService = new PaymentOrderConnectionTableUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var clientQueryService = new ClientQueryService(dbContext);
            var depositQueryService = new DepositQueryService(dbContext);
            var myDeclarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            List<string> declarationIdList = null;
            string fUStatusRemarks = "";
            string DeclarationConvertionText = "";

            var id = paymentOrderQueryService.GetIdByPaymentNumber(customResponse.PaymentOrderReply.PaymentDetails.paymentID.ToString(), requestParams.Tenant); // requestParams.PaymentNumber
            if (!String.IsNullOrWhiteSpace(id))
            {           
                this._PaymentOrderPM = paymentOrderQueryService.GetSingle(id, true, false); //Retrieval of existing payment data
                LogPayment("1");
                if (_PaymentOrderPM.IsClosed)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.RequestDescription = "הוראת תשלום " + _PaymentOrderPM.PaymentNumber;
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                    this.MyRequestSheetParam.EntityId1 = _PaymentOrderPM.Id;

                    if (_PaymentOrderPM.PaymentOrderConnectionTables != null)
                    {
                        foreach (var connectedEntityItem in _PaymentOrderPM.PaymentOrderConnectionTables)
                        {
                            if (connectedEntityItem.ConnectedEntityCode == "D")
                            {
                                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                                this.MyRequestSheetParam.EntityId2 = connectedEntityItem.ConnectedEntityId;
                                break;
                            }
                        }
                    }

                    //this.MyResponseData = new INF_MSG_GenericResponseData();
                    this.MyResponseData = new PaymentOrderReplyResponseData();
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.ApplicationID = _PaymentOrderPM.Id;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.UserMessage = "לא ניתן לעדכן ,הוראת תשלום סגורה";
                    if (requestParams.RequestParamsVersion > 0)
                    {
                        BuildPaymentOrderReply(requestParams.Tenant, customResponse);
                    }

                    return; // If the payment is close dont update
                }

                LogPayment("2");
                if (customResponse.PaymentOrderReply.paymentStatus == 5)
                {
                    _PaymentOrderPM.ChangeSetOp = ChangeSetOperation.Update;
                    fUStatusRemarks = CalcFUStatusRemarks(requestParams.Tenant, customResponse.PaymentOrderReply);
                    var myInsertEventContextTagModel = new EventContextTagModel() // Indication to Create Unifreight Status "POC"
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCancel,
                        EventCode = "POC",
                        EventRemarks = "הוראת תשלום " + this._PaymentOrderPM.PaymentNumber + " בוטלה",
                        FUStatusRemarks = "הוראת תשלום " + this._PaymentOrderPM.PaymentNumber + " בוטלה" + fUStatusRemarks,
                    };
                    _PaymentOrderPM.CurrentContextTag = myInsertEventContextTagModel;
                    _ReturnMessage = "בוטלה הוראה " + customResponse.PaymentOrderReply.PaymentDetails.paymentID;
                }
                else
                {
                    _PaymentOrderPM.ChangeSetOp = ChangeSetOperation.Update;
                    fUStatusRemarks = CalcFUStatusRemarks(requestParams.Tenant, customResponse.PaymentOrderReply);
                    var myInsertEventContextTagModel = new EventContextTagModel() // Indication to Create Unifreight Status "POU"
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceUpdate,
                        EventCode = "POU",
                        EventRemarks = "הוראת תשלום " + this._PaymentOrderPM.PaymentNumber + " עודכנה",
                        FUStatusRemarks = "הוראת תשלום " + this._PaymentOrderPM.PaymentNumber + " עודכנה" + fUStatusRemarks,
                    };
                    _PaymentOrderPM.CurrentContextTag = myInsertEventContextTagModel;
                    _ReturnMessage = "עודכנה הוראה " + customResponse.PaymentOrderReply.PaymentDetails.paymentID;
                }
                LogPayment("3");
            }

            if (_PaymentOrderPM == null)
            {
                //Create a new paymet order
                _PaymentOrderPM = new PaymentOrderPM();
                _PaymentOrderPM.Tenant = requestParams.Tenant;
                _PaymentOrderPM.ChangeSetOp = ChangeSetOperation.Insert;
                _PaymentOrderPM.IsClosed = false;
                _PaymentOrderPM.CreateDate = DateTime.Now;

                fUStatusRemarks = CalcFUStatusRemarks(requestParams.Tenant, customResponse.PaymentOrderReply);
                var myInsertEventContextTagModel = new EventContextTagModel() // Indication to Create Unifreight Status "POR"
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCreate,
                    EventCode = "POR",
                    EventRemarks = "הוראת תשלום " + customResponse.PaymentOrderReply.PaymentDetails.paymentID.ToString() + " נוצרה",
                    FUStatusRemarks = "הוראת תשלום " + customResponse.PaymentOrderReply.PaymentDetails.paymentID.ToString() + " נוצרה" + fUStatusRemarks,
                };
                _PaymentOrderPM.CurrentContextTag = myInsertEventContextTagModel;
                _ReturnMessage = "נוצרה הוראה " + customResponse.PaymentOrderReply.PaymentDetails.paymentID;
                LogPayment("4");
            }

            // Delete old Payment Order Lines
            DeletedPaymentOrderLines(paymentOrderLinesUpdateService);

            // Update payment order details
            _PaymentOrderPM.PaymentNumber = customResponse.PaymentOrderReply.PaymentDetails.paymentID.ToString();
            _PaymentOrderPM.TotalSumToPay = customResponse.PaymentOrderReply.paymentOrderTotalSumToPay;
            _PaymentOrderPM.LastPayDate = customResponse.PaymentOrderReply.paymentOrderPayDate;
            _PaymentOrderPM.Reason = customResponse.PaymentOrderReply.paymentOrderReason;
            string externalID = "";
            if (!string.IsNullOrWhiteSpace(customResponse.PaymentOrderReply.PaymentDetails.externalID.ToString()))
            {
                externalID = customResponse.PaymentOrderReply.PaymentDetails.externalID.ToString();
                if (externalID.Length < 9)
                {
                    externalID = customResponse.PaymentOrderReply.PaymentDetails.externalID.ToString().PadLeft(9, '0');
                }
                var importerId = clientQueryService.GetIdByCode(externalID, requestParams.Tenant, true);
                _PaymentOrderPM.ImporterId = importerId;
            }
            LogPayment("5");
            _PaymentOrderPM.CustomerActivityTypeCode = customResponse.PaymentOrderReply.PaymentDetails.CustomerActivityType.ToString();
            _PaymentOrderPM.PaymentOrderTypeCode = customResponse.PaymentOrderReply.paymentOrderType.ToString();
            _PaymentOrderPM.PaymentProcessCode = customResponse.PaymentOrderReply.paymentProcess.ToString();
            _PaymentOrderPM.PaymentStatusCode = customResponse.PaymentOrderReply.paymentStatus.ToString();
            _PaymentOrderPM.PaymentOrderLeftAmount = customResponse.PaymentOrderReply.paymentOrderLeftAmountToPay; // moran 6.7.16 - Task 21934
            _PaymentOrderPM.CustomsHouseCode = customResponse.PaymentOrderReply.customsHouse.ToString();
            _PaymentOrderPM.PaymentOrderLines = GetPaymentOrderLines(_PaymentOrderPM.PaymentNumber, requestParams.Tenant, customResponse.PaymentOrderReply.taxParagraph);

            // Update Payment Order connected entity
            _PaymentOrderPM.CustomsEntityTypeCode = customResponse.PaymentOrderReply.ConnectedEntity.entityType.ToString();
            _PaymentOrderPM.FirstEntityID = customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey1;
            _PaymentOrderPM.SecondEntityID = customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey2;
            _PaymentOrderPM.ThirdEntityID = customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey3;

            if (customResponse.PaymentOrderReply.paymentStatus == 3 || customResponse.PaymentOrderReply.paymentStatus == 5)//15473
            {
                _PaymentOrderPM.IsClosed = true;
            }
            else
            {
                _PaymentOrderPM.IsClosed = false;
            }

            string firstDeclaratioNumber = null;
            //Search for connented entity
            if (customResponse.PaymentOrderReply.ConnectedEntity.entityType == 1055 || customResponse.PaymentOrderReply.ConnectedEntity.entityType == 11118 || customResponse.PaymentOrderReply.ConnectedEntity.entityType == 11121) //Import Declaration
            {
                declarationIdList = new List<string>();
                //var declarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey1, requestParams.Tenant);
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                DeclarationPM myDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey1.Split('/')[0], requestParams.Tenant); // moran 19.10.16 - Task 23013 - update handle for retrieving Declaration Number
                if (myDeclarationPM != null && !string.IsNullOrWhiteSpace(myDeclarationPM.Id))
                {
                    firstDeclaratioNumber = myDeclarationPM.DeclarationNumber;
                    declarationIdList.Add(myDeclarationPM.Id);
                    if (myDeclarationPM.IsConvertedDeclaration)
                    {
                        DeclarationConvertionText = "\n" + myDeclarationPM.UserNotes;
                    }
                }
            }
            else if (customResponse.PaymentOrderReply.ConnectedEntity.entityType == 11122) //Leading Deficit File
            {
                var tapagQueryService = new TapagQueryService(dbContext);
                var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(dbContext);

                TapagPM tapagPM = tapagQueryService.GetSingleTapagByLeadingFileNumber(customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey1, requestParams.Tenant);
                if (tapagPM != null)
                {
                    declarationIdList = new List<string>();
                    List<TapagConnectionTablePM> tapagConnectionTable = tapagConnectionTableQueryService.GetTapagConnectionByTapagId(tapagPM.Id, tapagPM.Tenant);
                    foreach (var tapagConnectionTableItem in tapagConnectionTable)
                    {
                        declarationIdList.Add(tapagConnectionTableItem.DeclarationId);
                    }
                    if (tapagConnectionTable != null && tapagConnectionTable.Count > 0 & tapagConnectionTable[0].DeclarationId != null)
                    {
                        DeclarationPM myDeclarationPM = myDeclarationQueryService.GetAcceptDeclarationAmendment(tapagConnectionTable[0].DeclarationId, requestParams.Tenant);
                        if (myDeclarationPM != null && !string.IsNullOrWhiteSpace(myDeclarationPM.DeclarationNumber))
                        {
                            firstDeclaratioNumber = myDeclarationPM.DeclarationNumber;
                        }
                    }

                }
            }
            IDisposable disposableToken = null;
            try
            {
                if (firstDeclaratioNumber != null)
                {
                    string key = ProcessLockTableUtil.Instance.GetKey4Declaration(firstDeclaratioNumber, requestParams.Tenant);
                    disposableToken =
                           ///ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(key, "2470ResponseService.Update");
                           ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, true, key, "2470ResponseService.Update");
                }
                LogPayment("6");
                //Connect payment to declaration
                if (declarationIdList != null && declarationIdList.Count > 0)
                {
                    _PaymentOrderPM.CustomsRequestsDeclarationId = new List<string>();
                    foreach (var declarationId in declarationIdList)
                    {
                        _DeclarationPM = myDeclarationQueryService.GetAcceptDeclarationAmendment(declarationId, requestParams.Tenant);
                        if (_DeclarationPM != null)
                        {
                            _PaymentOrderPM.CustomerId = _DeclarationPM.CustomerId;
                            List<string> connectedDeclarationList = CheckPaymentOrderConnectionTables(paymentOrderConnectionTableUpdateService, "D");
                            if (connectedDeclarationList.Count() == 0 || (connectedDeclarationList.Count() == 1 & connectedDeclarationList.Contains(declarationId)))
                            {
                                _PaymentOrderPM.AccountingCustomFile = _DeclarationPM.CustomFileNo; // In case the payment order is connected only to one delcartion
                                _PaymentOrderPM.PaymentOrderSelectedLabel = "AccountingCustomFile"; // moran 18.7.16 - Task 21934
                            }
                            else
                            {
                                _PaymentOrderPM.AccountingCustomFile = null;
                                _PaymentOrderPM.PaymentOrderSelectedLabel = null; // moran 18.7.16 - Task 21934
                            }

                            //Create a new record in PaymentOrderConnectionTables (connented entity)
                            if (!connectedDeclarationList.Contains(declarationId))
                            {
                                PaymentOrderConnectionTablePM _paymentOrderConnectionTablePM = new PaymentOrderConnectionTablePM();
                                _paymentOrderConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                                _paymentOrderConnectionTablePM.Tenant = requestParams.Tenant;
                                _paymentOrderConnectionTablePM.ConnectedEntityId = declarationId;
                                _paymentOrderConnectionTablePM.ConnectedEntityCode = "D";
                                _PaymentOrderPM.PaymentOrderConnectionTables.Add(_paymentOrderConnectionTablePM);
                            }

                            _ReturnMessage = string.Concat(_ReturnMessage, ", ההוראה קושרה לתיק ", _DeclarationPM.CustomFileNo, DeclarationConvertionText);
                            _PaymentOrderPM.CustomsRequestsDeclarationId.Add(_DeclarationPM.CustomFileNo);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(externalID))
                    {
                        // Get customerId by VatNumber (cards table)
                        var cardRepository = new CardRepository(_CommonContext);
                        Card card = cardRepository.GetSingleCardByVatNumber(externalID, requestParams.Tenant);
                        if (card != null)
                        {
                            _PaymentOrderPM.CustomerId = card.Id;
                        }
                    }
                }

                LogPayment("7");
                //<--- Yuval Chalup 28.09.2016 TASK-23005 (If NO Declaration is connected - do not send status)
                if (_DeclarationPM == null)
                {
                    _PaymentOrderPM.CurrentContextTag = null;
                }
                //Yuval Chalup 28.09.2016 --->
                if (_DeclarationPM != null && _DeclarationPM.IsCourierDeclaration) // moran 16.11.17 - AMI-61878
                {
                    LogMessagingUtil.Instance.AppendLine("_PaymentOrderPM.PaymentProcessCode= " + _PaymentOrderPM.PaymentProcessCode + " _PaymentOrderPM.PaymentStatusCode= " + _PaymentOrderPM.PaymentStatusCode);
                    if (_PaymentOrderPM.PaymentProcessCode == "1" && _PaymentOrderPM.PaymentStatusCode == "3")// && HighLowValue != "L")//Eitan H 12/12/18 task 46063 remove != "L"
                    {
                        var myInsertEventContextTagModel = _PaymentOrderPM.CurrentContextTag as EventContextTagModel;
                        myInsertEventContextTagModel.UnifreighTaskCode = "LP2UB";
                        _PaymentOrderPM.CurrentContextTag = myInsertEventContextTagModel;
                        LogMessagingUtil.Instance.AppendLine("Added LP2UB " + _PaymentOrderPM.CustomFiles);
                        var myDeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                        _DeclarationPM.PaymentStatusCode = _PaymentOrderPM.PaymentStatusCode;
                        _DeclarationPM.PaymentOrderNumber = _PaymentOrderPM.PaymentNumber;

                        LogPayment("8");

                        _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                        if (_DeclarationPM.PaymentDate == null && customResponse.ResponseContentHeader.TransmitionDateTime != null)
                        {
                            _DeclarationPM.PaymentDate = customResponse.ResponseContentHeader.TransmitionDateTime;
                        }
                        LogMessagingUtil.Instance.AppendLine("Before DeclarationUpdateService: _DeclarationPM.PaymentStatusCode= " + _DeclarationPM.PaymentStatusCode + " _DeclarationPM.PaymentOrderNumber= " + _DeclarationPM.PaymentOrderNumber);
                        try
                        {
                            myDeclarationUpdateService.Update(_DeclarationPM, true);
                        }
                        catch (DbEntityValidationException ex)
                        {
                            var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                            LogMessagingUtil.Instance.AppendLine("Declaration Update Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(2000));
                            LogMessagingUtil.Instance.AppendLine("Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                            LogPayment("9");
                            return;
                        }
                        catch (System.Exception e)
                        {
                            LogMessagingUtil.Instance.AppendLine("Declaration Update Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(2000));
                            LogMessagingUtil.Instance.AppendLine("Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                            LogPayment("10");
                            return;
                        }

                        LogPayment("11");


                    }
                    else
                    {
                        LogMessagingUtil.Instance.AppendLine("NO LP2UB! " + "_PaymentOrderPM.PaymentProcessCode= " + _PaymentOrderPM.PaymentProcessCode + " _PaymentOrderPM.PaymentStatusCode= " + _PaymentOrderPM.PaymentStatusCode);
                    }



                }
                _PaymentOrderPM.CustomsRequestsSheetId = requestParams.CustomsRequestsSheetId;
                paymentOrderUpdateService.Update(_PaymentOrderPM, true);

                LogPayment("12");

                bool useTheAnalyzePaymentDocumentManager = false;//due not tested 4 now 
                if (useTheAnalyzePaymentDocumentManager)
                {
                    var myAnalyzePaymentDocumentManager = new AnalyzePaymentDocumentManager(_CommonContext, _PaymentOrderPM, _DeclarationPM);
                    myAnalyzePaymentDocumentManager.AnalyzePaymentDocument(customResponse.PaymentOrderReply.PrintedPaymentForm.content, requestParams);
                }
                else
                {
                    // Add Document- Printed Payment Form
                    AnalyzePaymentDocument(customResponse.PaymentOrderReply.PrintedPaymentForm, requestParams);
                }
                //this.MyResponseData = new INF_MSG_GenericResponseData()
                this.MyResponseData = new PaymentOrderReplyResponseData()
                {
                    Succeeded = true,
                    ApplicationID = _PaymentOrderPM.Id,
                    HasException = false,
                    UserMessage = _ReturnMessage,
                };
                if (requestParams.RequestParamsVersion > 0)
                {
                    BuildPaymentOrderReply(requestParams.Tenant, customResponse);
                }

                LogPayment("13");

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = "הוראת תשלום " + _PaymentOrderPM.PaymentNumber + DeclarationConvertionText;
                if (requestParams.RequestParamsVersion == 0)
                {
                    this.MyRequestSheetParam.RequestDescription = "אחזור הוראת תשלום " + requestParams.PaymentNumber + DeclarationConvertionText;
                }
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                this.MyRequestSheetParam.EntityId1 = _PaymentOrderPM.Id;
                this.MyRequestSheetParam.EntityReference = _PaymentOrderPM.PaymentNumber;
                if (_DeclarationPM != null)
                {
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = _DeclarationPM.Id;
                }

                LogPayment("14");
            }
            catch (ProcessLockException processLockException)
            {
                LogMessagingUtil.Instance.AppendLine("processLockException wait a minute!! ,the worker Role is proccesing anther response of the same Declaration  ");
                throw;
            }
            finally
            {
                if (disposableToken != null)
                {
                    disposableToken.Dispose();
                }
            }
        }

        private void LogPayment(string msg)
        {
            DateTime stopLogAt = DateTime.MinValue;
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20230601T000000.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            msg += $" TSH_MSG2_3050_PaymentOrderReplyResponseService.Update = _PaymentOrderPM.Id: {_PaymentOrderPM?.Id}, _PaymentOrderPM.PaymentNumber: {_PaymentOrderPM?.PaymentNumber} IsClosed: {_PaymentOrderPM?.IsClosed}, PaymentOrderLeftAmount: {_PaymentOrderPM?.PaymentOrderLeftAmount}, TotalSumToPay: {_PaymentOrderPM?.TotalSumToPay}";
            LogitudeSettings.HandleLogMe(msg, false, "CreateUD2LTService", stopLogAt);
        }

        private void BuildPaymentOrderReply(int tenant, TSH_MSG2_PaymentOrderReply customResponse)
        {
            ICustomContext myICustomContext = CustomContext.GetContext(tenant);
            PaymentOrderStatusQueryService myPaymentOrderStatusQueryService = new PaymentOrderStatusQueryService(myICustomContext);
            PaymentOrderTypeQueryService myPaymentOrderTypeQueryService = new PaymentOrderTypeQueryService(myICustomContext);
            CustomerActivityTypeQueryService myCustomerActivityTypeQueryService = new CustomerActivityTypeQueryService(myICustomContext);
            PaymentProcessQueryService myPaymentProcessQueryService = new PaymentProcessQueryService(myICustomContext);
            CustomsHouseTypeQueryService myCustomsHouseTypeQueryService = new CustomsHouseTypeQueryService(myICustomContext);
            EntityTypeLookupQueryService myEntityTypeLookupQueryService = new EntityTypeLookupQueryService(myICustomContext);
            ParagraphTypeQueryService myParagraphTypeQueryService = new ParagraphTypeQueryService(myICustomContext);
            PaymentMethodStatusQueryService myPaymentMethodStatusQueryService = new PaymentMethodStatusQueryService(myICustomContext);
            PaymentMethodTypeQueryService myPaymentMethodTypeQueryService = new PaymentMethodTypeQueryService(myICustomContext);

            this.MyResponseData.PaymentNumber = customResponse.PaymentOrderReply.PaymentDetails.paymentID.ToString();
            this.MyResponseData.PaymentOrderTotalSumToPay = customResponse.PaymentOrderReply.paymentOrderTotalSumToPay;

            if (customResponse.PaymentOrderReply.paymentStatus > 0)
            {
                this.MyResponseData.PaymentStatus = customResponse.PaymentOrderReply.paymentStatus;
                PaymentOrderStatusPM myPaymentOrderStatusPM = myPaymentOrderStatusQueryService.GetSingle(customResponse.PaymentOrderReply.paymentStatus.ToString(), false, false);
                if (myPaymentOrderStatusPM != null)
                {
                    this.MyResponseData.PaymentStatusName = myPaymentOrderStatusPM.LocalName;
                    if (string.IsNullOrWhiteSpace(this.MyResponseData.PaymentStatusName))
                    {
                        this.MyResponseData.PaymentStatusName = this.MyResponseData.PaymentStatus.ToString();
                    }
                }
            }
            if (customResponse.PaymentOrderReply.paymentOrderType > 0)
            {
                this.MyResponseData.PaymentOrderType = customResponse.PaymentOrderReply.paymentOrderType;
                PaymentOrderTypePM myPaymentOrderTypePM = myPaymentOrderTypeQueryService.GetSingle(customResponse.PaymentOrderReply.paymentOrderType.ToString(), false, false);
                if (myPaymentOrderTypePM != null)
                {
                    this.MyResponseData.PaymentOrderTypeName = myPaymentOrderTypePM.LocalName;
                    if (string.IsNullOrWhiteSpace(this.MyResponseData.PaymentOrderTypeName))
                    {
                        this.MyResponseData.PaymentOrderTypeName = this.MyResponseData.PaymentOrderType.ToString();
                    }
                }
            }
            this.MyResponseData.PaymentOrderPayDate = customResponse.PaymentOrderReply.paymentOrderPayDate;
            if (customResponse.PaymentOrderReply.PaymentDetails.CustomerActivityType != null)
            {
                this.MyResponseData.PaymentDetailData = new Logitude.CustomsMessaging.Common.ResponseData.PaymentDetailData()
                {
                    CustomerActivityType = customResponse.PaymentOrderReply.PaymentDetails.CustomerActivityType,
                    ExternalID = customResponse.PaymentOrderReply.PaymentDetails.externalID
                };

                CustomerActivityTypePM myCustomerActivityTypePM = myCustomerActivityTypeQueryService.GetSingle(customResponse.PaymentOrderReply.PaymentDetails.CustomerActivityType.ToString(), false, false);
                if (myCustomerActivityTypePM != null)
                {
                    this.MyResponseData.PaymentDetailData.CustomerActivityTypeName = myCustomerActivityTypePM.LocalName;
                    if (string.IsNullOrWhiteSpace(this.MyResponseData.PaymentDetailData.CustomerActivityTypeName))
                    {
                        this.MyResponseData.PaymentDetailData.CustomerActivityTypeName = this.MyResponseData.PaymentDetailData.CustomerActivityType.ToString();
                    }
                }
            }

            if (customResponse.PaymentOrderReply.paymentProcess > 0)
            {
                this.MyResponseData.PaymentProcess = customResponse.PaymentOrderReply.paymentProcess;
                PaymentProcessPM myPaymentProcess = myPaymentProcessQueryService.GetSingle(customResponse.PaymentOrderReply.paymentProcess.ToString(), false, false);
                if (myPaymentProcess != null)
                {
                    this.MyResponseData.PaymentProcessName = myPaymentProcess.LocalName;
                    if (string.IsNullOrWhiteSpace(this.MyResponseData.PaymentOrderTypeName))
                    {
                        this.MyResponseData.PaymentOrderTypeName = this.MyResponseData.PaymentOrderType.ToString();
                    }
                }
            }
            if (customResponse.PaymentOrderReply.customsHouse > 0)
            {
                this.MyResponseData.CustomsHouse = customResponse.PaymentOrderReply.customsHouse;
                CustomsHouseTypePM myCustomsHouseTypePM = myCustomsHouseTypeQueryService.GetSingle(customResponse.PaymentOrderReply.customsHouse.ToString(), false, false);
                if (myCustomsHouseTypePM != null)
                {
                    this.MyResponseData.CustomsHouseName = myCustomsHouseTypePM.LocalName;
                    if (string.IsNullOrWhiteSpace(this.MyResponseData.CustomsHouseName))
                    {
                        this.MyResponseData.CustomsHouseName = this.MyResponseData.CustomsHouse.ToString();
                    }
                }
            }

            this.MyResponseData.PaymentOrderReason = customResponse.PaymentOrderReply.paymentOrderReason;
            if (customResponse.PaymentOrderReply.ConnectedEntity != null)
            {
                this.MyResponseData.ConnectedEntityData = new Logitude.CustomsMessaging.Common.ResponseData.ConnectedEntityData()
                {
                    EntityType = customResponse.PaymentOrderReply.ConnectedEntity.entityType,
                    EntityIdKey1 = customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey1,
                    EntityIdKey2 = customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey2,
                    EntityIdKey3 = customResponse.PaymentOrderReply.ConnectedEntity.entityIdKey3
                };
                if (customResponse.PaymentOrderReply.ConnectedEntity.entityType > 0)
                {
                    EntityTypeLookupPM myEntityTypeLookupPM = myEntityTypeLookupQueryService.GetSingle(customResponse.PaymentOrderReply.ConnectedEntity.entityType.ToString(), false, false);
                    if (myEntityTypeLookupPM != null)
                    {
                        this.MyResponseData.ConnectedEntityData.EntityTypeName = myEntityTypeLookupPM.LocalName;
                        if (string.IsNullOrWhiteSpace(this.MyResponseData.ConnectedEntityData.EntityTypeName))
                        {
                            this.MyResponseData.ConnectedEntityData.EntityTypeName = this.MyResponseData.ConnectedEntityData.EntityType.ToString();
                        }
                    }
                }
            }
            if (customResponse.PaymentOrderReply.taxParagraph != null && customResponse.PaymentOrderReply.taxParagraph.Count() > 0)
            {
                this.MyResponseData.TaxParagraphList = new List<Logitude.CustomsMessaging.Common.ResponseData.TaxParagraphData>();
                foreach (var tmptaxParagraph in customResponse.PaymentOrderReply.taxParagraph)
                {
                    var taxParagraph = new Logitude.CustomsMessaging.Common.ResponseData.TaxParagraphData()
                    {
                        ParagraphType = tmptaxParagraph.paragraphType,
                        Amount = tmptaxParagraph.amount
                    };
                    if (tmptaxParagraph.paragraphType > 0)
                    {
                        ParagraphTypePM myParagraphType = new ParagraphTypePM();
                        myParagraphType = myParagraphTypeQueryService.GetSingle(tmptaxParagraph.paragraphType.ToString(), false, false);
                        if (myParagraphType != null)
                        {
                            taxParagraph.ParagraphTypeName = myParagraphType.LocalName;
                            if (string.IsNullOrWhiteSpace(taxParagraph.ParagraphTypeName))
                            {
                                taxParagraph.ParagraphTypeName = taxParagraph.ParagraphType.ToString();
                            }
                        }
                    }
                    this.MyResponseData.TaxParagraphList.Add(taxParagraph);
                }
            }
            if (customResponse.PaymentOrderReply.PaymentMethods != null && customResponse.PaymentOrderReply.PaymentMethods.Count() > 0)
            {
                this.MyResponseData.PaymentMethodsList = new List<Logitude.CustomsMessaging.Common.ResponseData.PaymentMethodData>();
                foreach (var tmpPaymentMethods in customResponse.PaymentOrderReply.PaymentMethods)
                {
                    var paymentMethod = new Logitude.CustomsMessaging.Common.ResponseData.PaymentMethodData()
                    {
                        PaymentMethodType = tmpPaymentMethods.type,
                        Amount = tmpPaymentMethods.amount,
                        PaymentMethodStatus = tmpPaymentMethods.PaymentMethodStatus
                    };
                    if (tmpPaymentMethods.type > 0)
                    {
                        PaymentMethodTypePM myPaymentMethodTypePM = new PaymentMethodTypePM();
                        myPaymentMethodTypePM = myPaymentMethodTypeQueryService.GetSingle(tmpPaymentMethods.type.ToString(), false, false);
                        if (myPaymentMethodTypePM != null)
                        {
                            paymentMethod.PaymentMethodTypeName = myPaymentMethodTypePM.LocalName;
                            if (string.IsNullOrWhiteSpace(paymentMethod.PaymentMethodTypeName))
                            {
                                paymentMethod.PaymentMethodTypeName = paymentMethod.PaymentMethodType.ToString();
                            }
                        }
                    }
                    if (tmpPaymentMethods.PaymentMethodStatus > 0)
                    {
                        PaymentMethodStatusPM myPaymentMethodStatusPM = new PaymentMethodStatusPM();
                        myPaymentMethodStatusPM = myPaymentMethodStatusQueryService.GetSingle(tmpPaymentMethods.PaymentMethodStatus.ToString(), false, false);
                        if (myPaymentMethodStatusPM != null)
                        {
                            paymentMethod.PaymentMethodStatusName = myPaymentMethodStatusPM.LocalName;
                            if (string.IsNullOrWhiteSpace(paymentMethod.PaymentMethodStatusName))
                            {
                                paymentMethod.PaymentMethodStatusName = paymentMethod.PaymentMethodStatus.ToString();
                            }
                        }
                    }
                    this.MyResponseData.PaymentMethodsList.Add(paymentMethod);
                }
            }

            return;
        }

        private string CalcFUStatusRemarks(int tenant,
            //TSH_MSG2_PaymentOrderReplyPaymentOrderReply 
            PaymentOrderReply paymentOrderReply)
        {
            string fUStatusRemarks = "";
            if (paymentOrderReply != null)
            {
                string paymentStatusName = "";
                string paymentProcessName = "";
                if (paymentOrderReply.paymentStatus > 0)
                {
                    PaymentOrderStatusQueryService paymentOrderStatusQueryService = new PaymentOrderStatusQueryService(tenant);
                    PaymentOrderStatusPM paymentOrderStatus = paymentOrderStatusQueryService.GetSingle(paymentOrderReply.paymentStatus.ToString(), false, true);
                    if (paymentOrderStatus != null)
                    {
                        paymentStatusName = paymentOrderStatus.LocalName;
                    }
                }
                if (paymentOrderReply.paymentProcess > 0)
                {
                    PaymentProcessQueryService paymentProcessQueryService = new PaymentProcessQueryService(tenant);
                    PaymentProcessPM paymentProcess = paymentProcessQueryService.GetSingle(paymentOrderReply.paymentProcess.ToString(), false, true);
                    if (paymentProcess != null)
                    {
                        paymentProcessName = paymentProcess.LocalName;
                    }
                }

                fUStatusRemarks = "\n" + "סטטוס הוראה: " + paymentStatusName
                                 + "\n" + "תאריך אחרון לתשלום: " + paymentOrderReply.paymentOrderPayDate.Date.ToString("dd/MM/yyyy")
                                 + "\n" + "סכום לתשלום: " + paymentOrderReply.paymentOrderTotalSumToPay
                                 + "\n" + "התהליך היוצר: " + paymentProcessName;
            }

            return fUStatusRemarks;
        }

        private List<string> CheckPaymentOrderConnectionTables(PaymentOrderConnectionTableUpdateService paymentOrderConnectionTableUpdateService, string connectedEntityCode)
        {
            List<string> declarationList = new List<string>();

            foreach (var paymentItem in _PaymentOrderPM.PaymentOrderConnectionTables)
            {
                // Check connection - if it from the same type 
                if (paymentItem.ConnectedEntityCode == connectedEntityCode)
                {
                    declarationList.Add(paymentItem.ConnectedEntityId);
                }
            }

            return declarationList;
        }

        private void AnalyzePaymentDocument(Attachment attachment, NewPaymentRequestParams requestParams)
        {
            //ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(_CommonContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "3053", IsCourier = IsCourier(requestParams.Tenant) });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            DocumentsFilingPM documentsFilingPM = null;

            if (attachment == null)
            {
                return;
            }

            //Check if file already exists
            string objectTableId = "";
            string entityId = "";
            string childEntityId = "";
            string dir = "I";
            if (_DeclarationPM != null) // If the Payment order is connected to Declaration
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                entityId = _DeclarationPM.Id;
                childEntityId = _PaymentOrderPM.Id;
                dir = _DeclarationPM.Direction;
            }
            else
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                entityId = _PaymentOrderPM.Id;
            }
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("POR", _PaymentOrderPM.Tenant);
            var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, "I", requestParams.Tenant);

            foreach (var documentItem in documentsFilingPMList)
            {
                if (documentType != null) //Yuval Chalup 14.10.2015 TASK-16973 (Add only the IF)
                {
                    if (documentItem.DocumentTypeId == documentType.Id)
                    {
                        documentsFilingPM = documentItem;
                        break;
                    }
                }
            }

            if (documentsFilingPM == null)
            {
                CreatePaymentDocument(attachment, requestParams);
            }
            else
            {
                UpdatePaymentDocument(documentsFilingPM, attachment, requestParams);
            }
        }

        private void UpdatePaymentDocument(DocumentsFilingPM documentsFilingPM, Attachment attachment, NewPaymentRequestParams requestParams)
        {
            //ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(_CommonContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "3053", IsCourier = IsCourier(requestParams.Tenant) });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            string logMessage = "";

            documentsFilingService.OnlyIfChangeUpdateAndAddVersion = true;
            
            documentsFilingService.Update(documentsFilingPM, attachment.content, requestParams.LoggingUserId);

            if (_DeclarationPM != null)
            {
                logMessage = " -For declaration " + _DeclarationPM.DeclarationNumber;
            }
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + logMessage);
            _ReturnMessage = string.Concat(_ReturnMessage, " ועודכן מסמך ", documentsFilingPM.Code);
        }

        private void CreatePaymentDocument(Attachment attachment, NewPaymentRequestParams requestParams)
        {
            //ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(_CommonContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "3053" , IsCourier= IsCourier(requestParams.Tenant) });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            string logMessage = "";

            var documentsFilingPM = new DocumentsFilingPM();
            documentsFilingPM.Tenant = _PaymentOrderPM.Tenant;
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("POR", _PaymentOrderPM.Tenant);
            //<--- Yuval Chalup 14.10.2015 TASK-16973
            if (documentType == null)
            {
                LogMessagingUtil.Instance.AppendLine("Did not create DocumentsFiling - Could not find DocumentType POR in DB");
                _ReturnMessage = string.Concat(_ReturnMessage, "Did not create DocumentsFiling - Could not find DocumentType POR in DB");
            }
            //Yuval Chalup 14.10.2015 TASK-16973 --->
            documentsFilingPM.DocumentTypeId = documentType.Id;
            if (_DeclarationPM != null)
            {
                documentsFilingPM.EntityId = _DeclarationPM.Id;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                documentsFilingPM.ChildEntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                documentsFilingPM.ExternalEntityReference = _DeclarationPM.CustomFileNo;
                logMessage = " -For declaration " + _DeclarationPM.DeclarationNumber;
            }
            else
            {
                documentsFilingPM.EntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                
                
                //HD#330080 - moti will connect id to ref 
                documentsFilingPM.ChildEntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");


            }

            documentsFilingPM.ChildEntityReference = _PaymentOrderPM.PaymentNumber;
            documentsFilingPM.CreatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.OwnerId = requestParams.LoggingUserId;
            documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.ReceivedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.DirectionCode = "I";
            documentsFilingPM.Description = "הוראת תשלום " + _PaymentOrderPM.PaymentNumber;
            documentsFilingPM.ExternalEntityName = "CFIFILEM";
            documentsFilingPM.FileExtension = "PDF";

            documentsFilingService.Create(documentsFilingPM, attachment.content, requestParams.LoggingUserId);
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + logMessage);
            _ReturnMessage = string.Concat(_ReturnMessage, " ונוצר מסמך ", documentsFilingPM.Code);
        }

        private bool IsCourier(int tenant)
        {
            var pm=CustomsSettingQueryService.GetSettingByTenant(tenant);
            return pm?.CompanyType == "B";//Courier
        }

        private List<PaymentOrderLinePM> GetPaymentOrderLines(string paymentOrderNumber, int tenant, TaxParagraph[] taxParagraph)
        {
            var myPaymentOrderLines = new List<PaymentOrderLinePM>();
            foreach (var orderLine in taxParagraph)
            {
                myPaymentOrderLines.Add(CreatePaymentOrderLines(orderLine));
            }
            return myPaymentOrderLines;
        }

        private PaymentOrderLinePM CreatePaymentOrderLines(TaxParagraph orderLine)
        {
            var mypaymentOrderLinePM = new PaymentOrderLinePM();
            mypaymentOrderLinePM.ParagraphTypeCode = orderLine.paragraphType.ToString();
            mypaymentOrderLinePM.Amount = orderLine.amount;
            mypaymentOrderLinePM.ChangeSetOp = ChangeSetOperation.Insert;
            return mypaymentOrderLinePM;
        }

        private void DeletedPaymentOrderLines(PaymentOrderLineUpdateService myPaymentOrderLineUpdateService)
        {
            foreach (var item in _PaymentOrderPM.PaymentOrderLines)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
                myPaymentOrderLineUpdateService.Update(item, false);
            }
        }

        private void DeletedPaymentOrderConnectionTables(PaymentOrderConnectionTableUpdateService paymentOrderConnectionTableUpdateService, string connectedEntityCode)
        {
            foreach (var paymentItem in _PaymentOrderPM.PaymentOrderConnectionTables)
            {
                // Delete old connection if it from the same type 
                if (paymentItem.ConnectedEntityCode == connectedEntityCode)
                {
                    paymentItem.ChangeSetOp = ChangeSetOperation.Delete;
                    paymentOrderConnectionTableUpdateService.Update(paymentItem, false);
                }
            }
        }
    }

    public class AnalyzePaymentDocumentManager
    {
        private ICommonDataContext _CommonContext;
        private PaymentOrderPM _PaymentOrderPM;
        private DeclarationPM _DeclarationPM;
        private string _ReturnMessage;
        public AnalyzePaymentDocumentManager(ICommonDataContext CommonContext, PaymentOrderPM PaymentOrderPM, DeclarationPM DeclarationPM)
        {
            _PaymentOrderPM = PaymentOrderPM;
            _DeclarationPM = DeclarationPM;
            _CommonContext = CommonContext ?? CommonDataContext.GetContext(_PaymentOrderPM.Tenant);

        }
        public void AnalyzePaymentDocument(/*Attachment attachment*/byte[] content, RequestParamsBase requestParams)
        {

            if (/*attachment*/content == null)
            {
                return;
            }
            //ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            DocumentsFilingPM documentsFilingPM = GetDocumentsFiling(requestParams);

            if (documentsFilingPM == null)
            {
                CreatePaymentDocument(/*attachment*/content, requestParams);
            }
            else
            {
                UpdatePaymentDocument(documentsFilingPM, /*attachment*/content, requestParams);
            }
        }

        public DocumentsFilingPM GetDocumentsFiling(RequestParamsBase requestParams)
        {
            var documentsFilingService = new UnifreightDocumentsFilingService(_CommonContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "3053", IsCourier = IsCourier(requestParams.Tenant) });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            DocumentsFilingPM documentsFilingPM = null;



            //Check if file already exists
            string objectTableId = "";
            string entityId = "";
            string childEntityId = "";
            string dir = "I";
            if (_DeclarationPM != null) // If the Payment order is connected to Declaration
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                entityId = _DeclarationPM.Id;
                childEntityId = _PaymentOrderPM.Id;
                dir = _DeclarationPM.Direction;
            }
            else
            {
                objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                entityId = _PaymentOrderPM.Id;
            }
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("POR", _PaymentOrderPM.Tenant);
            var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, "I", requestParams.Tenant);

            foreach (var documentItem in documentsFilingPMList)
            {
                if (documentType != null) //Yuval Chalup 14.10.2015 TASK-16973 (Add only the IF)
                {
                    if (documentItem.DocumentTypeId == documentType.Id)
                    {
                        documentsFilingPM = documentItem;
                        break;
                    }
                }
            }

            return documentsFilingPM;
        }

        public void UpdatePaymentDocument(DocumentsFilingPM documentsFilingPM, /*Attachment attachment*/byte[] content, RequestParamsBase requestParams)
        {
            //ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(_CommonContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "3053", IsCourier = IsCourier(requestParams.Tenant) });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            string logMessage = "";

            documentsFilingService.OnlyIfChangeUpdateAndAddVersion = true;

            documentsFilingService.Update(documentsFilingPM, /*attachment.*/content, requestParams.LoggingUserId);

            if (_DeclarationPM != null)
            {
                logMessage = " -For declaration " + _DeclarationPM.DeclarationNumber;
            }
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + logMessage);
            _ReturnMessage = string.Concat(_ReturnMessage, " ועודכן מסמך ", documentsFilingPM.Code);
        }
        private bool IsCourier(int tenant)
        {
            var pm = CustomsSettingQueryService.GetSettingByTenant(tenant);
            return pm?.CompanyType == "B";//Courier
        }
        public void CreatePaymentDocument(/*Attachment attachment*/byte[] content, RequestParamsBase requestParams)
        {
            //ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(_CommonContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "3053", IsCourier = IsCourier(requestParams.Tenant) });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            string logMessage = "";

            var documentsFilingPM = new DocumentsFilingPM();
            documentsFilingPM.Tenant = _PaymentOrderPM.Tenant;
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("POR", _PaymentOrderPM.Tenant);
            //<--- Yuval Chalup 14.10.2015 TASK-16973
            if (documentType == null)
            {
                LogMessagingUtil.Instance.AppendLine("Did not create DocumentsFiling - Could not find DocumentType POR in DB");
                _ReturnMessage = string.Concat(_ReturnMessage, "Did not create DocumentsFiling - Could not find DocumentType POR in DB");
            }
            //Yuval Chalup 14.10.2015 TASK-16973 --->
            documentsFilingPM.DocumentTypeId = documentType.Id;
            if (_DeclarationPM != null)
            {
                documentsFilingPM.EntityId = _DeclarationPM.Id;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                documentsFilingPM.ChildEntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");
                documentsFilingPM.ExternalEntityReference = _DeclarationPM.CustomFileNo;
                logMessage = " -For declaration " + _DeclarationPM.DeclarationNumber;
            }
            else
            {
                documentsFilingPM.EntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");

                //HD#330080 - moti will connect id to ref 
                documentsFilingPM.ChildEntityId = _PaymentOrderPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.PaymentOrder");

            }

            documentsFilingPM.ChildEntityReference = _PaymentOrderPM.PaymentNumber;
            documentsFilingPM.CreatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.OwnerId = requestParams.LoggingUserId;
            documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.ReceivedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.DirectionCode = "I";
            documentsFilingPM.Description = "הוראת תשלום " + _PaymentOrderPM.PaymentNumber;
            documentsFilingPM.ExternalEntityName = "CFIFILEM";
            documentsFilingPM.FileExtension = "PDF";

            documentsFilingService.Create(documentsFilingPM, /*attachment.*/content, requestParams.LoggingUserId);
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + logMessage);
            _ReturnMessage = string.Concat(_ReturnMessage, " ונוצר מסמך ", documentsFilingPM.Code);
        }
    }
}