using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
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
    public class UserDefinedReportManager
    {
        const string GLAccountType = "1";
        const string ChartofAccountType = "2";

        private int tenant;
        public string UserDefinedReportId;
        public bool IncludeAnOpeningBalance;
        public DateTime? FirstPeriodDateFrom;
        public DateTime? FirstPeriodDateTo;
        public DateTime? SecoundPeriodDateFrom;
        public DateTime? SecoundPeriodDateTo;
        public bool DetailedCurrencies;
        public bool ExpandChartOfAccountToGLAccounts;
        private IAccountingContext accountingContext;
        private UserDefinedReportDataProvider iDataProvider;
        public List<string> AllChartsofAccountTypeCodeinReport;
        public List<CalculatedChartsOfAccountsLinePeriod> ParentCalculatedChartsofAccountsLines;
        public List<CalculatedChartsOfAccountsLinePeriod> SubParentCalculatedChartsofAccountsLines;
        List<string> ReportGLAccountIdsList = new List<string>();
        List<string> ReportChartsofAccountsIdsList = new List<string>();
        public UserDefinedReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            accountingContext = AccountingContext.GetContext(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            GetAllReportFilters(iQueryOperations);
        }

        private void GetAllReportFilters(QueryOperations iQueryOperations)
        {
            this.GetFirstPeriodDatesFilters(iQueryOperations);
            this.GetSecoundPeriodDatesFilters(iQueryOperations);
            this.GetUserDefinedReportIdFilter(iQueryOperations);
            this.GetIncludeAnOpeningBalanceFilter(iQueryOperations);
            this.GetDetailedCurrenciesFilter(iQueryOperations);
            this.GetExpandChartOfAccountToGLAccountsFilter(iQueryOperations);
        }
        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserDefinedReportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void LoadDataProvider()
        {
            this.iDataProvider = new UserDefinedReportDataProvider();
            this.BuildReportHeader();
            this.BuildSourceData();
        }
        private void BuildReportHeader()
        {
        
            iDataProvider.IncludeAnOpeningBalance = this.IncludeAnOpeningBalance;
            iDataProvider.FirstPeriodDateFrom = this.FirstPeriodDateFrom;
            iDataProvider.FirstPeriodDateTo = this.FirstPeriodDateTo;
            iDataProvider.SecoundPeriodDateFrom = this.SecoundPeriodDateFrom;
            iDataProvider.SecoundPeriodDateTo = this.SecoundPeriodDateTo;
            iDataProvider.ExpandChartOfAccountToGLAccounts = this.ExpandChartOfAccountToGLAccounts;

        }
        private void BuildSourceData()
        {
            UserDefinedReportPM UserDefinedReport = GetSingleUserDefinedReportByFilters();
            GetChildsAccountsConnectWithChartsofAccountsWhenApplyExpandFilter();
            BuildCalculatedChartsofAccountPeriod(UserDefinedReport);
        }

        private void GetChildsAccountsConnectWithChartsofAccountsWhenApplyExpandFilter()
        {

        }
        private UserDefinedReportPM GetSingleUserDefinedReportByFilters()
        {
            UserDefinedReportQueryService userDefinedReportQueryService = new UserDefinedReportQueryService(accountingContext);
            UserDefinedReportPM UserDefinedReportPM = userDefinedReportQueryService.GetSingle(this.UserDefinedReportId, true, false);
            return UserDefinedReportPM;
        }

        private void BuildCalculatedChartsofAccountPeriod(UserDefinedReportPM userDefinedReport)
        { 
            if (userDefinedReport != null && !userDefinedReport.IsCancelled)
            {
                MappingUserDefinedReportDataProvider(userDefinedReport);
                FillAllCalculatedChartsofAccountsLineListDataProvider();
                SetCalculatedChartsofAccountsLinesAmountByLedgerTransactionConnectedWithAccounts(userDefinedReport);
            }

        }
        private void MappingUserDefinedReportDataProvider(UserDefinedReportPM userDefinedReport)
        {

            iDataProvider.UserDefinedReportPeriod = new UserDefinedReportPeriod()
            {
                EnglishName = userDefinedReport.EnglishName != null ? userDefinedReport.EnglishName : userDefinedReport.LocalName,
                LocalName = userDefinedReport.LocalName != null ? userDefinedReport.LocalName : userDefinedReport.EnglishName,
                CreateByUserLocalName = userDefinedReport.CreatedByLocalName != null ? userDefinedReport.CreatedByLocalName : userDefinedReport.CreatedByEnglishName,
                CreateByUserEnglishName = userDefinedReport.CreatedByEnglishName != null ? userDefinedReport.CreatedByEnglishName : userDefinedReport.CreatedByLocalName,
                CalculatedChartsOfAccountPeriods = MappingCalculatedChartsOfAccountPeriods(userDefinedReport),
            };

        }

        private List<CalculatedChartsOfAccountPeriod>  MappingCalculatedChartsOfAccountPeriods(UserDefinedReportPM userDefinedReport)
        {
            var CalculatedChartsOfAccountPeriods = (from a in userDefinedReport.CalculatedChartsOfAccounts.Where(s => !s.IsCancelled)
                                                    select new CalculatedChartsOfAccountPeriod()
                                                    {
                                                        LocalName = a.LocalName != null ? a.LocalName : a.EnglishName,
                                                        EnglishName = a.EnglishName != null ? a.EnglishName : a.LocalName,
                                                        ChartOfAccountTypeCode = a.ChartOfAccountTypeCode,
                                                        ChartOfAccountTypeEnglishName = a.ChartOfAccountTypeEnglishName != null ? a.ChartOfAccountTypeEnglishName : a.ChartOfAccountTypeLocalName,
                                                        ChartOfAccountTypeLocalName = a.ChartOfAccountTypeLocalName != null ? a.ChartOfAccountTypeLocalName : a.ChartOfAccountTypeEnglishName,
                                                        Id = a.Id,
                                                        CalculatedChartsOfAccountsLinePeriods = MappingCalculatedChartsOfAccountsLinePeriods(a),
                                                    }).ToList();

            return CalculatedChartsOfAccountPeriods;

        }
        private List<CalculatedChartsOfAccountsLinePeriod> MappingCalculatedChartsOfAccountsLinePeriods(CalculatedChartsOfAccountPM  CalculatedChartsOfAccount )
        {
            var CalculatedChartsOfAccountsLinePeriods = (from b in CalculatedChartsOfAccount.CalculatedChartsOfAccountLines.Where(s => !s.IsCancelled)
                                                         select new CalculatedChartsOfAccountsLinePeriod()
                                                         {
                                                             IsSubParent = false,
                                                             IsParent = false,
                                                             EnglishName = b.LineTypeCode == GLAccountType ?
                                                                           b.GLAccountEnglishName != null ? b.GLAccountEnglishName : b.GLAccountLocalName :
                                                                           b.ChartOfAccountEnglishName != null ? b.ChartOfAccountEnglishName : b.ChartOfAccountLocalName,
                                                             LocalName = b.LineTypeCode == GLAccountType ?
                                                                           b.GLAccountLocalName != null ? b.GLAccountLocalName : b.GLAccountEnglishName :
                                                                           b.ChartOfAccountLocalName != null ? b.ChartOfAccountLocalName : b.ChartOfAccountEnglishName,
                                                             Code = b.LineTypeCode == GLAccountType ? b.GLAccountDisplayNumber : b.ChartsofAccountCode,
                                                             EnglishType = b.LineTypeCode == GLAccountType ? "GLAccount" : "Chart of Account",
                                                             LocalType = b.LineTypeCode == GLAccountType ? "כרטיס" : "קבוצת מאזן",
                                                             ChartsofAccountTypeCode = b.CalculatedChartsOfAccountsId,
                                                             ParentChartsofAccountTypeCode = CalculatedChartsOfAccount.ChartOfAccountTypeCode + "_" + b.Id,
                                                             ChartsofAccountId = b.ChartOfAccountId,
                                                             LineTypeCode = b.LineTypeCode,
                                                             GLAccountId = b.GLAccountId,
                                                         }).ToList();

            return CalculatedChartsOfAccountsLinePeriods;

        }


        private CalculatedChartsOfAccountsLinePeriod MappingCalculatedChartsOfAccountsLinePeriodFromGLAccount(GLAccount gLAccount, CalculatedChartsOfAccountsLinePeriod line)
        {

            var MappingLine = new CalculatedChartsOfAccountsLinePeriod()
            {
                EnglishName = gLAccount.EnglishName != null ? gLAccount.EnglishName : gLAccount.LocalName,
                LocalName = gLAccount.LocalName != null ? gLAccount.LocalName : gLAccount.EnglishName,
                Code = gLAccount.DisplayNumber,
                GLAccountId = gLAccount.Id,
                LocalType = "כרטיס",
                EnglishType = "GLAccount",
                ChartsofAccountTypeCode = line.ChartsofAccountTypeCode,
                ParentChartsofAccountTypeCode = line.ParentChartsofAccountTypeCode,
                LineTypeCode = line.LineTypeCode,
            };

            return MappingLine;
        }
        private void FillAllCalculatedChartsofAccountsLineListDataProvider()
        {
            AllChartsofAccountTypeCodeinReport = new List<string>();
            ParentCalculatedChartsofAccountsLines = new List<CalculatedChartsOfAccountsLinePeriod>();
            SubParentCalculatedChartsofAccountsLines = new List<CalculatedChartsOfAccountsLinePeriod>();
            iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods = new List<CalculatedChartsOfAccountsLinePeriod>();
            foreach (CalculatedChartsOfAccountPeriod Period in iDataProvider.UserDefinedReportPeriod.CalculatedChartsOfAccountPeriods)
            {
                FillParentCalculatedChartsofAccountsLinesList(Period);
                FillSubParentCalculatedChartsofAccountsLinesList(Period);
                foreach (CalculatedChartsOfAccountsLinePeriod Line in Period.CalculatedChartsOfAccountsLinePeriods)
                {
                    iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods.Add(Line);
                }

            }

        }
        private void FillParentCalculatedChartsofAccountsLinesList(CalculatedChartsOfAccountPeriod period)
        {
            if (!AllChartsofAccountTypeCodeinReport.Contains(period.ChartOfAccountTypeCode))
            {
                AllChartsofAccountTypeCodeinReport.Add(period.ChartOfAccountTypeCode);
                ParentCalculatedChartsofAccountsLines.Add(new CalculatedChartsOfAccountsLinePeriod()
                {
                    IsParent = true,
                    ParentChartsofAccountTypeCode = period.ChartOfAccountTypeCode,
                    ParentEnglishType = period.ChartOfAccountTypeEnglishName != null ? period.ChartOfAccountTypeEnglishName : period.ChartOfAccountTypeLocalName,
                    ParentLocalType = period.ChartOfAccountTypeLocalName != null ? period.ChartOfAccountTypeLocalName : period.ChartOfAccountTypeEnglishName,
                });

            }


        }

        private void FillSubParentCalculatedChartsofAccountsLinesList(CalculatedChartsOfAccountPeriod period)
        {
                AllChartsofAccountTypeCodeinReport.Add(period.ChartOfAccountTypeCode);
                ParentCalculatedChartsofAccountsLines.Add(new CalculatedChartsOfAccountsLinePeriod()
                {
                    IsSubParent = true,
                    ParentChartsofAccountTypeCode = period.Id,
                    ChartsofAccountTypeCode = period.ChartOfAccountTypeCode,
                    SubParentEnglishType = period.EnglishName != null ? period.EnglishName : period.LocalName,
                    SubParentLocalType = period.LocalName != null ? period.LocalName : period.EnglishName,
                });;
        }

        private void SetCalculatedChartsofAccountsLinesAmountByLedgerTransactionConnectedWithAccounts(UserDefinedReportPM userDefinedReport)
        {

            FillGLAccountAndCharstofAccountsIdsListFromUserDefinedReport(userDefinedReport);
            SetCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForChartsofAccounts();
            SetCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForGLAcountIds();
            AddParentLinesToAllCalculatedChartsOfAccountsLinePeriods();

        }

        private void AddParentLinesToAllCalculatedChartsOfAccountsLinePeriods()
        {
            foreach (CalculatedChartsOfAccountsLinePeriod parentLine in ParentCalculatedChartsofAccountsLines)
            {
                this.iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods.Add(parentLine);
            }
        }

        private void SetCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForGLAcountIds()
        {
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(accountingContext);
            IQueryable<LedgerTransaction> IQueryableledgerTransactions = ledgerTransactionQueryService.GetIQueryableLedgerTransactionsByGLAccountIdsList(ReportGLAccountIdsList, this.tenant);
            SetCalculatedChartsofAcclountsLinesAmountsFields(IQueryableledgerTransactions, GLAccountType);
        }

        private void SetCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForChartsofAccounts()
        {
            List<string> GLAccountIdsList = new List<string>();
            GetAllGLAccountIdsConnectedToChartsofAccount(GLAccountIdsList);
            if (!this.ExpandChartOfAccountToGLAccounts)
            {
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(accountingContext);
                IQueryable<LedgerTransaction> IQueryableledgerTransactions = ledgerTransactionQueryService.GetIQueryableLedgerTransactionsByGLAccountIdsList(GLAccountIdsList, this.tenant);
                SetCalculatedChartsofAcclountsLinesAmountsFields(IQueryableledgerTransactions, ChartofAccountType);

            }
           
        }
        private void SetCalculatedChartsofAcclountsLinesAmountsFields(IQueryable<LedgerTransaction> IQueryableledgerTransactions, string LineChartsofAccountType)
        {
            IQueryable<LedgerTransaction> FirstPeriodLedgerTransaction = FilterLedgerTransactionByPeriod(this.FirstPeriodDateFrom, this.FirstPeriodDateTo, IQueryableledgerTransactions);
            IQueryable<LedgerTransaction> SecoundPeriodLedgerTransaction = FilterLedgerTransactionByPeriod(this.SecoundPeriodDateFrom, this.SecoundPeriodDateTo, IQueryableledgerTransactions);
            foreach (CalculatedChartsOfAccountsLinePeriod Line in this.iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods.Where(s => s.LineTypeCode == LineChartsofAccountType))
            {       Line.FirstPeriodAmount = FirstPeriodLedgerTransaction.Where(s => Line.LineTypeCode == ChartofAccountType? s.Account.ChartOfAccountsId == Line.ChartsofAccountId:
                                                                                      s.AccountId == Line.GLAccountId).ToList().Sum(s => s.LocalAmountDebit - s.LocalAmountCredit);
                    Line.SecoundPeriodAmount = SecoundPeriodLedgerTransaction.Where(s => Line.LineTypeCode == ChartofAccountType ? s.Account.ChartOfAccountsId == Line.ChartsofAccountId :
                                                                                         s.AccountId == Line.GLAccountId).ToList().Sum(s => s.LocalAmountDebit - s.LocalAmountCredit);
                    Line.Difference = Line.FirstPeriodAmount==0 && Line.SecoundPeriodAmount == 0?0: 
                                      Line.FirstPeriodAmount == 0?100:
                                      SetLineDefferenceField(Line);
                    SetParentCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForGLAcountIds(Line);
                    SetSubParentCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForGLAcountIds(Line);
            }
        }

        private void SetParentCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForGLAcountIds(CalculatedChartsOfAccountsLinePeriod line)
        {
            FillParentsLinesFieldsFromSubLinesFields(line, ParentCalculatedChartsofAccountsLines);
        }

        private void SetSubParentCalculatedChartsofAccountsLinesAmountByLedgerTransactionsForGLAcountIds(CalculatedChartsOfAccountsLinePeriod line)
        {
            FillParentsLinesFieldsFromSubLinesFields(line, SubParentCalculatedChartsofAccountsLines);
        }

        private void FillParentsLinesFieldsFromSubLinesFields(CalculatedChartsOfAccountsLinePeriod line, List<CalculatedChartsOfAccountsLinePeriod> Parents)
        {
            foreach (CalculatedChartsOfAccountsLinePeriod parentLine in Parents)
            {
                if (parentLine.ParentChartsofAccountTypeCode == line.ChartsofAccountTypeCode)
                {
                    FillParentLineAmountFromSubLineAmount(parentLine, line);
                    break;
                }
            }
        }

        private void FillParentLineAmountFromSubLineAmount(CalculatedChartsOfAccountsLinePeriod parentLine, CalculatedChartsOfAccountsLinePeriod line)
        {
            parentLine.FirstPeriodAmount += line.FirstPeriodAmount;
            parentLine.SecoundPeriodAmount += line.SecoundPeriodAmount;
            parentLine.Difference = parentLine.FirstPeriodAmount == 0 && parentLine.SecoundPeriodAmount == 0 ? 0 :
                                    parentLine.FirstPeriodAmount == 0 ? 100 :
                                    SetLineDefferenceField(parentLine);
        }

        private decimal SetLineDefferenceField(CalculatedChartsOfAccountsLinePeriod line)
        {
            line.Difference = line.FirstPeriodAmount == 0 && line.SecoundPeriodAmount == 0 ? 0 :
                                 line.FirstPeriodAmount == 0 ? 100 :
                                 Math.Abs(((line.FirstPeriodAmount - line.SecoundPeriodAmount) / line.FirstPeriodAmount) * 100);

            return line.Difference;

        }
        private IQueryable<LedgerTransaction> FilterLedgerTransactionByPeriod(DateTime?  periodDateFrom , DateTime?  periodDateTo , IQueryable<LedgerTransaction> ledgerTransactions)
        {
            IQueryable<LedgerTransaction>  PeriodLedgerTransaction = ledgerTransactions.Where(s => DbFunctions.TruncateTime(s.AccountingDate) >= DbFunctions.TruncateTime(periodDateFrom) &&
                                                                                 DbFunctions.TruncateTime(s.AccountingDate) <= DbFunctions.TruncateTime(periodDateTo));

            return PeriodLedgerTransaction;

        }

        private void GetAllGLAccountIdsConnectedToChartsofAccount(List<string> gLAccountIdsList)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(accountingContext);
            foreach (string ChartsofAccountsId in ReportChartsofAccountsIdsList)
            {
                CalculatedChartsOfAccountsLinePeriod itemToRemove =null;
                if (this.ExpandChartOfAccountToGLAccounts)
                {
                    itemToRemove = iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods.Single(r => r.LineTypeCode == ChartofAccountType && r.ChartsofAccountId == ChartsofAccountsId);
                    iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods.Remove(itemToRemove);
                }
                List<GLAccount> GLAccountsConnectedWithChartofAccount = gLAccountQueryService.GetAllGLAccountIdsByChartsofAccountId(this.tenant, ChartsofAccountsId);
                foreach (GLAccount GLAccount in GLAccountsConnectedWithChartofAccount)
                {
                    if(this.ExpandChartOfAccountToGLAccounts)
                      iDataProvider.UserDefinedReportPeriod.AllCalculatedChartsOfAccountsLinePeriods.Add(this.MappingCalculatedChartsOfAccountsLinePeriodFromGLAccount(GLAccount, itemToRemove));
                    else
                      gLAccountIdsList.Add(GLAccount.Id);
                }
            }
            
        }


        private void FillGLAccountAndCharstofAccountsIdsListFromUserDefinedReport(UserDefinedReportPM userDefinedReport)
        {
            foreach (CalculatedChartsOfAccountPM Period in userDefinedReport.CalculatedChartsOfAccounts.Where(s=>!s.IsCancelled))
            {
                foreach (CalculatedChartsOfAccountsLinePM Line in Period.CalculatedChartsOfAccountLines.Where(s => !s.IsCancelled))
                {
                    if (Line.LineTypeCode == GLAccountType)
                    {
                        ReportGLAccountIdsList.Add(Line.GLAccountId);
                    }
                    else
                    {
                        ReportChartsofAccountsIdsList.Add(Line.ChartOfAccountId);
                    }

                }

            }
        }
        private void GetUserDefinedReportIdFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_UserDefinedReportId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "UserDefinedReportId").FirstOrDefault();
            if (filterItem_UserDefinedReportId != null)
            {
                if (filterItem_UserDefinedReportId.FieldValue != null)
                {
                    UserDefinedReportId = filterItem_UserDefinedReportId.FieldValue.ToString();
                }
            }
        }
 

        private void GetExpandChartOfAccountToGLAccountsFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_ExpandChartOfAccountToGLAccounts = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ExpandChartOfAccountToGLAccounts").FirstOrDefault();
            if (filterItem_ExpandChartOfAccountToGLAccounts != null)
            {
                if (filterItem_ExpandChartOfAccountToGLAccounts.FieldValue != null)
                {
                    ExpandChartOfAccountToGLAccounts = (bool)filterItem_ExpandChartOfAccountToGLAccounts.FieldValue;
                }
            }
        }

        private void GetDetailedCurrenciesFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_DetailedCurrencies = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "DetailedCurrencies").FirstOrDefault();
            if (filterItem_DetailedCurrencies != null)
            {
                if (filterItem_DetailedCurrencies.FieldValue != null)
                {
                    DetailedCurrencies = (bool)filterItem_DetailedCurrencies.FieldValue;
                }
            }
        }

        private void GetIncludeAnOpeningBalanceFilter(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_IncludeAnOpeningBalance = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeAnOpeningBalance").FirstOrDefault();
            if (filterItem_IncludeAnOpeningBalance != null)
            {
                if (filterItem_IncludeAnOpeningBalance.FieldValue != null)
                {
                    IncludeAnOpeningBalance = (bool)filterItem_IncludeAnOpeningBalance.FieldValue;
                }
            }
        }

        private void GetFirstPeriodDatesFilters(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_FirstPeriodDateFrom = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FirstPeriodDateFrom").FirstOrDefault();
            if (filterItem_FirstPeriodDateFrom != null)
            {
                if (filterItem_FirstPeriodDateFrom.FieldValue != null)
                {
                    FirstPeriodDateFrom = (DateTime)filterItem_FirstPeriodDateFrom.FieldValue;
                }
            }
            QueryFilterItem filterItem_FirstPeriodDateTo = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FirstPeriodDateTo").FirstOrDefault();
            if (filterItem_FirstPeriodDateTo != null)
            {
                if (filterItem_FirstPeriodDateTo.FieldValue != null)
                {
                    FirstPeriodDateTo = (DateTime)filterItem_FirstPeriodDateTo.FieldValue;
                }
            }
        }

        private void GetSecoundPeriodDatesFilters(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_SecoundPeriodDateFrom = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SecoundPeriodDateFrom").FirstOrDefault();
            if (filterItem_SecoundPeriodDateFrom != null)
            {
                if (filterItem_SecoundPeriodDateFrom.FieldValue != null)
                {
                    SecoundPeriodDateFrom = (DateTime)filterItem_SecoundPeriodDateFrom.FieldValue;
                }
            }
            QueryFilterItem filterItem_SecoundPeriodDateTo = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SecoundPeriodDateTo").FirstOrDefault();
            if (filterItem_SecoundPeriodDateTo != null)
            {
                if (filterItem_SecoundPeriodDateTo.FieldValue != null)
                {
                    SecoundPeriodDateTo = (DateTime)filterItem_SecoundPeriodDateTo.FieldValue;
                }
            }
        }


    }

}