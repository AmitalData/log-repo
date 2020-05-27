"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CachedData = /** @class */ (function () {
    function CachedData() {
    }
    CachedData.GetCachedData = function (objectTableName) {
        if (!this.cachedDataList) {
            this.cachedDataList = {};
        }
        return this.cachedDataList[objectTableName];
    };
    CachedData.StoreCachedData = function (objectTableName, data) {
        if (!this.cachedDataList) {
            this.cachedDataList = {};
        }
        this.cachedDataList[objectTableName] = data;
    };
    return CachedData;
}());
exports.CachedData = CachedData;
//# sourceMappingURL=CachedData.js.map