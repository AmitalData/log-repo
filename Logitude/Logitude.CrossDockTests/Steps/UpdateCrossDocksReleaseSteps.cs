using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Logitude.CrossDockTests.Steps
{
    [Binding]
    public class UpdateCrossDocksReleaseSteps
    {
        private readonly CrossDockContext crossDockContext;

        public UpdateCrossDocksReleaseSteps(CrossDockContext crossDockContext)
        {
            this.crossDockContext = crossDockContext;
        }


        [Given(@"release cross dock")]
        public void GivenReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease = APICaller.CallGet<CrossDockReleasePM>(Urls.CrossDockReleaseGetSingle(CrossDockData.CrossDockReleaseId), UserTenant.Token).Data;
        }

        [Given(@"CustomerRef1 '(.*)' and House '(.*)'")]
        public void GivenFollowingProperties(string customerRef1, string house)
        {
            crossDockContext.CrossDockRelease = new CrossDockReleaseBuilder()
               .WithModel(crossDockContext.CrossDockRelease)
               .HouseNumber(house)
               .CustomerRef1(customerRef1)
               .Build();
        }

        [When(@"update release cross dock")]
        public void WhenUpdateReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease.Id = APICaller.CallPut<CrossDockReleasePM>(crossDockContext.CrossDockRelease, Urls.CrossReleaseGetController, UserTenant.Token)?.Data?.Id;
        }

        [Then(@"the release cross dock should update successfully")]
        public void ThenTheReleaseCrossDockShouldUpdateSuccessfully()
        {
            crossDockContext.CrossDockRelease.Id.Should().NotBeNull();
        }
    }
}
