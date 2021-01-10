using FluentAssertions;
using Logitude.SecurityTests.Models.CompanyAddressSetting;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.AddressSetting
{
    [Binding]
    public class AddressSettingSecurityAccessSteps
    {
        protected AddressSettingSecurityAccessStepsContext Context;

        public AddressSettingSecurityAccessSteps(MultiUsers multiUsers, AddressSettingSecurityAccessStepsContext context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"Get Address Settings request sent for User's Tenant")]
        public void WhenGetAddressSettingsRequestSentForUserSTenant()
        {
            Context.FirstUserTenant = GetAddressSettings(Context.FirstUser.Tenant, Context.FirstUser.Token);
        }
        
        [When(@"Get Address Settings request sent for other Tenant")]
        public void WhenGetAddressSettingsRequestSentForOtherTenant()
        {
            Context.SecondUserTenant = GetAddressSettings(Context.FirstUser.Tenant, Context.SecondUser.Token);
        }
        
        [When(@"Update Address Settings request sent for User's Tenant")]
        public void WhenUpdateAddressSettingsRequestSentForUserSTenant()
        {
            Context.FirstUserAddressSetting = UpdateFirstUserAddressSettings(Context.FirstUser.Token);
        }

        [When(@"Update Address Settings request sent for other Tenant")]
        public void WhenUpdateAddressSettingsRequestSentForOtherTenant()
        {
            Context.SecondUserAddressSetting = UpdateFirstUserAddressSettings(Context.SecondUser.Token);
        }

        [Then(@"Address Settings should be exists")]
        public void ThenAddressSettingsShouldBeExists()
        {
            Context.FirstUserTenant.Should().NotBeNull();
            Context.FirstUserTenant.Id.Should().Be(Context.FirstUser.Tenant);
        }

        [Then(@"Address Settings should not be exists")]
        public void ThenAddressSettingsShouldNotBeExists()
        {
            Context.SecondUserTenant.Should().BeNull();
        }

        [Then(@"Address Settings should be Updated successfully")]
        public void ThenAddressSettingsShouldBeUpdatedSuccessfully()
        {
            Context.FirstUserAddressSetting.Should().NotBeNull();
            Context.FirstUserAddressSetting.Tenant.Should().Be(Context.FirstUser.Tenant);
        }
        
        [Then(@"Address Settings should not be Updated")]
        public void ThenAddressSettingsShouldNotBeUpdated()
        {
            Context.SecondUserAddressSetting.Should().BeNull();
        }

        protected TenantPM GetAddressSettings(int Tenant, string Token)
        {
            string TenantUrl = "tenants/getsingle?id=" + Tenant;
            TenantPM tenant = APICaller.CallGet<TenantPM>(TenantUrl, Token, null);
            return tenant;
        }

        protected AddressPM UpdateFirstUserAddressSettings(string Token)
        {
            AddressPM FirstUserAdressSettings = GetAFirstUserAdressSettings();

            AddressPM UpdatedAddressSettings = APICaller.CallPut<AddressPM>(FirstUserAdressSettings, "addresses", Token);
            return UpdatedAddressSettings;
        }

        protected AddressPM GetAFirstUserAdressSettings()
        {
            AddressPM AddressPM = new AddressPM
            {
                Tenant = Context.FirstUser.Tenant,
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

            return APICaller.CallPost<AddressPM>(AddressPM, "addresses", Context.FirstUser.Token);
        }
    }
}
