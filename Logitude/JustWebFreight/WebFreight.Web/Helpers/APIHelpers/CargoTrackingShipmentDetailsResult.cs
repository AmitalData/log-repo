using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.BL.APIDataContract;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class CargoTrackingShipmentDetailsResult
    {
       public CargoTrackingShipmentDetails CargoTrackingShipmentDetails { get; set; }
       public bool HasMoreThanOneShipmentWithSameHouse { get; set; }

    }
}