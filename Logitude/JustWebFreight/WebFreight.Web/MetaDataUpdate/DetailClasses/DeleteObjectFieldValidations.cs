using System.Collections.Generic;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class DeleteObjectFieldValidations
    {
        public static void DeleteObjectFieldValidation(List<ObjectFieldValidation> tenantObjectFieldValidations,ObjectFieldValidationRepository objectFieldValidationRepository)
        {
            foreach (ObjectFieldValidation validation in tenantObjectFieldValidations)
            {
                objectFieldValidationRepository.Remove(validation);
            }
            objectFieldValidationRepository.SubmitChanges();
        }
    }
}
