using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                   ErrorMessage = ErrorMessage,
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

                //errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                apiException = new APIException()
                {
                    ErrorType = ex.GetType().Name,
                    ErrorMessage = errorMessage,
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
    }
}
