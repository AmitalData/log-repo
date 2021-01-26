using FluentAssertions;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Models;

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

        #region Step Region
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
            GetFTPDetailForTheSecondUserBaseOnFirstUserFTPDetails();
        }

        [Then(@"the Detail for second user should not be exists")]
        public void ThenTheDetailForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #region Private Function Region
        private void GetFTPDetailForTheSecondUserBaseOnFirstUserFTPDetails()
        {
            IEnumerable<FTPDetailPM> firstUserDetailList = GetFTPDetailsListForFirstUser();
            string ftpDetailsGetSingleUrl = Urls.FTPDetailsGetSingle(firstUserDetailList?.FirstOrDefault()?.Id);
            ApiResponse<FTPDetailPM> response = APICaller.CallGet<FTPDetailPM>(ftpDetailsGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        private IEnumerable<FTPDetailPM> GetFTPDetailsListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<FTPDetailPM>> response = APICaller.CallGetByFilters<IEnumerable<FTPDetailPM>>(Urls.FTPDetailViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data;
        }
        #endregion

    }
}