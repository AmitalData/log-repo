using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.Resolvers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    public partial class JournalsReapprovalTool : Form
    {
        public JournalsReapprovalTool()
        {
            InitializeComponent();
            LoggedContactResolver.RegisterLoggedContactUtil();

        }

        private void JournalsReapprovalTool_Load(object sender, EventArgs e)
        {

        }

        private void GetJournalsBtn_Click(object sender, EventArgs e)
        {
            JournalsReapproveService reapproveService = new JournalsReapproveService();

            JournalsFilter filters = BuildJournalsFilter();

            List<Journal> journals = reapproveService.GetUnapprovedJournals(filters);
            
            SetGridViewSource(journals);
        }

        private void SetGridViewSource(List<Journal> journals)
        {
            resultGridView.DataSource = journals;
            countLbl.Text = journals.Count().ToString();
        }

        private JournalsFilter BuildJournalsFilter()
        {
            var filters = new JournalsFilter()
            {
                Tenant = int.Parse(tenantTextBox.Text),
                IsExternalSystem = externalSystemCheckBox.Checked,
                JournalId = journalIdTextBox.Text,
                JournalNumber = JournalNumberTextBox.Text,
                StatusCode = StatusTextBox.Text,
            };

            if (fromDatePicker.Checked)
                filters.FromDate = fromDatePicker.Value;

            if (toDatePicker.Checked)
                filters.ToDate = toDatePicker.Value;

            return filters;
        }

        private void approveButton_Click(object sender, EventArgs e)
        {
            SetStartingProgressStatus();

            JournalsReapproveService reapproveService = new JournalsReapproveService();
            JournalsFilter filters = BuildJournalsFilter();

            reapproveService.ReapprovedJournals(filters);

            reapproveService.progressChanged += ReapproveService_ProgressChanged;
        }

        private void SetStartingProgressStatus()
        {
            progressLabel.Text = "Starting ...";
            approveBtn.Enabled = false;
        }

        public void ReapproveService_ProgressChanged(object sender, EventArgs eventArgs)
        {
            var journalsReapproveService = sender as JournalsReapproveService;
            var progress = journalsReapproveService.progress;
            progressBar1.Value = progress;
            UpdateProgressLabel(progress);
            if(progress == 100)
                approveBtn.Enabled = true;
        }

        private void UpdateProgressLabel(int progress)
        {
            if (progress == 1)
                progressLabel.Text = "Started...";
            else if (progress == 98)
                progressLabel.Text = "Almost...";
            else if (progress == 100)
                progressLabel.Text = "Done";
        }
    }
}
