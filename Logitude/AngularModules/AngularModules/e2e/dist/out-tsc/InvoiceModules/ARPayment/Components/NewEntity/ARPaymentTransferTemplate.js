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
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ARPaymentPMService_1 = require("../../../../Invoice/Services/StandardPMs/ARPaymentPMService");
var ARPaymentTransferTemplate = /** @class */ (function (_super) {
    __extends(ARPaymentTransferTemplate, _super);
    function ARPaymentTransferTemplate() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "ARPayment";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.IsTheFirstTime = true;
        _this.IsNew = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isBillToFinished = false;
        _this.isCurrencyFinished = false;
        _this.CurrencyExternalId = "";
        _this.BillToExternalId = "";
        _this.ValidationErrorsList = [];
        return _this;
    }
    ARPaymentTransferTemplate.prototype.InitTemplate = function (entity) {
        this.EntityPM = entity;
        this.FillFieldsData();
    };
    ARPaymentTransferTemplate.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.EntityId = args.EntityId;
            this.IsNew = args.IsNewTemplate;
            this.GetSingleEntityPM();
        }
    };
    ARPaymentTransferTemplate.prototype.GetSingleEntityPM = function () {
        var _this = this;
        var service = new ARPaymentPMService_1.ARPaymentPMService();
        service.get(this.EntityId).subscribe(function (response) {
            if (!response.HasError) {
                var entityPM = response.Result;
                if (entityPM) {
                    _this.EntityPM = entityPM;
                    _this.FillFieldsData();
                }
            }
        });
    };
    ARPaymentTransferTemplate.prototype.BuildList = function () {
        if (this.isBillToFinished && this.isCurrencyFinished) {
            this.ItemsSource = [];
            this.ItemsSource.push(new ARTransferLineArgs(this.EntityPM, this, "BLTO"));
            this.ItemsSource.push(new ARTransferLineArgs(this.EntityPM, this, "CURR"));
            this.UpdateTransferData();
        }
    };
    ARPaymentTransferTemplate.prototype.FillFieldsData = function () {
        this.GetCurrencyData();
        this.GetBillToData();
    };
    ARPaymentTransferTemplate.prototype.GetCurrencyData = function () {
        var _this = this;
        var currencyListService = new CurrencyListService_1.CurrencyListService();
        currencyListService.getSingle(this.EntityPM.PaymentCurrencyId).subscribe(function (response) {
            if (!response.HasError) {
                var currency = response.Result;
                if (currency != null) {
                    _this.CurrencyExternalId = currency.AccountingExternalCode;
                }
                _this.isCurrencyFinished = true;
                _this.BuildList();
            }
        });
    };
    ARPaymentTransferTemplate.prototype.GetBillToData = function () {
        var _this = this;
        var cardListService = new CardListService_1.CardListService();
        cardListService.getSingle(this.EntityPM.BillToId).subscribe(function (response) {
            if (!response.HasError) {
                var card = response.Result;
                if (card != null) {
                    _this.BillToExternalId = card.ReceivablesAccountingCard;
                }
                _this.isBillToFinished = true;
                _this.BuildList();
            }
        });
    };
    //UpdateTransferData
    ARPaymentTransferTemplate.prototype.UpdateTransferData = function () {
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
    Object.defineProperty(ARPaymentTransferTemplate.prototype, "IsEditingEnabled", {
        //Properties
        get: function () {
            var myResult = true;
            if (this.TransferStatusCode == "TR") {
                myResult = false;
            }
            else if (this.EntityPM && this.EntityPM.StatusCode == "LL") {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentTransferTemplate.prototype, "SetBlockedButtonVisibility", {
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
    Object.defineProperty(ARPaymentTransferTemplate.prototype, "SetUnBlockedButtonVisibility", {
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
    Object.defineProperty(ARPaymentTransferTemplate.prototype, "TransferStatusCode", {
        get: function () {
            if (this.EntityPM) {
                return this.EntityPM.TransferStatusCode;
            }
        },
        set: function (value) {
            var _this = this;
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
                if (this.IsTheFirstTime && !this.IsNew) {
                    this.IsTheFirstTime = false;
                    if (this.CurrentSession.CurrentEditComponent != null) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Width = 450;
                        confirmWindow.Show("Accounting details needed for transfer is updated, update the transfer status ?");
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                        });
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentTransferTemplate.prototype, "IsReadyForTransfer", {
        get: function () {
            return this.TransferStatusCode == "RD" ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentTransferTemplate.prototype, "TransferStatusName", {
        get: function () {
            if (this.EntityPM && this.EntityPM.TransferStatusName != null) {
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
    ARPaymentTransferTemplate.prototype.SetBlockedTransferClicked = function () {
        this.TransferStatusCode = "BL";
        this.UpdateTransferData();
    };
    ARPaymentTransferTemplate.prototype.SetUnBlockedTransferClicked = function () {
        this.TransferStatusCode = "NR";
        this.UpdateTransferData();
    };
    // Commands
    ARPaymentTransferTemplate.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ARPaymentTransferTemplate.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new ARPaymentPMService_1.ARPaymentPMService();
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
    ARPaymentTransferTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARPaymentTransferTemplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], ARPaymentTransferTemplate);
    return ARPaymentTransferTemplate;
}(BaseComponent_1.BaseComponent));
exports.ARPaymentTransferTemplate = ARPaymentTransferTemplate;
var ARTransferLineArgs = /** @class */ (function (_super) {
    __extends(ARTransferLineArgs, _super);
    function ARTransferLineArgs(entity, father, typeCode) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.ObjectTableName = "ARPayment";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Props
        _this.DescriptionTitle = "";
        _this.DescriptionValue = "";
        _this.DescriptionHelp = "";
        _this.DescriptionHelpVisibility = false;
        _this.EntityPM = entity;
        _this.Code = typeCode;
        _this.GetDescriptionTitle();
        _this.GetDescriptionValue();
        _this.GetDescriptionHelp();
        _this.GetDescriptionHelpVisibility();
        return _this;
    }
    ARTransferLineArgs.prototype.GetDescriptionTitle = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "Bill To";
                    break;
                }
            case "CURR":
                {
                    result = "Payment Currency";
                    break;
                }
        }
        this.DescriptionTitle = result;
    };
    ARTransferLineArgs.prototype.GetDescriptionValue = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = this.EntityPM.BillToName;
                    break;
                }
            case "CURR":
                {
                    result = this.EntityPM.PaymentCurrencyCode;
                    break;
                }
        }
        this.DescriptionValue = result;
    };
    ARTransferLineArgs.prototype.GetDescriptionHelp = function () {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "Please enter bill to account";
                    break;
                }
            case "CURR":
                {
                    result = "Please enter external code for " + this.EntityPM.PaymentCurrencyCode;
                    break;
                }
        }
        this.DescriptionHelp = result;
    };
    ARTransferLineArgs.prototype.GetDescriptionHelpVisibility = function () {
        var result = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
            result = true;
        }
        this.DescriptionHelpVisibility = result;
    };
    Object.defineProperty(ARTransferLineArgs.prototype, "IsTransferredEnabled", {
        get: function () {
            var myResult = true;
            if (this.father.TransferStatusCode == "TR") {
                myResult = false;
            }
            else if (this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "LL") {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARTransferLineArgs.prototype, "EditingFieldValue", {
        get: function () {
            var result = "";
            switch (this.Code) {
                case "BLTO":
                    {
                        result = this.father.BillToExternalId;
                        break;
                    }
                case "CURR":
                    {
                        result = this.father.CurrencyExternalId;
                        break;
                    }
            }
            return result;
        },
        set: function (value) {
            switch (this.Code) {
                case "BLTO":
                    {
                        if (this.father.BillToExternalId != value) {
                            this.father.BillToExternalId = value;
                        }
                        break;
                    }
                case "CURR":
                    {
                        if (this.father.CurrencyExternalId != value) {
                            this.father.CurrencyExternalId = value;
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
    // Commands
    ARTransferLineArgs.prototype.EditClicked = function () {
        var _this = this;
        switch (this.Code) {
            case "BLTO":
                {
                    var tableName = "";
                    var entityId = "";
                    var tabCode = "";
                    var title = "";
                    switch (this.EntityPM.BillToPartnerTypeId) {
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
                    entityId = this.EntityPM.BillToId;
                    title = "Edit";
                    break;
                }
            case "CURR":
                {
                    tableName = "Currency";
                    title = "Edit Currency";
                    entityId = this.EntityPM.PaymentCurrencyId;
                    tabCode = "CRAC";
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
                    _this.father.UpdateTransferData();
                }
            });
        });
        editWindow.ShowEditComponent(entityId, tableName, tabCode, true);
    };
    return ARTransferLineArgs;
}(BaseComponent_1.BaseComponent));
exports.ARTransferLineArgs = ARTransferLineArgs;
//# sourceMappingURL=ARPaymentTransferTemplate.js.map