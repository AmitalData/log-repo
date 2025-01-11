using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityAMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Newtonsoft.Json;
using Logitude.Server.Tools.QueueService;
using WebFreight.Web.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Transactions;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.StorageService;
using Microsoft.ServiceBus.Messaging;
using Logitude.SystemLogs;
using Logitude.BL.ShipmentsModel.Tools.ContainerTracking;
using Logitude.BL.DataContracts;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ContainerTrackingController : ApiController
    {
        

        public HttpResponseMessage PostVizionUpdateContainerStatus(VisionContainerStatus containerStatus)
        {
            try
            {
                GeneralContainerTrackingService containerTrackingService = new GeneralContainerTrackingService();
                containerTrackingService.UpdateStatusFromVizion(containerStatus);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {

                throw new Exception();
            }
            
        }

        
    }
}