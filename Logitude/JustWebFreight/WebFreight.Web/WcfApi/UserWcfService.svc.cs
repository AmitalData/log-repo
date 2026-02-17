using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using WebFreight.Web.Security;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Validators;
using Simplog.Server.Infrastructure;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserHypredService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserHypredService.svc or UserHypredService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserWcfService : IUserWcfService
    {

        public Response Upsert(UserPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("User", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    
                    ClassLevelValidator validationClass = new ClassLevelValidator("User", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                    UserRepository userRepository = new UserRepository(objectContext);
                    BranchRepository branchRepository = new BranchRepository(objectContext);
                    DepartmentRepository departmentRepository = new DepartmentRepository(objectContext);
                    RoleRepository roleRepository = new RoleRepository(objectContext);
                    CardRepository cardRepository = new CardRepository(objectContext);
                    RoleQuery rolesQuery = new RoleQuery(roleRepository);
                     
                    UserService service = new UserService(objectContext, entityPM.Tenant);
                    if (LogitudeSettings.IsCostomsDeploy)
                    {
                        service.SuppressMustChangePasswordDueSSO = true;
                    }
                    

                    entityPM.IsHybrid = true;

                    if (entityPM.BranchId != null)
                    {
                        Branch branch = branchRepository.GetSingleBranchByCode(entityPM.BranchId, entityPM.Tenant);
                        if (branch != null)
                        {
                            entityPM.BranchId = branch.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "BranchId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }
                    if (entityPM.DepartmentId != null)
                    {
                        Department department = departmentRepository.GetSingleDepartmentByCode(entityPM.DepartmentId, entityPM.Tenant);
                        if (department != null)
                        {
                            entityPM.DepartmentId = department.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "DepartmentId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.FreelancerId != null)
                    {
                        Card card = cardRepository.GetSingleCardByCode(entityPM.FreelancerId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.FreelancerId = card.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "FreelancerId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (string.IsNullOrEmpty(entityPM.Code))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Code field is required";
                        return response;
                    }

                    foreach (UserRolesPM userRole in entityPM.Roles)
                    {
                        if (!string.IsNullOrEmpty(userRole.Id))
                        {
                            RolePM storedRole = rolesQuery.GetSinglePMByCode(userRole.Id, entityPM.Tenant);
                            if (storedRole != null)
                            {
                                userRole.Id = storedRole.Id;
                                userRole.Name = storedRole.Name;
                                userRole.Tenant = entityPM.Tenant;
                                userRole.Added = true;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "UserRole Id  doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                    }

                    User entity = userRepository.GetSingleUserByCodeOrEmailForTenant(entityPM.Code, entityPM.Email, entityPM.Tenant, false);
                    if (entity == null)
                    {
                        RolePM defaultRole = null;
                        if (!string.IsNullOrEmpty(entityPM.RoleCode))
                        {
                            defaultRole = rolesQuery.GetSinglePMByCode(entityPM.RoleCode, entityPM.Tenant);
                        }
                        if (defaultRole == null)
                        {
                            defaultRole = rolesQuery.GetSinglePMByCode("ADMN", 0);
                        }

                        if (entityPM.Roles != null && entityPM.Roles.Count == 0 && !entityPM.Roles.Any(r => r.Id == defaultRole.Id))
                        {
                            entityPM.Roles.Add(new UserRolesPM()
                            {
                                Id = defaultRole.Id,
                                Name = defaultRole.Name,
                                Tenant = entityPM.Tenant,
                                Added = true,
                            });
                        }

                        if (string.IsNullOrEmpty(entityPM.Password))
                        {
                            entityPM.Password = PasswordGenerator.Generate(8);
                           
                        }
                        
                        service.Create(entityPM);

                    }
                    else
                    {

                        if (entityPM.Roles != null && entityPM.Roles.Count != 0) // DON'T DELETE EXISTING ROLES IF NOT SENT BY HYBRID
                        {
                            List<RolePM> oldRoles = rolesQuery.GetRolesForContact(entity.Id, entity.Tenant).Where(r => r.Exists).ToList();
                            foreach (RolePM role in oldRoles)
                            {
                                UserRolesPM userRolePM = new UserRolesPM()
                                {
                                    Id = role.Id,
                                    Name = role.Name,
                                    Removed = true,
                                    UserId = entity.Id,
                                    Tenant = entity.Tenant,
                                };

                                entityPM.Roles.Add(userRolePM);
                            }
                        }

                        entityPM.Id = entity.Id;
                        service.Update(entityPM);

                    }

                    response.Result = entityPM.Id;
                    scope.Complete();
                    return response;
                }
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

       


        public UserPM GetUser(UserApiFilters filter, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("User", "READ", tenant);//UPDATE//READ

                UserPM user = null;
                UserQuery userQuery = new UserQuery(tenant);
                if (!string.IsNullOrEmpty(filter.SearchCode))
                {
                    if (filter.ById)
                    {
                        user = userQuery.GetSingleUserPM(filter.SearchCode, tenant, false);
                    }
                    if (filter.ByCode)
                    {
                        user = userQuery.GetSingleUserPMByCode(filter.SearchCode, tenant, false);
                    }

                    if (filter.ByEmail)
                    {
                        user = userQuery.GetSingleUserPMByEmail(filter.SearchCode, tenant, false);
                    }
                }

                if (user != null)
                {
                    RoleQuery roleQuery = new RoleQuery(tenant);
                    List<RolePM> userRoles = roleQuery.GetRolesByUser(user.Id, tenant).Where(r => r.Exists).ToList();
                    user.Roles = new List<UserRolesPM>();
                    foreach (RolePM role in userRoles)
                    {
                        UserRolesPM userRolePM = new UserRolesPM()
                        {
                            Id = role.Code,
                            Name = role.Name,
                            Added = true,
                            UserId = user.Code,
                            Tenant = user.Tenant,
                        };

                        user.Roles.Add(userRolePM);
                    }

                }

                return user;


            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }
    }
}
