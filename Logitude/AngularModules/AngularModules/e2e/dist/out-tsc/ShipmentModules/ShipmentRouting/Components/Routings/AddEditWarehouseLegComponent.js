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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var WarehouseHelper_1 = require("../../../../Warehouse/Helpers/WarehouseHelper");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var AddEditWarehouseLegComponent = /** @class */ (function (_super) {
    __extends(AddEditWarehouseLegComponent, _super);
    function AddEditWarehouseLegComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsNewLeg = false;
        _this.IsFCLEntity = false;
        _this.TransportModeId = null;
        _this.ValidationErrorsList = [];
        _this.IsShowNewWarehouseEntryButton = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = true;
        _this.IsFirmCodeVisible = true;
        _this.oldFollowups = [];
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.InitServices();
        return _this;
    }
    AddEditWarehouseLegComponent.prototype.InitServices = function () {
        this.myAddressListService = new AddressListService_1.AddressListService();
    };
    AddEditWarehouseLegComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.LegType = args['LegType'];
        this.IsNewLeg = args['IsNewLeg'];
        if (this.EntityPM) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WarehouseEntry", "Module")) {
                    this.IsShowNewWarehouseEntryButton = true;
                }
            }
        }
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.WarehouseAddressList = this.FatherComponent.WarehouseAddressList;
        this.SetUIProperties();
        this.Clone();
        if (this.IsNewLeg) {
            if (this.LegType == "WarehouseLeg_Pickups") {
                if (this.EntityPM.ShipmentPickUps.length > 0) {
                    var FirstPickup = this.EntityPM.ShipmentPickUps.sort(function (a, b) { return a.PickUpDeliveryNumber.toLowerCase() == b.PickUpDeliveryNumber.toLowerCase() ? 0 : a.PickUpDeliveryNumber.toLowerCase() < b.PickUpDeliveryNumber.toLowerCase() ? -1 : 1; })[0];
                    if (FirstPickup) {
                        this.WarehouseLegExpectedEntryDate = FirstPickup.ETA;
                        this.WarehouseLegActualEntryDate = FirstPickup.ATA;
                    }
                }
            }
        }
    };
    AddEditWarehouseLegComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.IsEditingEnabled = isEditingEnabled;
        this.IsFirmCodeVisible = (this.TenantPM.CountryCode.toUpperCase()) == "US" ? true : false;
        this.UIProperties.SetEnabled("WarehouseLegWarehouseId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegAddressId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegTerminalCode", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegRemarks", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegLastFreeDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegExpectedEntryDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegExpectedReleaseDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegActualEntryDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegActualReleaseDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("WarehouseLegReference", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetRequired("WarehouseLegWarehouseId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId) ? true : false);
        this.SetUIProperties_ValidateActualDates();
    };
    AddEditWarehouseLegComponent.prototype.SetUIProperties_ValidateActualDates = function () {
        this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, true, null);
        if (this.LegType == "WarehouseLeg_Pickups") {
            var date = Tools_1.DateTool.GetCurrentDateAsUtc();
            if (this.IsDateBigger(this.WarehouseLegActualEntryDate, date)) {
                var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate"));
                this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, false, errorMessage);
            }
            if (this.IsDateBigger(this.WarehouseLegActualReleaseDate, date)) {
                var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate"));
                this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, false, errorMessage);
            }
        }
        else {
            if (!Tools_1.DateTool.IsActualDateValid(this.WarehouseLegActualEntryDate)) {
                var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate"));
                this.UIProperties.SetValidity("WarehouseLegActualEntryDate", this.ObjectTableName, false, errorMessage);
            }
            if (!Tools_1.DateTool.IsActualDateValid(this.WarehouseLegActualReleaseDate)) {
                var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate"));
                this.UIProperties.SetValidity("WarehouseLegActualReleaseDate", this.ObjectTableName, false, errorMessage);
            }
        }
    };
    AddEditWarehouseLegComponent.prototype.IsDateBigger = function (date1, date2) {
        var myResult = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(date1) && !Tools_1.AppTool.IsNullOrEmpty(date2)) {
            var Date1Parts = Tools_1.DateTool.GetDateParts(date1);
            var Date2Parts = Tools_1.DateTool.GetDateParts(date2);
            var Date1Ticks = (Date1Parts.Day * 1) + (Date1Parts.Month * 30) + (Date1Parts.Year * 365);
            var Date2Ticks = (Date2Parts.Day * 1) + (Date2Parts.Month * 30) + (Date2Parts.Year * 365);
            if (Date1Ticks > Date2Ticks) {
                myResult = true;
            }
        }
        return myResult;
    };
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegWarehouseId", {
        get: function () { return this.EntityPM.WarehouseLegWarehouseId; },
        set: function (newValue) {
            if (this.EntityPM.WarehouseLegWarehouseId != newValue) {
                this.EntityPM.WarehouseLegWarehouseId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.GetAddress();
                }
                else {
                    this.WarehouseLegAddressId = null;
                    this.FatherComponent.WarehouseLegTerminalName = "";
                }
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditWarehouseLegComponent.prototype.GetAddress = function () {
        var _this = this;
        var myService = new CardListService_1.CardListService();
        myService.getSingle(this.WarehouseLegWarehouseId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result = myResponse.Result;
                    if (result) {
                        _this.WarehouseLegAddressId = result.MainAddressId;
                        _this.FatherComponent.WarehouseLegTerminalName = result.EnglishName;
                        if (_this.IsFirmCodeVisible) {
                            _this.WarehouseLegTerminalCode = result.FirmCode;
                        }
                    }
                }
            }
        });
    };
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseAddressList", {
        get: function () { return this.myWarehouseAddressList; },
        set: function (newValue) {
            this.myWarehouseAddressList = newValue;
            this.FatherComponent.WarehouseAddressList = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegAddressId", {
        get: function () { return this.EntityPM.WarehouseLegAddressId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.WarehouseLegAddressId != newValue) {
                this.EntityPM.WarehouseLegAddressId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.WarehouseAddressList = null;
                }
                else {
                    this.myAddressListService.getSingle(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.WarehouseAddressList = myResponse.Result;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegTerminalCode", {
        get: function () { return this.EntityPM.WarehouseLegTerminalCode; },
        set: function (newValue) {
            if (this.EntityPM.WarehouseLegTerminalCode != newValue) {
                this.EntityPM.WarehouseLegTerminalCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegReference", {
        get: function () { return this.EntityPM.WarehouseLegReference; },
        set: function (newValue) {
            if (this.EntityPM.WarehouseLegReference != newValue) {
                this.EntityPM.WarehouseLegReference = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegRemarks", {
        get: function () { return this.EntityPM.WarehouseLegRemarks; },
        set: function (newValue) {
            if (this.EntityPM.WarehouseLegRemarks != newValue) {
                this.EntityPM.WarehouseLegRemarks = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegLastFreeDate", {
        get: function () { return this.EntityPM.WarehouseLegLastFreeDate; },
        set: function (newValue) {
            if (this.EntityPM.WarehouseLegLastFreeDate != newValue) {
                this.EntityPM.WarehouseLegLastFreeDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegExpectedEntryDate", {
        get: function () { return this.EntityPM.WarehouseLegExpectedEntryDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegExpectedEntryDate != value) {
                this.EntityPM.WarehouseLegExpectedEntryDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegExpectedReleaseDate", {
        get: function () { return this.EntityPM.WarehouseLegExpectedReleaseDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegExpectedReleaseDate != value) {
                this.EntityPM.WarehouseLegExpectedReleaseDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegActualEntryDate", {
        get: function () { return this.EntityPM.WarehouseLegActualEntryDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegActualEntryDate != value) {
                this.EntityPM.WarehouseLegActualEntryDate = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegActualReleaseDate", {
        get: function () { return this.EntityPM.WarehouseLegActualReleaseDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegActualReleaseDate != value) {
                this.EntityPM.WarehouseLegActualReleaseDate = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "TerminalAvailable", {
        get: function () { return this.EntityPM.TerminalAvailable; },
        set: function (value) {
            if (this.EntityPM.TerminalAvailable != value) {
                this.EntityPM.TerminalAvailable = value;
                this.FatherComponent.EntityPM.TerminalAvailable = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegCutOffDate", {
        get: function () { return this.EntityPM.WarehouseLegCutOffDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegCutOffDate != value) {
                this.EntityPM.WarehouseLegCutOffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditWarehouseLegComponent.prototype, "WarehouseLegVGMCutOffDate", {
        get: function () { return this.EntityPM.WarehouseLegVGMCutOffDate; },
        set: function (value) {
            if (this.EntityPM.WarehouseLegVGMCutOffDate != value) {
                this.EntityPM.WarehouseLegVGMCutOffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditWarehouseLegComponent.prototype.NewWarehouseEntryButtonClicked = function () {
        var windowArgs = {};
        windowArgs.ExpectedEntryDate = this.WarehouseLegExpectedEntryDate;
        windowArgs.ActualEntryDate = this.WarehouseLegActualEntryDate;
        windowArgs.WarehouseId = this.WarehouseLegWarehouseId;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsNotSetWarehouseIdForWarehouseLegShipment = true;
        var warehouseHelper = new WarehouseHelper_1.WarehouseHelper();
        warehouseHelper.ShowNewWarehouseEntryComponent(windowArgs);
    };
    AddEditWarehouseLegComponent.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "WarehouseLegExpectedReleaseDate": {
                this.WarehouseLegActualReleaseDate = Tools_1.DateTool.GetDateParts(this.WarehouseLegExpectedReleaseDate).DateObject;
                break;
            }
            case "WarehouseLegExpectedEntryDate": {
                this.WarehouseLegActualEntryDate = Tools_1.DateTool.GetDateParts(this.WarehouseLegExpectedEntryDate).DateObject;
                break;
            }
        }
    };
    AddEditWarehouseLegComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditWarehouseLegComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.WarehouseLegWarehouseId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegWarehouseId")));
        }
        // Series Dates
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, this.LegType);
        // Actual Dates
        if (this.LegType == "WarehouseLeg_Pickups") {
            var date = Tools_1.DateTool.GetCurrentDateAsUtc();
            if (this.IsDateBigger(this.WarehouseLegActualEntryDate, date)) {
                errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate")));
            }
            if (this.IsDateBigger(this.WarehouseLegActualReleaseDate, date)) {
                errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate")));
            }
        }
        else {
            if (!Tools_1.DateTool.IsActualDateValid(this.WarehouseLegActualEntryDate)) {
                errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualEntryDate")));
            }
            if (!Tools_1.DateTool.IsActualDateValid(this.WarehouseLegActualReleaseDate)) {
                errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.WarehouseLegActualReleaseDate")));
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditWarehouseLegComponent.prototype.Clone = function () {
        var _this = this;
        this.EntityPM.FollowUps.forEach(function (item) {
            var oldItem = new ShipmentFollowUpPM_1.ShipmentFollowUpPM(null);
            oldItem.Id = item.Id;
            oldItem.Date = item.Date;
            oldItem.Deleted = item.Deleted;
            oldItem.Done = item.Done;
            oldItem.DoneDateTime = item.DoneDateTime;
            oldItem.DoneNote = item.DoneNote;
            oldItem.EntityDateId = item.EntityDateId;
            oldItem.EventTypeFollowUpName = item.EventTypeFollowUpName;
            oldItem.EventTypeId = item.EventTypeId;
            oldItem.Tenant = item.Tenant;
            oldItem.ExternalDocumentId = item.ExternalDocumentId;
            oldItem.IsNew = item.IsNew;
            oldItem.JobId = item.JobId;
            oldItem.LegType = item.LegType;
            oldItem.ManualActivatedFollowUp = item.ManualActivatedFollowUp;
            oldItem.Note = item.Note;
            oldItem.OwnerUserId = item.OwnerUserId;
            oldItem.OwnerUserName = item.OwnerUserName;
            oldItem.ShipmentId = item.ShipmentId;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            _this.oldFollowups.push(oldItem);
        });
        this.myCloner = new Cloner_1.Cloner(this);
        this.myCloner.AddField('WarehouseLegWarehouseId');
        this.myCloner.AddField('WarehouseLegAddressId');
        this.myCloner.AddField('WarehouseLegTerminalCode');
        this.myCloner.AddField('WarehouseLegExpectedReleaseDate');
        this.myCloner.AddField('WarehouseLegExpectedEntryDate');
        this.myCloner.AddField('WarehouseLegActualEntryDate');
        this.myCloner.AddField('WarehouseLegActualReleaseDate');
        this.myCloner.AddField('WarehouseLegLastFreeDate');
        this.myCloner.AddField('WarehouseLegRemarks');
        this.myCloner.AddField('WarehouseLegTerminalName');
        this.myCloner.AddField('TerminalAvailable');
        this.myCloner.AddField('WarehouseLegCutOffDate');
        this.myCloner.AddField('WarehouseLegVGMCutOffDate');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.WarehouseAddressList);
        this.myCloner.AddEntity(this.FatherComponent.WarehouseAddressList);
    };
    AddEditWarehouseLegComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldFollowups.forEach(function (item) {
            var existingItem = _this.EntityPM.FollowUps.filter(function (f) { return f.LegType == item.LegType; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.FollowUps.forEach(function (item) {
            var oldItem = _this.oldFollowups.filter(function (f) { return f.LegType == item.LegType; })[0];
            if (oldItem == null) {
                addedItems.push(item);
            }
        });
        if (addedItems.length > 0 || removedItems.length > 0) {
            addedItems.forEach(function (item) {
                _this.EntityPM.RemoveShipmentFollowUp(item);
            });
            removedItems.forEach(function (item) {
                _this.EntityPM.AddShipmentFollowUp(item);
            });
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
        this.myCloner.RejectChanges();
    };
    AddEditWarehouseLegComponent = __decorate([
        core_1.Component({
            moduleId: './ShipmentModules/ShipmentRouting/Components/Routings/',
            templateUrl: 'AddEditWarehouseLegComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditWarehouseLegComponent);
    return AddEditWarehouseLegComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditWarehouseLegComponent = AddEditWarehouseLegComponent;
//# sourceMappingURL=AddEditWarehouseLegComponent.js.map