"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var GeneralTabComponent = /** @class */ (function () {
    function GeneralTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    GeneralTabComponent.prototype.GeneralTab = function (LogitudeShipType) {
        this.Helper.WaitByIdAndClick('Shipment.TH.General');
        if (LogitudeShipType == 'D' || LogitudeShipType == 'H') {
            // Fill some fields in General tab
            // var mainHarmonize = this.Helper.WaitByIdAndFill('Shipment_MainHarmonize', 'Protractor test ')
            // var otherCharges = this.Helper.WaitByIdAndFill('Shipment_OtherPrepaidCollectId', 'coll');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            // var freightPC = this.Helper.WaitByIdAndFill('Shipment_FreightPrepaidCollectId', 'coll');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var moveType = this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var incoterm = this.Helper.WaitByIdAndFill('Shipment_IncotermId', 'LDE Incoterm');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var AMSBL = this.Helper.WaitByIdAndFill('Shipment_AMSBL', 'Shipment AMSBL');
            var department = this.Helper.WaitByIdAndFill('Shipment_DepartmentId', 'man');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var department = this.Helper.WaitByIdAndFill('Shipment_BranchId', 'main');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        else if (LogitudeShipType == 'M') {
            var freightPC = this.Helper.WaitByIdAndFill('Shipment_FreightPrepaidCollectId', 'coll');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var otherCharges = this.Helper.WaitByIdAndFill('Shipment_OtherPrepaidCollectId', 'coll');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var moveType = this.Helper.WaitByIdAndFill('Shipment_MoveTypeId', 'TestMoveType');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            var AMSBL = this.Helper.WaitByIdAndFill('Shipment_AMSBL', 'Shipment AMSBL');
        }
    };
    return GeneralTabComponent;
}());
exports.GeneralTabComponent = GeneralTabComponent;
//# sourceMappingURL=GeneralTab.js.map