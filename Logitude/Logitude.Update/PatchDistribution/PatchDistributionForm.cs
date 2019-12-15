using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.PatchDistribution;
using Logitude.Customs.BL.PatchDistribution.Patches;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Logitude.Update.PatchDistribution
{
    public partial class PatchDistributionForm : Form
    {
        private List<PatchDistributionBase> _PatchDistributionList;
        private PatchDistributionManager _PatchDistributionManager;
        private PatchDistributionMatchModel _PatchDistributionMatchModel;

        public bool StartEnabled { get => this.doItToolStripMenuItem.Enabled; set => this.doItToolStripMenuItem.Enabled = value; }

        public PatchDistributionForm()
        {
            InitializeComponent();
            //DbContextBaseUtil.ToLog = checkBox1.Checked = true;
            TraceListener debugListener = new MyTraceListener(this.textBoxLogger);
            Debug.Listeners.Add(debugListener);
            StartEnabled = false;
        }



        private void doItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _PatchDistributionManager.Exec(_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion);
        }




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DbContextBaseUtil.ToLog = checkBox1.Checked;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void PatchDistributionForm_Load(object sender, EventArgs e)
        {
            var myP19R03_0000_PatchDist = new P19R03_0001_PatchDist();
            myP19R03_0000_PatchDist.Enshure_SeedDbMigrateTable();

            _PatchDistributionManager = new PatchDistributionManager();
            _PatchDistributionManager.Check_PatchDistributionListAreValid();



            var assemblyUtil = new Logitude.Server.Tools.Helpers.AssemblyUtil();
            var prodInfo = assemblyUtil.GetProductInfo(typeof(JustWebFreight.WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses.MyEntityUpdateClass).Assembly);
            var assemblyVersion = assemblyUtil.GetVersion(prodInfo);
            Debug.WriteLine($"assemblyVersion ={assemblyVersion}");


            var patchDistributionMatch = new PatchDistributionMatch();
            _PatchDistributionMatchModel =patchDistributionMatch.GetPatchDistributionMatchModel(assemblyVersion);
            Debug.WriteLine(_PatchDistributionMatchModel.Message);
            Debug.WriteLine($"DB MajorVersion={_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion}");
            Debug.WriteLine($"DB MinorVersion={_PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion}");


            if (_PatchDistributionMatchModel.NotDistributionBranch)
            {
                if (MessageBox.Show($"{_PatchDistributionMatchModel.Message}  to continue (should be D not {prodInfo}) ?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }


            switch (_PatchDistributionMatchModel.MajorVersionMatch)
            {
                case PatchDistributionMatch.MajorVersionMatchEnum.OldDB:
                    var patchDistributionList_RealyOldDB = _PatchDistributionManager.GetPatchDistribution_Waiting2Exec(_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion);

                    if (patchDistributionList_RealyOldDB.Count == 0)
                    {
                        MessageBox.Show("Nothing TODO the MajorVersion is old - exec Main major script " + _PatchDistributionMatchModel.Message);
                        return;
                    }
                    if (MessageBox.Show($"{_PatchDistributionMatchModel.Message}  to continue Execute minor script Of Old  MajorVersion ?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    StartEnabled = true;
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.OK_DBAndAssemblyREqual:

                    var patchDistributionList = _PatchDistributionManager.GetPatchDistribution_Waiting2Exec(_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion);
                    if (patchDistributionList.Count == 0)
                    {
                        MessageBox.Show("Nothing TODO- OK_DB And Assembly R Equal  MajorVersion+MinorVersion ");
                        return;
                    }
                    Debug.WriteLine("Menu >> Start >  Doit !!!");
                    _PatchDistributionList = patchDistributionList;

                    StartEnabled = true;
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.NotDistributionBranch:
                case PatchDistributionMatch.MajorVersionMatchEnum.OldSource:
                default:
                    MessageBox.Show(_PatchDistributionMatchModel.Message ?? " Nothing i can do ");
                    break;
            }



            


        }



    }

    public class MyTraceListener : TraceListener
    {
        private TextBoxBase output;

        public MyTraceListener(TextBoxBase output)
        {
            this.Name = "Trace";
            this.output = output;
        }


        public override void Write(string message)
        {

            Action append = delegate ()
            {
                output.AppendText(string.Format("[{0}] ", DateTime.Now.ToString()));
                output.AppendText(message);
            };
            if (output.InvokeRequired)
            {
                output.BeginInvoke(append);
            }
            else
            {
                append();
            }

        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }

}