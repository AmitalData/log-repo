"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var InfrastructureDomainService_1 = require("../Services/InfrastructureDomainService");
var LastFilterClass = /** @class */ (function () {
    function LastFilterClass() {
    }
    LastFilterClass.MapJSON = function (json) {
        if (json != null) {
            var allLists = json;
            for (var key in allLists) {
                var entity = this.MapJsonToEntityList(allLists[key]);
                this.AllFilters.push(entity);
            }
        }
    };
    LastFilterClass.MapJsonToEntityList = function (jsonItem) {
        var entity = new CRMFilterSettingList();
        var jsonItemKeys = Object.keys(jsonItem);
        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entity[property] = jsonItem[property];
        }
        return entity;
    };
    LastFilterClass.GetFilterValue = function (myControlName, myFilterName) {
        var myResult = null;
        var filter = this.AllFilters.filter(function (f) { return f.ControlNameSpace == myControlName && f.FilterName == myFilterName; })[0];
        if (filter != null) {
            myResult = filter.FilterValue;
        }
        return myResult;
    };
    LastFilterClass.UpdateFilter = function (myControlName, myFilterName, myFilterValue) {
        var _this = this;
        var myService = new InfrastructureDomainService_1.InfrastructureDomainService();
        myService.UpdateLastFilter(myControlName, myFilterName, myFilterValue).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.AllFilters = [];
                    _this.MapJSON(myResponse.Result);
                }
            }
        });
    };
    LastFilterClass.AllFilters = [];
    return LastFilterClass;
}());
exports.LastFilterClass = LastFilterClass;
var CRMFilterSettingList = /** @class */ (function () {
    function CRMFilterSettingList() {
    }
    return CRMFilterSettingList;
}());
exports.CRMFilterSettingList = CRMFilterSettingList;
//# sourceMappingURL=LastFilterClass.js.map