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
var PackageTypeListService_1 = require("../../../Common/Services/StandardLists/PackageTypeListService");
var CurrencyRatesService_1 = require("../../../Common/Services/CurrencyRatesService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var DecimalFormatter_1 = require("../../../Infrastructure/Utilities/DecimalFormatter");
var FCLChargesComponent = /** @class */ (function (_super) {
    __extends(FCLChargesComponent, _super);
    function FCLChargesComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "Quote";
        _this.DataContext = _this;
        _this.IsAdhoc = true;
        _this.IsRoutingRate = false;
        _this.IsShowTotalPerContainer = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.AllRates = [];
        _this.AllCurrencies = [];
        _this.AllMeasurements = [];
        _this.AllVatTypes = [];
        _this.AllPackageTypes = [];
        _this.AllVatPercentages = [];
        // SetLabels
        _this.CostQuentityHeader = [];
        _this.CostPriceHeader = [];
        _this.CostAmountHeader = [];
        _this.SaleQuantityHeader = [];
        _this.SalePriceHeader = [];
        _this.SaleAmountHeader = [];
        _this.SaleLocalAmountHeader = [];
        _this.Cost1Header = [];
        _this.Cost2Header = [];
        _this.Cost3Header = [];
        _this.Cost4Header = [];
        _this.Cost5Header = [];
        _this.Sale1Header = [];
        _this.Sale2Header = [];
        _this.Sale3Header = [];
        _this.Sale4Header = [];
        _this.Sale5Header = [];
        _this.CostMinAmountHeader = [];
        _this.CostMaxAmountHeader = [];
        _this.SaleMinAmountHeader = [];
        _this.SaleMaxAmountHeader = [];
        _this.Cost1HeaderTooltip = null;
        _this.Cost2HeaderTooltip = null;
        _this.Cost3HeaderTooltip = null;
        _this.Cost4HeaderTooltip = null;
        _this.Cost5HeaderTooltip = null;
        _this.Sale1HeaderTooltip = null;
        _this.Sale2HeaderTooltip = null;
        _this.Sale3HeaderTooltip = null;
        _this.Sale4HeaderTooltip = null;
        _this.Sale5HeaderTooltip = null;
        _this.VATColumnWidth = 100;
        _this.IsCostMinMaxColumnVisible = false;
        _this.IsSaleMinMaxColumnVisible = false;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.IsExchangeRateEnabled = true;
        _this.IsCurrencyFilterVisible = true;
        _this.IsCostQuantityVisible = false;
        _this.IsCostPriceVisible = false;
        _this.IsSaleQuantityVisible = false;
        _this.IsSalePriceVisible = false;
        _this.IsCost1Visible = false;
        _this.IsCost2Visible = false;
        _this.IsCost3Visible = false;
        _this.IsCost4Visible = false;
        _this.IsCost5Visible = false;
        _this.IsSale1Visible = false;
        _this.IsSale2Visible = false;
        _this.IsSale3Visible = false;
        _this.IsSale4Visible = false;
        _this.IsSale5Visible = false;
        _this.SelectedRow = null;
        _this.IsLocalCurrency = false;
        _this.isFixedCurrency = false;
        _this.isSameCostCurrency = false;
        _this.DisableSameCostCurrency = false;
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
        _this.AllInMatchText = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.UnableToDoAllIn") + "\n" + TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.M.IfMatchesFrieghtCharge");
        //if( FeatureLocator.HasFeaturePermession("Quote", "QUOTEQUOTATION")) {
        //     this.IsShowTotalPerContainer = true;
        // }
        _this.InitializeServices();
        _this.LoadRequiredData();
        _this.SetLabels();
        _this.SetUIProperties();
        _this.CheckUpdateQuantities();
        _this.SetGridColumns();
        _this.BuildItemsSource();
        _this.InitializeProfit();
        _this.Listen();
        return _this;
    }
    FCLChargesComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "FCLPackagesChanged") {
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildProfitData();
                }
            });
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
                    _this.SetLabelsAttached();
                    _this.SetUIProperties();
                    _this.UpdateCharges();
                }
            });
        }
    };
    FCLChargesComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    FCLChargesComponent.prototype.InitializeServices = function () {
        this.myCardListService = new CardListService_1.CardListService();
        this.myVatTypeService = new VatTypeListService_1.VatTypeListService();
        this.myCurrencyService = new CurrencyListService_1.CurrencyListService();
        this.myChargesTypeService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myMeasurementService = new MeasurementListService_1.MeasurementListService();
        this.myPackageTypeService = new PackageTypeListService_1.PackageTypeListService();
        this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        this.myCommonDomainService = new CommonDomainService_1.CommonDomainService();
    };
    FCLChargesComponent.prototype.LoadRequiredData = function () {
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
        this.myPackageTypeService.getAllFromCache().subscribe(function (myResponse6) {
            if (!myResponse6.HasError) {
                _this.AllPackageTypes = myResponse6.Result;
                _this.SetLabelsAttached();
            }
        });
    };
    FCLChargesComponent.prototype.GetVatTypePercentage = function (vatTypeId) {
        var myResult = null;
        var vatTypePercentagePM = this.AllVatPercentages.filter(function (d) { return d.VatTypeId == vatTypeId; })[0];
        if (vatTypePercentagePM != null) {
            myResult = vatTypePercentagePM.Percentage;
        }
        return myResult;
    };
    FCLChargesComponent.prototype.SetLabels = function () {
        this.CostQuentityHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostQuantity").split('%n');
        this.CostPriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostPrice").split('%n');
        this.CostAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostAmount").split('%n');
        this.SaleQuantityHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleQuantity").split('%n');
        this.SaleLocalAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmountLocal").replace("%LocalCurrencyCode", this.LocalCurrencyCode).split('%n');
        this.CostMinAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostMinAmount", false).split('%n');
        this.CostMaxAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.CostMaxAmount", false).split('%n');
        this.SaleMinAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleMinAmount", false).split('%n');
        this.SaleMaxAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleMaxAmount", false).split('%n');
        this.SetLabelsAttached();
    };
    FCLChargesComponent.prototype.SetLabelsAttached = function () {
        var _this = this;
        var myCostString = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.Cost");
        var mySaleCurrencyCode = Tools_1.AppTool.IsNullOrEmpty(this.SaleCurrencyCode) ? "" : this.SaleCurrencyCode;
        if (this.EntityPM.IsSaleCurrencySameAsCost) {
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").split('%n');
        }
        else {
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", mySaleCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount").replace("%SaleCurrencyCode", mySaleCurrencyCode).split('%n');
        }
        this.Cost1Header = [2];
        this.Cost2Header = [2];
        this.Cost3Header = [2];
        this.Cost4Header = [2];
        this.Cost5Header = [2];
        this.Sale1Header = [3];
        this.Sale2Header = [3];
        this.Sale3Header = [3];
        this.Sale4Header = [3];
        this.Sale5Header = [3];
        var q1 = Tools_1.AppTool.IsNullOrZero(this.EntityPM.PackageType1Quantity) ? "" : this.EntityPM.PackageType1Quantity.toString() + "X";
        var q2 = Tools_1.AppTool.IsNullOrZero(this.EntityPM.PackageType2Quantity) ? "" : this.EntityPM.PackageType2Quantity.toString() + "X";
        var q3 = Tools_1.AppTool.IsNullOrZero(this.EntityPM.PackageType3Quantity) ? "" : this.EntityPM.PackageType3Quantity.toString() + "X";
        var q4 = Tools_1.AppTool.IsNullOrZero(this.EntityPM.PackageType4Quantity) ? "" : this.EntityPM.PackageType4Quantity.toString() + "X";
        var q5 = Tools_1.AppTool.IsNullOrZero(this.EntityPM.PackageType5Quantity) ? "" : this.EntityPM.PackageType5Quantity.toString() + "X";
        var p1 = "";
        var p2 = "";
        var p3 = "";
        var p4 = "";
        var p5 = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            var item = this.AllPackageTypes.filter(function (d) { return d.Id == _this.EntityPM.PackageType1Id; })[0];
            if (item) {
                p1 = item.Code;
                var value1 = q1 + " " + item.EnglishName;
                this.Cost1HeaderTooltip = myCostString + " " + q1 + value1;
                this.Sale1HeaderTooltip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value1;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            var item = this.AllPackageTypes.filter(function (d) { return d.Id == _this.EntityPM.PackageType2Id; })[0];
            if (item) {
                p2 = item.Code;
                var value2 = q2 + " " + item.EnglishName;
                this.Cost2HeaderTooltip = myCostString + " " + value2;
                this.Sale2HeaderTooltip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value2;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            var item = this.AllPackageTypes.filter(function (d) { return d.Id == _this.EntityPM.PackageType3Id; })[0];
            if (item) {
                p3 = item.Code;
                var value3 = q3 + " " + item.EnglishName;
                this.Cost3HeaderTooltip = myCostString + " " + value3;
                this.Sale3HeaderTooltip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value3;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            var item = this.AllPackageTypes.filter(function (d) { return d.Id == _this.EntityPM.PackageType4Id; })[0];
            if (item) {
                p4 = item.Code;
                var value4 = q4 + " " + item.EnglishName;
                this.Cost4HeaderTooltip = myCostString + " " + value4;
                this.Sale4HeaderTooltip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value4;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
            var item = this.AllPackageTypes.filter(function (d) { return d.Id == _this.EntityPM.PackageType5Id; })[0];
            if (item) {
                p5 = item.Code;
                var value5 = q5 + " " + item.EnglishName;
                this.Cost5HeaderTooltip = myCostString + " " + value5;
                this.Sale5HeaderTooltip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", "").replace("(", "").replace(")", "").replace("%n", " ").replace("\n", " ") + " " + value5;
            }
        }
        this.Cost1Header[0] = myCostString;
        this.Cost2Header[0] = myCostString;
        this.Cost3Header[0] = myCostString;
        this.Cost4Header[0] = myCostString;
        this.Cost5Header[0] = myCostString;
        this.Cost1Header[1] = q1 + p1;
        this.Cost2Header[1] = q2 + p2;
        this.Cost3Header[1] = q3 + p3;
        this.Cost4Header[1] = q4 + p4;
        this.Cost5Header[1] = q5 + p5;
        this.Sale1Header[0] = this.SalePriceHeader[0];
        this.Sale2Header[0] = this.SalePriceHeader[0];
        this.Sale3Header[0] = this.SalePriceHeader[0];
        this.Sale4Header[0] = this.SalePriceHeader[0];
        this.Sale5Header[0] = this.SalePriceHeader[0];
        this.Sale1Header[1] = this.SalePriceHeader[1];
        this.Sale2Header[1] = this.SalePriceHeader[1];
        this.Sale3Header[1] = this.SalePriceHeader[1];
        this.Sale4Header[1] = this.SalePriceHeader[1];
        this.Sale5Header[1] = this.SalePriceHeader[1];
        this.Sale1Header[2] = q1 + p1;
        this.Sale2Header[2] = q2 + p2;
        this.Sale3Header[2] = q3 + p3;
        this.Sale4Header[2] = q4 + p4;
        this.Sale5Header[2] = q5 + p5;
    };
    FCLChargesComponent.prototype.SetGridColumns = function () {
        var isCostMinMaxColumnVisible = false;
        var isSaleMinMaxColumnVisible = false;
        if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode != "BCNT"; }).length > 0) {
            isCostMinMaxColumnVisible = true;
        }
        if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode != "BCNT"; }).length > 0) {
            isSaleMinMaxColumnVisible = true;
        }
        this.IsCostMinMaxColumnVisible = isCostMinMaxColumnVisible;
        this.IsSaleMinMaxColumnVisible = isSaleMinMaxColumnVisible;
    };
    FCLChargesComponent.prototype.SetGridColumnsWidth = function () {
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
    FCLChargesComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.ItemsSource.Collection.forEach(function (item) {
            item.SetUIProperties();
        });
        this.SetUIProperties_Summary();
        this.SetUIProperties_Columns();
    };
    FCLChargesComponent.prototype.SetUIProperties_Summary = function () {
        var isExchangeRateEnabled = false;
        if (this.IsEditingEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "QouteEditExchangeRate")) {
                if (this.SaleCurrencyId) {
                    if (this.SaleCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                        isExchangeRateEnabled = false;
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
    FCLChargesComponent.prototype.SetUIProperties_Columns = function () {
        var isCostQuantityVisible = false;
        var isCostPriceVisible = false;
        var isSaleQuantityVisible = false;
        var isSalePriceVisible = false;
        var isCost1Visible = false;
        var isCost2Visible = false;
        var isCost3Visible = false;
        var isCost4Visible = false;
        var isCost5Visible = false;
        var isSale1Visible = false;
        var isSale2Visible = false;
        var isSale3Visible = false;
        var isSale4Visible = false;
        var isSale5Visible = false;
        if (this.EntityPM.QuoteCharges.filter(function (d) { return d.CostMeasurementCode != "BCNT"; }).length > 0) {
            if (this.IsAdhoc) {
                isCostQuantityVisible = true;
                isSaleQuantityVisible = true;
            }
            isCostPriceVisible = true;
            isSalePriceVisible = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType1Id)) {
            isCost1Visible = true;
            isSale1Visible = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType2Id)) {
            isCost2Visible = true;
            isSale2Visible = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType3Id)) {
            isCost3Visible = true;
            isSale3Visible = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType4Id)) {
            isCost4Visible = true;
            isSale4Visible = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PackageType5Id)) {
            isCost5Visible = true;
            isSale5Visible = true;
        }
        this.IsCostQuantityVisible = isCostQuantityVisible;
        this.IsCostPriceVisible = isCostPriceVisible;
        this.IsSaleQuantityVisible = isSaleQuantityVisible;
        this.IsSalePriceVisible = isSalePriceVisible;
        this.IsCost1Visible = isCost1Visible;
        this.IsCost2Visible = isCost2Visible;
        this.IsCost3Visible = isCost3Visible;
        this.IsCost4Visible = isCost4Visible;
        this.IsCost5Visible = isCost5Visible;
        this.IsSale1Visible = isSale1Visible;
        this.IsSale2Visible = isSale2Visible;
        this.IsSale3Visible = isSale3Visible;
        this.IsSale4Visible = isSale4Visible;
        this.IsSale5Visible = isSale5Visible;
    };
    FCLChargesComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    FCLChargesComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var itemsCollection = [];
        this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).forEach(function (item) {
            itemsCollection.push(new FCLQuoteChargeItem(item, _this, false));
        });
        this.EntityPM.QuoteCharges.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).forEach(function (item) {
            itemsCollection.push(new FCLQuoteChargeItem(item, _this, false));
        });
        this.ItemsSource.InsertCollection(itemsCollection);
        this.SetGridColumnsWidth();
    };
    // Commands
    FCLChargesComponent.prototype.AddChargeClicked = function () {
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
        var itemComponent = new FCLQuoteChargeItem(newItem, this, true);
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.AddCharges");
        this.RunAddEditCharge(itemComponent, title);
    };
    FCLChargesComponent.prototype.EditChargeClicked = function (itemComponent) {
        var title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.EditCharges");
        this.RunAddEditCharge(itemComponent, title);
    };
    FCLChargesComponent.prototype.DeleteChargeClicked = function (itemComponent) {
        var _this = this;
        if (itemComponent.EntityPM.ChargesGroupCode == "FRT" && this.EntityPM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            var window = new MessageWindow_1.MessageWindow();
            window.Show("Can't delete this charge because it's connected to other All In charges");
            window.WindowClosed.subscribe(function (event) {
            });
        }
        else if (itemComponent.EntityPM.IsAllIN) {
            var window = new MessageWindow_1.MessageWindow();
            window.Show("Can't delete this charge because it's All In");
            window.WindowClosed.subscribe(function (event) {
            });
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
                    _this.SetGridColumns();
                    _this.BuildItemsSource();
                    _this.ComputeTotals();
                }
            });
        }
    };
    FCLChargesComponent.prototype.RunAddEditCharge = function (itemComponent, windowTitle) {
        var _this = this;
        itemComponent.SetEditScreenGridHeaders();
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 880;
        logitudeWindow.Height = 550;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/AddEditFCLChargeComponent');
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.SetGridColumns();
            }
        });
    };
    FCLChargesComponent.prototype.GetCurrencyCode = function (myCurrencyId) {
        var myCode = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCurrencyId)) {
            var list = this.AllCurrencies.filter(function (d) { return d.Id == myCurrencyId; })[0];
            if (list != null) {
                myCode = list.Code;
            }
        }
        return myCode;
    };
    FCLChargesComponent.prototype.GetCurrencyRate = function (myCurrencyId) {
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
    FCLChargesComponent.prototype.GetCurrencyRateDate = function (myCurrencyId) {
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
    FCLChargesComponent.prototype.InitializeProfit = function () {
        this.SelectedCurrencyCode = this.SaleCurrencyCode;
        this.IsSameCostCurrency = this.IsSaleCurrencySameAsCost;
        this.IsFixedCurrency = !this.IsSameCostCurrency;
        this.BuildProfitData();
    };
    FCLChargesComponent.prototype.OnSelectCurrency = function (myArgs) {
        this.SelectedCurrencyCode = myArgs;
        if (myArgs == this.LocalCurrencyCode) {
            this.IsLocalCurrency = true;
        }
        else {
            this.IsLocalCurrency = false;
        }
        this.BuildProfitData();
    };
    FCLChargesComponent.prototype.SetFixedSameCurrency = function (setType) {
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
                    _this.IsSaleCurrencySameAsCost = _this.IsSameCostCurrency;
                });
            }
            else {
                this.IsFixedCurrency = false;
                this.IsSameCostCurrency = true;
                this.IsSaleCurrencySameAsCost = this.IsSameCostCurrency;
                this.OnFixedSameChanges();
            }
        }
    };
    FCLChargesComponent.prototype.OnFixedSameChanges = function () {
        var _this = this;
        this.ItemsSource.Collection.forEach(function (item) {
            if (item.CostCurrencyId) {
                if (item.CostCurrencyId != _this.EntityPM.SaleCurrencyId) {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostUnitPrice)) {
                        item.SaleUnitPrice = null;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType1UnitPrice)) {
                        item.SaleUnitPrice1String = null;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType2UnitPrice)) {
                        item.SaleUnitPrice2String = null;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType3UnitPrice)) {
                        item.SaleUnitPrice3String = null;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType4UnitPrice)) {
                        item.SaleUnitPrice4String = null;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.CostContainerType5UnitPrice)) {
                        item.SaleUnitPrice5String = null;
                    }
                }
            }
            item.OnQuoteSaleCurrencySameAsCost();
        });
    };
    Object.defineProperty(FCLChargesComponent.prototype, "SelectedCurrencyCode", {
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
    Object.defineProperty(FCLChargesComponent.prototype, "IsFixedCurrency", {
        get: function () { return this.isFixedCurrency; },
        set: function (value) {
            if (this.isFixedCurrency != value) {
                this.isFixedCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLChargesComponent.prototype, "IsSameCostCurrency", {
        get: function () { return this.isSameCostCurrency; },
        set: function (value) {
            if (this.isSameCostCurrency != value) {
                this.isSameCostCurrency = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLChargesComponent.prototype, "IsSaleCurrencySameAsCost", {
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
    Object.defineProperty(FCLChargesComponent.prototype, "SaleCurrencyId", {
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
    Object.defineProperty(FCLChargesComponent.prototype, "SaleCurrencyCode", {
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
    Object.defineProperty(FCLChargesComponent.prototype, "ExchangeRate", {
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
    FCLChargesComponent.prototype.UpdateCurrencyRateClicked = function () {
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
    Object.defineProperty(FCLChargesComponent.prototype, "IsFixedPrice", {
        get: function () { return this.EntityPM.IsFixedPrice; },
        set: function (newValue) {
            if (this.EntityPM.IsFixedPrice != newValue) {
                this.EntityPM.IsFixedPrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLChargesComponent.prototype, "TotalPerContainer", {
        get: function () {
            //if (this.EntityPM.TotalPerContainer == true) {
            //    this.DisableSameCostCurrency = true;
            //}
            //else this.DisableSameCostCurrency = false;
            return this.EntityPM.TotalPerContainer;
        },
        set: function (newValue) {
            if (this.EntityPM.TotalPerContainer != newValue) {
                this.EntityPM.TotalPerContainer = newValue;
                // IsSameCostCurrency
                //if (newValue == true) {
                //    this.IsSameCostCurrency = false;
                //    if (this.IsFixedCurrency == false) {
                //        this.IsFixedCurrency = true;
                //        this.SetFixedSameCurrency("F");
                //    }
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLChargesComponent.prototype, "EstimateProfit", {
        get: function () { return this.EntityPM.EstimateProfit; },
        set: function (value) {
            if (this.EntityPM.EstimateProfit != value) {
                this.EntityPM.EstimateProfit = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLChargesComponent.prototype, "EstimateProfitEdited", {
        get: function () { return this.EntityPM.EstimateProfitEdited; },
        set: function (newValue) {
            if (this.EntityPM.EstimateProfitEdited != newValue) {
                this.EntityPM.EstimateProfitEdited = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    FCLChargesComponent.prototype.BuildProfitData = function () {
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
    FCLChargesComponent.prototype.CheckUpdateQuantities = function () {
        var _this = this;
        if (this.IsAdhoc) {
            var updateMessage = null;
            var entityQuantity = null;
            var isDifferentOrders = false;
            entityQuantity = this.EntityPM.TEU;
            if (this.EntityPM.QuoteCharges.filter(function (f) { return f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity; }).length > 0) {
                isDifferentOrders = true;
            }
            else if (this.EntityPM.QuoteCharges.filter(function (f) { return f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity; }).length > 0) {
                isDifferentOrders = true;
            }
            if (this.EntityPM.QuoteCharges.filter(function (d) { return d.SaleUnitPrice != null || d.CostUnitPrice != null; }).length > 0) {
                if (this.EntityPM.QuoteCharges.filter(function (d) { return (d.CostMeasurementCode == "PRVL" && d.CostQuantity != _this.EntityPM.ValueOfGoods) || (d.CostMeasurementCode == "PRVL" && d.CostQuantity != _this.EntityPM.ValueOfGoods); }).length > 0) {
                    updateMessage = "You have updated the Value of Goods, apply the new values?";
                }
                else if (this.EntityPM.QuoteCharges.filter(function (d) { return d.SaleMeasurementCode == "PRVL" && d.SaleQuantity != _this.EntityPM.ValueOfGoods; }).length > 0) {
                    updateMessage = "You have updated the Value of Goods, apply the new values?";
                }
            }
            if (isDifferentOrders) {
                updateMessage = "You have updated the expected order details, apply the new values?";
            }
            this.UpdateQuantitiesMessage = updateMessage;
            this.UpdateQuantitiesMessageWidth = Tools_1.AppTool.GetTextWidth(updateMessage, 11);
            this.IsUpdateQuantitiesVisible = Tools_1.AppTool.IsNullOrEmpty(updateMessage) ? false : true;
        }
    };
    FCLChargesComponent.prototype.UpdateQuantitiesClicked = function () {
        this.ItemsSource.Collection.forEach(function (item) {
            item.SetCostQuantity();
            item.SetSaleQuantity();
        });
        //this.SetUIProperties_UpdateCharges();
    };
    FCLChargesComponent.prototype.UpdateCharges = function () {
        var _this = this;
        this.EntityPM.QuoteCharges.forEach(function (item) {
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.PackageType1Id)) {
                item.CostContainerType1UnitPrice = null;
                item.SaleContainerType1UnitPrice = null;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.PackageType2Id)) {
                item.CostContainerType2UnitPrice = null;
                item.SaleContainerType2UnitPrice = null;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.PackageType3Id)) {
                item.CostContainerType3UnitPrice = null;
                item.SaleContainerType3UnitPrice = null;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.PackageType4Id)) {
                item.CostContainerType4UnitPrice = null;
                item.SaleContainerType4UnitPrice = null;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.PackageType5Id)) {
                item.CostContainerType5UnitPrice = null;
                item.SaleContainerType5UnitPrice = null;
            }
        });
        this.BuildItemsSource();
    };
    FCLChargesComponent.prototype.ComputeTotals = function () {
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
    Object.defineProperty(FCLChargesComponent.prototype, "IsChargesByVAT", {
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
    FCLChargesComponent.prototype.BuildTotalVATs = function () {
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
    FCLChargesComponent.prototype.AddVatGroupItem = function (myQroups, vatTypeId, myCharge) {
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
    FCLChargesComponent.prototype.VATDetailsClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.VATDetails");
        logitudeWindow.WindowArgs = { IsLocalCurrency: this.IsLocalCurrency, SaleCurrencyCode: this.SaleCurrencyCode, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible, TotalVATs: this.EntityPM.TotalVATs };
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/QuoteVATDetailsComponent');
    };
    FCLChargesComponent.prototype.OnFreightAmountChanged = function () {
        this.ItemsSource.Collection.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).forEach(function (item) {
            if (item.CostMeasurementCode == "PRFR") {
                item.SetCostQuantity();
            }
            if (item.SaleMeasurementCode == "PRFR") {
                item.SetSaleQuantity();
            }
        });
    };
    FCLChargesComponent = __decorate([
        core_1.Component({
            selector: 'FCLChargesComponent',
            moduleId: module.id,
            templateUrl: './FCLChargesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], FCLChargesComponent);
    return FCLChargesComponent;
}(BaseComponent_1.BaseComponent));
exports.FCLChargesComponent = FCLChargesComponent;
var FCLQuoteChargeItem = /** @class */ (function (_super) {
    __extends(FCLQuoteChargeItem, _super);
    function FCLQuoteChargeItem(entity, fatherComponent, isNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "QuoteCharge";
        _this.DataContext = _this;
        _this.IsNew = false;
        _this.IsAdhoc = false;
        _this.IsRoutingRate = false;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        _this.IsEditExchangeRateVisible = false;
        _this.IsAllInCheckBoxVisible = false;
        _this.IsAllInInfoIconVisible = false;
        _this.IsAllInInfoIconVisible_AmoutLocal = false;
        _this.IsEnabled_CostQuantity = false;
        _this.IsEnabled_CostUnitPrice = false;
        _this.IsEnabled_CostMinAmount = false;
        _this.IsEnabled_CostExchangeRate = false;
        _this.IsEnabled_CostUnitPriceFCL = false;
        _this.IsEnabled_SaleQuantity = false;
        _this.IsEnabled_SaleUnitPrice = false;
        _this.IsEnabled_SaleMinAmount = false;
        _this.IsEnabled_SaleUnitPriceFCL = false;
        _this.SaleUnitPriceColor = Tools_1.FontTool.Black;
        _this.SaleUnitPrice1Color = Tools_1.FontTool.Black;
        _this.SaleUnitPrice2Color = Tools_1.FontTool.Black;
        _this.SaleUnitPrice3Color = Tools_1.FontTool.Black;
        _this.SaleUnitPrice4Color = Tools_1.FontTool.Black;
        _this.SaleUnitPrice5Color = Tools_1.FontTool.Black;
        _this.SaleTotalAmountColor = Tools_1.FontTool.Black;
        _this.SaleTotalAmountLocalColor = Tools_1.FontTool.Black;
        _this.IsCostQuantityColumnVisible = false;
        _this.IsCostPriceColumnVisible = false;
        _this.IsSaleQuantityColumnVisible = false;
        _this.IsSalePriceColumnVisible = false;
        _this.IsCost1Visible = false;
        _this.IsCost2Visible = false;
        _this.IsCost3Visible = false;
        _this.IsCost4Visible = false;
        _this.IsCost5Visible = false;
        _this.IsSale1Visible = false;
        _this.IsSale2Visible = false;
        _this.IsSale3Visible = false;
        _this.IsSale4Visible = false;
        _this.IsSale5Visible = false;
        _this.CostMinMaxIconTitle = "";
        _this.CostMinMaxIconIsVisibile = false;
        _this.SaleMinMaxIconTitle = "";
        _this.SaleMinMaxIconIsVisibile = false;
        _this.VatTypeUpdateIsVisible = false;
        _this.VatTypeMultiIconVisible = false;
        _this.VatTypesGroups = [];
        _this.mySaleUnitPriceString = null;
        _this.mySaleUnitPrice1String = null;
        _this.mySaleUnitPrice2String = null;
        _this.mySaleUnitPrice3String = null;
        _this.mySaleUnitPrice4String = null;
        _this.mySaleUnitPrice5String = null;
        _this.CellMarkupText = null;
        _this.CellMarkup1Text = null;
        _this.CellMarkup2Text = null;
        _this.CellMarkup3Text = null;
        _this.CellMarkup4Text = null;
        _this.CellMarkup5Text = null;
        _this.SalePriceHeader = [];
        _this.SaleAmountHeader = [];
        _this.Sale1Header = [];
        _this.Sale2Header = [];
        _this.Sale3Header = [];
        _this.Sale4Header = [];
        _this.Sale5Header = [];
        _this.IsNew = isNew;
        _this.EntityPM = entity;
        _this.QuotePM = fatherComponent.EntityPM;
        _this.IsAdhoc = fatherComponent.IsAdhoc;
        _this.IsRoutingRate = fatherComponent.IsRoutingRate;
        _this.TransportModeId = fatherComponent.TransportModeId;
        _this.AllInMatchText = fatherComponent.AllInMatchText;
        _this.SetUIProperties();
        _this.ReadVatTypeData();
        _this.ComputeMarkUpString();
        _this.ComputeMarkUp1String();
        _this.ComputeMarkUp2String();
        _this.ComputeMarkUp3String();
        _this.ComputeMarkUp4String();
        _this.ComputeMarkUp5String();
        return _this;
    }
    FCLQuoteChargeItem.prototype.SetUIProperties = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            if (this.IsAdhoc) {
                this.IsEditExchangeRateVisible = true;
            }
        }
        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.SetUIProperties_AllIn();
        this.SetUIProperties_Columns();
        this.SetUIProperties_CostFields();
        this.SetUIProperties_SaleFields();
        this.SetUIProperties_CellsColors();
        this.SetUIProperties_VAT();
        this.SetUIProperties_CostMinMax();
        this.SetUIProperties_SaleMinMax();
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, (this.IsEditingEnabled && !this.IsAllIN) ? true : false);
        this.UIProperties.SetEnabled("CostCurrencyId", this.ObjectTableName, (this.IsEditingEnabled && !this.IsAllIN) ? true : false);
        this.UIProperties.SetEnabled("CostIsFixedRate", this.ObjectTableName, (this.IsEditingEnabled && !this.IsAllIN) ? true : false);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsChargeBySteps", this.ObjectTableName, this.IsEditingEnabled);
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_AllIn = function () {
        var isAllInCheckBoxVisible = false;
        var isAllInInfoIconVisible = false;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            isAllInCheckBoxVisible = false;
            //isAllInInfoIconVisible = false;
        }
        if (this.ChargesGroupCode == "FRT") {
            isAllInCheckBoxVisible = false;
            isAllInInfoIconVisible = false;
        }
        //else if (this.IsRoutingRate) {
        //    isAllInCheckBoxVisible = false;
        //    isAllInInfoIconVisible = false;
        //}
        else if (this.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; }).length == 0) {
            isAllInCheckBoxVisible = false;
            isAllInInfoIconVisible = false;
        }
        else {
            var freightCharge = this.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; })[0];
            if (this.CostMeasurementId == freightCharge.CostMeasurementId && this.CostCurrencyId == freightCharge.CostCurrencyId) {
                isAllInCheckBoxVisible = true;
            }
            else {
                isAllInInfoIconVisible = true;
            }
        }
        this.IsAllInCheckBoxVisible = isAllInCheckBoxVisible;
        this.IsAllInInfoIconVisible = isAllInInfoIconVisible;
        this.IsAllInInfoIconVisible_AmoutLocal = this.IsAllIN && this.CostMeasurementCode == "BCNT" ? true : false;
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_CostFields = function () {
        var isEnabled_CostQuantity = false;
        var isEnabled_CostUnitPrice = false;
        var isEnabled_CostMinAmount = false;
        var isEnabled_CostMeasurement = false;
        var isEnabled_CostUnitPriceFCL = false;
        if (this.IsEditingEnabled) {
            isEnabled_CostQuantity = true;
            isEnabled_CostUnitPrice = true;
            isEnabled_CostMinAmount = true;
            isEnabled_CostMeasurement = true;
            isEnabled_CostUnitPriceFCL = true;
            switch (this.CostMeasurementCode) {
                //case "GRWT":
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
                isEnabled_CostMeasurement = false;
                isEnabled_CostUnitPriceFCL = false;
            }
            if (this.CostMeasurementCode != "BCNT") {
                isEnabled_CostUnitPriceFCL = false;
            }
            if (this.CostMeasurementCode == "BCNT") {
                isEnabled_CostQuantity = false;
                isEnabled_CostUnitPrice = false;
            }
            if (this.IsChargeBySteps) {
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                isEnabled_CostUnitPriceFCL = false;
            }
            if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
                isEnabled_CostQuantity = false;
                isEnabled_CostUnitPrice = false;
                isEnabled_CostMinAmount = false;
                isEnabled_CostMeasurement = false;
                isEnabled_CostUnitPriceFCL = false;
            }
            if (this.ChargesGroupCode == "FRT") {
                isEnabled_CostMeasurement = false;
            }
            if (this.CostMeasurementCode == "BCNT") {
                isEnabled_CostMinAmount = false;
            }
        }
        this.IsEnabled_CostQuantity = isEnabled_CostQuantity;
        this.IsEnabled_CostUnitPrice = isEnabled_CostUnitPrice;
        this.IsEnabled_CostMinAmount = isEnabled_CostMinAmount;
        this.IsEnabled_CostUnitPriceFCL = isEnabled_CostUnitPriceFCL;
        this.UIProperties.SetEnabled("CostMeasurementId", this.ObjectTableName, isEnabled_CostMeasurement);
        this.UIProperties.SetEnabled("CostMinAmount", this.ObjectTableName, isEnabled_CostMinAmount);
        this.UIProperties.SetEnabled("CostMaxAmount", this.ObjectTableName, isEnabled_CostMinAmount);
        this.SetUIProperties_CostRate();
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_CostRate = function () {
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
    FCLQuoteChargeItem.prototype.SetUIProperties_SaleFields = function () {
        var isEnabled_SaleQuantity = false;
        var isEnabled_SaleUnitPrice = false;
        var isEnabled_SaleMinAmount = false;
        var isEnabled_SaleMeasurement = false;
        var isEnabled_SaleUnitPriceFCL = false;
        if (this.IsEditingEnabled) {
            isEnabled_SaleQuantity = true;
            isEnabled_SaleUnitPrice = true;
            isEnabled_SaleMinAmount = true;
            isEnabled_SaleMeasurement = true;
            isEnabled_SaleUnitPriceFCL = true;
            switch (this.SaleMeasurementCode) {
                //case "GRWT":
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
            if (this.IsAllIN) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleMeasurement = false;
                isEnabled_SaleUnitPriceFCL = false;
            }
            if (this.CostMeasurementCode != "BCNT") {
                isEnabled_SaleUnitPriceFCL = false;
            }
            if (this.SaleMeasurementCode == "BCNT") {
                isEnabled_SaleQuantity = false;
                isEnabled_SaleUnitPrice = false;
            }
            if (this.IsChargeBySteps) {
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleUnitPriceFCL = false;
            }
            if (this.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
                isEnabled_SaleQuantity = false;
                isEnabled_SaleUnitPrice = false;
                isEnabled_SaleMinAmount = false;
                isEnabled_SaleMeasurement = false;
                isEnabled_SaleUnitPriceFCL = false;
            }
            if (this.ChargesGroupCode == "FRT") {
                isEnabled_SaleMeasurement = false;
            }
            if (this.SaleMeasurementCode == "BCNT") {
                isEnabled_SaleMinAmount = false;
            }
        }
        this.IsEnabled_SaleQuantity = isEnabled_SaleQuantity;
        this.IsEnabled_SaleUnitPrice = isEnabled_SaleUnitPrice;
        this.IsEnabled_SaleMinAmount = isEnabled_SaleMinAmount;
        this.IsEnabled_SaleUnitPriceFCL = isEnabled_SaleUnitPriceFCL;
        this.UIProperties.SetEnabled("SaleMeasurementId", this.ObjectTableName, isEnabled_SaleMeasurement);
        this.UIProperties.SetEnabled("SaleMinAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
        this.UIProperties.SetEnabled("SaleMaxAmount", this.ObjectTableName, isEnabled_SaleMinAmount);
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_CellsColors = function () {
        // Price
        var myCostPrice = this.CostUnitPriceInSaleCurrency;
        var mySalePrice = this.SaleUnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            this.SaleUnitPriceColor = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPriceColor = mySalePrice < myCostPrice ? Tools_1.FontTool.Red : (mySalePrice > myCostPrice ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Price1
        var myCostPrice1 = this.CostUnitPrice1InSaleCurrency;
        var mySalePrice1 = this.SaleContainerType1UnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice1) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice1)) {
            this.SaleUnitPrice1Color = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPrice1Color = mySalePrice1 < myCostPrice1 ? Tools_1.FontTool.Red : (mySalePrice1 > myCostPrice1 ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Price2
        var myCostPrice2 = this.CostUnitPrice2InSaleCurrency;
        var mySalePrice2 = this.SaleContainerType2UnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice2) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice2)) {
            this.SaleUnitPrice2Color = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPrice2Color = mySalePrice2 < myCostPrice2 ? Tools_1.FontTool.Red : (mySalePrice2 > myCostPrice2 ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Price3
        var myCostPrice3 = this.CostUnitPrice3InSaleCurrency;
        var mySalePrice3 = this.SaleContainerType3UnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice3) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice3)) {
            this.SaleUnitPrice3Color = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPrice3Color = mySalePrice3 < myCostPrice3 ? Tools_1.FontTool.Red : (mySalePrice3 > myCostPrice3 ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Price4
        var myCostPrice4 = this.CostUnitPrice4InSaleCurrency;
        var mySalePrice4 = this.SaleContainerType4UnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice4) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice4)) {
            this.SaleUnitPrice4Color = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPrice4Color = mySalePrice4 < myCostPrice4 ? Tools_1.FontTool.Red : (mySalePrice4 > myCostPrice4 ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
        // Price5
        var myCostPrice5 = this.CostUnitPrice5InSaleCurrency;
        var mySalePrice5 = this.SaleContainerType5UnitPrice;
        if (Tools_1.AppTool.IsNullOrEmpty(myCostPrice5) || Tools_1.AppTool.IsNullOrEmpty(mySalePrice5)) {
            this.SaleUnitPrice5Color = this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor;
        }
        else {
            this.SaleUnitPrice5Color = mySalePrice5 < myCostPrice5 ? Tools_1.FontTool.Red : (mySalePrice5 > myCostPrice5 ? Tools_1.FontTool.Green : (this.IsEditingEnabled ? Tools_1.FontTool.Black : Tools_1.FontTool.CellDisabledColor));
        }
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_Columns = function () {
        var isCostQuantityColumnVisible = false;
        var isCostPriceColumnVisible = false;
        var isSaleQuantityColumnVisible = false;
        var isSalePriceColumnVisible = false;
        var isCost1Visible = false;
        var isCost2Visible = false;
        var isCost3Visible = false;
        var isCost4Visible = false;
        var isCost5Visible = false;
        var isSale1Visible = false;
        var isSale2Visible = false;
        var isSale3Visible = false;
        var isSale4Visible = false;
        var isSale5Visible = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CostMeasurementCode)) {
            if (this.EntityPM.CostMeasurementCode != "BCNT") {
                isCostPriceColumnVisible = true;
                isSalePriceColumnVisible = true;
                if (this.QuotePM.QuoteTypeCode == "A") {
                    isCostQuantityColumnVisible = true;
                    isSaleQuantityColumnVisible = true;
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost1Visible = true;
                isSale1Visible = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost2Visible = true;
                isSale2Visible = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost3Visible = true;
                isSale3Visible = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost4Visible = true;
                isSale4Visible = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Id) && this.EntityPM.CostMeasurementCode == "BCNT") {
                isCost5Visible = true;
                isSale5Visible = true;
            }
        }
        this.IsCostQuantityColumnVisible = isCostQuantityColumnVisible;
        this.IsCostPriceColumnVisible = isCostPriceColumnVisible;
        this.IsSaleQuantityColumnVisible = isSaleQuantityColumnVisible;
        this.IsSalePriceColumnVisible = isSalePriceColumnVisible;
        this.IsCost1Visible = isCost1Visible;
        this.IsCost2Visible = isCost2Visible;
        this.IsCost3Visible = isCost3Visible;
        this.IsCost4Visible = isCost4Visible;
        this.IsCost5Visible = isCost5Visible;
        this.IsSale1Visible = isSale1Visible;
        this.IsSale2Visible = isSale2Visible;
        this.IsSale3Visible = isSale3Visible;
        this.IsSale4Visible = isSale4Visible;
        this.IsSale5Visible = isSale5Visible;
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_VAT = function () {
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
    FCLQuoteChargeItem.prototype.SetUIProperties_CostMinMax = function () {
        var iTitle = null;
        var iVisible = false;
        if (this.CostMeasurementCode != "BCNT") {
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
        }
        this.CostMinMaxIconTitle = iTitle;
        this.CostMinMaxIconIsVisibile = iVisible;
    };
    FCLQuoteChargeItem.prototype.SetUIProperties_SaleMinMax = function () {
        var iTitle = null;
        var iVisible = false;
        if (this.SaleMeasurementCode != "BCNT") {
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
        }
        this.SaleMinMaxIconTitle = iTitle;
        this.SaleMinMaxIconIsVisibile = iVisible;
    };
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ChargesTypeId", {
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
    FCLQuoteChargeItem.prototype.OnChargesTypeChanged = function (list) {
        if (list) {
            if (this.fatherComponent.IsChargesByVAT) {
                this.VatTypeId = list.VatTypeId;
            }
            this.ChargesTypeCode = list.Code;
            this.ChargesTypeName = list.EnglishName;
            this.ChargesGroupCode = list.ChargesGroupCode;
            this.CostMeasurementId = !Tools_1.AppTool.IsNullOrEmpty(list.ContainerMeasurementId) ? list.ContainerMeasurementId : list.MeasurementId;
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ChargesTypeCode", {
        get: function () { return this.EntityPM.ChargesTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargesTypeCode != newValue) {
                this.EntityPM.ChargesTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ChargesTypeName", {
        get: function () { return this.EntityPM.ChargesTypeName; },
        set: function (newValue) {
            if (this.EntityPM.ChargesTypeName != newValue) {
                this.EntityPM.ChargesTypeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ChargesGroupCode", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ChargesTypeCodeName", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "VatTypeId", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "VatTypeName", {
        get: function () { return this.EntityPM.VatTypeName; },
        set: function (value) {
            if (this.EntityPM.VatTypeName != value) {
                this.EntityPM.VatTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "VatPercentage", {
        get: function () { return this.EntityPM.VatPercentage; },
        set: function (value) {
            if (this.EntityPM.VatPercentage != value) {
                this.EntityPM.VatPercentage = value;
                this.ReadVatTypeData;
                this.SetUIProperties_VAT();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "VatIsMultiPercentage", {
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
    FCLQuoteChargeItem.prototype.ReadVatTypeData = function () {
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
    FCLQuoteChargeItem.prototype.UpdateVatPercentageClicked = function () {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "VendorId", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (newValue) {
            if (this.EntityPM.VendorName != newValue) {
                this.EntityPM.VendorName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostMeasurementId", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostMeasurementCode", {
        get: function () { return this.EntityPM.CostMeasurementCode; },
        set: function (value) {
            if (this.EntityPM.CostMeasurementCode != value) {
                this.EntityPM.CostMeasurementCode = value;
                if (value == "BCNT") {
                    this.EntityPM.CostMinAmount = null;
                    this.EntityPM.CostMaxAmount = null;
                }
                this.SetUIProperties_CostMinMax();
                this.fatherComponent.SetGridColumns();
                this.SetCostQuantity();
                this.SetUIProperties_CostFields();
                this.SetUIProperties_AllIn();
                this.UpdateCostSaleDataVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostMeasurementShortName", {
        get: function () { return this.EntityPM.CostMeasurementShortName; },
        set: function (newValue) {
            if (this.EntityPM.CostMeasurementShortName != newValue) {
                this.EntityPM.CostMeasurementShortName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostCurrencyId", {
        get: function () { return this.EntityPM.CostCurrencyId; },
        set: function (value) {
            if (this.EntityPM.CostCurrencyId != value) {
                this.EntityPM.CostCurrencyId = value;
                this.SetUIProperties_CostRate();
                this.SetUIProperties_AllIn();
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostCurrencyCode", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostQuantity", {
        get: function () { return this.EntityPM.CostQuantity; },
        set: function (value) {
            if (this.EntityPM.CostQuantity != value) {
                this.EntityPM.CostQuantity = Tools_1.AppTool.Round(value, 2);
                this.ComputeCostAmounts();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPrice", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostContainerType1UnitPrice", {
        get: function () { return this.EntityPM.CostContainerType1UnitPrice; },
        set: function (value) {
            if (this.EntityPM.CostContainerType1UnitPrice != value) {
                this.EntityPM.CostContainerType1UnitPrice = Tools_1.AppTool.Round(value, 3);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ContainerType1MarkUpValue = 0;
                    this.ContainerType1MarkUpTypeCode = "F";
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostContainerType2UnitPrice", {
        get: function () { return this.EntityPM.CostContainerType2UnitPrice; },
        set: function (value) {
            if (this.EntityPM.CostContainerType2UnitPrice != value) {
                this.EntityPM.CostContainerType2UnitPrice = Tools_1.AppTool.Round(value, 3);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ContainerType2MarkUpValue = 0;
                    this.ContainerType2MarkUpTypeCode = "F";
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostContainerType3UnitPrice", {
        get: function () { return this.EntityPM.CostContainerType3UnitPrice; },
        set: function (value) {
            if (this.EntityPM.CostContainerType3UnitPrice != value) {
                this.EntityPM.CostContainerType3UnitPrice = Tools_1.AppTool.Round(value, 3);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ContainerType3MarkUpValue = 0;
                    this.ContainerType3MarkUpTypeCode = "F";
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostContainerType4UnitPrice", {
        get: function () { return this.EntityPM.CostContainerType4UnitPrice; },
        set: function (value) {
            if (this.EntityPM.CostContainerType4UnitPrice != value) {
                this.EntityPM.CostContainerType4UnitPrice = Tools_1.AppTool.Round(value, 3);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ContainerType4MarkUpValue = 0;
                    this.ContainerType4MarkUpTypeCode = "F";
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice4();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostContainerType5UnitPrice", {
        get: function () { return this.EntityPM.CostContainerType5UnitPrice; },
        set: function (value) {
            if (this.EntityPM.CostContainerType5UnitPrice != value) {
                this.EntityPM.CostContainerType5UnitPrice = Tools_1.AppTool.Round(value, 3);
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.ContainerType5MarkUpValue = 0;
                    this.ContainerType5MarkUpTypeCode = "F";
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice5();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPriceInSaleCurrency", {
        // InSaleCurrency
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPrice1InSaleCurrency", {
        get: function () { return this.EntityPM.CostUnitPrice1InSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostUnitPrice1InSaleCurrency != value) {
                this.EntityPM.CostUnitPrice1InSaleCurrency = Tools_1.AppTool.Round(value, 3);
                this.ComputeSalePrice1();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPrice2InSaleCurrency", {
        get: function () { return this.EntityPM.CostUnitPrice2InSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostUnitPrice2InSaleCurrency != value) {
                this.EntityPM.CostUnitPrice2InSaleCurrency = Tools_1.AppTool.Round(value, 3);
                this.ComputeSalePrice2();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPrice3InSaleCurrency", {
        get: function () { return this.EntityPM.CostUnitPrice3InSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostUnitPrice3InSaleCurrency != value) {
                this.EntityPM.CostUnitPrice3InSaleCurrency = Tools_1.AppTool.Round(value, 3);
                this.ComputeSalePrice3();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPrice4InSaleCurrency", {
        get: function () { return this.EntityPM.CostUnitPrice4InSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostUnitPrice4InSaleCurrency != value) {
                this.EntityPM.CostUnitPrice4InSaleCurrency = Tools_1.AppTool.Round(value, 3);
                this.ComputeSalePrice4();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostUnitPrice5InSaleCurrency", {
        get: function () { return this.EntityPM.CostUnitPrice5InSaleCurrency; },
        set: function (value) {
            if (this.EntityPM.CostUnitPrice5InSaleCurrency != value) {
                this.EntityPM.CostUnitPrice5InSaleCurrency = Tools_1.AppTool.Round(value, 3);
                this.ComputeSalePrice5();
                this.SetUIProperties_CellsColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostTotalAmount", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostTotalAmountLocal", {
        get: function () { return this.EntityPM.CostTotalAmountLocal; },
        set: function (value) {
            if (this.EntityPM.CostTotalAmountLocal != value) {
                this.EntityPM.CostTotalAmountLocal = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostAmountInSaleCurrency", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostExchangeRate", {
        get: function () { return this.EntityPM.CostExchangeRate; },
        set: function (value) {
            if (this.EntityPM.CostExchangeRate != value) {
                this.EntityPM.CostExchangeRate = Tools_1.AppTool.Round(value, 5);
                if (this.QuotePM.IsSaleCurrencySameAsCost) {
                    this.SaleExchangeRate = value;
                }
                this.ComputeCostAmounts();
                this.ComputeCostInSalePrice();
                this.ComputeCostInSalePrice1();
                this.ComputeCostInSalePrice2();
                this.ComputeCostInSalePrice3();
                this.ComputeCostInSalePrice4();
                this.ComputeCostInSalePrice5();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostMinAmount", {
        get: function () { return this.EntityPM.CostMinAmount; },
        set: function (newValue) {
            if (this.EntityPM.CostMinAmount != newValue) {
                this.EntityPM.CostMinAmount = newValue;
                this.ComputeCostAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostMaxAmount", {
        get: function () { return this.EntityPM.CostMaxAmount; },
        set: function (newValue) {
            if (this.EntityPM.CostMaxAmount != newValue) {
                this.EntityPM.CostMaxAmount = newValue;
                this.ComputeCostAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "CostIsFixedRate", {
        get: function () { return this.EntityPM.CostIsFixedRate; },
        set: function (value) {
            if (this.EntityPM.CostIsFixedRate != value) {
                this.EntityPM.CostIsFixedRate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    FCLQuoteChargeItem.prototype.UpdateCurrencyRateClicked = function () {
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
    FCLQuoteChargeItem.prototype.SetCostQuantity = function () {
        var _this = this;
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
                    myResult = this.QuotePM.NumberOfContainers;
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
                default:
                    {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.CostMeasurementId)) {
                            var list = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.MeasurementId == _this.CostMeasurementId; })[0];
                            if (list != null) {
                                myResult = 0;
                                if (list.Id == this.QuotePM.PackageType1Id) {
                                    myResult = this.QuotePM.PackageType1Quantity == null ? myResult : myResult + this.QuotePM.PackageType1Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType2Id) {
                                    myResult = this.QuotePM.PackageType2Quantity == null ? myResult : myResult + this.QuotePM.PackageType2Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType3Id) {
                                    myResult = this.QuotePM.PackageType3Quantity == null ? myResult : myResult + this.QuotePM.PackageType3Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType4Id) {
                                    myResult = this.QuotePM.PackageType4Quantity == null ? myResult : myResult + this.QuotePM.PackageType4Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType5Id) {
                                    myResult = this.QuotePM.PackageType5Quantity == null ? myResult : myResult + this.QuotePM.PackageType5Quantity;
                                }
                                myResult = myResult == 0 ? null : myResult;
                            }
                        }
                        break;
                    }
            }
        }
        this.CostQuantity = myResult;
        this.SetUIProperties();
    };
    FCLQuoteChargeItem.prototype.ComputeCostAmounts = function () {
        var myTotalAmount = null;
        if (this.CostMeasurementCode == "BCNT") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType1UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Quantity)) {
                var R1 = this.CostContainerType1UnitPrice * this.QuotePM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType2UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Quantity)) {
                var R2 = this.CostContainerType2UnitPrice * this.QuotePM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType3UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Quantity)) {
                var R3 = this.CostContainerType3UnitPrice * this.QuotePM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType4UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Quantity)) {
                var R4 = this.CostContainerType4UnitPrice * this.QuotePM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType5UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Quantity)) {
                var R5 = this.CostContainerType5UnitPrice * this.QuotePM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostQuantity) && !Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice)) {
                if (this.CostMeasurementCode == "PRVL" || this.CostMeasurementCode == "PRFR") {
                    myTotalAmount = this.CostQuantity * this.CostUnitPrice / 100;
                }
                else {
                    myTotalAmount = this.CostQuantity * this.CostUnitPrice;
                }
            }
        }
        /* MinMax */
        if (myTotalAmount != null) {
            if (this.CostMinAmount != null) {
                if (myTotalAmount < this.CostMinAmount) {
                    myTotalAmount = this.CostMinAmount;
                }
            }
            if (this.CostMaxAmount != null) {
                if (myTotalAmount > this.CostMaxAmount) {
                    myTotalAmount = this.CostMaxAmount;
                }
            }
        }
        // Amounts
        if (Tools_1.AppTool.IsNullOrEmpty(myTotalAmount)) {
            this.CostTotalAmount = null;
            this.CostTotalAmountLocal = null;
            this.CostAmountInSaleCurrency = null;
        }
        else {
            this.CostTotalAmount = myTotalAmount;
            if (Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate)) {
                this.CostTotalAmountLocal = null;
            }
            else {
                this.CostTotalAmountLocal = myTotalAmount * this.CostExchangeRate;
            }
            this.ComputeCostInSaleAmount();
        }
        this.SetUIProperties_CostMinMax();
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    };
    FCLQuoteChargeItem.prototype.ComputeCostInSaleAmount = function () {
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
    FCLQuoteChargeItem.prototype.ComputeCostInSalePrice = function () {
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
    FCLQuoteChargeItem.prototype.ComputeCostInSalePrice1 = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType1UnitPrice)) {
                myResult = this.CostContainerType1UnitPrice;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType1UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostContainerType1UnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostUnitPrice1InSaleCurrency = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeCostInSalePrice2 = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType2UnitPrice)) {
                myResult = this.CostContainerType2UnitPrice;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType2UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostContainerType2UnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostUnitPrice2InSaleCurrency = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeCostInSalePrice3 = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType3UnitPrice)) {
                myResult = this.CostContainerType3UnitPrice;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType3UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostContainerType3UnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostUnitPrice3InSaleCurrency = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeCostInSalePrice4 = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType4UnitPrice)) {
                myResult = this.CostContainerType4UnitPrice;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType4UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostContainerType4UnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostUnitPrice4InSaleCurrency = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeCostInSalePrice5 = function () {
        var myResult = null;
        if (this.QuotePM.IsSaleCurrencySameAsCost) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType5UnitPrice)) {
                myResult = this.CostContainerType5UnitPrice;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CostContainerType5UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.CostExchangeRate) && !Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.ExchangeRate)) {
                myResult = this.CostContainerType5UnitPrice * this.CostExchangeRate / this.fatherComponent.ExchangeRate;
            }
        }
        this.CostUnitPrice5InSaleCurrency = myResult;
    };
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleMeasurementId", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleMeasurementCode", {
        get: function () { return this.EntityPM.SaleMeasurementCode; },
        set: function (value) {
            if (this.EntityPM.SaleMeasurementCode != value) {
                this.EntityPM.SaleMeasurementCode = value;
                if (value == "BCNT") {
                    this.EntityPM.SaleMinAmount = null;
                    this.EntityPM.SaleMaxAmount = null;
                }
                this.SetUIProperties_SaleMinMax();
                this.fatherComponent.SetGridColumns();
                this.SetSaleQuantity();
                this.SetUIProperties_SaleFields();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleMeasurementShortName", {
        get: function () { return this.EntityPM.SaleMeasurementShortName; },
        set: function (newValue) {
            if (this.EntityPM.SaleMeasurementShortName != newValue) {
                this.EntityPM.SaleMeasurementShortName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleCurrencyId", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleCurrencyCode", {
        get: function () { return this.EntityPM.SaleCurrencyCode; },
        set: function (newValue) {
            if (this.EntityPM.SaleCurrencyCode != newValue) {
                this.EntityPM.SaleCurrencyCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleQuantity", {
        get: function () { return this.EntityPM.SaleQuantity; },
        set: function (value) {
            if (this.EntityPM.SaleQuantity != value) {
                this.EntityPM.SaleQuantity = Tools_1.AppTool.Round(value, 2);
                this.ComputeSaleAmounts();
                this.fatherComponent.CheckUpdateQuantities();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPrice", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleContainerType1UnitPrice", {
        get: function () { return this.EntityPM.SaleContainerType1UnitPrice; },
        set: function (value) {
            if (this.EntityPM.SaleContainerType1UnitPrice != value) {
                this.EntityPM.SaleContainerType1UnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleContainerType2UnitPrice", {
        get: function () { return this.EntityPM.SaleContainerType2UnitPrice; },
        set: function (value) {
            if (this.EntityPM.SaleContainerType2UnitPrice != value) {
                this.EntityPM.SaleContainerType2UnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleContainerType3UnitPrice", {
        get: function () { return this.EntityPM.SaleContainerType3UnitPrice; },
        set: function (value) {
            if (this.EntityPM.SaleContainerType3UnitPrice != value) {
                this.EntityPM.SaleContainerType3UnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleContainerType4UnitPrice", {
        get: function () { return this.EntityPM.SaleContainerType4UnitPrice; },
        set: function (value) {
            if (this.EntityPM.SaleContainerType4UnitPrice != value) {
                this.EntityPM.SaleContainerType4UnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleContainerType5UnitPrice", {
        get: function () { return this.EntityPM.SaleContainerType5UnitPrice; },
        set: function (value) {
            if (this.EntityPM.SaleContainerType5UnitPrice != value) {
                this.EntityPM.SaleContainerType5UnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputeSaleAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleTotalAmount", {
        get: function () { return this.EntityPM.SaleTotalAmount; },
        set: function (value) {
            if (this.EntityPM.SaleTotalAmount != value) {
                this.EntityPM.SaleTotalAmount = Tools_1.AppTool.Round(value, 2);
                if (this.ChargesGroupCode == "FRT") {
                    this.fatherComponent.OnFreightAmountChanged();
                }
                this.SetUIProperties_SaleMinMax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleTotalAmountLocal", {
        get: function () { return this.EntityPM.SaleTotalAmountLocal; },
        set: function (value) {
            if (this.EntityPM.SaleTotalAmountLocal != value) {
                this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.Round(value, 2);
                this.SetUIProperties_SaleMinMax();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleExchangeRate", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleMinAmount", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleMaxAmount", {
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
    FCLQuoteChargeItem.prototype.SetSaleQuantity = function () {
        var _this = this;
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
                    myResult = this.QuotePM.NumberOfContainers;
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
                default:
                    {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleMeasurementId)) {
                            var list = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.MeasurementId == _this.SaleMeasurementId; })[0];
                            if (list != null) {
                                myResult = 0;
                                if (list.Id == this.QuotePM.PackageType1Id) {
                                    myResult = this.QuotePM.PackageType1Quantity == null ? myResult : myResult + this.QuotePM.PackageType1Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType2Id) {
                                    myResult = this.QuotePM.PackageType2Quantity == null ? myResult : myResult + this.QuotePM.PackageType2Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType3Id) {
                                    myResult = this.QuotePM.PackageType3Quantity == null ? myResult : myResult + this.QuotePM.PackageType3Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType4Id) {
                                    myResult = this.QuotePM.PackageType4Quantity == null ? myResult : myResult + this.QuotePM.PackageType4Quantity;
                                }
                                if (list.Id == this.QuotePM.PackageType5Id) {
                                    myResult = this.QuotePM.PackageType5Quantity == null ? myResult : myResult + this.QuotePM.PackageType5Quantity;
                                }
                                myResult = myResult == 0 ? null : myResult;
                            }
                        }
                        break;
                    }
            }
        }
        this.SaleQuantity = myResult;
        this.SetUIProperties();
    };
    FCLQuoteChargeItem.prototype.ComputeSaleAmounts = function () {
        var myTotalAmount = null;
        if (this.SaleMeasurementCode == "BCNT") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType1UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Quantity)) {
                var R1 = this.SaleContainerType1UnitPrice * this.QuotePM.PackageType1Quantity;
                myTotalAmount = myTotalAmount == null ? R1 : myTotalAmount + R1;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType2UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Quantity)) {
                var R2 = this.SaleContainerType2UnitPrice * this.QuotePM.PackageType2Quantity;
                myTotalAmount = myTotalAmount == null ? R2 : myTotalAmount + R2;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType3UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Quantity)) {
                var R3 = this.SaleContainerType3UnitPrice * this.QuotePM.PackageType3Quantity;
                myTotalAmount = myTotalAmount == null ? R3 : myTotalAmount + R3;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType4UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Quantity)) {
                var R4 = this.SaleContainerType4UnitPrice * this.QuotePM.PackageType4Quantity;
                myTotalAmount = myTotalAmount == null ? R4 : myTotalAmount + R4;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType5UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Quantity)) {
                var R5 = this.SaleContainerType5UnitPrice * this.QuotePM.PackageType5Quantity;
                myTotalAmount = myTotalAmount == null ? R5 : myTotalAmount + R5;
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleQuantity) && !Tools_1.AppTool.IsNullOrEmpty(this.SaleUnitPrice)) {
                if (this.SaleMeasurementCode == "PRVL" || this.SaleMeasurementCode == "PRFR") {
                    myTotalAmount = this.SaleQuantity * this.SaleUnitPrice / 100;
                }
                else {
                    myTotalAmount = this.SaleQuantity * this.SaleUnitPrice;
                }
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
        this.SetUIProperties_CellsColors();
        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }
        this.SetUIProperties_SaleMinMax();
        this.fatherComponent.ComputeTotals();
        this.fatherComponent.CheckUpdateQuantities();
    };
    FCLQuoteChargeItem.prototype.ComputeSalePrice = function () {
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
    FCLQuoteChargeItem.prototype.ComputeSalePrice1 = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice1InSaleCurrency)) {
            this.SaleContainerType1UnitPrice = null;
        }
        else {
            var myResult = this.SaleContainerType1UnitPrice;
            var markup = this.ContainerType1MarkUpValue == null ? 0 : this.ContainerType1MarkUpValue;
            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice1InSaleCurrency + (this.CostUnitPrice1InSaleCurrency * (markup / 100));
            }
            else {
                myResult = this.CostUnitPrice1InSaleCurrency + markup;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }
            this.SaleContainerType1UnitPrice = myResult;
        }
    };
    FCLQuoteChargeItem.prototype.ComputeSalePrice2 = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice2InSaleCurrency)) {
            this.SaleContainerType2UnitPrice = null;
        }
        else {
            var myResult = this.SaleContainerType2UnitPrice;
            var markup = this.ContainerType2MarkUpValue == null ? 0 : this.ContainerType2MarkUpValue;
            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice2InSaleCurrency + (this.CostUnitPrice2InSaleCurrency * (markup / 100));
            }
            else {
                myResult = this.CostUnitPrice2InSaleCurrency + markup;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }
            this.SaleContainerType2UnitPrice = myResult;
        }
    };
    FCLQuoteChargeItem.prototype.ComputeSalePrice3 = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice3InSaleCurrency)) {
            this.SaleContainerType3UnitPrice = null;
        }
        else {
            var myResult = this.SaleContainerType3UnitPrice;
            var markup = this.ContainerType3MarkUpValue == null ? 0 : this.ContainerType3MarkUpValue;
            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice3InSaleCurrency + (this.CostUnitPrice3InSaleCurrency * (markup / 100));
            }
            else {
                myResult = this.CostUnitPrice3InSaleCurrency + markup;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }
            this.SaleContainerType3UnitPrice = myResult;
        }
    };
    FCLQuoteChargeItem.prototype.ComputeSalePrice4 = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice4InSaleCurrency)) {
            this.SaleContainerType4UnitPrice = null;
        }
        else {
            var myResult = this.SaleContainerType4UnitPrice;
            var markup = this.ContainerType4MarkUpValue == null ? 0 : this.ContainerType4MarkUpValue;
            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice4InSaleCurrency + (this.CostUnitPrice4InSaleCurrency * (markup / 100));
            }
            else {
                myResult = this.CostUnitPrice4InSaleCurrency + markup;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }
            this.SaleContainerType4UnitPrice = myResult;
        }
    };
    FCLQuoteChargeItem.prototype.ComputeSalePrice5 = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice5InSaleCurrency)) {
            this.SaleContainerType5UnitPrice = null;
        }
        else {
            var myResult = this.SaleContainerType5UnitPrice;
            var markup = this.ContainerType5MarkUpValue == null ? 0 : this.ContainerType5MarkUpValue;
            if (this.MarkUpTypeCode == "P") {
                myResult = this.CostUnitPrice5InSaleCurrency + (this.CostUnitPrice5InSaleCurrency * (markup / 100));
            }
            else {
                myResult = this.CostUnitPrice5InSaleCurrency + markup;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = null;
            }
            this.SaleContainerType5UnitPrice = myResult;
        }
    };
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPriceString", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPrice1String", {
        get: function () {
            var myResult = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType1UnitPrice)) {
                myResult = this.SaleContainerType1UnitPrice + "";
            }
            this.mySaleUnitPrice1String = myResult;
            return this.mySaleUnitPrice1String;
        },
        set: function (value) {
            if (this.mySaleUnitPrice1String != value) {
                var mySalePrice = null;
                var myMarkUpValue = 0;
                var myMarkUpCode = "F";
                var myCostPrice = this.CostUnitPrice1InSaleCurrency;
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
                this.SaleContainerType1UnitPrice = mySalePrice;
                this.ContainerType1MarkUpTypeCode = myMarkUpCode;
                this.ContainerType1MarkUpValue = Tools_1.AppTool.Round(myMarkUpValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPrice2String", {
        get: function () {
            var myResult = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType2UnitPrice)) {
                myResult = this.SaleContainerType2UnitPrice + "";
            }
            this.mySaleUnitPrice2String = myResult;
            return this.mySaleUnitPrice2String;
        },
        set: function (value) {
            if (this.mySaleUnitPrice2String != value) {
                var mySalePrice = null;
                var myMarkUpValue = 0;
                var myMarkUpCode = "F";
                var myCostPrice = this.CostUnitPrice2InSaleCurrency;
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
                this.SaleContainerType2UnitPrice = mySalePrice;
                this.ContainerType2MarkUpTypeCode = myMarkUpCode;
                this.ContainerType2MarkUpValue = Tools_1.AppTool.Round(myMarkUpValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPrice3String", {
        get: function () {
            var myResult = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType3UnitPrice)) {
                myResult = this.SaleContainerType3UnitPrice + "";
            }
            this.mySaleUnitPrice3String = myResult;
            return this.mySaleUnitPrice3String;
        },
        set: function (value) {
            if (this.mySaleUnitPrice3String != value) {
                var mySalePrice = null;
                var myMarkUpValue = 0;
                var myMarkUpCode = "F";
                var myCostPrice = this.CostUnitPrice3InSaleCurrency;
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
                this.SaleContainerType3UnitPrice = mySalePrice;
                this.ContainerType3MarkUpTypeCode = myMarkUpCode;
                this.ContainerType3MarkUpValue = Tools_1.AppTool.Round(myMarkUpValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPrice4String", {
        get: function () {
            var myResult = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType4UnitPrice)) {
                myResult = this.SaleContainerType4UnitPrice + "";
            }
            this.mySaleUnitPrice4String = myResult;
            return this.mySaleUnitPrice4String;
        },
        set: function (value) {
            if (this.mySaleUnitPrice4String != value) {
                var mySalePrice = null;
                var myMarkUpValue = 0;
                var myMarkUpCode = "F";
                var myCostPrice = this.CostUnitPrice4InSaleCurrency;
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
                this.SaleContainerType4UnitPrice = mySalePrice;
                this.ContainerType4MarkUpTypeCode = myMarkUpCode;
                this.ContainerType4MarkUpValue = Tools_1.AppTool.Round(myMarkUpValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "SaleUnitPrice5String", {
        get: function () {
            var myResult = null;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType5UnitPrice)) {
                myResult = this.SaleContainerType5UnitPrice + "";
            }
            this.mySaleUnitPrice5String = myResult;
            return this.mySaleUnitPrice5String;
        },
        set: function (value) {
            if (this.mySaleUnitPrice5String != value) {
                var mySalePrice = null;
                var myMarkUpValue = 0;
                var myMarkUpCode = "F";
                var myCostPrice = this.CostUnitPrice5InSaleCurrency;
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
                this.SaleContainerType5UnitPrice = mySalePrice;
                this.ContainerType5MarkUpTypeCode = myMarkUpCode;
                this.ContainerType5MarkUpValue = Tools_1.AppTool.Round(myMarkUpValue, 3);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "MarkUpValue", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType1MarkUpValue", {
        get: function () { return this.EntityPM.ContainerType1MarkUpValue; },
        set: function (value) {
            if (this.EntityPM.ContainerType1MarkUpValue != value) {
                this.EntityPM.ContainerType1MarkUpValue = Tools_1.AppTool.Round(value, 3);
                this.ComputeMarkUp1String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType2MarkUpValue", {
        get: function () { return this.EntityPM.ContainerType2MarkUpValue; },
        set: function (value) {
            if (this.EntityPM.ContainerType2MarkUpValue != value) {
                this.EntityPM.ContainerType2MarkUpValue = Tools_1.AppTool.Round(value, 3);
                this.ComputeMarkUp2String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType3MarkUpValue", {
        get: function () { return this.EntityPM.ContainerType3MarkUpValue; },
        set: function (value) {
            if (this.EntityPM.ContainerType3MarkUpValue != value) {
                this.EntityPM.ContainerType3MarkUpValue = Tools_1.AppTool.Round(value, 3);
                this.ComputeMarkUp3String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType4MarkUpValue", {
        get: function () { return this.EntityPM.ContainerType4MarkUpValue; },
        set: function (value) {
            if (this.EntityPM.ContainerType4MarkUpValue != value) {
                this.EntityPM.ContainerType4MarkUpValue = Tools_1.AppTool.Round(value, 3);
                this.ComputeMarkUp4String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType5MarkUpValue", {
        get: function () { return this.EntityPM.ContainerType5MarkUpValue; },
        set: function (value) {
            if (this.EntityPM.ContainerType5MarkUpValue != value) {
                this.EntityPM.ContainerType5MarkUpValue = Tools_1.AppTool.Round(value, 3);
                this.ComputeMarkUp5String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "MarkUpTypeCode", {
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
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType1MarkUpTypeCode", {
        get: function () { return this.EntityPM.ContainerType1MarkUpTypeCode; },
        set: function (value) {
            if (this.EntityPM.ContainerType1MarkUpTypeCode != value) {
                this.EntityPM.ContainerType1MarkUpTypeCode = value;
                this.ComputeMarkUp1String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType2MarkUpTypeCode", {
        get: function () { return this.EntityPM.ContainerType2MarkUpTypeCode; },
        set: function (value) {
            if (this.EntityPM.ContainerType2MarkUpTypeCode != value) {
                this.EntityPM.ContainerType2MarkUpTypeCode = value;
                this.ComputeMarkUp2String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType3MarkUpTypeCode", {
        get: function () { return this.EntityPM.ContainerType3MarkUpTypeCode; },
        set: function (value) {
            if (this.EntityPM.ContainerType3MarkUpTypeCode != value) {
                this.EntityPM.ContainerType3MarkUpTypeCode = value;
                this.ComputeMarkUp3String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType4MarkUpTypeCode", {
        get: function () { return this.EntityPM.ContainerType4MarkUpTypeCode; },
        set: function (value) {
            if (this.EntityPM.ContainerType4MarkUpTypeCode != value) {
                this.EntityPM.ContainerType4MarkUpTypeCode = value;
                this.ComputeMarkUp4String();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "ContainerType5MarkUpTypeCode", {
        get: function () { return this.EntityPM.ContainerType5MarkUpTypeCode; },
        set: function (value) {
            if (this.EntityPM.ContainerType5MarkUpTypeCode != value) {
                this.EntityPM.ContainerType5MarkUpTypeCode = value;
                this.ComputeMarkUp5String();
            }
        },
        enumerable: true,
        configurable: true
    });
    FCLQuoteChargeItem.prototype.ComputeMarkUp = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.EntityPM.MarkUpValue;
        var myCostPrice = this.CostUnitPriceInSaleCurrency;
        var mySalePrice = this.SaleUnitPrice;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice) && !Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((mySalePrice - myCostPrice) * 100) / myCostPrice;
            }
            else {
                myResult = mySalePrice - myCostPrice;
            }
        }
        this.MarkUpValue = myResult == null ? 0 : myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp1 = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.ContainerType1MarkUpValue;
        var myCostPrice = this.CostUnitPrice1InSaleCurrency;
        var mySalePrice = this.SaleContainerType1UnitPrice;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice) && !Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((mySalePrice - myCostPrice) * 100) / myCostPrice;
            }
            else {
                myResult = mySalePrice - myCostPrice;
            }
        }
        this.ContainerType1MarkUpValue = myResult == null ? 0 : myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp2 = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.ContainerType2MarkUpValue;
        var myCostPrice = this.CostUnitPrice2InSaleCurrency;
        var mySalePrice = this.SaleContainerType2UnitPrice;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice) && !Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((mySalePrice - myCostPrice) * 100) / myCostPrice;
            }
            else {
                myResult = mySalePrice - myCostPrice;
            }
        }
        this.ContainerType2MarkUpValue = myResult == null ? 0 : myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp3 = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.ContainerType3MarkUpValue;
        var myCostPrice = this.CostUnitPrice3InSaleCurrency;
        var mySalePrice = this.SaleContainerType3UnitPrice;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice) && !Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((mySalePrice - myCostPrice) * 100) / myCostPrice;
            }
            else {
                myResult = mySalePrice - myCostPrice;
            }
        }
        this.ContainerType3MarkUpValue = myResult == null ? 0 : myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp4 = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.ContainerType4MarkUpValue;
        var myCostPrice = this.CostUnitPrice4InSaleCurrency;
        var mySalePrice = this.SaleContainerType4UnitPrice;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice) && !Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((mySalePrice - myCostPrice) * 100) / myCostPrice;
            }
            else {
                myResult = mySalePrice - myCostPrice;
            }
        }
        this.ContainerType4MarkUpValue = myResult == null ? 0 : myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp5 = function () {
        if (this.EntityPM.ChargesGroupCode == "FRT" && this.QuotePM.QuoteCharges.filter(function (d) { return d.IsAllIN; }).length > 0) {
            return;
        }
        var myResult = this.ContainerType5MarkUpValue;
        var myCostPrice = this.CostUnitPrice5InSaleCurrency;
        var mySalePrice = this.SaleContainerType5UnitPrice;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCostPrice) && !Tools_1.AppTool.IsNullOrEmpty(mySalePrice)) {
            if (this.EntityPM.MarkUpTypeCode == "P") {
                myResult = ((mySalePrice - myCostPrice) * 100) / myCostPrice;
            }
            else {
                myResult = mySalePrice - myCostPrice;
            }
        }
        this.ContainerType5MarkUpValue = myResult == null ? 0 : myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUpString = function () {
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
    FCLQuoteChargeItem.prototype.ComputeMarkUp1String = function () {
        var myResult = null;
        var markUpValue = this.ContainerType1MarkUpValue;
        var markUpCode = this.ContainerType1MarkUpTypeCode;
        if (!Tools_1.AppTool.IsNullOrZero(markUpValue)) {
            var myCostPrice = Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice1InSaleCurrency) ? 0 : this.CostUnitPrice1InSaleCurrency;
            var mySalePrice = Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType1UnitPrice) ? 0 : this.SaleContainerType1UnitPrice;
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
        this.CellMarkup1Text = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp2String = function () {
        var myResult = null;
        var markUpValue = this.ContainerType2MarkUpValue;
        var markUpCode = this.ContainerType2MarkUpTypeCode;
        if (!Tools_1.AppTool.IsNullOrZero(markUpValue)) {
            var myCostPrice = Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice2InSaleCurrency) ? 0 : this.CostUnitPrice2InSaleCurrency;
            var mySalePrice = Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType2UnitPrice) ? 0 : this.SaleContainerType2UnitPrice;
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
        this.CellMarkup2Text = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp3String = function () {
        var myResult = null;
        var markUpValue = this.ContainerType3MarkUpValue;
        var markUpCode = this.ContainerType3MarkUpTypeCode;
        if (!Tools_1.AppTool.IsNullOrZero(markUpValue)) {
            var myCostPrice = Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice3InSaleCurrency) ? 0 : this.CostUnitPrice3InSaleCurrency;
            var mySalePrice = Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType3UnitPrice) ? 0 : this.SaleContainerType3UnitPrice;
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
        this.CellMarkup3Text = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp4String = function () {
        var myResult = null;
        var markUpValue = this.ContainerType4MarkUpValue;
        var markUpCode = this.ContainerType4MarkUpTypeCode;
        if (!Tools_1.AppTool.IsNullOrZero(markUpValue)) {
            var myCostPrice = Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice4InSaleCurrency) ? 0 : this.CostUnitPrice4InSaleCurrency;
            var mySalePrice = Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType4UnitPrice) ? 0 : this.SaleContainerType4UnitPrice;
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
        this.CellMarkup4Text = myResult;
    };
    FCLQuoteChargeItem.prototype.ComputeMarkUp5String = function () {
        var myResult = null;
        var markUpValue = this.ContainerType5MarkUpValue;
        var markUpCode = this.ContainerType5MarkUpTypeCode;
        if (!Tools_1.AppTool.IsNullOrZero(markUpValue)) {
            var myCostPrice = Tools_1.AppTool.IsNullOrEmpty(this.CostUnitPrice5InSaleCurrency) ? 0 : this.CostUnitPrice5InSaleCurrency;
            var mySalePrice = Tools_1.AppTool.IsNullOrEmpty(this.SaleContainerType5UnitPrice) ? 0 : this.SaleContainerType5UnitPrice;
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
        this.CellMarkup5Text = myResult;
    };
    Object.defineProperty(FCLQuoteChargeItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "IsChargeBySteps", {
        get: function () { return this.EntityPM.IsChargeBySteps; },
        set: function (newValue) {
            if (this.EntityPM.IsChargeBySteps != newValue) {
                this.EntityPM.IsChargeBySteps = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FCLQuoteChargeItem.prototype, "IsAllIN", {
        get: function () { return this.EntityPM.IsAllIN; },
        set: function (newValue) {
            if (this.EntityPM.IsAllIN != newValue) {
                this.EntityPM.IsAllIN = newValue;
                this.SetUIProperties_AllIn();
                this.UpdateCostSaleDataVisibility();
                this.UpdateAllInFreight();
            }
        },
        enumerable: true,
        configurable: true
    });
    FCLQuoteChargeItem.prototype.UpdateAllInFreight = function () {
        var allFreightModel = this.fatherComponent.ItemsSource.Collection.filter(function (d) { return d.EntityPM.ChargesGroupCode == "FRT"; });
        allFreightModel.forEach(function (item) {
            item.SetUIProperties();
        });
        var freightModel = allFreightModel[0];
        if (freightModel != null) {
            if (this.EntityPM.IsAllIN) {
                if (this.EntityPM.CostMeasurementCode == "BCNT") {
                    // SalePrice1
                    if (this.QuotePM.PackageType1Id != null) {
                        var freightPrice = freightModel.SaleContainerType1UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType1UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }
                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }
                        freightModel.SaleContainerType1UnitPrice = freightPrice;
                    }
                    //SalePrice2
                    if (this.QuotePM.PackageType2Id != null) {
                        var freightPrice = freightModel.SaleContainerType2UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType2UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }
                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }
                        freightModel.SaleContainerType2UnitPrice = freightPrice;
                    }
                    //SalePrice3
                    if (this.QuotePM.PackageType3Id != null) {
                        var freightPrice = freightModel.SaleContainerType3UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType3UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }
                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }
                        freightModel.SaleContainerType3UnitPrice = freightPrice;
                    }
                    // SalePrice4
                    if (this.QuotePM.PackageType4Id != null) {
                        var freightPrice = freightModel.SaleContainerType4UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType4UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }
                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }
                        freightModel.SaleContainerType4UnitPrice = freightPrice;
                    }
                    // SalePrice5
                    if (this.QuotePM.PackageType5Id != null) {
                        var freightPrice = freightModel.SaleContainerType5UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType5UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice + myAllInPrice;
                        }
                        else {
                            freightPrice = freightPrice == null ? myAllInPrice : freightPrice;
                        }
                        freightModel.SaleContainerType5UnitPrice = freightPrice;
                    }
                }
                else {
                }
            }
            else {
                if (this.EntityPM.CostMeasurementCode == "BCNT") {
                    // SalePrice1
                    if (this.QuotePM.PackageType1Id != null) {
                        var freightPrice = freightModel.SaleContainerType1UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType1UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }
                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }
                        freightModel.SaleContainerType1UnitPrice = freightPrice;
                    }
                    // SalePrice2
                    if (this.QuotePM.PackageType2Id != null) {
                        var freightPrice = freightModel.SaleContainerType2UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType2UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }
                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }
                        freightModel.SaleContainerType2UnitPrice = freightPrice;
                    }
                    // SalePrice3
                    if (this.QuotePM.PackageType3Id != null) {
                        var freightPrice = freightModel.SaleContainerType3UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType3UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }
                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }
                        freightModel.SaleContainerType3UnitPrice = freightPrice;
                    }
                    // SalePrice4
                    if (this.QuotePM.PackageType4Id != null) {
                        var freightPrice = freightModel.SaleContainerType4UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType4UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }
                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }
                        freightModel.SaleContainerType4UnitPrice = freightPrice;
                    }
                    // SalePrice5
                    if (this.QuotePM.PackageType5Id != null) {
                        var freightPrice = freightModel.SaleContainerType5UnitPrice;
                        var myAllInPrice = this.EntityPM.SaleContainerType5UnitPrice;
                        if (freightPrice != null && myAllInPrice != null) {
                            freightPrice = freightPrice - myAllInPrice;
                        }
                        else {
                            freightPrice = myAllInPrice == null ? freightPrice : null;
                        }
                        freightModel.SaleContainerType5UnitPrice = freightPrice;
                    }
                }
                else {
                }
            }
        }
        this.fatherComponent.ComputeTotals();
    };
    FCLQuoteChargeItem.prototype.UpdateCostSaleDataVisibility = function () {
        if (this.EntityPM.CostMeasurementCode == "BCNT") {
            this.EntityPM.CostUnitPrice = null;
            this.EntityPM.SaleUnitPrice = null;
        }
        else {
            this.EntityPM.CostContainerType1UnitPrice = null;
            this.EntityPM.CostContainerType2UnitPrice = null;
            this.EntityPM.CostContainerType3UnitPrice = null;
            this.EntityPM.CostContainerType4UnitPrice = null;
            this.EntityPM.CostContainerType5UnitPrice = null;
            this.EntityPM.SaleContainerType1UnitPrice = null;
            this.EntityPM.SaleContainerType2UnitPrice = null;
            this.EntityPM.SaleContainerType3UnitPrice = null;
            this.EntityPM.SaleContainerType4UnitPrice = null;
            this.EntityPM.SaleContainerType5UnitPrice = null;
        }
        this.ComputeCostAmounts();
        this.ComputeSaleAmounts();
        this.SetUIProperties();
        this.fatherComponent.ComputeTotals();
    };
    FCLQuoteChargeItem.prototype.OnQuoteSaleCurrencyChanged = function () {
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
        this.ComputeCostInSalePrice1();
        this.ComputeCostInSalePrice2();
        this.ComputeCostInSalePrice3();
        this.ComputeCostInSalePrice4();
        this.ComputeCostInSalePrice5();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : Tools_1.AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.SetUIProperties_AllIn();
    };
    FCLQuoteChargeItem.prototype.OnQuoteSaleCurrencySameAsCost = function () {
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
        this.ComputeCostInSalePrice1();
        this.ComputeCostInSalePrice2();
        this.ComputeCostInSalePrice3();
        this.ComputeCostInSalePrice4();
        this.ComputeCostInSalePrice5();
        this.ComputeCostInSaleAmount();
        this.EntityPM.SaleTotalAmountLocal = Tools_1.AppTool.IsNullOrEmpty(this.SaleTotalAmount) ? null : Tools_1.AppTool.Round(this.SaleTotalAmount * this.SaleExchangeRate, 2);
        this.SetUIProperties_AllIn();
    };
    FCLQuoteChargeItem.prototype.OnMeasurementsChanged = function () {
        if (this.CostMeasurementId != this.SaleMeasurementId) {
            this.EntityPM.MarkUpValue = 0;
            this.EntityPM.ContainerType1MarkUpValue = 0;
            this.EntityPM.ContainerType2MarkUpValue = 0;
            this.EntityPM.ContainerType3MarkUpValue = 0;
            this.EntityPM.ContainerType4MarkUpValue = 0;
            this.EntityPM.ContainerType5MarkUpValue = 0;
            this.mySaleUnitPriceString = this.SaleUnitPrice ? this.SaleUnitPrice + "" : null;
            this.mySaleUnitPrice1String = this.SaleContainerType1UnitPrice ? this.SaleContainerType1UnitPrice + "" : null;
            this.mySaleUnitPrice2String = this.SaleContainerType2UnitPrice ? this.SaleContainerType2UnitPrice + "" : null;
            this.mySaleUnitPrice3String = this.SaleContainerType3UnitPrice ? this.SaleContainerType3UnitPrice + "" : null;
            this.mySaleUnitPrice4String = this.SaleContainerType4UnitPrice ? this.SaleContainerType4UnitPrice + "" : null;
            this.mySaleUnitPrice5String = this.SaleContainerType5UnitPrice ? this.SaleContainerType5UnitPrice + "" : null;
        }
        else {
            this.ComputeMarkUp();
            this.ComputeMarkUp1();
            this.ComputeMarkUp2();
            this.ComputeMarkUp3();
            this.ComputeMarkUp4();
            this.ComputeMarkUp5();
        }
    };
    FCLQuoteChargeItem.prototype.SetEditScreenGridHeaders = function () {
        var _this = this;
        var mySaleCurrencyCode = Tools_1.AppTool.IsNullOrEmpty(this.SaleCurrencyCode) ? "" : this.SaleCurrencyCode;
        this.Sale1Header = [3];
        this.Sale2Header = [3];
        this.Sale3Header = [3];
        this.Sale4Header = [3];
        this.Sale5Header = [3];
        var q1 = Tools_1.AppTool.IsNullOrZero(this.QuotePM.PackageType1Quantity) ? "" : this.QuotePM.PackageType1Quantity.toString() + "X";
        var q2 = Tools_1.AppTool.IsNullOrZero(this.QuotePM.PackageType2Quantity) ? "" : this.QuotePM.PackageType2Quantity.toString() + "X";
        var q3 = Tools_1.AppTool.IsNullOrZero(this.QuotePM.PackageType3Quantity) ? "" : this.QuotePM.PackageType3Quantity.toString() + "X";
        var q4 = Tools_1.AppTool.IsNullOrZero(this.QuotePM.PackageType4Quantity) ? "" : this.QuotePM.PackageType4Quantity.toString() + "X";
        var q5 = Tools_1.AppTool.IsNullOrZero(this.QuotePM.PackageType5Quantity) ? "" : this.QuotePM.PackageType5Quantity.toString() + "X";
        var p1 = "";
        var p2 = "";
        var p3 = "";
        var p4 = "";
        var p5 = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType1Id)) {
            var item = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.Id == _this.QuotePM.PackageType1Id; })[0];
            if (item) {
                p1 = item.Code;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType2Id)) {
            var item = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.Id == _this.QuotePM.PackageType2Id; })[0];
            if (item) {
                p2 = item.Code;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType3Id)) {
            var item = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.Id == _this.QuotePM.PackageType3Id; })[0];
            if (item) {
                p3 = item.Code;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType4Id)) {
            var item = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.Id == _this.QuotePM.PackageType4Id; })[0];
            if (item) {
                p4 = item.Code;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuotePM.PackageType5Id)) {
            var item = this.fatherComponent.AllPackageTypes.filter(function (d) { return d.Id == _this.QuotePM.PackageType5Id; })[0];
            if (item) {
                p5 = item.Code;
            }
        }
        if (this.fatherComponent.IsSaleCurrencySameAsCost) {
            var myCurrencyCode = Tools_1.AppTool.IsNullOrEmpty(this.CostCurrencyCode) ? "" : this.CostCurrencyCode;
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount").replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }
        else {
            var myCurrencyCode = Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.SaleCurrencyCode) ? "" : this.fatherComponent.SaleCurrencyCode;
            this.SalePriceHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SalePrice").replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
            this.SaleAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.O.Charges.SaleAmount").replace("%SaleCurrencyCode", myCurrencyCode).split('%n');
        }
        this.Sale1Header[0] = this.SalePriceHeader[0];
        this.Sale2Header[0] = this.SalePriceHeader[0];
        this.Sale3Header[0] = this.SalePriceHeader[0];
        this.Sale4Header[0] = this.SalePriceHeader[0];
        this.Sale5Header[0] = this.SalePriceHeader[0];
        this.Sale1Header[1] = this.SalePriceHeader[1];
        this.Sale2Header[1] = this.SalePriceHeader[1];
        this.Sale3Header[1] = this.SalePriceHeader[1];
        this.Sale4Header[1] = this.SalePriceHeader[1];
        this.Sale5Header[1] = this.SalePriceHeader[1];
        this.Sale1Header[2] = q1 + p1;
        this.Sale2Header[2] = q2 + p2;
        this.Sale3Header[2] = q3 + p3;
        this.Sale4Header[2] = q4 + p4;
        this.Sale5Header[2] = q5 + p5;
    };
    return FCLQuoteChargeItem;
}(BaseComponent_1.BaseComponent));
exports.FCLQuoteChargeItem = FCLQuoteChargeItem;
//# sourceMappingURL=FCLChargesComponent.js.map