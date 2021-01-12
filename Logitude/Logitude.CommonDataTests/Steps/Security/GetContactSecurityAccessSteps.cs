
using FluentAssertions;
using Logitude.CommonDataTests.Models.Contact;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;


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
            IEnumerable<ContactPM> FirstUserContactList = GetContactsListForFirstUser();
            Context.FirstUserPMData.Id = FirstUserContactList?.FirstOrDefault()?.Id;
        }

        [Then(@"the Contact for first user should be exists")]
        public void ThenTheContactForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Second user get the contact that requested by first user")]
        public void WhenSecondUserGetTheContactThatRequestedByFirstUser()
        {
            IEnumerable<ContactPM> FirstUserContactList = GetContactsListForFirstUser();
            string singleContactUrl = "Contact/GetSingle?id=" + FirstUserContactList?.FirstOrDefault()?.Id;
            ContactPM contactPM = APICaller.CallGet<ContactPM>(singleContactUrl, Context.SecondUser.Token, null);
            Context.SecondUserPMData.Id = contactPM?.Id;
        }

        [Then(@"the Contact for second user should not be exists")]
        public void ThenTheContactForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private IEnumerable<ContactPM> GetContactsListForFirstUser()
        {
            string contactsListUrl = "contactviews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<ContactPM> contactPMs = APICaller.CallGet<IEnumerable<ContactPM>>(contactsListUrl, Context.FirstUser.Token, "Result");
            return contactPMs;
        }


    }
}