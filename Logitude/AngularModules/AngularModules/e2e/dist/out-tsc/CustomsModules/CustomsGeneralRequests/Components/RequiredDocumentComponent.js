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
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var PaymentOrderPM_1 = require("../../../Customs/EntityPMs/PaymentOrderPM");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var NewPaymentRequestParams_1 = require("../../../Customs/DataContract/RequestParams/NewPaymentRequestParams");
var RequiredDocumentResponseData_1 = require("../../../Customs/DataContract/ResponseData/RequiredDocumentResponseData");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var RequiredDocumentComponent = /** @class */ (function (_super) {
    __extends(RequiredDocumentComponent, _super);
    function RequiredDocumentComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new PaymentOrderPM_1.PaymentOrderPM();
        _this.isControlEnabled = true;
        _this.ValidationErrors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.ValidationErrors = [];
        _this.DocumentConnectedEntitiesList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(function (response) {
            });
        });
        return _this;
    }
    RequiredDocumentComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    RequiredDocumentComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    RequiredDocumentComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new NewPaymentRequestParams_1.NewPaymentRequestParams();
        }
        if (this.ResponseData) {
            if (this.ResponseData.DocumentConnectedEntitiesList) {
                this.DocumentConnectedEntitiesList.InsertCollection(this.ResponseData.DocumentConnectedEntitiesList);
            }
        }
        else {
            this.ResponseData = new RequiredDocumentResponseData_1.RequiredDocumentResponseData();
            this.ResponseData.DocumentConnectedEntitiesList = [];
        }
    };
    Object.defineProperty(RequiredDocumentComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "Title", {
        get: function () { return this.ResponseData ? this.ResponseData.Title : null; },
        set: function (newValue) { this.ResponseData.Title = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "VerificationDecisionType", {
        get: function () { return this.ResponseData ? this.ResponseData.VerificationDecisionType : null; },
        set: function (newValue) { this.ResponseData.VerificationDecisionType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "Remarks", {
        get: function () { return this.ResponseData ? this.ResponseData.Remarks : null; },
        set: function (newValue) { this.ResponseData.Remarks = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "DocumentTypeName", {
        get: function () { return this.ResponseData ? this.ResponseData.DocumentTypeName : null; },
        set: function (newValue) { this.ResponseData.DocumentTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "DocumentNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.DocumentNumber : null; },
        set: function (newValue) { this.ResponseData.DocumentNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "DocumentWorkerName", {
        get: function () { return this.ResponseData ? this.ResponseData.DocumentWorkerName : null; },
        set: function (newValue) { this.ResponseData.DocumentWorkerName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "ReplacingDocumentId", {
        get: function () { return this.ResponseData ? this.ResponseData.ReplacingDocumentId : null; },
        set: function (newValue) { this.ResponseData.ReplacingDocumentId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "DocumentTypeVisibility", {
        get: function () { return this.ResponseData ? this.ResponseData.DocumentTypeVisibility : null; },
        set: function (newValue) { this.ResponseData.DocumentTypeVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RequiredDocumentComponent.prototype, "ReplacingDocumentIdVisibility", {
        get: function () { return this.ResponseData ? this.ResponseData.ReplacingDocumentIdVisibility : null; },
        set: function (newValue) { this.ResponseData.ReplacingDocumentIdVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    RequiredDocumentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RequiredDocumentComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], RequiredDocumentComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    RequiredDocumentComponent = __decorate([
        core_1.Component({
            selector: 'RequiredDocumentComponent',
            moduleId: module.id,
            templateUrl: './RequiredDocumentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], RequiredDocumentComponent);
    return RequiredDocumentComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.RequiredDocumentComponent = RequiredDocumentComponent;
//# sourceMappingURL=RequiredDocumentComponent.js.map