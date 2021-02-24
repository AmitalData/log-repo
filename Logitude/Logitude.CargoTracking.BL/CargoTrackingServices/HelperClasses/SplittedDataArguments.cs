using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    class SplittedDataArguments
    {
        public DataRow tableRow { get; set; }
        public DataTable dataTable { get; set; }
        public string coulmnName { get; set; }
        public char delimiter { get; set; }

        public SplittedDataArguments(DataRow tableRow, DataTable dataTable, string coulmnName, char delimiter)
        {
            this.tableRow = tableRow;
            this.dataTable = dataTable;
            this.coulmnName = coulmnName;
            this.delimiter = delimiter;
        }
    }
}
