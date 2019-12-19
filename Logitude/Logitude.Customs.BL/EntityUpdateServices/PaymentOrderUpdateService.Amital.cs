using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
//using Logitude.Customs.BL.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PaymentOrderUpdateService
    {
        private long lCUSTOMFILENO;
        private AmitalContext _AmitalContext;
        private DeclarationPM _DirtyDeclarationPM;
        private PaymentOrderPM _DirtyEntityPM;
        private string _LoggingUserId;

        private void UpdateUnifreight(PaymentOrderPM dirtyEntityPM)
        {
            this._DirtyEntityPM = dirtyEntityPM;
            this._LoggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);
            string loggingUserId = this._LoggingUserId;

            bool toSendStatusPOP = false;
            bool toSendStatusPOC = false;
            bool toSendStatusPOU = false;
            bool toSendStatusPOR = false;
            bool toLoadDeclarationPM = false;
            bool toRaiseEventDFN = false; // moran 3.11.14 - Task 8327

            DeclarationPM connectedDeclarationPM = GetDBEntity(dirtyEntityPM);
            this._DirtyDeclarationPM = connectedDeclarationPM;
            var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG7_AgentPaymentReplyResponseService:
                        toSendStatusPOP = true;
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCancel:
                        toSendStatusPOC = true;
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceUpdate:
                        toSendStatusPOU = true;
                        break;
                    case EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCreate:
                        toSendStatusPOR = true;
                        break;
                    // moran 3.11.14 - Task 8327 - add case
                    case EventContextTagModel.ProccessEnum.Deficit_NG_5009_MSG14_FirstAndSeconderyRequirementsMessageResponseService:
                        toRaiseEventDFN = true;
                        break;
                }

                if(eventContextTagModel.UnifreighTaskCode == "LP2UB")
                {
                    UpdateUnifreightPaymentOrderBLD(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
                }
                LogMessagingUtil.Instance.AppendLine("eventContextTagModel.UnifreighTaskCode = " + eventContextTagModel.UnifreighTaskCode ?? "NULL");
                if (eventContextTagModel.UnifreighTaskCode == "LE2U")
                {
                    string remarks = "מספר הוראת תשלום " + dirtyEntityPM.PaymentNumber;
                    SendPPT(connectedDeclarationPM.Tenant,connectedDeclarationPM.CustomFileNo, loggingUserId, remarks);
                }
            }

            toLoadDeclarationPM = (toSendStatusPOP);           

            if(toSendStatusPOP == true)
            {
                UpdateUnifreightPaymentOrder(dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }

            if (toSendStatusPOC)
            {
                SendPaymentStatus("POC",dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }
            if (toSendStatusPOU)
            {
                SendPaymentStatus("POU", dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }
            if (toSendStatusPOR)
            {
                SendPaymentStatus("POR", dirtyEntityPM, connectedDeclarationPM, loggingUserId);
            }

            if (toRaiseEventDFN) // moran 3.11.14 - Task 8327
            {
                RaiseEvent("DFN", dirtyEntityPM, connectedDeclarationPM, loggingUserId);

            }
        }

        
        private void SendPOP(PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PaymentOrder",
                    EventCode = "POP",
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status POP from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo, // to check witch entity
                        status = "new",
                        xml_status = "new",
                        status_id = "POP",
                        status_DateTime = DateTime.Now,
                        status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }


        private void SendPaymentStatus(string statusId,PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            List<string> connectedDeclarationList = new List<string>();
            if (dirtyEntityPM.CustomsRequestsDeclarationId != null && dirtyEntityPM.CustomsRequestsDeclarationId.Count() > 0)
            {
                connectedDeclarationList = dirtyEntityPM.CustomsRequestsDeclarationId;
            }
            else
            {
                connectedDeclarationList.Add(connectedDeclarationPM.CustomFileNo);
            }

            foreach (var connectedFileItem in connectedDeclarationList)
            {
                try
                {
                    var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {
                        Tenant = dirtyEntityPM.Tenant,
                        objectTableName = "Customs.PaymentOrder",
                        EventCode = statusId,
                        notes = eventContextTagModel.EventRemarks,
                        CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                        EntityId = dirtyEntityPM.Id,
                        UserId = loggingUserId,

                        CommunicationSubject = "FU Status " + statusId + " from logitude",
                        MyFUStatus = new AmitalEventTracerModel.FUStatus()
                        {
                            entname = "CFIFILEM",
                            primary_number = connectedFileItem, 
                            status = "new",
                            xml_status = "new",
                            status_id = statusId,
                            status_DateTime = DateTime.Now,
                            //status_place = "FRA",
                            //status_save = "no_fail",
                            comments = eventContextTagModel.FUStatusRemarks,
                        }
                    };
                    if (connectedDeclarationList.Count() > 1 && connectedDeclarationList.IndexOf(connectedFileItem) != 0)
                    {
                        myAmitalEventTracerModel.notes = "DO_NOT_RAISE_EVENT";
                    }
                    AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
                }
                catch (Exception)
                {
                    // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                    throw;
                }
            }
        }


        private DeclarationPM GetDBEntity(PaymentOrderPM dirtyEntityPM)
        {
            var declarationQueryService = new DeclarationQueryService(dirtyEntityPM.Tenant);
            var myDBEntity = new DeclarationPM();
            string declarationId = "";

            if (!string.IsNullOrWhiteSpace(dirtyEntityPM.AccountingCustomFile))
            {
                declarationId = declarationQueryService.GetIdByCustomFileNo(dirtyEntityPM.AccountingCustomFile, dirtyEntityPM.Tenant);
            }
            else if (dirtyEntityPM.PaymentOrderConnectionTables != null && dirtyEntityPM.PaymentOrderConnectionTables.Count() > 0)
            {
                foreach(var decalrationItem in dirtyEntityPM.PaymentOrderConnectionTables)
                {
                    if(decalrationItem.ConnectedEntityCode == "D")
                    {
                        declarationId = decalrationItem.ConnectedEntityId;
                        break;
                    }
                }
            }
            else
            {
                if (dirtyEntityPM.CustomsEntityTypeCode == "1055" || dirtyEntityPM.CustomsEntityTypeCode == "11118") //If Type is Import Declaration
                {
                    declarationId = declarationQueryService.GetIdByDeclarationNumber(dirtyEntityPM.FirstEntityID, dirtyEntityPM.Tenant);
                }
            }

            if (!string.IsNullOrWhiteSpace(declarationId))
            {
                myDBEntity = declarationQueryService.GetSingle(declarationId, true, false);
            }
            return myDBEntity ?? new DeclarationPM();
        }

        private void UpdateUnifreightPaymentOrder(PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            //return;
            LogMessagingUtil.Instance.AppendLine("!UpdateUnifreightBilling>FileState=" + dirtyEntityPM.PaymentStatusCode ?? "NULL");
            if (dirtyEntityPM.PaymentOrderMethods == null || dirtyEntityPM.PaymentOrderMethods.Count == 0)
            { // moran 7.6.15 - Task 12424 - add checks for not null or zero //Mirit 27/9/18 Remove: dirtyEntityPM.PaymentOrderMethods[0].CustomerActivityTypeCode != "3"
                LogMessagingUtil.Instance.AppendLine("!UpdateUnifreightBilling> PaymentOrderMethods are empty");
                return;
            }
            var myFile = new LOGIPAYORD();
            var myLogitudePaymentOrder = new LogitudePaymentOrder();
            myLogitudePaymentOrder.Type = "PaymentOrder";
            myLogitudePaymentOrder.Primary = dirtyEntityPM.AccountingCustomFile;
            myLogitudePaymentOrder.NIS_Amount = dirtyEntityPM.TotalSumToPay.Value.ToString();
            myLogitudePaymentOrder.AccountingCard = dirtyEntityPM.AccountingCustomFile; // moran 2.5.16 - task 19778
            if (dirtyEntityPM.TotalSumToPay.Value != dirtyEntityPM.PaymentOrderLeftAmount.Value)
            {
                myLogitudePaymentOrder.Reference = dirtyEntityPM.PaymentNumber + "-1"; // moran 18.7.16 - Task 21934
            }
            else
            {
                myLogitudePaymentOrder.Reference = dirtyEntityPM.PaymentNumber;
            }
            if (dirtyEntityPM.ActualPayDate != null)
            {
                myLogitudePaymentOrder.Value_Date = dirtyEntityPM.ActualPayDate.Value.ToString("dd.MM.yy hh:mm");
            }
            myLogitudePaymentOrder.PaymentProcessCode = dirtyEntityPM.PaymentProcessCode; // moran 26.7.15 - Task 14566
            if (dirtyEntityPM.PaymentOrderMethods != null && dirtyEntityPM.PaymentOrderMethods.Count() > 0) // moran 17.9.15 - Task 16495
            {
                myLogitudePaymentOrder.PaymentMethods = GetPaymentMethods(dirtyEntityPM);
            }
            myFile.LogitudePaymentOrder = new LogitudePaymentOrder[] { myLogitudePaymentOrder };

            // moran 4.6.15 - Task 12424 -->
            //<--- Yuval Chalup 25.04.2016 TASK-19778(Replace line above - Add option to sent Accounting Card in LA2U) - CHANGED FROM: 
            //if (dirtyEntityPM.AccountingCustomFile == connectedDeclarationPM.CustomFileNo || dirtyEntityPM.CustomFiles == connectedDeclarationPM.CustomFileNo)
            //TO:
            if (dirtyEntityPM.PaymentOrderSelectedLabel == "AccountingCustomFile" && (dirtyEntityPM.AccountingCustomFile == connectedDeclarationPM.CustomFileNo || dirtyEntityPM.CustomFiles == connectedDeclarationPM.CustomFileNo))
            {
                myFile.LogitudePaymentOrder.FirstOrDefault().AccountingCard = null;
                var xml = XmlGenericUtil<LOGIPAYORD>.SerializeObject(myFile, true);
                OpenUnifreighTask(dirtyEntityPM.AccountingCustomFile,connectedDeclarationPM, "LA2U", "POP", true, xml);
                LogMessagingUtil.Instance.AppendLine("UpdateUnifreightPaymentOrder->OpenUnifreighTask->LA2U= " + Environment.NewLine + xml);
            }
            else
            {
                if (dirtyEntityPM.PaymentOrderSelectedLabel == "PaymentOrderAccCard" && !string.IsNullOrWhiteSpace(dirtyEntityPM.AccountingCustomFile)) // moran 10.5.16 - task 19778 add NOT to IsNullOrWhiteSpace check
                {
                    myFile.LogitudePaymentOrder.FirstOrDefault().Primary = null;
                    var xml = XmlGenericUtil<LOGIPAYORD>.SerializeObject(myFile, true);
                    OpenUnifreighTask(dirtyEntityPM.AccountingCustomFile,connectedDeclarationPM, "LA2U", "POP", false, xml);
                    LogMessagingUtil.Instance.AppendLine("UpdateUnifreightPaymentOrder->OpenUnifreighTask->LA2U= " + Environment.NewLine + xml);
                }
                else // moran 18.7.16 - Task 21934
                {
                    myFile.LogitudePaymentOrder.FirstOrDefault().AccountingCard = null;
                    var xml = XmlGenericUtil<LOGIPAYORD>.SerializeObject(myFile, true);
                    OpenUnifreighTask(dirtyEntityPM.AccountingCustomFile,connectedDeclarationPM, "LA2U", "POP", true, xml);
                    LogMessagingUtil.Instance.AppendLine("UpdateUnifreightPaymentOrder->OpenUnifreighTask->LA2U= " + Environment.NewLine + xml);
                }
            }
            //Yuval Chalup 25.04.2016 TASK-19778 --->

            return;
            // moran 4.6.15 - Task 12424 <--

            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, "CWSFLOGIPAYORD", "UpsertPaymentOrder")

            {
                Tenant = dirtyEntityPM.Tenant,
                objectTableName = "Customs.Declaration",

                CommunicationLoggingEntityReference = connectedDeclarationPM.DeclarationNumber,
                EntityId = dirtyEntityPM.Id,
                UserId = loggingUserId,
                CommunicationSubject = "Logitude Declaration File UpsertPaymentOrder",

            };

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, LOGIPAYORD>(
                amitalCustomFileCommunicationModel, myFile);
            var info = myUServerCommunicationService.Send(true);
            if (String.IsNullOrWhiteSpace(info.ImmediatelyResponse))
            {
                throw new Exception("ImmediatelyResponse is null ");
            }
            var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(info.ImmediatelyResponse);
            var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();
            if (genericResponseObj == null)
            {
                throw new Exception("GenericResponse.GenericResponseObj is null ");
            }
            if (!String.IsNullOrWhiteSpace(genericResponseObj.Status))
            {
                int sts;
                int.TryParse(genericResponseObj.Status, out sts);
                if (sts < 0)
                {
                    string mess = "Failed To Update Payment Order in Unifreight";
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.ErrorDescription))
                    {
                        mess = mess + Environment.NewLine + genericResponseObj.ErrorDescription;
                    }
                    if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
                    {
                        mess = mess + Environment.NewLine + genericResponseObj.Message;
                    }
                    LogMessagingUtil.Instance.AppendLine("UpdateUnifreightPaymentOrder>genericResponseObj>Message= " + mess);
                    throw new Exception(mess);
                }
            }

            if (!String.IsNullOrWhiteSpace(genericResponseObj.Message))
            {
                LogMessagingUtil.Instance.AppendLine("UpdateUnifreightPaymentOrder>genericResponseObj>Message= " + genericResponseObj.Message);
            }

        }

        private void UpdateUnifreightPaymentOrderBLD(PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            LogMessagingUtil.Instance.AppendLine("!UpdateUnifreightBLD>FileState=" + dirtyEntityPM.PaymentStatusCode ?? "NULL");

            var myFile = new LOGIPAYORD();
            var myLogitudePaymentOrder = new LogitudePaymentOrder();
            myLogitudePaymentOrder.Type = "PaymentOrder";
            myLogitudePaymentOrder.Primary = dirtyEntityPM.AccountingCustomFile;
            myLogitudePaymentOrder.NIS_Amount = dirtyEntityPM.TotalSumToPay.Value.ToString();
            myLogitudePaymentOrder.AccountingCard = dirtyEntityPM.AccountingCustomFile;
            if (dirtyEntityPM.TotalSumToPay.Value != dirtyEntityPM.PaymentOrderLeftAmount.Value)
            {
                myLogitudePaymentOrder.Reference = dirtyEntityPM.PaymentNumber + "-1"; 
            }
            else
            {
                myLogitudePaymentOrder.Reference = dirtyEntityPM.PaymentNumber;
            }
            if (dirtyEntityPM.ActualPayDate != null)
            {
                myLogitudePaymentOrder.Value_Date = dirtyEntityPM.ActualPayDate.Value.ToString("dd.MM.yy hh:mm");
            }
            myLogitudePaymentOrder.PaymentProcessCode = dirtyEntityPM.PaymentProcessCode; 
            if (dirtyEntityPM.PaymentOrderMethods != null && dirtyEntityPM.PaymentOrderMethods.Count() > 0) 
            {
                myLogitudePaymentOrder.PaymentMethods = GetPaymentMethods(dirtyEntityPM);
            }
            myFile.LogitudePaymentOrder = new LogitudePaymentOrder[] { myLogitudePaymentOrder };

            EventContextTagModel eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null && eventContextTagModel.UnifreighTaskCode == "LP2UB") // moran 16.11.17 - AMI-61878
            {
                eventContextTagModel.UnifreighTaskCode = "";
                dirtyEntityPM.CurrentContextTag = eventContextTagModel;
                OpenUnifreighTask(dirtyEntityPM.AccountingCustomFile, connectedDeclarationPM, "LP2UB", "RSH", true, "");
                LogMessagingUtil.Instance.AppendLine("UpdateUnifreightPaymentOrder->OpenUnifreighTask->LP2UB");
            }

        }

        private PaymentMethods[] GetPaymentMethods(PaymentOrderPM dirtyEntityPM) // moran 17.9.15 - Task 16495
        {
            var PaymentMethodList = new List<PaymentMethods>();
            foreach (var paymentOrderMethod in dirtyEntityPM.PaymentOrderMethods)
            {
                var myLogitudePaymentOrderMethod = new PaymentMethods();
                myLogitudePaymentOrderMethod.BankCode = paymentOrderMethod.InternalBankId;
                myLogitudePaymentOrderMethod.Amount = paymentOrderMethod.Amount.GetValueOrDefault().ToString();
                myLogitudePaymentOrderMethod.CustomerActivityType = paymentOrderMethod.CustomerActivityTypeCode;
                myLogitudePaymentOrderMethod.PaymentMethod = paymentOrderMethod.TypeCode;
                PaymentMethodList.Add(myLogitudePaymentOrderMethod);
            }
            return PaymentMethodList.ToArray();
        }


        // moran 3.11.14 - Task 8327 -->
        private void RaiseEvent(string eventCode, PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PaymentOrder",
                    EventCode = eventCode,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,
                };
                //<--- Yuval Chalup 04.12.2014 TASK-9142 (Raise )
                if (eventCode == "DFN")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = myAmitalEventTracerModel.Tenant,
                        EventTypeCode = myAmitalEventTracerModel.EventCode,
                        UserId = myAmitalEventTracerModel.UserId,
                        EntityId = myAmitalEventTracerModel.EntityId,
                        ObjectTableName = myAmitalEventTracerModel.objectTableName,
                        Notes = myAmitalEventTracerModel.notes,                        
                        IsAddedManually = myAmitalEventTracerModel.manually,
                        NewStatusId = myAmitalEventTracerModel.newStatusId,
                        CurrentStatusId = myAmitalEventTracerModel.currentStatusId,
                    });
                }
                //Yuval Chalup 04.12.2014 TASK-9142 --->
                else
                {
                    AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
                }   
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
        // moran 3.11.14 - Task 8327 <--

        // moran 4.6.15 - Task 12424 -->
        private void OpenUnifreighTask(string accountingCustomFile,DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlReq)
        {
            var sw = Stopwatch.StartNew();

            TransactionScope scope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant))
                {
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var requestData = "";

                    var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyDeclarationPM.Tenant);
                    if (raiseStatus == true)
                    {
                        //var myDeclarationUpdateService = new UnifrightDeclarationUpdateService(dirtyDeclarationPM, null, null);
                        //requestData = myDeclarationUpdateService.GetMyFUStatusXML(status, status, "", "new", DateTime.Now, true);
                        // moran 22.7.15 - Task 14521 - all statuses for tasks should have user MEHES -->
                        ICommonDataContext dbContext = CommonDataContext.GetContext(dirtyDeclarationPM.Tenant);
                        UserRepository userRepository = new UserRepository(dbContext);
                        var user = userRepository.GetSingleUserByCode("MEHES", dirtyDeclarationPM.Tenant, true);
                        if (user != null)
                        {
                            _LoggingUserId = user.Id;
                        }
                        // moran 22.7.15 - Task 14521 - all statuses for tasks should have user MEHES <--
                        requestData = GetMyFUStatusXML(status, status, "", "new", DateTime.Now, true);
                    }
                    if (xmlReq != null)
                    {
                        if (requestData != null)
                        {
                            requestData = "<requestData>" + Environment.NewLine + xmlReq + Environment.NewLine + requestData + Environment.NewLine + "</requestData>";
                        }
                        else
                        {
                            requestData = xmlReq;
                        }
                    }

                    var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = !string.IsNullOrWhiteSpace(dirtyDeclarationPM.CustomFileNo) ? "CFIFILEM" : "GNDCARD",
                        PRIMARYNUM = accountingCustomFile,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),
                        //PRIORITY = priority,
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F", // moran 28.6.16 - AMI-57170
                        //LOGTIME = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                    };
                    //myYCULTASKPM.TASKID = CommCounterUtil.GetUnique30(myYCULTASKPM.LOGTIME);
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = !string.IsNullOrWhiteSpace(dirtyDeclarationPM.CustomFileNo) ? "CFIFILEM" : "GNDCARD",
                        PRIMARYNUM = accountingCustomFile,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "A",
                        GSTRING1 = "",
                        //GSTRING1 = string.IsNullOrWhiteSpace(dirtyDeclarationPM.CustomFileNo) ? "NO_LOCK" : "",
                        //GSTRING1 = myYCULTASKPM.TASKID,
                    };
                    myGGGQUpdateService.Update(myGGGQPM, true);
                    if (scope != null)
                    {
                        scope.Complete();
                    }
                }
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }
            LogMessagingUtil.Instance.AppendLine("OpenUnifreighTask:Took:" + sw.ElapsedMilliseconds);
        }
        // moran 4.6.15 - Task 12424 <--

        // moran 4.6.15 - Task 12424 -->
        private string GetMyFUStatusXML//(string entname, string primary_number, string status_id, string status_place, string comments, string xmlStatus)
            (string event_id, string status_id, string comments, string xmlStatus, DateTime statusDateTime, bool isRaiseEvent = false) 
        {

            string loggingUserId = this._LoggingUserId;
            if (String.IsNullOrWhiteSpace(xmlStatus))
            {
                xmlStatus = "new";
            }
             if (string.IsNullOrWhiteSpace(loggingUserId))
            {
                loggingUserId = AuthenticationUtil.ResolveUserId(this._DirtyDeclarationPM.Tenant);
            }

            if (isRaiseEvent)
            {
                RaiseEvent(this._DirtyEntityPM, this._DirtyDeclarationPM, loggingUserId, event_id, status_id, false);
            }
            var myFUStatus = new AmitalEventTracerModel.FUStatus()
            {
                entname = "CFIFILEM",
                primary_number = this._DirtyDeclarationPM.CustomFileNo,
                status = "new",
                xml_status = xmlStatus,
                status_id = status_id,
                status_DateTime = DateTime.Now,
                //status_place = status_place,
                //status_save = "no_fail",
                comments = comments,
            };

            var myAmitalEventTracerModel = new AmitalEventTracerModel();
            myAmitalEventTracerModel.MyFUStatus = myFUStatus;
            GFUSTS myGFUSTS = AmitalEventTracer.GetFUStatus(myAmitalEventTracerModel);
            var xml = XmlGenericUtil<GFUSTS>.SerializeObject(myGFUSTS, true);

            return xml;
        }

        private void RaiseEvent(PaymentOrderPM dirtyEntityPM, DeclarationPM connectedDeclarationPM, string loggingUserId, string eventCode, string status_id, bool doNotSendStatus = false)
        {
            try
            {
                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.PaymentOrder",
                    EventCode = eventCode,
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyEntityPM.PaymentNumber,
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status" + status_id + " from logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = connectedDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = status_id,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };
                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode = " + eventCode + " PaymentNumber = " + dirtyEntityPM.PaymentNumber + " CustomFileNo = " + connectedDeclarationPM.CustomFileNo + "  ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel,true);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
        // moran 4.6.15 - Task 12424 <--

        private static void SendPPT(int Tenant, string CustomFileNo, string loggedContactId, string remarks)
        {
            if (string.IsNullOrWhiteSpace(loggedContactId))
            {
                ContactRepository contactRepository = new ContactRepository(Tenant);
                var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }
            }
            string unifrieghtEvent = "PPT";
            string eventRemarks = remarks;
            var MyUnifreightEventParam = new UnifreightEventParam()
            {
                Code = unifrieghtEvent,
                Mode = UnifreightEventMode.@new,
                EventDateTime = DateTime.Now,
                Entname = "CFIFILEM",
                PrimaryNum = CustomFileNo,
                EventRemarks = eventRemarks,
            };
            LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
            var myOpenUnifreighTask = new UnifreightEventTaskService();
            myOpenUnifreighTask.UpsertEventLE2U(
                Tenant,
                loggedContactId,
                MyUnifreightEventParam);
        }

    }
}
