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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AddressWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AddressWcfService.svc or AddressWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AddressWcfService : IAddressWcfService
    {
        public Response Upsert(AddressPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
                {
                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);

                    AddressRepository AddressRepository = new AddressRepository(objectContext);
                    CardRepository cardRepository = new CardRepository(objectContext);
                    CountryRepository countryRespository = new CountryRepository(objectContext);
                    StateRepository stateRepository = new StateRepository(objectContext);
                    AddressService service = new AddressService(objectContext, entityPM.Tenant);

                    entityPM.IsHybrid = true;
                    if (entityPM.CardId != null)
                    {
                        Card card = cardRepository.GetSingleCardByCode(entityPM.CardId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.CardId = card.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CardId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }

                    }

                    if (entityPM.CountryId != null)
                    {
                        Country country = countryRespository.GetSingleCountryByCode(entityPM.CountryId, entityPM.Tenant);
                        if (country != null)
                        {
                            entityPM.CountryId = country.Id;
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


                    if (entityPM.SearchFields != null && entityPM.SearchFields.Length > 1000)
                    {
                        entityPM.SearchFields = entityPM.SearchFields.Substring(0, 1000);
                    }

                    ClassLevelValidator validationClass = new ClassLevelValidator("Address", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    Address entity = null;
                    if (entityPM.AddressTypeId != "O" && !string.IsNullOrEmpty(entityPM.CardId) && string.IsNullOrEmpty(entityPM.ExternalId))
                    {
                        entity = AddressRepository.GetSingleAddressByCardIdAndTypeId(entityPM.CardId,entityPM.AddressTypeId, entityPM.Tenant);
                    }
                    else
                    {
                        entity = AddressRepository.GetSingleAddressByExternalId(entityPM.ExternalId, entityPM.Tenant);
                    }

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
                response.IsAuthenticationError = true;
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


        public AddressPM GetAddressByExternalId(string externalId, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                AddressQuery addressQuery = new AddressQuery(tenant);
                return addressQuery.GetSinglePMByExternalId(externalId, tenant);
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
