using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.Resolvers;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            if (System.Environment.CommandLine.EndsWith("/UpdateRatesByExternalXmlForAllTenantWithSchedular", StringComparison.OrdinalIgnoreCase))
            {
                UpdateAllRates();
                return;
            }

            if (System.Environment.CommandLine.EndsWith("/JenkinsAccountingUpdate", StringComparison.OrdinalIgnoreCase))
            {
                new Form1().UpdateModule(0, "accounting", new Label());
                return;
            } 

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        private static void UpdateAllRates()
        {
            try
            {
                Console.WriteLine("UpdateAllRates" + DateTime.Now.ToString());
                Console.WriteLine(System.Environment.CommandLine);
                Console.WriteLine("Form1.LoadLogitudeSettings()" + DateTime.Now.ToString());
                Form1.LoadLogitudeSettings();
                var listofTenants = Form1.GetTenantListThatHasTaskScheduler();
                foreach (var tenant in listofTenants)
                {
                    Console.WriteLine("update tenant:" + tenant);
                    Form1.UpdateRatesByExternalXmlForAllTenantWithSchedular(tenant);
                }
                Console.WriteLine("End:" + DateTime.Now.ToString());
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                //throw e;
                Environment.Exit(-1);
            }
        }
        private static void JenkinsCustomUpdate()
        {
            try
            {
                //var sw = Stopwatch.StartNew();
                //AllocConsole();
                Console.WriteLine("JenkinsCustomUpdate()" + DateTime.Now.ToString());
                ///throw new Exception("Test Exception");
                Console.WriteLine(System.Environment.CommandLine);
                ///throw new Exception("JenkinsCustomUpdate throw ");
                Console.WriteLine("Form1.LoadLogitudeSettings()" + DateTime.Now.ToString());
                Form1.LoadLogitudeSettings();
                if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                {
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["Oracle_Globalstr"].ConnectionString);
                }

                Console.WriteLine("UpdateDataForTenant(customs):" + DateTime.Now.ToString());
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.UpdateDataForTenant(0, "customs");
                Console.WriteLine("Build ObjectTables Zip Files Data for customs:" + DateTime.Now.ToString());
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData(false, true);
                Console.WriteLine("Build ObjectTables Zip Files Data:" + DateTime.Now.ToString());
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData(false, false);

                Console.WriteLine("TableLastUpdateClass.UpdateCacheTableHistory:" + DateTime.Now.ToString());
                BL.Helpers.TableLastUpdateClass.UpdateCacheTableHistory(0);
                Console.WriteLine("TenantsUpdateClass.UpdateTenants:" + DateTime.Now.ToString());
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.UpdateTenants();
                Console.WriteLine("End:" + DateTime.Now.ToString());
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
