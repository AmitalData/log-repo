"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var UserCustomFilter = /** @class */ (function () {
    function UserCustomFilter() {
    }
    UserCustomFilter.GetFilteredQuery = function (addtionalFiltersValues, data) {
        //addtionalFiltersValues = addtionalFiltersValues.replace('[', '').replace(']','');
        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "EmployeeGroupCustomFilter") {
                var groupId = propName.FieldValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(groupId)) {
                    data = data.filter(function (d) { return d.GroupId != null && (d.GroupId.indexOf(groupId) > -1); }); //.indexOf(groupId) > -1);
                }
            }
        }
        return data;
        //for (var i = 0; i < addtionalFiltersValues.length; i++) {
        //    var pair = addtionalFiltersValues[i].split('=');
        //    if (decodeURIComponent(pair[0]) == "AdditionalFilters") {
        //        var obj = JSON.parse(pair[1]);
        //        for (var key in obj) {
        //            if (key == 'EmployeeGroupCustomFilter') {
        //                var groupId = obj[key];
        //                if (!AppTool.IsNullOrEmpty(groupId)) {
        //                    data = data.filter(d => d.GroupId != null && d.GroupId.indexOf(groupId) > -1);
        //                }
        //                return data;
        //            }
        //        }
        //    }
        //}
    };
    return UserCustomFilter;
}());
exports.UserCustomFilter = UserCustomFilter;
//# sourceMappingURL=UserCustomFilter.js.map