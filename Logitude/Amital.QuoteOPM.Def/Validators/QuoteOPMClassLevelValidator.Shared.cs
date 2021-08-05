using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.Def.Validators
{
    public partial class QuoteOPMClassLevelValidator : IQuoteOPMClassLevelValidator
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            //return ValidationResult.Success;
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
            if (objectTableName == "List") return null; 

            Type type = Type.GetType(context.ObjectType.FullName);

            PropertyInfo tenantProp = null;
            tenantProp = type.GetProperty("Tenant");
            int tenant = 0;
            if (objectTableName != "Tenant" && objectTableName != "TenantManagement")
            {
                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
            }
            else
            {
                tenantProp = type.GetProperty("Id");
                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
            }


            Validators.QuoteOPMClassLevelValidator temp = new Validators.QuoteOPMClassLevelValidator(objectTableName, tenant);
            //Client client1 = Context.ObjectInstance as Client;
            bool valid = temp.IsValid(value, context.ObjectInstance, context.MemberName);

            List<ValidationResult> vList = new List<ValidationResult>();
            if (!valid)
            {


                ValidationResult v = new ValidationResult(temp.GetErrorMessage(context.ObjectInstance, context.MemberName), new string[] { context.MemberName });
                return v;
            }
            return null;


        }
    }

    public interface IQuoteOPMClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}
