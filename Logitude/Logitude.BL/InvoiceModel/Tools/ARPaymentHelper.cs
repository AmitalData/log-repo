using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools;
using System.Transactions;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Microsoft.Practices.Unity;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Intuit.Ipp.Core;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
using Logitude.SystemLogs;
using Logitude.Server.Tools.QueueService;
using Simplog.Global.Data.GlobalModel;


namespace Logitude.BL.InvoiceModel.Tools
{
    public class ARPaymentHelper
    {
        private  int tenant { set; get; }
        private  string tenantName { set; get; }
        private  ICommonDataContext commonContext;
        private  DocumentRepository documentRepository;
        private  CommunicationLogRepository communicationLogRepository;
        private  string ARPaymentId;
        private  ARPaymentPM ARPayment;
        private  string myObjectTableId;
        private  IInvoiceContext objectContext;
        private  string ExternalCurrencyCode;
        private  string ExternalTableIdCustomerRef;
        private string ExternalPaymentMethodRef;
        public  string PaymentTermExternalCode = null;
        private  string LoggedContactId { get; set; }
        private  List<string> ExternalChargesTypesCode;
        private  List<string> ExternalVatTypesCode;
        private  Boolean IsNewEntity;
        private  Boolean IsSameHomeCurrency = false;
        private  string AccountingSystemCode;
        private ARInvoicePaymentRepository invoicePaymentRepository;
        private Tenant loggedTenant;
        private AccountingSystemPM accountingSystem;

        private void GetObjectTableData()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("ARPayment", 0, true);
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }
        }
        public  void ARPaymentQuickbooksValidating(ARPaymentPM entityPM, Boolean IsSetApproved, Boolean isNewEntity, ARPayment entityPOCO, IInvoiceContext invoiceContext, ICommonDataContext CommonContext, Boolean isSetVoided,bool setCancelApproved,bool SetReSendQBO, bool isErrorInTransfer, bool SystemWorkerRole=false)
        {
            if (entityPM.TransferStatusCode == "BL")
            {
                return;
            }

            if (isSetVoided || (entityPM.StatusCode == "VD" && SetReSendQBO == true))
            {
                bool isTransferingVoiding = true;

                if (entityPOCO.StatusCode == null || entityPOCO.StatusCode == "DR")
                {
                    isTransferingVoiding = false;
                }

                else if (isErrorInTransfer)
                {
                    isTransferingVoiding = false;
                }

                if (isTransferingVoiding)
                {
                    commonContext = CommonContext;
                    loggedTenant = (from a in commonContext.Tenants.Include("AccountingSetting") where a.Id == entityPM.Tenant select a).FirstOrDefault();
                    tenant = loggedTenant.Id;
                    tenantName = loggedTenant.Company;
                    AccountingSystemCode = loggedTenant.AccountingSetting.AccountingSystemCode;
                    AccountingSystemQuery query = new AccountingSystemQuery(tenant);
                    accountingSystem = query.GetSingleAccountingSystemPM(AccountingSystemCode);
                    if (loggedTenant.AccountingSetting != null)
                        if (IsQuickBooksAccoutingSystemTransfer(entityPM))
                        {
                            if (entityPM.ExternalAccountingEntityId != null)
                            {
                                ARPayment = entityPM;
                                ARPaymentId = entityPM.Id;
                                GetObjectTableData();
                                documentRepository = new DocumentRepository(commonContext);
                                communicationLogRepository = new CommunicationLogRepository(commonContext);
                                ContactRepository contactRepository = new ContactRepository(commonContext);
                                Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact;
                                if (SystemWorkerRole)
                                {
                                    loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
                                }
                                else
                                {
                                    loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                                }
                                LoggedContactId = loggedContact.Id;

                                entityPM.TransferStatusCode = "IP";
                                entityPM.TransferError = null;
                                this.SendXMLFileInvoiceVoid(entityPM.ExternalAccountingEntityId, "QBO");
                            }
                            else
                            {
                                throw new ApplicationException("This Payment is not transfered yet to quickbooks online.");

                            }


                        }
                }
            }

            else if (IsSetApproved && setCancelApproved == false)
            {
                if (entityPM.TransferStatusCode == "RD" && entityPM.AccountingPaymentMethodCode == "FS")
                    return;
                else
                {
                    commonContext = CommonContext;
                    loggedTenant = (from a in commonContext.Tenants.Include("AccountingSetting") where a.Id == entityPM.Tenant select a).FirstOrDefault();
                    tenant = loggedTenant.Id;
                    tenantName = loggedTenant.Company;
                    AccountingSystemCode = loggedTenant.AccountingSetting.AccountingSystemCode;
                    AccountingSystemQuery query = new AccountingSystemQuery(tenant);
                    accountingSystem = query.GetSingleAccountingSystemPM(AccountingSystemCode);

                    if (loggedTenant.AccountingSetting != null)
                        if (IsQuickBooksAccoutingSystemTransfer(entityPM))
                        {
                            entityPM.ExternalAccountingEntityId = entityPOCO.ExternalAccountingEntityId;
                            ARPayment = entityPM;
                            ARPaymentId = entityPM.Id;
                            documentRepository = new DocumentRepository(commonContext);
                            communicationLogRepository = new CommunicationLogRepository(commonContext);
                            GetObjectTableData();
                            ContactRepository contactRepository = new ContactRepository(commonContext);
                            Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact;
                            if (SystemWorkerRole)
                            {
                                loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
                            }
                            else
                            {
                                loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                            }
                            LoggedContactId = loggedContact.Id;
                            objectContext = invoiceContext;
                            this.invoicePaymentRepository = new ARInvoicePaymentRepository(this.objectContext);
                            if (loggedTenant.CurrencyId == entityPM.PaymentCurrencyId)
                                IsSameHomeCurrency = true;
                            IsNewEntity = isNewEntity;

                            ExternalChargesTypesCode = new List<string>();
                            ExternalVatTypesCode = new List<string>();
                            #region

                            bool isReady = true;
                            string myError = null;
                            string ExternalCodeError = "Bill To: " + entityPM.BillToName + ". External ID is missing";
                            string CurrencyError = "";
                            string ARPaymentMethodError = "";
                            CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
                            IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

                            CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entityPM.BillToId && d.CurrencyId == entityPM.PaymentCurrencyId select d).FirstOrDefault();
                            if (myCardExternal != null)
                            {
                                if (FieldIsEmpty(myCardExternal.ExternalRecievableTableId))
                                {
                                    isReady = false;
                                    myError = ExternalCodeError;
                                }
                                else
                                {
                                    ExternalTableIdCustomerRef = myCardExternal.ExternalRecievableTableId;
                                }
                            }
                            else
                            {
                                isReady = false;
                                myError = ExternalCodeError;
                            }




                            Simplog.Data.CommonDataModel.EntityPOCOs.Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.PaymentCurrencyId, tenant, false);
                            if (loggedTenant.CurrencyId != myCurrency.Id)
                            {
                                if (FieldIsEmpty(myCurrency.AccountingExternalCode))
                                {
                                    isReady = false;
                                    CurrencyError = "Currency: " + myCurrency.Code + ". External ID is missing.";
                                    myError = string.IsNullOrEmpty(myError) ? CurrencyError : myError + ";" + CurrencyError;


                                }
                                else
                                {
                                    ExternalCurrencyCode = myCurrency.AccountingExternalCode;
                                }
                            }
                            else
                                ExternalCurrencyCode = myCurrency.AccountingExternalCode;


                            Simplog.Data.InvoiceModel.Repositories.AccountingPaymentMethodRepository ARPaymentMethodRespository = new Simplog.Data.InvoiceModel.Repositories.AccountingPaymentMethodRepository(invoiceContext);


                            Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod ARPaymentMethod = ARPaymentMethodRespository.GetSingleAccountingPaymentMethod(entityPM.AccountingPaymentMethodId, tenant);
                            if (ARPaymentMethod != null)
                            {
                                if (FieldIsEmpty(ARPaymentMethod.ARExternalId))
                                {
                                    isReady = false;
                                    ARPaymentMethodError = "Payment Method: " + ARPaymentMethod.Name + ". External ID is missing.";
                                    myError = string.IsNullOrEmpty(myError) ? ARPaymentMethodError : myError + ";" + ARPaymentMethodError;


                                }
                                else
                                {
                                    ExternalPaymentMethodRef = ARPaymentMethod.ARExternalId;
                                }
                            }
                            else
                            {
                                isReady = false;
                                myError = string.IsNullOrEmpty(myError) ? "Payment Method doesn't exist" : myError + ";" + "Payment Method doesn't exist";
                            }




                            if (isReady)
                            {
                                entityPM.TransferStatusCode = "IP";
                                entityPM.TransferError = null;
                                entityPOCO.TransferStatusCode = "IP";
                                entityPOCO.TransferError = null;
                                Run(entityPM);
                            }

                            else
                            {
                                entityPM.TransferStatusCode = "NR";
                                entityPM.TransferError = myError;
                                entityPOCO.TransferStatusCode = "IP";
                                entityPOCO.TransferError = myError;
                                throw new ApplicationException(myError);
                            }
                            #endregion
                        }
                }
            }
        }

        private bool IsQuickBooksAccoutingSystemTransfer(ARPaymentPM arPaymentPM)
        {
            if (!(AccountingSystemCode == "QBO" || AccountingSystemCode == "QBOG")) return false;
            if (!(loggedTenant.AccountingSetting.IsARPaymentsTransferEnabled)) return false;
            if (!(accountingSystem.AllowARPaymentsTransfer)) return false;
            if (!(loggedTenant.AccountingSetting.APInvoiceTransferStartDate != null && arPaymentPM.RegisterDate >= loggedTenant.AccountingSetting.APInvoiceTransferStartDate)) return false;
            return true;
        }

        private void SendXMLFileInvoiceVoid(string ARInvoiceExternalId, string queueName)
        {


            Type myType = ARInvoiceExternalId.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();



            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, ARInvoiceExternalId, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(myMemoryStream);

            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AR Payment Void");

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            try
            {
                DbQueueService queueservice;
                queueservice = new DbQueueService();

                queueservice.InitializeQueue("QBO", 0);
                queueservice.Send(new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "ARPaymentVoid" }, { "OldTransferStatusCode", null } }, tenant);
                queueservice.Complete();
            }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quickbooks online web service", null, ip);
            }



        }

        public List<Intuit.Ipp.Data.Customer> GetQuickBooksOnlineCustomersByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Customer> customerQueryService = new QueryService<Intuit.Ipp.Data.Customer>(context);
                List<Intuit.Ipp.Data.Customer> myResult = customerQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }
        public  List<Intuit.Ipp.Data.Vendor> GetQuickBooksOnlineVendorByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Vendor> VendorQueryService = new QueryService<Intuit.Ipp.Data.Vendor>(context);
                List<Intuit.Ipp.Data.Vendor> myResult = VendorQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }
        }
        public  List<Intuit.Ipp.Data.TaxCode> GetQuickBooksOnlineVatTypesByText(String sql, String tenant)
        {
            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.TaxCode> TaxCodeQueryService = new QueryService<Intuit.Ipp.Data.TaxCode>(context);
                List<Intuit.Ipp.Data.TaxCode> myResult = TaxCodeQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }            
        }        
        public  List<Intuit.Ipp.Data.Item> GetQuickBooksOnlineReceivableChargesTypesByText(String sql, String tenant)
        {
            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Item> ItemQueryService = new QueryService<Intuit.Ipp.Data.Item>(context);
                List<Intuit.Ipp.Data.Item> myResult = ItemQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;                
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }
        public  List<Intuit.Ipp.Data.Account> GetQuickBooksOnlinePayablesChargesTypesByText(String sql, String tenant)
        {

            try
            {

                ServiceContext context = QuickbooksService.GetServiceContext(tenant);

                QueryService<Intuit.Ipp.Data.Account> AccountQueryService = new QueryService<Intuit.Ipp.Data.Account>(context);
                List<Intuit.Ipp.Data.Account> myResult = AccountQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }
        public  List<Intuit.Ipp.Data.CompanyCurrency> GetQuickBooksOnlineCurrenciesByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.CompanyCurrency> CompanyCurrencyQueryService = new QueryService<Intuit.Ipp.Data.CompanyCurrency>(context);
                List<Intuit.Ipp.Data.CompanyCurrency> myResult = CompanyCurrencyQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;



            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }
        public  List<Intuit.Ipp.Data.Term> GetQuickBooksOnlinePaymentTermsByText(String sql, String tenant)
        {

            try
            {
                ServiceContext context = QuickbooksService.GetServiceContext(tenant);
                QueryService<Intuit.Ipp.Data.Term> TermQueryService = new QueryService<Intuit.Ipp.Data.Term>(context);
                List<Intuit.Ipp.Data.Term> myResult = TermQueryService.ExecuteIdsQuery(sql).ToList();
                return myResult;
            }

            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());

            }


        }

        private void Run(ARPaymentPM ARPayment)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                Intuit.Ipp.Data.Payment QBOPayment = new Payment();
                QBOPayment.CustomerRef = new ReferenceType { Value = ExternalTableIdCustomerRef };
                QBOPayment.PaymentRefNum = ARPayment.PaymentNo;
                QBOPayment.TotalAmt = ARPayment.AmountInPaymentCurrency != null ? (decimal)ARPayment.AmountInPaymentCurrency : 0;
                QBOPayment.TotalAmtSpecified = true;
                QBOPayment.Id = ARPayment.ExternalAccountingEntityId;
                QBOPayment.TxnDate = ARPayment.RegisterDate.Value;
                QBOPayment.TxnDateSpecified = true;
                QBOPayment.PaymentMethodRef = new ReferenceType { Value = ExternalPaymentMethodRef };
                if (!IsSameHomeCurrency)
                {
                    QBOPayment.ExchangeRate = decimal.Parse(ARPayment.PaymentCurrencyExchangeRate + "");
                    QBOPayment.ExchangeRateSpecified = true;
                    QBOPayment.CurrencyRef = new ReferenceType { Value = ExternalCurrencyCode };
                }
                else
                {
                    QBOPayment.ExchangeRateSpecified = false;
                    QBOPayment.CurrencyRef = null;
                }

                

                List<Line> lineList = new List<Line>();
                var invoicesData = (from d in objectContext.ARInvoicePayments.Include("ARInvoice")
                                    where d.Tenant == tenant
                                    && d.ARPaymentId == ARPayment.Id  && d.ARInvoice.TransferStatusCode=="TR"
                                    select new
                                    {
                                        InvoiceId = d.ARInvoiceId,
                                        ForeignAmount = d.ForeignAmount,
                                        ExternalAccountingEntityId = d.ARInvoice.ExternalAccountingEntityId,
                                    }).ToList();

                

                if (invoicesData.Count > 0)
                {
                    foreach (var item in invoicesData)
                    {
                        Line line = new Line();
                        List<LinkedTxn> list = new List<LinkedTxn>();
                        LinkedTxn l1 = new LinkedTxn();
                        l1.TxnId = item.ExternalAccountingEntityId;
                        l1.TxnType = "Invoice";
                        line.Amount =  item.ForeignAmount!=null?Decimal.Parse(item.ForeignAmount + ""):0;
                        if (line.Amount < 0)
                        {
                            line.Amount *= -1;
                            l1.TxnType = "CreditMemo";
                        }
                        line.AmountSpecified = true;
                        line.DetailType = LineDetailTypeEnum.DescriptionOnly;
                        line.DetailTypeSpecified = true;
                      
                        list.Add(l1);
                        line.LinkedTxn = list.ToArray();
                        lineList.Add(line);
                    }                   
                }
                QBOPayment.Line = lineList.ToArray();
                SendXMLFile(QBOPayment, "QBO");
                scope.Complete();
            }
        }

        private  string myDocumentId;
        private  string myDocumentFolder;
        private  string myDocumentExtension;
        private  string myCommunicationLogId;

        private  void SendXMLFile(Intuit.Ipp.Data.Payment myQBARPayment, string queueName)
        {
            Type myType = myQBARPayment.GetType();
            MemoryStream myMemoryStream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };
            XmlWriter writer = XmlTextWriter.Create(myMemoryStream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, myQBARPayment, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(myMemoryStream);
            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AR Payment");

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            try
            {
                DbQueueService queueservice;
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("QBO", 0);
                Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "ARPayment" }, { "OldTransferStatusCode", null } };
                queueservice.Send(param, tenant);
                queueservice.Complete();
            }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quickbooks online web service", null, ip);
            }



        }



        private void BuildCommunicationLog(int? fileSize, string subject)
        {

            string xmlSubject = subject;

            string xmlTarget = "QBO";


            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = fileSize,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = xmlTarget.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = xmlTarget,
                InOut = "O",
                From = tenantName,
                EntityId = ARPaymentId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = LoggedContactId,
                DocumentId = document.Id,
                EntityReference = ARPayment.PaymentNo,
                SearchFields = ARPayment.PaymentNo + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = DateTime.UtcNow,
            };


            communicationLogRepository.Add(commLog);

            myDocumentId = document.Id;
            myDocumentFolder = document.Folder;
            myDocumentExtension = document.Extension;
            myCommunicationLogId = commLog.Id;
            communicationLogRepository.SubmitChanges();
        }


        private  bool FieldIsEmpty(string myField)
        {
            bool myResult = false;

            if (myField == null || (myField != null && string.IsNullOrEmpty(myField.Trim())))
            {
                myResult = true;
            }

            return myResult;
        }



    }
}
