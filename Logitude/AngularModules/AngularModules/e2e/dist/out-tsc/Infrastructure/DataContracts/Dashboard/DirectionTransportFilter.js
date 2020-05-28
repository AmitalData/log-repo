"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var List_1 = require("./List");
var DirectionTransportFilter = /** @class */ (function () {
    function DirectionTransportFilter(title, directionID, transportID) {
        this.FilterTitle = title;
        this.FilterDirectionID = directionID;
        this.FilterTransportID = transportID;
    }
    Object.defineProperty(DirectionTransportFilter.prototype, "FilterTitle", {
        get: function () { return this.filterTitle; },
        set: function (newValue) { this.filterTitle = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DirectionTransportFilter.prototype, "FilterDirectionID", {
        get: function () { return this.filterDirectionID; },
        set: function (newValue) { this.filterDirectionID = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DirectionTransportFilter.prototype, "FilterTransportID", {
        get: function () { return this.filterTransportID; },
        set: function (newValue) { this.filterTransportID = newValue; },
        enumerable: true,
        configurable: true
    });
    DirectionTransportFilter.myList = function () {
        var list = new List_1.List();
        var f0 = new DirectionTransportFilter("All", "", "");
        var f1 = new DirectionTransportFilter("Export", "E", "");
        var f2 = new DirectionTransportFilter("Import", "I", "");
        var f3 = new DirectionTransportFilter("Export Air", "E", "A");
        var f4 = new DirectionTransportFilter("Export Ocean", "E", "O");
        var f5 = new DirectionTransportFilter("Export Inland", "E", "I");
        var f6 = new DirectionTransportFilter("Import Air", "I", "A");
        var f7 = new DirectionTransportFilter("Import Ocean", "I", "O");
        var f8 = new DirectionTransportFilter("Import Inland", "I", "I");
        var f9 = new DirectionTransportFilter("Domestic Air", "D", "A");
        var f10 = new DirectionTransportFilter("Domestic Ocean", "D", "O");
        var f11 = new DirectionTransportFilter("Domestic Inland", "D", "I");
        var f12 = new DirectionTransportFilter("Drop Air", "R", "A");
        var f13 = new DirectionTransportFilter("Drop Ocean", "R", "O");
        var f14 = new DirectionTransportFilter("Drop Inland", "R", "I");
        list.add(f0);
        list.add(f1);
        list.add(f2);
        list.add(f3);
        list.add(f4);
        list.add(f5);
        list.add(f6);
        list.add(f7);
        list.add(f8);
        list.add(f9);
        list.add(f10);
        list.add(f11);
        list.add(f12);
        list.add(f13);
        list.add(f14);
        return list;
    };
    return DirectionTransportFilter;
}());
exports.DirectionTransportFilter = DirectionTransportFilter;
//# sourceMappingURL=DirectionTransportFilter.js.map