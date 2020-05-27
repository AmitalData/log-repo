"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var ShipmentHelper_1 = require("../ShipmentHelper");
var MasterShipment = /** @class */ (function () {
    function MasterShipment() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.ShipmentModes = new ShipmentHelper_1.ShipmentHelper();
    }
    MasterShipment.prototype.CreateMasterShipment = function (ReferenceNumber, LogitudeShipType, Direction, TransportMode, ShipmentType) {
        var AWBToggle = this.Helper.WaitByIdAndClick('NEWSHIP');
        this.Helper.WaitByIdAndClick('NEWMASTER');
        this.ShipmentModes.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);
        this.FillMasterShipmentFields(ReferenceNumber, TransportMode, Direction);
        this.Helper.WaitByIdAndClick('MasterCreatebtn');
    };
    MasterShipment.prototype.FillMasterShipmentFields = function (ShipperRef, TransportMode, Direction) {
        this.Helper.WaitByIdAndFill('Master_AgentId', 'TestAgentExport1');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Master_AgentReference1_1', ShipperRef);
        if (Direction == 'Domestic') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_MainCarriageToPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        else {
            this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_MainCarriageToPortId', 'mvd');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        // this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'TestAirlineL8');
        // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        // this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '115')
        if (TransportMode == 'A') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '115');
        }
        else if (TransportMode == 'O') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'MAEU');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', 'Voyage 1');
        }
        else {
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'Trucker1London');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', 'Trucker # 1');
        }
        this.Helper.WaitByIdAndFill('Master_DescriptionOfGoods', 'Protractor testing - Create New Master ... ');
    };
    return MasterShipment;
}());
exports.MasterShipment = MasterShipment;
//# sourceMappingURL=MasterShipment.js.map