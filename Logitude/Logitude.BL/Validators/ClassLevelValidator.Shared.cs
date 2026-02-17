using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.Validators
{
    public partial class ClassLevelValidator : IClassLevelValidator
    {
        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            if (value != null)
            {
                string objectTableName = context.ObjectType.Name.Substring(0, context.ObjectType.Name.Length - 2);
                Type type = Type.GetType(context.ObjectType.FullName);
                type = value.GetType();
                if (objectTableName == "Shipment")
                {
                    ShipmentPM shipment = value as ShipmentPM;
                    if (shipment != null)
                    {
                        if (shipment.ShipmentLevelCode == "C")
                        {
                            objectTableName = "Master";
                        }
                    }
                }

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
                    case "TenantManagmentPrivateLables":
                        {
                            break;
                        }

                    default:
                        {
                            tenantProp = type.GetProperty("Tenant");
                            if (context.ObjectInstance.GetType() == type)
                            {
                                tenant = (int)tenantProp.GetValue(context.ObjectInstance, null);
                            }
                            break;
                        }
                }

                Validators.ClassLevelValidator temp = new Validators.ClassLevelValidator(objectTableName, tenant);
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

    public interface IClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}