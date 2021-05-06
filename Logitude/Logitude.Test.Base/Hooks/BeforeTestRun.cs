using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Infrastructure;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.BillingsPreparation;

namespace Logitude.Test.Base.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun(Order = 0)]
        public static void SetupBeforeTestRun()
        {
            SetupBaseSettings();
            SetupUsersAuthentication();
            SetupDefaultUserTenant();
            SetupLocationPreparationVariables();
            SetupPartnerPreparationVariables();
            SetupBillingPreparationVariables();
        }

        public static void PrepareTheData(string email, string password,string url)
        {
            SetupBaseSettingsForForm(email, password, url);
            SetupDefaultUserAuthentication();
            SetupDefaultUserTenant();
            SetupLocationPreparationVariables();
            SetupPartnerPreparationVariables();
            SetupBillingPreparationVariables();
        }

        private static void SetupBaseSettings()
        {
            Configurations configurations = GetConfigurations();
            if(configurations != null)
            {
                Settings.ServerUrl = configurations.ServerSettings.Url;
                Settings.DefaultUserCredentials = GetUserCredentialsFromConfigurations(configurations, true);
                Settings.OtherUserCredentials = GetUserCredentialsFromConfigurations(configurations, false);
            }
        }

        private static void SetupBaseSettingsForForm(string email, string password, string url)
        {
            Settings.ServerUrl = url;
            Credentials userCredentials = new Credentials
            {
                Email = email,
                Password = password
            };
            Settings.DefaultUserCredentials = userCredentials;
        }

        private static void SetupUsersAuthentication()
        {
            SetupDefaultUserAuthentication();
            SetupOtherUserAuthentication();
        }

        private static void SetupDefaultUserTenant()
        {
            SetupDefaultTenant();
            SetupDefaultUser();
        }

        private static void SetupLocationPreparationVariables()
        {
            LocationsVariables locationsVariables = DataPreparation.GetLocationsVariables();
            LocationsDataMap(locationsVariables);
        }

        private static void SetupPartnerPreparationVariables()
        {
            PartnersVariables partnersVariables = DataPreparation.GetPartnersVariables();
            PartnersDataMap(partnersVariables);
        }
        public static void SetupBillingPreparationVariables()
        {
            BillingVariables BillingsVariables = DataPreparation.GetBillingVariables();
            BillingDataMap(BillingsVariables);
        }

        private static Configurations GetConfigurations()
        {
            try
            {
                string configurationsXmlFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Configurations\configurations.xml";
                string configurationsXmlString = File.ReadAllText(configurationsXmlFilePath);
                Configurations configurations = configurationsXmlString.ParseXML<Configurations>();
                return configurations;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Credentials GetUserCredentialsFromConfigurations(Configurations configurations, bool isDefaultUser)
        {
            ConfigurationsUser userConfiguration = configurations?.Users.Where(u => u.Default == isDefaultUser).FirstOrDefault();
            Credentials userCredentials = new Credentials
            {
                Email = userConfiguration?.Email,
                Password = userConfiguration?.Password
            };

            return userCredentials;
        }

        private static void SetupDefaultUserAuthentication()
        {
            UserLogin userLogin = GetUserLogin(Settings.DefaultUserCredentials);
            UserTenant.Token = userLogin?.Token;
            UserTenant.Tenant = userLogin == null ? 0 : userLogin.Tenant;
            UserTenant.UserId = userLogin?.UserId;
            UserTenant.UserName = userLogin?.UserName;
        }

        private static void SetupOtherUserAuthentication()
        {
            UserLogin userLogin = GetUserLogin(Settings.OtherUserCredentials);
            UserOtherTenant.Token = userLogin?.Token;
            UserOtherTenant.Tenant = userLogin == null ? 0 : userLogin.Tenant;
            UserOtherTenant.UserId = userLogin?.UserId;
            UserOtherTenant.UserName = userLogin?.UserName;
        }

        private static UserLogin GetUserLogin(Credentials userCredentials)
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = userCredentials.Email,
                Password = userCredentials.Password,
                ClientType = "Web",
                GetToken = true
            };
            ApiResponse<UserLogin> userLoginResponse = APICaller.CallPost<UserLogin>(loginParameters, Urls.AuthenticationController, null);
            return userLoginResponse.Data;
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

            ApiResponse<IEnumerable<User>> usersResponse = APICaller.CallGetByFilters<IEnumerable<User>>(Urls.UserViewsGetByFilters, UserTenant.Token, apiQueryFilters);

            User user = usersResponse.Data?.FirstOrDefault();
            UserTenant.BranchId = user?.BranchId;
            UserTenant.DepartmentId = user?.DepartmentId;
            UserTenant.BusinessUnitId = user?.BusinessUnitId;
        }

        private static void LocationsDataMap(LocationsVariables locationsVariables)
        {
            LocationsData.PortLHRId = locationsVariables.PortLHRId;
            LocationsData.PortLASDomesticId = locationsVariables.PortLASDomesticId;
            LocationsData.PortMIADomesticId = locationsVariables.PortMIADomesticId;
            LocationsData.PortAirJFKId = locationsVariables.PortAirJFKId;
            LocationsData.PortOceanNYCId = locationsVariables.PortOceanNYCId;
            LocationsData.PortOceanSOUId = locationsVariables.PortOceanSOUId;
            LocationsData.PortInlandNYCId = locationsVariables.PortInlandNYCId;
            LocationsData.PortLONId = locationsVariables.PortLONId;
            LocationsData.PortMANId = locationsVariables.PortMANId;
            LocationsData.CountryUSId = locationsVariables.CountryUSId;
            LocationsData.CountryGBId = locationsVariables.CountryGBId;
            LocationsData.CountryTSId = locationsVariables.CountryTSId;
            LocationsData.StateAKId = locationsVariables.StateAKId;
            LocationsData.CityAnchorageId = locationsVariables.CityAnchorageId;
            LocationsData.CityManchesterId = locationsVariables.CityManchesterId;
        }

        private static void PartnersDataMap(PartnersVariables partnersVariables)
        {
            PartnersData.VendorId = partnersVariables.VendorId;
            PartnersData.AgentId = partnersVariables.AgentId;
            PartnersData.AgentCode = partnersVariables.AgentCode;
            PartnersData.CustomerId = partnersVariables.CustomerId;
            PartnersData.CustomAgentId = partnersVariables.CustomAgentId;
            PartnersData.ShippingAgentId = partnersVariables.ShippingAgentId;
            PartnersData.PotentialCustomerId = partnersVariables.PotentialCustomerId;
            PartnersData.TruckerTLONId = partnersVariables.TruckerTLONId;
            PartnersData.TruckerTNYCId = partnersVariables.TruckerTNYCId;
            PartnersData.ShipperExportId = partnersVariables.ShipperExportId;
            PartnersData.ShipperExportCode = partnersVariables.ShipperExportCode;
            PartnersData.ShipperImportId = partnersVariables.ShipperImportId;
            PartnersData.ShipperImportCode = partnersVariables.ShipperImportCode;
            PartnersData.ConsigneeExportId = partnersVariables.ConsigneeExportId;
            PartnersData.ConsigneeImportId = partnersVariables.ConsigneeImportId;
            PartnersData.AirlineAAId = partnersVariables.AirlineAAId;
            PartnersData.AirlineBAId = partnersVariables.AirlineBAId;
            PartnersData.ShippingLineMSCUId = partnersVariables.ShippingLineMSCUId;
            PartnersData.ShippingLineMAEUId = partnersVariables.ShippingLineMAEUId;
            PartnersData.ShippingLineYMLUId = partnersVariables.ShippingLineYMLUId;
            PartnersData.WarehouseId = partnersVariables.WarehouseId;
        }

        private static void BillingDataMap(BillingVariables billingVariables)
        {
            BillingData.CurrencyEURId = billingVariables.CurrencyEURId;
            BillingData.MeasurementGRWTId = billingVariables.MeasurementGRWTId;
            BillingData.ChargeTypeAFTId = billingVariables.ChargeTypeAFTId;
        }
    }
}