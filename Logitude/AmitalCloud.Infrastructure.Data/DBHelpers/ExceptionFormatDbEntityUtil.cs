using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.DBHelpers
{
    internal class ExceptionFormatDbEntityUtil
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
            var builder = new StringBuilder();
            builder.Append("DbEntityValidationException ").Append(ex.Message).Append(" The validation errors are: ").Append(fullErrorMessage);
            foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
            {
                string entityName = validationResult.Entry.Entity.GetType().Name;
                foreach (DbValidationError error in validationResult.ValidationErrors)
                {
                    builder.AppendLine(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                }
                var e = validationResult.Entry;
                if (e != null && e.CurrentValues != null)
                {
                    foreach (var propertyName in e.CurrentValues.PropertyNames)
                    {
                        builder.AppendLine().AppendFormat("Property Name: {0}", propertyName);
                        if (e.State != EntityState.Added)
                        {
                            //get original value
                            var orgVal = e.OriginalValues[propertyName];
                            builder.AppendFormat("     Original Value: {0}", orgVal);
                        }
                        //get current values
                        var curVal = e.CurrentValues[propertyName];
                        builder.AppendFormat("     Current Value: {0}", curVal);
                    }
                }
            }
            // Throw a new DbEntityValidationException with the improved exception message.
            return new DbEntityValidationException(builder.ToString(), ex.EntityValidationErrors);
        }
    }
}
