using FluentAssertions;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Constants;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class GetFTPDetailSecurityAccessSteps
    {
        private SecurityAccessStepsContext<FTPDetailPM> Context;
        public GetFTPDetailSecurityAccessSteps(SecurityAccessStepsContext<FTPDetailPM> context)
        {
            Context = context;
        }

        [When(@"First user get the first FTP Detail from FTP Detail list")]
        public void WhenFirstUserGetTheFirstFTPDetailFromFTPDetailList()
        {
            IEnumerable<FTPDetailPM> firstUserDetailList = GetFTPDetailsListForFirstUser();
            Context.FirstUserPMData.Id = firstUserDetailList?.FirstOrDefault()?.Id;
        }

        [Then(@"the Detail for first user should be exists")]
        public void ThenTheDetailForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Second user get the FTP Detail that requested by first user")]
        public void WhenSecondUserGetTheFTPDetailThatRequestedByFirstUser()
        {
            IEnumerable<FTPDetailPM> firstUserDetailList = GetFTPDetailsListForFirstUser();
            string ftpDetailsGetSingleUrl = URLs.FTPDetailsGetSingle(firstUserDetailList?.FirstOrDefault()?.Id);
            APIResponse<FTPDetailPM> response = APICaller.CallGet<FTPDetailPM>(ftpDetailsGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the Detail for second user should not be exists")]
        public void ThenTheDetailForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private IEnumerable<FTPDetailPM> GetFTPDetailsListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            APIResponse<IEnumerable<FTPDetailPM>> response = APICaller.CallGetByFilters<IEnumerable<FTPDetailPM>>(URLs.FTPDetailViewsGetByFilters(), Context.FirstUser.Token, apiQueryFilters);
            return response.Data;
        }

    }
}