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
var CustomMessageWrapperComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PaymentOrderPM_1 = require("../../../../Customs/EntityPMs/PaymentOrderPM");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var NewPaymentRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/NewPaymentRequestParams");
var PaymentOrderReplyResponseData_1 = require("../../../../Customs/DataContract/ResponseData/PaymentOrderReplyResponseData");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var PaymentOrderReplyComponent = /** @class */ (function (_super) {
    __extends(PaymentOrderReplyComponent, _super);
    function PaymentOrderReplyComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new PaymentOrderPM_1.PaymentOrderPM();
        _this.ObjectTableName = "Customs.PaymentOrder";
        _this.isControlEnabled = true;
        _this.ValidationErrors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.ValidationErrors = [];
        _this.PaymentDetailDataList = new ObservableCollection_1.ObservableCollection([]);
        _this.ConnectedEntityDataList = new ObservableCollection_1.ObservableCollection([]);
        _this.TaxParagraphList = new ObservableCollection_1.ObservableCollection([]);
        _this.PaymentMethodsList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(function (response) {
            });
        });
        return _this;
    }
    PaymentOrderReplyComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    PaymentOrderReplyComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    PaymentOrderReplyComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new NewPaymentRequestParams_1.NewPaymentRequestParams();
        }
        if (this.ResponseData) {
            if (this.ResponseData.PaymentDetailData) {
                this.PaymentDetailDataList.InsertCollection(this.ResponseData.PaymentDetailData);
            }
            if (this.ResponseData.ConnectedEntityData) {
                this.ConnectedEntityDataList.InsertCollection(this.ResponseData.ConnectedEntityData);
            }
            if (this.ResponseData.TaxParagraphList) {
                this.TaxParagraphList.InsertCollection(this.ResponseData.TaxParagraphList);
            }
            if (this.ResponseData.PaymentMethodsList) {
                this.PaymentMethodsList.InsertCollection(this.ResponseData.PaymentMethodsList);
            }
        }
        else {
            this.ResponseData = new PaymentOrderReplyResponseData_1.PaymentOrderReplyResponseData();
            this.ResponseData.PaymentDetailData = [];
            this.ResponseData.ConnectedEntityData = [];
            this.ResponseData.TaxParagraphList = [];
            this.ResponseData.PaymentMethodsList = [];
        }
    };
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentNumberHeader", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.PaymentNumber)) {
                return "הוראת תשלום" + " " + this.ResponseData.PaymentNumber;
            }
            return "הוראת תשלום";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentOrderTotalSumToPay", {
        get: function () { return this.ResponseData ? this.ResponseData.PaymentOrderTotalSumToPay : null; },
        set: function (newValue) { this.ResponseData.PaymentOrderTotalSumToPay = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "UserMessage", {
        get: function () { return this.ResponseData ? this.ResponseData.UserMessage : null; },
        set: function (newValue) { this.ResponseData.UserMessage = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentStatusName", {
        get: function () { return this.ResponseData ? this.ResponseData.PaymentStatusName : null; },
        set: function (newValue) { this.ResponseData.PaymentStatusName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentOrderTypeName", {
        get: function () { return this.ResponseData ? this.ResponseData.PaymentOrderTypeName : null; },
        set: function (newValue) { this.ResponseData.PaymentOrderTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentOrderPayDate", {
        get: function () { return this.ResponseData ? this.ResponseData.PaymentOrderPayDate : null; },
        set: function (newValue) { this.ResponseData.PaymentOrderPayDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentDetailDataCustomerActivityTypeExternalID", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.PaymentDetailData)) {
                var text = "";
                if (this.ResponseData.PaymentDetailData.CustomerActivityType != null) {
                    text = this.ResponseData.PaymentDetailData.CustomerActivityTypeName;
                }
                text = text + " - ";
                if (this.ResponseData.PaymentDetailData.ExternalID != null) {
                    text = this.ResponseData.PaymentDetailData.ExternalID.toString();
                }
                return text;
            }
            return "";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentProcessName", {
        get: function () { return this.ResponseData ? this.ResponseData.PaymentProcessName : null; },
        set: function (newValue) { this.ResponseData.PaymentProcessName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "CustomsHouseName", {
        get: function () { return this.ResponseData ? this.ResponseData.CustomsHouseName : null; },
        set: function (newValue) { this.ResponseData.CustomsHouseName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "PaymentOrderReason", {
        get: function () { return this.ResponseData ? this.ResponseData.PaymentOrderReason : null; },
        set: function (newValue) { this.ResponseData.PaymentOrderReason = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "EntityTypeName", {
        get: function () { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityTypeName : null; },
        set: function (newValue) { this.ResponseData.ConnectedEntityData.EntityTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "EntityIdKey1", {
        get: function () { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityIdKey1 : null; },
        set: function (newValue) { this.ResponseData.ConnectedEntityData.EntityIdKey1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "EntityIdKey2", {
        get: function () { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityIdKey2 : null; },
        set: function (newValue) { this.ResponseData.ConnectedEntityData.EntityIdKey2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrderReplyComponent.prototype, "EntityIdKey3", {
        get: function () { return this.ResponseData ? this.ResponseData.ConnectedEntityData.EntityIdKey3 : null; },
        set: function (newValue) { this.ResponseData.ConnectedEntityData.EntityIdKey3 = newValue; },
        enumerable: true,
        configurable: true
    });
    PaymentOrderReplyComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PaymentOrderReplyComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], PaymentOrderReplyComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    PaymentOrderReplyComponent = __decorate([
        core_1.Component({
            selector: 'PaymentOrderReplyComponent',
            moduleId: module.id,
            templateUrl: './PaymentOrderReplyComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PaymentOrderReplyComponent);
    return PaymentOrderReplyComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.PaymentOrderReplyComponent = PaymentOrderReplyComponent;
//# sourceMappingURL=PaymentOrderReplyComponent.js.map