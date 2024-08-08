using Logitude.Server.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;


namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel
    
    public class CityHybridController : ApiController
    {
        // GET: City
        [HttpPost]
        public Response Test([FromBody] object[] t)
        {

            return new Response();
        
        }


    }
}