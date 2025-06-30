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
using Logitude.Accounting.Data.Repositories;

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

        public NewLedgerTransactionReportLoader(int _tenant)
        {
            tenant = _tenant;
            accountingContext = AccountingContext.GetContext(tenant);

        }

        public NewLedgerTransactionDataProvider LoadFromXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);
            var reportParams = BuildReportParameters();
            CheckSalesmanAbilities(reportParams);
           CardIndexReportService cardIndexReportService = new CardIndexReportService(accountingContext, reportParams);
            cardIndexReportService.Run();
            return BuildDataProvider(cardIndexReportService);
        }



        private NewLedgerTransactionDataProvider BuildDataProvider(CardIndexReportService cardIndexReportService)
        {
            transactionsDataProvider = new NewLedgerTransactionDataProvider
            {
                FromDate = GetFromDate(),
                ToDate = GetToDate()
            };

            FillGLAccountFields(cardIndexReportService);
            FillTenantFields();
            FillPrintingInformation();
            FillLedgerTransactions(cardIndexReportService);

            if (!string.IsNullOrEmpty(GetFilterValue<string>("GLAccountId")) && cardIndexReportService.CardIndexs.Any())
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
            var userQuery = new UserQuery(tenant);
            if (AuthenticationUtil.AuthenticatedUserEmail != null)
                return userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, tenant);

            var loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
            return userQuery.GetSinglePM(loggedContact.Id, tenant);
        }
        private DateTime GetToDate()
        {
            var fromDate = GetFilterValue<DateTime?>("FromDate");
            if (fromDate.HasValue)
                return new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, 0, 0, 0);
            throw new InvalidOperationException("FromDate filter is required.");
        }

        private DateTime GetFromDate()
        {
            var fromDate = GetFilterValue<DateTime?>("FromDate");
            if (fromDate.HasValue)
                return new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, 0, 0, 0);
            throw new InvalidOperationException("FromDate filter is required.");
           }

        private void FillGLAccountBalance(List<LedgerTransactionBalanceResponse> cardIndexs)
        {
            var ltbCallback = BuildLTBFilterCallback(cardIndexs);
            SetOpenBalanceForGLAccount(ltbCallback);
            SetClosedBalanceForGLAccount(ltbCallback);
            SetBalanceForSingleCurrencyAccount(ltbCallback);
        }

        private void SetBalanceForSingleCurrencyAccount(LedgerTransactionBalanceFilterCallBack LTBFilterCallBack)
        {
            transactionsDataProvider.LocalOpenBalance = (decimal)LTBFilterCallBack.StartBalanceLocal;
            transactionsDataProvider.LocalClosedBalance = (decimal)LTBFilterCallBack.EndBalanceLocal;
        }


        private void SetClosedBalanceForGLAccount(LedgerTransactionBalanceFilterCallBack ltbCallback)
        {
            transactionsDataProvider.LocalClosedBalanceList = new List<NewGLAccountBalanceList>();
            if (ltbCallback.EndBalanceForeignList == null || ltbCallback.EndBalanceForeignList.Count == 0)
            {
                transactionsDataProvider.LocalClosedBalanceList.Add(new NewGLAccountBalanceList());
            }
            else
            {
                foreach (var item in ltbCallback.EndBalanceForeignList)
                {
                    var currency = GetCurrencyById(item.CurrencyId);
                    var glAccountMoreData = new GLAccountMoreDataRepository(tenant).GetSingle(ltbCallback.GLAccountId, tenant);

                    transactionsDataProvider.LocalClosedBalanceList.Add(new NewGLAccountBalanceList
                    {
                        BalanceForeign = item.BalanceForeign,
                        BalanceLocal = item.BalanceLocal,
                        CurrencyId = item.CurrencyId,
                        LocalCurrencySign = GetTenantPM().CurrencySign,
                        ForeignCurrencySign = currency.Sign,
                        LocalBalanceInDue = glAccountMoreData.LocalBalanceInDue,
                        BalanceInForeignCurrency = glAccountMoreData.BalanceInForeignCurrency,
                        ForeignBalanceInDue = glAccountMoreData.ForeignBalanceInDue
                    });
                }
            }
            transactionsDataProvider.LocalClosedBalance = transactionsDataProvider.LocalClosedBalanceList.Sum(d => d.BalanceLocal).GetValueOrDefault();
            transactionsDataProvider.ForeignClosedBalance = transactionsDataProvider.LocalClosedBalanceList.Sum(d => d.BalanceForeign).GetValueOrDefault();
        }
        private void SetOpenBalanceForGLAccount(LedgerTransactionBalanceFilterCallBack ltbCallback)
        {
            transactionsDataProvider.LocalOpenBalanceList = new List<NewGLAccountBalanceList>();
            if (ltbCallback.StartBalanceForeignList == null || ltbCallback.StartBalanceForeignList.Count == 0)
            {
                transactionsDataProvider.LocalOpenBalanceList.Add(new NewGLAccountBalanceList());
            }
            else
            {
                foreach (var item in ltbCallback.StartBalanceForeignList)
                {
                    var currency = GetCurrencyById(item.CurrencyId);
                    transactionsDataProvider.LocalOpenBalanceList.Add(new NewGLAccountBalanceList
                    {
                        BalanceForeign = item.BalanceForeign,
                        BalanceLocal = item.BalanceLocal,
                        CurrencyId = item.CurrencyId,
                        LocalCurrencySign = GetTenantPM().CurrencySign,
                        ForeignCurrencySign = currency.Sign
                    });
                }
            }
            transactionsDataProvider.LocalOpenBalance = transactionsDataProvider.LocalOpenBalanceList.Sum(d => d.BalanceLocal).GetValueOrDefault();
            transactionsDataProvider.ForeignOpenBalance = transactionsDataProvider.LocalOpenBalanceList.Sum(d => d.BalanceForeign).GetValueOrDefault();
        }

        private Currency GetCurrencyById(string currencyId)
        {
            return new CurrencyRepository(tenant).GetSingleCurrency(currencyId, tenant);
        }
        private void FillLedgerTransactions(CardIndexReportService cardIndexReportService)
        {
            var transactions = cardIndexReportService.CardIndexs
                .SelectMany(ci => ci.MyLedgerTransactionList ?? Enumerable.Empty<LedgerTransactionList>())
                .ToList();

            transactionsAccounts = GetGLAccountsInsideTransactions(transactions);
            foreach (var account in transactionsAccounts)
            {
                var accountTransactions = transactions
                    .Where(t => t.AccountId == account.Id)
                    .ToList();

                var newGLAccountList = new NewGLAccountList
                {
                    Transactions = accountTransactions
                        .Select(transaction =>
                        {
                            var reportTransaction = GetReportNewLedgerTransaction(transaction);
                            FillReportTransactionGLAccountFields(transactionsAccounts, reportTransaction);
                            return reportTransaction;
                        })
                        .ToList()
                };
                 newGLAccountList.LastCumulativeOpenAmount = accountTransactions.LastOrDefault()?.CumulativeOpenAmount ?? 0;
                  transactionsDataProvider.NewGLAccountList.Add(newGLAccountList);

            }
           
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
                TenantCurrencySign = GetTenantPM().CurrencySign,

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
            transactionsDataProvider.PrintedByUser = GetLoggedContactName();
            transactionsDataProvider.UserEnglishName = GetLoggedContactEnglishName();
            transactionsDataProvider.PrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        }

        private string GetContactName(ContactPM loggedContact)
        {
            return loggedContact.DontShowLocal ? loggedContact.EnglishName : loggedContact.LocalName;
        }

        private ContactPM  GetLoggedContact()
        {
            return LoggedContactResolver.GetLoggedContact(tenant);
           
        }
        private string GetLoggedContactName()
        {
            var contact = AuthenticationUtil.AuthenticatedUserEmail != null
                ? GetContactByEmail(AuthenticationUtil.AuthenticatedUserEmail)
                : GetLoggedContact();
            return GetContactName(contact);
        }
        private string GetLoggedContactEnglishName()
        {
            var contact = AuthenticationUtil.AuthenticatedUserEmail != null
                ? GetContactByEmail(AuthenticationUtil.AuthenticatedUserEmail)
                : GetLoggedContact();
            return contact.EnglishName;
        }
        private ContactPM GetContactByEmail(string email)
        {
            return new ContactQuery(tenant).GetContactByEmailOnly(email, tenant);
        }
        private void FillTenantFields()
        {
            var tenantPM = GetTenantPM();
            if (tenantPM == null) return;
            transactionsDataProvider.TenantCurrencyCode = tenantPM.CurrencyCode;
            transactionsDataProvider.TenantCurrencySign = tenantPM.CurrencySign;
        }

        private TenantPM GetTenantPM()
        {
            return new TenantQuery(tenant).GetSinglePM(tenant);
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
            }
        }

      


        private GLAccountPM GetGLAccountById(string glAccountId)
        {
            return new GLAccountQueryService(accountingContext).GetSingle(glAccountId, false, false);
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
                ListGLAccounts = GetFilterValue<List<string>>("ListGLAccounts"),
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