using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
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
    public partial class FixDuplicatedJournals : Form
    {
        public FixDuplicatedJournals()
        {
            InitializeComponent();
        }

        private void GetJournalsBtn_Click(object sender, EventArgs e)
        {
            DuplicatedJournalsService duplicatedJournalsService = new DuplicatedJournalsService();

            int tenant = Convert.ToInt32(tenantTextBox.Text);

            var arpaymentIds = duplicatedJournalsService.GetARPaymentsIdsHavingDuplicatedJournals(tenant);

            countLbl.Text = arpaymentIds.Count().ToString();
            dataGridView1.DataSource = arpaymentIds;


            //foreach (var arpaymentId in arpaymentIds)
            //{

            //}
            //// Get journals
            //JournalQueryService journalQueryService = new JournalQueryService(tenant);
            //var res = journalQueryService.GetByAccountingEntityIdAndAccountingEntityCode()



            //var context = AccountingContext.GetContext(tenant);
            //var service = new JournalVoidUpdateService(context, new Dictionary<string, IContext>(), tenant);

            //var StornoOverrideM = new Accounting.Def.EntityUpdateServicesExt.StornoOverrideM()
            //{
            //    AccountingEntityCode = AccountingEntityCode,
            //    AccountingEntityId = AccountingEntityId,
            //    AccountingEntityReference = AccountingEntityReference,
            //};
            //journalPM = service.VoidJournal(JournalId, tenant, StornoOverrideM);




        }

        private void button1_Click(object sender, EventArgs e)
        {
            DuplicatedJournalsService duplicatedJournalsService = new DuplicatedJournalsService();

            int tenant = Convert.ToInt32(tenantTextBox.Text);

            var journals = duplicatedJournalsService.GetJournalsToVoid(tenant);
            countLbl.Text = journals.Count().ToString();
            dataGridView1.DataSource = journals;
        }

        private void fixDupJournals_Click(object sender, EventArgs e)
        {
            DuplicatedJournalsService duplicatedJournalsService = new DuplicatedJournalsService();

            int tenant = Convert.ToInt32(tenantTextBox.Text);
            statusLbl.Text = "Getting Journals ...";

            var journals = duplicatedJournalsService.GetJournalsToVoid(tenant);

            countLbl.Text = journals.Count().ToString();
            dataGridView1.DataSource = journals;

            statusLbl.Text = "Voiding ...";
            duplicatedJournalsService.FixDuplicatedjournals(tenant);
            statusLbl.Text = "Fixed";


        }
    }
}
