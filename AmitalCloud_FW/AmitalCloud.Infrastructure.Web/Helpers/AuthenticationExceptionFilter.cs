using System.Web.Http.Filters;
using System.Net;
using System.Net.Http;
using AmitalCloud.Infrastructure.Data.Security;

public class AuthenticationExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(HttpActionExecutedContext context)
    {
        if (context.Exception is AutenticationException)
        {
            context.Response = context.Request.CreateResponse(
                HttpStatusCode.Unauthorized,
                new {
                    Message = "Access Denied",
                    ErrorType = nameof(AutenticationException),
                    ExceptionMessage = context.Exception.Message,
                }
            );

            NetCommonHelper.Logger.DevLog.Instance.WriteError($"access denied: {context.Exception.Message}");
        }
    }
}
