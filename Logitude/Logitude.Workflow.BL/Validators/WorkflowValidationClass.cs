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
    public partial class WorkflowValidationClass : IWorkflowValidationClass
    {


        bool ready;
        int tenant = 0;
        private string errorMessage = "";
        List<ObjectField> objectFieldList;

        public WorkflowValidationClass(string objectTableName, int tenant)
        {
            this.tenant = tenant;

            objectFieldList = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList();
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

    interface IWorkflowValidationClass
    {
        bool IsValid(object value, object objectInstance, string propertyName);
        string GetErrorMessage(object value, object instance, string propertyName);
    }
}
