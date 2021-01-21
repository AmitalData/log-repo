using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class GetContactSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ContactPM> Context;
        public GetContactSecurityAccessSteps(SecurityAccessStepsContext<ContactPM> context)
        {
            Context = context;
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
            GetContactForTheSecondUserBaseOnFirstUserContacts();
        }

        [Then(@"the Contact for second user should not be exists")]
        public void ThenTheContactForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private void GetContactForTheSecondUserBaseOnFirstUserContacts()
        {
            IEnumerable<ContactPM> firstUserContactList = GetContactsListForFirstUser();
            string contactsGetSingleUrl = Urls.ContactsGetSingle(firstUserContactList?.FirstOrDefault()?.Id);
            ApiResponse<ContactPM> response = APICaller.CallGet<ContactPM>(contactsGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        private IEnumerable<ContactPM> GetContactsListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<ContactPM>> response = APICaller.CallGetByFilters<IEnumerable<ContactPM>>(Urls.ContactViewsGetByFilters(), UserTenant.Token, apiQueryFilters);
            return response.Data;
        }
    }
}