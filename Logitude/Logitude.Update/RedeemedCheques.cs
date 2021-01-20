using Logitude.Accounting.BL.CoreBL;
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
    public partial class RedeemedCheques : Form
    {
        public RedeemedCheques()
        {
            InitializeComponent();
        }

        private void GetChequesBtn_Click(object sender, EventArgs e)
        {
            int tenant = Convert.ToInt32(tenantTextBox.Text);

            BankDepositRedeemedChequesVerifyService verifyService = new BankDepositRedeemedChequesVerifyService(tenant);
            var cheques = verifyService.GetNotRedeemedReconciledCheques();

            dataGridView1.DataSource = cheques;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int tenant = Convert.ToInt32(tenantTextBox.Text);
                string chequeId = chequeIdTextBox.Text;

                BankDepositRedeemedChequesVerifyService verifyService = new BankDepositRedeemedChequesVerifyService(tenant);
                verifyService.UpdateChequeAsRedeemed(tenant, chequeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            

        }
    }
}
