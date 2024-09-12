using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Logitude.Server.Tools.Utils
{
    public static class TODELETE_Logger
    {
      
        static Dictionary<string, string> suffixs = null;



        delegate DialogResult Show(string text, string caption);
        static ConcurrentQueue<Tuple<string, bool, string>> cq = null;
        static Dictionary<string, Dictionary<string, StreamWriter>> dicStream = null;
        static DateTime lastOldStreamCheck;
        static TODELETE_Logger()
        {
            lastOldStreamCheck = DateTime.Now;
            dicStream = new Dictionary<string, Dictionary<string, StreamWriter>>();
            suffixs = new Dictionary<string, string>();
            cq = new ConcurrentQueue<Tuple<string, bool, string>>();
            
        }

      
      
        private static StringBuilder _SBUIErrorBuffer = new StringBuilder();
        private static DateTime UIErrorBufferAt;



      

        private delegate void LogMeDelegate(string mess, string suffix);
        private static DateTime LastDelOldAt = DateTime.MinValue;

      
        private static int? _LoggerFileSizeLimitInMB = null;
        private static int GetLimitInMB()
        {
            if (_LoggerFileSizeLimitInMB.HasValue)
            {
                return _LoggerFileSizeLimitInMB.Value;
            }
            int LoggerFileSizeLimitInMB = 100;
            try
            {
                var sLoggerFileSizeLimitInMB = ConfigurationManager.AppSettings["Logger.FileSizeLimitInMB"];
                if (!String.IsNullOrWhiteSpace(sLoggerFileSizeLimitInMB))
                {
                    LoggerFileSizeLimitInMB = int.Parse(sLoggerFileSizeLimitInMB);
                }
            }
            catch (Exception)
            {

                //throw;
            }
            _LoggerFileSizeLimitInMB = LoggerFileSizeLimitInMB;
            return _LoggerFileSizeLimitInMB.Value;
        }

    
        internal static StreamWriter GetStreamWriter(bool error, string suffix)
        {
            string datestr = DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day;
            if (!dicStream.ContainsKey(datestr))
                dicStream.Add(datestr, new Dictionary<string, StreamWriter>());
            var dic = dicStream[datestr];
            string key = suffix + error;
            if (dic.ContainsKey(key))
                return (dic[key]);
            string suffixFile = "";
            if (error)
            {
                if (suffix != "")
                    suffixFile = "Error." + suffix + ".Log";
                else
                    suffixFile = "Error.Log";
            }
            else
            {
                if (suffix != "")
                    suffixFile = suffix + ".Log";
                else
                    suffixFile = ".Log";
            }
            string filename = WorkingDir + ValidFileName(Application.ProductName) + "." + DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day + "." + suffixFile;
            StreamWriter sw = File.AppendText(filename);
            dic.Add(key, sw);
            return (sw);
        }
        public static String GetLogMessageFileName(string suffix)
        {
            string suffixFile = "Mess.Log";
            if (suffix != "") suffixFile = "Mess." + ValidFileName(suffix) + ".Log";


            return WorkingDir + ValidFileName(Application.ProductName) + "." + DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day + "." + suffixFile;
        }
        private static void LogMessage(string mess, string suffix)
        {
            try
            {
                //if (suffix != "") // log again in 
                //{
                //    suffix = ValidFileName(suffix);
                //    LogMeDelegate logMe = new LogMeDelegate(LogMessage);
                //    logMe.BeginInvoke(new StringBuilder().Append(suffix).Append("==> ").AppendLine(mess).ToString(), "", null, null);
                //}
                //lock (typeof(Logger))
                //{
                //    InitWorkingDir();
                //    string fn = GetLogMessageFileName(suffix);
                //    using (StreamWriter sw = File.AppendText(fn))
                //    {

                //        if (String.IsNullOrWhiteSpace(suffix))
                //        {
                //            sw.WriteLine("<<==" + DateTime.Now.ToLocalTime());
                //        }
                //        sw.WriteLine(mess);
                //    }

                //}
                StreamWriter sw = GetStreamWriter(false, suffix);
                if (String.IsNullOrWhiteSpace(suffix))
                {
                    sw.WriteLine("<<==" + DateTime.Now.ToLocalTime());
                }
                sw.WriteLine(mess);
                sw.Flush();
            }
            catch
            {
            }
        }
        public static void DeleteAllLogState()
        {
            try
            {
                lock (typeof(TODELETE_Logger))
                {
                    string[] files = Directory.GetFiles(WorkingDir, ValidFileName(Application.ProductName) + ".*.State.txt", SearchOption.TopDirectoryOnly);
                    FileInfo fi;
                    if (files != null)
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            fi = new FileInfo(files[i]);
                            fi.Delete();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
            }
        }
        private static string ValidFileName(string FileName)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                    return (string.Empty);
                if (suffixs.ContainsKey(FileName))
                    return (suffixs[FileName]);
                string origFileName = FileName;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    FileName = FileName.Replace(c, '.');
                }
                lock (typeof(TODELETE_Logger))
                {
                    suffixs.Add(origFileName, FileName);
                }
            }
            catch (Exception)
            {
            }
            return FileName;

        }
        public static void LogState(string mess, string Suffix)
        {
            string suffixFile = ValidFileName(Suffix) + ".State.txt";
            try
            {
                lock (typeof(TODELETE_Logger))
                {
                   
                    using (StreamWriter sw = File.CreateText(WorkingDir + ValidFileName(Application.ProductName) + "." + suffixFile))
                    {
                        sw.WriteLine(mess);
                    }
                }
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
            }
        }
        public static void CloseOldStream()
        {
            if (DateTime.Now.AddMinutes(-10) < lastOldStreamCheck && DateTime.Now.Hour >= 1)
                return;
            lastOldStreamCheck = DateTime.Now;
            string datestr = DateTime.Now.AddDays(-1).Year + "." + DateTime.Now.AddDays(-1).Month + "." + DateTime.Now.AddDays(-1).Day;
            if (dicStream.ContainsKey(datestr))
            {
                var dic = dicStream[datestr];
                if (dic == null)
                {
                    dicStream.Remove(datestr);
                    return;
                }
                foreach (var item in dic)
                {
                    try
                    {
                        var sw = item.Value;
                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                       NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                    }
                }
                dicStream.Remove(datestr);
            }

        }


        public static string WorkingDir = "";
        public static string OverrideExecutablePath = "";





        public static bool ToLogUntilDateyyyyMMdd(string appSettingsLogUntilDateyyyyMMdd, DateTime? graceTimeUntill = null)
        {

            try
            {
                if (graceTimeUntill.HasValue &&
                    DateTime.Now < graceTimeUntill.Value)
                {
                    return true;
                }
                string untilDateyyyyMMdd = System.Configuration.ConfigurationManager.AppSettings[appSettingsLogUntilDateyyyyMMdd];
                //["20230112HDCall409236.LogUntilDateyyyyMMdd"];
                if (!string.IsNullOrWhiteSpace(untilDateyyyyMMdd))
                {
                    DateTime stopLogAt = DateTime.ParseExact(untilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        style: DateTimeStyles.None);
                    bool tolog = (DateTime.Now < stopLogAt);
                    return (DateTime.Now < stopLogAt);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }



       

        

        
      
     

       
    }

}



//public static class IQueryableExtensions
//{
//    /// <summary>
//    /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <param name="query"></param>
//    /// <returns></returns>
//    public static string ToTraceQuery<T>(this IQueryable<T> query)
//    {
//        ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

//        var result = objectQuery.ToTraceString();
//        foreach (var parameter in objectQuery.Parameters)
//        {
//            var name = "@" + parameter.Name;
//            var value = parameter.Value is null ? "NULL" : "'" + parameter.Value.ToString() + "'";

//            DateTime dt = new DateTime();
//            if (value != null && value.ToString().Length > 10 && DateTime.TryParse(value.Substring(1,11), out dt))
//            {
//                value = string.Format("cast('{0}' as date)", dt.ToString("yyyy-MM-dd"));
//            }

//            result = result.Replace(name, value);
//        }

//        return result;
//    }

//    /// <summary>
//    /// For an Entity Framework IQueryable, returns the SQL and Parameters.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <param name="query"></param>
//    /// <returns></returns>
//    public static string ToTraceString<T>(this IQueryable<T> query)
//    {
//        ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

//        var traceString = new StringBuilder();

//        traceString.AppendLine(objectQuery.ToTraceString());
//        traceString.AppendLine();

//        foreach (var parameter in objectQuery.Parameters)
//        {
//            traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
//        }

//        return traceString.ToString();
//    }

//    private static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
//    {
//        var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
//        var internalQuery = internalQueryField.GetValue(query);
//        var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
//        return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;
//    }
//}



