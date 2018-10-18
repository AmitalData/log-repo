using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.Validators
{
    public partial class TimeManagementValidationClass : ITimeManagementValidateContext
    {


        bool ready;
        int tenant = 0;
        private string errorMessage = "";
        List<ObjectField> objectFieldList;

        public TimeManagementValidationClass(string objectTableName, int tenant)
        {
            this.tenant = tenant;

            objectFieldList = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList();
        }

        public string GetErrorMessage(object value, object instance, string propertyName)
        {
            string error = "";
            Type type = instance.GetType();
            object typeProp = null;

            string objectType = "";
            if (typeProp != null)
            {
                objectType = typeProp.ToString();
            }
            else
            {
                objectType = type.Name.ToString();
                if (objectType.Contains("FollowUp"))
                {
                    objectType = "FollowUp";
                }
                else if (objectType.Contains("PM"))
                {
                    objectType = objectType.Substring(0, objectType.Length - 2);
                }
            }

            ObjectField field = (from a in objectFieldList
                                 where a.ObjectTable.Name == objectType && a.FieldName == propertyName
                                 select a).FirstOrDefault();

            if (field.IsRequiered)
            {

                if (value == null)
                {
                    error = TimeManagementTranslateTextsClass.GetTranslation("General.M.FieldIsRequired", field.FullNameTextCode.Code, null, null, field.Tenant);
                }
                if (string.IsNullOrEmpty(error) && value != null)
                {
                    if (value is string)
                    {
                        string valueString = value as string;
                        if (String.IsNullOrEmpty(value.ToString()))
                        {
                            error = TimeManagementTranslateTextsClass.GetTranslation("General.M.FieldIsRequired", field.FullNameTextCode.Code, null, null, field.Tenant);

                        }
                    }
                }
            }


            if (field.DataTypeCode == "Text")
            {

                if (field.IsCustom && value != null)
                {
                    CustomFieldClass fieldClass = value as CustomFieldClass;
                    value = fieldClass.Value;
                }

                string valueString = value != null ? value.ToString() : "";
                if (!string.IsNullOrEmpty(valueString))
                {
                    if (!field.IsMaxLength)
                    {
                        if (valueString.Length > field.MaxLength || valueString.Length < field.MinLength)
                        {
                            error = TimeManagementTranslateTextsClass.GetTranslation("General.M.MinMax", field.FullNameTextCode.Code, field.MinLength.ToString(), field.MaxLength.ToString(), field.Tenant);

                        }
                    }
                }
            }

            if (field.DataTypeCode == "Decimal" && field.NumberOfDigits != 0)
            {
                string valueString = value != null ? value.ToString() : "";
                if (!string.IsNullOrEmpty(valueString))
                {
                    //  string[] digits = valueString.Split('.');
                    int beforepointlength = valueString.Substring(0, valueString.LastIndexOf(".")).Length;
                    int afterpointlength = valueString.Substring(valueString.IndexOf(".") + 1).Length;
                    //if (valueString.Length > field.NumberOfDigits || valueString.Length < field.DigitsAfterPoint)
                    //{
                    //    error = TranslateTextsClass.GetTranslation("General.M.MinMax", field.FullNameTextCode.Code, field.MinLength.ToString(), field.MaxLength.ToString(), field.Tenant);

                    //}
                }
            }


            if (!String.IsNullOrEmpty(errorMessage))
            {
                error = errorMessage;
            }


            return error;
        }

        public bool IsValid(object value, object objectInstance, string propertyName)
        {
            bool valid = true;
            errorMessage = "";
            Type type = objectInstance.GetType();
            object typeProp = null;

            string objectType = "";

            if (typeProp != null)
            {
                objectType = typeProp.ToString();
            }

            else
            {
                objectType = type.Name.ToString();
                if (objectType.Contains("FollowUp"))
                {
                    objectType = "FollowUp";
                }
                else if (objectType.Contains("PM"))
                {
                    objectType = objectType.Substring(0, objectType.Length - 2);
                }
            }

            ObjectField field = (from a in objectFieldList
                                 where a.ObjectTable.Name == objectType && a.FieldName == propertyName
                                 select a).FirstOrDefault();

            if (field != null)
            {

                if (field.IsRequiered)
                {
                    if (field.IsCustom && value != null)
                    {
                        CustomFieldClass fieldClass = value as CustomFieldClass;
                        value = fieldClass.Value;
                    }

                    if (value == null)
                    {
                        valid = false;
                    }

                    else if (value is string)
                    {
                        if (String.IsNullOrEmpty(value.ToString()))
                        {
                            valid = false;

                        }
                    }

                    else
                    {
                        valid = true;
                    }
                }

                if (field.DataTypeCode == "Text")
                {
                    if (field.IsCustom && value != null)
                    {
                        CustomFieldClass fieldClass = value as CustomFieldClass;
                        value = fieldClass.Value;
                    }

                    string valueString = value != null ? value.ToString() : "";
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        if (!field.IsMaxLength)
                        {
                            if (valueString.Length > field.MaxLength || valueString.Length < field.MinLength)
                            {
                                valid = false;
                            }
                        }
                    }
                }

                if (field.DataTypeCode == "Decimal" && field.NumberOfDigits != 0)
                {
                    if (value != null)
                    {
                        string valueString = value.ToString();
                        if (valueString.Contains('.'))
                        {

                        }
                    }
                }

                if (value != null)
                {
                    //ValidationFieldResult result = fieldValidator.ValidateField(field, value, instance, tenant);

                    //if (!result.Valid)
                    //{
                    //    valid = result.Valid;
                    //    errorMessage = result.ErrorMessage;
                    //}
                }
            }

            return valid;
        }

        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            // return ValidationResult.Success;
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


            //PropertyInfo tenantProp = type.GetProperty("Tenant");

            //int tenant =(int) tenantProp.GetValue(Context.ObjectInstance, null);

            //Client client1 = Context.ObjectInstance as Client;

            /* by islam
             */
            Validators.TimeManagementValidationClass temp = new Validators.TimeManagementValidationClass(objectTableName, tenant);
            bool valid = temp.IsValid(value, context.ObjectInstance, context.MemberName);


            if (!valid)
            {
                List<string> d = new List<string>();
                d.Add(context.MemberName);

                //throw new Exception(temp.GetErrorMessage(Context.ObjectInstance, Context.MemberName));
                ValidationResult v = new ValidationResult(temp.GetErrorMessage(value, context.ObjectInstance, context.MemberName), new string[] { context.MemberName });

                return v;
            }

            return ValidationResult.Success;


        }

    }

    interface ITimeManagementValidateContext
    {
        bool IsValid(object value, object objectInstance, string propertyName);
        string GetErrorMessage(object value, object instance, string propertyName);
    }
}
