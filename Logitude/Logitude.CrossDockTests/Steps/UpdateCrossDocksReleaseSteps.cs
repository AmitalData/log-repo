using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
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

        [Given(@"CustomerRef1 '(.*)' and House '(.*)'")]
        public void GivenFollowingProperties(string customerRef1, string house)
        {
            crossDockContext.House = house;
            crossDockContext.CustomerRef1 = customerRef1;
        }

        [Given(@"release cross dock")]
        public void GivenReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease = APICaller.CallGet<CrossDockReleasePM>(Urls.CrossDockReleaseGetSingle(CrossDockReleaseData.Id), UserTenant.Token).Data;
        }

        [When(@"update release cross dock")]
        public void WhenUpdateReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease = new CrossDockReleaseBuilder()
            .WithModel(crossDockContext.CrossDockRelease)
            .HouseNumber(crossDockContext.House)
            .CustomerRef1(crossDockContext.CustomerRef1).Build();

            crossDockContext.CrossDockRelease.Id = APICaller.CallPut<CrossDockReleasePM>(crossDockContext.CrossDockRelease, Urls.CrossReleaseGetController, UserTenant.Token)?.Data?.Id;
        }

        [Then(@"the release cross dock should update successfully")]
        public void ThenTheReleaseCrossDockShouldUpdateSuccessfully()
        {
            crossDockContext.CrossDockRelease.Id.Should().NotBeNull();
        }
    }
}
