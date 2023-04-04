using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Data.Entity.Core.Objects;
using System.Text.RegularExpressions;
using System.Numerics;

namespace Logitude.Accounting.BL.DataContract
{
    public class ARinvoiceSequencesReportDataProvider
    {
        public int Tenant;
        public DateTime startDate;
        public DateTime endDate;
        private IInvoiceContext invoiceContext;
        private Simplog.Data.CommonDataModel.ICommonDataContext commoncontext;
        private IAccountingContext accountingContext;
        private TenantPM tenantPM;
        DateTime? reportMonth;
        ARinvoiceSequencesReportParameters _ARinvoiceSequencesReportParameters;
        public ARinvoiceSequencesReportDataProvider(int tenant, ARinvoiceSequencesReportParameters aRinvoiceSequencesReportParameters)
        {
            Tenant = tenant;
            _ARinvoiceSequencesReportParameters = aRinvoiceSequencesReportParameters;
            SetDates();

            invoiceContext = InvoiceContext.GetContext(tenant);
            commoncontext = CommonDataContext.GetContext(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
        }
        private void SetDates()
        {
            if (_ARinvoiceSequencesReportParameters != null)
            {
                startDate = _ARinvoiceSequencesReportParameters.FromDate;
                endDate = _ARinvoiceSequencesReportParameters.ToDate;
            }
        }
        ARinvoiceSequencesReportData aRinvoiceSequencesReportData;
        public ARinvoiceSequencesReportData GetARinvoiceSequencesReportData()
        {
            aRinvoiceSequencesReportData = new ARinvoiceSequencesReportData();
            var invoicesNumbers = GetARInvoiceNumber();
            var sequances = GetARInvoicesSequnaces(invoicesNumbers);
            aRinvoiceSequencesReportData.Sequances = sequances;
            return aRinvoiceSequencesReportData;
        }

        List<string> GetARInvoicesSequnaces(List<string> invoicesNumbers)
        {
            List<string> FormatedinvoicesNumbers = new List<string>();
            foreach (string a in invoicesNumbers)
            {
                FormatedinvoicesNumbers.Add(FormatInvocieNumber(a));
            }
            FormatedinvoicesNumbers = FormatedinvoicesNumbers.Select(BigInteger.Parse).OrderBy(e => e).Select(e => e.ToString()).Distinct().ToList();
            List<string> sequances = new List<string>();
            string seqFrom = null;
            for (var i = 0; i < FormatedinvoicesNumbers.Count - 1; i++)
            {
                if (seqFrom == null)
                {
                    seqFrom = FormatedinvoicesNumbers[i];
                }

                if (int.TryParse(Regex.Replace(FormatedinvoicesNumbers[i], @"[^\d]", ""), out int curr) && int.TryParse(Regex.Replace(FormatedinvoicesNumbers[i + 1], @"[^\d]", ""), out int next))
                {

                    if ((next - curr) == 1)
                    {
                        if ((i + 1) == FormatedinvoicesNumbers.Count - 1)
                        {
                            sequances.Add(seqFrom + " - " + FormatedinvoicesNumbers[i + 1]);
                        }
                        continue;
                    }
                    else
                    {
                        sequances.Add(seqFrom + " - " + FormatedinvoicesNumbers[i]);
                        if ((i + 1) == FormatedinvoicesNumbers.Count - 1)
                        {
                            sequances.Add(FormatedinvoicesNumbers[i + 1] + " - " + FormatedinvoicesNumbers[i + 1]);
                        }
                        seqFrom = null;
                    }
                }
            }

            return sequances;
        }

        private string FormatInvocieNumber(string invoiceNumber) {
            int count = 0;
            for (int i = 0; i < invoiceNumber.Length; i++)
            {
                if (!Char.IsDigit(invoiceNumber[i]))
                {
                    count = i + 1;
                }
            }
            return invoiceNumber.Remove(0, count);
        }
        public List<string> GetARInvoiceNumber()
        {
            Int64 dummy;
            List<string> invoicesNumbers = new List<string>();
            var list = (from a in invoiceContext.ARInvoices
                        where (EntityFunctions.TruncateTime(a.CreateDate) >= startDate.Date && EntityFunctions.TruncateTime(a.CreateDate) <= endDate.Date)
                        select a   
                        ).ToList();
            List<string> invoiceNumbersContainOnlyNumbers = list.Where(a => a.Tenant == Tenant && Int64.TryParse(a.InvoiceNumber, out dummy) == true).OrderBy(
                                                            a => Convert.ToInt64(a.InvoiceNumber)).Select(x => x.InvoiceNumber).ToList();


            List<string> invoiceNumbersContainCharacters = list.Where(a => a.Tenant == Tenant && Int64.TryParse(a.InvoiceNumber, out dummy) == false).OrderBy(
                                                            a => a.InvoiceNumber).Select(x => x.InvoiceNumber).ToList();
            invoicesNumbers.AddRange(invoiceNumbersContainOnlyNumbers);
            invoicesNumbers.AddRange(invoiceNumbersContainCharacters);
            return invoicesNumbers;
        }



    }
}
