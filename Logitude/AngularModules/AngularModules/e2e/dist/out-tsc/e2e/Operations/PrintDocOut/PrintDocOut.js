"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var PrintDocOut = /** @class */ (function () {
    function PrintDocOut() {
        this.helper = new FieldsHelper_1.FieldsHelper();
    }
    PrintDocOut.prototype.isPrintingCompleted = function (expectedId, closePopup) {
        // this.helper.waitElementByIDPresence(expectedId);
        this.helper.ItemsPresent(expectedId);
        if (closePopup) {
            this.helper.WaitByCssStringAndClick('.Button', 'Close');
        }
    };
    return PrintDocOut;
}());
exports.PrintDocOut = PrintDocOut;
//# sourceMappingURL=PrintDocOut.js.map