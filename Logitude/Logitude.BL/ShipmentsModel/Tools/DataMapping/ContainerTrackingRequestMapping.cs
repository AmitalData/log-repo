using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ContainerTrackingRequestMapping
    {
       
        public static void MapFields(ContainerTrackingRequestPM containerTrackingRequestPM, ContainerTrackingRequest containerTrackingRequest, bool isNewEntity)
        {
            if (isNewEntity)
            {
                containerTrackingRequest.Id = containerTrackingRequestPM.Id;
                containerTrackingRequest.Tenant = containerTrackingRequestPM.Tenant;
                containerTrackingRequest.CreateDate = DateTime.Now;
            }

            containerTrackingRequest.ContainerNumber = containerTrackingRequestPM.ContainerNumber;
            containerTrackingRequest.Master = containerTrackingRequestPM.Master;
            containerTrackingRequest.Provider = containerTrackingRequestPM.Provider;
            containerTrackingRequest.RequestId = containerTrackingRequestPM.RequestId;
            containerTrackingRequest.ShipmentId = containerTrackingRequestPM.ShipmentId;
            containerTrackingRequest.CarrierCode = containerTrackingRequestPM.CarrierCode;
            containerTrackingRequest.ScacCode = containerTrackingRequestPM.ScacCode;
            containerTrackingRequest.Status = containerTrackingRequestPM.Status;
            containerTrackingRequest.IsSimulate = containerTrackingRequestPM.IsSimulate;
            containerTrackingRequest.ContainerId = containerTrackingRequestPM.ContainerId;
            BuildSearchField(containerTrackingRequestPM, containerTrackingRequest);
        }

        public static void BuildSearchField(ContainerTrackingRequestPM containerTrackingRequestPM, ContainerTrackingRequest containerTrackingRequest)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingRequest.ContainerNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingRequest.Master);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingRequest.RequestId);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingRequest.Provider);

            containerTrackingRequestPM.SearchFields = mySearchFields;
            containerTrackingRequest.SearchFields = mySearchFields;
        }
    }
}
