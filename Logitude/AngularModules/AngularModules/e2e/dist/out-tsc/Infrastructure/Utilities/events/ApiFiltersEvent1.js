"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Subject_1 = require("rxjs/Subject");
var ApiFiltersEvent1 = /** @class */ (function (_super) {
    __extends(ApiFiltersEvent1, _super);
    function ApiFiltersEvent1() {
        return _super.call(this) || this;
    }
    ApiFiltersEvent1.prototype.emit = function (value) { _super.prototype.next.call(this, value); };
    return ApiFiltersEvent1;
}(Subject_1.Subject));
exports.ApiFiltersEvent1 = ApiFiltersEvent1;
//import {CustomerEventEmitter} from './customer-event-emitter';
var PubSubService1 = /** @class */ (function () {
    function PubSubService1() {
        this.Stream = new ApiFiltersEvent1();
    }
    return PubSubService1;
}());
exports.PubSubService1 = PubSubService1;
//# sourceMappingURL=ApiFiltersEvent1.js.map