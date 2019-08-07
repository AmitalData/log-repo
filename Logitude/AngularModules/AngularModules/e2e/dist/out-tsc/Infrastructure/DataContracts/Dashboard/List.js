"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var List = /** @class */ (function () {
    function List() {
        this.items = [];
    }
    List.prototype.size = function () {
        return this.items.length;
    };
    List.prototype.add = function (value) {
        this.items.push(value);
    };
    List.prototype.get = function (index) {
        return this.items[index];
    };
    List.prototype.set = function (index, value) {
        this.items[index] = value;
    };
    List.prototype.getAll = function () {
        return this.items;
    };
    List.prototype.Assign = function (arr) {
        this.items = arr;
    };
    List.prototype.Clear = function () {
        this.items = [];
    };
    return List;
}());
exports.List = List;
//# sourceMappingURL=List.js.map