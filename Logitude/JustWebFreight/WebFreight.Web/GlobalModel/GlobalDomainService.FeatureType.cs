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
        public IQueryable<FeatureType> GetFeatureTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureTypeRepository = new FeatureTypeRepository();
            return featureTypeRepository.GetFeatureTypes();
        }

        public IQueryable<FeatureType> GetFeatureTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureTypeRepository = new FeatureTypeRepository();
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