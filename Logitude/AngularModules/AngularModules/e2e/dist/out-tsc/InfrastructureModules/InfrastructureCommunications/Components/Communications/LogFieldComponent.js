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
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogFieldComponent = /** @class */ (function (_super) {
    __extends(LogFieldComponent, _super);
    function LogFieldComponent(entityArgs, cd) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.ObjectTableName = "Customs.CommunicationLog";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        return _this;
    }
    LogFieldComponent.prototype.ngOnInit = function () {
        ///this.BuildColumns();
        var a = 1;
    };
    LogFieldComponent.prototype.SetTabArgs = function (args) {
        this._CommunicationLogStepDataViewModel = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        this._inputLogId = "_inputLogId";
    };
    LogFieldComponent.prototype.CopyLog2Clipboard = function () {
        var temp = document.getElementById(this._inputLogId);
        //temp.value = this._Log;
        temp.select();
        document.execCommand("copy");
    };
    //SetWindowArgs(args: any) {
    //    this.EntityPM = args; ///CommunicationLogStepDataViewModel
    //    if (this.EntityPM)
    //    {
    //        this._Log = this.EntityPM.Log;
    //    }
    //    this.Tab = args.Tab;
    //    this.IsDisplayOnly = args.Disabled;
    //}
    LogFieldComponent.prototype.SetWindowArgs = function (ShowLog) {
        this._Log = ShowLog;
        //this.Tab = args.Tab;
        //this.IsDisplayOnly = args.Disabled;
    };
    LogFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'communication-LogField',
            templateUrl: './LogFieldComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], LogFieldComponent);
    return LogFieldComponent;
}(BaseComponent_1.BaseComponent));
exports.LogFieldComponent = LogFieldComponent;
//# sourceMappingURL=LogFieldComponent.js.map