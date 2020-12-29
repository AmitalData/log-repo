using FluentAssertions;
using Logitude.SpecFlow.Models;
using Logitude.SpecFlow.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SpecFlow.Steps
{
    [Binding]
    public class LoginSteps
    {
        protected readonly UserData User;
        protected LoginParameters LoginParameters;

        public LoginSteps(UserData userData, LoginParameters loginParameters)
        {
            User = userData;
            LoginParameters = loginParameters;  
        }

        [Given(@"user have the following Login Properties")]
        public void GivenUserHaveTheFollowingLoginProperties(Table loginInfo)
        {
            LoginInfo myLoginInfo = loginInfo.CreateInstance<LoginInfo>();
            LoginParameters.ClientType = "Web";
            LoginParameters.GetToken = true;
            LoginParameters.Email = myLoginInfo.Email;
            LoginParameters.Password = myLoginInfo.Password;
        }

        [When(@"the user call Login API")]
        public void WhenTheUserCallLoginAPI()
        {
            UserData userData = APICaller.CallPost<UserData>(LoginParameters, "Authentication", "");
            if (userData != null)
            {
                User.Token = userData.Token;
                User.Tenant = userData.Tenant;
            }
        } 

        [Then(@"the user will have a token")]
        public void ThenTheUserWillHaveAToken()
        {
            User.Token.Should().NotBeNullOrEmpty();
        }
    }
}