using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reconcile
{
    public class ImportReconcileExternalPageLineFromCsv
    {
        private List<ReconcileExternalPageLinePM> listFromCsv = new List<ReconcileExternalPageLinePM>();


        public List<ReconcileExternalPageLinePM> ImportCsvFile(string key,string decodedString)
        {
            ReadDataFromCsvFile(decodedString);
            return listFromCsv;
        }
        public void ReadDataFromCsvFile(string decodedString)
        {
            List<string> rows = decodedString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> results = new List<string>();
            foreach (string row in rows)
            {
                results.AddRange(Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"));
            }
            for (int i = 5; i < results.Count;) // the excel has 5 cols  , Excel Analayze
            {
                ReconcileExternalPageLinePM entity = new ReconcileExternalPageLinePM();
                DateTime.TryParse(results[i + 0], out DateTime referenceDate);
                entity.ReferenceDate = referenceDate;
                decimal.TryParse(results[i + 1], out decimal debitAmount);
                entity.DebitAmount = debitAmount;
                decimal.TryParse(results[i + 2], out decimal CreditAmount);
                entity.CreditAmount = CreditAmount;
                entity.Reference = results[i + 3];
                entity.Notes = results[i + 4];
                listFromCsv.Add(entity);
                i += 5;
            }
        }
    }
}
