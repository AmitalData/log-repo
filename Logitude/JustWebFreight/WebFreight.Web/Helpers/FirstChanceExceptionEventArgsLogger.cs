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
		public static void LogException(FirstChanceExceptionEventArgs args)
		{
			//if (args.Exception != null)
			//{
			//	string WorkingDir = Path.GetDirectoryName(Application.ExecutablePath);
			//	using (StreamWriter w = File.AppendAllText(WorkingDir + "exceptionslog.txt"))
			//	{

			//		string message = args.Exception.Source + " " + args.Exception.Message + " " + args.Exception.StackTrace;
			//		Log(message, w);

			//		w.Close();
			//	}
			//}
		

		}

		private static void Log(string logMessage, TextWriter w)
		{
			w.Write("\r\nLog Entry : ");
			w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
			//w.WriteLine("  :");
			w.WriteLine($"  :{logMessage}");
			w.WriteLine("-------------------------------");
		}
	}
}