using FluentAssertions;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Services;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.TimeManagementTests.Steps.DataEntry
{
    [Binding]
    public class UpdateDataEntrySteps
    {
        private readonly TimeManagementContext timeManagementContext;
        private readonly DataEntryServices dataEntryServices;

        public UpdateDataEntrySteps(TimeManagementContext timeManagementContext, DataEntryServices dataEntryServices)
        {
            this.timeManagementContext = timeManagementContext;
            this.dataEntryServices = dataEntryServices;
        }
        [Given(@"a data entry")]
        public void GivenADataEntry()
        {
            string path = Urls.GetDataEntryTimeSheetList(UserTenant.UserId, "A", DateTime.Now, DateTime.Now);
            timeManagementContext.DataEntry = APICaller.CallGet<TimeManagementAPIHelper>(path, UserTenant.Token).Data;
        }

        [Given(@"following data entry properties")]
        public void GivenFollowingDataEntryProperties(Table table)
        {
            timeManagementContext.DataEntry = dataEntryServices.UpdateInstance(table, timeManagementContext.DataEntry);
        }
        
        [When(@"update data entry")]
        public void WhenUpdateDataEntry()
        {
            timeManagementContext.UpdatedDataEntry = dataEntryServices.Update(timeManagementContext.DataEntry);
        }

        [Then(@"the data entry should update successfully")]
        public void ThenTheDataEntryShouldUpdateSuccessfully()
        {
            dataEntryServices.Assert(timeManagementContext.DataEntry, timeManagementContext.UpdatedDataEntry);
        }
    }
}
