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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PackagesTabComponent_1 = require("../../../ShipmentPackages/Components/Packages/PackagesTabComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AddEditOnCarriageComponent = /** @class */ (function (_super) {
    __extends(AddEditOnCarriageComponent, _super);
    function AddEditOnCarriageComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.IsConnectedHouse = false;
        _this.IsFCLEntity = false;
        _this.ItemsSource = [];
        _this.DepartureHeader = "Departure";
        _this.ArrivalHeader = "Arrival";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = false;
        _this.CarrierDependencyProperty1 = null;
        _this.searchText = null;
        _this.oldFollowups = [];
        _this.InitServices();
        return _this;
    }
    AddEditOnCarriageComponent.prototype.InitServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myVesselListService = new VesselListService_1.VesselListService();
    };
    AddEditOnCarriageComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.Clone();
        this.SetDefaultValues();
        this.SetUIProperties();
        this.SetDependencies();
        this.ComputeHeaders();
        this.BuildData();
    };
    AddEditOnCarriageComponent.prototype.SetDefaultValues = function () {
        this.IsFCLEntity = Tools_1.AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            this.IsConnectedHouse = true;
            this.OnCarriageFromPortId = this.EntityPM.ToPortId;
        }
    };
    AddEditOnCarriageComponent.prototype.ComputeHeaders = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageFromPortName)) {
            this.DepartureHeader = "Departure from " + this.EntityPM.OnCarriageFromPortName;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageToPortName)) {
            this.ArrivalHeader = "Arrival to " + this.EntityPM.OnCarriageToPortName;
        }
    };
    AddEditOnCarriageComponent.prototype.BuildData = function () {
        var _this = this;
        this.ItemsSource = [];
        var myItems;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            myItems = this.EntityPM.ShipmentPackages.filter(function (f) { return f.ContainerNumber != null && f.ContainerNumber.toLowerCase().startsWith(_this.SearchText.toLowerCase()); });
        }
        else {
            myItems = this.EntityPM.ShipmentPackages;
        }
        myItems.forEach(function (item) {
            _this.ItemsSource.push(new OnCarriagePackageItem(item, _this));
        });
    };
    AddEditOnCarriageComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isTransportFieldEnabled = false;
        var isCarrierNumberFieldEnabled = false;
        if (isEditingEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageTransportModeId)) {
                isTransportFieldEnabled = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageCarrierId)) {
                isCarrierNumberFieldEnabled = true;
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("OnCarriageTransportModeId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageFromPortId", this.ObjectTableName, isTransportFieldEnabled && this.IsConnectedHouse == false);
        this.UIProperties.SetEnabled("OnCarriageToPortId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageCarrierId", this.ObjectTableName, isTransportFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageCarrierNumber", this.ObjectTableName, isCarrierNumberFieldEnabled);
        this.UIProperties.SetEnabled("OnCarriageVesselId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageETD", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageETA", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageATD", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageATA", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetVisibility("OnCarriageVesselId", this.ObjectTableName, this.OnCarriageTransportModeId == "O" ? true : false);
        this.SetUIProperties_RequiredFields();
        this.SetUIProperties_ValidateActualDates();
    };
    AddEditOnCarriageComponent.prototype.SetUIProperties_RequiredFields = function () {
        this.UIProperties.SetRequired("OnCarriageTransportModeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageTransportModeId) ? true : false);
        this.UIProperties.SetRequired("OnCarriageFromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("OnCarriageToPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageToPortId) ? true : false);
    };
    AddEditOnCarriageComponent.prototype.SetUIProperties_ValidateActualDates = function () {
        this.UIProperties.SetValidity("OnCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("OnCarriageATA", this.ObjectTableName, true, null);
        if (!Tools_1.DateTool.IsActualDateValid(this.OnCarriageATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATD"));
            this.UIProperties.SetValidity("OnCarriageATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.OnCarriageATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATA"));
            this.UIProperties.SetValidity("OnCarriageATA", this.ObjectTableName, false, errorMessage);
        }
    };
    AddEditOnCarriageComponent.prototype.SetDependencies = function () {
        var myResult = null;
        switch (this.OnCarriageTransportModeId) {
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
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageTransportModeId", {
        get: function () { return this.EntityPM.OnCarriageTransportModeId; },
        set: function (value) {
            if (this.EntityPM.OnCarriageTransportModeId != value) {
                this.EntityPM.OnCarriageTransportModeId = value;
                if (!this.IsConnectedHouse) {
                    this.OnCarriageFromPortId = null;
                }
                this.OnCarriageToPortId = null;
                this.OnCarriageCarrierId = null;
                this.OnCarriageCarrierNumber = null;
                this.OnCarriageVesselId = null;
                this.SetUIProperties();
                this.SetDependencies();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageAdditionalTransportModeCode", {
        get: function () { return this.EntityPM.OnCarriageAdditionalTransportModeCode; },
        set: function (value) {
            if (this.EntityPM.OnCarriageAdditionalTransportModeCode != value) {
                this.EntityPM.OnCarriageAdditionalTransportModeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageFromPortId", {
        get: function () { return this.EntityPM.OnCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.OnCarriageFromPortId != value) {
                this.EntityPM.OnCarriageFromPortId = value;
                this.SetUIProperties_RequiredFields();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.OnCarriageFromPortChanged(this.EntityPM, null);
                    this.ComputeHeaders();
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.OnCarriageFromPortChanged(_this.EntityPM, list);
                                _this.ComputeHeaders();
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        list = myResponse2.Result;
                                        Tools_2.RoutingHelper.OnCarriageFromPortChanged(_this.EntityPM, list);
                                        _this.ComputeHeaders();
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
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageToPortId", {
        get: function () { return this.EntityPM.OnCarriageToPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.OnCarriageToPortId != value) {
                this.EntityPM.OnCarriageToPortId = value;
                this.SetUIProperties_RequiredFields();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.OnCarriageToPortCode = null;
                    this.EntityPM.OnCarriageToPortName = null;
                    this.EntityPM.OnCarriageToPortCountryCode = null;
                    this.EntityPM.OnCarriageToPortCountryName = null;
                    this.ComputeHeaders();
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.OnCarriageToPortCode = list.Code;
                                _this.EntityPM.OnCarriageToPortName = list.EnglishName;
                                _this.EntityPM.OnCarriageToPortCountryCode = list.CountryCode;
                                _this.EntityPM.OnCarriageToPortCountryName = list.CountryName;
                                _this.ComputeHeaders();
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        list = myResponse2.Result;
                                        if (list) {
                                            _this.EntityPM.OnCarriageToPortCode = list.Code;
                                            _this.EntityPM.OnCarriageToPortName = list.EnglishName;
                                            _this.EntityPM.OnCarriageToPortCountryCode = list.CountryCode;
                                            _this.EntityPM.OnCarriageToPortCountryName = list.CountryName;
                                            _this.ComputeHeaders();
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
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageCarrierId", {
        get: function () { return this.EntityPM.OnCarriageCarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.OnCarriageCarrierId != value) {
                this.EntityPM.OnCarriageCarrierId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.OnCarriageCarrierCode = null;
                    this.EntityPM.OnCarriageCarrierName = null;
                    this.EntityPM.OnCarriageCarrierWebSite = null;
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.OnCarriageCarrierCode = list.Code;
                                _this.EntityPM.OnCarriageCarrierName = list.EnglishName;
                                _this.EntityPM.OnCarriageCarrierWebSite = list.WebSite;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageCarrierNumber", {
        get: function () { return this.EntityPM.OnCarriageCarrierNumber; },
        set: function (value) {
            if (this.EntityPM.OnCarriageCarrierNumber != value) {
                this.EntityPM.OnCarriageCarrierNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageVesselId", {
        get: function () { return this.EntityPM.OnCarriageVesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.OnCarriageVesselId != value) {
                this.EntityPM.OnCarriageVesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.OnCarriageVesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.OnCarriageVesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageETD", {
        get: function () { return this.EntityPM.OnCarriageETD; },
        set: function (value) {
            if (this.EntityPM.OnCarriageETD != value) {
                this.EntityPM.OnCarriageETD = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageETA", {
        get: function () { return this.EntityPM.OnCarriageETA; },
        set: function (value) {
            if (this.EntityPM.OnCarriageETA != value) {
                this.EntityPM.OnCarriageETA = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageATD", {
        get: function () { return this.EntityPM.OnCarriageATD; },
        set: function (value) {
            if (this.EntityPM.OnCarriageATD != value) {
                this.EntityPM.OnCarriageATD = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "OnCarriageATA", {
        get: function () { return this.EntityPM.OnCarriageATA; },
        set: function (value) {
            if (this.EntityPM.OnCarriageATA != value) {
                this.EntityPM.OnCarriageATA = value;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "SplitOnCarriage", {
        get: function () { return this.EntityPM.SplitOnCarriage; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.SplitOnCarriage != value) {
                this.EntityPM.SplitOnCarriage = value;
                this.EntityPM.ShipmentPackages.forEach(function (item) {
                    if (item.OnCarriageETD == null) {
                        item.OnCarriageETD = _this.OnCarriageETD;
                    }
                    if (item.OnCarriageATD == null) {
                        item.OnCarriageATD = _this.OnCarriageATD;
                    }
                    if (item.OnCarriageETA == null) {
                        item.OnCarriageETA = _this.OnCarriageETA;
                    }
                    if (item.OnCarriageATA == null) {
                        item.OnCarriageATA = _this.OnCarriageATA;
                    }
                });
                this.BuildData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditOnCarriageComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditOnCarriageComponent.prototype.SearchTextChanged = function (text) {
        this.SearchText = text;
        this.BuildData();
    };
    AddEditOnCarriageComponent.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "OnCarriageETD": {
                this.OnCarriageATD = Tools_1.DateTool.GetDateParts(this.OnCarriageETD).DateObject;
                break;
            }
            case "OnCarriageETA": {
                this.OnCarriageATA = Tools_1.DateTool.GetDateParts(this.OnCarriageETA).DateObject;
                break;
            }
        }
    };
    AddEditOnCarriageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditOnCarriageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageTransportModeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.TransportMode")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.From")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.OnCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.To")));
        }
        if (!this.SplitOnCarriage) {
            this.EntityPM.ShipmentPackages.forEach(function (item) {
                item.OnCarriageETD = null;
                item.OnCarriageATD = null;
                item.OnCarriageETA = null;
                item.OnCarriageATA = null;
            });
        }
        // Series Dates
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "OnCarriage");
        // Actual Dates
        if (!Tools_1.DateTool.IsActualDateValid(this.OnCarriageATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATD")));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.OnCarriageATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATA")));
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.OnCarriageAdditionalTransportModeCode) {
                this.EntityPM.ShipmentPackages.forEach(function (item) {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.DeliveryId)) {
                        item.DeliveryTransportModeCode = _this.OnCarriageAdditionalTransportModeCode;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(item.EmptyContainerReturnId)) {
                        item.ECRTransportModeCode = _this.OnCarriageAdditionalTransportModeCode;
                    }
                });
            }
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AddEditOnCarriageComponent.prototype.EditContainer = function (itemComponent) {
        var tabComponent = new PackagesTabComponent_1.PackagesTabComponent(this.FatherComponent.entityArgs, new EntityResourceService_1.EntityResourceService());
        var packagecomponent = new PackagesTabComponent_1.ShipmentPackageItem(itemComponent.EntityPM, tabComponent);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.O.EditContainer");
        logWindow.Width = 940;
        logWindow.Height = 550;
        logWindow.DataContext = packagecomponent;
        logWindow.Show("./ShipmentModules/ShipmentPackages/Components/Packages/AddEditOceanPackageComponent");
    };
    AddEditOnCarriageComponent.prototype.Clone = function () {
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
        this.myCloner.AddField('OnCarriageTransportModeId');
        this.myCloner.AddField('OnCarriageFromPortId');
        this.myCloner.AddField('OnCarriageToPortId');
        this.myCloner.AddField('OnCarriageCarrierId');
        this.myCloner.AddField('OnCarriageCarrierNumber');
        this.myCloner.AddField('OnCarriageVesselId');
        this.myCloner.AddField('OnCarriageETD');
        this.myCloner.AddField('OnCarriageETA');
        this.myCloner.AddField('OnCarriageATD');
        this.myCloner.AddField('OnCarriageATA');
        this.myCloner.AddField('SplitOnCarriage');
        this.myCloner.AddEntity(this.EntityPM);
        this.entityCloner = new Cloner_1.Cloner(this.EntityPM);
        this.entityCloner.AddField('Transshipment3ToPortId');
        this.entityCloner.AddField('Transshipment3ToPortCode');
        this.entityCloner.AddField('Transshipment3ToPortName');
        this.entityCloner.AddField('Transshipment3ToPortCountryCode');
        this.entityCloner.AddField('Transshipment3ToPortCountryName');
        this.entityCloner.AddField('Transshipment2ToPortId');
        this.entityCloner.AddField('Transshipment2ToPortCode');
        this.entityCloner.AddField('Transshipment2ToPortName');
        this.entityCloner.AddField('Transshipment2ToPortCountryCode');
        this.entityCloner.AddField('Transshipment2ToPortCountryName');
        this.entityCloner.AddField('Transshipment1ToPortId');
        this.entityCloner.AddField('Transshipment1ToPortCode');
        this.entityCloner.AddField('Transshipment1ToPortName');
        this.entityCloner.AddField('Transshipment1ToPortCountryCode');
        this.entityCloner.AddField('Transshipment1ToPortCountryName');
        this.entityCloner.AddField('MainCarriageToPortId');
        this.entityCloner.AddField('MainCarriageToPortCode');
        this.entityCloner.AddField('MainCarriageToPortName');
        this.entityCloner.AddField('MainCarriageToPortCountryCode');
        this.entityCloner.AddField('MainCarriageToPortCountryName');
        this.entityCloner.AddField('ToCountryId');
        this.entityCloner.AddField('ToCountryIsEC');
        this.entityCloner.AddField('FinalDistenationPortId');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortId');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortCode');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortName');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortCountryCode');
        this.entityCloner.AddField('MainCarriageFinalDestinationPortCountryName');
        this.entityCloner.AddEntity(this.EntityPM);
    };
    AddEditOnCarriageComponent.prototype.RejectChanges = function () {
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
        this.ItemsSource.forEach(function (item) {
            item.RejectChanges();
        });
        this.myCloner.RejectChanges();
        this.entityCloner.RejectChanges();
    };
    AddEditOnCarriageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditOnCarriageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditOnCarriageComponent);
    return AddEditOnCarriageComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditOnCarriageComponent = AddEditOnCarriageComponent;
var OnCarriagePackageItem = /** @class */ (function (_super) {
    __extends(OnCarriagePackageItem, _super);
    function OnCarriagePackageItem(item, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "ShipmentPackage";
        _this.EntityPM = item;
        _this.ShipmentPM = fatherComponent.EntityPM;
        _this.Clone();
        _this.ComputeDepartureArrival();
        return _this;
    }
    OnCarriagePackageItem.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('OnCarriageETD');
        this.myCloner.AddField('OnCarriageETA');
        this.myCloner.AddField('OnCarriageATD');
        this.myCloner.AddField('OnCarriageATA');
        this.myCloner.AddEntity(this.EntityPM);
    };
    OnCarriagePackageItem.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    OnCarriagePackageItem.prototype.ComputeDepartureArrival = function () {
        var myDepartureColor = Tools_1.FontTool.Orange;
        var myArrivalColor = Tools_1.FontTool.Orange;
        var myDepartureDate;
        var myArrivalDate;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var myETD = this.EntityPM.OnCarriageETD;
        var myATD = this.EntityPM.OnCarriageATD;
        var myETA = this.EntityPM.OnCarriageETA;
        var myATA = this.EntityPM.OnCarriageATA;
        if (myATD != null) {
            myDepartureDate = myATD;
            myDepartureColor = Tools_1.FontTool.Green;
        }
        else if (myETD != null) {
            myDepartureDate = myETD;
            if (Tools_1.DateTool.GetDateParts(myDepartureDate).DateTicks < Tools_1.DateTool.GetDateParts(todayDate).DateTicks) {
                myDepartureColor = Tools_1.FontTool.Red;
            }
        }
        if (myATA != null) {
            myArrivalDate = myATA;
            myArrivalColor = Tools_1.FontTool.Green;
        }
        else if (myETA != null) {
            myArrivalDate = myETA;
            if (Tools_1.DateTool.GetDateParts(myArrivalDate).DateTicks < Tools_1.DateTool.GetDateParts(todayDate).DateTicks) {
                myArrivalColor = Tools_1.FontTool.Red;
            }
        }
        this.DepartureDate = myDepartureDate;
        this.DepartureColor = myDepartureColor;
        this.ArrivalDate = myArrivalDate;
        this.ArrivalColor = myArrivalColor;
    };
    OnCarriagePackageItem.prototype.OnDateComponentClosed = function (dateComponent) {
        if (dateComponent) {
            this.EntityPM = dateComponent.EntityPM;
            this.ComputeDepartureArrival();
        }
    };
    Object.defineProperty(OnCarriagePackageItem.prototype, "ContainerNumber", {
        get: function () { return this.EntityPM.ContainerNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OnCarriagePackageItem.prototype, "ContainerType", {
        get: function () { return this.EntityPM.PackageTypeCode; },
        enumerable: true,
        configurable: true
    });
    return OnCarriagePackageItem;
}(BaseComponent_1.BaseComponent));
exports.OnCarriagePackageItem = OnCarriagePackageItem;
//# sourceMappingURL=AddEditOnCarriageComponent.js.map