using Logitude.CargoTracking.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class SetTableLogicArgs
    {
        public DataRow TableRow { get; set; }
        public string TableName { get; set; }
        public int ConditionNumber { get; set; }
        public List<CargoTrackingMilestoneList> Milestones { get; set; }
        public Dictionary<int, Dictionary<string, string>> NotPermittedMilestones { get; internal set; }
    }
}
