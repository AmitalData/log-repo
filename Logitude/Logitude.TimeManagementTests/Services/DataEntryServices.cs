using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.TimeManagementTests.Services
{
    public class DataEntryServices
    {

        public TimeManagementAPIHelper CreateInstance(Table table)
        {
            dynamic dataTable = table.CreateDynamicInstance();
            return new TimeManagementAPIHelperBuilder()
                .WithDefualtValues()
                .LocationCode((string)dataTable.Location)
                .ItemsPM(GetTMEmployeeTime(dataTable))
                .Build();
        }

        private TMEmployeeTimePM GetTMEmployeeTime(dynamic dataTable)
        {
            return new TMEmployeeTimePMBuilder().WithDefualtValues()
                .LocationCode((string)dataTable.Location)
                .Description((string)dataTable.Description)
                .TimeInMinutes((int)dataTable.Minuts)
                .DateOfWork(DateTime.Now)
                .Build();
        }

    }
}
