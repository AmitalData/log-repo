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
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var VesselListService_1 = require("../../../../Common/Services/StandardLists/VesselListService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AddEditPreCarriageComponent = /** @class */ (function (_super) {
    __extends(AddEditPreCarriageComponent, _super);
    function AddEditPreCarriageComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.IsConnectedHouse = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = true;
        _this.CarrierDependencyProperty1 = null;
        _this.oldFollowups = [];
        _this.InitServices();
        return _this;
    }
    AddEditPreCarriageComponent.prototype.InitServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myVesselListService = new VesselListService_1.VesselListService();
    };
    AddEditPreCarriageComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.Clone();
        this.SetDefaultValues();
        this.SetUIProperties();
        this.SetDependencies();
    };
    AddEditPreCarriageComponent.prototype.SetDefaultValues = function () {
        if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            this.IsConnectedHouse = true;
            this.PreCarriageToPortId = this.EntityPM.MainCarriageFromPortId;
        }
    };
    AddEditPreCarriageComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        if (isEditingEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId)) {
                isTransportFieldEnabled = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("PreCarriageTransportModeId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageFromPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageToPortId", this.ObjectTableName, isTransportFieldEnabled && this.IsConnectedHouse == false);
        this.UIProperties.SetEnabled("PreCarriageCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("PreCarriageVesselId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageETD", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageETA", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageATD", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PreCarriageATA", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetVisibility("PreCarriageVesselId", this.ObjectTableName, this.PreCarriageTransportModeId == "O" ? true : false);
        this.SetUIProperties_RequiredFields();
        this.SetUIProperties_ValidateActualDates();
    };
    AddEditPreCarriageComponent.prototype.SetUIProperties_RequiredFields = function () {
        this.UIProperties.SetRequired("PreCarriageTransportModeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId) ? true : false);
        this.UIProperties.SetRequired("PreCarriageFromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("PreCarriageToPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageToPortId) ? true : false);
    };
    AddEditPreCarriageComponent.prototype.SetUIProperties_ValidateActualDates = function () {
        this.UIProperties.SetValidity("PreCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("PreCarriageATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.PreCarriageATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("PreCarriageATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.PreCarriageATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("PreCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    };
    AddEditPreCarriageComponent.prototype.SetDependencies = function () {
        var myResult = null;
        switch (this.PreCarriageTransportModeId) {
            case "A": {
                myResult = "AL";
                break;
            }
            case "O": {
                myResult = "SL";
                break;
            }
            case "I": {
                myResult = "TR";
                break;
            }
        }
        this.CarrierDependencyProperty1 = myResult;
    };
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageTransportModeId", {
        get: function () { return this.EntityPM.PreCarriageTransportModeId; },
        set: function (value) {
            if (this.EntityPM.PreCarriageTransportModeId != value) {
                this.EntityPM.PreCarriageTransportModeId = value;
                this.PreCarriageFromPortId = null;
                if (!this.IsConnectedHouse) {
                    this.PreCarriageToPortId = null;
                }
                this.PreCarriageCarrierId = null;
                this.PreCarriageCarrierNumber = null;
                this.PreCarriageVesselId = null;
                this.SetUIProperties();
                this.SetDependencies();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageFromPortId", {
        get: function () { return this.EntityPM.PreCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PreCarriageFromPortId != value) {
                this.EntityPM.PreCarriageFromPortId = value;
                this.SetUIProperties_RequiredFields();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.PreCarriageFromPortCode = null;
                    this.EntityPM.PreCarriageFromPortName = null;
                    this.EntityPM.PreCarriageFromPortCountryCode = null;
                    this.EntityPM.PreCarriageFromPortCountryName = null;
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.PreCarriageFromPortCode = list.Code;
                                _this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                                _this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                                _this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        list = myResponse2.Result;
                                        if (list) {
                                            _this.EntityPM.PreCarriageFromPortCode = list.Code;
                                            _this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                                            _this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                                            _this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                                        }
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
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageToPortId", {
        get: function () { return this.EntityPM.PreCarriageToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PreCarriageToPortId != value) {
                this.EntityPM.PreCarriageToPortId = value;
                this.SetUIProperties_RequiredFields();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.PreCarriageToPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.PreCarriageToPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        list = myResponse2.Result;
                                        Tools_2.RoutingHelper.PreCarriageToPortChanged(_this.EntityPM, list);
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
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageCarrierId", {
        get: function () { return this.EntityPM.PreCarriageCarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PreCarriageCarrierId != value) {
                this.EntityPM.PreCarriageCarrierId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.PreCarriageCarrierCode = null;
                    this.EntityPM.PreCarriageCarrierName = null;
                    this.EntityPM.PreCarriageCarrierWebSite = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.PreCarriageCarrierCode = list.Code;
                                _this.EntityPM.PreCarriageCarrierName = list.EnglishName;
                                _this.EntityPM.PreCarriageCarrierWebSite = list.WebSite;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageCarrierNumber", {
        get: function () { return this.EntityPM.PreCarriageCarrierNumber; },
        set: function (value) {
            if (this.EntityPM.PreCarriageCarrierNumber != value) {
                this.EntityPM.PreCarriageCarrierNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageVesselId", {
        get: function () { return this.EntityPM.PreCarriageVesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PreCarriageVesselId != value) {
                this.EntityPM.PreCarriageVesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.PreCarriageVesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.PreCarriageVesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageETD", {
        get: function () { return this.EntityPM.PreCarriageETD; },
        set: function (value) {
            if (this.EntityPM.PreCarriageETD != value) {
                this.EntityPM.PreCarriageETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageETA", {
        get: function () { return this.EntityPM.PreCarriageETA; },
        set: function (value) {
            if (this.EntityPM.PreCarriageETA != value) {
                this.EntityPM.PreCarriageETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageATD", {
        get: function () { return this.EntityPM.PreCarriageATD; },
        set: function (value) {
            if (this.EntityPM.PreCarriageATD != value) {
                this.EntityPM.PreCarriageATD = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditPreCarriageComponent.prototype, "PreCarriageATA", {
        get: function () { return this.EntityPM.PreCarriageATA; },
        set: function (value) {
            if (this.EntityPM.PreCarriageATA != value) {
                this.EntityPM.PreCarriageATA = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditPreCarriageComponent.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "PreCarriageETD": {
                this.PreCarriageATD = Tools_1.DateTool.GetDateParts(this.PreCarriageETD).DateObject;
                break;
            }
            case "PreCarriageETA": {
                this.PreCarriageATA = Tools_1.DateTool.GetDateParts(this.PreCarriageETA).DateObject;
                break;
            }
        }
    };
    AddEditPreCarriageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPreCarriageComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.PreCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }
        // Series Dates
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "PreCarriage");
        // Actual Dates
        if (!Tools_1.DateTool.IsActualDateValid(this.PreCarriageATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.PreCarriageATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditPreCarriageComponent.prototype.Clone = function () {
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
        this.myCloner.AddField('PreCarriageTransportModeId');
        this.myCloner.AddField('PreCarriageFromPortId');
        this.myCloner.AddField('PreCarriageToPortId');
        this.myCloner.AddField('PreCarriageCarrierId');
        this.myCloner.AddField('PreCarriageCarrierNumber');
        this.myCloner.AddField('PreCarriageVesselId');
        this.myCloner.AddField('PreCarriageETD');
        this.myCloner.AddField('PreCarriageETA');
        this.myCloner.AddField('PreCarriageATD');
        this.myCloner.AddField('PreCarriageATA');
        this.myCloner.AddEntity(this.EntityPM);
        this.entityCloner = new Cloner_1.Cloner(this.EntityPM);
        this.entityCloner.AddField('FromCountryId');
        this.entityCloner.AddField('FromCountryIsEC');
        this.entityCloner.AddField('MainCarriageFromPortId');
        this.entityCloner.AddField('MainCarriageFromPortCode');
        this.entityCloner.AddField('MainCarriageFromPortName');
        this.entityCloner.AddField('MainCarriageFromPortCountryCode');
        this.entityCloner.AddField('MainCarriageFromPortCountryName');
        this.entityCloner.AddEntity(this.EntityPM);
    };
    AddEditPreCarriageComponent.prototype.RejectChanges = function () {
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
        this.entityCloner.RejectChanges();
    };
    AddEditPreCarriageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPreCarriageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPreCarriageComponent);
    return AddEditPreCarriageComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPreCarriageComponent = AddEditPreCarriageComponent;
//# sourceMappingURL=AddEditPreCarriageComponent.js.map