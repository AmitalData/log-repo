using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Services;
using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using FluentAssertions;

namespace Logitude.TimeManagementTests.Steps.DataEntry
{
    [Binding]
    public class CreateDataEntrySteps
    {

        private readonly TimeManagementContext timeManagementContext;
        private readonly DataEntryServices dataEntryServices;

        public CreateDataEntrySteps(TimeManagementContext timeManagementContext, DataEntryServices dataEntryServices)
        {
            this.timeManagementContext = timeManagementContext;
            this.dataEntryServices = dataEntryServices;
        }

        [Given(@"a data entry with the following properties")]
        public void GivenADataEntryWithTheFollowingProperties(Table table)
        {
            timeManagementContext.DataEntry = dataEntryServices.CreateInstance(table);
        }

        [When(@"create data entry")]
        public void WhenCreateDataEntry()
        {
            timeManagementContext.DataEntry = APICaller.CallPut<TimeManagementAPIHelper>(timeManagementContext.DataEntry, Urls.TimeManagementDomainController, UserTenant.Token)?.Data;
        }

        [Then(@"the data entry should create successfully")]
        public void ThenTheDataEntryShouldCreateSuccessfully()
        {
            timeManagementContext.DataEntry.Should().NotBeNull();
        }
    }
}
