using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.Resolvers;
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

            LoggedContactResolver.RegisterLoggedContactUtil();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DuplicatedJournalsVoidingService duplicatedJournalsService = new DuplicatedJournalsVoidingService();

            int tenant = Convert.ToInt32(tenantTextBox.Text);

            var journals = duplicatedJournalsService.GetARPaymentDuplicatedJournals(tenant);

            ShowData(journals);

        }

        private void removeDuplicatedJournals_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Voiding ...";

            DuplicatedJournalsVoidingService voidingService = new DuplicatedJournalsVoidingService();
            var voidedJournals = voidingService.VoidDuplicatedJournalsOfARPayment(Convert.ToInt32(tenantTextBox.Text));
            
            ShowData(voidedJournals);

            statusLabel.Text = "Fixed";
        }

        public void ShowData(List<Journal> voidedJournals)
        {
            SetCount(voidedJournals.Count());
            SetGridViewDataSource(voidedJournals);
        }

        private void SetGridViewDataSource(List<Journal> voidedJournals)
        {
            dataGridView1.DataSource = voidedJournals;
        }

        private void SetCount(int count)
        {
            countLbl.Text = count.ToString();
        }

        private void FixDuplicatedJournals_Load(object sender, EventArgs e)
        {

        }
    }
}
