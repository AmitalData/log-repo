using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
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
        private UserRepository userRepository;
        private UserLicenseRepository userLicenseRepository;
        public NumberOfUsersService(UserRepository userRepository, int tenant)
        {
            this.tenant = tenant;
            this.userRepository = userRepository;
            this.userLicenseRepository = new UserLicenseRepository(userRepository.context);

            this.InitializeService();            
        }

        private void InitializeService()
        {
            this.GetTenantManagementProperties();
            this.GetTenantUsersCount();
            this.GetLoggedContactId();
        }

        private string packageCode;
        private int? totalTenantManagementUsers = 0;
        private bool isManageLicencesPerUser = false;
        private bool isMultiPackage = false;
        private bool mainAdditionalPackageApplied = false;
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

        private int tenantUsersCount = 0;
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
        
        private string loggedContactId = null;
        private void GetLoggedContactId()
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), 0);

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant);
            }

            if(loggedContact != null)
            {
                this.loggedContactId = loggedContact.Id;
            }
        }

        public void CheckNumberOfUsersOnCreateUser(NumberOfUsersArgs numberOfUsersArgs)
        {
            bool canAddUser = false;

            if (this.tenant == 0 && !numberOfUsersArgs.UserPMIsDistributor)
            {
                canAddUser = true;
            }

            else
            {
                canAddUser = this.ValidateTenantManagementPackagesMode(numberOfUsersArgs);
            }

            if (!canAddUser)
            {
                throw new Exception("Sorry You reached the maximum number of users!");
            }
        }
        private bool ValidateTenantManagementPackagesMode(NumberOfUsersArgs numberOfUsersArgs)
        {
            bool isVlalid = false;

            if (mainAdditionalPackageApplied)
            {
                isVlalid = this.ValidateNewMode(numberOfUsersArgs);
            }

            else
            {
                isVlalid = this.ValidateOldMode(numberOfUsersArgs);
            }

            return isVlalid;
        }
        private bool ValidateNewMode(NumberOfUsersArgs numberOfUsersArgs)
        {
            bool isVlalid = false;

            if (tenantUsersCount < totalTenantManagementUsers)
            {
                isVlalid = true;
            }

            else
            {
                if (numberOfUsersArgs.UserPMAdditionalPackagesOnly)
                {
                    isVlalid = true;
                }

                else
                {
                    throw new Exception("You reached maximum number of users, mark the user as AdditionalPackagesOnly!");
                }
            }

            return isVlalid;
        }
        private bool ValidateOldMode(NumberOfUsersArgs numberOfUsersArgs)
        {
            bool isVlalid = false;

            if (isMultiPackage)
            {
                isVlalid = true;
            }

            else
            {
                if (isManageLicencesPerUser)
                {
                    if (numberOfUsersArgs.UserPMLicencedUser)
                    {
                        if (tenantUsersCount < totalTenantManagementUsers)
                        {
                            isVlalid = true;
                        }
                    }

                    else
                    {
                        isVlalid = true;
                    }
                }

                else
                {
                    if (tenantUsersCount < totalTenantManagementUsers)
                    {
                        isVlalid = true;
                    }
                }
            }

            return isVlalid;
        }
        
        public void CheckNumberOfUsersOnUpdateUser(NumberOfUsersArgs numberOfUsersArgs)
        {
            bool isUsersCountAllowed = false;

            if (isManageLicencesPerUser)
            {
                if (!numberOfUsersArgs.UserPOCOLicencedUser && numberOfUsersArgs.UserPMLicencedUser)
                {
                    if (isMultiPackage)
                    {
                        isUsersCountAllowed = true;
                    }

                    else if (tenantUsersCount < totalTenantManagementUsers)
                    {
                        isUsersCountAllowed = true;
                    }

                    else if (tenant == 0 && !numberOfUsersArgs.UserPMIsDistributor)
                    {
                        isUsersCountAllowed = true;
                    }

                    if (isUsersCountAllowed)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "UPMG",
                            UserId = this.loggedContactId,
                            EntityId = tenant.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = "The user " + numberOfUsersArgs.UserEnglishName + " is Licenced!",
                        });
                    }

                    else
                    {
                        throw new ApplicationException("Sorry You can't Licence this user since you reached the maximum number of users!");
                    }
                }
            }

            else
            {
                string eventNotes = null;

                if (mainAdditionalPackageApplied)
                {
                    if (numberOfUsersArgs.UserPOCOInactive && !numberOfUsersArgs.UserPMInactive)
                    {
                        if (!numberOfUsersArgs.UserPOCOAdditionalPackagesOnly && numberOfUsersArgs.UserPMAdditionalPackagesOnly)
                        {
                            isUsersCountAllowed = true;
                            eventNotes = "The user " + numberOfUsersArgs.UserEnglishName + " has been activated and Additional Packages Only!";
                        }

                        else
                        {
                            if (tenantUsersCount < totalTenantManagementUsers)
                            {
                                isUsersCountAllowed = true;
                                eventNotes = "The user " + numberOfUsersArgs.UserEnglishName + " has been activated";
                            }

                            else
                            {
                                throw new Exception("Sorry You can't activate this user since you reached the maximum number of users!");
                            }
                        }                       
                    }

                    if (numberOfUsersArgs.UserPOCOAdditionalPackagesOnly && !numberOfUsersArgs.UserPMAdditionalPackagesOnly)
                    {
                        if (tenantUsersCount < totalTenantManagementUsers)
                        {
                            isUsersCountAllowed = true;
                            eventNotes = "The user " + numberOfUsersArgs.UserEnglishName + " is Additional Packages Only";
                        }

                        else
                        {
                            throw new Exception("Sorry You can't activate this user since you reached the maximum number of users!");
                        }
                    }

                    if (isUsersCountAllowed)
                    {
                        UserLicense userLicense = userLicenseRepository.GetSingleUserLicenseByUserAndPackage(numberOfUsersArgs.UserId, packageCode, tenant);
                        if (userLicense == null)
                        {
                            userLicense = new UserLicense()
                            {
                                Id = IdCounter.GetNumber("UserLicense", tenant).ToString(),
                                UserId = numberOfUsersArgs.UserId,
                                PackageCode = packageCode,
                                Tenant = tenant,
                            };

                            userLicenseRepository.Add(userLicense);
                        }

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = 0,
                            EventTypeCode = "UPMG",
                            UserId = this.loggedContactId,
                            EntityId = tenant.ToString(),
                            ObjectTableName = "TenantManagement",
                            Notes = eventNotes,
                        });
                    }
                }

                else
                {
                    if (numberOfUsersArgs.UserPOCOInactive && !numberOfUsersArgs.UserPMInactive)
                    {
                        if (isMultiPackage)
                        {
                            isUsersCountAllowed = true;
                        }

                        else if (tenantUsersCount < totalTenantManagementUsers)
                        {
                            isUsersCountAllowed = true;
                        }

                        else if (tenant == 0 && !numberOfUsersArgs.UserPMIsDistributor)
                        {
                            isUsersCountAllowed = true;
                        }

                        if (isUsersCountAllowed)
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = 0,
                                EventTypeCode = "UPMG",
                                UserId = this.loggedContactId,
                                EntityId = tenant.ToString(),
                                ObjectTableName = "TenantManagement",
                                Notes = "The user " + numberOfUsersArgs.UserEnglishName + " has been activated",
                            });
                        }

                        else
                        {
                            throw new Exception("Sorry You can't activate this user since you reached the maximum number of users!");
                        }
                    }
                }
            }
        }
        
        public void DeleteUserLicenses(NumberOfUsersArgs numberOfUsersArgs)
        {
            if (isMultiPackage || mainAdditionalPackageApplied)
            {
                if (numberOfUsersArgs.UserPMInactive && !numberOfUsersArgs.UserPOCOInactive)
                {
                    List<UserLicense> licenses = userLicenseRepository.GetUserLicensesByUserId(numberOfUsersArgs.UserId, tenant);

                    if (licenses.Count > 0)
                    {
                        foreach (UserLicense item in licenses)
                        {
                            userLicenseRepository.Remove(item);
                        }
                    }
                }
            }
        }
    }

    public class NumberOfUsersArgs
    {
        public string UserId { get; set; }
        public string UserEnglishName { get; set; }
        public bool UserPOCOInactive { get; set; }
        public bool UserPMInactive { get; set; }
        public bool UserPOCOLicencedUser { get; set; }
        public bool UserPMLicencedUser { get; set; }
        public bool UserPOCOAdditionalPackagesOnly { get; set; }
        public bool UserPMAdditionalPackagesOnly { get; set; }
        public bool UserPMIsDistributor { get; set; }
        public bool IsNewUser { get; set; }
    }
}
