using FluentAssertions;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using Logitude.TimeManagementTests.Models;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.TimeManagementTests.Steps.DataEntry
{
    [Binding]
    public class GetDataEntrySteps
    {
        private readonly TimeManagementContext timeManagementContext;

        public GetDataEntrySteps(TimeManagementContext timeManagementContext)
        {
            this.timeManagementContext = timeManagementContext;
        }

        [When(@"get data entry")]
        public void WhenGetDataEntry()
        {
            string path = Urls.GetDataEntryTimeSheetList(TimeManagementData.DataEntry.EmployeeUserId, TimeManagementData.DataEntry.LocationCode, DateTime.Now, DateTime.Now);
            timeManagementContext.DataEntry = APICaller.CallGet<TimeManagementAPIHelper>(path, UserTenant.Token).Data;
        }

        [Then(@"data entry should be avaliable")]
        public void ThenDataEntryShouldBeAvaliable()
        {
            timeManagementContext.DataEntry.LocationCode.Should().NotBeNull();
            timeManagementContext.DataEntry.EmployeeUserId.Should().NotBeNull();
        }
    }
}
