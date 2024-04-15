using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Logitude.Server.Tools.Utils
{
    public static class Logger
    {
        public static readonly NLog.Logger NLogger = NLog.LogManager.GetLogger("AmitalLogger");

        private static void InitNlogConfig()
        {
            if (NLog.LogManager.Configuration == null)
               NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config"));

        }

        delegate DialogResult Show(string text, string caption);
        public static void LogMe(string mess, bool Error)
        {
            LogMe(mess, Error, "");
        }
        private static string UIErrorBuffer;
        private static DateTime UIErrorBufferAt;
        public static void LogMe(string mess, bool Error, string suffix)
        {
            suffix = ValidFileName(suffix);
            LogMeDelegate logMe;
            Debug.WriteLine(mess);

            if (Error)
            {
                logMe = new LogMeDelegate(LogError);

                bool Send = false;
                if (UIErrorBufferAt == DateTime.MinValue)
                    Send = true;
                else if (DateTime.Now.Subtract(UIErrorBufferAt) > new TimeSpan(1, 0, 0))
                    Send = true;
                UIErrorBuffer += mess + Environment.NewLine;
                if (Send)
                {
                    if (System.Environment.UserInteractive)
                    {
                        Show myDel = new Show(System.Windows.Forms.MessageBox.Show);
                        myDel.BeginInvoke(UIErrorBuffer, ValidFileName(ValidFileName(Application.ProductName)), null, null);
                        //System.Windows.Forms.MessageBox.Show(m);

                    }
                    SMTP.SendItdelegate SendItP = new SMTP.SendItdelegate(SMTP.SendItDefault);
                    SendItP.BeginInvoke(UIErrorBuffer, null, null);
                    UIErrorBuffer = "";
                    UIErrorBufferAt = DateTime.Now;
                }

                LogError(mess + " " + suffix);
            }
            else
            {
                logMe = new LogMeDelegate(LogMessage);

                LogInfo(mess + " " + suffix);

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
                    WorkingDir = WorkingDir + @"\LogService\";
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
                            var day2delete = -14;var maxLengthInMB = 50;// ihab +itzik
                            if (fi.LastWriteTime < DateTime.Today.AddDays(day2delete))
                            {
                                fi.Delete();
                            }
                            else if (fi.Length > (1048576 * maxLengthInMB))
                            {
                                fi.Delete();
                            }
                        }
                    }
                }

            }
            catch
            {
            }
        }

        private static void LogError(string mess, string suffix)
        {

            if (suffix != "") // log again in Main Error File
            {
                LogMeDelegate logMe = new LogMeDelegate(LogError);
                logMe.BeginInvoke(suffix + "==> " + mess, "", null, null);
            }
            string suffixFile = "Error.Log";
            if (suffix != "") suffixFile = "Error." + suffix + ".Log";
            lock (typeof(Logger))
            {
                InitWorkingDir();
                using (StreamWriter sw = File.AppendText(WorkingDir + ValidFileName(Application.ProductName) + "." + DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day + "." + suffixFile))
                {
                    sw.WriteLine("<<==" + DateTime.Now.ToLocalTime());
                    sw.WriteLine(mess);
                    //sw.WriteLine("==>>");
                }
                //if (_form == null)
                //{
                //    _form = new frmLogger();
                //    //_form.Show();
                //}
                //_form.LogMess(mess);  

            }
        }
        public static String GetLogMessageFileName(string suffix)
        {
            string suffixFile = "Mess.Log";
            if (suffix != "") suffixFile = "Mess." + ValidFileName(suffix) + ".Log";


            return WorkingDir + ValidFileName(Application.ProductName) + "." + DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day + "." + suffixFile;
        }
        private static void LogMessage(string mess, string suffix)
        {

            if (suffix != "") // log again in 
            {
                suffix = ValidFileName(suffix);
                LogMeDelegate logMe = new LogMeDelegate(LogMessage);
                logMe.BeginInvoke(suffix + "==> " + mess, "", null, null);
            }


            lock (typeof(Logger))
            {
                InitWorkingDir();
                string fn = GetLogMessageFileName(suffix);
                using (StreamWriter sw = File.AppendText(fn))
                {

                    sw.WriteLine("<<==" + DateTime.Now.ToLocalTime());
                    sw.WriteLine(mess);
                    //sw.WriteLine("==>>");
                }
                //if (_form == null)
                //{
                //    _form = new frmLogger();
                //    //_form.Show();
                //}
                //_form.LogMess(mess);  

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
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    FileName = FileName.Replace(c, '.');
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


        public static string WorkingDir = "";
        public static string OverrideExecutablePath = "";

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

            InitNlogConfig();

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

}
