using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class HybridTenantStateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        public void Post(string id, int tenant, int failedQueue, int waitingQueue)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            HybridTenantStateRepository hybridTenantStateRepository = new HybridTenantStateRepository(tenant);
            HybridTenantState entity = new HybridTenantState() {Tenant = tenant, FailedQueue = failedQueue , WaitingQueue = waitingQueue , LastUpdateDateTime = DateTime.UtcNow};
            hybridTenantStateRepository.Add(entity);
            hybridTenantStateRepository.SubmitChanges();

        }


        public void Delete(int id)
        {
        }
    }
}