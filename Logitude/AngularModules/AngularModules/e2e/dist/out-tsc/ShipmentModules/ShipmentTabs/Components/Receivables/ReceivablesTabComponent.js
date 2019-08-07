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
var ShipmentReceivablePM_1 = require("../../../../Shipment/EntityPMs/ShipmentReceivablePM");
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
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var QuotePMService_1 = require("../../../../Quote/Services/StandardPMs/QuotePMService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ReceivablesTabComponent = /** @class */ (function (_super) {
    __extends(ReceivablesTabComponent, _super);
    function ReceivablesTabComponent(entityArgs, entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityResourceService = entityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = null;
        _this.DataContext = _this;
        _this.IsLCLEntity = false;
        _this.IsFCLEntity = false;
        _this.ShipmentLevelCode = null;
        _this.ProfitCurrencyId = null;
        _this.ProfitCurrencyCode = null;
        _this.LocalCurrencyId = null;
        _this.LocalCurrencyCode = null;
        _this.IsResourcesReady = false;
        _this.IsCustomsInvoiceVisible = false;
        _this.IsCustomsCreditVisible = false;
        _this.IsCustomsToggleVisible = false;
        _this.IsProrateReceivablesVisible = false;
        _this.IsEditExchangeRateVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        // Load RequiredData
        _this.BaseQuote = null;
        _this.AllRates = [];
        _this.SelectedRow = null;
        // SetUIProperties
        _this.IsEditingEnabled = true;
        // ItemsSource
        _this.InvoiceColumnWidth = 100;
        // Summary
        _this.OpenReceivablesCount = 0;
        _this.InvoicesList = [];
        _this.CreditNotesList = [];
        // Profit
        _this.IsProfitAreaVisible = false;
        _this.IsCurrencyFilterVisible = false;
        _this.IsProfitRateVisible = false;
        _this.IsByLocalCurrency = false;
        _this.SelectedCurrencyCode = null;
        _this.ProfitRate = "N/A";
        _this.Profit = null;
        _this.Defference = null;
        _this.ProfitInSelectedCurrencyText = "N/A";
        _this.PayablesInSelectedCurrencyText = "N/A";
        _this.ReceivablesInSelectedCurrencyText = "N/A";
        _this.DefferenceInSelectedCurrencyText = "";
        _this.estimateProfitInSelectedCurrency = null;
        // Generate
        _this.IsGenerateButtonsVisible = false;
        _this.IsGenerateFromQuoteChargesEnabled = false;
        _this.IsNoChargesTextVisibil = false;
        _this.SavingRequested = false;
        _this.SavingRequestCode = null;
        _this.SavingRequestParam = null;
        _this.UpdateQuantitiesMessageWidth = 0;
        _this.IsUpdateQuantitiesVisible = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.OriginShipment = entityArgs.OriginEntity;
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.ShipmentLevelCode = _this.EntityPM.ShipmentLevelCode;
        _this.ProfitCurrencyId = _this.EntityPM.ProfitCurrencyId;
        _this.ProfitCurrencyCode = _this.EntityPM.ProfitCurrencyCode;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        _this.Listen();
        _this.Initialize();
        _this.SetEditEnabled();
        _this.LoadRequiredData();
        return _this;
    }
    ReceivablesTabComponent.prototype.Initialize = function () {
        if (this.ShipmentLevelCode == "C") {
            this.IsProrateReceivablesVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        if (this.ShipmentLevelCode == "D" || this.ShipmentLevelCode == "H") {
            if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                    this.IsCustomsToggleVisible = true;
                }
            }
        }
        if (this.IsCustomsToggleVisible) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "CREATENEWCUSTOMSINVOICE")) {
                this.IsCustomsInvoiceVisible = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "CREATENEWCUSTOMSCREDIT")) {
                this.IsCustomsCreditVisible = true;
            }
        }
    };
    ReceivablesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "ReceivablesGenerated") {
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
                if (tabCode == "SHRE" || tabCode == "JHRE") {
                    _this.BuildProfitData();
                    _this.CheckUpdateQuantities();
                }
            });
        }
    };
    ReceivablesTabComponent.prototype.ngOnDestroy = function () {
        Tools_2.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_2.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_2.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_2.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ReceivablesTabComponent.prototype.LoadRequiredData = function () {
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
    ReceivablesTabComponent.prototype.LoadOtherRequiredData = function () {
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
    ReceivablesTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.IsLCLEntity = Tools_2.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = Tools_2.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.entityResourceService.getEntityResourceByTableName("ShipmentReceivable").subscribe(function (res) {
                _this.IsResourcesReady = true;
                _this.SetLabels();
                _this.SetUIProperties();
                _this.BuildItemsSource();
                _this.BuildSummaryData();
                _this.InitializeProfitArea();
                _this.SetGenerateButtons();
            });
        }
    };
    ReceivablesTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    ReceivablesTabComponent.prototype.OnRowLoaded = function (Row) {
        var isExpandaple = false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (Row) {
                var item = Row.rowData;
                if (item) {
                    if (item.EntityPM.ChildShipmentReceivables.length > 0) {
                        if (this.ProrateReceivables) {
                            isExpandaple = true;
                        }
                    }
                }
                Row.SetExpandaple(isExpandaple);
            }
        }
    };
    ReceivablesTabComponent.prototype.SetLabels = function () {
        this.AmountLocalColumnHeader = TextCodeTranslator_1.TextCodeTranslator.Translate('Shipment.O.Receivables.AmountLocal').replace('%LocalCurrencyCode', SessionLocator_1.SessionLocator.TenantPM.CurrencyCode);
    };
    ReceivablesTabComponent.prototype.SetEditEnabled = function () {
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
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.AllowReceivables")) {
                        isEditingEnabled = true;
                    }
                }
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
    };
    ReceivablesTabComponent.prototype.SetUIProperties = function () {
        this.SetEditEnabled();
    };
    ReceivablesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        var invoiceColumnWidth = 100;
        var itemsCollection = [];
        this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
            var itemComponent = new ShipmentReceivableItem(item, _this);
            if (!Tools_2.AppTool.IsNullOrEmpty(item.ARInvoiceId)) {
                var textWidth = Tools_2.AppTool.GetTextWidth(itemComponent.InvoiceNumber + itemComponent.InvoiceStatus, 11) + 10;
                if (textWidth > invoiceColumnWidth) {
                    invoiceColumnWidth = textWidth;
                }
            }
            itemsCollection.push(itemComponent);
        });
        this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
            var itemComponent = new ShipmentReceivableItem(item, _this);
            if (!Tools_2.AppTool.IsNullOrEmpty(item.ARInvoiceId)) {
                var textWidth = Tools_2.AppTool.GetTextWidth(itemComponent.InvoiceNumber + itemComponent.InvoiceStatus, 11) + 10;
                if (textWidth > invoiceColumnWidth) {
                    invoiceColumnWidth = textWidth;
                }
            }
            itemsCollection.push(itemComponent);
        });
        this.SetGenerateButtons();
        this.InvoiceColumnWidth = invoiceColumnWidth;
        this.ItemsSource.InsertCollection(itemsCollection);
    };
    ReceivablesTabComponent.prototype.BuildSummaryData = function () {
        this.OpenReceivablesCount = this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" || f.ShipmentReceivableLineStatusCode == "DRFT"; }).length;
        this.InvoicesList = this.EntityPM.ShipmentARInvoices.filter(function (f) { return f.InvoiceTypeCode == "IN" || f.InvoiceTypeCode == "MN" || f.InvoiceTypeCode == "CI"; });
        this.CreditNotesList = this.EntityPM.ShipmentARInvoices.filter(function (f) { return f.InvoiceTypeCode == "CD" || f.InvoiceTypeCode == "CC"; });
    };
    ReceivablesTabComponent.prototype.InitializeProfitArea = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Profit")) {
            this.IsProfitAreaVisible = true;
        }
        this.IsCurrencyFilterVisible = SessionLocator_1.SessionLocator.LocalCurrencyId == this.EntityPM.ProfitCurrencyId ? false : true;
        this.SelectedCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.BuildProfitData();
    };
    ReceivablesTabComponent.prototype.GetPayablesInSelectedCurrency = function () {
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
    ReceivablesTabComponent.prototype.GetReceivablesInSelectedCurrency = function () {
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
    ReceivablesTabComponent.prototype.GetProfitInSelectedCurrency = function () {
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
    ReceivablesTabComponent.prototype.GetEstimateProfitInSelectedCurrency = function () {
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
    ReceivablesTabComponent.prototype.OnSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }
        else {
            this.IsByLocalCurrency = false;
        }
        this.BuildProfitData();
    };
    ReceivablesTabComponent.prototype.BuildProfitData = function () {
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
        this.Profit = myProfitInSelectedCurrency;
        this.Defference = myDefferenceInSelectedCurrency;
    };
    ReceivablesTabComponent.prototype.UpdateCurrencyRateClicked = function () {
        var _this = this;
        var loadingDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.UpdateCurrencyRate");
        logWindow.WindowArgs = { CurrencyId: this.EntityPM.ProfitCurrencyId, CurrencyCode: this.EntityPM.ProfitCurrencyCode, Rate: this.EntityPM.ProfitExchangeRate, Date: loadingDate };
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.AllRates = comp.RatesList;
                    _this.ProfitExchangeRate = Tools_2.AppTool.Round(comp.Rate, 5);
                }
            });
        });
        logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
    };
    ReceivablesTabComponent.prototype.OnProfitExchangeRateChanged = function () {
        var _this = this;
        this.EntityPM.ShipmentReceivables.forEach(function (item) {
            item.ProfitCurrencyExchangeRate = _this.ProfitExchangeRate;
            if (item.CurrencyId == _this.ProfitCurrencyId) {
                item.AmountInProfitCurrency = item.TotalAmount;
            }
            else {
                var profitAmount = item.TotalAmountLocal / item.ProfitCurrencyExchangeRate;
                item.AmountInProfitCurrency = Tools_2.AppTool.Round(profitAmount, 2);
            }
        });
        this.EntityPM.ShipmentPayables.forEach(function (item) {
            item.ProfitCurrencyExchangeRate = _this.ProfitExchangeRate;
            if (item.CurrencyId == _this.ProfitCurrencyId) {
                item.ExpectedAmountInProfitCurrency = item.ExpectedAmount;
            }
            else {
                var profitAmount = item.ExpectedAmountLocal / item.ProfitCurrencyExchangeRate;
                item.ExpectedAmountInProfitCurrency = Tools_2.AppTool.Round(profitAmount, 2);
            }
            // Other Amounts
            if (item.ShipmentPayableLineStatusCode == "EMPT" || item.ShipmentPayableLineStatusCode == "OAMT") {
                item.CorrectionAmount = 0;
                item.AccountedAmount = 0;
                item.AccountedAmountInLocalCurrency = 0;
                item.AccountedAmountInProfitCurrency = 0;
                if (item.OpenAmount != item.ExpectedAmount) {
                    item.OpenAmount = item.ExpectedAmount;
                    item.OpenAmountInLocalCurrency = item.OpenAmount * item.Rate;
                    item.OpenAmountInProfitCurrency = item.OpenAmountInLocalCurrency / item.ProfitCurrencyExchangeRate;
                    var expe = item.ExpectedAmount == null ? 0 : item.ExpectedAmount;
                    var acct = item.AccountedAmount == null ? 0 : item.AccountedAmount;
                    var open = item.OpenAmount == null ? 0 : item.OpenAmount;
                    var correction = expe - acct - open;
                    item.CorrectionAmount = Tools_2.AppTool.Round(correction, 2);
                    item.CorrectionByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    item.CorrectionDate = Tools_2.DateTool.GetCurrentDateAsUtc();
                    if (!Tools_2.AppTool.IsNullOrEmpty(item.CorrectionByUserId)) {
                        Tools_1.ShipmentTool.SetPayableLineStatus(item);
                    }
                }
                else {
                    item.OpenAmountInLocalCurrency = item.OpenAmount * item.Rate;
                    item.OpenAmountInProfitCurrency = item.OpenAmountInLocalCurrency / item.ProfitCurrencyExchangeRate;
                }
            }
        });
        this.ComputeShipmentFields();
        this.BuildItemsSource();
    };
    ReceivablesTabComponent.prototype.ShowProfitClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 750;
        logWindow.ShowCloseButton = true;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Profit.ProfitDetails");
        logWindow.WindowArgs = { ShipmentPM: this.EntityPM, IsByLocalCurrency: this.IsByLocalCurrency, IsCurrencyFilterVisible: this.IsCurrencyFilterVisible };
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Profit/ProfitComponent');
    };
    Object.defineProperty(ReceivablesTabComponent.prototype, "EstimateProfitInSelectedCurrency", {
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
    Object.defineProperty(ReceivablesTabComponent.prototype, "ProfitExchangeRate", {
        get: function () { return this.EntityPM.ProfitExchangeRate; },
        set: function (value) {
            if (this.EntityPM.ProfitExchangeRate != value) {
                this.EntityPM.ProfitExchangeRate = Tools_2.AppTool.Round(value, 5);
                this.OnProfitExchangeRateChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    ReceivablesTabComponent.prototype.SetGenerateButtons = function () {
        var isGenerateButtonsVisible = false;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            isGenerateButtonsVisible = false;
        }
        else if (this.EntityPM.ShipmentReceivables.length > 0) {
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
    ReceivablesTabComponent.prototype.GenerateClicked = function (myCommandCode) {
        var _this = this;
        var isConfirming = false;
        switch (myCommandCode) {
            case "ATDS": {
                isConfirming = true;
                break;
            }
            case "QTRC": {
                isConfirming = true;
                break;
            }
            case "QTRP": {
                isConfirming = true;
                break;
            }
        }
        if (isConfirming && this.EntityPM.ShipmentPackages.length == 0) {
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
    ReceivablesTabComponent.prototype.StartGenerating = function (myCommandCode) {
        var _this = this;
        switch (myCommandCode) {
            case "ATDS": {
                // AutoDisplay                
                var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GenerateReceivablesAutoDisplay();
                this.OnEntityDataGenerated();
                break;
            }
            case "QTRC": {
                // FromQuoteReceivablesOnly
                var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GenerateReceivablesFromQuote(this.BaseQuote);
                this.OnEntityDataGenerated();
                break;
            }
            case "QTRP": {
                // FromQuoteReceivablesAndPayables
                var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                Generator.GeneratePayablesFromQuote(this.BaseQuote);
                Generator.GenerateReceivablesFromQuote(this.BaseQuote);
                this.OnEntityDataGenerated();
                this.CurrentSession.FireEvent("PayablesGenerated");
                break;
            }
            case "PAYB": {
                // FromPayables
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.IsFillScreen = true;
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.GenerateReceivablesFromPayables");
                logWindow.WindowArgs = { EntityPM: this.EntityPM, QuotePM: this.BaseQuote };
                logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Payables/PayablesComponent');
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.OnEntityDataGenerated();
                    }
                });
                break;
            }
            case "QTLS": {
                // FromQuoteList
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 950;
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.GenerateFromQuotesList");
                logWindow.WindowArgs = { EntityPM: this.EntityPM, AllRates: this.AllRates };
                logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Windows/Quotes/QuotesComponent');
                logWindow.ComponentLoaded.subscribe(function (comp) {
                    logWindow.WindowClosed.subscribe(function (s) {
                        if (s) {
                            _this.BaseQuote = comp.BaseQuote;
                            _this.OnEntityDataGenerated();
                            _this.CurrentSession.FireEvent("PayablesGenerated");
                        }
                    });
                });
                break;
            }
            case "ORGN": {
                // FromOriginShipment
                if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.OriginShipmentId)) {
                    if (this.OriginShipment) {
                        var Generator = new Tools_1.ShipmentGenerator(this.EntityPM, this.AllRates);
                        Generator.GenerateReceivablesFromOriginShipment(this.OriginShipment);
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
                                Generator.GenerateReceivablesFromOriginShipment(_this.OriginShipment);
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
    ReceivablesTabComponent.prototype.OnEntityDataGenerated = function () {
        this.BuildItemsSource();
        this.ComputeShipmentFields();
    };
    ReceivablesTabComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    ReceivablesTabComponent.prototype.AddReceivable = function () {
        var newItem = new ShipmentReceivablePM_1.ShipmentReceivablePM(null);
        newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newItem.ShipmentId = this.EntityPM.Id;
        newItem.ShipmentNumber = this.EntityPM.ShipmentNumber;
        newItem.CreateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        newItem.UpdateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
        newItem.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newItem.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newItem.ShipmentReceivableLineStatusCode = "EMPT";
        newItem.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
        var itemComponent = new ShipmentReceivableItem(newItem, this, true);
        this.RunAddEditReceivable(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.AddReceivable"));
    };
    ;
    ReceivablesTabComponent.prototype.EditReceivable = function (itemComponent) {
        this.RunAddEditReceivable(itemComponent, TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.EditReceivable"));
    };
    ReceivablesTabComponent.prototype.RunAddEditReceivable = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/Receivables/AddEditReceivableComponent');
    };
    ReceivablesTabComponent.prototype.DeleteItem = function (itemComponent) {
        var _this = this;
        if (itemComponent.IsEditingEnabled) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.DeleteThisReceivable"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.EntityPM.RemoveReceivable(itemComponent.EntityPM);
                    _this.BuildItemsSource();
                    if (itemComponent.ChargesGroupCode == "FRT") {
                        _this.OnFreightAmountChanged();
                    }
                    _this.ComputeShipmentFields();
                }
            });
        }
    };
    ReceivablesTabComponent.prototype.ComputeShipmentFields = function () {
        if (this.EntityPM != null) {
            Tools_1.ShipmentTool.ComputeTotals(this.EntityPM);
        }
        this.BuildSummaryData();
        this.BuildProfitData();
    };
    ReceivablesTabComponent.prototype.OnFreightAmountChanged = function () {
        this.ItemsSource.Collection.filter(function (f) { return f.ChargesGroupCode != "FRT" && f.MeasurementCode == "PRFR"; }).forEach(function (item) {
            if (item.IsLineAttachted == false) {
                item.SetQuantity();
            }
        });
    };
    // Invoice
    ReceivablesTabComponent.prototype.CreateInvoiceClicked = function (type) {
        if (!this.SavingRequested) {
            this.SavingRequested = true;
            this.SavingRequestCode = "NewInvoice";
            this.SavingRequestParam = type;
            this.EntityPM.ShipmentReceivables.forEach(function (item) {
                if (Tools_2.AppTool.IsNullOrEmpty(item.ARInvoiceId) && Tools_2.AppTool.IsNullOrEmpty(item.ARInvoiceLineId)) {
                    if (Tools_2.AppTool.IsNullOrEmpty(item.ShipmentReceivableLineStatusCode) || item.ShipmentReceivableLineStatusCode == "EMPT") {
                        if (!Tools_2.AppTool.IsNullOrZero(item.Quantity) && !Tools_2.AppTool.IsNullOrZero(item.UnitPrice)) {
                            item.ShipmentReceivableLineStatusCode = "OAMT";
                        }
                    }
                }
            });
            var availableAmount = 0;
            var availableAmountText = null;
            switch (type) {
                case "IN":
                case "CI":
                    {
                        availableAmountText = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.NoOpenedAmounts");
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowMinusInvoicelines) {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null; }).length;
                        }
                        else {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null && f.UnitPrice > 0; }).length;
                        }
                        if (availableAmount == 0) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Height = 150;
                            messageWindow.Show(availableAmountText);
                            this.StopSavingFlags();
                        }
                        else {
                            this.SaveChanges();
                        }
                        break;
                    }
                case "CD":
                case "CC":
                    {
                        availableAmountText = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.NoOpenedMinusAmounts");
                        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AllowPositiveAmountsInTheCreditNote) {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null; }).length;
                        }
                        else {
                            availableAmount = this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ShipmentReceivableLineStatusCode == "OAMT" && f.ARInvoiceId == null && f.ARInvoiceLineId == null && f.UnitPrice < 0; }).length;
                        }
                        if (availableAmount == 0) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Height = 150;
                            messageWindow.Show(availableAmountText);
                            this.StopSavingFlags();
                        }
                        else {
                            this.SaveChanges();
                        }
                        break;
                    }
                case "MN": {
                    this.SaveChanges();
                    break;
                }
                default: {
                    this.StopSavingFlags();
                    break;
                }
            }
        }
    };
    ReceivablesTabComponent.prototype.ViewInvoiceClicked = function (invoiceId) {
        if (!Tools_2.AppTool.IsNullOrEmpty(invoiceId)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "ViewInvoice";
                this.SavingRequestParam = invoiceId;
                this.SaveChanges();
            }
        }
    };
    ReceivablesTabComponent.prototype.ShowQuoteClicked = function () {
        if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            if (!this.SavingRequested) {
                this.SavingRequested = true;
                this.SavingRequestCode = "ViewQuote";
                this.SavingRequestParam = this.EntityPM.QuoteId;
                this.SaveChanges();
            }
        }
    };
    ReceivablesTabComponent.prototype.SaveChanges = function () {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    ReceivablesTabComponent.prototype.StopSavingFlags = function () {
        this.SavingRequested = false;
        this.SavingRequestCode = null;
        this.SavingRequestParam = null;
    };
    ReceivablesTabComponent.prototype.ApplySavingCommand = function () {
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
    ReceivablesTabComponent.prototype.RunViewInvoice = function () {
        var _this = this;
        if (this.SavingRequestParam) {
            var entityId = this.SavingRequestParam;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'ARInvoice', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    else if (cmpRef.instance.IsReloadNeeded) {
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
    ReceivablesTabComponent.prototype.RunViewQuote = function () {
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
    ReceivablesTabComponent.prototype.RunNewInvoice = function () {
        var _this = this;
        var invoiceType = this.SavingRequestParam;
        switch (this.SavingRequestParam) {
            case "MN": {
                this.entityArgs.EditComponent.StartBusyIndicatorLoading();
                this.myDomainService.GetMasterReceivables(this.EntityPM.Id).subscribe(function (myResponse) {
                    _this.entityArgs.EditComponent.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        if (myResponse.Result.length == 0) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Height = 150;
                            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.NoOpenedAmounts"));
                        }
                        else {
                            _this.RunNewInvoiceWindow(myResponse.Result, invoiceType);
                        }
                    }
                });
                break;
            }
            default: {
                this.RunNewInvoiceWindow(this.EntityPM.ShipmentReceivables, invoiceType);
                break;
            }
        }
    };
    ReceivablesTabComponent.prototype.RunNewInvoiceWindow = function (myReceivables, invoiceType) {
        var _this = this;
        var windowTitle = null;
        switch (invoiceType) {
            case "IN": {
                windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.NewInvoice");
                break;
            }
            case "CI": {
                windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.NewCustomsInvoice");
                break;
            }
            case "CC": {
                windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.NewCustomsCredit");
                break;
            }
            case "CD": {
                windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.NewCreditInvoice");
                break;
            }
            case "MN": {
                windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.NewManifestInvoice");
                break;
            }
        }
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.WindowArgs = { Shipment: this.EntityPM, InvoiceTypeCode: invoiceType, EntityLevelCode: this.EntityPM.ShipmentLevelCode, EntityTableName: this.ObjectTableName, EntityReceivables: myReceivables };
        logitudeWindow.ComponentLoaded.subscribe(function (comp) {
            logitudeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: comp.EntityPM, ObjectTableName: 'ARInvoice', BackButtonLabel: _this.ObjectTableName + ": " + _this.EntityPM.ShipmentNumber });
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
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Intercompany") || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            logitudeWindow.Height = 540;
        }
        logitudeWindow.Show('./InvoiceModules/ARInvoice/Components/NewEntity/NewARInvoiceComponent');
    };
    Object.defineProperty(ReceivablesTabComponent.prototype, "ProrateReceivables", {
        get: function () { return this.EntityPM.ProrateReceivables; },
        set: function (value) {
            if (this.EntityPM.ProrateReceivables != value) {
                this.EntityPM.ProrateReceivables = value;
                this.BuildItemsSource();
                this.ComputeShipmentFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    ReceivablesTabComponent.prototype.CheckUpdateQuantities = function () {
        var _this = this;
        var updateMessage = null;
        var activeLines = [];
        activeLines = this.EntityPM.ShipmentReceivables;
        activeLines = activeLines.filter(function (d) { return d.ShipmentReceivableParentId == null; });
        activeLines = activeLines.filter(function (d) { return d.ARInvoiceId == null; });
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
                    case "VCBM": {
                        if (item.Quantity != _this.EntityPM.VolumeInCBM) {
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
                        if (_this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ChargesGroupCode == "FRT"; }).length > 0) {
                            var FRT_Quantity = Tools_2.ArrayTool.Sum(_this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_2.AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId); }), "TotalAmount");
                            if (item.Quantity != FRT_Quantity) {
                                isDifferentPRVL = true;
                            }
                            if (_this.EntityPM.ShipmentReceivables.filter(function (f) { return f.MeasurementCode == "PRFR" && f.Quantity != FRT_Quantity; }).length > 0) {
                                isDifferentPRFR = true;
                            }
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
    ReceivablesTabComponent.prototype.UpdateQuantitiesClicked = function () {
        var activeLines = [];
        activeLines = this.ItemsSource.Collection;
        activeLines = activeLines.filter(function (d) { return d.EntityPM.ShipmentReceivableParentId == null; });
        activeLines = activeLines.filter(function (d) { return d.EntityPM.ARInvoiceId == null; });
        activeLines.forEach(function (item) {
            item.SetQuantity();
        });
        this.CheckUpdateQuantities();
    };
    ReceivablesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReceivablesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ReceivablesTabComponent);
    return ReceivablesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ReceivablesTabComponent = ReceivablesTabComponent;
var ShipmentReceivableItem = /** @class */ (function (_super) {
    __extends(ShipmentReceivableItem, _super);
    function ShipmentReceivableItem(entity, fatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "ShipmentReceivable";
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
        _this.IsEditingEnabled = false;
        _this.IsProfitAmountVisible = false;
        _this.IsLineAttachted = false;
        _this.IsEditExchangeRateVisible = false;
        // Line Cells
        _this.SatusTypeToolTip = null;
        _this.MinMaxFromQuoteToolTip = "";
        _this.IsMinMaxFromQuoteIconVisible = false;
        _this.IsFixedAmountIconVisible = false;
        // BCNT | ByContainers
        _this.IsByContainerType = false;
        // Amounts
        _this.RelativeRateDate = null;
        _this.rateDate = null;
        // Line Summary
        _this.myUserListService = null;
        _this.CreatedByUserName = null;
        _this.UpdatedByUserName = null;
        _this.ReceivableSummary = null;
        _this.PayableSummary = null;
        _this.ProfitSummary = null;
        // Invoice
        _this.InvoiceNumber = null;
        _this.InvoiceStatus = null;
        _this.ExpectedAmountColor = null;
        _this.EntityPM = entity;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.IsNewEntity = isNew;
        _this.LocalCurrencyCode = fatherComponent.LocalCurrencyCode;
        _this.ProfitCurrencyCode = fatherComponent.ProfitCurrencyCode;
        _this.SetUIProperties();
        _this.SetLineCells();
        _this.SetLineSummary();
        _this.SetInvoiceData();
        _this.BuildInsideReceivables();
        return _this;
    }
    ShipmentReceivableItem.prototype.SetUIProperties = function () {
        this.IsEditExchangeRateVisible = this.fatherComponent.IsEditExchangeRateVisible;
        var isLineAttachted = false;
        var isEditingEnabled = this.fatherComponent.IsEditingEnabled;
        if (this.EntityPM.ShipmentReceivableParentId != null) {
            isLineAttachted = true;
        }
        if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceId)) {
            isLineAttachted = true;
        }
        if (isEditingEnabled) {
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
        this.IsEditingEnabled = isEditingEnabled;
        this.IsLineAttachted = isLineAttachted;
        this.UIProperties.SetEnabled("TotalAmount", this.ObjectTableName, isTotalAmountEnabled);
        this.UIProperties.SetEnabled("TotalAmountLocal", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Rate", this.ObjectTableName, isRateEnabled);
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, isChargeEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isQuantityEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, isUnitPriceEnabled);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MeasurementId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("PrepaidCollectId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("IsExchangeRateFixed", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_AmountProfit();
    };
    ShipmentReceivableItem.prototype.SetUIProperties_AmountProfit = function () {
        var isFieldVisible = false;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.CurrencyId)) {
            if (this.ShipmentPM.ProfitCurrencyId != this.CurrencyId && this.ShipmentPM.ProfitCurrencyId != SessionLocator_1.SessionLocator.LocalCurrencyId) {
                isFieldVisible = true;
            }
        }
        this.IsProfitAmountVisible = isFieldVisible;
        this.UIProperties.SetEnabled("AmountInProfitCurrency", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("AmountInProfitCurrency", this.ObjectTableName, isFieldVisible);
    };
    ShipmentReceivableItem.prototype.SetLineCells = function () {
        this.SetSatusTypeToolTip();
        this.SetMinMaxFromQuoteIconVisibility();
        this.SetFixedAmountIconVisibility();
    };
    ShipmentReceivableItem.prototype.SetSatusTypeToolTip = function () {
        var myResult;
        switch (this.EntityPM.ShipmentReceivableLineStatusCode) {
            case "APPD": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.Approved");
                break;
            }
            case "OAMT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.OpenAmount");
                break;
            }
            case "ACCT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.Accounted");
                break;
            }
            case "DRFT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.Draft");
                break;
            }
            default: {
                break;
            }
        }
        this.SatusTypeToolTip = myResult;
    };
    ShipmentReceivableItem.prototype.SetMinMaxFromQuoteIconVisibility = function () {
        var isVisible = false;
        var iTitle = null;
        var iAmount = null;
        if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
            iAmount = Tools_2.AppTool.Round(this.EntityPM.Quantity * this.EntityPM.UnitPrice, 2);
        }
        if (iAmount != null) {
            if (this.QuoteSaleMinAmount != null) {
                if (iAmount < this.QuoteSaleMinAmount) {
                    iAmount = this.QuoteSaleMinAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.Receivables.AmountdueQuoteMinimum");
                }
            }
            if (this.QuoteSaleMaxAmount != null) {
                if (iAmount > this.QuoteSaleMaxAmount) {
                    iAmount = this.QuoteSaleMaxAmount;
                    isVisible = true;
                    iTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.Receivables.AmountdueQuoteMaximum");
                }
            }
        }
        this.MinMaxFromQuoteToolTip = iTitle;
        this.IsMinMaxFromQuoteIconVisible = isVisible;
    };
    ShipmentReceivableItem.prototype.SetFixedAmountIconVisibility = function () {
        this.IsFixedAmountIconVisible = this.EntityPM.IsFixedPrice && this.EntityPM.IsFromQuote ? true : false;
    };
    Object.defineProperty(ShipmentReceivableItem.prototype, "ChargesTypeName", {
        // Properties
        get: function () { return this.EntityPM.ChargesTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "ChargesGroupCode", {
        get: function () { return this.EntityPM.ChargesGroupCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "ChargesTypeId", {
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
    ShipmentReceivableItem.prototype.OnChargesTypeChanged = function (list) {
        if (list) {
            this.EntityPM.ChargesTypeCode = list.Code;
            this.EntityPM.ChargesTypeName = list.EnglishName;
            this.EntityPM.ChargesGroupCode = list.ChargesGroupCode;
            this.EntityPM.DueTypeCode = list.DueTypeCode;
            this.EntityPM.DueTypeName = list.DueTypeName;
            this.VatTypeId = list.VatTypeId;
            this.EntityPM.IATACodeId = list.IATACodeId;
            //this.EntityPM.IsBackToBack = list.IsBackToBack;
            this.EntityPM.IsExpense = list.IsExpense;
            this.SetPrepaidCollectId();
            if (!Tools_2.AppTool.IsNullOrEmpty(list.ReceivablesDefaultCurrencyId)) {
                this.CurrencyId = list.ReceivablesDefaultCurrencyId;
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "VatTypeId", {
        get: function () { return this.EntityPM.VatTypeId; },
        set: function (value) {
            if (this.EntityPM.VatTypeId != value) {
                this.EntityPM.VatTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "CurrencyId", {
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "CurrencyCode", {
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
    ShipmentReceivableItem.prototype.SetLineRate = function () {
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.MeasurementId != newValue) {
                this.EntityPM.MeasurementId = newValue;
                this.IsByContainerType = false;
                this.ByContainersItemsSource = [];
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
                                        _this.Quantity = Tools_2.ArrayTool.Sum(_this.ShipmentPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_2.AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId); }), "TotalAmount");
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "MeasurementCode", {
        get: function () { return this.EntityPM.MeasurementCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "MeasurementShortName", {
        get: function () { return this.EntityPM.MeasurementShortName; },
        enumerable: true,
        configurable: true
    });
    ShipmentReceivableItem.prototype.ApplyByContainersFields = function () {
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
    ShipmentReceivableItem.prototype.BuildByContainersItemsSource = function () {
        var _this = this;
        var byContainersItemsSource = [];
        var myGrouped = Tools_1.ShipmentTool.GetByPckageTypeGrouped(this.ShipmentPM);
        myGrouped.forEach(function (item) {
            var newEntity = new ShipmentReceivablePM_1.ShipmentReceivablePM(null);
            newEntity.ShipmentId = _this.ShipmentPM.Id;
            newEntity.ShipmentNumber = _this.ShipmentPM.ShipmentNumber;
            newEntity.Tenant = _this.ShipmentPM.Tenant;
            newEntity.ShipmentReceivableLineStatusCode = "EMPT";
            newEntity.ChargesTypeId = _this.EntityPM.ChargesTypeId;
            newEntity.ChargesTypeCode = _this.EntityPM.ChargesTypeCode;
            newEntity.ChargesTypeName = _this.EntityPM.ChargesTypeName;
            newEntity.ChargesGroupCode = _this.EntityPM.ChargesGroupCode;
            newEntity.DueTypeCode = _this.EntityPM.DueTypeCode;
            newEntity.DueTypeName = _this.EntityPM.DueTypeName;
            newEntity.IATACodeId = _this.EntityPM.IATACodeId;
            newEntity.VatTypeId = _this.EntityPM.VatTypeId;
            newEntity.PrepaidCollectId = _this.EntityPM.PrepaidCollectId;
            newEntity.CurrencyId = _this.EntityPM.CurrencyId;
            newEntity.CurrencyCode = _this.EntityPM.CurrencyCode;
            newEntity.Rate = _this.EntityPM.Rate;
            newEntity.ProfitCurrencyExchangeRate = _this.EntityPM.ProfitCurrencyExchangeRate;
            newEntity.CreateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
            newEntity.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            newEntity.UpdateDate = Tools_2.DateTool.GetCurrentDateAsUtc();
            newEntity.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            newEntity.Quantity = item.Quantity;
            newEntity.MeasurementId = item.MeasurementId;
            newEntity.MeasurementCode = item.MeasurementCode;
            newEntity.MeasurementShortName = item.MeasurementShortName;
            var exsistingEntity = byContainersItemsSource.filter(function (f) { return f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId; })[0];
            if (exsistingEntity == null) {
                byContainersItemsSource.push(newEntity);
            }
            else {
                //var acctEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentReceivableLineStatusCode == "ACCT" || f.ShipmentReceivableLineStatusCode == "DRFT"))[0];
                //var openEntity = byContainersItemsSource.filter(f => f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && (f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"))[0];
                var acctEntity = byContainersItemsSource.filter(function (f) { return f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && f.ShipmentReceivableLineStatusCode == "ACCT"; })[0];
                var openEntity = byContainersItemsSource.filter(function (f) { return f.ChargesTypeId == newEntity.ChargesTypeId && f.MeasurementId == newEntity.MeasurementId && f.ShipmentReceivableLineStatusCode != "ACCT"; })[0];
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
    ShipmentReceivableItem.prototype.SetPrepaidCollectId = function () {
        if (Tools_2.AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            if (this.EntityPM.ChargesGroupCode == "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.FreightPrepaidCollectId;
            }
            else if (this.EntityPM.ChargesGroupCode != "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.OtherPrepaidCollectId;
            }
        }
    };
    Object.defineProperty(ShipmentReceivableItem.prototype, "PrepaidCollectId", {
        get: function () { return this.EntityPM.PrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.PrepaidCollectId != newValue) {
                this.EntityPM.PrepaidCollectId = newValue;
                this.ApplyByContainersFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newVaule) {
            if (this.EntityPM.Notes != newVaule) {
                this.EntityPM.Notes = newVaule;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "RateDate", {
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "Rate", {
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "IsExchangeRateFixed", {
        get: function () { return this.EntityPM.IsExchangeRateFixed; },
        set: function (newVaule) {
            if (this.EntityPM.IsExchangeRateFixed != newVaule) {
                this.EntityPM.IsExchangeRateFixed = newVaule;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "Quantity", {
        get: function () { return this.EntityPM.Quantity; },
        set: function (newVaule) {
            if (this.EntityPM.Quantity != newVaule) {
                this.EntityPM.Quantity = Tools_2.AppTool.Round(newVaule, 3);
                if (this.EntityPM.IsChargeBySteps) {
                    Tools_1.ShipmentTool.SetReceivableUnitPriceBySteps(this.EntityPM, this.fatherComponent.BaseQuote);
                }
                this.ComputeTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "UnitPrice", {
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "QuoteSaleMinAmount", {
        get: function () { return this.EntityPM.QuoteSaleMinAmount; },
        set: function (value) {
            if (this.EntityPM.QuoteSaleMinAmount != value) {
                this.EntityPM.QuoteSaleMinAmount = Tools_2.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "QuoteSaleMaxAmount", {
        get: function () { return this.EntityPM.QuoteSaleMaxAmount; },
        set: function (value) {
            if (this.EntityPM.QuoteSaleMaxAmount != value) {
                this.EntityPM.QuoteSaleMaxAmount = Tools_2.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "TotalAmount", {
        get: function () { return this.EntityPM.TotalAmount; },
        set: function (ivalue) {
            var value = ivalue;
            if (value) {
                if (this.QuoteSaleMinAmount != null) {
                    if (value < this.QuoteSaleMinAmount) {
                        value = this.QuoteSaleMinAmount;
                    }
                }
                if (this.QuoteSaleMaxAmount != null) {
                    if (value > this.QuoteSaleMaxAmount) {
                        value = this.QuoteSaleMaxAmount;
                    }
                }
            }
            if (this.EntityPM.TotalAmount != value) {
                this.EntityPM.TotalAmount = Tools_2.AppTool.Round(value, 2);
                this.SetLineStatus();
                this.ComputeUnitPrice(value);
                this.ComputeTotalAmountLocal();
                this.SetMinMaxFromQuoteIconVisibility();
                this.OnLineAmountChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "TotalAmountLocal", {
        get: function () { return this.EntityPM.TotalAmountLocal; },
        set: function (newVaule) {
            var _this = this;
            if (this.EntityPM.TotalAmountLocal != newVaule) {
                this.EntityPM.TotalAmountLocal = Tools_2.AppTool.Round(newVaule, 2);
                this.SetLineSummary();
                this.fatherComponent.ItemsSource.Collection.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }).forEach(function (item) {
                    item.SetLineSummary();
                });
                this.ComputeInsideReceivablesData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "AmountInProfitCurrency", {
        get: function () { return this.EntityPM.AmountInProfitCurrency; },
        set: function (newVaule) {
            if (this.EntityPM.AmountInProfitCurrency != newVaule) {
                this.EntityPM.AmountInProfitCurrency = Tools_2.AppTool.Round(newVaule, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "ProfitCurrencyExchangeRate", {
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
    ShipmentReceivableItem.prototype.SetLineStatus = function () {
        Tools_1.ShipmentTool.SetReceivableLineStatus(this.EntityPM);
    };
    ShipmentReceivableItem.prototype.ComputeUnitPrice = function (myTotalAmount) {
        if (!this.EntityPM.IsFixedPrice) {
            var myResult = null;
            if (this.EntityPM.IsChargeBySteps) {
            }
            else {
                if (myTotalAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }
                    else {
                        myResult = myTotalAmount / this.EntityPM.Quantity;
                    }
                }
                this.EntityPM.UnitPrice = Tools_2.AppTool.Round(myResult, 3);
            }
        }
    };
    ShipmentReceivableItem.prototype.ComputeTotalAmount = function () {
        this.SetLineStatus();
        if (!this.EntityPM.IsFixedPrice) {
            var iAmount = null;
            if (this.EntityPM.Quantity != null && this.EntityPM.UnitPrice != null) {
                if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                    var price = this.EntityPM.UnitPrice / 100;
                    iAmount = this.EntityPM.Quantity * price;
                }
                else {
                    iAmount = this.EntityPM.Quantity * this.EntityPM.UnitPrice;
                }
            }
            /* MinMax */
            if (iAmount != null) {
                if (this.QuoteSaleMinAmount != null) {
                    if (iAmount < this.QuoteSaleMinAmount) {
                        iAmount = this.QuoteSaleMinAmount;
                    }
                }
                if (this.QuoteSaleMaxAmount != null) {
                    if (iAmount > this.QuoteSaleMaxAmount) {
                        iAmount = this.QuoteSaleMaxAmount;
                    }
                }
            }
            this.EntityPM.TotalAmount = Tools_2.AppTool.Round(iAmount, 2);
            this.ComputeTotalAmountLocal();
            this.SetMinMaxFromQuoteIconVisibility();
            this.OnLineAmountChanged();
        }
    };
    ShipmentReceivableItem.prototype.ComputeTotalAmountLocal = function () {
        var myResult = null;
        if (this.EntityPM.TotalAmount != null && this.EntityPM.Rate != null) {
            myResult = Tools_2.AppTool.Round(this.EntityPM.TotalAmount * this.EntityPM.Rate, 2);
        }
        this.TotalAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
        this.ComputeInsideReceivablesData();
    };
    ShipmentReceivableItem.prototype.ComputeTotalAmountInProfitCurrency = function () {
        if (this.EntityPM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.AmountInProfitCurrency = this.EntityPM.TotalAmount;
        }
        else {
            this.AmountInProfitCurrency = (this.TotalAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }
        if (this.fatherComponent != null) {
            this.fatherComponent.ComputeShipmentFields();
        }
    };
    ShipmentReceivableItem.prototype.UpdateCurrencyRateClicked = function () {
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
    Object.defineProperty(ShipmentReceivableItem.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentReceivableItem.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    ShipmentReceivableItem.prototype.SetLineSummary = function () {
        var _this = this;
        if (this.myUserListService == null) {
            this.myUserListService = new UserListService_1.UserListService();
            this.myUserListService.getSingleFromCache(this.EntityPM.CreatedByUserId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        _this.CreatedByUserName = list.EnglishName;
                    }
                    else {
                        _this.myUserListService.getSingle(_this.EntityPM.CreatedByUserId).subscribe(function (myResponse) {
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
                this.myUserListService.getSingleFromCache(this.EntityPM.UpdateByUserId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list) {
                            _this.UpdatedByUserName = list.EnglishName;
                        }
                        else {
                            _this.myUserListService.getSingle(_this.EntityPM.UpdateByUserId).subscribe(function (myResponse) {
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
        if (!Tools_2.ArrayTool.Contains(this.ShipmentPM.ShipmentReceivables, this.EntityPM)) {
            if (!Tools_2.AppTool.IsNullOrEmpty(this.TotalAmountLocal)) {
                myReceivableSummary += this.TotalAmountLocal;
            }
        }
        if (this.ShipmentPM.ShipmentPayables.length > 0) {
            var openAmount = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }), "OpenAmountInLocalCurrency");
            var acctAmount = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == _this.ChargesTypeId; }), "AccountedAmountInLocalCurrency");
            myPayableSummary = openAmount + acctAmount;
        }
        this.ReceivableSummary = myReceivableSummary;
        this.PayableSummary = myPayableSummary;
        this.ProfitSummary = this.ReceivableSummary - this.PayableSummary;
    };
    Object.defineProperty(ShipmentReceivableItem.prototype, "ARInvoiceId", {
        get: function () { return this.EntityPM.ARInvoiceId; },
        enumerable: true,
        configurable: true
    });
    ShipmentReceivableItem.prototype.SetInvoiceData = function () {
        var _this = this;
        if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ARInvoiceId)) {
            var shipmentARInvoicePM = this.ShipmentPM.ShipmentARInvoices.filter(function (d) { return d.Id === _this.EntityPM.ARInvoiceId; })[0];
            if (shipmentARInvoicePM != null) {
                this.InvoiceNumber = shipmentARInvoicePM.InvoiceNumber;
                this.InvoiceStatus = shipmentARInvoicePM.StatusName;
            }
        }
    };
    ShipmentReceivableItem.prototype.BuildInsideReceivables = function () {
        var _this = this;
        this.InsideItemsSource = [];
        this.EntityPM.ChildShipmentReceivables.forEach(function (item) {
            var myConsoleShipmentPM = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (f) { return f.Id == item.ShipmentId; })[0];
            var insideReceivable = new InsideReceivableViewModel(item, myConsoleShipmentPM, _this.ShipmentPM.ProfitCurrencyId);
            _this.InsideItemsSource.push(insideReceivable);
        });
        this.ComputeInsideReceivablesTotals();
    };
    ShipmentReceivableItem.prototype.UpdateInsideItemsSource_Rate = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(function (insidePayable) {
                insidePayable.Rate = _this.Rate;
            });
        }
    };
    ShipmentReceivableItem.prototype.UpdateInsideItemsSource_Currency = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            this.InsideItemsSource.forEach(function (insidePayable) {
                insidePayable.CurrencyId = _this.CurrencyId;
                insidePayable.CurrencyCode = _this.CurrencyCode;
            });
        }
    };
    ShipmentReceivableItem.prototype.UpdateInsideItemsSource_Measurement = function () {
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
    ShipmentReceivableItem.prototype.ComputeInsideReceivablesData = function () {
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
                            case "GWTN": {
                                _QuantityTotal = Tools_2.ArrayTool.Sum(_this.InsideItemsSource, "GrossWeightPerTon");
                                _Ratio = _QuantityTotal == 0 ? 0 : _this.Quantity / _QuantityTotal;
                                unitPrice = _Ratio * _this.UnitPrice;
                                quantity = item.GrossWeightPerTon;
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
                        var totalAmount = quantity * unitPrice;
                        if (_this.MeasurementCode == "PRVL" || _this.MeasurementCode == "PRFR") {
                            totalAmount = quantity * unitPrice / 100;
                        }
                        var totalAmountLocal = totalAmount * _this.Rate;
                        var totalAmountInProfitCurrency = totalAmountLocal / _this.ProfitCurrencyExchangeRate;
                        item.EntityPM.TotalAmount = Tools_2.AppTool.Round(totalAmount, 3);
                        item.EntityPM.TotalAmountLocal = Tools_2.AppTool.Round(totalAmountLocal, 3);
                        item.EntityPM.AmountInProfitCurrency = Tools_2.AppTool.Round(totalAmountInProfitCurrency, 3);
                    });
                    _this.ComputeInsideReceivablesTotals();
                }
            });
        }
    };
    ShipmentReceivableItem.prototype.ComputeInsideReceivablesTotals = function () {
        this.SumOfQuantity = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "Quantity");
        this.SumOfAmount = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "TotalAmount");
        this.SumOfAmountLocal = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "TotalAmountLocal");
        this.SetExpectedAmountCell();
    };
    ShipmentReceivableItem.prototype.SetExpectedAmountCell = function () {
        var myColor = Tools_2.FontTool.Black;
        if (this.InsideItemsSource) {
            var sumOfAmount = Tools_2.ArrayTool.Sum(this.InsideItemsSource, "TotalAmount");
            var sumOfAmount = Tools_2.AppTool.Round(sumOfAmount, 2);
            if ((sumOfAmount < 0 || sumOfAmount > 0) && !Tools_2.AppTool.IsNullOrEmpty(this.TotalAmount) && sumOfAmount != this.TotalAmount) {
                myColor = Tools_2.FontTool.Red;
            }
            else if (this.EntityPM.IsChargeBySteps) {
                myColor = Tools_2.FontTool.Gray;
            }
            else if (this.EntityPM.ShipmentReceivableLineStatusCode == "ACCT" || this.EntityPM.ShipmentReceivableLineStatusCode == "DRFT") {
                myColor = Tools_2.FontTool.Gray;
            }
            else if (!Tools_2.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentReceivableParentId)) {
                myColor = Tools_2.FontTool.Gray;
            }
        }
        this.ExpectedAmountColor = myColor;
    };
    ShipmentReceivableItem.prototype.SetQuantity = function () {
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
                result = Tools_2.ArrayTool.Sum(this.ShipmentPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode == "FRT" && Tools_2.AppTool.IsNullOrEmpty(d.ShipmentReceivableParentId); }), "TotalAmount");
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
    ShipmentReceivableItem.prototype.OnLineAmountChanged = function () {
        if (this.ChargesGroupCode == "FRT") {
            this.fatherComponent.OnFreightAmountChanged();
        }
    };
    return ShipmentReceivableItem;
}(BaseComponent_1.BaseComponent));
exports.ShipmentReceivableItem = ShipmentReceivableItem;
var InsideReceivableViewModel = /** @class */ (function () {
    function InsideReceivableViewModel(entityPM, myConsoleShipmentPM, profitCurrencyId) {
        this.SatusTypeToolTip = null;
        this.EntityPM = entityPM;
        this.ShipmentPM = myConsoleShipmentPM;
        this.ProfitCurrencyId = profitCurrencyId;
        this.SetSatusTypeToolTip();
    }
    InsideReceivableViewModel.prototype.SetSatusTypeToolTip = function () {
        var myResult;
        switch (this.EntityPM.ShipmentReceivableLineStatusCode) {
            case "APPD": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.Approved");
                break;
            }
            case "OAMT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.OpenAmount");
                break;
            }
            case "ACCT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.Accounted");
                break;
            }
            case "DRFT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.Draft");
                break;
            }
            default: {
                break;
            }
        }
        this.SatusTypeToolTip = myResult;
    };
    Object.defineProperty(InsideReceivableViewModel.prototype, "TEU", {
        // Shipment Properties
        get: function () { return this.ShipmentPM.TEU; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "Volume", {
        get: function () { return this.ShipmentPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "GrossWeight", {
        get: function () { return this.ShipmentPM.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "ChargeableWeight", {
        get: function () { return this.ShipmentPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "GrossWeightPerTon", {
        get: function () { return this.ShipmentPM.GrossWeightPerTon; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "ValueOfGoods", {
        get: function () { return this.ShipmentPM.ValueOfGoods; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "FreightPayablesAmount", {
        get: function () { return this.ShipmentPM.FreightPayablesAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "FreightReceivablesAmount", {
        get: function () { return this.ShipmentPM.FreightReceivablesAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "NumberOfPackages", {
        get: function () { return this.ShipmentPM.NumberOfPackages; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "NumberOfContainers", {
        get: function () { return this.ShipmentPM.NumberOfContainers; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "ChargeableWeightInKG", {
        get: function () { return this.ShipmentPM.ChargeableWeightInKG; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "GrossWeightInKG", {
        get: function () { return this.ShipmentPM.GrossWeightInKG; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "VolumeInCBM", {
        get: function () { return this.ShipmentPM.VolumeInCBM; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "ShipmentId", {
        // Receivable Properties
        get: function () { return this.EntityPM.ShipmentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newVaule) {
            if (this.EntityPM.Notes != newVaule) {
                this.EntityPM.Notes = newVaule;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "MeasurementId", {
        get: function () { return this.EntityPM.MeasurementId; },
        set: function (value) {
            if (this.EntityPM.MeasurementId != value) {
                this.EntityPM.MeasurementId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "MeasurementCode", {
        get: function () { return this.EntityPM.MeasurementCode; },
        set: function (value) {
            if (this.EntityPM.MeasurementCode != value) {
                this.EntityPM.MeasurementCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "MeasurementShortName", {
        get: function () { return this.EntityPM.MeasurementShortName; },
        set: function (value) {
            if (this.EntityPM.MeasurementShortName != value) {
                this.EntityPM.MeasurementShortName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "CurrencyCode", {
        get: function () { return this.EntityPM.CurrencyCode; },
        set: function (value) {
            if (this.EntityPM.CurrencyCode != value) {
                this.EntityPM.CurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "Rate", {
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
    InsideReceivableViewModel.prototype.OnMeasurementChanged = function () {
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
            case "QTY": {
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
    Object.defineProperty(InsideReceivableViewModel.prototype, "Quantity", {
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
    Object.defineProperty(InsideReceivableViewModel.prototype, "UnitPrice", {
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
    Object.defineProperty(InsideReceivableViewModel.prototype, "TotalAmount", {
        get: function () { return this.EntityPM.TotalAmount; },
        set: function (value) {
            if (this.EntityPM.TotalAmount != value) {
                this.EntityPM.TotalAmount = Tools_2.AppTool.Round(value, 2);
                this.ComputeUnitPrice();
                this.ComputeTotalAmountLocal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InsideReceivableViewModel.prototype, "TotalAmountLocal", {
        get: function () { return this.EntityPM.TotalAmountLocal; },
        set: function (value) {
            if (this.EntityPM.TotalAmountLocal != value) {
                this.EntityPM.TotalAmountLocal = Tools_2.AppTool.Round(value, 2);
                this.ComputeTotalAmountInProfitCurrency();
            }
        },
        enumerable: true,
        configurable: true
    });
    InsideReceivableViewModel.prototype.SetLineStatus = function () {
        Tools_1.ShipmentTool.SetReceivableLineStatus(this.EntityPM);
        this.SetSatusTypeToolTip();
    };
    InsideReceivableViewModel.prototype.ComputeUnitPrice = function () {
        if (!this.EntityPM.IsChargeBySteps) {
            var myResult = null;
            if (this.EntityPM.IsChargeBySteps) {
            }
            else {
                if (this.EntityPM.TotalAmount != null && this.EntityPM.Quantity != null) {
                    if (this.EntityPM.Quantity == 0) {
                        myResult = 0;
                    }
                    else {
                        myResult = this.EntityPM.TotalAmount / this.EntityPM.Quantity;
                    }
                }
                this.EntityPM.UnitPrice = Tools_2.AppTool.Round(myResult, 3);
            }
        }
    };
    InsideReceivableViewModel.prototype.ComputeTotalAmount = function () {
        var myResult = null;
        this.SetLineStatus();
        if (this.Quantity != null && this.UnitPrice != null) {
            myResult = this.Quantity * this.UnitPrice;
            if (this.MeasurementCode == "PRVL" || this.MeasurementCode == "PRFR") {
                myResult = this.Quantity * this.UnitPrice / 100;
            }
        }
        this.EntityPM.TotalAmount = Tools_2.AppTool.Round(myResult, 2);
        this.ComputeTotalAmountLocal();
    };
    InsideReceivableViewModel.prototype.ComputeTotalAmountLocal = function () {
        var myResult = null;
        if (this.EntityPM.TotalAmount != null && this.EntityPM.Rate != null) {
            myResult = Tools_2.AppTool.Round(this.EntityPM.TotalAmount * this.EntityPM.Rate, 2);
        }
        this.TotalAmountLocal = myResult;
        this.ComputeTotalAmountInProfitCurrency();
    };
    InsideReceivableViewModel.prototype.ComputeTotalAmountInProfitCurrency = function () {
        if (this.EntityPM.CurrencyId == this.ProfitCurrencyId) {
            this.EntityPM.AmountInProfitCurrency = this.EntityPM.TotalAmount;
        }
        else {
            this.EntityPM.AmountInProfitCurrency = (this.TotalAmountLocal / this.EntityPM.ProfitCurrencyExchangeRate);
        }
        this.ComputeOtherAmounts();
    };
    InsideReceivableViewModel.prototype.ComputeOtherAmounts = function () {
    };
    return InsideReceivableViewModel;
}());
exports.InsideReceivableViewModel = InsideReceivableViewModel;
//# sourceMappingURL=ReceivablesTabComponent.js.map