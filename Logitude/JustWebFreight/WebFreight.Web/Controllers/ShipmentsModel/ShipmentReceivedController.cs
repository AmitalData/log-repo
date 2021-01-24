using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.EntityAMs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ShipmentReceivedController : ApiController
    {
        public HttpResponseMessage Post(ShipmentAdditionalCloudDataAM Data)
        {
            try
            {
                ExternalTasksQueueHelper externalTasksQueueHelper = new ExternalTasksQueueHelper(Data.Tenant);
                StatusUpdateExternalTasksQueueResult tasksQueueResult = externalTasksQueueHelper.PostStatusUpdateExternalTaskQueue(Data);
                if (tasksQueueResult.Response.HasError)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(tasksQueueResult.Exception));
                }

                return Request.CreateResponse(HttpStatusCode.OK, new List<string>() { });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}