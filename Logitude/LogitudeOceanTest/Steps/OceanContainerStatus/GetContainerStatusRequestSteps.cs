using Logitude.OceanTest.Models;
using System;
using TechTalk.SpecFlow;

namespace Logitude.OceanTest.Steps.OceanContainerStatus
{
    [Binding]
    public class GetContainerStatusRequestSteps
    {
        public OceanContext _oceanContext { get; set; }
        public GetContainerStatusRequestSteps(OceanContext oceanContext)
        {
            _oceanContext = oceanContext;

        }
        [Given(@"waiting time is (.*) minutes")]
        public void GivenWaitingTimeIsMinutes(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"get container status request")]
        public void WhenGetContainerStatusRequest()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"the communication logs added")]
        public void WhenTheCommunicationLogsAdded()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"get request status OK")]
        public void ThenGetRequestStatusOK()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the communication logs status should be ""(.*)"" or ""(.*)""")]
        public void ThenTheCommunicationLogsStatusShouldBeOr(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        [When(@"after the communication logs added")]
        public void WhenAfterTheCommunicationLogsAdded()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the communication logs status should be or ""(.*)""")]
        public void ThenTheCommunicationLogsStatusShouldBeOr(string p0)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
