using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.CommonDataTests.Models.Builders;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class AddressSettingSecurityAccessSteps
    {
        private SecurityAccessStepsContext<AddressPM> Context;

        public AddressSettingSecurityAccessSteps(SecurityAccessStepsContext<AddressPM> context )
        {
            Context = context;
        }

        [When(@"Update Address Settings request sent for User's Tenant")]
        public void WhenUpdateAddressSettingsRequestSentForUserSTenant()
        {
            Context.FirstUserPMData = UpdateFirstUserAddressSettings(UserTenant.Token);
        }

        [Then(@"Address Settings should be Updated successfully")]
        public void ThenAddressSettingsShouldBeUpdatedSuccessfully()
        {
            Context.FirstUserPMData.Should().NotBeNull();
            Context.FirstUserPMData.Tenant.Should().Be(UserTenant.Tenant);
        }

        [When(@"Update Address Settings request sent for other Tenant")]
        public void WhenUpdateAddressSettingsRequestSentForOtherTenant()
        {
            Context.SecondUserPMData = UpdateFirstUserAddressSettings(UserOtherTenant.Token);
        }

        [Then(@"Address Settings should not be Updated")]
        public void ThenAddressSettingsShouldNotBeUpdated()
        {
            Context.SecondUserPMData.Should().BeNull();
        }

        private AddressPM UpdateFirstUserAddressSettings(string Token)
        {
            AddressPM FirstUserAdressSettings = GetAFirstUserAdressSettings();

            ApiResponse<AddressPM> UpdatedAddressSettings = APICaller.CallPut<AddressPM>(FirstUserAdressSettings, Urls.AddressController , Token);
            return UpdatedAddressSettings.Data;
        }

        private AddressPM GetAFirstUserAdressSettings()
        {
            AddressPM addressPM = new AddressBuilder()
                .WithDefualtValues()
                .Description("Integration Test")
                .City("New York City")
                .Name("Te")
                .AddressTypeId("M")
                .Address1("18 West 48th Street ")
                .Address2("#5B, New York3")
                .ZipCode("+001")
                .FaxNumber("asd")
                .PhoneNumber("+001598137715")
                .StateEnglishName("New York")
                .VatNumber("89898")
                .CardCode("10027")
                .CardEnglishName("Simplog LTD.")
                .Build();

            ApiResponse<AddressPM> response = APICaller.CallPost<AddressPM>(addressPM, Urls.AddressController, UserTenant.Token);
            return response.Data;
        }
    }
}