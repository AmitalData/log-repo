using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public IQueryable<ObjectTableType> GetObjectTableTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            objectTableTypeRepository = new ObjectTableTypeRepository(tenant);
            return objectTableTypeRepository.GetObjectTableTypes();
        }

        public IQueryable<ObjectTableType> GetObjectTableTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            objectTableTypeRepository = new ObjectTableTypeRepository(tenant);
            return objectTableTypeRepository.GetObjectTableTypes();
        }

        public void InsertObjectTableType(ObjectTableType objectTableType)
        {
            objectTableTypeRepository.Add(objectTableType);
        }

        public void UpdateObjectTableType(ObjectTableType currentObjectTableType)
        {
            objectTableTypeRepository.Update(currentObjectTableType);
        }

        public void DeleteObjectTableType(ObjectTableType objectTableType)
        {
            objectTableTypeRepository.Remove(objectTableType);
        }
    }
}