using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.BusinessUnitFilters
{
    public class CustomerBusinessUnitFilter
    {
        private int myCurrentTenant;
        private string loggedUserEmail;
        private User loggedUser;
        public CustomerBusinessUnitFilter(int tenant)
        {
            this.myCurrentTenant = tenant;

            this.loggedUserEmail = AuthenticationUtil.AuthenticatedUserEmail; // Run using worker role report and document
            if (string.IsNullOrEmpty(this.loggedUserEmail)) this.loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();

            UserRepository userRepository = new UserRepository(myCurrentTenant);
            this.loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, myCurrentTenant, true);
        }

        public IQueryable<Customer> RunFilter(IQueryable<Customer> iQueryableData)
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
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUser.BusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUser.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<CustomersDataView> RunFilter(IQueryable<CustomersDataView> iQueryableData)
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
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId != null && d.SalesmanBusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId != null && d.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<CustomerList> RunFilter(IQueryable<CustomerList> iQueryableData)
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
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<CustomerPM> RunFilter(IQueryable<CustomerPM> iQueryableData)
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
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<Card> RunFilter(IQueryable<Card> iQueryableData)
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
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUser.BusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUser.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<CardList> RunFilter(IQueryable<CardList> iQueryableData)
        {
            var iQueryableData_NotCustomers = iQueryableData.Where(d => d.PartnerTypeId != "CS" && d.PartnerTypeId != "PO");

            iQueryableData = iQueryableData.Where(d => d.PartnerTypeId == "CS" || d.PartnerTypeId == "PO");

            if (iQueryableData.Count() > 0)
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
                            iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId == loggedUser.BusinessUnitId);
                        }

                        else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                        {
                            iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                        }
                    }
                }
            }

            iQueryableData = iQueryableData.Concat(iQueryableData_NotCustomers);

            return iQueryableData;
        }

        public List<CardList> RunFilter(List<CardList> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanUserId == loggedUser.Id).ToList();
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId == loggedUser.BusinessUnitId).ToList();
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId)).ToList();
                    }
                }
            }

            return iQueryableData;
        }

        public bool IsCustomerAllowed(CustomerPM entityPM)
        {
            bool myResult = true;

            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        if (entityPM.SalesmanUserId == null)
                        {
                            myResult = false;
                        }

                        else if (entityPM.SalesmanUserId == loggedUser.Id)
                        {
                            myResult = true;
                        }

                        else
                        {
                            myResult = false;
                        }
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                    {
                        if (entityPM.SalesmanUserId == null)
                        {
                            myResult = false;
                        }

                        else if (entityPM.SalesmanBusinessUnitId == loggedUser.BusinessUnitId)
                        {
                            myResult = true;
                        }

                        else
                        {
                            myResult = false;
                        }
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        if (entityPM.SalesmanUserId == null)
                        {
                            myResult = false;
                        }

                        else if (entityPM.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId))
                        {
                            myResult = true;
                        }

                        else
                        {
                            myResult = false;
                        }
                    }
                }
            }

            return myResult;
        }

        public void SetBlockedQuickSearch(List<CustomerList> myList)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    foreach (CustomerList item in myList) //.Where(d => d.SalesmanUserId != null)
                    {
                        if (string.IsNullOrEmpty(item.SalesmanUserId))
                        {
                            item.IsBlockedQuickSearch = true;
                        }

                        else
                        {
                            if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                            {
                                if (item.SalesmanUserId != loggedUser.Id)
                                {
                                    item.IsBlockedQuickSearch = true;
                                }
                            }

                            else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                            {
                                if (item.SalesmanBusinessUnitId != loggedUser.BusinessUnitId)
                                {
                                    item.IsBlockedQuickSearch = true;
                                }
                            }

                            else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                            {
                                if (!item.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId))
                                {
                                    item.IsBlockedQuickSearch = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetBlockedBusinessUnit(CustomerList entityList)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (entityList.SalesmanUserId != null)
                    {
                        if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                        {
                            if (entityList.SalesmanUserId != loggedUser.Id)
                            {
                                entityList.IsBlockedQuickSearch = true;
                                entityList.IsBlockedBusinessUnit = true;
                            }
                        }

                        else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                        {
                            if (entityList.SalesmanBusinessUnitId != loggedUser.BusinessUnitId)
                            {
                                entityList.IsBlockedQuickSearch = true;
                                entityList.IsBlockedBusinessUnit = true;
                            }
                        }

                        else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                        {
                            if (!entityList.SalesmanBusinessUnitId.StartsWith(loggedUser.BusinessUnitId))
                            {
                                entityList.IsBlockedQuickSearch = true;
                                entityList.IsBlockedBusinessUnit = true;
                            }
                        }
                    }
                }
            }
        }

        private List<RoleFeature> GetFeaturesRoles()
        {
            List<RoleFeature> myFeatureRoles = new List<RoleFeature>();

            if (loggedUserEmail != "admin@fnarsoft.com")
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(myCurrentTenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Customer", 0, true);

                FeatureRepository featureRepository = new FeatureRepository(myCurrentTenant);
                Feature myFeature = featureRepository.GetSingleFeatureByCode(objectTable.Id, "READ", myCurrentTenant);
                if (myFeature != null)
                {
                    RoleRepository roleRepository = new RoleRepository(myCurrentTenant);
                    RoleFeatureRepository roleFeatureRepository = new RoleFeatureRepository(myCurrentTenant);
                    List<string> myRolesIds = roleRepository.GetUserRolesIds(loggedUser.Id, myCurrentTenant);

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

        public IQueryable<CustomerProduct> RunFilter(IQueryable<CustomerProduct> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUserId == loggedUser.Id);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUser.BusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUser.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<CustomerProductActualData> RunFilter(IQueryable<CustomerProductActualData> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUserId == loggedUser.Id);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUser.BusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUser.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }

        public IQueryable<CustomerAdditionalService> RunFilter(IQueryable<CustomerAdditionalService> iQueryableData)
        {
            List<RoleFeature> myFeatureRoles = this.GetFeaturesRoles();

            if (myFeatureRoles.Count > 0)
            {
                if (!myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any())
                {
                    if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUserId == loggedUser.Id);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUser.BusinessUnitId == loggedUser.BusinessUnitId);
                    }

                    else if (myFeatureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
                    {
                        iQueryableData = iQueryableData.Where(d => d.Customer.SalesmanUser.BusinessUnitId.StartsWith(loggedUser.BusinessUnitId));
                    }
                }
            }

            return iQueryableData;
        }
    }
}