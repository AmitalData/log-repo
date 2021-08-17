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
        private TimeSheetItem _timeSheetItem;

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
        public TimeSheetItemBuilder LocationCode(string locationName)
        {
            _timeSheetItem.LocationCode = lcoations[locationName];
            return this;
        }

        public TimeSheetItemBuilder Day(TimeSheetItemDay timeSheetItemDay)
        {
            if (_timeSheetItem.Days == null)
                _timeSheetItem.Days = new List<TimeSheetItemDay>();

            _timeSheetItem.Days.Add(timeSheetItemDay);
            return this;
        }

        public TimeSheetItemBuilder Description(string description)
        {
            _timeSheetItem.Description = description;
            return this;
        }

        public TimeSheetItemBuilder WINumber(string wINumber)
        {
            _timeSheetItem.WINumber = wINumber;
            return this;
        }

        public TimeSheetItem Build()
        {
            TimeSheetItem result = _timeSheetItem;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            _timeSheetItem = new TimeSheetItem();
        }

        public TimeSheetItemBuilder WithModel(TimeSheetItem timeSheetItem)
        {
            _timeSheetItem = timeSheetItem;
            return this;
        }

        public TimeSheetItemBuilder WithDefualtValues()
        {
            _timeSheetItem = new TimeSheetItem
            {
                EmployeeUserId = UserTenant.UserId,
                ProjectId = TimeManagementData.ProjectId
            };
            return this;
        }
    }
}
