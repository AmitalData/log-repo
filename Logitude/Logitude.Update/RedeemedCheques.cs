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

        List<int> Tenants = new List<int>();

        private void GetChequesBtn_Click(object sender, EventArgs e)
        {
            int tenant = Convert.ToInt32(tenantTextBox.Text);

            BankDepositRedeemedChequesVerifyService verifyService = new BankDepositRedeemedChequesVerifyService();
            var cheques = verifyService.GetNotRedeemedReconciledCheques(tenant);

            dataGridView1.DataSource = cheques;


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
            progressBar1.Value = 0;
            progressBar1.Maximum = Tenants.Count();

            Thread thread = new Thread(() => UpdateAllCheques());
            thread.IsBackground = true;
            thread.Start();

        }

        private void UpdateAllCheques()
        {
            foreach (var tenant in Tenants)
            {
                service.GetAndUpdateChequesForTenant(tenant);
                progressBar1.Value++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            logTextBox.Text = service.LoggingText;
        }
    }
}
