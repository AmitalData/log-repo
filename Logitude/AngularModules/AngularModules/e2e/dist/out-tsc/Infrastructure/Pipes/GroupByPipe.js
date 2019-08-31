"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var GroupByPipe = /** @class */ (function () {
    function GroupByPipe() {
    }
    GroupByPipe.prototype.transform = function (value, field) {
        var groupedObj = value.reduce(function (prev, cur) {
            //if(prev)
            if (!prev[cur[field]]) {
                prev[cur[field]] = [cur];
            }
            else {
                prev[cur[field]].push(cur);
            }
            return prev;
        }, {});
        return Object.keys(groupedObj).map(function (key) { return { key: key, value: groupedObj[key] }; });
    };
    GroupByPipe.prototype.transformMultiFields = function (value, field, field2) {
        var groupedObj = value.reduce(function (prev, cur) {
            //if(prev)
            var temp = prev[cur[field]];
            var temp1 = cur[field];
            var temp2 = [cur];
            if ((!prev[cur[field]]) && (!prev[cur[field2]])) {
                prev[cur[field] + ',' + cur[field2]] = [cur];
                //  prev[cur[field2]] = [cur];
            }
            else {
                prev[cur[field]].push(cur);
                // prev[cur[field2]].push(cur);
            }
            return prev;
        }, {});
        return Object.keys(groupedObj).map(function (key) { return { key: key, value: groupedObj[key] }; });
    };
    GroupByPipe.prototype.ShapeGrouping = function (value, field) {
        var groupedObj = value.reduce(function (prev, cur) {
            //if(prev)
            if (!prev[cur[field]]) {
                prev[cur[field]] = [cur];
            }
            else {
                prev[cur[field]].push(cur);
            }
            return prev;
        }, {});
        var temp = Object.keys(groupedObj).map(function (key) { return { key: key, value: groupedObj[key] }; });
        var finalarray = [];
        var i = 0;
        temp.forEach(function (item, key) {
            finalarray.push({ Type: 'Head', Data: item.key, Index: i, ChildrenFirstIndex: i + 1, Count: item.value.length });
            i++;
            item.value.forEach(function (inneritem, key) {
                finalarray.push({ Type: 'Body', Data: inneritem, Index: i });
                i++;
            });
        });
        return finalarray;
    };
    GroupByPipe.prototype.ShapeList = function (value) {
        var finalarray = [];
        var i = 0;
        value.forEach(function (item, key) {
            finalarray.push({ Type: 'Body', Data: item, Index: i });
            i++;
        });
        return finalarray;
    };
    GroupByPipe = __decorate([
        core_1.Pipe({ name: 'groupBy' })
    ], GroupByPipe);
    return GroupByPipe;
}());
exports.GroupByPipe = GroupByPipe;
//# sourceMappingURL=GroupByPipe.js.map