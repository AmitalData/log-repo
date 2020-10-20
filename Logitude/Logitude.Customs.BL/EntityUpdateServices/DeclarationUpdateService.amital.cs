using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital.CustomFile;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
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
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Def.Messaging.Customs;
using System.Xml.Linq;
using Logitude.Customs.BL.Validators;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationUpdateService
    {
        private long lCUSTOMFILENO;
        private AmitalContext _AmitalContext;
        private Boolean IsUpdateUnifreight; // moran 14.6.16 - Task 21737
        private bool _NO_LD2U;
        private bool IsDelayedDeclarationStatusRequestSent;

        private void UpdateUnifreight(DeclarationPM dirtyDeclarationPM)
        {
            if (dirtyDeclarationPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update &&
                dirtyDeclarationPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert) //Yuval Chalup 02.06.2015 AMI-53891 (Update CCUFILEM on creating a new Declaration)
            {
                return;
            }
            OurVersionToUpdateDeclarationPlatformFeeAndPrimaryInvoice(dirtyDeclarationPM);
            //var setting = CustomsSettingQueryService.GetSettingByTenant(dirtyDeclarationPM.Tenant);
            //if (!setting.IsConnectedToUniFreight)
            if(!dirtyDeclarationPM.IsConnectedToUnifreight)
            {
                return;
            }

            string loggingUserId = "";
            /*var contactRep = new ContactRepository(dirtyDeclarationPM.Tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(dirtyDeclarationPM.Tenant), dirtyDeclarationPM.Tenant);
            if (contact == null)
            {
                throw new BusinessErrorException("could not Resolve UserIdentityName per Tenant");
            }
            loggingUserId = contact.Id;*/
            //loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);
            if (RequestSheetContext.Current != null) loggingUserId = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
            if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);

            IsUpdateUnifreight = true; // moran 14.6.16 - Task 21737 
            DeclarationPM dbOccDeclarationPM = GetDBEntity(dirtyDeclarationPM);

            //<--- Yuval Chalup 10.05.2015 TASK-13252
            string xmlStatus = null;
            string statusId = ""; // moran 20.7.15 - Task 14521
            //if (dirtyDeclarationPM.VersionId == "0.1")
            if (dirtyDeclarationPM.DeclarationStatusTypeCode == "14") //Yuval Chalup 28.05.2015 TASK-13252 (Replace line above)
            {
                if (dbOccDeclarationPM.PaymentDate != null && dirtyDeclarationPM.PaymentDate == null)
                {
                    // moran 20.7.15 - Task 14521 -->
                    //xmlStatus = "del";
                    //DelDeclarationStatus(dirtyDeclarationPM, loggingUserId, "RSH");
                    // moran 20.7.15 - Task 14521
                    // moran 13.7.15 - Task 14521 -->
                    string notificationDeclaration = dirtyDeclarationPM.DeclarationNumber;
                    string notificationCode = "2754N";
                    UpdateNotification(dirtyDeclarationPM, notificationCode, notificationDeclaration, dirtyDeclarationPM.Tenant, "", "");
                    statusId = "RPD";
                    // moran 13.7.15 - Task 14521 <--
                }
                
            }
            //Yuval Chalup 10.05.2015 TASK-13252 --->


            #region 12/3/15 task 11788
            //eitan h 12/3/15 task 11788 -->
            Boolean doTask = true;
            //if (!string.IsNullOrWhiteSpace((string)dirtyDeclarationPM.CurrentContextTag))
            dirtyDeclarationPM.CurrentContextTag = dirtyDeclarationPM.CurrentContextTag ?? "";
            switch (dirtyDeclarationPM.CurrentContextTag.ToString())
            {
                case Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst:
                    doTask = false;
                    break;
                case Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst:
                    doTask = false;
                    break;
            }
            //<-- eitan h 12/3/15 task 11788
            #endregion

            //<--- Yuval Chalup 02.06.2015 AMI-53891 (YES Update CCUFILEM but NO task to update CFIFILEM) - CHANGED FROM:
            //if (dirtyDeclarationPM.CurrentContextTag != Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService.UpsertActionConst)
            //{
            //    var myDeclarationUpdateService = new UnifrightDeclarationUpdateService(dirtyDeclarationPM, dbOccDeclarationPM, loggingUserId);
            //    myDeclarationUpdateService.Update(doTask);
            //}
            //TO:
            if (dirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService.UpsertActionConst)
            {
                doTask = false;
            }
            if (dirtyDeclarationPM.CurrentContextTag.ToString().Contains("Upsert"))  // moran 28.7.16 - Task 22249
            {
                if (dirtyDeclarationPM.CurrentContextTag != null && dirtyDeclarationPM.CurrentContextTag.ToString() == "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService.Upsert()+CourierMasterChange")
                {
                    dirtyDeclarationPM.CurrentContextTag = "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService.Upsert()";
                }
                else
                {
                    doTask = false;
                }
            }

            if (dirtyDeclarationPM.IsAmendment==true)
            {
                var eventContextTagModel2 = dirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                if (eventContextTagModel2 != null)
                {
                    if (eventContextTagModel2.CallProccessID != EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate)
                    {
                        doTask = false;

                    }
                    //if (dirtyDeclarationPM.HatraDate != dbOccDeclarationPM.HatraDate)
                    //{
                    //  //  doTask = true;
                    //}

                }
            }

            if (dirtyDeclarationPM.DepositionStatusCode == "L" && dbOccDeclarationPM.DepositionStatusCode != "L")
            {
                OpenLogBoxUnifreighTask(dirtyDeclarationPM, "LDR2C", "", false, "");
                return;
            }
            bool deleteStatus = false;
            try
            {

                var myRequestSheetContext = RequestSheetContext.Current.GetContextOrDefault();
                if (myRequestSheetContext != null)
                {
                    dynamic my = myRequestSheetContext.RequestParams;
                    deleteStatus =my.DeleteStatus;
                }
            }
            catch (Exception)
            {

                ///throw;
            }
            var sw = Stopwatch.StartNew();
            // moran 3.7.16 - AMI-57242 -->
            var saveConsignments = dirtyDeclarationPM.Consignments; // moran 8.8.16 - Task 21737
            var saveSupplierInvoices = dirtyDeclarationPM.SupplierInvoices;
            try
            {
                if (HttpContextUtil.IsCustomDomainService())
                {
                    Logitude.Customs.Def.EntityPMs.SupplierInvoicePM mySupplierInvoicePM;
                    object entityPOCO; object entityPM; object entityParentPM;
                    this.GetAncestor(out entityPOCO, out entityPM, out entityParentPM);
                    mySupplierInvoicePM = (entityPM as Logitude.Customs.Def.EntityPMs.SupplierInvoicePM);
                    if (mySupplierInvoicePM != null)
                    {
                        Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService supplierInvoiceQueryService = new Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService(dirtyDeclarationPM.Tenant);
                        dirtyDeclarationPM.SupplierInvoices = supplierInvoiceQueryService.GetSupplierInvoicesForDeclaration(dirtyDeclarationPM.Id, dirtyDeclarationPM.Tenant, true);

                        if (dirtyDeclarationPM.SupplierInvoices != null && dirtyDeclarationPM.SupplierInvoices.Count() > 0)
                        {
                            int index = -1;
                            index = dirtyDeclarationPM.SupplierInvoices.FindIndex(d => d.InvoiceCounterKey == mySupplierInvoicePM.InvoiceCounterKey);
                            if (index != -1)
                            {
                                dirtyDeclarationPM.SupplierInvoices[index] = mySupplierInvoicePM;
                            }
                            else
                            {
                                dirtyDeclarationPM.SupplierInvoices.Add(mySupplierInvoicePM);
                            }
                        }
                        else
                        {
                            dirtyDeclarationPM.SupplierInvoices.Add(mySupplierInvoicePM);
                        }
                    }
                    else // moran 31.7.16 - Task 21737
                    {
                        if (saveSupplierInvoices == null || saveSupplierInvoices.Count() == 0)
                        {
                            Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService supplierInvoiceQueryService = new Logitude.Customs.BL.EntityQueryServices.SupplierInvoiceQueryService(dirtyDeclarationPM.Tenant);
                            dirtyDeclarationPM.SupplierInvoices = supplierInvoiceQueryService.GetSupplierInvoicesForDeclaration(dirtyDeclarationPM.Id, dirtyDeclarationPM.Tenant, false);
                        }
                    }
                }
                if (dirtyDeclarationPM.Consignments == null || dirtyDeclarationPM.Consignments.Count() == 0) // moran 7.8.16 - Task 21737 
                {
                    DeclarationKeys declarationKeys = new DeclarationKeys { Id = dirtyDeclarationPM.Id };
                    Logitude.Customs.BL.EntityQueryServices.ConsignmentQueryService consignmentQueryService = new Logitude.Customs.BL.EntityQueryServices.ConsignmentQueryService(dirtyDeclarationPM.Tenant);
                    dirtyDeclarationPM.Consignments = consignmentQueryService.GetMulti(declarationKeys, true);
                }

                if (dirtyDeclarationPM.SupplierInvoices != null && dirtyDeclarationPM.SupplierInvoices.Count() > 0)
                {
                    dirtyDeclarationPM.TotalInvoiceAmountInUSD = 0;
                    dirtyDeclarationPM.TotalInvoiceAmountInUSD = dirtyDeclarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD);
                }
                var myUnifrightDeclarationUpdateService = new UnifrightDeclarationUpdateService(dirtyDeclarationPM, dbOccDeclarationPM, loggingUserId);
                myUnifrightDeclarationUpdateService.Update(doTask);
                this._NO_LD2U = myUnifrightDeclarationUpdateService._NO_LD2U;
                AddExternalTrace("UnifrightDeclarationUpdateService:Took:" + sw.ElapsedMilliseconds);
            }
            finally
            {
                dirtyDeclarationPM.SupplierInvoices = saveSupplierInvoices;
                dirtyDeclarationPM.Consignments = saveConsignments; // moran 8.8.16 - Task 21737
            }
            // moran 3.7.16 - AMI-57242 <--
            
            //Yuval Chalup 02.06.2015 AMI-53891 --->

            //Remaked according to Yaron's request:
            //if (dirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst)
            //{
            //    UpdateUnifreightBilling(dirtyDeclarationPM, dbOccDeclarationPM, loggingUserId);
            //}
            //if (dirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst)
            //{
            //    CreateUnifreightPayment(dirtyDeclarationPM, dbOccDeclarationPM, loggingUserId);
            //}
            if (dirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst)
            {
                bool raiseStatus=false;
                if (dirtyDeclarationPM.DeclarationStatusTypeCode == "13")
                {
                    raiseStatus=true;
                    statusId = "DOK"; // moran 20.7.15 - Task 14521
                }
                // moran 20.7.15 - Task 14521 -->
                //OpenUnifreighTask(dirtyDeclarationPM, "LD2U", "DOK", raiseStatus, xmlStatus);
                if (statusId != "") raiseStatus = true;
                if (!this._NO_LD2U)
                {
                    OpenUnifreighTask(dirtyDeclarationPM, "LD2U", statusId, raiseStatus, xmlStatus, DateTime.Now);
                }
                //UnifreightTaskService myUnifreightTaskService = new UnifreightTaskService();
                //myUnifreightTaskService.OpenUnifreighTask(dirtyDeclarationPM, "LD2U", statusId, raiseStatus, xmlStatus);
                // moran 20.7.15 - Task 14521 <--
            }
            if (dirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst)
            {
                DateTime statusDateTimeLP2U = DateTime.Now;
                if(dirtyDeclarationPM.PaymentDate.HasValue)
                {
                    DateTime paymentDateTime = dirtyDeclarationPM.PaymentDate.Value;
                    if (statusDateTimeLP2U.Subtract(paymentDateTime).TotalMinutes > 30 || (paymentDateTime.Hour == 13 && paymentDateTime.Minute >= 45) || (paymentDateTime.Hour == 14 && paymentDateTime.Minute <= 15))
                    {
                        using (var trans = TransactionFactory.GetNewTransaction())
                        {
                            SendDelayedDeclarationStatusRequest(dirtyDeclarationPM);
                            trans.Complete();
                        }
                    }
                    statusDateTimeLP2U = dirtyDeclarationPM.PaymentDate.Value; // Task 36100
                }
                OpenUnifreighTask(dirtyDeclarationPM, "LP2U", "RSH", true, xmlStatus, statusDateTimeLP2U);
                //UnifreightTaskService myUnifreightTaskService = new UnifreightTaskService();
                //myUnifreightTaskService.OpenUnifreighTask(dirtyDeclarationPM, "LP2U", "RSH", true, xmlStatus);
            }
            //CHECK IF THE EVENT SHOULED BE REMARKED

            var eventContextTagModel = dirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.None:
                        break;
                    case EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                //RaiseReleaseGoodsEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode); //Mirit 13/05/15 Task 1623
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseService:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                RaiseGuaranteeCreationNotificationEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode);
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.DF_NG_2754_MSG10004_SubmitDeclarationFuturePayment:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                RaiseFuturePaymentEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode);
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.DF_NG_5018_MSG14004_ImportDeclarationCancellation:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                RaiseFuturePaymentEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode);
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService: // Mirit 07/07/15 - Task 11406  
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                RaiseReleaseGoodsEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode); 
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.DF_NG_8251_Web02_DeclarationStatusResponseServiceCancel: // moran 8.9.16 - Task 20303  
                    case EventContextTagModel.ProccessEnum.DF_NG_8251_Web02_DeclarationStatusResponseServicePreClearance:
                    case EventContextTagModel.ProccessEnum.DF_NG_8251_Web02_DeclarationStatusResponseServiceMessageToAgent:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                RaiseDeclarationStatusCancelEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode);
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.MN_MSG4_SendManifestFeedBack_MessageResponseService: 
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                RaiseDeclarationManifestEvent(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            ///            SendUnifreightCustomInterface(dirtyDeclarationPM, loggingUserId, dbOccDeclarationPM); //Remarked by Yuval Chalup 01.09.2014 AMI-50859
///         CalcReleaseGoodEvent(dirtyDeclarationPM, loggingUserId, dbOccDeclarationPM);

        }


        private void OpenLogBoxUnifreighTask(DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlStatus)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            var statusDateTime = DateTime.Now;

            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant))
                {
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;
                    var requestData = "";
                    string vendorCode = null;
                    string vendorName = null;
                    if (dirtyDeclarationPM.SupplierInvoices != null && dirtyDeclarationPM.SupplierInvoices.Count() > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(dirtyDeclarationPM.SupplierInvoices[0].VendorId))
                        {
                            var myQueryService = new CustomsVendorQueryService(this.currentContext);
                            var custVendor = myQueryService.GetSingle(dirtyDeclarationPM.SupplierInvoices[0].VendorId, false, true);
                            if (custVendor != null)
                            {
                                vendorCode = custVendor.VendorNumber;
                                vendorName = custVendor.VendorName;
                            }
                        }
                    }
                    if (vendorCode == null ) return;
                    
                    var XMLData = new XDocument(
                        new XElement("DepositionRequestPM",
                            new XElement("RequestDateTime", DateTime.Now.ToString("o")),
                            new XElement("ForwarderShipmentNumber", dirtyDeclarationPM.CustomFileNo),
                            new XElement("VendorCode", vendorCode),
                            new XElement("VendorName", vendorName)
                            )
                            );


                    requestData = XMLData.ToString(SaveOptions.None);
                    string unifreightUser = null;

                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyDeclarationPM.Tenant);
                    }

                    var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "DEPOSITION",
                        PRIMARYNUM = dirtyDeclarationPM.Id,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F",
                    };

                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT",
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20),
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = "DEPOSITION",
                        PRIMARYNUM = "0",
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "A",
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


        public void SendDelayedDeclarationStatusRequest(DeclarationPM dirtyDeclarationPM)
        {
            //bool DoNotsendDelayedDeclarationStatusRequest = String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["20181024.SendDelayedDeclarationStatusRequest"]);
            //if (DoNotsendDelayedDeclarationStatusRequest)
            //{
            //    LogMessagingUtil.Instance.AppendLine("DoNotsendDelayedDeclarationStatusRequest ==String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[20181024.SendDelayedDeclarationStatusRequest]) 14:45 til 15:15");
            //    return;
            //}
            if (this.IsDelayedDeclarationStatusRequestSent == true) return;
            LogMessagingUtil.Instance.AppendLine("SendDelayedDeclarationStatusRequest 13:45 til 14:15");
            LogitudeSettings.HandleLogMe(
                "DeclarationId:" + dirtyDeclarationPM.Id + Environment.NewLine + Environment.StackTrace.ToString()
                , false, "8250", new DateTime(2021, 1, 1));

            string user = null;
            if (RequestSheetContext.Current != null) user = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
            if (RequestSheetContext.Current != null) user = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
            if (string.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);
            DateTime? execTime = DateTime.Now;
            execTime = execTime.Value.AddMinutes(10);
            var newDeclarationStatusRequestParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,
                IsFakeResponse = true,
                InterfaceTypeCode = "8250",
                CustomFileNo = dirtyDeclarationPM.CustomFileNo,
                DeclarationNumber = dirtyDeclarationPM.DeclarationNumber,
                Tenant = dirtyDeclarationPM.Tenant,
                RequestName = "Declaration Status (Delayed)",
                ResponseName = "Declaration Status (Delayed)",
                CargoRadio = false,
                DeclarationRadio = true,
                OldReshimonRadio = false,
                OldReshimonNumber = null,
                LoggingEntityId = dirtyDeclarationPM.Id,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingUserId = user,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                SuppressSplitWR = true
            };

            try
            {
                SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.DeclarationStatusRequestParams>(newDeclarationStatusRequestParams
                    , false, execTime
                    );
            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
            {
                if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("8250 RequestInProgress stop create a new one !! ");
                }
                throw;
            }
            this.IsDelayedDeclarationStatusRequestSent = true;
        }


        private void RaiseDeclarationManifestEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + eventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        status_save = "no_fail",
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }

        }


        private void RaiseDeclarationStatusCancelEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode) // moran 8.9.16 - Task 20303
        {
            
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + eventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        
        }

        private void OurVersionToUpdateDeclarationPlatformFeeAndPrimaryInvoice(DeclarationPM dirtyDeclarationPM)
        {
            if (dirtyDeclarationPM.SupplierInvoices == null) return;
            if (dirtyDeclarationPM.SupplierInvoices.Count<1) return;
            var modPMs =dirtyDeclarationPM.SupplierInvoices.SelectMany(si => si.SupplierInvoiceModifications);
            dirtyDeclarationPM.PlatformFee = modPMs.Where(a => a.TypeCode == "I02" || a.TypeCode == "I01").Sum(d => d.Amount); 

        }

        private void OpenUnifreighTask(DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlStatus, DateTime statusDateTime)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            if (statusDateTime == null)
            {
                statusDateTime = DateTime.Now;
            }
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant))
                {
                    var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(_AmitalContext);
                    myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var requestData = "";
                    var addStatus = ""; // moran 17.9.15 - Task 15458
                    var comment = ""; // moran 20.9.15 - Task 15458
                    var addComment = ""; // moran 20.9.15 - Task 15458

                    CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", dirtyDeclarationPM.CustomFileNo, false);
                    if (myCCUQUELOCK == null)
                    {
                        var myCCUQUELOCKPM = new CCUQUELOCKPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            ENTNAME = "CFIFILEM",
                            FILENO = dirtyDeclarationPM.CustomFileNo,
                        };
                        myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                    }
                    if (taskType == "LD2U" && dirtyDeclarationPM.IsSignedVersion) // moran 17.9.15 - Task 15458
                    {
                        if (raiseStatus != true)
                        {
                            raiseStatus = true;
                            status = "INP";
                            comment = RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName;
                        }
                        else
                        {
                            addStatus = "INP";
                            addComment = RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName;
                        }
                    }

                    //var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyDeclarationPM.Tenant);
                    string unifreightUser = null;
                    if (RequestSheetContext.Current != null)
                    {
                        var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                        if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                        {
                            UserRepository userRep = new UserRepository(Tenant);
                            User user = userRep.GetSingleUser(loggingUserIdFromRS, dirtyDeclarationPM.Tenant, true);
                            if (user != null)
                            {
                                if (!String.IsNullOrWhiteSpace(user.Code))
                                {
                                    unifreightUser = user.Code;
                                }
                            }
                        }
                    }
                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyDeclarationPM.Tenant);
                    }
                    
                    if (raiseStatus == true)
                    {
                        string loggingUserId = null;
                        //if (status == "DOK") // moran 20.7.15 - Task 14521 - commented - all statuses for tasks should have user MEHES
                        {
                            ICommonDataContext dbContext = CommonDataContext.GetContext(dirtyDeclarationPM.Tenant);
                            UserRepository userRepository = new UserRepository(dbContext);
                            var user = userRepository.GetSingleUserByCode("MEHES", dirtyDeclarationPM.Tenant, true);
                            if (user != null)
                            {
                                loggingUserId = user.Id;
                            }
                        }
                        var myDeclarationUpdateService = new UnifrightDeclarationUpdateService(dirtyDeclarationPM, null, loggingUserId);
                        // moran 13.7.15 - Task 14521 -->
                        //requestData = myDeclarationUpdateService.GetMyFUStatusXML(status, status, "", "new", DateTime.Now, true);
                        requestData = myDeclarationUpdateService.GetMyFUStatusXML(status, status, comment, xmlStatus, statusDateTime, true);
                        // moran 13.7.15 - Task 14521 <--
                        if (!String.IsNullOrWhiteSpace(addStatus)) // moran 17.9.15 - Task 15458
                        {
                            var requestData2 = myDeclarationUpdateService.GetMyFUStatusXML(addStatus, addStatus, addComment, xmlStatus, DateTime.Now, true);
                            requestData = string.Concat(requestData, requestData2);
                        }
                    }
                    bool isxmltransmission = false;
                    if (taskType == "LP2U" && string.IsNullOrWhiteSpace(dirtyDeclarationPM.PaymentOrderNumber) && dirtyDeclarationPM.DeclarationStatusTypeCode == "5" && dirtyDeclarationPM.TotalTax <= 5)
                    {
                        GFUSTS myGFUSTS = XmlGenericUtil<GFUSTS>.DeSerializeObject(requestData);
                        transmission mytransmission = GetTransmission(myGFUSTS, "AMITAL", "FU Status from logitude");
                        var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                        requestData = xmltransmission;
                        requestData = requestData.Replace("</transmission>", string.Concat("<PAYMENTMODE>LOW_VALUE</PAYMENTMODE>", "</transmission>"));
                        isxmltransmission = true;
                    }

                    if (dirtyDeclarationPM != null && dirtyDeclarationPM.IsCourierDeclaration)
                    {
                        DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(dirtyDeclarationPM.Tenant);
                        DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(dirtyDeclarationPM.Id, true, false);
                        if (currentDeclarationCourierStatusPM != null && taskType == "LP2U" && string.IsNullOrWhiteSpace(dirtyDeclarationPM.PaymentOrderNumber) && dirtyDeclarationPM.TotalTax > 5)
                        {
                            if (raiseStatus == true)
                            {
                                if (isxmltransmission != true)
                                {
                                    GFUSTS myGFUSTS = XmlGenericUtil<GFUSTS>.DeSerializeObject(requestData);
                                    transmission mytransmission = GetTransmission(myGFUSTS, "AMITAL", "FU Status from logitude");
                                    var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                                    requestData = xmltransmission;
                                    isxmltransmission = true;
                                }
                                requestData = requestData.Replace("</transmission>", string.Concat("<CourierHighLow>", currentDeclarationCourierStatusPM.HighLowValue, "</CourierHighLow>", "</transmission>"));
                            }
                            else
                            {
                                requestData = string.Concat("<CourierHighLow>", currentDeclarationCourierStatusPM.HighLowValue, "</CourierHighLow>");
                            }

                        }
                    }
                    
                        //eitan h 12/3/15 moved to static -->
                        //short priority = 9;
                        //switch (taskType)
                        //{
                        //    case "L2U":
                        //        priority = 1;
                        //        break;
                        //    case "LD2U":
                        //        priority = 2;
                        //        break;
                        //    case "LP2U":
                        //        priority = 3;
                        //        break;
                        //    default:
                        //        break;
                        //}
                        //<--eitan h 12/3/15 moved to static

                        var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),//eitan h 12/3/15 new static operation
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
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "A",
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

        /*private string GetMyFUStatusXML(string entname, string primary_number, string status_id, string status_place, string comments, string xmlStatus)
        {
            if (String.IsNullOrWhiteSpace(xmlStatus))
            {
                xmlStatus = "new";
            }
            var myFUStatus = new AmitalEventTracerModel.FUStatus()
            {
                entname = entname,
                primary_number = primary_number,
                status = "new",
                xml_status = xmlStatus,
                status_id = status_id,
                status_DateTime = DateTime.Now,
                //status_place = status_place,
                status_save = "no_fail",
                comments = comments,
            };

            var myAmitalEventTracerModel = new AmitalEventTracerModel();
            myAmitalEventTracerModel.MyFUStatus = myFUStatus;
            GFUSTS myGFUSTS = AmitalEventTracer.GetFUStatus(myAmitalEventTracerModel);
            var xml = XmlGenericUtil<GFUSTS>.SerializeObject(myGFUSTS, true);

            return xml;
        }*/

        private void RaiseGuaranteeCreationNotificationEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string p)
        {
            try
            {
                var eventContextTagModel = dirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = "CGN",
                    notes = eventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status CGN from logitude (Guarantee Creation)",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "CGN",
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = eventContextTagModel.FUStatusRemarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode = CGN CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private void SendUnifreightCustomInterface(DeclarationPM dirtyDeclarationPM, string loggingUserId, DeclarationPM dbOccDeclarationPM)
        {
            ;
            var dirtyDeclarationConsignmentPM = new ConsignmentPM();
            var dbOccDeclarationConsignmentPM = new ConsignmentPM();
            var dirtyConsignmentPackagePM = new ConsignmentPackagePM();
            var dbOccConsignmentPackagePM = new ConsignmentPackagePM();

            //if (dirtyDeclarationPM.Consignments != null) // && dirtyDeclarationPM.Consignments.Count == 1) // moran 18.12.13 - task 2423 - multi Consignments adjusments
            if (dirtyDeclarationPM.Consignments != null && dirtyDeclarationPM.Consignments.Count > 0) // Yuval Chalup 31.12.2013 TASK-2682 (Change line above)
            {
                dirtyDeclarationConsignmentPM = dirtyDeclarationPM.Consignments[0]; ;
                if (dirtyDeclarationConsignmentPM.ConsignmentPackages.Count > 0)
                {
                    dirtyConsignmentPackagePM = dirtyDeclarationConsignmentPM.ConsignmentPackages.FirstOrDefault();
                }
            }
            //if (dbOccDeclarationPM.Consignments != null) // && dbOccDeclarationPM.Consignments.Count == 1) // moran 18.12.13 - task 2423 - multi Consignments adjusments
            if (dbOccDeclarationPM.Consignments != null && dbOccDeclarationPM.Consignments.Count > 0) // Yuval Chalup 31.12.2013 TASK-2682 (Change line above)
            {
                dbOccDeclarationConsignmentPM = dbOccDeclarationPM.Consignments[0];
                if (dbOccDeclarationConsignmentPM.ConsignmentPackages.Count > 0)
                {
                    dbOccConsignmentPackagePM = dbOccDeclarationConsignmentPM.ConsignmentPackages.FirstOrDefault();
                }
            }

            ////////////////////////////
            

            bool toSend = false;
            ///....
            if (dirtyDeclarationPM.CurrentContextTag == 
                Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService.UpsertActionConst)
            {
                return;
            }
            if (dirtyDeclarationPM.CustomFileNo == null)
            {
                return;   
            }
            int dirtyDeclarationQuantity = 0;
            decimal dirtyDeclarationWeight = 0;
            if (dirtyDeclarationPM.DeclarationOfficeCode != dbOccDeclarationPM.DeclarationOfficeCode || 
                dirtyDeclarationPM.TransportModeId != dbOccDeclarationPM.TransportModeId)
            {
                toSend = true;
            }
            else
            {

                if (dirtyDeclarationConsignmentPM.LoadingPortCode != dbOccDeclarationConsignmentPM.LoadingPortCode || 
                    dirtyDeclarationConsignmentPM.OriginCountryCode != dbOccDeclarationConsignmentPM.OriginCountryCode)
                {
                        toSend = true;
                    }
                    else
                    {
                    if (
                        dirtyDeclarationConsignmentPM.CargoDescription != dbOccDeclarationConsignmentPM.CargoDescription 
                        || 
                        dirtyDeclarationConsignmentPM.SecondCargoID != dbOccDeclarationConsignmentPM.SecondCargoID)
                        {
                            toSend = true;
                        }
                        else
                        {

                            if (dirtyConsignmentPackagePM.PackageTypeCode != dbOccConsignmentPackagePM.PackageTypeCode)
                            {
                                toSend = true;
                            }
                            else
                            {
                                // moran 18.12.13 - task 2423 - multi Consignments adjusments
                                /*      
                              if (dirtyConsignmentPackagePM.PackageQuantity != dbOccConsignmentPackagePM.PackageQuantity)
                                      {
                                          toSend = true;
                                      }
                                      else
                                      {
                                  if (dirtyConsignmentPackagePM.GrossMassMeasure != 
                                      dbOccConsignmentPackagePM.GrossMassMeasure)
                                          {
                                              toSend = true;
                                          }
                                      }
                              */
                               
                                foreach (var consignment in dirtyDeclarationPM.Consignments)
                                {
                                    foreach (var package in consignment.ConsignmentPackages)
                                    {
                                        dirtyDeclarationWeight += package.GrossMassMeasure != null ? package.GrossMassMeasure.Value : 0;
                                        dirtyDeclarationQuantity += package.PackageQuantity != null ? package.PackageQuantity.Value : 0;
                                    }
                                }
                                int dbOccDeclarationQuantity = 0;
                                decimal dbOccDeclarationWeight = 0;
                                foreach (var consignment in dbOccDeclarationPM.Consignments)
                                {
                                    foreach (var package in consignment.ConsignmentPackages)
                                    {
                                        dbOccDeclarationWeight += package.GrossMassMeasure != null ? package.GrossMassMeasure.Value : 0;
                                        dbOccDeclarationQuantity += package.PackageQuantity != null ? package.PackageQuantity.Value : 0;
                                    }
                                }
                                if (dirtyDeclarationQuantity != dbOccDeclarationQuantity)
                                {
                                    toSend = true;
                                }
                                else
                                {
                                    if (dirtyDeclarationWeight != dbOccDeclarationWeight)
                                    {
                                        toSend = true;
                                    }
                                }
                            }
                                

                    }
                }

            }
            if (!toSend)
            {
                if (dirtyDeclarationPM.TransportModeId == "A" && 
                    dirtyDeclarationConsignmentPM.ThirdCargoID!=dbOccDeclarationConsignmentPM.ThirdCargoID)
                {
                    toSend = true;
                }
                else
                {
                    // moran 3.12.13 - Task 2123  - always - not only for Ocean
                    //if (dirtyDeclarationPM.TransportModeId == "O" && 
                       if( dirtyDeclarationConsignmentPM.ManifestNumber!=dbOccDeclarationConsignmentPM.ManifestNumber)
                    {
                        toSend = true;
                    }
                }
            }
            if (!toSend)
            {
                if (dirtyDeclarationPM.HatraDate != dbOccDeclarationPM.HatraDate)//wi 1621 17.10.13
                {
                    toSend = true;
                }
                else if (dirtyDeclarationConsignmentPM.ManifestDate != dbOccDeclarationConsignmentPM.ManifestDate || //moran wi 1830 + 1855 14.11.13
                    dirtyDeclarationConsignmentPM.UnloadDate != dbOccDeclarationConsignmentPM.UnloadDate)
                {
                    toSend = true;
                }
            }
            ///
            LogMessagingUtil.Instance.AppendLine("!UpsertActionConst>SendUnifreightCustomInterface>tosend=" + toSend.ToString());
            if (!toSend) return;
            var myFile = new LOGICUSTFILE();
            myFile.LogitudeCustomsFile = new LogitudeCustomsFile[] { new LogitudeCustomsFile() };
            
            //init fields to be sent
            myFile.LogitudeCustomsFile[0].CustomFileNo = dirtyDeclarationPM.CustomFileNo;
            myFile.LogitudeCustomsFile[0].Id = dirtyDeclarationPM.Id;
            myFile.LogitudeCustomsFile[0].DeclarationOfficeCode = dirtyDeclarationPM.DeclarationOfficeCode;
            myFile.LogitudeCustomsFile[0].TransportModeId = dirtyDeclarationPM.TransportModeId;
            //myFile.LogitudeCustomsFile[0].GrantDate = dirtyDeclarationPM.HatraDate.ToString();
            if (dirtyDeclarationPM.HatraDate != null)
            {
                myFile.LogitudeCustomsFile[0].GrantDate = dirtyDeclarationPM.HatraDate.Value.Date.ToString("dd.MM.yy");
            }
            // moran 18.12.13 - task 2423 - multi Consignments adjusments -->
            //if (dirtyDeclarationPM.Consignments != null && dirtyDeclarationPM.Consignments.Count == 1)
            if (dirtyDeclarationPM.Consignments != null)
            // moran 18.12.13 - task 2423 - multi Consignments adjusments <--
            {

                myFile.LogitudeCustomsFile[0].LoadingPortCode = dirtyDeclarationConsignmentPM.LoadingPortCode;
                myFile.LogitudeCustomsFile[0].OriginCountryCode = dirtyDeclarationConsignmentPM.OriginCountryCode;
                myFile.LogitudeCustomsFile[0].CargoDescription = dirtyDeclarationConsignmentPM.CargoDescription;
                if (dirtyDeclarationConsignmentPM.ManifestDate != null)
                {
                    myFile.LogitudeCustomsFile[0].ManifestDate = dirtyDeclarationConsignmentPM.ManifestDate.Value.Date.ToString("dd.MM.yy"); //moran wi 1855 14.11.13
                }
                if (dirtyDeclarationConsignmentPM.UnloadDate != null)
                {
                    myFile.LogitudeCustomsFile[0].ArrivalDateTime = dirtyDeclarationConsignmentPM.UnloadDate.Value.Date.ToString("dd.MM.yy"); //moran wi 1830  14.11.13
                }
                // moran 18.12.13 - task 2423 - multi Consignments adjusments -->
                //if (dirtyDeclarationConsignmentPM.ConsignmentPackages != null && 
                //    dirtyDeclarationConsignmentPM.ConsignmentPackages.Count == 1)
                if (dirtyDeclarationConsignmentPM.ConsignmentPackages != null)
                // moran 18.12.13 - task 2423 - multi Consignments adjusments <--
                {
                    if (dirtyConsignmentPackagePM == null)
                    {
                        dirtyConsignmentPackagePM = new ConsignmentPackagePM();
                    }
                    myFile.LogitudeCustomsFile[0].PackageTypeCode = dirtyConsignmentPackagePM.PackageTypeCode;
                    // moran 18.12.13 - task 2423 - multi Consignments adjusments -->
                    //myFile.LogitudeCustomsFile[0].PackageQuantity = dirtyConsignmentPackagePM.PackageQuantity.ToString();
                    //myFile.LogitudeCustomsFile[0].GrossMassMeasure = dirtyConsignmentPackagePM.GrossMassMeasure.ToString();
                    
                    myFile.LogitudeCustomsFile[0].PackageQuantity = dirtyDeclarationQuantity.ToString();
                    myFile.LogitudeCustomsFile[0].GrossMassMeasure = dirtyDeclarationWeight.ToString();
                    // moran 18.12.13 - task 2423 - multi Consignments adjusments <--
                }

                if (dirtyDeclarationPM.TransportModeId == "A")
                {
                    myFile.LogitudeCustomsFile[0].MAWB = dirtyDeclarationConsignmentPM.SecondCargoID;
                    myFile.LogitudeCustomsFile[0].HAWB = dirtyDeclarationConsignmentPM.ThirdCargoID;
                }
                else
                {

                    string mySecondCargoID = dirtyDeclarationConsignmentPM.SecondCargoID ?? "";

                    if (mySecondCargoID.Length == 10 && mySecondCargoID.StartsWith("I"))
                    {
                        myFile.LogitudeCustomsFile[0].DealId = dirtyDeclarationConsignmentPM.SecondCargoID.Substring(1);
                    }
                    else
                    {
                        myFile.LogitudeCustomsFile[0].DealId = dirtyDeclarationConsignmentPM.SecondCargoID;

                    }
                }
                //if (dirtyDeclarationPM.TransportModeId == "O") // moran 3.12.13 - Task 2123 - always - no if
                //{
                    myFile.LogitudeCustomsFile[0].ManifestNumber = dirtyDeclarationConsignmentPM.ManifestNumber;
                //}
            }


            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
                Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, "CWSFLOGIFILE", "DeclarationUpsertPut")
            {
                Tenant = dirtyDeclarationPM.Tenant,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                EntityId = dirtyDeclarationPM.Id,
                UserId = loggingUserId,
                CommunicationSubject = "Logitude Declaration File",
                //LogitudeFile = myFile,
            };
#if (true)

            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, LOGICUSTFILE>(
                amitalCustomFileCommunicationModel, myFile);
            bool syn = true;
            if (DateTime.Now < new DateTime(2014, 04, 01))
            {
                syn = false;// due uroter 54 use 9501 instead 96 
            }

            var info = myUServerCommunicationService.Send(syn);

#else

            var myUServerCommunicationCustomFileService = new UServerCommunicationCustomFileService(amitalCustomFileCommunicationModel);
                var info = myUServerCommunicationCustomFileService.Send();
            
#endif
            LogMessagingUtil.Instance.AppendLine("UServerCommunicationCustomFileService CommunicationMessage =" + info.ImmediatelyMessage ?? "NULL");
            LogMessagingUtil.Instance.AppendLine("UServerCommunicationCustomFileService CommunicationLogId =" + info.CommunicationLogId ?? "NULL");
 
        }

        private static void CalcReleaseGoodEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, DeclarationPM dbOccDeclarationPM)
        {
            var eventCode = "";
            if (dbOccDeclarationPM.HatraDate != dirtyDeclarationPM.HatraDate)
            {
                if (!dirtyDeclarationPM.HatraDate.HasValue)
                {
                    if (dbOccDeclarationPM.HatraDate.HasValue)
                    {
                        eventCode = "RSG";
                    }
                }
                else
                {
                    eventCode = "RSC";
                }
            }
            if (!string.IsNullOrWhiteSpace(eventCode))
            {
                RaiseReleaseGoodsEvent(dirtyDeclarationPM, loggingUserId, eventCode);
            }
        }
        private static void RaiseReleaseGoodsEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + eventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        //status_place = "FRA",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        public static void SetCLSHWB(String DeclarationID , string loggingUserId, int a_tenent, UnifreightEventMode a_Mode)
        {
            DeclarationPM myDeclarationPM;
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(a_tenent);
            
            var customContext = CustomContext.GetContext(a_tenent);
            DeclarationCourierStatusQueryService courierStatusQueryService = new DeclarationCourierStatusQueryService(a_tenent);
            DeclarationCourierStatusPM mydeclarationCourierStatusPM = courierStatusQueryService.GetSingle(DeclarationID, true, false);
            DeclarationCourierStatusUpdateService courierStatusUpdateService =  new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), a_tenent);

            try
            {
                myDeclarationPM = declarationQueryService.GetSingleDeclarationById(DeclarationID, a_tenent);

                if (a_Mode == UnifreightEventMode.@new)
                {
                    mydeclarationCourierStatusPM.IsClosedForFollowUp = true;
                }
                else
                {
                    mydeclarationCourierStatusPM.IsClosedForFollowUp = false;
                }

                
                mydeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                courierStatusUpdateService.Update(mydeclarationCourierStatusPM,true);

                string unifrieghtEvent = "CLSHWB"; //"CLSHWB";
                //string eventRemarks = remarks;
                var MyUnifreightEventParam = new UnifreightEventParam()
                {
                    Code = unifrieghtEvent,
                    // Mode = UnifreightEventMode.@new,
                    Mode = a_Mode,
                    EventDateTime = DateTime.Now,
                    Entname = "CFIFILEM",
                    PrimaryNum = myDeclarationPM.CustomFileNo
                    //EventRemarks = eventRemarks,
                };
                LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                var myOpenUnifreighTask = new UnifreightEventTaskService();
                myOpenUnifreighTask.UpsertEventLE2U(
                    a_tenent,
                    loggingUserId,
                    MyUnifreightEventParam);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }


        private static void RaiseFuturePaymentEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + eventCode + " from logitude (Future Payment) ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        //status_place = "DFP",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private DeclarationPM GetDBEntity(DeclarationPM dirtyDeclarationPM)
        {
            
            var declarationQueryService = new DeclarationQueryService(dirtyDeclarationPM.Tenant);
           // declarationQueryService.LoadSupplierInvoices = false;
            declarationQueryService.LoadSupplierInvoicesWithItems = false;
            //if (IsUpdateUnifreight == true) ; //declarationQueryService.LoadSupplierInvoices = true; // moran 14.6.16 - Task 21737
            var myDBEntity = declarationQueryService.GetSingle(dirtyDeclarationPM.Id, true, false);
            return myDBEntity ?? new DeclarationPM();
  
        }
        void UpdateUnifreightCustomFile(DeclarationPM entityPM)
        {
            //class to add xml to communication log;
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Customs.Declaration", 0, true);
            amitalInfo info = new amitalInfo();
            info.DeclarationId = entityPM.DeclarationNumber;
            byte[] dataInfo = Communications.SerializeData<amitalInfo>(info); //serialize class and get bytes and if needed xml data string can be send in the communication params too
            CommunicationsParams communicationsParams = new CommunicationsParams()
            {
                ByteData = dataInfo,
                Tenant = entityPM.Tenant,
                LoggingObjectTableId = objectTable.Id,
                To = "amital",
                CommunicationLogTypeCode = "T",
                FolderName = "amital",
                From = "IIGC" , //"logitude",
                InOut = "O",
                LoggingEntityId = entityPM.Id,
                Status = "W",
                Subject = "Update Declaration",
                LoggingEntityReference = entityPM.DeclarationNumber,
                //XMLData=some xml data string 
            };
            string communicationLogId = Communications.AddCommunicationLog(communicationsParams);
            throw new Exception("UpdateUnifreightCustomFile:amitalqueue is not valid !!!!"); //Communications.SendCommunicationLogMessageToQueue("amitalqueue", communicationLogId, entityPM.Tenant);

        }

        // moran 13.7.15 - Task 14521 -->
        private void UpdateNotification(DeclarationPM DeclarationPM, string notificationDefinitionCode, string notificationDeclaration, int tenant, string msgString, string logisticPermitId)
        {
            string loggingUserId = "";
            var contactRep = new ContactRepository(tenant);
            var contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.ResolveUserIdentityName(tenant), tenant);

            loggingUserId = contact.Id;

            DoUpdateNotification(DeclarationPM, loggingUserId, notificationDefinitionCode, notificationDeclaration, tenant, msgString, logisticPermitId);

        }

        private void DoUpdateNotification(DeclarationPM declarationPM, string loggingUserId, string notificationDefinitionCode, string notificationDeclaration, int tenant, string msgString, string logisticPermitId)
        {

            string desc = "";
            string type = "A";
            string customerId = null;
            string referentUserId = null;

            desc = "יש להגיש הצהרה מחדש ";
            if (declarationPM != null)
            {
                desc = desc + " מספר הצהרה " + declarationPM.DeclarationNumber;
            }
            LogMessagingUtil.Instance.AppendLine("New Declaration Re-Payment Notification");

            ICustomContext dbContext = CustomContext.GetContext(tenant);
            var notificationUpdateService = new NotificationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            var notificationQueryService = new NotificationQueryService(dbContext);

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = tenant;
            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;
            newNotificationPM.EntityId = declarationPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");

            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;
            //newNotificationPM.Reference1Number = logisticPermitId;
            newNotificationPM.Reference2Number = declarationPM.DeclarationNumber;
           
            newNotificationPM.DepartmentId = declarationPM.DepartmentId;
            newNotificationPM.DeclarationOfficeCode = declarationPM.DeclarationOfficeCode;
            newNotificationPM.Reference1Number = declarationPM.CustomFileNo;
            customerId = declarationPM.CustomerId;
            referentUserId = declarationPM.ReferentUserId;
            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.CustomerId)) newNotificationPM.CustomerId = declarationPM.CustomerId; // moran 20.6.16 - Task 20789

            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = type;

            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, customerId, referentUserId, notificationDefinitionCode, "");

            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }
            
            notificationUpdateService.Update(newNotificationPM, true);

        }

        private static void RaiseRePayDeclarationEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = "",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + eventCode + " from logitude (RePay Declaration) ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        //status_place = "DFP",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        public static void DelDeclarationStatus(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    //notes = "Delete FU Status " + eventCode + " in unifreight ",
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "Delete FU Status " + eventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "del",
                        xml_status = "del",
                        status_id = eventCode,
                        status_DateTime = DateTime.Now,
                        comments = "",
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + eventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        // moran 13.7.15 - Task 14521 <--

        transmission GetTransmission<T>(T mySerilazeObject, string from, string Subject)
            where T : class

        {
            var CommunicationsParamsSubject = Subject;
            var mytransmission = new transmission();
            var mytransmission_details = new List<transmission_details>();
            var mytransmission_detail1 = new transmission_details()
            {
                sender = new sender() { Value = from },
                subject = new subject() { Value = CommunicationsParamsSubject }
            };

            string xml;


            xml = XmlGenericUtil<T>.SerializeObject(mySerilazeObject, true);
            
            var myListdata = new List<data>() { new data() { entity = xml } };

            mytransmission.data = myListdata.ToArray();// GetDataList().ToArray();
            if (mytransmission.data.Count() < 1)
            {
                throw new Exception("(mytransmission.data.Count < 1)");
            }

            mytransmission_details.Add(mytransmission_detail1);
            mytransmission.transmission_details = mytransmission_details.ToArray();
            return mytransmission;
        }
    }
    public class amitalInfo
    {
        public string DeclarationId { get; set; }
    }
}
