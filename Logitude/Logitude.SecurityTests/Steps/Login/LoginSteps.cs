using FluentAssertions;
using Logitude.SecurityTests.Models.Login;
using Logitude.Test.Services;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.Login
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

        [Given(@"User email is (.*) and password is (.*)")]
        public void GivenUserEmailAndPassword(string email, string password)
        {
            LoginParameters.Email = email;
            LoginParameters.Password = password;
            LoginParameters.ClientType = "Web";
            LoginParameters.GetToken = true;
        }

        [When(@"User make login request")]
        public void WhenUserMakeLoginRequest()
        {
            UserData userData = APICaller.CallPost<UserData>(LoginParameters, "Authentication", null);
            if (userData != null)
            {
                User.Token = userData.Token;
                User.Tenant = userData.Tenant;
            }
        } 

        [Then(@"User should have token")]
        public void ThenUserShouldHaveToken()
        {
            User.Token.Should().NotBeNullOrEmpty();
        }
    }
}