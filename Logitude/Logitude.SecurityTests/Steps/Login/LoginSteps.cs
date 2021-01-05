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
        protected readonly UsersData UsersData;
        protected LoginsParameters LoginsParameters;

        public LoginSteps(UsersData usersData, LoginsParameters loginsParameters)
        {
            UsersData = usersData;
            LoginsParameters = loginsParameters;
        }

        [Given(@"Users with following credentials")]
        public void GivenUsersWithFollowingCredentials(Table credentialsTable)
        {
            IEnumerable<LoginParameters> loginParameters = credentialsTable.CreateSet<LoginParameters>();
            LoginsParameters.Logins.AddRange(loginParameters);
            LoginsParameters.Logins.ForEach(login =>
            {
                login.ClientType = "Web";
                login.GetToken = true;
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
                        UserId = userData.UserId,
                        UserName = userData.UserName
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
    }
}