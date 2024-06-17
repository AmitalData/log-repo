using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    public static class Logger
    {
        static Thread writeLogLoop = null;
        static Dictionary<string, string> suffixs = null;

        private static void InitNlogConfig()
        {

        }
        delegate DialogResult Show(string text, string caption);
        static ConcurrentQueue<Tuple<string, bool, string>> cq = null;
        static Dictionary<string, Dictionary<string, StreamWriter>> dicStream = null;
        static DateTime lastOldStreamCheck;
        static Logger()
        {
            lastOldStreamCheck = DateTime.Now;
            dicStream = new Dictionary<string, Dictionary<string, StreamWriter>>();
            suffixs = new Dictionary<string, string>();
            cq = new ConcurrentQueue<Tuple<string, bool, string>>();
            writeLogLoop = new Thread(new ParameterizedThreadStart(ManagerThreadLoop));
            writeLogLoop.Start();
        }

        static void ManagerThreadLoop(object threadParam)
        {
            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {
                try
                {
                    Tuple<string, bool, string> item = null;
                    if (cq.TryDequeue(out item))
                    {
                        string prefix = ValidFileName(item.Item3);
                        InitWorkingDir();
                        if (item.Item2)
                        {
                            LogError(item.Item1, prefix);
                        }
                        else
                        {
                            LogMessage(item.Item1, prefix);
                        }
                    }
                    CloseOldStream();
                    Thread.Sleep(10);
                }
                catch { }
            }

        }

        public static void LogMe(string mess, bool Error)
        {
            LogMe(mess, Error, "");
        }
        private static StringBuilder _SBUIErrorBuffer = new StringBuilder();
        private static DateTime UIErrorBufferAt;

        public static void LogMe(string mess, bool Error, string suffix)
        {
            try
            {
                mess = $"[{DateTime.Now.ToString("G")}]{mess}";
                if (!Error)
                {
                    if (string.IsNullOrEmpty(suffix)) suffix = "Mess";
                    cq.Enqueue(new Tuple<string, bool, string>(mess, false, suffix));
                }
                else
                {
                    cq.Enqueue(new Tuple<string, bool, string>(mess, true, ""));
                    if (string.IsNullOrEmpty(suffix)) suffix = "Mess";
                    cq.Enqueue(new Tuple<string, bool, string>(mess, false, suffix));
                    bool Send = false;
                    if (UIErrorBufferAt == DateTime.MinValue)
                        Send = true;
                    else if (DateTime.Now.Subtract(UIErrorBufferAt) > new TimeSpan(1, 0, 0))
                        Send = true;
                    _SBUIErrorBuffer.AppendLine(mess);
                    #region Send Email
                    if (Send)
                    {
                        if (System.Environment.UserInteractive)
                        {
                            Show myDel = new Show(System.Windows.Forms.MessageBox.Show);
                            myDel.BeginInvoke(_SBUIErrorBuffer.ToString(), ValidFileName(ValidFileName(Application.ProductName)), null, null);
                            //System.Windows.Forms.MessageBox.Show(m);

                        }
                        SMTP.SendItdelegate SendItP = new SMTP.SendItdelegate(SMTP.SendItDefault);
                        SendItP.BeginInvoke(_SBUIErrorBuffer.ToString(), null, null);
                        _SBUIErrorBuffer = new StringBuilder();
                        UIErrorBufferAt = DateTime.Now;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

      

        private delegate void LogMeDelegate(string mess, string suffix);
        private static DateTime LastDelOldAt = DateTime.MinValue;
        private static void InitWorkingDir()
        {
            try
            {

                if (WorkingDir == "" ||
                 (DateTime.Now.Subtract(LastDelOldAt) > new TimeSpan(12, 0, 0))
                 )
                {
                    try
                    {
                        //WorkingDir = System.Configuration.ConfigurationSettings.AppSettings["WorkingDir"].ToString();
                        //WorkingDir = Path.GetDirectoryName(Application.ExecutablePath);


                        if (string.IsNullOrEmpty(OverrideExecutablePath))
                        {

                            WorkingDir = Path.GetDirectoryName(Application.ExecutablePath);
                        }
                        else
                        {
                            WorkingDir = OverrideExecutablePath;
                        }

                    }
                    catch
                    {
                        //WorkingDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                    }
                    LastDelOldAt = DateTime.Now;
                    var Machine_User = ValidFileName(Environment.MachineName + "_" + Environment.UserName);
                    WorkingDir = WorkingDir + @"\LogService_" + Machine_User + @"\";
                    if (!Directory.Exists(WorkingDir))
                    {
                        Directory.CreateDirectory(WorkingDir);
                    }
                    string fileName = "";
                    DateTime d;
                    string[] files = Directory.GetFiles(WorkingDir, ValidFileName(Application.ProductName) + ".*.Log", SearchOption.TopDirectoryOnly);
                    FileInfo fi;
                    if (files != null)
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            fi = new FileInfo(files[i]);
                            if (fi.LastWriteTime < DateTime.Today.AddDays(-7))
                            {
                                fi.Delete();
                            }
                            else
                            {
                                int LoggerFileSizeLimitInMB = GetLimitInMB();
                                if (fi.Length > (1048576 * LoggerFileSizeLimitInMB))
                                {
                                    fi.Delete();
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
            }
        }
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
                var sLoggerFileSizeLimitInMB = System.Configuration.ConfigurationSettings.AppSettings["Logger.FileSizeLimitInMB"];
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

        private static void LogError(string mess, string suffix)
        {

            //if (suffix != "") // log again in Main Error File
            //{
            //    LogMeDelegate logMe = new LogMeDelegate(LogError);
            //    logMe.BeginInvoke(new StringBuilder().Append(suffix).AppendFormat("==> ").AppendLine(mess).ToString(), "", null, null);
            //}
            //string suffixFile = "Error.Log";
            //if (suffix != "") suffixFile = "Error." + suffix + ".Log";
            //lock (typeof(Logger))
            //{
            //    InitWorkingDir();
            //    using (StreamWriter sw = File.AppendText(WorkingDir + ValidFileName(Application.ProductName) + "." + DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day + "." + suffixFile))
            //    {
            //        if (String.IsNullOrWhiteSpace(suffix))
            //        {
            //            sw.WriteLine("<<==" + DateTime.Now.ToLocalTime());
            //        }
            //        sw.WriteLine(mess);
            //    }
            //}
            StreamWriter sw = GetStreamWriter(true, suffix);
            if (String.IsNullOrWhiteSpace(suffix))
            {
                sw.WriteLine("<<==" + DateTime.Now.ToLocalTime());
            }
            sw.WriteLine(mess);
            sw.Flush();
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
                lock (typeof(Logger))
                {
                    InitWorkingDir();
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
                LogMe(e.ToString(), true);
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
                lock (typeof(Logger))
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
                lock (typeof(Logger))
                {
                    InitWorkingDir();
                    using (StreamWriter sw = File.CreateText(WorkingDir + ValidFileName(Application.ProductName) + "." + suffixFile))
                    {
                        sw.WriteLine(mess);
                    }
                }
            }
            catch (Exception e)
            {
                LogMe(e.ToString(), true);
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
                        Debug.WriteLine(ex.ToString());
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



