using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Xml.Serialization;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataProviders;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.InvoiceModel;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Web;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for PaymentWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PaymentWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] GetPaymentData(string paymentId, int tenant, string documentTypeId)
        {
            PaymentDataProvider paymentDataProvider = GetPaymentDataProvider(paymentId, tenant, documentTypeId);
            XmlSerializer serializer = new XmlSerializer(typeof(PaymentDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, paymentDataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public PaymentDataProvider GetPaymentDataForAPi(string paymentId, int tenant, string documentTypeId)
        {
            PaymentDataProvider paymentDataProvider = GetPaymentDataProvider(paymentId, tenant, documentTypeId);
            
            return paymentDataProvider;
        }

        public PaymentDataProvider GetPaymentDataProvider(string paymentId, int tenant, string documentTypeId)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            PaymentDataProvider paymentDataProvider = new PaymentDataProvider();
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);
            ARPaymentRepository paymentRep = new ARPaymentRepository(invoiceCotnext);
            ARInvoicePaymentRepository invoicePaymentRep = new ARInvoicePaymentRepository(invoiceCotnext);
            BankAccountLiteRepository bankAccountLiteRepository = new BankAccountLiteRepository(invoiceCotnext);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webFreighContext = WebFreightContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(tenant);
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);

            ARPayment currentPayment = (from a in invoiceCotnext.ARPayments.Include("PaymentCurrency").Include("BankAccountLite").Include("CreditCardType")
                                        where a.Id == paymentId
                                        select a).FirstOrDefault();

            if (currentPayment != null)
            {
                SATInterfaceSettingRepository satInterfaceSettingRepository = new SATInterfaceSettingRepository(invoiceCotnext);
                SATInterfaceSetting satSetting = satInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);

                if (!string.IsNullOrEmpty(currentPayment.SATTransferStatusCode))
                {
                    SATTransferStatusRepository satTransferStatusRep = new SATTransferStatusRepository(tenant);
                    SATTransferStatus satStatus = satTransferStatusRep.GetSingleSATTransferStatus(currentPayment.SATTransferStatusCode);
                    paymentDataProvider.SATTransferStatus = satStatus != null ? satStatus.Name : null;
                }

                paymentDataProvider.BankName = currentPayment.Bank;
                paymentDataProvider.BankAccountNumber = currentPayment.Account;
                paymentDataProvider.PaymentExchangeRate = currentPayment.PaymentCurrencyExchangeRate;
                if (!string.IsNullOrEmpty(currentPayment.BankAccountLiteId))
                {
                    BankAccountLite bankAccount = bankAccountLiteRepository.GetSingleBankAccountLite(currentPayment.BankAccountLiteId, tenant);
                    if (bankAccount != null)
                    {
                        paymentDataProvider.DepositBankName = !string.IsNullOrEmpty(bankAccount.LocalName) ? bankAccount.LocalName : bankAccount.EnglishName;
                        paymentDataProvider.DepositBankAccountNumber = bankAccount.AccountNumber;
                        paymentDataProvider.VATNumber = bankAccount.VatNumber;
                    }

                }

                ObjectTable currentObjectTable = null;

                Tenant tenantSettings = (from a in commonContext.Tenants
                                         where a.Id == currentPayment.Tenant
                                         select a).FirstOrDefault();


                currentObjectTable = (from obj in webFreighContext.ObjectTables
                                      where obj.Name == "ARPayment"
                                      select obj).FirstOrDefault();
                // tenant data
                if (tenantSettings != null)
                {
                    paymentDataProvider.Address1 = tenantSettings.InvoiceSection1;
                    paymentDataProvider.Address2 = tenantSettings.InvoiceSection2;
                    paymentDataProvider.TenantName = tenantSettings.Company != null ? tenantSettings.Company : "";
                    paymentDataProvider.TenantVatNo = tenantSettings.VatNumber != null ? tenantSettings.VatNumber : "";
                    paymentDataProvider.Signature = tenantSettings.Signature != null ? tenantSettings.Signature : "";

                    Address address = addressRepository.GetSingleAddress(tenantSettings.AddressId, tenant);

                    if (address != null)
                    {
                        paymentDataProvider.TenantAddress = paymentDataProvider.TenantName + DataProviders.General.GetAddress(address);
                        paymentDataProvider.TelLable = address.PhoneNumber != null ? "Tel:" : "";
                        paymentDataProvider.Phone = address.PhoneNumber != null ? address.PhoneNumber : "";

                        paymentDataProvider.FaxLable = address.FaxNumber != null ? "Fax:" : "";
                        paymentDataProvider.Fax = address.FaxNumber != null ? address.FaxNumber : "";

                        paymentDataProvider.TenantData = paymentDataProvider.TenantName + Environment.NewLine + DataProviders.General.GetAddress(address);
                    }

                    else
                    {
                        paymentDataProvider.TenantData = paymentDataProvider.TenantName;
                    }
                }

                if (currentPayment.BankAccountLite != null)
                {
                    paymentDataProvider.BankCode = currentPayment.BankAccountLite.BankCode;
                    paymentDataProvider.BankAccountEnglishName = currentPayment.BankAccountLite.EnglishName;
                    paymentDataProvider.BankAccountLocalName = currentPayment.BankAccountLite.LocalName;
                    paymentDataProvider.AccountNumber = currentPayment.BankAccountLite.AccountNumber;
                    paymentDataProvider.BranchNumber = currentPayment.BankAccountLite.BranchNumber;
                }

                DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies
                                                     where copy.DocumentTypeId == documentTypeId
                                                     select copy).FirstOrDefault();
                //received from = bill to name + address
                Card billToCard = (from a in commonContext.Cards
                                   where a.Id == currentPayment.BillToId
                                   select a).FirstOrDefault();

                if (currentObjectTable != null)
                {
                    if (documenttypecopy != null)
                    {
                        paymentDataProvider.CopyName = documenttypecopy.Name != null ? documenttypecopy.Name : "";
                    }

                    //payment number
                    paymentDataProvider.PaymentNo = currentPayment.PaymentNo != null ? currentPayment.PaymentNo : "";

                    Contact loggedContact = GetLoggedContact(currentPayment.Tenant);

                    if (billToCard != null)
                    {
                        paymentDataProvider.BillToName = billToCard.EnglishName != null ? billToCard.EnglishName + Environment.NewLine : "";
                        paymentDataProvider.BillToVatNo = billToCard.VatNumber != null ? billToCard.VatNumber : "";
                        paymentDataProvider.IRSPlace = billToCard.IRSPlace;
                        paymentDataProvider.IRSNumber = billToCard.IRSNumber;

                        Address address = addressRepository.GetSingleAddress(currentPayment.BillToAddressId, tenant);

                        if (address != null)
                        {


                            if (!loggedContact.DontShowLocalLabels)
                            {
                                if (!string.IsNullOrEmpty(billToCard.LocalName))
                                {
                                    paymentDataProvider.BillToName = billToCard.LocalName != null ? billToCard.LocalName + Environment.NewLine : "";
                                }
                            }


                            paymentDataProvider.BillToAddress = paymentDataProvider.BillToName + DataProviders.General.GetAddress(address);
                        }

                        //client number
                        paymentDataProvider.ClientNumber = billToCard.Code;

                        // accounting card
                        paymentDataProvider.AccountingCard = billToCard.ReceivablesAccountingCard;
                        paymentDataProvider.ReceivedFrom = paymentDataProvider.BillToName + DataProviders.General.GetAddress(address);
                        paymentDataProvider.ReceivedFromInLocal = billToCard.LocalName + Environment.NewLine + DataProviders.General.GetAddress(address);
                        if (billToCard.PartnerTypeId == "CS")
                        {
                            CustomerQuery customerQuery = new CustomerQuery(tenant);
                            CustomerPM customer = customerQuery.GetSinglePM(billToCard.Id, tenant);
                            if (customer != null)
                            {

                                customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, customer, paymentDataProvider);
                            }
                        }
                    }

                    //payment Date
                    if (currentPayment.RegisterDate != null)
                    {
                        paymentDataProvider.RegisterDate = currentPayment.RegisterDate.Value.Date;
                        paymentDataProvider.PaymentDate = currentPayment.RegisterDate.Value.Date;
                    }

                    //print date
                    //paymentDataProvider.PrintDate = String.Format("{0:dd MMM yyyy}", DateTime.Now.Date);
                    paymentDataProvider.PrintDate = DateTime.Now.Date;

                    //paymentDataProvider.Today = String.Format("{0:dd MMM yyyy}", DateTime.Now.Date);
                    paymentDataProvider.Today = DateTime.Now.Date;
                    // payment method
                    AccountingPaymentMethod paymentMethod = (from a in invoiceCotnext.AccountingPaymentMethods
                                                             where a.Id == currentPayment.AccountingPaymentMethodId
                                                             select a).FirstOrDefault();
                    if (paymentMethod != null)
                    {
                        if (paymentMethod.Code == "CC")
                        {
                            if (currentPayment.CreditCardType != null)
                            {
                                paymentDataProvider.PaymentMethodName = paymentMethod.Name + "-" + currentPayment.CreditCardType.Name;
                            }
                        }
                        else
                        {
                            paymentDataProvider.PaymentMethodName = paymentMethod.Name;
                        }
                    }

                    // Local Name
                    if (paymentMethod != null)
                    {
                        paymentDataProvider.PaymentMethodLocalName = GetPaymentMethodLocalName(paymentMethod.Code);
                    }

                    // payment status
                    ARPaymentStatus paymentStatus = (from a in invoiceCotnext.ARPaymentStatus
                                                     where a.Code == currentPayment.StatusCode
                                                     select a).FirstOrDefault();
                    if (paymentStatus != null)
                    {
                        paymentDataProvider.StatusName = paymentStatus.Name;
                    }

                    DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
                    DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);
                    DocumentTypePM documentTypePM = documentTypeQuery.GetSingelDocumentTypeById(documentTypeId, tenant);

                    if (documentTypePM != null)
                    {
                        List<FormCustomField> customfieldsList = commonContext.FormCustomFields.Where(fc => fc.DocumentTypeId == documentTypePM.Id).ToList();

                        List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.Where(fc => fc.DocumentTypeId == documentTypePM.Id).ToList();

                        FormCustomField paidByCustomField = (from a in customfieldsList
                                                             where a.FieldCode == "PaidBy" && a.EntityId == currentPayment.Id
                                                             select a).FirstOrDefault();

                        DocumentTypeCustomField paidByDocumentCustom = (from a in documentCustomfieldsList
                                                                        where a.FieldCode == "PaidBy"
                                                                        select a).FirstOrDefault();
                        //paid by   
                        paymentDataProvider.PaidBy = paidByCustomField != null ? paidByCustomField.Value : (paidByDocumentCustom != null ? paidByDocumentCustom.DefaultValue : "");
                    }

                    // payment currency
                    Currency paymentCurrency = (from a in commonContext.Currencies
                                                where a.Id == currentPayment.PaymentCurrencyId
                                                select a).FirstOrDefault();

                    string paymentCurrencyLocalName = "";

                    if (paymentCurrency != null)
                    {
                        paymentDataProvider.PaymentCurrencyCode = paymentCurrency.Code;

                        paymentCurrencyLocalName = paymentCurrency.LocalName;
                    }

                    //internal notes
                    paymentDataProvider.InternalNotes = currentPayment.InternalNotes != null ? currentPayment.InternalNotes : "";

                    paymentDataProvider.PrintNotes = currentPayment.PrintNotes != null ? currentPayment.PrintNotes : "";

                    //Crate by user
                    ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(tenant);
                    User createByUserId = aRPaymentQuery.getUserByARPayment(currentPayment);


                    if (createByUserId != null)
                    {
                        Contact contact = createByUserId.Contact;
                        if (contact != null)
                        {
                            paymentDataProvider.IssuedByUserName = contact.EnglishName != null ? contact.EnglishName : "";
                            paymentDataProvider.IssuedByLocalName = contact.LocalName != null ? contact.LocalName : "";
                        }
                    }

                    //payment Lines
                    if (currentPayment.AccountingPaymentMethod != null)
                    {
                        TenantRepository tenantRepository = new TenantRepository(tenant);
                        Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);

                        if (currentPayment.AccountingPaymentMethod.Name == "Cash")
                        {
                            paymentDataProvider.ChequeOrPaymentRef = "Cash";
                            paymentDataProvider.Bank = "Cash";
                            paymentDataProvider.Branch = "Cash";
                            paymentDataProvider.Account = "Cash";
                        }
                        else if (tenantPOCO.AccountingActivated && currentPayment.AccountingPaymentMethod.Name.ToLower() == "bank transfer")
                        {
                            paymentDataProvider = SetBankData(currentPayment, paymentDataProvider);
                        }
                        else
                        {
                            paymentDataProvider.ChequeOrPaymentRef = currentPayment.ChequeOrPaymentRef != null ? currentPayment.ChequeOrPaymentRef : "";
                            paymentDataProvider.Bank = currentPayment.Bank != null ? currentPayment.Bank : "";
                            paymentDataProvider.Branch = currentPayment.BankBranch != null ? currentPayment.BankBranch : "";
                            paymentDataProvider.Account = currentPayment.Account != null ? currentPayment.Account : "";
                        }
                    }

                    if (currentPayment.AccountingPaymentMethod != null)
                    {
                        if (currentPayment.AccountingPaymentMethod.Name == "Cash")
                        {
                            paymentDataProvider.ValueDate = currentPayment.RegisterDate.Value.Date;
                        }
                        else
                        {
                            paymentDataProvider.ValueDate = currentPayment.ValueDate.Value.Date;
                        }
                    }
                    else
                    {
                        paymentDataProvider.ValueDate = currentPayment.ValueDate.Value.Date;
                    }

                    paymentDataProvider.TotalPayment = currentPayment.AmountInPaymentCurrency;

                    if (!string.IsNullOrEmpty(currentPayment.BranchId))
                    {
                        BranchRepository branchRepository = new BranchRepository(tenant);
                        Branch branch = branchRepository.GetSingleBranch(currentPayment.BranchId, tenant);

                        if (branch != null)
                        {
                            if (!string.IsNullOrEmpty(branch.AddressId))
                            {
                                Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                                paymentDataProvider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                            }
                        }
                    }



                    //Invoice payments
                    double? totalAmount = 0;
                    List<ARInvoiceTotalVAT> ARInvoiceTotalVATs = new List<ARInvoiceTotalVAT>();
                    paymentDataProvider.PaidInvoicesList = new List<PaymentDataProvider.InvoicePayments>();
                    List<ARInvoicePayment> invoices = invoiceCotnext.ARInvoicePayments.Include("ARInvoice").Where(inv => inv.ARPaymentId == currentPayment.Id && inv.Tenant == currentPayment.Tenant).ToList();

                    var payments = (from a in invoices
                                    group a by new
                                    {
                                        a.ARPaymentId,
                                        a.ARInvoiceId,
                                        a.ARInvoice.InvoiceNumber,
                                        a.ARInvoice.MainEntityReference,
                                        a.PaymentAmount,
                                        a.ARInvoice.AmountDue,
                                        a.ARInvoice.AmountInInvoiceCurrency,
                                        a.ARInvoice
                                    } into gr
                                    select new
                                    {
                                        ARPaymentId = gr.Key.ARPaymentId,
                                        ARInvoiceId = gr.Key.ARInvoiceId,
                                        InvoiceNumber = gr.Key.InvoiceNumber,
                                        Reference = gr.Key.MainEntityReference,
                                        AmountPaid = gr.Key.PaymentAmount,
                                        AmountDue = gr.Key.AmountDue,
                                        OriginalAmount = gr.Key.AmountInInvoiceCurrency,
                                        HouseNumber = gr.Key.ARInvoice.HouseNumber,
                                        InvoiceCurrencyId = gr.Key.ARInvoice.InvoiceCurrencyId,
                                        ExchangeRate = gr.Key.ARInvoice.InvoiceCurrencyExchangeRate,
                                    }).ToList();

                    foreach (var item in payments)
                    {
                        PaymentDataProvider.InvoicePayments reportPayments = new PaymentDataProvider.InvoicePayments();
                        ARInvoiceTotalVATs = invoiceCotnext.ARInvoiceTotalVATs.Where(a => a.Tenant == currentPayment.Tenant && a.ARInvoiceId == item.ARInvoiceId).ToList();

                        reportPayments.InvoiceNumber = item.InvoiceNumber == null ? "" : item.InvoiceNumber;
                        reportPayments.Reference = item.Reference == null ? "" : item.Reference;
                        reportPayments.HAWB = item.HouseNumber == null ? "" : item.HouseNumber;
                        reportPayments.AmountPaid = item.AmountPaid;
                        reportPayments.AmountDue = item.AmountDue;
                        reportPayments.Vat = ARInvoiceTotalVATs.Sum(s => (s.InvoiceCurrencyVatableAmount * s.VatPercent) / 100);
                        reportPayments.OriginalAmount = item.OriginalAmount;
                        reportPayments.InvoicePaymentExchangeRate = item.ExchangeRate;

                        Currency myCurrency = CurrencyRepository.GetSingleCurrency(item.InvoiceCurrencyId, tenant, true);
                        if (myCurrency != null)
                        {
                            reportPayments.InvoiceCurrency = myCurrency.Code;
                        }

                        paymentDataProvider.PaidInvoicesList.Add(reportPayments);
                        totalAmount += item.AmountPaid;
                    }

                    paymentDataProvider.TotalAmount = totalAmount;

                    this.PrintTotalAmountInEnglishAndSpanish(paymentDataProvider, paymentCurrencyLocalName);

                    paymentDataProvider.OutstandingBalance = currentPayment.AmountInPaymentCurrency - totalAmount;
                    paymentDataProvider.Logo = DataProviders.General.GetLogo(tenantSettings.Id);
                }

                if (!string.IsNullOrEmpty(currentPayment.BillToAddressId))
                {
                    ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(currentPayment.Tenant);
                    AddressRepository addressReposirory = new AddressRepository(currentPayment.Tenant);

                    Address billToAddress = addressReposirory.GetSingleAddress(currentPayment.BillToAddressId, currentPayment.Tenant);
                    string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
                    if (billToAddress.Country != null)
                    {
                        paymentDataProvider.BillToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
                    }
                }

                if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33") && tenantSettings != null)
                {
                    this.MapPaymentProfact33Fields(currentPayment, paymentDataProvider, tenantSettings, invoiceCotnext, billToCard);
                }

                paymentDataProvider.AmountInLocalCurrency = currentPayment.AmountInLocalCurrency;

            }

            FillARPaymentCheques(paymentId, tenant, paymentDataProvider);
            SetFullAccountingBankName(currentPayment,paymentDataProvider);

            customFieldResolver.SetDataProviderCustomFieldsValues("ARPayment", tenant, currentPayment, paymentDataProvider);

            return paymentDataProvider;
        }

        private void SetFullAccountingBankName(ARPayment payment, PaymentDataProvider paymentDataProvider)
        {
            Contact loggedContact = GetLoggedContact(payment.Tenant);

            BankAccountPM bankAccount = GetBankAccountPM(payment);
            if (bankAccount != null)
            {
                paymentDataProvider.FullAccountingBankEnglishName = bankAccount.EnglishName;
                paymentDataProvider.FullAccountingBankLocalName = bankAccount.LocalName;

                if (loggedContact.DontShowLocalLabels)
                {
                    paymentDataProvider.FullAccountingBankName = bankAccount.EnglishName;
                }
                else
                {
                    paymentDataProvider.FullAccountingBankName = bankAccount.LocalName;
                }
            }
        }

        private PaymentDataProvider SetBankData(ARPayment payment, PaymentDataProvider paymentDataProvider)
        {
            BankAccountPM bankAccount = GetBankAccountPM(payment);
            if (bankAccount != null)
            {
                paymentDataProvider.Branch = bankAccount.BranchNumber;
                paymentDataProvider.Account = bankAccount.AccountNumber;
                paymentDataProvider.Bank = GetBankName(bankAccount.BankId, bankAccount.Tenant);
            }
            return paymentDataProvider;

        }
        private static BankAccountPM GetBankAccountPM(ARPayment payment)
        {
            BankAccountQueryService bankAccountRepository = new BankAccountQueryService(payment.Tenant);
            BankAccountPM bankAccount = bankAccountRepository.GetSingle(payment.BankAccountId, false, false);
            return bankAccount;
        }

        private string GetBankName(string id , int tenant)
        {
            BankCodeQueryService bankCodeQueryService = new BankCodeQueryService(tenant);
            BankCodePM bankCode = bankCodeQueryService.GetSingle(id, false, false);
            return bankCode != null ? bankCode.LocalName : null;

        }
        private Contact GetLoggedContact(int tenant)
        {
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            return loggedContact;
        }
        private void PrintTotalAmountInEnglishAndSpanish(PaymentDataProvider paymentDataProvider, string paymentCurrencyLocalName)
        {
            NumbersConverterToWords numbersConverterToWords = new NumbersConverterToWords();
            var resultOfTotalAmount = decimal.Parse(paymentDataProvider.TotalAmount + "") - Math.Truncate(decimal.Parse(paymentDataProvider.TotalAmount + ""));
            var resulyFirstdigits = (int)(Math.Round(resultOfTotalAmount, 2) * 100);
            string resultstr = "";
            if (resulyFirstdigits < 10 && resulyFirstdigits > 0)
                resultstr = 0 + "" + resulyFirstdigits + "/100";
            else
                resultstr = resulyFirstdigits + "/100";

            if ((int)(Math.Round(resultOfTotalAmount, 2) * 100) <= 0)
            {
                resultstr = "";
            }
            paymentDataProvider.TotalAmountInWordsSpanish = numbersConverterToWords.NumbersToSpanish((int)paymentDataProvider.TotalAmount) + " " + paymentCurrencyLocalName + " " + resultstr;
            paymentDataProvider.TotalAmountInWordsEnglish = numbersConverterToWords.NumbersToEnglish((int)paymentDataProvider.TotalAmount) + " " + paymentCurrencyLocalName + " " + resultstr;
        }

        private string GetPaymentMethodLocalName(string code)
        {
            string translation = ""; 
            if(code == "CH")
            {
                translation = "המחאה"; 
            }
            else if (code == "CA")
            {
                translation = "מזומן";
            }
            else if (code == "BT")
            {
                translation = "העברה בנקאית";
            }
            else if (code == "CC")
            {
                translation = "כרטיס אשראי";
            }
            else if (code == "FS")
            {
                translation = "קיזוז";
            }
            return translation;
        }

        private void MapPaymentProfact33Fields(ARPayment currentPayment, PaymentDataProvider paymentDataProvider, Tenant tenantSettings, IInvoiceContext invoiceCotnext, Card billToCard)
        {
			if (billToCard != null)
			{
				paymentDataProvider.SAT.SATForeignRFC = (billToCard.SATForeignRFC ?? null);
                paymentDataProvider.ForeignRFC = (billToCard.SATForeignRFC ?? null);
            }

			if (!string.IsNullOrEmpty(currentPayment.SATXML))
            {
                UsoCFDIRepository usoCFDIRepository = new UsoCFDIRepository(currentPayment.Tenant);
                List<UsoCFDI> allUsoCFDIs = usoCFDIRepository.GetUsoCFDIs().ToList();
                Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(currentPayment.SATXML);
                //invoicedataprovider.SelloSAT = comprobante.sellocurrentPayment
                //invoicedataprovider.NoCertificadoSAT = comprobante.noCertificado;
                if (comprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        //Profact.TimbraCFDI.get
                        //public string FormaPago { get; set; }
                        //public string UsoCFDI { get; set; }
                        //public string Fecha { get; set; }
                        //public string Serie { get; set; }
                        //public string Folio { get; set; }
                        //public string TipoDeComprobante { get; set; }
                        //public string RegimenFiscal { get; set; }
                        //public string LugarExpedicion { get; set; }
                        //public string NoCertificado { get; set; }
                        //public string Certificado { get; set; }
                        //public string FechaTimbrado { get; set; }
                        //public string NoCertificadoSAT { get; set; }
                        //public string SelloCFD { get; set; }
                        //public string SelloSAT { get; set; }
                        //public System.Drawing.Image QRImage { get; set; }
                       
                        List<SATPaymentMethod> allPaymentMethods = (from d in invoiceCotnext.SATPaymentMethods select d).ToList();

                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        if (!string.IsNullOrEmpty(currentPayment.SATPaymentMethodCode))
                        {
                            SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == currentPayment.SATPaymentMethodCode).FirstOrDefault();
                            paymentDataProvider.SAT.FormaPago = satPaymentMethod != null ? (satPaymentMethod.Code + "," + satPaymentMethod.LocalName) : comprobante.FormaPago;
                        }

                        UsoCFDI usoCFDI = allUsoCFDIs.FirstOrDefault(f => f.Code == comprobante.Receptor.UsoCFDI);
                        if (usoCFDI != null)
                        {
                            paymentDataProvider.SAT.UsoCFDI = usoCFDI.Code + " " + usoCFDI.Name;
                        }

                        paymentDataProvider.SAT.Fecha = comprobante.Fecha;
                        paymentDataProvider.SAT.Serie = comprobante.Serie;
                        paymentDataProvider.SAT.Folio = comprobante.Folio;
                        if (comprobante.TipoDeComprobante == "E")
                        {
                            paymentDataProvider.SAT.TipoDeComprobante = "Egreso";
                        }
                        else
                        {
                            paymentDataProvider.SAT.TipoDeComprobante = "Ingreso";
                        }

                        if (comprobante.Emisor.RegimenFiscal != null && comprobante.Emisor.RegimenFiscal.Length > 0)
                        {
                            paymentDataProvider.SAT.RegimenFiscal = comprobante.Emisor.RegimenFiscal;
                        }
                        paymentDataProvider.SAT.LugarExpedicion = comprobante.LugarExpedicion;
                        paymentDataProvider.SAT.NoCertificado = comprobante.NoCertificado;
                        paymentDataProvider.SAT.Certificado = comprobante.Certificado;
                        paymentDataProvider.SAT.FechaTimbrado = digitalTi.FechaTimbrado;
                        paymentDataProvider.SAT.NoCertificadoSAT = GetSATTimbreFiscalDigitalValue("NoCertificadoSAT", timbreFiscalDigitalElement);//digitalTi.noCertificadoSAT;
                        paymentDataProvider.SAT.SelloCFD = GetSATTimbreFiscalDigitalValue("SelloCFD", timbreFiscalDigitalElement);//digitalTi.selloCFD;
                        paymentDataProvider.SAT.SelloSAT = GetSATTimbreFiscalDigitalValue("SelloSAT", timbreFiscalDigitalElement); //digitalTi.selloSAT;
                        paymentDataProvider.SAT.UUID = digitalTi.UUID;
                        if (!string.IsNullOrEmpty(comprobante.MetodoPago))
                        {
                            paymentDataProvider.SAT.MetodoPago = (comprobante.MetodoPago == "PUE" ? "PUE Pago en una sola exhibición" : "PPD Pago en parcialidades o diferido");
                        }

                        if (!string.IsNullOrEmpty(currentPayment.SATAdditionalFieldsXML))
                        {
                            SATAdditionalFields additionalFields = LogitudeXmlSerializer.DeserializeObject<SATAdditionalFields>(currentPayment.SATAdditionalFieldsXML);
                            paymentDataProvider.SAT.CadenaOriginal = additionalFields.CadenaOriginal;
                            if (!string.IsNullOrEmpty(additionalFields.QRImage))
                            {
                                paymentDataProvider.SAT.QRImage = Image.FromStream(new MemoryStream(Convert.FromBase64String(additionalFields.QRImage)));
                            }
                        }

                        if (comprobante.PagosSpecified && comprobante.Complemento.Any != null)
                        {
                            List<XmlElement> LXmlComplementos = comprobante.Complemento.Any.ToList();
                            XmlElement documentElement = LXmlComplementos.First();

                            Profact.TimbraCFDI33.Complementos.Pagos10.Pagos pagos = Profact.TimbraCFDI.XMLUtilerias.DeserializaObjeto<Profact.TimbraCFDI33.Complementos.Pagos10.Pagos>(documentElement.OuterXml);
                            Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago pagoItem = pagos.Pago.ToList().FirstOrDefault();
                            if (pagoItem != null)
                            {
                                paymentDataProvider.SAT.NumOperacion = pagoItem.NumOperacion;
                                paymentDataProvider.SAT.FechaPago = pagoItem.FechaPago;
                                paymentDataProvider.SAT.Monto = pagoItem.Monto;

                                paymentDataProvider.SAT.TipoCadenaPago = pagoItem.TipoCadPago;
                                paymentDataProvider.SAT.CadPago = pagoItem.CadPago;

                                //Encoding encoding = Encoding.UTF8;
                               // if (pagoItem.CertPago != null)
                                    paymentDataProvider.SAT.CertPago = pagoItem.CertPago;//encoding.GetString(pagoItem.CertPago);

                               // if (pagoItem.SelloPago != null)
                                    paymentDataProvider.SAT.SelloPago = pagoItem.SelloPago;//encoding.GetString(pagoItem.SelloPago);



                                foreach (Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado doctoItem in pagoItem.DoctoRelacionado.ToList())
                                {

                                    PaymentDataProvider.InvoicePayments invoicePayment = null;
                                    string invoicenumber = doctoItem.Folio;
                                    if (!string.IsNullOrEmpty(doctoItem.Serie) && doctoItem.Serie != "A")
                                    {
                                        invoicenumber = doctoItem.Serie + doctoItem.Folio;
                                        invoicePayment = paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber);
                                    }
                                    else
                                    {
                                        invoicePayment = paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber);
                                        if(invoicePayment == null)
                                        {
                                            invoicenumber = doctoItem.Serie + doctoItem.Folio;
                                            invoicePayment = paymentDataProvider.PaidInvoicesList.FirstOrDefault(i => i.InvoiceNumber == invoicenumber);
                                        }

                                    }
                                    if (invoicePayment != null)
                                    {
                                        invoicePayment.UUID = doctoItem.IdDocumento;
                                        invoicePayment.CurrencyCode = doctoItem.MonedaDR;
                                        invoicePayment.TipoCambio = doctoItem.TipoCambioDR;
                                        invoicePayment.MetodoPagoCode = doctoItem.MetodoDePagoDR;
                                        invoicePayment.Serie = doctoItem.Serie;
                                        invoicePayment.Folio = doctoItem.Folio;
                                        invoicePayment.NumParcialidad = doctoItem.NumParcialidad;
                                        invoicePayment.ImpSaldoAnt = doctoItem.ImpSaldoAnt;
                                        invoicePayment.ImpPagado = doctoItem.ImpPagado;
                                        invoicePayment.ImpSaldoInsoluto = doctoItem.ImpSaldoInsoluto;
                                    }
                                }
                            }
                           
                        }
                    }

                }


                //foreach(PaymentDataProvider.InvoicePayments invoicePayment in paymentDataProvider.PaidInvoicesList)
                //{
                //    ARInvoice invoice = invoiceCotnext.ARInvoices.FirstOrDefault(d => d.InvoiceNumber == invoicePayment.InvoiceNumber && d.Tenant == tenantSettings.Id);
                //    if(invoice != null && !string.IsNullOrEmpty(invoice.SATXML))
                //    {
                //        MapPaymentInvoiceProfact33Fields(currentPayment, invoice, invoicePayment, tenantSettings);
                //    }
                //}
            }
        }

        //private void MapPaymentInvoiceProfact33Fields(ARPayment currentPayment, ARInvoice currentInvoice, PaymentDataProvider.InvoicePayments invoicePaymentDataProvider, Tenant tenantSettings)
        //{
           

        //    UsoCFDIRepository usoCFDIRepository = new UsoCFDIRepository(currentInvoice.Tenant);
        //    List<UsoCFDI> allUsoCFDIs = usoCFDIRepository.GetUsoCFDIs().ToList();
        //    Profact.TimbraCFDI33.Comprobante invoiceComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(currentInvoice.SATXML);
        //    invoicePaymentDataProvider.CurrencyCode = invoiceComprobante.Moneda;
        //    invoicePaymentDataProvider.TipoCambio = invoiceComprobante.TipoCambio;
        //    invoicePaymentDataProvider.MetodoPagoCode = invoiceComprobante.MetodoPago;
        //    invoicePaymentDataProvider.Serie = invoiceComprobante.Serie;

        //    if (invoiceComprobante.Complemento.Any != null)
        //    {
        //        List<System.Xml.XmlElement> myLXmlComplementos = invoiceComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
        //        var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
        //        if (timbreFiscalDigitalElement != null)
        //        {
        //            Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
        //            invoicePaymentDataProvider.UUID = digitalTi.UUID;
        //        }
        //    }
        //}

        private  string GetSATTimbreFiscalDigitalValue(string attributeName, System.Xml.XmlElement timbreFiscalDigitalElement)
        {
            string value = "";
            if (timbreFiscalDigitalElement != null && timbreFiscalDigitalElement.Attributes[attributeName] != null)
            {
                value = timbreFiscalDigitalElement.Attributes[attributeName].Value;
            }

            return value;
        }

        private  void FillARPaymentCheques(string paymentId, int tenant, PaymentDataProvider paymentDataProvider)
        {
            List<ARPaymentChequePM> arPaymentChequesPMs = GetAllARPaymentCheques(paymentId, tenant);
            paymentDataProvider.ARPaymentCheques = new List<PaymentDataProvider.ARPaymentCheque>();
            CreateARPaymentCheques(paymentDataProvider, arPaymentChequesPMs);
        }

        private  List<ARPaymentChequePM> GetAllARPaymentCheques(string paymentId, int tenant)
        {
            ARPaymentChequeQueryService aRPaymentChequeQuery = new ARPaymentChequeQueryService(tenant);
            List<ARPaymentChequePM> arPaymentCheques = aRPaymentChequeQuery.GetListByPaymentId(paymentId, tenant);
            return arPaymentCheques;
        }

        private  void CreateARPaymentCheques(PaymentDataProvider paymentDataProvider, List<ARPaymentChequePM> arPaymentCheques)
        {
            foreach (ARPaymentChequePM cheque in arPaymentCheques)
            {
                AddChequeToARPaymentCheques(paymentDataProvider, MapPaymentChequeFieldsByEntityPM(cheque));
            }
        }

        private  void AddChequeToARPaymentCheques(PaymentDataProvider paymentDataProvider, PaymentDataProvider.ARPaymentCheque ChequeFromDataProvider)
        {
            paymentDataProvider.ARPaymentCheques.Add(ChequeFromDataProvider);
        }

        private  PaymentDataProvider.ARPaymentCheque MapPaymentChequeFieldsByEntityPM(ARPaymentChequePM chequePM)
        {
            PaymentDataProvider.ARPaymentCheque cheque = new PaymentDataProvider.ARPaymentCheque();
            cheque.ChequeOrPaymentRef = chequePM.ChequeNumber;
            cheque.Bank = chequePM.BankId;
            cheque.Branch = chequePM.BankBranch;
            cheque.Account = chequePM.BankAccount;
            cheque.ValueDate = chequePM.ValueDate.ToShortDateString();
            cheque.CurrencyCode = chequePM.CurrencyCode;
            cheque.LocalAmount = chequePM.LocalAmount;
            cheque.ForeignAmount = chequePM.ForeignAmount;
            cheque.StatusName = chequePM.StatusName;
            return cheque;
        }

    }
}
