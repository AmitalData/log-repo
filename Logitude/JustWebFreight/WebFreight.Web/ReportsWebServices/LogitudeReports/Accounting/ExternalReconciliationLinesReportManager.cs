using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Security;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class ExternalReconciliationLinesReportManager
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
        IAccountingContext accountingContext;
        private ExternalReconciliationLinesReportDataProvider iDataProvider;
        public ExternalReconciliationLinesReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            accountingContext = AccountingContext.GetContext(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.FilterByRefDates(iQueryOperations);
            this.FilterByTypeAndBank(iQueryOperations);
            this.FilterByReconciled(iQueryOperations);
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExternalReconciliationLinesReportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void LoadDataProvider()
        {
            this.iDataProvider = new ExternalReconciliationLinesReportDataProvider();
            this.BuildReportHeader();
            this.BuildSourceData();
        }
        private void BuildReportHeader()
        {

            if (this.ExternalReconciliationNumber ==null)
            {
                iDataProvider.BankAccountId = this.BankAccountId;
                iDataProvider.Type = this.Type;
                iDataProvider.RefDateFrom = this.RefDateFrom;
                iDataProvider.RefDateTo = this.RefDateTo;
                iDataProvider.IsExternalReconciled = this.IsExternalReconciled;
                iDataProvider.IncludesTransferGlaccount = this.IncludesTransferGlaccount;

            }
            iDataProvider.ExternalReconciliationNumber = this.ExternalReconciliationNumber;
            iDataProvider.SortBy = this.SortBy;


        }
        private void BuildSourceData()
        {
            List<BankAccountPM> AllBankAccounts = GetAllBankAccountsByFilters();

            List<ExternalReconciliationPeriod> AllExternalReconciliationPeriod = GetAllBankTransactionForTypeFilter(AllBankAccounts);
            if (AllBankAccounts==null)
            {
                AllBankAccounts =  GetAllBankAccountsFromTransactions(AllExternalReconciliationPeriod);
            }
            if (AllBankAccounts!=null)
            {
                iDataProvider.BankDetails = (from a in AllBankAccounts
                                             select new BankDetails()
                                             {
                                                 BankAccountCode = a.AccountNumber,
                                                 BankAccountLocalName = a.LocalName,
                                                 BankAccountEnglishName = a.EnglishName,
                                                 BankAccountId = a.Id,
                                                 TotalClosed = AllExternalReconciliationPeriod == null ? null : AllExternalReconciliationPeriod.Where(s => s.BankAccountId == a.Id && s.IsRecomncile == true).Sum(b => b.Amount),
                                                 TotalOpen = AllExternalReconciliationPeriod == null ? null : AllExternalReconciliationPeriod.Where(s => s.BankAccountId == a.Id && s.IsRecomncile == false).Sum(b => b.Amount),
                                                 ExternalReconciliationPeriods = AllExternalReconciliationPeriod == null ? null : AllExternalReconciliationPeriod.Where(s => s.BankAccountId == a.Id).ToList(),

                                             }).Where(s => s.ExternalReconciliationPeriods.Count > 0).ToList();

            }

        }


        private List<BankAccountPM> GetAllBankAccountsByFilters()
        {

            if (ExternalReconciliationNumber == null)
            {
                IQueryable<BankAccountPM> AllBankAccounts = GetBankAccounts();

                if (!string.IsNullOrEmpty(this.BankAccountId))
                {
                    AllBankAccounts = AllBankAccounts.Where(s => s.Id == this.BankAccountId);
                }

                return AllBankAccounts.ToList();
            }

            else
            {

                return null;

            }

        }

        private List<BankAccountPM> GetAllBankAccountsFromTransactions(List<ExternalReconciliationPeriod> AllExternalReconciliationPeriod)
        {

            List<string> BankAccountIds = (from a in AllExternalReconciliationPeriod select a.BankAccountId).ToList();

            IQueryable<BankAccountPM> AllBankAccounts = GetBankAccountsFromBankAccountIds(BankAccountIds);

            return AllBankAccounts.ToList();

        }

        private IQueryable<BankAccountPM> GetBankAccounts()
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
                                                             GLAccountId =  page.GLAccountId,
                                                             Tenant = page.Tenant,
                                                             Inactive = page.Inactive,
                                                             TransferGLAcccountId = page.TransferGLAcccountId,
                                                         });


            return AllBankAccounts;

        }

        private List<ExternalReconciliationPeriod> GetAllBankTransactionForTypeFilter(List<BankAccountPM> AllBankAccounts)
        {
            IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines = null;
            IQueryable<LedgerTransaction> ledgerTransactions = null;
            IQueryable<LedgerTransaction> TransferledgerTransactions = null;

            if (this.ExternalReconciliationNumber == null)
            {
                switch (Type)
                {
                    case "bank":
                        {
                            reconcileExternalPageLines = GetAllTransactionsForBankByFilter(reconcileExternalPageLines);

                            break;
                        }
                    case "glAccount":
                        {
                            ledgerTransactions = GetAllTransactionsForGLAccountByFilter(ledgerTransactions, AllBankAccounts);
                            if (this.IncludesTransferGlaccount == true)
                            {
                                TransferledgerTransactions = GetAllTransactionsForGLAccountByFilter(TransferledgerTransactions, AllBankAccounts, true);
                            }
                            break;
                        }
                    default:
                        {
                            reconcileExternalPageLines = GetAllTransactionsForBankByFilter(reconcileExternalPageLines);
                            ledgerTransactions = GetAllTransactionsForGLAccountByFilter(ledgerTransactions, AllBankAccounts);
                            if (this.IncludesTransferGlaccount == true)
                            {
                                TransferledgerTransactions = GetAllTransactionsForGLAccountByFilter(TransferledgerTransactions, AllBankAccounts, true);
                            }
                            break;
                        }

                }
            }
            else
            {
                reconcileExternalPageLines = GetAllTransactionsForBankByFilter(reconcileExternalPageLines);
                ledgerTransactions = GetAllTransactionsForGLAccountByFilter(ledgerTransactions, AllBankAccounts);
                TransferledgerTransactions = GetAllTransactionsForGLAccountByFilter(TransferledgerTransactions, AllBankAccounts, true);

            }


            return  MappingTransactionToExternalReconciliationPeriod(reconcileExternalPageLines, ledgerTransactions , TransferledgerTransactions);
        }


         public List<ExternalReconciliationPeriod> MappingTransactionToExternalReconciliationPeriod(IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines = null, IQueryable<LedgerTransaction> ledgerTransactions = null, IQueryable<LedgerTransaction> TransferledgerTransactions = null)
         {
          
            List<ExternalReconciliationPeriod> periodsResult = null;
            if (reconcileExternalPageLines!=null)
            {
                periodsResult = MappinReconcileExternalPageLineToPeriods(reconcileExternalPageLines);

            }
            if (ledgerTransactions != null)
            {
                periodsResult = periodsResult.Union(MappinLedgerTransactionToPeriods(ledgerTransactions)).ToList();

            }
            if (TransferledgerTransactions != null)
            {
                periodsResult = periodsResult.Union(MappinLedgerTransactionToPeriods(TransferledgerTransactions, true)).ToList();

            }

            if (periodsResult!=null)
            {
                foreach (ExternalReconciliationPeriod period in periodsResult)
                {
                    if (!string.IsNullOrEmpty(period.EntitySource))
                    {
                        period.EntitySource = AccountingEntities.getEntityIcon(period.EntitySource);
                    }

                    if (period.ExternalPageLineId!=null && periodsResult.Where(s => s.ExternalPageLineId == period.ExternalPageLineId).Count() > 1)
                    {
                        period.IsDuplicated = true;
                    }
                }

            }

            periodsResult =  SortExternalReconciliationPeriodBy(periodsResult);
            return periodsResult;
        }


        private List<ExternalReconciliationPeriod> MappinReconcileExternalPageLineToPeriods( IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines)
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
                                                              EntityType = a.ReconcileExternalPage.PageNo.ToString(),
                                                              IsRecomncile = a.IsReconciled,
                                                              LocalBoolean = a.IsReconciled == true ? "כן" : "לא",
                                                              EnglishBoolean = a.IsReconciled == true ? "True" : "False",
                                                              Note = a.Notes,
                                                              ReconcileNumber = Ex.ReconciliationId == null ? null : Ex.ExternalReconciliation.ReconciliationNumber,
                                                              Ref1 = a.Reference,
                                                              Ref2 = null,
                                                              ExternalPageLineId = Ex.ExternalPageLineId,

                                                          }).ToList();


            return periods;
        }

        private List<ExternalReconciliationPeriod> MappinLedgerTransactionToPeriods(IQueryable<LedgerTransaction> ledgerTransactions,bool IsTransfer=false)
        {
            IQueryable<ExternalReconciliationLine> ExternalReconciliationLines = (from a in accountingContext.ExternalReconciliationLines where a.ExternalReconciliation.IsCancelled == false select a);
            List<ExternalReconciliationPeriod> periods = (from a in ledgerTransactions
                                                          join BK in accountingContext.BankAccounts on a.AccountId equals IsTransfer ==true ? BK.TransferGLAcccountId : BK.GLAccountId
                                                          join Ex in ExternalReconciliationLines on a.Id equals Ex.LedgerTransactionId into LedgerTransactionJoinExternalReconciliation
                                                          from Ex in LedgerTransactionJoinExternalReconciliation.DefaultIfEmpty()
                                                          where BK.Inactive == false //&& (Ex.ExternalReconciliation==null || Ex.ExternalReconciliation.IsCancelled == false)
                                                          select new ExternalReconciliationPeriod()
                                                          {
                                                              EnglishType = "GLAccount",  //GLAccount.O.GLAccount
                                                              LocalType = "מזהה פנימי לכרטיס",
                                                              Number = a.Account.DisplayNumber,
                                                              BankAccountId = BK.Id,
                                                              Amount = a.ForeignAmountDebit == 0 ? a.ForeignAmountCredit * -1 : a.ForeignAmountDebit,
                                                              ReferenceDate = a.DocumentDate,
                                                              EntitySource = a.JournalLine.Journal.AccountingEntityCode,
                                                              EntityType = a.JournalLine.Journal.AccountingEntityReference,
                                                              IsRecomncile = a.IsExternalReconcile,
                                                              LocalBoolean = a.IsExternalReconcile == true ? "כן" : "לא",
                                                              EnglishBoolean = a.IsExternalReconcile == true ? "True" : "False",
                                                              Note = a.Notes,
                                                              ReconcileNumber = Ex.ReconciliationId == null ? null : Ex.ExternalReconciliation.ReconciliationNumber,
                                                              Ref1 = a.Reference1,
                                                              Ref2 = a.Reference2,
                                                              ExternalPageLineId = null,

                                                          }).ToList();


            return periods;
        }

        private IQueryable<LedgerTransaction> GetAllTransactionsForGLAccountByFilter(IQueryable<LedgerTransaction> ledgerTransactions , List<BankAccountPM> AllBankAccounts,bool IsTransfer=false)
        {

            if (ExternalReconciliationNumber!=null)
            {
                ledgerTransactions = (from a in accountingContext.ExternalReconciliationLines where a.ExternalReconciliation.ReconciliationNumber == ExternalReconciliationNumber && a.Tenant == tenant && a.LedgerTransactionId!=null select a.LedgerTransaction);
            }
            else
            {
                ledgerTransactions = (from a in accountingContext.LedgerTransactions where a.Tenant == tenant && DbFunctions.TruncateTime(a.DocumentDate) >= DbFunctions.TruncateTime(RefDateFrom)  && DbFunctions.TruncateTime(a.DocumentDate) <= DbFunctions.TruncateTime(RefDateTo) select a);
            }
            

            ledgerTransactions = ApplyFiltersForLedgerTransactions(ledgerTransactions, AllBankAccounts, IsTransfer);

            return ledgerTransactions;

        }

        private IQueryable<LedgerTransaction> ApplyFiltersForLedgerTransactions(IQueryable<LedgerTransaction> ledgerTransactions, List<BankAccountPM> AllBankAccounts,bool IsTransfer)
        {
            if (ExternalReconciliationNumber == null)
            {
               
                IQueryable<LedgerTransaction> TransferledgerTransactions = null;
                IQueryable<LedgerTransaction> MainledgerTransactions = null;

                if (IsTransfer == true)
                {
                    List<string> AllTransferGlAccountId = AllBankAccounts.Select(a => a.TransferGLAcccountId).ToList();
                    ledgerTransactions = ledgerTransactions.Where(s =>  AllTransferGlAccountId.Contains(s.AccountId));
                }
                else
                {
                    List<string> AllGlAccountId = AllBankAccounts.Select(a => a.GLAccountId).ToList();
                    ledgerTransactions = ledgerTransactions.Where(s => AllGlAccountId.Contains(s.AccountId));
                }

                if (IsExternalReconciled == "close")
                {
                    ledgerTransactions = ledgerTransactions.Where(s => s.IsExternalReconcile == true);
                }
                else if (IsExternalReconciled == "open")
                {
                    ledgerTransactions = ledgerTransactions.Where(s => s.IsExternalReconcile == false);
                }
            }
           

            return ledgerTransactions;
        }

        private IQueryable<ReconcileExternalPageLine> GetAllTransactionsForBankByFilter(IQueryable<ReconcileExternalPageLine> reconcileExternalPageLines)
        {
            if (ExternalReconciliationNumber == null)
            {
                

                 reconcileExternalPageLines = (from line in accountingContext.ReconcileExternalPageLines
                                                  where line.Tenant == tenant
                                                        && line.ReferenceDate !=null
                                                        && DbFunctions.TruncateTime(line.ReferenceDate) >= DbFunctions.TruncateTime(RefDateFrom)
                                                        && DbFunctions.TruncateTime(line.ReferenceDate) <= DbFunctions.TruncateTime(RefDateTo)
                                                         
                                                  select line);

                if (!string.IsNullOrEmpty(ObjectTableId))
                {
                    reconcileExternalPageLines = reconcileExternalPageLines.Where(line => line.ReconcileExternalPage.ObjectTableId == ObjectTableId);
                }
                if (!string.IsNullOrEmpty(BankAccountId))
                {
                    reconcileExternalPageLines = reconcileExternalPageLines.Where(line=> line.ReconcileExternalPage.EntityId == BankAccountId);
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

                reconcileExternalPageLines = (from a in accountingContext.ExternalReconciliationLines where a.ExternalReconciliation.ReconciliationNumber == ExternalReconciliationNumber && a.Tenant == tenant && a.ExternalPageLineId!=null select a.ReconcileExternalPageLine);
 
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

        private void FilterByTypeAndBank(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_BankId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "BankAccountId").FirstOrDefault();
            if (filterItem_BankId != null)
            {
                if (filterItem_BankId.FieldValue != null)
                {
                    BankAccountId = filterItem_BankId.FieldValue.ToString();
                }
            }

            QueryFilterItem filterItem_ObjectTableId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ObjectTableId").FirstOrDefault();
            if (filterItem_ObjectTableId != null)
            {
                if (filterItem_ObjectTableId.FieldValue != null)
                {
                    ObjectTableId = filterItem_ObjectTableId.FieldValue.ToString();
                }
            }
            QueryFilterItem filterItem_Type = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Type").FirstOrDefault();
            if (filterItem_Type != null)
            {
                if (filterItem_Type.FieldValue != null)
                {
                    Type = filterItem_Type.FieldValue.ToString();
                }
            }

            QueryFilterItem filterItem_SortBy = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SortBy").FirstOrDefault();
            if (filterItem_SortBy != null)
            {
                if (filterItem_SortBy.FieldValue != null)
                {
                    SortBy = filterItem_SortBy.FieldValue.ToString();
                }
            }
        }

        private void FilterByReconciled(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_IsExternalReconciled = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsExternalReconciled").FirstOrDefault();
            if (filterItem_IsExternalReconciled != null)
            {
                if (filterItem_IsExternalReconciled.FieldValue != null)
                {
                    IsExternalReconciled = filterItem_IsExternalReconciled.FieldValue.ToString();
                }
            }
            QueryFilterItem filterItem_ExternalReconciliationNumber = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExternalReconciliationNumber").FirstOrDefault();
            if (filterItem_ExternalReconciliationNumber != null)
            {
                if (filterItem_ExternalReconciliationNumber.FieldValue != null)
                {
                    ExternalReconciliationNumber = int.Parse(filterItem_ExternalReconciliationNumber.FieldValue.ToString());
                }
            }
            QueryFilterItem filterItem_IncludesTransferGlaccount = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludesTransferGlaccount").FirstOrDefault();
            if (filterItem_IncludesTransferGlaccount != null)
            {
                if (filterItem_IncludesTransferGlaccount.FieldValue != null)
                {
                    IncludesTransferGlaccount = (bool)filterItem_IncludesTransferGlaccount.FieldValue;
                }
            }
        }

        private void FilterByRefDates(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_RefDateFrom = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "REFFromDate").FirstOrDefault();
            if (filterItem_RefDateFrom != null)
            {
                if (filterItem_RefDateFrom.FieldValue != null)
                {
                    RefDateFrom = (DateTime)filterItem_RefDateFrom.FieldValue;
                }
            }
            QueryFilterItem filterItem_RefDateTo = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "REFToDate").FirstOrDefault();
            if (filterItem_RefDateTo != null)
            {
                if (filterItem_RefDateTo.FieldValue != null)
                {
                    RefDateTo = (DateTime)filterItem_RefDateTo.FieldValue;
                }
            }
        }


      

    }

}