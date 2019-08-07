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
var PaymentMessagesService_1 = require("../../../../Customs/Services/WebServices/PaymentMessagesService");
var MasavPaymentsToAgentRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/MasavPaymentsToAgentRequestParams");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var MasavPaymentsToAgentComponent = /** @class */ (function (_super) {
    __extends(MasavPaymentsToAgentComponent, _super);
    function MasavPaymentsToAgentComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._PaymentMessagesService = new PaymentMessagesService_1.PaymentMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.AgentMasavPaymentResultList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    MasavPaymentsToAgentComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new MasavPaymentsToAgentRequestParams_1.MasavPaymentsToAgentRequestParams();
            this.UIProperties.SetRequired("PaymentDate", null, true);
        }
        if (this.ResponseData && this.ResponseData.AgentMasavPaymentResultList) {
            for (var _i = 0, _a = this.ResponseData.AgentMasavPaymentResultList; _i < _a.length; _i++) {
                var item = _a[_i];
                item.RelatedEntityListObs = new ObservableCollection_1.ObservableCollection(item.RelatedEntityList);
            }
            this.AgentMasavPaymentResultList.InsertCollection(this.ResponseData.AgentMasavPaymentResultList);
        }
    };
    MasavPaymentsToAgentComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    MasavPaymentsToAgentComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    };
    Object.defineProperty(MasavPaymentsToAgentComponent.prototype, "PaymentDate", {
        //#region Properties
        get: function () { return this.RequestParams.PaymentDate; },
        set: function (value) {
            if (this.RequestParams.PaymentDate != value) {
                this.RequestParams.PaymentDate = value;
            }
            if (value) {
                this.UIProperties.SetRequired("PaymentDate", null, false);
            }
            else {
                this.UIProperties.SetRequired("PaymentDate", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region General Commands
    MasavPaymentsToAgentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    MasavPaymentsToAgentComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.RequestParams.PaymentDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.MasavPaymentsToAgentQuery.O.PaymentDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    MasavPaymentsToAgentComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.AgentMasavPaymentResultList.Clear();
        var currRequestParams = new MasavPaymentsToAgentRequestParams_1.MasavPaymentsToAgentRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.PaymentDate = this.PaymentDate;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לבקשת דוח קופה לסוכן", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._PaymentMessagesService.PostMasavPaymentsToAgentRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], MasavPaymentsToAgentComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    MasavPaymentsToAgentComponent = __decorate([
        core_1.Component({
            selector: 'MasavPaymentsToAgentComponent',
            moduleId: module.id,
            templateUrl: './MasavPaymentsToAgentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MasavPaymentsToAgentComponent);
    return MasavPaymentsToAgentComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.MasavPaymentsToAgentComponent = MasavPaymentsToAgentComponent;
//# sourceMappingURL=MasavPaymentsToAgentComponent.js.map