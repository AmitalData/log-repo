using FluentAssertions;
using Logitude.SpecFlow.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SpecFlow.Steps
{
    [Binding]
    public class ChargeTypeSteps
    {
        protected readonly UserData _user;
        protected ChargeTypePM _chargeTypePM;

        public ChargeTypeSteps(UserData userData, ChargeTypePM chargeTypePM)
        {
            _user = userData;
            _chargeTypePM = chargeTypePM;
            _chargeTypePM.Tenant = _user.Tenant;
        }

        [Given(@"Charge type with the following data")]
        public void GivenChargeTypeWithTheFollowingData(Table chargeTypeDataTable)
        {
            ChargeTypePM chargeTypePM = chargeTypeDataTable.CreateInstance<ChargeTypePM>();
            _chargeTypePM.Code = chargeTypePM.Code;
            _chargeTypePM.EnglishName = chargeTypePM.EnglishName;
            _chargeTypePM.ChargesGroupCode = chargeTypePM.ChargesGroupCode;
            _chargeTypePM.MeasurementId = chargeTypePM.MeasurementId;
        }

        [When(@"Try to create the charge type")]
        public void WhenTryToCreateTheChargeType()
        {
            HttpRequest httpRequest = new HttpRequest("ChargesTypes", HttpRequestType.BodyRequestType.Post, _user.Token, _chargeTypePM);
            _chargeTypePM = httpRequest.GetResponse<ChargeTypePM>();
        }

        [Then(@"The charge type will created successfully")]
        public void ThenTheChargeTypeWillCreatedSuccessfully()
        {
            _chargeTypePM.Id.Should().NotBeNullOrEmpty();
        }
    }
}