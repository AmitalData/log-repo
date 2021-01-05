using FluentAssertions;
using Logitude.SecurityTests.Models.Login;
using Logitude.Test.Services;
using System.Collections.Generic;
using System.Linq;
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
        protected readonly UserData User;

        protected LoginsParameters LoginsParameters;
        protected List<LoginParameters> UsersLoginParameters = new List<LoginParameters>();

        public LoginSteps(UserData userData, UsersData usersData, LoginParameters loginParameters, LoginsParameters loginsParameters)
        {
            UserData = userData;
            UsersData = usersData;
            LoginParameters = loginParameters;
            User = userData;
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
            userCredentials.ToList().ForEach(userCredential =>
            {
                LoginsParameters.Logins.Add(new LoginParameters
                {
                    Email = userCredential.Email,
                    Password = userCredential.Password,
                    ClientType = "Web",
                    GetToken = true
                });
            });
        }

        [When(@"Users make login request")]
        public void WhenUsersMakeLoginRequest()
        {
            LoginsParameters.Logins.ForEach(login =>
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
            });
        }

        [Then(@"Users should have token")]
        public void ThenUsersShouldHaveToken()
        {
            UsersData.Users.ForEach(user =>
            {
                user.Token.Should().NotBeNull();
            });
        }



        


        [Given(@"Email and Password for these users")]
        public void GivenEmailAndPasswordForTheseUsers(Table UsersLoginInfo)
        {
            IEnumerable<LoginParameters> usersLoginParameters = UsersLoginInfo.CreateSet<LoginParameters>();
            UsersLoginParameters.AddRange(usersLoginParameters);
            UsersLoginParameters.ForEach((usersLoginParameter) => {
                usersLoginParameter.ClientType = "Web";
                usersLoginParameter.GetToken = true;
            });
        }

        [When(@"Login API called for given users")]
        public void WhenLoginAPICalledForGivenUsers()
        {
            UsersLoginParameters.ForEach((usersLoginParameter) => {
                UserData userData = APICaller.CallPost<UserData>(usersLoginParameter, "Authentication", null);
                UsersData.ListOfUserData.Add(userData);
            });
        }

        [Then(@"All user will has a token")]
        public void ThenAllUserWillHasAToken()
        {
            UsersData.ListOfUserData.ForEach((userData) => {
                userData.Token.Should().NotBeNullOrEmpty();
            });
        }
    }
}