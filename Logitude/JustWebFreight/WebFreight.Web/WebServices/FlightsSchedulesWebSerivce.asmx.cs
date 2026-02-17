using Logitude.XSD;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using System.Web.Services;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Helpers;
using Logitude.XSD.FVR;


namespace WebFreight.Web.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FlightsSchedulesWebSerivce : System.Web.Services.WebService
    {
        [WebMethod]
        public FVRResultClass Send(string myAirlineId, string myFromPortId, string myToPortId, DateTime? myETD, DateTime? myETA, decimal? myVolume, decimal? myGrossWeight, string myVolumeUnitCode, string myGrossWeightUnitCode, string myShipmentId, string myBookingId, int tenant, string myRecipient)
        {
            FVRManager myManager = new FVRManager(tenant);
            FVRResultClass myResultClass = myManager.SendFVR(myAirlineId, myFromPortId, myToPortId, myETD, myETA, myVolume, myGrossWeight, myVolumeUnitCode, myGrossWeightUnitCode, myShipmentId, myBookingId, myRecipient);
            return myResultClass;
        }
    }
}
