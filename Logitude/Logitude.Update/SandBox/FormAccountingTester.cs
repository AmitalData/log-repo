using Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Logitude.Update.PatchDistribution;
using Simplog.Server.Infrastructure.Helpers;
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
            LoggedContactResolver.RegisterLoggedContactUtil();

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
            var openRecoDataValidation = new OpenRecoDataValidationService();
            openRecoDataValidation.GetDataAndValid(csvText);

            var allrows = openRecoDataValidation.GoodRows;

            var goodRowsGoodAccount = allrows.Where(r => !openRecoDataValidation.BadAccounts.Contains(r.InternalNumber)).ToList();

            LogMessagingUtil.Instance.AppendLine($"GoodRows {allrows.Count} ::: goodRowsGoodAccount {goodRowsGoodAccount.Count()}");


            RealOpenAmountService.Get(tenant);
            LogMessagingUtil.Instance.AppendLine($"RealOpenAmount.MyList.Count {RealOpenAmountService.MyList.Count} ");
            if (MessageBox.Show("to continue?", "", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
            {
                return;
            }


            foreach (var itemGroug in goodRowsGoodAccount.GroupBy(r => r.InternalNumber))
            {

                
                    var updateOpenReconcile = new UpdateOpenReconcileService();
                    updateOpenReconcile.DoAccount(tenant, itemGroug.ToList());
                
            }

            ///List<MMPSDataM> list100 = Do100(tenant, goodRowsGoodAccount);
            Debug.WriteLine("done !!!!!!!!!");
        }

        private static List<MMPSDataM> Do100(int tenant, List<MMPSDataM> goodRowsGoodAccount)
        {
            var list100 = new List<MMPSDataM>();
            foreach (var itemGroug in goodRowsGoodAccount.GroupBy(r => r.InternalNumber))
            {

                list100.AddRange(itemGroug.ToList());
                if (list100.Count > 100)
                {
                    var updateOpenReconcile = new UpdateOpenReconcileService();
                    updateOpenReconcile.DoAccount100(tenant, list100);
                    list100.Clear();
                }
            }
            if (list100.Count > 0)
            {
                var updateOpenReconcile = new UpdateOpenReconcileService();
                updateOpenReconcile.DoAccount100(tenant, list100);

            }

            return list100;
        }

        private void agingFixRepoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int tenant = 1;
            string reconcileId = "1-100";

            var agingReportRebulidTesterService = new AgingReportRebulidTesterService();
            agingReportRebulidTesterService.RebulidReconcile(tenant, reconcileId);
        }
    }
}
