using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Transactions;
using Logitude.BL.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class UserService
    {
        bool isNewEntity;
        private int tenant;
        public User Poco { get; set; }
        private UserPM entityPm;
        private ICommonDataContext objectContext;
        private UserRepository entityRepository;
        private ContactRepository contactRepository;
        private RoleRepository roleRepository;
        private ContactTenantRoleRepository contactTenantRoleRepository;
        private ContactTenantRepository contactTenantRepository;
        private UserPermittedBranchRepository userPermittedBranchRepository;
        private UserPermittedProductRepository userPermittedProductRepository;
        public UserService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new UserRepository(objectContext);
            this.userPermittedBranchRepository = new UserPermittedBranchRepository(objectContext);
            this.userPermittedProductRepository = new UserPermittedProductRepository(objectContext);
        }

        private List<UserPermittedBranchPM> userPermittedBranchPMChangeSet;
        private List<UserPermittedProductPM> userPermittedProductPMChangeSet;
        public void SetChangeSet(List<UserPermittedBranchPM> userPermittedBranchPMChangeSet)
        {
            this.userPermittedBranchPMChangeSet = userPermittedBranchPMChangeSet;
        }
        public void SetProductChangeSet(List<UserPermittedProductPM> userPermittedProductPMChangeSet)
        {
            this.userPermittedProductPMChangeSet = userPermittedProductPMChangeSet;
        }
        public bool SuppressMustChangePasswordDueSSO { get; set; }

        public void Create(UserPM entityPM)
        {
            entityPM.Email = entityPM.Email.ToLower();

            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("User", tenant).ToString();
            this.entityPm.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            this.Poco = new User();
            this.Poco.Id = this.entityPm.Id;

            UserValidating.Validate(entityPM);

            bool canAddUser = this.CheckTenantManagementNumberOfUsers();
            if (!canAddUser)
            {
                throw new Exception("Sorry You reached the maximum number of users !");
            }

            if (CheckUserId(entityPM, this.Poco))
            {
                throw new Exception("This user personal Id already exists for another user!");
            }

            contactRepository = new ContactRepository(objectContext);
            roleRepository = new RoleRepository(objectContext);
            contactTenantRoleRepository = new ContactTenantRoleRepository(objectContext);
            contactTenantRepository = new ContactTenantRepository(objectContext);

            ContactTenant newContactTenant;
            Contact existedContact = contactRepository.GetSingleContactByEmailSpecificTenant(entityPM.Email, entityPM.Tenant);
            if (existedContact != null)
            {
                newContactTenant = ConnectToExistedContact(entityPm, existedContact);
            }

            else
            {
                newContactTenant = InseartNewContact(entityPm);
            }


			#region newUser roles

			UpdateUserRolesForHybrid(entityPM, newContactTenant);

            #endregion

            foreach (UserPermittedBranchPM itemPM in entityPM.UserPermittedBranches)
            {
                this.CreateUserPermittedBranch(itemPM);
            }

            foreach (UserPermittedProductPM itemPM in entityPM.UserPermittedProducts)
            {
                this.CreateUserPermittedProduct(itemPM);
            }

            ContactQuery contactQuery = new ContactQuery(contactRepository);
            if (!entityPM.IsHybrid)
            {
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant); //itzik
                ContactPM contact = contactQuery.GetContactByNameAndTenant(resolveLoggingUserId, entityPM.Tenant, true);
            }

            if (!entityPM.IsHybrid && !entityPM.SignupRole)
            {
                UserTracing.Trace(entityPM, Poco, isNewEntity);
            }

            entityPm.UserRoles = this.ComputeUserRoles();
            CheckDocumentFilingInbox(entityPm, Poco);
            UserMapping.MapEntity(entityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            if (!entityPM.IsHybrid)
            {
                UpdateRolePM(entityPM);
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "User");
        }

        private void UpdateUserRolesForHybrid(UserPM entityPM, ContactTenant contactTenant)
        {
            if (entityPM.IsHybrid || isNewEntity)
            {
                if (entityPM.Roles != null)
                {
                    List<Role> allRoles = roleRepository.GetRoles(tenant).ToList();
                    Role freelancerRole = allRoles.FirstOrDefault(r => r.Code.StartsWith("FRL"));
                    if (freelancerRole != null && entityPM.IsFreelancer && !entityPM.Roles.Any(r => r.Id == freelancerRole.Id))
                    {
                        throw new Exception("המשתמש הינו פרילנסר, יש לבחור רק תקפיד המוגדר כפרילנסר"); // ("Must choose a freelancer role!");
                    }

                    foreach (UserRolesPM role in entityPM.Roles)
                    {
                        Role currentRole = allRoles.FirstOrDefault(r => r.Id == role.Id);
                        #region update role
                        if (role.Added)
                        {
                            ContactTenantRole contactTenantRole = new ContactTenantRole() { ContactTenantId = contactTenant.Id, RoleId = currentRole.Id, Id = IdCounter.GetNumber("ContactTenantRole", entityPM.Tenant).ToString(), Tenant = entityPM.Tenant };
                            contactTenantRoleRepository.Add(contactTenantRole);
                        }
                        if (role.Removed)
                        {
                            ContactTenantRole contactTenantRole = contactTenantRoleRepository.GetContactTenantRoleByRoleIdAndContactTenant(currentRole.Id, contactTenant.Id, entityPM.Tenant);
                            if (contactTenantRole != null)
                            {
                                contactTenantRoleRepository.Remove(contactTenantRole);
                            }
                        }

                        roleRepository.Update(currentRole);
                        if (!entityPM.IsHybrid)
                        {
                            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Role");
                        }
                        #endregion
                    }
                }
            }
        }
        
        public void Update(UserPM entityPM, bool mapComposition = false)
        {
            if (mapComposition)
            {
                this.userPermittedBranchPMChangeSet = entityPM.UserPermittedBranches;
                this.userPermittedProductPMChangeSet = entityPM.UserPermittedProducts;
            }

            contactRepository = new ContactRepository(objectContext);
            roleRepository = new RoleRepository(objectContext);
            contactTenantRoleRepository = new ContactTenantRoleRepository(objectContext);
            contactTenantRepository = new ContactTenantRepository(objectContext);

            var contactTenant = contactTenantRepository.GetContactTenantForContactId(entityPM.Id, entityPM.Tenant);

            if (entityPM.IsHybrid)
            {
                this.UpdateUserRolesForHybrid(entityPM, contactTenant);
            }
            else
            {
                UpdateRolePM(entityPM);
                if (entityPM.IsFreelancer)
                {
                    ValidateFreelancerRoles(entityPM); // validate roles for freelancer / customs
                }
            }

            this.isNewEntity = false;
            entityPM.Email = entityPM.Email.ToLower();
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleUser(entityPM.Id, entityPm.Tenant, false);

            contactRepository = new ContactRepository(objectContext);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            ContactPM currentContact = null;

            if (this.CheckUserId(entityPM, this.Poco))
            {
                throw new Exception("This user ID already exists !");
            }

            if (!entityPM.IsHybrid)
            {
                currentContact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant, true);
                if (currentContact == null)
                {
                    currentContact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), 0, false);
                }
            }

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

            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                entityName = "User" + entityPM.Code + entityPM.Tenant;
                entityPmName = "UserPM" + entityPM.Code + entityPM.Tenant;
                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }
                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Email))
            {
                entityName = "User" + entityPM.Email + entityPM.Tenant;
                entityPmName = "UserPM" + entityPM.Email + entityPM.Tenant;
                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }
                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }

            ContactPM contact = contactQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);

            this.UpdateUserPermittedPermitions();

            if (entityPM.IsBranchRestricted)
            {
                if (entityPM.UserPermittedBranches.Count > 0)
                {
                    if (!entityPM.UserPermittedBranches.Where(c => c.BranchId == entityPM.BranchId).Any())
                    {
                        throw new Exception("user default branch must be permitted!!");
                    }
                }
                else
                {
                    throw new Exception("user must have at least one permitted branch!!");
                }
            }

            if (entityPM.IsProductRestricted)
            {
                if (entityPM.UserPermittedProducts.Count == 0)
                {
                    throw new Exception("user must have at least one permitted product!!");
                }
            }

            bool isMultiPackages = false;
            TenantManagement tenantMngmnt = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalContactRepository globalContactRepository = new GlobalContactRepository();
                GlobalContact globalContact = globalContactRepository.GetSingleGlobalContact(entityPM.Id);
                if (globalContact != null)
                {
                    globalContact.Email = entityPM.Email;
                    globalContact.InActive = entityPM.InActive;
                    globalContactRepository.Update(globalContact);
                }

                else
                {
                    GlobalContact gcontact = new GlobalContact() { Email = entityPM.Email, Id = entityPM.Id, GlobalTenantId = entityPM.Tenant, IsUser = true, };
                    globalContactRepository.Add(gcontact);
                }

                globalContactRepository.SubmitChanges();

                TenantManagementRepository tenantMngmntRep = new TenantManagementRepository();
                tenantMngmnt = tenantMngmntRep.GetSingleTenantManagement(entityPM.Tenant);

                isMultiPackages = tenantMngmnt.IsMultiPackage;

                if (contact.Email.ToLower() != entityPm.Email.ToLower())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();

                    if (!globalContext.ContactPasswords.Where(c => c.Email == entityPm.Email).Any())
                    {
                        ContactPassword contactPassword = new ContactPassword()
                        {
                            Email = entityPM.Email,
                            Password = "123",
                            IsLocked = false,
                            NumberOfRetries = 0,
                            MustChangePassword = false,
                        };

                        if (contactPassword.MustChangePassword && SuppressMustChangePasswordDueSSO)
                        {
                            contactPassword.MustChangePassword = false;
                        }

                        globalContext.ContactPasswords.Add(contactPassword);
                        globalContext.SaveChanges();
                    }
                }

                scope.Complete();
            }

            if (!entityPM.IsHybrid)
            {
                int? totalUsers = 0;
                if (string.IsNullOrEmpty(tenantMngmnt.FreeUsers.ToString()))
                {
                    totalUsers = tenantMngmnt.NumberOfUsers;
                }
                else
                {
                    totalUsers = tenantMngmnt.NumberOfUsers + tenantMngmnt.FreeUsers;
                }

                int tenantUsersCount = 0;
                if (tenantMngmnt.ManageLicencesPerUser)
                {
                    tenantUsersCount = entityRepository.GetUsers(entityPM.Tenant).Where(d => d.Contact.InActive == false && d.Contact.Email != "customercare@logitudeworld.com" && d.LicencedUser == true).Count();

                    if (!Poco.LicencedUser && entityPM.LicencedUser)
                    {
                        bool isUsersCountAllowed = false;

                        if (isMultiPackages)
                        {
                            isUsersCountAllowed = true;
                        }

                        else if (tenantUsersCount < totalUsers)
                        {
                            isUsersCountAllowed = true;
                        }

                        else if (entityPM.Tenant == 0 && !entityPm.IsDistributor)
                        {
                            isUsersCountAllowed = true;
                        }

                        if (isUsersCountAllowed)
                        {
                            UserTracing.Trace(entityPM, Poco, isNewEntity);

                            if (currentContact != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = 0,
                                    EventTypeCode = "UPMG",
                                    UserId = currentContact.Id,
                                    EntityId = tenantMngmnt.Id.ToString(),
                                    ObjectTableName = "TenantManagement",
                                    Notes = "The user " + entityPM.EnglishName + " is Licenced !!",
                                });
                            }
                        }

                        else
                        {
                            throw new ApplicationException("Sorry You can't Licence this user since you reached the maximum number of users !!");
                        }
                    }
                }

                else
                {
                    tenantUsersCount = entityRepository.GetUsers(entityPM.Tenant).Where(d => d.Contact.InActive == false && d.Contact.Email != "customercare@logitudeworld.com").Count();

                    if (this.Poco.Contact.InActive && !entityPM.InActive)
                    {
                        bool isUsersCountAllowed = false;

                        if (isMultiPackages)
                        {
                            isUsersCountAllowed = true;
                        }

                        else if (tenantUsersCount < totalUsers)
                        {
                            isUsersCountAllowed = true;
                        }

                        else if (entityPM.Tenant == 0 && !entityPm.IsDistributor)
                        {
                            isUsersCountAllowed = true;
                        }

                        if (isUsersCountAllowed)
                        {
                            UserTracing.Trace(entityPM, Poco, isNewEntity);

                            if (contact != null)
                            {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = 0,
                                    EventTypeCode = "UPMG",
                                    UserId = currentContact.Id,
                                    EntityId = tenantMngmnt.Id.ToString(),
                                    ObjectTableName = "TenantManagement",
                                    Notes = "The user " + entityPM.EnglishName + " has been activated !!",
                                });
                            }
                        }

                        else
                        {
                            throw new Exception("Sorry You can't activate this user since you reached the maximum number of users !!");
                        }
                    }
                }

                //inactive + licenses
                if (tenantMngmnt.IsMultiPackage)
                {
                    if (entityPM.InActive && !Poco.Contact.InActive)
                    {
                        UserLicenseRepository repository = new UserLicenseRepository(objectContext);
                        List<UserLicense> licenses = repository.GetUserLicensesByUserId(entityPM.Id, tenant);

                        if (licenses.Count > 0)
                        {
                            foreach (UserLicense item in licenses)
                            {
                                repository.Remove(item);
                            }

                            repository.SubmitChanges();
                        }
                    }
                }
            }

            if (!entityPM.IsHybrid)
            {
                UserTracing.Trace(entityPM, Poco, isNewEntity);
            }

            entityPm.UserRoles = this.ComputeUserRoles();
            CheckDocumentFilingInbox(entityPM, Poco);
            ContactService service = new ContactService(objectContext, entityPM.Tenant);
            MapUserToContact(entityPM, contact);
            service.Update(contact);

            UserValidating.Validate(entityPM);
            UserMapping.MapEntity(entityPm, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "User");
        }

        private void MapUserToContact(UserPM user, Contact contact)
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
            contact.Notes = user.Notes;
            contact.Tenant = user.Tenant;

            //contact.DontShowLocalLabels = LogitudeSettings.WorkEnvironment == "customs" ? false : true; // bug 44449

            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, user.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, user.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, user.Email);
            MethodHelper.AddToSearchFields(ref mySearchFields, user.BusinessPhone);
            MethodHelper.AddToSearchFields(ref mySearchFields, user.Mobile);
            MethodHelper.AddToSearchFields(ref mySearchFields, user.Fax);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            contact.SearchFields = mySearchFields;
        }

        private void MapUserToContact(UserPM user, ContactPM contact)
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

        private ContactTenant InseartNewContact(UserPM entityPM)
        {

            entityPM.Email = entityPM.Email.ToLower();

            Contact newContact = new Contact();
			newContact.DontShowLocalLabels = LogitudeSettings.WorkEnvironment == "customs" ? false : true; // Mohammad & Islam: related to bug 44449

			MapUserToContact(entityPM, newContact);

            Contact adminContact = contactRepository.GetSingleContactByEmail("admin@fnarsoft.com", 0);
            if (adminContact != null)
            {
                newContact.Signature = adminContact.Signature;
                newContact.SignatureHtml = adminContact.SignatureHtml;
            }

            #region insertContact

            Random rnd = new Random();

            newContact.Id = IdCounter.GetNumber("Contact", entityPM.Tenant).ToString();
            newContact.ComputedKey = (!string.IsNullOrEmpty(newContact.Email) ? newContact.Email : newContact.Id);
            newContact.UserType = "R";
            newContact.IndexColor = rnd.Next(1, 20);
            RoleQuery roleQuery = new RoleQuery(roleRepository);
            ContactTenant newContactTenant = new ContactTenant()
            {
                Id = IdCounter.GetNumber("ContactTenant", entityPM.Tenant).ToString(),
                TenantId = newContact.Tenant,
                ContactId = newContact.Id,
            };
            contactRepository.Add(newContact);
            contactTenantRepository.Add(newContactTenant);

            #region admin role for signup
            if (entityPM.SignupRole)
            {
                RolePM adimnrole = roleQuery.GetSinglePMByName("Administrator", newContact.Tenant);

                ContactTenantRole admincontactTenantRole = new ContactTenantRole()
                {
                    ContactTenantId = newContactTenant.Id,
                    Id = IdCounter.GetNumber("ContactTenantRole", entityPM.Tenant).ToString(),
                    RoleId = adimnrole.Id,
                    Tenant = newContact.Tenant

                };
                contactTenantRoleRepository.Add(admincontactTenantRole);
            }
            #endregion

            if (!string.IsNullOrEmpty(newContact.Email))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    if (!string.IsNullOrEmpty(entityPM.Password))
                    {
                        string newPassword = PasswordGenerator.GetBCryptHashedPassword(entityPM.Email, entityPM.Password);
                        if (!globalContext.ContactPasswords.Where(c => c.Email == entityPm.Email).Any())
                        {
                            ContactPassword contactPassword = new ContactPassword()
                            {
                                Email = entityPM.Email,
                                Password = newPassword,
                                IsLocked = false,
                                NumberOfRetries = 0,
                                MustChangePassword = (newContact.Email != "customercare@logitudeworld.com"),
                                IsBCrypt = true,
                            };

                            if (contactPassword.MustChangePassword && SuppressMustChangePasswordDueSSO)
                            {
                                contactPassword.MustChangePassword = false;
                            }
                            globalContext.ContactPasswords.Add(contactPassword);
                        }
                    }

                    GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);

                    bool globalContactExists = (from a in globalContactRep.GetGlobalContactByTenant(newContact.Tenant)
                                                where a.Email == newContact.Email
                                                select a).Any();
                    if (!globalContactExists)
                    {
                        GlobalContact conflictcontact = globalContactRep.GetSingleGlobalContact(newContact.Id);
                        if (conflictcontact != null)
                        {
                            globalContactRep.Remove(conflictcontact);
                        }
                        GlobalContact gcontact = new GlobalContact() { Email = newContact.Email, Id = newContact.Id, GlobalTenantId = newContact.Tenant, IsUser = true, };

                        globalContactRep.Add(gcontact);
                        globalContext.SaveChanges();
                    }

                    scope.Complete();
                }
            }
            #endregion

            this.Poco.Id = newContact.Id;
            entityPM.Id = this.Poco.Id;
            return newContactTenant;
        }
        private ContactTenant ConnectToExistedContact(UserPM entityPM, Contact contact)
        {

            entityPM.Email = entityPM.Email.ToLower();

            Contact adminContact = contactRepository.GetSingleContactByEmail("admin@fnarsoft.com", 0);
            if (adminContact != null)
            {
                contact.Signature = adminContact.Signature;
            }

            contact.UserType = "R";
            if (!string.IsNullOrEmpty(contact.Email))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == contact.Email).FirstOrDefault();
                    if (contactPassword == null)
                    {
                        if (!string.IsNullOrEmpty(entityPM.Password))
                        {
                            string newPassword = PasswordGenerator.GetBCryptHashedPassword(entityPM.Email, entityPM.Password);

                            contactPassword = new ContactPassword()
                            {
                                Email = contact.Email,
                                Password = newPassword,
                                IsLocked = false,
                                NumberOfRetries = 0,
                                MustChangePassword = (contact.Email != "customercare@logitudeworld.com"),
                                IsBCrypt = true,
                            };
                            if (contactPassword.MustChangePassword && SuppressMustChangePasswordDueSSO)
                            {
                                contactPassword.MustChangePassword = false;
                            }
                            globalContext.ContactPasswords.Add(contactPassword);
                        }
                    }

                    GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);
                    GlobalContact globalContact = globalContactRep.GetSingleGlobalContact(contact.Id);

                    if (globalContact != null)
                    {
                        globalContact.IsUser = true;
                        globalContactRep.Update(globalContact);
                    }
                    else
                    {
                        GlobalContact conflictcontact = globalContactRep.GetSingleGlobalContact(contact.Id);

                        if (conflictcontact != null)
                        {
                            globalContactRep.Remove(conflictcontact);
                        }

                        GlobalContact gcontact = new GlobalContact() { Email = contact.Email, Id = contact.Id, GlobalTenantId = contact.Tenant, IsUser = true, };
                        globalContactRep.Add(gcontact);
                    }

                    globalContext.SaveChanges();
                    scope.Complete();
                }
            }

            this.Poco.Id = contact.Id;
            entityPM.Id = this.Poco.Id;
            contactRepository.Update(contact);
            contactRepository.SubmitChanges();

            ContactTenant contactTenant = contactTenantRepository.GetContactTenantForContactId(contact.Id, entityPm.Tenant);
            if (contactTenant == null)
            {
                contactTenant = new ContactTenant()
                {
                    Id = IdCounter.GetNumber("ContactTenant", entityPM.Tenant).ToString(),
                    TenantId = contact.Tenant,
                    ContactId = contact.Id,
                };

                contactTenantRepository.Add(contactTenant);
                contactTenantRepository.SubmitChanges();
            }
            return contactTenant;
        }

        private void UpdateUserPermittedPermitions()
        {
            if (userPermittedBranchPMChangeSet != null)
            {
                foreach (UserPermittedBranchPM itemPM in userPermittedBranchPMChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateUserPermittedBranch(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateUserPermittedBranch(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteUserPermittedBranch(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }

            if (userPermittedProductPMChangeSet != null)
            {
                foreach (UserPermittedProductPM itemPM in userPermittedProductPMChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateUserPermittedProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateUserPermittedProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteUserPermittedProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private bool CheckTenantManagementNumberOfUsers()
        {
            bool canAddUser = false;

            if (entityPm.Tenant == 0 && !entityPm.IsDistributor)
            {
                canAddUser = true;
            }

            else
            {
                int? totalUsers = 0;
                bool isManageLicencesPerUser = false;
                bool isMultiPackages = false;

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantMngmntRep = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantMngmntRep.GetSingleTenantManagement(tenant);

                    isMultiPackages = tenantManagement.IsMultiPackage;

                    totalUsers = tenantManagement.NumberOfUsers;
                    if (tenantManagement.FreeUsers != null)
                    {
                        totalUsers += tenantManagement.FreeUsers;
                    }

                    isManageLicencesPerUser = tenantManagement.ManageLicencesPerUser;
                }

                if (isMultiPackages)
                {
                    canAddUser = true;
                }

                else
                {
                    IQueryable<User> allTenantUsers = entityRepository.GetUsers(tenant);
                    int tenantUsers = 0;

                    if (isManageLicencesPerUser)
                    {
                        allTenantUsers = allTenantUsers.Where(d => d.Contact.Email.ToLower() != "customercare@logitudeworld.com" && d.Contact.InActive == false && d.LicencedUser == true);
                        tenantUsers = allTenantUsers.Count();

                        if (entityPm.LicencedUser)
                        {
                            if (tenantUsers < totalUsers)
                            {
                                canAddUser = true;
                            }
                        }

                        else
                        {
                            canAddUser = true;
                        }
                    }

                    else
                    {
                        allTenantUsers = allTenantUsers.Where(d => d.Contact.Email.ToLower() != "customercare@logitudeworld.com" && d.Contact.InActive == false);
                        tenantUsers = allTenantUsers.Count();

                        if (tenantUsers < totalUsers)
                        {
                            canAddUser = true;
                        }
                    }
                }
            }

            return canAddUser;
        }

        private void CreateUserPermittedBranch(UserPermittedBranchPM entityPM)
        {
            entityPM.Id = IdCounter.GetNumber("UserPermittedBranch", tenant).ToString();
            UserPermittedBranch Poco = new UserPermittedBranch();
            Poco.Id = entityPM.Id;

            UserPermittedBranchValidating.Validate(entityPM);

            UserPermittedBranchMapping.MapEntity(entityPM, Poco, true);
            userPermittedBranchRepository.Add(Poco);

        }
        private void UpdateUserPermittedBranch(UserPermittedBranchPM entityPM)
        {

            UserPermittedBranch Poco = userPermittedBranchRepository.GetSingleUserPermittedBranch(entityPM.Id, entityPM.Tenant);

            UserPermittedBranchValidating.Validate(entityPM);

            UserPermittedBranchMapping.MapEntity(entityPM, Poco, false);
            userPermittedBranchRepository.Update(Poco);


        }
        private void DeleteUserPermittedBranch(UserPermittedBranchPM entityPM)
        {

            UserPermittedBranch Poco = userPermittedBranchRepository.GetSingleUserPermittedBranch(entityPM.Id, entityPM.Tenant);

            UserPermittedBranchValidating.Validate(entityPM);


            userPermittedBranchRepository.Remove(Poco);


        }

        private void CreateUserPermittedProduct(UserPermittedProductPM entityPM)
        {
            entityPM.Id = IdCounter.GetNumber("UserPermittedProduct", tenant).ToString();
            UserPermittedProduct Poco = new UserPermittedProduct();
            Poco.Id = entityPM.Id;

            UserPermittedProductValidating.Validate(entityPM);
            UserPermittedProductMapping.MapEntity(entityPM, Poco, true);
            userPermittedProductRepository.Add(Poco);

        }
        private void UpdateUserPermittedProduct(UserPermittedProductPM entityPM)
        {
            UserPermittedProduct Poco = userPermittedProductRepository.GetSingleUserPermittedProduct(entityPM.Id, entityPM.Tenant);
            UserPermittedProductValidating.Validate(entityPM);
            UserPermittedProductMapping.MapEntity(entityPM, Poco, false);
            userPermittedProductRepository.Update(Poco);
        }
        private void DeleteUserPermittedProduct(UserPermittedProductPM entityPM)
        {
            UserPermittedProduct Poco = userPermittedProductRepository.GetSingleUserPermittedProduct(entityPM.Id, entityPM.Tenant);
            UserPermittedProductValidating.Validate(entityPM);
            userPermittedProductRepository.Remove(Poco);
        }

        private void UpdateRolePM(UserPM entityPM)
        {
            if (entityPM.RolePMLists != null && entityPM.RolePMLists.Count > 0)
            {
                foreach (RolePM rolePM in entityPM.RolePMLists)
                {
                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    RoleService roleService = new RoleService(MyContext, entityPM.Tenant);
                    roleService.Update(rolePM);
                }
                entityPM.RolePMLists = new List<RolePM>();
            }
        }

        private void ValidateFreelancerRoles(UserPM entityPM)
        {
            RoleQuery roleQuery = new RoleQuery(entityPM.Tenant);
            List<RolePM> rolePmLists = roleQuery.GetRolesByUser(entityPM.Id, entityPM.Tenant).Where(d => d.Exists).ToList();
            foreach (RolePM rolePM in rolePmLists)
            {
                if (!rolePM.Code.StartsWith("FRL"))
                {
                    throw new Exception("המשתמש הינו פרילנסר, יש לבחור רק תקפיד המוגדר כפרילנסר"); // ("Must choose a freelancer role!");
                }
            }
        }
        
        private bool CheckUserId(UserPM entityPM, User entityPoco)
        {
            bool exists = false;
            if (entityPm.PersonalId != entityPoco.PersonalId)
            {
                if (!string.IsNullOrEmpty(entityPm.PersonalId) && !string.IsNullOrWhiteSpace(entityPm.PersonalId))
                {
                    exists = entityRepository.IsUserIdExist(entityPM.PersonalId, entityPM.Tenant);
                }
            }
            return exists;
        }

        private void CheckDocumentFilingInbox(UserPM entityPM, User entityPOCO)
        {
            if (this.isNewEntity)
            {
                var filingInboxName = entityPM.Email.Split('@')[0] + '.' + entityPM.Email.Split('@')[1].Split('.')[0];
                var emailIndex = 0;
                var counter = 0;
                var check = true;
                var documentFilingInbox = filingInboxName;
                var usersList = this.objectContext.Users.Where(a => a.Tenant == tenant);

                while (counter <= usersList.Count() && check)
                {
                    entityPM.DocumentFilingInbox = documentFilingInbox;
                    emailIndex = emailIndex + 1;
                    if (!(entityRepository.IsDocumentFilingInboxExist(entityPM.DocumentFilingInbox)))
                    {
                        check = false;
                        counter = counter + 1;
                        break;
                    }
                    else
                    {
                        documentFilingInbox = filingInboxName + "" + emailIndex;
                    }
                }
            }
            else
            {
                var isExists = entityRepository.IsDocumentFilingInboxExist(entityPM.DocumentFilingInbox);
                if (isExists && (entityPM.DocumentFilingInbox != entityPOCO.DocumentFilingInbox))
                {
                    throw new Exception("Inbox field already exists");
                }
            }
        }

        private string ComputeUserRoles()
        {
            string myResult = "";

            ContactTenant contacttenant = (from a in this.objectContext.ContactTenants
                                           where a.ContactId == entityPm.Id && a.TenantId == tenant
                                           select a).FirstOrDefault();

            if (contacttenant == null)
            {
                contacttenant = (from a in this.objectContext.ContactTenants
                                 where a.ContactId == entityPm.Id && a.TenantId == 0
                                 select a).FirstOrDefault();
            }

            if (contacttenant != null)
            {
                List<ContactTenantRole> contactTenantRoles = (from a in this.objectContext.ContactTenantRoles
                                                              where a.ContactTenantId == contacttenant.Id && a.Tenant == tenant
                                                              select a).ToList();
                
                foreach (ContactTenantRole contacttenantrole in contactTenantRoles)
                {
                    Role role = this.objectContext.Roles.Where(a => a.Id == contacttenantrole.RoleId && (a.Tenant == tenant || a.Tenant == 0)).FirstOrDefault();
                    
                    if(role != null)
                    {
                        if (string.IsNullOrEmpty(myResult))
                        {
                            myResult = role.Name;
                        }

                        else
                        {
                            myResult = myResult + ", " + role.Name;
                        }
                    }
                }
            }
            
            return myResult;
        }
    }
}