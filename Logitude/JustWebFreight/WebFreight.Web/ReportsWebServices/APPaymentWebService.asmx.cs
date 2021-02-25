using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices
{ 
    /// <summary>
    /// Summary description for APPaymentWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class APPaymentWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetAPPaymentData(string paymentId, int tenant, string documentTypeId)
        {
            APPaymentDataProvider apPaymentDataProvider = GetAPPaymentDataProvider(paymentId, tenant, documentTypeId);
            XmlSerializer serializer = new XmlSerializer(typeof(APPaymentDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, apPaymentDataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public APPaymentDataProvider GetAPPaymentDataProvider(string paymentId, int tenant, string documentTypeId)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            APPaymentDataProvider apPaymentDataProvider = new APPaymentDataProvider();
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);
            APPaymentRepository paymentRep = new APPaymentRepository(invoiceCotnext);
            APInvoicePaymentRepository invoicePaymentRep = new APInvoicePaymentRepository(invoiceCotnext);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webFreighContext = WebFreightContext.GetContext(tenant);
            APPayment currentPayment = paymentRep.GetSingleAPPayment(paymentId, tenant);
            AddressRepository addressRepository = new AddressRepository(tenant);
            Contact loggedContact = GetLoggedContact(tenant);
            BankAccount bankAccount = GetBankAccountById(currentPayment.BankAccountId, currentPayment.Tenant);
            if (currentPayment != null)
            {
                Tenant tenantSettings = (from a in commonContext.Tenants
                                         where a.Id == currentPayment.Tenant
                                         select a).FirstOrDefault();

                ObjectTable currentObjectTable = (from obj in webFreighContext.ObjectTables
                                                  where obj.Name == "APPayment"
                                                  select obj).FirstOrDefault();
                apPaymentDataProvider.CreateDate = currentPayment.CreateDate;
                apPaymentDataProvider.VendorBankName = currentPayment.VendorBankName;
                apPaymentDataProvider.VendorBankAddress = currentPayment.VendorBankAddress;
                apPaymentDataProvider.VendorSwift = currentPayment.VendorSwift;
                apPaymentDataProvider.VendorBankAccountNumber = currentPayment.VendorBankAccountNumber;
                apPaymentDataProvider.VendorIBANNo = currentPayment.VendorIBANNumber;
               
 
                if (bankAccount != null)
                {
                    if (loggedContact.DontShowLocalLabels)
                    {
                        apPaymentDataProvider.BankAccountName = bankAccount.EnglishName;
                        apPaymentDataProvider.BankAccountEnglishName = bankAccount.EnglishName;

                    }
                    else
                    {
                        apPaymentDataProvider.BankAccountName = bankAccount.LocalName;
                    }

                }

                // tenant data
                if (tenantSettings != null)
                {
                    double offset = 0;
                    if (tenantSettings.TimeZoneOffset != null)
                    {
                        offset = tenantSettings.TimeZoneOffset.Value;
                    }

                    if (tenantSettings.DayLightOffset != 0)
                    {
                        if (tenantSettings.DayLightStartDate.Value.Date <= DateTime.Now.Date && tenantSettings.DayLightEndDate.Value.Date >= DateTime.Now.Date)
                        {
                            offset = offset + tenantSettings.DayLightOffset;
                        }
                    }

                    apPaymentDataProvider.TodayLocal = DateTime.UtcNow.AddHours(offset);
                    apPaymentDataProvider.TenantVat = tenantSettings.VatNumber != null ? tenantSettings.VatNumber : "";
                    apPaymentDataProvider.Signature = tenantSettings.Signature != null ? tenantSettings.Signature : "";

                    Address address = addressRepository.GetSingleAddress(tenantSettings.AddressId, tenant);

                    if (address != null)
                    {
                        apPaymentDataProvider.TelLable = address.PhoneNumber != null ? "Tel:" : "";
                        apPaymentDataProvider.Phone = address.PhoneNumber != null ? address.PhoneNumber : "";
                        apPaymentDataProvider.FaxLable = address.FaxNumber != null ? "Fax:" : "";
                        apPaymentDataProvider.Fax = address.FaxNumber != null ? address.FaxNumber : "";
                        apPaymentDataProvider.TenantData = (tenantSettings.Company != null ? tenantSettings.Company : "") + Environment.NewLine + DataProviders.General.GetAddress(address);
                        apPaymentDataProvider.Company = tenantSettings.Company != null ? tenantSettings.Company : ""; ;
                    }

                    else
                    {
                        apPaymentDataProvider.TenantData = tenantSettings.Company != null ? tenantSettings.Company : "";
                    }
                }

                DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies
                                                     where copy.DocumentTypeId == documentTypeId
                                                     select copy).FirstOrDefault();

                if (currentObjectTable != null)
                {
                    apPaymentDataProvider.APPaymentNo = currentPayment.PaymentNo != null ? currentPayment.PaymentNo : "";
                    
                    //paid to = bill to name + address
                    Card paidToCard = (from a in commonContext.Cards.Include("Airline")
                                       where a.Id == currentPayment.VendorId && a.Tenant == currentPayment.Tenant
                                       select a).FirstOrDefault();

                    if (paidToCard != null)
                    {
                        apPaymentDataProvider.PaidToVatNo = paidToCard.VatNumber != null ? paidToCard.VatNumber : "";
                        apPaymentDataProvider.IRSPlace = paidToCard.IRSPlace;
                        apPaymentDataProvider.IRSNumber = paidToCard.IRSNumber;
                        apPaymentDataProvider.ClientNumber = paidToCard.Code; //client number
                        apPaymentDataProvider.PaidToName = paidToCard.EnglishName;
                        apPaymentDataProvider.BankAddress = paidToCard.BankAddress;
                        apPaymentDataProvider.BankName = paidToCard.BankName;
                        apPaymentDataProvider.BankAccountNumber = paidToCard.AccountNumber;
                        apPaymentDataProvider.IBANNumber = paidToCard.IBANNumber;
                        apPaymentDataProvider.Swift = paidToCard.Swift;
                        apPaymentDataProvider.AccountNumber = GetPartnerAccountNumber(paidToCard);
                        apPaymentDataProvider.PaidToCode = paidToCard.Code;

                        Address address = addressRepository.GetSingleAddress(currentPayment.VendorAddressId, tenant);
                        if (address != null)
                        {
                            apPaymentDataProvider.PaidToAddress = DataProviders.General.GetAddress(address);

                            if (!loggedContact.DontShowLocalLabels)
                            {
                                if (!string.IsNullOrEmpty(paidToCard.LocalName))
                                {
                                    apPaymentDataProvider.PaidTo = (paidToCard.LocalName != null ? paidToCard.LocalName + Environment.NewLine : "") + DataProviders.General.GetAddress(address);
                                }

                                else
                                {
                                    apPaymentDataProvider.PaidTo = (paidToCard.EnglishName != null ? paidToCard.EnglishName + Environment.NewLine : "") + DataProviders.General.GetAddress(address);
                                }
                            }

                            else
                            {
                                apPaymentDataProvider.PaidTo = (paidToCard.EnglishName != null ? paidToCard.EnglishName + Environment.NewLine : "") + DataProviders.General.GetAddress(address);
                            }
                        }

                        if (paidToCard.PartnerTypeId == "CS")
                        {
                            CustomerQuery customerQuery = new CustomerQuery(tenant);
                            CustomerPM customer = customerQuery.GetSinglePM(paidToCard.Id, tenant);
                            if (customer != null)
                            {
                              
                                customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, customer, apPaymentDataProvider);
                            }
                        }
                    }

                    //payment Date
                    if (currentPayment.RegisterDate != null) { apPaymentDataProvider.RegisterDate = currentPayment.RegisterDate.Value.Date; }

                    //print date
                    apPaymentDataProvider.PrintDate = DateTime.Now.Date;

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
                                apPaymentDataProvider.PaymentMethodName = paymentMethod.Name + "-" + currentPayment.CreditCardType.Name;
                            }
                        }
                        else
                        {
                            apPaymentDataProvider.PaymentMethodName = paymentMethod.Name;
                        }
                    }

                    DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
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
                        apPaymentDataProvider.PaidBy = paidByCustomField != null ? paidByCustomField.Value : (paidByDocumentCustom != null ? paidByDocumentCustom.DefaultValue : "");
                    }

                    // payment currency
                    Currency paymentCurrency = (from a in commonContext.Currencies
                                                where a.Id == currentPayment.PaymentCurrencyId
                                                select a).FirstOrDefault();

                    if (paymentCurrency != null)
                    {
                        apPaymentDataProvider.PaymentCurrencyCode = paymentCurrency.Code;
                        apPaymentDataProvider.PaymentCurrencyName = paymentCurrency.LocalName;
                    }

                    this.PrintAmountInWord(currentPayment, apPaymentDataProvider);
                   
                    //print notes
                    apPaymentDataProvider.PrintNotes = currentPayment.PrintNotes != null ? currentPayment.PrintNotes : "";

                    //Issued By User 
                    User createByUserId = (from user in commonContext.Users.Include("Contact")
                                           where user.Id == currentPayment.CreatedByUserId
                                           select user).FirstOrDefault();
                    if (createByUserId != null)
                    {
                        Contact contact = createByUserId.Contact;
                        if (contact != null)
                        {
                            if (loggedContact.DontShowLocalLabels)
                            {
                                apPaymentDataProvider.IssuedByUserName = contact.EnglishName != null ? contact.EnglishName : "";
                            }
                            else
                            {
                                apPaymentDataProvider.IssuedByUserName = contact.LocalName != null ? contact.LocalName : "";
                            }
                        }
                    }



                    if (currentPayment.AccountingPaymentMethod != null)
                    {
                        if (currentPayment.AccountingPaymentMethod.Name == "Cash")
                        {
                            apPaymentDataProvider.ChequeOrPaymentRef = "Cash";
                            apPaymentDataProvider.Bank = "Cash";
                            apPaymentDataProvider.Branch = "Cash";
                            apPaymentDataProvider.Account = "Cash";
                        }
                        else
                        {
                            apPaymentDataProvider.ChequeOrPaymentRef = currentPayment.ChequeOrPaymentRef != null ? currentPayment.ChequeOrPaymentRef : "";
                            apPaymentDataProvider.Bank = currentPayment.Bank != null ? currentPayment.Bank : "";
                            apPaymentDataProvider.Branch = currentPayment.BankBranch != null ? currentPayment.BankBranch : "";
                            apPaymentDataProvider.Account = currentPayment.Account != null ? currentPayment.Account : "";
                        }
                    }
                    else
                    {
                        apPaymentDataProvider = SetDataProviderbankFields(currentPayment, apPaymentDataProvider);
                    }

                    apPaymentDataProvider.ValueDate = currentPayment.ValueDate;

                    if (!string.IsNullOrEmpty(currentPayment.BranchId))
                    {
                        BranchRepository branchRepository = new BranchRepository(tenant);
                        Branch branch = branchRepository.GetSingleBranch(currentPayment.BranchId, tenant);

                        if (branch != null)
                        {
                            apPaymentDataProvider.BranchName = branch.LocalName;
                            if (!string.IsNullOrEmpty(branch.AddressId))
                            {
                                Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                                apPaymentDataProvider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                            }
                        }
                    }

                    //Invoice payments
                    double? totalAmount = 0;
                    List<APInvoiceTotalVAT> APInvoiceTotalVATs = new List<APInvoiceTotalVAT>();
                    apPaymentDataProvider.PaidAPInvoicesList = new List<APPaymentDataProvider.ReportAPInvoicePayments>();

                    List<APInvoicePayment> invoices = invoiceCotnext.APInvoicePayments.Include("APInvoice").Where(inv => inv.APPaymentId == currentPayment.Id && inv.Tenant == currentPayment.Tenant).ToList();

                    var payments = (from a in invoices
                                    group a by new
                                    {
                                        a.APPaymentId,
                                        a.APInvoiceId,
                                        a.APInvoice.InvoiceNumber,
                                        a.APInvoice.MainEntityReference,
                                        a.ForeignAmount,
                                        a.APInvoice.AmountDue,
                                        a.APInvoice.AmountInInvoiceCurrency,
                                        a.APInvoice.InvoiceDate,
                                    } into gr
                                    select new
                                    {
                                        APPaymentId = gr.Key.APPaymentId,
                                        APInvoiceId = gr.Key.APInvoiceId,
                                        InvoiceNumber = gr.Key.InvoiceNumber,
                                        Reference = gr.Key.MainEntityReference,
                                        AmountPaid = gr.Key.ForeignAmount,
                                        AmountDue = gr.Key.AmountDue,
                                        OriginalAmount = gr.Key.AmountInInvoiceCurrency,
                                        InvoiceDate = gr.Key.InvoiceDate,
                                    }).ToList();


                    foreach (var item in payments)
                    {
                        APPaymentDataProvider.ReportAPInvoicePayments apInvoicePayments = new APPaymentDataProvider.ReportAPInvoicePayments();
                        APInvoiceTotalVATs = invoiceCotnext.APInvoiceTotalVATs.Where(a => a.Tenant == currentPayment.Tenant && a.APInvoiceId == item.APInvoiceId).ToList();

                        apInvoicePayments.InvoiceNumber = item.InvoiceNumber == null ? "" : item.InvoiceNumber;
                        apInvoicePayments.Reference = item.Reference == null ? "" : item.Reference;
                        apInvoicePayments.AmountPaid = item.AmountPaid;
                        apInvoicePayments.AmountDue = item.AmountDue;
                        apInvoicePayments.Vat = APInvoiceTotalVATs.Sum(s => (s.InvoiceCurrencyVATAmount));
                        apInvoicePayments.OriginalAmount = item.OriginalAmount;
                        apInvoicePayments.InvoiceData = item.InvoiceDate;
                        apPaymentDataProvider.PaidAPInvoicesList.Add(apInvoicePayments);
                        totalAmount += item.AmountPaid;
                    }

                    apPaymentDataProvider.TotalPayment = currentPayment.AmountInPaymentCurrency;
                    apPaymentDataProvider.TotalAmount = totalAmount;
                    apPaymentDataProvider.OutstandingBalance = currentPayment.AmountInPaymentCurrency - totalAmount;
                    apPaymentDataProvider.Logo = DataProviders.General.GetLogo(tenantSettings.Id);


                    apPaymentDataProvider.DeductionPercentage = currentPayment.TaxDeductionPercentage;
                    if (currentPayment.TaxDeductionLocalAmount != null)
                    {
                        apPaymentDataProvider.DeductionAmount = (double?)currentPayment.TaxDeductionLocalAmount;
                    }
                    if (apPaymentDataProvider.TotalPayment != null)
                    {
                        apPaymentDataProvider.TotalPaymentAfterDeduction = (double?)apPaymentDataProvider.TotalPayment;
                    }
                    if (apPaymentDataProvider.DeductionAmount != null)
                    {
                        if (apPaymentDataProvider.TotalPaymentAfterDeduction != null)
                        {
                            apPaymentDataProvider.TotalPaymentAfterDeduction -= (double?)apPaymentDataProvider.DeductionAmount;
                        }
                        else {
                            apPaymentDataProvider.TotalPaymentAfterDeduction = (double?)apPaymentDataProvider.DeductionAmount;
                        }
                        
                    }
                    
                }
            }

            customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetDataProviderCustomFieldsValues("APPayment", tenant, currentPayment, apPaymentDataProvider);

            return apPaymentDataProvider;
        }

        private APPaymentDataProvider SetDataProviderbankFields(APPayment payment, APPaymentDataProvider apPaymentDataProvider)
        {
            if (payment.AccountingPaymentMethod != null)
            {
                if (payment.AccountingPaymentMethod.Code == "CA")
                {
                    apPaymentDataProvider.ChequeOrPaymentRef = "Cash";
                    apPaymentDataProvider.Bank = "Cash";
                    apPaymentDataProvider.Branch = "Cash";
                    apPaymentDataProvider.Account = "Cash";
                }
                else
                {
                    apPaymentDataProvider.ChequeOrPaymentRef = payment.ChequeOrPaymentRef != null ? payment.ChequeOrPaymentRef : "";
                    apPaymentDataProvider.Bank = payment.Bank != null ? payment.Bank : "";
                    apPaymentDataProvider.Branch = payment.BankBranch != null ? payment.BankBranch : "";
                    apPaymentDataProvider.Account = payment.Account != null ? payment.Account : "";
                }
            }

            return apPaymentDataProvider;

        }
        private Contact GetLoggedContact(int tenant)
        {

            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            return loggedContact;
        }
        private BankAccount GetBankAccountById(string bankId, int tenant)
        {
            BankAccountRepository bankRepository = new BankAccountRepository(tenant);
            BankAccount bankAccount = bankRepository.GetSingleBankAccount(bankId, tenant);
            return bankAccount;

        }

        private string GetPartnerAccountNumber(Card card)
        {
          
            if (card.PartnerTypeId =="AL")
            {
               
             return card.Airline.AccountNumber;
                

            }
            else return null;

        }

        private void PrintAmountInWord(APPayment payment, APPaymentDataProvider apPaymentDataProvider)
        {
            var result = (decimal)payment.AmountInPaymentCurrency - Math.Truncate((decimal)payment.AmountInPaymentCurrency);
            var Firstdigits = (int)(Math.Round(result, 2) * 100);
           
            var FrenchFractions = "";
            if (Firstdigits > 0)
            {
                FrenchFractions = Firstdigits + " Cts";
            }

            NumbersConverterToWords numbersConverterToWords = new NumbersConverterToWords();
            apPaymentDataProvider.TotalPaymentInWordFR = FirstCharToUpper(numbersConverterToWords.NumbersToFrench((int)payment.AmountInPaymentCurrency) + " ") + apPaymentDataProvider.PaymentCurrencyName + " " + FrenchFractions;

        }
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }    
}
