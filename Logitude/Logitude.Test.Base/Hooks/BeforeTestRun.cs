using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.Test.Base.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun(Order = 0)]
        public static void SetupBeforeTestRun()
        {
            ReadConfigurations();
            AuthenticateUsers();
            SetupDefaultUserTenant();
            SetupLocationPreparationVariables();
            SetupPartnerPreparationVariables();
        }

        private static void ReadConfigurations()
        {
            string configurationsXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Configurations\configurations.xml";
            string configurationsXmlString = File.ReadAllText(configurationsXmlFilePath);
            Configurations configurations = configurationsXmlString.ParseXML<Configurations>();

            Settings.ServerUrl = configurations.General.ServerUrl;

            ConfigurationsUser defaultUserConfiguration = configurations.Users.Where(u => u.Default).FirstOrDefault();
            ConfigurationsUser otherUserConfiguration = configurations.Users.Where(u => !u.Default).FirstOrDefault();

            Settings.DefaultUserCredentials = new Credentials
            {
                Email = defaultUserConfiguration?.Email,
                Password = defaultUserConfiguration?.Password
            };

            Settings.OtherUserCredentials = new Credentials
            {
                Email = otherUserConfiguration?.Email,
                Password = otherUserConfiguration?.Password
            };
        }

        private static void AuthenticateUsers()
        {
            AuthenticateDefaultUser();
            AuthenticateOtherUser();
        }

        private static void SetupDefaultUserTenant()
        {
            SetupDefaultTenant();
            SetupDefaultUser();
        }

        private static void AuthenticateDefaultUser()
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = Settings.DefaultUserCredentials.Email,
                Password = Settings.DefaultUserCredentials.Password,
                ClientType = "Web",
                GetToken = true
            };

            ApiResponse<UserLogin> userLoginResponse = APICaller.CallPost<UserLogin>(loginParameters, Urls.UserAuthentication(), null);
            UserTenant.Token = userLoginResponse.Data?.Token;
            UserTenant.Tenant = userLoginResponse.Data == null ? 0 : userLoginResponse.Data.Tenant;
            UserTenant.UserId = userLoginResponse.Data?.UserId;
            UserTenant.UserName = userLoginResponse.Data?.UserName;
        }

        private static void AuthenticateOtherUser()
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = Settings.OtherUserCredentials.Email,
                Password = Settings.OtherUserCredentials.Password,
                ClientType = "Web",
                GetToken = true
            };

            ApiResponse<UserLogin> userLoginResponse = APICaller.CallPost<UserLogin>(loginParameters, Urls.UserAuthentication(), null);
            UserOtherTenant.Token = userLoginResponse.Data?.Token;
            UserOtherTenant.Tenant = userLoginResponse.Data == null ? 0 : userLoginResponse.Data.Tenant;
            UserOtherTenant.UserId = userLoginResponse.Data?.UserId;
            UserOtherTenant.UserName = userLoginResponse.Data?.UserName;
        }

        private static void SetupDefaultTenant()
        {
            string tenantUrl = Urls.TenantsGetSingle(UserTenant.Tenant);
            ApiResponse<Tenant> tenantResponse = APICaller.CallGet<Tenant>(tenantUrl, UserTenant.Token);

            UserTenant.LocalCurrencyId = tenantResponse.Data?.CurrencyId;
            UserTenant.ProfitCurrencyId = tenantResponse.Data?.ProfitCurrencyId;
            UserTenant.ProfitCurrencyRate = tenantResponse.Data?.ProfitCurrencyRate;
        }

        private static void SetupDefaultUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "SearchFields",
                Filter1Operator = "Contains",
                Filter1Value = Settings.DefaultUserCredentials.Email
            };

            ApiResponse<IEnumerable<User>> usersResponse = APICaller.CallGetByFilters<IEnumerable<User>>(Urls.UserViewsGetByFilters(), UserTenant.Token, apiQueryFilters);
            User user = usersResponse.Data?.FirstOrDefault();

            UserTenant.BranchId = user?.BranchId;
            UserTenant.DepartmentId = user?.DepartmentId;
            UserTenant.BusinessUnitId = user?.BusinessUnitId;
        }

        private static void SetupLocationPreparationVariables()
        {
            ApiResponse<LocationsVariables> locationsVariablesResponse = APICaller.CallGet<LocationsVariables>(Urls.IntegrationTestGetBaseLocations(), UserTenant.Token);
            LocationDataMap(locationsVariablesResponse.Data);
        }

        private static void SetupPartnerPreparationVariables()
        {
            ApiResponse<PartnersVariables> partnersVariablesResponse = APICaller.CallGet<PartnersVariables>(Urls.IntegrationTestGetBasePartners(), UserTenant.Token);
            PartnerDataMap(partnersVariablesResponse.Data);
        }

        private static void LocationDataMap(LocationsVariables vars)
        {
            LocationsData.PortLHRId = vars.PortLHRId;
            LocationsData.PortLASId = vars.PortLASId;
            LocationsData.PortMIAId = vars.PortMIAId;
            LocationsData.PortJFKId = vars.PortJFKId;
            LocationsData.PortSOUId = vars.PortSOUId;
            LocationsData.PortNYCId = vars.PortNYCId;
            LocationsData.PortLONId = vars.PortLONId;
            LocationsData.PortMANId = vars.PortMANId;
            LocationsData.GlobalZoneEUId = vars.GlobalZoneEUId;
            LocationsData.CountryUSId = vars.CountryUSId;
            LocationsData.StateAKId = vars.StateAKId;
        }

        private static void PartnerDataMap(PartnersVariables vars)
        {
            PartnersData.CountryGBId = vars.CountryGBId;
            PartnersData.CountryUSId = vars.CountryUSId;
            PartnersData.VendorId = vars.VendorId;
            PartnersData.AgentId = vars.AgentId;
            PartnersData.CustomerId = vars.CustomerId;
            PartnersData.CustomAgentId = vars.CustomAgentId;
            PartnersData.ShippingAgentId = vars.ShippingAgentId;
            PartnersData.PotentialCustomerId = vars.PotentialCustomerId;
            PartnersData.TruckerId = vars.TruckerId;
            PartnersData.ShipperExport1 = vars.ShipperExport1;
            PartnersData.AirlineAAId = vars.AirlineAAId;
            PartnersData.AirlineBAId = vars.AirlineBAId;
            PartnersData.ShippingLineMSCUId = vars.ShippingLineMSCUId;
            PartnersData.ShippingLineMAEUId = vars.ShippingLineMAEUId;
            PartnersData.WarehouseId = vars.WarehouseId;
        }
    }
}