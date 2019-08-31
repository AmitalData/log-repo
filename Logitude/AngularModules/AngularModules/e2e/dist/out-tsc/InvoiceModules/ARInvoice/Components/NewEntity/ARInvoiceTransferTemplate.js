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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var VatTypeListService_1 = require("../../../../Common/Services/StandardLists/VatTypeListService");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ARInvoicePMService_1 = require("../../../../Invoice/Services/StandardPMs/ARInvoicePMService");
var ARInvoiceTransferTemplate = /** @class */ (function (_super) {
    __extends(ARInvoiceTransferTemplate, _super);
    function ARInvoiceTransferTemplate() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.IsNew = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        return _this;
    }
    ARInvoiceTransferTemplate.prototype.InitTemplate = function (entity) {
        this.EntityPM = entity;
        this.BuildList();
    };
    ARInvoiceTransferTemplate.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.EntityId = args.EntityId;
            this.IsNew = args.IsNewTemplate;
            this.GetSingleEntityPM();
        }
    };
    ARInvoiceTransferTemplate.prototype.GetSingleEntityPM = function () {
        var _this = this;
        var service = new ARInvoicePMService_1.ARInvoicePMService();
        service.get(this.EntityId).subscribe(function (response) {
            if (!response.HasError) {
                var entityPM = response.Result;
                if (entityPM) {
                    _this.EntityPM = entityPM;
                    _this.BuildList();
                }
            }
        });
    };
    ARInvoiceTransferTemplate.prototype.RefreshData = function () {
        if (this.EntityPM != null) {
            this.BuildList();
        }
    };
    ARInvoiceTransferTemplate.prototype.BuildList = function () {
        var _this = this;
        this.ItemsSource = [];
        this.ItemsSource.push(new ARInvoiceTransferLineArgs(this.EntityPM, null, this, "BLTO"));
        this.ItemsSource.push(new ARInvoiceTransferLineArgs(this.EntityPM, null, this, "CURR"));
        this.EntityPM.InvoiceLines.forEach(function (item) {
            _this.ItemsSource.push(new ARInvoiceTransferLineArgs(null, item, _this, "Line"));
        });
        var listGrouped = [];
        this.EntityPM.InvoiceLines.filter(function (f) { return f.VatTypeId != null; }).forEach(function (item) {
            var itemGrouped = listGrouped.filter(function (f) { return f.VatTypeId == item.VatTypeId; })[0];
            if (itemGrouped == null) {
                itemGrouped = new InvoiceCodeNameClass();
                itemGrouped.VatTypeId = item.VatTypeId;
                listGrouped.push(itemGrouped);
            }
        });
        listGrouped.forEach(function (item) {
            var linePM = _this.EntityPM.InvoiceLines.filter(function (d) { return d.VatTypeId == item.VatTypeId && d.VatPercentage != null; })[0];
            if (linePM != null) {
                if (linePM.VatPercentage != 0) {
                    _this.ItemsSource.push(new ARInvoiceTransferLineArgs(null, linePM, _this, "TAX"));
                }
            }
        });
        this.UpdateTransferData();
    };
    //UpdateTransferData
    ARInvoiceTransferTemplate.prototype.UpdateTransferData = function () {
        if (this.EntityPM.TransferStatusCode != "TR" && this.EntityPM.TransferStatusCode != "IP" && this.EntityPM.TransferStatusCode != "ET") {
            var isReady = true;
            var isValid = this.ItemsSource.filter(function (d) { return Tools_1.AppTool.IsNullOrEmpty(d.EditingFieldValue); })[0];
            if (isValid != null) {
                isReady = false;
            }
            if (this.EntityPM.TransferStatusCode != "BL") {
                this.TransferStatusCode = isReady ? "RD" : "NR";
            }
        }
    };
    Object.defineProperty(ARInvoiceTransferTemplate.prototype, "IsEditingEnabled", {
        //Properties
        get: function () {
            var myResult = true;
            if (this.TransferStatusCode == "TR") {
                myResult = false;
            }
            else if (this.EntityPM != null && this.EntityPM.StatusCode == "LL") {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferTemplate.prototype, "SetBlockedButtonVisibility", {
        get: function () {
            var result = false;
            if (this.TransferStatusCode != "TR") {
                if (this.TransferStatusCode != "BL") {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferTemplate.prototype, "SetUnBlockedButtonVisibility", {
        get: function () {
            var result = false;
            if (this.TransferStatusCode != "TR") {
                if (this.TransferStatusCode == "BL") {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferTemplate.prototype, "TransferStatusCode", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.TransferStatusCode;
            }
        },
        set: function (value) {
            if (this.EntityPM.TransferStatusCode != value) {
                this.EntityPM.TransferStatusCode = value;
                switch (value) {
                    case "RD": {
                        this.TransferStatusName = "Ready";
                        break;
                    }
                    case "NR": {
                        this.TransferStatusName = "Not Ready";
                        break;
                    }
                    case "BL": {
                        this.TransferStatusName = "Blocked";
                        break;
                    }
                    default: {
                        this.TransferStatusName = "Transferred";
                        break;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferTemplate.prototype, "IsReadyForTransfer", {
        get: function () {
            return this.TransferStatusCode == "RD" ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferTemplate.prototype, "TransferStatusName", {
        get: function () {
            if (this.EntityPM != null && this.EntityPM.TransferStatusName != null) {
                return this.EntityPM.TransferStatusName;
            }
            else {
                return "Not Ready";
            }
        },
        set: function (value) {
            if (this.EntityPM.TransferStatusName != value) {
                this.EntityPM.TransferStatusName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Block Commands
    ARInvoiceTransferTemplate.prototype.SetBlockedTransferClicked = function () {
        this.TransferStatusCode = "BL";
        this.UpdateTransferData();
    };
    ARInvoiceTransferTemplate.prototype.SetUnBlockedTransferClicked = function () {
        this.TransferStatusCode = "NR";
        this.UpdateTransferData();
    };
    // Commands
    ARInvoiceTransferTemplate.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ARInvoiceTransferTemplate.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new ARInvoicePMService_1.ARInvoicePMService();
            service.update(this.EntityPM).subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    _this.ValidationErrorsList = response.ErrorsArray;
                }
            });
        }
    };
    ARInvoiceTransferTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceTransferTemplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], ARInvoiceTransferTemplate);
    return ARInvoiceTransferTemplate;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceTransferTemplate = ARInvoiceTransferTemplate;
var ARInvoiceTransferLineArgs = /** @class */ (function (_super) {
    __extends(ARInvoiceTransferLineArgs, _super);
    function ARInvoiceTransferLineArgs(entity, line, father, typeCode) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.DataContext = _this;
        // Props
        _this.DescriptionTitle = "";
        _this.DescriptionValue = "";
        _this.DescriptionHelp = "";
        _this.DescriptionHelpVisibility = false;
        _this.InitalizeServices();
        if (entity != null) {
            _this.invoicePM = entity;
            _this.ObjectTableName = "ARInvoice";
        }
        if (line != null) {
            _this.invoicePM = father.EntityPM;
            _this.invoiceLinePM = line;
            _this.ObjectTableName = "ARInvoiceLine";
            _this.VatPercentage = line.VatPercentage;
        }
        _this.Code = typeCode;
        _this.GetEditingFieldName();
        _this.GetDescriptionTitle();
        _this.GetDescriptionValue();
        _this.GetDescriptionHelp();
        _this.GetDescriptionHelpVisibility();
        _this.SetUIProperties();
        _this.FillFieldsData();
        return _this;
    }
    ARInvoiceTransferLineArgs.prototype.InitalizeServices = function () {
        this.CardListService = new CardListService_1.CardListService();
        this.CurrencyListService = new CurrencyListService_1.CurrencyListService();
        this.VatTypeListService = new VatTypeListService_1.VatTypeListService();
        this.PaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
        this.ChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.InvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
    };
    // FillFieldsData
    ARInvoiceTransferLineArgs.prototype.FillFieldsData = function () {
        if (this.IsTransferredEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
                switch (this.Code) {
                    case "BLTO":
                        {
                            this.GetBillToData();
                            break;
                        }
                    case "CURR":
                        {
                            this.GetCurrencyData();
                            break;
                        }
                    case "VAT":
                        {
                            this.GetVatTypeData();
                            break;
                        }
                    case "TAX":
                        {
                            if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "HV" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "RH") {
                                this.GetAccountingSystemData();
                            }
                            else {
                                this.GetVatTypeData();
                            }
                            break;
                        }
                    case "PYTM":
                        {
                            this.GetPaymentTermData();
                            break;
                        }
                    default: {
                        this.GetChargesTypeData();
                        break;
                    }
                }
            }
        }
    };
    ARInvoiceTransferLineArgs.prototype.GetBillToData = function () {
        var _this = this;
        this.CardListService.getSingle(this.invoicePM.BillToId).subscribe(function (response) {
            if (!response.HasError) {
                var card = response.Result;
                if (card != null) {
                    _this.EditingFieldValue = card.ReceivablesAccountingCard;
                }
            }
        });
    };
    ARInvoiceTransferLineArgs.prototype.GetCurrencyData = function () {
        var _this = this;
        this.CurrencyListService.getSingle(this.invoicePM.InvoiceCurrencyId).subscribe(function (response) {
            if (!response.HasError) {
                var currency = response.Result;
                if (currency != null) {
                    _this.EditingFieldValue = currency.AccountingExternalCode;
                }
            }
        });
    };
    ARInvoiceTransferLineArgs.prototype.GetVatTypeData = function () {
        var _this = this;
        this.VatTypeListService.getSingle(this.invoiceLinePM.VatTypeId).subscribe(function (response) {
            if (!response.HasError) {
                var vat = response.Result;
                if (vat != null) {
                    _this.EditingFieldValue = vat.ExternalTAXItemId;
                }
            }
        });
    };
    ARInvoiceTransferLineArgs.prototype.GetPaymentTermData = function () {
        var _this = this;
        this.PaymentTermListService.getSingle(this.invoicePM.PaymentTermId).subscribe(function (response) {
            if (!response.HasError) {
                var payment = response.Result;
                if (payment != null) {
                    _this.EditingFieldValue = payment.ExternalId;
                }
            }
        });
    };
    ARInvoiceTransferLineArgs.prototype.GetChargesTypeData = function () {
        var _this = this;
        this.ChargesTypeListService.getSingle(this.invoiceLinePM.ChargesTypeId).subscribe(function (response) {
            if (!response.HasError) {
                var charge = response.Result;
                if (charge != null) {
                    if (charge.AccountingVATSplit) {
                        _this.GetChargeTypeAccountingList();
                    }
                    else {
                        _this.EditingFieldValue = charge.ReceivableCreditAccount;
                    }
                }
            }
        });
    };
    ARInvoiceTransferLineArgs.prototype.GetChargeTypeAccountingList = function () {
        var _this = this;
        this.InvoiceDomainService.GetSingleChargeTypeAccountingList(this.invoiceLinePM.ChargesTypeId, this.invoiceLinePM.VatTypeId).subscribe(function (respo) {
            if (!respo.HasError) {
                var list = respo.Result;
                if (list != null) {
                    _this.EditingFieldValue = list.ReceivableCreditAccount;
                }
            }
        });
    };
    ARInvoiceTransferLineArgs.prototype.GetAccountingSystemData = function () {
        this.EditingFieldValue = SessionLocator_1.SessionLocator.AccountingSettingPM.ReceivableVATCard;
    };
    // SetUIProperties
    ARInvoiceTransferLineArgs.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled(this.EditingFieldName, this.ObjectTableName, this.IsTransferredEnabled);
    };
    ARInvoiceTransferLineArgs.prototype.GetDescriptionTitle = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "Bill To";
                    break;
                }
            case "CURR":
                {
                    result = "Invoice Currency";
                    break;
                }
            case "PYTM":
                {
                    result = "Payment Term";
                    break;
                }
            case "VAT":
                {
                    result = "VAT Type Item Code";
                    break;
                }
            case "TAX":
                {
                    result = "VAT Type Tax Code";
                    break;
                }
            default:
                {
                    result = "Charge Type";
                    break;
                }
        }
        this.DescriptionTitle = result;
    };
    ARInvoiceTransferLineArgs.prototype.GetDescriptionValue = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = this.invoicePM.BillToName;
                    break;
                }
            case "CURR":
                {
                    result = this.invoicePM.InvoiceCurrencyCode;
                    break;
                }
            case "PYTM":
                {
                    result = this.invoicePM.PaymentTermName;
                    break;
                }
            case "VAT":
                {
                    result = this.invoiceLinePM.VatTypeName;
                    break;
                }
            case "TAX":
                {
                    result = this.invoiceLinePM.VatTypeName;
                    break;
                }
            default:
                {
                    result = this.invoiceLinePM.Description;
                    break;
                }
        }
        this.DescriptionValue = result;
    };
    ARInvoiceTransferLineArgs.prototype.GetDescriptionHelp = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "Please enter debit account";
                    break;
                }
            case "CURR":
                {
                    result = "Please enter external code for " + this.invoicePM.InvoiceCurrencyCode;
                    break;
                }
            case "PYTM":
                {
                    result = "Please enter external payment term";
                    break;
                }
            case "VAT":
                {
                    result = "Please enter external vat card";
                    break;
                }
            case "TAX":
                {
                    result = "Please enter external tax code";
                    break;
                }
            default:
                {
                    result = "Please enter credit account";
                    break;
                }
        }
        this.DescriptionHelp = result;
    };
    ARInvoiceTransferLineArgs.prototype.GetDescriptionHelpVisibility = function () {
        var result = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
            result = true;
        }
        this.DescriptionHelpVisibility = result;
    };
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "IsTransferredEnabled", {
        get: function () {
            var myResult = true;
            if (this.father.TransferStatusCode == "TR") {
                myResult = false;
            }
            else if (this.invoicePM.StatusCode == "DR" || this.invoicePM.StatusCode == "LL") {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceTransferLineArgs.prototype.GetEditingFieldName = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "DebitAccount";
                    break;
                }
            case "CURR":
                {
                    result = "AccountingExternalCode";
                    break;
                }
            case "PYTM":
                {
                    result = "PaymentTermExternalId";
                    break;
                }
            case "VAT":
                {
                    result = "ExternalTAXItemId";
                    break;
                }
            case "TAX":
                {
                    result = "ExternalVATCard";
                    break;
                }
            default:
                {
                    result = "CreditAccount";
                    break;
                }
        }
        this.EditingFieldName = result;
    };
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "EditingFieldValue", {
        get: function () {
            var result = "";
            switch (this.Code) {
                case "BLTO":
                    {
                        result = this.invoicePM.DebitAccount;
                        break;
                    }
                case "CURR":
                    {
                        result = this.invoicePM.AccountingExternalCode;
                        break;
                    }
                case "PYTM":
                    {
                        result = this.invoicePM.PaymentTermExternalId;
                        break;
                    }
                case "VAT":
                    {
                        result = this.invoiceLinePM.ExternalTAXItemId;
                        break;
                    }
                case "TAX":
                    {
                        result = this.invoiceLinePM.ExternalVATCard;
                        break;
                    }
                default:
                    {
                        result = this.invoiceLinePM.CreditAccount;
                        break;
                    }
            }
            return result;
        },
        set: function (value) {
            var _this = this;
            switch (this.Code) {
                case "BLTO":
                    {
                        if (this.invoicePM.DebitAccount != value) {
                            this.invoicePM.DebitAccount = value;
                        }
                        break;
                    }
                case "CURR":
                    {
                        if (this.invoicePM.AccountingExternalCode != value) {
                            this.invoicePM.AccountingExternalCode = value;
                        }
                        break;
                    }
                case "PYTM":
                    {
                        if (this.invoicePM.PaymentTermExternalId != value) {
                            this.invoicePM.PaymentTermExternalId = value;
                        }
                        break;
                    }
                case "VAT":
                    {
                        if (this.invoiceLinePM.ExternalTAXItemId != value) {
                            this.invoiceLinePM.ExternalTAXItemId = value;
                            this.father.EntityPM.InvoiceLines.filter(function (d) { return d.VatTypeId == _this.invoiceLinePM.VatTypeId; }).forEach(function (item) {
                                item.ExternalTAXItemId = value;
                            });
                        }
                        break;
                    }
                case "TAX":
                    {
                        if (this.invoiceLinePM.ExternalVATCard != value) {
                            this.invoiceLinePM.ExternalVATCard = value;
                            this.father.EntityPM.InvoiceLines.filter(function (d) { return d.VatTypeId == _this.invoiceLinePM.VatTypeId; }).forEach(function (item) {
                                item.ExternalVATCard = value;
                            });
                        }
                        break;
                    }
                default:
                    {
                        if (this.invoiceLinePM.CreditAccount != value) {
                            this.invoiceLinePM.CreditAccount = value;
                        }
                        break;
                    }
            }
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "DebitAccount", {
        get: function () { return this.invoicePM.DebitAccount; },
        set: function (value) {
            if (this.invoicePM.DebitAccount != value) {
                this.invoicePM.DebitAccount = value;
                this.father.UpdateTransferData();
                this.GetDescriptionHelpVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "AccountingExternalCode", {
        get: function () { return this.invoicePM.AccountingExternalCode; },
        set: function (value) {
            if (this.invoicePM.AccountingExternalCode != value) {
                this.invoicePM.AccountingExternalCode = value;
                this.father.UpdateTransferData();
                this.GetDescriptionHelpVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "PaymentTermExternalId", {
        get: function () { return this.invoicePM.PaymentTermExternalId; },
        set: function (value) {
            if (this.invoicePM.PaymentTermExternalId != value) {
                this.invoicePM.PaymentTermExternalId = value;
                this.father.UpdateTransferData();
                this.GetDescriptionHelpVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "ExternalTAXItemId", {
        get: function () { return this.invoiceLinePM.ExternalTAXItemId; },
        set: function (value) {
            var _this = this;
            if (this.invoiceLinePM.ExternalTAXItemId != value) {
                this.invoiceLinePM.ExternalTAXItemId = value;
                this.father.EntityPM.InvoiceLines.filter(function (d) { return d.VatTypeId == _this.invoiceLinePM.VatTypeId; }).forEach(function (item) {
                    item.ExternalTAXItemId = value;
                    _this.father.UpdateTransferData();
                    _this.GetDescriptionHelpVisibility();
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "ExternalVATCard", {
        get: function () { return this.invoiceLinePM.ExternalVATCard; },
        set: function (value) {
            var _this = this;
            if (this.invoiceLinePM.ExternalVATCard != value) {
                this.invoiceLinePM.ExternalVATCard = value;
                this.father.EntityPM.InvoiceLines.filter(function (d) { return d.VatTypeId == _this.invoiceLinePM.VatTypeId; }).forEach(function (item) {
                    item.ExternalVATCard = value;
                    _this.father.UpdateTransferData();
                    _this.GetDescriptionHelpVisibility();
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoiceTransferLineArgs.prototype, "CreditAccount", {
        get: function () { return this.invoiceLinePM.CreditAccount; },
        set: function (value) {
            if (this.invoiceLinePM.CreditAccount != value) {
                this.invoiceLinePM.CreditAccount = value;
                this.father.UpdateTransferData();
                this.GetDescriptionHelpVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    ARInvoiceTransferLineArgs.prototype.EditClicked = function () {
        var _this = this;
        var tableName = "";
        var entityId = "";
        var tabCode = "";
        var title = "";
        switch (this.Code) {
            case "BLTO":
                {
                    switch (this.invoicePM.BillToPartnerTypeId) {
                        case "CS":
                            {
                                tableName = "Customer";
                                tabCode = "CLAC";
                                break;
                            }
                        case "AG":
                            {
                                tableName = "Agent";
                                tabCode = "AGAC";
                                break;
                            }
                        case "CG":
                            {
                                tableName = "CustomAgent";
                                tabCode = "CUAC";
                                break;
                            }
                        case "AL":
                            {
                                tableName = "Airline";
                                tabCode = "ALAC";
                                break;
                            }
                        case "TR":
                            {
                                tableName = "Trucker";
                                tabCode = "TRAC";
                                break;
                            }
                        case "VD":
                            {
                                tableName = "Vendor";
                                tabCode = "VDAC";
                                break;
                            }
                        case "SG":
                            {
                                tableName = "ShippingAgent";
                                tabCode = "SAAC";
                                break;
                            }
                        case "SL":
                            {
                                tableName = "ShippingLine";
                                tabCode = "SLAC";
                                break;
                            }
                        case "WH":
                            {
                                tableName = "Warehouse";
                                tabCode = "WHAC";
                                break;
                            }
                    }
                    entityId = this.invoicePM.BillToId;
                    title = "Edit";
                    break;
                }
            case "CURR":
                {
                    tableName = "Currency";
                    title = "Edit Currency";
                    entityId = this.invoicePM.InvoiceCurrencyId;
                    tabCode = "CRAC";
                    break;
                }
            case "PYTM":
                {
                    tableName = "PaymentTerm";
                    title = "Edit Payment Term";
                    entityId = this.invoicePM.PaymentTermId;
                    tabCode = "PTAC";
                    break;
                }
            case "VAT":
                {
                    tableName = "VatType";
                    title = "Edit VAT Type";
                    entityId = this.invoiceLinePM.VatTypeId;
                    tabCode = "VTAC";
                    break;
                }
            case "TAX":
                {
                    tableName = "VatType";
                    title = "Edit VAT Type";
                    entityId = this.invoiceLinePM.VatTypeId;
                    tabCode = "VTAC";
                    break;
                }
            default:
                {
                    tableName = "ChargesType";
                    title = "Edit Charges Type";
                    entityId = this.invoiceLinePM.ChargesTypeId;
                    tabCode = "CHAC";
                    break;
                }
        }
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.IsFillScreen = true;
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = title;
        editWindow.IsEditComponent = true;
        editWindow.ComponentLoaded.subscribe(function (s) {
            editWindow.WindowClosed.subscribe(function (d) {
                var entity = s.EntityPM;
                if (entity != null) {
                    if (_this.Code == "BLTO") {
                        _this.EditingFieldValue = entity.ReceivablesAccountingCard;
                    }
                    else if (_this.Code == "CURR") {
                        _this.EditingFieldValue = entity.AccountingExternalCode;
                    }
                    else if (_this.Code == "PYTM") {
                        _this.EditingFieldValue = entity.ExternalId;
                    }
                    else if (_this.Code == "VAT") {
                        _this.EditingFieldValue = entity.ExternalVATCard;
                    }
                    else if (_this.Code == "TAX") {
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "HV" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "RH") {
                            _this.EditingFieldValue = SessionLocator_1.SessionLocator.AccountingSettingPM.ReceivableVATCard;
                        }
                        else {
                            _this.EditingFieldValue = entity.ExternalVATCard;
                        }
                    }
                    else { // Charge Type
                        if (entity.AccountingVATSplit) {
                            _this.father.ItemsSource.filter(function (d) { return d.Code == "Line"; }).forEach(function (item) {
                                if (item.invoiceLinePM.ChargesTypeId == entity.Id) {
                                    var charge = entity.ChargeTypeAccountings.filter(function (d) { return d.VatTypeId == _this.invoiceLinePM.VatTypeId; })[0];
                                    if (charge != null) {
                                        item.EditingFieldValue = charge.ReceivableCreditAccount;
                                    }
                                }
                            });
                        }
                        else {
                            _this.father.ItemsSource.filter(function (d) { return d.Code == "Line"; }).forEach(function (item) {
                                if (item.invoiceLinePM.ChargesTypeId == entity.Id) {
                                    //if (this.father.isJournal) {
                                    item.EditingFieldValue = entity.ReceivableCreditAccount;
                                    // }
                                    //else {
                                    // item.EditingFieldValue = entity.ChargesTypeExternalCode;
                                    //}
                                }
                            });
                        }
                    }
                    _this.father.UpdateTransferData();
                }
            });
        });
        editWindow.ShowEditComponent(entityId, tableName, tabCode, true);
    };
    return ARInvoiceTransferLineArgs;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceTransferLineArgs = ARInvoiceTransferLineArgs;
var InvoiceCodeNameClass = /** @class */ (function () {
    function InvoiceCodeNameClass() {
    }
    return InvoiceCodeNameClass;
}());
exports.InvoiceCodeNameClass = InvoiceCodeNameClass;
//# sourceMappingURL=ARInvoiceTransferTemplate.js.map