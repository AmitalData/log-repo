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
    public class LogChangesService_custom
    {
        StringBuilder sb = new StringBuilder();
        string AppSettingKeyValueIsLogUntilDateyyyyMMdd = "";
        bool IsDateValid = true;
        public LogChangesService_custom(string appSettingKeyValueIsLogUntilDateyyyyMMdd)
        {
            var untilDateyyyyMMdd = ConfigurationManager.AppSettings[appSettingKeyValueIsLogUntilDateyyyyMMdd];//"2018062018HD312280.LogUntilDateyyyyMMdd"];
            AppSettingKeyValueIsLogUntilDateyyyyMMdd = appSettingKeyValueIsLogUntilDateyyyyMMdd;
            if (string.IsNullOrWhiteSpace(untilDateyyyyMMdd))
            {
                IsDateValid = false;
            }
            else
            {
                DateTime stopLogAt = DateTime.MinValue;
                stopLogAt = DateTime.ParseExact(untilDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                if (DateTime.Now > stopLogAt)
                {
                    IsDateValid = false;
                }
            }
        }
        //public void AddDataToLog<POCOType>(POCOType entityPOCO)
        //{
        //    try
        //    {
        //        if (!IsDateValid) return;
                
        //        string jsonPOCO = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPOCO);
                
        //        sb
        //            .AppendLine("**Stack:")
        //            .AppendLine(Environment.StackTrace)
        //            .AppendLine("**POCO:old:")
        //            .AppendLine(jsonPOCO);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //}
        public void AddTextToLog(string text)
        {
            try
            {
                if (!IsDateValid) return;
                sb.AppendLine(text);
            }
            catch (Exception)
            {

            }
        }
        public void LogIt()
        {
            if (!IsDateValid) return;

            LogitudeSettings.HandleLogMe(sb.ToString(), false, AppSettingKeyValueIsLogUntilDateyyyyMMdd, DateTime.MaxValue);
        }
    }
}
