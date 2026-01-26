
using Simplog.Server.Infrastructure.Helpers;
using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Collections.Generic;


namespace WebFreight.Web.Controllers.InfrastructureModel.Generated.PMControllers
{
   
    public class RatesTablesCustomController : ApiController
    {
        public HttpResponseMessage UpdateRate(UpdateRateRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        RatesTablePM entityPM = request.EntityPM;
                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        RatesTableService service = new RatesTableService(MyContext, entityPM.Tenant);

                        if(IsFullAccountingActivated(entityPM.Tenant))
                        {
                           entityPM.Rate = CalculateRateAccordingUnits(entityPM);
                        }

                        service.Create(entityPM);

                        List<CurrencyRatePM> currencyRates = request.CurrencyRates;
                        currencyRates.ForEach(currencyRate => currencyRate.Rate = CalculateRateAccordingUnits(currencyRate.Rate, entityPM.Unit));
                        CurrencyRateService currencyRateUpdateService = new CurrencyRateService(MyContext, entityPM.Tenant);
                        currencyRateUpdateService.Create(currencyRates, entityPM);

                        scope.Complete();
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage GetLastUpdateByCurrencyCode(string foreignCurrency,string tenantCurrencyId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                IWebFreightContext MyContext = WebFreightContext.GetContext(authToken.Tenant);
                RatesTableQuery ratesTableQuery = new RatesTableQuery(authToken.Tenant);
                CurrencyQuery queryService = new CurrencyQuery(authToken.Tenant);

                if (IsFullAccountingActivated(authToken.Tenant))
                {
                    CurrencyPM currency = queryService.GetSingleCurrencyByCode(foreignCurrency, authToken.Tenant);
                    if (currency != null)
                    {
                        var lastRate = ratesTableQuery.GetLastRecord(authToken.Tenant, currency.Id, tenantCurrencyId);
                        return Request.CreateResponse(HttpStatusCode.OK, lastRate);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private double CalculateRateAccordingUnits(RatesTablePM ratesTable)
        {
            if (ratesTable.Unit != null)
            {
                if (ratesTable.Unit > 0)
                {
                    return (double)(ratesTable.Rate / ratesTable.Unit);
                }
            }
            return (double)ratesTable.Rate;

        }
        private double CalculateRateAccordingUnits(double rate,int? unit)
        {
            if (unit != null)
            {
                if (unit > 0)
                {
                    return (double)(rate / unit);
                }
            }
            return (double)rate;

        }

        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }

        public class UpdateRateRequest
        {
            public RatesTablePM EntityPM { get; set; }
            public List<CurrencyRatePM> CurrencyRates { get; set; }
        }
    }
}