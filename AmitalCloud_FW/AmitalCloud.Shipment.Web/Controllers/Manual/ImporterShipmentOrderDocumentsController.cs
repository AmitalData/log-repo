using Logitude.BL.CommonDataModel.EntityAMs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.ImporterShipmentOrderDocuments;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ImporterShipmentOrderDocumentsController : ApiController
    {
        public HttpResponseMessage Post(DocumentsFilingAM documentsFilingAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(documentsFilingAM.ImporterTenant);
                string correlationId = HttpContext.Current.Request.Headers["CorrelationId"];
                var documentsFiling = new ImporterShipmentOrderDocumentsService(documentsFilingAM.ImporterTenant, correlationId).CreateDocument(documentsFilingAM);
                return Request.CreateResponse(HttpStatusCode.OK, new List<string>() { documentsFiling.Id, documentsFiling.EntityNumber });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}