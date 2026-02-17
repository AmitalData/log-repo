using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace Logitude.CRM.Data.BusinessUnitFilters
{
    public class ActivityBusinessUnitFilter
    {
        private int myCurrentTenant;
        private string loggedUserEmail;
        private User loggedUser;
        public ActivityBusinessUnitFilter(int tenant)
        {
            this.myCurrentTenant = tenant;
            this.loggedUserEmail = Tools.GetAuthenticatedUser();

            UserRepository userRepository = new UserRepository(myCurrentTenant);
            this.loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, myCurrentTenant, true);
        }

        public IQueryable<Activity> RunFilter(IQueryable<Activity> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    //iQueryableData = iQueryableData;
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                {
                    iQueryableData = iQueryableData.Where(d => d.OwnerId == loggedUser.Id);
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                {
                    iQueryableData = iQueryableData.Where(d => d.BusinessUnitId == loggedUser.BusinessUnitId);
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                {
                    iQueryableData = iQueryableData.Where(d => d.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                }
            }
           
            return iQueryableData;
        }

        private List<RoleFeature> GetFeaturesRoles()
        {
            List<RoleFeature> myFeatureRoles = new List<RoleFeature>();

            if (loggedUserEmail != "admin@fnarsoft.com")
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myCurrentTenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Activity", 0, true);

                FeatureRepository featureRepository = new FeatureRepository();
                Feature myFeature = featureRepository.GetSingleFeatureByCode(objectTable.Id, "READ", myCurrentTenant);
                if (myFeature != null)
                {
					ContactTenantRepository contactTenantRepository = new ContactTenantRepository(myCurrentTenant);
                    RoleFeatureRepository roleFeatureRepository = new RoleFeatureRepository();
                    List<string> myRolesIds = contactTenantRepository.GetUserRolesIds(loggedUser.Id, myCurrentTenant);

                    if (myFeature.IsBusinessUnitEnabled)
                    {                        
                        foreach (string myRoleId in myRolesIds)
                        {
                            RoleFeature myRoleFeature = roleFeatureRepository.GetBusinessUnitFilterRoleFeature(myRoleId, myFeature.Id, myCurrentTenant);
                            if (myRoleFeature != null)
                            {
                                myFeatureRoles.Add(myRoleFeature);
                            }
                        }
                    }
                }
            }

            return myFeatureRoles;
        }
    }
}
