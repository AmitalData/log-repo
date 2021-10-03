using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models.Builders
{
    public class TMEmployeeTimePMBuilder
    {
        private TMEmployeeTimePM _tMEmployeeTime;

        private static readonly Dictionary<string, string> lcoations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"Office", "O"},
            {"Day Off", "D"},
            {"Home", "H"},
            {"Client", "C"},
        };

        public TMEmployeeTimePMBuilder()
        {
            this.Reset();
        }

        public TMEmployeeTimePM Build()
        {
            TMEmployeeTimePM result = _tMEmployeeTime;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            _tMEmployeeTime = new TMEmployeeTimePM();
        }

        public TMEmployeeTimePMBuilder LocationCode(string locationName)
        {
            _tMEmployeeTime.LocationCode = lcoations[locationName];
            return this;
        }

        public TMEmployeeTimePMBuilder Description(string description)
        {
            _tMEmployeeTime.Description = description;
            return this;
        }

        public TMEmployeeTimePMBuilder TimeInMinutes(int timeInMinutes)
        {
            _tMEmployeeTime.TimeInMinutes = timeInMinutes;
            return this;
        }

        public TMEmployeeTimePMBuilder DateOfWork(DateTime dateOfWork)
        {
            _tMEmployeeTime.DateOfWork = dateOfWork;
            return this;
        }


        public TMEmployeeTimePMBuilder WINumber(string wINumber)
        {
            _tMEmployeeTime.WINumber = wINumber;
            return this;
        }

        public TMEmployeeTimePMBuilder WithModel(TMEmployeeTimePM tMEmployeeTime)
        {
            _tMEmployeeTime = tMEmployeeTime;
            return this;
        }

        public TMEmployeeTimePMBuilder WithDefualtValues()
        {
            _tMEmployeeTime = new TMEmployeeTimePM
            {
                EmployeeUserId = UserTenant.UserId,
                ProjectId = TimeManagementData.ProjectId,
                SprintId = TimeManagementData.SprintId,
            };
            return this;
        }

    }
}
