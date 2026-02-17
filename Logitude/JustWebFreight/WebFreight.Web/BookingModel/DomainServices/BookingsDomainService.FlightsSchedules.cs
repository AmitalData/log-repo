using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.BookingModel.DomainServices
{
    public partial class BookingsDomainService
    {
        private FlightsSchedulesRequestQueryService flightsSchedulesRequestQueryService;

        public FlightsSchedulesRequestPM GetSingleFlightsSchedulesRequestPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            flightsSchedulesRequestQueryService = new FlightsSchedulesRequestQueryService(objectContext);

            FlightsSchedulesRequestPM entityPM = flightsSchedulesRequestQueryService.GetSingle(id, true, false);

            return entityPM;
        }

        public void UpdateFlightsSchedulesRequest(FlightsSchedulesRequestPM entityPM)
        {

        }

        public void UpdateFlightsSchedulesResponse(FlightsSchedulesResponsePM entityPM)
        {

        }
    }
}