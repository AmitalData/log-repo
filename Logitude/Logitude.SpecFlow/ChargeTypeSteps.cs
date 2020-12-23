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
        private readonly ChargeTypeStepsContext _context;
        private readonly MeasurementQuery _measurementQuery;
        private readonly ChargesTypeQuery _chargesTypeQuery;
        private readonly ChargesTypeService _chargesTypeService;

        public ChargeTypeSteps(ChargeTypeStepsContext context)
        {
            _context = context;
            _context.Tenant = 0;

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(_context.Tenant);
            _measurementQuery = new MeasurementQuery(_context.Tenant);
            _chargesTypeQuery = new ChargesTypeQuery(_context.Tenant);
            _chargesTypeService = new ChargesTypeService(commonDataContext, _context.Tenant);
        }

        [Given(@"The charge type code is (.*)")]
        public void GivenTheChargeTypeCodeIs(string chargeTypeCode)
        {
            _context.Code = chargeTypeCode;
        }
        
        [Given(@"The charge type name is (.*)")]
        public void GivenTheChargeTypeNameIs(string chargeTypeName)
        {
            _context.Name = chargeTypeName;
        }
        
        [Given(@"The charge type group code is (.*)")]
        public void GivenTheChargeTypeGroupCodeIs(string chargeTypeGroupCode)
        {
            _context.GroupCode = chargeTypeGroupCode;
        }
        
        [Given(@"The charge type measurement code is (.*)")]
        public void GivenTheChargeTypeMeasurementCodeIs(string chargeTypeMeasurementCode)
        {
            MeasurementPM measurementPM = _measurementQuery.GetSinglePMByCode(chargeTypeMeasurementCode, _context.Tenant);
            _context.MeasurementId = measurementPM?.Id;
        }
        
        [When(@"Try to create the charge type")]
        public void WhenTryToCreateTheChargeType()
        {
            ChargesTypePM chargesTypePM = new ChargesTypePM
            {
                Tenant = _context.Tenant,
                Code = _context.Code,
                EnglishName = _context.Name,
                ChargesGroupCode = _context.GroupCode,
                MeasurementId = _context.MeasurementId
            };
            _chargesTypeService.Create(chargesTypePM);
            _context.Id = chargesTypePM.Id;
        }
        
        [Then(@"The charge type will created successfully")]
        public void ThenTheChargeTypeWillCreatedSuccessfully()
        {
            ChargesTypePM chargesTypePM = _chargesTypeQuery.GetSinglePM(_context.Id, _context.Tenant);
            Assert.Equal(_context.Id, chargesTypePM?.Id);
        }
    }
}