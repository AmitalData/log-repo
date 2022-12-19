using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;  
    using System.Net.Http;  
    using System.Web.Http.Filters;
using System.Configuration;

namespace WebFreight.Web.Helpers.APIHelpers
{
	public class ApiExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(actionExecutedContext.Exception));
			actionExecutedContext.Response = response;

            if (ConfigurationManager.AppSettings.Get("logExcptionsToFile") == "true")
            {
                try
                {
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
                    string logMessage = Environment.NewLine + Environment.NewLine + "******************* Error *******************" + Environment.NewLine + Environment.NewLine +
                        DateTime.Now.ToString()+ Environment.NewLine + 
                        "source: " + actionExecutedContext.Exception.Source + Environment.NewLine + 
                        "error message: " + actionExecutedContext.Exception.Message + Environment.NewLine + 
                        "stack trace: " + actionExecutedContext.Exception.StackTrace;
                    System.IO.File.AppendAllText(filePath, logMessage);
                }
                catch { }
            }
        }
	}
}