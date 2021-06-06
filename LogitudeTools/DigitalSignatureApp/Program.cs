using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Cloud.Sign.App
{
    /// <summary>
    /// 
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            String thisprocessname = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().ToList().Count(p => p.ProcessName == thisprocessname) > 1)
            {
                MessageBox.Show("Sign app is already running");
                return;
            } 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}