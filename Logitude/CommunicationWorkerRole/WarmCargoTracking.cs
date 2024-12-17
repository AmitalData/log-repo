using System;
using System.Collections.Generic;
using System.Threading;
using Logitude.SystemLogs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.QueueService;
using System.Web;
using System.Reflection;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System.Linq;
using Logitude.CargoTracking.BL.EntityQueryServices;


namespace CommunicationWorkerRole
{
    class WarmCargoTracking : WorkerEntryPoint
    {
        public ICargoTrackingContext MyContext;
        public CargoTrackingShipmentSearchListQueryService  ShipmentSearchQuery;
        public CargoTrackingShipmentQueryService cargoTrackingShipmentQueryService;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    GetWarmShipments();
                    Thread.Sleep(300000);
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }

        }


        public void GetWarmShipments()
        {
            CargoTrackingShipmentSearch shipmentSearch = ShipmentSearchQuery.GetFirstShipmentSearchesForWarmCargoTracking();
            List<CargoTrackingShipmentList> shipments = cargoTrackingShipmentQueryService.GetShipments(shipmentSearch.SearchFields, shipmentSearch.Tenant).OrderByDescending(s => s.CreateDate).ToList();

        }


        public override bool OnStart()
        {
            MyContext = CargoTrackingContext.GetContext((int)Tenant);
            ShipmentSearchQuery = new CargoTrackingShipmentSearchListQueryService(MyContext);
            cargoTrackingShipmentQueryService = new CargoTrackingShipmentQueryService(MyContext);
            return base.OnStart();

        }

    }
}
