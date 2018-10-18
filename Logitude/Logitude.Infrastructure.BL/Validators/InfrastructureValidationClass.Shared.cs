using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Logitude.Infrastructure.BL.Validators
{
    public partial class InfrastructureValidationClass : IInfrastructureValidateContext
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
            Type type = Type.GetType(context.ObjectType.FullName);

            PropertyInfo tenantProp = null;
            tenantProp = type.GetProperty("Tenant");
            int tenant = 0;

            if (objectTableName == "BookingProduct")
            {
                tenant = 0;
            }

            else if (objectTableName == "Tenant" && objectTableName == "TenantManagement")
            {
                tenantProp = type.GetProperty("Id");
                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
            }

            else
            {

                tenantProp = type.GetProperty("Tenant");
                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
            }


            Validators.InfrastructureValidationClass temp = new Validators.InfrastructureValidationClass(objectTableName, tenant);
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

    interface IInfrastructureValidateContext
    {
        bool IsValid(object value, object objectInstance, string propertyName);
        string GetErrorMessage(object value, object instance, string propertyName);
    }
}
