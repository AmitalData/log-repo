using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Logitude.Infrastructure.BL.Validators
{
    public partial class InfrastructureClassLevelValidator : IInfrastructureClassLevelValidator
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            //return ValidationResult.Success;
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
            Type type = Type.GetType(context.ObjectType.FullName);

            if (context.ObjectType.FullName == "WebFreight.Web.DataContracts.BIReportXMLData")
            {
                return null;
            }

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

            else if (objectTableName == "BIReport")
            {
                BIReportClassLevelValidator bIReportClassLevelValidator = new BIReportClassLevelValidator(objectTableName, tenant);
                if (!bIReportClassLevelValidator.IsValid(value, context.ObjectInstance, context.MemberName))
                {
                    ValidationResult v = new ValidationResult(bIReportClassLevelValidator.GetErrorMessage(value, context.ObjectInstance, context.MemberName), new string[] { context.MemberName });
                    return v;
                }
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

            Validators.InfrastructureClassLevelValidator temp = new Validators.InfrastructureClassLevelValidator(objectTableName, tenant);
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

    public interface IInfrastructureClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}
