using FluentAssertions;
using Logitude.SpecFlow.Models;
using TechTalk.SpecFlow;

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

        [Given(@"The charge type code is (.*)")]
        public void GivenTheChargeTypeCodeIs(string chargeTypeCode)
        {
            _chargeTypePM.Code = chargeTypeCode;
        }

        [Given(@"The charge type name is (.*)")]
        public void GivenTheChargeTypeNameIs(string chargeTypeName)
        {
            _chargeTypePM.EnglishName = chargeTypeName;
        }

        [Given(@"The charge type group code is (.*)")]
        public void GivenTheChargeTypeGroupCodeIs(string chargeTypeGroupCode)
        {
            _chargeTypePM.ChargesGroupCode = chargeTypeGroupCode;
        }

        [Given(@"The charge type measurement id is (.*)")]
        public void GivenTheChargeTypeMeasurementIdIs(string chargeTypeMeasurementId)
        {
            _chargeTypePM.MeasurementId = chargeTypeMeasurementId;
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