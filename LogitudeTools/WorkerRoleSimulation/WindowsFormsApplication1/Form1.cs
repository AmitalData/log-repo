using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //EventLog.WriteEntry("worker_DoWork", "Start Point");
                CommunicationWorkerRole.ThreadedRoleEntryPoint d = new CommunicationWorkerRole.ThreadedRoleEntryPoint();
                d.OnStart();
                d.Run();
            }
            catch (Exception exception)
            {
                string ErrorMessage = "";

                ErrorMessage += exception.Message;

                if (exception.InnerException != null)
                {
                    ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                    if (exception.InnerException.InnerException != null)
                    {
                        ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                        if (exception.InnerException.InnerException.InnerException != null)
                        {
                            ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                        }
                    }

                    //EventLog.WriteEntry("worker_DoWork", ErrorMessage);
                    //if (exception.InnerException.Message.Contains("Physical connection is not usable"))
                    //{
                    //    SqlConnection.ClearAllPools();
                    //}
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            this.Close();
            Application.Exit();
        }

        
    }
}
