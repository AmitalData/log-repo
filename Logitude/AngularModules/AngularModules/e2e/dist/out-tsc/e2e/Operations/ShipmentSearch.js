"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("./../Helpers/GeneralFunctions");
// import { Driver } from 'selenium-webdriver/safari';
var ShipmentSearch = /** @class */ (function () {
    function ShipmentSearch() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.operationTab = new GeneralFunctions_1.GeneralFunctions();
    }
    return ShipmentSearch;
}());
exports.ShipmentSearch = ShipmentSearch;
//# sourceMappingURL=ShipmentSearch.js.map