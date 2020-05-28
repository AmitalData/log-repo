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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var InvoiceDomainService_1 = require("../../Services/InvoiceDomainService");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ARInvoiceMenuButtonsComponent = /** @class */ (function (_super) {
    __extends(ARInvoiceMenuButtonsComponent, _super);
    function ARInvoiceMenuButtonsComponent() {
        var _this = _super.call(this) || this;
        _this.EntityPM = null;
        _this.EventCode = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "ARInvoice";
        _this.OkButtonLabel = "Ok";
        _this.IsOkButtonEnabled = true;
        _this.ValidationErrorsList = [];
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        return _this;
    }
    ARInvoiceMenuButtonsComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args["EntityPM"];
        this.EventCode = args["EventCode"];
        this.InitializeComponent();
        this.SetButtonLabel();
        this.SetButtonEnabled();
        this.Clone();
    };
    ARInvoiceMenuButtonsComponent.prototype.InitializeComponent = function () {
        if (this.EventCode == "AutoCreditInvoiceDate") {
            this.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
    };
    ARInvoiceMenuButtonsComponent.prototype.SetButtonLabel = function () {
        switch (this.EventCode) {
            case "SetAsSent": {
                this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
                break;
            }
            default: {
                this.OkButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Ok");
                break;
            }
        }
    };
    ARInvoiceMenuButtonsComponent.prototype.SetButtonEnabled = function () {
        switch (this.EventCode) {
            case "AutoCreditInvoiceDate": {
                this.IsOkButtonEnabled = this.Date == null ? false : true;
                break;
            }
            case "AutoCreditManualNumber": {
                this.IsOkButtonEnabled = Tools_1.AppTool.IsNullOrEmpty(this.ManualNumber) ? false : true;
                break;
            }
            default: {
                this.IsOkButtonEnabled = true;
                break;
            }
        }
    };
    Object.defineProperty(ARInvoiceMenuButtonsComponent.prototype, "EventNote", {
        // Properties
        get: function () { return this.EntityPM.EventNote; },
        set: function (value) {
            if (this.EntityPM.EventNote != value) {
                this.EntityPM.EventNote = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceMenuButtonsComponent.prototype, "ManualNumber", {
        get: function () { return this.manualNumber; },
        set: function (value) {
            if (this.manualNumber != value) {
                this.manualNumber = value;
                this.SetButtonEnabled();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceMenuButtonsComponent.prototype, "Date", {
        get: function () { return this.date; },
        set: function (value) {
            if (this.date != value) {
                this.date = value;
                this.SetButtonEnabled();
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceMenuButtonsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    ARInvoiceMenuButtonsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        switch (this.EventCode) {
            case "AutoCreditManualNumber": {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ManualNumber)) {
                    errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.F.InvoiceNumber")));
                }
                break;
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.EventCode == "AutoCreditManualNumber") {
                this.CurrentSession.StartBusyIndicatorLoading();
                var myService = new InvoiceDomainService_1.InvoiceDomainService();
                myService.IsARInvoiceNumberExists(this.ManualNumber).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        var isExists = myResponse.Result;
                        if (isExists) {
                            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.S.AutoCreditingMsg5"));
                            _this.ValidationErrorsList = errors;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                    }
                });
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }
    };
    ARInvoiceMenuButtonsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('EventNote');
        this.myCloner.AddEntity(this.EntityPM);
    };
    ARInvoiceMenuButtonsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    ARInvoiceMenuButtonsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceMenuButtonsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ARInvoiceMenuButtonsComponent);
    return ARInvoiceMenuButtonsComponent;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceMenuButtonsComponent = ARInvoiceMenuButtonsComponent;
//# sourceMappingURL=ARInvoiceMenuButtonsComponent.js.map