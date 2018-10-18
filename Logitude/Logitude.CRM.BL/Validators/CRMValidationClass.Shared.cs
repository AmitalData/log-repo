using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Logitude.CRM.BL.Validators
{
    public partial class CRMValidationClass : ICRMValidateContext
    {
         public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
           // return ValidationResult.Success;
            string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
             Type type=Type.GetType(context.ObjectType.FullName);


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


             //PropertyInfo tenantProp = type.GetProperty("Tenant");

             //int tenant =(int) tenantProp.GetValue(Context.ObjectInstance, null);
          
            //Client client1 = Context.ObjectInstance as Client;

             /* by islam
              */
             Validators.CRMValidationClass temp = new Validators.CRMValidationClass(objectTableName,tenant);
             bool valid = temp.IsValid(value,context.ObjectInstance,context.MemberName);
           
            
            if (!valid)
            {
                List<string> d = new List<string>();
                d.Add(context.MemberName);
                
                 //throw new Exception(temp.GetErrorMessage(Context.ObjectInstance, Context.MemberName));
                ValidationResult v = new ValidationResult(temp.GetErrorMessage(value, context.ObjectInstance, context.MemberName),new string[] { context.MemberName });

                return v;
            }
               
             return ValidationResult.Success;
            

        }
    }
    
    interface ICRMValidateContext
    {
        bool IsValid(object value,object objectInstance,string propertyName);
        string GetErrorMessage(object value, object instance, string propertyName);
    }
}