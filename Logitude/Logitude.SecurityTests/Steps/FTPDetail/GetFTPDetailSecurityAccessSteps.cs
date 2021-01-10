using FluentAssertions;
using Logitude.SecurityTests.Models.FTPDetail;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.FTPDetail
{
    [Binding]
    public class GetFTPDetailSecurityAccessSteps
    {
        FTPDetailContext Context;
        public GetFTPDetailSecurityAccessSteps(MultiUsers multiUsers, FTPDetailContext context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"First user get the first FTP Detail from FTP Detail list")]
        public void WhenFirstUserGetTheFirstFTPDetailFromFTPDetailList()
        {
            IEnumerable<FTPDetailPM> FirstUserDetailList = GetFTPDetailsListForFirstUser();
            Context.FirstUserDetail.Id = FirstUserDetailList?.FirstOrDefault()?.Id;
        }

        [Then(@"the Detail for first user should be exists")]
        public void ThenTheDetailForFirstUserShouldBeExists()
        {
            Context.FirstUserDetail.Id.Should().NotBeNull();
        }

        [When(@"Second user get the FTP Detail that requested by first user")]
        public void WhenSecondUserGetTheFTPDetailThatRequestedByFirstUser()
        {
            IEnumerable<FTPDetailPM> FirstUserDetailList = GetFTPDetailsListForFirstUser();
            string singleDetailUrl = "ftpdetailviews/GetSingle?id=" + FirstUserDetailList?.FirstOrDefault()?.Id;
            FTPDetailPM detailPM = APICaller.CallGet<FTPDetailPM>(singleDetailUrl, Context.SecondUser.Token, null);
            Context.SecondUserDetail.Id = detailPM?.Id;
        }  
        
        [Then(@"the Detail for second user should not be exists")]
        public void ThenTheDetailForSecondUserShouldNotBeExists()
        {
            Context.SecondUserDetail.Id.Should().BeNull();
        }

        private IEnumerable<FTPDetailPM> GetFTPDetailsListForFirstUser()
        {
            string contactsListUrl = "ftpdetailviews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<FTPDetailPM> FTPDetailPMs = APICaller.CallGet<IEnumerable<FTPDetailPM>>(contactsListUrl, Context.FirstUser.Token, "Result");
            return FTPDetailPMs;
        }

    }
}
