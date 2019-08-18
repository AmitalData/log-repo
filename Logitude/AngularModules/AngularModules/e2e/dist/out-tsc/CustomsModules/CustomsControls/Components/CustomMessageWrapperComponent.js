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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var CustomSendOptionsComponent_1 = require("./CustomSendOptionsComponent");
//import { CustomSendOptionsComponent } from './CustomSendOptionsComponent'
var CommunicationLogStepListService_1 = require("../../../Common/Services/ExtendedLists/CommunicationLogStepListService");
var CustomMessageWrapperComponent = /** @class */ (function (_super) {
    __extends(CustomMessageWrapperComponent, _super);
    function CustomMessageWrapperComponent() {
        var _this = _super.call(this) || this;
        _this._ValidationErrorsList = [];
        _this.CustomSendOptionsButtonCanForcePersonalSign = false;
        _this.IsShowCustomResponseContent = true;
        _this.IsShowCustomToolBar = true;
        //@Output()
        //private CustomSendOptionsClick: EventEmitter<CustomSendOptionsArgs> = new EventEmitter<CustomSendOptionsArgs>();
        _this.MyCustomSendOptionsComponent = new CustomSendOptionsComponent_1.CustomSendOptionsComponent(null, null);
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._AfterContentInit = false;
        _this._IsDisableToggle = false;
        _this.MyLastCustomsRequestSheetId = null;
        _this.MyCustomsMenuItem = null;
        _this.MyCommunicationLogId = null;
        return _this;
    }
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "ValidationErrorsList", {
        get: function () { return this._ValidationErrorsList; },
        set: function (newValue) {
            this._ValidationErrorsList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "CustomSendOptionsButtonIsDisable", {
        //@Input()
        //public CanExportExcel: boolean = false;
        get: function () { return this.MyCustomSendOptionsComponent ? this.MyCustomSendOptionsComponent.IsDisabled : null; },
        set: function (newValue) {
            this.MyCustomSendOptionsComponent.IsDisabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    CustomMessageWrapperComponent.prototype.ngAfterContentInit = function () {
        //////alert(this.MyCustomSendOptionsComponent);
        this._AfterContentInit = true;
    };
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "FormTitle", {
        get: function () {
            if (this.CurrentSession.CurrentWindow) {
                return this.CurrentSession.CurrentWindow.Title;
            }
            else {
                return "";
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomMessageWrapperComponent.prototype.MessageDisplayIsDisableToggle = function () {
        this._IsDisableToggle = !this._IsDisableToggle;
        this.CustomRequestContentIsDisable = this.CustomResponseContentIsDisable = this.CustomSendOptionsButtonIsDisable = this._IsDisableToggle;
    };
    CustomMessageWrapperComponent.prototype.ExportExcel = function () {
        //this._IsDisableToggle = !this._IsDisableToggle;
        console.log(this.MyLastCustomsRequestSheetId);
        var communicationLogStepListService = new CommunicationLogStepListService_1.CommunicationLogStepListService();
        var mainInterfaceCode, RequestId, tenant;
        //communicationLogStepListService.GetExportExcelByRequestId("8305", this.MyLastCustomsRequestSheetId, SessionLocator.Tenant);
        //http://localhost:9996/api/CommunicationLogStep/GetExportExcelByRequestId/?mainInterfaceCode=8305&requestId=3333&tenant=2
        var url = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MyCommunicationLogId)) {
            url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogStep/GetExportExcelByLogId/?mainInterfaceCode=' + this.MyCustomsMenuItem.MainInterfaceCode + '&logId=' + this.MyCommunicationLogId + '&tenant=' + SessionLocator_1.SessionLocator.Tenant.toString();
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MyLastCustomsRequestSheetId)) {
            url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogStep/GetExportExcelByRequestId/?mainInterfaceCode=' + this.MyCustomsMenuItem.MainInterfaceCode + '&requestId=' + this.MyLastCustomsRequestSheetId + '&tenant=' + SessionLocator_1.SessionLocator.Tenant.toString();
        }
        window.open(url);
    };
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "RequestParams", {
        get: function () { return this._RequestParams; },
        set: function (newValue) {
            this._RequestParams = newValue;
            ;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "ResponseData", {
        get: function () { return this._ResponseData; },
        set: function (newValue) {
            this._ResponseData = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "ShowCustomResponseContent", {
        get: function () { return this.IsShowCustomResponseContent; },
        set: function (newValue) {
            if (this.IsShowCustomResponseContent != newValue) {
                this.IsShowCustomResponseContent = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomMessageWrapperComponent.prototype, "ShowCustomToolBar", {
        get: function () { return this.IsShowCustomToolBar; },
        set: function (newValue) {
            if (this.IsShowCustomToolBar != newValue) {
                this.IsShowCustomToolBar = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomMessageWrapperComponent.prototype.DisposeMyState = function () {
        this._RequestParams = null;
        this._ResponseData = null;
        this.MyCustomSendOptionsComponent = null;
    };
    CustomMessageWrapperComponent.prototype.CancelButtonClickedBase = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array),
        __metadata("design:paramtypes", [Array])
    ], CustomMessageWrapperComponent.prototype, "ValidationErrorsList", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomMessageWrapperComponent.prototype, "CustomSendOptionsButtonCanForcePersonalSign", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomMessageWrapperComponent.prototype, "IsShowCustomResponseContent", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomMessageWrapperComponent.prototype, "IsShowCustomToolBar", void 0);
    __decorate([
        core_1.ViewChild(CustomSendOptionsComponent_1.CustomSendOptionsComponent),
        __metadata("design:type", CustomSendOptionsComponent_1.CustomSendOptionsComponent)
    ], CustomMessageWrapperComponent.prototype, "MyCustomSendOptionsComponent", void 0);
    CustomMessageWrapperComponent = __decorate([
        core_1.Component({
            selector: 'custom-message-wrapper',
            moduleId: module.id,
            templateUrl: '././CustomMessageWrapperComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomMessageWrapperComponent);
    return CustomMessageWrapperComponent;
}(BaseComponent_1.BaseComponent
//implements OnInit, AfterContentInit, AfterViewInit,IRequestsSheetMassagingView 
));
exports.CustomMessageWrapperComponent = CustomMessageWrapperComponent;
//# sourceMappingURL=CustomMessageWrapperComponent.js.map