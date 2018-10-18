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
            APPaymentDataProvider apPaymentDataProvider = new APPaymentDataProvider();
            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);
            APPaymentRepository paymentRep = new APPaymentRepository(invoiceCotnext);
            APInvoicePaymentRepository invoicePaymentRep = new APInvoicePaymentRepository(invoiceCotnext);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webFreighContext = WebFreightContext.GetContext(tenant);
            APPayment currentPayment = paymentRep.GetSingleAPPayment(paymentId);
            AddressRepository addressRepository = new AddressRepository(tenant);

            if (currentPayment != null)
            {
                Tenant tenantSettings = (from a in commonContext.Tenants
                                 where a.Id == currentPayment.Tenant
                                 select a).FirstOrDefault();

                ObjectTable currentObjectTable = (from obj in webFreighContext.ObjectTables
                                      where obj.Name == "APPayment" 
                                      select obj).FirstOrDefault();

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
                    //payment number
                    apPaymentDataProvider.APPaymentNo = currentPayment.PaymentNo != null ? currentPayment.PaymentNo : "";

                    //paid to = bill to name + address
                    Card paidToCard = (from a in commonContext.Cards
                                       where a.Id == currentPayment.VendorId
                                       select a).FirstOrDefault();
                     
                    if (paidToCard != null)
                    {
                        apPaymentDataProvider.PaidToVatNo = paidToCard.VatNumber != null ? paidToCard.VatNumber : "";
                        apPaymentDataProvider.IRSPlace = paidToCard.IRSPlace;
                        apPaymentDataProvider.IRSNumber = paidToCard.IRSNumber;
                        apPaymentDataProvider.VendorBankName = paidToCard.BankName;
                        apPaymentDataProvider.VendorBankAddress = paidToCard.BankAddress;
                        apPaymentDataProvider.VendorSwift = paidToCard.Swift;
                        apPaymentDataProvider.VendorBankAccountNumber = paidToCard.AccountNumber;
                        apPaymentDataProvider.VendorIBANNo = paidToCard.IBANNumber;
                        apPaymentDataProvider.ClientNumber = paidToCard.Code; //client number
                        apPaymentDataProvider.PaidToName = paidToCard.EnglishName;

                        Address address = addressRepository.GetSingleAddress(currentPayment.VendorAddressId, tenant);
                        if (address != null)
                        {
                            apPaymentDataProvider.PaidToAddress = DataProviders.General.GetAddress(address);

                            if (address.IsLocalLanguage)
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
                                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                                customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, customer, apPaymentDataProvider);
                            }
                        }
                    }

                    //payment Date
                    if (currentPayment.RegisterDate != null) { apPaymentDataProvider.RegisterDate = currentPayment.RegisterDate.Value.Date; }

                    //print date
                    apPaymentDataProvider.PrintDate = DateTime.Now.Date;

                    // payment method
                    APPaymentMethod paymentMethod = (from a in invoiceCotnext.APPaymentMethods
                                                     where a.Id == currentPayment.PaymentMethodId
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
                    }

                    //print notes
                    apPaymentDataProvider.PrintNotes = currentPayment.PrintNotes != null ? currentPayment.PrintNotes : "";

                    //Issued By User 
                    User createByUserId = (from user in commonContext.Users
                                           where user.Id == currentPayment.CreatedByUserId
                                           select user).FirstOrDefault();
                    if (createByUserId != null)
                    {
                        Contact contact = createByUserId.Contact;
                        if (contact != null)
                        {
                            apPaymentDataProvider.IssuedByUserName = contact.EnglishName != null ? contact.EnglishName : "";
                        }
                    }

                    if (currentPayment.PaymentMethod != null)
                    {
                        if (currentPayment.PaymentMethod.Name == "Cash")
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

                    apPaymentDataProvider.ValueDate = currentPayment.ValueDate;

                    if (!string.IsNullOrEmpty(currentPayment.BranchId))
                    {
                        BranchRepository branchRepository = new BranchRepository(tenant);
                        Branch branch = branchRepository.GetSingleBranch(currentPayment.BranchId, tenant);

                        if (branch != null)
                        {
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
                }
            }

         return apPaymentDataProvider;
        }
    }
}
