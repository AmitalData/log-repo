using FluentAssertions;
using Logitude.CommonTests.Models;
using Logitude.Base.Services;
using Logitude.Base.Context;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.UserTenantPreparation;

namespace Logitude.CommonTests.Steps.Security
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

        #region Get contact for user's tenant
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
        #endregion

        #region Get contact for other tenant
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
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues().Build();

            ApiResponse<IEnumerable<ContactPM>> response = APICaller.CallGetByFilters<IEnumerable<ContactPM>>(Urls.ContactViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}