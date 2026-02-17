using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<RoleType> GetRoleTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            roleTypeRepository = new RoleTypeRepository(tenant);
            return roleTypeRepository.GetRoleTypes();
        }

        public IQueryable<RoleType> GetRoleTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            roleTypeRepository = new RoleTypeRepository(tenant);
            return roleTypeRepository.GetRoleTypes();
        }

        public void InsertRoleType(RoleType roleType)
        {
            roleTypeRepository.Add(roleType);
        }

        public void UpdateRoleType(RoleType currentRoleType)
        {
            roleTypeRepository.Update(currentRoleType);
        }

        public void DeleteRoleType(RoleType roleType)
        {
            roleTypeRepository.Remove(roleType);
        }
    }
}