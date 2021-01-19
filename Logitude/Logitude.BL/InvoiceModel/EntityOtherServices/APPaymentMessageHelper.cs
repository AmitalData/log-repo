using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using System.IO;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Xml;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using System.Web;
using Logitude.SystemLogs;
using Logitude.Server.Tools.QueueService;
using Logitude.BL.Helpers;

namespace Logitude.BL.InvoiceModel.EntityOtherServices
{
    public class APPaymentMessageHelper
    {
        private int tenant;
        private string filename;
        private List<APPayment> allEntities;
        private string myVATableTempCard;
        private string myVATExemptTempCard;
        private string myAccountingSystemCode;
        private bool isDropBox = false;
        private bool UsingFTP = false;
        private string FTPDetailId;
        public APPaymentMessageHelper(int tenant)
        {
            this.tenant = tenant;

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                myVATableTempCard = accountingSetting.PayableVATableTempCard;
                myVATExemptTempCard = accountingSetting.PayableVATExemptTempCard;
                myAccountingSystemCode = accountingSetting.AccountingSystemCode;
            }
        }
        public APPaymentMessageHelper(List<APPayment> allEntities, string filename, int tenant, bool isDropBox = false, bool isFTP = false)
        {
            this.tenant = tenant;
            this.filename = filename;
            this.allEntities = allEntities;
            this.isDropBox = isDropBox;
            this.UsingFTP = isFTP;

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                myVATableTempCard = accountingSetting.PayableVATableTempCard;
                myVATExemptTempCard = accountingSetting.PayableVATExemptTempCard;
                myAccountingSystemCode = accountingSetting.AccountingSystemCode;
                FTPDetailId = accountingSetting.TransferFTPDetailId;
            }
        }

        public void Transfer()
        {
            if (myAccountingSystemCode == "GI" || myAccountingSystemCode == "AI")
            {
                this.GetGenericInterfaceData();
            }

            else
            {
                this.GetOriginalTranferData();
            }
        }
        public void RebuildFile()
        {
            if (filename.ToLower().EndsWith(".xml"))
            {
                this.GetGenericInterfaceData();
            }

            else
            {
                this.GetOriginalTranferData();
            }
        }

        private void GetOriginalTranferData()
        {
            
        }

        private void GetGenericInterfaceData()
        {
            APPaymentRoot log = new APPaymentRoot();
            log.Paymnets = new List<APPaymnetElement>();

            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<string> allPaymentIds = allEntities.Select(s => s.Id).ToList();
            List<CreditCardType> creditCardTypes = (from d in invoiceContext.CreditCardTypes where d.Tenant == tenant select d).ToList();
            List<AccountingPaymentMethod> paymentMethods = (from d in invoiceContext.AccountingPaymentMethods where d.IsAP == true && d.Tenant == tenant select d).ToList();
            
            List<APInvoicePayment> allInvoicesPayments = (from d in invoiceContext.APInvoicePayments
                                                          where d.Tenant == tenant
                                                          && allPaymentIds.Contains(d.APPaymentId)
                                                          select d).ToList();

            List<APInvoice> allInvoices = (from d in invoiceContext.APInvoicePayments
                                           where d.Tenant == tenant
                                           && allPaymentIds.Contains(d.APPaymentId)
                                           select d.APInvoice).ToList();

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            List<string> allAddressesId = allEntities.Select(s => s.VendorAddressId).ToList();
            List<Address> allAddresses = (from d in commonDataContext.Addresses.Include("Country")
                                          where d.Tenant == tenant
                                          && allAddressesId.Contains(d.Id)
                                          select d).ToList();


            foreach (APPayment item in allEntities)
            {
                APPaymnetElement paymentElement = new APPaymnetElement();

                string myCreditCardTypeCode = null;
                string myCreditCardTypeName = null;
                CreditCardType myCreditCardType = creditCardTypes.Where(d => d.Id == item.CreditCardTypeId).FirstOrDefault();
                if (myCreditCardType != null)
                {
                    myCreditCardTypeCode = myCreditCardType.Code;
                    myCreditCardTypeName = myCreditCardType.Name;
                }

                string myPaymentMethodCode = null;
                AccountingPaymentMethod myPaymentMethod = paymentMethods.Where(d => d.Id == item.AccountingPaymentMethodId).FirstOrDefault();
                if (myPaymentMethod != null)
                {
                    myPaymentMethodCode = myPaymentMethod.Code;
                }

                #region paymnet
                paymentElement.DocumentType = "APPAY";
                paymentElement.PaymentNumber = item.PaymentNo;
                paymentElement.RegisterDate = item.RegisterDate;
                paymentElement.PaymentCurrency = item.PaymentCurrency != null ? item.PaymentCurrency.Code: null;
                paymentElement.PaymentMethod = myPaymentMethodCode;
                paymentElement.PaymentAmount = item.AmountInPaymentCurrency;
                paymentElement.ExchngeRate = item.PaymentCurrencyExchangeRate;
                paymentElement.PaymentAmountInLocalCurrency = item.AmountInLocalCurrency;
                paymentElement.ValueDate = item.ValueDate;
                paymentElement.CreditCardCode = myCreditCardTypeCode;
                paymentElement.CreditCardName = myCreditCardTypeName;
                paymentElement.Tenant = tenant;

                if (myPaymentMethodCode == "CH")
                {
                    paymentElement.ChequeNumber = item.ChequeOrPaymentRef;
                }
                else
                {
                    paymentElement.PaymentRef = item.ChequeOrPaymentRef;
                }
                paymentElement.BankCode = item.Bank;
                paymentElement.BankBranch = item.BankBranch;
                paymentElement.BankAccount = item.Account;
                #endregion

                #region Card
                paymentElement.Card = new APPaymnetCardElement();
                Card vendor = CardRepository.GetSingleCard(item.VendorId, tenant, true);
                if (vendor != null)
                {
                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    paymentElement.Card.AccountingCard = accountingSystemHelper.GetGenericCreditAccount(vendor.Id, item.PaymentCurrencyId, tenant, true);
                    paymentElement.Card.IntercompanyCode = vendor.ExternalId2;
                    paymentElement.Card.Name = vendor.EnglishName;
                    paymentElement.Card.Code = vendor.Code;

                    Address address = allAddresses.Where(d => d.Id == item.VendorAddressId).FirstOrDefault();

                    if (address != null)
                    {
                        paymentElement.Card.Address1 = address.Address1;
                        paymentElement.Card.Address2 = address.Address2;
                        paymentElement.Card.City = address.City;
                        paymentElement.Card.ZipCode = address.ZipCode;
                        paymentElement.Card.Country = address.Country == null ? "" : address.Country.EnglishName;
                        paymentElement.Card.CountryCode = address.Country == null ? "" : address.Country.Code;
                    }
                    paymentElement.Card.VatNumber = vendor.VatNumber;
                }
                #endregion

                #region Invoices

                paymentElement.Invoices = new List<APPaymentInvoiceElement>();
                List<string> myInvoicesIds = allInvoicesPayments.Where(d => d.APPaymentId == item.Id).Select(s => s.APInvoiceId).ToList();

                foreach (APInvoice myline in allInvoices.Where(d => myInvoicesIds.Contains(d.Id)))
                {
                    //string lineType = invoiceTypes.Where(d => d.Code == myline.APInvoiceTypeCode).FirstOrDefault().Name;
                    double linePaidAmount = 0;
                    APInvoicePayment myAPInvoicePayment = allInvoicesPayments.Where(d => d.APPaymentId == item.Id && d.APInvoiceId == myline.Id).FirstOrDefault();

                    if (myAPInvoicePayment != null)
                    {
                        if (myAPInvoicePayment.ForeignAmount != null)
                        {
                            linePaidAmount = Math.Round(myAPInvoicePayment.ForeignAmount.Value, 2);
                        }
                    }

                    APPaymentInvoiceElement invoiceElement = new APPaymentInvoiceElement()
                    {
                        //InvoiceType = lineType,
                        InvoiceNumber = myline.InvoiceNumber,
                        PaidAmount = linePaidAmount,
                    };             

                    paymentElement.Invoices.Add(invoiceElement);
                }

                #endregion

                log.Paymnets.Add(paymentElement);
            }

            this.BuildXMLFile(log, tenant, filename);
        }

        #region Build XML
        private void BuildXMLFile(APPaymentRoot myClass, int tenant, string fileName)
        {
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(APPaymentRoot));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //ns.Add("", "http://www.champ.aero/GCCS/CargoXML");

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace
            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);

            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");

            ser.Serialize(writer, myClass, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();

            content = content.Replace(" />", "/>");

            byte[] bytearray = Encoding.UTF8.GetBytes(content);

            if (this.isDropBox)
            {
                this.BulidDropBoxXMLLFile(bytearray);
            }

            else if (this.UsingFTP && !string.IsNullOrEmpty(this.FTPDetailId))
            {
                this.BuildFile_ViaFTP(bytearray);
            }

            else
            {
                if (bytearray != null)
                {
                    string[] fileProps = fileName.Split('.');
                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = fileProps[0],
                        Extension = fileProps.Length > 1 ? fileProps[1] : "xml",
                        Tenant = tenant,
                        FileSize = bytearray.Length,
                        HasExternalContainer = true,
                        ExternalContainerName = "tenant" + tenant
                    };
                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    storageservice.Write(bytearray, fileInfo);
                }
            }
        }
        private void BulidDropBoxXMLLFile(byte[] bytearray)
        {
            DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("APPayment");
            var payment = this.allEntities.FirstOrDefault();
            var entityId = "";
            if (payment != null)
            {
                entityId = payment.Id;
            }
            var commLog = helper.CreateDropBoxCommunicationLog(tenant, objectTableId, bytearray, this.filename, "APPayments", "AP Payment", entityId);
        }

        private string myDocumentId;
        private string myDocumentFolder;
        private string myDocumentExtension;
        private string myCommunicationLogId;
        private void BuildFile_ViaFTP(byte[] myByteArray)
        {
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            string objectTableId = repo.GetObjectTableIdByName("APPayment");
            string FTPFileName = "";

            AccountingTranferViaFTPHelper helper = new AccountingTranferViaFTPHelper(tenant, objectTableId, FTPDetailId);
            var payment = this.allEntities.FirstOrDefault();
            var entityId = "";
            if (payment != null)
            {
                entityId = payment.Id;
                FTPFileName = ("APPayment_" + payment.PaymentNo).ToLower();
            }

            CommunicationLog commLog = helper.CreateCommunicationLog(myByteArray, FTPFileName, entityId, myAccountingSystemCode);

            this.myDocumentId = helper.DocumentId;
            this.myDocumentFolder = helper.DocumentFolder;
            this.myDocumentExtension = helper.DocumentExtension;
            this.myCommunicationLogId = commLog.Id;

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(myByteArray, fileInfo);

            try
            {
                //helper.Test(commLog, tenant);
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(commLog.QueueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() } }, tenant);
            }

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, System.DateTime.Now, 0, null, "AP Payment Transfer", null, ip);
            }
        }
        #endregion
    }

    #region GenericInterface
    [XmlRoot("Logitude")]
    public class APPaymentRoot
    {
        [XmlArray("Payments")]
        [XmlArrayItem("Payment")]
        public List<APPaymnetElement> Paymnets { get; set; }
    }

    public class APPaymnetElement
    {
        public string DocumentType { get; set; }
        public string PaymentNumber { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime? RegisterDate { get; set; }
        public string PaymentCurrency { get; set; }
        public string PaymentMethod { get; set; }
        public double? PaymentAmount { get; set; }
        public double? ExchngeRate { get; set; }
        public double? PaymentAmountInLocalCurrency { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime? ValueDate { get; set; }
        public string PaymentRef { get; set; }
        public string CreditCardCode { get; set; }
        public string CreditCardName { get; set; }
        public string ChequeNumber { get; set; }
        public string BankCode { get; set; }
        public string BankBranch { get; set; }
        public string BankAccount { get; set; }
        public int Tenant { get; set; }

        [XmlElement(ElementName = "Vendor")]
        public APPaymnetCardElement Card { get; set; }

        [XmlArray("Invoices")]
        [XmlArrayItem("Invoice")]
        public List<APPaymentInvoiceElement> Invoices { get; set; }
    }

    public class APPaymnetCardElement
    {
        public string Code { get; set; }
        public string AccountingCard { get; set; }
        public string IntercompanyCode { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string VatNumber { get; set; }
    }

    public class APPaymentInvoiceElement
    {
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }
        public double PaidAmount { get; set; }

    }
    #endregion
}
