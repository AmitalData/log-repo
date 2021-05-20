using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Infrastructure.BL.Validators
{
    public partial class BIReportClassLevelValidator : IInfrastructureClassLevelValidator
    {
        int tenant;
        string objectTableName;
        string errorMessage = "";

        public BIReportClassLevelValidator(string objectTableName, int tenant)
        {
            this.objectTableName = objectTableName;
            this.tenant = tenant;
        }

        public bool IsValid(object value, object instance, string propertyName)
        {
            errorMessage = "";
            if (value != null)
            {
                string biReportName = (string)value.GetType().GetProperty("Name").GetValue(value);
                if (biReportName.Contains("#") || biReportName.Contains("&") || biReportName.Contains("+"))
                {
                    errorMessage = "Name field can't contain the following special characters # & +";
                    return false;
                }
            }

            return true;
        }

        public string GetErrorMessage(object value, object instance, string property)
        {
            if (!String.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }
            return "";
        }

        public System.Collections.Generic.List<string> GetErrorsInObject(object instance)
        {
            throw new NotImplementedException();
        }

        public string GetErrorMessage(object instance, string property)
        {
            return "";
        }
    }
}
