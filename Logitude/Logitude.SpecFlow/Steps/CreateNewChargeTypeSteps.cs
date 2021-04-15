using FluentAssertions;
using Logitude.SpecFlow.Builders.ChargeType;
using Logitude.SpecFlow.Models.ChargeType;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.SpecFlow.Steps
{
    [Binding]
    public class CreateNewChargeTypeSteps
    {
        protected ChargeTypeContext Context;

        public CreateNewChargeTypeSteps(ChargeTypeContext context)
        {
            Context = context;
        }
        
        [Given(@"Charge type with the following properties")]
        public void GivenUserAddAChargeTypeWithTheFollowingProperties(Table chargeTypeData)
        {
            ChargeTypeBuilder chargeTypeBuilder = new ChargeTypeBuilder();

            //Context.ChargeTypePM = chargeTypeBuilder.FromDataTable(chargeTypeData).Build();
            ChargeTypePM chargeTypePM = chargeTypeBuilder.FromDataTable(chargeTypeData).Build();
            Context.ChargeTypePM = chargeTypeBuilder.WithDefualtValues()
                                                          .Tenant(UserTenant.Tenant)
                                                          .Code(chargeTypePM.Code)
                                                          .EnglishName(chargeTypePM.EnglishName)
                                                          .ChargesGroupCode(chargeTypePM.ChargesGroupCode)
                                                          .MeasurementId(chargeTypePM.MeasurementId)
                                                          .ChargesGroupId(chargeTypePM.ChargesGroupId)
                                                          .Build();


            //Context.ChargeTypePM.Tenant = Context.User.Tenant;
            //Context.ChargeTypePM.Code = chargeTypePM.Code;
            //Context.ChargeTypePM.EnglishName = chargeTypePM.EnglishName;
            //Context.ChargeTypePM.ChargesGroupCode = chargeTypePM.ChargesGroupCode;
            //Context.ChargeTypePM.MeasurementId = chargeTypePM.MeasurementId;
            //Context.ChargeTypePM.ChargesGroupId = chargeTypePM.ChargesGroupId;
        }

        [When(@"Create charge type")]
        public void WhenTheUserCallCreateChargeTypeAPI()
        {
            Context.ChargeTypePM = APICaller.CallPost<ChargeTypePM>(Context.ChargeTypePM, "ChargesTypes", UserTenant.Token).Data;
        }
        
        [Then(@"New charge type should be created")]
        public void ThenANewChargeTypeShouldBeAdded()
        {
            Context.ChargeTypePM?.Id.Should().NotBeNullOrEmpty();
        }
    }
}