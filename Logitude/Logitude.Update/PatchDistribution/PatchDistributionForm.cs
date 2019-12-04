using Devart.Data.Oracle;
using Logitude.Customs.BL.PatchDistribution;
using Logitude.Customs.BL.PatchDistribution.Patches;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
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
        public bool StartEnabled { get; private set; }

        public PatchDistributionForm()
        {
            InitializeComponent();
            //DbContextBaseUtil.ToLog = checkBox1.Checked = true;
            TraceListener debugListener = new MyTraceListener(this.textBoxLogger);
            Debug.Listeners.Add(debugListener);


        }



        private void doItToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            myP19R03_0000_PatchDist.CreateSeedDbMigrateTable();

            var myPatchDistributionManager = new PatchDistributionManager();
            myPatchDistributionManager.GetValidPatchDistributionList();



            var assemblyUtil = new Logitude.Server.Tools.Helpers.AssemblyUtil();
            var prodInfo = assemblyUtil.GetProductInfo(typeof(JustWebFreight.WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses.MyEntityUpdateClass).Assembly);

            var assemblyVersion = assemblyUtil.GetVersion(prodInfo);

            var patchDistributionMatch = new PatchDistributionMatch();
            var patchDistributionMatchModel =patchDistributionMatch.GetPatchDistributionMatchModel(assemblyVersion);
            Debug.WriteLine(patchDistributionMatchModel.Message);
            switch (patchDistributionMatchModel.MajorVersionMatch)
            {
                case PatchDistributionMatch.MajorVersionMatchEnum.NotDistributionBranch:
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.OldSource:
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.OldDB:
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.OK:
                    if (patchDistributionMatchModel.NotDistributionBranch)
                    {
                        if (MessageBox.Show("NotDistributionBranch to continue ?","", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    myPatchDistributionManager.GetPatchDistributionToDo(patchDistributionMatchModel.LastDBMigration.MajorVersion, patchDistributionMatchModel.LastDBMigration.MinorVersion);
                    StartEnabled = true;
                    break;
                default:
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