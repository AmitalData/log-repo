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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var OverviewTabComponent = /** @class */ (function () {
    function OverviewTabComponent(entityArgs, entityResourceService) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.IsLCLEntity = false;
        this.IsFCLEntity = false;
        this.ObjectTableName = null;
        this.ContainersList = [];
        this.FollowupsList = [];
        this.IsVisibile = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.MasterLabel = "";
        this.CarrierLabel = "";
        this.CarrierNoLabel = "";
        this.CarrierDateLabel = "";
        this.MasterDataLableWidth = "70px";
        // Money Information
        this.IsProfitAreaVisible = false;
        this.IsCurrencyFilterVisible = false;
        this.IsByLocalCurrency = false;
        this.SelectedCurrencyCode = null;
        this.ARInvoices = 0;
        this.APInvoices = 0;
        this.OpenReceivables = 0;
        this.OpenPayables = 0;
        this.Profit = 0;
        this.TotalAR = 0;
        this.TotalAP = 0;
        this.ProfitColor = "#282E30";
        // Followup
        this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup.png";
        // Notes    
        this.NotesList = [];
        entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (response) {
            _this.IsVisibile = true;
            _this.EntityPM = _this.entityArgs.EntityPM;
            _this.ObjectTableName = _this.entityArgs.ObjectTableName;
            _this.Listen();
            if (_this.EntityPM != null) {
                _this.IsLCLEntity = Tools_1.AppTool.IsLCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                _this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(_this.EntityPM.TransportModeId, _this.EntityPM.ShipmentTypeId);
                _this.BuildCargoData();
                _this.InitializeMoneyData();
                _this.BuildFollowups();
                _this.BuildNotesList();
            }
        });
    }
    OverviewTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "FollowupsChanged") {
                    _this.BuildFollowups();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHOV" || tabCode == "JHOV") {
                    _this.BuildCargoData();
                    _this.BuildMoneyData();
                    _this.BuildNotesList();
                    _this.BuildFollowups();
                }
            });
        }
    };
    OverviewTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    Object.defineProperty(OverviewTabComponent.prototype, "Volume", {
        // Cargo Information    
        get: function () { return this.EntityPM.Volume == null ? 0 : this.EntityPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ChargeableWeight", {
        get: function () { return this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "NumberOfPackages", {
        get: function () { return this.EntityPM.NumberOfPackages == null ? 0 : this.EntityPM.NumberOfPackages; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "VolumeUnitCode", {
        get: function () { return this.EntityPM.VolumeUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageCarrierName", {
        get: function () { return this.EntityPM.MainCarriageCarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MasterDepartureDate", {
        get: function () { return this.EntityPM.MainCarriageATD != null ? this.EntityPM.MainCarriageATD : this.EntityPM.MainCarriageETD; },
        enumerable: true,
        configurable: true
    });
    OverviewTabComponent.prototype.BuildCargoData = function () {
        if (this.IsFCLEntity) {
            this.ContainersList = [];
            var list = [];
            this.EntityPM.ShipmentPackages.filter(function (f) { return f.IsContainer == true; }).forEach(function (item) {
                var myContainer = list.filter(function (f) { return f.PackageTypeId == item.PackageTypeId; })[0];
                if (myContainer == null) {
                    myContainer = new Container();
                    myContainer.Quantity = Tools_1.AppTool.IsNullOrEmpty(item.Quantity) ? 0 : item.Quantity;
                    myContainer.PackageTypeId = item.PackageTypeId;
                    myContainer.PackageTypeName = item.PackageTypeName;
                    list.push(myContainer);
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
                        myContainer.Quantity += item.Quantity;
                    }
                }
            });
            this.ContainersList = list;
        }
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.MasterDataLableWidth = "70px";
                this.MasterLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.MAWB");
                this.CarrierLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.Flight");
                this.CarrierNoLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.FlightNo");
                this.CarrierDateLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.FlightDate");
                break;
            }
            case "O": {
                this.MasterDataLableWidth = "80px";
                this.MasterLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.OBL");
                this.CarrierLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.ShippingLine");
                this.CarrierNoLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.VoyageNo");
                this.CarrierDateLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.VoyageDate");
                break;
            }
            case "I": {
                this.MasterDataLableWidth = "80px";
                this.MasterLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.CMR/RWB#");
                this.CarrierLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.Trucker");
                this.CarrierNoLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.TruckNo");
                this.CarrierDateLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Overview.TruckerDate");
                break;
            }
        }
    };
    Object.defineProperty(OverviewTabComponent.prototype, "LocalCurrencyCode", {
        get: function () { return SessionLocator_1.SessionLocator.LocalCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ProfitCurrencyCode", {
        get: function () { return this.EntityPM.ProfitCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    OverviewTabComponent.prototype.InitializeMoneyData = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Profit")) {
            this.IsProfitAreaVisible = true;
        }
        this.IsCurrencyFilterVisible = SessionLocator_1.SessionLocator.LocalCurrencyId == this.EntityPM.ProfitCurrencyId ? false : true;
        this.SelectedCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.BuildMoneyData();
    };
    OverviewTabComponent.prototype.BuildMoneyData = function () {
        this.ARInvoices = 0;
        this.APInvoices = 0;
        this.OpenReceivables = 0;
        this.OpenPayables = 0;
        this.Profit = 0;
        if (this.EntityPM != null && this.EntityPM !== undefined) {
            if (this.IsByLocalCurrency) {
                this.OpenPayables = this.EntityPM.OpenPayablesInLocalCurrency;
                this.OpenReceivables = this.EntityPM.OpenReceivablesInLocalCurrency;
                this.ARInvoices = Tools_1.ArrayTool.Sum(this.EntityPM.ShipmentARInvoices, "AmountInLocalCurrency");
                //this.APInvoices = ArrayTool.Sum(this.EntityPM.ShipmentAPInvoices, "GrandTotalInLocalCurrency");
                this.APInvoices = this.EntityPM.AccountedPayablesInLocalCurrency;
                this.Profit = this.EntityPM.ProfitInLocalCurrency;
            }
            else {
                this.OpenPayables = this.EntityPM.OpenPayablesInProfitCurrency;
                this.OpenReceivables = this.EntityPM.OpenReceivablesInProfitCurrency;
                this.ARInvoices = Tools_1.ArrayTool.Sum(this.EntityPM.ShipmentARInvoices, "AmountInProfitCurrency");
                //this.APInvoices = ArrayTool.Sum(this.EntityPM.ShipmentAPInvoices, "GrandTotalInProfitCurrency");
                this.APInvoices = this.EntityPM.AccountedPayablesInProfitCurrency;
                this.Profit = this.EntityPM.ProfitInProfitCurrency;
            }
            this.TotalAR = this.ARInvoices + this.OpenReceivables;
            this.TotalAP = this.APInvoices + this.OpenPayables;
            if (this.Profit > 0) {
                this.ProfitColor = "#009161";
            }
            else if (this.Profit < 0) {
                this.ProfitColor = "#E53030";
            }
            else {
                this.ProfitColor = "#282E30";
            }
        }
    };
    OverviewTabComponent.prototype.BuildFollowups = function () {
        var _this = this;
        this.FollowupsList = [];
        this.EntityPM.FollowUps.filter(function (f) { return f.Done == false; }).forEach(function (item) {
            _this.FollowupsList.push(new FollowupClass(item));
        });
        this.SetFollowupsIconPath();
    };
    OverviewTabComponent.prototype.SetFollowupsIconPath = function () {
        if (this.FollowupsList.length == 0) {
            this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup.png";
        }
        else {
            if (this.FollowupsList.filter(function (f) { return f.IsOld == true; }).length > 0) {
                this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup_Red.png";
            }
            else {
                this.FollowupsIconPath = "./_Resources/Images/Icons/Followups/Followup_Black.png";
            }
        }
    };
    Object.defineProperty(OverviewTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        enumerable: true,
        configurable: true
    });
    OverviewTabComponent.prototype.BuildNotesList = function () {
        this.NotesList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperNote)) {
            this.NotesList.push({ Header: "Shipper", Notes: this.EntityPM.ShipperNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeNote)) {
            this.NotesList.push({ Header: "Consignee", Notes: this.EntityPM.ConsigneeNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AgentNote)) {
            this.NotesList.push({ Header: "Agent", Notes: this.EntityPM.AgentNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomAgentExportNote)) {
            this.NotesList.push({ Header: "Custom Agent Export", Notes: this.EntityPM.CustomAgentExportNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomAgentImportNote)) {
            this.NotesList.push({ Header: "Custom Agent Import", Notes: this.EntityPM.CustomAgentImportNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1Note)) {
            this.NotesList.push({ Header: "Notify1", Notes: this.EntityPM.Notify1Note });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify2Note)) {
            this.NotesList.push({ Header: "Notify2", Notes: this.EntityPM.Notify2Note });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeNotImporterNote)) {
            this.NotesList.push({ Header: "Consignee Not Importer", Notes: this.EntityPM.ConsigneeNotImporterNote });
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperNotExporterNote)) {
            this.NotesList.push({ Header: "Shipper Not Exporter", Notes: this.EntityPM.ShipperNotExporterNote });
        }
    };
    OverviewTabComponent.prototype.onSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }
        else {
            this.IsByLocalCurrency = false;
        }
        this.BuildMoneyData();
    };
    OverviewTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './OverviewTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], OverviewTabComponent);
    return OverviewTabComponent;
}());
exports.OverviewTabComponent = OverviewTabComponent;
var Container = /** @class */ (function () {
    function Container() {
    }
    return Container;
}());
var FollowupClass = /** @class */ (function () {
    function FollowupClass(entityPM) {
        this.ItemId = null;
        this.ItemTooltipId = null;
        this.Done = false;
        this.Date = null;
        this.Name = null;
        this.Notes = null;
        this.IconPath = null;
        this.TextColor = null;
        this.IsOld = false;
        this.Background = "white";
        this.ListItemHeight = 40;
        this.TooltipHeight = 130;
        this.TooltipWidth = 270;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowTooltip = false;
        this.EntityPM = entityPM;
        var idIndex = this.CurrentSession.GetNewId("FollowupItem");
        this.ItemId = "FollowupItem_" + idIndex;
        this.ItemTooltipId = "FollowupItemTooltip_" + idIndex;
        this.Done = entityPM.Done;
        this.Date = entityPM.Date;
        this.Name = entityPM.EventTypeFollowUpName;
        this.Notes = entityPM.Note;
        if (this.Date) {
            if (Tools_1.DateTool.GetDateParts(this.Date).DateTicks < Tools_1.DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.IsOld = true;
                this.Background = "#F7E3E3";
            }
        }
        this.SetIconPath();
    }
    FollowupClass.prototype.SetIconPath = function () {
        var iconPath = "./_Resources/Images/Icons/Followups/Document.png";
        if (this.Done) {
            iconPath = "./_Resources/Images/Icons/Followups/Done.png";
        }
        else if (this.Name) {
            var name = this.Name.toLowerCase();
            if (name.indexOf("arrived") > -1 || name.indexOf("departed") > -1 || name.indexOf("departure") > -1 || name.indexOf("arrival") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Routing.png";
            }
            else if (name.indexOf("reminder") > -1 || name.indexOf("arranged") > -1) {
                iconPath = "./_Resources/Images/Icons/Followups/Reminder.png";
            }
        }
        this.IconPath = iconPath;
    };
    FollowupClass.prototype.ShowTooltip = function (isShowTooltip) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Notes)) {
            if (isShowTooltip) {
                var item = document.getElementById(this.ItemId);
                var itemRect = item.getBoundingClientRect();
                document.getElementById(this.ItemTooltipId).style.top = (itemRect.top - (this.TooltipHeight / 2) + (this.ListItemHeight / 2)) + 'px';
                document.getElementById(this.ItemTooltipId).style.left = (itemRect.left - this.TooltipWidth + 5) + 'px';
            }
            this.IsShowTooltip = isShowTooltip;
        }
    };
    return FollowupClass;
}());
//# sourceMappingURL=OverviewTabComponent.js.map