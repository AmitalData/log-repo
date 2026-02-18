
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
using Logitude.Accounting.BL.CloseTables;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.Enums;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class TaxReportQueryService
    {
        const string TaxReportLineInputType = "I";
        const string TaxReportLineOutType = "O";

        private string[] _InputsTaxReportLineTypes = new[]
{
            InputsTaxReportLineTypes.IsraeliVendorInputs,
            InputsTaxReportLineTypes.SelfInputs,
            InputsTaxReportLineTypes.SmallCashInputs,
            InputsTaxReportLineTypes.ImportCustomsInputs,
            InputsTaxReportLineTypes.PalestinianVendorsInputs,
            InputsTaxReportLineTypes.LawDefinedDocumentInputs
        };
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

            int count = lines.Where(d => d.StatusCode != "6" && (d.TransmitStatusCode == "1" || d.TransmitStatusCode == "4")).Count(); 

            return count;
        }


        public int GetDuplicateInputReportLines(string taxReportId, int tenant)
        {
            TaxReportLineRepository linesRepo = new TaxReportLineRepository(context);

            IQueryable<TaxReportLine> lines = linesRepo.GetByReportId(taxReportId, tenant);

            int count = lines.Where(d => d.StatusCode == TaxReportLineStatusValues.DuplicateThereIsAnotherTransactionWithTheSameVATNoAndReference 
            && d.TransmitStatusCode == TaxReportLineTransmitStatusValues.ForTransmit
            && _InputsTaxReportLineTypes.Contains(d.LineTypeCode)).Count(); 

            return count;
        }

        public IQueryable<TaxReportLine> GetReportLines(string taxReportId, int tenant)
        {

            IQueryable<TaxReportLine> query = (from a in context.TaxReportLines
                                               where a.TaxReportId == taxReportId && a.Tenant == tenant
                                               select a);
            return query;
        }

        public List<DuplicateRows> GetDuplicateRows(string taxReportId, int tenant)
        {
            IQueryable<string> duplicates = from t in context.TaxReportLines
                             where t.Tenant == tenant && t.TaxReportId == taxReportId
                             group t by new { t.Reference, t.VatNumber } into g
                             where g.Count() > 1
                             select g.Key.Reference + "_" + g.Key.VatNumber;  

            IQueryable<DuplicateRows> q = from t in context.TaxReportLines.Include("Journal")
                    where t.Tenant == tenant &&
                        t.TaxReportId == taxReportId && 
                        duplicates.Contains(t.Reference + "_" + t.VatNumber)
                    select new DuplicateRows
                    {
                        Reference = t.Reference,
                        VatNumber = t.VatNumber,
                        IsVoided = t.Journal.IsVoided,
                        ReferenceDate = t.ReferenceDate,
                        AccountingEntityId = t.Journal.AccountingEntityId,
                        AccountingEntityCode = t.Journal.AccountingEntityCode,
                        Line = t.Line
                    };

            List<DuplicateRows> res = q.ToList();

            return res;
        }
        public List<TaxReportLinePM> GetSpecificReportLines(string taxReportId, int tenant)
        {
            IQueryable<TaxReportLine> query = (from a in context.TaxReportLines
                                               where a.TaxReportId == taxReportId && a.Tenant == tenant
                                               select a);

            var listQuery = query.Select(d => new TaxReportLinePM()
            {
                Tenant = d.Tenant,
                TaxReportId = d.TaxReportId,
                Line = d.Line,
                VatNumber = d.VatNumber,
                Reference = d.Reference,
                StatusCode = d.StatusCode,
            }).ToList();
            return listQuery;
        }

        public List<TaxReportLinePM> GetReportLinesPMs(string taxReportId, int tenant)
        {
            IQueryable<TaxReportLine> query = (from a in context.TaxReportLines
                                               where a.TaxReportId == taxReportId && a.Tenant == tenant
                                               select a);

            var listQuery = query.Select(d => new TaxReportLinePM()
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
                TaxReportDate=d.TaxReportDate,
                TotalInvoiceAmount = d.TotalInvoiceAmount,
                SubTotalInLocalCurrency=d.SubTotalInLocalCurrency
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

            return reports.Where(d => d.IsCancelled == false).ToList();
        }

        public List<TaxReport> GetFutureActiveReportsByTaxReportMonth(DateTime dateTime, int tenant) // not cancelled
        {
            TaxReportRepository reportsRepo = new TaxReportRepository(context);

            IQueryable<TaxReport> reports = reportsRepo.GetFutureReportsByTaxReportMonth(dateTime, tenant);

            return reports.Where(d => d.IsCancelled == false).ToList();
        }

        public List<TaxReportPM> GetTransmittedTaxReports(int tenant)
        {
            TaxReportRepository reportsRepo = new TaxReportRepository(context);

            List<TaxReport> reports = reportsRepo.GetTransmittedReports(tenant).OrderByDescending(d => d.TaxReportMonth).ToList();
            return reports.Select(r => this.GetEntityPM(r, false)).ToList();

        }

        public bool CheckIfTaxReportCanHaveClosingJournal(string taxReportId, string vatOutputGLAccountId, string vatInputsGLAccountId, int tenant, ref List<TaxReportLineForErrors> reconciledLines)
        {
            var sameReferenceAndOppositeVatLines = context.TaxReportLines.Where(x => x.TaxReportId == taxReportId && x.Tenant == tenant && x.VatAmount != 0)
                      .GroupBy(x => new { reference = x.Reference, vatAmount = Math.Abs(x.VatAmount.Value) }).Where(g => g.Count() > 1).ToList();
            var taxReportLinesReferences = sameReferenceAndOppositeVatLines.Select(x => x.Key.reference);

            var outputReconciledLinesQ = (from line in context.TaxReportLines
                                          join ledger in context.LedgerTransactions on line.JournalId equals ledger.JournalId
                                          join journal in context.Journals on line.JournalId equals journal.Id
                                          where line.OutputOrInput == TaxReportLineOutType && line.TaxReportId == taxReportId && line.Tenant == tenant && line.VatAmount != 0 && ledger.AccountId == vatOutputGLAccountId
                                                 && (line.TransmitStatusCode == TaxReportLineTransmitStatusValues.ForTransmit || line.TransmitStatusCode == TaxReportLineTransmitStatusValues.TransmitEvenIfDuplicate)
                                                 && !taxReportLinesReferences.Contains(line.Reference) && (ledger.IsReconciled == true || ledger.InReconcileProgress == true)
                                          select new TaxReportLineForErrors
                                          {
                                              Line = line.Line,
                                              JournalNumber = journal.JournalNumber,
                                              JournalId = journal.Id,
                                          });

            bool hasOutputReconciledLines = outputReconciledLinesQ.Any();

            if (hasOutputReconciledLines)
            {
                reconciledLines = outputReconciledLinesQ.ToList();
            }
            else
            {
                reconciledLines = new List<TaxReportLineForErrors>();
            }

            var inputReconciledLinesQ = (from line in context.TaxReportLines
                                            join ledger in context.LedgerTransactions on line.LedgerTransactionId equals ledger.Id
                                         join journal in context.Journals on line.JournalId equals journal.Id

                                         where line.OutputOrInput == TaxReportLineInputType && line.TaxReportId == taxReportId && line.Tenant == tenant && line.VatAmount != 0 && ledger.AccountId == vatInputsGLAccountId
                                                     && (line.TransmitStatusCode == TaxReportLineTransmitStatusValues.ForTransmit || line.TransmitStatusCode == TaxReportLineTransmitStatusValues.TransmitEvenIfDuplicate)
                                                     && !taxReportLinesReferences.Contains(line.Reference) && (ledger.IsReconciled == true || ledger.InReconcileProgress == true)
                                         select new TaxReportLineForErrors
                                         {
                                             Line = line.Line,
                                             JournalNumber = journal.JournalNumber,
                                             JournalId = journal.Id,
                                         });

            bool hasInputReconciledLines = inputReconciledLinesQ.Any();

            if (hasInputReconciledLines)
            {
                reconciledLines = reconciledLines.Concat(inputReconciledLinesQ).ToList();
            }


            return !(hasOutputReconciledLines || hasInputReconciledLines);
        }

        public List<TaxReportReconciledLines> GetTaxReportReconciledLines(string taxReportId, int tenant)
        {
            FullAccountingSettingQueryService settingQueryService = new FullAccountingSettingQueryService(tenant);
            var fullAccountingSettings = settingQueryService.GetSingleFullAccountingSetting(tenant);

            // Reconciled / in progress Ledger Transaction Lines linked to this TaxReport
            var reconciledLTLines = (from line in context.TaxReportLines
                                     join ledger in context.LedgerTransactions on line.JournalId equals ledger.JournalId
                                     where line.OutputOrInput == TaxReportLineOutType && line.TaxReportId == taxReportId && line.Tenant == tenant && line.VatAmount != 0 && ledger.AccountId == fullAccountingSettings.VATOutputGLAccountId
                                              && (line.TransmitStatusCode == TaxReportLineTransmitStatusValues.ForTransmit || line.TransmitStatusCode == TaxReportLineTransmitStatusValues.TransmitEvenIfDuplicate) && (ledger.IsReconciled == true || ledger.InReconcileProgress == true)
                                     select ledger).Union(
                                             from line in context.TaxReportLines
                                             join ledger in context.LedgerTransactions on line.LedgerTransactionId equals ledger.Id
                                             where line.OutputOrInput == TaxReportLineInputType && line.TaxReportId == taxReportId && line.Tenant == tenant && line.VatAmount != 0 && ledger.AccountId == fullAccountingSettings.VATInputsGLAccountId
                                                      && (line.TransmitStatusCode == TaxReportLineTransmitStatusValues.ForTransmit || line.TransmitStatusCode == TaxReportLineTransmitStatusValues.TransmitEvenIfDuplicate) && (ledger.IsReconciled == true || ledger.InReconcileProgress == true)
                                             select ledger);

            // Reconciliation Lines of the reconciledLTLines
            var ourRecoLines = (from ledger in reconciledLTLines
                                join reconcLine in context.ReconciliationLines on ledger.Id equals reconcLine.TransactionId
                                join reco in context.Reconciliations on reconcLine.ReconciliationId equals reco.Id
                                join taxReportLine in context.TaxReportLines on ledger.Id equals taxReportLine.LedgerTransactionId
                                where reconcLine.Tenant == tenant && taxReportLine.TaxReportId == taxReportId && reco.IsCancelled == false
                                select new TaxReportReconciledLines
                                {
                                    ReconsileNumber = reco.Number,
                                    Line = taxReportLine.Line
                                }).ToList();

            return ourRecoLines;
        }

        public TaxReport GetByReportNunber(string reportNunber) =>
            (from a in context.TaxReports
                where a.TaxReportNumber == reportNunber
                select a).FirstOrDefault();        
    }
    

    public class DuplicateRows
    {
        public string Reference { get;set; }
        public string VatNumber { get;set; }
        public bool? IsVoided { get;set; }
        public DateTime? ReferenceDate { get;set; }
        public string AccountingEntityId { get;set; }
        public string AccountingEntityCode { get; set; }
        public int Line { get; set; }
    }
    public class TaxReportReconciledLines
    {
        public string ReconsileNumber { get; set; }
        public int Line { get; set; }
    }

    public class TaxReportLineForErrors
    {
        public int Line { get; set; }
        public string JournalNumber { get; set; }
        public string JournalId { get; set; }

    }


}
