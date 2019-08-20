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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var APInvoiceMultipleShipmentPM_1 = require("../../../../Invoice/EntityPMs/APInvoiceMultipleShipmentPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var NumbersPipe_1 = require("../../../../Infrastructure/Pipes/NumbersPipe");
var Tools_2 = require("../../../../Invoice/Tools");
var Args_1 = require("../../../../Invoice/Args");
var PaymentTermListService_1 = require("../../../../Common/Services/StandardLists/PaymentTermListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var APInvoiceMultipleDetailsTabComponent = /** @class */ (function (_super) {
    __extends(APInvoiceMultipleDetailsTabComponent, _super);
    function APInvoiceMultipleDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "APInvoice";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsEditingEnabled = false;
        _this.PaymentTermDisplayInLOV = true;
        _this.isTotalInLocalCurrency = false;
        _this.TotalsList = [];
        _this.SummaryItems = [];
        _this.editingShipmentId = null;
        _this.editingShipmentRequested = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.InitializeServices();
        _this.SetUIProperties();
        _this.BuildScreenData();
        _this.Listen();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "APInvoiceEditExchangeRate")) {
            _this.IsEditExchangeRateVisible = true;
        }
        return _this;
    }
    APInvoiceMultipleDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    if (_this.editingShipmentRequested) {
                        _this.RunEditShipment();
                    }
                }
                _this.editingShipmentRequested = false;
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                }
            });
        }
    };
    APInvoiceMultipleDetailsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    APInvoiceMultipleDetailsTabComponent.prototype.InitializeServices = function () {
        this.myPaymentTermListService = new PaymentTermListService_1.PaymentTermListService();
    };
    APInvoiceMultipleDetailsTabComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.InvoiceTool.IsEditingAPInvoiceEnabled(this.EntityPM);
        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceCurrencyExchangeRate", this.ObjectTableName, false);
        //this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InvoiceDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("AmountInInvoiceCurrency", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VATNumber", this.ObjectTableName, isEditingEnabled);
        this.SetUIProperties_VatNumber();
        this.SetUIProperties_DueDate();
    };
    APInvoiceMultipleDetailsTabComponent.prototype.SetUIProperties_VatNumber = function () {
        var isFieldRequired = false;
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.IsVatNumberMandatoryInAP) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VATNumber)) {
                isFieldRequired = true;
            }
            this.UIProperties.SetRequired("VATNumber", this.ObjectTableName, isFieldRequired);
        }
    };
    APInvoiceMultipleDetailsTabComponent.prototype.SetUIProperties_DueDate = function () {
        var AllowManuallyDueDate = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            AllowManuallyDueDate = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowManuallyDueDate;
        }
        if (AllowManuallyDueDate) {
            this.PaymentTermDisplayInLOV = null;
        }
        if (!this.IsEditingEnabled) {
            AllowManuallyDueDate = false;
        }
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, AllowManuallyDueDate);
    };
    APInvoiceMultipleDetailsTabComponent.prototype.BuildScreenData = function () {
        this.BuildItemsSource();
    };
    APInvoiceMultipleDetailsTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var list = this.EntityPM.InvoiceMultipleShipments;
        list = list.sort(function (a, b) { return a.IndexOrder == b.IndexOrder ? 0 : a.IndexOrder < b.IndexOrder ? -1 : 1; });
        list.forEach(function (item) {
            _this.ItemsSource.push(new MultipleShipmentLine(item));
        });
        this.BuildTotalsCollection();
    };
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "VendorDependencyProperty1", {
        // Vendor
        get: function () { return Tools_2.InvoiceTool.GetVendorPartnerTypes(); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "VendorId", {
        get: function () { return this.EntityPM.VendorId; },
        set: function (value) {
            if (this.EntityPM.VendorId != value) {
                this.EntityPM.VendorId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (value) {
            if (this.EntityPM.VendorName != value) {
                this.EntityPM.VendorName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "InvoiceNumber", {
        get: function () { return this.EntityPM.InvoiceNumber; },
        set: function (value) {
            if (this.EntityPM.InvoiceNumber != value) {
                this.EntityPM.InvoiceNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "VATNumber", {
        get: function () { return this.EntityPM.VATNumber; },
        set: function (value) {
            if (this.EntityPM.VATNumber != value) {
                this.EntityPM.VATNumber = value;
                this.SetUIProperties_VatNumber();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "InvoiceCurrencyId", {
        get: function () { return this.EntityPM.InvoiceCurrencyId; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyId != value) {
                this.EntityPM.InvoiceCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyCode != value) {
                this.EntityPM.InvoiceCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "InvoiceCurrencyExchangeRate", {
        get: function () { return this.EntityPM.InvoiceCurrencyExchangeRate; },
        set: function (value) {
            if (this.EntityPM.InvoiceCurrencyExchangeRate != value) {
                this.EntityPM.InvoiceCurrencyExchangeRate = Tools_1.AppTool.Round(value, 5);
                this.ComputeAllAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "ExchangeRateDate", {
        get: function () { return this.EntityPM.ExchangeRateDate; },
        set: function (value) {
            if (this.EntityPM.ExchangeRateDate != value) {
                this.EntityPM.ExchangeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "RelativeRateDate", {
        get: function () { return Tools_1.DateTool.GetRelativeRateDate(this.InvoiceDate, this.ExchangeRateDate, "old"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "ProfitCurrencyId", {
        get: function () { return this.EntityPM.ProfitCurrencyId; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyId != value) {
                this.EntityPM.ProfitCurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (value) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != value) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_1.AppTool.Round(value, 5);
                this.ComputeAllAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.PaymentTermId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PaymentTermId != value) {
                this.EntityPM.PaymentTermId = value;
                Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.PaymentTermName = null;
                }
                else {
                    this.myPaymentTermListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.PaymentTermName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "PaymentTermName", {
        get: function () { return this.EntityPM.PaymentTermName; },
        set: function (value) {
            if (this.EntityPM.PaymentTermName != value) {
                this.EntityPM.PaymentTermName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "InvoiceDate", {
        get: function () { return this.EntityPM.InvoiceDate; },
        set: function (value) {
            if (this.EntityPM.InvoiceDate != value) {
                this.EntityPM.InvoiceDate = value;
                Tools_2.InvoiceTool.ComputeAPInvoiceDueDate(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "DueDate", {
        get: function () { return this.EntityPM.DueDate; },
        set: function (value) {
            if (this.EntityPM.DueDate != value) {
                this.EntityPM.DueDate = value;
                Tools_2.InvoiceTool.ComputeAPInvoicePaymentTerm(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "AmountInInvoiceCurrency", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInInvoiceCurrency != value) {
                this.EntityPM.AmountInInvoiceCurrency = Tools_1.AppTool.Round(value, 2);
                this.EntityPM.InvoiceExpectedAmount = Tools_1.AppTool.Round(value, 2);
                this.ComputeAllAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "IsCurrencyFilterVisible", {
        // Totals
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.InvoiceCurrencyId)) {
                if (this.LocalCurrencyId != this.InvoiceCurrencyId) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "IsTotalInLocalCurrency", {
        get: function () { return this.isTotalInLocalCurrency; },
        set: function (value) {
            if (this.isTotalInLocalCurrency != value) {
                this.isTotalInLocalCurrency = value;
                this.BuildTotalsControl();
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceMultipleDetailsTabComponent.prototype.ComputeAllAmounts = function () {
        this.AmountInLocalCurrency = Tools_1.AppTool.Round(this.AmountInInvoiceCurrency * this.InvoiceCurrencyExchangeRate, 2);
        if (this.ProfitCurrencyId == this.InvoiceCurrencyId) {
            this.AmountInProfitCurrency = this.AmountInInvoiceCurrency;
        }
        else {
            if (Tools_1.AppTool.IsNullOrZero(this.ProfitCurrencyExchangeRate)) {
                this.AmountInProfitCurrency = 0;
            }
            else {
                this.AmountInProfitCurrency = Tools_1.AppTool.Round(this.AmountInLocalCurrency / this.ProfitCurrencyExchangeRate, 2);
            }
        }
        this.AmountDue = this.AmountInInvoiceCurrency == null ? 0 : this.AmountInInvoiceCurrency;
        this.AmountDueInLocalCurrency = this.AmountInLocalCurrency == null ? 0 : this.AmountInLocalCurrency;
        this.AmountDueInProfitCurrency = this.AmountInProfitCurrency == null ? 0 : this.AmountInProfitCurrency;
    };
    APInvoiceMultipleDetailsTabComponent.prototype.ComputeTotals = function () {
        this.BuildTotalsCollection(true);
    };
    APInvoiceMultipleDetailsTabComponent.prototype.BuildTotalsCollection = function (isComputingTotals) {
        var _this = this;
        if (isComputingTotals === void 0) { isComputingTotals = false; }
        var totalsList = [];
        var myDataList = this.EntityPM.InvoiceMultipleShipments;
        var allLinesList = [];
        myDataList.forEach(function (item) {
            var vatsList = item.TotalVatsList;
            if (vatsList.length > 0) {
                vatsList.forEach(function (myString) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(myString)) {
                        var lineArray = myString.split(':');
                        var myRecord = new Args_1.InvoiceTotalsClass();
                        myRecord.VatTypeId = lineArray[0];
                        myRecord.VatTypePercentage = _this.ConvertToDouble(lineArray[1]);
                        myRecord.LocalCurrencyAmount = _this.ConvertToDouble(lineArray[2]);
                        myRecord.InvoiceCurrencyAmount = _this.ConvertToDouble(lineArray[3]);
                        myRecord.ProfitCurrencyAmount = _this.ConvertToDouble(lineArray[4]);
                        myRecord.VatTypeCell = lineArray[5];
                        allLinesList.push(myRecord);
                    }
                });
            }
        });
        var dataGroupList = [];
        allLinesList.filter(function (f) { return f.VatTypeId != null; }).forEach(function (item) {
            var dataGroupItem = dataGroupList.filter(function (d) { return d.VatTypeCell == item.VatTypeCell; })[0];
            if (dataGroupItem == null) {
                dataGroupItem = new Args_1.InvoiceTotalsClass();
                dataGroupItem.RowLabel = item.VatTypeCell;
                dataGroupItem.VatTypeCell = item.VatTypeCell;
                dataGroupItem.LocalCurrencyAmount = 0;
                dataGroupItem.InvoiceCurrencyAmount = 0;
                dataGroupList.push(dataGroupItem);
            }
            if (item.VatTypePercentage != null) {
                if (item.LocalCurrencyAmount != null) {
                    dataGroupItem.LocalCurrencyAmount = dataGroupItem.LocalCurrencyAmount + Tools_1.AppTool.Round((item.VatTypePercentage * item.LocalCurrencyAmount / 100), 2);
                }
                if (item.InvoiceCurrencyAmount != null) {
                    dataGroupItem.InvoiceCurrencyAmount = dataGroupItem.InvoiceCurrencyAmount + Tools_1.AppTool.Round((item.VatTypePercentage * item.InvoiceCurrencyAmount / 100), 2);
                }
            }
        });
        var subTotalItem = new Args_1.InvoiceTotalsClass();
        var allTotalItem = new Args_1.InvoiceTotalsClass();
        subTotalItem.RowLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.S.Details.Subtotal");
        allTotalItem.RowLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency");
        if (dataGroupList.length > 0) {
            subTotalItem.LocalCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "SubTotalInLocalCurrency");
            subTotalItem.InvoiceCurrencyAmount = Tools_1.ArrayTool.Sum(myDataList, "SubTotalInInvoiceCurrency");
            totalsList.push(subTotalItem);
            dataGroupList.forEach(function (item) {
                totalsList.push(item);
            });
            allTotalItem.LocalCurrencyAmount = subTotalItem.LocalCurrencyAmount + Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(dataGroupList, "LocalCurrencyAmount"), 2);
            allTotalItem.InvoiceCurrencyAmount = subTotalItem.InvoiceCurrencyAmount + Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(dataGroupList, "InvoiceCurrencyAmount"), 2);
            totalsList.push(allTotalItem);
        }
        if (isComputingTotals) {
            this.SubTotalInLocalCurrency = Tools_1.AppTool.Round(subTotalItem.LocalCurrencyAmount, 2);
            this.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(subTotalItem.InvoiceCurrencyAmount, 2);
        }
        this.TotalsList = totalsList;
        this.BuildTotalsControl();
    };
    APInvoiceMultipleDetailsTabComponent.prototype.BuildTotalsControl = function () {
        this.BuildSummary();
        //this.SummaryItems = [];
        //var pipe = new NumbersPipe();
        //var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";
        //if (this.TotalsList.length == 0) {
        //    var myTotalItem = new SummaryItem();
        //    myTotalItem.Label = TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency");
        //    myTotalItem.Label += " " + selectedCurrencyCode;
        //    myTotalItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(0, "N2") : pipe.transform(0, "N2");
        //    this.SummaryItems.push(myTotalItem);
        //}
        //else {
        //    for (var i = 0; i < this.TotalsList.length; i++) {
        //        var item: InvoiceTotalsClass = this.TotalsList[i];
        //        var mySummaryItem = new SummaryItem();
        //        mySummaryItem.Label = item.RowLabel;
        //        mySummaryItem.Value = this.IsTotalInLocalCurrency ? pipe.transform(item.LocalCurrencyAmount, "N2") : pipe.transform(item.InvoiceCurrencyAmount, "N2");
        //        this.SummaryItems.push(mySummaryItem);
        //        if (i + 2 < this.TotalsList.length) {
        //            var myOperatorItem = new SummaryItem();
        //            myOperatorItem.Value = "+";
        //            this.SummaryItems.push(myOperatorItem);
        //        }
        //        else if (i + 1 < this.TotalsList.length) {
        //            var myOperatorItem = new SummaryItem();
        //            myOperatorItem.Value = "=";
        //            this.SummaryItems.push(myOperatorItem);
        //        }
        //        else if (i + 1 == this.TotalsList.length) {
        //            mySummaryItem.Label += " " + selectedCurrencyCode;
        //        }
        //    }
        //}
    };
    APInvoiceMultipleDetailsTabComponent.prototype.BuildSummary = function () {
        var _this = this;
        this.SummaryItems = [];
        var pipe = new NumbersPipe_1.NumbersPipe();
        var selectedCurrencyCode = this.IsTotalInLocalCurrency ? "(" + this.LocalCurrencyCode + ")" : "(" + this.InvoiceCurrencyCode + ")";
        if (this.EntityPM.TotalVATs.length > 0) {
            var mySummaryItem_Sub = new Args_1.SummaryItem();
            mySummaryItem_Sub.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.S.Details.Subtotal");
            mySummaryItem_Sub.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.SubTotalInLocalCurrency, "N2") : pipe.transform(this.EntityPM.SubTotalInInvoiceCurrency, "N2");
            this.SummaryItems.push(mySummaryItem_Sub);
            this.EntityPM.TotalVATs.forEach(function (item) {
                var myOperatorItem = new Args_1.SummaryItem();
                myOperatorItem.Value = "+";
                _this.SummaryItems.push(myOperatorItem);
                var mySummaryItem = new Args_1.SummaryItem();
                mySummaryItem.Label = item.VatTypeCell;
                mySummaryItem.Value = _this.IsTotalInLocalCurrency ? pipe.transform(item.LocalVATAmount, "N2") : pipe.transform(item.InvoiceCurrencyVATAmount, "N2");
                _this.SummaryItems.push(mySummaryItem);
            });
            var myOperatorItem = new Args_1.SummaryItem();
            myOperatorItem.Value = "=";
            this.SummaryItems.push(myOperatorItem);
        }
        var mySummaryItem_All = new Args_1.SummaryItem();
        mySummaryItem_All.Label = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.F.AmountInInvoiceCurrency") + " " + selectedCurrencyCode;
        mySummaryItem_All.Value = this.IsTotalInLocalCurrency ? pipe.transform(this.EntityPM.AmountInLocalCurrency_Summary, "N2") : pipe.transform(this.EntityPM.AmountInInvoiceCurrency_Summary, "N2");
        this.SummaryItems.push(mySummaryItem_All);
    };
    APInvoiceMultipleDetailsTabComponent.prototype.ConvertToDouble = function (myString) {
        var myResult = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(myString)) {
            myResult = +myString;
        }
        return myResult;
    };
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.EntityPM.SubTotalInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.SubTotalInLocalCurrency != value) {
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    value = 0;
                }
                this.EntityPM.SubTotalInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.EntityPM.SubTotalInInvoiceCurrency; },
        set: function (value) {
            if (this.EntityPM.SubTotalInInvoiceCurrency != value) {
                this.EntityPM.SubTotalInInvoiceCurrency = Tools_1.AppTool.Round(value, 2);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    value = 0;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInLocalCurrency != value) {
                this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "AmountInProfitCurrency", {
        get: function () { return this.EntityPM.AmountInProfitCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInProfitCurrency != value) {
                this.EntityPM.AmountInProfitCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "AmountDue", {
        get: function () { return this.EntityPM.AmountDue; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (setValue == null) {
                setValue = 0;
            }
            if (this.EntityPM.AmountDue != setValue) {
                this.EntityPM.AmountDue = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "AmountDueInLocalCurrency", {
        get: function () { return this.EntityPM.AmountDueInLocalCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (setValue == null) {
                setValue = 0;
            }
            if (this.EntityPM.AmountDueInLocalCurrency != setValue) {
                this.EntityPM.AmountDueInLocalCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoiceMultipleDetailsTabComponent.prototype, "AmountDueInProfitCurrency", {
        get: function () { return this.EntityPM.AmountDueInProfitCurrency; },
        set: function (value) {
            var setValue = Tools_1.AppTool.Round(value, 2);
            if (setValue == null) {
                setValue = 0;
            }
            if (this.EntityPM.AmountDueInProfitCurrency != setValue) {
                this.EntityPM.AmountDueInProfitCurrency = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    APInvoiceMultipleDetailsTabComponent.prototype.AddShipment = function (item) {
        if (item != null) {
            if (this.EntityPM.InvoiceMultipleShipments.filter(function (f) { return f.ShipmentId == item.Id; }).length == 0) {
                if (item.IsAccountingClosed) {
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show("Please open for accounting to enable adding");
                }
                else {
                    var itemPM = new APInvoiceMultipleShipmentPM_1.APInvoiceMultipleShipmentPM(null);
                    itemPM.ShipmentId = item.Id;
                    itemPM.APInvoiceId = this.EntityPM.Id;
                    itemPM.Tenant = this.EntityPM.Tenant;
                    itemPM.House = item.House;
                    itemPM.Master = item.Master;
                    itemPM.LongMaster = item.LongMaster;
                    itemPM.ShipmentNumber = item.ShipmentNumber;
                    itemPM.ShipmentLevelCode = item.ShipmentLevelCode;
                    itemPM.PartnerType = item.ShipmentLevelCode == "C" ? "Agent:" : "Customer:";
                    itemPM.PartnerName = item.ShipmentLevelCode == "C" ? item.AgentName : item.CustomerName;
                    itemPM.MainCarriageCarrierName = item.MainCarriageCarrierName;
                    itemPM.ExpectedAmount = 0;
                    itemPM.OpenAmount = 0;
                    itemPM.TotalAmount = 0;
                    itemPM.TotalVATAmount = 0;
                    itemPM.AccountedAmount = 0;
                    itemPM.SubTotalInLocalCurrency = 0;
                    itemPM.SubTotalInInvoiceCurrency = 0;
                    var myIndexOrder = 0;
                    if (this.EntityPM.InvoiceMultipleShipments.length > 0) {
                        myIndexOrder = Tools_1.ArrayTool.Max(this.EntityPM.InvoiceMultipleShipments, "IndexOrder");
                        myIndexOrder += 1;
                    }
                    itemPM.IndexOrder = myIndexOrder;
                    itemPM.TotalVatsList = new Array();
                    if (this.EntityPM.InvoiceCurrencyId == this.EntityPM.ProfitCurrencyId) {
                        itemPM.OpenAmount = item.OpenPayablesInProfitCurrency;
                        itemPM.ExpectedAmount = Tools_1.AppTool.Round(item.OpenPayablesInProfitCurrency + item.AccountedPayablesInProfitCurrency, 2);
                    }
                    else {
                        if (Tools_1.AppTool.IsNullOrZero(this.InvoiceCurrencyExchangeRate)) {
                            itemPM.OpenAmount = 0;
                            itemPM.ExpectedAmount = 0;
                        }
                        else {
                            itemPM.OpenAmount = Tools_1.AppTool.Round(item.OpenPayablesInLocalCurrency / this.InvoiceCurrencyExchangeRate, 2);
                            itemPM.ExpectedAmount = Tools_1.AppTool.Round((item.OpenPayablesInLocalCurrency + item.AccountedPayablesInLocalCurrency) / this.InvoiceCurrencyExchangeRate, 2);
                        }
                    }
                    this.EntityPM.AddAPInvoiceMultipleShipmentPM(itemPM);
                    this.BuildItemsSource();
                    this.ComputeTotals();
                }
            }
        }
    };
    APInvoiceMultipleDetailsTabComponent.prototype.EditShipmentLine = function (item) {
        if (!this.editingShipmentRequested) {
            this.editingShipmentId = item.ShipmentId;
            this.editingShipmentRequested = true;
            this.SaveChanges();
        }
    };
    APInvoiceMultipleDetailsTabComponent.prototype.RunEditShipment = function () {
        var _this = this;
        if (this.editingShipmentId) {
            var entityPM = this.EntityPM.InvoiceMultipleShipments.filter(function (f) { return f.ShipmentId == _this.editingShipmentId; })[0];
            if (entityPM) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 600;
                logWindow.Title = "Edit Shipment Lines";
                logWindow.WindowArgs = { APInvoicePM: this.EntityPM, EntityShipmentPM: entityPM, IsEditingEnabled: this.IsEditingEnabled };
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        if (_this.CurrentSession.CurrentEditComponent) {
                            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        }
                    }
                });
                logWindow.Show('./InvoiceModules/APInvoice/Components/EditTabs/EditMultipleShipmentComponent');
            }
        }
    };
    APInvoiceMultipleDetailsTabComponent.prototype.DeleteShipmentLine = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this shipment line?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.EntityPM.RemoveAPInvoiceMultipleShipmentPM(item.EntityPM);
                if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                    _this.BuildItemsSource();
                }
                else {
                    _this.SaveChanges();
                }
            }
        });
    };
    APInvoiceMultipleDetailsTabComponent.prototype.ViewShipmentClicked = function (item) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: item.ShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: "A/P Invoice: " + _this.EntityPM.InvoiceNumber });
        });
    };
    APInvoiceMultipleDetailsTabComponent.prototype.SaveChanges = function () {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    APInvoiceMultipleDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './APInvoiceMultipleDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], APInvoiceMultipleDetailsTabComponent);
    return APInvoiceMultipleDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.APInvoiceMultipleDetailsTabComponent = APInvoiceMultipleDetailsTabComponent;
var MultipleShipmentLine = /** @class */ (function () {
    function MultipleShipmentLine(item) {
        this.EntityPM = item;
    }
    Object.defineProperty(MultipleShipmentLine.prototype, "IndexOrder", {
        get: function () { return this.EntityPM.IndexOrder; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "APInvoiceId", {
        get: function () { return this.EntityPM.APInvoiceId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "ShipmentId", {
        get: function () { return this.EntityPM.ShipmentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "House", {
        get: function () { return this.EntityPM.House; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "PartnerType", {
        get: function () { return this.EntityPM.PartnerType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "PartnerName", {
        get: function () { return this.EntityPM.PartnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "MainCarriageCarrierName", {
        get: function () { return this.EntityPM.MainCarriageCarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "ExpectedAmount", {
        get: function () { return this.EntityPM.ExpectedAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "AccountedAmount", {
        get: function () { return this.EntityPM.AccountedAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "OpenAmount", {
        get: function () { return this.EntityPM.OpenAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "TotalAmount", {
        get: function () { return this.EntityPM.TotalAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "TotalVATAmount", {
        get: function () { return this.EntityPM.TotalVATAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "SubTotalInLocalCurrency", {
        get: function () { return this.EntityPM.SubTotalInLocalCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MultipleShipmentLine.prototype, "SubTotalInInvoiceCurrency", {
        get: function () { return this.EntityPM.SubTotalInInvoiceCurrency; },
        enumerable: true,
        configurable: true
    });
    return MultipleShipmentLine;
}());
exports.MultipleShipmentLine = MultipleShipmentLine;
//# sourceMappingURL=APInvoiceMultipleDetailsTabComponent.js.map