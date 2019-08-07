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
var ApiFiltersEvent = /** @class */ (function (_super) {
    __extends(ApiFiltersEvent, _super);
    function ApiFiltersEvent() {
        return _super.call(this) || this;
    }
    ApiFiltersEvent.prototype.emit = function (value) { _super.prototype.next.call(this, value); };
    return ApiFiltersEvent;
}(Subject_1.Subject));
exports.ApiFiltersEvent = ApiFiltersEvent;
//import {CustomerEventEmitter} from './customer-event-emitter';
var PubSubService = /** @class */ (function () {
    function PubSubService() {
        this.Stream = new ApiFiltersEvent();
    }
    return PubSubService;
}());
exports.PubSubService = PubSubService;
//# sourceMappingURL=ApiFiltersEvent.js.map