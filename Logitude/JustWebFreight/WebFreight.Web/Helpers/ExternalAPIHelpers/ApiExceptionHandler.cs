using Logitude.BL.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace WebFreight.Web.Helpers.ExternalAPIHelpers
{
    public class ApiExceptionHandler
    {
        public static APIExceptionResult HandleException(Exception ex)
        {

            APIExceptionResult result = new APIExceptionResult();
            APIException apiException = new APIException();
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (ex != null)
                {
                    if (ex.GetType() == typeof(DbEntityValidationException))
                    {
                        apiException = BuildDBException(ex);
                        statusCode = HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        apiException = BuildInternalException(ex);
                    }

                    if (ex.GetType() == typeof(Logitude.BL.Security.AutenticationException)
                        || ex.GetType() == typeof(WebFreight.Web.Security.AutenticationException))
                    {
                        statusCode = HttpStatusCode.Unauthorized;
                    }

                    if (ex.GetType() == typeof(ApplicationException))
                    {
                        statusCode = HttpStatusCode.BadRequest;
                    }
                }

            result.StatusCode = statusCode;
            result.Exception = apiException;
         

            return result;
        }

        public static APIExceptionResult HandleModelException(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            APIExceptionResult result = new APIExceptionResult();
            APIException apiException = new APIException();

            if (modelState != null)
            {
                string ErrorMessage = "";
                string ShortErrorMessage = "";

                foreach (var modValue in modelState.Values)
                {
                    foreach (var error in modValue.Errors)
                    {
                        string errorStr = error.ErrorMessage;
                        if (!string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            errorStr = error.ErrorMessage;
                        }
                        else if (error.Exception != null)
                        {
                            errorStr = error.Exception.Message;
                            if (error.Exception.InnerException != null)
                                errorStr = error.Exception.InnerException.Message;

                        }

                        ErrorMessage += errorStr + Environment.NewLine;
                        ShortErrorMessage += errorStr + Environment.NewLine;
                    }
                }

                apiException = new APIException()
                {
                    ErrorType = "Invalid Xml",
                    ErrorMessage = ErrorMessage,
                    ShortErrorMessage = ShortErrorMessage,
                };

            }

            result.StatusCode = HttpStatusCode.BadRequest;
            result.Exception = apiException;


            return result;
        }

        private static APIException BuildInternalException(Exception ex)
        {
            APIException apiException;
            string errorMessage = ex.Message + Environment.NewLine;
            string shortErrorMessage = ex.Message + Environment.NewLine;
            //errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
            if (ex.InnerException != null && ex.GetType() != typeof(ApplicationException))
            {

                errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                shortErrorMessage = shortErrorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
              

            }
           

            apiException = new APIException()
            {
                ErrorType = ex.GetType().Name,
                ErrorMessage = errorMessage,
                ShortErrorMessage = shortErrorMessage
            };
            return apiException;
        }

        private static APIException BuildDBException(Exception ex)
        {
            string ErrorMessage = "";
            string ShortErrorMessage = "";
            var exception = ((System.Data.Entity.Validation.DbEntityValidationException)ex);
            if (exception != null)
            {

                foreach (var eve in exception.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        ErrorMessage += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + ";";

                        ShortErrorMessage += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + ";";
                    }
                }
            }
            APIException apiException = new APIException()
            {
                ErrorType = ex.GetType().Name,
                ErrorMessage = ErrorMessage,
                ShortErrorMessage = ShortErrorMessage,
            };

            return apiException;
        }

    }

    public class APIException
    {
        public string ErrorType { get; set; }
        public string ShortErrorMessage { get; set; }
        public string ErrorMessage { get; set; }
    }


    public class APIExceptionResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public APIException Exception { get; set; }

    }

}