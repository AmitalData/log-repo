"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DashBoardFilters = /** @class */ (function () {
    function DashBoardFilters(title, index) {
        this.title = title;
        this.index = index;
    }
    Object.defineProperty(DashBoardFilters.prototype, "Index", {
        get: function () { return this.index; },
        set: function (value) { this.index = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DashBoardFilters.prototype, "Title", {
        get: function () { return this.title; },
        set: function (value) { this.title = value; },
        enumerable: true,
        configurable: true
    });
    return DashBoardFilters;
}());
exports.DashBoardFilters = DashBoardFilters;
//# sourceMappingURL=DashboardFilters.js.map