using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Cloud.Sign.App.Helpers
{
    public static class LogFileUtil
    {
        /// <summary>
        /// Supported log-types
        /// </summary>
        public enum LogType
        {
            /// <summary>simple text-log</summary>
            TXT,
            /// <summary>xhtml-formatted log</summary>
            XHTML_Plain
        };

        /// <summary>
        /// Supported log-Levels
        /// </summary>
        [Flags]
        public enum LogLevel
        {
            /// <summary>debug-level</summary>
            Debug = 0x01,
            /// <summary>info-level</summary>
            Info = 0x02,
            /// <summary>warn-level</summary>
            Warn = 0x04,
            /// <summary>errorlevel</summary>
            Error = 0x08,
            /// <summary>user-defined level 1</summary>
            User = 0x10,
            /// <summary>all levels</summary>
            All = 0xFF
        };

        /** PRIVATE GLOBAL VARS *****/
        public static string _Filename = "SignAppLogFile_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

        static DateTime date = DateTime.Today;
        private static LogType Type;
        private static LogLevel Level;
        private static LogLevel DefaultLevel;
        public static string text;

        // const string _Filename = "CRMLogFile.xml";


        static string AppDataFolderName = "SignAppLogs";




        public static void LogFileDelete()
        {
            try
            {
                string[] files = Directory.GetFiles(GetAndCreateFolder());

                foreach (string file in files)
                {
                    FileInfo myFileInfo = new FileInfo(file);
                    if (myFileInfo.LastAccessTime < DateTime.Now.AddMonths(-2))
                        myFileInfo.Delete();
                }

            }
            catch (Exception e)
            {
                LogFileUtil.Log("LogFileDelete Failed :" + e.ToString(), LogFileUtil.LogLevel.Debug);
            }
            // var filePath = GetAndCreateFolder();
            // //var filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //File.Exists(filePath+"CRMLogFile_"+ DateTime.Now.ToString("dd-MM-yyyy")+".txt");

            // DateTime Oldtime = DateTime.Now - DateTime.




            //     using System.IO; 


        }




        public static void LogFile(bool append, LogType type, LogLevel level, string title)
        {


            Type = type;
            Level = level;
            DefaultLevel = level;

            LogRaw(title + "   " + DateTime.Now.ToString());

        }



        /// <summary>
        /// Write a single line to log (without Date/Time and Log-Level)
        /// </summary>
        /// <param name="text">text to write</param>
        private static void LogRaw(string text)
        {
            WriteLine(text, true);
        }

        /// <summary>
        /// Write a single line to log with Date/Time. Use the default Log-Level
        /// </summary>
        /// <param name="text">text to write</param>
        private static void Log(string text)
        {
            Log(text, DefaultLevel);
        }

        /// <summary>
        /// Write a single line to log with Date/Time and Log-Level
        /// </summary>
        /// <param name="text">text to write</param>
        /// <param name="level">log-level</param>
        public static void Log(string text, LogLevel level)
        {
            // Check Level
            //if ((level & Level) == 0) return;
            if (level == LogLevel.All) level = DefaultLogLevel;

            // format pre-string
            string prestring;
            switch (level)
            {
                case (LogLevel.Debug): prestring = "DEBUG " + DateTime.Now.ToString() + " "; break;
                case (LogLevel.Info): prestring = "INFO  " + DateTime.Now.ToString() + " "; break;
                case (LogLevel.Warn): prestring = "WARN  " + DateTime.Now.ToString() + " "; break;
                case (LogLevel.Error):
                    {

                        prestring = "ERROR " + DateTime.Now.ToString() + " "; break;
                    }
                case (LogLevel.User): prestring = "USER1 " + DateTime.Now.ToString() + " "; break;

                default: prestring = ""; break;
            }

            // format text depening on type
            string formatted_text;
            switch (Type)
            {
                // HTML_Plain
                case LogType.XHTML_Plain: formatted_text = prestring + text + "<br/>"; break;

                // PLAINTEXT
                default: formatted_text = prestring + text; break;
            }

            // write to file and flush
            WriteLine(formatted_text, true);
        }

        /// <summary>
        /// returns current log-type (read-only)
        /// </summary>
        public static LogType CurrentLogType
        {
            get
            {
                return Type;
            }
        }

        /// <summary>
        /// returns or sets current log-level 
        /// </summary>
        public static LogLevel CurrentLogLevel
        {
            get
            {
                return Level;
            }
            set
            {
                Level = value;
            }
        }

        /// <summary>
        /// returns or sets default log-level 
        /// </summary>
        public static LogLevel DefaultLogLevel
        {
            get
            {
                return DefaultLevel;
            }
            set
            {
                DefaultLevel = value;
            }
        }

        /// <summary>
        /// ProgramLogger Version
        /// </summary>
        public static Version Version
        {
            get
            {
                
                return (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            }
        }




        // write line to file
        private static void WriteLine(string text, bool append)
        {
            // open file
            // If an error occurs throw it to the caller.
            try
            {
                Debug.WriteLine(text);
                StreamWriter Writer;

                var fileNameLocation = GetFileName();
                //var fileNameLocation = Path.GetTempPath() + _Filename;
                if (!File.Exists(fileNameLocation))
                {
                    Writer = new StreamWriter(fileNameLocation, append, Encoding.UTF8);
                }
                else
                {
                    Writer = File.AppendText(fileNameLocation);
                }

                if (text != "") Writer.WriteLine(text);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                throw;
            }
        }


        private static string GetAndCreateFolder()
        {

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = Path.Combine(folder, AppDataFolderName);

            // Check if folder exists and if not, create it
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);


            return specificFolder;


        }

        public static string GetFileName()
        {

            var filePath = GetAndCreateFolder();
            //var filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(filePath, _Filename);
        }





    }
}
