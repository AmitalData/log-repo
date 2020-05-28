"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var HouseShipment_1 = require("../NewEntity/HouseShipment");
var ShipmentsTabComponent = /** @class */ (function () {
    function ShipmentsTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.CreateHouseShipment = new HouseShipment_1.HouseShipment();
    }
    ShipmentsTabComponent.prototype.ShipmentsTab = function () {
        this.Helper.WaitByIdAndClick('Shipment.TH.Consolidation');
        this.Helper.WaitBusyIndicator();
        this.Helper.WaitByIdAndClick('NewHouseBtn');
        this.Helper.WaitBusyIndicator();
        this.CreateHouseShipment.FillHouseShipmentFields('CreatedFromMaster', '', '');
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
        this.Helper.WaitWindowClosed();
        this.Helper.WaitBusyIndicator();
    };
    return ShipmentsTabComponent;
}());
exports.ShipmentsTabComponent = ShipmentsTabComponent;
//# sourceMappingURL=ShipmentsTab.js.map