using System;
using System.Collections.Generic; 
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.Tools.EntityService; 
using Logitude.BL.ShipmentsModel.EntityAMs; 
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries; 
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel; 
using Logitude.Server.Tools; 
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ForwarderAExportShipmentsController : ApiController
    {
         
        public HttpResponseMessage Post(NewAExporterShipmentAM newAExporterShipmentAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(newAExporterShipmentAM.Tenant);

                ForwarderExportShipmentsServie forwarderExportShipmentsServie = new ForwarderExportShipmentsServie(newAExporterShipmentAM, newAExporterShipmentAM.Tenant);
                string communicationLogId = forwarderExportShipmentsServie.AddCommunicationLogQueueMessages();
                return Request.CreateResponse(HttpStatusCode.OK, communicationLogId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}