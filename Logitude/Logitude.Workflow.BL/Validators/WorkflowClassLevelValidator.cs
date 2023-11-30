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

namespace Logitude.Workflow.BL.Validators
{
    public partial class WorkflowClassLevelValidator : IWorkflowClassLevelValidator
    {
        int tenant;
        string objectTableName;

        public WorkflowClassLevelValidator(string objectTableName, int tenant)
        {
            this.objectTableName = objectTableName;
            this.tenant = tenant;
        }

        public string GetErrorMessage(object instance, string property)
        {
            return "";
        }

        public List<string> GetErrorsInObject(object instance)
        {
            throw new NotImplementedException();
        }

        public static ValidationResult ValidateClass(object value, ValidationContext context)
        {
            return ValidationResult.Success;
        }


        public bool IsValid(object value, object instance, string propertyName)
        {
            return true;
        }
    }


    public interface IWorkflowClassLevelValidator
    {
        bool IsValid(object value, object instance, string propertyName);
        string GetErrorMessage(object instance, string property);
        List<string> GetErrorsInObject(object instance);
    }
}
