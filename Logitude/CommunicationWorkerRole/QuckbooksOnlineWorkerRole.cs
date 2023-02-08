using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;

namespace CommunicationWorkerRole
{
    class QuckbooksOnlineWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueservice;
        private ICommonDataContext commonContext;
        private IInvoiceContext invoiceContext;
        private ARInvoiceRepository arInvoiceRepository;
        private APInvoiceRepository apInvoiceRepository;
        private ARPaymentRepository arPaymentRepository;
        private APPaymentRepository apPaymentRepository;
        private ARInvoiceQuery arInvoiceQuery;
        private APInvoiceQuery apInvoiceQuery;        
        private ARPaymentQuery arPaymentQuery;
        private APPaymentQuery apPaymentQuery;
        private CommunicationLogRepository communicationLogRep;
        private CommunicationLog communicationLog;
        private string queueName;
        private string type;
        private string QBOIDSuccess = null;
        private string APInvoiceId = null;
        private string oldTransferStatusCode;
        private string creditNoteCode = "CD";
        private string invoiceTxnType = "Invoice";
        private string creditMemoTxnType = "CreditMemo";
        private string communicationLogId;
        private int tenant = 0;
        private byte[] dataInByte = null;
        private bool isConcurrencyToggleEnabled = false;
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "QuickbooksOnline";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            queueName = "QBO";
            ConnectClient();
            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0);
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = queueservice = new DbQueueService(queueName, tenant);
                        QueueResponse response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
                        LastActivity = DateTime.UtcNow;
                        if (response.MessageId != null)
                        {
                            this.ReadMessageProperties(response);
                            this.Initialize();
                            this.GetCommunicationLog();

                            if (communicationLog == null) return;

                            if (communicationLog.CommunicationStatusTypeCode == "T")
                                queueservice.Complete();

                            else
                            {
                                HandleCommunicationLog();
                                queueservice.Complete();
                                LogDoneItemInMemory();
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

        private void ReadMessageProperties(QueueResponse response)
        {
            communicationLogId = response.MessageValues["QuickbooksOnline"].ToString();
            type = response.MessageValues["type"].ToString();
            int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);

            if (!response.MessageValues.ContainsKey("OldTransferStatusCode"))
                oldTransferStatusCode = null;
            else
                oldTransferStatusCode = response.MessageValues["OldTransferStatusCode"];
        }
        private void Initialize()
        {
            isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", tenant);

            commonContext = CommonDataContext.GetContext(tenant);
            invoiceContext = InvoiceContext.GetContext(tenant);
            communicationLogRep = new CommunicationLogRepository(commonContext);

            arInvoiceRepository = new ARInvoiceRepository(invoiceContext);
            apInvoiceRepository = new APInvoiceRepository(invoiceContext);
            arPaymentRepository = new ARPaymentRepository(invoiceContext);
            apPaymentRepository = new APPaymentRepository(invoiceContext);

            arInvoiceQuery = new ARInvoiceQuery(arInvoiceRepository);
            apInvoiceQuery = new APInvoiceQuery(apInvoiceRepository);
            arPaymentQuery = new ARPaymentQuery(arPaymentRepository);
            apPaymentQuery = new APPaymentQuery(apPaymentRepository);
        }
        private void GetCommunicationLog()
        {
            communicationLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
        }
        private void HandleCommunicationLog()
        {
            try
            {
                if (communicationLog.Retries < 5)
                    SendWaitingCommunicationLog();

                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(communicationLog.ExceptionMessage);
                    sb.AppendLine("Failed To Send , Check your Translations");
                    SendingFail(sb.ToString(), null, APInvoiceId);
                }
            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);
                this.HandleException(exc);
            }
        }
        private void SendWaitingCommunicationLog()
        {
            try
            {
                string xmlfile = "";                
                string filename;
                if (communicationLog.Document != null)
                {
                    filename = communicationLog.DocumentId + "." + communicationLog.Document.Extension;
                    xmlfile = this.AddQBOFileToStorage();                    
                }

                switch (communicationLog.CommunicationLogTypeCode)
                {
                    case "T":
                        {
                            if (!string.IsNullOrEmpty(xmlfile) && communicationLog.InOut == "O")
                            {
                                string myTarget = "";
                                if (!string.IsNullOrEmpty(communicationLog.To))
                                {
                                    myTarget = communicationLog.To;
                                }

                                switch (myTarget.ToUpper())
                                {
                                    case "QBO":
                                        {
                                            SendCommunicationLogToQuickBooksOnline(dataInByte);
                                            break;
                                        }

                                    default:
                                        {
                                            throw new Exception("Sending to unknow!");
                                        }
                                }
                            }

                            break;
                        }
                }
            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);
                this.HandleException(exc);
            }
        }
        private string AddQBOFileToStorage()
        {
            string xmlfile = null;

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = communicationLog.Document.Id,
                FolderName = communicationLog.Document.Folder,
                Extension = communicationLog.Document.Extension,
                Tenant = communicationLog.Document.Tenant,
                FileSize = communicationLog.Document.FileSize,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            dataInByte = storageservice.Read(fileInfo);
            if (dataInByte != null && communicationLog.CommunicationLogTypeCode == "T")
            {
                Encoding encoding = Encoding.UTF8;
                xmlfile = encoding.GetString(dataInByte);
            }

            return xmlfile;
        }
        private void SendCommunicationLogToQuickBooksOnline(byte[] xmlfile)
        {
            try
            {
                if (type == "Invoice")
                    this.SendARInvoice(xmlfile);

                else if (type == "MEMO")
                    this.SendCreditMemo(xmlfile);

                else if (type == "APInvoice")
                    this.SendAPInvoice(xmlfile);

                else if (type == "VendorCredit")
                    this.SendVendorCredit(xmlfile);

                else if (type == "ARPayment")
                    this.SendARPayment(xmlfile);

                else if (type == "APPayment")
                    this.SendAPPayment(xmlfile);

                else if (type == "ARInvoiceVoid")
                    this.SendARInvoiceVoid(xmlfile);

                else if (type == "ARPaymentVoid")
                    this.SendARPaymentVoid(xmlfile);
            }

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);
                this.HandleException(exc);                
            }
        }
        private void SendARInvoice(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Invoice));
            Invoice final = (Invoice)xmlSerializer.Deserialize(memorystream);
            string externalTableIdCustomerCurrencyRef = "";
            string errorMessage = "";
            ARInvoiceHelper ARservice = new ARInvoiceHelper();
            List<Intuit.Ipp.Data.Customer> customer = ARservice.GetQuickBooksOnlineCustomersByText("Select * from Customer where Id='" + final.CustomerRef.Value + "'", tenant + "");

            if (customer.Count != 0)
            {
                externalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                errorMessage = this.ValidateCurrencyRef(final.CurrencyRef, externalTableIdCustomerCurrencyRef);
            }

            else
                errorMessage = "The Customer Reference doesn't exists in your Quickbooks online company ";
             
            if (!string.IsNullOrEmpty(errorMessage))
            {
                SendingFail(errorMessage, null, null);
            }
            else
            {
                ServiceContext serviceContext = GetServiceContext();
                DataService service = new DataService(serviceContext);
                if (CheckInvoiceExisitance(final.DocNumber))
                {
                    Invoice Result = service.Add(final) as Invoice;
                    if (Result == null)
                        throw new Exception("Failed to Send");
                    else
                    {
                        SendingSuccessfully(Result.Id, null, null);
                        SendPaymentForCloseTheCreditMemoWithIncoive(Result);
                    }
                }
                else
                {
                    SendingSuccessfully(null, "The Invoice number already exists in your Quickbooks online invoices", null);
                }
            }
        }
        private void SendCreditMemo(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CreditMemo));
            CreditMemo final = (CreditMemo)xmlSerializer.Deserialize(memorystream);
            string externalTableIdCustomerCurrencyRef = "";
            string errorMessage = "";
            ARInvoiceHelper APService = new ARInvoiceHelper();
            List<Intuit.Ipp.Data.Customer> customer = APService.GetQuickBooksOnlineCustomersByText("Select * from Customer where Id='" + final.CustomerRef.Value + "'", tenant + "");

            if (customer.Count != 0)
            {
                externalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                errorMessage = this.ValidateCurrencyRef(final.CurrencyRef, externalTableIdCustomerCurrencyRef);
            }

            else
                errorMessage = "The Customer Reference doesn't exists in your Quickbooks online company ";
                        
            if (!string.IsNullOrEmpty(errorMessage))
            {
                SendingFail(errorMessage, null, null);
            }
            else
            {
                ServiceContext serviceContext = GetServiceContext();
                DataService service = new DataService(serviceContext);
                if (CheckInvoiceExisitance(final.DocNumber))
                {
                    CreditMemo Result = service.Add(final) as CreditMemo;
                    if (Result == null)
                        throw new Exception("Failed to Send");
                    else
                    {
                        SendingSuccessfully(Result.Id, null, null);
                        SendPaymentForCloseTheIncoiveWithCreditMemo( Result);
                    }
                }
                else
                {
                    SendingSuccessfully(null, "The Credit Note number already exists in your Quickbooks online Credit Notes", null);
                }
            }
        }
        private void SendAPInvoice(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Bill));
            Bill final = (Bill)xmlSerializer.Deserialize(memorystream);
            string externalTableIdCustomerCurrencyRef = "";
            string errorMessage = "";
            ARInvoiceHelper APService = new ARInvoiceHelper();
            List<Intuit.Ipp.Data.Vendor> vendor = APService.GetQuickBooksOnlineVendorByText("Select * from Vendor where Id='" + final.VendorRef.Value + "'", tenant + "");
            if (vendor.Count != 0)
            {
                if (vendor[0].CurrencyRef != null)
                {
                    externalTableIdCustomerCurrencyRef = vendor[0].CurrencyRef.Value;
                    errorMessage = this.ValidateCurrencyRef(final.CurrencyRef, externalTableIdCustomerCurrencyRef);
                }
            }
            else            
                errorMessage = "The Vendor Reference doesn't exists in your Quickbooks online company ";

            if (!string.IsNullOrEmpty(errorMessage))
            {
                SendingFail(errorMessage, null, null);
            }
            else
            {
                ServiceContext serviceContext = GetServiceContext();
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
                        SendingSuccessfully(final.domain, null, Id);
                    }
                    else
                    {
                        final.domain = null;
                        Result = service.Add(final) as Bill;
                        if (Result == null)
                            SendingFail("Failed to Send", null, Id);
                        else
                        {
                            SendingSuccessfully(Result.Id, null, Id);
                        }
                    }
                }
                else
                {
                    final.domain = null;
                    Result = service.Add(final) as Bill;
                    if (Result == null)
                        SendingFail("Failed to Send", null, Id);
                    else                    
                        SendingSuccessfully(Result.Id, null, Id);                    
                }
            }
        }
        private void SendVendorCredit(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VendorCredit));
            VendorCredit final = (VendorCredit)xmlSerializer.Deserialize(memorystream);
            string externalTableIdCustomerCurrencyRef = "";
            string errorMessage = "";
            ARInvoiceHelper APService = new ARInvoiceHelper();
            List<Intuit.Ipp.Data.Vendor> vendor = APService.GetQuickBooksOnlineVendorByText("Select * from Vendor where Id='" + final.VendorRef.Value + "'", tenant + "");

            if (vendor.Count != 0)
            {
                externalTableIdCustomerCurrencyRef = vendor[0].CurrencyRef.Value;
                errorMessage = this.ValidateCurrencyRef(final.CurrencyRef, externalTableIdCustomerCurrencyRef);
            }

            else
                errorMessage = "The Vendor Reference doesn't exists in your Quickbooks online company ";

            if (!string.IsNullOrEmpty(errorMessage))
            {
                SendingFail(errorMessage, null, null);
            }
            else
            {
                ServiceContext serviceContext = GetServiceContext();
                DataService service = new DataService(serviceContext);
                string Id = final.Id;
                final.Id = null;
                VendorCredit Result = service.Add(final) as VendorCredit;
                if (Result == null)
                    SendingFail("Failed to Send", null, Id);
                else
                {
                    SendingSuccessfully(Result.Id, null, Id);
                }
            }
        }
        private void SendARPayment(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Payment));
            Payment final = (Payment)xmlSerializer.Deserialize(memorystream);
            string externalTableIdCustomerCurrencyRef = "";
            string errorMessage = "";
            ARPaymentHelper ARPService = new ARPaymentHelper();
            List<Intuit.Ipp.Data.Customer> customer = ARPService.GetQuickBooksOnlineCustomersByText("Select * from Customer where Id='" + final.CustomerRef.Value + "'", tenant + "");

            if (customer.Count != 0)
            {
                externalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                errorMessage = this.ValidateCurrencyRef(final.CurrencyRef, externalTableIdCustomerCurrencyRef);
            }

            else
                errorMessage = "The Customer Reference doesn't exists in your Quickbooks online company ";
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                SendingFail(errorMessage, null, null);
            }
            else
            {
                ServiceContext serviceContext = GetServiceContext();
                DataService service = new DataService(serviceContext);
                if (final.Id != null)
                    ARPaymentUpdate(final.Id, final, service);
                else
                {
                    Payment Result = service.Add(final) as Payment;
                    if (Result == null)
                        throw new Exception("Failed to Send");
                    else
                    {
                        SendingSuccessfully(Result.Id, null, null);
                    }
                }
            }
        }
        private void SendAPPayment(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BillPayment));
            BillPayment final = (BillPayment)xmlSerializer.Deserialize(memorystream);
            string externalTableIdCustomerCurrencyRef = "";
            string errorMessage = "";
            APPaymentHelper APPService = new APPaymentHelper();
            List<Intuit.Ipp.Data.Vendor> customer = APPService.GetQuickBooksOnlineVendorByText("Select * from Vendor where Id='" + final.VendorRef.Value + "'", tenant + "");

            if (customer.Count != 0)
            {
                externalTableIdCustomerCurrencyRef = customer[0].CurrencyRef.Value;
                errorMessage = this.ValidateCurrencyRef(final.CurrencyRef, externalTableIdCustomerCurrencyRef);
            }

            else
                errorMessage = "The Vendor Reference doesn't exists in your Quickbooks online company ";            

            if (!string.IsNullOrEmpty(errorMessage))
            {
                SendingFail(errorMessage, null, null);
            }
            else
            {
                ServiceContext serviceContext = GetServiceContext();
                DataService service = new DataService(serviceContext);
                if (final.Id != null)
                    APPaymentUpdate(final.Id, final, service);
                else
                {
                    BillPayment Result = service.Add(final) as BillPayment;
                    if (Result == null)
                        throw new Exception("Failed to Send");
                    else
                    {
                        SendingSuccessfully(Result.Id, null, null);
                    }
                }
            }
        }
        private void SendARInvoiceVoid(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(string));
            string externalId = (string)xmlSerializer.Deserialize(memorystream);
            ServiceContext serviceContext = GetServiceContext();
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            DataService service = new DataService(serviceContext);
            ARInvoiceVoid(externalId, service);
        }
        private void SendARPaymentVoid(byte[] xmlfile)
        {
            MemoryStream memorystream = new MemoryStream(xmlfile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(string));
            string externalId = (string)xmlSerializer.Deserialize(memorystream);
            ServiceContext serviceContext = GetServiceContext();
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            DataService service = new DataService(serviceContext);
            ARPaymentVoid(externalId, service);
        }
        private string ValidateCurrencyRef(ReferenceType currencyRef, string externalTableIdCustomerCurrencyRef)
        {
            if (currencyRef == null) return null;
            if (currencyRef.Value == null) return null;

            if (externalTableIdCustomerCurrencyRef != currencyRef.Value)
                return "The Currencies are different on your Customer External Currency  and System External Currency ";

            return null;
        }

        // // // // // 
        private void SetNextTryDateTime()
        {
            DateTime date = TenantServerConfigration.GetCurrentDateTime(communicationLog.Tenant);
            DateTime dateUtc = DateTime.UtcNow;
            switch (communicationLog.Retries)
            {
                case 1:
                case 2:
                    {
                        communicationLog.NextTryDateTime = date.AddSeconds(1);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddSeconds(1);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 1));

                        break;
                    }
                case 3:
                case 4:
                    {
                        communicationLog.NextTryDateTime = date.AddSeconds(5);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
                        queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                        break;
                    }
                case 5:
                    {
                        communicationLog.NextTryDateTime = date.AddMinutes(1);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
                        queueservice.Delay(new TimeSpan(0, 0, 1));
                        break;
                    }
                case 6:
                case 7:
                case 8:
                    {
                        communicationLog.NextTryDateTime = date.AddMinutes(2);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
                        queueservice.Delay(new TimeSpan(0, 0, 2));
                        break;
                    }
                case 9:
                    {
                        communicationLog.NextTryDateTime = date.AddMinutes(5);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
                        queueservice.Delay(new TimeSpan(0, 0, 5));
                        break;
                    }
                case 10:
                    {
                        communicationLog.NextTryDateTime = date.AddMinutes(10);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
                        queueservice.Delay(new TimeSpan(0, 0, 10));
                        break;
                    }
                default:
                    {
                        communicationLog.NextTryDateTime = date.AddMinutes(20);
                        communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
                        queueservice.Delay(new TimeSpan(0, 0, 20));
                        break;
                    }
            }

            communicationLog.Logs += Environment.NewLine + "Retry #" + communicationLog.Retries + " Next Retry: " + communicationLog.NextTryDateTimeUTC.ToString();
        }
        private void HandleException(Exception exc)
        {
            if (communicationLog.Retries >= 5)
            {
                this.StopCommunicationLog(exc);
            }

            else
            {
                communicationLog.Retries++;
                communicationLog.ExceptionMessage = exc.Message;

                if (exc.InnerException != null)
                    communicationLog.ExceptionMessage = communicationLog.ExceptionMessage + Environment.NewLine + exc.InnerException;

                if (exc.StackTrace != null)
                    communicationLog.ExceptionMessage = communicationLog.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;

                SetNextTryDateTime();
                communicationLogRep.Update(communicationLog);
                communicationLogRep.SubmitChanges();
            }
        }
        private void StopCommunicationLog(Exception exc)
        {
            communicationLog.CommunicationStatusTypeCode = "F";
            communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            communicationLog.DoneDateUTC = DateTime.UtcNow;
            communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            communicationLog.LastStatusDateUTC = DateTime.UtcNow;
            communicationLog.ExceptionMessage = exc.Message;
            communicationLogRep.Update(communicationLog);
            communicationLogRep.SubmitChanges();
            queueservice.CompleteAsFailed();
        }
        private void SendingFail(string message, string QBOId, string Id)
        {
            CommunicationLogRepository commLogrepository = new CommunicationLogRepository(commonContext);
            communicationLog.CommunicationStatusTypeCode = "F";
            communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            communicationLog.DoneDateUTC = DateTime.UtcNow;
            communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            communicationLog.LastStatusDateUTC = DateTime.UtcNow;
            communicationLog.ExceptionMessage = message;
            commLogrepository.Update(communicationLog);
            commLogrepository.SubmitChanges();

            if (type == "APInvoice" || type == "VendorCredit")            
                this.SaveEntity("APInvoice", QBOId, false);               
            
            else if (type == "ARPayment" || type == "ARPaymentVoid")
                this.SaveEntity("ARPayment", QBOId, false); 

            else if (type == "APPayment")
                this.SaveEntity("APPayment", QBOId, false);    

            else
                this.SaveEntity("ARInvoice", QBOId, false);   

            queueservice.Complete();
            QBOIDSuccess = null;
        }
        private void SendingSuccessfully(string QBOId, string Exception, string Id)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                if (string.IsNullOrEmpty(QBOId) && string.IsNullOrEmpty(QBOIDSuccess))
                {
                    throw new Exception("Failed to Send !");
                }

                CommunicationLogRepository commLogrepository = new CommunicationLogRepository(commonContext);
                communicationLog.CommunicationStatusTypeCode = "D";
                communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                communicationLog.DoneDateUTC = DateTime.UtcNow;
                communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                communicationLog.LastStatusDateUTC = DateTime.UtcNow;
                communicationLog.ExceptionMessage = Exception;
                commLogrepository.Update(communicationLog);
                commLogrepository.SubmitChanges();

                if (type == "APInvoice" || type == "VendorCredit")
                {
                    bool WasErrorInTransfer = oldTransferStatusCode == "ET" ? true : false;
                    this.SaveEntity("APInvoice", QBOId, true);

                    if (WasErrorInTransfer)
                    {
                        APInvoicePaymentRepository aPInvoicePaymentRepository = new APInvoicePaymentRepository(tenant);
                        List<APPayment> aRPayments = aPInvoicePaymentRepository.GetAPInvoicePaymentTransferedByInvoiceId(communicationLog.EntityId, tenant).ToList();
                        APPaymentRepository paymentRepository = new APPaymentRepository(tenant);

                        foreach (APPayment payment in aRPayments)
                        {
                            APPaymentHelper service = new APPaymentHelper();
                            APPaymentQuery PaymentQuery = new APPaymentQuery(paymentRepository);
                            APPaymentPM paymentPM = PaymentQuery.GetSinglePM(payment.Id, tenant);
                            service.APPaymentQuickbooksValidating(paymentPM, true, false, payment, invoiceContext, commonContext, false, true);
                        }
                    }

                    this.HandelSendAPInvoiceAttachments(Id);
                }

                else if (type == "ARPayment" || type == "ARPaymentVoid")
                {
                    this.SaveEntity("ARPayment", QBOId, true);                    
                }

                else if (type == "APPayment")
                {
                    this.SaveEntity("APPayment", QBOId, true);                    
                }

                else
                {
                    bool WasErrorInTransfer = oldTransferStatusCode == "ET" ? true : false;
                    this.SaveEntity("ARInvoice", QBOId, true);

                    if (WasErrorInTransfer)
                    {
                        ARInvoicePaymentRepository aRInvoicePaymentRepository = new ARInvoicePaymentRepository(tenant);
                        List<ARPayment> aRPayments = aRInvoicePaymentRepository.GetARInvoicePaymentTransferedByInvoiceId(communicationLog.EntityId, tenant).ToList();
                        ARPaymentRepository paymentRepository = new ARPaymentRepository(tenant);

                        foreach (ARPayment payment in aRPayments)
                        {
                            ARPaymentHelper service = new ARPaymentHelper();
                            ARPaymentQuery PaymentQuery = new ARPaymentQuery(paymentRepository);
                            ARPaymentPM paymentPM = PaymentQuery.GetSinglePM(payment.Id, tenant);
                            service.ARPaymentQuickbooksValidating(paymentPM, true, false, payment, invoiceContext, commonContext, false, false, paymentPM.SetReSendQBO, true, true);
                        }
                    }
                }

                QBOIDSuccess = null;
                scope.Complete();
            }
        }
        // // // // // 

        private void ARPaymentUpdate(string ARPaymentId, Payment payment, DataService service)
        {
            ServiceContext serviceContext = GetServiceContext();
            QueryService<Payment> ARPaymentQueryService = new QueryService<Payment>(serviceContext);
            List<Payment> myResult = ARPaymentQueryService.ExecuteIdsQuery("Select * from payment where Id='" + ARPaymentId + "'").ToList();
            if (myResult.Count > 0)
            {
                myResult[0].Line = payment.Line;
                myResult[0].PaymentMethodRef = payment.PaymentMethodRef;
                Payment final = service.Update(myResult[0]) as Payment;
                SendingSuccessfully(final.Id, null, null);
            }

            else
            {
                throw new Exception("Failed to Send");
            }
        }
        private void APPaymentUpdate(string APPaymentId, BillPayment payment, DataService service)
        {
            ServiceContext serviceContext = GetServiceContext();
            QueryService<BillPayment> APPaymentQueryService = new QueryService<BillPayment>(serviceContext);
            List<BillPayment> myResult = APPaymentQueryService.ExecuteIdsQuery("Select * from BillPayment where Id='" + APPaymentId + "'").ToList();

            if (myResult.Count > 0)
            {
                myResult[0].AnyIntuitObject = payment.AnyIntuitObject;
                myResult[0].Line = payment.Line == null ? new List<Line>().ToArray() : payment.Line;
                BillPayment final = service.Update(myResult[0]) as BillPayment;
                SendingSuccessfully(final.Id, null, null);
            }

            else
            {
                throw new Exception("Failed to Send");
            }
        }
        private void ARInvoiceVoid(string ARInvoiceId, DataService service)
        {
            ServiceContext serviceContext = GetServiceContext();
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            QueryService<Invoice> ARInvoiceQueryService = new QueryService<Invoice>(serviceContext);
            List<Invoice> myResult = ARInvoiceQueryService.ExecuteIdsQuery("Select * from Invoice where Id='" + ARInvoiceId + "'").ToList();
            if (myResult.Count > 0)
            {
                Invoice final = service.Void(myResult[0]) as Invoice;

                if (final == null)
                    throw new Exception("Failed to Void");

                SendingSuccessfully(final.Id, null, null);
            }

            else
            {
                SendingFail("This Invoice doesn't exist on quickbooks online to be voided .", null, null);
            }
        }
        private void ARPaymentVoid(string ARPaymentId, DataService service)
        {
            ServiceContext serviceContext = GetServiceContext();
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            QueryService<Payment> ARInvoiceQueryService = new QueryService<Payment>(serviceContext);
            List<Payment> myResult = ARInvoiceQueryService.ExecuteIdsQuery("Select * from Payment where Id='" + ARPaymentId + "'").ToList();
            if (myResult.Count > 0)
            {
                Payment final = service.Void(myResult[0]) as Payment;
                if (final == null)
                    throw new Exception("Failed to Void");

                SendingSuccessfully(final.Id, null, null);
            }

            else
            {
                SendingFail("This Invoice doesn't exist on quickbooks online to be voided .", null, null);
            }
        }

        // // // // //
        private void SaveEntity(string type, string QBOId, bool isSuccess)
        {
            if (isConcurrencyToggleEnabled)
                this.SaveEntity_UpdateService(type, QBOId, isSuccess);

            else
                this.SaveEntity_Repository(type, QBOId, isSuccess);
        }
        private void SaveEntity_UpdateService(string type, string QBOId, bool isSuccess)
        {
            switch (type)
            {
                case "ARInvoice":
                    {
                        ARInvoicePM invoice = arInvoiceQuery.GetSingleInvoiceByInvoiceNumber(communicationLog.EntityReference, tenant);
                        if (invoice != null)
                        {
                            if (isSuccess)
                            {
                                invoice.IsUpdatedByQBO = true;
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "TR";
                                invoice.IsTransferStarted = false;
                                invoice.IsTransferStatusSetManually = true;

                                if (QBOId != null)
                                    invoice.ExternalAccountingEntityId = QBOId;
                                else if (QBOIDSuccess != null)
                                    invoice.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                invoice.IsUpdatedByQBO = true;
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "ET";
                                invoice.IsTransferStarted = false;
                                invoice.IsTransferStatusSetManually = true;

                                if (QBOId != null)
                                    invoice.ExternalAccountingEntityId = QBOId;
                            }

                            this.SaveARInvoice(invoice);
                        }
                        break;
                    }

                case "APInvoice":
                    {
                        APInvoicePM invoice = apInvoiceQuery.GetSinglePM(communicationLog.EntityId, tenant);
                        if (invoice != null)
                        {
                            if (isSuccess)
                            {
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "TR";
                                invoice.IsTransferStarted = false;
                                invoice.IsTransferStatusSetManually = true;

                                if (!string.IsNullOrEmpty(QBOId))
                                    invoice.ExternalAccountingEntityId = QBOId;

                                else if (!string.IsNullOrEmpty(QBOIDSuccess))
                                    invoice.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "ET";
                                invoice.IsTransferStarted = false;
                                invoice.IsTransferStatusSetManually = true;

                                if (!string.IsNullOrEmpty(QBOId))
                                    invoice.ExternalAccountingEntityId = QBOId;
                            }

                            this.SaveAPInvoice(invoice);
                        }
                        break;
                    }

                case "ARPayment":
                    {
                        ARPaymentPM payment = arPaymentQuery.GetSinglePM(communicationLog.EntityId, tenant);
                        if (payment != null)
                        {
                            if (isSuccess)
                            {
                                payment.IsUpdatedByQBO = true;
                                payment.TransferError = null;
                                payment.TransferStatusCode = "TR";
                                payment.IsTransferStarted = false;
                                payment.IsTransferStatusSetManually = true;

                                if (!string.IsNullOrEmpty(QBOId))
                                    payment.ExternalAccountingEntityId = QBOId;
                                else if (!string.IsNullOrEmpty(QBOIDSuccess))
                                    payment.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                payment.IsUpdatedByQBO = true;
                                payment.TransferError = null;
                                payment.TransferStatusCode = "ET";
                                payment.IsTransferStarted = false;
                                payment.IsTransferStatusSetManually = true;

                                if (!string.IsNullOrEmpty(QBOId))
                                    payment.ExternalAccountingEntityId = QBOId;
                            }

                            this.SaveARPayment(payment);
                        }
                        break;
                    }

                case "APPayment":
                    {
                        APPaymentPM payment = apPaymentQuery.GetSinglePM(communicationLog.EntityId, tenant);
                        if (payment != null)
                        {
                            if (isSuccess)
                            {
                                payment.TransferError = null;
                                payment.TransferStatusCode = "TR";

                                if (QBOId != null)
                                    payment.ExternalAccountingEntityId = QBOId;
                                else if (QBOIDSuccess != null)
                                    payment.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                payment.TransferError = null;
                                payment.TransferStatusCode = "ET";

                                if (QBOId != null)
                                    payment.ExternalAccountingEntityId = QBOId;
                            }

                            this.SaveAPPayment(payment);
                        }
                        break;
                    }
            }
        }
        private void SaveEntity_Repository(string type, string QBOId, bool isSuccess)
        {
            switch (type)
            {
                case "ARInvoice":
                    {
                        ARInvoice invoice = arInvoiceRepository.GetARInvoiceByInvoiceNumber(tenant, communicationLog.EntityReference);
                        if (invoice != null)
                        {
                            if (isSuccess)
                            {
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "TR";
                                invoice.IsTransferStarted = false;

                                if (QBOId != null)
                                    invoice.ExternalAccountingEntityId = QBOId;
                                else if (QBOIDSuccess != null)
                                    invoice.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "ET";
                                invoice.IsTransferStarted = false;

                                if (QBOId != null)
                                    invoice.ExternalAccountingEntityId = QBOId;
                            }

                            arInvoiceRepository.Update(invoice);
                            arInvoiceRepository.SubmitChanges();
                        }
                        break;
                    }

                case "APInvoice":
                    {
                        APInvoice invoice = apInvoiceRepository.GetSingleAPInvoice(communicationLog.EntityId, tenant);
                        if (invoice != null)
                        {
                            if (isSuccess)
                            {
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "TR";
                                invoice.IsTransferStarted = false;

                                if (!string.IsNullOrEmpty(QBOId))
                                    invoice.ExternalAccountingEntityId = QBOId;

                                else if (!string.IsNullOrEmpty(QBOIDSuccess))
                                    invoice.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                invoice.TransferError = null;
                                invoice.TransferStatusCode = "ET";
                                invoice.IsTransferStarted = false;
                                if (!string.IsNullOrEmpty(QBOId))
                                    invoice.ExternalAccountingEntityId = QBOId;
                            }

                            apInvoiceRepository.Update(invoice);
                            apInvoiceRepository.SubmitChanges();
                        }
                        break;
                    }

                case "ARPayment":
                    {
                        ARPayment payment = arPaymentRepository.GetSingleARPayment(communicationLog.EntityId, tenant);
                        if (payment != null)
                        {
                            if (isSuccess)
                            {
                                payment.TransferError = null;
                                payment.TransferStatusCode = "TR";
                                payment.IsTransferStarted = false;

                                if (!string.IsNullOrEmpty(QBOId))
                                    payment.ExternalAccountingEntityId = QBOId;
                                else if (!string.IsNullOrEmpty(QBOIDSuccess))
                                    payment.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                payment.TransferError = null;
                                payment.TransferStatusCode = "ET";
                                payment.IsTransferStarted = false;

                                if (!string.IsNullOrEmpty(QBOId))
                                    payment.ExternalAccountingEntityId = QBOId;
                            }

                            arPaymentRepository.Update(payment);
                            arPaymentRepository.SubmitChanges();
                        }
                        break;
                    }

                case "APPayment":
                    {
                        APPayment payment = apPaymentRepository.GetSingleAPPayment(communicationLog.EntityId, tenant);
                        if (payment != null)
                        {
                            if (isSuccess)
                            {
                                payment.TransferError = null;
                                payment.TransferStatusCode = "TR";

                                if (QBOId != null)
                                    payment.ExternalAccountingEntityId = QBOId;
                                else if (QBOIDSuccess != null)
                                    payment.ExternalAccountingEntityId = QBOIDSuccess;
                            }

                            else
                            {
                                payment.TransferError = null;
                                payment.TransferStatusCode = "ET";

                                if (QBOId != null)
                                    payment.ExternalAccountingEntityId = QBOId;
                            }

                            apPaymentRepository.Update(payment);
                            apPaymentRepository.SubmitChanges();
                        }
                        break;
                    }
            }
        }
        private void SaveARInvoice(ARInvoicePM invoice)
        {
            ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, tenant);
            invoiceService.Update(invoice);
        }
        private void SaveAPInvoice(APInvoicePM invoice)
        {
            APInvoiceService invoiceService = new APInvoiceService(invoiceContext, tenant);
            invoiceService.Update(invoice);
        }
        private void SaveARPayment(ARPaymentPM payment)
        {
            ARPaymentService paymentService = new ARPaymentService(invoiceContext, tenant);
            paymentService.Update(payment);
        }
        private void SaveAPPayment(APPaymentPM payment)
        {
            APPaymentService paymentService = new APPaymentService(invoiceContext, tenant);
            paymentService.Update(payment);
        }
        // // // // // 

        private void SendPaymentForCloseTheCreditMemoWithIncoive(Invoice result)
        {
            var arInvoiceRepository = new ARInvoiceRepository(tenant);
            var autoCreditARInvoice = arInvoiceRepository.GetARInvoiceByInvoiceNumber(tenant, communicationLog.EntityReference);
            if (!autoCreditARInvoice.IsAutoCredit)
                return;

            var creditMemo = arInvoiceRepository.GetARInvoiceById(tenant, autoCreditARInvoice.CreditedByARInvoiceId).FirstOrDefault();
            if (creditMemo == null || creditMemo.ARInvoiceTypeCode != creditNoteCode)
                return;
            Intuit.Ipp.Data.Payment QBOPayment = CreateQBOPaymentFromInvoice(result, creditMemo);
            ServiceContext serviceContext = GetServiceContext();
            DataService service = new DataService(serviceContext);
            Payment Result = service.Add(QBOPayment) as Payment;
        }
        private Payment CreateQBOPaymentFromInvoice(Invoice invoice, ARInvoice creditMemo)
        {
            var QBOPayment = new Payment();
            QBOPayment.CustomerRef = new ReferenceType { Value = invoice.CustomerRef.Value };
            QBOPayment.TotalAmt = 0;
            QBOPayment.TotalAmtSpecified = true;
            List<Line> lineList = new List<Line>();
            var invoiceLine = CreateLine(invoice.TotalAmt, invoice.Id, invoiceTxnType);
            var creditMemoLine = CreateLine(decimal.Parse(creditMemo.AmountInInvoiceCurrency.Value.ToString()), creditMemo.ExternalAccountingEntityId, creditMemoTxnType);
            lineList.Add(invoiceLine);
            lineList.Add(creditMemoLine);
            QBOPayment.Line = lineList.ToArray();
            return QBOPayment;
        }
        private void SendPaymentForCloseTheIncoiveWithCreditMemo(CreditMemo creditMemo)
        {
            var arInvoiceRepository = new ARInvoiceRepository(tenant);
            var autoCreditARInvoice = arInvoiceRepository.GetARInvoiceByInvoiceNumber(tenant, communicationLog.EntityReference);
            if (!autoCreditARInvoice.IsAutoCredit)
                return;

            var arInvoice = arInvoiceRepository.GetARInvoiceById(tenant, autoCreditARInvoice.CreditedByARInvoiceId).FirstOrDefault();
            if (arInvoice == null)
                return;

            Intuit.Ipp.Data.Payment QBOPayment = CreateQBOPayment(creditMemo, arInvoice);
            ServiceContext serviceContext = GetServiceContext();
            DataService service = new DataService(serviceContext);
            Payment Result = service.Add(QBOPayment) as Payment;
        }
        private Payment CreateQBOPayment(CreditMemo creditMemo, ARInvoice arInvoice)
        {
            var QBOPayment = new Payment();
            QBOPayment.CustomerRef = new ReferenceType { Value = creditMemo.CustomerRef.Value };
            QBOPayment.TotalAmt = 0;
            QBOPayment.TotalAmtSpecified = true;
            List<Line> lineList = new List<Line>();
            var invoiceLine = CreateLine(decimal.Parse(arInvoice.AmountInInvoiceCurrency.Value.ToString()), arInvoice.ExternalAccountingEntityId, invoiceTxnType);
            var creditMemoLine = CreateLine(creditMemo.TotalAmt, creditMemo.Id, creditMemoTxnType);
            lineList.Add(invoiceLine);
            lineList.Add(creditMemoLine);
            QBOPayment.Line = lineList.ToArray();
            return QBOPayment;
        }
        private Line CreateLine(decimal amount, string txnId, string txnType)
        {
            return new Line()
            {
                Amount = Math.Abs(amount),
                AmountSpecified = true,
                LinkedTxn = new LinkedTxn[] { new LinkedTxn()
                {
                    TxnId = txnId,
                    TxnType = txnType
                }}
            };
        }

        private bool CheckInvoiceExisitance(string invoiceNumber)
        {
            try
            {
                if (type == "Invoice")
                {
                    ServiceContext serviceContext = GetServiceContext();
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
                    ServiceContext serviceContext = GetServiceContext();
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
                    ServiceContext serviceContext = GetServiceContext();
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
                    ServiceContext serviceContext = GetServiceContext();
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

            catch (Exception exc)
            {
                ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);
                this.HandleException(exc);
                throw;
            }
        }
        public string GetAccessToken(AccountingSettingPM entityPM, Setting mySetting)
        {
            var oauth2Client = new OAuth2Client(mySetting.QBOClientID,
                    mySetting.QBOClientSecret,
                    "https://developer.intuit.com/v2/OAuth2Playground/RedirectUrl",
                    "production");

            var previousRefreshToken = entityPM.RefreshToken;
            var tokenResp = oauth2Client.RefreshTokenAsync(previousRefreshToken);
            tokenResp.Wait();
            var data = tokenResp.Result;

            if (!string.IsNullOrEmpty(data.Error) || string.IsNullOrEmpty(data.RefreshToken) || string.IsNullOrEmpty(data.AccessToken))
            {
                throw new Exception("Refresh token failed - " + data.Error);
            }

            if (previousRefreshToken != data.RefreshToken)
            {
                entityPM.RefreshToken = data.RefreshToken;
                ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Id);
                AccountingSetting accountingSetting = MyContext.AccountingSettings.Where(p => p.Id == entityPM.Id).FirstOrDefault();
                if (accountingSetting != null)
                {
                    accountingSetting.RefreshToken = data.RefreshToken;
                    MyContext.AccountingSettings.Attach(accountingSetting);
                    MyContext.SetAsModified(accountingSetting);
                    MyContext.SaveChanges();
                }
            }

            return data.AccessToken;
        }
        private ServiceContext GetServiceContextAuth2(AccountingSettingPM entityPM, Setting mySetting)
        {
            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(GetAccessToken(entityPM, mySetting));
            ServiceContext serviceContext = new ServiceContext(entityPM.QBOrealMeID, IntuitServicesType.QBO, oauthValidator);
            serviceContext.IppConfiguration.BaseUrl.Qbo = QuickbooksService.GetQBOBaseURL(mySetting);

            return serviceContext;
        }
        public ServiceContext GetServiceContext()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Setting mySetting = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SettingRepository mySettingRepository = new SettingRepository();
                mySetting = mySettingRepository.GetSingleSetting("1");
                scope.Complete();
            }

            AccountingSettingQuery query = new AccountingSettingQuery(tenant);
            AccountingSettingPM entityPM = query.GetSingleAccountingSettingPMById(tenant);
            return GetServiceContextAuth2(entityPM, mySetting);
        }
        private void HandelSendAPInvoiceAttachments(string invoiceId)
        {
            if (string.IsNullOrEmpty(invoiceId))
                return;

            DocumentsFiling documentsFiling = this.GetAPInvoiceDocumentFilings(invoiceId);

            if (documentsFiling == null)
                return;

            this.SendQueueOfEntityDocumnetsToQuickbooks(documentsFiling);
        }
        private DocumentsFiling GetAPInvoiceDocumentFilings(string invoiceId)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            string shipmentObjectTableId = GetObjectTableIdByName("Shipment", objectTableRepository);
            string apInvoiceObjectTableId = GetObjectTableIdByName("APInvoice", objectTableRepository);
            string documentCode = "APDNCN";

            var documentsFiling = (from a in commonContext.DocumentsFilings.Include("DocumentType")
                                   where a.Tenant == tenant && (a.EntityId == invoiceId || a.ChildEntityId == invoiceId)
                                   && (a.ObjectTableId == shipmentObjectTableId || a.ObjectTableId == apInvoiceObjectTableId)
                                   && a.IsDeleted == false && (a.IsTransferdToQBO == null || a.IsTransferdToQBO == false)
                                   && (a.DocumentType != null && a.DocumentType.Code == documentCode)
                                   select a).FirstOrDefault();

            return documentsFiling;
        }
        private string GetObjectTableIdByName(string objectTableName, ObjectTableRepository objectTableRepository)
        {
            var objectTable = objectTableRepository.GetObjectTableByName(objectTableName, tenant, true);
            if (objectTable == null)
                return "";

            return objectTable.Id;
        }
        private void SendQueueOfEntityDocumnetsToQuickbooks(DocumentsFiling documentFiling)
        {
            if (documentFiling == null)
            {
                return;
            }

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("QBODocumnetsUploaderQueue", documentFiling.Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "EntityId", documentFiling.Id }, { "Tenant", documentFiling.Tenant.ToString() },
                                                                 { "DocumentCode",  "APDNCN" }, { "IsDocumentUploaded", true.ToString() },
                                                                 { "IsDocumentDeleted", false.ToString() } }, documentFiling.Tenant, null, null, null, null);
        }
    }
}
