using Logitude.Accounting.BL.CoreBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    public partial class RedeemedCheques : Form
    {
        public RedeemedCheques()
        {
            InitializeComponent();
        }

        BankDepositRedeemedChequesVerifyService service = new BankDepositRedeemedChequesVerifyService();

        List<string> Tenants = new List<string>();

        private void GetChequesBtn_Click(object sender, EventArgs e)
        {
            int tenant = Convert.ToInt32(tenantTextBox.Text);

            BankDepositRedeemedChequesVerifyService verifyService = new BankDepositRedeemedChequesVerifyService();
            var cheques = verifyService.GetNotRedeemedReconciledCheques(tenant);

            dataGridView1.DataSource = cheques;
            countLbl.Text = cheques.Count().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int tenant = Convert.ToInt32(tenantTextBox.Text);
                string chequeId = chequeIdTextBox.Text;

                BankDepositRedeemedChequesVerifyService verifyService = new BankDepositRedeemedChequesVerifyService();
                verifyService.UpdateChequeAsRedeemed(tenant, chequeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            

        }

        private void chequesUpdateAll_Click(object sender, EventArgs e)
        {
            logTextBox.Text = "";
            progressBar1.Value = 0;
            service.DoneTenants = 0;
            service.LoggingText = "";

            var tenantsCSV = tenantsTextBox.Text;
            Tenants = tenantsCSV.Split(',').ToList();
            progressBar1.Maximum = Tenants.Count();

            Thread thread = new Thread(() => service.UpdateCheqesForTenantList(Tenants));
            thread.IsBackground = true;
            thread.Start();

            button3.Enabled = true;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            logTextBox.Text = service.LoggingText;
            progressBar1.Value = service.DoneTenants;
        }

        private void RedeemedCheques_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void fixTotalsBtn_Click(object sender, EventArgs e)
        {
            service.LoggingText = "";

            var tenantsCSV = tenantsTextBox.Text;
            Tenants = tenantsCSV.Split(',').ToList();
            progressBar1.Maximum = Tenants.Count();

            Thread thread = new Thread(() => service.RecalculateChequesTotals(Tenants));
            thread.IsBackground = true;
            thread.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            service.LoggingText = "";

            var tenantsCSV = tenantsTextBox.Text;
            Tenants = tenantsCSV.Split(',').ToList();
            progressBar1.Maximum = Tenants.Count();

            Thread thread = new Thread(() => service.RecalculateAllBilltoChequesTotals(Tenants));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
