"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var ShipmentWorkSpace_1 = require("./ShipmentWorkSpace");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
var OperationsComp = /** @class */ (function () {
    function OperationsComp() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.ShipmentWorkSpace = new ShipmentWorkSpace_1.ShipmentWorkSpace();
        this.Operation = new GeneralFunctions_1.GeneralFunctions();
    }
    OperationsComp.prototype.DoOperations = function () {
        this.Operation.GoToMainMenu('General.MH.Operations');
        this.Operation.SelectMenuWorkSpaceTabs('SHIP');
        //  this.ShipmentWorkSpace.CreateWizard('M');
        this.ShipmentWorkSpace.CreateShipment(protractor_1.browser.params.ShipParams.ShipmentLevelCode, protractor_1.browser.params.ShipParams.Direction, protractor_1.browser.params.ShipParams.TransportMode, protractor_1.browser.params.ShipParams.ShipmentType);
        // this.ShipmentWorkSpace.CreateShipment('D', 'Export','A', '');
    };
    return OperationsComp;
}());
exports.OperationsComp = OperationsComp;
//# sourceMappingURL=Operations.po.js.map