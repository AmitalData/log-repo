using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Services.CustomizedCounter;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class CountersDomainController : ApiController
    {
        public HttpResponseMessage GetTenantCounters()
        {
            try
            {
                // test 
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                CounterQuery entityQuery = new CounterQuery(tenant);
                List<CounterPM> myResult = entityQuery.GetCountersByTenant(tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCounterDefinitions(string CounterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                CounterDefinitionQuery entityQuery = new CounterDefinitionQuery(tenant);
                List<CounterDefinitionPM> myResult = entityQuery.GetCounterDefinitionsByCounterId(CounterId, tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCounterAPIHelper(string CounterId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                CounterAPIHelper myResult = new CounterAPIHelper()
                {
                    CounterId = CounterId,
                };

                IWebFreightContext myContext = WebFreightContext.GetContext(tenant);

                CounterRepository myCounterRepository = new CounterRepository(myContext);
                CounterDefinitionRepository myCounterDefinitionRepository = new CounterDefinitionRepository(myContext);
                CounterQuery myCounterQuery = new CounterQuery(myCounterRepository);
                CounterDefinitionQuery myCounterDefinitionQuery = new CounterDefinitionQuery(myCounterDefinitionRepository);
                myResult.CounterPM = myCounterQuery.GetSingleCounterPM(CounterId, tenant);
                myResult.CounterDefinitions = myCounterDefinitionQuery.GetCounterDefinitionsByCounterId(CounterId, tenant).ToList();

                UserQuery userQuery = new UserQuery(tenant);
                UserPM user = userQuery.GetSingleUserByEmailOrIdAndTenantOrTenantZero(null, loggedUserEmail, tenant);
                if (user.IsCustomerCare)
                {
                    myResult.IsCounterUsed = false;
                }

                else
                {
                    myResult.IsCounterUsed = myCounterDefinitionQuery.CheckIfUsed(CounterId, tenant);
                }

                if (myResult.CounterPM.Code == "HAWB")
                {
                    TenantSettingRepository myTenantSettingRepository = new TenantSettingRepository(myContext);
                    TenantSettingQuery myTenantSettingQuery = new TenantSettingQuery(myTenantSettingRepository);
                    myResult.TenantSettings = myTenantSettingQuery.GetTenantSettingsByTenant(tenant).ToList();

                    CounterStat myCounterStats = (from d in myContext.CounterStats
                                                  where d.Tenant == tenant && d.CounterId == CounterId
                                                  select d).FirstOrDefault();

                    if (myCounterStats != null)
                    {
                        myResult.LastDBValue = myCounterStats.LastValue;
                    }
                }

                if (LogitudeSettings.DeploymentStage == "Dev")
                {
                    myResult.IsCounterUsed = false;
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Post(CounterAPIHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    IWebFreightContext Context = WebFreightContext.GetContext(tenant);
                    TenantSettingService myTenantSettingService = new TenantSettingService(Context, tenant);
                    CounterDefinitionService myCounterDefinitionService = new CounterDefinitionService(Context, loggedUserEmail, tenant);
                    CounterDefinitionQuery myCounterDefinitionQuery = new CounterDefinitionQuery(tenant);
                    CustomizedARInvoiceCounterService myCustomizedARInvoiceCounterService = new CustomizedARInvoiceCounterService(tenant, myCounterDefinitionService);
                    bool hasCustomizedARInvoiceCounterFeature = FeatureToggleHelper.HasFeatureToggle("ICC", tenant);
                    if (args.IsCustomized && hasCustomizedARInvoiceCounterFeature)
                    {
                        myCustomizedARInvoiceCounterService.Run(args.CounterDefinitions, args.CounterId);
                        args.CounterDefinitions = myCounterDefinitionQuery.GetCustomizedCounterDefinitionsByCounterId(args.CounterId, tenant).ToList();
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, args);
                    }

                    myCustomizedARInvoiceCounterService.RemoveCustomizedCounterDefinitionsByCounterId(args.CounterId);

                    foreach (CounterDefinitionPM item in args.CounterDefinitions)
                    {
                        if (item.Id == null)
                        {
                            myCounterDefinitionService.Create(item);
                        }

                        else
                        {
                            myCounterDefinitionService.Update(item);
                        }
                    }

                    if (args.CounterPM?.Code == "HAWB")
                    {
                        foreach (TenantSettingPM item in args.TenantSettings)
                        {
                            if (item.Id == null)
                            {
                                myTenantSettingService.Create(item);
                            }

                            else
                            {
                                myTenantSettingService.Update(item);
                            }
                        }
                    }

                    args.CounterDefinitions = myCounterDefinitionQuery.GetCounterDefinitionsByCounterId(args.CounterId, tenant).ToList();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCounterProperties(string counterCode)
        {
            // SHIP
            // MAST

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;

            SecurityUtility.AuthenticationOnTenant(tenant);

            CounterProperties myResult = new CounterProperties();

            if (!string.IsNullOrEmpty(counterCode))
            {
                IWebFreightContext context = WebFreightContext.GetContext(tenant);

                Counter iCounter = (from d in context.Counters where d.Tenant == tenant && d.Code.ToLower() == counterCode.ToLower() select d).FirstOrDefault();
                if (iCounter != null)
                {
                    myResult.CounterId = iCounter.Id;

                    int myGroupbyCount = (from d in context.CounterDefinitions
                                         where d.CounterId == iCounter.Id
                                         group d by d.Prefix into g
                                         select g).Count();

                    switch (myGroupbyCount)
                    {
                        case 1:
                            {
                                myResult.SameForAllDirectios = true;
                                myResult.SameForAllTransports = true;
                                break;
                            }

                        case 2:
                        case 3:
                            {
                                myResult.SameForAllDirectios = true;
                                myResult.SameForAllTransports = false;
                                break;
                            }

                        case 4:
                            {
                                myResult.SameForAllDirectios = false;
                                myResult.SameForAllTransports = true;
                                break;
                            }

                        default:
                            {
                                myResult.SameForAllDirectios = false;
                                myResult.SameForAllTransports = false;
                                break;
                            }
                    }
                }
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }
        //public HttpResponseMessage GetLargestLastValueFromCounterStatByCounterId(string counterId)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;

        //        SecurityUtility.AuthenticationOnTenant(tenant);

        //        CounterStatRepository counterStatRepository = new CounterStatRepository(tenant);
        //        int myResult = counterStatRepository.GetLargestLastValueOfCounterStatsByCounterId(counterId, tenant);

        //        return Request.CreateResponse(HttpStatusCode.OK, myResult);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}
    }

    public class CounterAPIHelper
    {
        public string CounterId { get; set; }
        public bool IsCounterUsed { get; set; }
        public int LastDBValue { get; set; }
        public CounterPM CounterPM { get; set; }
        public List<TenantSettingPM> TenantSettings { get; set; }
        public List<CounterDefinitionPM> CounterDefinitions { get; set; }
        public bool IsCustomized { get; set; }
    }

    public class CounterProperties
    {
        public string CounterId { get; set; }
        public bool SameForAllDirectios { get; set; }
        public bool SameForAllTransports { get; set; }
    }
}