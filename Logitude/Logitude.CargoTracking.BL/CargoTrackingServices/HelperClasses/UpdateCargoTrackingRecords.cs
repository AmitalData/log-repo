using Logitude.CargoTracking.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class UpdateCargoTrackingRecords
    {
        public int NumberRecordUpdated { get; set; }
        public int NumberRecordUpdated2 { get; set; }
        public bool IsUpadteWaterMark { get; set; }
        public RecordUpdated RecordUpdated { get; set; }
        public List<CargoTrackingMilestoneList> Milestones { get; set; }
        public CargoTrackingUpdateDataBaseArgs CargoTrackingUpdateDataBaseArgs { get; set; }

        // first key = tenant , second key = milestone code , value = milestone code
        public Dictionary<int, Dictionary<string, string>> MilestonesNotPermitted { get; internal set; }
        public List<int> AllTenantIds { get; internal set; }
    }
}
