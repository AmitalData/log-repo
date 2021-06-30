using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.CRMTests.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class CreateTaskSteps
    {

        private readonly CRMContext crmContext;
        private readonly ActivityTaskServices activityTaskServices;

        public CreateTaskSteps(CRMContext activitesContext, ActivityTaskServices taskServices)
        {
            this.crmContext = activitesContext;
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
