using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public IQueryable<RoleFeaturePM> GetRoleFeaturesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            roleFeatureQuery = new RoleFeatureQuery(tenant);
            return roleFeatureQuery.GetRoleFeaturePMsByTenant(tenant);
        }

        public void InsertRoleFeature(RoleFeaturePM roleFeature)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(roleFeature.Tenant);
            }

            RoleFeatureService service = new RoleFeatureService(objectContext, roleFeature.Tenant);
            service.Create(roleFeature);

            TableLastUpdateClass.UpdateTableHistory(roleFeature.Tenant, "RoleFeature");
        }

        public void UpdateRoleFeature(RoleFeaturePM currentRoleFeature)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentRoleFeature.Tenant);
            }

            RoleFeatureService service = new RoleFeatureService(objectContext, currentRoleFeature.Tenant);
            service.Update(currentRoleFeature);

            TableLastUpdateClass.UpdateTableHistory(currentRoleFeature.Tenant, "RoleFeature");
        }

        public void DeleteRoleFeature(RoleFeaturePM roleFeature)
        {
            roleFeatureRepository = new RoleFeatureRepository(roleFeature.Tenant);
            RoleFeature entity = roleFeatureRepository.GetSingleRoleFeature(roleFeature.Id);
            roleFeatureRepository.Remove(entity);
        }
    }
}