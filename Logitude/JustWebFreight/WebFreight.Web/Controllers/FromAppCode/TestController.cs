using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebFreight.Web.Controllers.Test
{
    //[RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        [HttpGet]        
        public HttpResponseMessage iis()
        {
            string XML = $"<pingdom_http_custom_check><status>OK</status><response_time>{HttpContext.Current.Timestamp.Millisecond}</response_time></pingdom_http_custom_check>";
            return new HttpResponseMessage()
            {
                Content = new StringContent(XML, Encoding.UTF8, "application/xml")
            };
        } 
    }
}
