using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class LogMessagingUtil
    {
        [ThreadStatic]
        private static LogMessagingUtil _Instance;
        public static LogMessagingUtil Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LogMessagingUtil();
                }
                return _Instance;
            }

        }
        StringBuilder _StringBuilder;
        private int _Max;
        public bool LogMessaging { get; private set; }

        //public static LogMessagingUtil Instance { get; private set; }



        LogMessagingUtil()
        {
            if (Environment.UserDomainName.Equals("ntdomain", StringComparison.OrdinalIgnoreCase))
            {
                LogMessaging = true;
            }
            LogMessaging = true;//default yes yes yes !!!
            _StringBuilder = new StringBuilder();
        }
        public bool ToggleLogMessaging()
        {
            LogMessaging = !LogMessaging;
            return LogMessaging;
        }

        public LogMessagingUtil AppendLine(string value)
        {
            if (value.Length > 2048)
            {
                value = "<<<Truncate" + value.Substring(0, 2048) + "Truncate>>>";
            }
            Debug.WriteLine(value);
            if (!LogMessaging) return this;
            
            if (_StringBuilder.Length > _Max) return this;
            _StringBuilder.AppendLine(value);
            return this;
        }
        public LogMessagingUtil Append(object value)
        {
            Debug.Write(value);
            if (!LogMessaging) return this;
            if (_StringBuilder.Length > _Max) return this;
            _StringBuilder.Append(value);
            return this;
        }
        public void Clear(int max = 10000)
        {
            _Max = max;
            if (!LogMessaging) return;
            _StringBuilder.Clear();
        }
        public override string ToString()
        {
            return _StringBuilder.ToString();
        }
        public LogMessagingUtil LogActionTime(Action myAction,
            string ActionName = ""
            , [CallerMemberName] string myCallerMemberName = ""
            , [CallerFilePath] string myCallerFilePath = ""
            , [CallerLineNumber] int myCallerLineNumber = 0
            )
        {
            string err = "";
            var sw = Stopwatch.StartNew();
            try
            {
                myAction();
            }
            catch (Exception ee)
            {
                err = ee.ToString();
                throw;
            }

            finally
            {

                if (string.IsNullOrWhiteSpace(ActionName))
                {
                    this.Append(ActionName);
                }
                else
                {
                    this.Append(myCallerFilePath).Append(":").Append(myCallerMemberName).Append("+").Append(myCallerLineNumber);
                }
                this.Append(":took:").Append(sw);


            }
            return this;
        }
        public string GetLastChars(int maxLength)
        {


            return _StringBuilder.ToString().GetLast(maxLength);
        }
        public string ToString(int maxLength)
        {
            if (_StringBuilder.Length < maxLength)
            {
                return ToString();
            }
            return _StringBuilder.ToString().Substring(0, maxLength - 1);
            //return _StringBuilder.ToString().Substring(_StringBuilder.Length - maxLength);

        }
    }
    public static class StringExt
    {
        public static string GetLast(this string myString, int maxLength)
        {
            myString = myString ?? "";
            var len = myString.Length;
            if (len < maxLength)
            {
                return myString;
            }
            //return _StringBuilder.ToString().Substring(0, maxLength - 1);
            return myString.Substring(len - maxLength);

        }
    }


    public class AssemblyUtil
    {
        public string GetVersion(string ProductInfo)
        {
            var parts = ProductInfo.Split(new char[] { ',' });
            return parts[0].Split(new char[] { ':' })[0];

        }
        public string BurnAt(string ProductInfo)
        {
            var parts = ProductInfo.Split(new char[] { ',' });
            return parts[0].Split(new char[] { ':' })[1];

        }
        public string GetProductInfo(Assembly assembly)
        {
            string prodInfo = "";
            try
            {


                var d111 = System.IO.File.GetLastWriteTime(assembly.Location);
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fileVersionInfo.ProductVersion;
                prodInfo = "Version:" + version + ",Burn At:" + d111.ToString();
            }
            catch (Exception)
            {


            }
            return prodInfo;
        }
    }
}
