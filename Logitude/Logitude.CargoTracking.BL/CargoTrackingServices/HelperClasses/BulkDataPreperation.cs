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
        public DataTable dataTable { get; set; }
        public DataTable dataTable2 { get; set; }
        public SqlDataReader sqlDataReader { get; set; }
        public CargoTable cargoTable { get; set; }
        public DateTime? automaticLastUpdateDate { get; set; }
        public int NumberOfCoulmnsUpdated { get; set; }
        public int NumberOfCoulmnsUpdated2 { get; set; }

        public int MaxRecoredTakeEachTime { get; set; }
        public int NumberRecoredTake { get; set; }
    }
}
