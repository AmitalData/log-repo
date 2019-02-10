using Logitude.BL.QuoteModel.APIDataContract;
using Logitude.BL.QuoteModel.APIDataContract.QueryService;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs
{
    public class QuoteStatusController : ApiController
    {
        public HttpResponseMessage GetSingleQuoteStatusById(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				QuoteStatusQueryService Service = new QuoteStatusQueryService(tenant);
                QuoteStatus Result = Service.GetQuoteStatusById(id, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleQuoteStatusByNumber(string quoteNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				QuoteStatusQueryService Service = new QuoteStatusQueryService(tenant);
                QuoteStatus Result = Service.GetQuoteStatusByNumber(quoteNumber, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(QuoteStatus entity)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
						QuoteStageRepository repository = new QuoteStageRepository(authToken.Tenant);
                        QuoteQuery quoteQuery = new QuoteQuery(authToken.Tenant);
                        QuotePM quotePM = quoteQuery.GetSinglePMByQuoteNumber(entity.QuoteNumber, authToken.Tenant);
                         
                        if(entity.QuoteAcceptDate != null)
                        {
                            quotePM.IsClosed = true;
                            quotePM.ActionType = "Accept";
                            quotePM.EventNote = entity.QuoteAcceptNote;
                        }

                        if (entity.QuoteDeclineDate != null)
                        {
                            quotePM.IsClosed = true;
                            quotePM.ActionType = "Decline";
                            quotePM.EventNote = entity.QuoteDeclineNote;

                            if (entity.QuoteDeclineReason != null)
                            {
                                quotePM.QuoteClosingReasonCode = entity.QuoteDeclineReason.Code;
                            }
                        }

                        if (entity.QuoteCancelDate != null || (entity.IsQuoteCancel != null && entity.IsQuoteCancel.Value))
                        {
                            quotePM.IsCancelled = true;
                            quotePM.EventNote = entity.QuoteCancelNote;
                        }

                        quotePM.DontExportQuotationsToIntegratedSystem = true;

                        IQuotesContext MyContext = QuotesContext.GetContext(authToken.Tenant);
                        QuoteService service = new QuoteService(MyContext, authToken.Tenant);
                        service.SetChangeSet(new List<QuoteChargePM>(), new List<QuoteFollowUpPM>(), new List<QuotePackagePM>(), new List<QuoteDocumentVersionPM>());
                        service.Update(quotePM);

                        APIHelper.AddCommunicationLog("D", entity, quotePM, "Quote", quotePM.Id, "Quote Status API", authToken.Tenant);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Quote",null, "Quote Status API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Quote", null, "Quote Status API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
      
            }
        }
    }
}