using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
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
        public HttpResponseMessage Put(AccountingCurrencyHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (args.TenantPM != null)
                    {
                        WebFreightDomainService webFreightDomain = new WebFreightDomainService();
                        CommonDataDomainService commonDataDomain = new CommonDataDomainService();

                        commonDataDomain.UpdateTenantPM(args.TenantPM);

                        foreach (LastRate item in args.LastRates)
                        {
                            RatesTablePM entityPM = new RatesTablePM();
                            entityPM.Tenant = item.Tenant;
                            entityPM.BaseCurrencyId = item.BaseCurrencyId;
                            entityPM.ForeignCurrencyId = item.ForeignCurrencyId;
                            entityPM.LogDateTime = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                            entityPM.ValueDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                            entityPM.Rate = item.Rate;
                            webFreightDomain.InsertRatesTable(entityPM);
                        }

                        TenantQuery tenantQuery = new TenantQuery(tenant);
                        TenantPM tenantPM = tenantQuery.GetTenantFromDB(tenant);
                        args.TenantPM = tenantPM;
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args.TenantPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetProfitCurrencyLastRate(string baseCurrencyId, string profitCurrencyId, string dateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DateTime? loadingDate = DateHelper.GetDate(dateString);
                if (loadingDate == null)
                {
                    loadingDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                }

                IWebFreightContext objectContext = WebFreightContext.GetContext(authToken.Tenant);
                RatesTableRepository myRepository = new RatesTableRepository(objectContext);
                RatesTableQuery myQuery = new RatesTableQuery(myRepository);
                LastRate lastRate = myQuery.GetLastRecordByValueDate(authToken.Tenant, profitCurrencyId, baseCurrencyId, loadingDate);

                return Request.CreateResponse(HttpStatusCode.OK, lastRate);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostChangeCurrency")]
        public HttpResponseMessage PostChangeCurrency(ChangeCurrencyArgs args)
        {
            try
            {
                string token = System.Web.HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                args.Tenant = authToken.Tenant;
                ChangeCurrencyManager changeCurrencyManager = new ChangeCurrencyManager(args);
                changeCurrencyManager.StartChange();

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class AccountingCurrencyHelper
    {
        public TenantPM TenantPM { get; set; }
        public List<LastRate> LastRates { get; set; }
    }

    public class ChangeCurrencyManager
    {
        private ChangeCurrencyArgs myArgs;
        private Tenant myTenant;
        private TenantRepository tenantRepository;
        private RatesTableRepository ratesTableRepository;
        public ChangeCurrencyManager(ChangeCurrencyArgs args)
        {
            this.myArgs = args;

            IWebFreightContext objectContext = WebFreightContext.GetContext(myArgs.Tenant);
            ratesTableRepository = new RatesTableRepository(objectContext);
            tenantRepository = new TenantRepository(myArgs.Tenant);

            this.GetTenant();
        }

        private void GetTenant()
        {
            myTenant = tenantRepository.GetSingleTenant(myArgs.Tenant);
        }
        public void StartChange()
        {
            if (myTenant != null)
            {
                this.UpdateTenantCurrency();
                this.CreateRates();
                this.SaveToDatabase();
                this.ComputeTotals();
            }
        }

        private void UpdateTenantCurrency()
        {
            switch(myArgs.Type)
            {
                case "Accounting":
                    {
                        this.myTenant.CurrencyId = myArgs.NewCurrencyId;
                        break;
                    }

                case "Profit":
                    {
                        this.myTenant.ProfitCurrencyId = myArgs.NewCurrencyId;
                        break;
                    }
            }

            tenantRepository.Update(myTenant);            
        }
        private void CreateRates()
        {
            switch (myArgs.Type)
            {
                case "Accounting":
                    {
                        this.CreateLocalCurrencyRates();
                        break;
                    }

                case "Profit":
                    {
                        this.CheckProfitCurrencyRate();
                        break;
                    }
            }
        }
        private void CreateLocalCurrencyRates()
        {
            foreach (LastRate item in myArgs.LastRates)
            {
                RatesTable ratesTable = this.CreateSingleRatesTable(item);                
                ratesTableRepository.Add(ratesTable);
            }
        }
        private void CheckProfitCurrencyRate()
        {
            RatesTable lastRate = ratesTableRepository.GetLastRateByValueDate(myArgs.Tenant, myArgs.NewCurrencyId, myTenant.CurrencyId, TenantServerConfigration.GetCurrentDateTime(myArgs.Tenant));
            if (lastRate != null)
            {
                lastRate.Rate = myArgs.ProfitCurrencyRate;
                ratesTableRepository.Update(lastRate);
            }

            else
            {
                RatesTable ratesTable = this.CreateSingleRatesTable(null);                
                ratesTableRepository.Add(ratesTable);
            }
        }
        private RatesTable CreateSingleRatesTable(LastRate lastRate)
        {
            RatesTable ratesTable = new RatesTable();
            ratesTable.Id = IdCounter.GetNumber("RatesTable", myArgs.Tenant).ToString();
            ratesTable.Tenant = myArgs.Tenant;
            ratesTable.BaseCurrencyId = lastRate != null ? lastRate.BaseCurrencyId : myTenant.CurrencyId;
            ratesTable.ForeignCurrencyId = lastRate != null ? lastRate.ForeignCurrencyId : myArgs.NewCurrencyId;
            ratesTable.LogDateTime = TenantServerConfigration.GetCurrentDateTime(myArgs.Tenant);
            ratesTable.ValueDate = TenantServerConfigration.GetCurrentDateTime(myArgs.Tenant).Date;
            ratesTable.Rate = lastRate != null ? lastRate.Rate : myArgs.ProfitCurrencyRate;
            return ratesTable;
        }
        private void SaveToDatabase()
        {
            tenantRepository.SubmitChanges();
            ratesTableRepository.SubmitChanges();
        }
        private void ComputeTotals()
        {
            switch (myArgs.Type)
            {
                case "Accounting":
                    {
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantLocalCurrency_Shipments", myArgs.NewCurrencyCode, myArgs.Tenant);
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantLocalCurrency_Quotes", myArgs.NewCurrencyCode, myArgs.Tenant);
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantLocalCurrency_ARInvoices", myArgs.NewCurrencyCode, myArgs.Tenant);
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantLocalCurrency_APInvoices", myArgs.NewCurrencyCode, myArgs.Tenant);
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantLocalCurrency_ARPayments", myArgs.NewCurrencyCode, myArgs.Tenant);
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantLocalCurrency_APPayments", myArgs.NewCurrencyCode, myArgs.Tenant);
                        break;
                    }

                case "Profit":
                    {
                        RunStoredProcedureClass.RunChangeSystemCurrencyProcedure("dbo.usp_ChangeTenantProfitCurrency", myArgs.NewCurrencyCode, myArgs.Tenant);
                        break;
                    }
            }
        }
    }
    
    public class ChangeCurrencyArgs
    {
        public int Tenant { get; set; }
        public string NewCurrencyId { get; set; }
        public string NewCurrencyCode { get; set; }
        public string Type { get; set; }
        public double? ProfitCurrencyRate { get; set; }
        public List<LastRate> LastRates { get; set; }
    }
}