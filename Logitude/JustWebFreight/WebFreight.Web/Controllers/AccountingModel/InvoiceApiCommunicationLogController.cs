


using Logitude.Accounting.BL.Interfaces.Magaya;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.AccountingModel
{ 

    
    public partial class InvoiceApiCommunicationLogController : ApiController
    {
        [HttpPost]
        [Route("api/InvoiceApiCommunicationLog/ReSendCommunication")]
        public HttpResponseMessage ReSendCommunication(string id)
        {
            try
            {
                if (!HttpContext.Current.Request.Headers.AllKeys.Contains("Token"))
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Missing authentication token.");

                string token = HttpContext.Current.Request.Headers["Token"]; 
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (string.IsNullOrWhiteSpace(id))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing or empty ID.");

                InvoiceApiService invoiceApiService = new InvoiceApiService();
                bool res = invoiceApiService.ReSendQueue(id, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"ReSendCommunication failed: {ex.Message}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}
	 