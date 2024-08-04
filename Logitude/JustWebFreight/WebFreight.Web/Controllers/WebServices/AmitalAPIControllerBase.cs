using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.AmitalAPI;

namespace WebFreight.Web.Controllers.WebServices
{
    public class AmitalAPIControllerBase<API, DataMember>  : ApiController
    {
        TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
        AmitalApiCRUDApiBase<DataMember> amitalApiCRUDApi = (AmitalApiCRUDApiBase<DataMember>)Activator.CreateInstance(typeof(API));

        [HttpGet]
        public virtual HttpResponseMessage Get(string id)
        {
            string token = AutorizeAndGetToken();
            var res = amitalApiCRUDApi.Get(token, id);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpGet]
        public virtual HttpResponseMessage GetAll()
        {
            string token = AutorizeAndGetToken();
            var res = amitalApiCRUDApi.GetAll(token);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpPost]
        public virtual HttpResponseMessage Post([FromBody] DataMember body)
        {            
            AuthenticationToken tokenData = HeaderHelper.GetTokenData();
            if (tokenData == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            string token = GetAmitalApiToken(tokenData.Tenant);
            var res = amitalApiCRUDApi.Create(token, body, tokenData.Email);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpDelete]
        public virtual HttpResponseMessage Delete(string id)
        {
            AuthenticationToken tokenData = HeaderHelper.GetTokenData();
            if (tokenData == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            string token = GetAmitalApiToken(tokenData.Tenant);
            var res = amitalApiCRUDApi.Delete(token, id, tokenData.Email);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        [HttpPut]
        public virtual HttpResponseMessage Put(string id, [FromBody] DataMember body)
        {
            AuthenticationToken tokenData = HeaderHelper.GetTokenData();
            if (tokenData == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            string token = GetAmitalApiToken(tokenData.Tenant);
            var res = amitalApiCRUDApi.Update(token, id, body, tokenData.Email);
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }

        protected string GetAmitalApiToken(int tenant) => tenantManagementQuery.GetSinglePM(tenant).AmitalApiToken;

        protected string AutorizeAndGetToken()
        {
            AuthenticationToken tokenData = HeaderHelper.GetTokenData();
            if (tokenData == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return GetAmitalApiToken(tokenData.Tenant);
        }
    }
}