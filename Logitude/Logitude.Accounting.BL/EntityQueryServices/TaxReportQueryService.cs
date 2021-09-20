
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
        public List<TaxReportLinePM> GetReportLinesPMs(string taxReportId, int tenant)
        {
            IQueryable<TaxReportLine> query = (from a in context.TaxReportLines
                                               where a.TaxReportId == taxReportId && a.Tenant == tenant
                                               select a);

            var listQuery  = query.Select(d => new TaxReportLinePM()
            {
                Tenant = d.Tenant,
                LastUpdateDateTime = d.LastUpdateDateTime,
                UpdatedByUserId = d.UpdatedByUserId,
                SearchFields = d.SearchFields,
                TaxReportId = d.TaxReportId,
                Line = d.Line,
                OutputOrInput = d.OutputOrInput,
                LineTypeCode = d.LineTypeCode,
                VatNumber = d.VatNumber,
                Reference = d.Reference,
                ReferecneGroup = d.ReferecneGroup,
                ReferenceDate = d.ReferenceDate,
                VatAmount = d.VatAmount,
                VatableInvoiceAmount = d.VatableInvoiceAmount,
                StatusCode = d.StatusCode,
                TransmitStatusCode = d.TransmitStatusCode,
                JournalId = d.JournalId,
                IsManuallyChanged = d.IsManuallyChanged,
                IsEquipment = d.IsEquipment,
                TotalInvoiceAmount = d.TotalInvoiceAmount
            }).ToList();
            return listQuery;
        }
        public bool CheckIfThereIsLineWithoutTransmit(string taxReportId, int tenant)
        {
            return (from a in context.TaxReportLines
                                               where a.TaxReportId == taxReportId && a.Tenant == tenant && a.TransmitStatusCode == "0"
                                               select a).Any();
          
        }

        public List<int> CheckErrorsInLines(string taxReportId, int tenant, List<string> errorCodes)
        {
            TaxReportLineRepository linesRepo = new TaxReportLineRepository(context);
         return linesRepo.CheckErrorsInLines(taxReportId, tenant, errorCodes);
        }


        public List<TaxReport> GetFutureActiveReports(DateTime dateTime, int tenant) // not cancelled
        {
            TaxReportRepository reportsRepo = new TaxReportRepository(context);

            IQueryable<TaxReport> reports = reportsRepo.GetFutureReports(dateTime, tenant);

            return reports.Where(d=>d.IsCancelled == false).ToList();
        }
       
    
    }

}
