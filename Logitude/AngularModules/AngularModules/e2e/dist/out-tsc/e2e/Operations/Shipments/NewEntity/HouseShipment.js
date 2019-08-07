"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var ShipmentHelper_1 = require("../ShipmentHelper");
var HouseShipment = /** @class */ (function () {
    function HouseShipment() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.ShipmentModes = new ShipmentHelper_1.ShipmentHelper();
    }
    HouseShipment.prototype.CreateHouseShipment = function (ReferenceNumber, ShipmentLevelCode, Direction, TransportMode, ShipmentType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.WaitByIdAndClick('NEWHOUSE');
        this.ShipmentModes.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);
        this.FillHouseShipmentFields(ReferenceNumber, Direction, TransportMode);
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
    };
    HouseShipment.prototype.FillHouseShipmentFields = function (ShipperRef, Direction, TransportMode) {
        if (Direction == 'Domestic' && TransportMode == 'I') {
            this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef); // test random number randomWholeNum
            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        else {
            this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', ShipperRef); // test random number randomWholeNum
            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestConsignee');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            if (ShipperRef != 'CreatedFromMaster') {
                if (Direction == 'Domestic' && TransportMode != 'I') {
                    this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'MIA');
                    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                    this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'MIA');
                    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                }
                else {
                    this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
                    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                    this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'mvd');
                    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                }
            }
        }
        this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'LDE');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        // this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        // this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing - Create New House ... ');
    };
    return HouseShipment;
}());
exports.HouseShipment = HouseShipment;
//# sourceMappingURL=HouseShipment.js.map