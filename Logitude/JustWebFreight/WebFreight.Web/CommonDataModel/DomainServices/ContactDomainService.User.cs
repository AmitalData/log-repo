using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System.Web;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityPMs;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.Helpers;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class ContactDomainService
    {
        public UserLastLoginPM GetUserLastLogin(string userId, int tenant)
        {
            userLastLoginQuery = new UserLastLoginQuery(tenant);
            return userLastLoginQuery.GetSinglePM(userId, tenant);
        }

        public void UpdateUserLastLogin(UserLastLoginPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

            UserLastLoginRepository repository = new UserLastLoginRepository(objectContext);
            UserLastLogin entity = repository.GetSingleUserLastLogin(currentEntity.Id, currentEntity.Tenant, false);

            entity.ComputerId = currentEntity.ComputerId;
            entity.WorkEnvironment = LogitudeSettingConfigration.GetWorkEnvironment();
            repository.Update(entity);
        }

        public void UpdateUserList(UserList currentEntity)
        {
        }

        public IQueryable<UserPM> GetUsersByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            UserQuery userQuery = new UserQuery(userRepository);
            return userQuery.GetUserPMsByTenant(tenant);
        }

        public UserPM GetSingleUser(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            UserQuery userQuery = new UserQuery(userRepository);
            return userQuery.GetSingleUserPM(id, tenant, false);
        }

        public void MapUserUserPM(UserPM userPm, User user)
        {
            user.BranchId = userPm.BranchId;
            user.DepartmentId = userPm.DepartmentId;
            user.Notes = userPm.Notes;
            user.Tenant = userPm.Tenant;
            user.PersonalId = userPm.PersonalId;
            user.SearchFields = userPm.EnglishName + "," + userPm.LocalName + "," + userPm.Email + "," + userPm.PersonalId;//userPm.Position + "," + userPm.BusinessPhone + "," + userPm.Mobile + "," + userPm.Fax
        }

        public void MapUserToContact(UserPM user, Contact contact)
        {
            contact.Anniversary = user.Anniversary;
            contact.Birthday = user.Birthday;
            contact.BusinessPhone = user.BusinessPhone;
            contact.Email = user.Email;
            contact.EnglishName = user.EnglishName;
            contact.FacebookId = user.FacebookId;
            contact.Fax = user.Fax;
            contact.InActive = user.InActive;
            contact.LocalName = user.LocalName;
            contact.Mobile = user.Mobile;
            //contact.Password = user.Password != null ? user.Password : contact.Password;
            contact.Notes = user.Notes;
            contact.Tenant = user.Tenant;
            contact.DontShowLocalLabels = LogitudeSettings.WorkEnvironment == "customs" ? false : true;
            contact.SearchFields = user.EnglishName + "," + user.LocalName + "," + user.Email + "," + user.BusinessPhone + "," + user.Mobile + "," + user.Fax;
        }

        public void MapUserToContact(UserPM user, ContactPM contact)
        {
            contact.Anniversary = user.Anniversary;
            contact.Birthday = user.Birthday;
            contact.BusinessPhone = user.BusinessPhone;
            contact.Email = user.Email;
            contact.EnglishName = user.EnglishName;
            contact.FacebookId = user.FacebookId;
            contact.Fax = user.Fax;
            contact.InActive = user.InActive;
            contact.LocalName = user.LocalName;
            contact.Mobile = user.Mobile;
            contact.Password = user.Password != null ? user.Password : contact.Password;
            contact.Notes = user.Notes;
            contact.Tenant = user.Tenant;
            contact.SearchFields = user.SearchFields;
            contact.IsUser = true;
            contact.DontShowLocal = true;
        }

        public void InsertUser(UserPM user)
        {
            SecurityUtility.AuthenticationOnTenant(user.Tenant);
            SecurityUtility.CheckContactFeature("User", "NEW", user.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(user.Tenant);
            }        

            userRepository = new UserRepository(objectContext);

            bool exists = userRepository.DoesUserExist(user.Email, user.Tenant);
            if (!exists)
            {
                UserService service = new UserService(objectContext, user.Tenant);
                service.Create(user);
            }

            else
            {
                throw new Exception("Sorry this user is already exists !!");
            }
        }

        public UserList GetSingleUserList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            User user = userRepository.GetSingleUser(id, tenant, false);
            UserList userList = null;
            UserQuery userQuery = new UserQuery(userRepository);
            if (user != null)
            {
                List<User> singleEntityList = new List<User>();
                singleEntityList.Add(user);

                IQueryable<User> iQueryable = singleEntityList.AsQueryable();
                IQueryable<UserList> iQueryableEntityList = userQuery.GetIQueryableEntityList(iQueryable);
                userList = iQueryableEntityList.FirstOrDefault();
            }

            return userList;
        }

        public List<UserList> GetUserLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            IQueryable<User> users = userRepository.GetUsers(tenant);

            UserQuery userQuery = new UserQuery(userRepository);
            List<UserList> query2 = userQuery.GetIQueryableEntityList(users).ToList();

            List<EmployeeGroupLine> employees = new List<EmployeeGroupLine>();
            EmployeeGroupLineRepository employeeRep = new EmployeeGroupLineRepository(tenant);
            employees = employeeRep.GetAll(tenant).ToList();

            foreach (UserList item in query2)
            {
                List<string> temp = employees.Where(a => a.UserId == item.Id && a.Tenant == item.Tenant).Select(a => a.EmployeeGroupId).ToList();
                if (temp.Count > 0)
                {
                    item.GroupId = temp.ToList();
                }
            }

            return query2;
        }



        public List<UserList> GetUserListsByUserIds(List<string>userIds,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            UserQuery userQuery = new UserQuery(tenant);

            return userQuery.GetUserListByUserIds(userIds, tenant);

        }



        [Query(HasSideEffects = true)]
        public List<UserList> GetUserFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<User> users = userRepository.GetUsers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            UserCustomFilter entityCustomFilter = new UserCustomFilter(tenant);
            users = entityCustomFilter.GetFilteredQuery(queryOperations, users);

            users = filter.GetFilteredQuery<User>(nonListQueryOperation, users);

            int skippedPorts = queryOperations.PageIndex;
            UserQuery userQuery = new UserQuery(userRepository);
            IQueryable<UserList> query2 = userQuery.GetIQueryableEntityList(users);
            query2 = filter.GetFilteredQuery<UserList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(UserList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("User", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<UserList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<UserList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<UserList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<UserList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<UserList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<UserList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }
            
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);


            List<UserList> userLists = query2.ToList();

            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                contactRepository = new ContactRepository(tenant);
                Contact contact = (from a in contactRepository.context.Contacts
                                   where a.Email == HttpContext.Current.User.Identity.Name.ToLower() && a.Tenant == tenant
                                   select a).FirstOrDefault();

                if (contact != null)
                {
                    FollowerQueryService followerQueryService = new FollowerQueryService(contact.Tenant);
                    List<FollowerPM> followee = followerQueryService.GetUserFollowee(contact.Id, contact.Tenant);

                    foreach (UserList userlist in userLists)
                    {

                        userlist.IsFollowed = followee.Where(f => f.FolloweeUserId == userlist.Id).Any();

                    }
                }
            }

            return userLists;
        }

        public int GetUserFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<User> users = userRepository.GetUsers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            UserCustomFilter entityCustomFilter = new UserCustomFilter(tenant);
            users = entityCustomFilter.GetFilteredQuery(queryOperations, users);

            users = filter.GetFilteredQuery<User>(nonListQueryOperation, users);
            UserQuery userQuery = new UserQuery(userRepository);
            IQueryable<UserList> query2 = userQuery.GetIQueryableEntityList(users);
            query2 = filter.GetFilteredQuery<UserList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<UserPM> GetUsersSearch(string email, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("User", "READ", tenant);

            userRepository = new UserRepository(tenant);
            UserQuery userQuery = new UserQuery(userRepository);
            IQueryable<UserPM> q = userQuery.GetUsersByEmailOrName(email, name, tenant);
            return q;
        }

        public void UpdateUser(UserPM currentUser)
        {
            SecurityUtility.CheckContactFeature("User", "UPDATE", currentUser.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentUser.Tenant);
            }
            
            List<UserPermittedBranchPM> userPermittedBranchPMChangeSet = ChangeSet.GetAssociatedChanges(currentUser, d => d.UserPermittedBranches).Cast<UserPermittedBranchPM>().ToList();
            foreach (UserPermittedBranchPM itemPM in userPermittedBranchPMChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<UserPermittedProductPM> userPermittedProductPMChangeSet = ChangeSet.GetAssociatedChanges(currentUser, d => d.UserPermittedProducts).Cast<UserPermittedProductPM>().ToList();
            foreach (UserPermittedProductPM itemPM in userPermittedProductPMChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            UserService service = new UserService(objectContext, currentUser.Tenant);
            service.SetChangeSet(userPermittedBranchPMChangeSet);
            service.SetProductChangeSet(userPermittedProductPMChangeSet);
            service.Update(currentUser);

            TableLastUpdateClass.UpdateTableHistory(currentUser.Tenant, "User");
        }

        public void DeleteUser(UserPM user)
        {

        }

        public UsersWorkspaceSummary GetUsersWorkspaceSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            UsersWorkspaceSummary myResult = new UsersWorkspaceSummary() { Id = tenant };

            if (SecurityUtility.CheckTableContactFeature("User", "READ", tenant))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);

                    if (tenantManagement != null)
                    {
                        myResult.LicensesCount = tenantManagement.NumberOfUsers == null ? 0 : tenantManagement.NumberOfUsers.Value;                        
                    }
                    scope.Complete();
                }

                userRepository = new UserRepository(tenant);

                IQueryable<User> iQueryableData = userRepository.GetUsers(tenant);

                myResult.AllUsersCount = iQueryableData.Count();
                myResult.ActiveUsersCount = iQueryableData.Where(d => d.Contact.InActive == false).Count();
                myResult.InactiveUsersCount = iQueryableData.Where(d => d.Contact.InActive == true).Count();
                myResult.ActiveLicensedCount = iQueryableData.Where(d => d.Contact.InActive == false && d.LicencedUser == true).Count();
                myResult.ActiveNotLicensedCount = iQueryableData.Where(d => d.Contact.InActive == false && d.LicencedUser == false).Count();                
            }

            return myResult;
        }

        public List<UsersWorkspaceRecentItem> GetUsersWorkspaceRecentLogins(int tenant)
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

                List<UserLoginLog> allLoginLogs = (from d in userLoginLogRepository.context.UserLoginLogs
                                                   where d.Tenant == tenant && lastLoginsUserIdsList.Contains(d.UserId)
                                                   select d).ToList();

                foreach (UserLastLogin item in lastLoginsList)
                {
                    UsersWorkspaceRecentItem myRecord = new UsersWorkspaceRecentItem()
                    {
                        Id = item.Id,
                        Username = item.User == null ? "" : (item.User.Contact == null ? "" : item.User.Contact.EnglishName),
                        BusinessUnit = item.User == null ? "" : (item.User.BusinessUnit == null ? "" : item.User.BusinessUnit.Name),
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

            return myResult;
        }

        public IQueryable<UserLoginLog> GetUserLoginHistory(string userId, int tenant)
        {
            UserLoginLogRepository userLoginLogRepository = new UserLoginLogRepository(tenant);
            IQueryable<UserLoginLog> iQueryable = userLoginLogRepository.GetUserLoginHistory(userId, tenant);
            return iQueryable.OrderByDescending(o => o.LocalDateTime);
        }

        public IQueryable<UserLicensePM> GetUserLicenses(int tenant)
        {
            UserLicenseRepository myRepository = new UserLicenseRepository(tenant);
            UserLicenseQuery myQuery = new UserLicenseQuery(myRepository);
            IQueryable<UserLicensePM> myResult = myQuery.GetUserLicensesByTenant(tenant);
            return myResult;
        }

        public void InsertUserLicense(UserLicensePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.UserId != null && entityPM.PackageCode != null)
            {
                UserLicenseRepository licenseRepository = new UserLicenseRepository(objectContext);
                UserLicense entityPOCO = licenseRepository.GetSingleUserLicenseByUserAndPackage(entityPM.UserId, entityPM.PackageCode, entityPM.Tenant);
                if (entityPOCO == null)
                {
                    entityPM.Id = IdCounter.GetNumber("UserLicense", entityPM.Tenant).ToString();

                    entityPOCO = new UserLicense()
                    {
                        Id = entityPM.Id,
                        UserId = entityPM.UserId,
                        PackageCode = entityPM.PackageCode,
                        Tenant = entityPM.Tenant,
                    };

                    licenseRepository.Add(entityPOCO);
                    licenseRepository.SubmitChanges();

                    string key = entityPM.Email + "_" + entityPM.Tenant + "_info";
                    CacheManager.CacheWrapper.Invalidate(key);
                }
            }
        }

        public void UpdateUserLicense(UserLicensePM entityPM)
        {
            // Dont remove this empty Method

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }
        }

        public void DeleteUserLicense(UserLicensePM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            if (entityPM.UserId != null && entityPM.PackageCode != null)
            {
                UserLicenseRepository licenseRepository = new UserLicenseRepository(objectContext);
                UserLicense entityPOCO = licenseRepository.GetSingleUserLicenseByUserAndPackage(entityPM.UserId, entityPM.PackageCode, entityPM.Tenant);
                if (entityPOCO != null)
                {
                    licenseRepository.Remove(entityPOCO);
                    licenseRepository.SubmitChanges();
                }

                ContactRepository myContactRepository = new ContactRepository(entityPM.Tenant);
                string myEmail = myContactRepository.GetConactEmail(entityPM.UserId);
                if (myEmail != null)
                {
                    string key = myEmail + "_" + entityPM.Tenant + "_info";
                    CacheManager.CacheWrapper.Invalidate(key);
                }
            }
        }
    }

    public class UsersWorkspaceSummary
    {
        [Key]
        public int Id { get; set; }
        public int LicensesCount { get; set; }
        public int AllUsersCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public int InactiveUsersCount { get; set; }
        public int ActiveLicensedCount { get; set; }
        public int ActiveNotLicensedCount { get; set; }
        public int ActiveNotAdditionalUsersCount { get; set; }
    }

    public class UsersWorkspaceRecentItem
    {
        [Key]
        public string Id { get; set; }
        public string Username { get; set; }
        public string BusinessUnit { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string IP { get; set; }
        public string Countryname { get; set; }
        public bool BlockEditUser { get; set; }
        public bool IsCustomerCareUser { get; set; }
    }
}
