using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Logitude.Accounting.BL.Utils
{
    public static class TODELETE_AccountingLogger
    {

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
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(mess);
            if (Error)
            {
                logMe = new LogMeDelegate(LogError);

                bool Send = false;
                if (UIErrorBufferAt == DateTime.MinValue)
                    Send = true;
                else if (DateTime.Now.Subtract(UIErrorBufferAt) > new TimeSpan(1, 0, 0))
                    Send = true;
                UIErrorBuffer += mess + Environment.NewLine;
                //if (Send)
                //{
                //    if (System.Environment.UserInteractive)
                //    {
                //        Show myDel = new Show(System.Windows.Forms.MessageBox.Show);
                //        myDel.BeginInvoke(UIErrorBuffer, ValidFileName(ValidFileName(Application.ProductName)), null, null);
                //        //System.Windows.Forms.MessageBox.Show(m);

                //    }
                //    SMTP.SendItdelegate SendItP = new SMTP.SendItdelegate(SMTP.SendItDefault);
                //    SendItP.BeginInvoke(UIErrorBuffer, null, null);
                //    UIErrorBuffer = "";
                //    UIErrorBufferAt = DateTime.Now;
                //}
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
                        //WorkingDir = ConfigurationManager.AppSettings["WorkingDir"].ToString();
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
                    WorkingDir = WorkingDir + @"\AccountingLogService\";
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
                            if (fi.LastWriteTime < DateTime.Today.AddDays(-3))
                            {
                                fi.Delete();
                            }
                            else if (fi.Length > (1048576 * 30))
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
            lock (typeof(TODELETE_AccountingLogger))
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


            lock (typeof(TODELETE_AccountingLogger))
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
                lock (typeof(TODELETE_AccountingLogger))
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
                lock (typeof(TODELETE_AccountingLogger))
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
    }
}
