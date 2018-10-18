using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using Logitude.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CityWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CityWcfService.svc or CityWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CityWcfService : ICityWcfService
    {

        public Response Upsert(CountryCityPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("CountryCity", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("CountryCity", entityPM.Tenant);
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                    CountryRepository countryRepository = new CountryRepository(objectContext);

                    CountryCityRepository countryCityRepository = new CountryCityRepository(objectContext);
                    CountryCityService service = new CountryCityService(objectContext, entityPM.Tenant);

                    entityPM.IsHybrid = true;

                    if (string.IsNullOrEmpty(entityPM.Code))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Code field is required";
                        return response;
                    }


                    if (entityPM.CountryId != null)
                    {
                        Country country = countryRepository.GetSingleCountryByCode(entityPM.CountryId, entityPM.Tenant);
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

                    CountryCity entity = countryCityRepository.GetSingleCountryCityByCodeAndCountry(entityPM.Code, entityPM.CountryId, entityPM.Tenant);
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

        public CountryCityList GetCityListByCode(string code, string countryCode, int tenant, ref Response response)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CountryCity", "READ", tenant);
            CountryCityList countryCityList = null;
            try
            {
                CountryCityRepository countryCityRepository = new CountryCityRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                Country country = countryRepository.GetSingleCountryByCode(countryCode,tenant);
                if (country != null)
                {

                    CountryCity CountryCity = countryCityRepository.GetSingleCountryCityByCodeAndCountry(code, country.Id, tenant);

                    if (CountryCity != null)
                    {
                        List<CountryCity> singleEntityList = new List<CountryCity>();
                        singleEntityList.Add(CountryCity);

                        CountryCityQuery countryCityQuery = new CountryCityQuery(countryCityRepository);
                        IQueryable<CountryCity> iQueryable = singleEntityList.AsQueryable();
                        IQueryable<CountryCityList> iQueryableEntityList = countryCityQuery.GetIQueryableEntityList(iQueryable);
                        countryCityList = iQueryableEntityList.FirstOrDefault();
                    }
                }

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
            return countryCityList;
        }
    }
}
