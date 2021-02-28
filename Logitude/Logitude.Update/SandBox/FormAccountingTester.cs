using Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile;
using Logitude.Update.PatchDistribution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update.SandBox
{
    public partial class FormAccountingTester : Form
    {
        public FormAccountingTester()
        {
            InitializeComponent();
            TraceListener debugListener = new MyTraceListener(this.textBoxLogger);
            Debug.Listeners.Add(debugListener);

        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int tenant = int.Parse(_TBTenant.Text);
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var CSV = openFileDialog1.FileName;
            string csvText = File.ReadAllText(CSV);
            var openRecoDataValidation = new OpenRecoDataValidation();
            openRecoDataValidation.GetDataAndValid(csvText);
            if (MessageBox.Show("to continue?", "", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
            {
                return;
            }
            var allrows = openRecoDataValidation.GoodRows;
            var list100 = new List<MMPSDataM>();
            foreach (var itemGroug in allrows.GroupBy(r => r.InternalNumber))
            {

                list100.AddRange(itemGroug.ToList());
                if (list100.Count > 100)
                {
                    var updateOpenReconcile = new UpdateOpenReconcile();
                    updateOpenReconcile.DoAccount100(tenant,list100);
                    list100.Clear();
                }
            }
            if (list100.Count > 0)
            {
                var updateOpenReconcile = new UpdateOpenReconcile();
                updateOpenReconcile.DoAccount100(tenant, list100);

            }
            Debug.WriteLine("done !!!!!!!!!");
        }
    }
}
