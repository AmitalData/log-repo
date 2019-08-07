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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var QuotePMService_1 = require("../../../../../Quote/Services/StandardPMs/QuotePMService");
var QuoteListService_1 = require("../../../../../Quote/Services/StandardLists/QuoteListService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_2 = require("../../../../../Shipment/Tools");
var QuotesComponent = /** @class */ (function () {
    function QuotesComponent() {
        this.AllRates = [];
        this.ItemsSource = [];
        this.IsNoData = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedItem = null;
        this.isShowingUsedSpotRateQuotes = false;
        this.isGeneratePayables = true;
        this.myService = new QuoteListService_1.QuoteListService();
    }
    QuotesComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.AllRates = args['AllRates'];
        this.LoadData();
    };
    Object.defineProperty(QuotesComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuotesComponent.prototype, "IsShowingUsedSpotRateQuotes", {
        get: function () { return this.isShowingUsedSpotRateQuotes; },
        set: function (value) {
            if (this.isShowingUsedSpotRateQuotes != value) {
                this.isShowingUsedSpotRateQuotes = value;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuotesComponent.prototype, "IsGeneratePayables", {
        get: function () { return this.isGeneratePayables; },
        set: function (value) {
            if (this.isGeneratePayables != value) {
                this.isGeneratePayables = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    QuotesComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.IsNoData = false;
        this.ItemsSource = [];
        this.BaseQuote = null;
        this.SelectedItem = null;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "OpenDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("DirectionId", this.EntityPM.DirectionId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("TransportModeId", this.EntityPM.TransportModeId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("ShipmentTypeId", this.EntityPM.ShipmentTypeId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("CustomerId", this.EntityPM.CustomerId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("FromPortId", this.EntityPM.MainCarriageFromPortId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("ToPortId", this.EntityPM.MainCarriageFinalDestinationPortId, null, null, "StartsWith", false, false, false, "string");
        filters.addAdditionalFilter("IsShowingUsedSpotRateQuotes", this.IsShowingUsedSpotRateQuotes, null, null, "Equals", true, false, false, "Boolean");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentId)) {
            filters.addAdditionalFilter("RoutingRatesAgentId", this.EntityPM.AgentId, null, null, "StartsWith", true, false, false, "string");
        }
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse == null) {
                _this.IsNoData = true;
                _this.ItemsSource = [];
            }
            else {
                if (myResponse.HasError) {
                }
                else {
                    var list = myResponse.Result;
                    if (list.length == 0) {
                        _this.IsNoData = true;
                    }
                    list.forEach(function (item) {
                        _this.ItemsSource.push(new QuoteItem(item, _this.EntityPM.GrossWeight));
                    });
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    QuotesComponent.prototype.ViewQuoteClicked = function (QuoteId) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(QuoteId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit Quote";
            logWindow.IsFillScreen = true;
            var isSaveEditSuccess = false;
            logWindow.ComponentLoaded.subscribe(function (comp) {
                comp.SaveCompleted.subscribe(function (isSaveSuccess) {
                    isSaveEditSuccess = isSaveSuccess;
                });
            });
            logWindow.WindowClosed.subscribe(function (s) {
                if (isSaveEditSuccess) {
                    _this.LoadData();
                }
            });
            logWindow.ShowEditComponent(QuoteId, "Quote");
        }
    };
    QuotesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    QuotesComponent.prototype.GenerateButtonClicked = function () {
        if (this.SelectedItem) {
            if (this.SelectedItem.StageName == "Used" || this.SelectedItem.StageName == "Accepted") {
                this.LoadQuotePM(this.SelectedItem.Id);
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Height = 150;
                messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.GenerateIsAvailableAfterQuoteApproval"));
            }
        }
    };
    QuotesComponent.prototype.LoadQuotePM = function (QuoteId) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(QuoteId)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var myService = new QuotePMService_1.QuotePMService();
            myService.get(QuoteId).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.BaseQuote = myResponse.Result;
                    if (_this.BaseQuote) {
                        _this.Generate();
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    };
    QuotesComponent.prototype.Generate = function () {
        if (this.BaseQuote) {
            this.EntityPM.QuoteId = this.BaseQuote.Id;
            var Generator = new Tools_2.ShipmentGenerator(this.EntityPM, this.AllRates);
            Generator.GenerateReceivablesFromQuote(this.BaseQuote);
            if (this.IsGeneratePayables) {
                Generator.GeneratePayablesFromQuote(this.BaseQuote);
            }
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    QuotesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuotesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuotesComponent);
    return QuotesComponent;
}());
exports.QuotesComponent = QuotesComponent;
var QuoteItem = /** @class */ (function () {
    function QuoteItem(entity, shipmentGrossWeight) {
        this.Entity = null;
        this.shipmentGrossWeight = 0;
        this.WeightDiffernece = 0;
        this.StageBackground = "transparent";
        this.Entity = entity;
        this.shipmentGrossWeight = shipmentGrossWeight;
        this.SetDiffernece();
        this.SetStageBackground();
    }
    Object.defineProperty(QuoteItem.prototype, "Id", {
        get: function () { return this.Entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "QuoteNumber", {
        get: function () { return this.Entity.QuoteNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "OpenDate", {
        get: function () { return this.Entity.OpenDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "ExpirationDate", {
        get: function () { return this.Entity.ExpirationDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "StageName", {
        get: function () { return this.Entity.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "QuoteTypeName", {
        get: function () { return this.Entity.QuoteTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "CarrierName", {
        get: function () { return this.Entity.CarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "ChargeableWeight", {
        get: function () { return this.Entity.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "GrossWeight", {
        get: function () { return this.Entity.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "UsageCount", {
        get: function () { return this.Entity.UsageCount == 0 ? null : this.Entity.UsageCount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItem.prototype, "Notes", {
        get: function () { return this.Entity.Notes; },
        enumerable: true,
        configurable: true
    });
    QuoteItem.prototype.SetDiffernece = function () {
        var shipmentWeight = Tools_1.AppTool.IsNullOrEmpty(this.shipmentGrossWeight) ? 0 : this.shipmentGrossWeight;
        var quoteWeight = Tools_1.AppTool.IsNullOrEmpty(this.GrossWeight) ? 0 : this.GrossWeight;
        this.WeightDiffernece = shipmentWeight - quoteWeight;
    };
    QuoteItem.prototype.SetStageBackground = function () {
        if (this.StageName == "Used" || this.StageName == "Accepted") {
            this.StageBackground = "transparent";
        }
        else {
            this.StageBackground = "rgba(255, 171, 3, 0.6)";
        }
    };
    return QuoteItem;
}());
//# sourceMappingURL=QuotesComponent.js.map