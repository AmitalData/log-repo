using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;
using WebFreight.Web.WebServices;

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
                    report = exportDocumentHelper.LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                }
            }



        }

        public PaymentChequeDataProvider LoadDataProvider(string entityId, int tenant)
        {
            PaymentChequeDataProvider PaymentChequeDP = new PaymentChequeDataProvider();

            PaymentChequeDP.CompanyLogo = WebFreight.Web.DataProviders.General.GetLogo(tenant);
            FullAccountingSettingPM setting=   GetAccountingSettingPM(tenant);
            byte[] byteImage = null;
          
                 byteImage = GetLogo(setting.PaymentChequesLogoId, tenant);
                if (byteImage != null)
                {
                    PaymentChequeDP.AccountingLogo = Image.FromStream(new MemoryStream(byteImage));
                }

            


            PaymentChequeQueryService PaymentChequeQuery = new PaymentChequeQueryService(tenant);
            BankAccountQueryService bankAccountQuery = new BankAccountQueryService(tenant);
            //   CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, false);
            PaymentChequePM paymentChequePM = PaymentChequeQuery.GetSingle(entityId, true, false);
            BankCodePM bankCode = GetBankCodeByPayToGLAccount(paymentChequePM);
            if (bankCode != null) {
             
                byteImage = GetLogo(bankCode.LogoId, tenant);
                if(byteImage != null)
                {
                    PaymentChequeDP.BankLogo = Image.FromStream(new MemoryStream(byteImage));
                }
            }

            if(paymentChequePM.PrintDate == null)
            {
                UpdatePaymentChequePrintDate(paymentChequePM);
               
               
            }
            PaymentChequeDP.PrintDate = paymentChequePM.PrintDate;


            AddressQuery addressQuery = new AddressQuery(tenant);
            AddressPM address = addressQuery.GetSinglePM(tenantPM.AddressId, tenant);
            NumbersConverterToWords numbersConverterToWords = new NumbersConverterToWords();
            //  PaymentChequeDP.APPaymentNumber = GetRelatedPayment(paymentChequePM);
            APPaymentPM appayment = GetRelatedPayment(paymentChequePM);
            if(appayment != null)
            {
                PaymentChequeDP.APPaymentNumber = appayment.PaymentNo;
                PaymentChequeDP.TaxDeductionLocalAmount = appayment.TaxDeductionLocalAmount;
                PaymentChequeDP.TaxDeductionPercentage = appayment.TaxDeductionPercentage;
                PaymentChequeDP.AmountInLocalCurrency = appayment.AmountInLocalCurrency;
                PaymentChequeDP.PrintNotes = appayment.PrintNotes;

            }
            if (paymentChequePM.CurrencyCode == "NIS" || paymentChequePM.CurrencyCode == "ILS")
            {
                string curr_name = "ש\"ח";
                string subunit_name = "אגורות";
                PaymentChequeDP.AmountInHebrew = numbersConverterToWords.NumbersToHebrew((double)paymentChequePM.ForeignAmount, curr_name, subunit_name, false);
            }
            else
            {
                PaymentChequeDP.AmountInHebrew = numbersConverterToWords.NumbersToHebrew((double)paymentChequePM.ForeignAmount, paymentChequePM.CurrencyCode, "", false);
            }
            if (paymentChequePM != null)
            {
                PaymentChequeDP.ChequeNumber = paymentChequePM.ChequeNumber;
                PaymentChequeDP.ValueDate = paymentChequePM.ValueDate.Value;
                PaymentChequeDP.PayToName = paymentChequePM.PayToName;

                // BankAccount mapping
                BankAccountPM bankAccount = bankAccountQuery.GetSingle(paymentChequePM.BankAccountId, false, false);
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
                if (tenantPM != null)
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
                    PaymentChequeDP.Telephone = address.PhoneNumber;
                }
                // map lines
                List<PaymentChequeLine> lines = paymentChequePM.PaymentChequeLines.Select(d => new PaymentChequeLine()
                {
                    Amount = d.Amount,
                    Note = d.Notes,


                }).ToList();
                PaymentChequeDP.PaymentChequeLines = lines;
                PaymentChequeDP.TotalAmount = (decimal)paymentChequePM.ForeignAmount;// (decimal) paymentChequePM.PaymentChequeLines.Sum(d => d.Amount);
                PaymentChequeDP.AccountDisplayNumber = GetPayToGLAccountDisplayNumber(paymentChequePM);

            }

            return PaymentChequeDP;
        }

        public static BankCodePM GetBankCodeByPayToGLAccount(PaymentChequePM paymentCheque)
        {
            // GLAccountPM gLAccount = GetPayToGLAccount(paymentCheque);
            BankAccountPM bankAccount = GetBankAccountByPaymentChequet(paymentCheque.BankAccountId, paymentCheque.Tenant);


            return GetBankCodePM(bankAccount);
            
           

        }
        private void UpdatePaymentChequePrintDate(PaymentChequePM paymentCheque)
        {
            paymentCheque.PrintDate = TenantServerConfigration.GetCurrentDateTime(paymentCheque.Tenant);
            paymentCheque.ChangeSetOp = ChangeSetOperation.Update;
            var accountingContext = AccountingContext.GetContext(paymentCheque.Tenant);

            PaymentChequeUpdateService service = new PaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), paymentCheque.Tenant);
            service.Update(paymentCheque, true);

        }
        public static BankAccountPM GetBankAccountByPaymentChequet(string  bankAccountId, int tenant)
        {
            BankAccountQueryService bankAccountQuery = new BankAccountQueryService(tenant);
            return bankAccountQuery.GetSingle(bankAccountId, false, false);
        }

        public static BankCodePM GetBankCodePM(BankAccountPM bankAccount)
        {
            BankCodeQueryService bankCodeQuery = new BankCodeQueryService(bankAccount.Tenant);
            return bankCodeQuery.GetSingleByCode(bankAccount.BankCode, bankAccount.Tenant);
        }

        //public static GLAccountPM GetPayToGLAccount(PaymentChequePM paymentCheque)
        //{
        //    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(paymentCheque.Tenant);
        //    return gLAccountQueryService.GetSinglePM(paymentCheque.PayToGLAccountId, paymentCheque.Tenant);

        //}
        public static byte[] GetLogo(string id,int tenant)
        {
           
            ImageDetail imageDetail=   GetImageDetail(id,tenant);
            if (imageDetail != null)
            {
                return GetFile(imageDetail.Id, imageDetail.Extension, "images", tenant);
            }
            else return null;
        }

        public static ImageDetail GetImageDetail(string id, int tenant)
        {
            ImageDetailRepository imageDetailsRepository = new ImageDetailRepository(tenant);
           return imageDetailsRepository.GetSingleImageDetail(id, tenant);
        
        }

        public static byte[] GetFile(string fileid, string extention, string location, int tenant)
        {
            try
            {
             

                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = fileid,
                    FolderName = location,
                    Extension = extention,
                    Tenant = tenant,

                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;

                return storageservice.Read(fileInfo);



            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static FullAccountingSettingPM GetAccountingSettingPM(int tenant)
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            return fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
        }


        public APPaymentPM GetRelatedPayment(PaymentChequePM paymentCheque)
        {
            APPaymentQuery paymentQuery = new APPaymentQuery(paymentCheque.Tenant);
            APPaymentPM payment = paymentQuery.GetSingleAPPaymentPM(paymentCheque.APPaymentId, paymentCheque.Tenant);
            return payment;
        }

        public string GetPayToGLAccountDisplayNumber(PaymentChequePM paymentCheque)
        {
            GLAccountQueryService accountQueryService = new GLAccountQueryService(paymentCheque.Tenant);
            GLAccountPM account = accountQueryService.GetSinglePM(paymentCheque.PayToGLAccountId, paymentCheque.Tenant);
            if(account != null)
            {
                return account.DisplayNumber;

            }
            else
            {
                return null;
            }

        }

    }
}