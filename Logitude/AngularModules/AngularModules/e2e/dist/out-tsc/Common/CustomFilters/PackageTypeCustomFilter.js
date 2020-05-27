"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PackageTypeCustomFilter = /** @class */ (function () {
    function PackageTypeCustomFilter() {
    }
    PackageTypeCustomFilter.GetFilteredQuery = function (addtionalFiltersValues, data) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "TransportModeId") {
                var value = propName.FieldValue;
                if (value == "A") {
                    data = data.filter(function (d) { return d.IsAir == true; });
                }
                if (value == "O") {
                    data = data.filter(function (d) { return d.IsOcean == true; });
                }
                if (value == "I") {
                    data = data.filter(function (d) { return d.IsInland == true; });
                }
            }
        }
        return data;
    };
    return PackageTypeCustomFilter;
}());
exports.PackageTypeCustomFilter = PackageTypeCustomFilter;
//# sourceMappingURL=PackageTypeCustomFilter.js.map