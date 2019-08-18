"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var BookingProductCustomFilter = /** @class */ (function () {
    function BookingProductCustomFilter() {
    }
    BookingProductCustomFilter.GetFilteredQuery = function (addtionalFiltersValues, data) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        if (mykeys.filter(function (d) { return d.IsCustom == true; })[0]) {
            for (var i in mykeys.filter(function (d) { return d.IsCustom; })) {
                var propName = mykeys[i];
                if (propName.FieldName == "AirlineId") {
                    var myAirlineId = propName.FieldValue;
                    if (!Tools_1.AppTool.IsNullOrEmpty(myAirlineId)) {
                        data = data.filter(function (d) { return d.AirlineId == null || d.AirlineId == myAirlineId; });
                    }
                    else {
                        data = data.filter(function (d) { return d.AirlineId == null; });
                    }
                }
            }
        }
        else {
            data = data.filter(function (d) { return d.AirlineId == null; });
        }
        return data;
    };
    return BookingProductCustomFilter;
}());
exports.BookingProductCustomFilter = BookingProductCustomFilter;
//# sourceMappingURL=BookingProductCustomFilter.js.map