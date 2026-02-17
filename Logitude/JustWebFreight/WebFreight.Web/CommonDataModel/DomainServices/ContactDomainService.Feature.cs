using System.Collections.Generic;
using System.Web;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public List<FeaturePM> GetFeaturesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureQuery = new FeatureQuery(tenant);
            return featureQuery.GetFeaturePMsByTenant(tenant);
        }

        public List<FeaturePM> GetSelectedAndUnselectedFeatures(string roleId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<string> allowedPackages = new List<string>();
            string email = HttpContext.Current.User.Identity.Name;
            ContactInfo inf = SecurityUtility.GetContactInfo(email, tenant);
            if (inf != null)
            {
                allowedPackages = inf.PackagesCodes;
            }

            featureQuery = new FeatureQuery(tenant);
            List<FeaturePM> myResult = featureQuery.GetSelectedAndUnSelectedFeatures(roleId, allowedPackages, tenant);
            return myResult;
        }

        public List<FeaturePM> GetSelectedAndUnselectedPackagesFeatures(string packageCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            featureQuery = new FeatureQuery(tenant);
            List<FeaturePM> myResult = featureQuery.GetSelectedAndUnselectedPackagesFeatures(packageCode, tenant);
            return myResult;
        }

        public List<FeaturePM> GetAllowedFeaturesForLoggedUser(string loggedUserId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            featureRepository = new FeatureRepository(objectContext);
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

            featureQuery = new FeatureQuery(tenant);
            myResult = featureQuery.GetNewFeaturesList(tenant);

            return myResult;
        }

        public void UpdateFeatureList(FeatureList entityList)
        {

        }

        public void InsertFeature(FeaturePM feature)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(feature.Tenant);
            }

            FeatureService service = new FeatureService(objectContext, feature.Tenant);
            service.Create(feature);

            TableLastUpdateClass.UpdateTableHistory(feature.Tenant, "Feature");
        }

        public void UpdateFeature(FeaturePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            FeatureService service = new FeatureService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Feature");
        }

        public void DeleteFeature(FeaturePM feature)
        {
            featureRepository = new FeatureRepository(feature.Tenant);
            Feature entity = featureRepository.GetSingleFeature(feature.Id);
            featureRepository.Remove(entity);
        }
    }
}