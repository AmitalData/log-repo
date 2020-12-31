using FluentAssertions;
using Logitude.SecurityTests.Models.Login;
using Logitude.Test.Services;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SecurityTests.Steps.Login
{
    [Binding]
    public class LoginSteps
    {
        protected readonly UserData UserData;
        protected readonly UsersData UsersData;
        protected LoginParameters LoginParameters;
        protected LoginsParameters LoginsParameters;

        public LoginSteps(UserData userData, UsersData usersData, LoginParameters loginParameters, LoginsParameters loginsParameters)
        {
            UserData = userData;
            UsersData = usersData;
            LoginParameters = loginParameters;
            LoginsParameters = loginsParameters;
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
            UserData user = APICaller.CallPost<UserData>(LoginParameters, "Authentication", null);
            if (user != null)
            {
                UserData.Token = user.Token;
                UserData.Tenant = user.Tenant;
            }
        } 

        [Then(@"User should have token")]
        public void ThenUserShouldHaveToken()
        {
            UserData.Token.Should().NotBeNull();
        }

        [Given(@"Users with following credentials")]
        public void GivenUsersWithFollowingCredentials(Table credentialsTable)
        {
            IEnumerable<UserCredential> userCredentials = credentialsTable.CreateSet<UserCredential>();
            foreach(UserCredential userCredential in userCredentials)
            {
                LoginsParameters.Logins.Add(new LoginParameters
                {
                    Email = userCredential.Email,
                    Password = userCredential.Password,
                    ClientType = "Web",
                    GetToken = true
                });
            }
        }

        [When(@"Users make login request")]
        public void WhenUsersMakeLoginRequest()
        {
            foreach(LoginParameters login in LoginsParameters.Logins)
            {
                UserData userData = APICaller.CallPost<UserData>(login, "Authentication", null);
                if (userData != null)
                {
                    UsersData.Users.Add(new UserData
                    {
                        Token = userData.Token,
                        Tenant = userData.Tenant,
                    });
                }
            }
        }

        [Then(@"Users should have token")]
        public void ThenUsersShouldHaveToken()
        {
            foreach(UserData user in UsersData.Users)
            {
                user.Token.Should().NotBeNull();
            }
        }
    }
}