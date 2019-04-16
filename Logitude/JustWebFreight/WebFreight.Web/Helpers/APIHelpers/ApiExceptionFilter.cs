using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;  
    using System.Net.Http;  
    using System.Web.Http.Filters;

namespace WebFreight.Web.Helpers.APIHelpers
{
	public class ApiExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(actionExecutedContext.Exception));
			actionExecutedContext.Response = response;
		}
	}
}