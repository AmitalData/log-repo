using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<EntityDate> GetEntityDates(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityDateRepository = new EntityDateRepository(tenant);
            return entityDateRepository.GetEntityDates();
        }

        public IQueryable<EntityDate> GetEntityDatesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            entityDateRepository = new EntityDateRepository(tenant);
            return entityDateRepository.GetEntityDates();
        }

        public void InsertEntityDate(EntityDate entity)
        {
            entityDateRepository.Add(entity);
        }

        public void UpdateEntityDate(EntityDate currentEntity)
        {
            entityDateRepository.Update(currentEntity);
        }

        public void DeleteEntityDate(EntityDate entity)
        {
            entityDateRepository.Remove(entity);
        }
    }
}