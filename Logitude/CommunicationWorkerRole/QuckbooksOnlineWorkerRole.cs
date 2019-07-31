using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace CommunicationWorkerRole
{
    class QuckbooksOnlineWorkerRole : WorkerEntryPoint
    {


        CloudQueue quickbooksonlinequeue;
        DbQueueService queueservice;
        private string queueName;
        ICommonDataContext Commoncontext;
        IInvoiceContext Invoicecontext;

        private string type;
        private string QBOIDSuccess = null;
        private string APInvoiceId = null;
        private string OldTransferStatusCode;
        public override void Run()
        {
            while (IsRunning)
           {
                if (!General.IsUpdating())
                {
                    int tenant = 0;
                    try
                    {
                        queueservice = queueservice = new DbQueueService(queueName, tenant);
                        var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;
                        if (response.MessageId != null)
                        {

                            string communicationLogId = response.MessageValues["QuickbooksOnline"].ToString();
                            type = response.MessageValues["type"].ToString();
                            int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                            if (!response.MessageValues.ContainsKey("OldTransferStatusCode"))
                            {
                                OldTransferStatusCode = null;
                            }
                            else 
                            OldTransferStatusCode = response.MessageValues["OldTransferStatusCode"];
                            Commoncontext = CommonDataContext.GetContext(tenant);
                            Invoicecontext = InvoiceContext.GetContext(tenant);
                            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(Commoncontext);
                            CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                            if (cl != null)
                            {
                                if (cl.CommunicationStatusTypeCode == "T")
                                {
                                    queueservice.Complete();
                                }
                                else
                                {
                                    SendCommunicationLog(communicationLogId, tenant, cl, communicationLogRep);
                                    queueservice.Complete();

                                    LogDoneItemInMemory();

                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                    }                    
                }
                else
                {
                    Thread.Sleep(60000);

                }

            }
        }

    

        private  ServiceContext getServiceContext(String tenant)
        {

            Setting mySetting = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository mySettingRepository = new SettingRepository();
                mySetting = mySettingRepository.GetSingleSetting("1");

                scope.Complete();
            }

            AccountingSettingQuery query = new AccountingSettingQuery(int.Parse(tenant));
            AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(int.Parse(tenant));
            OAuthRequestValidator oauthValidator = new OAuthRequestValidator(entityPM.QBOAccessToken, entityPM.QBOAccessTokenSecret, mySetting.QBOConsumerKey, mySetting.QBOConsumerSecretKey);
            ServiceContext context = new ServiceContext(mySetting.QBOAppToken, entityPM.QBOrealMeID, IntuitServicesType.QBO, oauthValidator);



            return context;

        }


        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "QuickbooksOnline";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            queueName = "QBO";
            ConnectClient();
            //quickbooksonlinequeue = StorageAcountDetails.QueueClient.GetQueueReference("QBO");
            //quickbooksonlinequeue.CreateIfNotExists();
            //quickbooksonlinequeue.Clear();
            return base.OnStart();


        }
       
        public void ConnectClient()
        {
            try
            {

                queueservice = new DbQueueService(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }
        private void SetNextTryDateTime(CommunicationLog cl)
        {
            string newLog = null;
            DateTime date = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
            DateTime dateUtc = DateTime.UtcNow;
            switch (cl.Retries)
            {
                case 1:
                case 2:
                    {
                        cl.NextTryDateTime = date.AddSeconds(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(1);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 1));

                        break;
                    }
                case 3:
                case 4:
                    {
                        cl.NextTryDateTime = date.AddSeconds(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                        break;
                    }
                case 5:
                    {
                        cl.NextTryDateTime = date.AddMinutes(1);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
                        queueservice.Delay(new TimeSpan(0, 0, 1));
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        cl.NextTryDateTime = date.AddMinutes(2);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
                        queueservice.Delay(new TimeSpan(0, 0, 2));
                        break;
                    }
                case 9:
                    {
                        cl.NextTryDateTime = date.AddMinutes(5);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
                        queueservice.Delay(new TimeSpan(0, 0, 5));
                        break;
                    }
                case 10:
                    {
                        cl.NextTryDateTime = date.AddMinutes(10);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
                        queueservice.Delay(new TimeSpan(0, 0, 10));
                        break;
                    }
                default:
                    {
                        cl.NextTryDateTime = date.AddMinutes(20);
                        cl.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
                        queueservice.Delay(new TimeSpan(0, 0, 20));
                        break;
                    }
            }
            cl.Logs += Environment.NewLine + "Retry #" + cl.Retries + " Next Retry: " + cl.NextTryDateTimeUTC.ToString();
        }

        public void SendWaitingCommunicationLog(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, int tenant)
        {
            try {
            CommunicationAttachmentRepository communicationAttachmentRep = new CommunicationAttachmentRepository(waitingCommLog.Tenant);

            List<CommunicationAttachment> attachmentsList = communicationAttachmentRep.GetCommunicationAttachmentsForCommLog(waitingCommLog.Id, waitingCommLog.Tenant).ToList();

            string xmlfile = "";
            if (waitingCommLog != null)
            {
                byte[] datainByte = null;
                string filename;
                if (waitingCommLog.Document != null)
                {
                    filename = waitingCommLog.DocumentId + "." + waitingCommLog.Document.Extension;

                    CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(waitingCommLog.Tenant);
                    var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, waitingCommLog.Document.Folder));

                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = waitingCommLog.Document.Id,
                        FolderName = waitingCommLog.Document.Folder,
                        Extension = waitingCommLog.Document.Extension,
                        Tenant = waitingCommLog.Document.Tenant,
                        FileSize = waitingCommLog.Document.FileSize,
                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    datainByte = storageservice.Read(fileInfo);
                    if (datainByte != null && waitingCommLog.CommunicationLogTypeCode == "T")
                    {
                        Encoding encoding = Encoding.UTF8;
                        xmlfile = encoding.GetString(datainByte);

                    }


                }


                switch (waitingCommLog.CommunicationLogTypeCode)
                {
                    case "T":
                        {
                            if (!string.IsNullOrEmpty(xmlfile))
                            {
                                if (waitingCommLog.InOut == "O")
                                {
                                    string myTarget = "";
                                    if (!string.IsNullOrEmpty(waitingCommLog.To))
                                    {
                                        myTarget = waitingCommLog.To;
                                    }

                                    switch (myTarget.ToUpper())
                                    {
                                        case "QBO":
                                            {
                                                SendCommunicationLogToQuickBooksOnline(waitingCommLog, datainByte, communicationLogRep,tenant);
                                                break;
                                            }

                                        default:
                                            {
                                                throw new Exception("Sending to unknow!");
                                            }
                                    }
                                }
                            }

                            break;
                        }
                }
            }
            }

            catch (Exception exc)
            {

                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                //Change number of retries
                if (waitingCommLog.Retries >= 5)
                {
                    this.StopCommunicationLog(exc, waitingCommLog, communicationLogRep);

                }
                else
                {
                    waitingCommLog.Retries++;
                    waitingCommLog.ExceptionMessage = exc.Message;
                    if (exc.InnerException != null)
                    {
                        waitingCommLog.ExceptionMessage = waitingCommLog.ExceptionMessage + Environment.NewLine + exc.InnerException;
                    }
                    if (exc.StackTrace != null)
                    {
                        waitingCommLog.ExceptionMessage = waitingCommLog.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                    }
                    SetNextTryDateTime(waitingCommLog);
                    if (Commoncontext != null)
                    {
                        communicationLogRep.Update(waitingCommLog);
                        communicationLogRep.SubmitChanges();
                    }
                }
                
                }            
            
        }

        private void StopCommunicationLog(Exception exc,CommunicationLog cl, CommunicationLogRepository communicationLogRep)
        {
            cl.CommunicationStatusTypeCode = "F";
            cl.DoneDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
            cl.DoneDateUTC = DateTime.UtcNow;
            cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
            cl.LastStatusDateUTC = DateTime.UtcNow;
            cl.ExceptionMessage = exc.Message;
            communicationLogRep.Update(cl);
            communicationLogRep.SubmitChanges();
            queueservice.CompleteAsFailed();

        }
        private void SendCommunicationLog(string communicationLogId, int tenant, CommunicationLog cl, CommunicationLogRepository communicationLogRep)
        {


            try
            {
                if (cl.Retries < 5)
                {
                    SendWaitingCommunicationLog(cl, communicationLogRep, tenant);
                }

                else
                {
                    if (Commoncontext != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(cl.ExceptionMessage);
                        sb.AppendLine("Failed To Send , Check your Translations");
                        SendingFail(cl, tenant, sb.ToString(), null, APInvoiceId);
                    }

                }
            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                //Change number of retries
                if (cl.Retries >= 5)
                {
                    this.StopCommunicationLog(exc, cl, communicationLogRep);
                    
                }
                else
                {
                    cl.Retries++;
                    cl.ExceptionMessage = exc.Message;
                    if (exc.InnerException != null)
                    {
                        cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + exc.InnerException;
                    }
                    if (exc.StackTrace != null)
                    {
                        cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                    }
                    SetNextTryDateTime(cl);
                    if (Commoncontext != null)
                    {
                        communicationLogRep.Update(cl);
                        communicationLogRep.SubmitChanges();
                    }
                }
            }
        }

        private void SendingFail(CommunicationLog waitingCommLog, int tenant,string message,string QBOId,string Id) {

         

            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(Commoncontext);
            waitingCommLog.CommunicationStatusTypeCode = "F";
            waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
            waitingCommLog.DoneDateUTC = DateTime.UtcNow;
            waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
            waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
            waitingCommLog.ExceptionMessage = message;
            commLogrepository.Update(waitingCommLog);
            commLogrepository.SubmitChanges();

            if (type == "APInvoice" || type=="VendorCredit")
            {
                APInvoiceRepository repository = new APInvoiceRepository(tenant);
                APInvoice invoice = repository.GetSingleAPInvoice(Id,tenant);
                invoice.TransferError = null;
                invoice.TransferStatusCode = "ET";
                invoice.IsTransferStarted = false;
                if (QBOId != null)
                    invoice.ExternalAccountingEntityId = QBOId;
                repository.Update(invoice);
                repository.SubmitChanges();
            }
            else if (type == "ARPayment" || type == "ARPaymentVoid")
            {
                ARPaymentRepository repository = new ARPaymentRepository(tenant);
                ARPayment payment = repository.GetSingleARPayment( waitingCommLog.EntityId,tenant);
                payment.TransferError = null;
                payment.TransferStatusCode = "ET";
                payment.IsTransferStarted = false;
                if (QBOId != null)
                    payment.ExternalAccountingEntityId = QBOId;
                repository.Update(payment);
                repository.SubmitChanges();
            }

            else if (type == "APPayment")
            {
                APPaymentRepository repository = new APPaymentRepository(tenant);
                APPayment payment = repository.GetSingleAPPayment(waitingCommLog.EntityId, tenant);
                payment.TransferError = null;
                payment.TransferStatusCode = "ET";
                if (QBOId != null)
                    payment.ExternalAccountingEntityId = QBOId;
                repository.Update(payment);
                repository.SubmitChanges();
            }
            else
            {
                ARInvoiceRepository repository = new ARInvoiceRepository(tenant);
                ARInvoice invoice = repository.GetARInvoiceByInvoiceNumber(tenant, waitingCommLog.EntityReference);
                invoice.TransferError = null;
                invoice.TransferStatusCode = "ET";
                invoice.IsTransferStarted = false;
                if (QBOId != null)
                    invoice.ExternalAccountingEntityId = QBOId;
                repository.Update(invoice);
                repository.SubmitChanges();
            }

            queueservice.Complete();
            QBOIDSuccess = null;



        }


        private void ARPaymentUpdate(string ARPaymentId,Payment payment, CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, int tenant, DataService service)
        {
            ServiceContext serviceContext = getServiceContext(tenant + "");
            QueryService<Payment> ARPaymentQueryService = new QueryService<Payment>(serviceContext);
            List<Payment> myResult = ARPaymentQueryService.ExecuteIdsQuery("Select * from payment where Id='" + ARPaymentId + "'").ToList();
            if (myResult.Count > 0)
            {
                myResult[0].Line = payment.Line;
                myResult[0].PaymentMethodRef = payment.PaymentMethodRef;
                Payment final = service.Update(myResult[0]) as Payment;
                SendingSuccessfully(waitingCommLog, tenant, final.Id,null,null);                
            }
            else
            {
                throw new Exception("Failed to Send");
            }

        }


        private void APPaymentUpdate(string APPaymentId, BillPayment payment, CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, int tenant, DataService service)
        {
            ServiceContext serviceContext = getServiceContext(tenant + "");
            QueryService<BillPayment> APPaymentQueryService = new QueryService<BillPayment>(serviceContext);
            List<BillPayment> myResult = APPaymentQueryService.ExecuteIdsQuery("Select * from BillPayment where Id='" + APPaymentId + "'").ToList();
            if (myResult.Count > 0)
            {
                myResult[0].AnyIntuitObject = payment.AnyIntuitObject;
                myResult[0].Line = payment.Line==null ? new List<Line>().ToArray():payment.Line;
                BillPayment final = service.Update(myResult[0]) as BillPayment;
                SendingSuccessfully(waitingCommLog, tenant, final.Id, null,null);
            }
            else
            {
                throw new Exception("Failed to Send");
            }

        }



        private void ARInvoiceVoid(string ARInvoiceId, CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, int tenant, DataService service)
        {
            ServiceContext serviceContext = getServiceContext(tenant + "");
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            QueryService<Invoice> ARInvoiceQueryService = new QueryService<Invoice>(serviceContext);
            List<Invoice> myResult = ARInvoiceQueryService.ExecuteIdsQuery("Select * from Invoice where Id='" + ARInvoiceId + "'").ToList();
            if (myResult.Count > 0)
            {
                Invoice final = service.Void(myResult[0]) as Invoice;
                if(final==null)
                    throw new Exception("Failed to Void");

                SendingSuccessfully(waitingCommLog, tenant, final.Id, null,null);

            }
            else
            {
                SendingFail(waitingCommLog, tenant, "This Invoice doesn't exist on quickbooks online to be voided .", null,null);

            }

        }

        private void ARPaymentVoid(string ARPaymentId, CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, int tenant, DataService service)
        {
            ServiceContext serviceContext = getServiceContext(tenant + "");
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            QueryService<Payment> ARInvoiceQueryService = new QueryService<Payment>(serviceContext);
            List<Payment> myResult = ARInvoiceQueryService.ExecuteIdsQuery("Select * from Payment where Id='" + ARPaymentId + "'").ToList();
            if (myResult.Count > 0)
            {
                Payment final = service.Void(myResult[0]) as Payment;
                if (final == null)
                    throw new Exception("Failed to Void");

                SendingSuccessfully(waitingCommLog, tenant, final.Id, null,null);

            }
            else
            {
                SendingFail(waitingCommLog, tenant, "This Invoice doesn't exist on quickbooks online to be voided .", null,null);

            }

        }




        private void SendCommunicationLogToQuickBooksOnline(CommunicationLog waitingCommLog, byte[] xmlfile, CommunicationLogRepository communicationLogRep,int tenant)
        {
            try
            {
                if (type == "Invoice")
                {
                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Invoice));
                    Invoice final = (Invoice)xmlSerializer.Deserialize(memorystream);
                    string ExternalTableIdCustomerCurrencyRef = "";
                    string ErrorMessage = "";
                    ARInvoiceHelper ARservice = new ARInvoiceHelper();
                    List<Intuit.Ipp.Data.Customer> customer = ARservice.GetQuickBooksOnlineCustomersByText("Select * from Customer where Id='" + final.CustomerRef.Value + "'", tenant + "");
                    if (customer.Count != 0)
                        ExternalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                    else
                    {
                        ErrorMessage = "The Customer Reference doesn't exists in your Quickbooks online company ";

                    }
                    if(final.CurrencyRef!=null)
                    if (ExternalTableIdCustomerCurrencyRef != final.CurrencyRef.Value)
                    {
                        if(final.CurrencyRef.Value!=null)
                        ErrorMessage = "The Currencies are different on your Customer External Currency  and System External Currency ";
                    }                   
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        SendingFail(waitingCommLog, tenant, ErrorMessage,null,null);
                    }
                    else
                    {
                        ServiceContext serviceContext = getServiceContext(tenant + "");
                        DataService service = new DataService(serviceContext);
                        if (checkInvoiceExisitance(final.DocNumber, waitingCommLog, communicationLogRep, tenant))
                        {
                            Invoice Result = service.Add(final) as Invoice;
                            if (Result == null)
                                throw new Exception("Failed to Send");
                            else
                            {
                                SendingSuccessfully(waitingCommLog, tenant, Result.Id,null,null);
                            }
                        }
                        else
                        {
                            SendingSuccessfully(waitingCommLog, tenant,null,"The Invoice number already exists in your Quickbooks online invoices",null);
                        }
                    }
                }

                else if (type == "MEMO")
                {
                     
                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(CreditMemo));
                    CreditMemo final = (CreditMemo)xmlSerializer.Deserialize(memorystream);
                    string ExternalTableIdCustomerCurrencyRef = "";
                    string ErrorMessage = "";
                    ARInvoiceHelper APService = new ARInvoiceHelper();
                    List<Intuit.Ipp.Data.Customer> customer = APService.GetQuickBooksOnlineCustomersByText("Select * from Customer where Id='" + final.CustomerRef.Value + "'", tenant + "");
                    if (customer.Count != 0)
                        ExternalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                    else
                    {
                        ErrorMessage = "The Customer Reference doesn't exists in your Quickbooks online company ";

                    }
                    if (final.CurrencyRef != null)
                        if (ExternalTableIdCustomerCurrencyRef != final.CurrencyRef.Value)
                    {
                        if (final.CurrencyRef.Value != null)
                            ErrorMessage = "The Currencies are different on your Customer External Currency  and System External Currency ";
                    }                  
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        SendingFail(waitingCommLog, tenant, ErrorMessage,null,null);
                    }
                    else
                    {
                        ServiceContext serviceContext = getServiceContext(tenant + "");
                        DataService service = new DataService(serviceContext);
                        if (checkInvoiceExisitance(final.DocNumber, waitingCommLog, communicationLogRep, tenant))
                        {
                            CreditMemo Result = service.Add(final) as CreditMemo;
                            if (Result == null)
                                throw new Exception("Failed to Send");
                            else
                            {
                                SendingSuccessfully(waitingCommLog, tenant, Result.Id,null, null);
                            }
                        }
                        else
                        {
                            SendingSuccessfully(waitingCommLog, tenant,null, "The Credit Note number already exists in your Quickbooks online Credit Notes", null);
                        }
                    }



                }

                else if (type == "APInvoice")
                {

                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Bill));
                    Bill final = (Bill)xmlSerializer.Deserialize(memorystream);
                    string ExternalTableIdCustomerCurrencyRef = "";
                    string ErrorMessage = "";
                    ARInvoiceHelper  APService = new ARInvoiceHelper();
                    List<Intuit.Ipp.Data.Vendor> vendor = APService.GetQuickBooksOnlineVendorByText("Select * from Vendor where Id='" + final.VendorRef.Value + "'", tenant + "");
                    if (vendor.Count != 0)
                    {
                        if (vendor[0].CurrencyRef != null)
                            ExternalTableIdCustomerCurrencyRef = vendor[0].CurrencyRef.Value;
                    }
                    else
                    {
                        ErrorMessage = "The Vendor Reference doesn't exists in your Quickbooks online company ";

                    }
                    if (final.CurrencyRef != null)
                        if (ExternalTableIdCustomerCurrencyRef != final.CurrencyRef.Value)
                    {
                        if (final.CurrencyRef.Value != null)
                            ErrorMessage = "The Currencies are different on your Customer External Currency  and System External Currency ";
                    }                  
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        SendingFail(waitingCommLog, tenant, ErrorMessage,null, null);
                    }
                    else
                    {
                        ServiceContext serviceContext = getServiceContext(tenant + "");
                        DataService service = new DataService(serviceContext);
                        string Id = final.Id;
                        APInvoiceId = Id;
                        final.Id = null;
                        Bill Result;
                        if (!string.IsNullOrEmpty(final.domain))
                        {
                            QueryService<Bill> invoiceQueryService = new QueryService<Bill>(serviceContext);
                            List<Bill> myResult = invoiceQueryService.ExecuteIdsQuery("Select * from Bill where Id='" + final.domain + "'").ToList();
                            if (myResult.Count != 0)
                            {
                                SendingSuccessfully(waitingCommLog, tenant, final.domain, null, Id);
                            }
                            else
                            {
                                final.domain = null;
                                Result = service.Add(final) as Bill;
                                if (Result == null)
                                    SendingFail(waitingCommLog, tenant, "Failed to Send", null, Id);
                                else
                                {
                                    SendingSuccessfully(waitingCommLog, tenant, Result.Id, null, Id);
                                }
                            }
                        }
                        else
                        {
                            final.domain = null;
                            Result = service.Add(final) as Bill;
                            if (Result == null)
                                SendingFail(waitingCommLog, tenant, "Failed to Send", null, Id);
                            else
                            {
                                SendingSuccessfully(waitingCommLog, tenant, Result.Id, null, Id);
                            }


                        }
                    }



                }
                else if (type == "VendorCredit")
                {

                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(VendorCredit));
                    VendorCredit final = (VendorCredit)xmlSerializer.Deserialize(memorystream);
                    string ExternalTableIdCustomerCurrencyRef = "";
                    string ErrorMessage = "";
                    ARInvoiceHelper APService = new ARInvoiceHelper();
                    List<Intuit.Ipp.Data.Vendor> vendor = APService.GetQuickBooksOnlineVendorByText("Select * from Vendor where Id='" + final.VendorRef.Value + "'", tenant + "");
                    if (vendor.Count != 0)
                        ExternalTableIdCustomerCurrencyRef = vendor[0].CurrencyRef.Value;
                    else
                    {
                        ErrorMessage = "The Vendor Reference doesn't exists in your Quickbooks online company ";

                    }
                    if (final.CurrencyRef != null)
                        if (ExternalTableIdCustomerCurrencyRef != final.CurrencyRef.Value)
                        {
                            if (final.CurrencyRef.Value != null)
                                ErrorMessage = "The Currencies are different on your Customer External Currency  and System External Currency ";
                        }
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        SendingFail(waitingCommLog, tenant, ErrorMessage, null, null);
                    }
                    else
                    {
                        ServiceContext serviceContext = getServiceContext(tenant + "");
                        DataService service = new DataService(serviceContext);
                        string Id = final.Id;
                        final.Id = null;
                        VendorCredit Result = service.Add(final) as VendorCredit;
                        if (Result == null)
                            SendingFail(waitingCommLog, tenant, "Failed to Send", null, Id);
                        else
                        {
                            SendingSuccessfully(waitingCommLog, tenant, Result.Id, null, Id);
                        }
                    }



                }

                else if (type == "ARPayment")
                {
                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Payment));
                    Payment final = (Payment)xmlSerializer.Deserialize(memorystream);
                    string ExternalTableIdCustomerCurrencyRef = "";
                    string ErrorMessage = "";
                    ARPaymentHelper ARPService = new ARPaymentHelper();
                    List<Intuit.Ipp.Data.Customer> customer = ARPService.GetQuickBooksOnlineCustomersByText("Select * from Customer where Id='" + final.CustomerRef.Value + "'", tenant + "");
                    if (customer.Count != 0)
                        ExternalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                    else
                    {
                        ErrorMessage = "The Customer Reference doesn't exists in your Quickbooks online company ";

                    }
                    if (final.CurrencyRef != null)
                    if (ExternalTableIdCustomerCurrencyRef != final.CurrencyRef.Value)
                    {
                        if (final.CurrencyRef.Value != null)
                            ErrorMessage = "The Currencies are different on your Customer External Currency  and System External Currency ";
                    }
                    if (!String.IsNullOrEmpty(ErrorMessage))
                    {
                        SendingFail(waitingCommLog, tenant, ErrorMessage, null, null);
                    }
                    else
                    {
                        ServiceContext serviceContext = getServiceContext(tenant + "");
                        DataService service = new DataService(serviceContext);
                        if (final.Id != null)
                            ARPaymentUpdate(final.Id, final, waitingCommLog, communicationLogRep, tenant, service);
                        else
                        {
                            Payment Result = service.Add(final) as Payment;
                            if (Result == null)
                                throw new Exception("Failed to Send");
                            else
                            {
                                SendingSuccessfully(waitingCommLog, tenant, Result.Id,null, null);
                            }
                        }
                        
                    }
                }
            


            else if (type == "APPayment")
            {
                MemoryStream memorystream = new MemoryStream(xmlfile);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(BillPayment));
                BillPayment final = (BillPayment)xmlSerializer.Deserialize(memorystream);
                string ExternalTableIdCustomerCurrencyRef = "";
                string ErrorMessage = "";
                APPaymentHelper APPService = new APPaymentHelper();
                List<Intuit.Ipp.Data.Vendor> customer = APPService.GetQuickBooksOnlineVendorByText("Select * from Vendor where Id='" + final.VendorRef.Value + "'", tenant + "");
                if (customer.Count != 0)
                    ExternalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                else
                {
                    ErrorMessage = "The Vendor Reference doesn't exists in your Quickbooks online company ";

                }
                if (final.CurrencyRef != null)
                    if (ExternalTableIdCustomerCurrencyRef != final.CurrencyRef.Value)
                    {
                        if (final.CurrencyRef.Value != null)
                            ErrorMessage = "The Currencies are different on your Customer External Currency  and System External Currency ";
                    }
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    SendingFail(waitingCommLog, tenant, ErrorMessage, null, null);
                }
                else
                {
                    ServiceContext serviceContext = getServiceContext(tenant + "");
                    DataService service = new DataService(serviceContext);
                    if (final.Id != null)
                        APPaymentUpdate(final.Id, final, waitingCommLog, communicationLogRep, tenant, service);
                    else
                    {
                        BillPayment Result = service.Add(final) as BillPayment;
                        if (Result == null)
                            throw new Exception("Failed to Send");
                        else
                        {
                            SendingSuccessfully(waitingCommLog, tenant, Result.Id, null, null);
                        }
                    }

                }
            }

                else if (type == "ARInvoiceVoid")
                {
                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(string));
                    string externalId = (string)xmlSerializer.Deserialize(memorystream);
                        ServiceContext serviceContext = getServiceContext(tenant + "");
                    serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
                    DataService service = new DataService(serviceContext);
                            ARInvoiceVoid(externalId, waitingCommLog, communicationLogRep, tenant, service);
                       
                        

                    
                }


                else if (type == "ARPaymentVoid")
                {
                    MemoryStream memorystream = new MemoryStream(xmlfile);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(string));
                    string externalId = (string)xmlSerializer.Deserialize(memorystream);
                    ServiceContext serviceContext = getServiceContext(tenant + "");
                    serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
                    DataService service = new DataService(serviceContext);
                    ARPaymentVoid(externalId, waitingCommLog, communicationLogRep, tenant, service);




                }
            }
            catch (Exception exc)
            {

                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                //Change number of retries
                if (waitingCommLog.Retries >= 5)
                {
                    this.StopCommunicationLog(exc, waitingCommLog, communicationLogRep);

                }
                else
                {
                    waitingCommLog.Retries++;
                    waitingCommLog.ExceptionMessage = exc.Message;
                    if (exc.InnerException != null)
                    {
                        waitingCommLog.ExceptionMessage = waitingCommLog.ExceptionMessage + Environment.NewLine + exc.InnerException;
                    }
                    if (exc.StackTrace != null)
                    {
                        waitingCommLog.ExceptionMessage = waitingCommLog.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                    }
                    SetNextTryDateTime(waitingCommLog);
                    if (Commoncontext != null)
                    {
                        communicationLogRep.Update(waitingCommLog);
                        communicationLogRep.SubmitChanges();
                    }
                }
                
            }

        }

        private void SendingSuccessfully(CommunicationLog waitingCommLog,int tenant,string QBOId,string Exception,string Id){
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                if(String.IsNullOrEmpty(QBOId) && String.IsNullOrEmpty(QBOIDSuccess))

                {

                    throw new Exception("Failed to Send !");
                }

                CommunicationLogRepository commLogrepository = new CommunicationLogRepository(Commoncontext);
                waitingCommLog.CommunicationStatusTypeCode = "D";
                waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.DoneDateUTC = DateTime.UtcNow;
                waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
                waitingCommLog.ExceptionMessage = Exception;
                commLogrepository.Update(waitingCommLog);
                commLogrepository.SubmitChanges();

                if (type == "APInvoice" || type == "VendorCredit")
                {
                    APInvoiceRepository repository = new APInvoiceRepository(tenant);
                    APInvoice invoice = repository.GetSingleAPInvoice(Id, tenant);
                    bool WasErrorInTransfer = OldTransferStatusCode == "ET"?true:false;
                    invoice.TransferError = null;
                    invoice.TransferStatusCode = "TR";
                    invoice.IsTransferStarted = false;
                    if (QBOId != null)
                        invoice.ExternalAccountingEntityId = QBOId;

                    else if (QBOIDSuccess != null)
                        invoice.ExternalAccountingEntityId = QBOIDSuccess;

                    repository.Update(invoice);
                    repository.SubmitChanges();
                    if (WasErrorInTransfer)
                    {
                        APInvoicePaymentRepository aPInvoicePaymentRepository = new APInvoicePaymentRepository(tenant);

                        List<APPayment> aRPayments = aPInvoicePaymentRepository.GetAPInvoicePaymentTransferedByInvoiceId(waitingCommLog.EntityId, tenant).ToList();

                        APPaymentRepository paymentRepository = new APPaymentRepository(tenant);


                        foreach (APPayment payment in aRPayments)
                        {
                            APPaymentHelper service = new APPaymentHelper();
                            APPaymentQuery PaymentQuery = new APPaymentQuery(paymentRepository);
                            APPaymentPM paymentPM = PaymentQuery.GetSinglePM(payment.Id, tenant);
                            service.APPaymentQuickbooksValidating(paymentPM, true, false, payment, this.Invoicecontext, this.Commoncontext, false, true);
                        }

                    }



                }

                else if (type == "ARPayment" || type == "ARPaymentVoid")
                {
                    ARPaymentRepository repository = new ARPaymentRepository(tenant);
                    ARPayment payment = repository.GetSingleARPayment(waitingCommLog.EntityId, tenant);
                    payment.TransferError = null;
                    payment.TransferStatusCode = "TR";
                    payment.IsTransferStarted = false;
                    if (QBOId != null)
                        payment.ExternalAccountingEntityId = QBOId;
                    else if (QBOIDSuccess != null)
                        payment.ExternalAccountingEntityId = QBOIDSuccess;
                    repository.Update(payment);
                    repository.SubmitChanges();
                }

                else if (type == "APPayment")
                {
                    APPaymentRepository repository = new APPaymentRepository(tenant);
                    APPayment payment = repository.GetSingleAPPayment(waitingCommLog.EntityId, tenant);
                    payment.TransferError = null;
                    payment.TransferStatusCode = "TR";
                    if (QBOId != null)
                        payment.ExternalAccountingEntityId = QBOId;
                    else if (QBOIDSuccess != null)
                        payment.ExternalAccountingEntityId = QBOIDSuccess;
                    repository.Update(payment);
                    repository.SubmitChanges();
                }


                else
                {
                    ARInvoiceRepository repository = new ARInvoiceRepository(tenant);
                    ARInvoice invoice = repository.GetARInvoiceByInvoiceNumber(tenant, waitingCommLog.EntityReference);
                    invoice.TransferError = null;
                    bool WasErrorInTransfer = OldTransferStatusCode == "ET" ? true : false;
                    invoice.TransferStatusCode = "TR";
                    invoice.IsTransferStarted = false;
                    if (QBOId != null)
                        invoice.ExternalAccountingEntityId = QBOId;
                    else if (QBOIDSuccess != null)
                        invoice.ExternalAccountingEntityId = QBOIDSuccess;
                    
                    repository.Update(invoice);
                    repository.SubmitChanges();
                    if (WasErrorInTransfer)
                    {
                        ARInvoicePaymentRepository aRInvoicePaymentRepository = new ARInvoicePaymentRepository(tenant);

                        List<ARPayment> aRPayments = aRInvoicePaymentRepository.GetARInvoicePaymentTransferedByInvoiceId(waitingCommLog.EntityId, tenant).ToList();

                        ARPaymentRepository paymentRepository = new ARPaymentRepository(tenant);


                        foreach (ARPayment payment in aRPayments)
                        {
                            ARPaymentHelper service = new ARPaymentHelper();
                            ARPaymentQuery PaymentQuery = new ARPaymentQuery(paymentRepository);
                            ARPaymentPM paymentPM = PaymentQuery.GetSinglePM(payment.Id, tenant);
                            service.ARPaymentQuickbooksValidating(paymentPM, true, false, payment, this.Invoicecontext, this.Commoncontext, false, false, paymentPM.SetReSendQBO, true);
                        }
                    }

                }


                QBOIDSuccess = null;
                scope.Complete();
            }
          
        }


        private Boolean checkInvoiceExisitance(string invoiceNumber,CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, int tenant)
        {
            try
            {
                if (type == "Invoice")
                {
                    ServiceContext serviceContext = getServiceContext(tenant + "");
                    QueryService<Invoice> invoiceQueryService = new QueryService<Invoice>(serviceContext);
                    List<Invoice> myResult = invoiceQueryService.ExecuteIdsQuery("Select * from Invoice where DocNumber='" + invoiceNumber + "'").ToList();
                    if (myResult.Count != 0)
                    {
                        QBOIDSuccess = myResult[0].Id;
                        return false;

                    }
                    return true;
                }


                else if (type == "MEMO")
                {

                    ServiceContext serviceContext = getServiceContext(tenant + "");
                    QueryService<CreditMemo> invoiceQueryService = new QueryService<CreditMemo>(serviceContext);
                    List<CreditMemo> myResult = invoiceQueryService.ExecuteIdsQuery("Select * from CreditMemo where DocNumber='" + invoiceNumber + "'").ToList();
                    if (myResult.Count != 0)
                    {
                        QBOIDSuccess = myResult[0].Id;
                        return false;

                    }

                    return true;


                }

                else if (type == "APInvoice")
                {
                    ServiceContext serviceContext = getServiceContext(tenant + "");
                    QueryService<Bill> invoiceQueryService = new QueryService<Bill>(serviceContext);
                    List<Bill> myResult = invoiceQueryService.ExecuteIdsQuery("Select * from Bill where DocNumber='" + invoiceNumber + "'").ToList();
                    if (myResult.Count != 0)
                    {
                        QBOIDSuccess = myResult[0].Id;
                        return false;

                    }

                    return true;

                }

                else if (type == "VendorCredit")
                {
                    ServiceContext serviceContext = getServiceContext(tenant + "");
                    QueryService<VendorCredit> invoiceQueryService = new QueryService<VendorCredit>(serviceContext);
                    List<VendorCredit> myResult = invoiceQueryService.ExecuteIdsQuery("Select * from VendorCredit where DocNumber='" + invoiceNumber + "'").ToList();
                    if (myResult.Count != 0)
                    {
                        QBOIDSuccess = myResult[0].Id;
                        return false;

                    }

                    return true;

                }


                else
                    return true;

            }

            catch (Exception exc) {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

                //Change number of retries
                if (waitingCommLog.Retries >= 5)
                {
                    this.StopCommunicationLog(exc, waitingCommLog, communicationLogRep);

                }
                else
                {
                    waitingCommLog.Retries++;
                    waitingCommLog.ExceptionMessage = exc.Message;
                    if (exc.InnerException != null)
                    {
                        waitingCommLog.ExceptionMessage = waitingCommLog.ExceptionMessage + Environment.NewLine + exc.InnerException;
                    }
                    if (exc.StackTrace != null)
                    {
                        waitingCommLog.ExceptionMessage = waitingCommLog.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
                    }
                    SetNextTryDateTime(waitingCommLog);
                    if (Commoncontext != null)
                    {
                        communicationLogRep.Update(waitingCommLog);
                        communicationLogRep.SubmitChanges();
                    }
                }
                throw;
                }



        }




    }
}
