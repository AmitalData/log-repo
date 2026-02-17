using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;
namespace WebFreight.Web.AccountingModel.Reports.PaymentCheque
{
    public class PaymentChequePrintService
    {

        public void BuildPaymentChequeReport(string entityId, int tenant, string documentOutId)
        {
           
            PaymentChequeDataProvider PaymentChequeDP = LoadDataProvider(entityId, tenant);


            // 2
            // Get byte[] of DataProvider
            XmlSerializer serializer = new XmlSerializer(typeof(PaymentChequeDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, PaymentChequeDP);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();


            // 3
            // Get StiObject
            StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "PCDR", Name = "PaymentChequeDataProvider", BusinessObjectValue = PaymentChequeDP };


            // 4
            //Build report
            Byte[] templatedata = null;
            StiReport report = new StiReport();
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);


            DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
            DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentOut.DocumentTemplateId);

            if (defaulttemplate != null)
                templatedata = defaulttemplate.TemplateBody;


            if (templatedata != null)
            {
                if (templatedata.Length != 0)
                {
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    report = exportDocumentHelper.LoadandRender(report, templatedata, defaulttemplate, currentBusinessObject, documentTypeTemplaterep, tenant);
                }
            }



        }

        public PaymentChequeDataProvider LoadDataProvider(string entityId, int tenant)
        {
            PaymentChequeDataProvider PaymentChequeDP = new PaymentChequeDataProvider();

            PaymentChequeDP.CompanyLogo = WebFreight.Web.DataProviders.General.GetLogo(tenant);
            PaymentChequeQueryService PaymentChequeQuery = new PaymentChequeQueryService(tenant);
            BankAccountQueryService bankAccountQuery = new BankAccountQueryService(tenant);
         //   CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant,false);
            PaymentChequePM paymentChequePM = PaymentChequeQuery.GetSingle(entityId, true, false);
            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSinglePM(tenantPM.AddressId, tenant);
            InvoiceWebService invoiceWebService = new InvoiceWebService();
            if (paymentChequePM.CurrencyCode == "NIS" || paymentChequePM.CurrencyCode == "ILS")
            {
                string curr_name = "ש\"ח";
                string subunit_name = "אגורות";
                PaymentChequeDP.AmountInHebrew = invoiceWebService.NumbersToHebrew((double)paymentChequePM.ForeignAmount, curr_name, subunit_name, false);
            }
            else
            { 
                PaymentChequeDP.AmountInHebrew = invoiceWebService.NumbersToHebrew((double)paymentChequePM.ForeignAmount, paymentChequePM.CurrencyCode, "", false);
            }
            if (paymentChequePM != null)
            {
                PaymentChequeDP.ChequeNumber = paymentChequePM.ChequeNumber;
                PaymentChequeDP.ValueDate = paymentChequePM.ValueDate.Value;
                PaymentChequeDP.PayToName = paymentChequePM.PayToName;

                // BankAccount mapping
                BankAccountPM bankAccount = bankAccountQuery.GetSingle(paymentChequePM.BankAccountId, false,false);
                if (bankAccount != null)
                {
                    PaymentChequeDP.BankAccountNumber = bankAccount.AccountNumber;
                    PaymentChequeDP.BankAddress = bankAccount.BranchAddress == null ? "" : bankAccount.BranchAddress;
                    PaymentChequeDP.BankCode = bankAccount.BankCode;
                    PaymentChequeDP.BranchNumber = bankAccount.BranchNumber;
                }
                else
                {
                    //no connected bank account

                }
                if(tenantPM != null)
                {
                    //PaymentChequeDP.AddressName = tenantPM.CompanyAddress;
                    PaymentChequeDP.VatNumber = tenantPM.VatNumber;

                    PaymentChequeDP.Signature = tenantPM.Signature;
                }

                if (address != null)
                {
                    PaymentChequeDP.Address1 = address.Address1;
                    PaymentChequeDP.Address2 = address.Address2;
                    PaymentChequeDP.FAX = address.FaxNumber;
                    PaymentChequeDP.AddressName = address.Name;
                }
                // map lines
                List<PaymentChequeLine> lines = paymentChequePM.PaymentChequeLines.Select(d => new PaymentChequeLine()
                {
                  Amount = d.Amount,
                  Note = d.Notes,
                   
                  
                }).ToList();
                PaymentChequeDP.PaymentChequeLines = lines;
                PaymentChequeDP.TotalAmount =(decimal) paymentChequePM.ForeignAmount;// (decimal) paymentChequePM.PaymentChequeLines.Sum(d => d.Amount);
                

            }

            return PaymentChequeDP;
        }

   
    }
}