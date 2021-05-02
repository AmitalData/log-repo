using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class LedgerTransactionReportLoader
    {

        public const int PAGE_SIZE = 1000 * 20;
        public const int PAGE_RECORD_START_INDEX = 0;

        private QueryOperations reportQueryOperations;
        private bool showLocals = false;
        private int tenant;
        private IAccountingContext accountingContext;
        LedgerTransactionsDataProvider transactionsDataProvider;
        private GLAccountPM glaccountPM;
        private List<GLAccountList> transactionsAccounts = new List<GLAccountList>();

        public LedgerTransactionReportLoader(int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);
            accountingContext = AccountingContext.GetContext(tenant);

        }

        public LedgerTransactionsDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            CheckSalesmanAbilities(BuildReportParameters());

            CardIndexReportService cardIndexReportService = new CardIndexReportService(accountingContext, BuildReportParameters());
            cardIndexReportService.Run();

            glaccountPM = GetGLAccountById(GetFilterValue<string>("GLAccountId"));




            return BuildDataProvider(cardIndexReportService);
        }





        private LedgerTransactionsDataProvider BuildDataProvider(CardIndexReportService cardIndexReportService)
        {
            transactionsDataProvider = new LedgerTransactionsDataProvider();

            transactionsDataProvider.FromDate = GetFromDate();
            transactionsDataProvider.ToDate = GetToDate();

            FillGLAccountFields(cardIndexReportService);
            FillTenantFields();
            FillPrintingInformation();
            FillLedgerTransactions(cardIndexReportService);
            if (GetFilterValue<string>("GLAccountId") != null)
                FillGLAccountBalance(cardIndexReportService.CardIndexs);

            return transactionsDataProvider;
        }

        private void CheckSalesmanAbilities(CardIndexReportParams args)
        {
            UserPM loggedUser = GetLoggedUser();

            bool isSalsmanRestrictionsEnabled = SecurityUtility.CheckFeature("GLAccount", "SalesmanLTRP", args.Tenant);
            if (isSalsmanRestrictionsEnabled && loggedUser?.IsSalesman == true && args.SalesmanId == null)
                throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.NoSalesman", args.Tenant, LoggedContactResolver.GetLoggedContactShowLocal(args.Tenant)));
        }

        private UserPM GetLoggedUser()
        {
            UserPM loggedUser;
            UserQuery userQuery = new UserQuery(tenant);
            if (AuthenticationUtil.AuthenticatedUserEmail != null)
            { // user set and passed from from WR
                loggedUser = userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, tenant);
            }
            else
            {
                ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
                loggedUser = userQuery.GetSinglePM(loggedContact.Id, tenant);
            }
            return loggedUser;
        }

        private DateTime GetToDate()
        {
            return new DateTime(GetFilterValue<DateTime>("ToDate").Year, GetFilterValue<DateTime>("ToDate").Month, GetFilterValue<DateTime>("ToDate").Day, 23, 59, 59);
        }

        private DateTime GetFromDate()
        {
            return new DateTime(GetFilterValue<DateTime>("FromDate").Year, GetFilterValue<DateTime>("FromDate").Month, GetFilterValue<DateTime>("FromDate").Day, 0, 0, 0);
        }

        private void FillGLAccountBalance(List<LedgerTransactionBalanceResponse> cardIndexs)
        {
            LedgerTransactionBalanceFilterCallBack LTBFilterCallBack = BuildLTBFilterCallback(cardIndexs);

            //GLAccountPM glaccountPM = GetGLAccountById(GetFilterValue<string>("GLAccountId"));

            //if (glaccountPM.IsMultiCurrency == true)
                SetBalanceForMultiCurrencyAccount(LTBFilterCallBack);
            //else
                SetBalanceForSingleCurrencyAccount(LTBFilterCallBack);


        }

        private void SetBalanceForSingleCurrencyAccount(LedgerTransactionBalanceFilterCallBack LTBFilterCallBack)
        {
            transactionsDataProvider.LocalOpenBalance = (decimal)LTBFilterCallBack.StartBalanceLocal;
            transactionsDataProvider.LocalClosedBalance = (decimal)LTBFilterCallBack.EndBalanceLocal;
        }

        private void SetBalanceForMultiCurrencyAccount(LedgerTransactionBalanceFilterCallBack LTBFilterCallBack)
        {
            SetOpenBalanceForGLAccount(LTBFilterCallBack);
            SetClosedBalanceForGLAccount(LTBFilterCallBack);
        }

        private void SetClosedBalanceForGLAccount(LedgerTransactionBalanceFilterCallBack LTBFilterCallBack)
        {
            transactionsDataProvider.LocalClosedBalanceList = new List<GLAccountBalanceList>();
            if (LTBFilterCallBack.EndBalanceForeignList.Count == 0)
            {
                transactionsDataProvider.LocalClosedBalanceList.Add(new GLAccountBalanceList()
                {
                    BalanceForeign = 0,
                    BalanceLocal = 0,
                    CurrencyId = "",
                    LocalCurrencySign = "",
                    ForeignCurrencySign = ""
                });
            }

            foreach (var item in LTBFilterCallBack.EndBalanceForeignList)
            {
                // get currency
                CurrencyRepository currencyRepo = new CurrencyRepository(tenant);
                Currency currency = currencyRepo.GetSingleCurrency(item.CurrencyId, tenant);

                transactionsDataProvider.LocalClosedBalanceList.Add(new GLAccountBalanceList()
                {
                    BalanceForeign = item.BalanceForeign,
                    BalanceLocal = item.BalanceLocal,
                    CurrencyId = item.CurrencyId,
                    LocalCurrencySign = GetTenantPM().CurrencySign,
                    ForeignCurrencySign = currency.Sign
                });
            }

            transactionsDataProvider.LocalClosedBalance = transactionsDataProvider.LocalClosedBalanceList.Sum(d => d.BalanceLocal).Value;
            transactionsDataProvider.ForeignClosedBalance = transactionsDataProvider.LocalClosedBalanceList.Sum(d => d.BalanceForeign).Value;

        }

        private void SetOpenBalanceForGLAccount(LedgerTransactionBalanceFilterCallBack LTBFilterCallBack)
        {
            transactionsDataProvider.LocalOpenBalanceList = new List<GLAccountBalanceList>();
            if (LTBFilterCallBack.StartBalanceForeignList.Count == 0)
            {
                transactionsDataProvider.LocalOpenBalanceList.Add(new GLAccountBalanceList()
                {
                    BalanceForeign = 0,
                    BalanceLocal = 0,
                    CurrencyId = "",
                    LocalCurrencySign = "",
                    ForeignCurrencySign = ""
                });
            }

            foreach (var item in LTBFilterCallBack.StartBalanceForeignList)
            {
                // get currency
                Currency currency = GetCurrencyById(item.CurrencyId);

                transactionsDataProvider.LocalOpenBalanceList.Add(new GLAccountBalanceList()
                {
                    BalanceForeign = item.BalanceForeign,
                    BalanceLocal = item.BalanceLocal,
                    CurrencyId = item.CurrencyId,
                    LocalCurrencySign = GetTenantPM().CurrencySign,
                    ForeignCurrencySign = currency.Sign
                });
            }

            transactionsDataProvider.LocalOpenBalance = transactionsDataProvider.LocalOpenBalanceList.Sum(d=>d.BalanceLocal).Value;
            transactionsDataProvider.ForeignOpenBalance = transactionsDataProvider.LocalOpenBalanceList.Sum(d=>d.BalanceForeign).Value;

        }

        private Currency GetCurrencyById(string currencyId)
        {
            CurrencyRepository currencyRepo = new CurrencyRepository(tenant);
            Currency currency = currencyRepo.GetSingleCurrency(currencyId, tenant);
            return currency;
        }
        private void FillLedgerTransactions(CardIndexReportService cardIndexReportService)
        {
            List<LedgerTransactionList> transactions = new List<LedgerTransactionList>();

            List<List<LedgerTransactionList>> transactionsCollections = cardIndexReportService.CardIndexs.Select(d => d.MyLedgerTransactionList).ToList();

            transactionsCollections.ForEach(collection =>
            {
                transactions.AddRange(collection);
            });


            //if (GetFilterValue<string>("ChartOfAccountId") != null)
                transactionsAccounts = GetGLAccountsInsideTransactions(transactions);

            transactionsDataProvider.Transactions = new List<ReportLedgerTransaction>();

            foreach (LedgerTransactionList transaction in transactions)
            {
                ReportLedgerTransaction reportTransaction = GetReportNewLedgerTransaction(transaction);
                FillReportTransactionGLAccountFields(glaccountPM, transactionsAccounts, reportTransaction);

                transactionsDataProvider.Transactions.Add(reportTransaction);
            }

            transactionsDataProvider.LastCumulativeOpenAmount = transactions.Count > 1 ? transactions[transactions.Count - 1].CumulativeOpenAmount : 0;
        }

        private List<GLAccountList> GetGLAccountsInsideTransactions(List<LedgerTransactionList> transactions)
        {
            List<string> accountsIds = transactions.GroupBy(d => d.AccountId).Select(d => d.Key).ToList();
            GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(accountingContext);
            var glaccounts = gLAccountListQueryService.GetByIds(accountsIds, tenant,false).ToList();
            return glaccounts;
        }

        private void FillReportTransactionGLAccountFields(GLAccountPM glaccountPM, List<GLAccountList> accounts, ReportLedgerTransaction reportTransaction)
        {
            if (glaccountPM != null)
            {
                reportTransaction.GLAccountRecoMethodCode = glaccountPM.ReconcileMethodCode;
                reportTransaction.AccountNumber = glaccountPM.DisplayNumber;
                reportTransaction.AccountEnglishName = glaccountPM.EnglishName;
                reportTransaction.AccountLocalName = glaccountPM.LocalName;
            }
            else
            {
                GLAccountList account = accounts.FirstOrDefault(d => d.Id == reportTransaction.AccountId);
                reportTransaction.GLAccountRecoMethodCode = account.ReconcileMethodCode;
                reportTransaction.AccountNumber = account.DisplayNumber;
                reportTransaction.AccountEnglishName = account.EnglishName;
                reportTransaction.AccountLocalName = account.LocalName;

            }
            
        }

        private ReportLedgerTransaction GetReportNewLedgerTransaction(LedgerTransactionList transaction)
        {
            return new ReportLedgerTransaction
            {
                Id = transaction.Id,
                Tenant = transaction.Tenant,
                JournalId = transaction.JournalId,
                JournalLineNumber = transaction.JournalLineNumber,
                CreateDate = transaction.CreateDate,
                ControlAccountId = transaction.ControlAccountId,
                AccountId = transaction.AccountId,
                AccountingDate = transaction.AccountingDate,
                DocumentDate = transaction.DocumentDate,
                DueDate = transaction.DueDate,
                LocalAmountDebit = transaction.LocalAmountDebit,
                LocalAmountCredit = transaction.LocalAmountCredit,
                CurrencyId = transaction.CurrencyId,
                ForeignAmountDebit = transaction.ForeignAmountDebit,
                ForeignAmountCredit = transaction.ForeignAmountCredit,
                ExchangeRate = transaction.ExchangeRate,
                Reference1 = transaction.Reference1,
                Reference2 = transaction.Reference2,
                Reference3 = transaction.Reference3,
                OpenAmount = transaction.OpenAmount,
                OppositeAccountId = transaction.OppositeAccountId,
                SearchFields = transaction.SearchFields,
                OpenAmountCurrencyId = transaction.OpenAmountCurrencyId,
                Notes = transaction.Notes,
                AmountToReconcile = transaction.AmountToReconcile,
                Mark = transaction.Mark,
                IsReconciled = transaction.IsReconciled,
                IsExternalReconcile = transaction.IsExternalReconcile,
                InReconcileProgress = transaction.InReconcileProgress,
                ReconcileRemarks = transaction.ReconcileRemarks,
                CurrencyCode = transaction.CurrencyCode,
                CurrencySign = transaction.CurrencySign,
                OpenAmountCurrencyCode = transaction.OpenAmountCurrencyCode,
                OpenAmountCurrencySign = transaction.OpenAmountCurrencySign,

                Source = transaction.Source,
                JournalNumber = transaction.JournalNumber,
                TenantCurrencySign = GetTenantPM().CurrencySign,

                OppositeAccountDisplayNumber = transaction.OppositeAccountDisplayNumber,
                OppositeAccountEnglishName = transaction.OppositeAccountEnglishName,
                OppositeAccountLocalName = transaction.OppositeAccountLocalName,


                CumulativeForeignAmount = transaction.CumulativeForeignAmount,
                CumulativeLocalAmount = transaction.CumulativeLocalAmount,
                CalculatedForeignAmount = transaction.CalculatedForeignAmount,
                CumulativeOpenAmount = transaction.CumulativeOpenAmount,

            };
        }

        private void FillPrintingInformation()
        {
            transactionsDataProvider.PrintedByUser = GetLoggedContactName();
            transactionsDataProvider.PrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        }

        private string GetLoggedContactName()
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            var x = showLocals ? loggedContact.LocalName : loggedContact.EnglishName;
            return x;
        }

        private void FillTenantFields()
        {
            TenantPM tenantPM = GetTenantPM();
            if (tenantPM != null)
            {
                transactionsDataProvider.TenantCurrencyCode = tenantPM.CurrencyCode;
                transactionsDataProvider.TenantCurrencySign = tenantPM.CurrencySign;
            }

        }

        private TenantPM GetTenantPM()
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            return tenantPM;
        }

        private void FillGLAccountFields(CardIndexReportService cardIndexReportService)
        {
            string glAccountId = GetFilterValue<string>("GLAccountId");
            if (glAccountId != null)
            {
                GLAccountPM glaccountPM = GetGLAccountById(glAccountId);

                transactionsDataProvider.AccountNumber = glaccountPM.DisplayNumber;
                transactionsDataProvider.AccountEnglishName = glaccountPM.EnglishName;
                transactionsDataProvider.AccountLocalName = glaccountPM.LocalName;
                transactionsDataProvider.IsAccountMulticurrency = (bool)glaccountPM.IsMultiCurrency;
                transactionsDataProvider.AccountCurrencySign = glaccountPM.CurrencySign;
                transactionsDataProvider.AccountCurrencyCode = glaccountPM.CurrencyCode;
                transactionsDataProvider.AccountReconcileMethod = glaccountPM.ReconcileMethodCode;

                SetGLAccountStartTotalOpenAmountField(cardIndexReportService);
            }
        }

        private void SetGLAccountStartTotalOpenAmountField(CardIndexReportService cardIndexReportService)
        {
            var cardIndex = cardIndexReportService.CardIndexs.FirstOrDefault();
            if (cardIndex != null)
                transactionsDataProvider.StartTotalOpenAmount = cardIndex.StartTotalOpenAmount;
        }

        private Logitude.Accounting.Def.EntityPMs.GLAccountPM GetGLAccountById(string glAccountId)
        {
            GLAccountQueryService glaQueryService = new GLAccountQueryService(accountingContext);
            var glaccountPM = glaQueryService.GetSingle(glAccountId, false, false);
            return glaccountPM;
        }

        private static LedgerTransactionBalanceFilterCallBack BuildLTBFilterCallback(List<LedgerTransactionBalanceResponse> cardIndexs)
        {
            LedgerTransactionBalanceFilterCallBack LTBFilterCallBack = new LedgerTransactionBalanceFilterCallBack()
            {
                //EndBalanceForeign = cardIndexs.First().EndBalanceForeign,
                EndBalanceForeignList = cardIndexs.First().EndBalanceForeignList,
                EndBalanceLocal = cardIndexs.First().EndBalanceLocal,
                Have1CurrencyIdInPeriod = cardIndexs.First().Have1CurrencyIdInPeriod,
                MaxCreateAt = cardIndexs.First().MaxCreateAt,

                //StartBalanceForeign = ledgerTransactionBalanceService.Response.StartBalanceForeign,
                StartBalanceForeignList = cardIndexs.First().StartBalanceForeignList,
                StartBalanceLocal = cardIndexs.First().StartBalanceLocal,
                TotalRowCount = cardIndexs.First().TotalRowCount,
                YearTransferLedgerTransactionIds = cardIndexs.First().YearTransferLedgerTransactionIds,
                SuppressCumulativeDueMultiCurrencyInPeriod = cardIndexs.First().SuppressCumulativeDueMultiCurrencyInPeriod

            };
            return LTBFilterCallBack;
        }

        private CardIndexReportParams BuildReportParameters()
        {

            CardIndexReportParams cardIndexParameters = new CardIndexReportParams()
            {
                Tenant = tenant,
                From = GetFromDate(),
                To = GetToDate(),
                CurrencyId = GetFilterValue<string>("CurrencyId"),
                DateTypeCode = GetFilterValue<string>("DateTypeCode"),
                GLAccountId = GetFilterValue<string>("GLAccountId"),
                SearchFields = GetFilterValue<string>("SearchFields"),
                PageStartAtRecordIndex = PAGE_RECORD_START_INDEX,
                PageSize = PAGE_SIZE,
                SalesmanId = GetFilterValue<string>("SalesmanUserId"),
                Category1Id = "",
                Category2Id = "",
                Category3Id = "",
                Category4Id = "",
                Category5Id = "",

                ChartOfAccountsId = GetFilterValue<string>("ChartOfAccountId"),
                ChartOfAccountsTypeCode = GetFilterValue<string>("ChartOfAccountsTypeCode"),
                AccountTypeCode = GetFilterValue<string>("AccountTypeCode"),
                IsReconciled = GetFilterValue<bool?>("IsReconciled"),
                IncludeChildAccounts = GetFilterValue<bool>("IncludeChildAccounts"),
                IncludeRelatedCurrenciesAccount = GetFilterValue<bool>("IncludeRelatedCurrenciesAccount"),

            };
            SetReportCategoryParameters(cardIndexParameters);

            return cardIndexParameters;
        }

        private void SetReportCategoryParameters(CardIndexReportParams reportParameters)
        {
            string category1Id = null;
            string category2Id = null;
            string category3Id = null;
            string category4Id = null;
            string category5Id = null;

            string categoryIndex = GetFilterValue<string>("CategoryIndex");
            string categoryValue = GetFilterValue<string>("CategoryValue");

            if (!string.IsNullOrEmpty(categoryIndex))
            {

                switch (categoryIndex)
                {
                    case "Category1": { category1Id = categoryValue; break; }
                    case "Category2": { category2Id = categoryValue; break; }
                    case "Category3": { category3Id = categoryValue; break; }
                    case "Category4": { category4Id = categoryValue; break; }
                    case "Category5": { category5Id = categoryValue; break; }
                }
            }

            reportParameters.Category1Id = category1Id;
            reportParameters.Category2Id = category2Id;
            reportParameters.Category3Id = category3Id;
            reportParameters.Category4Id = category4Id;
            reportParameters.Category5Id = category5Id;
        }
        private QueryOperations DeserializeQueryOperationFromXml(byte[] xmlFilters)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            return queryOperations;
        }
        public T GetFilterValue<T>(string FieldName)
        {
            QueryFilterItem filterItem = reportQueryOperations.QueryFilterItems
                .Where(d => d.FieldName == FieldName).FirstOrDefault();

            if (filterItem != null && filterItem.FieldValue != null)
            {
                return (T)filterItem.FieldValue;
            }

            return default(T);
        }

    }
}