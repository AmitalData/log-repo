using Logitude.Server.Tools;
using Marvin.JsonPatch.Exceptions;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace WebFreight.Web.Helpers
{
    public class ApiExceptionBuilder
    {
        public static APIException BuildException(Exception ex)
        {

            APIException apiException = new APIException();
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

                apiException = new APIException()
                {
                    ErrorType = ex.GetType().Name,
                    ErrorMessage = ErrorMessage ,
                    ShortErrorMessage = ShortErrorMessage,
                };
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

                apiException = new APIException()
                {
                    ErrorType = ex.GetType().Name,
                    ErrorMessage = errorMessage + " (" + ex.StackTrace + ")",
                    ShortErrorMessage = shortErrorMessage
                };
            }

            return apiException;
        }


        public static APIException BuildModelException(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            APIException apiException = new APIException();
            string ErrorMessage = "";
            string ShortErrorMessage = "";

            foreach (var modValue in modelState.Values)
            {
                foreach (var error in modValue.Errors)
                {
                    //ErrorMessage += error.ErrorMessage + Environment.NewLine;
                    ErrorMessage += (String.IsNullOrWhiteSpace(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage ) + Environment.NewLine;
                    ShortErrorMessage += error.ErrorMessage + Environment.NewLine;
                }
            }

            apiException = new APIException()
            {
                ErrorType = "ModelStateError",
                ErrorMessage = ErrorMessage,
                ShortErrorMessage = ShortErrorMessage,
            };

            return apiException;
        }

        public static object BuildJsonPatchException(JsonPatchException exception, string entityId, object jsonPatch)
        {
            string errorMessage = "Error Message: " + exception.Message + Environment.NewLine + 
                                  "Entity Id: " + entityId + Environment.NewLine +
                                  "Json Patch: " + JsonConvert.SerializeObject(jsonPatch);

            string shortErrorMessage = GetShortErrorMessage(exception);

            return new APIException()
            {
                ErrorType = exception.GetType().Name,
                ErrorMessage = errorMessage,
                ShortErrorMessage = shortErrorMessage
            };
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

        private static string GetChildEntityName(JsonPatchException exception)
        {
            var failedOperation = exception.FailedOperation;
            string entityNameInCamelCase = failedOperation.path.Split('/')[1];

            string returnedEntityName = Regex.Replace(entityNameInCamelCase, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());
            return returnedEntityName;
        }
    }
}
