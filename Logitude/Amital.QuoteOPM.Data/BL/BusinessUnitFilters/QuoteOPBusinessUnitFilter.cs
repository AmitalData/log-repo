using Amital.QuoteOPM.Data.EntityLists;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.Data.BL.BusinessUnitFilters
{
    public class QuoteOPBusinessUnitFilter
    {
        private int myCurrentTenant;
        private string loggedUserEmail;
        private User loggedUser;
        public QuoteOPBusinessUnitFilter(int tenant/*,string loggedUserEmail*/)
        {
            this.myCurrentTenant = tenant;
            this.loggedUserEmail = /*loggedUserEmail;//*/AuthenticationUtil.GetLoggedUserEmail(tenant);

            UserRepository userRepository = new UserRepository(myCurrentTenant);
            this.loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, myCurrentTenant, true);
        }

        public IQueryable<QuoteOP> RunFilter(IQueryable<QuoteOP> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUserId == loggedUser.Id);
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
            }

            return iQueryableData;
        }
        public IQueryable<QuoteOPList> RunFilter(IQueryable<QuoteOPList> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUserId == loggedUser.Id);
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
            }

            return iQueryableData;
        }
#if false
        public IQueryable<QuoteOPFollowUpDataView> RunFilter(IQueryable<QuoteOPFollowUpDataView> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUserId == loggedUser.Id);
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
            }

            return iQueryableData;
        }


#endif
        public QuoteOP RunFilter(QuoteOP entity)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    return entity;
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                {
                    if (entity.SalesmanUserId == loggedUser.Id)
                    {
                        return entity;
                    }
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                {
                    if (entity.BusinessUnitId == loggedUser.BusinessUnitId)
                    {
                        return entity;
                    }
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                {
                    if (entity.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId))
                    {
                        return entity;
                    }
                }
            }

            return null;
        }
        public QuoteOPList RunFilter(QuoteOPList entityList)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    return entityList;
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                {
                    if (entityList.SalesmanUserId == loggedUser.Id)
                    {
                        return entityList;
                    }
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                {
                    if (entityList.BusinessUnitId == loggedUser.BusinessUnitId)
                    {
                        return entityList;
                    }
                }

                else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                {
                    if (entityList.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId))
                    {
                        return entityList;
                    }
                }
            }

            return null;
        }

        private List<RoleFeature> GetFeaturesRoles()
        {
            List<RoleFeature> myFeatureRoles = new List<RoleFeature>();

            if (loggedUserEmail != "admin@fnarsoft.com")
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myCurrentTenant);
                var objectTable = objectTabelRepository.GetObjectTableByName("Quote", 0, true);

                FeatureRepository featureRepository = new FeatureRepository(myCurrentTenant);
                Feature myFeature = featureRepository.GetSingleFeatureByCode(objectTable.Id, "READ", myCurrentTenant);
                if (myFeature != null)
                {
                    RoleRepository roleRepository = new RoleRepository(myCurrentTenant);
                    RoleFeatureRepository roleFeatureRepository = new RoleFeatureRepository(myCurrentTenant);
                    List<string> myRolesIds = null;
                    if (loggedUser.Tenant == 0 && !loggedUser.IsDistributor)
                    {
                        myRolesIds = roleRepository.GetUserRolesIds(loggedUser.Id, 0);
                    }
                    else
                    {
                        myRolesIds = roleRepository.GetUserRolesIds(loggedUser.Id, myCurrentTenant);
                    }

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
