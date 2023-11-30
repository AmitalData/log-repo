using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.CRMTests.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class UpdateTaskSteps
    {

        private readonly CRMContext crmContext;
        private readonly ActivityTaskServices activityTaskServices;

        public UpdateTaskSteps(CRMContext crmContext, ActivityTaskServices taskServices)
        {
            this.crmContext = crmContext;
            this.activityTaskServices = taskServices;
        }

        [Given(@"task")]
        public void GivenTask()
        {
            crmContext.ActiviyTask = APICaller.CallGet<ActivityPM>(Urls.ActivitySingle(CRMData.ActivityTaskId), UserTenant.Token).Data;
        }

        [Given(@"following task properties")]
        public void GivenFollowingTaskProperties(Table table)
        {
            crmContext.ActiviyTask = activityTaskServices.UpdateInstance(table, crmContext.ActiviyTask);
        }
        
        [When(@"update task")]
        public void WhenUpdateTask()
        {
            crmContext.ActiviyTask = APICaller.CallPut<ActivityPM>(crmContext.ActiviyTask, Urls.ActivitiesController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the task should update successfully")]
        public void ThenTheTaskShouldUpdateSuccessfully()
        {
            crmContext.ActiviyTask.Id.Should().NotBeNull();
        }
    }
}
