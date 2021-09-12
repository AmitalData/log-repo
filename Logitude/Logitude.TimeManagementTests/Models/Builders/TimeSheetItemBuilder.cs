using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models.Builders
{
    public class TimeSheetItemBuilder
    {
        private TimeSheetItem timeSheetItem;

        private static readonly Dictionary<string, string> lcoations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"Office", "O"},
            {"Day Off", "D"},
            {"Home", "H"},
            {"Client", "C"},
        };

        public TimeSheetItemBuilder()
        {
            this.Reset();
        }

        public TimeSheetItem Build()
        {
            TimeSheetItem result = timeSheetItem;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            timeSheetItem = new TimeSheetItem();
        }

        public TimeSheetItemBuilder LocationCode(string locationName)
        {
            timeSheetItem.LocationCode = lcoations[locationName];
            return this;
        }
        public TimeSheetItemBuilder Days(List<TimeSheetItemDay> days)
        {
            timeSheetItem.Days = days;
            return this;
        }

        public TimeSheetItemBuilder Description(string description)
        {
            timeSheetItem.Description = description;
            return this;
        }

        public TimeSheetItemBuilder WINumber(string wINumber)
        {
            timeSheetItem.WINumber = wINumber;
            return this;
        }
        public TimeSheetItemBuilder ProjectId(string projectId)
        {
            timeSheetItem.ProjectId = projectId;
            return this;
        }
        

        public TimeSheetItemBuilder WithModel(TimeSheetItem tMEmployeeTime)
        {
            timeSheetItem = tMEmployeeTime;
            return this;
        }

        public TimeSheetItemBuilder WithDefualtValues()
        {
            timeSheetItem = new TimeSheetItem
            {
                EmployeeUserId = UserTenant.UserId,
                ProjectId = TimeManagementData.ProjectId,
            };
            return this;
        }

    }
}
