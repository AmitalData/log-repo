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
        [When(@"get contact for user's tenant")]
        public void WhenGetContactForUsersTenant()
        {
            ContactPM userTenantContact = GetUserTenantContact();
            Context.FirstUserPMData.Id = userTenantContact?.Id;
        }

        [Then(@"contact should available")]
        public void ThenContactShouldAvailable()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"get contact for other tenant")]
        public void WhenGetContactForOtherTenant()
        {
            ContactPM otherTenantContact = GetUserTenantContactFromOtherUserTenant();
            Context.SecondUserPMData.Id = otherTenantContact?.Id;
        }

        [Then(@"contact should not available")]
        public void ThenContactShouldNotAvailable()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #region Private Function Region
        private ContactPM GetUserTenantContactFromOtherUserTenant()
        {
            ContactPM userTenantContact = GetUserTenantContact();
            string contactsGetSingleUrl = Urls.ContactsGetSingle(userTenantContact?.Id);
            ApiResponse<ContactPM> contactResponse = APICaller.CallGet<ContactPM>(contactsGetSingleUrl, UserOtherTenant.Token);
            return contactResponse.Data;
        }

        private ContactPM GetUserTenantContact()
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