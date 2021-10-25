using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Dca
{
    public class DedicatedCourierDCAService
    {

        public DedicatedCourierDCAModel CreateDedicatedCourierDCA()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("DedicatedCourierDCA:Tenant")))//Task 147744: AMITALCUSTOMSSERVER העברת הטיפול בכספת בבלדרות לתהליך
            {


                Logger.LogMe("DedicatedCourierDCA:Tenant", false);
                int t = -99;
                int.TryParse(ConfigurationManager.AppSettings.Get("DedicatedCourierDCA:Tenant"), out t);
                if (t == -99)
                {
                    throw new Exception(@"DedicatedCourierDCA:Tenant please insert tenant!!!!!  <add key=""DedicatedCourierDCA:Tenant"" value=""1"" /> ");
                }
                Logger.LogMe($"DedicatedCourierDCA:Tenant={t}", false);

                string backupPath = ConfigurationManager.AppSettings.Get("DedicatedCourierDCA:BackupPath");
                if (string.IsNullOrWhiteSpace(backupPath))
                {
                    throw new Exception(@"DedicatedCourierDCA:BackupPath please insert  BackupPath <add key=""DedicatedCourierDCA:BackupPath"" value=""C:\CyberArk_DCA\GLO-il550221105\Download\GLO\UDCABackupOrphaned"" /> ");
                }
                Logger.LogMe($"DedicatedCourierDCA:BackupPath={backupPath}", false);
                if (!Directory.Exists(backupPath))
                {
                    throw new Exception($"DedicatedCourierDCA:BackupPath please create backupPath !!!  {backupPath} ");
                }
                bool UseTPL = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("DedicatedCourierDCA:UseTPL"));
                Logger.LogMe($"DedicatedCourierDCA:UseTPL={UseTPL}", false);

                return new DedicatedCourierDCAModel()
                {
                    Tenant = t,
                    BackupPath = backupPath,
                    UseTPL = UseTPL
                };
            }
            return null;
        }
    }
    public class DedicatedCourierDCAModel
    {
        public int Tenant { get; set; }
        public string BackupPath { get; set; }
        public bool UseTPL { get; set; }
    }
}
