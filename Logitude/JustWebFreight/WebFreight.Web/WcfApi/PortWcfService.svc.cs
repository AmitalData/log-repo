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
using Logitude.BL.Validators;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PortHypredService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PortHypredService.svc or PortHypredService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PortWcfService : IPortWcfService
    {
        public Response Upsert(PortPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Port", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("Port", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);

                    PortRepository PortRepository = new PortRepository(objectContext);
                    CountryRepository countryRepository = new CountryRepository(objectContext);
                    StateRepository stateRepository = new StateRepository(objectContext);

                    PortService service = new PortService(objectContext, entityPM.Tenant);

                    entityPM.IsHybrid = true;

                    if (entityPM.CountryId != null)
                    {
                        Country country = countryRepository.GetSingleCountryByCode(entityPM.CountryId, entityPM.Tenant, false);
                        if (country != null)
                        {
                            entityPM.CountryId = country.Id;
                            entityPM.CountryCode = country.Code;

                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CountryId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }
                    if (entityPM.StateId != null)
                    {
                        State state = stateRepository.GetSingleStateByCode(entityPM.StateId, entityPM.Tenant);
                        if (state != null)
                        {
                            entityPM.StateId = state.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "StateId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    Port entity = PortRepository.GetSinglePortByCodeCountryId(entityPM.Tenant, entityPM.Code, entityPM.CountryId, false);
                    if (entity == null)
                    {
                        service.Create(entityPM);
                    }
                    else
                    {
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


        public List<Logitude.BL.CommonDataModel.EntityLists.PortList> GetList(ApiSearchFilters filters, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Port", "READ", tenant);

                List<Logitude.BL.CommonDataModel.EntityLists.PortList> result = new List<Logitude.BL.CommonDataModel.EntityLists.PortList>();
                PortRepository portRepository = new PortRepository(tenant);

                IQueryable<Port> ports = portRepository.GetPorts(tenant);



                if (!string.IsNullOrEmpty(filters.SearchFields))
                {
                    ports = ports.Where(s => s.SearchFields.Contains(filters.SearchFields));
                }

                ports = ports.OrderByDescending(d => d.Code).Skip(filters.Skip).Take(filters.Take);
                PortQuery portQuery = new PortQuery(portRepository);
                result = portQuery.GetIQueryableEntityList(ports).ToList();

                return result;
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


        public string GetPortId(DataContracts.PortApiFilters filters, int tenant, ref Response response)
        {
            try
            {
               
                SecurityUtility.AuthenticationOnTenant(tenant);
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                PortRepository portRepository = new PortRepository(objectContext);
                Port port = portRepository.GetSinglePortByCodeCountryCode(0, filters.PortCode, filters.CountryCode, false);

                string portId = (port != null ? port.Id : null);
                response.Result = portId;

                return portId;
            }
            catch (Exception ex)
            {
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
