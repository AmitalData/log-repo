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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var PaymentMessagesService_1 = require("../../../../Customs/Services/WebServices/PaymentMessagesService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var NewPaymentRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/NewPaymentRequestParams");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewPaymentOrderComponent = /** @class */ (function (_super) {
    __extends(NewPaymentOrderComponent, _super);
    function NewPaymentOrderComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.PaymentOrder";
        _this._PaymentMessagesService = new PaymentMessagesService_1.PaymentMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.QueryNameText = "";
        EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) { });
        return _this;
    }
    NewPaymentOrderComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    NewPaymentOrderComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.QueryNameText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.NewPaymentOrder");
        }
    };
    NewPaymentOrderComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new NewPaymentRequestParams_1.NewPaymentRequestParams();
            //this.UIProperties.SetRequired("PaymentNumber", this.ObjectTableName, true);
            //this.UIProperties.SetRequired("ImporterId", this.ObjectTableName, true);
        }
    };
    Object.defineProperty(NewPaymentOrderComponent.prototype, "PaymentNumber", {
        //#region Properties
        get: function () { return this.RequestParams.PaymentNumber; },
        set: function (value) {
            if (this.RequestParams.PaymentNumber != value) {
                this.RequestParams.PaymentNumber = value;
            }
            if (value) {
                this.UIProperties.SetRequired("PaymentNumber", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("PaymentNumber", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewPaymentOrderComponent.prototype, "ImporterId", {
        get: function () { return this.RequestParams.ExternalId; },
        set: function (value) {
            if (this.RequestParams.ExternalId != value) {
                this.RequestParams.ExternalId = value;
            }
            //if (value) {
            //    this.UIProperties.SetRequired("ImporterId", this.ObjectTableName, false);
            //}
            //else {
            //    this.UIProperties.SetRequired("ImporterId", this.ObjectTableName, true);
            //}
        },
        enumerable: true,
        configurable: true
    });
    NewPaymentOrderComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewPaymentOrderComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.PaymentNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.NewPaymentOrder.O.PaymentNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ImporterId)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.ImporterNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    NewPaymentOrderComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new NewPaymentRequestParams_1.NewPaymentRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.PaymentNumber = this.PaymentNumber;
        currRequestParams.ExternalId = this.ImporterId;
        currRequestParams.RequestParamsVersion = 0;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליפת הוראת תשלום", false)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._PaymentMessagesService.PostNewPaymentRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], NewPaymentOrderComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    NewPaymentOrderComponent = __decorate([
        core_1.Component({
            selector: 'NewPaymentOrderComponent',
            moduleId: module.id,
            templateUrl: './NewPaymentOrderComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewPaymentOrderComponent);
    return NewPaymentOrderComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.NewPaymentOrderComponent = NewPaymentOrderComponent;
//# sourceMappingURL=NewPaymentOrderComponent.js.map