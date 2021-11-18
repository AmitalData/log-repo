using Logitude.CargoTracking.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CheckCurrentMilestoneArgs
    {
        public int currentWheight = 0;
        public DataRow tableRow;
        public CargoTrackingMilestoneList milestone;
        public object date;

        public CheckCurrentMilestoneArgs(DataRow tableRow)
        {
            this.tableRow = tableRow;
        }
    }
}
