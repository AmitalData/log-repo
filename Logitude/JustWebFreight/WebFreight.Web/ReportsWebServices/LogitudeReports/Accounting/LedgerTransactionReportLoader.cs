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

        public LedgerTransactionReportLoader(int _tenant)
        {
            tenant = _tenant;
            showLocals = LoggedContactResolver.GetLoggedContactShowLocal(_tenant);
            accountingContext = AccountingContext.GetContext(tenant);

        }

        public LedgerTransactionsDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            CardIndexReportService cardIndexReportService = new CardIndexReportService(accountingContext, BuildReportParameters());
            cardIndexReportService.Run();

            return BuildDataProvider(cardIndexReportService);
        }



        private LedgerTransactionsDataProvider BuildDataProvider(CardIndexReportService cardIndexReportService)
        {
            transactionsDataProvider = new LedgerTransactionsDataProvider();

            transactionsDataProvider.FromDate = GetFromDate();
            transactionsDataProvider.ToDate = GetToDate();

            FillGLAccountFields(GetFilterValue<string>("GLAccountId"));
            FillTenantFields();
            FillPrintingInformation();
            FillLedgerTransactions(cardIndexReportService);
            if (GetFilterValue<string>("GLAccountId") != null)
                FillGLAccountBalance(cardIndexReportService.CardIndexs);

            return transactionsDataProvider;
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

            GLAccountPM glaccountPM = GetGLAccountById(GetFilterValue<string>("GLAccountId"));

            if (glaccountPM.IsMultiCurrency == true)
                SetBalanceForMultiCurrencyAccount(LTBFilterCallBack);
            else
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

            transactionsDataProvider.Transactions = new List<ReportLedgerTransaction>();

            GLAccountPM glaccountPM = GetGLAccountById(GetFilterValue<string>("GLAccountId"));

            foreach (LedgerTransactionList transaction in transactions)
            {
                ReportLedgerTransaction reportTransaction = GetReportNewLedgerTransaction(transaction);

                if (glaccountPM != null)
                    reportTransaction.GLAccountRecoMethodCode = glaccountPM.ReconcileMethodCode;

                transactionsDataProvider.Transactions.Add(reportTransaction);
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

                CumulativeForeignAmount = transaction.CumulativeForeignAmount,
                CumulativeLocalAmount = transaction.CumulativeLocalAmount,

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

        private void FillGLAccountFields(string glAccountId)
        {
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
            }
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
                
                Category1Id = "",
                Category2Id = "",
                Category3Id = "",
                Category4Id = "",
                Category5Id = "",

                ChartOfAccountsId = GetFilterValue<string>("ChartOfAccountId"),
                AccountTypeCode = GetFilterValue<string>("AccountTypeCode"),
                IsReconciled = GetFilterValue<bool>("IsReconciled"),
                IncludeChildAccounts = GetFilterValue<bool>("IncludeChildAccounts"),
                IncludeRelatedCurrenciesAccount = GetFilterValue<bool>("IncludeRelatedCurrenciesAccount"),

            };
            return cardIndexParameters;
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