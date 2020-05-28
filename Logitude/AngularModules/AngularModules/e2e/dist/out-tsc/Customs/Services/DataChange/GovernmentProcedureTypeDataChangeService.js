"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var GovernmentProcedureTypeDataChangeService = /** @class */ (function () {
    function GovernmentProcedureTypeDataChangeService() {
    }
    GovernmentProcedureTypeDataChangeService.prototype.ApplyDataChange = function (data) {
        //.sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        var result = [];
        var list1 = data.filter(function (d) { return d.IndexOrder != null; }).sort(function (a, b) {
            if (a.IndexOrder > b.IndexOrder) {
                return 1;
            }
            else if (a.IndexOrder < b.IndexOrder) {
                return -1;
            }
            else {
                if (a.Code > b.Code)
                    return 1;
                else if (a.Code < b.Code)
                    return -1;
                else
                    return 0;
            }
        });
        list1.forEach(function (item) {
            result.push(item);
        });
        var list2 = data.filter(function (d) { return d.IndexOrder == null; }).sort(function (a, b) {
            if (a.IndexOrder > b.IndexOrder) {
                return 1;
            }
            else if (a.IndexOrder < b.IndexOrder) {
                return -1;
            }
            else {
                if (a.Code > b.Code)
                    return 1;
                else if (a.Code < b.Code)
                    return -1;
                else
                    return 0;
            }
        });
        list2.forEach(function (item) {
            result.push(item);
        });
        return result;
    };
    return GovernmentProcedureTypeDataChangeService;
}());
exports.GovernmentProcedureTypeDataChangeService = GovernmentProcedureTypeDataChangeService;
//# sourceMappingURL=GovernmentProcedureTypeDataChangeService.js.map