using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Models
{
    public class APIResponse<T>
    {
        public string ErrorMessage { set; get; }
        public T Data { set; get; }
        public HttpStatusCode StatusCode { set; get; }
    }
}
