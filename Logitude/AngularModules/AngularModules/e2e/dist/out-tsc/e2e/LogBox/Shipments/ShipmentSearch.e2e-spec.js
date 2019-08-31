"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CreateLogboxShipment_1 = require("./CreateLogboxShipment");
var ShipmentSearch_1 = require("./ShipmentSearch");
var protractor_1 = require("protractor");
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
describe('Shipment search', function () {
    var shipmentSearch = new ShipmentSearch_1.ShipmentSearch();
    var logboxShipment = new CreateLogboxShipment_1.LogboxShipment();
    var GeneralFun = new GeneralFunctions_1.GeneralFunctions();
    beforeEach(function () {
        protractor_1.browser.driver.manage().window().maximize();
        protractor_1.browser.ignoreSynchronization = true;
    });
    it('Create Shipment successfully', function () {
        shipmentSearch.QuickSearch();
        var orderNumber = GeneralFun.RandomNum();
        logboxShipment.CreateShip(orderNumber);
        logboxShipment.SearchForCreatedShipment('SearchFieldsId_0_0', orderNumber);
        logboxShipment.AddDocument();
        //logboxShipment.ClickNewShipment();
        //logboxShipment.ChooseTransportMode();
        //var orderNo= logboxShipment.InsertOrderNumber();
        //logboxShipment.InsertAgent();
        //logboxShipment.SaveShipment();
        ////logboxShipment.SearchForCreatedShipment();
        //logboxShipment.SelectShipment();
        //logboxShipment.CreateDocument();
    });
});
//# sourceMappingURL=ShipmentSearch.e2e-spec.js.map