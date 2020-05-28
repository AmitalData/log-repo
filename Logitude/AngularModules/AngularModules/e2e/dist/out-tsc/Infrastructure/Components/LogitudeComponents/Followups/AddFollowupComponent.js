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
var Validator_1 = require("../../../Validators/Validator");
var BaseComponent_1 = require("../BaseComponent");
var Tools_1 = require("../../../Tools");
var FollowUpPM_1 = require("../../../EntityPMs/FollowUpPM");
var QuoteFollowUpPM_1 = require("../../../../Quote/EntityPMs/QuoteFollowUpPM");
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var EventTypeListService_1 = require("../../../Services/StandardLists/EventTypeListService");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var AddFollowupComponent = /** @class */ (function (_super) {
    __extends(AddFollowupComponent, _super);
    function AddFollowupComponent() {
        var _this = _super.call(this) || this;
        _this.QuotePM = null;
        _this.ShipmentPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "FollowUp";
        _this.ValidationErrorsList = [];
        _this.AllEventTypes = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new FollowUpPM_1.FollowUpPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.OwnerUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.Done = false;
        _this.EntityPM.IsNew = true;
        _this.EntityPM.Deleted = false;
        return _this;
    }
    AddFollowupComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.Date = args['Date'];
        this.QuotePM = args['QuotePM'];
        this.ShipmentPM = args['ShipmentPM'];
        this.EntityPM.LegType = args['FollowupLegType'];
        if (Tools_1.AppTool.IsNullOrEmpty(this.Date)) {
            this.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        var myService = new EventTypeListService_1.EventTypeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllEventTypes = myResponse.Result;
            }
            _this.SetupEntity();
        });
    };
    AddFollowupComponent.prototype.SetupEntity = function () {
        var ObjectTableId = null;
        if (this.QuotePM) {
            ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === "Quote"; })[0].Id;
        }
        else if (this.ShipmentPM) {
            ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === "Shipment"; })[0].Id;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(ObjectTableId)) {
            var Code = this.GetEventTypeCode();
            var eventType = this.AllEventTypes.filter(function (f) { return f.ObjectTableId == ObjectTableId && f.Code == Code; })[0];
            if (eventType) {
                this.EntityPM.EventTypeId = eventType.Id;
                this.EntityPM.EventTypeFollowUpName = eventType.FollowUpEnglishName;
            }
        }
    };
    AddFollowupComponent.prototype.GetEventTypeCode = function () {
        var myResult = null;
        if (this.EntityPM.LegType) {
            var legType = this.EntityPM.LegType.toLowerCase();
            if (legType.indexOf("departure") > -1) {
                switch (legType) {
                    case "precarriagedeparture": {
                        myResult = "PRCD";
                        break;
                    }
                    case "maincarriagedeparture":
                    case "transshipment1departure":
                    case "transshipment2departure":
                    case "transshipment3departure":
                        {
                            myResult = "DEP";
                            break;
                        }
                    case "oncarriagedeparture": {
                        myResult = "ONCD";
                        break;
                    }
                    case "warehouselegentry": {
                        myResult = "WHED";
                        break;
                    }
                    case "warehouselegrelease": {
                        myResult = "WHRD";
                        break;
                    }
                    default: {
                        if (legType.indexOf("pickup") > -1) {
                            myResult = "PICD";
                        }
                        else if (legType.indexOf("delivery") > -1) {
                            myResult = "DELD";
                        }
                        else if (legType.indexOf("emptycr") > -1) {
                            myResult = "DELD";
                        }
                        break;
                    }
                }
            }
            else if (legType.indexOf("arrival") > -1) {
                switch (legType) {
                    case "precarriagearrival": {
                        myResult = "PRCA";
                        break;
                    }
                    case "maincarriagearrival":
                    case "transshipment1arrival":
                    case "transshipment2arrival":
                    case "transshipment3arrival":
                        {
                            myResult = "ARR";
                            break;
                        }
                    case "oncarriagearrival": {
                        myResult = "ONCA";
                        break;
                    }
                    default: {
                        if (legType.indexOf("pickup") > -1) {
                            myResult = "RCS";
                        }
                        else if (legType.indexOf("delivery") > -1) {
                            myResult = "PIOD";
                        }
                        else if (legType.indexOf("emptycr") > -1) {
                            myResult = "PIOD";
                        }
                        break;
                    }
                }
            }
            else if (this.ShipmentPM) {
                if (legType.indexOf("customsclearancedate") > -1) {
                    if (this.ShipmentPM.DirectionId == "I") {
                        myResult = "ICUC";
                    }
                    else {
                        myResult = "ECUC";
                    }
                    this.Date = this.ShipmentPM.CustomsClearanceDate;
                }
                else if (legType.indexOf("freightrelease") > -1) {
                    myResult = "FRRL";
                    this.Date = this.ShipmentPM.FreightRelease;
                }
                else if (legType.indexOf("terminalavailable") > -1) {
                    myResult = "TRAV";
                    this.Date = this.ShipmentPM.TerminalAvailable;
                }
                else if (legType.indexOf("warehouselegentry") > -1) {
                    myResult = "WHED";
                    this.Date = this.ShipmentPM.WarehouseLegExpectedEntryDate;
                }
                else if (legType.indexOf("warehouselegrelease") > -1) {
                    myResult = "WHRD";
                    this.Date = this.ShipmentPM.WarehouseLegExpectedReleaseDate;
                }
                else if (legType.indexOf("mawbobldate") > -1) {
                    myResult = "OBLD";
                    this.Date = this.ShipmentPM.MAWBOBLDate;
                }
                else if (legType.indexOf("cutoffdate") > -1) {
                    myResult = "CUTO";
                    this.Date = this.ShipmentPM.CutoffDate;
                }
            }
        }
        return myResult;
    };
    Object.defineProperty(AddFollowupComponent.prototype, "Date", {
        get: function () { return this.EntityPM.Date; },
        set: function (value) {
            if (this.EntityPM.Date != value) {
                this.EntityPM.Date = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddFollowupComponent.prototype, "OwnerUserId", {
        get: function () { return this.EntityPM.OwnerUserId; },
        set: function (value) {
            if (this.EntityPM.OwnerUserId != value) {
                this.EntityPM.OwnerUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddFollowupComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddFollowupComponent.prototype.NumericButtonClicked = function (isIncreas) {
        if (this.Date == null) {
            this.Date = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        else {
            var day = isIncreas ? 1 : -1;
            var newDate = Tools_1.DateTool.GetDateParts(this.Date).DateObject;
            newDate.setUTCMilliseconds(0);
            newDate.setUTCSeconds(0);
            newDate.setUTCMinutes(0);
            newDate.setUTCHours(0);
            newDate.setUTCDate(newDate.getDate() + day);
            newDate.setUTCMonth(newDate.getMonth());
            newDate.setUTCFullYear(newDate.getFullYear());
            this.Date = newDate;
        }
    };
    AddFollowupComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddFollowupComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.QuotePM) {
                var myQuoteFollowUpPM = new QuoteFollowUpPM_1.QuoteFollowUpPM(null);
                myQuoteFollowUpPM.Tenant = this.QuotePM.Tenant;
                myQuoteFollowUpPM.QuoteId = this.QuotePM.Id;
                myQuoteFollowUpPM.EventTypeId = this.EntityPM.EventTypeId;
                myQuoteFollowUpPM.EventTypeFollowUpName = this.EntityPM.EventTypeFollowUpName;
                myQuoteFollowUpPM.ManualActivatedFollowUp = this.EntityPM.ManualActivatedFollowUp;
                myQuoteFollowUpPM.Date = this.Date;
                myQuoteFollowUpPM.OwnerUserId = this.OwnerUserId;
                myQuoteFollowUpPM.Note = this.Notes;
                myQuoteFollowUpPM.Done = this.EntityPM.Done;
                myQuoteFollowUpPM.IsNew = this.EntityPM.IsNew;
                myQuoteFollowUpPM.LegType = this.EntityPM.LegType;
                this.QuotePM.AddQuoteFollowUpPM(myQuoteFollowUpPM);
            }
            else if (this.ShipmentPM) {
                var myShipmentFollowUpPM = new ShipmentFollowUpPM_1.ShipmentFollowUpPM(null);
                myShipmentFollowUpPM.Tenant = this.ShipmentPM.Tenant;
                myShipmentFollowUpPM.ShipmentId = this.ShipmentPM.Id;
                myShipmentFollowUpPM.EventTypeId = this.EntityPM.EventTypeId;
                myShipmentFollowUpPM.EventTypeFollowUpName = this.EntityPM.EventTypeFollowUpName;
                myShipmentFollowUpPM.ManualActivatedFollowUp = this.EntityPM.ManualActivatedFollowUp;
                myShipmentFollowUpPM.Date = this.Date;
                myShipmentFollowUpPM.OwnerUserId = this.OwnerUserId;
                myShipmentFollowUpPM.Note = this.Notes;
                myShipmentFollowUpPM.Done = this.EntityPM.Done;
                myShipmentFollowUpPM.IsNew = this.EntityPM.IsNew;
                myShipmentFollowUpPM.LegType = this.EntityPM.LegType;
                this.ShipmentPM.AddShipmentFollowUp(myShipmentFollowUpPM);
            }
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    };
    AddFollowupComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddFollowupComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddFollowupComponent);
    return AddFollowupComponent;
}(BaseComponent_1.BaseComponent));
exports.AddFollowupComponent = AddFollowupComponent;
//# sourceMappingURL=AddFollowupComponent.js.map