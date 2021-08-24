using FluentAssertions;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.TimeManagementTests.Steps.DataEntry
{
    [Binding]
    public class UpdateDataEntrySteps
    {

        private readonly TimeManagementContext timeManagementContext;
        private readonly DataEntryServices dataEntryServices;
        private TimeManagementAPIHelper updatedDataEntry;

        public UpdateDataEntrySteps(TimeManagementContext timeManagementContext, DataEntryServices dataEntryServices)
        {
            this.timeManagementContext = timeManagementContext;
            this.dataEntryServices = dataEntryServices;
        }

        [Given(@"a data entry")]
        public void GivenADataEntry()
        {
            timeManagementContext.DataEntry = TimeManagementData.DataEntry;
        }

        [Given(@"following data entry properties")]
        public void GivenFollowingDataEntryProperties(Table table)
        {
           // dataEntryServices.UpdateInstance(table, timeManagementContext.DataEntry);
        }

        [When(@"update data entry")]
        public void WhenUpdateDataEntry()
        {
            updatedDataEntry = APICaller.CallPut<TimeManagementAPIHelper>(timeManagementContext.DataEntry, Urls.TicketsController, UserTenant.Token)?.Data;
        }

        [Then(@"the data entry should update successfully")]
        public void ThenTheDataEntryShouldUpdateSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
