using Logitude.Accounting.BL.CoreBL.Reports;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class GLAccountsAgingCSVBuilder
    {
        private const int MONTHS_BACKWARDS = 2;
        private int tenant;

        public GLAccountsAgingCSVBuilder(int tenant)
        {
            this.tenant = tenant;

        }

        public string BuildAndGet()
        {
            try
            {
                List<PeriodMExtended> agingPeriods = GetAgingPeriods();

                List<CSVRow> csvRows = BuildCSVFileRowsFromPeriods(agingPeriods);

                StringBuilder csvString = JoinCSVRowsIntoString(csvRows);

                return csvString.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("[GLAccountsAgingCSVBuilder Error]" + ex.Message);
            }
        }

        private StringBuilder JoinCSVRowsIntoString(List<CSVRow> CSVRows)
        {
            StringBuilder csvString = new StringBuilder();
            foreach (var row in CSVRows)
            {
                appendRow(csvString, row);
            }

            return csvString;
        }

        private static List<CSVRow> BuildCSVFileRowsFromPeriods(List<PeriodMExtended> agingPeriods)
        {
            var CSVRows = agingPeriods.GroupBy(period => period.AccountId)
                .Select(accountPeriods =>
                new CSVRow()
                {
                    InternalNumber = accountPeriods.First().AccountInternalNumber,
                    LocalBalance = accountPeriods.First().BalanceInLocalCurrency,
                    TotFutureOpenChequesInLocalCur = accountPeriods.First().TotalFutureOpenCheques,
                    TotalOpenFilesAmount = accountPeriods.First().TotalOpenShipments,
                    GIL1 = accountPeriods.OrderByDescending(p => p.OrderDate).First().Total,
                    GIL2 = accountPeriods.OrderBy(p => p.OrderDate).Skip(1).First().Total,
                    GIL3 = accountPeriods.OrderBy(p => p.OrderDate).First().Total,
                    OBLG = accountPeriods.First().CreditStatusAmount,
                    HRIG = accountPeriods.First().BalanceInLocalCurrency
                            + accountPeriods.First().TotalFutureOpenCheques
                            - accountPeriods.First().TotalOpenShipments,
                    InterestCreditLimit = accountPeriods.First().InterestCreditLimit,
                    InterestPercent = accountPeriods.First().GLAccountStandardInterestRate,
                    CurrencyCode = accountPeriods.First().CurrencyId != null ? accountPeriods.First().CurrencyCode : null
                }).ToList();
            return CSVRows;
        }

        private List<PeriodMExtended> GetAgingPeriods()
        {
            AgingReportService agingReportService = new AgingReportService(BuildAgingParameters());
            agingReportService.RunReport();
            List<PeriodMExtended> agingPeriods = agingReportService.MyPeriodExtendedList;
            return agingPeriods;
        }

        private void appendRow(StringBuilder csvString, CSVRow row)
        {
            csvString
                .Append(row.InternalNumber).Append(",")
                .Append(row.LocalBalance).Append(",")
                .Append(row.TotFutureOpenChequesInLocalCur).Append(",")
                .Append(row.TotalOpenFilesAmount).Append(",")
                .Append(row.GIL1).Append(",")
                .Append(row.GIL2).Append(",")
                .Append(row.GIL3).Append(",")
                .Append(row.OBLG).Append(",")
                .Append(row.HRIG).Append(",")
                .Append(row.InterestCreditLimit).Append(",")
                .Append(row.InterestPercent).Append(",")
                .Append(row.CurrencyCode)
                .Append("\n");
        }
        private AgingReportParam BuildAgingParameters()
        {
            AgingReportParam reportParameters = InitiateAgingReportParameters();
            reportParameters.Tenant = tenant;
            reportParameters.AgingForDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            reportParameters.NumberOfmonthsbackwards = MONTHS_BACKWARDS;
            //reportParameters.VendorCustomerId = "1-3491";
            reportParameters.Aging4AccountTypeCode = AgingReportParam.Aging4AccountTypeCodeEnum.Customer2;
            reportParameters.AggregateByGLAccountCurrencies = false;
            reportParameters.GroupByDate = AgingReportParam.DateEnum.AccountingDate;
            reportParameters.AgingMethod = AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString();
            //reportParameters.Aging4AccountTypeCode = AgingReportParam.Aging4AccountTypeCodeEnum.Customer2;
            return reportParameters;
        }

        private AgingReportParam InitiateAgingReportParameters()
        {
            return new AgingReportParam()
            {
                AgingMethod_Options = Enum.GetNames(typeof(AgingReportParam.MethodEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
                GroupByDate_Options = Enum.GetNames(typeof(AgingReportParam.DateEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
                Aging4AccountTypeCode_Options = Enum.GetNames(typeof(AgingReportParam.Aging4AccountTypeCodeEnum)).ToList().Aggregate((b4, aftr) => string.Concat(b4, ";", aftr)),
            };
        }
    }

    public class CSVRow
    {
        public string InternalNumber { get; set; }
        public decimal? LocalBalance { get; set; }
        public decimal? TotFutureOpenChequesInLocalCur { get; set; }
        public decimal? TotalOpenFilesAmount { get; set; }
        public decimal? GIL1 { get; set; }
        public decimal? GIL2 { get; set; }
        public decimal? GIL3 { get; set; }
        public decimal? OBLG { get; set; }
        public decimal? HRIG { get; set; }
        public decimal? InterestCreditLimit  { get; set; }
        public decimal InterestPercent { get; set; }
        public string CurrencyCode { get; set; }

    }
}
