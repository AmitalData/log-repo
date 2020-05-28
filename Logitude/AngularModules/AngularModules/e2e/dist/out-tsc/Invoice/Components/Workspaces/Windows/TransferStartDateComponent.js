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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var InvoiceDomainService_1 = require("../../../Services/InvoiceDomainService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var TransferStartDateComponent = /** @class */ (function (_super) {
    __extends(TransferStartDateComponent, _super);
    function TransferStartDateComponent() {
        var _this = _super.call(this) || this;
        _this.Code = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingSetting";
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isOkButtonClicked = false;
        _this.stepCount = 10;
        _this.sentCount = 0;
        _this.allEntitiesCount = 0;
        _this.entitiesIdsList = [];
        _this.NoDataText = null;
        _this.UpdatedDataText = null;
        _this.IsNoDataTextVisible = false;
        _this.IsUpdateTextVisible = false;
        _this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        return _this;
    }
    TransferStartDateComponent.prototype.SetWindowArgs = function (args) {
        this.Code = args['Code'];
        this.EntityPM = args['EntityPM'];
        switch (this.Code) {
            case "ARInvoice":
            case "APInvoice":
                {
                    this.singleEntityName = "invoice";
                    this.pluralEntityName = "invoices";
                    break;
                }
            case "ARPayment":
            case "APPayment":
                {
                    this.singleEntityName = "payment";
                    this.pluralEntityName = "payments";
                    break;
                }
        }
        this.IsResourcesReady = true;
        this.Clone();
    };
    Object.defineProperty(TransferStartDateComponent.prototype, "ARInvoiceTransferStartDate", {
        get: function () { return this.EntityPM.ARInvoiceTransferStartDate; },
        set: function (value) {
            if (this.EntityPM.ARInvoiceTransferStartDate != value) {
                this.EntityPM.ARInvoiceTransferStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferStartDateComponent.prototype, "APInvoiceTransferStartDate", {
        get: function () { return this.EntityPM.APInvoiceTransferStartDate; },
        set: function (value) {
            if (this.EntityPM.APInvoiceTransferStartDate != value) {
                this.EntityPM.APInvoiceTransferStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransferStartDateComponent.prototype, "ARPaymentTransferStartDate", {
        get: function () { return this.EntityPM.ARPaymentTransferStartDate; },
        set: function (value) {
            if (this.EntityPM.ARPaymentTransferStartDate != value) {
                this.EntityPM.ARPaymentTransferStartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TransferStartDateComponent.prototype.CloseButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    TransferStartDateComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!this.isOkButtonClicked) {
            this.isOkButtonClicked = true;
            var errors = [];
            var myStartDate = null;
            switch (this.Code) {
                case "ARInvoice": {
                    myStartDate = this.ARInvoiceTransferStartDate;
                    break;
                }
                case "APInvoice": {
                    myStartDate = this.APInvoiceTransferStartDate;
                    break;
                }
                case "ARPayment": {
                    myStartDate = this.ARPaymentTransferStartDate;
                    break;
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(myStartDate)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
                errors.push(msg.replace("%FieldName", "Start date"));
            }
            this.ValidationErrorsList = errors;
            if (errors.length == 0) {
                var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
                myConfirmWindow.Width = 400;
                myConfirmWindow.Show(this.GetConfirmMessage(myStartDate));
                myConfirmWindow.WindowClosed.subscribe(function (event) {
                    if (myConfirmWindow.Yes) {
                        _this.UpdateSystemStartDate(myStartDate);
                    }
                    else {
                        _this.ReApplyOkButton();
                    }
                });
            }
            else {
                this.ReApplyOkButton();
            }
        }
    };
    TransferStartDateComponent.prototype.ReApplyOkButton = function () {
        this.isOkButtonClicked = false;
        this.CurrentSession.StopBusyIndicator();
    };
    TransferStartDateComponent.prototype.GetConfirmMessage = function (myStartDate) {
        var myResult = "";
        var myDateString = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ARPaymentTransferStartDate)) {
            myDateString = Tools_1.DateTool.GetDateFormats(this.ARPaymentTransferStartDate).ShortDateString;
        }
        switch (this.Code) {
            case "ARInvoice":
                {
                    myResult = "Please note that all the AR invoices with an invoice date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }
            case "APInvoice":
                {
                    myResult = "Please note that all the AP invoices with an invoice date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }
            case "ARPayment":
                {
                    myResult = "Please note that all the AR payments with a register date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }
            case "APPayment":
                {
                    myResult = "Please note that all the AP payments with a register date smaller than " + myDateString + ", will be updated and marked as blocked for transfer";
                    break;
                }
        }
        return myResult;
    };
    TransferStartDateComponent.prototype.UpdateSystemStartDate = function (myStartDate) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Calculating data...");
        this.invoiceDomainService.SetAccountingSettingStartDate(this.Code, myStartDate).subscribe(function (myResponse1) {
            if (myResponse1.HasError) {
                _this.ValidationErrorsList = myResponse1.ErrorsArray;
                _this.ReApplyOkButton();
            }
            else {
                _this.Clone();
                _this.invoiceDomainService.GetOnStartDateEntitiesIds(_this.Code, myStartDate).subscribe(function (myResponse2) {
                    _this.sentCount = 0;
                    _this.entitiesIdsList = [];
                    _this.UpdatedDataText = "";
                    _this.IsNoDataTextVisible = false;
                    _this.IsUpdateTextVisible = false;
                    if (!myResponse2.HasError) {
                        _this.entitiesIdsList = myResponse2.Result;
                    }
                    _this.allEntitiesCount = _this.entitiesIdsList.length;
                    if (_this.allEntitiesCount == 0) {
                        _this.NoDataText = "No " + _this.pluralEntityName + " smaller than this date";
                        _this.IsNoDataTextVisible = true;
                        _this.ReApplyOkButton();
                    }
                    else {
                        _this.IsUpdateTextVisible = true;
                        _this.Blocking();
                    }
                });
            }
        });
    };
    TransferStartDateComponent.prototype.Blocking = function () {
        var _this = this;
        if (this.entitiesIdsList.length == 0) {
            if (this.sentCount == 1) {
                this.UpdatedDataText = "1 " + this.singleEntityName + " has been blocked";
            }
            else {
                this.UpdatedDataText = this.sentCount + " " + this.pluralEntityName + " have been blocked";
            }
            this.ReApplyOkButton();
        }
        else {
            var idsList = [];
            this.entitiesIdsList.forEach(function (id) {
                if (idsList.length < _this.stepCount) {
                    idsList.push(id);
                }
            });
            idsList.forEach(function (id) {
                var indexOfId = _this.entitiesIdsList.indexOf(id);
                if (indexOfId > -1) {
                    _this.entitiesIdsList.splice(indexOfId, 1);
                }
            });
            this.sentCount = this.sentCount + idsList.length;
            this.CurrentSession.StartBusyIndicator("Blocking " + this.sentCount + " from " + this.allEntitiesCount + " " + this.pluralEntityName);
            this.invoiceDomainService.BlockTransferEntities(idsList, this.Code).subscribe(function (myResponse) {
                _this.Blocking();
            });
        }
    };
    TransferStartDateComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('ARInvoiceTransferStartDate');
        this.myCloner.AddField('APInvoiceTransferStartDate');
        this.myCloner.AddField('ARPaymentTransferStartDate');
        this.myCloner.AddEntity(this.EntityPM);
    };
    TransferStartDateComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    TransferStartDateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TransferStartDateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TransferStartDateComponent);
    return TransferStartDateComponent;
}(BaseComponent_1.BaseComponent));
exports.TransferStartDateComponent = TransferStartDateComponent;
//# sourceMappingURL=TransferStartDateComponent.js.map