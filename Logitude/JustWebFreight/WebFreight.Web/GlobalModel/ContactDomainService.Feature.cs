using System.Collections.Generic;
using System.Web;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using System.Linq;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public List<FeaturePM> GetFeaturesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureQuery = new FeatureQuery();
            return featureQuery.GetFeaturePMsByTenant(tenant);
        }

        public List<FeaturePM> GetSelectedAndUnselectedFeatures(string roleId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<string> allowedPackages = new List<string>();
            string email = HttpContext.Current.User.Identity.Name;
            Logitude.BL.Security.ContactInfo inf = SecurityUtility.GetContactInfo(email, tenant);
            if (inf != null)
            {
                allowedPackages = inf.PackagesCodes;
            }

            featureQuery = new FeatureQuery();
            List<FeaturePM> myResult = featureQuery.GetSelectedAndUnSelectedFeatures(roleId, allowedPackages, tenant);
            return myResult;
        }

        public List<FeaturePM> GetSelectedAndUnselectedPackagesFeatures(string packageCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureQuery = new FeatureQuery();
            List<FeaturePM> myResult = featureQuery.GetSelectedAndUnselectedPackagesFeatures(packageCode, tenant);
            return myResult;
        }

        public List<FeaturePM> GetAllowedFeaturesForLoggedUser(string loggedUserId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (globalContext == null)
            {
                globalContext = GlobalContext.GetContext();
            }

            featureRepository = new FeatureRepository(globalContext);
            featureQuery = new FeatureQuery(featureRepository);

            LoggedUserFeatures loggedUserFeatures = featureQuery.GetAllowedFeaturesForLoggedUser(loggedUserId, tenant);
            List<FeaturePM> myResult = loggedUserFeatures.Features;

            return myResult;
        }

        public List<FeatureList> GetNewFeaturesList()
        {
            int tenant = 0;

            List<FeatureList> myResult = new List<FeatureList>();

            SecurityUtility.AuthenticationOnTenant(tenant);

            featureQuery = new FeatureQuery();
            myResult = featureQuery.GetNewFeaturesList(tenant);

            return myResult;
        }

        public void UpdateFeatureList(FeatureList entityList)
        {

        }

        public void InsertFeature(FeaturePM feature)
        {
            if (globalContext == null)
            {
                globalContext = GlobalContext.GetContext();
            }

            FeatureService service = new FeatureService(globalContext, feature.Tenant);
            service.Create(feature);

            TableLastUpdateClass.UpdateTableHistory(feature.Tenant, "Feature");
        }

        public void UpdateFeature(FeaturePM entityPM)
        {
            if (globalContext == null)
            {
				globalContext = GlobalContext.GetContext();
            }

            FeatureService service = new FeatureService(globalContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Feature");
        }

        public void DeleteFeature(FeaturePM feature)
        {
            featureRepository = new FeatureRepository();
            Feature entity = featureRepository.GetSingleFeature(feature.Id);
            featureRepository.Remove(entity);
        }
    }
}