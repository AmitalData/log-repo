using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
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

namespace Logitude.BL.GlobalModel.Tools.Validating
{
    public class TenantManagementValidating
    {
        public static void Validate(TenantManagementPM entityPM, TenantManagement entityPOCO, bool isNewEntity, TenantManagementRepository entityRepository)
        {
            if (string.IsNullOrEmpty(entityPM.PackageCode))
            {
                throw new ApplicationException("Main Package field is required");
            }

            ValidateCCSParameter(entityPM, entityRepository);
            ValidateNumberOfUsers(entityPM);
            ValidateConnectedAirline(entityPM, entityRepository);
            ValidateSupportEmail(entityPM, entityRepository);
        }

        private static void ValidateCCSParameter(TenantManagementPM entityPM, TenantManagementRepository entityRepository)
        {
            if (!string.IsNullOrEmpty(entityPM.TTY) || !string.IsNullOrEmpty(entityPM.PIMA))
            {
                IQueryable<TenantManagement> iQueryable = entityRepository.GetAllTenants();

                if (!string.IsNullOrEmpty(entityPM.TTY))
                {
                    if (iQueryable.Where(d => d.TTY == entityPM.TTY && d.Id != entityPM.Id).Any())
                    {
                        throw new ApplicationException("the TTY field is alredy used by another tenant");
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.PIMA))
                {
                    if (iQueryable.Where(d => d.PIMA == entityPM.PIMA && d.Id != entityPM.Id).Any())
                    {
                        throw new ApplicationException("the PIMA field is alredy used by another tenant");
                    }
                }
            }
        }
        private static void ValidateNumberOfUsers(TenantManagementPM entityPM)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (entityPM.IsMultiPackage)
                {
                    UserLicenseRepository userLicenseRepository = new UserLicenseRepository(entityPM.Id);
                    IQueryable<UserLicense> allTenantLicenses = userLicenseRepository.GetUserLicenses(entityPM.Id);

                    foreach(TenantManagementLicensePM item in entityPM.TenantManagementLicenses)
                    {
                        int licensesCount = allTenantLicenses.Where(d => d.PackageCode == item.PackageCode).Count();

                        int num1 = item.FreeUsers == null ? 0 : item.FreeUsers.Value;
                        int num2 = item.NumberOfUsers == null ? 0 : item.NumberOfUsers.Value;
                        
                        if (licensesCount > (num1 + num2))
                        {
                            throw new Exception("You can't change the number of users to less than " + licensesCount + " for package " + item.PackageCode);
                        }
                    }
                }

                else
                {
                    int tenantUsers = 0;
                    int totalUsers = 0;
                    string usersType = "active";

                    UserRepository userRepository = new UserRepository(entityPM.Id);
                    IQueryable<User> allTenantUsers = userRepository.GetUsers(entityPM.Id);

                    allTenantUsers = allTenantUsers.Where(d => d.Contact.Email.ToLower() != "customercare@logitudeworld.com" && d.Contact.InActive == false);

                    if (entityPM.ManageLicencesPerUser)
                    {
                        allTenantUsers = allTenantUsers.Where(d => d.LicencedUser == true);
                        usersType = "licenced";
                    }

                    if (entityPM.MainAdditionalPackageApplied)
                    {
                        allTenantUsers = allTenantUsers.Where(d => !d.AdditionalPackagesOnly);
                    }

                    tenantUsers = allTenantUsers.Count();

                    int num1 = entityPM.TotalFreeUsers == null ? 0 : entityPM.TotalFreeUsers.Value;
                    int num2 = entityPM.TotalNumberOfUsers == null ? 0 : entityPM.TotalNumberOfUsers.Value;
                    totalUsers = num1 + num2;

                    if (tenantUsers > totalUsers)
                    {
                        throw new Exception("You can't change the number of users to less than " + tenantUsers + " (Number of " + usersType + " users)");
                    }
                }                

                scope.Complete();
            }            
        }
        private static void ValidateConnectedAirline(TenantManagementPM entityPM, TenantManagementRepository entityRepository)
        {
            if (!string.IsNullOrEmpty(entityPM.TenantTypeCode) && entityPM.TenantTypeCode == "AIR")
            {
                if (!string.IsNullOrEmpty(entityPM.TenantConnectedToAirlineCode))
                {
                    Airline connectedAirline = null;
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        AirlineRepository airlineRepository = new AirlineRepository(entityPM.Id);
                        connectedAirline = airlineRepository.GetSingleAirlineByCode(entityPM.TenantConnectedToAirlineCode, 0);
                        scope.Complete();
                    }

                    if (connectedAirline != null)
                    {
                        IQueryable<TenantManagement> tenants = entityRepository.GetAllTenants();

                        if (tenants.Where(d => d.TenantConnectedToAirlineCode == entityPM.TenantConnectedToAirlineCode && d.Id != entityPM.Id).Any())
                        {
                            throw new Exception("The Airline you are trying to connect has been connected to another tenant");
                        }

                        else
                        {

                        }
                    }

                    else
                    {
                        throw new Exception("No airline found with such code");
                    }
                }
            }
        }
        private static void ValidateSupportEmail(TenantManagementPM entityPM, TenantManagementRepository entityRepository)
        {
            if (entityPM.SupportActivated)
            {
                if (string.IsNullOrEmpty(entityPM.SupportDomain))
                {
                    throw new Exception("Support Domain is Required");
                }

                else
                {
                    if (entityRepository.CheckSupportEmailTenantManagement(entityPM.SupportDomain, entityPM.Id))
                    {
                        throw new Exception("Support Domain is used");
                    }
                }
            }
        }        
    }
}