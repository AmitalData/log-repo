
using Simplog.Server.Infrastructure.Helpers;
using System;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;


namespace WebFreight.Web.Controllers.InfrastructureModel.Generated.PMControllers
{
   
    public class RatesTablesCustomController : ApiController
    {
        public HttpResponseMessage UpdateRate(RatesTablePM entityPM)
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

                        IWebFreightContext MyContext = WebFreightContext.GetContext(entityPM.Tenant);
                        RatesTableService service = new RatesTableService(MyContext, entityPM.Tenant);
                        
                        if(IsFullAccountingActivated(entityPM.Tenant))
                        {
                           entityPM.Rate = CalculateRateAccordingUnits(entityPM);
                        }
                       
                        service.Create(entityPM);

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

        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }
    }
}