using CommunicationWorkerRole.Stimulsoft.fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            if (!string.IsNullOrEmpty(txtWorkerRoleName.Text) && 
                !string.IsNullOrEmpty(txtStatusCode.Text))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                
                string filePath = Path.Combine(directoryInfo.FullName, "WorkerRoleName.xml");
                string oldFilePath = Path.Combine(directoryInfo.FullName, "WorkerRoleName" + DateTime.Now.ToString("yyyyMMdd") + ".xml");
                if (File.Exists(filePath) && !File.Exists(oldFilePath))
                { 
                    File.Move(filePath, Path.Combine(directoryInfo.FullName, "WorkerRoleName" + DateTime.Now.ToString("yyyyMMdd") + ".xml"));
                    Thread.Sleep(20);
                }

                var sb=new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.Append("<root>");
                sb.Append("<WorkerName>");
                sb.Append(txtStatusCode.Text);
                sb.Append("</WorkerName>");
                sb.Append("<ActiveWorkers>");
                sb.Append(txtWorkerRoleName.Text);
                sb.Append("</ActiveWorkers>");
              
                sb.Append("</root>");
                File.WriteAllText(filePath, sb.ToString());
                Thread.Sleep(20);

            }
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

                    throw new Exception(ErrorMessage);
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
