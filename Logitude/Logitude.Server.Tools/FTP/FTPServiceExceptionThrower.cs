using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.FTP
{
	public class FTPServiceExceptionThrower
	{
		public static string Throw(WebException ex, string host, string user, string remoteFile, string operation)
		{

			string errorMessage = "";
			var ftpResponse = ex.Response as FtpWebResponse;
            if (ftpResponse != null)
            {
                switch (ftpResponse.StatusCode)
                {
                    case FtpStatusCode.NotLoggedIn:
                        errorMessage += "Failed to perform 'Logon' to Host:'" + host + "' ,User:'" + user + Environment.NewLine + ftpResponse.StatusDescription;
                        break;


                    case FtpStatusCode.ActionNotTakenFileUnavailable:
                        errorMessage += "Failed to " + operation + " " + remoteFile + " file" + Environment.NewLine + ftpResponse.StatusDescription;
                        break;
                    default:
                        errorMessage += "Failed to " + operation + " " + (!string.IsNullOrEmpty(remoteFile) ? remoteFile + " file" : "") + Environment.NewLine + ftpResponse.StatusDescription;
                        break;
                }
            }
            else
            {
                errorMessage += "Failed to " + operation + " " + (!string.IsNullOrEmpty(remoteFile) ? remoteFile + " file" : "") + Environment.NewLine + ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    errorMessage += Environment.NewLine + ex.InnerException.Message;
                }
            }
			errorMessage = DateTime.Now.ToString() + " : " + errorMessage;
			throw new FTPServiceException(errorMessage);
			//return errorMessage;
		}
	}

	public class FTPServiceException : Exception
	{
		public FTPServiceException()
		{
		}

		public FTPServiceException(string message)
			: base(message)
		{
		}

		public FTPServiceException(string message, Exception inner)
	  : base(message, inner)
		{
		}
	}
}
