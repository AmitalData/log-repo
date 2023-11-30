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
    public partial class ContainerTrackingResponseMapping
    {
        public static void MapFields(ContainerTrackingResponsePM containerTrackingResponsePM, ContainerTrackingResponse containerTrackingResponse, bool isNewEntity)
        {
            if (isNewEntity)
            {
                containerTrackingResponse.Id = containerTrackingResponsePM.Id;
                containerTrackingResponse.Tenant = containerTrackingResponsePM.Tenant;
                containerTrackingResponse.CreateDate = DateTime.Now;
            }
            BuildSearchField(containerTrackingResponsePM, containerTrackingResponse);
            containerTrackingResponse.CommunicationLogId = containerTrackingResponsePM.CommunicationLogId;
            containerTrackingResponse.ContainerTrackingRequestId = containerTrackingResponsePM.ContainerTrackingRequestId;



        }

        public static void BuildSearchField(ContainerTrackingResponsePM containerTrackingResponsePM, ContainerTrackingResponse containerTrackingResponse)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingResponse.ContainerTrackingRequestId);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingResponse.CommunicationLogId);


            containerTrackingResponsePM.SearchFields = mySearchFields;
            containerTrackingResponse.SearchFields = mySearchFields;
        }
    }
}
