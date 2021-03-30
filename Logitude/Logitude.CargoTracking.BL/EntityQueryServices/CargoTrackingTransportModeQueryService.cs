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
    public partial class CargoTrackingTransportModeQueryService : EntityQueryService<CargoTrackingTransportMode, CargoTrackingTransportModeKeys, CargoTrackingTransportModePM, object, CargoTrackingTransportModeKeys>
    {
        internal CargoTrackingTransportModePM GetSinglePM(string id, int tenant)
        {
            throw new NotImplementedException();
        }
    }
}
