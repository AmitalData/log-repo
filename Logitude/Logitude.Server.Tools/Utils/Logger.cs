using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        public static readonly NLog.Logger NLogger = NLog.LogManager.GetLogger("AmitalLogger");

        static Thread writeLogLoop = null;
        static Dictionary<string, string> suffixs = null;
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

            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config"));
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

                    LogInfo(mess + " " + suffix);
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

                    LogError(mess + " " + suffix);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private static void LogMe2(string mess, bool Error, string suffix)
        {
            suffix = ValidFileName(suffix);

            LogMeDelegate logMe;
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(mess);
            }
            if (Error)
            {
                logMe = new LogMeDelegate(LogError);

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
            else
            {
                logMe = new LogMeDelegate(LogMessage);

            }
            logMe.BeginInvoke(mess, suffix, null, null);

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
                    bool tolog= (DateTime.Now < stopLogAt);
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

        #region Nlog functions

        public static void LogDebug(string mess, params object[] args)
        {
            LogDebug(null, mess, args);
        }
        public static void LogDebug(Exception ex, string mess, params object[] args)
        {

            LogToNlog(NLog.LogLevel.Debug, ex, mess, args);
        }

        public static void LogInfo(string mess, params object[] args)
        {
            LogInfo(null, mess, args);
        }
        public static void LogInfo(Exception ex, string mess, params object[] args)
        {

            LogToNlog(NLog.LogLevel.Info, ex, mess, args);
        }

        public static void LogWarn(string mess, params object[] args)
        {
            LogWarn(null, mess, args);
        }
        public static void LogWarn(Exception ex, string mess, params object[] args)
        {

            LogToNlog(NLog.LogLevel.Warn, ex, mess, args);
        }

        public static void LogError(string mess, params object[] args)
        {
            LogError(null, mess, args);
        }
        public static void LogError(Exception ex, string mess, params object[] args)
        {

            LogToNlog(NLog.LogLevel.Error, ex, mess, args);
        }

        public static void LogFatal(string mess, params object[] args)
        {
            LogFatal(null, mess, args);
        }
        public static void LogFatal(Exception ex, string mess, params object[] args)
        {

            LogToNlog(NLog.LogLevel.Fatal, ex, mess, args);
        }

        public static void LogTrace(string mess, params object[] args)
        {
            LogTrace(null, mess, args);
        }
        public static void LogTrace(Exception ex, string mess, params object[] args)
        {

            LogToNlog(NLog.LogLevel.Trace, ex, mess, args);
        }


        private static void LogToNlog(NLog.LogLevel level, Exception exception, string mess, params object[] args)
        {

            switch (level.Ordinal)
            {
                case 1:// NLog.LogLevel.Debug:
                    NLogger.Debug(exception, mess, args);
                    break;
                case 2:// NLog.LogLevel.Info:
                    NLogger.Info(exception, mess, args);
                    break;
                case 3:// NLog.LogLevel.Warn:
                    NLogger.Warn(exception, mess, args);
                    break;
                case 4: // NLog.LogLevel.Error:
                    NLogger.Error(exception, mess, args);
                    break;
                case 5:// NLog.LogLevel.Fatal:                       
                    NLogger.Fatal(exception, mess, args);
                    break;

                case 0:
                    NLogger.Trace(exception, mess, args);
                    break;


                default:
                    break;
            }


        }

        #endregion
    }
}
