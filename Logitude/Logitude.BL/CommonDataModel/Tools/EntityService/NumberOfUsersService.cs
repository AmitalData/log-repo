using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class NumberOfUsersService
    {
        private int tenant;
        private string packageCode;
        private int? totalTenantManagementUsers = 0;
        private bool isManageLicencesPerUser = false;
        private bool isMultiPackage = false;
        private bool mainAdditionalPackageApplied = false;
        private int tenantUsersCount = 0;
        private string loggedContactId;

        private string userId;
        private string englishName;
        private bool inactive_DB = false;
        private bool licencedUser_DB = false;
        private bool additionalPackagesOnly_DB = false;
        private bool inactive = false;
        private bool licencedUser = false;
        private bool additionalPackagesOnly = false;
        private bool isDistributor = false;
        private bool isNewUser = false;

        private bool isValid = false;
        private string eventNotes;
        private string exceptionMessage;

        private ICommonDataContext commonDataContext;
        private UserRepository userRepository;
        public NumberOfUsersService(UserPM entityPM, User entityPOCO, bool isNewEntity)
        {
            this.tenant = entityPM.Tenant;
            this.isNewUser = isNewEntity;
            this.commonDataContext = CommonDataContext.GetContext(this.tenant);
            this.userRepository = new UserRepository(this.commonDataContext);

            this.userId = entityPM.Id;
            this.englishName = entityPM.EnglishName;
            this.isDistributor = entityPM.IsDistributor;
            this.licencedUser = entityPM.LicencedUser;
            this.additionalPackagesOnly = entityPM.AdditionalPackagesOnly;
            this.licencedUser_DB = entityPOCO.LicencedUser;
            this.inactive_DB = this.isNewUser ? false : entityPOCO.Contact.InActive;
            this.inactive = entityPM.InActive;
            this.additionalPackagesOnly_DB = entityPOCO.AdditionalPackagesOnly;

            this.InitializeService();
        }
        public NumberOfUsersService(ContactPM entityPM, Contact entityPOCO)
        {
            this.tenant = entityPM.Tenant;
            this.userId = entityPM.Id;
            this.additionalPackagesOnly = entityPM.IsUserAdditionalPackagesOnly;
            this.licencedUser = entityPM.IsLicencedUser;
            this.commonDataContext = CommonDataContext.GetContext(this.tenant);
            this.userRepository = new UserRepository(this.commonDataContext);

            User user = this.GetUser_DB();
            if (user != null)
            {
                this.englishName = entityPM.EnglishName;
                this.isDistributor = user.IsDistributor;
                this.inactive_DB = entityPOCO.InActive;
                this.inactive = entityPM.InActive;
                this.additionalPackagesOnly_DB = user.AdditionalPackagesOnly;

                this.InitializeService();
            }
        }

        private void InitializeService()
        {
            this.GetTenantManagementProperties();
            this.GetTenantUsersCount();
            this.GetLoggedContactId();
        }

        private void GetTenantManagementProperties()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantMngmntRep = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantMngmntRep.GetSingleTenantManagement(tenant);

                if (tenantManagement != null)
                {
                    packageCode = tenantManagement.PackageCode;
                    isMultiPackage = tenantManagement.IsMultiPackage;
                    mainAdditionalPackageApplied = tenantManagement.MainAdditionalPackageApplied;
                    isManageLicencesPerUser = tenantManagement.ManageLicencesPerUser;

                    totalTenantManagementUsers = tenantManagement.NumberOfUsers;
                    if (tenantManagement.FreeUsers != null)
                    {
                        totalTenantManagementUsers += tenantManagement.FreeUsers;
                    }
                }

                scope.Complete();
            }
        }
        private void GetTenantUsersCount()
        {
            IQueryable<User> allTenantUsers = this.userRepository.GetUsers(tenant);
            allTenantUsers = allTenantUsers.Where(d => d.Contact.Email.ToLower() != "customercare@logitudeworld.com" && !d.Contact.InActive);

            if (isManageLicencesPerUser)
            {
                allTenantUsers = allTenantUsers.Where(d => d.LicencedUser);
            }

            if (mainAdditionalPackageApplied)
            {
                allTenantUsers = allTenantUsers.Where(d => !d.AdditionalPackagesOnly);
            }

            tenantUsersCount = allTenantUsers.Count();
        }
        private void GetLoggedContactId()
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant);
            }

            if (loggedContact != null)
            {
                this.loggedContactId = loggedContact.Id;
            }
        }
        private User GetUser_DB()
        {
            User user = this.userRepository.GetSingleUser(this.userId, this.tenant);
            return user;
        }

        public void Validate()
        {
            if (this.tenant != 0 || this.isDistributor)
            {
                if (this.mainAdditionalPackageApplied)
                {
                    this.ValidateMainAdditionalMode();
                }

                else
                {
                    this.ValidateSingleMultiMode();
                }
            }

            else
            {
                isValid = true;
            }

            if (this.isValid)
            {
                this.Trace();
                this.DeleteUserLicenses();
            }

            else
            {
                throw new ApplicationException(this.exceptionMessage);
            }
        }

        private void ValidateMainAdditionalMode()
        {
            if (this.isNewUser)
            {
                if (tenantUsersCount < totalTenantManagementUsers)
                {
                    this.isValid = true;
                }

                else
                {
                    if (this.additionalPackagesOnly)
                    {
                        this.isValid = true;
                    }
                }

                if (!this.isValid)
                {
                    if (this.isMultiPackage)
                    {
                        throw new Exception("You reached maximum number of users, mark the user as AdditionalPackagesOnly!");
                    }

                    else
                    {
                        throw new Exception("You reached maximum number of users!");
                    }
                }
            }

            else
            {
                if (isMultiPackage)
                {
                    if (this.additionalPackagesOnly_DB && !this.additionalPackagesOnly)
                    {
                        if (this.inactive_DB && !this.inactive)
                        {
                            if (tenantUsersCount < totalTenantManagementUsers)
                            {
                                isValid = true;
                                this.eventNotes = "The user " + this.englishName + " has been activated and Additional Packages Only!";
                            }

                            else
                            {
                                this.exceptionMessage = "Sorry You can't activate this user since you reached the maximum number of users!";
                            }
                        }

                        else
                        {
                            if (this.inactive)
                            {
                                this.isValid = true;
                                this.eventNotes = "The user " + this.englishName + " is Additional Packages Only";
                            }

                            else
                            {
                                if (tenantUsersCount < totalTenantManagementUsers)
                                {
                                    this.isValid = true;
                                    this.eventNotes = "The user " + this.englishName + " is Additional Packages Only";
                                }

                                else
                                {
                                    this.exceptionMessage = "Sorry You can't mark this user as not Additional Packages Only since you reached the maximum number of users!";
                                }
                            }
                        }                        
                    }

                    else
                    {
                        if (this.inactive_DB && !this.inactive)
                        {
                            if (this.additionalPackagesOnly)
                            {
                                this.isValid = true;
                                this.eventNotes = "The user " + this.englishName + " has been activated";
                            }

                            else
                            {
                                if (tenantUsersCount < totalTenantManagementUsers)
                                {
                                    this.isValid = true;
                                    this.eventNotes = "The user " + this.englishName + " has been activated";
                                }

                                else
                                {
                                    this.isValid = false;
                                    this.exceptionMessage = "Sorry You can't activate this user since you reached the maximum number of users!";
                                }
                            }
                        }

                        else
                        {
                            this.isValid = true;
                        }
                    }
                }

                else
                {
                    if (this.inactive_DB && !this.inactive)
                    {
                        if (tenantUsersCount < totalTenantManagementUsers)
                        {
                            this.isValid = true;
                            this.eventNotes = "The user " + this.englishName + " has been activated";
                        }

                        else
                        {
                            this.isValid = false;
                            this.exceptionMessage = "Sorry You can't activate this user since you reached the maximum number of users!";
                        }
                    }

                    else
                    {
                        this.isValid = true;
                    }
                }
            }
        }
        private void ValidateSingleMultiMode()
        {
            if (this.isMultiPackage)
            {
                this.isValid = true;
            }

            else
            {
                if (isNewUser)
                {
                    if (isManageLicencesPerUser)
                    {
                        if (this.licencedUser)
                        {
                            if (tenantUsersCount < totalTenantManagementUsers)
                            {
                                this.isValid = true;
                            }
                        }

                        else
                        {
                            this.isValid = true;
                        }
                    }

                    else
                    {
                        if (tenantUsersCount < totalTenantManagementUsers)
                        {
                            this.isValid = true;
                        }
                    }

                    if (!isValid)
                    {
                        this.exceptionMessage = "Sorry You reached the maximum number of users!";
                    }
                }

                else
                {
                    if (isManageLicencesPerUser)
                    {
                        if (!this.licencedUser_DB && this.licencedUser)
                        {
                            if (tenantUsersCount < totalTenantManagementUsers)
                            {
                                this.isValid = true;
                                this.eventNotes = "The user " + this.englishName + " has been Licenced!";
                            }

                            else
                            {
                                this.exceptionMessage = "Sorry You can't Licence this user since you reached the maximum number of users!";
                            }
                        }

                        else
                        {
                            if (this.inactive_DB && !this.inactive)
                            {
                                this.eventNotes = "The user " + this.englishName + " has been activated and Licensed!";
                            }

                            this.isValid = true;
                        }
                    }

                    else
                    {
                        if (this.inactive_DB && !this.inactive)
                        {
                            if (tenantUsersCount < totalTenantManagementUsers)
                            {
                                this.isValid = true;
                                this.eventNotes = "The user " + this.englishName + " has been activated";
                            }

                            else
                            {
                                this.isValid = false;
                                this.exceptionMessage = "Sorry You can't activate this user since you reached the maximum number of users!";
                            }
                        }

                        else
                        {
                            this.isValid = true;
                        }
                    }
                }
            }
        }

        private void Trace()
        {
            if (!string.IsNullOrEmpty(this.eventNotes))
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = 0,
                    EventTypeCode = "UPMG",
                    UserId = this.loggedContactId,
                    EntityId = tenant.ToString(),
                    ObjectTableName = "TenantManagement",
                    Notes = this.eventNotes,
                });
            }
        }
        private void DeleteUserLicenses()
        {
            if (isMultiPackage)
            {
                if (this.inactive && !this.inactive_DB)
                {
                    UserLicenseRepository userLicenseRepository = new UserLicenseRepository(this.commonDataContext);
                    List<UserLicense> licenses = userLicenseRepository.GetUserLicensesByUserId(this.userId, tenant);

                    if (licenses.Count > 0)
                    {
                        foreach (UserLicense item in licenses)
                        {
                            userLicenseRepository.Remove(item);
                        }

                        userLicenseRepository.SubmitChanges();
                    }
                }
            }
        }
    }
}
