using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Constants;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class GetContactSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ContactPM> Context;

        public GetContactSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<ContactPM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"First user get the first contact from contacts list")]
        public void WhenFirstUserGetTheFirstContactFromContactsList()
        {
            IEnumerable<ContactPM> firstUserContactList = GetContactsListForFirstUser();
            Context.FirstUserPMData.Id = firstUserContactList?.FirstOrDefault()?.Id;
        }

        [Then(@"the Contact for first user should be exists")]
        public void ThenTheContactForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Second user get the contact that requested by first user")]
        public void WhenSecondUserGetTheContactThatRequestedByFirstUser()
        {
            IEnumerable<ContactPM> firstUserContactList = GetContactsListForFirstUser();
            string contactsGetSingleUrl = URLs.ContactsGetSingle(firstUserContactList?.FirstOrDefault()?.Id);
            APIResponse<ContactPM> response = APICaller.CallGet<ContactPM>(contactsGetSingleUrl, Context.SecondUser.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the Contact for second user should not be exists")]
        public void ThenTheContactForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private IEnumerable<ContactPM> GetContactsListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            APIResponse<IEnumerable<ContactPM>> response = APICaller.CallGetByFilters<IEnumerable<ContactPM>>(URLs.ContactViewsGetByFilters(), Context.FirstUser.Token, apiQueryFilters);
            return response.Data;
        }
    }
}