using Intuit.Ipp.Data;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.Tools
{
    public class APInvoiceMultipleShipmentsHelper
    {
        private  int tenant { set; get; }
        private  string tenantName { set; get; }
        private  ICommonDataContext commonContext;
        private  IInvoiceContext objectContext;
        private  DocumentRepository documentRepository;
        private  CommunicationLogRepository communicationLogRepository;
        private  string APInvoiceId;
        private  APInvoicePM APInvoice;
        private  string myObjectTableId;
        private  string ExternalCurrencyCode;
        private  string ExternalTableIdVendorRef;
        public  string PaymentTermExternalCode = null;
        private  string LoggedContactId { get; set; }
        private  List<string> ExternalVatTypesCode;
        private  List<string> PayablesExternalChargesTypesCode;
        private  Boolean IsNewEntity;
        private  Boolean IsSameHomeCurrency = false;
        private  string myDocumentId;
        private  string myDocumentFolder;
        private  string myDocumentExtension;
        private  string myCommunicationLogId;
        private  string AccountingSystemCode;
        private List<APInvoiceMultipleShipmentPM> MultipleShipmentList;
        private List<APInvoiceLinePM> lines;
        private void GetObjectTableData()
        {
            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("APInvoice", 0, true);
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }
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

        public  void APInvoiceMultipleShipmentsQuickbooksValidating(APInvoicePM entityPM, Boolean IsSetApproved, Boolean isNewEntity, IInvoiceContext InvoiceContext, ICommonDataContext CommonContext)
        {
            if (IsSetApproved)
            {
                commonContext = CommonContext;
                Tenant loggedTenant = (from a in commonContext.Tenants.Include("AccountingSetting") where a.Id == entityPM.Tenant select a).FirstOrDefault();
                tenant = loggedTenant.Id;
                tenantName = loggedTenant.Company;
                AccountingSystemCode = loggedTenant.AccountingSetting.AccountingSystemCode;
                AccountingSystemQuery query = new AccountingSystemQuery(tenant);
                AccountingSystemPM AccountingSystempm = query.GetSingleAccountingSystemPM(AccountingSystemCode);
                if (loggedTenant.AccountingSetting != null)
                    if ((AccountingSystemCode == "QBO" || AccountingSystemCode=="QBOG") && loggedTenant.AccountingSetting.IsAPInvoicesTransferEnabled && AccountingSystempm.AllowAPInvoicesTransfer)
                    {
                        APInvoice = entityPM;
                        APInvoiceId = entityPM.Id;
                        documentRepository = new DocumentRepository(commonContext);
                        communicationLogRepository = new CommunicationLogRepository(commonContext);

                        GetObjectTableData();
                        objectContext = InvoiceContext;
                        PayablesExternalChargesTypesCode = new List<string>();
                        ExternalVatTypesCode = new List<string>();
                        ContactRepository contactRepository = new ContactRepository(commonContext);
                        Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);
                        LoggedContactId = loggedContact.Id;
                        MultipleShipmentList = new List<APInvoiceMultipleShipmentPM>();
                        lines = new List<APInvoiceLinePM>();


                        if (loggedTenant.CurrencyId == entityPM.InvoiceCurrencyId)
                            IsSameHomeCurrency = true;
                        IsNewEntity = isNewEntity;

                        bool isReady = true;
                        string myError = null;
                        string ExternalCodeError = "Vendor : " + entityPM.VendorName + ". External ID is missing"; //External ID
                        string chargeTypeError = "";
                        string CurrencyError = "";
                        string paymentTermError = "Payment Term: " + entityPM.PaymentTermName + ". External ID is missing"; // 
                        string vatError = "";
                        string InvoiceLengthError = "Due to QBO limitation, invoices with a number that exceeds 21 characters can't be transmitted";


                        CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(tenant);
                        IQueryable<CardExternalCodeByCurrency> iQueryable_CardExternals = cardExternalCodeByCurrencyRepository.GetCardExternalCodeByCurrenciesByTenant(tenant);

                        CardExternalCodeByCurrency myCardExternal = (from d in iQueryable_CardExternals where d.CardId == entityPM.VendorId && d.CurrencyId == entityPM.InvoiceCurrencyId select d).FirstOrDefault();
                        if (myCardExternal != null)
                        {
                            if (FieldIsEmpty(myCardExternal.ExternalPayableTableId))
                            {
                                isReady = false;
                                myError = ExternalCodeError;


                            }
                            else
                            {
                                ExternalTableIdVendorRef = myCardExternal.ExternalPayableTableId;
                            }
                        }
                        else
                        {
                            isReady = false;
                            myError = ExternalCodeError;


                        }

                        Simplog.Data.CommonDataModel.EntityPOCOs.Currency myCurrency = CurrencyRepository.GetSingleCurrency(entityPM.InvoiceCurrencyId, tenant, false);
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


                        PaymentTermRepository paymentTermRepository = new PaymentTermRepository(tenant);
                        if (entityPM.AmountInInvoiceCurrency >= 0)
                        {
                            if (entityPM.PaymentTermId != null)
                            {
                                PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPM.PaymentTermId, tenant);
                                if (FieldIsEmpty(myPaymentTerm.ExternalId))
                                {
                                    isReady = false;
                                    myError = string.IsNullOrEmpty(myError) ? paymentTermError : myError + ";" + paymentTermError;

                                }
                                else
                                {
                                    PaymentTermExternalCode = myPaymentTerm.ExternalId;

                                }
                            }
                            else
                            {

                                isReady = false;
                                myError = string.IsNullOrEmpty(myError) ? "Payment term Field Is Required " : myError + ";" + "Payment term Field Is Required";

                            }
                        }

                        if (entityPM.InvoiceNumber.Length > 21)
                        {
                            isReady = false;
                            myError = string.IsNullOrEmpty(myError) ? InvoiceLengthError : myError + ";" + InvoiceLengthError;

                        }

                        APInvoiceLineRepository APRepo = new APInvoiceLineRepository(this.objectContext);
                            APInvoiceLineQuery APQuery = new APInvoiceLineQuery(APRepo);
                        lines  = APQuery.GetInvoiceLinesByInvoiceId(entityPM.Id, tenant);
                        if (!isNewEntity)
                        {
                            lines = lines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                        }
                        
                        foreach (APInvoiceLinePM line in lines) {

                                ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, tenant, false);

                                if (myChargesType.PayablesChargesTypeExternalCode == null || (myChargesType.PayablesChargesTypeExternalCode != null && string.IsNullOrEmpty(myChargesType.PayablesChargesTypeExternalCode.Trim())))
                                {
                                    isReady = false;
                                    chargeTypeError = "Payable Charge Type: " + myChargesType.EnglishName + ". External ID is missing.";
                                    myError = string.IsNullOrEmpty(myError) ? chargeTypeError : myError + ";" + chargeTypeError;
                                }
                                else
                                {

                                    PayablesExternalChargesTypesCode.Add(myChargesType.PayablesChargesTypeExternalCode);

                                }
                                VatType myVatType = VatTypeRepository.GetSingleVatType(line.VatTypeId, tenant, false);

                                ExternalVatTypesCode.Add(myVatType.ReceivablesExternalId);


                            if (line.VatPercentage != 0)
                                {
                                    if (AccountingSystemCode == "QBO")
                                    {
                                        var error = "VAT Percentage must be 0 in Quickbooks online US";
                                        myError = string.IsNullOrEmpty(myError) ? error : myError + ";" + error;
                                        isReady = false;
                                        if (myError != null ? !myError.Contains(error) : true)
                                            myError = string.IsNullOrEmpty(myError) ? error : myError + ";" + error;
                                    }



                                    if (FieldIsEmpty(myVatType.ReceivablesExternalId))
                                {
                                    vatError = "VAT Type: " + myVatType.EnglishName + ". External ID is missing.";
                                        isReady = false;
                                        if (myError != null ? !myError.Contains(vatError) : true)
                                            myError = string.IsNullOrEmpty(myError) ? vatError : myError + ";" + vatError;
                                    }
                                }
                            }

                        



                        if (isReady)
                        {
                            entityPM.TransferStatusCode = "IP";
                            entityPM.TransferError = null;
                            Run(entityPM);
                        }

                        else
                        {
                            entityPM.TransferStatusCode = "NR";
                            myError.Replace(',', ' ');
                            entityPM.TransferError = myError;
                            throw new ApplicationException(myError);

                        }


                    }
            }

        }

        private  void Run(APInvoicePM invoice)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                if (invoice.AmountInInvoiceCurrency >= 0)
                {

                    Intuit.Ipp.Data.Bill QBOBill = new Bill();
                    QBOBill.VendorRef = new ReferenceType { Value = ExternalTableIdVendorRef };
                    QBOBill.TxnDate = invoice.InvoiceDate.Value;
                    QBOBill.TxnDateSpecified = true;
                    QBOBill.DocNumber = invoice.InvoiceNumber;
                    QBOBill.Id = invoice.Id;
                    QBOBill.domain = invoice.ExternalAccountingEntityId;

                    System.Collections.Generic.List<Line> lineList = new List<Line>();
                    string notes = "";
                    if (!String.IsNullOrEmpty(invoice.InternalNotes))
                    {
                        notes = invoice.InternalNotes;
                    }
                    QBOBill.PrivateNote =  notes;

                    if (PaymentTermExternalCode != null)
                        QBOBill.SalesTermRef = new ReferenceType { Value = PaymentTermExternalCode };

                    for (int i = 0; i < lines.Count; i++)
                    {
                        Line line = new Line();
                        line.Description = lines[i].Description;
                        if (AccountingSystemCode == "QBOG")
                        {
                            line.Amount = Decimal.Parse(lines[i].InvoiceCurrencyAmount + "");
                            line.AmountSpecified = true;
                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            line.DetailTypeSpecified = true;
                            if (lines[i].VatPercentage != 0)
                            {
                                line.AnyIntuitObject = new AccountBasedExpenseLineDetail
                                {
                                    AccountRef = new ReferenceType
                                    {
                                        Value = PayablesExternalChargesTypesCode[i]
                                    },
                                    TaxCodeRef = new ReferenceType
                                    {
                                        Value = ExternalVatTypesCode[i]
                                    },
                                };
                            }
                            else
                            {
                                line.AnyIntuitObject = new AccountBasedExpenseLineDetail
                                {
                                    AccountRef = new ReferenceType
                                    {
                                        Value = PayablesExternalChargesTypesCode[i]
                                    },

                                };
                            }

                        }
                        else if (AccountingSystemCode == "QBO")
                        {
                            line.Amount = Decimal.Parse(lines[i].InvoiceCurrencyAmount + "");//+( Decimal.Parse(invoice.InvoiceLines[i].InvoiceCurrencyAmount + "") * (Decimal.Parse(invoice.InvoiceLines[i].VatPercentage + "")/100));
                            line.AmountSpecified = true;
                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            line.DetailTypeSpecified = true;
                            line.AnyIntuitObject = new AccountBasedExpenseLineDetail
                            {
                                AccountRef = new ReferenceType
                                {
                                    Value = PayablesExternalChargesTypesCode[i]
                                },
                            };
                        }
                        lineList.Add(line);
                    }

                    QBOBill.Line = lineList.ToArray();
                    if (!IsSameHomeCurrency)
                    {
                        QBOBill.ExchangeRate = decimal.Parse(invoice.InvoiceCurrencyExchangeRate + "");
                        QBOBill.ExchangeRateSpecified = true;
                        QBOBill.CurrencyRef = new ReferenceType { Value = ExternalCurrencyCode };

                    }
                    else
                    {
                        QBOBill.ExchangeRateSpecified = false;
                        QBOBill.CurrencyRef = null;
                    }
                    SendXMLFile(QBOBill, "QBO");
                    scope.Complete();

                }
                else
                {
                    Intuit.Ipp.Data.VendorCredit QBOBill = new VendorCredit();
                    QBOBill.VendorRef = new ReferenceType { Value = ExternalTableIdVendorRef };
                    QBOBill.TxnDate = invoice.InvoiceDate.Value;
                    QBOBill.TxnDateSpecified = true;
                    QBOBill.DocNumber = invoice.InvoiceNumber;
                     QBOBill.Id = invoice.Id;
                    QBOBill.domain = invoice.ExternalAccountingEntityId;

                    string notes = "";
                    if (!String.IsNullOrEmpty(invoice.InternalNotes))
                    {
                        notes = invoice.InternalNotes;
                    }
                    QBOBill.PrivateNote = notes;

                    System.Collections.Generic.List<Line> lineList = new List<Line>();
                    List<APInvoiceLinePM> lines = new List<APInvoiceLinePM>();
                    lines = invoice.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                 
                    for (int i = 0; i < lines.Count; i++)
                    {
                        Line line = new Line();
                        line.Description = lines[i].Description;
                        if (AccountingSystemCode == "QBOG")
                        {
                           
                            line.Amount = Decimal.Parse(lines[i].InvoiceCurrencyAmount + "");
                            line.Amount = line.Amount * -1;
                            line.AmountSpecified = true;
                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            line.DetailTypeSpecified = true;
                            if (lines[i].VatPercentage != 0)
                            {
                                line.AnyIntuitObject = new AccountBasedExpenseLineDetail
                                {
                                    AccountRef = new ReferenceType
                                    {
                                        Value = PayablesExternalChargesTypesCode[i]
                                    },
                                    TaxCodeRef = new ReferenceType
                                    {
                                        Value = ExternalVatTypesCode[i]
                                    },
                                };
                            }
                            else
                            {
                                line.AnyIntuitObject = new AccountBasedExpenseLineDetail
                                {
                                    AccountRef = new ReferenceType
                                    {
                                        Value = PayablesExternalChargesTypesCode[i]
                                    },

                                };
                            }

                        }
                        else if (AccountingSystemCode == "QBO")
                        {
                            line.Amount = Decimal.Parse(lines[i].InvoiceCurrencyAmount + "");//+( Decimal.Parse(invoice.InvoiceLines[i].InvoiceCurrencyAmount + "") * (Decimal.Parse(invoice.InvoiceLines[i].VatPercentage + "")/100));
                            line.Amount = line.Amount * -1;
                            line.AmountSpecified = true;
                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            line.DetailTypeSpecified = true;
                            line.AnyIntuitObject = new AccountBasedExpenseLineDetail
                            {
                                AccountRef = new ReferenceType
                                {
                                    Value = PayablesExternalChargesTypesCode[i]
                                },
                            };
                        }
                        lineList.Add(line);
                    }

                    QBOBill.Line = lineList.ToArray();
                    if (!IsSameHomeCurrency)
                    {
                        QBOBill.ExchangeRate = decimal.Parse(invoice.InvoiceCurrencyExchangeRate + "");
                        QBOBill.ExchangeRateSpecified = true;
                        QBOBill.CurrencyRef = new ReferenceType { Value = ExternalCurrencyCode };

                    }
                    else
                    {
                        QBOBill.ExchangeRateSpecified = false;
                        QBOBill.CurrencyRef = null;
                    }
                    SendXMLFileVendorCredit(QBOBill, "QBO");
                    scope.Complete();




                }
            }
          

        }

        private  void SendXMLFile(Intuit.Ipp.Data.Bill myQBAPInvoice, string queueName)
        {
            Type myType = myQBAPInvoice.GetType();
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
            ser.Serialize(writer, myQBAPInvoice, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(myMemoryStream);
            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AP Invoice");
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
                Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "APInvoice" } };
                queueservice.Send(param);
                queueservice.Complete();           
                APInvoiceRepository repository = new APInvoiceRepository(tenant);
                APInvoice invoice = repository.GetSingleAPInvoice(APInvoice.Id,tenant);
                APInvoiceMapping.MapEntity(APInvoice, invoice, IsNewEntity);
                repository.Update(invoice);              
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

        private void SendXMLFileVendorCredit(Intuit.Ipp.Data.VendorCredit myQBAPInvoice, string queueName)
        {
            Type myType = myQBAPInvoice.GetType();
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
            ser.Serialize(writer, myQBAPInvoice, ns);
            myMemoryStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(myMemoryStream);
            string xmlString = reader.ReadToEnd();
            xmlString = xmlString.Replace(" />", "/>");
            byte[] myByteArray = Encoding.ASCII.GetBytes(xmlString);
            BuildCommunicationLog(myByteArray.Length, "AP Invoice Credit");
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
                Dictionary<string, string> param = new Dictionary<string, string>() { { "QuickbooksOnline", myCommunicationLogId }, { "Tenant", tenant.ToString() }, { "type", "VendorCredit" } };
                queueservice.Send(param);
                queueservice.Complete();
                APInvoiceRepository repository = new APInvoiceRepository(tenant);
                APInvoice invoice = repository.GetSingleAPInvoice(APInvoice.Id, tenant);
                APInvoiceMapping.MapEntity(APInvoice, invoice, IsNewEntity);
                repository.Update(invoice);
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
                EntityId = APInvoiceId,
                ObjectTableId = myObjectTableId,
                Subject = xmlSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = LoggedContactId,
                DocumentId = document.Id,
                EntityReference = APInvoice.InvoiceNumber,
                SearchFields = APInvoice.InvoiceNumber + "," + xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = DateTime.UtcNow,
            };            
            communicationLogRepository.Add(commLog);
            myDocumentId = document.Id;
            myDocumentFolder = document.Folder;
            myDocumentExtension = document.Extension;
            myCommunicationLogId = commLog.Id;
            communicationLogRepository.SubmitChanges();
        }





    }
}
