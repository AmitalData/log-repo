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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
//import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
var CommunicationLogListService_1 = require("../../../../Common/Services/StandardLists/CommunicationLogListService");
var CommunicationMoreComponent = /** @class */ (function (_super) {
    __extends(CommunicationMoreComponent, _super);
    function CommunicationMoreComponent(entityArgs, cd) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.ObjectTableName = "Customs.CommunicationLog";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.CorrelationID = "";
        _this.ExternalID = "";
        _this.Logs = "";
        _this.ExceptionMessage = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._input2CopyToClipboardId = "_input2CopyToClipboardId";
        _this._CommunicationLogListService = new CommunicationLogListService_1.CommunicationLogListService();
        return _this;
    }
    CommunicationMoreComponent.prototype.ngOnInit = function () {
        ///this.BuildColumns();
    };
    CommunicationMoreComponent.prototype.SetTabArgs = function (args) {
        //'Id': CustomsRequestsSheetList.RequestComminicationId,
        //'Tenant': CustomsRequestsSheetList.Tenant,
        //'CorrelationId': CustomsRequestsSheetList.CorrelationId,
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        if (this._CommunicationLogList == null) {
            this.LoadCommunicationLog();
        }
        console.log("arg");
    };
    CommunicationMoreComponent.prototype.Copy2Clipboard = function (token) {
        console.log("Copy2Clipboard..");
        var temp = document.getElementById(this._input2CopyToClipboardId);
        switch (token) {
            case "CorrelationID": {
                temp.value = this.CorrelationID;
                ;
                break;
            }
            case "ExternalID": {
                temp.value = this.ExternalID;
                ;
                break;
            }
            case "Logs": {
                temp.value = this.Logs;
                ;
                break;
            }
            case "ExceptionMessage": {
                temp.value = this.ExceptionMessage;
                ;
                break;
            }
        }
        temp.select();
        document.execCommand("copy");
    };
    CommunicationMoreComponent.prototype.LoadCommunicationLog = function () {
        var _this = this;
        this._CommunicationLogListService.getSingle(this.EntityPM.Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                _this._CommunicationLogList = result;
                _this.CorrelationID = _this._CommunicationLogList.CorrelationID || _this.EntityPM.CorrelationId;
                _this.ExternalID = _this.EntityPM.CustomsRequestsSheetId;
                _this.Logs = _this._CommunicationLogList.Logs;
                _this.ExceptionMessage = _this._CommunicationLogList.ExceptionMessage;
                // this.CurrentSession.StopBusyIndicator();
            }
            else {
                // this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    CommunicationMoreComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'communication-steps',
            templateUrl: './CommunicationMoreComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], CommunicationMoreComponent);
    return CommunicationMoreComponent;
}(BaseComponent_1.BaseComponent));
exports.CommunicationMoreComponent = CommunicationMoreComponent;
//# sourceMappingURL=CommunicationMoreComponent.js.map