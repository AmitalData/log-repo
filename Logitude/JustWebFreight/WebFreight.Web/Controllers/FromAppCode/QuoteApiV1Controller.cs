using Logitude.BL.QuoteModel.APIDataContract;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class QuoteApiV1Controller : ApiController
    {
        public HttpResponseMessage GetSingleQuote(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant =  authToken.Tenant;
                QuoteQueryService Service = new QuoteQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetQuoteById(id, tenant);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            } 
        }


        // POST api/<controller>
        //public HttpResponseMessage Post(QuoteApiV1 MyQuoteApiV1)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        SecurityUtility.AuthenticationOnTenant(EntityAM.ImporterTenant);
        //        SecurityUtility.CheckContactFeature("DocumentsFiling", "NEW", EntityAM.ImporterTenant);
        //        IQuotesContext context = QuotesContext.GetContext(tenant);
        //        QuoteService myQuoteService = new QuoteService(context, tenant);
        //        QuoteApiV1QueryService Service = new QuoteApiV1QueryService(tenant);
        //        var MyQuotePM = Service.QuoteApiV1DataMappingAndValidatin(MyQuoteApiV1, tenant);
        //        myQuoteService.Update(MyQuotePM);
        //        return Request.CreateResponse(HttpStatusCode.OK, MyQuotePM);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}


        //PUT api/<controller>/5
        //public HttpResponseMessage Put(QuoteApiV1 MyQuoteApiV1)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        SecurityUtility.AuthenticationOnTenant(EntityAM.ImporterTenant);
        //        SecurityUtility.CheckContactFeature("DocumentsFiling", "NEW", EntityAM.ImporterTenant);
        //        IQuotesContext context = QuotesContext.GetContext(tenant);
        //        QuoteService myQuoteService = new QuoteService(context, tenant);
        //        QuoteApiV1QueryService Service = new QuoteApiV1QueryService(tenant);
        //        var MyQuotePM = Service.QuoteApiV1DataMappingAndValidatin(MyQuoteApiV1, tenant);
        //        myQuoteService.Create(MyQuotePM);
        //        return Request.CreateResponse(HttpStatusCode.OK, MyQuotePM);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}
    }
}
