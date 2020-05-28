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
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var APILogsErrorsComponent = /** @class */ (function (_super) {
    __extends(APILogsErrorsComponent, _super);
    function APILogsErrorsComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        if (window.innerWidth > 1380) {
            _this.MessageWidth = "1380px";
        }
        else {
            _this.MessageWidth = (window.innerWidth - 250).toString();
        }
        return _this;
    }
    APILogsErrorsComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.ExceptionMessage = this.EntityPM.LastExceptionMessage;
        }
    };
    APILogsErrorsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'APILogsErrorsComponent',
            templateUrl: './APILogsErrorsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], APILogsErrorsComponent);
    return APILogsErrorsComponent;
}(BaseComponent_1.BaseComponent));
exports.APILogsErrorsComponent = APILogsErrorsComponent;
//# sourceMappingURL=APILogsErrorsComponent.js.map