using FluentAssertions;
using Logitude.CommonTests.Models;
using Logitude.CommonTests.Models.Builders;
using Logitude.Base.Context;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CommonTests.Steps.Security
{
    [Binding]
    public class AddressSettingSecurityAccessSteps
    {
        private SecurityAccessStepsContext<AddressPM> Context;

        public AddressSettingSecurityAccessSteps(SecurityAccessStepsContext<AddressPM> context )
        {
            Context = context;
        }

        #region Step Region

        #region Update address settings for user's tenant
        [When(@"update address for user's tenant")]
        public void WhenUpdateAddressForUsersTenant()
        {
            Context.FirstUserPMData = UpdateFirstUserAddressSettings(UserTenant.Token);
        }
        
        [Then(@"address should update successfully")]
        public void ThenAddressShouldUpdateSuccessfully()
        {
            Context.FirstUserPMData.Should().NotBeNull();
            Context.FirstUserPMData.Tenant.Should().Be(UserTenant.Tenant);
        }
        #endregion

        #region Update address settings for other tenant
        [When(@"update address for other tenant")]
        public void WhenUpdateAddressForOtherTenant()
        {
            Context.act = ()=> UpdateFirstUserAddressSettings(UserOtherTenant.Token);
        }

        [Then(@"should receive error message say no permission to do this operation on tenant")]
        public void ThenShouldReceiveErrorMessageSayNoPermissionToDoThisOperationOnTenant()
        {
            Context.act.Should().ThrowExactly<AggregateException>()
                .And.InnerExceptions[0].Message.Contains("Sorry! you have no permission to do this operation on Tenant");
        }
        #endregion

        #endregion

        #region Private Function Region
        private AddressPM UpdateFirstUserAddressSettings(string Token)
        {
            AddressPM FirstUserAdressSettings = GetAFirstUserAdressSettings();

            ApiResponse<AddressPM> UpdatedAddressSettings = APICaller.CallPut<AddressPM>(FirstUserAdressSettings, Urls.AddressController , Token);
            return UpdatedAddressSettings.Data;
        }
        #endregion

        #region Build Models Region
        private AddressPM GetAFirstUserAdressSettings()
        {
            AddressPM addressPM = new AddressBuilder()
                .WithDefualtValues()
                .Description("Integration Test")
                .City("New York City")
                .Name("Te")
                .AddressTypeId("M")
                //.Address1("18 West 48th Street ")
                //.Address2("#5B, New York3")
                //.ZipCode("+001")
                //.FaxNumber("asd")
                //.PhoneNumber("+001598137715")
                //.StateEnglishName("New York")
                //.VatNumber("89898")
                //.CardCode("10027")
                //.CardEnglishName("Simplog LTD.")
                .Build();

            ApiResponse<AddressPM> response = APICaller.CallPost<AddressPM>(addressPM, Urls.AddressController, UserTenant.Token);
            return response.Data;
        }
        #endregion
    }
}