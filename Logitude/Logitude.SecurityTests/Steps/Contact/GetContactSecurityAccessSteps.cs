using FluentAssertions;
using Logitude.SecurityTests.Models.Contact;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.Contact
{
    [Binding]
    public class GetContactSecurityAccessSteps
    {
        ContactContext Context;
        public GetContactSecurityAccessSteps(MultiUsers multiUsers, ContactContext context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"First user get the first contact from contacts list")]
        public void WhenFirstUserGetTheFirstContactFromContactsList()
        {
            IEnumerable<ContactPM> FirstUserContactList = GetContactsListForFirstUser();
            Context.FirstUserContact.Id = FirstUserContactList?.FirstOrDefault()?.Id;
        }

        [Then(@"the Contact for first user should be exists")]
        public void ThenTheContactForFirstUserShouldBeExists()
        {
            Context.FirstUserContact.Id.Should().NotBeNull();
        }

        [When(@"Second user get the contact that requested by first user")]
        public void WhenSecondUserGetTheContactThatRequestedByFirstUser()
        {
            IEnumerable<ContactPM> FirstUserContactList = GetContactsListForFirstUser();
            string singleContactUrl = "Contact/GetSingle?id=" + FirstUserContactList?.FirstOrDefault()?.Id;
            ContactPM contactPM = APICaller.CallGet<ContactPM>(singleContactUrl, Context.SecondUser.Token, null);
            Context.SecondUserContact.Id = contactPM?.Id;
        }      
        
        [Then(@"the Contact for second user should not be exists")]
        public void ThenTheContactForSecondUserShouldNotBeExists()
        {
            Context.SecondUserContact.Id.Should().BeNull();
        }

        private IEnumerable<ContactPM> GetContactsListForFirstUser()
        {
            string contactsListUrl = "contactviews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<ContactPM> contactPMs = APICaller.CallGet<IEnumerable<ContactPM>>(contactsListUrl, Context.FirstUser.Token, "Result");
            return contactPMs;
        }


    }
}
