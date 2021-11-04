using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Server.Tools.Utils
{
    public class LogChangesService
    {
        public void LogIt<PMType,POCOType>(
            string appSettingKeyValueIsLogUntilDateyyyyMMdd,
            PMType entityPM, POCOType entityPOCO)
        {

            try
            {

                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings[appSettingKeyValueIsLogUntilDateyyyyMMdd];//"2018062018HD312280.LogUntilDateyyyyMMdd"];
                if (string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    return;
                }

                DateTime stopLogAt = DateTime.MinValue; //new DateTime(2018, 02, 20);
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);

                if (DateTime.Now > stopLogAt)
                {
                    return;
                }




                string jsonPM = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPM);
                string jsonPOCO = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPOCO);
                var resolveUserIdentityName = "";
                try
                {
                    resolveUserIdentityName =AuthenticationUtil.ResolveUserIdentityName(0);
                }
                catch
                {

                }
                string trans = "-999";
                if (Transaction.Current != null)
                {
                    trans = Transaction.Current.GetHashCode().ToString();
                }
                var sb = new StringBuilder();
                sb
                    .AppendLine("Transaction.Current:")
                    .AppendLine(trans)

                    .AppendLine("ResolveUserIdentityName:")
                    .AppendLine(resolveUserIdentityName)
                    .AppendLine("**Stack:")
                    .AppendLine(Environment.StackTrace)
                    .AppendLine("**PM:New:")
                    .AppendLine(jsonPM)
                    .AppendLine("**POCO:old:")
                    .AppendLine(jsonPOCO);

                LogitudeSettings.HandleLogMe
                    //(mess, err, suffix, stopLogAt)
                    (sb.ToString(), false, appSettingKeyValueIsLogUntilDateyyyyMMdd, DateTime.MaxValue);
            }
            catch (Exception eeex)
            {

               
            }

        }
    }
}
