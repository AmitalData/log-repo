using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.WebHook
{
    public class WebHookServiceExceptionThrower
    {
        public static string Throw(WebException ex, string remoteFile, string operation)
        {
            string errorMessage = "";
            // Handle WEBHOOK EXCEPTION ...soon....
            //for now we made this
            errorMessage += "Failed to " + operation + " " + (!string.IsNullOrEmpty(remoteFile) ? remoteFile + " file" : "") + Environment.NewLine + ex.Message;
            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                errorMessage += Environment.NewLine + ex.InnerException.Message;
            }

            errorMessage = DateTime.Now.ToString() + " : " + errorMessage;
            throw new WebHookServiceException(errorMessage);
        }
    }

	public class WebHookServiceException : Exception
	{
		public WebHookServiceException()
		{
		}

		public WebHookServiceException(string message)
			: base(message)
		{
		}

		public WebHookServiceException(string message, Exception inner)
	  : base(message, inner)
		{
		}
	}
}
