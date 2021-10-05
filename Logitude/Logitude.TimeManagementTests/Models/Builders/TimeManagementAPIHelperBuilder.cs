using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models.Builders
{
    public class TimeManagementAPIHelperBuilder
    {

        private TimeManagementAPIHelper _timeManagementAPIHelper;

        private static readonly Dictionary<string, string> lcoations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"Office", "O"},
            {"Day Off", "D"},
            {"Home", "H"},
            {"Client", "C"},
        };

        public TimeManagementAPIHelperBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _timeManagementAPIHelper = new TimeManagementAPIHelper();
        }


        public TimeManagementAPIHelperBuilder LocationCode(string locationName)
        {
            _timeManagementAPIHelper.LocationCode = lcoations[locationName];
            return this;
        }
        public TimeManagementAPIHelperBuilder WithItemsPM(List<TMEmployeeTimePM> tMEmployeeTimePMs)
        {
            _timeManagementAPIHelper.ItemsPM = tMEmployeeTimePMs;
            return this;
        }
        public TimeManagementAPIHelperBuilder ItemsPM(TMEmployeeTimePM tMEmployeeTime)
        {
            if (_timeManagementAPIHelper.ItemsPM == null)
                _timeManagementAPIHelper.ItemsPM = new List<TMEmployeeTimePM>();

            _timeManagementAPIHelper.ItemsPM.Add(tMEmployeeTime);
            return this;
        }

        public TimeManagementAPIHelper Build()
        {
            TimeManagementAPIHelper result = _timeManagementAPIHelper;
            this.Reset();
            return result;
        }

        public TimeManagementAPIHelperBuilder WithModel(TimeManagementAPIHelper timeManagementAPIHelper)
        {
            _timeManagementAPIHelper = timeManagementAPIHelper;
            return this;
        }

        public TimeManagementAPIHelperBuilder WithDefualtValues()
        {
            _timeManagementAPIHelper = new TimeManagementAPIHelper
            {
                EmployeeUserId = UserTenant.UserId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            return this;
        }



    }
}
