using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class SharedShipmentSteps
    {
        protected User User;
        protected readonly ShipmentContext ShipmentContext;

        public SharedShipmentSteps(MultiUsers multiUsers, ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
            User = multiUsers.Users[0];
        }

        [Given(@"A master shipment")]
        public void GivenAMasterShipment()
        {
            string MastersURL = "shipmentviews/getbyfilters?&&ForceCacheRefresh=false&DontApplyVirtualization=false&GetAll=false&GetCount=false&PageIndex=0&PageSize=1&SortBy=&SortDirection=&AdditionalFilters=[{%22FieldName%22:%22OperationalOpenMastersDirects%22,%22FieldValue%22:%22false%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:true,%22DisplayInList%22:false,%22IsCustomField%22:false,%22FieldDataType%22:%22Constant%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false},{%22FieldName%22:%22ShipmentLevelCode%22,%22FieldValue%22:%22C%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:false,%22DisplayInList%22:true,%22IsCustomField%22:false,%22FieldDataType%22:%22string%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false}]";
            IEnumerable<ShipmentPM> MasterShipments = APICaller.CallGet<IEnumerable<ShipmentPM>>(MastersURL, User.Token, "Result");

            ShipmentContext.MasterShipment = MasterShipments.FirstOrDefault();
        }

        [Given(@"A house shipment")]
        public void GivenAHouseShipment()
        {
            string HousesURL = "shipmentviews/getbyfilters?&&ForceCacheRefresh=false&DontApplyVirtualization=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=1&SortBy=&SortDirection=&AdditionalFilters=[{%22FieldName%22:%22OperationalOpenHousesDirects%22,%22FieldValue%22:%22false%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:true,%22DisplayInList%22:false,%22IsCustomField%22:false,%22FieldDataType%22:%22Constant%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false},{%22FieldName%22:%22ShipmentLevelCode%22,%22FieldValue%22:%22H%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:false,%22DisplayInList%22:true,%22IsCustomField%22:false,%22FieldDataType%22:%22string%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false}]";
            IEnumerable<ShipmentPM> HouseShipments = APICaller.CallGet<IEnumerable<ShipmentPM>>(HousesURL, User.Token, "Result");

            ShipmentContext.HouseShipment = HouseShipments.FirstOrDefault();
        }
    }
}
