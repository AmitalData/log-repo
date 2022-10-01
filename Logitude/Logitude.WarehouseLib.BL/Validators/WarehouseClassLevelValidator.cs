using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.Helpers;

namespace Logitude.WarehouseLib.BL.Validators
{
    public partial class WarehouseClassLevelValidator : IWarehouseClassLevelValidator
    {
        int tenant;
        string objectTableName;
        string errorMessage = "";
        List<ObjectField> objectFieldList;
        static string validationOutputMessage = "";

        public WarehouseClassLevelValidator(string objectTableName, int tenant)
        {

            //ruleValidator = new RulesValidator(tenant); 
            this.objectTableName = objectTableName;
            this.tenant = tenant;


            objectFieldList = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList(); //Context.Where(d => d.ObjectTable.Name == objectTableName && (d.Tenant == tenant || d.Tenant == 0)).ToList();

        }

        public bool IsValid(object value, object instance, string propertyName)
        {

            errorMessage = "";
            validationOutputMessage = "";
            Type type = value.GetType();
            string objectName = type.Name;
            if (objectName.Contains("FollowUp"))
            {
                objectName = "FollowUp";
            }
            else if (objectName.Contains("PM"))
            {
                objectName = objectName.Substring(0, objectName.Length - 2);
            }

            //if (!ruleValidator.ValidateEntityRules(value, objectTableName,tenant, ref validationOutputMessage))
            //{
            //    return false;
            //}

            //List<ObjectTableRuleField> requiredObjectFields = ruleValidator.ValidateAllRequiredFieldRules(value, objectTableName, tenant);


            //if (requiredObjectFields.Count > 0)
            //{
            //    foreach (ObjectTableRuleField field in requiredObjectFields)
            //    {
            //        errorMessage = errorMessage + "," + TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", field.ObjectField.FullNameTextCode.Code, null, null, field.Tenant);

            //    }

            //    return false;
            //}

            PropertyInfo custom = type.GetProperty("CustomField");
            object cutomfield = null;
            Type customType = null;
            if (custom != null)
            {
                cutomfield = custom.GetValue(value, null);
                customType = custom.PropertyType;
            }



            //bool isValid = ruleValidator.ApplyDuplicationRules(instance, objectTableName, tenant, ref validationOutputMessage);
            //if (!isValid)
            //{
            //    return false;
            //}


            foreach (ObjectField objectfeildprop in objectFieldList)
            {

                PropertyInfo propertyInf = type.GetProperty(objectfeildprop.FieldName);
                if (propertyInf != null)
                {
                    object propertyValue = propertyInf.GetValue(value, null);

                    //if (propertyValue != null)
                    //{
                    //    if (propertyValue.ToString().Trim() != String.Empty)
                    //    {
                    //        bool isValid = RulesValidator.ApplyDuplicationRules(objectfeildprop.FieldName, instance, objectfeildprop.ObjectTable.Name, objectfeildprop.Tenant, ref ValidationOutputMessage);
                    //        if (!isValid)
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //}

                    if (objectfeildprop.IsRequiered)
                    {

                        if (propertyValue == null)
                        {
                            return false;
                        }
                        else if (propertyValue is string)
                        {

                            if (string.IsNullOrEmpty(propertyValue.ToString()))
                            {
                                return false;
                            }



                        }

                    }
                    if (objectfeildprop.DataTypeCode == "Text" || objectfeildprop.DataTypeCode == "nText")
                    {
                        if (objectfeildprop.IsCustom && propertyValue != null)
                        {
                            CustomFieldClass fieldClass = propertyValue as CustomFieldClass;
                            propertyValue = fieldClass.Value;
                        }
                        if (propertyValue != null && FieldValueValidator.IsNotValidMinMaxValue(objectfeildprop, propertyValue.ToString()))
                        {
                            return false;
                        }
                    }


                    //if (objectfeildprop.DataTypeCode == "DateTime" || objectfeildprop.DataTypeCode == "Date")
                    //{
                    //    if (propertyValue != null)
                    //    {
                    //        DateTime SelectableDateStart = DateTime.Now.AddYears(-100);
                    //        DateTime SelectableDateEnd = DateTime.Now.AddYears(100);
                    //        DateTime? dateValue = null;
                    //        if (objectfeildprop.IsCustom)
                    //        {
                    //            try
                    //            {
                    //                dateValue = DateTime.Parse(propertyValue.ToString());
                    //            }
                    //            catch { }
                    //        }
                    //        else
                    //        {
                    //            dateValue = (DateTime?)propertyValue;
                    //        }

                    //        if (dateValue.Value.Date < SelectableDateStart || dateValue.Value.Date > SelectableDateEnd)
                    //        {
                    //            ErrorMessage = "Selected date must be within the last 100 years or the next 100 years";
                    //            return false;
                    //        }

                    //        if (objectfeildprop.DataTypeCode == "Date")
                    //        {

                    //            if (dateValue.Value != null)
                    //            {
                    //                TimeSpan zerotime = new TimeSpan(0, 0, 0);
                    //                if (dateValue.Value.TimeOfDay != zerotime)
                    //                {
                    //                    ErrorMessage = "invsalid selected date, should be date not datetime";
                    //                    return false;
                    //                }
                    //            }
                    //        }


                    //    }


                    //}

                }
                else
                {
                    if (customType != null)
                    {
                        PropertyInfo field = customType.GetProperty(objectfeildprop.FieldName);
                        if (field != null)
                        {
                            if (objectfeildprop.IsRequiered)
                            {
                                if (cutomfield != null)
                                {
                                    object fieldValue = field.GetValue(cutomfield, null);
                                    if (fieldValue == null)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }



            return true;
        }

        public string GetErrorMessage(object instance, string property)
        {
            if (!String.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }
            string requiredError = "";
            string stringLengthError = "";
            // string variable1 = "";
            Type type = instance.GetType();
            string objectName = type.Name;
            if (objectName.Contains("FollowUp"))
            {
                objectName = "FollowUp";
            }
            else if (objectName.Contains("PM"))
            {
                objectName = objectName.Substring(0, objectName.Length - 2);
            }

            //if (objectName == "Shipment")
            //{
            //    ShipmentPM shipment = instance as ShipmentPM;
            //    if (shipment != null)
            //    {
            //        if (shipment.ShipmentLevelCode == "C")
            //        {
            //            objectName = "Master";
            //        }
            //    }
            //}
            //var OFList = from a in ObjectFieldList
            //             where a.ObjectTable.Name == objectName 
            //             select a;
            //List<ObjectField> RequiredOFList = OFList.ToList();
            PropertyInfo custom = type.GetProperty("CustomField");
            object cutomfield = null;
            Type customType = null;
            if (custom != null)
            {
                cutomfield = custom.GetValue(instance, null);
                customType = custom.PropertyType;
            }
            foreach (ObjectField objectfeildprop in objectFieldList)
            {

                PropertyInfo propertyInf = type.GetProperty(objectfeildprop.FieldName);
                if (propertyInf != null)
                {
                    object propertyValue = propertyInf.GetValue(instance, null);

                    if (objectfeildprop.IsCustom && propertyValue != null)
                    {
                        CustomFieldClass fieldClass = propertyValue as CustomFieldClass;
                        propertyValue = fieldClass.Value;
                    }

                    if (objectfeildprop.IsRequiered)
                    {
                        if (propertyValue == null)
                        {
                            requiredError = requiredError + "," + WarehouseTranslateTextsClass.GetTranslation("General.M.FieldIsRequired", objectfeildprop.FullNameTextCode.Code, null, null, objectfeildprop.Tenant);
                        }

                        else if (propertyValue is string)
                        {

                            if (string.IsNullOrEmpty(propertyValue.ToString()))
                            {
                                requiredError = requiredError + "," + WarehouseTranslateTextsClass.GetTranslation("General.M.FieldIsRequired", objectfeildprop.FullNameTextCode.Code, null, null, objectfeildprop.Tenant);
                            }


                        }

                    }


                    if (objectfeildprop.DataTypeCode == "Text" || objectfeildprop.DataTypeCode == "nText")
                    {
                        if (objectfeildprop.IsCustom && propertyValue != null)
                        {
                            CustomFieldClass fieldClass = propertyValue as CustomFieldClass;
                            propertyValue = fieldClass.Value;
                        }
                        if (propertyValue != null && FieldValueValidator.IsNotValidMinMaxValue(objectfeildprop, propertyValue.ToString()))
                        {
                            stringLengthError = stringLengthError + "," + WarehouseTranslateTextsClass.GetTranslation("General.M.MinMax", objectfeildprop.FullNameTextCode.Code, objectfeildprop.MinLength.ToString(), objectfeildprop.MaxLength.ToString(), objectfeildprop.Tenant);
                        }
                    }


                    //if (objectfeildprop.DataTypeCode == "Text")
                    //{


                    //    if (propertyValue == null)
                    //    {
                    //        propertyValue = "";
                    //    }

                    //    if (propertyValue.ToString().Length > objectfeildprop.MaxLength || propertyValue.ToString().Length < objectfeildprop.MinLength)
                    //    {
                    //        StringLengthError = StringLengthError + "," + TranslateTextsClass.GetTranslation("General.M.MinMax", objectfeildprop.FullNameTextCode.Code, objectfeildprop.MinLength.ToString(), objectfeildprop.MaxLength.ToString(), objectfeildprop.Tenant);
                    //    }
                    //}






                }
                else
                {
                    if (customType != null)
                    {
                        PropertyInfo field = customType.GetProperty(objectfeildprop.FieldName);
                        if (field != null)
                        {
                            if (cutomfield != null)
                            {
                                if (objectfeildprop.IsRequiered)
                                {
                                    object fieldValue = field.GetValue(cutomfield, null);
                                    if (fieldValue == null)
                                    {
                                        requiredError = requiredError + "," + WarehouseTranslateTextsClass.GetTranslation("General.M.FieldIsRequired", objectfeildprop.FullNameTextCode.Code, null, null, objectfeildprop.Tenant);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string errors = "";
            requiredError = requiredError.TrimStart(',');
            stringLengthError = stringLengthError.TrimStart(',');


            if (!string.IsNullOrEmpty(validationOutputMessage))
            {
                validationOutputMessage = validationOutputMessage.TrimStart(',');
                errors = validationOutputMessage;
            }

            if (!string.IsNullOrEmpty(requiredError))
            {
                errors = errors + "," + requiredError;
            }
            if (!string.IsNullOrEmpty(stringLengthError))
            {
                if (!string.IsNullOrEmpty(requiredError))
                {
                    errors = errors + "," + stringLengthError;
                }
                else
                {
                    errors = stringLengthError;
                }
            }

            errors = errors.TrimStart(',');
            return errors;
        }

        public System.Collections.Generic.List<string> GetErrorsInObject(object instance)
        {
            throw new NotImplementedException();
        }


        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            if (value != null)
            {
                string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
                Type type = Type.GetType(context.ObjectType.FullName);
                type = value.GetType();

                int tenant = 0;
                PropertyInfo tenantProp = null;

                switch (objectTableName)
                {
                    case "MessagingStock":
                        {
                            tenantProp = type.GetProperty("TenantNumber");
                            tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                            break;
                        }

                    case "Tenant":
                    case "TenantManagement":
                    case "AccountingSetting":
                        {
                            tenantProp = type.GetProperty("Id");
                            tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                            break;
                        }

                    case "Package":
                    case "PackageConnectedPackage":
                    case "Distributor":
                    case "ComputingPartner":
                    case "AccountingSystem":
                    case "IATACode":
                    case "AWBSpecialHandlingCode":
                    case "List":
                    case "PartnerService":
                        {
                            break;
                        }

                    default:
                        {
                            tenantProp = type.GetProperty("Tenant");
                            if (objectTableName != "Tenant" && objectTableName != "TenantManagement")
                            {
                                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                            }
                            else
                            {
                                tenantProp = type.GetProperty("Id");
                                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                            }
                            break;
                        }
                }

                WarehouseClassLevelValidator temp = new WarehouseClassLevelValidator(objectTableName, tenant);
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
            return null;
        }


    }

    public interface IWarehouseClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}
