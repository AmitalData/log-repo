"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var ShipmentHelper_1 = require("../ShipmentHelper");
var DirectShipment = /** @class */ (function () {
    function DirectShipment() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.ShipmentModes = new ShipmentHelper_1.ShipmentHelper();
    }
    DirectShipment.prototype.CreateDirectShipment = function (ReferenceNumber, Direction, TransportMode, ShipmentType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.WaitByIdAndClick('NEWDIRECT');
        this.ShipmentModes.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);
        this.FillDirectShipmentFields(ReferenceNumber, TransportMode, Direction);
        this.Helper.WaitByIdAndClick('ShipmentCreatebtn');
    };
    DirectShipment.prototype.WaitSearchBoxResult = function () {
        this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.ItemsVisibility('NEWDIRECT');
        this.Helper.WaitByIdAndClick('NEWSHIP');
    };
    DirectShipment.prototype.FillDirectShipmentFields = function (ShipperRef, TransportMode, Direction) {
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
            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestConsi');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            if (Direction == 'Domestic') {
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            }
            else {
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                this.Helper.WaitByIdAndFill('Shipment_MainCarriageToPortId', 'mvd');
                this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            }
        }
        if (TransportMode == 'A') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', '115');
            this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIdAirMTA');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        else if (TransportMode == 'O') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'MAEU');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Voyage 1');
            this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIDOceanMTO');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        else {
            // this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'Trucker1London');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', 'Trucker # 1');
            this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveTypeIdInlandMTI');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'CIF');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        // this.Helper.WaitByIdAndClick('Shipment_OrderIsDangerouseGoods');
        this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing - Create New Direct Shipment ... ');
    };
    return DirectShipment;
}());
exports.DirectShipment = DirectShipment;
//# sourceMappingURL=DirectShipment.js.map