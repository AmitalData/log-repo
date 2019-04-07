using Logitude.Server.Tools.StorageService;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;
 
namespace WebFreight.Web.Helpers
{
	public class FirstChanceExceptionEventArgsLogger
	{
		private static string filePath = HttpContext.Current.Server.MapPath(".") + "\\bin\\" + "exceptionslogfile.txt";
		//private static string 
		private static StringBuilder errorsStrBuilder = new StringBuilder();
		public static void LogException(FirstChanceExceptionEventArgs args)
		{
			try
			{
				if (args.Exception != null && args.Exception.Message != null && !args.Exception.Message.Contains("exceptionslogfile.txt"))
				{
					//WriteLogToAzureFile(args);

					string logMessage = args.Exception.Source + " " + args.Exception.Message + " " + args.Exception.StackTrace;
					string logEntry = DateTime.Now.ToString() + " " + logMessage;

					errorsStrBuilder.AppendLine(logEntry);

					if (errorsStrBuilder.Length > 10000)
					{

						WriteLogToServerFile(errorsStrBuilder);

						errorsStrBuilder = new StringBuilder();
					}

				}
				////AzureBlobService

			}
			catch { }

		}

		private static void WriteLogToServerFile(StringBuilder strBuilder)
		{
			using (StreamWriter w = File.AppendText(filePath))
			{
				w.WriteLine(strBuilder);
				w.Close();
			}
		}

		private static void WriteLogToServerFile(FirstChanceExceptionEventArgs args)
		{
			using (StreamWriter w = File.AppendText(filePath))
			{

				string logMessage = args.Exception.Source + " " + args.Exception.Message + " " + args.Exception.StackTrace;
				string logEntry = DateTime.Now.ToString() + " " + logMessage;
				w.WriteLine(logEntry);

				w.Close();
			}
		}



		private static void WriteLogToAzureFile(FirstChanceExceptionEventArgs args)
		{
			AzureBlobService blobService = new AzureBlobService();
			StringBuilder stringbuilder = new StringBuilder();
			DateTime now = DateTime.Now;
			CloudBlobContainer blobContainer = null;
			CloudBlockBlob blobfile = null;

			blobContainer = StorageAcountDetails.GetCurrentContainer(0);
			blobContainer.CreateIfNotExists();

			blobfile = blobContainer.GetBlockBlobReference("exceptionslogfile.txt");

			if (BlobExtensions.Exists(blobfile))
			{
				//Read data from errorlogs
				using (Stream blbstrRead = blobfile.OpenRead())
				{
					if (blbstrRead != null)
					{
						byte[] previousErrordata = new byte[blbstrRead.Length];
						blbstrRead.Read(previousErrordata, 0, previousErrordata.Length);

						Encoding encoding = new UTF8Encoding();
						stringbuilder.Append(encoding.GetString(previousErrordata));

						string logMessage = args.Exception.Source + " " + args.Exception.Message + " " + args.Exception.StackTrace;
						string logEntry = DateTime.Now.ToString() + " " + logMessage;
						stringbuilder.AppendLine(logEntry);
						byte[] logData = encoding.GetBytes(stringbuilder.ToString());
						using (Stream blbstr = blobfile.OpenWrite())
						{
							blbstr.Write(logData, 0, logData.Length);
						}
					}
				}
			}
		}

	}
}