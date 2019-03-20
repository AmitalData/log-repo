using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class UserQuery
    {
        UserRepository repository;

        public UserQuery()
        {
            repository = new UserRepository();
        }

        public UserQuery(int tenant)
        {
            repository = new UserRepository(tenant);
        }

        public UserQuery(UserRepository userRepository)
        {
            repository = userRepository;
        }

        public UserPM GetSinglePM(string id, int tenant)
        {
            string entityName = "UserPM" + id + tenant;
            UserPM entity;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = (from a in repository.context.Users.Include("Branch").Include("Contact").Include("Department").Include("Freelancer").Include("ProductType")
                              where a.Tenant == tenant
                              && a.Id == id
                              select new UserPM()
                              {
                                  BranchId = a.BranchId,
                                  BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                  DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                  DepartmentId = a.DepartmentId,
                                  Id = a.Id,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  Email = a.Contact.Email,
                                  EnglishName = a.Contact.EnglishName,
                                  Anniversary = a.Contact.Anniversary,
                                  Birthday = a.Contact.Birthday,
                                  BusinessPhone = a.Contact.BusinessPhone,
                                  FacebookId = a.Contact.FacebookId,
                                  Fax = a.Contact.Fax,
                                  InActive = a.Contact.InActive,
                                  LocalName = a.Contact.LocalName,
                                  SearchFields = a.SearchFields,
                                  Mobile = a.Contact.Mobile,
                                  Position = a.Contact.Position,
                                  ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                  IsBranchRestricted = a.IsBranchRestricted,
                                  IsSalesman = a.IsSalesman,
                                  IsFreelancer = a.IsFreelancer,
                                  FreelancerId = a.FreelancerId,
                                  FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                  BusinessUnitId = a.BusinessUnitId,
                                  Code = a.Code,
                                  CreateDate = a.CreateDate,
                                  ExpirationDate = a.ExpirationDate,
                                  LicencedUser = a.LicencedUser,
                                  IsProductRestricted = a.IsProductRestricted,
                                  ProductTypeCode = a.ProductTypeCode,
                                  ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                  IsDistributor = a.IsDistributor,
                                  DistributorCode = a.DistributorCode,
                                  IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                  PersonalId = a.PersonalId,
                                  IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                  Technology = a.Technology,
                                  SetAngularAsDefault = a.SetAngularAsDefault,
                                  DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                  IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                  DocumentFilingInbox = a.DocumentFilingInbox,
                                  ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                  ShowInboxToolTip = a.ShowInboxToolTip,
                                  ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                  UserRoles = a.UserRoles,
                                  ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                        UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                        UserLastLoginQuery query = new UserLastLoginQuery(rep);

                        entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                        UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                        UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                        entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                        UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                        UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                        entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                }
                else
                {
                    entity = (UserPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            else
            {
                entity = (from a in repository.context.Users.Include("Contact").Include("Freelancer")
                          where a.Tenant == tenant
                          && a.Id == id
                          select new UserPM()
                          {
                              BranchId = a.BranchId,
                              DepartmentId = a.DepartmentId,
                              Id = a.Id,
                              Notes = a.Notes,
                              Tenant = a.Tenant,
                              Email = a.Contact.Email,
                              EnglishName = a.Contact.EnglishName,
                              Anniversary = a.Contact.Anniversary,
                              Birthday = a.Contact.Birthday,
                              BusinessPhone = a.Contact.BusinessPhone,
                              FacebookId = a.Contact.FacebookId,
                              Fax = a.Contact.Fax,
                              InActive = a.Contact.InActive,
                              LocalName = a.Contact.LocalName,
                              SearchFields = a.SearchFields,
                              Mobile = a.Contact.Mobile,
                              Position = a.Contact.Position,
                              ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                              IsBranchRestricted = a.IsBranchRestricted,
                              IsSalesman = a.IsSalesman,
                              FreelancerId = a.FreelancerId,
                              IsFreelancer = a.IsFreelancer,
                              FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                              BusinessUnitId = a.BusinessUnitId,
                              Code = a.Code,
                              CreateDate = a.CreateDate,
                              ExpirationDate = a.ExpirationDate,
                              LicencedUser = a.LicencedUser,
                              IsProductRestricted = a.IsProductRestricted,
                              ProductTypeCode = a.ProductTypeCode,
                              IsDistributor = a.IsDistributor,
                              DistributorCode = a.DistributorCode,
                              IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                              IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                              PersonalId = a.PersonalId,
                              Technology = a.Technology,
                              SetAngularAsDefault = a.SetAngularAsDefault,
                              DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                              IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                              DocumentFilingInbox = a.DocumentFilingInbox,
                              ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                              ShowInboxToolTip = a.ShowInboxToolTip,
                              ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                              UserRoles = a.UserRoles,
                              ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                          }).FirstOrDefault();

                if (entity != null)
                {
                    entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                    UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                    UserLastLoginQuery query = new UserLastLoginQuery(rep);
                    entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                    UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                    UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                    entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                    UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                    UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                    entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                }
            }

            return entity;
        }

        public UserPM GetSingleUserPM(string id, int tenant, bool fromcache)
        {
            string entityName = "UserPM" + id + tenant;
            UserPM entity;

            if (fromcache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Users.Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("ProductType")
                                  where a.Tenant == tenant
                                  && a.Id == id
                                  select new UserPM()
                                  {
                                      BranchId = a.BranchId,
                                      BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                      DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                      DepartmentId = a.DepartmentId,
                                      Id = a.Id,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      Email = a.Contact.Email,
                                      EnglishName = a.Contact.EnglishName,
                                      Anniversary = a.Contact.Anniversary,
                                      Birthday = a.Contact.Birthday,
                                      BusinessPhone = a.Contact.BusinessPhone,
                                      FacebookId = a.Contact.FacebookId,
                                      Fax = a.Contact.Fax,
                                      InActive = a.Contact.InActive,
                                      LocalName = a.Contact.LocalName,
                                      SearchFields = a.SearchFields,
                                      Mobile = a.Contact.Mobile,
                                      Position = a.Contact.Position,
                                      ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                      IsBranchRestricted = a.IsBranchRestricted,
                                      IsSalesman = a.IsSalesman,
                                      FreelancerId = a.FreelancerId,
                                      IsFreelancer = a.IsFreelancer,
                                      FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                      BusinessUnitId = a.BusinessUnitId,
                                      Code = a.Code,
                                      CreateDate = a.CreateDate,
                                      ExpirationDate = a.ExpirationDate,
                                      LicencedUser = a.LicencedUser,
                                      IsProductRestricted = a.IsProductRestricted,
                                      ProductTypeCode = a.ProductTypeCode,
                                      ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                      IsDistributor = a.IsDistributor,
                                      DistributorCode = a.DistributorCode,
                                      IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                      IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                      PersonalId = a.PersonalId,
                                      Technology = a.Technology,
                                      SetAngularAsDefault = a.SetAngularAsDefault,
                                      DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                      IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                      DocumentFilingInbox = a.DocumentFilingInbox,
                                      ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                      ShowInboxToolTip = a.ShowInboxToolTip,
                                      ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                      UserRoles = a.UserRoles,
                                      ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                            UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                            UserLastLoginQuery query = new UserLastLoginQuery(rep);
                            entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                            UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                            UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                            entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                            UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                            UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                            entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();

                            if (CacheManager.CacheWrapper.Get(entityName) == null)
                            {
                                if (entity != null)
                                {
                                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                        }
                    }
                    else
                    {
                        entity = (UserPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Users.Include("Contact").Include("Freelancer")
                              where a.Tenant == tenant
                              && a.Id == id
                              select new UserPM()
                              {
                                  BranchId = a.BranchId,
                                  DepartmentId = a.DepartmentId,
                                  Id = a.Id,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  Email = a.Contact.Email,
                                  EnglishName = a.Contact.EnglishName,
                                  Anniversary = a.Contact.Anniversary,
                                  Birthday = a.Contact.Birthday,
                                  BusinessPhone = a.Contact.BusinessPhone,
                                  FacebookId = a.Contact.FacebookId,
                                  Fax = a.Contact.Fax,
                                  InActive = a.Contact.InActive,
                                  LocalName = a.Contact.LocalName,
                                  SearchFields = a.SearchFields,
                                  Mobile = a.Contact.Mobile,
                                  Position = a.Contact.Position,
                                  ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                  IsBranchRestricted = a.IsBranchRestricted,
                                  IsSalesman = a.IsSalesman,
                                  FreelancerId = a.FreelancerId,
                                  IsFreelancer = a.IsFreelancer,
                                  FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                  BusinessUnitId = a.BusinessUnitId,
                                  Code = a.Code,
                                  CreateDate = a.CreateDate,
                                  ExpirationDate = a.ExpirationDate,
                                  LicencedUser = a.LicencedUser,
                                  IsProductRestricted = a.IsProductRestricted,
                                  ProductTypeCode = a.ProductTypeCode,
                                  IsDistributor = a.IsDistributor,
                                  DistributorCode = a.DistributorCode,
                                  IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                  IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                  PersonalId = a.PersonalId,
                                  Technology = a.Technology,
                                  SetAngularAsDefault = a.SetAngularAsDefault,
                                  DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                  IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                  DocumentFilingInbox = a.DocumentFilingInbox,
                                  ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                  ShowInboxToolTip = a.ShowInboxToolTip,
                                  ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                  UserRoles = a.UserRoles,
                                  ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                        UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                        UserLastLoginQuery query = new UserLastLoginQuery(rep);
                        entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                        UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                        UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                        entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                        UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                        UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                        entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                    }
                }
            }
            else
            {
                entity = (from a in repository.context.Users.Include("Contact")
                          where a.Tenant == tenant
                          && a.Id == id
                          select new UserPM()
                          {
                              BranchId = a.BranchId,
                              DepartmentId = a.DepartmentId,
                              Id = a.Id,
                              Notes = a.Notes,
                              Tenant = a.Tenant,
                              Email = a.Contact.Email,
                              EnglishName = a.Contact.EnglishName,
                              Anniversary = a.Contact.Anniversary,
                              Birthday = a.Contact.Birthday,
                              BusinessPhone = a.Contact.BusinessPhone,
                              FacebookId = a.Contact.FacebookId,
                              Fax = a.Contact.Fax,
                              InActive = a.Contact.InActive,
                              LocalName = a.Contact.LocalName,
                              SearchFields = a.SearchFields,
                              Mobile = a.Contact.Mobile,
                              Position = a.Contact.Position,
                              ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                              IsBranchRestricted = a.IsBranchRestricted,
                              IsSalesman = a.IsSalesman,
                              FreelancerId = a.FreelancerId,
                              IsFreelancer = a.IsFreelancer,
                              FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                              BusinessUnitId = a.BusinessUnitId,
                              Code = a.Code,
                              CreateDate = a.CreateDate,
                              ExpirationDate = a.ExpirationDate,
                              LicencedUser = a.LicencedUser,
                              IsProductRestricted = a.IsProductRestricted,
                              ProductTypeCode = a.ProductTypeCode,
                              IsDistributor = a.IsDistributor,
                              DistributorCode = a.DistributorCode,
                              IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                              IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                              PersonalId = a.PersonalId,
                              Technology = a.Technology,
                              SetAngularAsDefault = a.SetAngularAsDefault,
                              DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                              IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                              DocumentFilingInbox = a.DocumentFilingInbox,
                              ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                              ShowInboxToolTip = a.ShowInboxToolTip,
                              ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                              UserRoles = a.UserRoles,
                              ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                          }).FirstOrDefault();

                if (entity != null)
                {
                    entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                    UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                    UserLastLoginQuery query = new UserLastLoginQuery(rep);
                    entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                    UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                    UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                    entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                    UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                    UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                    entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                }
            }

            return entity;
        }

        public UserPM GetSingleUserPMByCode(string code, int tenant, bool fromcache)
        {
            string entityName = "UserPM" + code + tenant;
            UserPM entity;

            if (fromcache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Users.Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("ProductType")
                                  where a.Tenant == tenant
                                  && a.Code == code
                                  select new UserPM()
                                  {
                                      BranchId = a.BranchId,
                                      BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                      DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                      DepartmentId = a.DepartmentId,
                                      Id = a.Id,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      Email = a.Contact.Email,
                                      EnglishName = a.Contact.EnglishName,
                                      Anniversary = a.Contact.Anniversary,
                                      Birthday = a.Contact.Birthday,
                                      BusinessPhone = a.Contact.BusinessPhone,
                                      FacebookId = a.Contact.FacebookId,
                                      Fax = a.Contact.Fax,
                                      InActive = a.Contact.InActive,
                                      LocalName = a.Contact.LocalName,
                                      SearchFields = a.SearchFields,
                                      Mobile = a.Contact.Mobile,
                                      Position = a.Contact.Position,
                                      ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                      IsBranchRestricted = a.IsBranchRestricted,
                                      IsSalesman = a.IsSalesman,
                                      IsFreelancer = a.IsFreelancer,
                                      FreelancerId = a.FreelancerId,
                                      FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                      BusinessUnitId = a.BusinessUnitId,
                                      Code = a.Code,
                                      CreateDate = a.CreateDate,
                                      ExpirationDate = a.ExpirationDate,
                                      LicencedUser = a.LicencedUser,
                                      IsProductRestricted = a.IsProductRestricted,
                                      ProductTypeCode = a.ProductTypeCode,
                                      ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                      IsDistributor = a.IsDistributor,
                                      DistributorCode = a.DistributorCode,
                                      IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                      PersonalId = a.PersonalId,
                                      IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                      Technology = a.Technology,
                                      SetAngularAsDefault = a.SetAngularAsDefault,
                                      DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                      IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                      DocumentFilingInbox = a.DocumentFilingInbox,
                                      ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                      ShowInboxToolTip = a.ShowInboxToolTip,
                                      ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                      UserRoles = a.UserRoles,
                                      ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                            UserLastLoginQuery query = new UserLastLoginQuery(rep);
                            entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                            UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                            UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                            entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                            UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                            UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                            entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                            
                            if (CacheManager.CacheWrapper.Get(entityName) == null)
                            {
                                if (entity != null)
                                {
                                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                        }
                    }
                    else
                    {
                        entity = (UserPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Users.Include("Contact").Include("Freelancer")
                              where a.Tenant == tenant
                              && a.Code == code
                              select new UserPM()
                              {
                                  BranchId = a.BranchId,
                                  BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                  DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                  DepartmentId = a.DepartmentId,
                                  Id = a.Id,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  Email = a.Contact.Email,
                                  EnglishName = a.Contact.EnglishName,
                                  Anniversary = a.Contact.Anniversary,
                                  Birthday = a.Contact.Birthday,
                                  BusinessPhone = a.Contact.BusinessPhone,
                                  FacebookId = a.Contact.FacebookId,
                                  Fax = a.Contact.Fax,
                                  InActive = a.Contact.InActive,
                                  LocalName = a.Contact.LocalName,
                                  SearchFields = a.SearchFields,
                                  Mobile = a.Contact.Mobile,
                                  Position = a.Contact.Position,
                                  ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                  IsBranchRestricted = a.IsBranchRestricted,
                                  IsSalesman = a.IsSalesman,
                                  IsFreelancer = a.IsFreelancer,
                                  FreelancerId = a.FreelancerId,
                                  FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                  BusinessUnitId = a.BusinessUnitId,
                                  Code = a.Code,
                                  CreateDate = a.CreateDate,
                                  ExpirationDate = a.ExpirationDate,
                                  LicencedUser = a.LicencedUser,
                                  IsProductRestricted = a.IsProductRestricted,
                                  ProductTypeCode = a.ProductTypeCode,
                                  ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                  IsDistributor = a.IsDistributor,
                                  DistributorCode = a.DistributorCode,
                                  IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                  PersonalId = a.PersonalId,
                                  IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                  Technology = a.Technology,
                                  SetAngularAsDefault = a.SetAngularAsDefault,
                                  DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                  IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                  DocumentFilingInbox = a.DocumentFilingInbox,
                                  ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                  ShowInboxToolTip = a.ShowInboxToolTip,
                                  ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                  UserRoles = a.UserRoles,
                                  ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                        UserLastLoginQuery query = new UserLastLoginQuery(rep);
                        entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                        UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                        UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                        entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                        UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                        UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                        entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                    }
                }
            }
            else
            {
                entity = (from a in repository.context.Users.Include("Contact")
                          where a.Tenant == tenant
                          && a.Code == code
                          select new UserPM()
                          {
                              BranchId = a.BranchId,
                              BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                              DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                              DepartmentId = a.DepartmentId,
                              Id = a.Id,
                              Notes = a.Notes,
                              Tenant = a.Tenant,
                              Email = a.Contact.Email,
                              EnglishName = a.Contact.EnglishName,
                              Anniversary = a.Contact.Anniversary,
                              Birthday = a.Contact.Birthday,
                              BusinessPhone = a.Contact.BusinessPhone,
                              FacebookId = a.Contact.FacebookId,
                              Fax = a.Contact.Fax,
                              InActive = a.Contact.InActive,
                              LocalName = a.Contact.LocalName,
                              SearchFields = a.SearchFields,
                              Mobile = a.Contact.Mobile,
                              Position = a.Contact.Position,
                              ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                              IsBranchRestricted = a.IsBranchRestricted,
                              IsSalesman = a.IsSalesman,
                              IsFreelancer = a.IsFreelancer,
                              FreelancerId = a.FreelancerId,
                              FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                              BusinessUnitId = a.BusinessUnitId,
                              Code = a.Code,
                              CreateDate = a.CreateDate,
                              ExpirationDate = a.ExpirationDate,
                              LicencedUser = a.LicencedUser,
                              IsProductRestricted = a.IsProductRestricted,
                              ProductTypeCode = a.ProductTypeCode,
                              ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                              IsDistributor = a.IsDistributor,
                              DistributorCode = a.DistributorCode,
                              IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                              PersonalId = a.PersonalId,
                              IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                              Technology = a.Technology,
                              SetAngularAsDefault = a.SetAngularAsDefault,
                              DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                              IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                              DocumentFilingInbox = a.DocumentFilingInbox,
                              ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                              ShowInboxToolTip = a.ShowInboxToolTip,
                              ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                              UserRoles = a.UserRoles,
                              ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                          }).FirstOrDefault();

                if (entity != null)
                {
                    UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                    UserLastLoginQuery query = new UserLastLoginQuery(rep);
                    entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                    UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                    UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                    entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                    UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                    UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                    entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                }
            }

            return entity;
        }

        public UserPM GetSingleUserPMByEmail(string email, int tenant, bool fromcache)
        {
            string entityName = "UserPM" + email + tenant;
            UserPM entity;

            if (fromcache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Users.Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("ProductType")
                                  where a.Tenant == tenant
                                  && a.Contact.Email == email
                                  select new UserPM()
                                  {
                                      BranchId = a.BranchId,
                                      BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                      DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                      DepartmentId = a.DepartmentId,
                                      Id = a.Id,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      Email = a.Contact.Email,
                                      EnglishName = a.Contact.EnglishName,
                                      Anniversary = a.Contact.Anniversary,
                                      Birthday = a.Contact.Birthday,
                                      BusinessPhone = a.Contact.BusinessPhone,
                                      FacebookId = a.Contact.FacebookId,
                                      Fax = a.Contact.Fax,
                                      InActive = a.Contact.InActive,
                                      LocalName = a.Contact.LocalName,
                                      SearchFields = a.SearchFields,
                                      Mobile = a.Contact.Mobile,
                                      Position = a.Contact.Position,
                                      ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                      IsBranchRestricted = a.IsBranchRestricted,
                                      IsSalesman = a.IsSalesman,
                                      FreelancerId = a.FreelancerId,
                                      IsFreelancer = a.IsFreelancer,
                                      FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                      BusinessUnitId = a.BusinessUnitId,
                                      Code = a.Code,
                                      CreateDate = a.CreateDate,
                                      ExpirationDate = a.ExpirationDate,
                                      LicencedUser = a.LicencedUser,
                                      IsProductRestricted = a.IsProductRestricted,
                                      ProductTypeCode = a.ProductTypeCode,
                                      ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                      IsDistributor = a.IsDistributor,
                                      DistributorCode = a.DistributorCode,
                                      IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                      IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                      PersonalId = a.PersonalId,
                                      Technology = a.Technology,
                                      SetAngularAsDefault = a.SetAngularAsDefault,
                                      DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                      DontShowLocal = a.Contact.DontShowLocalLabels,
                                      IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                      DocumentFilingInbox = a.DocumentFilingInbox,
                                      ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                      ShowInboxToolTip = a.ShowInboxToolTip,
                                      ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                      UserRoles = a.UserRoles,
                                      ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                            UserLastLoginQuery query = new UserLastLoginQuery(rep);
                            entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);
                            entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);
                            UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                            UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                            entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                            UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                            UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                            entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();

                            if (CacheManager.CacheWrapper.Get(entityName) == null)
                            {
                                //if (entity != null)
                                //{
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                //}
                            }
                        }
                    }
                    else
                    {
                        entity = (UserPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Users.Include("Contact").Include("Freelancer")
                              where a.Tenant == tenant
                              && a.Contact.Email == email
                              select new UserPM()
                              {
                                  BranchId = a.BranchId,
                                  DepartmentId = a.DepartmentId,
                                  Id = a.Id,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  Email = a.Contact.Email,
                                  EnglishName = a.Contact.EnglishName,
                                  Anniversary = a.Contact.Anniversary,
                                  Birthday = a.Contact.Birthday,
                                  BusinessPhone = a.Contact.BusinessPhone,
                                  FacebookId = a.Contact.FacebookId,
                                  Fax = a.Contact.Fax,
                                  InActive = a.Contact.InActive,
                                  LocalName = a.Contact.LocalName,
                                  SearchFields = a.SearchFields,
                                  Mobile = a.Contact.Mobile,
                                  Position = a.Contact.Position,
                                  ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                  IsBranchRestricted = a.IsBranchRestricted,
                                  IsSalesman = a.IsSalesman,
                                  FreelancerId = a.FreelancerId,
                                  IsFreelancer = a.IsFreelancer,
                                  FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                  BusinessUnitId = a.BusinessUnitId,
                                  Code = a.Code,
                                  CreateDate = a.CreateDate,
                                  ExpirationDate = a.ExpirationDate,
                                  LicencedUser = a.LicencedUser,
                                  IsProductRestricted = a.IsProductRestricted,
                                  ProductTypeCode = a.ProductTypeCode,
                                  IsDistributor = a.IsDistributor,
                                  DistributorCode = a.DistributorCode,
                                  IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                  IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                  PersonalId = a.PersonalId,
                                  Technology = a.Technology,
                                  SetAngularAsDefault = a.SetAngularAsDefault,
                                  DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                  DontShowLocal = a.Contact.DontShowLocalLabels,
                                  IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                  DocumentFilingInbox = a.DocumentFilingInbox,
                                  ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                  ShowInboxToolTip = a.ShowInboxToolTip,
                                  ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                  UserRoles = a.UserRoles,
                                  ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);
                        UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                        UserLastLoginQuery query = new UserLastLoginQuery(rep);
                        entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                        UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                        UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                        entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                        UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                        UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                        entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
                    }
                }
            }
            else
            {
                entity = (from a in repository.context.Users.Include("Contact")
                          where a.Tenant == tenant
                          && a.Contact.Email == email
                          select new UserPM()
                          {
                              BranchId = a.BranchId,
                              DepartmentId = a.DepartmentId,
                              Id = a.Id,
                              Notes = a.Notes,
                              Tenant = a.Tenant,
                              Email = a.Contact.Email,
                              EnglishName = a.Contact.EnglishName,
                              Anniversary = a.Contact.Anniversary,
                              Birthday = a.Contact.Birthday,
                              BusinessPhone = a.Contact.BusinessPhone,
                              FacebookId = a.Contact.FacebookId,
                              Fax = a.Contact.Fax,
                              InActive = a.Contact.InActive,
                              LocalName = a.Contact.LocalName,
                              SearchFields = a.SearchFields,
                              Mobile = a.Contact.Mobile,
                              Position = a.Contact.Position,
                              ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                              IsBranchRestricted = a.IsBranchRestricted,
                              IsSalesman = a.IsSalesman,
                              FreelancerId = a.FreelancerId,
                              IsFreelancer = a.IsFreelancer,
                              FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                              BusinessUnitId = a.BusinessUnitId,
                              Code = a.Code,
                              CreateDate = a.CreateDate,
                              ExpirationDate = a.ExpirationDate,
                              LicencedUser = a.LicencedUser,
                              IsProductRestricted = a.IsProductRestricted,
                              ProductTypeCode = a.ProductTypeCode,
                              IsDistributor = a.IsDistributor,
                              DistributorCode = a.DistributorCode,
                              IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                              IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                              PersonalId = a.PersonalId,
                              Technology = a.Technology,
                              SetAngularAsDefault = a.SetAngularAsDefault,
                              DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                              DontShowLocal = a.Contact.DontShowLocalLabels,
                              IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                              DocumentFilingInbox = a.DocumentFilingInbox,
                              ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                              ShowInboxToolTip = a.ShowInboxToolTip,
                              ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                              UserRoles = a.UserRoles,
                              ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                          }).FirstOrDefault();

                if (entity != null)
                {
                    entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                    UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                    UserLastLoginQuery query = new UserLastLoginQuery(rep);
                    entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                    UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                    UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                    entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                    UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                    UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                    entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();

                }
            }

            return entity;
        }

        public IQueryable<UserPM> GetUserPMsByTenant(int tenant)
        {
            IQueryable<UserPM> users = from a in repository.context.Users.Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("ProductType")
                                       where a.Tenant == tenant
                                       select new UserPM()
                                       {
                                           BranchId = a.BranchId,
                                           BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                           DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                           DepartmentId = a.DepartmentId,
                                           Id = a.Id,
                                           Notes = a.Notes,
                                           Tenant = a.Tenant,
                                           Email = a.Contact.Email,
                                           EnglishName = a.Contact.EnglishName,
                                           Anniversary = a.Contact.Anniversary,
                                           Birthday = a.Contact.Birthday,
                                           BusinessPhone = a.Contact.BusinessPhone,
                                           FacebookId = a.Contact.FacebookId,
                                           Fax = a.Contact.Fax,
                                           InActive = a.Contact.InActive,
                                           LocalName = a.Contact.LocalName,
                                           SearchFields = a.SearchFields,
                                           Mobile = a.Contact.Mobile,
                                           Position = a.Contact.Position,
                                           IsBranchRestricted = a.IsBranchRestricted,
                                           IsSalesman = a.IsSalesman,
                                           IsFreelancer = a.IsFreelancer,
                                           FreelancerId = a.FreelancerId,
                                           FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                           BusinessUnitId = a.BusinessUnitId,
                                           Code = a.Code,
                                           CreateDate = a.CreateDate,
                                           ExpirationDate = a.ExpirationDate,
                                           LicencedUser = a.LicencedUser,
                                           IsProductRestricted = a.IsProductRestricted,
                                           ProductTypeCode = a.ProductTypeCode,
                                           ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                           IsDistributor = a.IsDistributor,
                                           DistributorCode = a.DistributorCode,
                                           IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                           IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                           PersonalId = a.PersonalId,
                                           Technology = a.Technology,
                                           SetAngularAsDefault = a.SetAngularAsDefault,
                                           DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                           DontShowLocal = a.Contact.DontShowLocalLabels,
                                           IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                           DocumentFilingInbox = a.DocumentFilingInbox,
                                           ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                           ShowInboxToolTip = a.ShowInboxToolTip,
                                           ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                           UserRoles = a.UserRoles,
                                           ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                                       };
            return users;
        }

        public IQueryable<UserPM> GetUsersByEmailOrName(string email, string name, int tenant)
        {
            string emailNew = "";
            emailNew = email;

            string nameNew = "";
            nameNew = name;

            var query = from a in repository.context.Users.Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("ProductType")
                        where a.Tenant == tenant
                        select new UserPM()
                        {
                            BranchId = a.BranchId,
                            BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                            DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                            DepartmentId = a.DepartmentId,
                            Id = a.Id,
                            Notes = a.Notes,
                            Tenant = a.Tenant,
                            Email = a.Contact.Email,
                            EnglishName = a.Contact.EnglishName,
                            Anniversary = a.Contact.Anniversary,
                            Birthday = a.Contact.Birthday,
                            BusinessPhone = a.Contact.BusinessPhone,
                            FacebookId = a.Contact.FacebookId,
                            Fax = a.Contact.Fax,
                            InActive = a.Contact.InActive,
                            LocalName = a.Contact.LocalName,
                            SearchFields = a.SearchFields,
                            Mobile = a.Contact.Mobile,
                            Position = a.Contact.Position,
                            IsBranchRestricted = a.IsBranchRestricted,
                            IsSalesman = a.IsSalesman,
                            IsFreelancer = a.IsFreelancer,
                            FreelancerId = a.FreelancerId,
                            FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                            BusinessUnitId = a.BusinessUnitId,
                            Code = a.Code,
                            CreateDate = a.CreateDate,
                            ExpirationDate = a.ExpirationDate,
                            LicencedUser = a.LicencedUser,
                            IsProductRestricted = a.IsProductRestricted,
                            ProductTypeCode = a.ProductTypeCode,
                            ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                            IsDistributor = a.IsDistributor,
                            DistributorCode = a.DistributorCode,
                            IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                            IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                            PersonalId = a.PersonalId,
                            Technology = a.Technology,
                            SetAngularAsDefault = a.SetAngularAsDefault,
                            DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                            DontShowLocal = a.Contact.DontShowLocalLabels,
                            IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                            DocumentFilingInbox = a.DocumentFilingInbox,
                            ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                            ShowInboxToolTip = a.ShowInboxToolTip,
                            ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                            UserRoles = a.UserRoles,
                            ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                        };

            IQueryable<UserPM> query2 = null;
            if (!string.IsNullOrEmpty(email))
            {
                query2 = query.Where(d => d.Email.ToUpper().StartsWith(email.ToUpper()) && d.Tenant == tenant);
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()) && d.Tenant == tenant);
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()) && d.Tenant == tenant);
                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<UserList> GetIQueryableEntityList(IQueryable<User> iQueryable)
        {
            IQueryable<UserList> result = from user in iQueryable.Include("UserLastLogin").Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("BusinessUnit").Include("ProductType")
                                          select new UserList()
                                          {
                                              Email = user.Contact.Email,
                                              EnglishName = user.Contact.EnglishName ?? "",
                                              Id = user.Id,
                                              InActive = user.Contact.InActive,
                                              Notes = user.Notes,
                                              Tenant = user.Tenant,
                                              BranchId = user.BranchId,
                                              DepartmentId = user.DepartmentId,
                                              BranchName = user.Branch != null ? user.Branch.EnglishName : "",
                                              DepartmentName = user.Department != null ? user.Department.EnglishName : "",
                                              LocalName = user.Contact.LocalName ?? "",
                                              SearchFields = user.Contact.SearchFields,
                                              IsBranchRestricted = user.IsBranchRestricted,
                                              IsSalesman = user.IsSalesman,
                                              IsFreelancer = user.IsFreelancer,
                                              FreelancerId = user.FreelancerId,
                                              FreelancerName = user.Freelancer != null ? user.Freelancer.EnglishName : null,
                                              BusinessUnitId = user.BusinessUnitId,
                                              BusinessUnitName = user.BusinessUnit == null ? "" : user.BusinessUnit.Name,
                                              CreateDate = user.CreateDate,
                                              ExpirationDate = user.ExpirationDate,
                                              LicencedUser = user.LicencedUser,
                                              LastLoginDate = user.UserLastLogin == null ? null : user.UserLastLogin.LoginDateTime,
                                              IsProductRestricted = user.IsProductRestricted,
                                              ProductTypeCode = user.ProductTypeCode,
                                              ProductTypeName = user.ProductType != null ? user.ProductType.Name : null,
                                              IsDistributor = user.IsDistributor,
                                              DistributorCode = user.DistributorCode,
                                              IsShowContactDetailsInTheMobileApp = user.IsShowContactDetailsInTheMobileApp,
                                              Technology = user.Technology,
                                              PersonalId = user.PersonalId,
                                              SetAngularAsDefault = user.SetAngularAsDefault,
                                              IsTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled,
                                              DocumentFilingInbox = user.DocumentFilingInbox,
                                              ShowLocalNameInLOV = user.ShowLocalNameInLOV,
                                              UserRoles = user.UserRoles,
                                          };

            //int tenant = 0;
            //if (iQueryable != null && iQueryable.Count() > 0)
            //{
            //    tenant = iQueryable.First().Tenant;
            //}

            //List<EmployeeGroupLine> employees = new List<EmployeeGroupLine>();
            //EmployeeGroupLineRepository employeeRep = new EmployeeGroupLineRepository(tenant);
            //employees = employeeRep.GetAll(tenant).ToList();

            //List<UserList> entityList = new List<UserList>();
            //foreach (UserList item in result)
            //{
            //    List<string> temp = employees.Where(a => a.UserId == item.Id && a.Tenant == item.Tenant).Select(a => a.EmployeeGroupId).ToList();
            //    if (temp.Count > 0)
            //    {
            //        item.GroupId = temp;
            //    }

            //    entityList.Add(item);
            //}

            return result;
            //return entityList.AsQueryable();
        }

        public IQueryable<UserList> GetDemoTenantUserList(IQueryable<User> iQueryable, string loggedUserId, int tenant)
        {
            List<UserList> result = new List<UserList>();

            int index = 1;
            foreach (User user in iQueryable.Include("UserLastLogin").Include("Contact").Include("Department").Include("Branch").Include("Freelancer").Include("BusinessUnit").Include("ProductType"))
            {
                string email = user.Contact.Email;
                string name = user.Contact.EnglishName;

                if (user.Id != loggedUserId)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        string[] emailParts = user.Contact.Email.Split('@');
                        email = "user" + index + "@democompany.com ";
                    }

                    name = "User" + index;
                }

                UserList newItem = new UserList()
                {
                    Email = email,
                    EnglishName = name,
                    Id = user.Id,
                    InActive = user.Contact.InActive,
                    Notes = user.Notes,
                    Tenant = user.Tenant,
                    BranchId = user.BranchId,
                    DepartmentId = user.DepartmentId,
                    BranchName = user.Branch != null ? user.Branch.EnglishName : "",
                    DepartmentName = user.Department != null ? user.Department.EnglishName : "",
                    LocalName = user.Contact.LocalName ?? "",
                    SearchFields = user.Contact.SearchFields,
                    IsBranchRestricted = user.IsBranchRestricted,
                    IsSalesman = user.IsSalesman,
                    IsFreelancer = user.IsFreelancer,
                    FreelancerId = user.FreelancerId,
                    FreelancerName = user.Freelancer != null ? user.Freelancer.EnglishName : null,
                    BusinessUnitId = user.BusinessUnitId,
                    BusinessUnitName = user.BusinessUnit == null ? "" : user.BusinessUnit.Name,
                    CreateDate = user.CreateDate,
                    ExpirationDate = user.ExpirationDate,
                    LicencedUser = user.LicencedUser,
                    LastLoginDate = user.UserLastLogin == null ? null : user.UserLastLogin.LoginDateTime,
                    IsProductRestricted = user.IsProductRestricted,
                    ProductTypeCode = user.ProductTypeCode,
                    ProductTypeName = user.ProductType != null ? user.ProductType.Name : null,
                    IsDistributor = user.IsDistributor,
                    DistributorCode = user.DistributorCode,
                    IsShowContactDetailsInTheMobileApp = user.IsShowContactDetailsInTheMobileApp,
                    Technology = user.Technology,
                    PersonalId = user.PersonalId,
                    SetAngularAsDefault = user.SetAngularAsDefault,
                    IsTwoFactorAuthenticationEnabled = user.IsTwoFactorAuthenticationEnabled,
                    DocumentFilingInbox = user.DocumentFilingInbox,
                    ShowLocalNameInLOV = user.ShowLocalNameInLOV,
                };

                result.Add(newItem);
                index++;
            }

            return result.AsQueryable();
        }

        public int ComputeDaysLeft(DateTime? date)
        {
            DateTime? startDate = DateTime.Now.Date;
            int days = 0;

            if (startDate != null && date != null)
            {
                TimeSpan? day = (date - startDate);

                if (day.Value.Days == 0)
                {
                    days = 0;
                }
                else if (day.Value.Days < 0)
                {
                    days = day.Value.Days;
                }
                else
                {
                    days = day.Value.Days;
                }
            }
            return days;
        }

        public UserPM GetSingleUserByEmailOrIdAndTenantOrTenantZero(string id, string email, int tenant)
        {
            UserPM entity = (from a in repository.context.Users
                             where !a.Contact.InActive && a.Tenant == tenant
                             && (a.Contact.Email == email || a.Id == id)
                             select new UserPM()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 Email = a.Contact.Email,
                                 IsDistributor = a.IsDistributor,
                                 IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                 IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                 Technology = a.Technology,
                                 DocumentFilingInbox = a.DocumentFilingInbox,
                                 ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                 ShowInboxToolTip = a.ShowInboxToolTip,
                                 ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                             }).FirstOrDefault();

            if (entity == null)
            {
                entity = (from a in repository.context.Users
                          where a.Tenant == 0
                          && (a.Contact.Email == email || a.Id == id)
                          select new UserPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              Email = a.Contact.Email,
                              IsDistributor = a.IsDistributor,
                              IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                              IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                              Technology = a.Technology,
                              DocumentFilingInbox = a.DocumentFilingInbox,
                              ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                              ShowInboxToolTip = a.ShowInboxToolTip,
                              ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                          }).FirstOrDefault();
            }

            return entity;
        }

        public UserPM GetSinglePMByCode(string code, int tenant)
        {
            string entityName = "UserPM" + code + tenant;
            
            UserPM entity = (from a in repository.context.Users.Include("Contact")
                             where a.Tenant == tenant
                             && a.Code == code
                             select new UserPM()
                             {
                                 BranchId = a.BranchId,
                                 BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                 DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                 DepartmentId = a.DepartmentId,
                                 Id = a.Id,
                                 Notes = a.Notes,
                                 Tenant = a.Tenant,
                                 Email = a.Contact.Email,
                                 EnglishName = a.Contact.EnglishName,
                                 Anniversary = a.Contact.Anniversary,
                                 Birthday = a.Contact.Birthday,
                                 BusinessPhone = a.Contact.BusinessPhone,
                                 FacebookId = a.Contact.FacebookId,
                                 Fax = a.Contact.Fax,
                                 InActive = a.Contact.InActive,
                                 LocalName = a.Contact.LocalName,
                                 SearchFields = a.SearchFields,
                                 Mobile = a.Contact.Mobile,
                                 Position = a.Contact.Position,
                                 ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                                 IsBranchRestricted = a.IsBranchRestricted,
                                 IsSalesman = a.IsSalesman,
                                 IsFreelancer = a.IsFreelancer,
                                 FreelancerId = a.FreelancerId,
                                 FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                 BusinessUnitId = a.BusinessUnitId,
                                 Code = a.Code,
                                 CreateDate = a.CreateDate,
                                 ExpirationDate = a.ExpirationDate,
                                 LicencedUser = a.LicencedUser,
                                 IsProductRestricted = a.IsProductRestricted,
                                 ProductTypeCode = a.ProductTypeCode,
                                 ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                 IsDistributor = a.IsDistributor,
                                 DistributorCode = a.DistributorCode,
                                 IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                                 PersonalId = a.PersonalId,
                                 IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                 Technology = a.Technology,
                                 SetAngularAsDefault = a.SetAngularAsDefault,
                                 DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                                 IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                 DocumentFilingInbox = a.DocumentFilingInbox,
                                 ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                 ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                 UserRoles = a.UserRoles,
                                 ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                             }).FirstOrDefault();

            if (entity != null)
            {
                UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                UserLastLoginQuery query = new UserLastLoginQuery(rep);
                entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
            }

            return entity;
        }

        public List<UserList> GetUserListByUserIds(List<string> userIds, int tenant)
        {
            List<UserList> users = (from a in repository.context.Users.Include("Contact")
                                    where userIds.Contains(a.Id) && a.Tenant == tenant
                                    select new UserList()
                                    {
                                        BranchId = a.BranchId,
                                        BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                        DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                        DepartmentId = a.DepartmentId,
                                        Id = a.Id,
                                        Notes = a.Notes,
                                        Tenant = a.Tenant,
                                        Email = a.Contact.Email,
                                        EnglishName = a.Contact.EnglishName,
                                        InActive = a.Contact.InActive,
                                        LocalName = a.Contact.LocalName,
                                        SearchFields = a.SearchFields,
                                        IsBranchRestricted = a.IsBranchRestricted,
                                        IsSalesman = a.IsSalesman,
                                        IsFreelancer = a.IsFreelancer,
                                        FreelancerId = a.FreelancerId,
                                        FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                        BusinessUnitId = a.BusinessUnitId,
                                        CreateDate = a.CreateDate,
                                        ExpirationDate = a.ExpirationDate,
                                        LicencedUser = a.LicencedUser,
                                        IsProductRestricted = a.IsProductRestricted,
                                        ProductTypeCode = a.ProductTypeCode,
                                        ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                        IsDistributor = a.IsDistributor,
                                        DistributorCode = a.DistributorCode,
                                        IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                        PersonalId = a.PersonalId,
                                        Technology = a.Technology,
                                        SetAngularAsDefault = a.SetAngularAsDefault,
                                        Mobile = a.Contact.Mobile,
                                        Fax = a.Contact.Fax,
                                        BusinessPhone = a.Contact.BusinessPhone,
                                        IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                        DocumentFilingInbox = a.DocumentFilingInbox,
                                        ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                    }).ToList();
            return users;
        }
        
        public List<UserPM> GetUserPMsByUserIds(List<string> userIds, int tenant)
        {
            List<UserPM> users = (from a in repository.context.Users.Include("Contact")
                                  where userIds.Contains(a.Id) && a.Tenant == tenant
                                  select new UserPM()
                                  {
                                      BranchId = a.BranchId,
                                      BranchName = a.Branch != null ? a.Branch.EnglishName : "",
                                      DepartmentName = a.Department != null ? a.Department.EnglishName : "",
                                      DepartmentId = a.DepartmentId,
                                      Id = a.Id,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      Email = a.Contact.Email,
                                      EnglishName = a.Contact.EnglishName,
                                      InActive = a.Contact.InActive,
                                      LocalName = a.Contact.LocalName,
                                      SearchFields = a.SearchFields,
                                      Code= a.Code,
                                      IsBranchRestricted = a.IsBranchRestricted,
                                      IsSalesman = a.IsSalesman,
                                      IsFreelancer = a.IsFreelancer,
                                      FreelancerId = a.FreelancerId,
                                      FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                                      BusinessUnitId = a.BusinessUnitId,
                                      CreateDate = a.CreateDate,
                                      ExpirationDate = a.ExpirationDate,
                                      LicencedUser = a.LicencedUser,
                                      IsProductRestricted = a.IsProductRestricted,
                                      ProductTypeCode = a.ProductTypeCode,
                                      ProductTypeName = a.ProductType != null ? a.ProductType.Name : null,
                                      IsDistributor = a.IsDistributor,
                                      DistributorCode = a.DistributorCode,
                                      IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                                      PersonalId = a.PersonalId,
                                      Technology = a.Technology,
                                      SetAngularAsDefault = a.SetAngularAsDefault,
                                      IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                                      DocumentFilingInbox = a.DocumentFilingInbox,
                                      ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                                      ShowInboxToolTip = a.ShowInboxToolTip,
                                      ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                                      ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                                  }).ToList();
            return users;
        }

        public List<UserList> GetUserListsByidsString(string ids, int tenant)
        {
            List<UserList> users = new List<UserList>();
            List<string> emailsList = ids.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (emailsList.Count() > 0)
            {
                users = (from a in repository.context.Users.Include("Contact")
                         where emailsList.Contains(a.Contact.Id) && a.Tenant == tenant
                         select new UserList()
                         {
                             Id = a.Id,
                             Email = a.Contact.Email,
                             SearchFields = a.SearchFields,
                             EnglishName = a.Contact.EnglishName,
                             LocalName = a.Contact.LocalName,
                             DocumentFilingInbox = a.DocumentFilingInbox,
                         }).ToList();
            }

            return users;
        }

        public string GetTechnologyOfUser(string userId)
        {
            string technology = "";
            User user = repository.context.Users.Where(D => D.Id == userId).FirstOrDefault();

            if (user != null)
            {
                technology = user.Technology;
            }

            return technology;
        }

        public List<UserList> GetUserListsByEmailsString(string emails, int tenant)
        {
            List<UserList> users = new List<UserList>();
            List<string> emailsList = emails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            if (emailsList.Count() > 0)
            {
                users = (from a in repository.context.Users.Include("Contact")
                         where emailsList.Contains(a.Contact.Email) && a.Tenant == tenant
                         select new UserList()
                         {
                             Id = a.Id,
                             Email = a.Contact.Email,
                             SearchFields = a.SearchFields,
                             EnglishName = a.Contact.EnglishName,
                             LocalName = a.Contact.LocalName,
                             DocumentFilingInbox = a.DocumentFilingInbox,
                         }).ToList();
            }

            return users;
        }

        public UserPM GetSinglePMByEmail(string email, int tenant)
        {
            string entityName = "UserPM" + email + tenant;
            UserPM entity;

            entity = (from a in repository.context.Users.Include("Contact")
                      where a.Tenant == tenant
                      && a.Contact.Email == email
                      select new UserPM()
                      {
                          BranchId = a.BranchId,
                          DepartmentId = a.DepartmentId,
                          Id = a.Id,
                          Notes = a.Notes,
                          Tenant = a.Tenant,
                          Email = a.Contact.Email,
                          EnglishName = a.Contact.EnglishName,
                          Anniversary = a.Contact.Anniversary,
                          Birthday = a.Contact.Birthday,
                          BusinessPhone = a.Contact.BusinessPhone,
                          FacebookId = a.Contact.FacebookId,
                          Fax = a.Contact.Fax,
                          InActive = a.Contact.InActive,
                          LocalName = a.Contact.LocalName,
                          SearchFields = a.SearchFields,
                          Mobile = a.Contact.Mobile,
                          Position = a.Contact.Position,
                          ComputedLocalName = string.IsNullOrEmpty(a.Contact.LocalName) ? a.Contact.EnglishName : a.Contact.LocalName,
                          IsBranchRestricted = a.IsBranchRestricted,
                          IsSalesman = a.IsSalesman,
                          FreelancerId = a.FreelancerId,
                          IsFreelancer = a.IsFreelancer,
                          FreelancerName = a.Freelancer != null ? a.Freelancer.EnglishName : null,
                          BusinessUnitId = a.BusinessUnitId,
                          Code = a.Code,
                          CreateDate = a.CreateDate,
                          ExpirationDate = a.ExpirationDate,
                          LicencedUser = a.LicencedUser,
                          IsProductRestricted = a.IsProductRestricted,
                          ProductTypeCode = a.ProductTypeCode,
                          IsDistributor = a.IsDistributor,
                          DistributorCode = a.DistributorCode,
                          IsCustomerCare = a.Tenant == 0 && !a.IsDistributor,
                          IsShowContactDetailsInTheMobileApp = a.IsShowContactDetailsInTheMobileApp,
                          PersonalId = a.PersonalId,
                          Technology = a.Technology,
                          SetAngularAsDefault = a.SetAngularAsDefault,
                          DisplayGettingStarted = a.Contact.DisplayGettingStarted,
                          DontShowLocal = a.Contact.DontShowLocalLabels,
                          IsTwoFactorAuthenticationEnabled = a.IsTwoFactorAuthenticationEnabled,
                          DocumentFilingInbox = a.DocumentFilingInbox,
                          ShowLogBoxToolTip = a.ShowLogBoxToolTip,
                          ShowInboxToolTip = a.ShowInboxToolTip,
                          ShowLocalNameInLOV = a.ShowLocalNameInLOV,
                          ShowNewReleaseToolTip = a.ShowNewReleaseToolTip,
                      }).FirstOrDefault();

            if (entity != null)
            {
                entity.ExpirationDaysLeft = ComputeDaysLeft(entity.ExpirationDate);

                UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
                UserLastLoginQuery query = new UserLastLoginQuery(rep);
                entity.UserLastLogin = query.GetSinglePM(entity.Id, tenant);

                UserPermittedBranchRepository userPermRep = new UserPermittedBranchRepository(this.repository.context);
                UserPermittedBranchQuery userPermittedBranchQuery = new UserPermittedBranchQuery(userPermRep);
                entity.UserPermittedBranches = userPermittedBranchQuery.GetContactFromUserPermittedBranchPMsByUserId(entity.Id, entity.Tenant).ToList();

                UserPermittedProductRepository perProductRep = new UserPermittedProductRepository(this.repository.context);
                UserPermittedProductQuery perProductQuery = new UserPermittedProductQuery(perProductRep);
                entity.UserPermittedProducts = perProductQuery.GetContactFromUserPermittedProductPMsByUserId(entity.Id, entity.Tenant).ToList();
            }

            return entity;
        }
        
        public List<UserList> GetUserListsByTenant(int tenant, bool includeInactiveUsers)
        {
            List<UserList> result = new List<UserList>();

            IQueryable<UserList> users = (from a in repository.context.Users.Include("Contact")
                                          where a.Tenant == tenant
                                          select new UserList()
                                          {
                                              Id = a.Id,
                                              Email = a.Contact.Email,
                                              EnglishName = a.Contact.EnglishName,
                                              InActive = a.Contact.InActive,
                                          });


            if (!includeInactiveUsers)
            {
                users = users.Where(d => !d.InActive);
            }

            result = users.ToList();

            List<string> userIds = result.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();
            UserLastLoginRepository rep = new UserLastLoginRepository(this.repository.context);
            UserLastLoginQuery query = new UserLastLoginQuery(rep);
            //List<UserLastLoginPM> userLastLoginPMLists = query.GetUserLastLoginPMsByUserIds(userIds, tenant).ToList();

            //foreach (UserList user in result)
            //{
            //    UserLastLoginPM userLastLoginPM = userLastLoginPMLists.Where(d => d.Id == user.Id).FirstOrDefault();
            //    if (userLastLoginPM != null)
            //    {
            //        user.LastLoginDate = userLastLoginPM.LoginDateTime;
            //    }

            //}

            return result;
        }

        public bool CheckIfUserExistInTenant(string userId, int tenant)
        {
            bool isExist = repository.context.Users.Where(d => d.Id == userId && (d.Tenant == tenant || d.Tenant == 0)).Any();
            return isExist;
        }

        public UserPM GetSinglePMLite(string id,int tenant)
        {
            UserPM entity = (from a in repository.context.Users.Include("Contact")
                      where a.Tenant == tenant
                      && a.Id == id
                      select new UserPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          Email = a.Contact.Email,
                          EnglishName = a.Contact.EnglishName,
                      }).FirstOrDefault();
            return entity;
        }
        
        public UserPM GetSingleUserPMByEmailLite(string email, int tenant)
        {
           UserPM entity = (from a in repository.context.Users.Include("Contact")
                      where a.Tenant == tenant
                      && a.Contact.Email == email
                      select new UserPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          Email = a.Contact.Email,
                          EnglishName = a.Contact.EnglishName,
                      }).FirstOrDefault();

            return entity;
        }

        public List<string> GetUserIdsByTenant(int tenant)
        {
            var usersIds = (from a in repository.context.Users
                         where a.Tenant == tenant
                         select a.Id).ToList();

            return usersIds;
        }
    }
}