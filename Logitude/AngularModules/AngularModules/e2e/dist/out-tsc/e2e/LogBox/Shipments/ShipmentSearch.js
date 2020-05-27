"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("../Helpers/GeneralFunctions");
var ShipmentSearch = /** @class */ (function () {
    function ShipmentSearch() {
        this.helper = new FieldsHelper_1.FieldsHelper();
        this.LogboxTab = new GeneralFunctions_1.GeneralFunctions();
    }
    ShipmentSearch.prototype.QuickSearch = function () {
        this.LogboxTab.GoToMainMenu('General.MH.Importers');
        this.helper.WaitBusyIndicator();
        // this.helper.WaitByCssStringAndClick('Counter',' Agent Shipments'); 
        //var shipmentsTab = this.Helper.WaitByCssAndClick_SelectItemFromList('.PagesMenu', 1);
    };
    return ShipmentSearch;
}());
exports.ShipmentSearch = ShipmentSearch;
//# sourceMappingURL=ShipmentSearch.js.map