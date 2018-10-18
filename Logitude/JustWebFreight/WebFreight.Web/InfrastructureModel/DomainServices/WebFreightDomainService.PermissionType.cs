using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<PermissionType> GetPermissionTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            permissionTypesRepository = new PermissionTypeRepository(tenant);
            return permissionTypesRepository.GetPermissionTypes();
        }

        public IQueryable<PermissionType> GetPermissionTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            permissionTypesRepository = new PermissionTypeRepository(tenant);
            return permissionTypesRepository.GetPermissionTypes();
        }

        public void InsertPermissionType(PermissionType permissionType)
        {
            permissionTypesRepository.Add(permissionType);
        }

        public void UpdatePermissionType(PermissionType currentPermissionType)
        {
            permissionTypesRepository.Update(currentPermissionType);
        }

        public void DeletePermissionType(PermissionType permissionType)
        {
            permissionTypesRepository.Remove(permissionType);
        }      
    }
}