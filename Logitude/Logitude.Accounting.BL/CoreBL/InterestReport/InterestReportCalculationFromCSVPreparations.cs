using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            string[] csvPeriodsLines = cSVStringLinesGetter.GetstringLinesFromCSV(path+ "interestperiods.csv");
            for (int i = 0; i < csvPeriodsLines.Length; i++)
            {
                //InterestBaseTypeId	LineNumber	Tenant	CreateDate	CreatedByUserId	UpdateDate
                //UpdatedByUserId	InterestBaseStartDate	InterestRate
                string[] lineFields = csvPeriodsLines[i].Split(',');
                int lineNumber;
                int.TryParse(lineFields[1], out lineNumber);
                InterestBasesPeriodPM interestBasesPeriodPM = new InterestBasesPeriodPM()
                {
                    InterestBaseTypeId = lineFields[0],
                    LineNumber = lineNumber,
                    Tenant = tenant,
                    CreateDate = Convert.ToDateTime(lineFields[3]),
                    CreatedByUserId = lineFields[4],
                    UpdateDate = Convert.ToDateTime(lineFields[5]),
                    UpdatedByUserId = lineFields[6],
                    InterestBaseStartDate = Convert.ToDateTime(lineFields[7]),
                    InterestRate = Convert.ToDecimal(lineFields[8]),
                };
                interestBasesPeriodPMs.Add(interestBasesPeriodPM);
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

                string[] lineFields = csvPeriodsLines[i].Split(',');
                int lineNumber;
                int.TryParse(lineFields[0], out lineNumber);
                GLAccountInterestPeriodPM gLAccountInterestPeriodPM = new GLAccountInterestPeriodPM()
                {
                    LineNumber = lineNumber,
                    Tenant = 1,
                    PeriodStartDate = Convert.ToDateTime(lineFields[2]),
                    GLAccountId = lineFields[3],
                    StandardInterestRateBaseId = lineFields[4],
                    StandardAddInterestPercent = Convert.ToDecimal(lineFields[5]),
                    ExceptionalInterestRateBaseId = lineFields[6],
                    ExceptionalAddInterestPercent = Convert.ToDecimal(lineFields[7]),
                    CreditInterestRateBaseId = lineFields[8],
                    CreditAddInterestPercent = Convert.ToDecimal(lineFields[9]),
                    CreateDateTime = Convert.ToDateTime(lineFields[10]),
                    UpdateDateTime = Convert.ToDateTime(lineFields[11]),
                    CreatedByUserId = lineFields[12],
                    UpdatedByUserId = lineFields[13],
                };
                gLAccountInterestPeriodPMs.Add(gLAccountInterestPeriodPM);
            }
            return gLAccountInterestPeriodPMs;
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
                string[] lineFields = csvInterestReportLines[i].Split(',');
                interestReportPM.Id = lineFields[0];
                interestReportPM.Tenant = Convert.ToInt32(lineFields[1]);
                interestReportPM.CreateDateTime = Convert.ToDateTime(lineFields[2]);
                interestReportPM.CreatedByUserId = lineFields[3];
                interestReportPM.UpdatedByUserId = lineFields[4];
                interestReportPM.GLAccountId = lineFields[5];
                interestReportPM.ReportNumber = lineFields[6];
                interestReportPM.InterestCalculationDate = Convert.ToDateTime(lineFields[7]);
                interestReportPM.TotalAmount = Convert.ToDecimal(lineFields[8]);
                interestReportPM.OpenBalance = Convert.ToDecimal(lineFields[9]);
                interestReportPM.CloseBalance = Convert.ToDecimal(lineFields[10]);
                interestReportPM.ARinvoiceId = lineFields[11];
                interestReportPM.InvoiceAmount = Convert.ToDecimal(lineFields[12]);
            }
            return interestReportPM;
        }

        public List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(string glaccountId, DateTime InterestReportCalculationDate, int tenant)
        {
            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            string[] csvPeriodsLines = cSVStringLinesGetter.GetstringLinesFromCSV(path + "interesttransactions.csv");
            for (int i = 0; i < csvPeriodsLines.Length; i++)
            {
                //Id	Tenant	CreateDateTime	UpdateDateTime	SearchFields	GLAccountId	
                //InterestEntityTypeCode	EntityId	OriginalEntityLineNumber	LocalAmount
                //ForeignAmount	CurrencyId	InterestValueDate	InterestReportId	IsClosed
                string[] lineFields = csvPeriodsLines[i].Split(',');
                int originalEntityLineNumber; 
                int.TryParse(lineFields[8],out originalEntityLineNumber);
                InterestTransactionPM interestTransactionPM = new InterestTransactionPM()
                {
                    Id = lineFields[0],
                    Tenant = tenant,
                    CreateDateTime = Convert.ToDateTime(lineFields[2]),
                    UpdateDateTime = Convert.ToDateTime(lineFields[3]),
                    SearchFields = lineFields[4],
                    GLAccountId = lineFields[5],
                    InterestEntityTypeCode = lineFields[6],
                    EntityId = lineFields[7],
                    OriginalEntityLineNumber = originalEntityLineNumber,
                    LocalAmount = Convert.ToDecimal(lineFields[9]),
                    ForeignAmount = Convert.ToDecimal(lineFields[10]),
                    CurrencyId = lineFields[11],
                    InterestValueDate = Convert.ToDateTime(lineFields[12]),
                    InterestReportId = lineFields[13],
                    IsClosed = Convert.ToBoolean(lineFields[14]),
                };
                interestTransactionPMs.Add(interestTransactionPM);
            }
            return interestTransactionPMs;
        }

      
    }
}
