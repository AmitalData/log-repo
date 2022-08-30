using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Logitude.DashboardModule.BL.Validators
{
    public class DashboardModuleValidationClass : IDashboardModuleValidationClass
    {

        bool ready;
        int tenant = 0;
        private string errorMessage = "";

        public DashboardModuleValidationClass(string objectTableName, int tenant)
        {
            this.tenant = tenant;
        }

        public string GetErrorMessage(object value, object instance, string propertyName)
        {
            return "";
        }

        public bool IsValid(object value, object objectInstance, string propertyName)
        {
            return true;
        }

        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            return ValidationResult.Success;
        }

    }

    interface IDashboardModuleValidationClass
    {
        bool IsValid(object value, object objectInstance, string propertyName);
        string GetErrorMessage(object value, object instance, string propertyName);
    }

}