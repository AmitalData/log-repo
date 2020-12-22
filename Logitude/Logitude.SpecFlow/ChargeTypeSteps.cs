using Logitude.BL.CommonDataModel.EntityQueries;
using TechTalk.SpecFlow;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Xunit;

namespace Logitude.SpecFlow
{
    [Binding]
    public class ChargeTypeSteps
    {
        private readonly int _tenant = 0;
        private string _newchargeTypeId;
        private string _chargeTypeCode;
        private string _chargeTypeName;
        private string _chargeTypeGroupCode;
        private string _chargeTypeMeasurementId;
        
        [Given(@"The charge type code is (.*)")]
        public void GivenTheChargeTypeCodeIs(string chargeTypeCode)
        {
            _chargeTypeCode = chargeTypeCode;
        }
        
        [Given(@"The charge type name is (.*)")]
        public void GivenTheChargeTypeNameIs(string chargeTypeName)
        {
            _chargeTypeName = chargeTypeName;
        }
        
        [Given(@"The charge type group code is (.*)")]
        public void GivenTheChargeTypeGroupCodeIs(string chargeTypeGroupCode)
        {
            _chargeTypeGroupCode = chargeTypeGroupCode;
        }
        
        [Given(@"The charge type measurement code is (.*)")]
        public void GivenTheChargeTypeMeasurementCodeIs(string chargeTypeMeasurementCode)
        {
            MeasurementQuery measurementQuery = new MeasurementQuery(_tenant);
            MeasurementPM measurementPM = measurementQuery.GetSinglePMByCode(chargeTypeMeasurementCode, _tenant);
            _chargeTypeMeasurementId = measurementPM?.Id;
        }
        
        [When(@"Try to create the charge type")]
        public void WhenTryToCreateTheChargeType()
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(_tenant);
            ChargesTypeService chargesTypeService = new ChargesTypeService(commonDataContext, _tenant);
            ChargesTypePM chargesTypePM = new ChargesTypePM
            {
                Tenant = _tenant,
                Code = _chargeTypeCode,
                EnglishName = _chargeTypeName,
                ChargesGroupCode = _chargeTypeGroupCode,
                MeasurementId = _chargeTypeMeasurementId
            };
            chargesTypeService.Create(chargesTypePM);
            _newchargeTypeId = chargesTypePM.Id;
        }
        
        [Then(@"The charge type will created successfully")]
        public void ThenTheChargeTypeWillCreatedSuccessfully()
        {
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(_tenant);
            ChargesTypePM chargesTypePM = chargesTypeQuery.GetSinglePM(_newchargeTypeId, _tenant);
            Assert.Equal(_newchargeTypeId, chargesTypePM?.Id);
        }
    }
}