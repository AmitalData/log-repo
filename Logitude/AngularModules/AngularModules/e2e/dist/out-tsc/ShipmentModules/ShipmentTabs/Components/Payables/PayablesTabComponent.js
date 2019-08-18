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
var ShipmentPayablePM_1 = require("../../../../Shipment/EntityPMs/ShipmentPayablePM");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DecimalFormatter_1 = require("../../../../Infrastructure/Utilities/DecimalFormatter");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Shipment/Tools");
var Tools_2 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var MeasurementListService_1 = require("../../../../Common/Services/StandardLists/MeasurementListService");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var PackageTypeListService_1 = require("../../../../Common/Services/StandardLists/PackageTypeListService");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var QuotePMService_1 = require("../../../../Quote/Services/StandardPMs/QuotePMService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var PayablesTabComponent = /** @class */ (function () {
    function PayablesTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.ObjectTableName = null;
        this.DataContext = this;
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.ShipmentLevelCode = null;
        this.ProfitCurrencyId = null;
        this.ProfitCurrencyCode = null;
        this.LocalCurrencyId = null;
        this.LocalCurrencyCode = null;
        this.IsResourcesReady = false;
        this.myUserListService = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        // Load RequiredData
        this.BaseQuote = null;
        this.AllRates = [];
        this.SelectedRow = null;
        // SetUIProperties
        this.IsEditingEnabled = true;
        // Summary
        this.AccrualsPayables = 0;
        this.AccountedPayables = 0;
        this.OpenPayables = 0;
        this.DifferencePayablesText = "N/A";
        this.PayableList = [];
        // Profit
        this.IsProfitAreaVisible = false;
        this.IsCurrencyFilterVisible = false;
        this.IsProfitRateVisible = false;
        this.IsByLocalCurrency = false;
        this.SelectedCurrencyCode = null;
        this.ProfitRate = "N/A";
        this.ProfitInSelectedCurrencyText = "N/A";
        this.PayablesInSelectedCurrencyText = "N/A";
        this.ReceivablesInSelectedCurrencyText = "N/A";
        this.DefferenceInSelectedCurrencyText = "";
        this.estimateProfitInSelectedCurrency = null;
        // Generate
        this.IsGenerateButtonsVisible = false;
        this.IsGenerateFromQuoteChargesEnabled = false;
        this.IsNoChargesTextVisibil = false;
        this.SavingRequested = false;
        this.SavingRequestCode = null;
        this.SavingRequestParam = null;
        this.UpdateQuantitiesMessageWidth = 0;
        this.IsUpdateQuantitiesVisible = false;
        this.EntityPM = entityArgs.EntityPM;
        this.OriginShipment = entityArgs.OriginEntity;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        this.ProfitCurrencyId = this.EntityPM.ProfitCurrencyId;
        this.ProfitCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        this.myUserListService = new UserListService_1.UserListService();
        this.Listen();
        this.SetEditEnabled();
        this.LoadRequiredData();
    }
    PayablesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "PayablesGenerated") {
                    _this.BuildItemsSource();
                    _this.ComputeShipmentFields();
                }
                else if (s == "OriginShipmentLoaded") {
                    _this.OriginShipment = _this.entityArgs.OriginEntity;
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildSummaryData();
                    _this.BuildProfitData();
                    if (_this.SavingRequestCode) {
                        _this.ApplySavingCommand();
                    }
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.IsLCLEntity = Tools_2.AppTool.IsLCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                    _this.IsFCLEntity = Tools_2.AppTool.IsFCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildSummaryData();
                    _this.BuildProfitData();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHPY" || tabCode == "JHPY") {
                    _this.BuildProfitData();
                    _this.CheckUpdateQuantities();
                }
            });
        }
    };
    PayablesTabComponent.prototype.ngOnDestroy = function () {
        Tools_2.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_2.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_2.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_2.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    PayablesTabComponent.prototype.LoadRequiredData = function () {
        var _this = this;
        if (this.IsEditingEnabled) {
            this.entityArgs.EditComponent.StartBusyIndicatorLoading();
            if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
                this.LoadOtherRequiredData();
            }
            else {
                var myService = new QuotePMService_1.QuotePMService();
                myService.get(this.EntityPM.QuoteId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.BaseQuote = myResponse.Result;
                        _this.SetGenerateButtons();
                    }
                    _this.LoadOtherRequiredData();
                });
            }
        }
    };
    PayablesTabComponent.prototype.LoadOtherRequiredData = function () {
        var _this = this;
        var todayDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        var myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        myCurrencyRatesService.getAll(SessionLocator_1.SessionLocator.LocalCurrencyId, todayDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllRates = myResponse.Result;
            }
            _this.entityArgs.EditComponent.StopBusyIndicator();
        });
    };
    PayablesTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.IsLCLEntity = Tools_2.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = Tools_2.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.entityResourceService.getEntityResourceByTableName("ShipmentPayable").subscribe(function (res) {
                _this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe(function (res2) {
                    _this.IsResourcesReady = true;
                    _this.SetLabels();
                    _this.SetUIProperties();
                    _this.BuildItemsSource();
                    _this.BuildSummaryData();
                    _this.InitializeProfitArea();
                    _this.SetGenerateButtons();
                });
            });
        }
    };
    PayablesTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    PayablesTabComponent.prototype.OnRowLoaded = function (Row) {
        var isExpandaple = false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (Row) {
                var item = Row.rowData;
                if (item) {
                    if (item.EntityPM.ChildShipmentPayables.length > 0) {
                        isExpandaple = true;
                    }
                }
                Row.SetExpandaple(isExpandaple);
            }
        }
    };
    PayablesTabComponent.prototype.SetLabels = function () {
        this.ExpectedAmountLocalHeader = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPayable.F.ExpectedAmountLocal").replace("%LocalCurrencyCode", this.LocalCurrencyCode);
    };
    PayablesTabComponent.prototype.SetEditEnabled = function () {
        var isEditingEnabled = true;
        if (this.EntityPM) {
            if (this.EntityPM.IsCancelled) {
                isEditingEnabled = false;
            }
            else if (this.EntityPM.IsAccountingClosed) {
                isEditingEnabled = false;
            }
            if (isEditingEnabled) {
                if (this.EntityPM.IsOperationalClosed) {
                    isEditingEnabled = false;
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.AllowPayables")) {
                        isEditingEnabled = true;
                    }
                }
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
    };
    PayablesTabComponent.prototype.SetUIProperties = function () {
        this.SetEditEnabled();
    };
    // ItemsSource
    PayablesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var itemsCollection = [];
        this.EntityPM.ShipmentPayables.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
            itemsCollection.push(new ShipmentPayableItem(item, _this));
        });
        this.EntityPM.ShipmentPayables.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
            itemsCollection.push(new ShipmentPayableItem(item, _this));
        });
        this.SetGenerateButtons();
        this.ItemsSource.InsertCollection(itemsCollection);
    };
    PayablesTabComponent.prototype.BuildSummaryData = function () {
        this.PayableList = this.EntityPM.ShipmentAPInvoices;
        if (this.IsByLocalCurrency) {
            this.AccrualsPayables = Tools_2.ArrayTool.Sum(this.EntityPM.ShipmentPayables, "ExpectedAmountLocal");
            this.AccountedPayables = Tools_2.ArrayTool.Sum(this.EntityPM.ShipmentPayables, "AccountedAmountInLocalCurrency");
            this.OpenPayables = Tools_2.ArrayTool.Sum(this.EntityPM.ShipmentPayables, "OpenAmountInLocalCurrency");
        }
        else {
            this.AccrualsPayables = Tools_2.ArrayTool.Sum(this.EntityPM.ShipmentPayables, "ExpectedAmountInProfitCurrency");
            this.AccountedPayables = Tools_2.ArrayTool.Sum(this.EntityPM.ShipmentPayables, "AccountedAmountInProfitCurrency");
            this.OpenPayables = Tools_2.ArrayTool.Sum(this.EntityPM.ShipmentPayables, "OpenAmountInProfitCurrency");
        }
        var myDifferencePayablesText = "N/A";
        var myDifferencePayablesColor = Tools_2.FontTool.Black;
        if (this.EntityPM.ShipmentPayables.length == 0) {
            myDifferencePayablesText = DecimalFormatter_1.DecimalFormatter.format(0, 2);
        }
        else {
            if (this.EntityPM.ShipmentPayables.filter(function (f) { return f.ShipmentPayableAmountTypeCode == "ACCU"; }).length > 0) {
                var myDifference = this.AccrualsPayables - (this.AccountedPayables + this.OpenPayables);
                if (Tools_2.AppTool.IsNullOrEmpty(myDifference)) {
                    myDifference = 0;
                }
                myDifferencePayablesText = DecimalFormatter_1.DecimalFormatter.format(myDifference, 2);
                if (myDifference < 0) {
                    var myDifferencePayablesColor = Tools_2.FontTool.Red;
                }
                if (Tools_2.AppTool.IsNullOrEmpty(myDifferencePayablesText)) {
                    myDifferencePayablesText = DecimalFormatter_1.DecimalFormatter.format(0, 2);
                }
            }
        }
        this.DifferencePayablesText = myDifferencePayablesText;
        this.DifferencePayablesColor = myDifferencePayablesColor;
    };
    PayablesTabComponent.prototype.InitializeProfitArea = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Profit")) {
            this.IsProfitAreaVisible = true;
        }
        this.IsCurrencyFilterVisible = SessionLocator_1.SessionLocator.LocalCurrencyId == this.EntityPM.ProfitCurrencyId ? false : true;
        this.SelectedCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.BuildProfitData();
    };
    PayablesTabComponent.prototype.GetPayablesInSelectedCurrency = function () {
        var myResult = 0;
        if (this.IsByLocalCurrency) {
            if (this.EntityPM.OpenPayablesInLocalCurrency != null) {
                myResult += this.EntityPM.OpenPayablesInLocalCurrency;
            }
            if (this.EntityPM.AccountedPayablesInLocalCurrency != null) {
                myResult += this.EntityPM.AccountedPayablesInLocalCurrency;
            }
        }
        else {
            if (this.EntityPM.OpenPayablesInProfitCurrency != null) {
                myResult += this.EntityPM.OpenPayablesInProfitCurrency;
            }
            if (this.EntityPM.AccountedPayablesInProfitCurrency != null) {
                myResult += this.EntityPM.AccountedPayablesInProfitCurrency;
            }
        }
        if (myResult == null) {
            myResult = 0;
        }
        return myResult;
    };
    PayablesTabComponent.prototype.GetReceivablesInSelectedCurrency = function () {
        var myResult = 0;
        if (this.IsByLocalCurrency) {
            if (this.EntityPM.OpenReceivablesInLocalCurrency != null) {
                myResult += this.EntityPM.OpenReceivablesInLocalCurrency;
            }
            if (this.EntityPM.AccountedReceivablesInLocalCurrency != null) {
                myResult += this.EntityPM.AccountedReceivablesInLocalCurrency;
            }
        }
        else {
            if (this.EntityPM.OpenReceivablesInProfitCurrency != null) {
                myResult += this.EntityPM.OpenReceivablesInProfitCurrency;
            }
            if (this.EntityPM.AccountedReceivablesInProfitCurrency != null) {
                myResult += this.EntityPM.AccountedReceivablesInProfitCurrency;
            }
        }
        if (myResult == null) {
            myResult = 0;
        }
        return myResult;
    };
    PayablesTabComponent.prototype.GetProfitInSelectedCurrency = function () {
        var myResult = 0;
        if (this.IsByLocalCurrency) {
            myResult = this.EntityPM.ProfitInLocalCurrency;
        }
        else {
            myResult = this.EntityPM.ProfitInProfitCurrency;
        }
        if (myResult == null) {
            myResult = 0;
        }
        return myResult;
    };
    PayablesTabComponent.prototype.GetEstimateProfitInSelectedCurrency = function () {
        var myResult = 0;
        if (this.IsByLocalCurrency) {
            myResult = this.EntityPM.EstimateProfitInLocalCurrency;
        }
        else {
            myResult = this.EntityPM.EstimateProfitInProfitCurrency;
        }
        if (myResult == 0) {
            myResult = null;
        }
        return myResult;
    };
    PayablesTabComponent.prototype.OnSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }
        else {
            this.IsByLocalCurrency = false;
        }
        this.BuildSummaryData();
        this.BuildProfitData();
    };
    PayablesTabComponent.prototype.BuildProfitData = function () {
        var myProfitInSelectedCurrencyText = "N/A";
        var myPayablesInSelectedCurrencyText = "N/A";
        var myReceivablesInSelectedCurrencyText = "N/A";
        var myProfitRate = "N/A";
        var myDefferenceInSelectedCurrency = 0;
        var myDefferenceInSelectedCurrencyText = "";
        // Payables
        var CountOfPAY = this.EntityPM.ShipmentPayables.length;
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            CountOfPAY = this.EntityPM.ConnectedShipmentsPayablesCount;
        }
        // Receivables
        var CountOfREC = this.EntityPM.ShipmentReceivables.length;
        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            if (this.EntityPM.ProrateReceivables) {
                CountOfREC = this.EntityPM.ConnectedShipmentsReceivablesCount;
            }
            else {
                CountOfREC += this.EntityPM.ConnectedShipmentsReceivablesCount;
            }
        }
        if (CountOfPAY > 0) {
            myPayablesInSelectedCurrencyText = DecimalFormatter_1.DecimalFormatter.format(this.GetPayablesInSelectedCurrency(), 2);
        }
        if (CountOfREC > 0) {
            myReceivablesInSelectedCurrencyText = DecimalFormatter_1.DecimalFormatter.format(this.GetReceivablesInSelectedCurrency(), 2);
        }
        if (CountOfPAY > 0 || CountOfREC > 0) {
            myProfitInSelectedCurrencyText = DecimalFormatter_1.DecimalFormatter.format(this.GetProfitInSelectedCurrency(), 2);
        }
        if (this.EntityPM.ProfitExchangeRate != null) {
            myProfitRate = DecimalFormatter_1.DecimalFormatter.format(this.EntityPM.ProfitExchangeRate, 5);
        }
        this.ProfitInSelectedCurrencyText = myProfitInSelectedCurrencyText;
        this.PayablesInSelectedCurrencyText = myPayablesInSelectedCurrencyText;
        this.ReceivablesInSelectedCurrencyText = myReceivablesInSelectedCurrencyText;
        this.ProfitRate = myProfitRate;
        var myProfitInSelectedCurrency = this.GetProfitInSelectedCurrency();
        this.estimateProfitInSelectedCurrency = this.GetEstimateProfitInSelectedCurrency();
        if (myProfitInSelectedCurrency != null && this.EstimateProfitInSelectedCurrency != null) {
            myDefferenceInSelectedCurrency = myProfitInSelectedCurrency - this.EstimateProfitInSelectedCurrency;
            myDefferenceInSelectedCurrencyText = DecimalFormatter_1.DecimalFormatter.format(myDefferenceInSelectedCurrency, 2);
        }
        this.DefferenceInSelectedCurrencyText = myDefferenceInSelectedCurrencyText;
        var isProfitRateVisible = false;
        if (!this.IsByLocalCurrency) {
            if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ProfitCurrencyId) && this.EntityPM.ProfitCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                isProfitRateVisible = true;
            }
        }
        this.IsProfitRateVisible = isProfitRateVisible;
    };
    Object.defineProperty(PayablesTabComponent.prototype, "EstimateProfitInSelectedCurrency", {
        get: function () { return this.estimateProfitInSelectedCurrency; },
        set: function (value) {
            if (this.estimateProfitInSelectedCurrency != value) {
                this.estimateProfitInSelectedCurrency = value;
                var rate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                if (this.IsByLocalCurrency) {
                    this.EntityPM.EstimateProfitInLocalCurrency = Tools_2.AppTool.Round(value, 2);
                    this.EntityPM.EstimateProfitInProfitCurrency = Tools_2.AppTool.Round(value / rate, 2);
                }
                else {
                    this.EntityPM.EstimateProfitInProfitCurrency = Tools_2.AppTool.Round(value, 2);
                    this.EntityPM.EstimateProfitInLocalCurrency = Tools_2.AppTool.Round(value * rate, 2);
                }
                this.BuildProfitData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PayablesTabComponent.prototype, "ProfitExchangeRate", {
        get: function () { return this.EntityPM.ProfitExchangeRate; },
        set: function (value) {
            if (this.EntityPM.ProfitExchangeRate != value) {
                this.EntityPM.ProfitExchangeRate = Tools_2.AppTool.Round(value, 5);
                //this.OnProfitExchangeRateChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    PayablesTabComponent.prototype.ShowProfitClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 750;
        logWindow.ShowCloseButton = true;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Profit.ProfitDetails");
        logWindow.WindowArgs = { ShipmentPM: this.EntityPM, IsByLocalCurrency: this.IsByLocalCurrency, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible };
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Profit/ProfitComponent');
    };
    PayablesTabComponent.prototype.SetGenerateButtons = function () {
        var isGenerateButtonsVisible = false;
        if (this.EntityPM.ShipmentPayables.length > 0) {
            isGenerateButtonsVisible = false;
        }
        else {
            isGenerateButtonsVisible = true;
        }
        var isNoChargesTextVisibil = false;
        var isGenerateFromQuoteChargesEnabled = false;
        if (isGenerateButtonsVisible) {
            if (this.IsEditingEnabled) {
                if (this.BaseQuote != null) {
                    if (this.BaseQuote.QuoteCharges.length > 0) {
                        isGenerateFromQuoteChargesEnabled = true;
                    }
                    else {
                        isNoChargesTextVisibil = true;
                    }
                }
            }
        }
        this.IsNoChargesTextVisibil = isNoChargesTextVisibil;
        this.IsGenerateButtonsVisible = isGenerateButtonsVisible;
        this.IsGenerateFromQuoteChargesEnabled = isGenerateFromQuoteChargesEnabled;
    };
    PayablesTabComponent.prototype.GenerateClicked = function (myCommandCode) {
        var _this = this;
        if (this.EntityPM.ShipmentPackages.length == 0) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.ThereAreNoPackages"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.StartGenerating(myCommandCode);
                }
            });
        }
        else {
            this.StartGenerating(myCommandCode);
        }
    };
    PayablesTabComponent.prototype.StartGenerating = function (myCommandCode) {
        var _this = this;
        switch (myCommandCode) {
            case "ATDS": {
                // AutoDisplay                
                var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GeneratePayablesAutoDisplay();
                this.OnEntityDataGenerated();
                break;
            }
            case "QTPY": {
                // FromQuotePayablesOnly
                var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GeneratePayablesFromQuote(this.BaseQuote);
                this.OnEntityDataGenerated();
                break;
            }
            case "ORGN": {
                // FromOriginShipment
                if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.OriginShipmentId)) {
                    if (this.OriginShipment) {
                        var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                        Generator.GeneratePayablesFromOriginShipment(this.OriginShipment);
                        this.OnEntityDataGenerated();
                        this.ItemsSource.Collection.forEach(function (item) {
                            item.Rate = _this.GetCurrencyRate(item.CurrencyId);
                            item.SetQuantity();
                            item.ComputeTotalAmount();
                        });
                    }
                    else {
                        this.entityArgs.EditComponent.StartBusyIndicatorLoading();
                        var myService = new ShipmentPMService_1.ShipmentPMService();
                        myService.get(this.EntityPM.OriginShipmentId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.OriginShipment = myResponse.Result;
                                _this.entityArgs.OriginEntity = myResponse.Result;
                                _this.CurrentSession.FireEvent("OriginShipmentLoaded");
                                var Generator = new Tools_1.ShipmentGenerator(_this.EntityPM, _this.AllRates);
                                Generator.GeneratePayablesFromOriginShipment(_this.OriginShipment);
                                _this.OnEntityDataGenerated();
                                _this.ItemsSource.Collection.forEach(function (item) {
                                    item.Rate = _this.GetCurrencyRate(item.CurrencyId);
                                    item.SetQuantity();
                                    item.ComputeTotalAmount();
                                });
                            }
                            _this.entityArgs.EditComponent.StopBusyIndicator();
                        });
                    }
                }
                break;
            }
        }
    };
    PayablesTabComponent.prototype.OnEntityDataGenerated = function () {
        this.BuildItemsSource();
        this.ComputeShipmentFields();
    };
    PayablesTabComponent.prototype.GetCurrencyRate = function (currencyId) {
        var myResult;
        if (currencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
            myResult = 1;
        }
        else {
            var myLastRate = this.AllRates.filter(function (d) { return d.ForeignCurrencyId == currencyId; })[0];
            if (myLastRate != null) {
                myResult = myLastRate.Rate;
            }
        }
        return myResult;
    };
    // Commands
    PayablesTabComponent.prototype.AddPayable = function () {
        var newItem = new ShipmentPayablePM_1.ShipmentPayablePM(null);
        newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newItem.ShipmentId = this.EntityPM.Id;
        newItem.ShipmentNumber = this.EntityPM.ShipmentNumber;
        newItem.CreateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        newItem.UpdateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        newItem.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newItem.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newItem.ShipmentPayableLineStatusCode = "EMPT";
        newItem.ShipmentPayableAmountTypeCode = "ACCU";
        newItem.ShipmentPayableAmountTypeName = "Accrual";
        newItem.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        var itemComponent = new ShipmentPayableItem(newItem, this, true);
        this.RunAddEditPayable(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.AddPayable"));
    };
    PayablesTabComponent.prototype.EditPayable = function (itemComponent) {
        this.RunAddEditPayable(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.EditPayable"));
    };
    PayablesTabComponent.prototype.RunAddEditPayable = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Payables/AddEditPayableComponent');
    };
    PayablesTabComponent.prototype.DeleteItem = function (itemComponent) {
        var _this = this;
        if (itemComponent.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisPayable"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.EntityPM.RemovePayable(itemComponent.EntityPM);
                    _this.BuildItemsSource();
                    if (itemComponent.ChargesGroupCode == "FRT") {
                        _this.OnFreightAmountChanged();
                    }
                    _this.ComputeShipmentFields();
                }
            });
        }
    };
    // Invoice    
    PayablesTabComponent.prototype.ReceiveInvoiceClicked = function () {
        if (FeatureLocator_1.FeatureLocator.HasEntityPermessions("APInvoice", "NEW", true)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "NewInvoice";
                this.EntityPM.ShipmentPayables.forEach(function (item) {
                    if (Tools_2.AppTool.IsNullOrEmpty(item.ShipmentPayableLineStatusCode) || item.ShipmentPayableLineStatusCode == "EMPT") {
                        if (!Tools_2.AppTool.IsNullOrZero(item.Quantity) && !Tools_2.AppTool.IsNullOrZero(item.UnitPrice)) {
                            item.ShipmentPayableLineStatusCode = "OAMT";
                        }
                    }
                });
                this.SaveChanges();
            }
        }
    };
    PayablesTabComponent.prototype.ViewInvoiceClicked = function (invoiceId) {
        if (!Tools_2.AppTool.IsNullOrEmpty(invoiceId)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "ViewInvoice";
                this.SavingRequestParam = invoiceId;
                this.SaveChanges();
            }
        }
    };
    PayablesTabComponent.prototype.ShowQuoteClicked = function () {
        if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "ViewQuote";
                this.SavingRequestParam = this.EntityPM.QuoteId;
                this.SaveChanges();
            }
        }
    };
    PayablesTabComponent.prototype.SaveChanges = function () {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    PayablesTabComponent.prototype.StopSavingFlags = function () {
        this.SavingRequested = false;
        this.SavingRequestCode = null;
        this.SavingRequestParam = null;
    };
    PayablesTabComponent.prototype.ApplySavingCommand = function () {
        switch (this.SavingRequestCode) {
            case "NewInvoice": {
                this.RunNewInvoice();
                break;
            }
            case "ViewInvoice": {
                this.RunViewInvoice();
                break;
            }
            case "ViewQuote": {
                this.RunViewQuote();
                break;
            }
        }
        this.StopSavingFlags();
    };
    PayablesTabComponent.prototype.RunViewInvoice = function () {
        var _this = this;
        if (this.SavingRequestParam) {
            var entityId = this.SavingRequestParam;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                var iFields = [];
                iFields.push({ FieldName: "ShipmentConcurrencyGUID", FieldValue: _this.EntityPM.ConcurrencyGUID });
                iFields.push({ FieldName: "ShipmentNewConcurrencyGUID", FieldValue: _this.EntityPM.NewConcurrencyGUID });
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'APInvoice', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber, EntityFields: iFields });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                });
                cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
                cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
        }
    };
    PayablesTabComponent.prototype.RunViewQuote = function () {
        var _this = this;
        if (this.SavingRequestParam) {
            var entityId = this.SavingRequestParam;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'Quote', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber });
                cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.BaseQuote = cmpRef.instance.EntityPM;
                    }
                });
                cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.BaseQuote = cmpRef.instance.EntityPM;
                    }
                });
            });
        }
    };
    PayablesTabComponent.prototype.RunNewInvoice = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.B.Payables.ReceiveInvoice");
        logitudeWindow.WindowArgs = { ShipmentPM: this.EntityPM };
        logitudeWindow.ComponentLoaded.subscribe(function (comp) {
            logitudeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'APInvoice', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber });
                        var isEditComponentSaved = false;
                        cmpRef.instance.BackCompleted.subscribe(function (bk) {
                            if (isEditComponentSaved) {
                                _this.entityArgs.EditComponent.ReloadEntityPM();
                            }
                        });
                        cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                            if (isSaveSuccess) {
                                isEditComponentSaved = true;
                            }
                        });
                        cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                            if (isSaveSuccess) {
                                isEditComponentSaved = true;
                            }
                        });
                    });
                }
            });
        });
        logitudeWindow.Show('./InvoiceModules/APInvoice/Components/NewEntity/NewAPInvoiceComponent');
    };
    PayablesTabComponent.prototype.TariffsButtonClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("TariffHeader.O.Tariffs");
        logWindow.WindowArgs = this;
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Tariffs/TariffsComponent');
    };
    PayablesTabComponent.prototype.ComputeShipmentFields = function () {
        if (this.EntityPM != null) {
            Tools_1.ShipmentTool.ComputeTotals(this.EntityPM);
        }
        this.BuildSummaryData();
        this.BuildProfitData();
    };
    PayablesTabComponent.prototype.OnFreightAmountChanged = function () {
        this.ItemsSource.Collection.filter(function (f) { return f.ChargesGroupCode != "FRT" && f.MeasurementCode == "PRFR"; }).forEach(function (item) {
            if (item.IsLineAttachted == false) {
                item.SetQuantity();
            }
        });
    };
    PayablesTabComponent.prototype.CheckUpdateQuantities = function () {
        var _this = this;
        var updateMessage = null;
        var activeLines = [];
        activeLines = this.EntityPM.ShipmentPayables;
        activeLines = activeLines.filter(function (d) { return d.ShipmentPayableParentId == null; });
        activeLines = activeLines.filter(function (d) { return d.ShipmentPayableAmountTypeCode != "NEXP"; });
        activeLines = activeLines.filter(function (d) { return d.ShipmentPayableLineStatusCode != "ACCT"; });
        activeLines = activeLines.filter(function (d) { return d.ShipmentPayableLineStatusCode != "PACC"; });
        activeLines = activeLines.filter(function (d) { return d.UnitPrice != null; });
        if (activeLines.length > 0) {
            var isDifferentOrders = false;
            var isDifferentPRVL = false;
            var isDifferentPRFR = false;
            activeLines.forEach(function (item) {
                switch (item.MeasurementCode) {
                    case "CWKG": {
                        if (item.Quantity != _this.EntityPM.ChargeableWeightInKG) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "GWKG": {
                        if (item.Quantity != _this.EntityPM.GrossWeightInKG) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "GRWT": {
                        if (item.Quantity != _this.EntityPM.GrossWeight) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "GWTN": {
                        if (item.Quantity != _this.EntityPM.GrossWeightPerTon) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "QTY": {
                        if (_this.IsLCLEntity) {
                            if (item.Quantity != _this.EntityPM.NumberOfPackages) {
                                isDifferentOrders = true;
                            }
                        }
                        else {
                            if (item.Quantity != _this.EntityPM.NumberOfContainers) {
                                isDifferentOrders = true;
                            }
                        }
                        break;
                    }
                    case "CHWT": {
                        if (item.Quantity != _this.EntityPM.ChargeableWeight) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "VOLU": {
                        if (item.Quantity != _this.EntityPM.Volume) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "BTEU": {
                        if (item.Quantity != _this.EntityPM.TEU) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                    case "PRVL": {
                        if (item.Quantity != _this.EntityPM.ValueOfGoods) {
                            isDifferentPRVL = true;
                        }
                        break;
                    }
                    case "PRFR": {
                        if (_this.EntityPM.ShipmentPayables.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).length > 0) {
                            var FRT_Quantity = Tools_2.ArrayTool.Sum(_this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_2.AppTool.IsNullOrEmpty(d.ShipmentPayableParentId); }), "ExpectedAmount");
                            if (item.Quantity != FRT_Quantity) {
                                isDifferentPRVL = true;
                            }
                            if (_this.EntityPM.ShipmentPayables.filter(function (f) { return f.MeasurementCode == "PRFR" && f.Quantity != FRT_Quantity; }).length > 0) {
                                isDifferentPRFR = true;
                            }
                        }
                        break;
                    }
                    case "VCBM": {
                        if (item.Quantity != _this.EntityPM.VolumeInCBM) {
                            isDifferentOrders = true;
                        }
                        break;
                    }
                }
            });
            if (isDifferentOrders) {
                updateMessage = "You have updated the packages details, apply the new values?";
            }
            else if (isDifferentPRVL) {
                updateMessage = "You have updated the value of goods, apply the new values?";
            }
            else if (isDifferentPRFR) {
                updateMessage = "You have updated the value of freight charge, apply the new values?";
            }
        }
        this.UpdateQuantitiesMessage = updateMessage;
        this.UpdateQuantitiesMessageWidth = Tools_2.AppTool.GetTextWidth(updateMessage, 11);
        this.IsUpdateQuantitiesVisible = Tools_2.AppTool.IsNullOrEmpty(updateMessage) ? false : true;
    };
    PayablesTabComponent.prototype.UpdateQuantitiesClicked = function () {
        var activeLines = [];
        activeLines = this.ItemsSource.Collection;
        activeLines = activeLines.filter(function (d) { return d.EntityPM.ShipmentPayableParentId == null; });
        activeLines = activeLines.filter(function (d) { return d.EntityPM.ShipmentPayableAmountTypeCode != "NEXP"; });
        activeLines = activeLines.filter(function (d) { return d.EntityPM.ShipmentPayableLineStatusCode != "ACCT"; });
        activeLines = activeLines.filter(function (d) { return d.EntityPM.ShipmentPayableLineStatusCode != "PACC"; });
        activeLines.forEach(function (item) {
            item.SetQuantity();
        });
        this.CheckUpdateQuantities();
    };
    PayablesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PayablesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PayablesTabComponent);
    return PayablesTabComponent;
}());
exports.PayablesTabComponent = PayablesTabComponent;
var ShipmentPayableItem = /** @class */ (function (_super) {
    __extends(ShipmentPayableItem, _super);
    function ShipmentPayableItem(entity, fatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "ShipmentPayable";
        _this.DataContext = _this;
        _this.LocalCurrencyCode = null;
        _this.ProfitCurrencyCode = null;
        _this.ByContainersItemsSource = [];
        _this.InsideItemsSource = [];
        _this.IsNewEntity = false;
        // SetUIProperties
        _this.IsRateEnabled = false;
        _this.IsQuantityEnabled = false;
        _this.IsUnitPriceEnabled = false;
        _this.IsTotalAmountEnabled = false;
        _this.IsOpenAmountEnabled = false;
        _this.IsEditingEnabled = false;
        _this.IsLineAttachted = false;
        _this.IsProfitAmountVisible = false;
        _this.IsEditExchangeRateVisible = false;
        // Line Cells
        _this.SatusTypeToolTip = null;
        _this.MinMaxFromQuoteToolTip = "";
        _this.IsMinMaxFromQuoteIconVisible = false;
        _this.IsMinFromTarrifIconVisible = false;
        _this.IsMaxFromTarrifIconVisible = false;
        _this.IsAccountedAmountVisible = false;
        _this.IsInvoicesIconVisible = false;
        _this.ExpectedAmountColor = null;
        _this.IsOpenAmountVisible = false;
        _this.OpenAmountColor = null;
        // BCNT | ByContainers
        _this.IsByContainerType = false;
        // Amounts
        _this.RelativeRateDate = null;
        _this.rateDate = null;
        _this.CreatedByUserName = null;
        _this.UpdatedByUserName = null;
        _this.ReceivableSummary = null;
        _this.PayableSummary = null;
        _this.ProfitSummary = null;
        // Invoice
        _this.PayableInvoices = [];
        _this.isInvoicesListLoaded = false;
        _this.isInvoicesTooltipOpened = false;
        _this.EntityPM = entity;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.LocalCurrencyCode = fatherComponent.LocalCurrencyCode;
        _this.ProfitCurrencyCode = fatherComponent.ProfitCurrencyCode;
        _this.SetUIProperties();
        _this.SetLineCells();
        _this.SetLineSummary();
        _this.BuildInsidePayables();
        return _this;
    }
    ShipmentPayableItem.prototype.SetUIProperties = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        var isLineAttachted = false;
        var isEditingEnabled = this.fatherComponent.IsEditingEnabled;
        var isOpenAmountEnabled = false;
        if (this.EntityPM.ShipmentPayableParentId != null) {
            isLineAttachted = true;
        }
        if (this.EntityPM.ShipmentPayableLineStatusCode == "ACCT" || this.EntityPM.ShipmentPayableLineStatusCode == "PACC") {
            isLineAttachted = true;
        }
        if (isEditingEnabled) {
            isOpenAmountEnabled = true;
            if (this.EntityPM.ShipmentPayableParentId != null) {
                isOpenAmountEnabled = false;
            }
            if (this.EntityPM.ShipmentPayableAmountTypeCode == "NEXP" || this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
                isOpenAmountEnabled = false;
            }
            if (isLineAttachted) {
                isEditingEnabled = false;
            }
        }
        var isRateEnabled = false;
        var isChargeEnabled = false;
        var isQuantityEnabled = false;
        var isUnitPriceEnabled = false;
        var isTotalAmountEnabled = false;
        if (isEditingEnabled) {
            if (this.IsNewEntity) {
                isChargeEnabled = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "ShipmentEditExchangeRate")) {
                if (this.CurrencyId != null) {
                    if (this.CurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                        isRateEnabled = true;
                    }
                }
            }
            isQuantityEnabled = true;
            if (!this.EntityPM.IsChargeBySteps) {
                isUnitPriceEnabled = true;
                isTotalAmountEnabled = true;
            }
        }
        this.IsRateEnabled = isRateEnabled;
        this.IsQuantityEnabled = isQuantityEnabled;
        this.IsUnitPriceEnabled = isUnitPriceEnabled;
        this.IsTotalAmountEnabled = isTotalAmountEnabled;
        this.IsOpenAmountEnabled = isOpenAmountEnabled;
        this.IsEditingEnabled = isEditingEnabled;
        this.IsLineAttachted = isLineAttachted;
        this.UIProperties.SetEnabled("ExpectedAmount", this.ObjectTableName, this.IsTotalAmountEnabled);
        this.UIProperties.SetEnabled("ExpectedAmountLocal", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Rate", this.ObjectTableName, isRateEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, isChargeEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsQuantityEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, this.IsUnitPriceEnabled);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MeasurementId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PrepaidCollectId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_AmountProfit();
        this.SetUIProperties_MeasurementId();
    };
    ShipmentPayableItem.prototype.SetUIProperties_AmountProfit = function () {
        var isFieldVisible = false;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.CurrencyId)) {
            if (this.ShipmentPM.ProfitCurrencyId != this.CurrencyId && this.ShipmentPM.ProfitCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                isFieldVisible = true;
            }
        }
        this.IsProfitAmountVisible = isFieldVisible;
        this.UIProperties.SetEnabled("ExpectedAmountInProfitCurrency", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("ExpectedAmountInProfitCurrency", this.ObjectTableName, isFieldVisible);
    };
    ShipmentPayableItem.prototype.SetUIProperties_MeasurementId = function () {
        var isRequired = false;
        if (this.EntityPM.ShipmentPayableAmountTypeCode == "ACCU") {
            if (Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.MeasurementId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetRequired("MeasurementId", this.ObjectTableName, isRequired);
    };
    ShipmentPayableItem.prototype.SetLineCells = function () {
        this.SetSatusTypeToolTip();
        this.SetMinMaxFromQuoteIconVisibility();
        this.SetMinFromTarrifIconVisibility();
        this.SetMaxFromTarrifIconVisibility();
        this.SetOpenAmountCell();
        this.SetExpectedAmountCell();
        this.IsAccountedAmountVisible = this.AccountedAmount != null && this.AccountedAmount != 0;
        this.IsInvoicesIconVisible = (this.EntityPM.ShipmentPayableLineStatusCode == "PACC" || this.EntityPM.ShipmentPayableLineStatusCode == "ACCT") ? true : false;
    };
    ShipmentPayableItem.prototype.SetSatusTypeToolTip = function () {
        var myResult;
        switch (this.EntityPM.ShipmentPayableLineStatusCode) {
            case "APPD": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.Approved");
                break;
            }
            case "NOIN": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.NoInvoiceNeeded");
                break;
            }
            case "OAMT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.OpenAmount");
                break;
            }
            case "ACCT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.Accounted");
                break;
            }
            case "PACC": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.PartiallyAccounted");
                break;
            }
            default: {
                break;
            }
        }
        this.SatusTypeToolTip = myResult;
    };
    ShipmentPayableItem.prototype.SetMinMaxFromQuoteIconVisibility = function () {
        var isVisible = false;
        var iTitle = null;
        var iAmount = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            iAmount = Tools_2.AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }
        if (iAmount != null) {
            if (this.QuoteCostMinAmount != null) {
                if (iAmount < this.QuoteCostMinAmount) {
                    iAmount = this.QuoteCostMinAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.Payables.AmountdueQuoteMinimum");
                }
            }
            if (this.QuoteCostMaxAmount != null) {
                if (iAmount > this.QuoteCostMaxAmount) {
                    iAmount = this.QuoteCostMaxAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.Payables.AmountdueQuoteMaximum");
                }
            }
        }
        this.MinMaxFromQuoteToolTip = iTitle;
        this.IsMinMaxFromQuoteIconVisible = isVisible;
    };
    ShipmentPayableItem.prototype.SetMinFromTarrifIconVisibility = function () {
        var isVisible = false;
        var culculatedAmount = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            culculatedAmount = Tools_2.AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }
        if (culculatedAmount != null) {
            if (this.MinAmount != null) {
                if (culculatedAmount < this.MinAmount) {
                    isVisible = true;
                }
            }
        }
        this.IsMinFromTarrifIconVisible = isVisible;
    };
    ShipmentPayableItem.prototype.SetMaxFromTarrifIconVisibility = function () {
        var isVisible = false;
        var culculatedAmount = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            culculatedAmount = Tools_2.AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }
        if (culculatedAmount != null) {
            if (this.MaxAmount != null) {
                if (culculatedAmount > this.MaxAmount) {
                    isVisible = true;
                }
            }
        }
        this.IsMaxFromTarrifIconVisible = isVisible;
    };
    ShipmentPayableItem.prototype.SetExpectedAmountCell = function () {
        var myColor = Tools_2.FontTool.Black;
        if (this.InsideItemsSource) {
            var sumOfAmount = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "ExpectedAmount");
            var sumOfAmount = Tools_2.AppTool.Round(sumOfAmount, 2);
            if ((sumOfAmount < 0 || sumOfAmount > 0) && !Tools_2.AppTool.IsNullOrEmpty(this.ExpectedAmount) && sumOfAmount != this.ExpectedAmount) {
                myColor = Tools_2.FontTool.Red;
            }
            else if (this.EntityPM.IsChargeBySteps) {
                myColor = Tools_2.FontTool.Gray;
            }
            else if (this.EntityPM.ShipmentPayableLineStatusCode == "ACCT" || this.EntityPM.ShipmentPayableLineStatusCode == "PACC") {
                myColor = Tools_2.FontTool.Gray;
            }
            else if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentPayableParentId)) {
                myColor = Tools_2.FontTool.Gray;
            }
        }
        this.ExpectedAmountColor = myColor;
    };
    ShipmentPayableItem.prototype.SetOpenAmountCell = function () {
        var isOpenAmountVisible = true;
        if (Tools_2.AppTool.IsNullOrEmpty(this.OpenAmount)) {
            isOpenAmountVisible = false;
        }
        else if (this.OpenAmount == 0) {
            if (Tools_2.AppTool.IsNullOrZero(this.Quantity) || Tools_2.AppTool.IsNullOrZero(this.UnitPrice)) {
                isOpenAmountVisible = false;
            }
        }
        var myOpenAmountColor = Tools_2.FontTool.Black;
        if (this.EntityPM.ShipmentPayableAmountTypeCode == "NEXP") {
            myOpenAmountColor = Tools_2.FontTool.Gray;
        }
        else {
            if (this.OpenAmount < 0) {
                myOpenAmountColor = Tools_2.FontTool.Red;
            }
            else {
                if (this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
                    myOpenAmountColor = Tools_2.FontTool.Gray;
                }
            }
        }
        this.OpenAmountColor = myOpenAmountColor;
        this.IsOpenAmountVisible = isOpenAmountVisible;
        this.GetCorrectionUser();
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "ChargesTypeCode", {
        get: function () { return this.EntityPM.ChargesTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ChargesTypeName", {
        get: function () { return this.EntityPM.ChargesTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ChargesGroupCode", {
        get: function () { return this.EntityPM.ChargesGroupCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ChargesTypeId", {
        get: function () { return this.EntityPM.ChargesTypeId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ChargesTypeId != value) {
                this.EntityPM.ChargesTypeId = value;
                this.SetLineSummary();
                this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.ChargesTypeId == value; }).forEach(function (item) {
                    item.SetLineSummary();
                });
                if (value == null) {
                    this.OnChargesTypeChanged(null);
                }
                else {
                    var myService = new ChargesTypeListService_1.ChargesTypeListService();
                    myService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.OnChargesTypeChanged(list);
                            }
                            else {
                                myService.getSingle(value).subscribe(function (myResponse2) {
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
    ShipmentPayableItem.prototype.OnChargesTypeChanged = function (list) {
        if (list) {
            this.EntityPM.ChargesTypeCode = list.Code;
            this.EntityPM.ChargesTypeName = list.EnglishName;
            this.EntityPM.ChargesGroupCode = list.ChargesGroupCode;
            this.EntityPM.DueTypeCode = list.DueTypeCode;
            this.EntityPM.DueTypeName = list.DueTypeName;
            this.VatTypeId = list.VatTypeId;
            this.EntityPM.IATACodeId = list.IATACodeId;
            //this.EntityPM.IsBackToBack = list.IsBackToBack;
            this.SetPrepaidCollectId();
            if (!Tools_2.AppTool.IsNullOrEmpty(list.PayablesDefaultCurrencyId)) {
                this.CurrencyId = list.PayablesDefaultCurrencyId;
            }
            else {
                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                }
                else {
                    this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }
            if (this.fatherComponent.IsLCLEntity) {
                this.MeasurementId = list.MeasurementId;
            }
            else {
                this.MeasurementId = list.ContainerMeasurementId != null ? list.ContainerMeasurementId : list.MeasurementId;
            }
        }
        else {
            this.EntityPM.ChargesTypeCode = null;
            this.EntityPM.ChargesTypeName = null;
            this.EntityPM.ChargesGroupCode = null;
            this.EntityPM.DueTypeCode = null;
            this.EntityPM.DueTypeName = null;
            this.MeasurementId = null;
            this.PrepaidCollectId = null;
            this.CurrencyId = null;
            this.VatTypeId = null;
            this.EntityPM.IATACodeId = null;
            //this.EntityPM.IsBackToBack = false;
        }
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "VatTypeId", {
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (value) {
            if (this.EntityPM.VatTypeId != value) {
                this.EntityPM.VatTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.CurrencyId != newValue) {
                this.EntityPM.CurrencyId = newValue;
                this.SetUIProperties();
                if (newValue == null) {
                    this.Rate = null;
                    this.CurrencyCode = null;
                }
                else {
                    if (SessionLocator_1.SessionLocator.LocalCurrencyId == newValue) {
                        this.Rate = 1;
                        this.CurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
                    }
                    else {
                        var myService = new CurrencyListService_1.CurrencyListService();
                        myService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list != null) {
                                    _this.CurrencyCode = list.Code;
                                }
                            }
                        });
                    }
                    var todayDate = Tools_2.DateTool.GetCurrentDateAsUtc();
                    var myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
                    myCurrencyRatesService.getAll(SessionLocator_1.SessionLocator.LocalCurrencyId, todayDate).subscribe(function (myResponse2) {
                        if (!myResponse2.HasError) {
                            _this.fatherComponent.AllRates = myResponse2.Result;
                            _this.SetLineRate();
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "CurrencyCode", {
        get: function () { return this.EntityPM.CurrencyCode; },
        set: function (value) {
            if (this.EntityPM.CurrencyCode != value) {
                this.EntityPM.CurrencyCode = value;
                this.ApplyByContainersFields();
                this.UpdateInsideItemsSource_Currency();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPayableItem.prototype.SetLineRate = function () {
        var _this = this;
        if (Tools_2.AppTool.IsNullOrEmpty(this.CurrencyId)) {
            this.Rate = null;
        }
        else if (this.CurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
            this.Rate = 1;
        }
        else {
            var lastRate = this.fatherComponent.AllRates.filter(function (d) { return d.ForeignCurrencyId == _this.CurrencyId; })[0];
            if (lastRate != null) {
                this.Rate = lastRate.Rate;
                this.RateDate = lastRate.ValueDate;
            }
        }
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.MeasurementId != newValue) {
                this.EntityPM.MeasurementId = newValue;
                this.IsByContainerType = false;
                this.ByContainersItemsSource = [];
                this.SetUIProperties_MeasurementId();
                if (newValue == null) {
                    this.Quantity = null;
                    this.EntityPM.MeasurementCode = null;
                    this.EntityPM.MeasurementShortName = null;
                    this.UpdateInsideItemsSource_Measurement();
                }
                else {
                    var myService = new MeasurementListService_1.MeasurementListService();
                    myService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.EntityPM.MeasurementCode = list.Code;
                                _this.EntityPM.MeasurementShortName = list.ShortName;
                                switch (list.Code) {
                                    case "GRWT": {
                                        _this.Quantity = _this.ShipmentPM.GrossWeight;
                                        break;
                                    }
                                    case "CHWT": {
                                        _this.Quantity = _this.ShipmentPM.ChargeableWeight;
                                        break;
                                    }
                                    case "VOLU": {
                                        _this.Quantity = _this.ShipmentPM.Volume;
                                        break;
                                    }
                                    case "BTEU": {
                                        _this.Quantity = _this.ShipmentPM.TEU;
                                        break;
                                    }
                                    case "FIXD": {
                                        _this.Quantity = 1;
                                        break;
                                    }
                                    case "GWTN": {
                                        _this.Quantity = _this.ShipmentPM.GrossWeightPerTon;
                                        break;
                                    }
                                    case "QTY": {
                                        _this.Quantity = _this.fatherComponent.IsLCLEntity ? _this.ShipmentPM.NumberOfPackages : _this.ShipmentPM.NumberOfContainers;
                                        break;
                                    }
                                    case "CWKG": {
                                        _this.Quantity = _this.ShipmentPM.ChargeableWeightInKG;
                                        break;
                                    }
                                    case "GWKG": {
                                        _this.Quantity = _this.ShipmentPM.GrossWeightInKG;
                                        break;
                                    }
                                    case "VCBM": {
                                        _this.Quantity = _this.ShipmentPM.VolumeInCBM;
                                        break;
                                    }
                                    case "PRVL": {
                                        _this.Quantity = _this.ShipmentPM.ValueOfGoods;
                                        _this.CurrencyId = _this.ShipmentPM.ValueOfGoodsCurrencyId;
                                        break;
                                    }
                                    case "PRFR": {
                                        _this.Quantity = Tools_2.ArrayTool.Sum(_this.ShipmentPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_2.AppTool.IsNullOrEmpty(d.ShipmentPayableParentId); }), "ExpectedAmount");
                                        _this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                                        break;
                                    }
                                    case "BCNT": {
                                        _this.IsByContainerType = true;
                                        _this.BuildByContainersItemsSource();
                                        break;
                                    }
                                    default: {
                                        var myGrouped = Tools_1.ShipmentTool.GetByPckageTypeGrouped(_this.ShipmentPM);
                                        var itemGrouped = myGrouped.filter(function (f) { return f.MeasurementId == _this.MeasurementId; })[0];
                                        if (itemGrouped != null) {
                                            _this.Quantity = itemGrouped.Quantity;
                                        }
                                        break;
                                    }
                                }
                                _this.UpdateInsideItemsSource_Measurement();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "MeasurementCode", {
        get: function () { return this.EntityPM.MeasurementCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "MeasurementShortName", {
        get: function () { return this.EntityPM.MeasurementShortName; },
        enumerable: true,
        configurable: true
    });
    ShipmentPayableItem.prototype.ApplyByContainersFields = function () {
        var _this = this;
        if (this.ByContainersItemsSource) {
            this.ByContainersItemsSource.forEach(function (item) {
                item.PrepaidCollectId = _this.PrepaidCollectId;
                item.CurrencyId = _this.CurrencyId;
                item.CurrencyCode = _this.CurrencyCode;
                item.Rate = _this.Rate;
            });
        }
    };
    ShipmentPayableItem.prototype.BuildByContainersItemsSource = function () {
        var _this = this;
        var byContainersItemsSource = [];
        var listGrouped = Tools_1.ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);
        listGrouped.forEach(function (itemGrouped) {
            var newEntity = new ShipmentPayablePM_1.ShipmentPayablePM(null);
            newEntity.ShipmentId = _this.ShipmentPM.Id;
            newEntity.ShipmentNumber = _this.ShipmentPM.ShipmentNumber;
            newEntity.Tenant = _this.ShipmentPM.Tenant;
            newEntity.ShipmentPayableLineStatusCode = "EMPT";
            newEntity.ShipmentPayableAmountTypeCode = "ACCU";
            newEntity.ShipmentPayableAmountTypeName = "Accrual";
            newEntity.ChargesTypeId = _this.EntityPM.ChargesTypeId;
            newEntity.ChargesTypeCode = _this.EntityPM.ChargesTypeCode;
            newEntity.ChargesTypeName = _this.EntityPM.ChargesTypeName;
            newEntity.ChargesGroupCode = _this.EntityPM.ChargesGroupCode;
            newEntity.DueTypeCode = _this.EntityPM.DueTypeCode;
            newEntity.DueTypeName = _this.EntityPM.DueTypeName;
            newEntity.IATACodeId = _this.EntityPM.IATACodeId;
            newEntity.VatTypeId = _this.EntityPM.VatTypeId;
            newEntity.PrepaidCollectId = _this.EntityPM.PrepaidCollectId;
            newEntity.VendorId = _this.EntityPM.VendorId;
            newEntity.VendorName = _this.EntityPM.VendorName;
            newEntity.CurrencyId = _this.EntityPM.CurrencyId;
            newEntity.CurrencyCode = _this.EntityPM.CurrencyCode;
            newEntity.Rate = _this.EntityPM.Rate;
            newEntity.ProfitCurrencyExchangeRate = _this.EntityPM.ProfitCurrencyExchangeRate;
            newEntity.CreateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
            newEntity.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            newEntity.UpdateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
            newEntity.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            newEntity.Quantity = itemGrouped.Quantity;
            newEntity.MeasurementId = itemGrouped.MeasurementId;
            newEntity.MeasurementCode = itemGrouped.MeasurementCode;
            newEntity.MeasurementShortName = itemGrouped.MeasurementShortName;
            var exsistingEntity = byContainersItemsSource.filter(function (f) { return f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId; })[0];
            if (exsistingEntity == null) {
                byContainersItemsSource.push(newEntity);
            }
            else {
                var acctEntity = byContainersItemsSource.filter(function (f) { return f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentPayableLineStatusCode == "ACCT" || f.ShipmentPayableLineStatusCode == "PACC"); })[0];
                var openEntity = byContainersItemsSource.filter(function (f) { return f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentPayableLineStatusCode == "EMPT" || f.ShipmentPayableLineStatusCode == "OAMT"); })[0];
                if (acctEntity == null) {
                    openEntity.Quantity = newEntity.Quantity;
                }
                else if (newEntity.Quantity > acctEntity.Quantity) {
                    if (openEntity == null) {
                        byContainersItemsSource.push(newEntity);
                    }
                    else {
                        openEntity.Quantity = newEntity.Quantity;
                    }
                }
            }
        });
        this.ByContainersItemsSource = byContainersItemsSource;
    };
    ShipmentPayableItem.prototype.SetPrepaidCollectId = function () {
        if (Tools_2.AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            if (this.EntityPM.ChargesGroupCode == "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.FreightPrepaidCollectId;
            }
            else if (this.EntityPM.ChargesGroupCode != "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.OtherPrepaidCollectId;
            }
        }
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "PrepaidCollectId", {
        get: function () { return this.EntityPM.PrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.PrepaidCollectId != newValue) {
                this.EntityPM.PrepaidCollectId = newValue;
                //this.ApplyByContainersFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "VendorId", {
        get: function () { return this.EntityPM.VendorId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.VendorId != newValue) {
                this.EntityPM.VendorId = newValue;
                if (newValue == null) {
                    this.EntityPM.VendorName = null;
                    this.UpdateInsideItemsSource_Vendor();
                }
                else {
                    var myService = new CardListService_1.CardListService();
                    myService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.EntityPM.VendorName = list.EnglishName;
                                _this.UpdateInsideItemsSource_Vendor();
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newVaule) {
            if (this.EntityPM.Notes != newVaule) {
                this.EntityPM.Notes = newVaule;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "RateDate", {
        get: function () { return this.rateDate; },
        set: function (value) {
            if (this.rateDate != value) {
                this.rateDate = value;
                this.RelativeRateDate = Tools_2.DateTool.GetRelativeRateDate(Tools_2.DateTool.GetCurrentDateAsUtc(), value, "ago");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "Rate", {
        get: function () { return this.EntityPM.Rate; },
        set: function (newVaule) {
            if (this.EntityPM.Rate != newVaule) {
                this.EntityPM.Rate = Tools_2.AppTool.Round(newVaule, 5);
                this.ComputeTotalAmountLocal();
                this.ApplyByContainersFields();
                this.UpdateInsideItemsSource_Rate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newVaule) {
            if (this.EntityPM.Quantity != newVaule) {
                this.EntityPM.Quantity = Tools_2.AppTool.Round(newVaule, 3);
                if (this.EntityPM.IsChargeBySteps) {
                    Tools_1.ShipmentTool.SetPayableUnitPriceBySteps(this.EntityPM, this.fatherComponent.BaseQuote);
                }
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "UnitPrice", {
        get: function () { return this.EntityPM.UnitPrice; },
        set: function (newVaule) {
            if (this.EntityPM.UnitPrice != newVaule) {
                this.EntityPM.UnitPrice = Tools_2.AppTool.Round(newVaule, 3);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ExpectedAmount", {
        get: function () { return this.EntityPM.ExpectedAmount; },
        set: function (ivalue) {
            var value = ivalue;
            if (value) {
                /* MinMax Tariff */
                if (this.MinAmount != null) {
                    if (value < this.MinAmount) {
                        value = this.MinAmount;
                    }
                }
                if (this.MaxAmount != null) {
                    if (value > this.MaxAmount) {
                        value = this.MaxAmount;
                    }
                }
                /* MinMax Quote */
                if (this.QuoteCostMinAmount != null) {
                    if (value < this.QuoteCostMinAmount) {
                        value = this.QuoteCostMinAmount;
                    }
                }
                if (this.QuoteCostMaxAmount != null) {
                    if (value > this.QuoteCostMaxAmount) {
                        value = this.QuoteCostMaxAmount;
                    }
                }
            }
            if (this.EntityPM.ExpectedAmount != value) {
                this.EntityPM.ExpectedAmount = Tools_2.AppTool.Round(value, 2);
                this.SetLineStatus();
                this.SetLineCells();
                this.ComputeUnitPrice();
                this.ComputeTotalAmountLocal();
                this.OnLineAmountChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ExpectedAmountLocal", {
        get: function () { return this.EntityPM.ExpectedAmountLocal; },
        set: function (newVaule) {
            var _this = this;
            if (this.EntityPM.ExpectedAmountLocal != newVaule) {
                this.EntityPM.ExpectedAmountLocal = Tools_2.AppTool.Round(newVaule, 2);
                this.SetLineSummary();
                this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }).forEach(function (item) {
                    item.SetLineSummary();
                });
                this.ComputeInsidePayablesData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ExpectedAmountInProfitCurrency", {
        get: function () { return this.EntityPM.ExpectedAmountInProfitCurrency; },
        set: function (newVaule) {
            if (this.EntityPM.ExpectedAmountInProfitCurrency != newVaule) {
                this.EntityPM.ExpectedAmountInProfitCurrency = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "OpenAmount", {
        get: function () { return this.EntityPM.OpenAmount; },
        set: function (newVaule) {
            if (this.EntityPM.OpenAmount != newVaule) {
                this.EntityPM.OpenAmount = Tools_2.AppTool.Round(newVaule, 2);
                this.SetOpenAmountCell();
                this.OpenAmountInLocalCurrency = newVaule * this.EntityPM.Rate;
                this.OpenAmountInProfitCurrency = this.OpenAmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate;
                var expe = this.EntityPM.ExpectedAmount == null ? 0 : this.EntityPM.ExpectedAmount;
                var acct = this.EntityPM.AccountedAmount == null ? 0 : this.EntityPM.AccountedAmount;
                var open = newVaule == null ? 0 : newVaule;
                this.CorrectionAmount = expe - acct - open;
                if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.CorrectionByUserId)) {
                    this.SetLineStatus();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "OpenAmountInLocalCurrency", {
        get: function () { return this.EntityPM.OpenAmountInLocalCurrency; },
        set: function (newVaule) {
            if (this.EntityPM.OpenAmountInLocalCurrency != newVaule) {
                this.EntityPM.OpenAmountInLocalCurrency = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "OpenAmountInProfitCurrency", {
        get: function () { return this.EntityPM.OpenAmountInProfitCurrency; },
        set: function (newVaule) {
            if (this.EntityPM.OpenAmountInProfitCurrency != newVaule) {
                this.EntityPM.OpenAmountInProfitCurrency = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "AccountedAmount", {
        get: function () { return this.EntityPM.AccountedAmount; },
        set: function (newVaule) {
            if (this.EntityPM.AccountedAmount != newVaule) {
                this.EntityPM.AccountedAmount = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "CorrectionAmount", {
        get: function () { return this.EntityPM.CorrectionAmount; },
        set: function (newVaule) {
            if (this.EntityPM.CorrectionAmount != newVaule) {
                this.EntityPM.CorrectionAmount = Tools_2.AppTool.Round(newVaule, 2);
                this.CorrectionByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.CorrectionDate = Tools_2.DateTool.GetCurrentDateAsUtc();
                if (this.fatherComponent != null) {
                    this.fatherComponent.ComputeShipmentFields();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "CorrectionAmountColor", {
        get: function () {
            var myResult = Tools_2.FontTool.Black;
            if (this.CorrectionAmount) {
                if (this.CorrectionAmount > 0) {
                    myResult = "Orange";
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "CorrectionByUserId", {
        get: function () { return this.EntityPM.CorrectionByUserId; },
        set: function (value) {
            if (this.EntityPM.CorrectionByUserId != value) {
                this.EntityPM.CorrectionByUserId = value;
                this.GetCorrectionUser();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "CorrectionDate", {
        get: function () { return this.EntityPM.CorrectionDate; },
        set: function (value) {
            if (this.EntityPM.CorrectionDate != value) {
                this.EntityPM.CorrectionDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "IsCorrectionTooltipVisible", {
        get: function () {
            var myResult = false;
            if (this.CorrectionAmount) {
                if (this.CorrectionAmount < 0 || this.CorrectionAmount > 0) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPayableItem.prototype.GetCorrectionUser = function () {
        var _this = this;
        if (Tools_2.AppTool.IsNullOrEmpty(this.CorrectionByUserId)) {
            this.CorrectionByUserName = null;
        }
        else {
            this.fatherComponent.myUserListService.getSingleFromCache(this.CorrectionByUserId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.CorrectionByUserName = list.EnglishName;
                    }
                }
            });
        }
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "MinAmount", {
        get: function () { return this.EntityPM.MinAmount; },
        set: function (newVaule) {
            if (this.EntityPM.MinAmount != newVaule) {
                this.EntityPM.MinAmount = Tools_2.AppTool.Round(newVaule, 2);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "MaxAmount", {
        get: function () { return this.EntityPM.MaxAmount; },
        set: function (newVaule) {
            if (this.EntityPM.MaxAmount != newVaule) {
                this.EntityPM.MaxAmount = Tools_2.AppTool.Round(newVaule, 2);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "QuoteCostMinAmount", {
        get: function () { return this.EntityPM.QuoteCostMinAmount; },
        set: function (newVaule) {
            if (this.EntityPM.QuoteCostMinAmount != newVaule) {
                this.EntityPM.QuoteCostMinAmount = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "QuoteCostMaxAmount", {
        get: function () { return this.EntityPM.QuoteCostMaxAmount; },
        set: function (newVaule) {
            if (this.EntityPM.QuoteCostMaxAmount != newVaule) {
                this.EntityPM.QuoteCostMaxAmount = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "ProfitCurrencyExchangeRate", {
        get: function () { return this.EntityPM.ProfitCurrencyExchangeRate; },
        set: function (newVaule) {
            if (this.EntityPM.ProfitCurrencyExchangeRate != newVaule) {
                this.EntityPM.ProfitCurrencyExchangeRate = Tools_2.AppTool.Round(newVaule, 5);
                this.ComputeTotalAmountInProfitCurrency();
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPayableItem.prototype.SetLineStatus = function () {
        Tools_1.ShipmentTool.SetPayableLineStatus(this.EntityPM);
        this.SetSatusTypeToolTip();
    };
    ShipmentPayableItem.prototype.ComputeUnitPrice = function () {
        if (!this.EntityPM.IsChargeBySteps) {
            var myResult = null;
            if (this.EntityPM.IsChargeBySteps) {
            }
            else {
                if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }
                    else {
                        myResult = this.EntityPM.ExpectedAmount / this.EntityPM.Quantity;
                    }
                }
                this.EntityPM.UnitPrice = Tools_2.AppTool.Round(myResult, 3);
            }
        }
    };
    ShipmentPayableItem.prototype.ComputeTotalAmount = function () {
        var iAmount = null;
        if (this.Quantity != null && this.UnitPrice != null) {
            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                var price = this.EntityPM.UnitPrice / 100;
                iAmount = this.EntityPM.Quantity * price;
            }
            else {
                iAmount = this.Quantity * this.UnitPrice;
            }
        }
        /* MinMax Tariff */
        if (iAmount != null) {
            if (this.MinAmount != null) {
                if (iAmount < this.MinAmount) {
                    iAmount = this.MinAmount;
                }
            }
            if (this.MaxAmount != null) {
                if (iAmount > this.MaxAmount) {
                    iAmount = this.MaxAmount;
                }
            }
        }
        /* MinMax Quote */
        if (iAmount != null) {
            if (this.QuoteCostMinAmount != null) {
                if (iAmount < this.QuoteCostMinAmount) {
                    iAmount = this.QuoteCostMinAmount;
                }
            }
            if (this.QuoteCostMaxAmount != null) {
                if (iAmount > this.QuoteCostMaxAmount) {
                    iAmount = this.QuoteCostMaxAmount;
                }
            }
        }
        this.EntityPM.ExpectedAmount = Tools_2.AppTool.Round(iAmount, 2);
        this.ComputeTotalAmountLocal();
        this.SetLineCells();
        this.OnLineAmountChanged();
        this.SetLineStatus();
    };
    ShipmentPayableItem.prototype.ComputeTotalAmountLocal = function () {
        var myResult = null;
        if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Rate != null) {
            myResult = Tools_2.AppTool.Round(this.EntityPM.ExpectedAmount * this.EntityPM.Rate, 2);
        }
        this.ExpectedAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
        this.ComputeInsidePayablesData();
    };
    ShipmentPayableItem.prototype.ComputeTotalAmountInProfitCurrency = function () {
        if (this.EntityPM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.ExpectedAmountInProfitCurrency = this.EntityPM.ExpectedAmount;
        }
        else {
            this.ExpectedAmountInProfitCurrency = (this.ExpectedAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }
        this.ComputeOtherAmounts();
        if (this.fatherComponent != null) {
            this.fatherComponent.ComputeShipmentFields();
        }
    };
    ShipmentPayableItem.prototype.ComputeOtherAmounts = function () {
        if (this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
            this.EntityPM.CorrectionAmount = 0;
            this.EntityPM.AccountedAmount = 0;
            this.EntityPM.AccountedAmountInLocalCurrency = 0;
            this.EntityPM.AccountedAmountInProfitCurrency = 0;
            this.EntityPM.OpenAmount = this.EntityPM.ExpectedAmount;
            this.EntityPM.OpenAmountInLocalCurrency = this.EntityPM.ExpectedAmountLocal;
            this.EntityPM.OpenAmountInProfitCurrency = this.EntityPM.ExpectedAmountInProfitCurrency;
            this.IsOpenAmountVisible = this.OpenAmount != null && this.OpenAmount != 0;
        }
        if (this.fatherComponent != null) {
            this.fatherComponent.ComputeShipmentFields();
        }
        this.SetLineSummary();
    };
    ShipmentPayableItem.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        var loadingDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = "Update Currency Rate";
        logWindow.WindowArgs = { CurrencyId: this.EntityPM.CurrencyId, CurrencyCode: this.EntityPM.CurrencyCode, Rate: this.EntityPM.Rate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.fatherComponent.AllRates = comp.RatesList;
                    _this.Rate = Tools_2.AppTool.Round(comp.Rate, 5);
                    _this.RateDate = comp.RateDate;
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "CreateDate", {
        // Line Summary
        get: function () { return this.EntityPM.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentPayableItem.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    ShipmentPayableItem.prototype.SetLineSummary = function () {
        var _this = this;
        if (Tools_2.AppTool.IsNullOrEmpty(this.UpdatedByUserName)) {
            this.fatherComponent.myUserListService.getSingleFromCache(this.EntityPM.CreatedByUserId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.CreatedByUserName = list.EnglishName;
                    }
                    else {
                        _this.fatherComponent.myUserListService.getSingle(_this.EntityPM.CreatedByUserId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    _this.CreatedByUserName = list.EnglishName;
                                }
                            }
                        });
                    }
                }
            });
            if (this.EntityPM.UpdateByUserId == this.EntityPM.CreatedByUserId) {
                this.UpdatedByUserName = this.CreatedByUserName;
            }
            else {
                this.fatherComponent.myUserListService.getSingleFromCache(this.EntityPM.UpdateByUserId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list) {
                            _this.UpdatedByUserName = list.EnglishName;
                        }
                        else {
                            _this.fatherComponent.myUserListService.getSingle(_this.EntityPM.UpdateByUserId).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    var list = myResponse.Result;
                                    if (list) {
                                        _this.UpdatedByUserName = list.EnglishName;
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
        var myReceivableSummary = 0;
        var myPayableSummary = 0;
        var myProfitSummary = 0;
        if (this.ShipmentPM.ShipmentReceivables.length > 0) {
            myReceivableSummary = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentReceivables.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }), "TotalAmountLocal");
        }
        if (this.ShipmentPM.ShipmentPayables.length > 0) {
            var openAmount = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }), "OpenAmountInLocalCurrency");
            var acctAmount = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }), "AccountedAmountInLocalCurrency");
            myPayableSummary = openAmount + acctAmount;
        }
        if (!Tools_2.ArrayTool.Contains(this.ShipmentPM.ShipmentPayables, this.EntityPM)) {
            if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.AccountedAmountInLocalCurrency)) {
                var entityOpenAmount = this.EntityPM.OpenAmountInLocalCurrency == null ? 0 : this.EntityPM.OpenAmountInLocalCurrency;
                var entityAcctAmount = this.EntityPM.AccountedAmountInLocalCurrency == null ? 0 : this.EntityPM.AccountedAmountInLocalCurrency;
                var entityAmount = entityOpenAmount + entityAcctAmount;
                myPayableSummary = myPayableSummary + entityAmount;
            }
        }
        this.ReceivableSummary = myReceivableSummary;
        this.PayableSummary = myPayableSummary;
        this.ProfitSummary = this.ReceivableSummary - this.PayableSummary;
    };
    Object.defineProperty(ShipmentPayableItem.prototype, "IsInvoicesTooltipOpened", {
        get: function () { return this.isInvoicesTooltipOpened; },
        set: function (value) {
            var _this = this;
            if (this.isInvoicesTooltipOpened != value) {
                this.isInvoicesTooltipOpened = value;
                if (value) {
                    if (!this.isInvoicesListLoaded) {
                        this.isInvoicesListLoaded = true;
                        this.fatherComponent.myDomainService.GetPayableInvoices(this.EntityPM.Id, this.EntityPM.ShipmentPayableParentId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.PayableInvoices = myResponse.Result;
                            }
                        });
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentPayableItem.prototype.BuildInsidePayables = function () {
        var _this = this;
        this.InsideItemsSource = [];
        this.EntityPM.ChildShipmentPayables.forEach(function (item) {
            var myConsoleShipmentPM = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (f) { return f.Id == item.ShipmentId; })[0];
            var insidePayable = new InsidePayableViewModel(item, myConsoleShipmentPM, _this.ShipmentPM.ProfitCurrencyId);
            _this.InsideItemsSource.push(insidePayable);
        });
        this.ComputeInsidePayablesTotals();
    };
    ShipmentPayableItem.prototype.UpdateInsideItemsSource_Rate = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(function (insidePayable) {
                insidePayable.Rate = _this.Rate;
            });
        }
    };
    ShipmentPayableItem.prototype.UpdateInsideItemsSource_Vendor = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(function (insidePayable) {
                insidePayable.VendorId = _this.VendorId;
                insidePayable.VendorName = _this.VendorName;
            });
        }
    };
    ShipmentPayableItem.prototype.UpdateInsideItemsSource_Currency = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(function (insidePayable) {
                insidePayable.CurrencyId = _this.CurrencyId;
                insidePayable.CurrencyCode = _this.CurrencyCode;
            });
        }
    };
    ShipmentPayableItem.prototype.UpdateInsideItemsSource_Measurement = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(function (insidePayable) {
                insidePayable.MeasurementId = _this.MeasurementId;
                insidePayable.MeasurementCode = _this.MeasurementCode;
                insidePayable.MeasurementShortName = _this.MeasurementShortName;
                insidePayable.OnMeasurementChanged();
            });
        }
    };
    ShipmentPayableItem.prototype.ComputeInsidePayablesData = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            var myService = new PackageTypeListService_1.PackageTypeListService();
            myService.getAllFromCache().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var AllPackageTypes = myResponse.Result;
                    if (AllPackageTypes == null) {
                        AllPackageTypes = [];
                    }
                    var _QuantityTotal = null;
                    var _Ratio = null;
                    var quantity = null;
                    var unitPrice = null;
                    _this.InsideItemsSource.forEach(function (item) {
                        switch (_this.MeasurementCode) {
                            case "VCBM": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "VolumeInCBM");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.VolumeInCBM;
                                break;
                            }
                            case "VOLU": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "Volume");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.Volume;
                                break;
                            }
                            case "GRWT": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "GrossWeight");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.GrossWeight;
                                break;
                            }
                            case "GWTN": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "GrossWeightPerTon");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.GrossWeightPerTon;
                                break;
                            }
                            case "CWKG": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "ChargeableWeightInKG");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.ChargeableWeightInKG;
                                break;
                            }
                            case "GWKG": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "GrossWeightInKG");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.GrossWeightInKG;
                                break;
                            }
                            case "QTY": {
                                if (_this.fatherComponent.IsLCLEntity) {
                                    _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "NumberOfPackages");
                                    _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                    unitPrice = _Ratio * _this.UnitPrice;
                                    quantity = item.NumberOfPackages;
                                }
                                else {
                                    _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "NumberOfContainers");
                                    _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                    unitPrice = _Ratio * _this.UnitPrice;
                                    quantity = item.NumberOfContainers;
                                }
                                break;
                            }
                            case "CHWT": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "ChargeableWeight");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.ChargeableWeight;
                                break;
                            }
                            case "BTEU": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "TEU");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.TEU;
                                break;
                            }
                            case "FIXD": {
                                _Ratio = _this.InsideItemsSource.length == 0 ? 0 : _this.Quantity / _this.InsideItemsSource.length;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = 1;
                                break;
                            }
                            case "PRVL": {
                                unitPrice = _this.UnitPrice;
                                quantity = item.ValueOfGoods;
                                break;
                            }
                            case "PRFR": {
                                unitPrice = _this.UnitPrice;
                                quantity = item.FreightPayablesAmount;
                                break;
                            }
                            default: {
                                if (_this.fatherComponent.IsFCLEntity) {
                                    var list = AllPackageTypes.filter(function (f) { return f.MeasurementId == _this.MeasurementId; })[0];
                                    if (list) {
                                        var houseRecord = item.ShipmentPM;
                                        if (houseRecord) {
                                            var fclData = houseRecord.FCLDataList.filter(function (f) { return f.Id == list.Id; })[0];
                                            if (fclData) {
                                                quantity = fclData.Quantity;
                                            }
                                        }
                                        unitPrice = _this.UnitPrice;
                                    }
                                }
                                else {
                                    // Groupage by Chargeable
                                    _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "ChargeableWeight");
                                    _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                    unitPrice = _Ratio * _this.UnitPrice;
                                    quantity = item.ChargeableWeight;
                                }
                                break;
                            }
                        }
                        if (Tools_2.AppTool.IsNullOrEmpty(unitPrice)) {
                            unitPrice = 0;
                        }
                        if (Tools_2.AppTool.IsNullOrEmpty(quantity)) {
                            quantity = 0;
                        }
                        item.EntityPM.UnitPrice = Tools_2.AppTool.Round(unitPrice, 3);
                        item.EntityPM.Quantity = Tools_2.AppTool.Round(quantity, 3);
                        var expectedAmount = quantity * unitPrice;
                        if (_this.MeasurementCode == "PRVL" || _this.MeasurementCode == "PRFR") {
                            expectedAmount = quantity * unitPrice / 100;
                        }
                        var expectedAmountLocal = expectedAmount * _this.Rate;
                        var expectedAmountInProfitCurrency = expectedAmountLocal / _this.ProfitCurrencyExchangeRate;
                        item.EntityPM.ExpectedAmount = Tools_2.AppTool.Round(expectedAmount, 3);
                        item.EntityPM.ExpectedAmountLocal = Tools_2.AppTool.Round(expectedAmountLocal, 3);
                        item.EntityPM.ExpectedAmountInProfitCurrency = Tools_2.AppTool.Round(expectedAmountInProfitCurrency, 3);
                        item.EntityPM.OpenAmount = item.EntityPM.ExpectedAmount;
                        item.EntityPM.OpenAmountInLocalCurrency = item.EntityPM.ExpectedAmountLocal;
                        item.EntityPM.OpenAmountInProfitCurrency = item.EntityPM.ExpectedAmountInProfitCurrency;
                        item.EntityPM.AccountedAmount = 0;
                        item.EntityPM.AccountedAmountInLocalCurrency = 0;
                        item.EntityPM.AccountedAmountInProfitCurrency = 0;
                    });
                    _this.ComputeInsidePayablesTotals();
                }
            });
        }
    };
    ShipmentPayableItem.prototype.ComputeInsidePayablesTotals = function () {
        this.SumOfQuantity = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "Quantity");
        this.SumOfAmount = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "ExpectedAmount");
        this.SumOfAmountLocal = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "ExpectedAmountLocal");
        this.SetExpectedAmountCell();
    };
    ShipmentPayableItem.prototype.SetQuantity = function () {
        var _this = this;
        var result = null;
        switch (this.EntityPM.MeasurementCode) {
            case "GRWT": {
                result = this.ShipmentPM.GrossWeight;
                break;
            }
            case "CHWT": {
                result = this.ShipmentPM.ChargeableWeight;
                break;
            }
            case "VOLU": {
                result = this.ShipmentPM.Volume;
                break;
            }
            case "BTEU": {
                result = this.ShipmentPM.TEU;
                break;
            }
            case "FIXD": {
                result = 1;
                break;
            }
            case "PRVL": {
                result = this.ShipmentPM.ValueOfGoods;
                break;
            }
            case "PRFR": {
                result = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_2.AppTool.IsNullOrEmpty(d.ShipmentPayableParentId); }), "ExpectedAmount");
                break;
            }
            case "GWTN": {
                result = this.ShipmentPM.GrossWeightPerTon;
                break;
            }
            case "QTY": {
                result = this.fatherComponent.IsLCLEntity ? this.ShipmentPM.NumberOfPackages : this.ShipmentPM.NumberOfContainers;
                break;
            }
            case "CWKG": {
                result = this.ShipmentPM.ChargeableWeightInKG;
                break;
            }
            case "GWKG": {
                result = this.ShipmentPM.GrossWeightInKG;
                break;
            }
            case "VCBM": {
                result = this.ShipmentPM.VolumeInCBM;
                break;
            }
            case "BCNT": {
                break;
            }
            default: {
                if (!Tools_2.AppTool.IsNullOrEmpty(this.MeasurementId)) {
                    var allBCNTGrouped = Tools_1.ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);
                    var itemGrouped = allBCNTGrouped.filter(function (f) { return f.MeasurementId == _this.MeasurementId; })[0];
                    if (itemGrouped != null) {
                        result = itemGrouped.Quantity;
                    }
                }
                break;
            }
        }
        this.Quantity = result;
    };
    ShipmentPayableItem.prototype.OnLineAmountChanged = function () {
        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }
    };
    return ShipmentPayableItem;
}(BaseComponent_1.BaseComponent));
exports.ShipmentPayableItem = ShipmentPayableItem;
var InsidePayableViewModel = /** @class */ (function () {
    function InsidePayableViewModel(entityPM, myConsoleShipmentPM, profitCurrencyId) {
        this.SatusTypeToolTip = null;
        this.EntityPM = entityPM;
        this.ShipmentPM = myConsoleShipmentPM;
        this.ProfitCurrencyId = profitCurrencyId;
        this.SetSatusTypeToolTip();
    }
    InsidePayableViewModel.prototype.SetSatusTypeToolTip = function () {
        var myResult;
        switch (this.EntityPM.ShipmentPayableLineStatusCode) {
            case "APPD": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.Approved");
                break;
            }
            case "NOIN": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.NoInvoiceNeeded");
                break;
            }
            case "OAMT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.OpenAmount");
                break;
            }
            case "ACCT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.Accounted");
                break;
            }
            case "PACC": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Payables.PartiallyAccounted");
                break;
            }
            default: {
                break;
            }
        }
        this.SatusTypeToolTip = myResult;
    };
    Object.defineProperty(InsidePayableViewModel.prototype, "TEU", {
        // Shipment Properties
        get: function () { return this.ShipmentPM.TEU; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "Volume", {
        get: function () { return this.ShipmentPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "GrossWeight", {
        get: function () { return this.ShipmentPM.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ChargeableWeight", {
        get: function () { return this.ShipmentPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "GrossWeightPerTon", {
        get: function () { return this.ShipmentPM.GrossWeightPerTon; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ValueOfGoods", {
        get: function () { return this.ShipmentPM.ValueOfGoods; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "FreightPayablesAmount", {
        get: function () { return this.ShipmentPM.FreightPayablesAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "FreightReceivablesAmount", {
        get: function () { return this.ShipmentPM.FreightReceivablesAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "NumberOfPackages", {
        get: function () { return this.ShipmentPM.NumberOfPackages; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "NumberOfContainers", {
        get: function () { return this.ShipmentPM.NumberOfContainers; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ChargeableWeightInKG", {
        get: function () { return this.ShipmentPM.ChargeableWeightInKG; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "GrossWeightInKG", {
        get: function () { return this.ShipmentPM.GrossWeightInKG; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "VolumeInCBM", {
        get: function () { return this.ShipmentPM.VolumeInCBM; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ShipmentId", {
        // Payable Properties
        get: function () { return this.EntityPM.ShipmentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newVaule) {
            if (this.EntityPM.Notes != newVaule) {
                this.EntityPM.Notes = newVaule;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "VendorId", {
        get: function () { return this.EntityPM.VendorId; },
        set: function (value) {
            if (this.EntityPM.VendorId != value) {
                this.EntityPM.VendorId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (value) {
            if (this.EntityPM.VendorName != value) {
                this.EntityPM.VendorName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (value) {
            if (this.EntityPM.MeasurementId != value) {
                this.EntityPM.MeasurementId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "MeasurementCode", {
        get: function () { return this.EntityPM.MeasurementCode; },
        set: function (value) {
            if (this.EntityPM.MeasurementCode != value) {
                this.EntityPM.MeasurementCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "MeasurementShortName", {
        get: function () { return this.EntityPM.MeasurementShortName; },
        set: function (value) {
            if (this.EntityPM.MeasurementShortName != value) {
                this.EntityPM.MeasurementShortName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "CurrencyCode", {
        get: function () { return this.EntityPM.CurrencyCode; },
        set: function (value) {
            if (this.EntityPM.CurrencyCode != value) {
                this.EntityPM.CurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "Rate", {
        get: function () { return this.EntityPM.Rate; },
        set: function (value) {
            if (this.EntityPM.Rate != value) {
                this.EntityPM.Rate = Tools_2.AppTool.Round(value, 5);
                this.ComputeTotalAmountLocal();
            }
        },
        enumerable: true,
        configurable: true
    });
    InsidePayableViewModel.prototype.OnMeasurementChanged = function () {
        var myQuantity = null;
        switch (this.MeasurementCode) {
            case "CWKG": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.ChargeableWeightInKG;
                }
                break;
            }
            case "GWKG": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeightInKG;
                }
                break;
            }
            case "VCBM": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.VolumeInCBM;
                }
                break;
            }
            case "GRWT": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeight;
                }
                break;
            }
            case "GWTN": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.GrossWeightPerTon;
                }
                break;
            }
            case "GWTN": {
                if (this.ShipmentPM) {
                    if (this.ShipmentPM.IsLCL) {
                        myQuantity = this.ShipmentPM.NumberOfPackages;
                    }
                    else {
                        myQuantity = this.ShipmentPM.NumberOfContainers;
                    }
                }
                break;
            }
            case "CHWT": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.ChargeableWeight;
                }
                break;
            }
            case "VOLU": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.Volume;
                }
                break;
            }
            case "BTEU": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.TEU;
                }
                break;
            }
            case "PRVL": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.ValueOfGoods;
                }
                break;
            }
            case "PRFR": {
                if (this.ShipmentPM) {
                    myQuantity = this.ShipmentPM.FreightReceivablesAmount;
                }
                break;
            }
            default: {
                myQuantity = 1;
                break;
            }
        }
        this.Quantity = myQuantity;
    };
    Object.defineProperty(InsidePayableViewModel.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (value) {
            if (this.EntityPM.Quantity != value) {
                this.EntityPM.Quantity = Tools_2.AppTool.Round(value, 2);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "UnitPrice", {
        get: function () { return this.EntityPM.UnitPrice; },
        set: function (value) {
            if (this.EntityPM.UnitPrice != value) {
                this.EntityPM.UnitPrice = Tools_2.AppTool.Round(value, 3);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ExpectedAmount", {
        get: function () { return this.EntityPM.ExpectedAmount; },
        set: function (value) {
            if (this.EntityPM.ExpectedAmount != value) {
                this.EntityPM.ExpectedAmount = Tools_2.AppTool.Round(value, 2);
                this.ComputeUnitPrice();
                this.ComputeTotalAmountLocal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "ExpectedAmountLocal", {
        get: function () { return this.EntityPM.ExpectedAmountLocal; },
        set: function (value) {
            if (this.EntityPM.ExpectedAmountLocal != value) {
                this.EntityPM.ExpectedAmountLocal = Tools_2.AppTool.Round(value, 2);
                this.ComputeTotalAmountInProfitCurrency();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "MinAmount", {
        get: function () { return this.EntityPM.MinAmount; },
        set: function (newVaule) {
            if (this.EntityPM.MinAmount != newVaule) {
                this.EntityPM.MinAmount = Tools_2.AppTool.Round(newVaule, 2);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "MaxAmount", {
        get: function () { return this.EntityPM.MaxAmount; },
        set: function (newVaule) {
            if (this.EntityPM.MaxAmount != newVaule) {
                this.EntityPM.MaxAmount = Tools_2.AppTool.Round(newVaule, 2);
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "QuoteCostMinAmount", {
        get: function () { return this.EntityPM.QuoteCostMinAmount; },
        set: function (newVaule) {
            if (this.EntityPM.QuoteCostMinAmount != newVaule) {
                this.EntityPM.QuoteCostMinAmount = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsidePayableViewModel.prototype, "QuoteCostMaxAmount", {
        get: function () { return this.EntityPM.QuoteCostMaxAmount; },
        set: function (newVaule) {
            if (this.EntityPM.QuoteCostMaxAmount != newVaule) {
                this.EntityPM.QuoteCostMaxAmount = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    InsidePayableViewModel.prototype.SetLineStatus = function () {
        Tools_1.ShipmentTool.SetPayableLineStatus(this.EntityPM);
        this.SetSatusTypeToolTip();
    };
    InsidePayableViewModel.prototype.ComputeUnitPrice = function () {
        if (!this.EntityPM.IsChargeBySteps) {
            var myResult = null;
            if (this.EntityPM.IsChargeBySteps) {
            }
            else {
                if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }
                    else {
                        myResult = this.EntityPM.ExpectedAmount / this.EntityPM.Quantity;
                    }
                }
                this.EntityPM.UnitPrice = Tools_2.AppTool.Round(myResult, 3);
            }
        }
    };
    InsidePayableViewModel.prototype.ComputeTotalAmount = function () {
        var iAmount = null;
        this.SetLineStatus();
        if (this.Quantity != null && this.UnitPrice != null) {
            iAmount = this.Quantity * this.UnitPrice;
            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                iAmount = this.Quantity * this.UnitPrice / 100;
            }
        }
        /* MinMax Tariff */
        if (iAmount != null) {
            if (this.MinAmount != null) {
                if (iAmount < this.MinAmount) {
                    iAmount = this.MinAmount;
                }
            }
            if (this.MaxAmount != null) {
                if (iAmount > this.MaxAmount) {
                    iAmount = this.MaxAmount;
                }
            }
        }
        /* MinMax Quote */
        if (iAmount != null) {
            if (this.QuoteCostMinAmount != null) {
                if (iAmount < this.QuoteCostMinAmount) {
                    iAmount = this.QuoteCostMinAmount;
                }
            }
            if (this.QuoteCostMaxAmount != null) {
                if (iAmount > this.QuoteCostMaxAmount) {
                    iAmount = this.QuoteCostMaxAmount;
                }
            }
        }
        this.EntityPM.ExpectedAmount = Tools_2.AppTool.Round(iAmount, 2);
        this.ComputeTotalAmountLocal();
    };
    InsidePayableViewModel.prototype.ComputeTotalAmountLocal = function () {
        var myResult = null;
        if (this.EntityPM.ExpectedAmount != null && this.EntityPM.Rate != null) {
            myResult = Tools_2.AppTool.Round(this.EntityPM.ExpectedAmount * this.EntityPM.Rate, 2);
        }
        this.ExpectedAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
    };
    InsidePayableViewModel.prototype.ComputeTotalAmountInProfitCurrency = function () {
        if (this.EntityPM.CurrencyId == this.ProfitCurrencyId) {
            this.EntityPM.ExpectedAmountInProfitCurrency = this.EntityPM.ExpectedAmount;
        }
        else {
            this.EntityPM.ExpectedAmountInProfitCurrency = (this.ExpectedAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }
        this.ComputeOtherAmounts();
    };
    InsidePayableViewModel.prototype.ComputeOtherAmounts = function () {
        if (this.EntityPM.ShipmentPayableLineStatusCode == "EMPT" || this.EntityPM.ShipmentPayableLineStatusCode == "OAMT") {
            this.EntityPM.CorrectionAmount = 0;
            this.EntityPM.AccountedAmount = 0;
            this.EntityPM.AccountedAmountInLocalCurrency = 0;
            this.EntityPM.AccountedAmountInProfitCurrency = 0;
            if (this.EntityPM.OpenAmount != this.EntityPM.ExpectedAmount) {
                this.EntityPM.OpenAmount = this.EntityPM.ExpectedAmount;
            }
            else {
                this.EntityPM.OpenAmountInLocalCurrency = this.EntityPM.OpenAmount * this.EntityPM.Rate;
                this.EntityPM.OpenAmountInProfitCurrency = this.EntityPM.OpenAmountInLocalCurrency / this.EntityPM.ProfitCurrencyExchangeRate;
            }
        }
    };
    return InsidePayableViewModel;
}());
exports.InsidePayableViewModel = InsidePayableViewModel;
//# sourceMappingURL=PayablesTabComponent.js.map