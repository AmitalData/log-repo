using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
	{
        public IQueryable<RoleType> GetRoleTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            roleTypeRepository = new RoleTypeRepository();
            return roleTypeRepository.GetRoleTypes();
        }

        public IQueryable<RoleType> GetRoleTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            roleTypeRepository = new RoleTypeRepository();
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