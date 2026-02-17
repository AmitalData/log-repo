using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class ActivityLogController : ApiController
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
        public void Post(string email, string module, string activity, int tenant, string cardId)
        {
            if(cardId == "null")
            {
                cardId = null;
            }

            if (module == "Help Center")
            {
                ActivityLog.SendTotangoContactActivity(email, module, activity, tenant, false, cardId, null);
            }

            else
            {
                ActivityLog.SendTotangoContactActivity(email, module, activity, tenant, true, cardId, "Mobile");
            }
        }
        
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}