


using Logitude.Accounting.BL.Interfaces.Magaya;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.AccountingModel
{ 

    
    public partial class InvoiceApiCommunicationLogController : ApiController
    {

        public HttpResponseMessage ReSendCommunication(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                InvoiceApiService invoiceApiService = new InvoiceApiService();
                bool res = invoiceApiService.ReSendQueue(id, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}
	 