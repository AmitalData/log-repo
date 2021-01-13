using FluentAssertions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.CommonDataTests.Models.FTPDetail;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class GetFTPDetailSecurityAccessSteps
    {
        private SecurityAccessStepsContext<FTPDetailPM> Context;
        public GetFTPDetailSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<FTPDetailPM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"First user get the first FTP Detail from FTP Detail list")]
        public void WhenFirstUserGetTheFirstFTPDetailFromFTPDetailList()
        {
            IEnumerable<FTPDetailPM> FirstUserDetailList = GetFTPDetailsListForFirstUser();
            Context.FirstUserPMData.Id = FirstUserDetailList?.FirstOrDefault()?.Id;
        }

        [Then(@"the Detail for first user should be exists")]
        public void ThenTheDetailForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Second user get the FTP Detail that requested by first user")]
        public void WhenSecondUserGetTheFTPDetailThatRequestedByFirstUser()
        {
            IEnumerable<FTPDetailPM> FirstUserDetailList = GetFTPDetailsListForFirstUser();
            string singleDetailUrl = "ftpdetailviews/GetSingle?id=" + FirstUserDetailList?.FirstOrDefault()?.Id;
            FTPDetailPM detailPM = APICaller.CallGet<FTPDetailPM>(singleDetailUrl, Context.SecondUser.Token, null);
            Context.SecondUserPMData.Id = detailPM?.Id;
        }

        [Then(@"the Detail for second user should not be exists")]
        public void ThenTheDetailForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private IEnumerable<FTPDetailPM> GetFTPDetailsListForFirstUser()
        {
            string contactsListUrl = "ftpdetailviews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<FTPDetailPM> FTPDetailPMs = APICaller.CallGet<IEnumerable<FTPDetailPM>>(contactsListUrl, Context.FirstUser.Token, "Result");
            return FTPDetailPMs;
        }

    }
}
