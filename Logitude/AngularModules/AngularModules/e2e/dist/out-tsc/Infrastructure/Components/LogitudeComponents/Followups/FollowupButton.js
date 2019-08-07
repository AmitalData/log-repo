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
var Tools_1 = require("../../../Tools");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Utilities/TextCodeTranslator");
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../Services/EntityResourceService");
var EventTypeListService_1 = require("../../../Services/StandardLists/EventTypeListService");
var FollowupButton = /** @class */ (function () {
    function FollowupButton(entityResourceService) {
        var _this = this;
        this.entityResourceService = entityResourceService;
        this.LegType = null;
        this.QuotePM = null;
        this.ShipmentPM = null;
        this.ObjectTableName = null;
        this.FollowupLegType = null;
        this.HasFollowup = false;
        this.IsFeatureExists = false;
        this.IsComponentVisible = false;
        this.IsAutomatic = false;
        this.ExpDate = null;
        this.ActDate = null;
        this.ExpDateName = null;
        this.ActDateName = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.PropertyChangedEvent = null;
        this.FollowupsChangedEvent = null;
        this.isEnabled = true;
        this.isMouseOver = false;
        this.entityResourceService.getEntityResourceByTableName("FollowUp").subscribe(function (res) {
            _this.Listen();
        });
    }
    FollowupButton.prototype.Listen = function () {
        var _this = this;
        if (!this.FollowupsChangedEvent) {
            this.FollowupsChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "FollowupDeleted") {
                    _this.SetComponent();
                }
            });
        }
    };
    FollowupButton.prototype.ngOnInit = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.LegType)) {
            this.FollowupLegType = this.LegType.replace(" ", "");
            this.SetDateFieldsName();
        }
        if (this.QuotePM) {
            this.ObjectTableName = "Quote";
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Quote.Followups")) {
                this.IsFeatureExists = true;
            }
        }
        else if (this.ShipmentPM) {
            this.ObjectTableName = "Shipment";
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Shipment.Followups")) {
                this.IsFeatureExists = true;
                if (!this.PropertyChangedEvent) {
                    if (this.PickUpPM) {
                        this.PropertyChangedEvent = this.PickUpPM.PropertyChanged.subscribe(function (s) {
                            if (s) {
                                if (s.PropertyName == _this.ActDateName) {
                                    _this.SetComponent();
                                    _this.ActDate = Tools_1.DateTool.GetDateParts(_this.PickUpPM[_this.ActDateName]).DateObject;
                                    if (_this.ActDate != null) {
                                        _this.DeleteCurrentFollowup();
                                    }
                                }
                            }
                        });
                    }
                    else if (this.DeliveryPM) {
                        this.PropertyChangedEvent = this.DeliveryPM.PropertyChanged.subscribe(function (s) {
                            if (s) {
                                if (s.PropertyName == _this.ActDateName) {
                                    _this.SetComponent();
                                    _this.ActDate = Tools_1.DateTool.GetDateParts(_this.DeliveryPM[_this.ActDateName]).DateObject;
                                    if (_this.ActDate != null) {
                                        _this.DeleteCurrentFollowup();
                                    }
                                }
                            }
                        });
                    }
                    else {
                        this.PropertyChangedEvent = this.ShipmentPM.PropertyChanged.subscribe(function (s) {
                            if (s) {
                                if (s.PropertyName == _this.ActDateName) {
                                    _this.SetComponent();
                                    _this.ActDate = Tools_1.DateTool.GetDateParts(_this.ShipmentPM[_this.ActDateName]).DateObject;
                                    if (_this.ActDate != null) {
                                        _this.DeleteCurrentFollowup();
                                    }
                                    _this.SetSource();
                                    _this.SetTooltip();
                                }
                            }
                        });
                    }
                }
            }
        }
        this.SetComponent();
    };
    FollowupButton.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.FollowupsChangedEvent);
        Tools_1.AppTool.KillEventEmitter(this.PropertyChangedEvent);
    };
    FollowupButton.prototype.DeleteCurrentFollowup = function () {
        var _this = this;
        var myFollowup = this.ShipmentPM.FollowUps.filter(function (f) { return f.LegType == _this.FollowupLegType; })[0];
        if (myFollowup) {
            this.ShipmentPM.RemoveShipmentFollowUp(myFollowup);
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    FollowupButton.prototype.SetComponent = function () {
        var _this = this;
        if (this.IsFeatureExists) {
            this.HasFollowup = false;
            this.SetDateFields();
            if (this.QuotePM) {
            }
            else if (this.ShipmentPM) {
                if (this.ShipmentPM.FollowUps.filter(function (f) { return f.LegType == _this.FollowupLegType && f.Done != true; }).length > 0) {
                    this.HasFollowup = true;
                }
            }
            this.SetSource();
            this.SetTooltip();
        }
    };
    FollowupButton.prototype.SetSource = function () {
        var src = "./_Resources/Images/Icons/Followups/Followup.png";
        if (this.HasFollowup) {
            src = "./_Resources/Images/Icons/Followups/Followup_Black.png";
        }
        else if (this.IsMouseOver) {
            src = "./_Resources/Images/Icons/Followups/Followup_Black.png";
        }
        this.Source = src;
    };
    FollowupButton.prototype.SetTooltip = function () {
        if (this.HasFollowup) {
            this.Tootip = TextCodeTranslator_1.TextCodeTranslator.Translate("FollowUp.M.DeleteFollowUp");
        }
        else {
            this.Tootip = TextCodeTranslator_1.TextCodeTranslator.Translate("FollowUp.M.AddFollowUp");
        }
    };
    FollowupButton.prototype.SetDateFields = function () {
        var myExpDate = null;
        var myActDate = null;
        var legType = this.FollowupLegType;
        if (legType == null) {
            legType = "";
        }
        if (this.QuotePM) {
        }
        else if (this.ShipmentPM) {
            if (legType.indexOf("PickUp") > -1) {
                if (this.PickUpPM) {
                    myExpDate = Tools_1.DateTool.GetDateParts(this.PickUpPM[this.ExpDateName]).DateObject;
                    myActDate = Tools_1.DateTool.GetDateParts(this.PickUpPM[this.ActDateName]).DateObject;
                }
            }
            else if (legType.indexOf("Delivery") > -1) {
                if (this.DeliveryPM) {
                    myExpDate = Tools_1.DateTool.GetDateParts(this.DeliveryPM[this.ExpDateName]).DateObject;
                    myActDate = Tools_1.DateTool.GetDateParts(this.DeliveryPM[this.ActDateName]).DateObject;
                }
            }
            else if (legType.indexOf("EmptyCR") > -1) {
                if (this.DeliveryPM) {
                    myExpDate = Tools_1.DateTool.GetDateParts(this.DeliveryPM[this.ExpDateName]).DateObject;
                    myActDate = Tools_1.DateTool.GetDateParts(this.DeliveryPM[this.ActDateName]).DateObject;
                }
            }
            else if (legType.indexOf("CustomsClearanceDate") > -1 || legType.indexOf("FreightRelease") > -1 || legType.indexOf("TerminalAvailable") > -1
                || legType.indexOf("MAWBOBLDate") > -1 || legType.indexOf("CutoffDate") > -1) {
                if (this.ShipmentPM) {
                    myExpDate = Tools_1.DateTool.GetDateParts(this.ShipmentPM[this.ExpDateName]).DateObject;
                    myActDate = Tools_1.DateTool.GetDateParts(this.ShipmentPM[this.ActDateName]).DateObject;
                }
            }
            else {
                myExpDate = Tools_1.DateTool.GetDateParts(this.ShipmentPM[this.ExpDateName]).DateObject;
                myActDate = Tools_1.DateTool.GetDateParts(this.ShipmentPM[this.ActDateName]).DateObject;
            }
        }
        this.ExpDate = myExpDate;
        this.ActDate = myActDate;
        this.IsComponentVisible = Tools_1.AppTool.IsNullOrEmpty(this.ActDate) ? true : false;
    };
    FollowupButton.prototype.SetDateFieldsName = function () {
        if (this.QuotePM) {
        }
        else if (this.ShipmentPM) {
            switch (this.FollowupLegType) {
                // Pre
                case "PreCarriageDeparture": {
                    this.ExpDateName = "PreCarriageETD";
                    this.ActDateName = "PreCarriageATD";
                    break;
                }
                case "PreCarriageArrival": {
                    this.ExpDateName = "PreCarriageETA";
                    this.ActDateName = "PreCarriageATA";
                    break;
                }
                // Main
                case "MainCarriageDeparture": {
                    this.ExpDateName = "MainCarriageETD";
                    this.ActDateName = "MainCarriageATD";
                    break;
                }
                case "MainCarriageArrival": {
                    this.ExpDateName = "MainCarriageETA";
                    this.ActDateName = "MainCarriageATA";
                    break;
                }
                // TR1
                case "Transshipment1Departure": {
                    this.ExpDateName = "Transshipment1ETD";
                    this.ActDateName = "Transshipment1ATD";
                    break;
                }
                case "Transshipment1Arrival": {
                    this.ExpDateName = "Transshipment1ETA";
                    this.ActDateName = "Transshipment1ATA";
                    break;
                }
                // TR2
                case "Transshipment2Departure": {
                    this.ExpDateName = "Transshipment2ETD";
                    this.ActDateName = "Transshipment2ATD";
                    break;
                }
                case "Transshipment2Arrival": {
                    this.ExpDateName = "Transshipment2ETA";
                    this.ActDateName = "Transshipment2ATA";
                    break;
                }
                // TR3
                case "Transshipment3Departure": {
                    this.ExpDateName = "Transshipment3ETD";
                    this.ActDateName = "Transshipment3ATD";
                    break;
                }
                case "Transshipment3Arrival": {
                    this.ExpDateName = "Transshipment3ETA";
                    this.ActDateName = "Transshipment3ATA";
                    break;
                }
                // On
                case "OnCarriageDeparture": {
                    this.ExpDateName = "OnCarriageETD";
                    this.ActDateName = "OnCarriageATD";
                    break;
                }
                case "OnCarriageArrival": {
                    this.ExpDateName = "OnCarriageETA";
                    this.ActDateName = "OnCarriageATA";
                    break;
                }
                case "CustomsClearanceDate": {
                    this.ExpDateName = "CustomsClearanceDate";
                    this.ActDateName = "CustomsClearanceDate";
                    break;
                }
                case "FreightRelease": {
                    this.ExpDateName = "FreightRelease";
                    this.ActDateName = "FreightRelease";
                    break;
                }
                case "TerminalAvailable": {
                    this.ExpDateName = "TerminalAvailable";
                    this.ActDateName = "TerminalAvailable";
                    break;
                }
                case "WarehouseLegEntry": {
                    this.ExpDateName = "WarehouseLegExpectedEntryDate";
                    this.ActDateName = "WarehouseLegActualEntryDate";
                    break;
                }
                case "WarehouseLegRelease": {
                    this.ExpDateName = "WarehouseLegExpectedReleaseDate";
                    this.ActDateName = "WarehouseLegActualReleaseDate";
                    break;
                }
                case "MAWBOBLDate": {
                    this.ExpDateName = "MAWBOBLDate";
                    this.ActDateName = "MAWBOBLDate";
                    break;
                }
                case "CutoffDate": {
                    this.ExpDateName = "CutoffDate";
                    this.ActDateName = "CutoffDate";
                    break;
                }
                default: {
                    if (this.FollowupLegType) {
                        if (this.FollowupLegType.indexOf("Departure") > -1) {
                            this.ExpDateName = "ETD";
                            this.ActDateName = "ATD";
                        }
                        else if (this.FollowupLegType.indexOf("Arrival") > -1) {
                            this.ExpDateName = "ETA";
                            this.ActDateName = "ATA";
                        }
                    }
                    break;
                }
            }
        }
    };
    Object.defineProperty(FollowupButton.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FollowupButton.prototype, "IsMouseOver", {
        get: function () { return this.isMouseOver; },
        set: function (value) {
            if (this.isMouseOver != value) {
                this.isMouseOver = value;
                this.SetSource();
            }
        },
        enumerable: true,
        configurable: true
    });
    FollowupButton.prototype.ButtonClicked = function () {
        var _this = this;
        if (this.QuotePM) {
        }
        else if (this.ShipmentPM) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.FollowupLegType)) {
                var allFollowups = this.ShipmentPM.FollowUps;
                var myFollowup = allFollowups.filter(function (f) { return f.LegType == _this.FollowupLegType && f.Done != true; })[0];
                if (myFollowup) {
                    this.ShipmentPM.RemoveShipmentFollowUp(myFollowup);
                    this.CurrentSession.FireEvent("FollowupsChanged");
                    this.SetComponent();
                }
                else {
                    if (this.IsAutomatic) {
                        var eventTypeCode = null;
                        var dateTime = null;
                        switch (this.FollowupLegType) {
                            case "MainCarriageDeparture":
                                {
                                    eventTypeCode = "DEP";
                                    dateTime = this.ShipmentPM.MainCarriageETD;
                                    break;
                                }
                            case "Transshipment1Departure":
                                {
                                    eventTypeCode = "T1DP";
                                    dateTime = this.ShipmentPM.Transshipment1ETD;
                                    break;
                                }
                            case "Transshipment2Departure":
                                {
                                    eventTypeCode = "T2DP";
                                    dateTime = this.ShipmentPM.Transshipment2ETD;
                                    break;
                                }
                            case "Transshipment3Departure":
                                {
                                    eventTypeCode = "T3DP";
                                    dateTime = this.ShipmentPM.Transshipment3ETD;
                                    break;
                                }
                            case "MainCarriageArrival":
                                {
                                    eventTypeCode = "ARR";
                                    dateTime = this.ShipmentPM.MainCarriageETA;
                                    break;
                                }
                            case "Transshipment1Arrival":
                                {
                                    eventTypeCode = "T1AR";
                                    dateTime = this.ShipmentPM.Transshipment1ETA;
                                    break;
                                }
                            case "Transshipment2Arrival":
                                {
                                    eventTypeCode = "T2AR";
                                    dateTime = this.ShipmentPM.Transshipment1ETA;
                                    break;
                                }
                            case "Transshipment3Arrival":
                                {
                                    eventTypeCode = "T3AR";
                                    dateTime = this.ShipmentPM.Transshipment1ETA;
                                    break;
                                }
                            case "CustomsClearanceDate":
                                {
                                    if (this.ShipmentPM.DirectionId == "I") {
                                        eventTypeCode = "ICUC";
                                    }
                                    else {
                                        eventTypeCode = "ECUC";
                                    }
                                    dateTime = this.ShipmentPM.CustomsClearanceDate;
                                    break;
                                }
                            case "FreightRelease":
                                {
                                    eventTypeCode = "FRRL";
                                    dateTime = this.ShipmentPM.FreightRelease;
                                    break;
                                }
                            case "TerminalAvailable":
                                {
                                    eventTypeCode = "TRAV";
                                    dateTime = this.ShipmentPM.TerminalAvailable;
                                    break;
                                }
                            case "WarehouseLegEntry": {
                                eventTypeCode = "WHED";
                                dateTime = this.ShipmentPM.WarehouseLegExpectedEntryDate;
                                break;
                            }
                            case "WarehouseLegRelease": {
                                eventTypeCode = "WHRD";
                                dateTime = this.ShipmentPM.WarehouseLegExpectedReleaseDate;
                                break;
                            }
                            case "MAWBOBLDate":
                                {
                                    eventTypeCode = "OBLD";
                                    dateTime = this.ShipmentPM.MAWBOBLDate;
                                    break;
                                }
                            case "CutoffDate":
                                {
                                    eventTypeCode = "CUTO";
                                    dateTime = this.ShipmentPM.CutoffDate;
                                    break;
                                }
                        }
                        var myService = new EventTypeListService_1.EventTypeListService();
                        myService.getAllFromCache().subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var AllEventTypes = myResponse.Result;
                                var eventType = AllEventTypes.filter(function (f) { return f.Code == eventTypeCode; })[0];
                                if (eventType) {
                                    var newFollowUp = new ShipmentFollowUpPM_1.ShipmentFollowUpPM(null);
                                    newFollowUp.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                    newFollowUp.Date = dateTime;
                                    newFollowUp.IsNew = true;
                                    newFollowUp.Done = false;
                                    newFollowUp.Deleted = false;
                                    newFollowUp.EventTypeId = eventType.Id;
                                    newFollowUp.EventTypeFollowUpName = eventType.FollowUpEnglishName;
                                    newFollowUp.LegType = _this.FollowupLegType;
                                    newFollowUp.ShipmentId = _this.ShipmentPM.Id;
                                    newFollowUp.OwnerUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                                    _this.ShipmentPM.AddShipmentFollowUp(newFollowUp);
                                    _this.CurrentSession.FireEvent("FollowupsChanged");
                                    _this.SetComponent();
                                }
                            }
                        });
                    }
                    else {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddNewFollowUp");
                        logitudeWindow.WindowArgs = { Date: this.ExpDate, ShipmentPM: this.ShipmentPM, FollowupLegType: this.FollowupLegType };
                        logitudeWindow.Width = 350;
                        logitudeWindow.Height = 400;
                        logitudeWindow.WindowClosed.subscribe(function (s) {
                            if (s) {
                                _this.SetComponent();
                            }
                        });
                        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/Followups/AddFollowupComponent');
                    }
                }
            }
        }
    };
    FollowupButton = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FollowupButton.html',
            selector: "FollowupButton",
            inputs: ['QuotePM', 'ShipmentPM', 'PickUpPM', 'DeliveryPM', 'LegType', 'IsEnabled', 'IsAutomatic'],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], FollowupButton);
    return FollowupButton;
}());
exports.FollowupButton = FollowupButton;
//# sourceMappingURL=FollowupButton.js.map