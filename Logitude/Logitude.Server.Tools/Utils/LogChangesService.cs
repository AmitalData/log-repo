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
        public void LogIt<PMType, POCOType>(
            string appSettingKeyValueIsLogUntilDateyyyyMMdd,
            PMType entityPM, POCOType entityPOCO)
        {

            try
            {
                if (!IsLogEnable(appSettingKeyValueIsLogUntilDateyyyyMMdd))
                {
                    return;
                }


                string jsonPM = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPM);
                string jsonPOCO = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPOCO);
                string resolveUserIdentityName = GetUserIdentityName();
                string trans = GetTransaction();
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

        public void LogIt(
            string appSettingKeyValueIsLogUntilDateyyyyMMdd,
            StringBuilder stringBuilder
            )
        {
            try
            {
                if (!IsLogEnable(appSettingKeyValueIsLogUntilDateyyyyMMdd))
                {
                    return;
                }
                string resolveUserIdentityName = GetUserIdentityName();
                string trans = GetTransaction();
                stringBuilder
                    .AppendLine("Transaction.Current:")
                    .AppendLine(trans)

                    .AppendLine("ResolveUserIdentityName:")
                    .AppendLine(resolveUserIdentityName)
                    .AppendLine("**Stack:")
                    .AppendLine(Environment.StackTrace)
                    .AppendLine();

                LogitudeSettings.HandleLogMe
                    (stringBuilder.ToString(), false, appSettingKeyValueIsLogUntilDateyyyyMMdd, DateTime.MaxValue);

            }
            catch (Exception)
            {

                //throw;
            }
        }

        private static string GetTransaction()
        {
            string trans = "-999";
            if (Transaction.Current != null)
            {
                trans = Transaction.Current.GetHashCode().ToString();
            }

            return trans;
        }

        private static string GetUserIdentityName()
        {
            var resolveUserIdentityName = "";
            try
            {
                resolveUserIdentityName = AuthenticationUtil.ResolveUserIdentityName(0);
            }
            catch
            {

            }

            return resolveUserIdentityName;
        }


        private static bool IsLogEnable(string appSettingKeyValueIsLogUntilDateyyyyMMdd)
        {
            try
            {


                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings[appSettingKeyValueIsLogUntilDateyyyyMMdd];//"2018062018HD312280.LogUntilDateyyyyMMdd"];
                if (string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    return false;
                }

                DateTime stopLogAt = DateTime.MinValue; //new DateTime(2018, 02, 20);
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);

                if (DateTime.Now > stopLogAt)
                {
                    return false;
                }
                return true;
            }
            catch ///(Exception)
            {

                //throw;
                return false;
            }
        }
    }
}
