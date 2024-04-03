using Logitude.Base.Models.Api;
using Logitude.Base.Models.Infrastructure;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Models.Locations;
using Logitude.Base.Models.Partners;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Billings;

namespace Logitude.Base.Hooks
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

        public static void PrepareTheData(string email, string password, string url)
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
            if (configurations != null)
            {
                Settings.ServerUrl = configurations.ServerSettings.Url;
                Settings.DefaultUserCredentials = GetUserCredentialsFromConfigurations(configurations, true);
                Settings.OtherUserCredentials = GetUserCredentialsFromConfigurations(configurations, false);
                Settings.UserEmptyTenantCredentials = GetUserEmptyTenantCredentialsFromConfigurations(configurations);
            }
            Settings.ServerUrl = Environment.GetEnvironmentVariable("URL") ?? Settings.ServerUrl;
            Settings.DefaultUserCredentials = GetDefaultUserCredentialsFromEnvironmentVariable() ?? Settings.DefaultUserCredentials;
            Settings.OtherUserCredentials = GetOtherUserCredentialsFromEnvironmentVariable() ?? Settings.OtherUserCredentials;

        }

        private static Credentials GetOtherUserCredentialsFromEnvironmentVariable()
        {
            var email = Environment.GetEnvironmentVariable("OtherUserEmail");
            var password = Environment.GetEnvironmentVariable("OtherUserPassword");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            Credentials userCredentials = new Credentials
            {
                Email = email,
                Password = password
            };

            return userCredentials;
        }

        private static Credentials GetDefaultUserCredentialsFromEnvironmentVariable()
        {
            var email = Environment.GetEnvironmentVariable("DefaultUserEmail");
            var password = Environment.GetEnvironmentVariable("DefaultUserPassword");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            Credentials userCredentials = new Credentials
            {
                Email = email,
                Password = password
            };

            return userCredentials;
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
            SetupUserEmptyTenantAuthentication();
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
        private static Credentials GetUserEmptyTenantCredentialsFromConfigurations(Configurations configurations)
        {
            ConfigurationsUser userConfiguration = configurations?.Users.Where(u => u.DefaultSpecified).FirstOrDefault();
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
            UserTenant.DocumentDownloadToken = userLogin?.DocumentDownloadToken;
        }

        private static void SetupOtherUserAuthentication()
        {
            UserLogin userLogin = GetUserLogin(Settings.OtherUserCredentials);
            UserOtherTenant.Token = userLogin?.Token;
            UserOtherTenant.Tenant = userLogin == null ? 0 : userLogin.Tenant;
            UserOtherTenant.UserId = userLogin?.UserId;
            UserOtherTenant.UserName = userLogin?.UserName;
        }
        private static void SetupUserEmptyTenantAuthentication()
        {
            UserLogin userLogin = GetUserLogin(Settings.UserEmptyTenantCredentials);
            UserEmptyTenant.Token = userLogin?.Token;
            UserEmptyTenant.Tenant = userLogin == null ? 0 : userLogin.Tenant;
            UserEmptyTenant.UserId = userLogin?.UserId;
            UserEmptyTenant.UserName = userLogin?.UserName;
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
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("SearchFields")
                .Filter1Operator("Contains")
                .Filter1Value(Settings.DefaultUserCredentials.Email)
                .Build();

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
            PartnersData.CustomerContactId = partnersVariables.CustomerContactId;
        }

        private static void BillingDataMap(BillingVariables billingVariables)
        {
            BillingData.CurrencyEURId = billingVariables.CurrencyEURId;
            BillingData.CurrencyNISId = billingVariables.CurrencyNISId;
            BillingData.MeasurementGRWTId = billingVariables.MeasurementGRWTId;
            BillingData.ChargeTypeAFTId = billingVariables.ChargeTypeAFTId;
            BillingData.ChargeTypeOFTId = billingVariables.ChargeTypeOFTId;
            //BillingData.IncotermLDEId = billingVariables.IncotermLDEId;
            BillingData.VATTypeZeroId = billingVariables.VATTypeZeroId;
            BillingData.PaymentTermCashId = billingVariables.PaymentTermCashId;
           // BillingData.CreditCardTSId = billingVariables.CreditCardTSId;
        }


    }
}