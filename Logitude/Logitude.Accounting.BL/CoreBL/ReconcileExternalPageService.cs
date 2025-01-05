using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ReconcileExternalPageService
    {

        public List<ReconcileExternalPageLinePM> BuildReconcileExternalPageLineFromTextLines(String input, int tenant, string bankCodeId, string reconcileExternalPageId, int line, string GLAccountID)
        {
            const string _EmptyDate = "000000";

            List<string> sheet = new List<string>(
                                           input.Split(new string[] { "\r\n" },
                                           StringSplitOptions.RemoveEmptyEntries));


            List<ReconcileExternalPageLinePM> myResult = new List<ReconcileExternalPageLinePM>();
            var bankCodeRepository = new BankCodeRepository(tenant);
            var bankcodePM = bankCodeRepository.GetSingle(bankCodeId, tenant);
            string format = "M/d/yyyy h:mm:ss tt";
            if (bankcodePM?.DateFormat != null)
            {
                format = bankcodePM.DateFormat;
            }
            if (GLAccountID != null)
            {
                var glaccountRepository = new GLAccountRepository(tenant);
                var account = glaccountRepository.GetSingle(GLAccountID, tenant);
                if (account != null)
                {
                    format = account.DateFormat;
                }
            }

            foreach (string inputLine in sheet)
            {
                List<string> cells = new List<string>(
                           inputLine.Split(new string[] { "," },
                           StringSplitOptions.None));


                //String[] rowData = new String[sheet.Columns.Count()];
                ReconcileExternalPageLinePM textReconcileExternalPageLine = new ReconcileExternalPageLinePM();
                textReconcileExternalPageLine.ReconcileExternalPageId = reconcileExternalPageId;
                textReconcileExternalPageLine.Tenant = tenant;
                textReconcileExternalPageLine.ReconcileExternalPageId = "new";
                textReconcileExternalPageLine.IsReconciled = false;
                textReconcileExternalPageLine.LineNumber = line++;

                textReconcileExternalPageLine.Reference = cells[0];
                if (textReconcileExternalPageLine.Reference.Length > 1)
                    textReconcileExternalPageLine.Reference = textReconcileExternalPageLine.Reference.TrimStart('0');


                string txtDateTime = "";
                string fieldname = "";
                string pos = "";
                DateTime date = DateTime.MinValue;
                string referenceDateString = "";
                txtDateTime = cells[1].TrimEnd(' ').Replace('/', '.');

                if (txtDateTime.Length >= 8) txtDateTime = txtDateTime.Substring(0, 8);
                referenceDateString = txtDateTime;
                if (referenceDateString != _EmptyDate)
                {
                    fieldname = "ReferenceDate";
                    pos = "0, 6";
                    date = ReconcileExternalPageService.TryGetDateTime(cells[1], txtDateTime, fieldname, pos, format: "ddMMyy");
                    textReconcileExternalPageLine.ReferenceDate = date;
                }
                string notes = cells[2].TrimStart('"').TrimEnd('"');

                if (!String.IsNullOrWhiteSpace(notes))
                    notes = ReverseString(notes);

                textReconcileExternalPageLine.Notes = notes;



                decimal.TryParse(cells[3], out decimal amount);
                if (amount > 0)
                {
                    textReconcileExternalPageLine.CreditAmount = amount;
                    textReconcileExternalPageLine.DebitAmount = 0m;
                }
                else
                {
                    textReconcileExternalPageLine.DebitAmount = -amount;
                    textReconcileExternalPageLine.CreditAmount = 0m;
                }






                myResult.Add(textReconcileExternalPageLine);
            }
            return myResult;
        }
        public static string ReverseString(string input)
        {
            var inputArray = input.ToCharArray();
            Array.Reverse(inputArray);
            return new string(inputArray);
        }


        public static DateTime TryGetDateTime(string rawLine, string txtDateTime, string fieldname, string pos, string @format = "ddMMyy")
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(txtDateTime, @format/*"ddMMyy"*/, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (date == DateTime.MinValue)
            {
                throw new
                    Exception($"{fieldname} should be in the {@format} format, while Substring({pos}) ={rawLine}  ");
            }

            return date;
        }
    }
}
