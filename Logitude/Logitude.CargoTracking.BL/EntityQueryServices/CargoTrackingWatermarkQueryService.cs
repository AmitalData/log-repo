 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.BL.EntityDataMappings;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Data.EntityKeys;
using Logitude.CargoTracking.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CargoTracking.BL.EntityQueryServices
{ 
   public partial class CargoTrackingWatermarkQueryService
    {
        const string CargoTrackingShipments = "CargoTrackingShipments";
        public List<CargoTrackingWatermark> GetAllWaterMarks()
        {
            List<CargoTrackingWatermark> cargoTrackingWatermarks = repository.GetAll().ToList();
            return cargoTrackingWatermarks;
        }
        public CargoTrackingWatermark GetShipmentsWaterMarks()
        {
            CargoTrackingWatermark cargoTrackingWatermarks = repository.GetAll().Where(e=>e.TableName == CargoTrackingShipments).FirstOrDefault();
            return cargoTrackingWatermarks;
        }

    }
   
}
	 