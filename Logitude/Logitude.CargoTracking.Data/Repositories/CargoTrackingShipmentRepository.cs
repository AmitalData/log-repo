 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CargoTracking.Data.Repositories
{
   public partial class CargoTrackingShipmentRepository:IRepository<CargoTrackingShipment>
   {
        
		public List<CargoTrackingShipment> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<CargoTrackingShipment> GetBySecurityKeies(List<string> shipmentsSecurityKeies, int tenant)
        { 
            IQueryable<CargoTrackingShipment> shipments = (from shipment in currentContext.CargoTrackingShipments
                                                                 where
                                                                    shipmentsSecurityKeies.Contains(shipment.SecurityKey)
                                                                    && shipment.Tenant == tenant
                                                                 select shipment);

            return shipments;
        }

        public CargoTrackingShipment GetBySecurityKey(string SecurityKey, int tenant)
        {
            CargoTrackingShipment shipment = (from _shipment in currentContext.CargoTrackingShipments
                                                           where
                                                              _shipment.SecurityKey == SecurityKey
                                                              && _shipment.Tenant == tenant
                                                           select _shipment).FirstOrDefault();

            return shipment;
        }
    }

}
   