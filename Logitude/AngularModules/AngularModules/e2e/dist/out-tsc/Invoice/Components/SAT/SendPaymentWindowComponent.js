"use strict";
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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var InvoiceDomainService_1 = require("../../Services/InvoiceDomainService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var SendPaymentWindowComponent = /** @class */ (function () {
    function SendPaymentWindowComponent() {
        this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        this.IsSendButtonVisible = false;
        this.IsPaymentValid = false;
        this.IsPaymentWarning = false;
        this.IsPaymentError = false;
        this.PaymentWarningText = null;
        this.PaymentErrorText = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsInvoicesValid = false;
        this.IsInvoicesWarning = false;
        this.IsInvoicesError = false;
        this.InvoicesWarningText = null;
        this.InvoicesErrorText = null;
        this.IsInvoicesStatusVisible = true;
        this.ARInvoiceSATStatusList = [];
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
    }
    SendPaymentWindowComponent.prototype.SetWindowArgs = function (args) {
        this.Tenant = args.EnttiyPM.Tenant;
        this.entityPM = args.EnttiyPM;
        if (this.entityPM.MetodoPagoCode == "PUE") {
            this.IsInvoicesStatusVisible = false;
            this.IsPaymentValid = false;
            this.IsPaymentError = true;
            this.PaymentErrorText = "Invalid";
            this.ValidationErrorsList.push("You can only send payments to SAT when the Metodo Pago value is PPD");
            return;
        }
        if (this.entityPM.StatusCode == "AD" || this.entityPM.StatusCode == "CL") {
            if (this.entityPM.PaymentInvoices.length > 0) {
                this.IsPaymentValid = true;
                this.LoadARInvoiceSATStatus();
            }
            else {
                this.IsInvoicesStatusVisible = false;
                this.IsPaymentValid = false;
                this.IsPaymentError = true;
                this.PaymentErrorText = "Invalid";
                this.ValidationErrorsList.push("The payment should be connected to one invoice at least");
            }
        }
        else {
            this.IsInvoicesStatusVisible = false;
            this.IsPaymentValid = false;
            this.IsPaymentError = true;
            this.PaymentErrorText = "Invalid";
            this.ValidationErrorsList.push("The payment must be approved before sending it to SAT");
        }
    };
    Object.defineProperty(SendPaymentWindowComponent.prototype, "InvoicesStatusLabel", {
        get: function () {
            var myResult = "";
            myResult = "Invoice(s) Status";
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    SendPaymentWindowComponent.prototype.LoadARInvoiceSATStatus = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        this.invoiceDomainService.GetARInvoiceSATStatus(this.entityPM.Id).subscribe(function (response) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (response != null) {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        myResult.forEach(function (item) {
                            _this.ARInvoiceSATStatusList.push(new ARInvoiceSATStatusItemViewModel(item, _this));
                        });
                    }
                    if (!_this.ARInvoiceSATStatusList.some(function (a) { return a.IsSATValid == false; })) {
                        _this.IsPaymentValid = true;
                        _this.IsInvoicesValid = true;
                        _this.IsSendButtonVisible = true;
                    }
                    else {
                        _this.ValidationErrorsList.push("Some invoices are not approved from SAT");
                        _this.IsInvoicesError = true;
                        _this.InvoicesErrorText = "Invalid";
                    }
                }
                else {
                    _this.IsInvoicesStatusVisible = false;
                    _this.IsPaymentValid = false;
                    _this.IsPaymentError = true;
                    _this.PaymentErrorText = "Invalid";
                    _this.ValidationErrorsList = response.ErrorsArray;
                    //this.IsInvoicesValid = true;
                    //var messageWindow = new MessageWindow();
                    //messageWindow.Show(response.ErrorsArray.toString());
                }
            }
        });
    };
    SendPaymentWindowComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SendPaymentWindowComponent.prototype.SendClicked = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.invoiceDomainService.SendARPaymentSATXML(this.entityPM.Id).subscribe(function (response) {
            if (response != null) {
                if (!response.HasError) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.CurrentSession.CurrentWindow.Close("");
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(response.ErrorsArray.toString());
                }
            }
        });
    };
    SendPaymentWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SendPaymentWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SendPaymentWindowComponent);
    return SendPaymentWindowComponent;
}());
exports.SendPaymentWindowComponent = SendPaymentWindowComponent;
var ARInvoiceSATStatusItemViewModel = /** @class */ (function () {
    function ARInvoiceSATStatusItemViewModel(item, fatherComponent) {
        this.item = item;
        this.fatherComponent = fatherComponent;
        this.IsSATValid = false;
        this.BillTo = item.BillTo;
        this.SATStatusName = item.SATStatusName;
        this.ARInvoiceNumber = item.ARInvoiceNumber;
        this.IsSATValid = item.IsSATValid;
        this.SATStatusCode = item.SATStatusCode;
    }
    ARInvoiceSATStatusItemViewModel.prototype.ViewShipment = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Edit HAWB Wizard";
        logWindow.WindowArgs = this.item.ARInvoiceNumber;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.fatherComponent.LoadARInvoiceSATStatus(); });
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
    };
    return ARInvoiceSATStatusItemViewModel;
}());
exports.ARInvoiceSATStatusItemViewModel = ARInvoiceSATStatusItemViewModel;
//# sourceMappingURL=SendPaymentWindowComponent.js.map