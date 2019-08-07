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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var PaymentOrderWebService_1 = require("../../../../../Customs/Services/WebServices/PaymentOrderWebService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var PaymentOrderPMService_1 = require("../../../../../Customs/Services/StandardPMs/PaymentOrderPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DeclarationPaymentOrderTabComponent = /** @class */ (function (_super) {
    __extends(DeclarationPaymentOrderTabComponent, _super);
    function DeclarationPaymentOrderTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customs.Declaration";
        _this.paymentOrderWebService = new PaymentOrderWebService_1.PaymentOrderWebService;
        _this.paymentOrderPMService = new PaymentOrderPMService_1.PaymentOrderPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.paymentOrderlist = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                _this.LoadPaymentOrders();
                _this.Listen();
            });
        });
        return _this;
    }
    DeclarationPaymentOrderTabComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
    };
    DeclarationPaymentOrderTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.LoadPaymentOrders();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCPO") {
                        //this.LoadPaymentOrders();
                    }
                }
            }));
        }
    };
    DeclarationPaymentOrderTabComponent.prototype.LoadPaymentOrders = function () {
        var _this = this;
        this.paymentOrderlist = new ObservableCollection_1.ObservableCollection([]);
        this.paymentOrderWebService.GetPaymentOrderByPaymentOrderConnection("D", this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.GetPaymentOrderByPaymentOrderConnectionOp_Completed(myResponse, false);
        });
    };
    DeclarationPaymentOrderTabComponent.prototype.GetPaymentOrderByPaymentOrderConnectionOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            var paymentOrderResult = myResponse.Result;
            if (paymentOrderResult.length > 1)
                paymentOrderResult.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.CreateDate) === Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.CreateDate) > Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1; });
        }
        paymentOrderResult.forEach(function (item) {
            _this.paymentOrderlist.Insert(item);
        });
    };
    DeclarationPaymentOrderTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.paymentOrderPMService.get(item.Id).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                var windowArgs = {};
                windowArgs.EntityPM = response.Result;
                windowArgs.declarationPM = _this.EntityPM;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 1200;
                logWindow.Height = 850;
                logWindow.ShowCloseButton = false;
                logWindow.ShowHeaderButtons = true;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(function ($event) {
                    _this.LoadPaymentOrders();
                });
                logWindow.ShowEditComponent(item.Id, "Customs.PaymentOrder", "POGN");
            });
        }
    };
    DeclarationPaymentOrderTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationPaymentOrderTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], DeclarationPaymentOrderTabComponent);
    return DeclarationPaymentOrderTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationPaymentOrderTabComponent = DeclarationPaymentOrderTabComponent;
//# sourceMappingURL=DeclarationPaymentOrderTabComponent.js.map