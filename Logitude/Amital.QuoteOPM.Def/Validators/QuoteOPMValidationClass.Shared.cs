using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace Amital.QuoteOPM.Def.Validators
{
    public partial class QuoteOPMValidationClass : IQuoteOPMValidateContext
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
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


            Validators.QuoteOPMValidationClass temp = new Validators.QuoteOPMValidationClass(objectTableName, tenant);
            bool valid = temp.IsValid(value, context.ObjectInstance, context.MemberName);


            if (!valid)
            {
                List<string> d = new List<string>();
                d.Add(context.MemberName);

                ValidationResult v = new ValidationResult(temp.GetErrorMessage(value, context.ObjectInstance, context.MemberName), new string[] { context.MemberName });

                return v;
            }

            return ValidationResult.Success;


        }
    }

    interface IQuoteOPMValidateContext
    {
        bool IsValid(object value, object objectInstance, string propertyName);
        string GetErrorMessage(object value, object instance, string propertyName);
    }
}

