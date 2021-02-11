using System.Net;

namespace Logitude.Test.Base.Models.Api
{
    public class ApiResponse<T>
    {
        public string ErrorMessage { set; get; }
        public T Data { set; get; }
        public HttpStatusCode StatusCode { set; get; }
    }
}