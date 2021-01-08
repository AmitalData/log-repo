using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class CreateShipmentSteps
    {
        protected User User;
        protected ShipmentPM _masterPM, _housePM;

        public CreateShipmentSteps(MultiUsers multiUsers)
        {
            User = multiUsers.Users[0];
        }

        [Given(@"A master shipment fields")]
        public void GivenAMasterShipmentFields(Table table)
        {
            _masterPM = table.CreateInstance<ShipmentPM>();
            _masterPM.Tenant = User.Tenant;
            _masterPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        [When(@"Create master shipment API request sent")]
        public void WhenCreateMasterShipmentAPIRequestSent()
        {
            _masterPM = APICaller.CallPost<ShipmentPM>(_masterPM, "Shipment", User.Token);
        }

        [Then(@"A new master created successfully")]
        public void ThenANewMasterCreatedSuccessfully()
        {
            _masterPM.Id.Should().NotBeNull();
        }

        [Given(@"A house shipment fields")]
        public void GivenAHouseShipmentFields(Table table)
        {
            _housePM = table.CreateInstance<ShipmentPM>();
            _housePM.Tenant = User.Tenant;
            _housePM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        [Given(@"A master shipment")]
        public void GivenAMasterShipment()
        {
            string masterURL = "shipmentviews/getbyfilters?&&ForceCacheRefresh=false&DontApplyVirtualization=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=12";// &SortBy=&SortDirection=&AdditionalFilters=[{%22FieldName%22:%22OperationalOpenMastersDirects%22,%22FieldValue%22:%22false%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:true,%22DisplayInList%22:false,%22IsCustomField%22:false,%22FieldDataType%22:%22Constant%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false}]";
            string masterurl = "shipmentviews/getbyfilters?&&ForceCacheRefresh=false&DontApplyVirtualization=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=12&SortBy=&SortDirection=&AdditionalFilters=[{%22FieldName%22:%22OperationalOpenHousesDirects%22,%22FieldValue%22:%22false%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:true,%22DisplayInList%22:false,%22IsCustomField%22:false,%22FieldDataType%22:%22Constant%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false},{%22FieldName%22:%22ShipmentLevelCode%22,%22FieldValue%22:%22C%22,%22FieldValue2%22:null,%22FieldValue3%22:null,%22Operator%22:%22Equals%22,%22IsCustom%22:false,%22DisplayInList%22:true,%22IsCustomField%22:false,%22FieldDataType%22:%22string%22,%22IgnoreFilter%22:false,%22IsCacheOnClient%22:false,%22IsLookUpfilter%22:false}]";
            string shipmentsListUrl = "ShipmentViews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>(shipmentsListUrl, User.Token, "Result");

            _masterPM = shipmentPMs.FirstOrDefault();

            _housePM.MasterShipmentDataId = _masterPM.MasterShipmentDataId;
            _housePM.MasterShipmentNumber = _masterPM.MasterShipmentNumber;
        }

        [When(@"Create house shipment API request sent")]
        public void WhenCreateHouseShipmentAPIRequestSent()
        {
            _housePM = APICaller.CallPost<ShipmentPM>(_housePM, "Shipment", User.Token);
        }

        [Then(@"A new house created successfully")]
        public void ThenANewHouseCreatedSuccessfully()
        {
            _housePM.Id.Should().NotBeNull();
            _housePM.MasterShipmentDataId.Should().NotBeNull();
            _housePM.MasterShipmentNumber.Should().NotBeNull();
        }
    }
}
