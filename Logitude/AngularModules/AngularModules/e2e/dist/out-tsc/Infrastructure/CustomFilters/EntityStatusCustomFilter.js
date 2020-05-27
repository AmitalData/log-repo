"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var EntityStatusCustomFilter = /** @class */ (function () {
    function EntityStatusCustomFilter() {
    }
    EntityStatusCustomFilter.GetFilteredQuery = function (addtionalFiltersValues, data) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        if (mykeys.filter(function (d) { return d.IsCustom == true; })[0]) {
            for (var i in mykeys.filter(function (d) { return d.IsCustom; })) {
                var propName = mykeys[i];
                if (propName.FieldName == "AutomationStatusField") {
                    var result = [];
                    data.forEach(function (item) {
                        var fieldValue = propName.FieldValue;
                        var status = result.filter(function (d) { return d.Name == item.Name; })[0];
                        if (!status) {
                            result.push(item);
                        }
                        else {
                            if (!Tools_1.AppTool.IsNullOrEmpty(fieldValue)) {
                                if (item.Id == fieldValue) {
                                    var index = result.indexOf(status);
                                    if (index > -1) {
                                        result.splice(index, 1);
                                        result.push(item);
                                    }
                                }
                            }
                        }
                    });
                    data = result;
                }
            }
        }
        return data;
    };
    return EntityStatusCustomFilter;
}());
exports.EntityStatusCustomFilter = EntityStatusCustomFilter;
//# sourceMappingURL=EntityStatusCustomFilter.js.map