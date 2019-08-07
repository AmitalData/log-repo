"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//import {isBlank, isNumber} from '@angular/common/src/facade/lang';
var Tools_1 = require("../Tools");
var DecimalFormatter = /** @class */ (function () {
    function DecimalFormatter() {
    }
    DecimalFormatter.format = function (value, maxDigits) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            if (Tools_1.FormatTool.IsDecimal(value.toString())) {
                var myMinFractionDigits = 0;
                var myMaxFractionDigits = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(maxDigits)) {
                    myMinFractionDigits = myMaxFractionDigits = maxDigits;
                }
                myResult = Tools_1.FormatTool.FormatNumber(value, "N" + myMinFractionDigits); /* value.toLocaleString('en-US', { minimumFractionDigits: myMinFractionDigits, maximumFractionDigits: myMaxFractionDigits });*/
            }
        }
        return myResult;
    };
    return DecimalFormatter;
}());
exports.DecimalFormatter = DecimalFormatter;
//# sourceMappingURL=DecimalFormatter.js.map