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
var QuoteChargePM_1 = require("../../../Quote/EntityPMs/QuoteChargePM");
var QuoteTotalVATPM_1 = require("../../../Quote/EntityPMs/QuoteTotalVATPM");
var QuoteUtilities_1 = require("../../../Quote/Utilities/QuoteUtilities");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var VatTypeListService_1 = require("../../../Common/Services/StandardLists/VatTypeListService");
var CurrencyListService_1 = require("../../../Common/Services/StandardLists/CurrencyListService");
var ChargesTypeListService_1 = require("../../../Common/Services/StandardLists/ChargesTypeListService");
var MeasurementListService_1 = require("../../../Common/Services/StandardLists/MeasurementListService");
var CurrencyRatesService_1 = require("../../../Common/Services/CurrencyRatesService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var DecimalFormatter_1 = require("../../../Infrastructure/Utilities/DecimalFormatter");
var LCLChargesComponent = /** @class */ (function (_super) {
    __extends(LCLChargesComponent, _super);
    function LCLChargesComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "Quote";
        _this.DataContext = _this;
        _this.IsAdhoc = false;
        _this.IsRoutingRate = false;
        _this.DisplayTariffs = false;
        _this.IsEditExchangeRateVisible = false;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.AllRates = [];
        _this.AllCurrencies = [];
        _this.AllMeasurements = [];
        _this.AllVatTypes = [];
        _this.AllVatPercentages = [];
        // SetLabels
        _this.CostQuentityHeader = [];
        _this.CostPriceHeader = [];
        _this.CostAmountHeader = [];
        _this.SaleQuantityHeader = [];
        _this.SaleLocalAmountHeader = [];
        _this.SalePriceHeader = [];
        _this.SaleAmountHeader = [];
        _this.CostMinAmountHeader = [];
        _this.CostMaxAmountHeader = [];
        _this.SaleMinAmountHeader = [];
        _this.SaleMaxAmountHeader = [];
        _this.VATColumnWidth = 100;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.IsExchangeRateEnabled = true;
        _this.IsCurrencyFilterVisible = true;
        _this.SelectedRow = null;
        _this.IsLocalCurrency = false;
        _this.isFixedCurrency = false;
        _this.isSameCostCurrency = false;
        _this.SummaryHeader = "";
        _this.SummaryCostAmount = 0;
        _this.SummarySaleAmount = 0;
        _this.SummaryMarkupAmount = 0;
        _this.SummaryProfitAmount = 0;
        _this.SummaryProfitColor = Tools_1.FontTool.Black;
        _this.SubTotal = 0;
        _this.TotalVAT = 0;
        _this.TotalSale = 0;
        _this.UpdateQuantitiesMessageWidth = 0;
        _this.IsUpdateQuantitiesVisible = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.TransportModeId = _this.EntityPM.TransportModeId;
        _this.IsAdhoc = _this.EntityPM.QuoteTypeCode == "A" ? true : false;
        _this.IsRoutingRate = !_this.IsAdhoc;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        if (_this.EntityPM.TransportModeId == "A" && FeatureLocator_1.FeatureLocator.HasFeaturePermession(_this.ObjectTableName, "TARIFFS")) {
            _this.DisplayTariffs = true;
        }
        _this.InitializeServices();
        _this.LoadRequiredData();
        _this.SetLabels();
        _this.SetUIProperties();
        _this.CheckUpdateQuantities();
        _this.BuildItemsSource();
        _this.InitializeProfit();
        _this.Listen();
        return _this;
    }
    LCLChargesComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildProfitData();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildProfitData();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "QTCH") {
                    if (_this.IsAdhoc) {
                        if (_this.ItemsSource.Collection.filter(function (d) { return d.SaleUnitPrice != null || d.CostUnitPrice != null; }).length > 0) {
                            _this.CheckUpdateQuantities();
                        }
                        else {
                            _this.UpdateQuantitiesClicked();
                        }
                    }
                }
            });
        }
    };
    LCLChargesComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    LCLChargesComponent.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myVatTypeService = new VatTypeListService_1.VatTypeListService();
        this.myCurrencyService = new CurrencyListService_1.CurrencyListService();
        this.myChargesTypeService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myMeasurementService = new MeasurementListService_1.MeasurementListService();
        this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
    };
    LCLChargesComponent.prototype.LoadRequiredData = function () {
        var _this = this;
        var isEditingEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        if (isEditingEnabled) {
            this.myCurrencyRatesService.getAll(this.LocalCurrencyId, Tools_1.DateTool.GetCurrentDateAsUtc()).subscribe(function (myResponse0) {
                if (!myResponse0.HasError) {
                    _this.AllRates = myResponse0.Result;
                }
            });
            this.myCommonDomainService.GetVatTypePercentagePMByDate(Tools_1.DateTool.GetCurrentDateAsUtc()).subscribe(function (myResponse1) {
                if (!myResponse1.HasError) {
                    _this.AllVatPercentages = myResponse1.Result;
                }
            });
        }
        this.myCurrencyService.getAllFromCache().subscribe(function (myResponse2) {
            if (!myResponse2.HasError) {
                _this.AllCurrencies = myResponse2.Result;
            }
        });
        this.myMeasurementService.getAllFromCache().subscribe(function (myResponse3) {
            if (!myResponse3.HasError) {
                _this.AllMeasurements = myResponse3.Result;
            }
        });
        this.myVatTypeService.getAllFromCache().subscribe(function (myResponse4) {
            if (!myResponse4.HasError) {
                _this.AllVatTypes = myResponse4.Result;
            }
        });
    };
    LCLChargesComponent.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.AllVatPercentages.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    LCLChargesComponent.prototype.SetLabels = function () {
        this.CostQuentityHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostQuantity", false).split('%n');
        this.CostPriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostPrice", false).split('%n');
        this.CostAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostAmount", false).split('%n');
        this.SaleQuantityHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleQuantity", false).split('%n');
        this.SaleLocalAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmountLocal", false).replace("%LocalCurrencyCode", this.LocalCurrencyCode).split('%n');
        this.CostMinAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostMinAmount", false).split('%n');
        this.CostMaxAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostMaxAmount", false).split('%n');
        this.SaleMinAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleMinAmount", false).split('%n');
        this.SaleMaxAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleMaxAmount", false).split('%n');
        this.SetLabelsAttached();
    };
    LCLChargesComponent.prototype.SetLabelsAttached = function () {
        if (this.IsSaleCurrencySameAsCost) {
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
        }
        else {
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", this.SaleCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", this.SaleCurrencyCode).split('%n');
        }
    };
    LCLChargesComponent.prototype.SetGridColumnsWidth = function () {
        var myVATColumnWidth = 100;
        this.ItemsSource.Collection.forEach(function (item) {
            var myVATTextWidth = Tools_1.AppTool.GetTextWidth(item.VatTypeCell, 12) + 10;
            if (item.VatTypeUpdateIsVisible) {
                myVATTextWidth += 25;
            }
            if (myVATTextWidth > myVATColumnWidth) {
                myVATColumnWidth = myVATTextWidth;
            }
        });
        this.VATColumnWidth = myVATColumnWidth;
    };
    LCLChargesComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.ItemsSource.Collection.forEach(function (item) {
            item.SetUIProperties();
        });
        this.SetUIProperties_Summary();
    };
    LCLChargesComponent.prototype.SetUIProperties_Summary = function () {
        var isExchangeRateEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "QouteEditExchangeRate")) {
                if (this.SaleCurrencyId) {
                    if (this.SaleCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                        isExchangeRateEnabled = true;
                    }
                }
            }
        }
        this.IsExchangeRateEnabled = isExchangeRateEnabled;
        this.IsCurrencyFilterVisible = this.LocalCurrencyId == this.EntityPM.SaleCurrencyId ? false : true;
        this.UIProperties.SetEnabled("SaleCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ExchangeRate", this.ObjectTableName, isExchangeRateEnabled);
        this.UIProperties.SetEnabled("IsFixedPrice", this.ObjectTableName, this.IsEditingEnabled);
    };
    LCLChargesComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    LCLChargesComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var itemsCollection = [];
        this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).forEach(function (item) {
            itemsCollection.push(new QuoteChargeItem(item, _this, false));
        });
        this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).forEach(function (item) {
            itemsCollection.push(new QuoteChargeItem(item, _this, false));
        });
        this.ItemsSource.InsertCollection(itemsCollection);
        this.SetGridColumnsWidth();
    };
    // Commands
    LCLChargesComponent.prototype.AddChargeClicked = function () {
        var newItem = new QuoteChargePM_1.QuoteChargePM(null);
        newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newItem.QuoteId = this.EntityPM.Id;
        newItem.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newItem.MarkUpTypeCode = "F";
        newItem.MarkUpValue = 0;
        newItem.QuoteTypeCode = this.EntityPM.QuoteTypeCode;
        newItem.SaleCurrencyId = this.EntityPM.SaleCurrencyId;
        newItem.SaleCurrencyCode = this.EntityPM.SaleCurrencyCode;
        newItem.SaleExchangeRate = this.EntityPM.ExchangeRate;
        newItem.IsAllIN = false;
        newItem.CostIsFixedRate = false;
        newItem.SaleIsFixedRate = false;
        newItem.IsChargeBySteps = false;
        var itemComponent = new QuoteChargeItem(newItem, this, true);
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.AddCharges");
        this.RunAddEditCharge(itemComponent, title);
    };
    LCLChargesComponent.prototype.EditChargeClicked = function (itemComponent) {
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.EditCharges");
        this.RunAddEditCharge(itemComponent, title);
    };
    LCLChargesComponent.prototype.DeleteChargeClicked = function (itemComponent) {
        var _this = this;
        if (itemComponent.EntityPM.ChargesGroupCode == "FRT" && this.EntityPM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            var window = new MessageWindow_1.MessageWindow();
            window.Show("Can't delete this charge because it's connected to other All In charges");
        }
        else if (itemComponent.EntityPM.IsAllIN) {
            var window = new MessageWindow_1.MessageWindow();
            window.Show("Can't delete this charge because it's All In");
        }
        else {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.DeleteThisCharge"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.EntityPM.RemoveQuoteChargePM(itemComponent.EntityPM);
                    if (itemComponent.ChargesGroupCode == "FRT") {
                        _this.OnFreightAmountChanged();
                    }
                    _this.BuildItemsSource();
                    _this.ComputeTotals();
                }
            });
        }
    };
    LCLChargesComponent.prototype.RunAddEditCharge = function (itemComponent, windowTitle) {
        itemComponent.SetEditScreenGridHeaders();
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 880;
        logitudeWindow.Height = 550;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/AddEditLCLChargeComponent');
    };
    LCLChargesComponent.prototype.ShowTariffsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("TariffHeader.O.Tariffs");
        logWindow.WindowArgs = this;
        logWindow.Show('./QuoteModules/QuoteTabs/Components/Tariffs/TariffsComponent');
    };
    LCLChargesComponent.prototype.GetCurrencyCode = function (myCurrencyId) {
        var myCode = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCurrencyId)) {
            var list = this.AllCurrencies.filter(function (d) { return d.Id == myCurrencyId; })[0];
            if (list != null) {
                myCode = list.Code;
            }
        }
        return myCode;
    };
    LCLChargesComponent.prototype.GetCurrencyRate = function (myCurrencyId) {
        var myResult = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                myResult = 1;
            }
            else if (myCurrencyId == this.SaleCurrencyId) {
                myResult = this.ExchangeRate;
            }
            else {
                var lastRate = this.AllRates.filter(function (d) { return d.ForeignCurrencyId == myCurrencyId; })[0];
                if (lastRate != null) {
                    myResult = lastRate.Rate;
                }
            }
        }
        return myResult;
    };
    LCLChargesComponent.prototype.GetCurrencyRateDate = function (myCurrencyId) {
        var myResult = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (myCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                myResult = null;
            }
            else {
                var lastRate = this.AllRates.filter(function (d) { return d.ForeignCurrencyId == myCurrencyId; })[0];
                if (lastRate != null) {
                    myResult = lastRate.ValueDate;
                }
            }
        }
        return myResult;
    };
    // Profit
    LCLChargesComponent.prototype.InitializeProfit = function () {
        this.SelectedCurrencyCode = this.SaleCurrencyCode;
        this.IsSameCostCurrency = this.IsSaleCurrencySameAsCost;
        this.IsFixedCurrency = !this.IsSameCostCurrency;
        this.BuildProfitData();
    };
    LCLChargesComponent.prototype.OnSelectCurrency = function (mySelectedCode) {
        this.SelectedCurrencyCode = mySelectedCode;
        if (mySelectedCode == this.LocalCurrencyCode) {
            this.IsLocalCurrency = true;
        }
        else {
            this.IsLocalCurrency = false;
        }
        this.BuildProfitData();
    };
    LCLChargesComponent.prototype.SetFixedSameCurrency = function (setType) {
        var _this = this;
        this.IsFixedCurrency = null;
        this.IsSameCostCurrency = null;
        if (setType == "F") {
            this.IsFixedCurrency = true;
            this.IsSameCostCurrency = false;
            this.IsSaleCurrencySameAsCost = false;
            this.OnFixedSameChanges();
        }
        else {
            if (this.EntityPM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("You can't switch to multi-currency mode till you drop the all-in checks");
                messageWindow.WindowClosed.subscribe(function (event) {
                    _this.IsFixedCurrency = true;
                    _this.IsSameCostCurrency = false;
                    _this.IsSaleCurrencySameAsCost = false;
                });
            }
            else {
                this.IsFixedCurrency = false;
                this.IsSameCostCurrency = true;
                this.IsSaleCurrencySameAsCost = true;
                this.OnFixedSameChanges();
            }
        }
    };
    LCLChargesComponent.prototype.OnFixedSameChanges = function () {
        var _this = this;
        this.ItemsSource.Collection.forEach(function (item) {
            if (item.CostCurrencyId) {
                if (item.CostCurrencyId != _this.EntityPM.SaleCurrencyId) {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                        item.SaleUnitPrice = null;
                    }
                }
            }
            item.OnQuoteSaleCurrencySameAsCost();
        });
    };
    Object.defineProperty(LCLChargesComponent.prototype, "SelectedCurrencyCode", {
        get: function () { return this.selectedCurrencyCode; },
        set: function (value) {
            if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.selectedCurrencyCode = "";
            }
            else {
                this.selectedCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "IsFixedCurrency", {
        get: function () { return this.isFixedCurrency; },
        set: function (value) {
            if (this.isFixedCurrency != value) {
                this.isFixedCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "IsSameCostCurrency", {
        get: function () { return this.isSameCostCurrency; },
        set: function (value) {
            if (this.isSameCostCurrency != value) {
                this.isSameCostCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "IsSaleCurrencySameAsCost", {
        get: function () { return this.EntityPM.IsSaleCurrencySameAsCost; },
        set: function (newValue) {
            if (this.EntityPM.IsSaleCurrencySameAsCost != newValue) {
                this.EntityPM.IsSaleCurrencySameAsCost = newValue;
                this.SetLabelsAttached();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "SaleCurrencyId", {
        get: function () { return this.EntityPM.SaleCurrencyId; },
        set: function (value) {
            if (this.EntityPM.SaleCurrencyId != value) {
                this.EntityPM.SaleCurrencyId = value;
                this.SetUIProperties_Summary();
                this.SaleCurrencyCode = this.SelectedCurrencyCode = this.GetCurrencyCode(value);
                var myExchangeRate = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (value == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                        myExchangeRate = 1;
                    }
                    else {
                        var lastRate = this.AllRates.filter(function (d) { return d.ForeignCurrencyId == value; })[0];
                        if (lastRate != null) {
                            myExchangeRate = lastRate.Rate;
                        }
                    }
                }
                if (this.ExchangeRate != myExchangeRate) {
                    this.ExchangeRate = myExchangeRate;
                }
                else {
                    this.ItemsSource.Collection.forEach(function (item) {
                        item.OnQuoteSaleCurrencyChanged();
                    });
                    this.ItemsSource.Collection.forEach(function (item) {
                        if (item.IsAllIN) {
                            item.EntityPM.IsAllIN = false;
                            item.IsAllIN = true;
                        }
                    });
                    this.ComputeTotals();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "SaleCurrencyCode", {
        get: function () { return this.EntityPM.SaleCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.SaleCurrencyCode != value) {
                this.EntityPM.SaleCurrencyCode = value;
                this.SetLabelsAttached();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "ExchangeRate", {
        get: function () { return this.EntityPM.ExchangeRate; },
        set: function (newValue) {
            if (this.EntityPM.ExchangeRate != newValue) {
                this.EntityPM.ExchangeRate = Tools_1.AppTool.Round(newValue, 5);
                this.ItemsSource.Collection.forEach(function (item) {
                    item.OnQuoteSaleCurrencyChanged();
                });
                this.ItemsSource.Collection.forEach(function (item) {
                    if (item.IsAllIN) {
                        item.EntityPM.IsAllIN = false;
                        item.IsAllIN = true;
                    }
                });
                this.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    LCLChargesComponent.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        var loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.SaleCurrencyId, CurrencyCode: this.SaleCurrencyCode, Rate: this.ExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.AllRates = comp.RatesList;
                    _this.ExchangeRate = Tools_1.AppTool.Round(comp.Rate, 5);
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    };
    Object.defineProperty(LCLChargesComponent.prototype, "IsFixedPrice", {
        get: function () { return this.EntityPM.IsFixedPrice; },
        set: function (newValue) {
            if (this.EntityPM.IsFixedPrice != newValue) {
                this.EntityPM.IsFixedPrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "EstimateProfit", {
        get: function () { return this.EntityPM.EstimateProfit; },
        set: function (value) {
            if (this.EntityPM.EstimateProfit != value) {
                this.EntityPM.EstimateProfit = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LCLChargesComponent.prototype, "EstimateProfitEdited", {
        get: function () { return this.EntityPM.EstimateProfitEdited; },
        set: function (newValue) {
            if (this.EntityPM.EstimateProfitEdited != newValue) {
                this.EntityPM.EstimateProfitEdited = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    LCLChargesComponent.prototype.BuildProfitData = function () {
        this.SummaryCostAmount = 0;
        this.SummarySaleAmount = 0;
        this.SummaryProfitAmount = 0;
        this.SummaryMarkupAmount = 0;
        this.SummaryProfitColor = Tools_1.FontTool.Black;
        this.SubTotal = 0;
        this.TotalVAT = 0;
        this.TotalSale = 0;
        if (this.EntityPM) {
            var myCostAmountLocal = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.QuoteCharges, "CostTotalAmountLocal"), 2);
            var mySaleAmountLocal = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(function (f) { return f.IsAllIN == false; }), "SaleTotalAmountLocal"), 2);
            var mySaleProfitLocal = Tools_1.AppTool.Round(mySaleAmountLocal - myCostAmountLocal, 2);
            if (this.IsLocalCurrency) {
                this.SummaryHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.Charges.ProfitInLocalCurrency") + " (" + this.SelectedCurrencyCode + ")";
                this.SummaryCostAmount = Tools_1.AppTool.Round(myCostAmountLocal, 2);
                this.SummarySaleAmount = Tools_1.AppTool.Round(mySaleAmountLocal, 2);
                this.SummaryProfitAmount = Tools_1.AppTool.Round(mySaleProfitLocal, 2);
                this.SubTotal = this.SummarySaleAmount;
                this.TotalVAT = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "LocalCurrencyVATAmount"), 2);
                this.TotalSale = this.SubTotal + this.TotalVAT;
            }
            else {
                this.SummaryHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.ProfitInSaleCurrency.Short") + " (" + this.SelectedCurrencyCode + ")";
                if (!Tools_1.AppTool.IsNullOrZero(this.ExchangeRate)) {
                    this.SummaryCostAmount = Tools_1.AppTool.Round(myCostAmountLocal / this.ExchangeRate, 2);
                    this.SummarySaleAmount = Tools_1.AppTool.Round(mySaleAmountLocal / this.ExchangeRate, 2);
                    this.SummaryProfitAmount = Tools_1.AppTool.Round(mySaleProfitLocal / this.ExchangeRate, 2);
                    this.SubTotal = this.SummarySaleAmount;
                    this.TotalVAT = Tools_1.AppTool.Round(Tools_1.ArrayTool.Sum(this.EntityPM.TotalVATs, "QuoteCurrencyVATAmount"), 2);
                    this.TotalSale = this.SubTotal + this.TotalVAT;
                }
            }
            if (this.SummaryCostAmount < this.SummarySaleAmount) {
                this.SummaryProfitColor = Tools_1.FontTool.Green;
            }
            else if (this.SummaryCostAmount > this.SummarySaleAmount) {
                this.SummaryProfitColor = Tools_1.FontTool.Red;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SummaryCostAmount) && !Tools_1.AppTool.IsNullOrEmpty(this.SummarySaleAmount)) {
                if (!Tools_1.AppTool.IsNullOrZero(this.SummaryCostAmount)) {
                    this.SummaryMarkupAmount = ((this.SummarySaleAmount - this.SummaryCostAmount) * 100) / this.SummaryCostAmount;
                }
            }
        }
    };
    LCLChargesComponent.prototype.CheckUpdateQuantities = function () {
        if (this.IsAdhoc) {
            var updateMessage = null;
            if (this.EntityPM.QuoteCharges.filter(function (d) { return d.SaleUnitPrice != null || d.CostUnitPrice != null; }).length > 0) {
                var list = [];
                var entityQuantity = null;
                var isDifferentOrders = false;
                var isDifferentPRVL = false;
                var isDifferentPRFR = false;
                //"GRWT"
                entityQuantity = this.EntityPM.GrossWeight;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "GRWT" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "GRWT" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"GWTN"
                entityQuantity = this.EntityPM.GrossWeightPerTon;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "GWTN" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "GWTN" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"CHWT"
                entityQuantity = this.EntityPM.ChargeableWeight;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "CHWT" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "CHWT" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //CWKG
                entityQuantity = this.EntityPM.ChargeableWeightInKG;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "CWKG" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "CWKG" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //GWKG
                entityQuantity = this.EntityPM.GrossWeightInKG;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "GWKG" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "GWKG" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"VOLU"
                entityQuantity = this.EntityPM.Volume;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "VOLU" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "VOLU" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"VCBM"
                entityQuantity = this.EntityPM.VolumeInCBM;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "VCBM" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "VCBM" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"BTEU"
                entityQuantity = this.EntityPM.TEU;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"QTY"
                entityQuantity = this.EntityPM.NumberOfPackages;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "QTY" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "QTY" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentOrders = true;
                }
                //"PRVL"
                entityQuantity = this.EntityPM.ValueOfGoods;
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "PRVL" && f.CostQuantity != entityQuantity; }).length > 0) {
                    isDifferentPRVL = true;
                }
                else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "PRVL" && f.SaleQuantity != entityQuantity; }).length > 0) {
                    isDifferentPRVL = true;
                }
                //"PRFR"
                if (this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).length > 0) {
                    if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "PRFR" || f.SaleMeasurementCode == "PRFR"; }).length > 0) {
                        var FRT_CostQuantity = this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode == "FRT"; })[0].CostTotalAmount;
                        var FRT_SaleQuantity = this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode == "FRT"; })[0].SaleTotalAmount;
                        if (Tools_1.AppTool.IsNullOrZero(FRT_CostQuantity)) {
                            FRT_CostQuantity = 0;
                        }
                        if (Tools_1.AppTool.IsNullOrZero(FRT_SaleQuantity)) {
                            FRT_SaleQuantity = 0;
                        }
                        if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "PRFR" && f.CostQuantity != null && f.CostQuantity != 0 && f.CostQuantity != FRT_CostQuantity; }).length > 0) {
                            isDifferentPRFR = true;
                        }
                        if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "PRFR" && f.SaleQuantity != null && f.SaleQuantity != 0 && f.SaleQuantity != FRT_SaleQuantity; }).length > 0) {
                            isDifferentPRFR = true;
                        }
                    }
                }
                if (isDifferentOrders) {
                    updateMessage = "You have updated the expected order details, apply the new values?";
                }
                else if (isDifferentPRVL) {
                    updateMessage = "You have updated the value of goods, apply the new values?";
                }
                else if (isDifferentPRFR) {
                    updateMessage = "You have updated the value of freight charge, apply the new values?";
                }
            }
            this.UpdateQuantitiesMessage = updateMessage;
            this.UpdateQuantitiesMessageWidth = Tools_1.AppTool.GetTextWidth(updateMessage, 11);
            this.IsUpdateQuantitiesVisible = Tools_1.AppTool.IsNullOrEmpty(updateMessage) ? false : true;
        }
    };
    LCLChargesComponent.prototype.UpdateQuantitiesClicked = function () {
        if (this.IsAdhoc) {
            this.ItemsSource.Collection.forEach(function (item) {
                item.SetCostQuantity();
                item.SetSaleQuantity();
            });
            this.CheckUpdateQuantities();
        }
    };
    LCLChargesComponent.prototype.ComputeTotals = function () {
        this.EstimateProfitEdited = false;
        this.BuildTotalVATs();
        this.BuildProfitData();
        var myProfitInSaleCurrency = this.SummaryProfitAmount;
        if (this.IsLocalCurrency) {
            myProfitInSaleCurrency = this.ExchangeRate ? this.SummaryProfitAmount / this.ExchangeRate : null;
        }
        if (this.EntityPM.EstimateProfit != myProfitInSaleCurrency) {
            this.EntityPM.EstimateProfit = Tools_1.AppTool.Round(myProfitInSaleCurrency, 2);
        }
        this.SetUIProperties();
    };
    Object.defineProperty(LCLChargesComponent.prototype, "IsChargesByVAT", {
        get: function () { return this.EntityPM.IsChargesByVAT; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.IsChargesByVAT != value) {
                this.EntityPM.IsChargesByVAT = value;
                this.ItemsSource.Collection.forEach(function (item) {
                    if (value == true) {
                        _this.myChargesTypeService.getSingleFromCache(item.ChargesTypeId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    item.VatTypeId = list.VatTypeId;
                                }
                            }
                        });
                    }
                    else {
                        item.VatTypeId = null;
                    }
                    item.SetUIProperties_VAT();
                });
                this.SetGridColumnsWidth();
            }
        },
        enumerable: true,
        configurable: true
    });
    LCLChargesComponent.prototype.BuildTotalVATs = function () {
        var _this = this;
        this.EntityPM.TotalVATs = [];
        if (this.EntityPM.QuoteTypeCode == "A") {
            if (this.IsChargesByVAT) {
                var myCharges = this.EntityPM.QuoteCharges.filter(function (f) { return f.IsAllIN == false && f.VatTypeId != null; });
                if (myCharges.length > 0) {
                    var myQroups = [];
                    myCharges.forEach(function (item) {
                        if (item.VatIsMultiPercentage) {
                            var myVatGroups = SessionLocator_1.SessionLocator.AllVatTypesGroups.filter(function (f) { return f.GroupVATTypeId == item.VatTypeId; });
                            myVatGroups.forEach(function (itemGroup) {
                                _this.AddVatGroupItem(myQroups, itemGroup.SingleVATTypeId, item);
                            });
                        }
                        else {
                            if (item.VatPercentage != null) {
                                _this.AddVatGroupItem(myQroups, item.VatTypeId, item);
                            }
                        }
                    });
                    myQroups.forEach(function (itemGroup) {
                        _this.EntityPM.AddQuoteTotalVATPM(itemGroup);
                    });
                }
            }
        }
    };
    LCLChargesComponent.prototype.AddVatGroupItem = function (myQroups, vatTypeId, myCharge) {
        if (!Tools_1.AppTool.IsNullOrEmpty(vatTypeId)) {
            var vat = this.AllVatTypes.filter(function (f) { return f.Id == vatTypeId; })[0];
            if (vat) {
                var itemVatPercentage = myCharge.VatPercentage;
                if (myCharge.VatIsMultiPercentage) {
                    itemVatPercentage = this.GetVatTypePercentage(vatTypeId);
                }
                var itemVatTypeCell = vat.EnglishName + " (" + itemVatPercentage + "%)";
                var myVatableAmount = 0;
                var myVatableAmountLocal = 0;
                var myVATAmount = 0;
                var myVATAmountLocal = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(myCharge.SaleTotalAmount)) {
                    myVatableAmount = Tools_1.AppTool.Round(myCharge.SaleTotalAmount, 2);
                    if (!Tools_1.AppTool.IsNullOrEmpty(itemVatPercentage)) {
                        myVATAmount = Tools_1.AppTool.Round(itemVatPercentage * myVatableAmount / 100, 2);
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(myCharge.SaleTotalAmountLocal)) {
                    myVatableAmountLocal = Tools_1.AppTool.Round(myCharge.SaleTotalAmountLocal, 2);
                    if (!Tools_1.AppTool.IsNullOrEmpty(itemVatPercentage)) {
                        myVATAmountLocal = Tools_1.AppTool.Round(itemVatPercentage * myVatableAmountLocal / 100, 2);
                    }
                }
                var item = myQroups.filter(function (d) { return d.VatTypeCell == itemVatTypeCell; })[0];
                if (item == null) {
                    item = new QuoteTotalVATPM_1.QuoteTotalVATPM(null);
                    item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    item.QuoteId = this.EntityPM.Id;
                    item.VatTypeId = vatTypeId;
                    item.VatPercent = itemVatPercentage;
                    item.VatTypeCell = itemVatTypeCell;
                    item.ExternalVATCard = vat.ExternalVATCard;
                    item.ExternalTAXItemId = vat.ExternalTAXItemId;
                    item.QuoteCurrencyVatableAmount = myVatableAmount;
                    item.LocalCurrencyVatableAmount = myVatableAmountLocal;
                    item.QuoteCurrencyVATAmount = myVATAmount;
                    item.LocalCurrencyVATAmount = myVATAmountLocal;
                    myQroups.push(item);
                }
                else {
                    item.QuoteCurrencyVatableAmount += myVatableAmount;
                    item.LocalCurrencyVatableAmount += myVatableAmountLocal;
                    item.QuoteCurrencyVATAmount += myVATAmount;
                    item.LocalCurrencyVATAmount += myVATAmountLocal;
                }
            }
        }
    };
    LCLChargesComponent.prototype.VATDetailsClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.VATDetails");
        logitudeWindow.WindowArgs = { IsLocalCurrency: this.IsLocalCurrency, SaleCurrencyCode: this.SaleCurrencyCode, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible, TotalVATs: this.EntityPM.TotalVATs };
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/QuoteVATDetailsComponent');
    };
    LCLChargesComponent.prototype.OnFreightAmountChanged = function () {
        this.ItemsSource.Collection.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).forEach(function (item) {
            if (item.CostMeasurementCode == "PRFR") {
                item.SetCostQuantity();
            }
            if (item.SaleMeasurementCode == "PRFR") {
                item.SetSaleQuantity();
            }
        });
    };
    LCLChargesComponent = __decorate([
        core_1.Component({
            selector: 'LCLChargesComponent',
            moduleId: module.id,
            templateUrl: './LCLChargesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], LCLChargesComponent);
    return LCLChargesComponent;
}(BaseComponent_1.BaseComponent));
exports.LCLChargesComponent = LCLChargesComponent;
var QuoteChargeItem = /** @class */ (function (_super) {
    __extends(QuoteChargeItem, _super);
    function QuoteChargeItem(entity, fatherComponent, isNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "QuoteCharge";
        _this.DataContext = _this;
        _this.IsNew = false;
        _this.IsAdhoc = false;
        _this.IsRoutingRate = false;
        _this.IsEditingEnabled = false;
        _this.IsEditExchangeRateVisible = false;
        _this.IsAllInCheckBoxVisible = false;
        _this.IsEnabled_CostQuantity = false;
        _this.IsEnabled_CostUnitPrice = false;
        _this.IsEnabled_CostMinAmount = false;
        _this.IsEnabled_CostExchangeRate = false;
        _this.IsEnabled_SaleQuantity = false;
        _this.IsEnabled_SaleUnitPrice = false;
        _this.IsEnabled_SaleMinAmount = false;
        _this.IsEnabled_SaleTotalAmount = false;
        _this.IsEnabled_SaleTotalAmountLocal = false;
        _this.CostMinMaxIconTitle = "";
        _this.CostMinMaxIconIsVisibile = false;
        _this.SaleMinMaxIconTitle = "";
        _this.SaleMinMaxIconIsVisibile = false;
        _this.SaleUnitPriceColor = Tools_1.FontTool.Black;
        _this.SaleTotalAmountColor = Tools_1.FontTool.Black;
        _this.SaleTotalAmountLocalColor = Tools_1.FontTool.Black;
        _this.VatTypeUpdateIsVisible = false;
        _this.VatTypeMultiIconVisible = false;
        _this.VatTypesGroups = [];
        _this.mySaleUnitPriceString = null;
        _this.SalePriceHeader = [];
        _this.SaleAmountHeader = [];
        _this.IsNew = isNew;
        _this.EntityPM = entity;
        _this.QuotePM = fatherComponent.EntityPM;
        _this.IsAdhoc = fatherComponent.IsAdhoc;
        _this.IsRoutingRate = fatherComponent.IsRoutingRate;
        _this.TransportModeId = fatherComponent.TransportModeId;
        _this.SetUIProperties();
        _this.ReadVatTypeData();
        _this.ComputeMarkUpString();
        return _this;
    }
    QuoteChargeItem.prototype.SetUIProperties = function () {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.SetUIProperties_AllIn();
        this.SetUIProperties_CostMinMax();
        this.SetUIProperties_SaleMinMax();
        this.SetUIProperties_CostFields();
        this.SetUIProperties_SaleFields();
        this.SetUIProperties_CellsColors();
        this.SetUIProperties_VAT();
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsChargeBySteps", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CostCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CostIsFixedRate", this.ObjectTableName, this.IsEditingEnabled);
    };
    QuoteChargeItem.prototype.SetUIProperties_AllIn = function () {
        var isAllInCheckBoxVisible = true;
        if (this.ChargesGroupCode == "FRT") {
            isAllInCheckBoxVisible = false;
        }
        else if (this.IsRoutingRate) {
            isAllInCheckBoxVisible = false;
        }
        else if (this.QuotePM.IsSaleCurrencySameAsCost) {
            isAllInCheckBoxVisible = false;
        }
        else if (this.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; }).length == 0) {
            isAllInCheckBoxVisible = false;
        }
        this.IsAllInCheckBoxVisible = isAllInCheckBoxVisible;
    };
    QuoteChargeItem.prototype.SetUIProperties_CostFields = function () {
        var isEnabled_CostQuantity = false;
        var isEnabled_CostUnitPrice = false;
        var isEnabled_CostMinAmount = false;
        var isEnabled_CostMeasurement = false;
        if (this.IsEditingEnabled) {
            isEnabled_CostQuantity = true;
            isEnabled_CostUnitPrice = true;
            isEnabled_CostMinAmount = true;
            isEnabled_CostMeasurement = true;
            switch (this.CostMeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "CWKG":
                case "GWKG":
                case "VCBM":
                case "BTEU":
                case "FIXD":
                case "BCNT":
                case "PRVL":
                case "PRFR":
                case "GWTN":
                case "QTY":
                    {
                        isEnabled_CostQuantity = false;
                        break;
                    }
            }
            if (this.IsAllIN) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                //isEnabled_CostMeasurement = false;
            }
            else if (this.IsChargeBySteps) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
            }
            else if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                //isEnabled_CostMeasurement = false;
            }
        }
        this.IsEnabled_CostQuantity = isEnabled_CostQuantity;
        this.IsEnabled_CostUnitPrice = isEnabled_CostUnitPrice;
        this.IsEnabled_CostMinAmount = isEnabled_CostMinAmount;
        this.UIProperties.SetEnabled("CostMeasurementId", this.ObjectTableName, isEnabled_CostMeasurement);
        this.UIProperties.SetEnabled("CostMinAmount", this.ObjectTableName, isEnabled_CostMinAmount);
        this.UIProperties.SetEnabled("CostMaxAmount", this.ObjectTableName, isEnabled_CostMinAmount);
        this.SetUIProperties_CostRate();
    };
    QuoteChargeItem.prototype.SetUIProperties_CostRate = function () {
        var isEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "QouteEditExchangeRate")) {
                if (this.CostCurrencyId) {
                    if (this.CostCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                        if (this.CostCurrencyId != this.fatherComponent.SaleCurrencyId) {
                            isEnabled = true;
                        }
                    }
                }
            }
        }
        this.IsEnabled_CostExchangeRate = isEnabled;
        this.UIProperties.SetEnabled("CostExchangeRate", this.ObjectTableName, isEnabled);
    };
    QuoteChargeItem.prototype.SetUIProperties_SaleFields = function () {
        var isEnabled_SaleQuantity = false;
        var isEnabled_SaleUnitPrice = false;
        var isEnabled_SaleMinAmount = false;
        var isEnabled_SaleTotalAmount = false;
        var isEnabled_SaleTotalAmountLocal = false;
        var isEnabled_SaleMeasurement = false;
        if (this.IsEditingEnabled) {
            isEnabled_SaleQuantity = true;
            isEnabled_SaleUnitPrice = true;
            isEnabled_SaleMinAmount = true;
            isEnabled_SaleTotalAmount = true;
            isEnabled_SaleTotalAmountLocal = true;
            isEnabled_SaleMeasurement = true;
            switch (this.SaleMeasurementCode) {
                case "GRWT":
                case "CHWT":
                case "CWKG":
                case "GWKG":
                case "VCBM":
                case "BTEU":
                case "FIXD":
                case "BCNT":
                case "PRVL":
                case "PRFR":
                case "GWTN":
                case "QTY":
                    {
                        isEnabled_SaleQuantity = false;
                        break;
                    }
            }
            if (this.EntityPM.IsAllIN) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleTotalAmount = false;
                isEnabled_SaleTotalAmountLocal = false;
                //isEnabled_SaleMeasurement = false;
            }
            else if (this.IsChargeBySteps) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
            }
            else if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                //isEnabled_SaleMeasurement = false;
            }
        }
        this.IsEnabled_SaleQuantity = isEnabled_SaleQuantity;
        this.IsEnabled_SaleUnitPrice = isEnabled_SaleUnitPrice;
        this.IsEnabled_SaleMinAmount = isEnabled_SaleMinAmount;
        this.IsEnabled_SaleTotalAmount = isEnabled_SaleTotalAmount;
        this.IsEnabled_SaleTotalAmountLocal = isEnabled_SaleTotalAmountLocal;
        this.UIProperties.SetEnabled("SaleMeasurementId", this.ObjectTableName, isEnabled_SaleMeasurement);
        this.UIProperties.SetEnabled("SaleMinAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
        this.UIProperties.SetEnabled("SaleMaxAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
    };
    QuoteChargeItem.prototype.SetUIProperties_CostMinMax = function () {
        var iTitle = null;
        var iVisible = false;
        var iAmount = null;
        if (this.EntityPM.CostQuantity != null && this.EntityPM.CostUnitPrice != null) {
            iAmount = Tools_1.AppTool.Round(this.EntityPM.CostQuantity * this.EntityPM.CostUnitPrice, 2);
        }
        if (iAmount != null) {
            if (this.CostMinAmount != null) {
                if (iAmount < this.CostMinAmount) {
                    iAmount = this.CostMinAmount;
                    iVisible = true;
                    iTitle = "Amount is due to Charge Min Amount";
                    iTitle += "\n";
                    iTitle += "Cost Min Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.CostMinAmount, 3);
                    if (this.CostMaxAmount != null) {
                        iTitle += "\n";
                        iTitle += "Cost Max Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.CostMaxAmount, 3);
                    }
                }
            }
            if (this.CostMaxAmount != null) {
                if (iAmount > this.CostMaxAmount) {
                    iAmount = this.CostMaxAmount;
                    iVisible = true;
                    iTitle = "Amount is due to Charge Max Amount";
                    if (this.CostMinAmount != null) {
                        iTitle += "\n";
                        iTitle += "Cost Min Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.CostMinAmount, 3);
                    }
                    iTitle += "\n";
                    iTitle += "Cost Max Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.CostMaxAmount, 3);
                }
            }
        }
        this.CostMinMaxIconTitle = iTitle;
        this.CostMinMaxIconIsVisibile = iVisible;
    };
    QuoteChargeItem.prototype.SetUIProperties_SaleMinMax = function () {
        var iTitle = null;
        var iVisible = false;
        var iAmount = null;
        if (this.EntityPM.SaleQuantity != null && this.EntityPM.SaleUnitPrice != null) {
            iAmount = Tools_1.AppTool.Round(this.EntityPM.SaleQuantity * this.EntityPM.SaleUnitPrice, 2);
        }
        if (iAmount != null) {
            if (this.SaleMinAmount != null) {
                if (iAmount < this.SaleMinAmount) {
                    iAmount = this.SaleMinAmount;
                    iVisible = true;
                    iTitle = "Amount is due to Charge Min Amount";
                    iTitle += "\n";
                    iTitle += "Sale Min Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.SaleMinAmount, 3);
                    if (this.SaleMaxAmount != null) {
                        iTitle += "\n";
                        iTitle += "Sale Max Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.SaleMaxAmount, 3);
                    }
                }
            }
            if (this.SaleMaxAmount != null) {
                if (iAmount > this.SaleMaxAmount) {
                    iAmount = this.SaleMaxAmount;
                    iVisible = true;
                    iTitle = "Amount is due to Charge Max Amount";
                    if (this.SaleMinAmount != null) {
                        iTitle += "\n";
                        iTitle += "Sale Min Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.SaleMinAmount, 3);
                    }
                    iTitle += "\n";
                    iTitle += "Sale Max Amount: " + DecimalFormatter_1.DecimalFormatter.format(this.SaleMaxAmount, 3);
                }
            }
        }
        this.SaleMinMaxIconTitle = iTitle;
        this.SaleMinMaxIconIsVisibile = iVisible;
    };
    QuoteChargeItem.prototype.SetUIProperties_CellsColors = function () {
        // Price
        var myCostPrice = this.CostUnitPriceInSaleCurrency;
        var mySalePrice = this.SaleUnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            this.SaleUnitPriceColor = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPriceColor = mySalePrice < myCostPrice ? Tools_1.FontTool.Red : (mySalePrice > myCostPrice ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Total Amount
        var myCostTotalAmount = this.CostAmountInSaleCurrency;
        var mySaleTotalAmount = this.SaleTotalAmount;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostTotalAmount) || Tools_1.AppTool.IsNullOrEmpty(mySaleTotalAmount)) {
            this.SaleTotalAmountColor = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleTotalAmountColor = mySaleTotalAmount < myCostTotalAmount ? Tools_1.FontTool.Red : (mySaleTotalAmount > myCostTotalAmount ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Total Amount Local
        var myCostTotalAmountLocal = this.CostTotalAmountLocal;
        var mySaleTotalAmountLocal = this.SaleTotalAmountLocal;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostTotalAmountLocal) || Tools_1.AppTool.IsNullOrEmpty(mySaleTotalAmountLocal)) {
            this.SaleTotalAmountLocalColor = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleTotalAmountLocalColor = mySaleTotalAmountLocal < myCostTotalAmountLocal ? Tools_1.FontTool.Red : (mySaleTotalAmountLocal > myCostTotalAmountLocal ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
    };
    QuoteChargeItem.prototype.SetUIProperties_VAT = function () {
        this.UIProperties.SetEnabled("VatTypeId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, this.IsEditingEnabled);
        if (this.VatIsMultiPercentage) {
            this.UIProperties.SetEnabled("VatPercentage", this.ObjectTableName, false);
        }
        var isVatPercentageRequired = false;
        if (this.QuotePM.IsChargesByVAT) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
                if (!this.VatIsMultiPercentage) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.VatPercentage)) {
                        isVatPercentageRequired = true;
                    }
                }
            }
        }
        this.UIProperties.SetRequired("VatPercentage", this.ObjectTableName, isVatPercentageRequired);
    };
    Object.defineProperty(QuoteChargeItem.prototype, "ChargesTypeId", {
        // Charges Type
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.OnChargesTypeChanged(null);
                }
                else {
                    this.fatherComponent.myChargesTypeService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.OnChargesTypeChanged(list);
                            }
                            else {
                                _this.fatherComponent.myChargesTypeService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        list = myResponse2.Result;
                                        _this.OnChargesTypeChanged(list);
                                    }
                                });
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteChargeItem.prototype.OnChargesTypeChanged = function (list) {
        if (list) {
            if (this.fatherComponent.IsChargesByVAT) {
                this.VatTypeId = list.VatTypeId;
            }
            this.ChargesTypeCode = list.Code;
            this.ChargesTypeName = list.EnglishName;
            this.ChargesGroupCode = list.ChargesGroupCode;
            this.CostMeasurementId = list.MeasurementId;
            this.EntityPM.IsBackToBack = list.IsBackToBack;
            if (!Tools_1.AppTool.IsNullOrEmpty(list.PayablesDefaultCurrencyId)) {
                this.CostCurrencyId = list.PayablesDefaultCurrencyId;
            }
            else {
                if (this.ChargesGroupCode == "FRT" || this.ChargesGroupCode == "SCH") {
                    this.CostCurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                }
                else {
                    this.CostCurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }
        }
        else {
            this.VatTypeId = null;
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.ChargesGroupCode = null;
            this.CostMeasurementId = null;
            this.CostCurrencyId = null;
            this.EntityPM.IsBackToBack = false;
        }
    };
    Object.defineProperty(QuoteChargeItem.prototype, "ChargesTypeCode", {
        get: function () { return this.EntityPM.ChargesTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargesTypeCode != newValue) {
                this.EntityPM.ChargesTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "ChargesTypeName", {
        get: function () { return this.EntityPM.ChargesTypeName; },
        set: function (newValue) {
            if (this.EntityPM.ChargesTypeName != newValue) {
                this.EntityPM.ChargesTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "ChargesGroupCode", {
        get: function () { return this.EntityPM.ChargesGroupCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargesGroupCode != newValue) {
                this.EntityPM.ChargesGroupCode = newValue;
                this.SetUIProperties_CostFields();
                this.SetUIProperties_SaleFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "ChargesTypeCodeName", {
        get: function () {
            var myResult = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ChargesTypeCode) && !Tools_1.AppTool.IsNullOrEmpty(this.ChargesTypeName)) {
                myResult = "(" + this.ChargesTypeCode + ") " + this.ChargesTypeName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "VatTypeId", {
        // VAT Type
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.VatTypeId != value) {
                this.EntityPM.VatTypeId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.VatTypeName = null;
                    this.VatPercentage = null;
                    this.VatIsMultiPercentage = false;
                    this.EntityPM.ExternalVATCard = null;
                    this.EntityPM.ExternalTAXItemId = null;
                    this.ReadVatTypeData();
                    this.fatherComponent.ComputeTotals();
                    this.fatherComponent.SetGridColumnsWidth();
                }
                else {
                    this.fatherComponent.myVatTypeService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.VatTypeName = list.EnglishName;
                                _this.VatIsMultiPercentage = list.IsMultiPercentage;
                                _this.EntityPM.ExternalVATCard = list.ExternalVATCard;
                                _this.EntityPM.ExternalTAXItemId = list.ExternalTAXItemId;
                                if (list.IsMultiPercentage) {
                                    _this.VatPercentage = null;
                                }
                                else {
                                    _this.VatPercentage = _this.fatherComponent.GetVatTypePercentage(value);
                                }
                                _this.ReadVatTypeData();
                                _this.fatherComponent.ComputeTotals();
                                _this.fatherComponent.SetGridColumnsWidth();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "VatTypeName", {
        get: function () { return this.EntityPM.VatTypeName; },
        set: function (value) {
            if (this.EntityPM.VatTypeName != value) {
                this.EntityPM.VatTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "VatPercentage", {
        get: function () { return this.EntityPM.VatPercentage; },
        set: function (value) {
            if (this.EntityPM.VatPercentage != value) {
                this.EntityPM.VatPercentage = value;
                this.ReadVatTypeData();
                this.SetUIProperties_VAT();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "VatIsMultiPercentage", {
        get: function () { return this.EntityPM.VatIsMultiPercentage; },
        set: function (value) {
            if (this.EntityPM.VatIsMultiPercentage != value) {
                this.EntityPM.VatIsMultiPercentage = value;
                this.SetUIProperties_VAT();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteChargeItem.prototype.ReadVatTypeData = function () {
        var _this = this;
        var myValue = null;
        var myColor = Tools_1.FontTool.Black;
        var isUpdateVisible = false;
        var isMultiIconVisible = false;
        this.VatTypesGroups = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            if (this.VatIsMultiPercentage) {
                myValue = this.VatTypeName;
                myColor = Tools_1.FontTool.Black;
                isMultiIconVisible = true;
                this.VatTypesGroups = SessionLocator_1.SessionLocator.AllVatTypesGroups.filter(function (f) { return f.GroupVATTypeId == _this.VatTypeId; });
            }
            else if (this.VatPercentage != null) {
                myValue = this.VatTypeName + " (" + this.VatPercentage + "%)";
                myColor = Tools_1.FontTool.Black;
            }
            else {
                myValue = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.NoVat");
                myColor = Tools_1.FontTool.Red;
                isUpdateVisible = true;
            }
        }
        this.VatTypeCell = myValue;
        this.VatTypeCellColor = myColor;
        this.VatTypeUpdateIsVisible = isUpdateVisible;
        this.VatTypeMultiIconVisible = isMultiIconVisible;
    };
    QuoteChargeItem.prototype.UpdateVatPercentageClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VatTypeId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Add VAT Type Percentage";
            logWindow.WindowArgs = this.VatTypeId;
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.VatTypeId == _this.VatTypeId; }).forEach(function (item) {
                            item.VatPercentage = comp.Percentage;
                            item.ReadVatTypeData();
                        });
                        _this.fatherComponent.ComputeTotals();
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateVATPercentage/UpdateVATPercentageComponent');
        }
    };
    Object.defineProperty(QuoteChargeItem.prototype, "VendorId", {
        // Vendor
        get: function () { return this.EntityPM.VendorId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.VendorId != value) {
                this.EntityPM.VendorId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.VendorName = null;
                }
                else {
                    this.fatherComponent.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.VendorName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (newValue) {
            if (this.EntityPM.VendorName != newValue) {
                this.EntityPM.VendorName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostMeasurementId", {
        // Cost
        get: function () { return this.EntityPM.CostMeasurementId; },
        set: function (value) {
            if (this.EntityPM.CostMeasurementId != value) {
                this.EntityPM.CostMeasurementId = value;
                this.SaleMeasurementId = value;
                this.OnMeasurementsChanged();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.CostMeasurementCode = null;
                    this.CostMeasurementShortName = null;
                }
                else {
                    var list = this.fatherComponent.AllMeasurements.filter(function (d) { return d.Id == value; })[0];
                    if (list != null) {
                        this.CostMeasurementCode = list.Code;
                        this.CostMeasurementShortName = list.ShortName;
                        if (list.Code == "PRVL") {
                            this.CostCurrencyId = this.QuotePM.ValueOfGoodsCurrencyId;
                        }
                        if (list.Code == "PRFR") {
                            this.CostCurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostMeasurementCode", {
        get: function () { return this.EntityPM.CostMeasurementCode; },
        set: function (newValue) {
            if (this.EntityPM.CostMeasurementCode != newValue) {
                this.EntityPM.CostMeasurementCode = newValue;
                this.SetCostQuantity();
                this.SetUIProperties_CostFields();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostMeasurementShortName", {
        get: function () { return this.EntityPM.CostMeasurementShortName; },
        set: function (newValue) {
            if (this.EntityPM.CostMeasurementShortName != newValue) {
                this.EntityPM.CostMeasurementShortName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostCurrencyId", {
        get: function () { return this.EntityPM.CostCurrencyId; },
        set: function (value) {
            if (this.EntityPM.CostCurrencyId != value) {
                this.EntityPM.CostCurrencyId = value;
                this.SetUIProperties_CostRate();
                this.CostCurrencyCode = this.fatherComponent.GetCurrencyCode(value);
                this.CostExchangeRate = this.fatherComponent.GetCurrencyRate(value);
                this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(Tools_1.DateTool.GetCurrentDateAsUtc(), this.fatherComponent.GetCurrencyRateDate(value), "ago");
                if (this.QuotePM.IsSaleCurrencySameAsCost) {
                    this.SaleCurrencyId = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostCurrencyCode", {
        get: function () { return this.EntityPM.CostCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.CostCurrencyCode != newValue) {
                this.EntityPM.CostCurrencyCode = newValue;
                this.SetEditScreenGridHeaders();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostQuantity", {
        get: function () { return this.EntityPM.CostQuantity; },
        set: function (value) {
            if (this.EntityPM.CostQuantity != value) {
                this.EntityPM.CostQuantity = Tools_1.AppTool.Round(value, 3);
                this.ComputeCostAmounts();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostUnitPrice", {
        get: function () { return this.EntityPM.CostUnitPrice; },
        set: function (value) {
            if (this.EntityPM.CostUnitPrice != value) {
                this.EntityPM.CostUnitPrice = Tools_1.AppTool.Round(value, 3);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.MarkUpValue = 0;
                    this.MarkUpTypeCode = "F";
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostUnitPriceInSaleCurrency", {
        get: function () { return this.EntityPM.CostUnitPriceInSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostUnitPriceInSaleCurrency != value) {
                this.EntityPM.CostUnitPriceInSaleCurrency = Tools_1.AppTool.Round(value, 3);
                this.ComputeSalePrice();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostTotalAmount", {
        get: function () { return this.EntityPM.CostTotalAmount; },
        set: function (value) {
            if (this.EntityPM.CostTotalAmount != value) {
                this.EntityPM.CostTotalAmount = Tools_1.AppTool.Round(value, 2);
                if (this.ChargesGroupCode == "FRT") {
                    this.fatherComponent.OnFreightAmountChanged();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostTotalAmountLocal", {
        get: function () { return this.EntityPM.CostTotalAmountLocal; },
        set: function (value) {
            if (this.EntityPM.CostTotalAmountLocal != value) {
                this.EntityPM.CostTotalAmountLocal = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostAmountInSaleCurrency", {
        get: function () { return this.EntityPM.CostAmountInSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostAmountInSaleCurrency != value) {
                this.EntityPM.CostAmountInSaleCurrency = Tools_1.AppTool.Round(value, 2);
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostExchangeRate", {
        get: function () { return this.EntityPM.CostExchangeRate; },
        set: function (value) {
            if (this.EntityPM.CostExchangeRate != value) {
                this.EntityPM.CostExchangeRate = Tools_1.AppTool.Round(value, 5);
                if (this.QuotePM.IsSaleCurrencySameAsCost) {
                    this.SaleExchangeRate = value;
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostMinAmount", {
        get: function () { return this.EntityPM.CostMinAmount; },
        set: function (value) {
            if (this.EntityPM.CostMinAmount != value) {
                this.EntityPM.CostMinAmount = Tools_1.AppTool.Round(value, 2);
                this.ComputeCostAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostMaxAmount", {
        get: function () { return this.EntityPM.CostMaxAmount; },
        set: function (value) {
            if (this.EntityPM.CostMaxAmount != value) {
                this.EntityPM.CostMaxAmount = Tools_1.AppTool.Round(value, 2);
                this.ComputeCostAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CostIsFixedRate", {
        get: function () { return this.EntityPM.CostIsFixedRate; },
        set: function (value) {
            if (this.EntityPM.CostIsFixedRate != value) {
                this.EntityPM.CostIsFixedRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteChargeItem.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.CostCurrencyId, CurrencyCode: this.CostCurrencyCode, Rate: this.CostExchangeRate, Date: Tools_1.DateTool.GetCurrentDateAsUtc() };
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.fatherComponent.AllRates = comp.RatesList;
                    _this.CostExchangeRate = Tools_1.AppTool.Round(comp.Rate, 5);
                    _this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(Tools_1.DateTool.GetCurrentDateAsUtc(), comp.RateDate, "ago");
                }
            });
        });
    };
    QuoteChargeItem.prototype.SetCostQuantity = function () {
        var myResult = null;
        if (this.IsAdhoc) {
            switch (this.CostMeasurementCode) {
                case "GRWT": {
                    myResult = this.QuotePM.GrossWeight;
                    break;
                }
                case "CHWT": {
                    myResult = this.QuotePM.ChargeableWeight;
                    break;
                }
                case "VOLU": {
                    myResult = this.QuotePM.Volume;
                    break;
                }
                case "BTEU": {
                    myResult = this.QuotePM.TEU;
                    break;
                }
                case "FIXD": {
                    myResult = 1;
                    break;
                }
                case "PRVL": {
                    myResult = this.QuotePM.ValueOfGoods;
                    break;
                }
                case "PRFR": {
                    myResult = Tools_1.ArrayTool.Sum(this.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; }), "CostTotalAmount");
                    break;
                }
                case "GWTN": {
                    myResult = this.QuotePM.GrossWeightPerTon;
                    break;
                }
                case "QTY": {
                    myResult = this.QuotePM.NumberOfPackages;
                    break;
                }
                case "CWKG": {
                    myResult = this.QuotePM.ChargeableWeightInKG;
                    break;
                }
                case "GWKG": {
                    myResult = this.QuotePM.GrossWeightInKG;
                    break;
                }
                case "VCBM": {
                    myResult = this.QuotePM.VolumeInCBM;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        this.CostQuantity = myResult;
    };
    QuoteChargeItem.prototype.ComputeCostAmounts = function () {
        var iAmount = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CostQuantity) && !Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
            if (this.CostMeasurementCode == "PRVL" || this.CostMeasurementCode == "PRFR") {
                iAmount = this.CostQuantity * this.CostUnitPrice / 100;
            }
            else {
                iAmount = this.CostQuantity * this.CostUnitPrice;
            }
        }
        /* MinMax */
        if (iAmount != null) {
            if (this.CostMinAmount != null) {
                if (iAmount < this.CostMinAmount) {
                    iAmount = this.CostMinAmount;
                }
            }
            if (this.CostMaxAmount != null) {
                if (iAmount > this.CostMaxAmount) {
                    iAmount = this.CostMaxAmount;
                }
            }
        }
        // Amounts
        if (Tools_1.AppTool.IsNullOrEmpty(iAmount)) {
            this.CostTotalAmount = null;
            this.CostTotalAmountLocal = null;
            this.CostAmountInSaleCurrency = null;
        }
        else {
            this.CostTotalAmount = iAmount;
            if (Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate)) {
                this.CostTotalAmountLocal = null;
            }
            else {
                this.CostTotalAmountLocal = iAmount * this.CostExchangeRate;
            }
            this.ComputeCostInSaleAmount();
        }
        this.SetUIProperties_CostMinMax();
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    };
    QuoteChargeItem.prototype.ComputeCostInSaleAmount = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostTotalAmount)) {
                myResult = this.CostTotalAmount;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostTotalAmountLocal) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostTotalAmountLocal / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostAmountInSaleCurrency = myResult;
    };
    QuoteChargeItem.prototype.ComputeCostInSalePrice = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
                myResult = this.CostUnitPrice;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostUnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostUnitPriceInSaleCurrency = myResult;
    };
    Object.defineProperty(QuoteChargeItem.prototype, "SaleMeasurementId", {
        // Sale
        get: function () { return this.EntityPM.SaleMeasurementId; },
        set: function (value) {
            if (this.EntityPM.SaleMeasurementId != value) {
                this.EntityPM.SaleMeasurementId = value;
                this.OnMeasurementsChanged();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.SaleMeasurementCode = null;
                    this.SaleMeasurementShortName = null;
                }
                else {
                    var list = this.fatherComponent.AllMeasurements.filter(function (d) { return d.Id == value; })[0];
                    if (list != null) {
                        this.SaleMeasurementCode = list.Code;
                        this.SaleMeasurementShortName = list.ShortName;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleMeasurementCode", {
        get: function () { return this.EntityPM.SaleMeasurementCode; },
        set: function (newValue) {
            if (this.EntityPM.SaleMeasurementCode != newValue) {
                this.EntityPM.SaleMeasurementCode = newValue;
                this.SetSaleQuantity();
                this.SetUIProperties_SaleFields();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleMeasurementShortName", {
        get: function () { return this.EntityPM.SaleMeasurementShortName; },
        set: function (newValue) {
            if (this.EntityPM.SaleMeasurementShortName != newValue) {
                this.EntityPM.SaleMeasurementShortName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleCurrencyId", {
        get: function () { return this.EntityPM.SaleCurrencyId; },
        set: function (value) {
            if (this.EntityPM.SaleCurrencyId != value) {
                this.EntityPM.SaleCurrencyId = value;
                this.SaleCurrencyCode = this.fatherComponent.GetCurrencyCode(value);
                this.SaleExchangeRate = this.fatherComponent.GetCurrencyRate(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleCurrencyCode", {
        get: function () { return this.EntityPM.SaleCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.SaleCurrencyCode != newValue) {
                this.EntityPM.SaleCurrencyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleQuantity", {
        get: function () { return this.EntityPM.SaleQuantity; },
        set: function (value) {
            if (this.EntityPM.SaleQuantity != value) {
                this.EntityPM.SaleQuantity = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleUnitPrice", {
        get: function () { return this.EntityPM.SaleUnitPrice; },
        set: function (value) {
            if (this.EntityPM.SaleUnitPrice != value) {
                this.EntityPM.SaleUnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleMinAmount", {
        get: function () { return this.EntityPM.SaleMinAmount; },
        set: function (newValue) {
            if (this.EntityPM.SaleMinAmount != newValue) {
                this.EntityPM.SaleMinAmount = newValue;
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleMaxAmount", {
        get: function () { return this.EntityPM.SaleMaxAmount; },
        set: function (newValue) {
            if (this.EntityPM.SaleMaxAmount != newValue) {
                this.EntityPM.SaleMaxAmount = newValue;
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleTotalAmount", {
        get: function () { return this.EntityPM.SaleTotalAmount; },
        set: function (ivalue) {
            var value = ivalue;
            if (value != null) {
                if (this.SaleMinAmount != null) {
                    if (value < this.SaleMinAmount) {
                        value = this.SaleMinAmount;
                    }
                }
                if (this.SaleMaxAmount != null) {
                    if (value > this.SaleMaxAmount) {
                        value = this.SaleMaxAmount;
                    }
                }
            }
            if (this.EntityPM.SaleTotalAmount != value) {
                this.EntityPM.SaleTotalAmount = Tools_1.AppTool.Round(value, 2);
                if (this.ChargesGroupCode == "FRT") {
                    this.fatherComponent.OnFreightAmountChanged();
                }
                var myTotalAmount = value;
                var myPrice = this.SaleUnitPrice;
                if (myTotalAmount) {
                    if (this.SaleQuantity > 0) {
                        myPrice = myTotalAmount / this.SaleQuantity;
                    }
                }
                this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.Round(value * this.SaleExchangeRate, 2);
                this.EntityPM.SaleUnitPrice = Tools_1.AppTool.Round(myPrice, 3);
                this.ComputeMarkUp();
                this.SetUIProperties_SaleMinMax();
                this.fatherComponent.ComputeTotals();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleTotalAmountLocal", {
        get: function () { return this.EntityPM.SaleTotalAmountLocal; },
        set: function (ivalue) {
            var value = ivalue;
            if (value != null) {
                var iTotalAmount = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (!Tools_1.AppTool.IsNullOrZero(this.SaleExchangeRate)) {
                        iTotalAmount = value / this.SaleExchangeRate;
                    }
                }
                if (iTotalAmount != null) {
                    if (this.SaleMinAmount != null) {
                        if (iTotalAmount < this.SaleMinAmount) {
                            value = this.EntityPM.SaleTotalAmountLocal;
                        }
                    }
                    if (this.SaleMaxAmount != null) {
                        if (iTotalAmount > this.SaleMaxAmount) {
                            value = this.EntityPM.SaleTotalAmountLocal;
                        }
                    }
                }
            }
            if (this.EntityPM.SaleTotalAmountLocal != value) {
                this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.Round(value, 2);
                var myTotalAmount = null;
                if (value == 0) {
                    myTotalAmount = 0;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (!Tools_1.AppTool.IsNullOrZero(this.SaleExchangeRate)) {
                        myTotalAmount = value / this.SaleExchangeRate;
                    }
                }
                var myPrice = this.SaleUnitPrice;
                if (myTotalAmount) {
                    if (this.SaleQuantity > 0) {
                        myPrice = myTotalAmount / this.SaleQuantity;
                    }
                }
                this.EntityPM.SaleTotalAmount = Tools_1.AppTool.Round(myTotalAmount, 2);
                this.EntityPM.SaleUnitPrice = Tools_1.AppTool.Round(myPrice, 3);
                this.ComputeMarkUp();
                this.SetUIProperties_SaleMinMax();
                this.fatherComponent.ComputeTotals();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "SaleExchangeRate", {
        get: function () { return this.EntityPM.SaleExchangeRate; },
        set: function (value) {
            if (this.EntityPM.SaleExchangeRate != value) {
                this.EntityPM.SaleExchangeRate = Tools_1.AppTool.Round(value, 5);
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteChargeItem.prototype.SetSaleQuantity = function () {
        var myResult = null;
        if (this.IsAdhoc) {
            switch (this.SaleMeasurementCode) {
                case "GRWT": {
                    myResult = this.QuotePM.GrossWeight;
                    break;
                }
                case "CHWT": {
                    myResult = this.QuotePM.ChargeableWeight;
                    break;
                }
                case "VOLU": {
                    myResult = this.QuotePM.Volume;
                    break;
                }
                case "BTEU": {
                    myResult = this.QuotePM.TEU;
                    break;
                }
                case "FIXD": {
                    myResult = 1;
                    break;
                }
                case "PRVL": {
                    myResult = this.QuotePM.ValueOfGoods;
                    break;
                }
                case "PRFR": {
                    myResult = Tools_1.ArrayTool.Sum(this.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; }), "SaleTotalAmount");
                    break;
                }
                case "GWTN": {
                    myResult = this.QuotePM.GrossWeightPerTon;
                    break;
                }
                case "QTY": {
                    myResult = this.QuotePM.NumberOfPackages;
                    break;
                }
                case "CWKG": {
                    myResult = this.QuotePM.ChargeableWeightInKG;
                    break;
                }
                case "GWKG": {
                    myResult = this.QuotePM.GrossWeightInKG;
                    break;
                }
                case "VCBM": {
                    myResult = this.QuotePM.VolumeInCBM;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        this.SaleQuantity = myResult;
    };
    QuoteChargeItem.prototype.ComputeSaleAmounts = function () {
        var myTotalAmount = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleUnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.SaleQuantity)) {
            if (this.SaleMeasurementCode == "PRVL" || this.SaleMeasurementCode == "PRFR") {
                myTotalAmount = this.SaleQuantity * this.SaleUnitPrice / 100;
            }
            else {
                myTotalAmount = this.SaleQuantity * this.SaleUnitPrice;
            }
        }
        /* MinMax */
        if (myTotalAmount != null) {
            if (this.SaleMinAmount != null) {
                if (myTotalAmount < this.SaleMinAmount) {
                    myTotalAmount = this.SaleMinAmount;
                }
            }
            if (this.SaleMaxAmount != null) {
                if (myTotalAmount > this.SaleMaxAmount) {
                    myTotalAmount = this.SaleMaxAmount;
                }
            }
        }
        this.EntityPM.SaleTotalAmount = Tools_1.AppTool.Round(myTotalAmount, 2);
        this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(myTotalAmount) ? null : Tools_1.AppTool.Round(myTotalAmount * this.SaleExchangeRate, 2);
        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.SaleMeasurementCode == "PRFR"; }).forEach(function (item) {
                item.SetSaleQuantity();
            });
        }
        this.SetUIProperties_CellsColors();
        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }
        this.SetUIProperties_SaleMinMax();
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    };
    QuoteChargeItem.prototype.ComputeSalePrice = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPriceInSaleCurrency)) {
            this.SaleUnitPrice = null;
        }
        else {
            var myResult = this.SaleUnitPrice;
            var markup = this.MarkUpValue == null ? 0 : this.MarkUpValue;
            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPriceInSaleCurrency + (this.CostUnitPriceInSaleCurrency * (markup / 100));
            }
            else {
                myResult = this.CostUnitPriceInSaleCurrency + markup;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }
            this.SaleUnitPrice = myResult;
        }
    };
    Object.defineProperty(QuoteChargeItem.prototype, "SaleUnitPriceString", {
        get: function () {
            var myResult = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleUnitPrice)) {
                myResult = this.SaleUnitPrice + "";
            }
            this.mySaleUnitPriceString = myResult;
            return this.mySaleUnitPriceString;
        },
        set: function (value) {
            if (this.mySaleUnitPriceString != value) {
                var mySalePrice = null;
                var myMarkUpValue = 0;
                var myMarkUpCode = "F";
                var myCostPrice = this.CostUnitPriceInSaleCurrency;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    if (value.indexOf("-") > -1 || value.indexOf("+") > -1 || value.indexOf("%") > -1) {
                        var myMarkUpValueInput = value.replace("-", "").replace("+", "").replace("%", "");
                        if (!Tools_1.AppTool.IsNullOrEmpty(myMarkUpValueInput)) {
                            myMarkUpValue = +myMarkUpValueInput;
                            if (value.indexOf("%") > -1) {
                                myMarkUpCode = "P";
                            }
                            if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice)) {
                                if (value.indexOf("+") > -1) {
                                    if (myMarkUpCode == "F") {
                                        mySalePrice = myCostPrice + myMarkUpValue;
                                    }
                                    else {
                                        mySalePrice = myCostPrice + (myCostPrice * myMarkUpValue / 100);
                                    }
                                }
                                else if (value.indexOf("-") > -1) {
                                    if (myMarkUpCode == "F") {
                                        mySalePrice = myCostPrice - myMarkUpValue;
                                    }
                                    else {
                                        mySalePrice = myCostPrice - (myCostPrice * myMarkUpValue / 100);
                                    }
                                }
                            }
                        }
                    }
                    else {
                        mySalePrice = +value;
                        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice)) {
                            myMarkUpValue = mySalePrice - myCostPrice;
                        }
                    }
                }
                this.SaleUnitPrice = mySalePrice;
                this.MarkUpTypeCode = myMarkUpCode;
                this.MarkUpValue = Tools_1.AppTool.Round(myMarkUpValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "MarkUpValue", {
        // Markup
        get: function () { return this.EntityPM.MarkUpValue; },
        set: function (value) {
            if (this.EntityPM.MarkUpValue != value) {
                this.EntityPM.MarkUpValue = Tools_1.AppTool.Round(value, 3);
                this.ComputeMarkUpString();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "MarkUpTypeCode", {
        get: function () { return this.EntityPM.MarkUpTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.MarkUpTypeCode != newValue) {
                this.EntityPM.MarkUpTypeCode = newValue;
                this.ComputeMarkUpString();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "CellMarkupText", {
        get: function () { return this.myCellMarkupText; },
        set: function (value) {
            if (this.myCellMarkupText != value) {
                this.myCellMarkupText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteChargeItem.prototype.ComputeMarkUp = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.EntityPM.MarkUpValue;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPriceInSaleCurrency) && !Tools_1.AppTool.IsNullOrEmpty(this.SaleUnitPrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((this.SaleUnitPrice - this.CostUnitPriceInSaleCurrency) * 100) / this.CostUnitPriceInSaleCurrency;
            }
            else {
                myResult = this.SaleUnitPrice - this.CostUnitPriceInSaleCurrency;
            }
        }
        this.MarkUpValue = myResult == null ? 0 : myResult;
    };
    QuoteChargeItem.prototype.ComputeMarkUpString = function () {
        var myResult = null;
        var markUpValue = this.MarkUpValue;
        var markUpCode = this.MarkUpTypeCode;
        if (!Tools_1.AppTool.IsNullOrZero(markUpValue)) {
            var myCostPrice = Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPriceInSaleCurrency) ? 0 : this.CostUnitPriceInSaleCurrency;
            var mySalePrice = Tools_1.AppTool.IsNullOrEmpty(this.SaleUnitPrice) ? 0 : this.SaleUnitPrice;
            if (mySalePrice > myCostPrice) {
                myResult = "+" + markUpValue;
            }
            else if (mySalePrice < myCostPrice) {
                if (markUpValue < 0) {
                    myResult = "" + markUpValue;
                }
                else {
                    myResult = "-" + markUpValue;
                }
            }
            else {
                myResult = "+" + markUpValue;
            }
            if (markUpCode == "P") {
                myResult = myResult + "%";
            }
        }
        this.CellMarkupText = myResult;
    };
    Object.defineProperty(QuoteChargeItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "IsChargeBySteps", {
        get: function () { return this.EntityPM.IsChargeBySteps; },
        set: function (newValue) {
            if (this.EntityPM.IsChargeBySteps != newValue) {
                this.EntityPM.IsChargeBySteps = newValue;
                if (this.IsChargeBySteps) {
                    this.CostUnitPrice = null;
                    //this.EntityPM.CostUnitPriceInSaleCurrency = null;
                    this.SaleUnitPrice = null;
                    this.MarkUpValue = 0;
                    this.MarkUpTypeCode = "F";
                    //this.ComputeMarkUpString();
                    //this.SetUIProperties_CellsColors();
                }
                else {
                    this.CostMinAmount = null;
                    this.SaleMinAmount = null;
                }
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteChargeItem.prototype, "IsAllIN", {
        get: function () { return this.EntityPM.IsAllIN; },
        set: function (newValue) {
            if (this.EntityPM.IsAllIN != newValue) {
                this.EntityPM.IsAllIN = newValue;
                this.SetUIProperties();
                var allFreightModel = this.fatherComponent.ItemsSource.Collection.filter(function (d) { return d.EntityPM.ChargesGroupCode == "FRT"; });
                allFreightModel.forEach(function (item) {
                    item.SetUIProperties();
                });
                if (this.SaleTotalAmountLocal > 0) {
                    var freightModel = allFreightModel[0];
                    if (freightModel != null) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(freightModel.SaleTotalAmountLocal)) {
                            freightModel.SetUIProperties();
                            var freightTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(freightModel.SaleTotalAmountLocal) ? 0 : freightModel.SaleTotalAmountLocal;
                            if (this.IsAllIN) {
                                freightModel.SaleTotalAmountLocal = freightTotalAmountLocal + this.EntityPM.SaleTotalAmountLocal;
                            }
                            else {
                                freightModel.SaleTotalAmountLocal = freightTotalAmountLocal - this.EntityPM.SaleTotalAmountLocal;
                            }
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteChargeItem.prototype.OnQuoteSaleCurrencyChanged = function () {
        if (this.CostCurrencyId == this.fatherComponent.SaleCurrencyId) {
            if (this.CostExchangeRate != this.fatherComponent.ExchangeRate) {
                this.CostExchangeRate = this.fatherComponent.ExchangeRate;
            }
        }
        else {
            this.CostExchangeRate = this.fatherComponent.GetCurrencyRate(this.CostCurrencyId);
        }
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
        }
        else {
            this.EntityPM.SaleCurrencyId = this.fatherComponent.SaleCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.fatherComponent.SaleCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.fatherComponent.ExchangeRate;
        }
        // InSaleCurrency
        this.ComputeCostInSalePrice();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : Tools_1.AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.SetUIProperties_AllIn();
    };
    QuoteChargeItem.prototype.OnQuoteSaleCurrencySameAsCost = function () {
        if (this.CostCurrencyId == this.QuotePM.SaleCurrencyId) {
            if (this.CostExchangeRate != this.QuotePM.ExchangeRate) {
                this.CostExchangeRate = this.QuotePM.ExchangeRate;
            }
        }
        else {
            this.CostExchangeRate = this.fatherComponent.GetCurrencyRate(this.CostCurrencyId);
        }
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            this.EntityPM.SaleCurrencyId = this.EntityPM.CostCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.EntityPM.CostCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.EntityPM.CostExchangeRate;
        }
        else {
            this.EntityPM.SaleCurrencyId = this.fatherComponent.SaleCurrencyId;
            this.EntityPM.SaleCurrencyCode = this.fatherComponent.SaleCurrencyCode;
            this.EntityPM.SaleExchangeRate = this.fatherComponent.ExchangeRate;
        }
        // InSaleCurrency
        this.ComputeCostInSalePrice();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : Tools_1.AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.SetUIProperties_AllIn();
    };
    QuoteChargeItem.prototype.OnMeasurementsChanged = function () {
        if (this.CostMeasurementId != this.SaleMeasurementId) {
            this.EntityPM.MarkUpValue = 0;
            if (this.SaleUnitPrice) {
                this.mySaleUnitPriceString = this.SaleUnitPrice + "";
            }
            else {
                this.mySaleUnitPriceString = null;
            }
        }
        else {
            this.ComputeMarkUp();
        }
    };
    QuoteChargeItem.prototype.SetEditScreenGridHeaders = function () {
        if (this.fatherComponent.IsSaleCurrencySameAsCost) {
            var myCurrencyCode = Tools_1.AppTool.IsNullOrEmpty(this.CostCurrencyCode) ? "" : this.CostCurrencyCode;
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }
        else {
            var myCurrencyCode = Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.SaleCurrencyCode) ? "" : this.fatherComponent.SaleCurrencyCode;
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount", false).replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }
    };
    return QuoteChargeItem;
}(BaseComponent_1.BaseComponent));
exports.QuoteChargeItem = QuoteChargeItem;
//# sourceMappingURL=LCLChargesComponent.js.map