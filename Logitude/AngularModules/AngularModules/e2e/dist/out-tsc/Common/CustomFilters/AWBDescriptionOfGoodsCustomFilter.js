"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var AWBDescriptionOfGoodsCustomFilter = /** @class */ (function () {
    function AWBDescriptionOfGoodsCustomFilter() {
    }
    AWBDescriptionOfGoodsCustomFilter.GetFilteredQuery = function (addtionalFiltersValues, data) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        if (mykeys.filter(function (d) { return d.IsCustom == true; })[0]) {
            for (var i in mykeys.filter(function (d) { return d.IsCustom; })) {
                var propName = mykeys[i];
                if (propName.FieldName == "AirlineCode") {
                    var myAirlineCode = propName.FieldValue;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myAirlineCode)) {
                        data = data.filter(function (d) { return d.AirlineCode == null || d.AirlineCode == myAirlineCode; });
                    }
                    else {
                        data = data.filter(function (d) { return d.AirlineCode == null; });
                    }
                }
            }
        }
        else {
            data = data.filter(function (d) { return d.AirlineCode == null; });
        }
        return data;
    };
    return AWBDescriptionOfGoodsCustomFilter;
}());
exports.AWBDescriptionOfGoodsCustomFilter = AWBDescriptionOfGoodsCustomFilter;
//# sourceMappingURL=AWBDescriptionOfGoodsCustomFilter.js.map