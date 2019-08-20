"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ApiQueryFilters = /** @class */ (function () {
    function ApiQueryFilters(getAll) {
        if (getAll === void 0) { getAll = false; }
        this.AdditionalFilters = [];
        this.ForceCacheRefresh = false;
        this.GetAll = getAll;
    }
    ApiQueryFilters.prototype.addAdditionalFilter = function (FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient, ForceEnableAdd) {
        if (IgnoreFilter === void 0) { IgnoreFilter = false; }
        if (IsCacheOnClient === void 0) { IsCacheOnClient = false; }
        if (ForceEnableAdd === void 0) { ForceEnableAdd = false; }
        if (!IsCacheOnClient) {
            if (typeof (FieldValue) === "string") {
                if (FieldValue)
                    FieldValue = FieldValue.replace('"', '\\"');
                FieldValue = encodeURIComponent(FieldValue);
                //FieldValue = FieldValue.replace("%22", "\%22");
            }
            if (typeof (FieldValue2) === "string") {
                if (FieldValue)
                    FieldValue2 = FieldValue2.replace('"', '\\"');
                FieldValue2 = encodeURIComponent(FieldValue2);
                //FieldValue = FieldValue.replace("%20", " ");
            }
            if (typeof (FieldValue3) === "string") {
                if (FieldValue)
                    FieldValue3 = FieldValue3.replace('"', '\\"');
                FieldValue3 = encodeURIComponent(FieldValue3);
                //FieldValue = FieldValue.replace("%20", " ");
            }
        }
        var existedItem = this.AdditionalFilters.find(function (d) { return d.FieldName == FieldName; });
        if (!existedItem || ForceEnableAdd) {
            var item = new FilterItem(FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient);
            this.AdditionalFilters.push(item);
        }
    };
    ApiQueryFilters.prototype.removeAdditionalFilter = function (FieldName) {
        var item = this.AdditionalFilters.filter(function (d) { return d.FieldName == FieldName; })[0];
        if (item) {
            var index = this.AdditionalFilters.indexOf(item);
            this.AdditionalFilters.splice(index, 1);
        }
    };
    return ApiQueryFilters;
}());
exports.ApiQueryFilters = ApiQueryFilters;
var FilterItem = /** @class */ (function () {
    function FilterItem(FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient) {
        if (IsCacheOnClient === void 0) { IsCacheOnClient = false; }
        this.FieldName = FieldName;
        this.FieldValue = FieldValue;
        this.FieldValue2 = FieldValue2;
        this.FieldValue3 = FieldValue3;
        this.Operator = Operator;
        this.IsCustom = IsCustom;
        this.DisplayInList = DisplayInList;
        this.IsCustomField = IsCustomField;
        this.FieldDataType = FieldDataType;
        this.IgnoreFilter = IgnoreFilter;
        this.IsCacheOnClient = IsCacheOnClient;
    }
    return FilterItem;
}());
exports.FilterItem = FilterItem;
//# sourceMappingURL=ApiQueryFilters.js.map