using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportTestResultGetter
    {
        public List<InterestReportLinesByDatePM> GetInterestReportLinesByDatePMsFromCSV(string path)
        {
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = new List<InterestReportLinesByDatePM>();
            string[] csvInterestReportLinesByDatePMs = GetstringLinesFromCSV(path + "InterestReportLinesByDate.csv");
            for (int i = 0; i < csvInterestReportLinesByDatePMs.Length; i++)
            {
                //Id	Tenant	InterestReportId	FromDate	ToDate	TotalInterestDays	TotalAmount	AccumulatedAmount	
                //StandardInterestPercentage	ExceptionalInterestPercentage	CreditInterestPercentage	StandardInterestAmount
                //ExceptionalInterestAmount	CreditInterestAmount	CalculatedStandInterestAmount	CalculatedExcepInterestAmount	
                //CalculatedCreditInterestAmount	CalculationDetails	LineNumber
                string[] lineFields = csvInterestReportLinesByDatePMs[i].Split(',');
                int lineNumber;
                int.TryParse(lineFields[1], out lineNumber);
                InterestReportLinesByDatePM interestReportLinesByDatePM = new InterestReportLinesByDatePM()
                {
                    Id = lineFields[0],
                    Tenant = Convert.ToInt32(lineFields[1]),
                    InterestReportId = lineFields[2],
                    FromDate = Convert.ToDateTime(lineFields[3]),
                    ToDate = Convert.ToDateTime(lineFields[4]),
                    TotalInterestDays = Convert.ToInt32(lineFields[5]),
                    TotalAmount = Convert.ToDecimal(lineFields[6]),
                    AccumulatedAmount = Convert.ToDecimal(lineFields[7]),
                    StandardInterestPercentage = Convert.ToDecimal(lineFields[8]),
                    ExceptionalInterestPercentage = Convert.ToDecimal(lineFields[9]),
                    CreditInterestPercentage = Convert.ToDecimal(lineFields[10]),
                    StandardInterestAmount = Convert.ToDecimal(lineFields[11]),
                    ExceptionalInterestAmount = Convert.ToDecimal(lineFields[12]),
                    CreditInterestAmount = Convert.ToDecimal(lineFields[13]),
                    CalculatedStandInterestAmount = Convert.ToDecimal(lineFields[14]),
                    CalculatedExcepInterestAmount = Convert.ToDecimal(lineFields[15]),
                    CalculatedCreditInterestAmount = Convert.ToDecimal(lineFields[16]),
                    CalculationDetails = lineFields[17],
                    LineNumber = Convert.ToInt32(lineFields[18]),
                };
                interestReportLinesByDatePMs.Add(interestReportLinesByDatePM);
            }
            return interestReportLinesByDatePMs;
        }

        private string[] GetstringLinesFromCSV(string path)
        {
            string combinedPath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), path);
            string[] pathstringSeparators = new string[] { "bin\\Debug\\" };
            string[] pathParts = combinedPath.Split(pathstringSeparators, StringSplitOptions.None);
            string filePath = pathParts[0] + pathParts[1];
            string csvInterestBasesPeriods = File.ReadAllText(filePath);
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = csvInterestBasesPeriods.Split(stringSeparators, StringSplitOptions.None);
            lines = lines.Where(d => d != lines[0]).ToArray();
            return lines;
        }
    }
}
