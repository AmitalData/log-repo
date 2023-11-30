using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class GetGlAccountNotesByCardSteps
    {
        private List<object> data;
        
        [When(@"get gl account notes by card")]
        public void WhenGetGlAccountNotesByCard()
        {
            data =  APICaller.CallGet<List<object>>(Urls.GetNotesByCard(FullAccountingData.CustomerCardId), UserOtherTenant.Token).Data;
        }

        [Then(@"the get notes by card API should return you have no permissions")]
        public void ThenTheGetNotesByCardAPIShouldReturnYouHaveNoPermissions()
        {
            data.Should().BeEmpty();
        }
    }
}
