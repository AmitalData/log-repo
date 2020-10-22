using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.Utilities;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportCalculationFromCSVPreparations : IInterestReportCalculationPreparations
    {
        private string path;
        private CSVStringLinesGetter cSVStringLinesGetter;
        public InterestReportCalculationFromCSVPreparations(string path)
        {
            this.path = path;
            cSVStringLinesGetter = new CSVStringLinesGetter();
        }

        
        public List<InterestBasesPeriodPM> GetAllInterestBasesPeriodPMs(int tenant)
        {
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = new List<InterestBasesPeriodPM>();
            string[] csvPeriodsLines = cSVStringLinesGetter.GetstringLinesFromCSV(path + "interestperiods.csv");
            for (int i = 0; i < csvPeriodsLines.Length; i++)
            {
                //InterestBaseTypeId	LineNumber	Tenant	CreateDate	CreatedByUserId	UpdateDate
                //UpdatedByUserId	InterestBaseStartDate	InterestRate
                string[] lineFields = csvPeriodsLines[i].Split(',');
                if (csvPeriodsLines[i].Length > 0)
                {
                    int lineNumber;
                    int.TryParse(lineFields[1], out lineNumber);
                    InterestBasesPeriodPM interestBasesPeriodPM = new InterestBasesPeriodPM();

                    interestBasesPeriodPM.InterestBaseTypeId = lineFields[0];
                    interestBasesPeriodPM.LineNumber = lineNumber;
                    interestBasesPeriodPM.Tenant = Convert.ToInt32(lineFields[2]);
                    interestBasesPeriodPM.CreateDate = GetDateTimeFromString(lineFields[3]);
                    interestBasesPeriodPM.CreatedByUserId = lineFields[4];
                    interestBasesPeriodPM.UpdateDate = GetDateTimeFromString(lineFields[5]);
                    interestBasesPeriodPM.UpdatedByUserId = lineFields[6];
                    interestBasesPeriodPM.InterestBaseStartDate = GetDateTimeFromString(lineFields[7]);
                    interestBasesPeriodPM.InterestRate = Convert.ToDecimal(lineFields[8]);

                    interestBasesPeriodPMs.Add(interestBasesPeriodPM);
                }
            }
            return interestBasesPeriodPMs;
        }

     
        public List<GLAccountInterestPeriodPM> GetGlaccountInterestPeriods(InterestReportPM interestReportPM)
        {
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = new List<GLAccountInterestPeriodPM>();
            string[] csvPeriodsLines = cSVStringLinesGetter.GetstringLinesFromCSV(path + "glaccountperiods.csv");
            for (int i = 0; i < csvPeriodsLines.Length; i++)
            {
                //LineNumber	Tenant	PeriodStartDate	GLAccountId	StandardInterestRateBaseId
                //StandardAddInterestPercent	ExceptionalInterestRateBaseId	ExceptionalAddInterestPercent	
                //CreditInterestRateBaseId	CreditAddInterestPercent	CreateDateTime	UpdateDateTime
                //CreatedByUserId	UpdatedByUserId
                if (csvPeriodsLines[i].Length > 0)
                {
                    string[] lineFields = csvPeriodsLines[i].Split(',');
                    int lineNumber;
                    int.TryParse(lineFields[0], out lineNumber);
                    GLAccountInterestPeriodPM gLAccountInterestPeriodPM = new GLAccountInterestPeriodPM();

                    gLAccountInterestPeriodPM.LineNumber = lineNumber;
                    gLAccountInterestPeriodPM.Tenant = Convert.ToInt32(lineFields[1]);
                    gLAccountInterestPeriodPM.PeriodStartDate = GetDateTimeFromString(lineFields[2]);
                    gLAccountInterestPeriodPM.GLAccountId = lineFields[3];
                    gLAccountInterestPeriodPM.StandardInterestRateBaseId = lineFields[4];
                    gLAccountInterestPeriodPM.StandardAddInterestPercent = Convert.ToDecimal(lineFields[5]);
                    gLAccountInterestPeriodPM.ExceptionalInterestRateBaseId = lineFields[6];
                    gLAccountInterestPeriodPM.ExceptionalAddInterestPercent = Convert.ToDecimal(lineFields[7]);
                    gLAccountInterestPeriodPM.CreditInterestRateBaseId = lineFields[8];
                    gLAccountInterestPeriodPM.CreditAddInterestPercent = Convert.ToDecimal(lineFields[9]);
                    gLAccountInterestPeriodPM.CreateDateTime = GetDateTimeFromString(lineFields[10]);
                    gLAccountInterestPeriodPM.UpdateDateTime = GetDateTimeFromString(lineFields[11]);
                    gLAccountInterestPeriodPM.CreatedByUserId = lineFields[12];
                    gLAccountInterestPeriodPM.UpdatedByUserId = lineFields[13];

                    gLAccountInterestPeriodPMs.Add(gLAccountInterestPeriodPM);
                }
            }
            return gLAccountInterestPeriodPMs.Where(d=>d.PeriodStartDate<=interestReportPM.InterestCalculationDate).OrderByDescending(d => d.PeriodStartDate).ToList();
        }

        public InterestReportPM GetInterestReportPM(string interestReportId, int tenant)
        {
            //Id Tenant  CreateDateTime CreatedByUserId UpdatedByUserId GLAccountId ReportNumber InterestCalculationDate 
            //TotalAmount OpenBalance CloseBalance ARinvoiceId InvoiceAmount GLAccountInterestCreditLimit  
            //InterestReportStatusCode UpdateDateTime  SearchFields CustomerId

            string[] csvInterestReportLines = cSVStringLinesGetter.GetstringLinesFromCSV(path + "interestreport.csv");
            InterestReportPM interestReportPM=new InterestReportPM();
            for (int i = 0; i < csvInterestReportLines.Length; i++)
            {
                if (csvInterestReportLines[i].Length > 0)
                {
                    string[] lineFields = csvInterestReportLines[i].Split(',');
                    interestReportPM.Id = lineFields[0];
                    interestReportPM.Tenant = Convert.ToInt32(lineFields[1]);
                    interestReportPM.CreateDateTime = GetDateTimeFromString(lineFields[2]);
                    interestReportPM.CreatedByUserId = lineFields[3];
                    interestReportPM.UpdatedByUserId = lineFields[4];
                    interestReportPM.GLAccountId = lineFields[5];
                    interestReportPM.ReportNumber = lineFields[6];
                    interestReportPM.InterestCalculationDate = GetDateTimeFromString(lineFields[7]);
                    interestReportPM.TotalAmount = lineFields[8] != "NULL" ? Convert.ToDecimal(lineFields[8]) : 0;
                    interestReportPM.OpenBalance = lineFields[9] != "NULL" ? Convert.ToDecimal(lineFields[9]):0;
                    interestReportPM.CloseBalance = lineFields[10] != "NULL" ? Convert.ToDecimal(lineFields[10]):0;
                    interestReportPM.ARinvoiceId = lineFields[11] != "NULL" ? lineFields[11]:null;
                    interestReportPM.InvoiceAmount = lineFields[12] != "NULL" ? Convert.ToDecimal(lineFields[12]) : 0;
                    interestReportPM.GLAccountInterestCreditLimit= lineFields[13] != "NULL" ? Convert.ToDecimal(lineFields[13]) : 0;
                    interestReportPM.InterestReportStatusCode = lineFields[14];
                    interestReportPM.UpdateDateTime = GetDateTimeFromString(lineFields[15]);
                    interestReportPM.SearchFields = lineFields[16];
                    interestReportPM.CustomerId = lineFields[17];
                }
            }
            return interestReportPM;
        }

        public List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(InterestTransactionGetParameters interestTransactionGetParameters)
        {
            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            string[] csvPeriodsLines = cSVStringLinesGetter.GetstringLinesFromCSV(path + "interesttransactions.csv");
            for (int i = 0; i < csvPeriodsLines.Length; i++)
            {
                //Id	Tenant	CreateDateTime	UpdateDateTime	SearchFields	GLAccountId	
                //InterestEntityTypeCode	EntityId	OriginalEntityLineNumber	LocalAmount
                //ForeignAmount	CurrencyId	InterestValueDate	InterestReportId	IsClosed
                if (csvPeriodsLines[i].Length > 0)
                {
                    string[] lineFields = csvPeriodsLines[i].Split(',');
                    int originalEntityLineNumber;
                    int.TryParse(lineFields[8], out originalEntityLineNumber);
                    InterestTransactionPM interestTransactionPM = new InterestTransactionPM();

                    interestTransactionPM.Id = lineFields[0];
                    interestTransactionPM.Tenant = Convert.ToInt32(lineFields[1]);
                    interestTransactionPM.CreateDateTime = GetDateTimeFromString(lineFields[2]);
                    interestTransactionPM.UpdateDateTime = GetDateTimeFromString(lineFields[3]);
                    interestTransactionPM.SearchFields = lineFields[4];
                    interestTransactionPM.GLAccountId = lineFields[5];
                    interestTransactionPM.InterestEntityTypeCode = lineFields[6];
                    interestTransactionPM.EntityId = lineFields[7];
                    interestTransactionPM.OriginalEntityLineNumber = originalEntityLineNumber;
                    interestTransactionPM.LocalAmount = Convert.ToDecimal(lineFields[9]);
                    interestTransactionPM.ForeignAmount = Convert.ToDecimal(lineFields[10]);
                    interestTransactionPM.CurrencyId = lineFields[11];
                    interestTransactionPM.InterestValueDate = GetDateTimeFromString(lineFields[12]);
                    interestTransactionPM.InterestReportId = lineFields[13];
                    interestTransactionPM.IsClosed = lineFields[14] == "0" ? false : true;

                    if(!interestTransactionPM.IsCancelled 
                        && !interestTransactionPM.IsClosed 
                        && interestTransactionGetParameters.GLAccountIds.Contains( interestTransactionPM.GLAccountId)
                        && interestTransactionPM.InterestValueDate <= interestTransactionGetParameters.InterestCalculationDate)
                            interestTransactionPMs.Add(interestTransactionPM);
                }
            }
            return interestTransactionPMs;
        }

        public GLAccountPM GetGLAccount(string GLAccountId, int tenant) 
        {
            string[] csvInterestReportLines = cSVStringLinesGetter.GetstringLinesFromCSV(path + "interestreportglaccount.csv");
            GLAccountPM gLAccountPM = new GLAccountPM();
            for (int i = 0; i < csvInterestReportLines.Length; i++)
            {
                if (csvInterestReportLines[i].Length > 0)
                {
                    //Id	Tenant	InternalNumber	AccountTypeCode	DisplayNumber	LocalName	EnglishName	SearchFields	
                    //IsMultiCurrency	CurrencyId	RevenueExpenseType	IsControlAccount	ChartOfAccountsId	Inactive	
                    //ChartOfAccountsTypeCode	ReconcileMethodCode	ControlAccountId	AutomaticReconcileId	PreviousEnglishName	
                    //PreviousEnglishNameChangeDate	PreviousLocalName	PreviousLocalNameChangeDate	PreviousNumber	PreviousNumberChangeDate
                    //PreviousChartOfAccountsId	PreviousChartOfAccountsChangeDate	CustomerGLAccountId	RevaluationEnabled	ParentAccountId	
                    //Category1Id	Category2Id	Category3Id	Category4Id	Category5Id	IsVATExempt	DeductionFileTypeId	DeductionFileNumber	AssessingOfficeCode	
                    //Occupation	DeductionTypeId	ConsolidationVat	Drop_temp	IsEquipmentVendor	Drop_temp1	ExcludeFromDeductionReport	
                    //CreatedByUserId	UpdatedByUserId	CreateDate	UpdateDate	AllowEditChequePayToName	ActiveForInterest	InterestCalculationStartDate
                    //ActiveForInterestCreditInvoice	InterestCreditLimit	NameForPrintingCheques	Smallcashbook	MinimumInterestInvoiceBilling
                    string[] lineFields = csvInterestReportLines[i].Split(',');
                    gLAccountPM.Id = lineFields[0];
                    gLAccountPM.Tenant = Convert.ToInt32(lineFields[1]);
                    gLAccountPM.InternalNumber = lineFields[2];
                    gLAccountPM.AccountTypeCode = lineFields[3];
                    gLAccountPM.DisplayNumber = lineFields[4];
                    gLAccountPM.LocalName = lineFields[5];
                    gLAccountPM.EnglishName = lineFields[6];
                    gLAccountPM.SearchFields = lineFields[7];
                    gLAccountPM.ActiveForInterest = lineFields[52] == "0" ? false : true; 
                    gLAccountPM.InterestCalculationStartDate = GetDateTimeFromString(lineFields[53]);
                  
                }
            }
            return gLAccountPM;
        }
        
        
        public DateTime GetDateTimeFromString(string dateString)
        {
            DateTime dateValue;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime.TryParse(dateString,CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
            return dateValue;
        }

        public decimal? GetCreditLimitFromGLAccount(string GLAccount, int tenant)
        {
            throw new NotImplementedException();
        }

        public InterestTransactionPM GetOpenBalanceTransactionForInterestReport(string ReportId, int Tenant)
        {
            throw new NotImplementedException();
        }
    }
}
