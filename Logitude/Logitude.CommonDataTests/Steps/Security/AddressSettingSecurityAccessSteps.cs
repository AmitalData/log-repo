
using FluentAssertions;
using Logitude.CommonDataTests.Models.CompanyAddressSetting;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class AddressSettingSecurityAccessSteps
    {
        protected SecurityAccessStepsContext<AddressPM> Context;
        protected SecurityAccessStepsContext<TenantPM> TanentContext;
        public AddressSettingSecurityAccessSteps(SecurityAccessStepsContext<AddressPM> context ,SecurityAccessStepsContext<TenantPM> tanentContext)
        {
            Context = context;
            TanentContext = tanentContext;
        }

        [When(@"Get Address Settings request sent for User's Tenant")]
        public void WhenGetAddressSettingsRequestSentForUserSTenant()
        {
            TanentContext.FirstUserPMData = GetAddressSettings(UserTenant.Tenant, UserTenant.Token);
        }

        [When(@"Get Address Settings request sent for other Tenant")]
        public void WhenGetAddressSettingsRequestSentForOtherTenant()
        {
            TanentContext.SecondUserPMData = GetAddressSettings(UserTenant.Tenant, UserOtherTenant.Token);
        }

        [When(@"Update Address Settings request sent for User's Tenant")]
        public void WhenUpdateAddressSettingsRequestSentForUserSTenant()
        {
            Context.FirstUserPMData = UpdateFirstUserAddressSettings(UserTenant.Token);
        }

        [When(@"Update Address Settings request sent for other Tenant")]
        public void WhenUpdateAddressSettingsRequestSentForOtherTenant()
        {
            Context.SecondUserPMData = UpdateFirstUserAddressSettings(UserOtherTenant.Token);
        }

        [Then(@"Address Settings should be exists")]
        public void ThenAddressSettingsShouldBeExists()
        {
            TanentContext.FirstUserPMData.Should().NotBeNull();
            TanentContext.FirstUserPMData.Id.Should().Be(UserTenant.Tenant);
        }

        [Then(@"Address Settings should not be exists")]
        public void ThenAddressSettingsShouldNotBeExists()
        {
            TanentContext.SecondUserPMData.Should().BeNull();
        }

        [Then(@"Address Settings should be Updated successfully")]
        public void ThenAddressSettingsShouldBeUpdatedSuccessfully()
        {
            Context.FirstUserPMData.Should().NotBeNull();
            Context.FirstUserPMData.Tenant.Should().Be(UserTenant.Tenant);
        }

        [Then(@"Address Settings should not be Updated")]
        public void ThenAddressSettingsShouldNotBeUpdated()
        {
            Context.SecondUserPMData.Should().BeNull();
        }

        protected TenantPM GetAddressSettings(int Tenant, string Token)
        {
            string TenantUrl = "tenants/getsingle?id=" + Tenant;
            var tenant = APICaller.CallGet<TenantPM>(TenantUrl, Token);
            return tenant.Data;
        }

        protected AddressPM UpdateFirstUserAddressSettings(string Token)
        {
            AddressPM FirstUserAdressSettings = GetAFirstUserAdressSettings();

            var UpdatedAddressSettings = APICaller.CallPut<AddressPM>(FirstUserAdressSettings, "addresses", Token);
            return UpdatedAddressSettings.Data;
        }

        protected AddressPM GetAFirstUserAdressSettings()
        {
            AddressPM AddressPM = new AddressPM
            {
                Tenant = UserTenant.Tenant,
                AgentId = "1-140040",
                CurrencyId = "1-4319",
                Description = "Integration Test",
                City = "New York City",
                Name = "Te",
                AddressTypeId = "M",
                Address1 = "18 West 48th Street ",
                Address2 = "#5B, New York3",
                CountryId = "1-397",
                StateId = "1-89",
                ZipCode = "+001",
                FaxNumber = "asd",
                PhoneNumber = "+001598137715",
                CountryCode = "US",
                CountryName = "United States of America",
                CountryEnglishName = "United States of America",
                StateEnglishName = "New York",
                StateCode = "NY",
                SearchFields = "Te,18 West 48th Street ,#5B, New York3,+001,New York City,United States of America",
                VatNumber = "89898",
                CardCode = "10027",
                CardEnglishName = "Simplog LTD.",
                HasStates = true,
                IsStateRequired = true,
            };

            var response = APICaller.CallPost<AddressPM>(AddressPM, "addresses", UserTenant.Token);
            return response.Data;
        }
    }
}
