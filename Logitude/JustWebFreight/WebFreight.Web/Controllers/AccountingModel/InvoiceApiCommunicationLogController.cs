


using Logitude.Accounting.BL.Interfaces.Magaya;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

                string token = HttpContext.Current.Request.Headers["Token"]; 
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Empty authentication token.");

                if (string.IsNullOrWhiteSpace(id))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing or empty ID.");

                InvoiceApiService invoiceApiService = new InvoiceApiService();
               invoiceApiService.ReSendQueue(id, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"ReSendCommunication failed: {ex.Message}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}
	 