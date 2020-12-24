using FluentAssertions;
using Logitude.SpecFlow.Models;
using TechTalk.SpecFlow;

namespace Logitude.SpecFlow.Steps
{
    [Binding]
    public class LoginSteps
    {
        protected readonly UserData _user;
        protected LoginParameters _loginParameters;

        public LoginSteps(UserData userData, LoginParameters loginParameters)
        {
            _user = userData;
            _loginParameters = loginParameters;

            _loginParameters.ClientType = "Web";
            _loginParameters.GetToken = true;
        }

        [Given(@"The email is (.*)")]
        public void GivenTheEmailIs(string email)
        {
            _loginParameters.Email = email;
        }

        [Given(@"The password is (.*)")]
        public void GivenThePasswordIs(string password)
        {
            _loginParameters.Password = password;
        }

        [When(@"Make login")]
        public void WhenMakeLogin()
        {
            HttpRequest httpRequest = new HttpRequest("Authentication", HttpRequestType.BodyRequestType.Post, null, _loginParameters);
            UserData user = httpRequest.GetResponse<UserData>();
            if(user != null)
            {
                _user.Token = user.Token;
                _user.Tenant = user.Tenant;
            }
        }

        [Then(@"The user successfully logged in")]
        public void ThenTheUserSuccessfullyLoggedIn()
        {
            _user.Token.Should().NotBeNullOrEmpty();
        }
    }
}