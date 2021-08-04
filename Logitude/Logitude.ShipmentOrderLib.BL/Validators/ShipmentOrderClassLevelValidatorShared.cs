using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Logitude.ShipmentOrderLib.BL.Validators
{
   public partial class ShipmentOrderClassLevelValidator : IShipmentOrderClassLevelValidator
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            //return ValidationResult.Success;
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
            Type type = Type.GetType(context.ObjectType.FullName);

            PropertyInfo tenantProp = null;
            tenantProp = type.GetProperty("Tenant");
            int tenant = 0;

            if (objectTableName == "ShipmentOrderProduct")
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
                if (context.ObjectInstance.GetType() == type && tenantProp != null)
                {
                    object tenantvalue = tenantProp.GetValue(context.ObjectInstance, null);
                    if (tenantvalue != null)
                    {
                        tenant = (int)tenantvalue;
                    }
                }
            }

            Validators.ShipmentOrderClassLevelValidator temp = new Validators.ShipmentOrderClassLevelValidator(objectTableName, tenant);
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

    public interface IShipmentOrderClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}
