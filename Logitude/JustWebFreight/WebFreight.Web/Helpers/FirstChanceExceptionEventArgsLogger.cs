using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Windows.Forms;
using System.Xml;
 
namespace WebFreight.Web.Helpers
{
	public class FirstChanceExceptionEventArgsLogger
	{
		private static string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
		public static void LogException(FirstChanceExceptionEventArgs args)
		{
			try
			{
				if (args.Exception != null && args.Exception.Message != null && !args.Exception.Message.Contains("exceptionslogfile.txt"))
				{
					//string WorkingDir = Path.GetDirectoryName(Application.ExecutablePath);
					
					//filePath += "\\bin\\";
					//string fileName = "exceptionslogfile.txt";
					//filePath += fileName;
					using (StreamWriter w = File.AppendText(filePath))
					{

						string message = args.Exception.Source + " " + args.Exception.Message + " " + args.Exception.StackTrace;
						Log(message, w);

						w.Close();
					}
				}
			}
			catch { }

		}

		private static void Log(string logMessage, TextWriter w)
		{
			string logEntry = DateTime.Now.ToString()+ " "+ logMessage;
			w.WriteLine(logEntry);
			//w.Write("\r\nLog Entry : ");
			//w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
			////w.WriteLine("  :");
			//w.WriteLine($"  :{logMessage}");
			//w.WriteLine("-------------------------------");
		}
	}
}