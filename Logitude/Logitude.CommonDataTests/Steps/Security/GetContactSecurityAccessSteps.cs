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

        #region Step Region
        [When(@"First user get the first contact from contacts list")]
        public void WhenFirstUserGetTheFirstContactFromContactsList()
        {
            ContactPM firstUserContact = GeAContactsFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserContact?.Id;
        }

        [Then(@"the Contact for first user should be exists")]
        public void ThenTheContactForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Second user get the contact that requested by first user")]
        public void WhenSecondUserGetTheContactThatRequestedByFirstUser()
        {
            ApiResponse<ContactPM> response = GetAContactFornFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the Contact for second user should not be exists")]
        public void ThenTheContactForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #region Private Function Region
        private ApiResponse<ContactPM> GetAContactFornFirstUser(string Token)
        {
            ContactPM firstUserContact = GeAContactsFromFirstUserList();
            string contactsGetSingleUrl = Urls.ContactsGetSingle(firstUserContact?.Id);
             return APICaller.CallGet<ContactPM>(contactsGetSingleUrl, Token);
        }

        private ContactPM GeAContactsFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<ContactPM>> response = APICaller.CallGetByFilters<IEnumerable<ContactPM>>(Urls.ContactViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}