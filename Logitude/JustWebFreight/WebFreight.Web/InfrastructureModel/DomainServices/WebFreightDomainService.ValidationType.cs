using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<ValidationType> GetValidationTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validationTypesRepository = new ValidationTypeRepository(tenant);
            return validationTypesRepository.GetValidationTypes();
        }

        public IQueryable<ValidationType> GetValidationTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validationTypesRepository = new ValidationTypeRepository(tenant);
            return validationTypesRepository.GetValidationTypesByTenant(tenant).Where(d => d.Tenant == tenant);
        }

        public void InsertValidationType(ValidationType entity)
        {
            validationTypesRepository.Add(entity);
        }

        public void UpdateValidationType(ValidationType currentEntity)
        {
            validationTypesRepository.Update(currentEntity);
        }

        public void DeleteValidationType(ValidationType entity)
        {
            validationTypesRepository.Remove(entity);
        }
    }
}