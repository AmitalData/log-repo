using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Logitude.Customs.Data.Repsitories;
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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CurrencyWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CurrencyWcfService.svc or CurrencyWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CurrencyWcfService : ICurrencyWcfService
    {
        public List<CurrencyList> GetList(ApiSearchFilters filters, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

                List<Logitude.BL.CommonDataModel.EntityLists.CurrencyList> result = new List<Logitude.BL.CommonDataModel.EntityLists.CurrencyList>();
                CurrencyRepository currencyRepository = new CurrencyRepository(tenant);

                IQueryable<Currency> currencies = currencyRepository.GetCurrencies(tenant);



                if (!string.IsNullOrEmpty(filters.SearchFields))
                {
                    currencies = currencies.Where(s => s.SearchFields.Contains(filters.SearchFields));
                }
                
                currencies = currencies.OrderByDescending(d => d.Code).Skip(filters.Skip).Take(filters.Take);
                CurrencyQuery CurrencyQuery = new CurrencyQuery(currencyRepository);
                result = CurrencyQuery.GetIQueryableEntityList(currencies).ToList();

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

        public Response Upsert(CurrencyPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Currency", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    
                    ClassLevelValidator validationClass = new ClassLevelValidator("Currency", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }
                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);

                    CurrencyRepository currencyRepository = new CurrencyRepository(objectContext);
                    CurrencyService service = new CurrencyService(objectContext, entityPM.Tenant);

                    entityPM.IsHybrid = true;
                    Currency entity = currencyRepository.GetSingleCurrencyByCode(entityPM.Code, entityPM.Tenant);
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
    }
}
