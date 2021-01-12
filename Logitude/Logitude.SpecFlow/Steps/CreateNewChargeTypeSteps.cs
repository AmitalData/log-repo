using FluentAssertions;
using Logitude.SpecFlow.Models.ChargeType;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.ValueRetrievers;
using System;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SpecFlow.Steps
{
    [Binding]
    public class CreateNewChargeTypeSteps
    {
        protected ChargeTypeContext Context;

        public CreateNewChargeTypeSteps(MultiUsers multiUsers, ChargeTypeContext context)
        {
            Context = context;
            Context.User = multiUsers.Users[0];

            Service.Instance.ValueRetrievers.Register(new ForeignEntityValueRetriever(Context.User));
            Service.Instance.ValueRetrievers.Register(new RandomValueRetriever());
        }
        
        [Given(@"Charge type with the following properties")]
        public void GivenUserAddAChargeTypeWithTheFollowingProperties(Table chargeTypeData)
        {
            ChargeTypePM chargeTypePM = chargeTypeData.CreateInstance<ChargeTypePM>();
            Context.ChargeTypePM.Tenant = Context.User.Tenant;
            Context.ChargeTypePM.Code = chargeTypePM.Code;
            Context.ChargeTypePM.EnglishName = chargeTypePM.EnglishName;
            Context.ChargeTypePM.ChargesGroupCode = chargeTypePM.ChargesGroupCode;
            Context.ChargeTypePM.MeasurementId = chargeTypePM.MeasurementId;
            Context.ChargeTypePM.ChargesGroupId = chargeTypePM.ChargesGroupId;
        }

        [When(@"Create charge type")]
        public void WhenTheUserCallCreateChargeTypeAPI()
        {
            Context.ChargeTypePM = APICaller.CallPost<ChargeTypePM>(Context.ChargeTypePM, "ChargesTypes", Context.User.Token);
        }
        
        [Then(@"New charge type should be created")]
        public void ThenANewChargeTypeShouldBeAdded()
        {
            Context.ChargeTypePM?.Id.Should().NotBeNullOrEmpty();
        }
    }
}