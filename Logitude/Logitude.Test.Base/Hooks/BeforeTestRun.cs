using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Models.Base;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models;

namespace Logitude.Test.Base.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun(Order = 0)]
        public static void SetupBasePreparationVariables()
        {
            GetLoginParameters();
            FillUserTenant();
            SetupLocationPreparationVariables();
            SetupPartnerPreparationVariables();

        }

        private  static void SetupLocationPreparationVariables()
        {
            var LocationVariables = APICaller.CallGet<LocationsVariables>("IntegrationTest/GetBaseLocation", UserTenant.Token);
            LocationDataMap(LocationVariables.Data);
        }
        private static void SetupPartnerPreparationVariables()
        {
            var PartnerVariables = APICaller.CallGet<PartnersVariables>("IntegrationTest/GetBasePartners", UserTenant.Token);
            PartnerDataMap(PartnerVariables.Data);
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

        private static void GetLoginParameters()
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = BaseConfigurations.Email,
                Password = BaseConfigurations.Password,
                ClientType = "Web",
                GetToken = true
            };

            APIResponse<User> user= APICaller.CallPost<User>(loginParameters, URLs.UserAuthentication, null);
            UserTenant.Token = user.Data.Token;
            UserTenant.Tenant = user.Data.Tenant;
            UserTenant.LoginUserId = user.Data.UserId;
            UserTenant.LoginUserName = user.Data.UserName;
        }

        private static void FillUserTenant()
        {
            FillTenant();
            FillUser();
        }

        private static void FillTenant()
        {
            string TenantUrl = URLs.TenantsGetSingle + UserTenant.Tenant;
            APIResponse<TenantPM> tenantPM = APICaller.CallGet<TenantPM>(TenantUrl, UserTenant.Token);

            UserTenant.Tenant = tenantPM.Data.Id;
            UserTenant.LocalCurrencyId = tenantPM.Data.CurrencyId;
            UserTenant.ProfitCurrencyId = tenantPM.Data.ProfitCurrencyId;
            UserTenant.ProfitCurrencyRate = tenantPM.Data.ProfitCurrencyRate;
        }

        private static void FillUser()
        {
            //this method is waiting the CallGetByFilter to be implemented by Abd.M
            //string UserListUrl = URLs.UserviewsGetbyfilters + BaseConfigurations.Email;
            //List<UserList> users = APICaller.CallGetByFilter<List<UserList>>(UserListUrl, UserTenant.Token, "Result");
            //var user = users.FirstOrDefault();

            UserTenant.BranchId = "1-1102";//user.BranchId;
            UserTenant.DepartmentId = "1-2988";//user.DepartmentId;
            UserTenant.BusinessUnitId = "";//user.BusinessUnitId;
        }
    }
}
