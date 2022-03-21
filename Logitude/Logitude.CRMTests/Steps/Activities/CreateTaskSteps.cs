using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.CRMTests.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class CreateTaskSteps
    {

        private readonly CRMContext crmContext;
        private readonly ActivityTaskServices activityTaskServices;

        public CreateTaskSteps(CRMContext crmContext, ActivityTaskServices taskServices)
        {
            this.crmContext = crmContext;
            this.activityTaskServices = taskServices;
        }

        [Given(@"a task with the following properties")]
        public void GivenATaskWithTheFollowingProperties(Table table)
        {
            crmContext.ActiviyTask = activityTaskServices.CreateInstance(table);
        }
        
        [When(@"create task")]
        public void WhenCreateTask()
        {
            crmContext.ActiviyTask = APICaller.CallPost<ActivityPM>(crmContext.ActiviyTask, Urls.ActivitiesController, UserTenant.Token)?.Data;
        }

        [Then(@"the task should create successfully")]
        public void ThenTheTaskShouldCreateSuccessfully()
        {
            crmContext.ActiviyTask.Id.Should().NotBeNull();
        }
    }
}
