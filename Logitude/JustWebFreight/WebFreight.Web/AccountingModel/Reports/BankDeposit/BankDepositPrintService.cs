using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.AccountingModel.Reports.BankDeposit
{
    class BankDepositPrintService
    {
        private readonly int ChequePairValidationDays = 183;
        public BankDepositPM bankDepositPM;
        public void BuildBankDepositReport(string entityId, int tenant, string documentOutId)
        {
            // 1 
            // Fill DataProvider
            BankDepositDataProvider bankDepositDP = LoadDataProvider(entityId, tenant);


            // 2
            // Get byte[] of DataProvider
            XmlSerializer serializer = new XmlSerializer(typeof(BankDepositDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, bankDepositDP);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();


            // 3
            // Get StiObject
            StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "BDPR", Name = "BankDepositDataProvider", BusinessObjectValue = bankDepositDP };


            // 4
            //Build report
            Byte[] templatedata = null;
            StiReport report = new StiReport();
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);


            DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
            DocumentTypeTemplatePM defaulttemplate = documentTypeTemplateQuery.GetById(documentOut.DocumentTemplateId, tenant);

            if (defaulttemplate != null)
                templatedata = defaulttemplate.TemplateBody;


            if (templatedata != null)
            {

                if (templatedata.Length != 0)
                {
                    defaulttemplate.DocumentOutId = documentOut?.Id;
                    ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                    report = exportDocumentHelper.LoadandRender( defaulttemplate, currentBusinessObject, tenant);
                }
            }



        }

        public BankDepositDataProvider LoadDataProvider(string entityId, int tenant)
        {
            BankDepositDataProvider bankDepositDP = new BankDepositDataProvider();
            BankDepositQueryService bankDepositQuery = new BankDepositQueryService(tenant);
            BankAccountQueryService bankAccountQuery = new BankAccountQueryService(tenant);
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);

            bankDepositPM = bankDepositQuery.GetSingle(entityId, true, false);
   
            if(bankDepositPM != null)
            {
                bankDepositDP.DepositNumber = bankDepositPM.DepositNumber;
                bankDepositDP.BankAccountNumber = bankDepositPM.BankAccountNumber == null ? "" : bankDepositPM.BankAccountNumber;
                bankDepositDP.DepositDate = bankDepositPM.DepositDate;
                bankDepositDP.DepositDate = bankDepositPM.DepositDate;
                bankDepositDP.LocalDepositAmount = bankDepositPM.LocalDepositAmount;
                bankDepositDP.ForeignAmount = bankDepositPM.ForeignAmount;
                bankDepositDP.CurrencyCode = bankDepositPM.DepositCurrencyCode;

                SetCreatedByUserName(tenant, bankDepositDP, bankDepositPM);

                // BankAccount mapping
                BankAccountPM bankAccount = bankAccountQuery.GetSingle(bankDepositPM.DepositBankAccountId, false,false);
                if (bankAccount != null)
                {
                    bankDepositDP.BankAccountBranchNo = bankAccount.BranchNumber == null ? "" : bankAccount.BranchNumber;
                    bankDepositDP.BankAccountBranchAddress = bankAccount.BranchAddress == null ? "" : bankAccount.BranchAddress;
                    bankDepositDP.BankAccountLocalName = bankAccount.LocalName == null ? "" : bankAccount.LocalName;
                }


                // map lines
                List<BankDepositLine> lines = bankDepositPM.BankDepositLines
                       .Select(d => new
                       {
                           Line = d,
                           TimeGroupKey = (int)(d.DueDate.Ticks / TimeSpan.FromDays(this.ChequePairValidationDays).Ticks)
                       })
                       .GroupBy(x => new
                       {
                           x.Line.Bank,
                           x.Line.Branch,
                           x.Line.AccountNumber,
                           x.Line.ChequeNumber,
                           x.TimeGroupKey
                       })
                      .Select(group => new BankDepositLine
                      {
                          Bank = group.Key.Bank,
                          Branch = group.Key.Branch,
                          AccountNumber = group.Key.AccountNumber,
                          ChequeNumber = group.Key.ChequeNumber,
                          DueDate = group.First().Line.DueDate,
                          Currency = group.First().Line.Currency,
                          Line = group.First().Line.Line,
                          ARPaymentNumber = group.First().Line.ARPaymentNumber,
                          LocalAmount = group.Sum(x => x.Line.LocalAmount),
                          ForiegnAmount = group.Sum(x => x.Line.ForeignAmount)
                    
                      }).ToList();

                bankDepositDP.BankDepositLines = lines;

            }

            return bankDepositDP;
        }

        private static void SetCreatedByUserName(int tenant, BankDepositDataProvider bankDepositDP, BankDepositPM bankDepositPM)
        {
            UserQuery userQuery = new UserQuery(tenant);
            UserPM userPM = userQuery.GetSinglePM(bankDepositPM.CreatedByUserId, tenant);
            if (userPM != null)
            {
                bool showLocals = !userPM.DontShowLocalLabels;
                bankDepositDP.CreatedByUserName = showLocals ? userPM.LocalName == null ? userPM.EnglishName : userPM.LocalName : userPM.EnglishName;
            }
        }

        private ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }
}
