using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InvoiceModel.EntityOtherServices
{
    public class ARPaymentMessageHelper
    {
        private int tenant;
        private string filename;
        private List<ARPayment> allEntities;
        private string myVATableTempCard;
        private string myVATExemptTempCard;
        private string myAccountingSystemCode;
        private bool isDropBox = false;

        public ARPaymentMessageHelper(int tenant)
        {
            this.tenant = tenant;

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                myVATableTempCard = accountingSetting.ReceivableVATableTempCard;
                myVATExemptTempCard = accountingSetting.ReceivableVATExemptTempCard;
                myAccountingSystemCode = accountingSetting.AccountingSystemCode;
            }
        }
        public ARPaymentMessageHelper(List<ARPayment> allEntities, string filename, int tenant,  bool isDropBox = false)
        {
            this.tenant = tenant;
            this.filename = filename;
            this.allEntities = allEntities;
            this.isDropBox = isDropBox;

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);
            if (accountingSetting != null)
            {
                myVATableTempCard = accountingSetting.ReceivableVATableTempCard;
                myVATExemptTempCard = accountingSetting.ReceivableVATExemptTempCard;
                myAccountingSystemCode = accountingSetting.AccountingSystemCode;
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
            // Not implemented yet

        }
        private void GetGenericInterfaceData()
        {
            ARPaymentRoot log = new ARPaymentRoot();
            log.Paymnets = new List<ARPaymnetElement>();
            
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            List<string> allPaymentIds = allEntities.Select(s => s.Id).ToList();
            List<CreditCardType> creditCardTypes = (from d in invoiceContext.CreditCardTypes where d.Tenant == tenant select d).ToList();
            List<AccountingPaymentMethod> paymentMethods = (from d in invoiceContext.AccountingPaymentMethods where d.Tenant == tenant select d).ToList();
            List<ARInvoiceType> invoiceTypes = (from d in invoiceContext.ARInvoiceTypes select d).ToList();

            List<ARInvoicePayment> allInvoicesPayments = (from d in invoiceContext.ARInvoicePayments
                                                          where d.Tenant == tenant
                                                          && allPaymentIds.Contains(d.ARPaymentId)
                                                          select d).ToList();

            List<ARInvoice> allInvoices = (from d in invoiceContext.ARInvoicePayments
                                                          where d.Tenant == tenant
                                                          && allPaymentIds.Contains(d.ARPaymentId)
                                                          select d.ARInvoice).ToList();

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            List<string> allAddressesId = allEntities.Select(s => s.BillToAddressId).ToList();
            List<Address> allAddresses = (from d in commonDataContext.Addresses.Include("Country")
                                          where d.Tenant == tenant
                                          && allAddressesId.Contains(d.Id)
                                          select d).ToList();


            foreach (ARPayment item in allEntities)
            {
                ARPaymnetElement paymentElement = new ARPaymnetElement();

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
                paymentElement.DocumentType = "ARPAY";
                paymentElement.PaymentNumber = item.PaymentNo;
                paymentElement.RegisterDate = item.RegisterDate;
                paymentElement.PaymentCurrency = item.PaymentCurrency.Code;
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
                paymentElement.Card = new ARPaymnetCardElement();
                Card billTo = CardRepository.GetSingleCard(item.BillToId, tenant, true);
                if (billTo != null)
                {
                    paymentElement.Card.AccountingCard = billTo.ReceivablesAccountingCard;
                    paymentElement.Card.IntercompanyCode = billTo.ExternalId2;
                    paymentElement.Card.Name = billTo.EnglishName;
                    paymentElement.Card.Code = billTo.Code;

                    Address address = allAddresses.Where(d => d.Id == item.BillToAddressId).FirstOrDefault();

                    if (address != null)
                    {
                        paymentElement.Card.Address1 = address.Address1;
                        paymentElement.Card.Address2 = address.Address2;
                        paymentElement.Card.City = address.City;
                        paymentElement.Card.ZipCode = address.ZipCode;
                        paymentElement.Card.Country = address.Country == null ? "" : address.Country.EnglishName;
                    }
                    paymentElement.Card.VatNumber = billTo.VatNumber;
                }
                #endregion

                #region Invoices

                paymentElement.Invoices = new List<ARPaymentInvoiceElement>();
                List<string> myInvoicesIds = allInvoicesPayments.Where(d => d.ARPaymentId == item.Id).Select(s => s.ARInvoiceId).ToList();

                foreach (ARInvoice myline in allInvoices.Where(d=> myInvoicesIds.Contains(d.Id)))
                {
                    string lineType = invoiceTypes.Where(d => d.Code == myline.ARInvoiceTypeCode).FirstOrDefault().Name;

                    double linePaidAmount = 0;

                    ARInvoicePayment myARInvoicePayment = allInvoicesPayments.Where(d => d.ARPaymentId == item.Id && d.ARInvoiceId == myline.Id).FirstOrDefault();
                    if(myARInvoicePayment != null) {
                        if(myARInvoicePayment.ForeignAmount != null)
                        {
                            linePaidAmount = Math.Round(myARInvoicePayment.ForeignAmount.Value, 2);
                        }
                    }

                    ARPaymentInvoiceElement invoiceElement = new ARPaymentInvoiceElement()
                    {
                        InvoiceType = lineType,
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
        private void BuildXMLFile(ARPaymentRoot myClass, int tenant, string fileName)
        {
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ARPaymentRoot));
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
            else
            {
                Stream blbstr = null;
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

                    //CloudBlobContainer blobContainer = null;
                    //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                    //blobContainer.CreateIfNotExists();

                    //CloudBlockBlob blobfile = blobContainer.GetBlockBlobReference(fileName);

                    //using (blbstr = blobfile.OpenWrite())
                    //{
                    //    StringBuilder stringbuilder = new StringBuilder();
                    //    Encoding encoding = new UTF8Encoding();
                    //    blbstr.Write(bytearray, 0, bytearray.Length);
                    //}
                }
            }
        }
        private void BulidDropBoxXMLLFile(byte[] bytearray)
        {
            DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("ARPayment");
            var payment = this.allEntities.FirstOrDefault();
            var entityId = "";
            if (payment != null)
            {
                entityId = payment.Id;
            }
            var commLog = helper.CreateDropBoxCommunicationLog(tenant, objectTableId, bytearray, this.filename, "ARPayments", "AP Payment",entityId);
        }
        #endregion
    }

    #region GenericInterface
    [XmlRoot("Logitude")]
    public class ARPaymentRoot
    {
        [XmlArray("Payments")]
        [XmlArrayItem("Payment")]
        public List<ARPaymnetElement> Paymnets { get; set; }
    }

    public class ARPaymnetElement
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

        [XmlElement(ElementName = "BillTo")]
        public ARPaymnetCardElement Card { get; set; }

        [XmlArray("Invoices")]
        [XmlArrayItem("Invoice")]
        public List<ARPaymentInvoiceElement> Invoices { get; set; }
    }

    public class ARPaymnetCardElement
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
        public string VatNumber { get; set; }
    }

    public class ARPaymentInvoiceElement
    {
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }
        public double PaidAmount { get; set; }
    }
    #endregion
}


