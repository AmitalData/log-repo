using Devart.Data.Oracle;
using Logitude.Customs.BL.PatchDistribution;
using Logitude.Customs.BL.PatchDistribution.Patches;
using Logitude.Customs.Data;
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
        public PatchDistributionForm()
        {
            InitializeComponent();
            //DbContextBaseUtil.ToLog = checkBox1.Checked = true;
            TraceListener debugListener = new MyTraceListener(this.textBoxLogger);
            Debug.Listeners.Add(debugListener);
            var myP19R03_0000_PatchDist = new P19R03_0000_PatchDist( );
            myP19R03_0000_PatchDist.CreateSeedDbMigrateTable();
        }

       

        private void doItToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


      

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DbContextBaseUtil.ToLog = checkBox1.Checked;
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