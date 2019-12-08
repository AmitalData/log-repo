using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core.Login
{
    public class LoginService
    {
        public static async Task GetLoginTokenByUserEmailAndTenant()
        {
            int tenant = IntegrationTestLoginParameters.Tenant;
            var token = IntegrationTestLoginParameters.Token;
            if (string.IsNullOrEmpty(IntegrationTestLoginParameters.Token))
            {
                LoginParameters loginParameters = new LoginParameters()
                {
                    Email = IntegrationTestLoginParameters.Email,
                    Password = IntegrationTestLoginParameters.Password,
                    ByToken = false,
                    CardId = null,
                    CardType = null,
                    IsMobileLogin = false,
                    IsUser = true,
                    GetToken = true,
                    IsAngularLogin = true,
                    ClientType = "Web",
                };

                HttpResponseMessage httpResponseMessage = await RestClientService.PostAsync(loginParameters, "Authentication");
                var stringResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                UserData userData = JsonConvert.DeserializeObject<UserData>(stringResult);
                Assert.IsNotNull(userData.Token);
                IntegrationTestLoginParameters.Token = token = userData.Token;
            }
        }

    }

    public class LoginParameters
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ByToken { get; set; }
        public string CardId { get; set; }
        public string CardType { get; set; }
        public bool IsMobileLogin { get; set; }
        public bool IsUser { get; set; }
        public bool GetToken { get; set; }
        public bool IsAngularLogin { get; set; }
        public string ClientType { get; set; }
    }
}
