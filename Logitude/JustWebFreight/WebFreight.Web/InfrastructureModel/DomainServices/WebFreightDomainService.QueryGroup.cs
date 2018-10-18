using System.Linq;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<QueryGroup> GetQueryGroups(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            queryGroupRepository = new QueryGroupRepository(tenant);
            return queryGroupRepository.GetQueryGroups();
        }

        public IQueryable<QueryGroupPM> GetQueryGroupsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            queryGroupRepository = new QueryGroupRepository(tenant);
            queryGroupQuery = new QueryGroupQuery(queryGroupRepository);
            return queryGroupQuery.GetQueryGroupPMs();
        }

        public QueryGroupPM GetSingleQueryGroup(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            queryGroupRepository = new QueryGroupRepository(tenant);
            queryGroupQuery = new QueryGroupQuery(queryGroupRepository);
            return queryGroupQuery.GetSingleQueryGroupPM(code);
        }

        public void InsertQueryGroup(QueryGroup entity)
        {
            queryGroupRepository.Add(entity);
        }

        public void UpdateQueryGroup(QueryGroup currentEntity)
        {
            queryGroupRepository.Update(currentEntity);
        }

        public void DeleteQueryGroup(QueryGroup entity)
        {
            queryGroupRepository.Remove(entity);
        }
    }
}