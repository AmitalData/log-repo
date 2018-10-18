using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<FeatureType> GetFeatureTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureTypeRepository = new FeatureTypeRepository(tenant);
            return featureTypeRepository.GetFeatureTypes();
        }

        public IQueryable<FeatureType> GetFeatureTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureTypeRepository = new FeatureTypeRepository(tenant);
            return featureTypeRepository.GetFeatureTypes();
        }

        public void InsertFeatureType(FeatureType featureType)
        {
            featureTypeRepository.Add(featureType);
        }

        public void UpdateFeatureType(FeatureType currentFeatureType)
        {
            featureTypeRepository.Update(currentFeatureType);
        }

        public void DeleteFeatureType(FeatureType featureType)
        {
            featureTypeRepository.Remove(featureType);
        }
    }
}