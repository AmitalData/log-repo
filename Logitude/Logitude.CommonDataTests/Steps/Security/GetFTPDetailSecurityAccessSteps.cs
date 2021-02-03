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
        [When(@"get FTP detail for user's tenant")]
        public void WhenGetFTPDetailForUsersTenant()
        {
            FTPDetailPM userTenantFTPDetail = GetUserTenantFTPDetail();
            Context.FirstUserPMData.Id = userTenantFTPDetail?.Id;
        }

        [Then(@"FTP detail should available")]
        public void ThenFTPDetailShouldAvailable()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"get FTP detail for other tenant")]
        public void WhenGetFTPDetailForOtherTenant()
        {
            FTPDetailPM otherTenantFTPDetail = GetUserTenantContactFromOtherUserTenant();
            Context.SecondUserPMData.Id = otherTenantFTPDetail?.Id;
        }

        [Then(@"FTP detail should not available")]
        public void ThenFTPDetailShouldNotAvailable()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #region Private Function Region
        private FTPDetailPM GetUserTenantContactFromOtherUserTenant()
        {
            FTPDetailPM userTenantFTPDetail = GetUserTenantFTPDetail();
            string ftpDetailsGetSingleUrl = Urls.FTPDetailsGetSingle(userTenantFTPDetail?.Id);
            ApiResponse<FTPDetailPM> ftpDetailResponse = APICaller.CallGet<FTPDetailPM>(ftpDetailsGetSingleUrl, UserOtherTenant.Token);
            return ftpDetailResponse.Data;
        }

        private FTPDetailPM GetUserTenantFTPDetail()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<FTPDetailPM>> response = APICaller.CallGetByFilters<IEnumerable<FTPDetailPM>>(Urls.FTPDetailViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}