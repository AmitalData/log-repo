using FluentAssertions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Test.Base.Steps.Login
{
    [Binding]
    public class LoginSteps
    {
        protected readonly MultiUsers MultiUsers;
        protected MultiLoginParameters MultiLoginParameters;

        public LoginSteps(MultiUsers multiUsers, MultiLoginParameters multiLoginParameters)
        {
            MultiUsers = multiUsers;
            MultiLoginParameters = multiLoginParameters;
        }

        [Given(@"Users with following credentials")]
        public void GivenUsersWithFollowingCredentials(Table credentialsTable)
        {
            IEnumerable<LoginParameters> loginParameters = credentialsTable.CreateSet<LoginParameters>();
            MultiLoginParameters.Logins.AddRange(loginParameters);
            MultiLoginParameters.Logins.ForEach(login =>
            {
                login.ClientType = "Web";
                login.GetToken = true;
            });
        }

        [When(@"Users make login request")]
        public void WhenUsersMakeLoginRequest()
        {
            MultiLoginParameters.Logins.ForEach(login =>
            {
                User user = APICaller.CallPost<User>(login, "Authentication", null);
                if (user != null)
                {
                    MultiUsers.Users.Add(new User
                    {
                        Token = user.Token,
                        Tenant = user.Tenant,
                        UserId = user.UserId,
                        UserName = user.UserName,
                        InvalidToken = user.InvalidToken
                    });
                }
            });
        }

        [Then(@"Users should have token")]
        public void ThenUsersShouldHaveToken()
        {
            MultiUsers.Users.ForEach(user =>
            {
                user.Token.Should().NotBeNull();
            });
        }
    }
}