using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class PerformanceLogController : ApiController
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

     
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        public bool PostPerformanceLog(int tenant, PerformanceLog performanceLog)
        {
                
            bool IsScceed = false;
            // SecurityUtility.AuthenticationOnTenant(tenant);
            PerformanceLogRepository rep = new PerformanceLogRepository();
            performanceLog.LogDateTimeGMT = DateTime.UtcNow;
            rep.Add(performanceLog);
            rep.SubmitChanges();
            IsScceed = true;
            return IsScceed;

        }
    

    }
}