using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;

namespace Logitude.BL.QuoteModel.BusinessUnitFilters
{
    public class QuoteBusinessUnitFilter
    {
        private int myCurrentTenant;
        private string loggedUserEmail;
        public User loggedUser;
        public QuoteBusinessUnitFilter(int tenant)
        {
            this.myCurrentTenant = tenant;
            this.loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);

            UserRepository userRepository = new UserRepository(myCurrentTenant);
            this.loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, myCurrentTenant, true);
        }

        public IQueryable<Quote> RunFilter(IQueryable<Quote> iQueryableData)
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
        public IQueryable<QuoteList> RunFilter(IQueryable<QuoteList> iQueryableData)
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
        public IQueryable<QuoteFollowUpDataView> RunFilter(IQueryable<QuoteFollowUpDataView> iQueryableData)
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
        public Quote RunFilter(Quote entity)
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
        public QuoteList RunFilter(QuoteList entityList)
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

        public List<RoleFeature> GetFeaturesRoles()
        {
            List<RoleFeature> myFeatureRoles = new List<RoleFeature>();

            if (loggedUserEmail != "admin@fnarsoft.com")
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myCurrentTenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Quote", 0, true);

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