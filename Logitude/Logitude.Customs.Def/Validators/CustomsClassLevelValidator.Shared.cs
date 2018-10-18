using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;


namespace Logitude.Customs.Def.Validators
{
    public partial class CustomsClassLevelValidator : ICustomsClassLevelValidator
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            //return ValidationResult.Success;
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
            objectTableName = "Customs." + objectTableName;
            Type type = Type.GetType(context.ObjectType.FullName);
            type = value.GetType();
            PropertyInfo tenantProp = null;
            tenantProp = type.GetProperty("Tenant");
            int tenant = 0;
            if (objectTableName != "Tenant" && objectTableName != "TenantManagement")
            {
                tenantProp = type.GetProperty("Tenant");
                if (context.ObjectInstance.GetType() == type)
                {
                    tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                }
            }
            else
            {
                tenantProp = type.GetProperty("Id");
                tenantProp = type.GetProperty("Tenant");
                if (context.ObjectInstance.GetType() == type)
                {
                    tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                }
            }


            Validators.CustomsClassLevelValidator temp = new Validators.CustomsClassLevelValidator(objectTableName, tenant);
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

    public interface ICustomsClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}