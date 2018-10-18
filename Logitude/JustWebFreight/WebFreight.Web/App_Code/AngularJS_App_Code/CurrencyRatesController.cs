using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class CurrencyRatesController : ApiController
    {
        public HttpResponseMessage GetAll(string baseCurrencyId, string dateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                if (string.IsNullOrEmpty(baseCurrencyId))
                {
                    string msg = TranslateTextsClass.Translate("General.M.AccountingCurrencyIsNotSet", authToken.Tenant);
                    throw new ApplicationException(msg);
                }

                DateTime? loadingDate = DateHelper.GetDate(dateString);
                if (loadingDate == null)
                {
                    loadingDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }

                List<LastRate> myResult = new List<LastRate>();
                IWebFreightContext objectContext = WebFreightContext.GetContext(authToken.Tenant);

                RatesTableRepository myRepository = new RatesTableRepository(objectContext);
                RatesTableQuery myQuery = new RatesTableQuery(myRepository);
                CurrencyRepository currencyRepository = new CurrencyRepository(authToken.Tenant);
                Currency baseCurrency = currencyRepository.GetCurrencies(authToken.Tenant).Where(r => r.Id == baseCurrencyId).FirstOrDefault();
                List<Currency> foreignCurrencies = currencyRepository.GetCurrencies(authToken.Tenant).Where(c => c.Id != baseCurrencyId).ToList();

                foreach (Currency currency in foreignCurrencies)
                {
                    LastRate lastRate = myQuery.GetLastRecordByValueDate(authToken.Tenant, currency.Id, baseCurrencyId, loadingDate);
                    if (lastRate != null)
                    {
                        lastRate.BaseCurrencyId = baseCurrencyId;
                        lastRate.BaseCurrencyCode = baseCurrency.Code;
                        myResult.Add(lastRate);
                    }

                    else
                    {
                        LastRate newLastRate = new LastRate()
                        {
                            Id = IdCounter.GetNumber("LastRate", authToken.Tenant).ToString(),
                            Tenant = authToken.Tenant,
                            ForeignCurrencyId = currency.Id,
                            ForeignCurrencyCode = currency.Code,
                            ForeignCurrencyName = currency.EnglishName,
                            BaseCurrencyId = baseCurrency.Id,
                            BaseCurrencyCode = baseCurrency.Code,
                            HistoryCount = 0,
                            Rate = null,
                        };

                        myResult.Add(newLastRate);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCurrenciesExchangeRateByValueDate(string currencyId, string dateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                List<LastRate> results = new List<LastRate>();

                DateTime? loadingDate = DateHelper.GetDate(dateString);
                if (loadingDate == null)
                {
                    loadingDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }

                WebFreightDomainService domain = new WebFreightDomainService();
                results = domain.GetCurrenciesExchangeRateByValueDate(authToken.Tenant, currencyId, loadingDate);
                return Request.CreateResponse(HttpStatusCode.OK, results);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRatesByValueDate(string currencyId, string dateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                List<RatesTablePM> results = new List<RatesTablePM>();

                DateTime? loadingDate = DateHelper.GetDate(dateString);
                if (loadingDate == null)
                {
                    loadingDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }

                WebFreightDomainService domain = new WebFreightDomainService();
                results = domain.GetRatesByValueDate(authToken.Tenant, currencyId, loadingDate);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetInsertListOfRatesTable(List<LastRate> ratesTables)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                WebFreightDomainService domain = new WebFreightDomainService();

                if (ratesTables != null && ratesTables.Count() > 0)
                {
                    foreach (var item in ratesTables)
                    {
                        RatesTablePM entityPM = new RatesTablePM();
                        entityPM.Tenant = item.Tenant;
                        entityPM.BaseCurrencyId = item.BaseCurrencyId;
                        entityPM.ForeignCurrencyId = item.ForeignCurrencyId;
                        entityPM.LogDateTime = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                        entityPM.ValueDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                        entityPM.Rate = item.Rate;
                        domain.InsertRatesTable(entityPM);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}