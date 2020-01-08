using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Environment.CommandLine.EndsWith("/JenkinsCustomUpdate", StringComparison.OrdinalIgnoreCase))
            {
                Form1.LoadLogitudeSettings();
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.UpdateDataForTenant(0, "customs");
                WebFreight.Web.MetaDataUpdate.TenantsUpdateClass.BuildObjectTablesZipFilesData(false, true);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
