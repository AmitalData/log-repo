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
    public class ForwarderExportShipmentsController : ApiController
    {
         
        public HttpResponseMessage Post(NewAExporterShipmentAM Shipment)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Shipment.Tenant);  
                ForwarderExportShipmentsServie forwarderExportShipmentsServie = new ForwarderExportShipmentsServie(Shipment);
                CommunicationLog communicationLog = new CommunicationLog();  
                if (forwarderExportShipmentsServie.IsExternalHybridPartner())
                { 
                    List<QueueTask> tasks = forwarderExportShipmentsServie.AddExternalTaskQueue();
                    communicationLog = forwarderExportShipmentsServie.HandelCommunicationLogData(tasks); 

                    if (!string.IsNullOrEmpty(communicationLog.QueueName))
                    {
                        try
                        {
                            forwarderExportShipmentsServie.AddQueueMessages(communicationLog);
                        }
                        catch (Exception ex)
                        {
                            forwarderExportShipmentsServie.AddExceptionQueueMessages(communicationLog, ex);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                        }
                    }
                } 
                return Request.CreateResponse(HttpStatusCode.OK, communicationLog.Id); 
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        } 
    }
}