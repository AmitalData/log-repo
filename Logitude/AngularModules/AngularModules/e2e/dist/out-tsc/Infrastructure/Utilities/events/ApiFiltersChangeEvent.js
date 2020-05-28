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
var ApiFiltersChangeEvent = /** @class */ (function (_super) {
    __extends(ApiFiltersChangeEvent, _super);
    function ApiFiltersChangeEvent() {
        return _super.call(this) || this;
    }
    ApiFiltersChangeEvent.prototype.emit = function (value) { _super.prototype.next.call(this, value); };
    return ApiFiltersChangeEvent;
}(Subject_1.Subject));
exports.ApiFiltersChangeEvent = ApiFiltersChangeEvent;
//import {CustomerEventEmitter} from './customer-event-emitter';
var PubSubFiltersChangeEventService = /** @class */ (function () {
    function PubSubFiltersChangeEventService() {
        if (this.Stream == null) {
            this.Stream = new ApiFiltersChangeEvent();
        }
    }
    return PubSubFiltersChangeEventService;
}());
exports.PubSubFiltersChangeEventService = PubSubFiltersChangeEventService;
//# sourceMappingURL=ApiFiltersChangeEvent.js.map