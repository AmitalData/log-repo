
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.DataContract;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class TaxReportQueryService
    {
        public TaxReportLinesCounter GetReportLinesCounter(string taxReportId, int tenant)
        {
            TaxReportLineRepository linesRepo = new TaxReportLineRepository(context);

            IQueryable<TaxReportLine> lines = linesRepo.GetByReportId(taxReportId, tenant);

            TaxReportLinesCounter reportCounters = new TaxReportLinesCounter();
            reportCounters.TaxableTransactions = lines.Where(d => d.OutputOrInput == "O" && d.VatAmount != 0).Count();
            reportCounters.ExcemptTransactions = lines.Where(d => d.OutputOrInput == "O" && d.VatAmount == 0).Count();
            reportCounters.InputEquipments = lines.Where(d => d.OutputOrInput == "I" && d.IsEquipment == true).Count();
            reportCounters.InputOthers = lines.Where(d => d.OutputOrInput == "I" && d.IsEquipment == false).Count();

            return reportCounters;
        }

        public int GetReportLinesWithErrors(string taxReportId, int tenant)
        {
            TaxReportLineRepository linesRepo = new TaxReportLineRepository(context);

            IQueryable<TaxReportLine> lines = linesRepo.GetByReportId(taxReportId, tenant);
            //var l = lines.ToList();
            int count = lines.Where(d => d.StatusCode != "6" && d.TransmitStatusCode == "1").Count(); // 6- Ready for transmit , 1- For transmit

            return count;
        }

        public IQueryable<TaxReportLine> GetReportLines(string taxReportId, int tenant)
        {
            IQueryable<TaxReportLine> query = (from a in context.TaxReportLines
                    where a.TaxReportId == taxReportId && a.Tenant == tenant
                    select a);
            return query;
        }


    }

}
