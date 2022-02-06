using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data; 
using Logitude.CargoTracking.Data.EntityMapping;

namespace Logitude.CargoTracking.Data
{

    public partial interface ICargoTrackingContext : IContext
    {
   
       	 IDbSet<CargoDisconnectQueue> CargoDisconnectQueues { get; }
		 IDbSet<CargoReferencesSyncQueue> CargoReferencesSyncQueues { get; }
		 IDbSet<CargoTrackingCard> CargoTrackingCards { get; }
		 IDbSet<CargoTrackingCountry> CargoTrackingCountries { get; }
		 IDbSet<CargoTrackingHeaderEntityType> CargoTrackingHeaderEntityTypes { get; }
		 IDbSet<CargoTrackingIncrementalStat> CargoTrackingIncrementalStats { get; }
		 IDbSet<CargoTrackingMilestone> CargoTrackingMilestones { get; }
		 IDbSet<CargoTrackingPort> CargoTrackingPorts { get; }
		 IDbSet<CargoTrackingShipment> CargoTrackingShipments { get; }
		 IDbSet<CargoTrackingShipmentComputed> CargoTrackingShipmentComputeds { get; }
		 IDbSet<CargoTrackingShipmentMaster> CargoTrackingShipmentMasters { get; }
		 IDbSet<CargoTrackingShipmentSearch> CargoTrackingShipmentSearches { get; }
		 IDbSet<CargoTrackingTransportMode> CargoTrackingTransportModes { get; }
		 IDbSet<CargoTrackingWatermark> CargoTrackingWatermarks { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}