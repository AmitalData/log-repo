using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.PatchDistribution;
using Logitude.Customs.BL.PatchDistribution.Patches;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Utils;
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

        public bool UpdateDBEnabled { get => this.buttonUpdateDB.Enabled; set => this.buttonUpdateDB.Enabled = value; }
        public bool ApproveEnabled { get=> buttonApproveLastFailure.Enabled; set=> textBoxApproveRemarks.Enabled= buttonApproveLastFailure.Enabled = value;  }

        private PatchDistributionException _MyPatchDistributionException;

        public PatchDistributionForm()
        {
            InitializeComponent();
            //DbContextBaseUtil.ToLog = checkBox1.Checked = true;
            ApproveEnabled =UpdateDBEnabled = false;
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
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"assemblyVersion ={assemblyVersion}");


            var patchDistributionMatch = new PatchDistributionMatch();
            _PatchDistributionMatchModel =patchDistributionMatch.GetPatchDistributionMatchModel(assemblyVersion);

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(_PatchDistributionMatchModel.Message);
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"DB MajorVersion={_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion}");
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"DB MinorVersion Last Closed !!!={_PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion}");
            //Logger.LogMe($"DB MinorLine={_PatchDistributionMatchModel.Last_DBMigrationLine.CounterKey}", false);


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
                    var patchDistributionList_RealyOldDB = _PatchDistributionManager.GetPatchDistribution_MinorNotClosed(_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion);

                    if (patchDistributionList_RealyOldDB.Count == 0)
                    {
                        MessageBox.Show("Nothing TODO the MajorVersion is old - exec Main major script " + _PatchDistributionMatchModel.Message);
                        return;
                    }
                    if (MessageBox.Show($"{_PatchDistributionMatchModel.Message}  to continue Execute minor script Of Old  MajorVersion ?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    UpdateDBEnabled = true;
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.OK_DBAndAssemblyREqual:
                    
                    var patchDistributionList = _PatchDistributionManager.GetPatchDistribution_MinorNotClosed(_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion);
                    if (patchDistributionList.Count == 0)
                    {
                        MessageBox.Show(
                            //"Nothing TODO- OK_DB And Assembly R Equal  MajorVersion+MinorVersion "
                            "אין צורך בעידכונים- יש תאימות בן גרסת סכמת מאגר הנתונים לגרסת התוכנה"
                            );
                        return;
                    }
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Menu >> Start >  Doit !!!");
                    _PatchDistributionList = patchDistributionList;

                    UpdateDBEnabled = true;
                    break;
                case PatchDistributionMatch.MajorVersionMatchEnum.NotDistributionBranch:
                case PatchDistributionMatch.MajorVersionMatchEnum.OldSource:
                default:
                    MessageBox.Show(_PatchDistributionMatchModel.Message ?? " Nothing i can do ");
                    break;
            }



            


        }

        private void buttonUpdateDB_Click(object sender, EventArgs e)
        {
            try
            {
                _PatchDistributionManager.Exec(_PatchDistributionMatchModel.LastClosed_DBMigration.MajorVersion, _PatchDistributionMatchModel.LastClosed_DBMigration.MinorVersion,
                    (mess)=>
                {
                    MessageBox.Show(mess);
                }
                    );
                UpdateDBEnabled = false;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"End!!!!!!!!!!!!!!!!!");
                MessageBox.Show("Please don't forget to recycle the IIS");
            }
            catch (PatchDistributionException myPatchDistributionException)
            {
                UpdateDBEnabled = false;
                ApproveEnabled = true;
                _MyPatchDistributionException = myPatchDistributionException;
                MessageBox.Show(myPatchDistributionException.ToString());
            }
            catch (Exception ee)
            {
                UpdateDBEnabled = false;
                MessageBox.Show(ee.ToString());
            }
        }

        private void buttonApproveLastFailure_Click(object sender, EventArgs e)
        {
            if (_MyPatchDistributionException == null)
            {
                MessageBox.Show("_MyPatchDistributionException == null");
            }
            if(String.IsNullOrWhiteSpace(textBoxApproveRemarks.Text))
            {
                MessageBox.Show("נא הכנס הערה- מדוע אתה מאשר את הנפילה האחורנה");
                buttonApproveLastFailure.Focus();
                return;
            }
            string approveRemarks = textBoxApproveRemarks.Text;
            try
            {
                _PatchDistributionManager.ApproveLastFailure(_MyPatchDistributionException, approveRemarks);
            }
            catch (Exception ee)
            {
                ApproveEnabled = false;
               NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ee);
                MessageBox.Show(ee.ToString());
                return;
            }
            textBoxApproveRemarks.Text = "";
            ApproveEnabled = false;
            UpdateDBEnabled = true;

        }
    }

    public class TODELETE_MyTraceListener : TraceListener
    {
        private TextBoxBase output;

        public TODELETE_MyTraceListener(TextBoxBase output)
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