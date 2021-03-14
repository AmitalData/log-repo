using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalsReapproveService
    {
        public EventHandler progressChanged;
        const string journalApprovedStatus = "2";
        public JournalsReapproveService()
        {
        }

        public List<Journal> GetUnapprovedJournals(JournalsFilter journalsFilter)
        {
            IQueryable<Journal> journals = GetTenantJournalsWhereLedgerNotCreated(journalsFilter.Tenant);

            journals = FilterJournals(journalsFilter, journals);
            journals = OrderJournals(journals);

            return journals.ToList();
        }

        JournalsFilter journalsFilter;
        public int progress = 0;
        public void ReapprovedJournals(JournalsFilter journalsFilter)
        {
            this.journalsFilter = journalsFilter;

            BackgroundWorker worker = BuildBackgroundWorker();
            worker.RunWorkerAsync();
        }

        private BackgroundWorker BuildBackgroundWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += background_DoWork;
            worker.RunWorkerCompleted += background_Completed;
            worker.ProgressChanged += background_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            return worker;
        }

        private void background_DoWork(object sender, EventArgs eventArgs)
        {
            var worker = sender as BackgroundWorker;

            List<JournalPM> journals = GetJournals();

            var completedCount = 0;
            double progressRatio = 100.0 / journals.Count();

            using (var transaction = new TransactionScope())
            {
                try
                {
                    foreach (var journal in journals)
                    {
                        UpdateJournal(journal);

                        var currentProgress = Convert.ToInt32(++completedCount * progressRatio);
                        worker.ReportProgress(currentProgress);
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message +Environment.NewLine +ex.ToString());
                    throw ex;
                }               

            }

        }

        private void UpdateJournal(JournalPM journal)
        {           
            journal.StatusCode = journalApprovedStatus;
            journal.ChangeSetOp = ChangeSetOperation.Update;
            var accountingContex = AccountingContext.GetContext(journalsFilter.Tenant);
            JournalUpdateService service = new JournalUpdateService(accountingContex, new Dictionary<string, IContext>(), journalsFilter.Tenant);
            service.Update(journal, true);
        }

        private List<JournalPM> GetJournals()
        {
            IQueryable<Journal> journals = GetTenantJournalsWhereLedgerNotCreated(journalsFilter.Tenant);
            journals = FilterJournals(journalsFilter, journals);

            List<JournalPM> journalsPM = GetJournalsPMs(journals);
            return journalsPM;
        }

        private List<Def.EntityPMs.JournalPM> GetJournalsPMs(IQueryable<Journal> journals)
        {
            JournalQueryService journalQueryService = new JournalQueryService(journalsFilter.Tenant);
            List<string> journalsIds = journals.Select(j => j.Id).ToList();
            var journalsPM = journalQueryService.GetFullJournalPMsByIds(journalsIds, journalsFilter.Tenant);
            return journalsPM;
        }

        private void background_Completed(object sender, EventArgs eventArgs)
        {

        }
        private void background_ProgressChanged(object sender, ProgressChangedEventArgs eventArgs)
        {
            progress = eventArgs.ProgressPercentage;
            progressChanged.Invoke(this, eventArgs);
        }
        private static IQueryable<Journal> OrderJournals(IQueryable<Journal> journals)
        {
            journals = journals.OrderByDescending(j => j.CreateDate);
            return journals;
        }

        private static IQueryable<Journal> FilterJournals(JournalsFilter journalsFilter, IQueryable<Journal> journals)
        {
            if (journalsFilter.FromDate != null && journalsFilter.ToDate != null)
                journals = journals.Where(j => j.CreateDate >= journalsFilter.FromDate && j.CreateDate <= journalsFilter.ToDate);

            if (journalsFilter.IsExternalSystem)
                journals = journals.Where(j => j.ExternalSystem != null && j.ExternalNo != null);

            if (!string.IsNullOrWhiteSpace(journalsFilter.JournalId))
                journals = journals.Where(j => j.Id == journalsFilter.JournalId);

            if (!string.IsNullOrWhiteSpace(journalsFilter.JournalNumber))
                journals = journals.Where(j => j.JournalNumber == journalsFilter.JournalNumber);

            if (!string.IsNullOrWhiteSpace(journalsFilter.StatusCode))
                journals = journals.Where(j => j.StatusCode == journalsFilter.StatusCode);

            return journals;
        }

        private static IQueryable<Journal> GetTenantJournalsWhereLedgerNotCreated(int tenant)
        {
            var accountingContex = AccountingContext.GetContext(tenant);

            return accountingContex.Journals.Where(j => j.IsLedgerCreated == false && j.Tenant == tenant);
        }
    }

    public class JournalsFilter
    {
        public int Tenant { get; set; }
        public DateTime? FromDate  { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsExternalSystem { get; set; } = false;
        public string JournalId { get; set; }
        public string JournalNumber { get; set; }
        public string StatusCode { get; set; }
    }
}
