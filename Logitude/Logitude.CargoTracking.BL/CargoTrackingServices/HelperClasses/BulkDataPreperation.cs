using Logitude.CargoTracking.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class BulkDataPreperation
    {
        public DataTable MainDataTable { get; set; }
        public DataTable InnerDataTable { get; set; }
        public SqlDataReader SqlDataReader { get; set; }
        public CargoTrackingTable CargoTrackingTable { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public int NumberOfMainCoulmnsUpdated { get; set; }
        public int NumberOfInnerCoulmnsUpdated { get; set; }

        public int MaxRecoredTakeEachTime { get; set; }
        public int NumberRecoredTake { get; set; }
        public CargoTrackingUpdateDataBaseArgs CargoTrackingUpdateDataBaseArgs { get; set; }
        public DataTable SelectedDataTable { get; set; }
        public string CoulmnForCusstomMapping { get; set; }
        public List<CargoTrackingMilestoneList> MilestoneList { get; internal set; }

        // first key = tenant , second key = milestone code , value = milestone code
        public Dictionary<int, Dictionary<string, string>> MilestonesNotPermitted { get; internal set; }
    }
}
