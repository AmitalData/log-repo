using Logitude.BL.Validators;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "VendorCommissionWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select VendorCommissionWcfService.svc or VendorCommissionWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class VendorCommissionWcfService : IVendorCommissionWcfService
    {
        public Response Upsert(VendorCommissionPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("VendorCommission", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("VendorCommission", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICustomContext objectContext = CustomContext.GetContext(entityPM.Tenant);

                    CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(objectContext);
                    VendorCommissionUpdateService service = new VendorCommissionUpdateService(objectContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    VendorCommissionQueryService vendorCommissionQueryService = new VendorCommissionQueryService(objectContext);


                    CustomsVendorPM vendor = vendorQueryService.GetVendorByNumber(entityPM.VendorId, entityPM.Tenant);
                    ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);

                    CustomerRepository customerRepository = new CustomerRepository(commonContext);
                    Customer customer = customerRepository.GetSingleCustomerByCode(entityPM.CustomerId, entityPM.Tenant, false);

                    if (vendor != null && customer != null)
                    {
                    VendorCommissionPM vendorCommission = vendorCommissionQueryService.GetSingle(vendor.Id, customer.Id, false, false);



                    if (vendorCommission == null)
                    {

                            vendorCommission = new VendorCommissionPM()
                        {
                            VendorId = vendor.Id,
                            CustomerId = customer.Id,
                            Tenant = entityPM.Tenant,
                            CommisionPercentage = entityPM.CommisionPercentage,
                            ChangeSetOp = ChangeSetOperation.Insert
                        };


                        service.Update(vendorCommission, true);
                    }
                    else
                    {
                        vendorCommission.CommisionPercentage = entityPM.CommisionPercentage;
                        vendorCommission.ChangeSetOp = ChangeSetOperation.Update;
                        service.Update(vendorCommission, true);
                    }


                    response.Result = vendorCommission.VendorId;

                    }
                    else
                    {
                        response.HasError = true;
                        if (customer == null && vendor == null)
                        {
                            response.ErrorMessage = "Customer and vendor  are not found";
                        }

                        else if (vendor == null)
                        {
                            response.ErrorMessage = "vendor is not found";
                        }
                        else if (customer == null)
                        {
                            response.ErrorMessage = "Customer is not found";
                        }



                    }

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

        public Response Delete(VendorCommissionPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("VendorCommission", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("VendorCommission", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICustomContext objectContext = CustomContext.GetContext(entityPM.Tenant);

                    CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(objectContext);
                    VendorCommissionUpdateService service = new VendorCommissionUpdateService(objectContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    VendorCommissionQueryService vendorCommissionQueryService = new VendorCommissionQueryService(objectContext);


                    CustomsVendorPM vendor = vendorQueryService.GetVendorByNumber(entityPM.VendorId, entityPM.Tenant);
                    ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);

                    CustomerRepository customerRepository = new CustomerRepository(commonContext);
                    Customer customer = customerRepository.GetSingleCustomerByCode(entityPM.CustomerId, entityPM.Tenant, false);

                    if (vendor != null && customer != null)
                    {
                        VendorCommissionPM vendorCommission = vendorCommissionQueryService.GetSingle(vendor.Id, customer.Id, false, false);



                        if (vendorCommission != null)
                        {

                            vendorCommission = new VendorCommissionPM()
                            {
                                VendorId = vendor.Id,
                                CustomerId = customer.Id,
                                Tenant = entityPM.Tenant,
                                CommisionPercentage = entityPM.CommisionPercentage,
                                ChangeSetOp = ChangeSetOperation.Delete
                            };


                            service.Update(vendorCommission, true);
                    response.Result = vendorCommission.VendorId;
                        }
                        else
                        {
                            response.ErrorMessage = " vendor commission  is not found";
                        }


                       

                    }
                    else
                    {
                        response.HasError = true;
                        if (customer == null && vendor == null)
                        {
                            response.ErrorMessage = "Customer and vendor  are not found";
                        }

                        else if (vendor == null)
                        {
                            response.ErrorMessage = "vendor is not found";
                        }
                        else if (customer == null)
                        {
                            response.ErrorMessage = "Customer is not found";
                        }



                    }

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

     
     
    }
}
