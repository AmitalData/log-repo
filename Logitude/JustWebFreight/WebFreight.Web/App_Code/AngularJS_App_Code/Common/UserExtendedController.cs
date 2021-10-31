using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class UserExtendedController : ApiController
    {
        public HttpResponseMessage GetUsersWorkspaceSummary(int tenant, string type)
        {
            Authentication();

            UsersWorkspaceSummary myResult = new UsersWorkspaceSummary() { Id = tenant };

            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);

            if (tenantManagement != null)
            {
                myResult.LicensesCount = tenantManagement.NumberOfUsers == null ? 0 : tenantManagement.NumberOfUsers.Value;
            }

            UserRepository userRepository = new UserRepository(tenant);

            IQueryable<User> iQueryableData = userRepository.GetUsers(tenant);

            myResult.AllUsersCount = iQueryableData.Count();
            myResult.ActiveUsersCount = iQueryableData.Where(d => d.Contact.InActive == false).Count();
            myResult.InactiveUsersCount = iQueryableData.Where(d => d.Contact.InActive == true).Count();
            myResult.ActiveLicensedCount = iQueryableData.Where(d => d.Contact.InActive == false && d.LicencedUser == true).Count();
            myResult.ActiveNotLicensedCount = iQueryableData.Where(d => d.Contact.InActive == false && d.LicencedUser == false).Count();
            myResult.ActiveNotAdditionalUsersCount = iQueryableData.Where(d => d.Contact.InActive == false && d.AdditionalPackagesOnly == false).Count();

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        public HttpResponseMessage GetUsersWorkspaceRecentLogins(int tenant)
        {
            List<UsersWorkspaceRecentItem> myResult = new List<UsersWorkspaceRecentItem>();

            UserLoginLogRepository userLoginLogRepository = new UserLoginLogRepository(tenant);
            UserLastLoginRepository userLastLoginRepository = new UserLastLoginRepository(tenant);

            IQueryable<UserLastLogin> iQueryable = userLastLoginRepository.GetUsersWorkspaceLastLogins(tenant);
            iQueryable = iQueryable.Where(d => d.User.Contact.InActive == false);

            if (tenant != 0)
            {
                bool isCustomerCare = false;

                UserRepository userRepository = new UserRepository(tenant);
                string email = HttpContext.Current.User.Identity.Name;

                if (email != null)
                {
                    User loggedUser = userRepository.GetSingleUserByEmail(email, tenant, false);
                    if (loggedUser != null && loggedUser.Tenant == 0)
                    {
                        isCustomerCare = !loggedUser.IsDistributor;
                    }
                }

                if (!isCustomerCare)
                {
                    iQueryable = iQueryable.Where(d => d.User.Tenant != 0);
                }
            }

            List<UserLastLogin> lastLoginsList = iQueryable.ToList();

            if (lastLoginsList.Count > 0)
            {
                List<string> lastLoginsUserIdsList = lastLoginsList.Select(s => s.Id).ToList();

                List<UserLoginLog> allLoginLogs = (from d in userLoginLogRepository.context.UserLoginLogs.Include("User")
                                                   where d.Tenant == tenant && lastLoginsUserIdsList.Contains(d.UserId)
                                                   select d).ToList();

                foreach (UserLastLogin item in lastLoginsList)
                {
                    UsersWorkspaceRecentItem myRecord = new UsersWorkspaceRecentItem()
                    {
                        Id = item.Id,
                        Username = item.User == null ? "" : (item.User.Contact == null ? "" : item.User.Contact.EnglishName),
                        BusinessUnit = item.User == null ? "" : (item.User.BusinessUnit == null ? "" : item.User.BusinessUnit.Name),
                        IsCustomerCareUser = item.User != null ? (item.User.Tenant == 0 && tenant != 0) : false,
                    };

                    if (item.User.Tenant != tenant)
                    {
                        myRecord.BlockEditUser = true;
                    }

                    UserLoginLog myLog = (from d in allLoginLogs
                                          where d.Tenant == tenant && d.UserId == item.Id
                                          orderby d.LocalDateTime descending
                                          select d).FirstOrDefault();

                    if (myLog != null)
                    {
                        myRecord.IP = myLog.IP;
                        myRecord.LastAccessDate = myLog.LocalDateTime;
                    }

                    myResult.Add(myRecord);
                }
            }
            myResult = myResult.OrderByDescending(o => o.LastAccessDate).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        public HttpResponseMessage GetCustomQueriesList(string userId, string objectTableId, int tenant)
        {
            Authentication();
            QueryQuery queryQuery = new QueryQuery(tenant);
            List<QueryPM> queryPM = queryQuery.GetQueriesByObjectTableAndUserId(userId, objectTableId, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, queryPM);
        }

        public HttpResponseMessage GetUserLoginHistory(string userId, int tenant)
        {
            UserLoginLogQuery userLoginLogQuery = new UserLoginLogQuery(tenant);
            List<UserLoginLogList> iQueryable = userLoginLogQuery.GetUserLoginLogListsByTenant(userId, tenant).OrderByDescending(d => d.GMTDateTime).Take(1000).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, iQueryable.OrderByDescending(o => o.LocalDateTime));
        }

        public HttpResponseMessage GetUpdateUser(string userId, bool setAngularAsDefault, int tenant)
        {
            UserRepository userRepository = new UserRepository(tenant);
            User user = userRepository.GetSingleUser(userId, tenant);
            bool isSaveUser = false;
            if (user != null)
            {
                if (user.SetAngularAsDefault != setAngularAsDefault)
                {
                    user.SetAngularAsDefault = setAngularAsDefault;
                    userRepository.Update(user);
                    userRepository.SubmitChanges();
                    isSaveUser = true;

                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, isSaveUser);
        }

        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("User", "READ", authToken.Tenant);
        }

        public HttpResponseMessage GetUserLicenses()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                UserLicenseRepository myRepository = new UserLicenseRepository(tenant);
                UserLicenseQuery myQuery = new UserLicenseQuery(myRepository);
                IQueryable<UserLicensePM> myResult = myQuery.GetUserLicensesByTenant(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetUsersTwoFactorAuthenticationEnabled(int tenant)
        {
            Authentication();

            UserRepository userRepository = new UserRepository(tenant);
            List<string> iQueryableData = userRepository.GetUsersTwoFactorAuthenticationEnabled(tenant).Select(a => a.Id).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, iQueryableData);
        }

        public HttpResponseMessage PostUpdateTwoFactorAuthenticationEnabled(int tenant, string userIds)
        {
            Authentication();
            UserRepository userRepository = new UserRepository(tenant);

            string[] ids = userIds.Split(',');

            List<User> twoFactorUsers = (from a in userRepository.context.Users
                                         where ids.Contains(a.Id)
                                         select a).ToList();

            List<User> oldUsers = userRepository.GetUsersTwoFactorAuthenticationEnabled(tenant).ToList();

            foreach (User user in twoFactorUsers)
            {
                user.IsTwoFactorAuthenticationEnabled = true;
                userRepository.Update(user);
            }

            foreach (User user in oldUsers)
            {
                if (!twoFactorUsers.Any(u => u.Id == user.Id))
                {
                    user.IsTwoFactorAuthenticationEnabled = false;
                    userRepository.Update(user);
                }
            }

            userRepository.SubmitChanges();

            return Request.CreateResponse(HttpStatusCode.OK, userIds);
        }

        [HttpGet]
        public HttpResponseMessage GetUserExtendedByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                SecurityUtility.CheckContactFeature("User", "READ", authToken.Tenant);

                List<UserExtendedList> totalDataList = new List<UserExtendedList>();

                #region Tenant Management & Tenant Management Licenses
                IGlobalContext globalContext = GlobalContext.GetContext();
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository(globalContext);
                TenantManagementLicenseRepository myTenantRepository = new TenantManagementLicenseRepository(globalContext);

                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                IQueryable<TenantManagementLicense> myTenantResult = myTenantRepository.GetTenantManagementLicenses(tenant);
                List<TenantManagementLicense> tenantManagementLicenses  = myTenantResult.ToList();
                #endregion

                #region Users Licenses
                List<string> Codes = tenantManagementLicenses.Select(s => s.PackageCode).ToList();

                UserLicenseRepository myRepository = new UserLicenseRepository(tenant);
                IQueryable<UserLicense> myResult = myRepository.GetUserLicenses(tenant);
                List<UserLicense> usersLicenses = (from d in myResult
                                 where Codes.Contains(d.PackageCode)
                                 select d).ToList();
                #endregion

                #region Users
                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "User",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Users",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> UserObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("User", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";

                        ObjectField field = UserObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }
                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = UserObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }

                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                UserRepository userRepository = new UserRepository(MyContext);
                UserQuery userQuery = new UserQuery(userRepository);

                IQueryable<User> entityPocos = userRepository.GetUsers(tenant);
                //IQueryable<User> entityPocos = allUsers.Where(d => !d.Contact.InActive);

                //if (queryOperations.PageIndex == 0)
                //{
                //    IQueryable<User> entityPocos_inactive = allUsers.Where(d => d.Contact.InActive);
                //    List<string> inactiveIds = entityPocos_inactive.Select(s => s.Id).ToList();
                //    List<string> inactiveIds_Licenses = usersLicenses.Where(d => inactiveIds.Contains(d.UserId)).Select(s => s.UserId).ToList();
                //    entityPocos_inactive = entityPocos_inactive.Where(d => inactiveIds_Licenses.Contains(d.Id));

                //    List<UserList> users_inactive = userQuery.GetIQueryableEntityList(entityPocos_inactive).ToList();

                //    foreach (UserList user in users_inactive)
                //    {
                //        UserExtendedList myResultItem = new UserExtendedList()
                //        {
                //            Id = user.Id,
                //            Tenant = user.Tenant,
                //            EnglishName = user.EnglishName,
                //            Email = user.Email,
                //            AdditionalPackagesOnly = user.AdditionalPackagesOnly,
                //            SearchFields = user.EnglishName + "," + user.Email,
                //            InActive = user.InActive,
                //        };

                //        if (tenantManagement.MainAdditionalPackageApplied)
                //        {
                //            this.SetUserLicenseExists(myResultItem, 0, tenantManagement.PackageCode);
                //        }

                //        int index = 0;
                //        foreach (TenantManagementLicense license in tenantManagementLicenses.OrderBy(o => o.PackageCode))
                //        {
                //            index++;
                //            if (index <= 10)
                //            {
                //                UserLicense item = usersLicenses.Where(d => d.PackageCode == license.PackageCode && d.UserId == user.Id).FirstOrDefault();
                //                if (item != null)
                //                {
                //                    this.SetUserLicenseExists(myResultItem, index, license.PackageCode);
                //                }
                //            }
                //        }

                //        totalDataList.Add(myResultItem);
                //    }
                //}
                
                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                UserCustomFilter customfilters = new UserCustomFilter(tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);

                entityPocos = genericFilter.GetFilteredQuery<User>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;
                IQueryable<UserList> entityLists = userQuery.GetIQueryableEntityList(entityPocos);

                entityLists = genericFilter.GetFilteredQuery<UserList>(listQueryOperation, entityLists);
                List<UserList> usersList = entityLists.ToList();
                
                #endregion

                foreach (UserList user in usersList)
                {
                    bool addUser = true;
                    if (user.InActive)
                    {
                        addUser = usersLicenses.Where(d => d.UserId == user.Id).Any();
                    }

                    if (addUser)
                    {
                        UserExtendedList myResultItem = new UserExtendedList()
                        {
                            Id = user.Id,
                            Tenant = user.Tenant,
                            EnglishName = user.EnglishName,
                            Email = user.Email,
                            AdditionalPackagesOnly = user.AdditionalPackagesOnly,
                            SearchFields = user.EnglishName + "," + user.Email,
                            InActive = user.InActive,
                        };

                        if (tenantManagement.MainAdditionalPackageApplied)
                        {
                            this.SetUserLicenseExists(myResultItem, 0, tenantManagement.PackageCode);
                        }

                        int index = 0;
                        foreach (TenantManagementLicense license in tenantManagementLicenses.OrderBy(o => o.PackageCode))
                        {
                            index++;
                            if (index <= 10)
                            {
                                UserLicense item = usersLicenses.Where(d => d.PackageCode == license.PackageCode && d.UserId == user.Id).FirstOrDefault();
                                if (item != null)
                                {
                                    this.SetUserLicenseExists(myResultItem, index, license.PackageCode);
                                }
                            }
                        }

                        totalDataList.Add(myResultItem);
                    }
                }

                IQueryable<UserExtendedList> iQueryableData = totalDataList.AsQueryable();

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    string mySortField = queryOperations.SortByColumnName;
                    if (queryOperations.SortByColumnName.Contains(","))
                    {
                        string index_string = queryOperations.SortByColumnName.Split(',')[1];
                        mySortField = "IsChecked" + Convert.ToInt32(index_string);
                        queryOperations.SortByColumnName = mySortField;
                    }

                    PropertyInfo propInfo = typeof(UserExtendedList).GetProperty(queryOperations.SortByColumnName);
                    List<UserExtendedListField> fields = this.BuildUserExtendedListFields();

                    UserExtendedListField userField = (from a in fields
                                                       where a.FieldName == mySortField
                                                       select a).FirstOrDefault();

                    if (userField != null)
                    {
                        switch (userField.DataType.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    iQueryableData = sortClass.GetSorterQuery<UserExtendedList, string>(queryOperations, iQueryableData);
                                    break;
                                }
                            case "boolean":
                                {
                                    iQueryableData = sortClass.GetSorterQuery<UserExtendedList, bool>(queryOperations, iQueryableData);
                                    break;
                                }
                            default:
                                {
                                    iQueryableData = iQueryableData.OrderBy(d => d.EnglishName);
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    iQueryableData = iQueryableData.OrderBy(d => d.EnglishName);
                }

                if (!queryOperations.GetAll)
                {
                    iQueryableData = iQueryableData.Skip(skippedEntities);
                    iQueryableData = iQueryableData.Take(queryOperations.PageSize);
                }

                totalDataList = iQueryableData.ToList();

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    response.Count = totalDataList.Count;
                }

                response.Result = totalDataList;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void SetUserLicenseExists(UserExtendedList myResultItem, int index, string packageCode)
        {
            switch (index)
            {
                case 0:
                    {
                        myResultItem.IsChecked0 = myResultItem.AdditionalPackagesOnly ? false : true;
                        myResultItem.PackageCode0 = packageCode;
                        break;
                    }

                case 1:
                    {
                        myResultItem.IsChecked1 = true;
                        myResultItem.PackageCode1 = packageCode;
                        break;
                    }

                case 2:
                    {
                        myResultItem.IsChecked2 = true;
                        myResultItem.PackageCode2 = packageCode;
                        break;
                    }

                case 3:
                    {
                        myResultItem.IsChecked3 = true;
                        myResultItem.PackageCode3 = packageCode;
                        break;
                    }

                case 4:
                    {
                        myResultItem.IsChecked4 = true;
                        myResultItem.PackageCode4 = packageCode;
                        break;
                    }

                case 5:
                    {
                        myResultItem.IsChecked5 = true;
                        myResultItem.PackageCode5 = packageCode;
                        break;
                    }

                case 6:
                    {
                        myResultItem.IsChecked6 = true;
                        myResultItem.PackageCode6 = packageCode;
                        break;
                    }

                case 7:
                    {
                        myResultItem.IsChecked7 = true;
                        myResultItem.PackageCode7 = packageCode;
                        break;
                    }

                case 8:
                    {
                        myResultItem.IsChecked8 = true;
                        myResultItem.PackageCode8 = packageCode;
                        break;
                    }

                case 9:
                    {
                        myResultItem.IsChecked9 = true;
                        myResultItem.PackageCode9 = packageCode;
                        break;
                    }

                case 10:
                    {
                        myResultItem.IsChecked10 = true;
                        myResultItem.PackageCode10 = packageCode;
                        break;
                    }
            }
        }
        private List<UserExtendedListField> BuildUserExtendedListFields()
        {
            List<UserExtendedListField> result = new List<UserExtendedListField>();

            result.Add(new UserExtendedListField() { FieldName = "EnglishName", DataType = "Text" });
            result.Add(new UserExtendedListField() { FieldName = "Email", DataType = "Text" });
            result.Add(new UserExtendedListField() { FieldName = "SearchFields", DataType = "nText" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked0", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked1", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked2", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked3", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked4", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked5", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked6", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked7", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked8", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked9", DataType = "Boolean" });
            result.Add(new UserExtendedListField() { FieldName = "IsChecked10", DataType = "Boolean" });

            return result;
        }

        public HttpResponseMessage Put(UserPM entityPM)// JustFor LogBox,Please Call Rabaia
        {

            try
            {

                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("User", "UPDATE", authToken.Tenant);

                    string entityName = "User" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "UserPM" + entityPM.Id + entityPM.Tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    UserQuery myQuery = new UserQuery(authToken.Tenant);
                    var myuser = myQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);
                    myuser.ShowLogBoxToolTip = true;
                    UserService service = new UserService(MyContext, entityPM.Tenant);
                    service.Update(myuser);

                    //scope.Complete();


                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                //}
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetAnonymizationUser(string userId)
        {
            try
            {
                 string token = HttpContext.Current.Request.Headers["Token"];
                 AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                 SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                 ContactRepository contactRepository = new ContactRepository(authToken.Tenant);
                 Contact contact = contactRepository.GetSingleContact(userId, authToken.Tenant);

                 bool result = false;
                if (contact != null)
                {
                    #region Contact
                    string entityName = "ContactPM" + contact.Email + authToken.Tenant;
                    string entityName2 = "ContactPM" + contact.Name + authToken.Tenant;

                    CacheManager.CacheWrapper.Remove(entityName);
                    CacheManager.CacheWrapper.Remove(entityName2);

                    contact.Name = "xxx";
                    contact.LocalName = "xxx";
                    contact.EnglishName = "xxx";
                    contact.Email = "xxx@" + contact.Id + ".com";
                    contact.Mobile = null;
                    contact.BusinessPhone = null;
                    contact.Fax = null;
                    contact.Anniversary = null;
                    contact.AnniversaryReminder = false;
                    contact.Birthday = null;
                    contact.BirthDayOfYear = null;
                    contact.Notes = null;
                    contact.Position = null;
                    contact.SearchFields  = contact.Email + "," + contact.EnglishName;
                    contact.SignatureHtml = null;
                    contact.Signature = null;
                    contact.ImageDetailId = null;
                    contact.InActive = true;

                    contactRepository.Update(contact);
                    contactRepository.SubmitChanges();

                    #endregion

                    #region GlobalContact
                    GlobalContactRepository globalContactRepository = new GlobalContactRepository();
                    GlobalContact globalContact = globalContactRepository.GetSingleGlobalContact(contact.Id);
                    if (globalContact != null)
                    {
                        globalContact.Email = contact.Email;
                        globalContact.InActive = true;
                        globalContactRepository.Update(globalContact);
                        globalContactRepository.SubmitChanges();
                    }

                    #endregion

                    #region User
                    UserRepository userRepository = new UserRepository(authToken.Tenant);
                    User user = userRepository.GetSingleUser(userId, authToken.Tenant);
                    if (user != null)
                    {
                        user.Notes = null;
                        user.SearchFields = contact.SearchFields;
                        userRepository.Update(user);
                        userRepository.SubmitChanges();
                        entityName = "UserPM" + user.Id + authToken.Tenant;
                        entityName2 = "UserPM" + user.Code + authToken.Tenant;
                        CacheManager.CacheWrapper.Remove(entityName);
                        CacheManager.CacheWrapper.Remove(entityName2);
                    }
                    #endregion

                    result = true;
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetUserLicensesCountForUser(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                UserLicenseRepository myRepository = new UserLicenseRepository(tenant);
                List<UserLicense> myResult = myRepository.GetUserLicensesByUserId(userId,tenant);
                int count = myResult.Count;

                return Request.CreateResponse(HttpStatusCode.OK, count);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAddUserToReleaseNotesUsers(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                UsersReleaseNotesDisplayRepository usersReleaseNotesDisplayRepository = new UsersReleaseNotesDisplayRepository(tenant);
                UsersReleaseNotesDisplay usersReleaseNotesDisplay = new UsersReleaseNotesDisplay()
                {
                    Id = IdCounter.GetNumber("UsersReleaseNotesDisplay", tenant).ToString(),
                    Tenant = tenant,
                    UserId = userId,
                };

                usersReleaseNotesDisplayRepository.Add(usersReleaseNotesDisplay);
                usersReleaseNotesDisplayRepository.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, userId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCheckUserReleaseNotesToolTip(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                UsersReleaseNotesDisplayRepository usersReleaseNotesDisplayRepository = new UsersReleaseNotesDisplayRepository(tenant);
                UsersReleaseNotesDisplay usersReleaseNotesDisplay = usersReleaseNotesDisplayRepository.GetSingleUsersReleaseNotesDisplayByUserId(userId, tenant);

                bool show = true;
                if(usersReleaseNotesDisplay != null)
                {
                    show = false;
                }

                return Request.CreateResponse(HttpStatusCode.OK, show);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}

public class UserExtendedList
{
    [Key]
    public string Id { get; set; }
    public int Tenant { get; set; }
    public string EnglishName { get; set; }
    public string Email { get; set; }
    public bool AdditionalPackagesOnly { get; set; }
    public string SearchFields { get; set; }
    public bool InActive { get; set; }

    public string PackageCode0 { get; set; }
    public string PackageCode1 { get; set; }
    public string PackageCode2 { get; set; }
    public string PackageCode3 { get; set; }
    public string PackageCode4 { get; set; }
    public string PackageCode5 { get; set; }
    public string PackageCode6 { get; set; }
    public string PackageCode7 { get; set; }
    public string PackageCode8 { get; set; }
    public string PackageCode9 { get; set; }
    public string PackageCode10 { get; set; }

    public bool IsChecked0 { get; set; }
    public bool IsChecked1 { get; set; }
    public bool IsChecked2 { get; set; }
    public bool IsChecked3 { get; set; }
    public bool IsChecked4 { get; set; }
    public bool IsChecked5 { get; set; }
    public bool IsChecked6 { get; set; }
    public bool IsChecked7 { get; set; }
    public bool IsChecked8 { get; set; }
    public bool IsChecked9 { get; set; }
    public bool IsChecked10 { get; set; }
}

public class UserExtendedListField
{
    public string FieldName { get; set; }
    public string DataType { get; set; }
}