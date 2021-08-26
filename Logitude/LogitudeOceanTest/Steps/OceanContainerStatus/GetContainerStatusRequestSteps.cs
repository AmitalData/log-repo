using Logitude.OceanTest.Models;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace Logitude.OceanTest.Steps.OceanContainerStatus
{
    [Binding]
    public class GetContainerStatusRequestSteps
    {
        public OceanContext _oceanContext { get; set; }
        public static int order = 0;
        public GetContainerStatusRequestSteps(OceanContext oceanContext)
        {
            _oceanContext = oceanContext;

        }
        [Given(@"waiting time is (.*) minutes")]
        public void GivenWaitingTimeIsMinutes(int p0)
        {
            while (order != 2)
            {
                Thread.Sleep(1000);
            }
            ScenarioContext.Current.Pending();
            order++;

        }

        [When(@"get container status request")]
        public void WhenGetContainerStatusRequest()
        {
            while (order != 0)
            {
                Thread.Sleep(1000);
            }
            ScenarioContext.Current.Pending();
            order++;
        }

        [When(@"the communication logs added")]
        public void WhenTheCommunicationLogsAdded()
        {
            while (order != 1)
            {
                Thread.Sleep(1000);
            }
            ScenarioContext.Current.Pending();
            order++;

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
