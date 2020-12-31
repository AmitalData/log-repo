using FluentAssertions;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.SpecFlow.Models;
using Logitude.Test.Services;
using System;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SpecFlow.Steps
{
    [Binding]
    public class CreateNewChargeTypeSteps
    {
        protected readonly UserData User;
        protected ChargesTypePM ChargeTypePM;

        public CreateNewChargeTypeSteps(UserData user, ChargesTypePM chargeTypePM)
        {
            User = user;
            ChargeTypePM = chargeTypePM;
        }
         
        [Given(@"user add a charge type with the following properties")]
        public void GivenUserAddAChargeTypeWithTheFollowingProperties(Table chargeTypeData)
        {
            ChargesTypePM chargeTypePM = chargeTypeData.CreateInstance<ChargesTypePM>();
            var myCode = Regex.Replace(Guid.NewGuid().ToString(), "[^a-zA-Z]+", "");
            ChargeTypePM.Code = myCode.Substring(0,3);
            ChargeTypePM.Tenant = User.Tenant;
            ChargeTypePM.EnglishName = chargeTypePM.EnglishName;
            ChargeTypePM.ChargesGroupCode = chargeTypePM.ChargesGroupCode;
            ChargeTypePM.MeasurementId = chargeTypePM.MeasurementId;
            ChargeTypePM.ChargesGroupId = chargeTypePM.ChargesGroupId;

        }

        [When(@"the user call create charge type API")]
        public void WhenTheUserCallCreateChargeTypeAPI()
        {
            ChargeTypePM = APICaller.CallPost<ChargesTypePM>(ChargeTypePM, "ChargesTypes", User.Token);
        }
        
       
        
        [Then(@"a new charge type should be added")]
        public void ThenANewChargeTypeShouldBeAdded()
        {
            ChargeTypePM.Id.Should().NotBeNullOrEmpty();
        }
    }
}
