using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class GetTaskSteps
    {

        private readonly CRMContext crmContext;

        public GetTaskSteps(CRMContext crmContext)
        {
            this.crmContext = crmContext;
        }

        [When(@"get task with TaskId")]
        public void WhenGetTaskWithTaskId()
        {
            crmContext.ActiviyTask = APICaller.CallGet<ActivityPM>(Urls.ActivitySingle(CRMData.ActivityTaskId), UserTenant.Token).Data;
        }
        
        [Then(@"task should be avaliable")]
        public void ThenTaskShouldBeAvaliable()
        {
            crmContext.ActiviyTask.Id.Should().NotBeNull();
        }
    }
}
