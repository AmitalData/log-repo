using System.Data.Entity.Validation;
using System.Linq;

namespace AmitalCloud.Infrastructure.Domain.Helpers
{
    public class ExceptionFormatUtil
    {
        public static DbEntityValidationException GetFormated(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            var exceptionMessage = string.Concat("DbEntityValidationException ", ex.Message, " The validation errors are: ", fullErrorMessage);

            // Throw a new DbEntityValidationException with the improved exception message.
            return new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }
    }

}
