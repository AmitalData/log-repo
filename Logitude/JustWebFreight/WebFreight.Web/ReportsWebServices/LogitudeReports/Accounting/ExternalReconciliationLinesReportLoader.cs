using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.AccountingModel.LedgerTransactionService;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Security;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class ExternalReconciliationLinesReportLoader
    {
        private int tenant;
        private string Type;
        private string SortBy;
        private DateTime? RefDateFrom;
        private DateTime? RefDateTo;
        private string BankAccountId;
        private string IsExternalReconciled;
        private bool IncludesTransferGlaccount;
        private int? ExternalReconciliationNumber;
        private string ObjectTableId;
        List<TransactionBalance> transactionsBalances;
        IAccountingContext accountingContext;

        private ExternalReconciliationLinesReportDataProvider dataProvider;
        private QueryOperations reportQueryOperations;

        public ExternalReconciliationLinesReportLoader(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            accountingContext = AccountingContext.GetContext(tenant);


            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            SetReportParameters();
        }


        public ExternalReconciliationLinesReportDataProvider LoadDataProviderByXML(byte[] xmlFilters)
        {
            reportQueryOperations = DeserializeQueryOperationFromXml(xmlFilters);

            BuildDataProvider();

            return dataProvider;
        }



        LedgerTransactionBalanceFilter LTBFilter;
        public byte[] GetData()
        {
            BuildDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExternalReconciliationLinesReportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, dataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void BuildDataProvider()
        {
            dataProvider = new ExternalReconciliationLinesReportDataProvider();

            SetReportHeaderFields();

            SetBankPagesClosingBalance();

            SetBankDetails();

        }
        private void SetReportHeaderFields()
        {
            QueryOperationsFilterValueGetter valueGetter = new QueryOperationsFilterValueGetter(this.reportQueryOperations);

            if (valueGetter.GetFilterValue<int?>("ExternalReconciliationNumber") == null)
            {
                dataProvider.BankAccountId = valueGetter.GetFilterValue<string>("BankAccountId");
                dataProvider.Type = valueGetter.GetFilterValue<string>("Type");
                dataProvider.RefDateFrom = valueGetter.GetFilterValue<DateTime>("REFFromDate");
                dataProvider.RefDateTo = valueGetter.GetFilterValue<DateTime>("REFToDate");
                dataProvider.IsExternalReconciled = valueGetter.GetFilterValue<string>("IsExternalReconciled");
                dataProvider.IncludesTransferGlaccount = valueGetter.GetFilterValue<bool>("IncludesTransferGlaccount");
            }

            dataProvider.ExternalReconciliationNumber = valueGetter.GetFilterValue<int>("ExternalReconciliationNumber");
            dataProvider.SortBy = valueGetter.GetFilterValue<string>("SortBy");


        }

        private void SetBankPagesClosingBalance()
        {
            ExternalPagesBalanceService externalPagesBalanceService = new ExternalPagesBalanceService(tenant);
            dataProvider.BankPagesClosingBalance = externalPagesBalanceService.GetClosingBalanceByDate("BankAccount", BankAccountId, RefDateTo.Value);

        }
        private LedgerTransactionBalanceFilter CreateLedgerTransactionBalanceFilter(ExternalReconciliationPeriod period)
        {
            LedgerTransactionBalanceFilter ledgerTransactionBalanceFilter = new LedgerTransactionBalanceFilter
            {
                GLAccountId = period.GLAccountId,
                From = RefDateFrom.Value,
                To = RefDateTo.Value,
                DateTypeCode = "3",
                Tenant = tenant,
                PageSize = 30
            };
            return ledgerTransactionBalanceFilter;
        }
        private List<TransactionBalance> GetTransactionBalancesList()
        {
            transactionsBalances = new List<TransactionBalance>();
            foreach (ExternalReconciliationPeriod period in externalReconciliationsPeriods)
            {
                var exist = transactionsBalances.Where(d => d.BankAccountId == period.BankAccountId).Any();
                if (!exist)
                {
                    LedgerTransactionBalanceFilter ledgerTransactionBalanceFilter = CreateLedgerTransactionBalanceFilter(period);

                    TransactionsBalanceByFiltersService transactionsBalanceByFiltersService = new TransactionsBalanceByFiltersService();
                    TransactionsBalanceByFiltersResult transactionsBalanceByFiltersResult = transactionsBalanceByFiltersService.GetTransactionsBalanceByFilters(tenant, null, ledgerTransactionBalanceFilter);
                    TransactionBalance transactionBalance = new TransactionBalance()
                    {
                        BankAccountId = period.BankAccountId,
                        GLAccountId = period.GLAccountId,
                        TotalInLocalCurrency = transactionsBalanceByFiltersResult.ledgerTransactionBalanceService.Response.EndBalanceLocal
                    };
                    transactionsBalances.Add(transactionBalance);
                }
            }
            return transactionsBalances;
        }

        List<BankAccountPM> bankAccounts;
        List<ExternalReconciliationPeriod> externalReconciliationsPeriods;
        private void SetBankDetails()
        {
            bankAccounts = GetBankAccounts();

            externalReconciliationsPeriods = GetExternalReconciliationsPeriods(bankAccounts);

            if (bankAccounts == null)
                bankAccounts = GetBankAccountsFromExternalReoncilePeriods(externalReconciliationsPeriods);

            transactionsBalances = GetTransactionBalancesList();

            dataProvider.BankDetails = (from a in bankAccounts
                                        select new BankDetails()
                                        {
                                            BankAccountCode = a.AccountNumber,
                                            BankAccountLocalName = a.LocalName,
                                            BankAccountEnglishName = a.EnglishName,
                                            BankAccountId = a.Id,

                                            TotalClosed = externalReconciliationsPeriods == null ? null : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.IsRecomncile == true).Sum(b => b.Amount),
                                            TotalOpen = externalReconciliationsPeriods == null ? null : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.IsRecomncile == false).Sum(b => b.Amount),
                                            ExternalReconciliationPeriods = externalReconciliationsPeriods == null ? null : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id).ToList(),
                                            GlaccountTotalOpened = externalReconciliationsPeriods == null ? 0 : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.EnglishType == "GLAccount" && s.IsRecomncile == false).Sum(b => b.Amount),
                                            GlaccountTotalClosed = externalReconciliationsPeriods == null ? 0 : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.EnglishType == "GLAccount" && s.IsRecomncile == true).Sum(b => b.Amount),
                                            TransferTotalOpened = externalReconciliationsPeriods == null ? 0 : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.EnglishType == "Transfer" && s.IsRecomncile == false).Sum(b => b.Amount),
                                            TransferTotalClosed = externalReconciliationsPeriods == null ? 0 : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.EnglishType == "Transfer" && s.IsRecomncile == true).Sum(b => b.Amount),
                                            BankTotalOpened = externalReconciliationsPeriods == null ? 0 : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.EnglishType == "Bank" && s.IsRecomncile == false).Sum(b => b.Amount),
                                            BankTotalClosed = externalReconciliationsPeriods == null ? 0 : externalReconciliationsPeriods.Where(s => s.BankAccountId == a.Id && s.EnglishType == "Bank" && s.IsRecomncile == true).Sum(b => b.Amount),

                                            TotalInLocalCurrency = transactionsBalances.Where(d => d.BankAccountId == a.Id).FirstOrDefault() != null ? transactionsBalances.Where(d => d.BankAccountId == a.Id).FirstOrDefault().TotalInLocalCurrency : null,
                                        })
                                        .Where(s => s.ExternalReconciliationPeriods.Count > 0)
                                        .ToList();
        }

        private List<BankAccountPM> GetBankAccounts()
        {
            IQueryable<BankAccountPM> bankAccounts = GetTenantBankAccounts();

            if (!string.IsNullOrEmpty(BankAccountId))
                bankAccounts = bankAccounts.Where(s => s.Id == BankAccountId);

            return bankAccounts.ToList();
        }

        private List<BankAccountPM> GetBankAccountsFromExternalReoncilePeriods(List<ExternalReconciliationPeriod> externalReoncilePeriods)
        {

            List<string> BankAccountIds = (from a in externalReoncilePeriods select a.BankAccountId).ToList();

            IQueryable<BankAccountPM> AllBankAccounts = GetBankAccountsFromBankAccountIds(BankAccountIds);

            return AllBankAccounts.ToList();

        }

        private IQueryable<BankAccountPM> GetTenantBankAccounts()
        {
            IQueryable<BankAccountPM> AllBankAccounts = GetIQueryableBankAccountPM().Where(page => page.Tenant == tenant && page.Inactive == false);


            return AllBankAccounts;
        }

        private IQueryable<BankAccountPM> GetBankAccountsFromBankAccountIds(List<string> BankAccountIds)
        {
            IQueryable<BankAccountPM> AllBankAccounts = GetIQueryableBankAccountPM().Where(page => page.Tenant == tenant && page.Inactive == false && BankAccountIds.Contains(page.Id));


            return AllBankAccounts;
        }

        private IQueryable<BankAccountPM> GetIQueryableBankAccountPM()
        {
            

            IQueryable<BankAccountPM> AllBankAccounts = (from page in accountingContext.BankAccounts
                                                         select new BankAccountPM()
                                                         {
                                                             Id = page.Id,
                                                             LocalName = page.LocalName,
                                                             EnglishName = page.EnglishName,
                                                             AccountNumber = page.AccountNumber,
                                                             GLAccountId = page.GLAccountId,
                                                             Tenant = page.Tenant,
                                                             Inactive = page.Inactive,
                                                             TransferGLAcccountId = page.TransferGLAcccountId,
                                                         });


            return AllBankAccounts;

        }

        private List<ExternalReconciliationPeriod> GetExternalReconciliationsPeriods(List<BankAccountPM> bankAccounts)
        {
            if (ExternalReconciliationNumber != null)
                return GetExternalReconcilePeriodsByReconcileNumber(bankAccounts);
            else
                return GetExternalReconcilePeriodsOfAllBankAccount(bankAccounts);
        }

        private List<ExternalReconciliationPeriod> GetExternalReconcilePeriodsOfAllBankAccount(List<BankAccountPM> bankAccounts)
        {
            if (Type == "bank")
            {
                var reconcileExternalPageLines = GetExternalPagesLines();
                return BuildExternalReconciliationPeriods(reconcileExternalPageLines);
            }
            else if (Type == "glAccount")
            {
                var ledgerTransactions = GetNormalTransactionsOfBanksGLAccounts(bankAccounts);
                List<LedgerTransactionList> transferledgerTransactions = IncludesTransferGlaccount ? GetTransferTransactionsOfBanksGLAccounts(bankAccounts) : null;
                return BuildExternalReconciliationPeriods(null, ledgerTransactions, transferledgerTransactions);
            }
            else
            {
                var reconcileExternalPageLines = GetExternalPagesLines();
                var ledgerTransactions = GetNormalTransactionsOfBanksGLAccounts(bankAccounts);
                List<LedgerTransactionList> transferledgerTransactions = IncludesTransferGlaccount ? GetTransferTransactionsOfBanksGLAccounts(bankAccounts) : null;
                
                return BuildExternalReconciliationPeriods(reconcileExternalPageLines, ledgerTransactions, transferledgerTransactions);
            }
        }

        private List<ExternalReconciliationPeriod> GetExternalReconcilePeriodsByReconcileNumber(List<BankAccountPM> bankAccounts)
        {
            var reconcileExternalPageLines = GetExternalPagesLines();
            var ledgerTransactions = GetNormalTransactionsOfReconciliation(bankAccounts);
            var transferledgerTransactions = GetTransferTransactionsOfReconciliation(bankAccounts);

            var periods = BuildExternalReconciliationPeriods(reconcileExternalPageLines, ledgerTransactions, transferledgerTransactions.ToList());
            return periods;
        }

        public List<ExternalReconciliationPeriod> BuildExternalReconciliationPeriods(IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines = null,
            IQueryable<LedgerTransactionList> ledgerTransactions = null,
            List<LedgerTransactionList> TransferledgerTransactions = null)
        {

            List<ExternalReconciliationPeriod> periodsResult = null;
            if (reconcileExternalPageLines != null)
            {
                periodsResult = MappinReconcileExternalPageLineToPeriods(reconcileExternalPageLines);

            }
            if (ledgerTransactions != null)
            {
                periodsResult = periodsResult.Union(MappinLedgerTransactionToPeriods(ledgerTransactions.ToList())).ToList();

            }
            if (TransferledgerTransactions != null)
            {
                periodsResult = periodsResult.Union(MappinLedgerTransactionToPeriods(TransferledgerTransactions, true)).ToList();

            }

            if (periodsResult != null)
            {
                foreach (ExternalReconciliationPeriod period in periodsResult)
                {
                    if (!string.IsNullOrEmpty(period.EntitySource))
                    {
                        period.EntitySource = AccountingEntities.getEntityIcon(period.EntitySource);
                    }

                    if (period.ExternalPageLineId != null && periodsResult.Where(s => s.ExternalPageLineId == period.ExternalPageLineId).Count() > 1)
                    {
                        period.IsDuplicated = true;
                    }
                }

            }

            periodsResult = SortExternalReconciliationPeriodBy(periodsResult);
            return periodsResult;
        }


        private List<ExternalReconciliationPeriod> MappinReconcileExternalPageLineToPeriods(IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines)
        {

            IQueryable<ExternalReconciliationLine> ExternalReconciliationLines = (from a in accountingContext.ExternalReconciliationLines where a.ExternalReconciliation.IsCancelled == false select a);
            List<ExternalReconciliationPeriod> periods = (from a in reconcileExternalPageLines
                                                          join BK in accountingContext.BankAccounts on a.ReconcileExternalPage.GLAccountId equals BK.GLAccountId
                                                          join Ex in ExternalReconciliationLines on a.Id equals Ex.ExternalPageLineId into ReconcileExternalPageLinesJoinExternalReconciliation
                                                          from Ex in ReconcileExternalPageLinesJoinExternalReconciliation.DefaultIfEmpty()
                                                          where BK.Inactive == false //&& (Ex.ExternalReconciliation == null || Ex.ExternalReconciliation.IsCancelled == false)
                                                          select new ExternalReconciliationPeriod()
                                                          {
                                                              EnglishType = "Bank",
                                                              LocalType = "בנק",
                                                              BankAccountId = BK.Id,
                                                              Number = BK.AccountNumber,
                                                              Amount = a.CreditAmount != 0 ? a.CreditAmount * -1 : a.DebitAmount,
                                                              ReferenceDate = a.ReferenceDate,
                                                              EntitySource = null,
                                                              EntityType = a.ReconcileExternalPage == null ? null : (a.ReconcileExternalPage.PageNo == null ? null : a.ReconcileExternalPage.PageNo.ToString()),
                                                              IsRecomncile = a.IsReconciled,
                                                              LocalBoolean = a.IsReconciled == true ? "כן" : "לא",
                                                              EnglishBoolean = a.IsReconciled == true ? "True" : "False",
                                                              Note = a.Notes,
                                                              ReconcileNumber = Ex.ReconciliationId == null ? null : Ex.ExternalReconciliation.ReconciliationNumber,
                                                              Ref1 = a.Reference,
                                                              Ref2 = null,
                                                              ExternalPageLineId = Ex.ExternalPageLineId,
                                                              GLAccountId = BK.GLAccountId

                                                          }).ToList();


            return periods;
        }

        private List<ExternalReconciliationPeriod> MappinLedgerTransactionToPeriods(List<LedgerTransactionList> ledgerTransactions, bool IsTransfer = false)
        {
            

            IQueryable<ExternalReconciliationLine> ExternalReconciliationLines = (from a in accountingContext.ExternalReconciliationLines.Include("ExternalReconciliation") 
                                                                                  where a.ExternalReconciliation.IsCancelled == false 
                                                                                  select a);
            
            List<ExternalReconciliationPeriod> periods = (from a in ledgerTransactions
                                                          join BK in accountingContext.BankAccounts on a.AccountId equals IsTransfer == true ? BK.TransferGLAcccountId : BK.GLAccountId
                                                          join Ex in ExternalReconciliationLines on a.Id equals Ex.LedgerTransactionId into LedgerTransactionJoinExternalReconciliation
                                                          from Ex in LedgerTransactionJoinExternalReconciliation.DefaultIfEmpty()
                                                          where BK.Inactive == false //&& (Ex.ExternalReconciliation==null || Ex.ExternalReconciliation.IsCancelled == false)
                                                          select new ExternalReconciliationPeriod()
                                                          {
                                                              EnglishType = IsTransfer ? "Transfer" : "GLAccount",
                                                              LocalType = IsTransfer ? "דחוי " : "חשבון ",
                                                              Number = a.AccountDisplayNumber,
                                                              BankAccountId = BK.Id,
                                                              Amount = a.ForeignAmountDebit == 0 ? a.ForeignAmountCredit * -1 : a.ForeignAmountDebit,
                                                              ReferenceDate = a.DocumentDate,
                                                              EntitySource = a.SourceTypeCode,
                                                              EntityType = a.SourceNumber,
                                                              IsRecomncile = a.IsExternalReconcile,
                                                              LocalBoolean = a.IsExternalReconcile == true ? "כן" : "לא",
                                                              EnglishBoolean = a.IsExternalReconcile == true ? "True" : "False",
                                                              Note = a.Notes,
                                                              ReconcileNumber = Ex != null && Ex.ExternalReconciliation != null ? Ex.ExternalReconciliation.ReconciliationNumber : null,
                                                              Ref1 = a.Reference1,
                                                              Ref2 = a.Reference2,
                                                              ExternalPageLineId = null,
                                                              GLAccountId = BK.GLAccountId,
                                                          }).ToList();


            return periods;
        }

        private IQueryable<LedgerTransactionList> GetNormalTransactionsOfBanksGLAccounts(List<BankAccountPM> bankAccounts)
        {
            LedgerTransactionsFilter transactionsFilter = BuildNormalTransactionsFilter(bankAccounts);
            IQueryable<LedgerTransactionList> transactionsQuery = GetFilteredTransactions(transactionsFilter);

            transactionsQuery = FilterTransactionQueryByRefDatePeriod(transactionsQuery);

            return transactionsQuery;
        }

        private IQueryable<LedgerTransactionList> FilterTransactionQueryByRefDatePeriod(IQueryable<LedgerTransactionList> transactionsQuery)
        {
            transactionsQuery = transactionsQuery.Where(transaction => DbFunctions.TruncateTime(transaction.DocumentDate) >= DbFunctions.TruncateTime(RefDateFrom) && DbFunctions.TruncateTime(transaction.DocumentDate) <= DbFunctions.TruncateTime(RefDateTo));
            return transactionsQuery;
        }

        private List<LedgerTransactionList> GetTransferTransactionsOfBanksGLAccounts(List<BankAccountPM> bankAccounts)
        {
            LedgerTransactionsFilter transactionsFilter = BuildTransferTransactionsFilter(bankAccounts);
            IQueryable<LedgerTransactionList> transactionsQuery = GetFilteredTransactions(transactionsFilter);
            IQueryable<LedgerTransactionList> externalTransactions = GetTransferAccountsExternalTransactions(bankAccounts);

            transactionsQuery = FilterTransactionQueryByRefDatePeriod(transactionsQuery);
            externalTransactions = FilterTransactionQueryByRefDatePeriod(externalTransactions);

            var transferTransactions = new List<LedgerTransactionList>();
            transferTransactions.AddRange(transactionsQuery);
            transferTransactions.AddRange(externalTransactions);


            return transferTransactions;
        }

        private IQueryable<LedgerTransactionList> GetTransferAccountsExternalTransactions(List<BankAccountPM> bankAccounts)
        {
            List<string> transferAccountsIds = bankAccounts.Select(a => a.TransferGLAcccountId).ToList();
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(accountingContext);
            var externalTransactions = transactionListQuery.GetExternalTransactionsOfAccounts(tenant, transferAccountsIds);
            return externalTransactions;
        }

        private IQueryable<LedgerTransactionList> GetNormalTransactionsOfReconciliation(List<BankAccountPM> bankAccounts)
        {
            IQueryable<LedgerTransactionList> ledgerTransactions = GetExternalReconciliationTransactions();

            List<string> accountIds = bankAccounts.Select(a => a.GLAccountId).ToList();
            ledgerTransactions = ledgerTransactions.Where(transaction => accountIds.Contains(transaction.AccountId));

            return ledgerTransactions;
        }

        private IQueryable<LedgerTransactionList> GetTransferTransactionsOfReconciliation(List<BankAccountPM> bankAccounts)
        {
            IQueryable<LedgerTransactionList> ledgerTransactions = GetExternalReconciliationTransactions();

            List<string> accountIds = bankAccounts.Select(a => a.TransferGLAcccountId).ToList();
            ledgerTransactions = ledgerTransactions.Where(transaction => accountIds.Contains(transaction.AccountId));

            return ledgerTransactions;
        }

        private IQueryable<LedgerTransactionList> GetExternalReconciliationTransactions()
        {
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(accountingContext);
            IQueryable<LedgerTransactionList> ledgerTransactions = transactionListQuery.GetExternalReconciliationsTransactions(tenant, ExternalReconciliationNumber);
            return ledgerTransactions;
        }

        private IQueryable<LedgerTransactionList> GetFilteredTransactions(LedgerTransactionsFilter transferTransactionsFilter)
        {
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(accountingContext);
            IQueryable<LedgerTransactionList> transactionsQuery = transactionListQuery.GetFilteredTransactions(transferTransactionsFilter);
            return transactionsQuery;
        }

        private LedgerTransactionsFilter BuildNormalTransactionsFilter(List<BankAccountPM> AllBankAccounts)
        {
            return new LedgerTransactionsFilter()
            {
                Tenant = tenant,
                AccountsIds = AllBankAccounts.Select(a => a.GLAccountId).ToList(),
            };
        }
        private LedgerTransactionsFilter BuildTransferTransactionsFilter(List<BankAccountPM> AllBankAccounts)
        {
            return new LedgerTransactionsFilter()
            {
                Tenant = tenant,
                AccountsIds = AllBankAccounts.Select(a => a.TransferGLAcccountId).ToList(),
                GetDueDatedTransactions = true,
                AllowedSourceTypes = new string[] { AccountingEntityValues.APPayment, AccountingEntityValues.PaymentCheque }
            };
        }

        private string GetTenantCurrency()
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            return tenantPM.CurrencyId;
        }


        private IQueryable<ReconcileExternalPageLine> GetExternalPagesLines()
        {

            IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines = null;

            if (ExternalReconciliationNumber == null)
            {


                reconcileExternalPageLines = (from line in accountingContext.ReconcileExternalPageLines
                                              join page in accountingContext.ReconcileExternalPages
                                                            on line.ReconcileExternalPageId equals page.Id
                                              where line.Tenant == tenant
                                                    && line.ReferenceDate != null
                                                    && page.StatusCode != "3"
                                                    && DbFunctions.TruncateTime(line.ReferenceDate) >= DbFunctions.TruncateTime(RefDateFrom)
                                                    && DbFunctions.TruncateTime(line.ReferenceDate) <= DbFunctions.TruncateTime(RefDateTo)

                                              select line);

                if (!string.IsNullOrEmpty(ObjectTableId))
                {
                    reconcileExternalPageLines = reconcileExternalPageLines.Where(line => line.ReconcileExternalPage.ObjectTableId == ObjectTableId);
                }
                if (!string.IsNullOrEmpty(BankAccountId))
                {
                    reconcileExternalPageLines = reconcileExternalPageLines.Where(line => line.ReconcileExternalPage.EntityId == BankAccountId);
                }

                if (IsExternalReconciled == "close")
                {
                    reconcileExternalPageLines = reconcileExternalPageLines.Where(s => s.IsReconciled == true);
                }
                else if (IsExternalReconciled == "open")
                {
                    reconcileExternalPageLines = reconcileExternalPageLines.Where(s => s.IsReconciled == false);
                }


            }
            else
            {

                reconcileExternalPageLines = (from a in accountingContext.ExternalReconciliationLines where a.ExternalReconciliation.ReconciliationNumber == ExternalReconciliationNumber && a.Tenant == tenant && a.ExternalPageLineId != null select a.ReconcileExternalPageLine);

            }



            return reconcileExternalPageLines;

        }


        private List<ExternalReconciliationPeriod> SortExternalReconciliationPeriodBy(List<ExternalReconciliationPeriod> ExternalReconciliationPeriods)
        {
            if (ExternalReconciliationPeriods != null)
            {
                switch (SortBy)
                {
                    case "ReferenceDate":
                        {
                            ExternalReconciliationPeriods = ExternalReconciliationPeriods.OrderByDescending(s => s.ReferenceDate).ToList();

                            break;
                        }
                    case "ReconcileNumber":
                        {
                            ExternalReconciliationPeriods = ExternalReconciliationPeriods.OrderByDescending(s => s.ReconcileNumber).ToList();

                            break;
                        }
                    case "Amount":
                        {
                            ExternalReconciliationPeriods = ExternalReconciliationPeriods.OrderByDescending(s => s.Amount).ToList();

                            break;
                        }

                }
            }

            return ExternalReconciliationPeriods;
        }

        private void SetReportParameters()
        {
            QueryOperationsFilterValueGetter valueGetter = new QueryOperationsFilterValueGetter(reportQueryOperations);

            BankAccountId = valueGetter.GetFilterValue<string>("BankAccountId");
            Type = valueGetter.GetFilterValue<string>("Type");
            RefDateFrom = valueGetter.GetFilterValue<DateTime>("REFFromDate");
            RefDateTo = valueGetter.GetFilterValue<DateTime>("REFToDate");
            IsExternalReconciled = valueGetter.GetFilterValue<string>("IsExternalReconciled");
            IncludesTransferGlaccount = valueGetter.GetFilterValue<bool>("IncludesTransferGlaccount");
            ExternalReconciliationNumber = valueGetter.GetFilterValue<int?>("ExternalReconciliationNumber");
            SortBy = valueGetter.GetFilterValue<string>("SortBy");
            ObjectTableId = valueGetter.GetFilterValue<string>("ObjectTableId");

        }
        private QueryOperations DeserializeQueryOperationFromXml(byte[] xmlFilters)
        {
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            return queryOperations;
        }

    }

}

class TransactionBalance
{
    public string BankAccountId { get; set; }
    public string GLAccountId { get; set; }
    public decimal? TotalInLocalCurrency { get; set; }
}