using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class NewLedgerTransactionReportLoader
    {

        public const int PAGE_SIZE = 1000 * 20;
        public const int PAGE_RECORD_START_INDEX = 0;

        private QueryOperations reportQueryOperations;
        private readonly int tenant;
        private readonly IAccountingContext accountingContext;
        private NewLedgerTransactionDataProvider transactionsDataProvider;
        private List<GLAccountList> transactionsAccounts = new List<GLAccountList>();
        private TenantPM tenantPM;

        public NewLedgerTransactionReportLoader(int _tenant)
        {
            tenant = _tenant;
            accountingContext = AccountingContext.GetContext(tenant);
            tenantPM = GetTenantPM();

        }

        public NewLedgerTransactionDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);
            var reportParams = BuildReportParameters();
            CheckSalesmanAbilities(reportParams);
           CardIndexReportService cardIndexService = new CardIndexReportService(accountingContext, reportParams);
            cardIndexService.Run();

            return BuildDataProvider(cardIndexService);
        }



        private NewLedgerTransactionDataProvider BuildDataProvider(CardIndexReportService cardIndexService)
        {
            transactionsDataProvider = new NewLedgerTransactionDataProvider
            {
                FromDate = GetFromDate(),
                ToDate = GetToDate()
            };

            FillTenantFields();
            FillPrintingInformation();
            FillGLAccountsData(cardIndexService.CardIndexs);
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
            var userQuery = new UserQuery(tenant);
            if (AuthenticationUtil.AuthenticatedUserEmail != null)
                return userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, tenant);

            var loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            return userQuery.GetSinglePM(loggedContact.Id, tenant);
        }
        private DateTime GetToDate()
        {
            var toDate = GetFilterValue<DateTime?>("ToDate");
            return toDate?.Date ?? throw new InvalidOperationException("ToDate filter is required.");
        }

        private DateTime GetFromDate()
        {
            var fromDate = GetFilterValue<DateTime?>("FromDate");
            return fromDate?.Date ?? throw new InvalidOperationException("FromDate filter is required.");
        }

        private void FillGLAccountsData(List<LedgerTransactionBalanceResponse> cardIndexs)
        {
            var glAccountQueryService = new GLAccountListQueryService(accountingContext);

            transactionsDataProvider.NewGLAccountList = new List<NewGLAccountList>();
            var allTransactions = cardIndexs.SelectMany(ci => ci.MyLedgerTransactionList ?? Enumerable.Empty<LedgerTransactionList>()).ToList();
            var accountIds = cardIndexs.Select(t => t.GLAccountId).Distinct().ToList();
             var glAccounts = glAccountQueryService.GetByIds(accountIds, tenant, false).ToList();

            foreach (LedgerTransactionBalanceResponse index in cardIndexs)
            {
                var ltbCallback = BuildLTBFilterCallback(new List<LedgerTransactionBalanceResponse> { index });
                var accountTransactions = allTransactions
                    .Where(t => t.AccountId == index.GLAccountId).ToList();
                var glAccount = glAccounts.FirstOrDefault(g => g.Id == index.GLAccountId);
                if (glAccount == null) continue;

                var accountList = new NewGLAccountList
                {
                    Id = glAccount.Id,
                    AccountNumber = glAccount.DisplayNumber,
                    AccountEnglishName = glAccount.EnglishName,
                    AccountLocalName = glAccount.LocalName,
                    IsAccountMulticurrency = glAccount.IsMultiCurrency ?? false,
                    AccountCurrencySign = glAccount.CurrencySign,
                    AccountCurrencyCode = glAccount.CurrencyCode,
                    AccountReconcileMethod = glAccount.ReconcileMethodCode,
                    LastCumulativeOpenAmount = accountTransactions.LastOrDefault()?.CumulativeOpenAmount ?? 0,
                    StartTotalOpenAmount = index.StartTotalOpenAmount
                };

                 SetOpenBalanceForGLAccount(ltbCallback, accountList);
                 SetClosedBalanceForGLAccount(ltbCallback, accountList);
                 SetBalanceForSingleCurrencyAccount(ltbCallback, accountList);
                 accountList.Transactions = accountTransactions
                                    .Select(transaction =>
                                    {
                                        var reportTransaction = GetReportNewLedgerTransaction(transaction);
                                        FillReportTransactionGLAccountFields(glAccounts, reportTransaction);
                                        return reportTransaction;
                                    }).ToList();

                 transactionsDataProvider.NewGLAccountList.Add(accountList);

                
            }
        }

        private void SetBalanceForSingleCurrencyAccount(LedgerTransactionBalanceFilterCallBack LTBFilterCallBack , NewGLAccountList newGLAccountList)
        {
            newGLAccountList.LocalOpenBalance = (decimal)LTBFilterCallBack.StartBalanceLocal;
            newGLAccountList.LocalClosedBalance = (decimal)LTBFilterCallBack.EndBalanceLocal;
        }


        private void SetClosedBalanceForGLAccount(LedgerTransactionBalanceFilterCallBack ltbCallback, NewGLAccountList newGLAccountList)
        {
            newGLAccountList.LocalClosedBalanceList = new List<NewGLAccountBalanceList>();
            if (ltbCallback.EndBalanceForeignList == null || ltbCallback.EndBalanceForeignList.Count == 0)
            {
                newGLAccountList.LocalClosedBalanceList.Add(new NewGLAccountBalanceList());
            }
            else
            {
                foreach (var item in ltbCallback.EndBalanceForeignList)
                {
                    var currency = GetCurrencyById(item.CurrencyId);
                    var glAccountMoreData = new GLAccountMoreDataRepository(tenant).GetSingle(ltbCallback.GLAccountId, tenant);

                    newGLAccountList.LocalClosedBalanceList.Add(new NewGLAccountBalanceList
                    {
                        BalanceForeign = item.BalanceForeign,
                        BalanceLocal = item.BalanceLocal,
                        CurrencyId = item.CurrencyId,
                        LocalCurrencySign = tenantPM.CurrencySign,
                        ForeignCurrencySign = currency.Sign,
                        LocalBalanceInDue = glAccountMoreData.LocalBalanceInDue,
                        BalanceInForeignCurrency = glAccountMoreData.BalanceInForeignCurrency,
                        ForeignBalanceInDue = glAccountMoreData.ForeignBalanceInDue
                    });
                }
            }
            newGLAccountList.LocalClosedBalance = newGLAccountList.LocalClosedBalanceList.Sum(d => d.BalanceLocal).GetValueOrDefault();
            newGLAccountList.ForeignClosedBalance = newGLAccountList.LocalClosedBalanceList.Sum(d => d.BalanceForeign).GetValueOrDefault();
        }
        private void SetOpenBalanceForGLAccount(LedgerTransactionBalanceFilterCallBack ltbCallback,NewGLAccountList newGLAccountList)
        {
            newGLAccountList.LocalOpenBalanceList = new List<NewGLAccountBalanceList>();
            if (ltbCallback.StartBalanceForeignList == null || ltbCallback.StartBalanceForeignList.Count == 0)
            {
                newGLAccountList.LocalOpenBalanceList.Add(new NewGLAccountBalanceList());
            }
            else
            {
                foreach (var item in ltbCallback.StartBalanceForeignList)
                {
                    var currency = GetCurrencyById(item.CurrencyId);
                    newGLAccountList.LocalOpenBalanceList.Add(new NewGLAccountBalanceList
                    {
                        BalanceForeign = item.BalanceForeign,
                        BalanceLocal = item.BalanceLocal,
                        CurrencyId = item.CurrencyId,
                        LocalCurrencySign =tenantPM.CurrencySign,
                        ForeignCurrencySign = currency.Sign
                    });
                }
            }
            newGLAccountList.LocalOpenBalance = newGLAccountList.LocalOpenBalanceList.Sum(d => d.BalanceLocal).GetValueOrDefault();
            newGLAccountList.ForeignOpenBalance = newGLAccountList.LocalOpenBalanceList.Sum(d => d.BalanceForeign).GetValueOrDefault();
        }

        private Currency GetCurrencyById(string currencyId)
        {
            return new CurrencyRepository(tenant).GetSingleCurrency(currencyId, tenant);
        }

        private List<GLAccountList> GetGLAccountsInsideTransactions(List<LedgerTransactionList> transactions)
        {
            var accountIds = transactions.Select(t => t.AccountId).Distinct().ToList();
            var glAccountListQueryService = new GLAccountListQueryService(accountingContext);
            return glAccountListQueryService.GetByIds(accountIds, tenant, false).ToList();
        }


        private void FillReportTransactionGLAccountFields(List<GLAccountList> accounts, NewReportLedgerTransaction reportTransaction)
        {
            var account = accounts.FirstOrDefault(a => a.Id == reportTransaction.AccountId);
            if (account == null) return;

            reportTransaction.GLAccountRecoMethodCode = account.ReconcileMethodCode;
            reportTransaction.AccountNumber = account.DisplayNumber;
            reportTransaction.AccountEnglishName = account.EnglishName;
            reportTransaction.AccountLocalName = account.LocalName;
            reportTransaction.Collector = account.Collector;
            reportTransaction.PaymentTerms = account.PaymentTerms;
            reportTransaction.Category1Id = account.Category1LocalName;
            reportTransaction.Category2Id = account.Category2LocalName;
            reportTransaction.Category3Id = account.Category3LocalName;
            reportTransaction.Category4Id = account.Category4LocalName;
            reportTransaction.Category5Id = account.Category5LocalName;
        }
        private NewReportLedgerTransaction GetReportNewLedgerTransaction(LedgerTransactionList transaction)
        {
            return new NewReportLedgerTransaction
            {
                Id = transaction.Id,
                Tenant = transaction.Tenant,
                JournalId = transaction.JournalId,
                JournalLineNumber = transaction.JournalLineNumber,
                CreateDate = transaction.CreateDate.GetValueOrDefault(),
                ControlAccountId = transaction.ControlAccountId,
                AccountId = transaction.AccountId,
                AccountingDate = transaction.AccountingDate.Date.ToString("dd/MM/yyyy"), 
                DocumentDate = transaction.DocumentDate.Date.ToString("dd/MM/yyyy"), 
                DueDate = transaction.DueDate.Date.ToString("dd/MM/yyyy"), 
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
                JournalCreatedByUser = transaction.JournalCreatedByUser,
                TenantCurrencySign = tenantPM.CurrencySign,

                OppositeAccountDisplayNumber = transaction.OppositeAccountDisplayNumber,
                OppositeAccountEnglishName = transaction.OppositeAccountEnglishName,
                OppositeAccountLocalName = transaction.OppositeAccountLocalName,


                CumulativeForeignAmount = transaction.CumulativeForeignAmount,
                CumulativeLocalAmount = transaction.CumulativeLocalAmount,
                CalculatedForeignAmount = transaction.CalculatedForeignAmount,
                CumulativeOpenAmount = transaction.CumulativeOpenAmount,

                IsExternalEntity = transaction.IsExternalEntity,

            };
        }

        private void FillPrintingInformation()
        {
            var contact = AuthenticationUtil.AuthenticatedUserEmail != null
                ? GetContactByEmail(AuthenticationUtil.AuthenticatedUserEmail)
                : GetLoggedContact();

            transactionsDataProvider.PrintedByUser = GetContactName(contact);
            transactionsDataProvider.UserEnglishName = contact.EnglishName;
            transactionsDataProvider.PrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        }

         private ContactPM GetLoggedContact() => LoggedContactResolver.GetLoggedContact(tenant);
        private ContactPM GetContactByEmail(string email) => new ContactQuery(tenant).GetContactByEmailOnly(email, tenant);
        private string GetContactName(ContactPM contact) => contact.DontShowLocal ? contact.EnglishName : contact.LocalName;
   
        private void FillTenantFields()
        {
            if (tenantPM == null) return;
            transactionsDataProvider.TenantCurrencyCode = tenantPM.CurrencyCode;
            transactionsDataProvider.TenantCurrencySign = tenantPM.CurrencySign;
        }

        private TenantPM GetTenantPM()
        {
            return new TenantQuery(tenant).GetSinglePM(tenant);
        }


        private static LedgerTransactionBalanceFilterCallBack BuildLTBFilterCallback(List<LedgerTransactionBalanceResponse> cardIndexs)
        {
            var first = cardIndexs.FirstOrDefault();
            if (first == null) return new LedgerTransactionBalanceFilterCallBack();
            return new LedgerTransactionBalanceFilterCallBack
            {
                EndBalanceForeignList = first.EndBalanceForeignList,
                EndBalanceLocal = first.EndBalanceLocal,
                Have1CurrencyIdInPeriod = first.Have1CurrencyIdInPeriod,
                MaxCreateAt = first.MaxCreateAt,
                StartBalanceForeignList = first.StartBalanceForeignList,
                StartBalanceLocal = first.StartBalanceLocal,
                TotalRowCount = first.TotalRowCount,
                YearTransferLedgerTransactionIds = first.YearTransferLedgerTransactionIds,
                SuppressCumulativeDueMultiCurrencyInPeriod = first.SuppressCumulativeDueMultiCurrencyInPeriod,
                GLAccountId = first.GLAccountId
            };

        }

        private CardIndexReportParams BuildReportParameters()
        {

            var p = new CardIndexReportParams
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
                CollectorId = GetFilterValue<string>("CollectorId"),
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
                UseSecurityLevel = GetFilterValue<bool>("UseSecurityLevel"),
                ListGLAccounts = GetFilterValue<string>("ListGLAccounts"),
                FromGLAccountDisplayNumber = GetFilterValue<string>("FromGLAccountDisplayNumber"),
                ToGLAccountDisplayNumber = GetFilterValue<string>("ToGLAccountDisplayNumber"),
            };
            SetReportCategoryParameters(p);
            return p;

        }

        private void SetReportCategoryParameters(CardIndexReportParams p)
        {
            string categoryIndex = GetFilterValue<string>("CategoryIndex");
            string categoryValue = GetFilterValue<string>("CategoryValue");

            if (!string.IsNullOrEmpty(categoryIndex))
            {
                switch (categoryIndex)
                {
                    case "Category1": p.Category1Id = categoryValue; break;
                    case "Category2": p.Category2Id = categoryValue; break;
                    case "Category3": p.Category3Id = categoryValue; break;
                    case "Category4": p.Category4Id = categoryValue; break;
                    case "Category5": p.Category5Id = categoryValue; break;
                }
            }
        }
        private QueryOperations DeserializeQueryOperationFromXml(byte[] xmlFilters)
        {
            using (var memorystream = new MemoryStream(xmlFilters))
            {
                var serializer = new XmlSerializer(typeof(QueryOperations));
                return (QueryOperations)serializer.Deserialize(memorystream);
            }
        }
        public T GetFilterValue<T>(string fieldName)
        {
            var filterItem = reportQueryOperations.QueryFilterItems
                .FirstOrDefault(d => d.FieldName == fieldName);

            if (filterItem != null && filterItem.FieldValue != null)
                return (T)filterItem.FieldValue;

            return default(T);
        }

    }
}