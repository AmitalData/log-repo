using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic
{
    public class ShareTableLogic
    {
        public static void CheckMilestone(CheckCurrentMilestoneArgs CurrentMilestoneArgs)
        {
            if (CurrentMilestoneArgs.milestone.Weight > CurrentMilestoneArgs.currentWheight)
            {
                CurrentMilestoneArgs.currentWheight = CurrentMilestoneArgs.milestone.Weight.HasValue ? CurrentMilestoneArgs.milestone.Weight.Value : 0;
                CurrentMilestoneArgs.tableRow.SetField("CurrentMilestoneDate", CurrentMilestoneArgs.date);
                CurrentMilestoneArgs.tableRow.SetField("CurrentMilestoneCode", CurrentMilestoneArgs.milestone.Code);
            }
        }
        public static bool CheckIfUserHasAccessToMilestone(Dictionary<int, Dictionary<string, string>> milestonesNotPermitted, string milestoneCode, int tenant)
        {
            if (!milestonesNotPermitted.ContainsKey(tenant))
                return true;

            var milestonesCodeNotPermitted = milestonesNotPermitted[tenant];
            if (milestonesCodeNotPermitted.ContainsKey(milestoneCode))
                return false;

            return true;
        }
    }
}
