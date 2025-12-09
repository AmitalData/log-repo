using Logitude.Server.Tools;
using Marvin.JsonPatch.Exceptions;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ApiExceptionBuilder
    {
        public static APIException BuildException(Exception ex)
        {

            string ErrorMessage = "";
            string ShortErrorMessage = "";
            if (ex.GetType().Name == "DbEntityValidationException")
            {
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

                var message = IsUserAuthenticated() ? ErrorMessage : "Validation error occurred. Please check the data and try again.";
                var stackTrace = IsUserAuthenticated() ? ex?.StackTrace : null;

                return BuildApiException(ex.GetType().Name, message, ShortErrorMessage, stackTrace);
            }
            else
            {
               
                string errorMessage = ex.Message + Environment.NewLine;
                string shortErrorMessage = ex.Message + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                    shortErrorMessage = shortErrorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                }
                var message = IsUserAuthenticated() ? errorMessage : "An unexpected error occurred. Please try again later.";
                var stackTrace = IsUserAuthenticated() ? ex?.StackTrace : null;

                return BuildApiException(ex.GetType().Name, message, shortErrorMessage, stackTrace);

            }

        }


        public static APIException BuildModelException(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            string ErrorMessage = "";
            string ShortErrorMessage = "";
           
            foreach (var modValue in modelState.Values)
            {
                foreach (var error in modValue.Errors)
                {
                    ErrorMessage += (String.IsNullOrWhiteSpace(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage ) + Environment.NewLine;
                    ShortErrorMessage += error.ErrorMessage + Environment.NewLine;
                }
            }
            var message = IsUserAuthenticated() ? ErrorMessage : "Validation error occurred. Please check the data and try again.";
            return BuildApiException("ModelStateError", message, ShortErrorMessage);
        }

        public static object BuildJsonPatchException(JsonPatchException exception, string entityId, object jsonPatch)
        {

            string errorMessage = "Error Message: " + exception.Message + Environment.NewLine + 
                                  "Entity Id: " + entityId + Environment.NewLine +
                                  "Json Patch: " + JsonConvert.SerializeObject(jsonPatch);

            string shortErrorMessage = GetShortErrorMessage(exception);
            var message = IsUserAuthenticated() ? errorMessage : "There was an issue with the data provided. Please check and try again.";
            return BuildApiException(exception.GetType().Name, errorMessage, shortErrorMessage);
           
        }

        private static string GetShortErrorMessage(JsonPatchException exception)
        {
            if (Regex.IsMatch(exception.Message, @"The current value '.*' at path '.*' is not equal to the test value '.*'\."))
            {
                return "Some of the " + GetChildEntityName(exception) + " were deleted.";
            }
            else
            {
                return "Invalid update";
            }
        }

        

        private static bool IsUserAuthenticated()
        {
            try
            {
                var token = HttpContext.Current?.Request?.Headers["Token"];
                if (string.IsNullOrWhiteSpace(token))
                    return false;  

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken == null)
                    return false;  

                var userRepository = new UserRepository(authToken.Tenant);
                var user = userRepository.GetSingleUserByEmail(authToken.Email, 0, true);

                return user != null;  
            }
            catch
            {
                return false;  
            }
        }

        private static APIException BuildApiException(string errorType, string errorMessage, string shortErrorMessage, string stackTrace = null)
        {
            return new APIException()
            {
                ErrorType = errorType,
                ErrorMessage = errorMessage + (stackTrace ?? string.Empty),
                ShortErrorMessage = shortErrorMessage
            };
        }


        private static string GetChildEntityName(JsonPatchException exception)
        {
            var failedOperation = exception.FailedOperation;
            string entityNameInCamelCase = failedOperation.path.Split('/')[1];

            string returnedEntityName = Regex.Replace(entityNameInCamelCase, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());
            return returnedEntityName;
        }
    }
}
