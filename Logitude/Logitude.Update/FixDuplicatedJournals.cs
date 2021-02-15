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

            var res = duplicatedJournalsService.GetARPaymentsIdsHavingDuplicatedJournals(tenant);

            dataGridView1.DataSource = res;
        }
    }
}
