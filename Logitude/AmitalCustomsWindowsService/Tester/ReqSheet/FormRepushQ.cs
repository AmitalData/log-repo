using CustomsWorkerRole.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmitalCustomsWindowsService.Tester.ReqSheet
{
    public partial class FormRepushQ : Form
    {
        public FormRepushQ()
        {
            InitializeComponent();
        }

        private void buttonRepush_Click(object sender, EventArgs e)
        {
            string customsRequestsSheetId = textBoxReqSheetId.Text ;
            int tenant = int.Parse(textBoxTenant.Text);
            clsTester.RequestSheetRepushQService(customsRequestsSheetId, tenant);
        }

        private void FormRepushQ_Load(object sender, EventArgs e)
        {
             
        }
    }
}
