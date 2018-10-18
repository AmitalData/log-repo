using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.TaxesApprovalModel
{
    /// <summary>
    /// Summary description for TaxesApprovalWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TaxesApprovalWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public bool GetTaxApprovalData(DateTime date1, DateTime date2, int tenant, string random, string email)
        {
            //IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);            
            //TenantRepository tenantRep = new TenantRepository(tenant);            
            //Tenant tenantPoco = tenantRep.GetSingleTenant(tenant);

            //string computed = this.Compute(date1, date2, tenantPoco, invoiceCotnext, random);
            //byte[] bytearray = this.ConvertEncoding(computed);

            ContactQuery contactQuery = new ContactQuery(tenant);

            BrokeredMessage message = new BrokeredMessage();
            message.Properties["ReportId"] = random;
            message.Properties["UserName"] = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant).EnglishName;
            message.Properties["Tenant"] = tenant;
            message.Properties["Email"] = email;
            message.Properties["Date1"] = date1;
            message.Properties["Date2"] = date2;


            string taxqueueName = WebFreightEntryPoint.GetQueueByEnviroment("taxdataqueue"); 


            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(taxqueueName);

            client.Send(message);


            return true;
        }

       // private string Compute(DateTime? date1, DateTime? date2, Tenant tenant, IInvoiceContext invoiceCotnext, string random)
       // {
       //     ARPaymentRepository paymentRep = new ARPaymentRepository(invoiceCotnext);
       //     List<ARPayment> payments = paymentRep.GetARPaymentsByTenant(tenant.Id).Where(p => (p.RegisterDate >= date1 && p.RegisterDate <= date2)).ToList();

       //     ARInvoiceRepository invoiceRep = new ARInvoiceRepository(invoiceCotnext);
       //     List<ARInvoice> invoices = invoiceRep.GetInvoicesByTenant(tenant.Id).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2)).ToList();

       //     APInvoiceRepository apInvoiceRep = new APInvoiceRepository(invoiceCotnext);
       //     List<APInvoice> apInvoices = apInvoiceRep.GetAPInvoicesByTenant(tenant.Id).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2)).ToList();
            
       //     ARInvoiceLineRepository invoiceLineRep = new ARInvoiceLineRepository(invoiceCotnext);
       //     ARInvoiceTotalVATRepository totalVatRep = new ARInvoiceTotalVATRepository(invoiceCotnext);
       //     APInvoiceLineRepository apInvoiceLineRep = new APInvoiceLineRepository(invoiceCotnext);
       //     APInvoiceTotalVATRepository apTotalVatRep = new APInvoiceTotalVATRepository(invoiceCotnext);

       //     StringBuilder main = new StringBuilder();
       //     int counter = 0;
       //     int apCounter = 0;

       //     counter += invoices.Count;
       //     counter += apInvoices.Count;
       //     counter += (payments.Count)*2;

       //     apCounter += invoices.Count;
       //     apCounter += (payments.Count) * 2;

       //     foreach (ARInvoice invoice in invoices)
       //     {
       //         counter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //         apCounter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //     }
       //     foreach (APInvoice invoice in apInvoices)
       //     {
       //         counter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //     }            

       //     main.AppendLine(BuildA100Line(tenant, random));
       //     main.AppendLine(BuildC100PaymentLines(payments, tenant));
       //     main.AppendLine(BuildC100InvoiceLines(((payments.Count)*2) + 1, invoices, tenant, invoiceLineRep, totalVatRep));
       //     main.AppendLine(BuildC100APInvoiceLines((apCounter + 1), apInvoices, tenant, apInvoiceLineRep, apTotalVatRep));
       //     main.AppendLine(BuildZ900Line((counter + 2), tenant, random));

       //     return Regex.Replace(main.ToString(), @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
       // }

       // private string BuildA100Line(Tenant tenant, string random)
       // {
       //     TaxesApproval.A100 a100 = new TaxesApproval.A100();

       //     a100.RecordCode = "A100";
       //     a100.RecordLineNumber = "1";
       //     a100.TenantVatNumber = tenant.VatNumber;
       //     a100.PrimaryId = random;
       //     a100.SystemConst = "&OF1.31&";
       //     a100.FutureUsage = "";

       //     StringBuilder str = new StringBuilder(95);

       //     str.Append(a100.RecordCode.PadRight(4));
       //     str.Append(a100.RecordLineNumber.PadLeft(9, '0'));

       //     if (a100.TenantVatNumber != null) { str.Append(a100.TenantVatNumber.PadLeft(9, '0')); }
       //     else { str.Append(' ', 9); }

       //     str.Append(a100.PrimaryId.PadRight(15));
       //     str.Append(a100.SystemConst.PadRight(8));
       //     str.Append(a100.FutureUsage.PadRight(50));
            
       //     return str.ToString();
       // }

       // private string BuildC100PaymentLines(List<ARPayment> payments, Tenant tenant)
       // {
       //     ContactQuery contactQuery = new ContactQuery(tenant.Id);
       //     AddressRepository addressRep = new AddressRepository(tenant.Id);
       //     CardRepository cardRep = new CardRepository(tenant.Id);
       //     CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

       //     StringBuilder str = new StringBuilder(444);
       //     int i = 2;

       //     foreach (ARPayment item in payments)
       //     {
       //         Address billToAddress = addressRep.GetSingleAddress(item.BillToAddressId, tenant.Id);
       //         Card billTo = cardRep.GetSingleCard(item.BillToId, tenant.Id);
       //         Currency paymentCurrency = currencyRep.GetSingleCurrency(item.PaymentCurrencyId, tenant.Id);
       //         TaxesApproval.C100 c100 = new TaxesApproval.C100();

       //         c100.RecordCode = "C100";
       //         c100.RecordLineNumber = i++.ToString();
       //         c100.TenantVatNumber = tenant.VatNumber;
       //         c100.DocumentType = "400";
       //         c100.DocumentNumber = item.PaymentNo;
       //         c100.DocumentCreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
       //         c100.DocumentCreateHour = String.Format("{0:hhmm}", item.CreateDate);
       //         c100.BillToName = billTo != null ? billTo.EnglishName : "";
       //         c100.BillToAddress_Street = billToAddress != null ? (billToAddress.Address1 != null ? billToAddress.Address1 : "") : "";
       //         c100.BillToAddress_HouseNumber = "";
       //         c100.BillToAddress_City = billToAddress != null ? (billToAddress.City != null ? billToAddress.City : "") : "";
       //         c100.BillToAddress_ZipCode = billToAddress != null ? (billToAddress.ZipCode != null ? billToAddress.ZipCode : "") : "";
       //         c100.BillToAddress_CountryName = billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.EnglishName : "") : "";
       //         c100.BillToAddress_CountryCode = billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : "") : "";
       //         c100.BillToAddress_Telephone = billToAddress != null ? (billToAddress.PhoneNumber != null ? billToAddress.PhoneNumber : "") : "";
       //         c100.BillToVAT = billTo != null ? (billTo.VatNumber != null ? billTo.VatNumber : "") : "";
       //         c100.ValueDate = String.Format("{0:yyyyMMdd}", item.RegisterDate);
       //         c100.TotalAmount = paymentCurrency.Code != "NIS" ? (item.AmountInLocalCurrency != null ? String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "") : "+00000000000000") : "+00000000000000";
       //         c100.CurrencyCode = paymentCurrency != null ? (paymentCurrency.Code != "NIS" ? paymentCurrency.Code : "") : "";
       //         c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
       //         c100.Discount = "+00000000000000";
       //         c100.SubTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
       //         c100.VAT = "+00000000000000";
       //         c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
       //         c100.Field1224 = "";
       //         c100.BillToId = billTo != null ? billTo.Code : "";
       //         c100.Field1226 = "";
       //         c100.Void_CancelledDocument = item.IsClosed ? "1" : "";
       //         c100.DocumentDate = String.Format("{0:yyyyMMdd}", item.RegisterDate);
       //         c100.BranchId = "";
       //         c100.UserName = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant.Id).EnglishName;
       //         c100.LinkingField = "0000000";
       //         c100.FutureUsage = "";

       //         str.Append(c100.RecordCode.PadRight(4));
       //         str.Append(c100.RecordLineNumber.PadLeft(9, '0'));
       //         str.Append(c100.TenantVatNumber.PadLeft(9, '0'));
       //         str.Append(c100.DocumentType.PadLeft(3, '0'));
       //         str.Append(c100.DocumentNumber.PadRight(20));
       //         str.Append(c100.DocumentCreateDate.PadLeft(8, '0'));
       //         str.Append(c100.DocumentCreateHour.PadLeft(4, '0'));
       //         str.Append(c100.BillToName.PadRight(50));
       //         str.Append(c100.BillToAddress_Street.PadRight(50));
       //         str.Append(c100.BillToAddress_HouseNumber.PadRight(10));
       //         str.Append(c100.BillToAddress_City.PadRight(30));
       //         str.Append(c100.BillToAddress_ZipCode.PadRight(8));
       //         str.Append(c100.BillToAddress_CountryName.PadRight(30));
       //         str.Append(c100.BillToAddress_CountryCode.PadRight(2));
       //         str.Append(c100.BillToAddress_Telephone.PadRight(15));
       //         str.Append(c100.BillToVAT.PadLeft(9, '0'));
       //         str.Append(c100.ValueDate.PadLeft(8, '0'));
       //         str.Append(c100.TotalAmount); 
       //         str.Append(c100.CurrencyCode.PadRight(3));
       //         str.Append(c100.AmountBeforeDiscount);
       //         str.Append(c100.Discount);
       //         str.Append(c100.SubTotal.PadRight(15));
       //         str.Append(c100.VAT);
       //         str.Append(c100.GrandTotal);
       //         str.Append(c100.Field1224.PadRight(12));
       //         str.Append(c100.BillToId.PadRight(15));
       //         str.Append(c100.Field1224.PadRight(10));
       //         str.Append(c100.Void_CancelledDocument.PadRight(1));
       //         str.Append(c100.DocumentDate.PadLeft(8, '0'));
       //         str.Append(c100.BranchId.PadRight(7));
       //         str.Append(c100.UserName.PadRight(9));
       //         str.Append(c100.LinkingField.PadRight(7));
       //         str.Append(c100.FutureUsage.PadRight(13));
       //         str.AppendLine();

       //         str.Append(BuildD120Lines(i++, item, tenant));
       //     }

       //     return str.ToString();
       // }

       // private string BuildD120Lines(int count, ARPayment payment, Tenant tenant)
       // {
       //     StringBuilder str = new StringBuilder(222);
            
       //     TaxesApproval.D120 d120 = new TaxesApproval.D120();

       //     d120.RecordCode = "D120";
       //     d120.RecordLineNumber = count.ToString();
       //     d120.TenantVatNumber = tenant.VatNumber;
       //     d120.DocumentType = "400";
       //     d120.DocumentNumber = payment.PaymentNo;
       //     d120.EntityLineNumber = "0001";

       //     if (payment.ARPaymentMethodCode == "CA")
       //     {
       //         d120.PaymentMethod = "1";
       //     }
       //     else if (payment.ARPaymentMethodCode == "CH")
       //     {
       //         d120.PaymentMethod = "2";
       //     }
       //     else if (payment.ARPaymentMethodCode == "CC")
       //     {
       //         d120.PaymentMethod = "3";
       //     }
       //     else if (payment.ARPaymentMethodCode == "BT")
       //     {
       //         d120.PaymentMethod = "4";
       //     }
       //     else
       //     {
       //         d120.PaymentMethod = "9";
       //     }
                        
       //     d120.BankNumber = payment.Bank != null ? payment.Bank : "";
       //     d120.BranchNumber = payment.BankBranch != null ? payment.BankBranch : "";
       //     d120.AccountBunber = payment.Account != null ? payment.Account : "";
       //     d120.ChequeNumber = payment.ChequeOrPaymentRef != null ? payment.ChequeOrPaymentRef : "";
       //     d120.PaymentDate = String.Format("{0:yyyyMMdd}", payment.RegisterDate);
       //     d120.Total = String.Format("{0:+000000000000.00}", payment.AmountInLocalCurrency.Value).Replace(".", "");
       //     d120.CreditCardCompany = "";
       //     d120.Field1314 = "";
       //     d120.CreditType = "";            
       //     d120.Field1320 = "";
       //     d120.DocumentDate = String.Format("{0:yyyyMMdd}", payment.RegisterDate);
       //     d120.Field1323 = "0000000";
       //     d120.Field1324 = "";

       //     str.Append(d120.RecordCode.PadRight(4));
       //     str.Append(d120.RecordLineNumber.PadLeft(9, '0'));
       //     str.Append(d120.TenantVatNumber.PadLeft(9, '0'));
       //     str.Append(d120.DocumentType.PadLeft(3, '0'));
       //     str.Append(d120.DocumentNumber.PadRight(20));
       //     str.Append(d120.EntityLineNumber.PadLeft(4));
       //     str.Append(d120.PaymentMethod.PadRight(1));
       //     str.Append(d120.BankNumber.PadLeft(10, '0'));
       //     str.Append(d120.BranchNumber.PadLeft(10, '0'));
       //     str.Append(d120.AccountBunber.PadLeft(15, '0'));
       //     str.Append(d120.ChequeNumber.PadLeft(10, '0'));
       //     str.Append(d120.PaymentDate.PadLeft(8, '0'));
       //     str.Append(d120.Total);
       //     str.Append(d120.CreditCardCompany.PadRight(1));
       //     str.Append(d120.Field1314.PadRight(20));
       //     str.Append(d120.CreditType.PadRight(1));
       //     str.Append(d120.Field1320.PadRight(7));
       //     str.Append(d120.DocumentDate.PadLeft(8, '0'));
       //     str.Append(d120.Field1323.PadRight(7));
       //     str.Append(d120.Field1324.PadRight(60));

       //     str.AppendLine();

       //     return str.ToString();
       // }

       // private string BuildC100InvoiceLines(int count, List<ARInvoice> invoices, Tenant tenant, ARInvoiceLineRepository invoiceLineRep, ARInvoiceTotalVATRepository totalVatRep)
       // {
       //     ContactQuery contactQuery = new ContactQuery(tenant.Id);
       //     AddressRepository addressRep = new AddressRepository(tenant.Id);
       //     CardRepository cardRep = new CardRepository(tenant.Id);
       //     CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

       //     StringBuilder str = new StringBuilder(444);
       //     int i = count + 1;

       //     foreach (ARInvoice item in invoices)
       //     {
       //         Address billToAddress = addressRep.GetSingleAddress(item.BillToAddressId, tenant.Id);
       //         Card billTo = cardRep.GetSingleCard(item.BillToId, tenant.Id);
       //         Currency invoiceCurrency = currencyRep.GetSingleCurrency(item.InvoiceCurrencyId, tenant.Id);
       //         List<ARInvoiceTotalVAT> totalVats = totalVatRep.GetInvoiceTotalVatsForInvoice(item.Id, tenant.Id).ToList();

       //         TaxesApproval.C100 c100 = new TaxesApproval.C100();

       //         c100.RecordCode = "C100";
       //         c100.RecordLineNumber = i++.ToString();
       //         c100.TenantVatNumber = tenant.VatNumber;

       //         if (item.ARInvoiceTypeCode == "IN")
       //         {
       //             c100.DocumentType = "305";
       //             c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:+000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "+00000000000000") : "+00000000000000";
       //         }
       //         else if (item.ARInvoiceTypeCode == "MN")
       //         {
       //             c100.DocumentType = "310";
       //             c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:+000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "+00000000000000") : "+00000000000000";
       //         }
       //         else if (item.ARInvoiceTypeCode == "CD")
       //         {
       //             c100.DocumentType = "330";
       //             c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:-000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "-00000000000000") : "-00000000000000";
       //         }

       //         c100.DocumentNumber = item.InvoiceNumber;
       //         c100.DocumentCreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
       //         c100.DocumentCreateHour = String.Format("{0:hhmm}", item.CreateDate);
       //         c100.BillToName = billTo != null ? billTo.EnglishName : "";
       //         c100.BillToAddress_Street = billToAddress != null ? (billToAddress.Address1 != null ? billToAddress.Address1 : "") : "";
       //         c100.BillToAddress_HouseNumber = "";
       //         c100.BillToAddress_City = billToAddress != null ? (billToAddress.City != null ? billToAddress.City : "") : "";
       //         c100.BillToAddress_ZipCode = billToAddress != null ? (billToAddress.ZipCode != null ? billToAddress.ZipCode : "") : "";
       //         c100.BillToAddress_CountryName = billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.EnglishName : "") : "";
       //         c100.BillToAddress_CountryCode = billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : "") : "";
       //         c100.BillToAddress_Telephone = billToAddress != null ? (billToAddress.PhoneNumber != null ? billToAddress.PhoneNumber : "") : "";
       //         c100.BillToVAT = billTo != null ? (billTo.VatNumber != null ? billTo.VatNumber : "") : "";
       //         c100.ValueDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
       //         c100.CurrencyCode = invoiceCurrency != null ? (invoiceCurrency.Code != "NIS" ? invoiceCurrency.Code : "") : "";
       //         c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
       //         c100.Discount = "+00000000000000";
       //         c100.SubTotal = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
       //         c100.VAT = totalVats != null ? String.Format("{0:+000000000000.00}", totalVats.Sum(d => d.LocalVATAmount)).Replace(".", "") : "+00000000000000";
       //         c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
       //         c100.Field1224 = "";
       //         c100.BillToId = billTo != null ? billTo.Code : "";
       //         c100.Field1226 = "";
       //         c100.Void_CancelledDocument = item.IsCancelled ? "1" : "";
       //         c100.DocumentDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
       //         c100.BranchId = "";
       //         c100.UserName = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant.Id).EnglishName;
       //         c100.LinkingField = "0000000";
       //         c100.FutureUsage = "";

       //         str.Append(c100.RecordCode.PadRight(4));
       //         str.Append(c100.RecordLineNumber.PadLeft(9, '0'));
       //         str.Append(c100.TenantVatNumber.PadLeft(9, '0'));
       //         str.Append(c100.DocumentType.PadLeft(3, '0'));
       //         str.Append(c100.DocumentNumber.PadRight(20));
       //         str.Append(c100.DocumentCreateDate.PadLeft(8, '0'));
       //         str.Append(c100.DocumentCreateHour.PadLeft(4, '0'));
       //         str.Append(c100.BillToName.PadRight(50));
       //         str.Append(c100.BillToAddress_Street.PadRight(50));
       //         str.Append(c100.BillToAddress_HouseNumber.PadRight(10));
       //         str.Append(c100.BillToAddress_City.PadRight(30));
       //         str.Append(c100.BillToAddress_ZipCode.PadRight(8));
       //         str.Append(c100.BillToAddress_CountryName.PadRight(30));
       //         str.Append(c100.BillToAddress_CountryCode.PadRight(2));
       //         str.Append(c100.BillToAddress_Telephone.PadRight(15));
       //         str.Append(c100.BillToVAT.PadLeft(9, '0'));
       //         str.Append(c100.ValueDate.PadLeft(8, '0'));
       //         str.Append(c100.TotalAmount);
       //         str.Append(c100.CurrencyCode.PadRight(3));
       //         str.Append(c100.AmountBeforeDiscount);
       //         str.Append(c100.Discount);
       //         str.Append(c100.SubTotal.ToString().PadRight(15));
       //         str.Append(c100.VAT);
       //         str.Append(c100.GrandTotal);
       //         str.Append(c100.Field1224.PadRight(12));
       //         str.Append(c100.BillToId.PadRight(15));
       //         str.Append(c100.Field1224.PadRight(10));
       //         str.Append(c100.Void_CancelledDocument.PadRight(1));
       //         str.Append(c100.DocumentDate.PadLeft(8, '0'));
       //         str.Append(c100.BranchId.PadRight(7));
       //         str.Append(c100.UserName.PadRight(9));
       //         str.Append(c100.LinkingField.PadRight(7));
       //         str.Append(c100.FutureUsage.PadRight(13));
       //         str.AppendLine();

       //         str.Append(BuildD110ARInvoiceLines(i, item, tenant, invoiceLineRep));
       //         i += invoiceLineRep.GetInvoiceLinesByInvoiceId(item.Id, tenant.Id).Count();
       //     }

       //     return str.ToString();
       // }

       // private string BuildD110ARInvoiceLines(int counter, ARInvoice invoice, Tenant tenant, ARInvoiceLineRepository invoiceLineRep)
       // {
       //     StringBuilder str = new StringBuilder(339);
       //     List<ARInvoiceLine> lines = invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).ToList();
       //     int j = 1;

       //     foreach (ARInvoiceLine line in lines)
       //     {
       //         TaxesApproval.D110 d110 = new TaxesApproval.D110();
       //         d110.RecordCode = "D110";
       //         d110.RecordLineNumber = counter++.ToString();
       //         d110.TenantVatNumber = tenant.VatNumber;

       //         if (invoice.ARInvoiceTypeCode == "IN") { d110.DocumentType = "305"; }
       //         else if (invoice.ARInvoiceTypeCode == "MN") { d110.DocumentType = "310"; }
       //         else if (invoice.ARInvoiceTypeCode == "CD") { d110.DocumentType = "330"; }

       //         d110.DocumentNumber = invoice.InvoiceNumber;
       //         d110.DocumentLineNumber = j++.ToString();
       //         d110.BaseDocumentType = "000";
       //         d110.BaseDocumentNumber = "";
       //         d110.ServiceType = "1";
       //         d110.Field1259 = "";
       //         d110.DescriptionOfSservice = line.Description;
       //         d110.ManifacturerName = "";
       //         d110.ProductSerialNumber = "";
       //         d110.Field1263 = "יחידה";
       //         d110.Quantiy = String.Format("{0:+000000000000.0000}", line.Quantity.Value).Replace(".", "");
       //         d110.UnitPrice = String.Format("{0:+000000000000.00}", (line.LocalCurrencyAmount / line.Quantity).Value).Replace(".", "");
       //         d110.LineDiscount = "+00000000000000";
       //         d110.LineAmount = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
       //         d110.LineVATPercentage = String.Format("{0:00.00}", line.VatPercentage.Value).Replace(".", "");
       //         d110.BranchId = "";
       //         d110.EntityDate = String.Format("{0:yyyyMMdd}", invoice.InvoiceDate);
       //         d110.Field1273 = "0000000";
       //         d110.Field1274 = "";
       //         d110.FutureUsage = "";

       //         str.Append(d110.RecordCode.PadRight(4));
       //         str.Append(d110.RecordLineNumber.PadLeft(9, '0'));
       //         str.Append(d110.TenantVatNumber.PadLeft(9, '0'));
       //         str.Append(d110.DocumentType.PadLeft(3, '0'));
       //         str.Append(d110.DocumentNumber.PadRight(20));
       //         str.Append(d110.DocumentLineNumber.PadLeft(4, '0'));
       //         str.Append(d110.BaseDocumentType.PadLeft(3, '0'));
       //         str.Append(d110.BaseDocumentNumber.PadRight(20));
       //         str.Append(d110.ServiceType.PadRight(1));
       //         str.Append(d110.Field1259.PadRight(20));
       //         str.Append(d110.DescriptionOfSservice.PadRight(30));
       //         str.Append(d110.ManifacturerName.PadRight(50));
       //         str.Append(d110.ProductSerialNumber.PadRight(30));
       //         str.Append(d110.Field1263.PadRight(20));
       //         str.Append(d110.Quantiy);
       //         str.Append(d110.UnitPrice);
       //         str.Append(d110.LineDiscount);
       //         str.Append(d110.LineAmount); 
       //         str.Append(d110.LineVATPercentage);
       //         str.Append(d110.BranchId.PadRight(7));
       //         str.Append(d110.EntityDate.PadLeft(8, '0'));
       //         str.Append(d110.Field1273.PadRight(7));
       //         str.Append(d110.Field1274.PadRight(7));
       //         str.Append(d110.FutureUsage.PadRight(21));

       //         str.AppendLine();
       //     }

       //     return str.ToString();
       // }

       // private string BuildC100APInvoiceLines(int count, List<APInvoice> invoices, Tenant tenant, APInvoiceLineRepository invoiceLineRep, APInvoiceTotalVATRepository totalVatRep)
       // {
       //     ContactQuery contactQuery = new ContactQuery(tenant.Id);
       //     AddressRepository addressRep = new AddressRepository(tenant.Id);
       //     CardRepository cardRep = new CardRepository(tenant.Id);
       //     CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

       //     StringBuilder str = new StringBuilder(444);
       //     int i = count + 1;

       //     foreach (APInvoice item in invoices)
       //     {
       //         Address vendorAddress = addressRep.GetMainAddressByCardId(item.VendorId, tenant.Id);
       //         Card vendor = cardRep.GetSingleCard(item.VendorId, tenant.Id);
       //         Currency invoiceCurrency = currencyRep.GetSingleCurrency(item.InvoiceCurrencyId, tenant.Id);
       //         List<APInvoiceTotalVAT> totalVats = totalVatRep.GetInvoiceTotalVatsByInvoiceId(item.Id, tenant.Id).ToList();

       //         TaxesApproval.C100 c100 = new TaxesApproval.C100();

       //         c100.RecordCode = "C100";
       //         c100.RecordLineNumber = i++.ToString();
       //         c100.TenantVatNumber = tenant.VatNumber;
       //         c100.DocumentType = "700";                
       //         c100.DocumentNumber = item.InvoiceNumber;
       //         c100.DocumentCreateDate = String.Format("{0:yyyyMMdd}", item.CreateDate);
       //         c100.DocumentCreateHour = String.Format("{0:hhmm}", item.CreateDate);
       //         c100.BillToName = vendor != null ? vendor.EnglishName : "";
       //         c100.BillToAddress_Street = vendorAddress != null ? (vendorAddress.Address1 != null ? vendorAddress.Address1 : "") : "";
       //         c100.BillToAddress_HouseNumber = "";
       //         c100.BillToAddress_City = vendorAddress != null ? (vendorAddress.City != null ? vendorAddress.City : "") : "";
       //         c100.BillToAddress_ZipCode = vendorAddress != null ? (vendorAddress.ZipCode != null ? vendorAddress.ZipCode : "") : "";
       //         c100.BillToAddress_CountryName = vendorAddress != null ? (vendorAddress.Country != null ? vendorAddress.Country.EnglishName : "") : "";
       //         c100.BillToAddress_CountryCode = vendorAddress != null ? (vendorAddress.Country != null ? vendorAddress.Country.Code : "") : "";
       //         c100.BillToAddress_Telephone = vendorAddress != null ? (vendorAddress.PhoneNumber != null ? vendorAddress.PhoneNumber : "") : "";
       //         c100.BillToVAT = vendor != null ? (vendor.VatNumber != null ? vendor.VatNumber : "") : "";
       //         c100.ValueDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
       //         c100.TotalAmount = invoiceCurrency.Code != "NIS" ? (item.AmountInInvoiceCurrency != null ? String.Format("{0:-000000000000.00}", item.AmountInInvoiceCurrency.Value).Replace(".", "") : "-00000000000000") : "-00000000000000";                
       //         c100.CurrencyCode = invoiceCurrency != null ? (invoiceCurrency.Code != "NIS" ? invoiceCurrency.Code : "") : "";
       //         c100.AmountBeforeDiscount = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
       //         c100.Discount = "+00000000000000";
       //         c100.SubTotal = String.Format("{0:+000000000000.00}", item.SubTotalInLocalCurrency.Value).Replace(".", "");
       //         c100.VAT = totalVats != null ? String.Format("{0:+000000000000.00}", totalVats.Sum(d => d.LocalVATAmount)).Replace(".", "") : "+00000000000000";
       //         c100.GrandTotal = String.Format("{0:+000000000000.00}", item.AmountInLocalCurrency.Value).Replace(".", "");
       //         c100.Field1224 = "";
       //         c100.BillToId = vendor != null ? vendor.Code : "";
       //         c100.Field1226 = "";
       //         c100.Void_CancelledDocument = item.IsClosed ? "1" : "";
       //         c100.DocumentDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate);
       //         c100.BranchId = "";
       //         c100.UserName = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant.Id).EnglishName;
       //         c100.LinkingField = "0000000";
       //         c100.FutureUsage = "";

       //         str.Append(c100.RecordCode.PadRight(4));
       //         str.Append(c100.RecordLineNumber.PadLeft(9, '0'));
       //         str.Append(c100.TenantVatNumber.PadLeft(9, '0'));
       //         str.Append(c100.DocumentType.PadLeft(3, '0'));
       //         str.Append(c100.DocumentNumber.PadRight(20));
       //         str.Append(c100.DocumentCreateDate.PadLeft(8, '0'));
       //         str.Append(c100.DocumentCreateHour.PadLeft(4, '0'));
       //         str.Append(c100.BillToName.PadRight(50));
       //         str.Append(c100.BillToAddress_Street.PadRight(50));
       //         str.Append(c100.BillToAddress_HouseNumber.PadRight(10));
       //         str.Append(c100.BillToAddress_City.PadRight(30));
       //         str.Append(c100.BillToAddress_ZipCode.PadRight(8));
       //         str.Append(c100.BillToAddress_CountryName.PadRight(30));
       //         str.Append(c100.BillToAddress_CountryCode.PadRight(2));
       //         str.Append(c100.BillToAddress_Telephone.PadRight(15));
       //         str.Append(c100.BillToVAT.PadLeft(9, '0'));
       //         str.Append(c100.ValueDate.PadLeft(8, '0'));
       //         str.Append(c100.TotalAmount);
       //         str.Append(c100.CurrencyCode.PadRight(3));
       //         str.Append(c100.AmountBeforeDiscount);
       //         str.Append(c100.Discount);
       //         str.Append(c100.SubTotal.ToString().PadRight(15));
       //         str.Append(c100.VAT);
       //         str.Append(c100.GrandTotal);
       //         str.Append(c100.Field1224.PadRight(12));
       //         str.Append(c100.BillToId.PadRight(15));
       //         str.Append(c100.Field1224.PadRight(10));
       //         str.Append(c100.Void_CancelledDocument.PadRight(1));
       //         str.Append(c100.DocumentDate.PadLeft(8, '0'));
       //         str.Append(c100.BranchId.PadRight(7));
       //         str.Append(c100.UserName.PadRight(9));
       //         str.Append(c100.LinkingField.PadRight(7));
       //         str.Append(c100.FutureUsage.PadRight(13));
       //         str.AppendLine();

       //         str.Append(BuildD110APInvoiceLines(i, item, tenant, invoiceLineRep));
       //         i += invoiceLineRep.GetInvoiceLinesByInvoiceId(item.Id, tenant.Id).Count();
       //     }
       //     return str.ToString();
       // }

       // private string BuildD110APInvoiceLines(int counter, APInvoice invoice, Tenant tenant, APInvoiceLineRepository invoiceLineRep)
       // {
       //     ChargesTypeRepository chargesRep = new ChargesTypeRepository(tenant.Id);

       //     StringBuilder str = new StringBuilder(339);
       //     List<APInvoiceLine> lines = invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).ToList();
       //     int j = 1;

       //     foreach (APInvoiceLine line in lines)
       //     {
       //         ChargesType charge = chargesRep.GetSingleChargesType(line.ChargesTypeId, tenant.Id);

       //         TaxesApproval.D110 d110 = new TaxesApproval.D110();
       //         d110.RecordCode = "D110";
       //         d110.RecordLineNumber = counter++.ToString();
       //         d110.TenantVatNumber = tenant.VatNumber;
       //         d110.DocumentType = "700";
       //         d110.DocumentNumber = invoice.InvoiceNumber;
       //         d110.DocumentLineNumber = j++.ToString();
       //         d110.BaseDocumentType = "000";
       //         d110.BaseDocumentNumber = "";
       //         d110.ServiceType = "1";
       //         d110.Field1259 = "";
       //         d110.DescriptionOfSservice = charge != null ? charge.EnglishName : "";
       //         d110.ManifacturerName = "";
       //         d110.ProductSerialNumber = "";
       //         d110.Field1263 = "יחידה";
       //         d110.Quantiy = "+0000000000010000";
       //         d110.UnitPrice = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
       //         d110.LineDiscount = "+00000000000000";
       //         d110.LineAmount = String.Format("{0:+000000000000.00}", line.LocalCurrencyAmount.Value).Replace(".", "");
       //         d110.LineVATPercentage = String.Format("{0:00.00}", line.VatPercentage.Value).Replace(".", "");
       //         d110.BranchId = "";
       //         d110.EntityDate = String.Format("{0:yyyyMMdd}", invoice.InvoiceDate);
       //         d110.Field1273 = "0000000";
       //         d110.Field1274 = "";
       //         d110.FutureUsage = "";

       //         str.Append(d110.RecordCode.PadRight(4));
       //         str.Append(d110.RecordLineNumber.PadLeft(9, '0'));
       //         str.Append(d110.TenantVatNumber.PadLeft(9, '0'));
       //         str.Append(d110.DocumentType.PadLeft(3, '0'));
       //         str.Append(d110.DocumentNumber.PadRight(20));
       //         str.Append(d110.DocumentLineNumber.PadLeft(4, '0'));
       //         str.Append(d110.BaseDocumentType.PadLeft(3, '0'));
       //         str.Append(d110.BaseDocumentNumber.PadRight(20));
       //         str.Append(d110.ServiceType.PadRight(1));
       //         str.Append(d110.Field1259.PadRight(20));
       //         str.Append(d110.DescriptionOfSservice.PadRight(30));
       //         str.Append(d110.ManifacturerName.PadRight(50));
       //         str.Append(d110.ProductSerialNumber.PadRight(30));
       //         str.Append(d110.Field1263.PadRight(20));
       //         str.Append(d110.Quantiy);
       //         str.Append(d110.UnitPrice);
       //         str.Append(d110.LineDiscount);
       //         str.Append(d110.LineAmount);
       //         str.Append(d110.LineVATPercentage);
       //         str.Append(d110.BranchId.PadRight(7));
       //         str.Append(d110.EntityDate.PadLeft(8, '0'));
       //         str.Append(d110.Field1273.PadRight(7));
       //         str.Append(d110.Field1274.PadRight(7));
       //         str.Append(d110.FutureUsage.PadRight(21));

       //         str.AppendLine();
       //     }

       //     return str.ToString();
       // }

       // private string BuildZ900Line(int counter, Tenant tenant, string random)
       // {
       //     TaxesApproval.Z900 z900 = new TaxesApproval.Z900();

       //     z900.RecordCode = "Z900";
       //     z900.RecordLineNumber = String.Format("{0:000000000}", counter);
       //     z900.TenantVatNumber = tenant.VatNumber;
       //     z900.PrimaryId = random;
       //     z900.SystemConst = "&OF1.31&";
       //     z900.TotalRecords = String.Format("{0:000000000000000}", counter);
       //     z900.FutureUsage = "";

       //     StringBuilder str = new StringBuilder(110);
                        
       //     str.Append(z900.RecordCode.PadRight(4));
       //     str.Append(z900.RecordLineNumber);

       //     if (z900.TenantVatNumber != null) { str.Append(z900.TenantVatNumber.PadLeft(9, '0')); }
       //     else { str.Append(' ', 9); }

       //     str.Append(z900.PrimaryId.PadRight(15));
       //     str.Append(z900.SystemConst.PadRight(8));
       //     str.Append(z900.TotalRecords.PadRight(15));
       //     str.Append(z900.FutureUsage.PadRight(50));

       //     return str.ToString();
       // }

       //// [WebMethod]
       // public byte[] BuildINIFile(DateTime? date1, DateTime? date2, int tenant, string random, string email)
       // {
       //     TenantRepository tenantRep = new TenantRepository(tenant);
       //     Tenant tenantPoco = tenantRep.GetSingleTenant(tenant);

       //     string str = this.BuildA000File(date1, date2, tenantPoco, random);
       //     byte[] bytearray = this.ConvertEncoding(str);
       //     return bytearray;
       // }

       // private string BuildA000File(DateTime? date1, DateTime? date2, Tenant tenant, string random)
       // {
       //     IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant.Id);
       //     ARPaymentRepository paymentRep = new ARPaymentRepository(invoiceCotnext);
       //     ARInvoiceRepository invoiceRep = new ARInvoiceRepository(invoiceCotnext);
       //     APInvoiceRepository apInvoiceRep = new APInvoiceRepository(invoiceCotnext);
       //     ARInvoiceLineRepository invoiceLineRep = new ARInvoiceLineRepository(invoiceCotnext);
       //     APInvoiceLineRepository apInvoiceLineRep = new APInvoiceLineRepository(invoiceCotnext);
       //     AddressRepository addressRep = new AddressRepository(tenant.Id);
       //     CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

       //     List<ARPayment> payments = paymentRep.GetARPaymentsByTenant(tenant.Id).Where(p => (p.RegisterDate >= date1 && p.RegisterDate <= date2)).ToList();
       //     List<ARInvoice> invoices = invoiceRep.GetInvoicesByTenant(tenant.Id).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2)).ToList();
       //     List<APInvoice> apInvoices = apInvoiceRep.GetAPInvoicesByTenant(tenant.Id).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2)).ToList();
       //     Address address = addressRep.GetSingleAddress(tenant.AddressId, tenant.Id);
       //     Currency currency = currencyRep.GetSingleCurrency(tenant.CurrencyId, tenant.Id);

       //     int totalcounter = 0;
       //     int C100Counter = 0;
       //     int D110Counter = 0;
       //     int D120Counter = 0;

       //     totalcounter += invoices.Count;
       //     totalcounter += apInvoices.Count;
       //     totalcounter += (payments.Count) * 2;

       //     foreach (ARInvoice invoice in invoices)
       //     {
       //         totalcounter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //         D110Counter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //     }
       //     foreach (APInvoice invoice in apInvoices)
       //     {
       //         totalcounter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //         D110Counter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //     }

       //     C100Counter = invoices.Count + apInvoices.Count + payments.Count;
       //     D120Counter = payments.Count;

       //     TaxesApproval.A000 a000 = new TaxesApproval.A000();
       //     a000.RecordCode = "A000";
       //     a000.FutureUsage = "";
       //     a000.TotalLinesInBKMVDATA = String.Format("{0:000000000000000}", totalcounter + 2);
       //     a000.TenantVatNumber = tenant.VatNumber;
       //     a000.MainId = random;
       //     a000.FixedValue = "&OF1.31&";
       //     a000.SystemRegistrationNumber = "";
       //     a000.SystemName = "Logitude World";
       //     a000.SystemVerion = "2.4";
       //     a000.LogitudeVATNumber = "514501774";
       //     a000.SystemManifacturer = "Logitude World LTD";
       //     a000.SystemType = "2";
       //     a000.FilesSavingDirectory = "C:\\OPENFRMT\\" + tenant.VatNumber + "." + String.Format("{0:yy}", DateTime.Now) + "\\" + String.Format("{0:MMddhhmm}", DateTime.Now);
       //     a000.SystemAcctType = "2";
       //     a000.NeededBalanceLevel = "2";
       //     a000.Field1016 = "";
       //     a000.TenantName = tenant.Company;
       //     a000.TenantAddress1 = address != null ? address.Address1 : "";
       //     a000.TenantHouseNumber = "";
       //     a000.TenantCity = address != null ? (address.City != null ? address.City : "") : "";
       //     a000.TenantZipCode = address != null ? (address.ZipCode != null ? address.ZipCode : "") : "";
       //     a000.TaxYear = String.Format("{0:yyyy}", date1);
       //     a000.Startingdate = String.Format("{0:yyyyMMdd}", date1);
       //     a000.EndDate = String.Format("{0:yyyyMMdd}", date2);
       //     a000.TodayDate = String.Format("{0:yyyyMMdd}", DateTime.Now);
       //     a000.TimeNow = String.Format("{0:hhmm}", DateTime.Now);
       //     a000.LanguageCode = "0";
       //     a000.Encoding = "1";
       //     a000.Field1030 = "WINZIP";
       //     a000.AccountingCurrencyCode = currency != null ? (currency.Code == "NIS" ? "ILS" : currency.Code) : "";
       //     a000.BranchInfo = "0";

       //     StringBuilder str = new StringBuilder(466);

       //     str.Append(a000.RecordCode.PadRight(4));
       //     str.Append(a000.FutureUsage.PadRight(5));
       //     str.Append(a000.TotalLinesInBKMVDATA);
       //     str.Append(a000.TenantVatNumber.PadRight(9));
       //     str.Append(a000.MainId.PadRight(15));
       //     str.Append(a000.FixedValue.PadRight(8));
       //     str.Append(a000.SystemRegistrationNumber.PadLeft(8, '0'));
       //     str.Append(a000.SystemName.PadRight(20));
       //     str.Append(a000.SystemVerion.PadRight(20));
       //     str.Append(a000.LogitudeVATNumber.PadLeft(9, '0'));
       //     str.Append(a000.SystemManifacturer.PadRight(20));
       //     str.Append(a000.SystemType.PadLeft(1, '0'));
       //     str.Append(a000.FilesSavingDirectory.PadRight(50));
       //     str.Append(a000.SystemAcctType.PadLeft(1, '0'));
       //     str.Append(a000.NeededBalanceLevel.PadLeft(1, '0'));
       //     str.Append(a000.TenantVatNumber.PadLeft(9, '0'));
       //     str.Append(a000.Field1016.PadLeft(9, '0'));
       //     str.Append(a000.FutureUsage.PadRight(10)); 
       //     str.Append(a000.TenantName.PadRight(50));
       //     str.Append(a000.TenantAddress1.PadRight(50));
       //     str.Append(a000.TenantHouseNumber.PadRight(10));
       //     str.Append(a000.TenantCity.PadRight(30));
       //     str.Append(a000.TenantZipCode.PadRight(8));
       //     str.Append(a000.TaxYear);
       //     str.Append(a000.Startingdate);
       //     str.Append(a000.EndDate);
       //     str.Append(a000.TodayDate);
       //     str.Append(a000.TimeNow);
       //     str.Append(a000.LanguageCode.PadLeft(1, '0'));
       //     str.Append(a000.Encoding.PadLeft(1, '0'));
       //     str.Append(a000.Field1030.PadRight(20));
       //     str.Append(a000.AccountingCurrencyCode.PadRight(3));
       //     str.Append(a000.BranchInfo.PadLeft(1, '0'));
       //     str.Append(a000.FutureUsage.PadRight(46));
       //     str.AppendLine();

       //     str.Append("C100");
       //     str.Append(String.Format("{0:000000000000000}", C100Counter));
       //     str.AppendLine();

       //     str.Append("D110");
       //     str.Append(String.Format("{0:000000000000000}", D110Counter));
       //     str.AppendLine();

       //     str.Append("D120");
       //     str.Append(String.Format("{0:000000000000000}", D120Counter));

       //     return str.ToString();
       // }

       // //[WebMethod]
       // public byte[] GetTaxReport(DateTime? date1, DateTime? date2, int tenant, string email)
       // {
       //     TenantRepository tenantRep = new TenantRepository(tenant);
       //     Tenant tenantPoco = tenantRep.GetSingleTenant(tenant);

       //     byte[] reportData = this.BuildTaxReportFile(date1, date2, tenantPoco);
       //     ////byte[] bytearray = this.ConvertEncoding(str);

       //     //XmlSerializer serializer = new XmlSerializer(typeof(string));
       //     //MemoryStream memstream = new MemoryStream();
       //     //serializer.Serialize(memstream, str);
       //     //memstream.Seek(0, SeekOrigin.Begin);
       //     //var reader = new StreamReader(memstream);
       //     //string content = reader.ReadToEnd();
       //     //byte[] reportData = memstream.ToArray();
       //     return reportData;
       // }

       // private byte[] BuildTaxReportFile(DateTime? date1, DateTime? date2, Tenant tenant)
       // {
       //     IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant.Id);
       //     ARPaymentRepository paymentRep = new ARPaymentRepository(invoiceCotnext);
       //     ARInvoiceRepository invoiceRep = new ARInvoiceRepository(invoiceCotnext);
       //     APInvoiceRepository apInvoiceRep = new APInvoiceRepository(invoiceCotnext);
       //     ARInvoiceLineRepository invoiceLineRep = new ARInvoiceLineRepository(invoiceCotnext);
       //     APInvoiceLineRepository apInvoiceLineRep = new APInvoiceLineRepository(invoiceCotnext);
       //     AddressRepository addressRep = new AddressRepository(tenant.Id);
       //     CurrencyRepository currencyRep = new CurrencyRepository(tenant.Id);

       //     List<ARPayment> payments = paymentRep.GetARPaymentsByTenant(tenant.Id).Where(p => (p.RegisterDate >= date1 && p.RegisterDate <= date2)).ToList();
       //     List<ARInvoice> invoices = invoiceRep.GetInvoicesByTenant(tenant.Id).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2)).ToList();
       //     List<APInvoice> apInvoices = apInvoiceRep.GetAPInvoicesByTenant(tenant.Id).Where(i => (i.InvoiceDate >= date1 && i.InvoiceDate <= date2)).ToList();
       //     Address address = addressRep.GetSingleAddress(tenant.AddressId, tenant.Id);
       //     Currency currency = currencyRep.GetSingleCurrency(tenant.CurrencyId, tenant.Id);

       //     int C100Counter = 0;
       //     int D110Counter = 0;
       //     int D120Counter = 0;

       //     foreach (ARInvoice invoice in invoices)
       //     {
       //         D110Counter += invoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //     }
       //     foreach (APInvoice invoice in apInvoices)
       //     {
       //         D110Counter += apInvoiceLineRep.GetInvoiceLinesByInvoiceId(invoice.Id, tenant.Id).Count();
       //     }

       //     C100Counter = invoices.Count + apInvoices.Count + payments.Count;
       //     D120Counter = payments.Count;

       //     TaxesApproval.TaxReport report = new TaxesApproval.TaxReport();

       //     report.TodayDate = String.Format("{0:yyyyMMdd}", DateTime.Now);
       //     report.TodayTime = String.Format("{0:hhmm}", DateTime.Now);
       //     report.TenantVATNumber = tenant.VatNumber;
       //     report.TenantName = tenant.Company;
       //     report.FileSavingDirectory = "C:\\OPENFRMT\\" + tenant.VatNumber + "." + String.Format("{0:yy}", DateTime.Now) + "\\" + String.Format("{0:MMddhhmm}", DateTime.Now);
       //     report.FromDate = String.Format("{0:yyyyMMdd}", date1);
       //     report.ToDate = String.Format("{0:yyyyMMdd}", date2);
       //     report.B110Count = "000000000000000";
       //     report.C100Count = String.Format("{0:000000000000000}", C100Counter);
       //     report.D110Count = String.Format("{0:000000000000000}", D110Counter);
       //     report.D120Count = String.Format("{0:000000000000000}", D120Counter);

       //     //StringBuilder str = new StringBuilder();

       //     //str.Append(report.TodayDate);
       //     //str.Append(report.TodayTime);
       //     //str.Append(report.TenantVATNumber.PadLeft(9, '0'));
       //     //str.Append(report.TenantName.PadRight(50));
       //     //str.Append(report.FileSavingDirectory.PadRight(50));
       //     //str.Append(report.FromDate);
       //     //str.Append(report.ToDate);
       //     //str.Append(report.B110Count);
       //     //str.Append(report.C100Count);
       //     //str.Append(report.D110Count);
       //     //str.Append(report.D120Count);

       //     XmlSerializer serializer = new XmlSerializer(typeof(TaxesApproval.TaxReport));
       //     MemoryStream memstream = new MemoryStream();
       //     serializer.Serialize(memstream, report);
       //     memstream.Seek(0, SeekOrigin.Begin);
       //     var reader = new StreamReader(memstream);
       //     string content = reader.ReadToEnd();
       //     byte[] bytearray = memstream.ToArray();

       //     return bytearray;
       // }

       // // encoding
       // private byte[] ConvertEncoding(string s)
       // {
       //     //38598 --- iso-8859-8-i --- Hebrew (ISO-Logical)
       //     Encoding iso = Encoding.GetEncoding(38598);
       //     Encoding utf8 = Encoding.UTF8;

       //     byte[] temp = utf8.GetBytes(s);
       //     byte[] isoBytes = Encoding.Convert(utf8, iso, temp);

       //     return isoBytes;
       // }
    }
}
