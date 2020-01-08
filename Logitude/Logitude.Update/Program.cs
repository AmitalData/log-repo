using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Environment.CommandLine.EndsWith("/JenkinsCustomUpdate", StringComparison.OrdinalIgnoreCase))
            {
                JenkinsCustomUpdate();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void JenkinsCustomUpdate()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                //AllocConsole();
                Console.WriteLine(System.Environment.CommandLine);
                ///throw new Exception("JenkinsCustomUpdate throw ");
                Form1.LoadLogitudeSettings();
                Console.WriteLine("UpdateDataForTenant(customs):" + DateTime.Now.ToString() );
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.UpdateDataForTenant(0, "customs");
                Console.WriteLine("BuildObjectTablesZipFilesData:" + DateTime.Now.ToString());
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData(false, true);
                Console.WriteLine("End:" + DateTime.Now.ToString();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                //throw e;
                Environment.Exit(-1);
            }

        }
    }
}
