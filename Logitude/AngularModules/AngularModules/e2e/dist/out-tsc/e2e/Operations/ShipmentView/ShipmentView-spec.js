"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var ShipmentViewScenario_1 = require("./ShipmentViewScenario");
var ShipmentView_1 = require("./ShipmentView");
describe('ShipmentView', function () {
    var ShipmentScenario = new ShipmentViewScenario_1.ShipmentViewScenario();
    var CreatView = new ShipmentView_1.ShipmentView();
    beforeEach(function () {
    });
    protractor_1.browser.ignoreSynchronization = true;
    it('OpenShipmentView', function () {
        CreatView.OpenShipmentView();
    });
    it('CreatShipmentView', function () {
        CreatView.CreatNewView();
    });
});
//# sourceMappingURL=ShipmentView-spec.js.map