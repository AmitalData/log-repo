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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ShipmentValidator_1 = require("../../../../Shipment/Validators/ShipmentValidator");
var Tools_2 = require("../../../../Shipment/Tools");
var ShipmentFollowUpPM_1 = require("../../../../Shipment/EntityPMs/ShipmentFollowUpPM");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var VesselListService_1 = require("../../../../Common/Services/StandardLists/VesselListService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../../../Common/Args");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var AWBStackDomainService_1 = require("../../../../Common/Services/AWBStackDomainService");
var AddEditMainCarriageComponent = /** @class */ (function (_super) {
    __extends(AddEditMainCarriageComponent, _super);
    function AddEditMainCarriageComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.LabelWidth = 100;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DirectionId = null;
        _this.TransportModeId = null;
        _this.FromTextCodeLabel = null;
        _this.Via1Label = null;
        _this.Via2Label = null;
        _this.Via3Label = null;
        _this.ToTextCodeLabel = null;
        _this.ConnectedMasterText = null;
        _this.Leg0TextCodeLabel = null;
        _this.Leg1TextCodeLabel = null;
        _this.Leg2TextCodeLabel = null;
        _this.Leg3TextCodeLabel = null;
        _this.CarrierTextCode = null;
        _this.CarrierNumberTextCode = null;
        _this.MasterTextCode = null;
        _this.MasterDateTextCode = null;
        _this.CarrierDependencyProperty1 = null;
        _this.ETDLabel = null;
        _this.ETALabel = null;
        _this.ATDLabel = null;
        _this.ATALabel = null;
        //get IsCloseMasterInfoVisible() {
        //    var myResult = false;
        //    if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
        //        myResult = true;
        //    }
        //    return myResult;
        //}
        _this.IsEditingEnabled = true;
        _this.IsEditingEntityEnabled = true;
        _this.IsPortsEditingEnabled = true;
        _this.IsCloseMasterInfoVisible = false;
        _this.IsFromStockVisible = false;
        _this.IsFromStockEnabled = false;
        _this.IsReturnStockVisible = false;
        _this.IsReturnStockEnabled = false;
        _this.IsAddInterlineEnabled = false;
        // Interline
        _this.isInterlineAdded = false;
        _this.isGetFromStock = false;
        _this.SaveCompletedEvent = null;
        // Validate Master
        _this.MasterFieldValidityMessage = null;
        _this.oldFollowups = [];
        _this.InitServices();
        return _this;
    }
    AddEditMainCarriageComponent.prototype.InitServices = function () {
        this.myPortListService = new PortListService_1.PortListService();
        this.myCardListService = new CardListService_1.CardListService();
        this.myAirlineListService = new AirlineListService_1.AirlineListService();
        this.myVesselListService = new VesselListService_1.VesselListService();
        this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
    };
    AddEditMainCarriageComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.LabelWidth = this.EntityPM.TransportModeId == "I" ? 115 : 100;
        if (this.EntityPM.TransportModeId == "A") {
            this.LabelWidth = 80;
        }
        else if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "E") {
            this.LabelWidth = 150;
        }
        this.InitializeComponent();
        this.SetUIProperties();
        this.Clone();
    };
    AddEditMainCarriageComponent.prototype.InitializeComponent = function () {
        this.DirectionId = this.EntityPM.DirectionId;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.Via1Label = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Via1");
        this.Via2Label = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Via2");
        this.Via3Label = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Via3");
        this.ETDLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ETD");
        this.ETALabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ETA");
        this.ATDLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATD");
        this.ATALabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.ATA");
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Gateway");
                this.ToTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Destination");
                this.Leg0TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg1");
                this.Leg1TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg2");
                this.Leg2TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg3");
                this.Leg3TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg4");
                this.CarrierTextCode = "Shipment.O.Routings.Airline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.FlightNo";
                this.MasterTextCode = "Shipment.O.Routings.MAWB";
                this.MasterDateTextCode = "Shipment.O.Routings.MAWBDate";
                this.CarrierDependencyProperty1 = "AL";
                break;
            }
            case "O": {
                this.FromTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.LoadingPort");
                this.ToTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.DischargePort");
                this.Leg0TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriage");
                this.Leg1TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Transshipment1");
                this.Leg2TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Transshipment2");
                this.Leg3TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Transshipment3");
                this.CarrierTextCode = "Shipment.O.Routings.Shippingline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.VoyageNo";
                this.MasterTextCode = "Shipment.O.Routings.OBL";
                this.MasterDateTextCode = "Shipment.O.Routings.OBLDate";
                this.CarrierDependencyProperty1 = "SL";
                break;
            }
            case "I": {
                this.FromTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.From");
                this.ToTextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.To");
                this.Leg0TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg1");
                this.Leg1TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg2");
                this.Leg2TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg3");
                this.Leg3TextCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MainCarriageLeg4");
                this.CarrierTextCode = "Shipment.O.Routings.Trucker";
                this.CarrierNumberTextCode = "Shipment.O.Routings.TruckNo";
                this.MasterTextCode = "Shipment.O.Routings.CMR/RWB#";
                this.MasterDateTextCode = "Shipment.O.Routings.CMR/RWBDate";
                this.CarrierDependencyProperty1 = "TR";
                break;
            }
        }
        this.ConnectedMasterText = "This master is departed and connected to house shipments, can't edit " + this.FromTextCodeLabel + " or " + this.ToTextCodeLabel;
    };
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "IsCloseHouseInfoVisible", {
        get: function () {
            var myResult = false;
            if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    AddEditMainCarriageComponent.prototype.SetUIProperties = function () {
        var isEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.IsEditingEntityEnabled = isEditingEnabled;
        if (isEditingEnabled) {
            if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                isEditingEnabled = false;
            }
        }
        var isPortsEditingEnabled = false;
        if (isEditingEnabled) {
            isPortsEditingEnabled = true;
            if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                isPortsEditingEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEditingEnabled = false;
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.IsPortsEditingEnabled = isPortsEditingEnabled;
        this.SetUIProperties_Ports();
        this.SetUIProperties_MainCarriage();
        this.SetUIProperties_Transshipment1();
        this.SetUIProperties_Transshipment2();
        this.SetUIProperties_Transshipment3();
        this.SetUIProperties_ValidateActualDates();
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_Ports = function () {
        var isPortsEditingEnabled = false;
        var isMainPortsEnabled = false;
        var isPortVia1Enabled = false;
        var isPortVia2Enabled = false;
        var isPortVia3Enabled = false;
        this.IsCloseMasterInfoVisible = false;
        if (this.IsEditingEnabled) {
            isPortsEditingEnabled = true;
            if (this.EntityPM.ShipmentLevelCode == "H" && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                isPortsEditingEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEditingEnabled = false;
            }
        }
        if (isPortsEditingEnabled) {
            isMainPortsEnabled = true;
            isPortVia1Enabled = true;
            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                //isMainPortsEnabled = false;
                if (this.EntityPM.StatusWeight >= 60) {
                    isMainPortsEnabled = false;
                    this.IsCloseMasterInfoVisible = true;
                }
            }
            if (this.Transshipment1FromPortId != null || this.Transshipment2FromPortId != null || this.Transshipment3FromPortId != null) {
                isPortVia2Enabled = true;
            }
            if (this.Transshipment2FromPortId != null || this.Transshipment3FromPortId != null) {
                isPortVia3Enabled = true;
            }
        }
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isMainPortsEnabled);
        this.UIProperties.SetEnabled("MainCarriageFinalDestinationPortId", this.ObjectTableName, isMainPortsEnabled);
        this.UIProperties.SetEnabled("Transshipment1FromPortId", this.ObjectTableName, isPortVia1Enabled);
        this.UIProperties.SetEnabled("Transshipment2FromPortId", this.ObjectTableName, isPortVia2Enabled);
        this.UIProperties.SetEnabled("Transshipment3FromPortId", this.ObjectTableName, isPortVia3Enabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageFinalDestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFinalDestinationPortId) ? true : false);
        //var isPortVia1Required = AppTool.IsNullOrEmpty(this.Transshipment1FromPortId) && !AppTool.IsNullOrEmpty(this.Transshipment2FromPortId) ? true : false;
        //var isPortVia2Required = AppTool.IsNullOrEmpty(this.Transshipment2FromPortId) && !AppTool.IsNullOrEmpty(this.Transshipment3FromPortId) ? true : false;
        //this.UIProperties.SetRequired("Transshipment1FromPortId", this.ObjectTableName, isPortVia1Required);
        //this.UIProperties.SetRequired("Transshipment2FromPortId", this.ObjectTableName, isPortVia2Required);
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_MainCarriage = function () {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isLegFieldsEnabled = this.IsEditingEnabled;
        var isInterlineEnabled = this.IsEditingEnabled;
        var isMasterEnabled = this.IsEditingEnabled;
        var isETDEnabled = this.IsEditingEnabled;
        if (this.IsEditingEnabled) {
            isLegFieldsEnabled = false;
            isMasterEnabled = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isCarrierEnabled = false;
                isInterlineEnabled = false;
                isETDEnabled = false;
            }
            else {
                if (this.TransportModeId == "A") {
                    var isTakenFromStock = (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) ? true : false;
                    if (isTakenFromStock || !Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                        isInterlineEnabled = false;
                        isCarrierEnabled = false;
                    }
                    if (!isTakenFromStock) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) || !Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                            isMasterEnabled = true;
                        }
                    }
                }
                else {
                    isMasterEnabled = true;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    isLegFieldsEnabled = true;
                }
                //else {
                //    isMasterEnabled = false;
                //}
            }
        }
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("InterlineId", this.ObjectTableName, isInterlineEnabled);
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isMasterEnabled);
        this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, isETDEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("MAWBOBLDate", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("CutoffDate", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageVesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageATA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OBLTypeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("DocumentsClosingDate", this.ObjectTableName, this.IsEditingEnabled);
        this.SetUIProperties_Interline();
        this.SetUIProperties_StockButton();
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_Transshipment1 = function () {
        var isLegFieldsEnabled = this.IsEditingEnabled;
        if (isLegFieldsEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1CarrierId)) {
                isLegFieldsEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Transshipment1CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1AdditionalMAWBOBLBL", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1VesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment1ATA", this.ObjectTableName, this.IsEditingEnabled);
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_Transshipment2 = function () {
        var isLegFieldsEnabled = this.IsEditingEnabled;
        if (isLegFieldsEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2CarrierId)) {
                isLegFieldsEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Transshipment2CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2AdditionalMAWBOBLBL", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2VesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment2ATA", this.ObjectTableName, this.IsEditingEnabled);
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_Transshipment3 = function () {
        var isLegFieldsEnabled = this.IsEditingEnabled;
        if (isLegFieldsEnabled) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment3CarrierId)) {
                isLegFieldsEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Transshipment3CarrierId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3CarrierPrefix", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment3CarrierNumber", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment3AdditionalMAWBOBLBL", this.ObjectTableName, isLegFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment3VesselId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ETD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ETA", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ATD", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("Transshipment3ATA", this.ObjectTableName, this.IsEditingEnabled);
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_ValidateActualDates = function () {
        this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment1ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment1ATA", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment2ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment2ATA", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment3ATD", this.ObjectTableName, true, null);
        this.UIProperties.SetValidity("Transshipment3ATA", this.ObjectTableName, true, null);
        // MainCarriage
        if (!Tools_1.DateTool.IsActualDateValid(this.MainCarriageATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("MainCarriageATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.MainCarriageATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("MainCarriageATA", this.ObjectTableName, false, errorMessage);
        }
        //Transshipment1
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment1ATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("Transshipment1ATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment1ATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("Transshipment1ATA", this.ObjectTableName, false, errorMessage);
        }
        //Transshipment2
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment2ATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("Transshipment2ATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment2ATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("Transshipment2ATA", this.ObjectTableName, false, errorMessage);
        }
        //Transshipment3
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment3ATD)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATDLabel);
            this.UIProperties.SetValidity("Transshipment3ATD", this.ObjectTableName, false, errorMessage);
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment3ATA)) {
            var errorMessage = Tools_1.DateTool.ActualDateMessage.replace("Field", this.ATALabel);
            this.UIProperties.SetValidity("Transshipment3ATA", this.ObjectTableName, false, errorMessage);
        }
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_Interline = function () {
        var isAddInterlineEnabled = this.IsEditingEnabled;
        if (isAddInterlineEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isAddInterlineEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) && this.MainCarriageIsFromStack) {
                isAddInterlineEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                isAddInterlineEnabled = false;
            }
        }
        this.IsAddInterlineEnabled = isAddInterlineEnabled;
    };
    AddEditMainCarriageComponent.prototype.SetUIProperties_StockButton = function () {
        var isFromStockVisible = false;
        var isReturnStockVisible = false;
        var isFromStockEnabled = this.IsEditingEnabled;
        var isReturnStockEnabled = this.IsEditingEnabled;
        if (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) {
            isReturnStockVisible = true;
        }
        isFromStockVisible = !isReturnStockVisible;
        if (this.IsEditingEnabled) {
            if (this.MainCarriageCarrierId == null && this.InterlineId == null) {
                isFromStockEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                isFromStockEnabled = false;
            }
            else if (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) {
                isFromStockEnabled = false;
            }
            else if (this.EntityPM.BookingId != null) {
                isFromStockEnabled = false;
                isReturnStockEnabled = false;
            }
        }
        this.IsFromStockVisible = isFromStockVisible;
        this.IsFromStockEnabled = isFromStockEnabled;
        this.IsReturnStockVisible = isReturnStockVisible;
        this.IsReturnStockEnabled = isReturnStockEnabled;
    };
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageFromPortId", {
        // Ports
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFromPortId != value) {
                this.EntityPM.MainCarriageFromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
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
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1FromPortId", {
        get: function () { return this.EntityPM.Transshipment1FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment1FromPortId != value) {
                this.EntityPM.Transshipment1FromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.Transshipment1FromPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.Transshipment1FromPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.Transshipment1FromPortChanged(_this.EntityPM, list);
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
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2FromPortId", {
        get: function () { return this.EntityPM.Transshipment2FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment2FromPortId != value) {
                this.EntityPM.Transshipment2FromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.Transshipment2FromPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.Transshipment2FromPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.Transshipment2FromPortChanged(_this.EntityPM, list);
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
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3FromPortId", {
        get: function () { return this.EntityPM.Transshipment3FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment3FromPortId != value) {
                this.EntityPM.Transshipment3FromPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.Transshipment3FromPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.Transshipment3FromPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.Transshipment3FromPortChanged(_this.EntityPM, list);
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
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageFinalDestinationPortId", {
        get: function () { return this.EntityPM.MainCarriageFinalDestinationPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFinalDestinationPortId != value) {
                this.EntityPM.MainCarriageFinalDestinationPortId = value;
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
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
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageCarrierId", {
        // Main Carrier
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageCarrierId != value) {
                this.EntityPM.MainCarriageCarrierId = value;
                this.SetUIProperties_MainCarriage();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.MainCarriageCarrierPrefix = null;
                    this.MainCarriageCarrierNumber = null;
                    Tools_2.RoutingHelper.MainCarriageCarrierChanged(this.EntityPM, null);
                    if (this.TransportModeId == "A") {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                            this.CarrierIsCheckDigit = false;
                            this.CarrierIsLimitedLength = false;
                            this.AirlinePrefix = null;
                        }
                        this.Master = null;
                        this.CarrierIsChampRegistered = false;
                        this.CarrierIsGLSHKRegistered = false;
                        Tools_2.ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                    }
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (_this.TransportModeId == "A") {
                                    _this.MainCarriageCarrierPrefix = list.Code;
                                }
                                Tools_2.RoutingHelper.MainCarriageCarrierChanged(_this.EntityPM, list);
                                if (_this.TransportModeId == "A") {
                                    // dont get from chach: if user choosed from tenant0 it wont get it
                                    _this.myAirlineListService.getSingle(value).subscribe(function (myAirlineListResponse) {
                                        if (myAirlineListResponse != null) {
                                            var myAirlineList = myAirlineListResponse.Result;
                                            if (myAirlineList != null) {
                                                if (Tools_1.AppTool.IsNullOrEmpty(_this.InterlineId)) {
                                                    _this.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                                    _this.CarrierIsLimitedLength = myAirlineList.LimitedLength;
                                                    var myPrefix = null;
                                                    if (!Tools_1.AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                        myPrefix = myAirlineList.Prefix.toString().trim();
                                                        myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                                                    }
                                                    _this.AirlinePrefix = myPrefix;
                                                }
                                                _this.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                                _this.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;
                                                _this.myPartnersDomainService.GetAirlineByCode(myAirlineList.Code, 0).subscribe(function (myResponse) {
                                                    if (!myResponse.HasError) {
                                                        Tools_2.ShipmentTool.MapTenantZeroAirline(_this.EntityPM, myResponse.Result);
                                                    }
                                                });
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageCarrierCode", {
        get: function () { return this.EntityPM.MainCarriageCarrierCode; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierCode != value) {
                this.EntityPM.MainCarriageCarrierCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageCarrierName", {
        get: function () { return this.EntityPM.MainCarriageCarrierName; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierName != value) {
                this.EntityPM.MainCarriageCarrierName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageCarrierPrefix", {
        get: function () { return this.EntityPM.MainCarriageCarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierPrefix != value) {
                this.EntityPM.MainCarriageCarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageCarrierWebSite", {
        get: function () { return this.EntityPM.MainCarriageCarrierWebSite; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierWebSite != value) {
                this.EntityPM.MainCarriageCarrierWebSite = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        set: function (value) {
            if (this.EntityPM.MainCarriageCarrierNumber != value) {
                this.EntityPM.MainCarriageCarrierNumber = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "CarrierIsCheckDigit", {
        get: function () { return this.EntityPM.CarrierIsCheckDigit; },
        set: function (value) {
            if (this.EntityPM.CarrierIsCheckDigit != value) {
                this.EntityPM.CarrierIsCheckDigit = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "CarrierIsLimitedLength", {
        get: function () { return this.EntityPM.CarrierIsLimitedLength; },
        set: function (value) {
            if (this.EntityPM.CarrierIsLimitedLength != value) {
                this.EntityPM.CarrierIsLimitedLength = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "CarrierIsChampRegistered", {
        get: function () { return this.EntityPM.CarrierIsChampRegistered; },
        set: function (value) {
            if (this.EntityPM.CarrierIsChampRegistered != value) {
                this.EntityPM.CarrierIsChampRegistered = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "CarrierIsGLSHKRegistered", {
        get: function () { return this.EntityPM.CarrierIsGLSHKRegistered; },
        set: function (value) {
            if (this.EntityPM.CarrierIsGLSHKRegistered != value) {
                this.EntityPM.CarrierIsGLSHKRegistered = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1CarrierId", {
        // Carrier
        get: function () { return this.EntityPM.Transshipment1CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment1CarrierId != value) {
                this.EntityPM.Transshipment1CarrierId = value;
                this.SetUIProperties_Transshipment1();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Transshipment1CarrierPrefix = null;
                    this.Transshipment1CarrierNumber = null;
                    this.Transshipment1AdditionalMAWBOBLBL = null;
                    Tools_2.RoutingHelper.Transshipment1CarrierChanged(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (_this.TransportModeId == "A") {
                                    _this.Transshipment1CarrierPrefix = list.Code;
                                }
                                Tools_2.RoutingHelper.Transshipment1CarrierChanged(_this.EntityPM, list);
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2CarrierId", {
        get: function () { return this.EntityPM.Transshipment2CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment2CarrierId != value) {
                this.EntityPM.Transshipment2CarrierId = value;
                this.SetUIProperties_Transshipment2();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Transshipment2CarrierPrefix = null;
                    this.Transshipment2CarrierNumber = null;
                    this.Transshipment2AdditionalMAWBOBLBL = null;
                    Tools_2.RoutingHelper.Transshipment2CarrierChanged(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (_this.TransportModeId == "A") {
                                    _this.Transshipment2CarrierPrefix = list.Code;
                                }
                                Tools_2.RoutingHelper.Transshipment2CarrierChanged(_this.EntityPM, list);
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3CarrierId", {
        get: function () { return this.EntityPM.Transshipment3CarrierId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment3CarrierId != value) {
                this.EntityPM.Transshipment3CarrierId = value;
                this.SetUIProperties_Transshipment3();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Transshipment3CarrierPrefix = null;
                    this.Transshipment3CarrierNumber = null;
                    this.Transshipment3AdditionalMAWBOBLBL = null;
                    Tools_2.RoutingHelper.Transshipment3CarrierChanged(this.EntityPM, null);
                }
                else {
                    this.myCardListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                if (_this.TransportModeId == "A") {
                                    _this.Transshipment3CarrierPrefix = list.Code;
                                }
                                Tools_2.RoutingHelper.Transshipment3CarrierChanged(_this.EntityPM, list);
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1CarrierPrefix", {
        // Carrier Prefix
        get: function () { return this.EntityPM.Transshipment1CarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.Transshipment1CarrierPrefix != value) {
                this.EntityPM.Transshipment1CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment2CarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.Transshipment2CarrierPrefix != value) {
                this.EntityPM.Transshipment2CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment3CarrierPrefix; },
        set: function (value) {
            if (this.EntityPM.Transshipment3CarrierPrefix != value) {
                this.EntityPM.Transshipment3CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1CarrierNumber", {
        // Carrier Number
        get: function () { return this.EntityPM.Transshipment1CarrierNumber; },
        set: function (value) {
            if (this.EntityPM.Transshipment1CarrierNumber != value) {
                this.EntityPM.Transshipment1CarrierNumber = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2CarrierNumber", {
        get: function () { return this.EntityPM.Transshipment2CarrierNumber; },
        set: function (value) {
            if (this.EntityPM.Transshipment2CarrierNumber != value) {
                this.EntityPM.Transshipment2CarrierNumber = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3CarrierNumber", {
        get: function () { return this.EntityPM.Transshipment3CarrierNumber; },
        set: function (value) {
            if (this.EntityPM.Transshipment3CarrierNumber != value) {
                this.EntityPM.Transshipment3CarrierNumber = Tools_1.AppTool.IsNullOrEmpty(value) ? value : value.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "AirlinePrefix", {
        // Master   
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (value) {
            if (this.EntityPM.AirlinePrefix != value) {
                this.EntityPM.AirlinePrefix = value;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (value) {
            if (this.EntityPM.Master != value) {
                this.EntityPM.Master = value;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "TrailerNumber", {
        get: function () { return this.EntityPM.TrailerNumber; },
        set: function (value) {
            if (this.EntityPM.TrailerNumber != value) {
                this.EntityPM.TrailerNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        set: function (newValue) {
            if (this.EntityPM.LongMaster != newValue) {
                this.EntityPM.LongMaster = newValue;
                this.ValidateMasterField();
                this.SetUIProperties_MainCarriage();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MAWBOBLDate", {
        get: function () { return this.EntityPM.MAWBOBLDate; },
        set: function (value) {
            if (this.EntityPM.MAWBOBLDate != value) {
                this.EntityPM.MAWBOBLDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "CutoffDate", {
        get: function () { return this.EntityPM.CutoffDate; },
        set: function (value) {
            if (this.EntityPM.CutoffDate != value) {
                this.EntityPM.CutoffDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1AdditionalMAWBOBLBL", {
        get: function () { return this.EntityPM.Transshipment1AdditionalMAWBOBLBL; },
        set: function (value) {
            if (this.EntityPM.Transshipment1AdditionalMAWBOBLBL != value) {
                this.EntityPM.Transshipment1AdditionalMAWBOBLBL = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2AdditionalMAWBOBLBL", {
        get: function () { return this.EntityPM.Transshipment2AdditionalMAWBOBLBL; },
        set: function (value) {
            if (this.EntityPM.Transshipment2AdditionalMAWBOBLBL != value) {
                this.EntityPM.Transshipment2AdditionalMAWBOBLBL = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3AdditionalMAWBOBLBL", {
        get: function () { return this.EntityPM.Transshipment3AdditionalMAWBOBLBL; },
        set: function (value) {
            if (this.EntityPM.Transshipment3AdditionalMAWBOBLBL != value) {
                this.EntityPM.Transshipment3AdditionalMAWBOBLBL = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "DocumentsClosingDate", {
        get: function () { return this.EntityPM.DocumentsClosingDate; },
        set: function (value) {
            if (this.EntityPM.DocumentsClosingDate != value) {
                this.EntityPM.DocumentsClosingDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "OBLTypeCode", {
        get: function () { return this.EntityPM.OBLTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.OBLTypeCode != newValue) {
                this.EntityPM.OBLTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageVesselId", {
        // Vessels    
        get: function () { return this.EntityPM.MainCarriageVesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageVesselId != value) {
                this.EntityPM.MainCarriageVesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.MainCarriageVesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.MainCarriageVesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1VesselId", {
        get: function () { return this.EntityPM.Transshipment1VesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment1VesselId != value) {
                this.EntityPM.Transshipment1VesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.Transshipment1VesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.Transshipment1VesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2VesselId", {
        get: function () { return this.EntityPM.Transshipment2VesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment2VesselId != value) {
                this.EntityPM.Transshipment2VesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.Transshipment2VesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.Transshipment2VesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3VesselId", {
        get: function () { return this.EntityPM.Transshipment3VesselId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment3VesselId != value) {
                this.EntityPM.Transshipment3VesselId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.EntityPM.Transshipment3VesselName = null;
                }
                else {
                    this.myVesselListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.EntityPM.Transshipment3VesselName = list.EnglishName;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageETD", {
        // Dates
        get: function () { return this.EntityPM.MainCarriageETD; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageETD != newValue) {
                this.EntityPM.MainCarriageETD = newValue;
                this.SetFlightDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageATD", {
        get: function () { return this.EntityPM.MainCarriageATD; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageATD != newValue) {
                this.EntityPM.MainCarriageATD = newValue;
                this.SetFlightDate();
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageETA", {
        get: function () { return this.EntityPM.MainCarriageETA; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageETA != newValue) {
                this.EntityPM.MainCarriageETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageATA", {
        get: function () { return this.EntityPM.MainCarriageATA; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageATA != newValue) {
                this.EntityPM.MainCarriageATA = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditMainCarriageComponent.prototype.SetFlightDate = function () {
        var flightDate = this.MainCarriageETD;
        var isFlightDateActual = false;
        if (this.MainCarriageATD != null) {
            flightDate = this.MainCarriageATD;
            isFlightDateActual = true;
        }
        this.EntityPM.FlightDate = flightDate;
        this.EntityPM.IsFlightDateActual = isFlightDateActual;
    };
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1ETD", {
        get: function () { return this.EntityPM.Transshipment1ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ETD != newValue) {
                this.EntityPM.Transshipment1ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1ATD", {
        get: function () { return this.EntityPM.Transshipment1ATD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ATD != newValue) {
                this.EntityPM.Transshipment1ATD = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1ETA", {
        get: function () { return this.EntityPM.Transshipment1ETA; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ETA != newValue) {
                this.EntityPM.Transshipment1ETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment1ATA", {
        get: function () { return this.EntityPM.Transshipment1ATA; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ATA != newValue) {
                this.EntityPM.Transshipment1ATA = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2ETD", {
        get: function () { return this.EntityPM.Transshipment2ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ETD != newValue) {
                this.EntityPM.Transshipment2ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2ATD", {
        get: function () { return this.EntityPM.Transshipment2ATD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ATD != newValue) {
                this.EntityPM.Transshipment2ATD = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2ETA", {
        get: function () { return this.EntityPM.Transshipment2ETA; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ETA != newValue) {
                this.EntityPM.Transshipment2ETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment2ATA", {
        get: function () { return this.EntityPM.Transshipment2ATA; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ATA != newValue) {
                this.EntityPM.Transshipment2ATA = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3ETD", {
        get: function () { return this.EntityPM.Transshipment3ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment3ETD != newValue) {
                this.EntityPM.Transshipment3ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3ATD", {
        get: function () { return this.EntityPM.Transshipment3ATD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment3ATD != newValue) {
                this.EntityPM.Transshipment3ATD = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3ETA", {
        get: function () { return this.EntityPM.Transshipment3ETA; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment3ETA != newValue) {
                this.EntityPM.Transshipment3ETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "Transshipment3ATA", {
        get: function () { return this.EntityPM.Transshipment3ATA; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment3ATA != newValue) {
                this.EntityPM.Transshipment3ATA = newValue;
                this.SetUIProperties_ValidateActualDates();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditMainCarriageComponent.prototype.ValidateActualDates = function (errors) {
        // MainCarriage
        if (!Tools_1.DateTool.IsActualDateValid(this.MainCarriageATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg0TextCodeLabel + " " + this.ATDLabel));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.MainCarriageATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg0TextCodeLabel + " " + this.ATALabel));
        }
        //Transshipment1
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment1ATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg1TextCodeLabel + " " + this.ATDLabel));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment1ATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg1TextCodeLabel + " " + this.ATALabel));
        }
        //Transshipment2
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment2ATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg2TextCodeLabel + " " + this.ATDLabel));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment2ATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg2TextCodeLabel + " " + this.ATALabel));
        }
        //Transshipment3
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment3ATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg3TextCodeLabel + " " + this.ATDLabel));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.Transshipment3ATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", this.Leg3TextCodeLabel + " " + this.ATALabel));
        }
    };
    AddEditMainCarriageComponent.prototype.SetActualDateClicked = function (fieldName) {
        switch (fieldName) {
            case "MainCarriageETD": {
                this.MainCarriageATD = Tools_1.DateTool.GetDateParts(this.MainCarriageETD).DateObject;
                break;
            }
            case "MainCarriageETA": {
                this.MainCarriageATA = Tools_1.DateTool.GetDateParts(this.MainCarriageETA).DateObject;
                break;
            }
            case "Transshipment1ETD": {
                this.Transshipment1ATD = Tools_1.DateTool.GetDateParts(this.Transshipment1ETD).DateObject;
                break;
            }
            case "Transshipment1ETA": {
                this.Transshipment1ATA = Tools_1.DateTool.GetDateParts(this.Transshipment1ETA).DateObject;
                break;
            }
            case "Transshipment2ETD": {
                this.Transshipment2ATD = Tools_1.DateTool.GetDateParts(this.Transshipment2ETD).DateObject;
                break;
            }
            case "Transshipment2ETA": {
                this.Transshipment2ATA = Tools_1.DateTool.GetDateParts(this.Transshipment2ETA).DateObject;
                break;
            }
            case "Transshipment3ETD": {
                this.Transshipment3ATD = Tools_1.DateTool.GetDateParts(this.Transshipment3ETD).DateObject;
                break;
            }
            case "Transshipment3ETA": {
                this.Transshipment3ATA = Tools_1.DateTool.GetDateParts(this.Transshipment3ETA).DateObject;
                break;
            }
        }
    };
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "IsInterlineAdded", {
        get: function () { return this.isInterlineAdded; },
        set: function (value) {
            if (this.isInterlineAdded != value) {
                this.isInterlineAdded = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "InterlineId", {
        get: function () { return this.EntityPM.InterlineId; },
        set: function (newValue) {
            if (this.EntityPM.InterlineId != newValue) {
                this.EntityPM.InterlineId = newValue;
                this.SetUIProperties_MainCarriage();
                this.OnInterlineChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditMainCarriageComponent.prototype.AddInterlineClicked = function () {
        this.IsInterlineAdded = true;
    };
    AddEditMainCarriageComponent.prototype.OnInterlineLostFocus = function ($event) {
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
    };
    AddEditMainCarriageComponent.prototype.OnInterlineChanged = function () {
        var _this = this;
        var myAirlineId = this.InterlineId;
        if (Tools_1.AppTool.IsNullOrEmpty(myAirlineId)) {
            myAirlineId = this.MainCarriageCarrierId;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(myAirlineId)) {
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;
            this.AirlinePrefix = null;
        }
        else {
            this.myAirlineListService.getSingleFromCache(myAirlineId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                        _this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;
                        var myPrefix = null;
                        if (!Tools_1.AppTool.IsNullOrEmpty(list.Prefix)) {
                            myPrefix = list.Prefix.toString().trim();
                            myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                        }
                        _this.AirlinePrefix = myPrefix;
                    }
                }
            });
        }
    };
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MAWBStackNumber", {
        // Stock
        get: function () { return this.EntityPM.MAWBStackNumber; },
        set: function (newValue) {
            if (this.EntityPM.MAWBStackNumber != newValue) {
                this.EntityPM.MAWBStackNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MAWBTakenFromStack", {
        get: function () { return this.EntityPM.MAWBTakenFromStack; },
        set: function (newValue) {
            if (this.EntityPM.MAWBTakenFromStack != newValue) {
                this.EntityPM.MAWBTakenFromStack = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MainCarriageIsFromStack", {
        get: function () { return this.EntityPM.MainCarriageIsFromStack; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageIsFromStack != newValue) {
                this.EntityPM.MainCarriageIsFromStack = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMainCarriageComponent.prototype, "MAWBReturnedToStack", {
        get: function () { return this.EntityPM.MAWBReturnedToStack; },
        set: function (newValue) {
            if (this.EntityPM.MAWBReturnedToStack != newValue) {
                this.EntityPM.MAWBReturnedToStack = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditMainCarriageComponent.prototype.AddStockClicked = function (airlineId) {
        if (!Tools_1.AppTool.IsNullOrEmpty(airlineId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit Airline";
            logWindow.ShowEditComponent(airlineId, "Airline", "ALST");
        }
    };
    AddEditMainCarriageComponent.prototype.GetStockClicked = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            var myShipmentValidator = new ShipmentValidator_1.ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.EntityPM);
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myShipmentErrors;
            if (myShipmentErrors.length == 0) {
                this.SetMAWBAirline();
                var windowArgs = new Args_1.GetStackWindowArgs();
                windowArgs.CardId = this.EntityPM.MAWBStackAirlineId;
                windowArgs.ShipperId = this.EntityPM.ShipperId;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 750;
                logWindow.Height = 450;
                logWindow.WindowArgs = windowArgs;
                logWindow.Title = "Select Air Waybill Number";
                logWindow.Show('./Common/Components/Partners/AWBStock/StackSelectionComponent');
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        if (windowArgs.SelectedStack != null) {
                            var stackNumber = windowArgs.SelectedStack.Number;
                            _this.myOldMAWBStackNumber = _this.MAWBStackNumber;
                            _this.myOldMAWBOBLDate = _this.MAWBOBLDate;
                            _this.MAWBTakenFromStack = true;
                            _this.MAWBStackNumber = Tools_1.AppTool.PadLeft(stackNumber.toString(), 8, '0');
                            _this.MAWBOBLDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                            _this.isGetFromStock = true;
                            if (!_this.SaveCompletedEvent) {
                                _this.SaveCompletedEvent = _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                                    if (isSaveSuccess) {
                                        _this.EntityPM = null;
                                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                                        _this.SetUIProperties_MainCarriage();
                                        _this.myCloner.AddField('Master');
                                        _this.myCloner.AddField('MAWBOBLDate');
                                        _this.myCloner.AddField('MAWBStackNumber');
                                        _this.myCloner.AddField('MAWBTakenFromStack');
                                        _this.myCloner.AddField('MAWBReturnedToStack');
                                        _this.myCloner.AddField('MainCarriageIsFromStack');
                                    }
                                    else {
                                        _this.ValidationErrorsList = _this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                                    }
                                    Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                                    _this.SaveCompletedEvent = null;
                                });
                                _this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                        }
                    }
                });
            }
        }
    };
    AddEditMainCarriageComponent.prototype.ReturnStockClicked = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent) {
            if (this.MainCarriageIsFromStack || this.MAWBTakenFromStack) {
                var myShipmentValidator = new ShipmentValidator_1.ShipmentValidator();
                var myShipmentErrors = myShipmentValidator.Validate(this.EntityPM);
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myShipmentErrors;
                if (myShipmentErrors.length == 0) {
                    this.SetMAWBAirline();
                    this.MAWBReturnedToStack = true;
                    this.MAWBStackNumber = this.Master;
                    this.isGetFromStock = false;
                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                            if (isSaveSuccess) {
                                _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                                _this.SetUIProperties_MainCarriage();
                                _this.myCloner.AddField('Master');
                                _this.myCloner.AddField('MAWBOBLDate');
                                _this.myCloner.AddField('MAWBStackNumber');
                                _this.myCloner.AddField('MAWBTakenFromStack');
                                _this.myCloner.AddField('MAWBReturnedToStack');
                                _this.myCloner.AddField('MainCarriageIsFromStack');
                            }
                            else {
                                _this.ValidationErrorsList = _this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                            }
                            Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                            _this.SaveCompletedEvent = null;
                        });
                        this.CurrentSession.CurrentEditComponent.SaveChanges();
                    }
                }
            }
        }
    };
    AddEditMainCarriageComponent.prototype.SetMAWBAirline = function () {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    };
    AddEditMainCarriageComponent.prototype.ValidateMasterField = function () {
        if (this.TransportModeId == "A") {
            this.MasterFieldValidityMessage = null;
            if (Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
            }
            else {
                var myResult = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);
                if (!Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    this.MasterFieldValidityMessage = myResult;
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myResult);
                }
                else {
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
                        this.ValidateMasterFieldIsUsed();
                    }
                }
            }
        }
    };
    AddEditMainCarriageComponent.prototype.ValidateMasterFieldIsUsed = function () {
        var _this = this;
        if (this.TransportModeId == "A") {
            if (this.myShipmentDomainService == null) {
                this.myShipmentDomainService = new ShipmentDomainService_1.ShipmentDomainService();
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                this.myShipmentDomainService.ValidateShipmentMasterFieldExistance(this.EntityPM.Id, this.EntityPM.BookingId, this.EntityPM.Master, this.EntityPM.AirlinePrefix, this.EntityPM.DirectionId, this.EntityPM.TransportModeId, this.EntityPM.ShipmentLevelCode, this.EntityPM.IsCancelled)
                    .subscribe(function (myResult) {
                    if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                        _this.UIProperties.SetValidity("Master", _this.ObjectTableName, true, "");
                    }
                    else {
                        _this.MasterFieldValidityMessage = myResult;
                        _this.UIProperties.SetValidity("Master", _this.ObjectTableName, false, myResult);
                    }
                });
            }
        }
    };
    AddEditMainCarriageComponent.prototype.ValidateMasterStack = function () {
        var _this = this;
        if (this.TransportModeId == "A") {
            if (this.EntityPM != null) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                    if (this.Master.length == 8 && !this.MainCarriageIsFromStack && !this.MAWBTakenFromStack) {
                        if (Tools_1.FormatTool.IsNumeric(this.Master)) {
                            this.StackDomainService.GetMAWBStackPMByNumber(+this.Master).subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    var myStackPM = myResponse.Result;
                                    if (myStackPM != null) {
                                        var myAirlineId = _this.MainCarriageCarrierId;
                                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.InterlineId)) {
                                            myAirlineId = _this.InterlineId;
                                        }
                                        if (myStackPM.AirlineId == myAirlineId) {
                                            if (!Tools_1.AppTool.IsNullOrEmpty(myStackPM.AssignedToId) && myStackPM.AssignedToId != _this.EntityPM.ShipperId) {
                                                var messageWindow = new MessageWindow_1.MessageWindow();
                                                messageWindow.Width = 450;
                                                messageWindow.Height = 190;
                                                messageWindow.Title = "Invalid Master";
                                                messageWindow.Show("AWB is assigned to another shipper");
                                                _this.Master = null;
                                            }
                                            else {
                                                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                                                confirmWindow.Title = "AWB exists in the stock";
                                                confirmWindow.Show("Do you want to get this awb from stock?");
                                                confirmWindow.WindowClosed.subscribe(function (event) {
                                                    if (confirmWindow.Yes) {
                                                        _this.MAWBTakenFromStack = true;
                                                        _this.MAWBStackNumber = Tools_1.AppTool.PadLeft(myStackPM.Number.toString(), 8, '0');
                                                        _this.MAWBOBLDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                                                        _this.SetMAWBAirline();
                                                        _this.isGetFromStock = true;
                                                        if (!_this.SaveCompletedEvent) {
                                                            _this.SaveCompletedEvent = _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                                                                if (isSaveSuccess) {
                                                                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                                                                    _this.SetUIProperties_MainCarriage();
                                                                    _this.myCloner.AddField('Master');
                                                                    _this.myCloner.AddField('MAWBOBLDate');
                                                                    _this.myCloner.AddField('MAWBStackNumber');
                                                                    _this.myCloner.AddField('MAWBTakenFromStack');
                                                                    _this.myCloner.AddField('MAWBReturnedToStack');
                                                                    _this.myCloner.AddField('MainCarriageIsFromStack');
                                                                }
                                                                else {
                                                                    _this.ValidationErrorsList = _this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                                                                }
                                                                Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                                                                _this.SaveCompletedEvent = null;
                                                            });
                                                            _this.CurrentSession.CurrentEditComponent.SaveChanges();
                                                        }
                                                    }
                                                    else {
                                                        _this.Master = null;
                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    };
    AddEditMainCarriageComponent.prototype.MasterLostFocus = function (input) {
        if (this.TransportModeId == "A") {
            this.ValidateMasterStack();
        }
    };
    AddEditMainCarriageComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditMainCarriageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", this.FromTextCodeLabel));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", this.ToTextCodeLabel));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment3FromPortId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
                errors.push("To Add " + this.Via3Label + " you need to add " + this.Via2Label);
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
                errors.push("To Add " + this.Via2Label + " you need to add " + this.Via1Label);
            }
        }
        if (this.TransportModeId == "A") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierPrefix)) {
                    if (this.EntityPM.MainCarriageCarrierPrefix.length != 2) {
                        errors.push("Main Carriage Carrier Code must be 2 characters");
                    }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierPrefix)) {
                    if (this.EntityPM.Transshipment1CarrierPrefix.length != 2) {
                        errors.push("Transshipment1 Carrier Code must be 2 characters");
                    }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierPrefix)) {
                    if (this.EntityPM.Transshipment2CarrierPrefix.length != 2) {
                        errors.push("Transshipment2 Carrier Code must be 2 characters");
                    }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3CarrierId)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3CarrierPrefix)) {
                    if (this.EntityPM.Transshipment3CarrierPrefix.length != 2) {
                        errors.push("Transshipment3 Carrier Code must be 2 characters");
                    }
                }
            }
        }
        else {
            this.MainCarriageCarrierPrefix = null;
            this.Transshipment1CarrierPrefix = null;
            this.Transshipment2CarrierPrefix = null;
            this.Transshipment3CarrierPrefix = null;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MasterFieldValidityMessage)) {
            errors.push(this.MasterFieldValidityMessage);
        }
        // Series Dates
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates(this.EntityPM, errors, "MainCarriage");
        // Actual Dates
        this.ValidateActualDates(errors);
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
                Tools_2.RoutingHelper.RemoveTransshipment1Leg(this.EntityPM);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
                Tools_2.RoutingHelper.RemoveTransshipment2Leg(this.EntityPM);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.Transshipment3FromPortId)) {
                Tools_2.RoutingHelper.RemoveTransshipment3Leg(this.EntityPM);
            }
            var isConfirmingPorts = false;
            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.OriginMainCarriageFromPortId != this.EntityPM.MainCarriageFromPortId) {
                    isConfirmingPorts = true;
                }
                else if (this.EntityPM.OriginFinalDestinationPortId != this.EntityPM.MainCarriageFinalDestinationPortId) {
                    isConfirmingPorts = true;
                }
            }
            if (isConfirmingPorts) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Title = "Ports Changed";
                confirmWindow.Show("Updating the Master shipment ports will update the house shipment accordingly");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        if (!_this.SaveCompletedEvent) {
                            _this.SaveCompletedEvent = _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                                Tools_1.AppTool.KillEventEmitter(_this.SaveCompletedEvent);
                                _this.SaveCompletedEvent = null;
                                if (isSaveSuccess) {
                                    _this.CurrentSession.SessionEvent.emit("ReloadHouses");
                                    _this.CloseOk();
                                }
                                else {
                                    _this.ValidationErrorsList = _this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                                }
                            });
                            _this.CurrentSession.CurrentEditComponent.SaveChanges();
                        }
                    }
                });
            }
            else {
                this.CloseOk();
            }
        }
    };
    AddEditMainCarriageComponent.prototype.CloseOk = function () {
        this.FatherComponent.BuildItemsCollection();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    AddEditMainCarriageComponent.prototype.Clone = function () {
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
        // Left Side
        this.myCloner.AddField('MainCarriageFromPortId');
        this.myCloner.AddField('Transshipment1FromPortId');
        this.myCloner.AddField('Transshipment2FromPortId');
        this.myCloner.AddField('Transshipment3FromPortId');
        this.myCloner.AddField('MainCarriageFinalDestinationPortId');
        // Main Carriage
        this.myCloner.AddField('Master');
        this.myCloner.AddField('MAWBOBLDate');
        this.myCloner.AddField('MAWBStackNumber');
        this.myCloner.AddField('MAWBTakenFromStack');
        this.myCloner.AddField('MAWBReturnedToStack');
        this.myCloner.AddField('MainCarriageIsFromStack');
        this.myCloner.AddField('MainCarriageCarrierCode');
        this.myCloner.AddField('MainCarriageCarrierName');
        this.myCloner.AddField('MainCarriageCarrierPrefix');
        this.myCloner.AddField('MainCarriageCarrierNumber');
        this.myCloner.AddField('MainCarriageCarrierWebSite');
        this.myCloner.AddField('MainCarriageCarrierId');
        this.myCloner.AddField('InterlineId');
        this.myCloner.AddField('AirlinePrefix');
        this.myCloner.AddField('CutoffDate');
        this.myCloner.AddField('MainCarriageVesselId');
        this.myCloner.AddField('MainCarriageETD');
        this.myCloner.AddField('MainCarriageETA');
        this.myCloner.AddField('MainCarriageATD');
        this.myCloner.AddField('MainCarriageATA');
        // Transshipment1
        this.myCloner.AddField('Transshipment1CarrierId');
        this.myCloner.AddField('Transshipment1CarrierPrefix');
        this.myCloner.AddField('Transshipment1CarrierNumber');
        this.myCloner.AddField('Transshipment1AdditionalMAWBOBLBL');
        this.myCloner.AddField('Transshipment1VesselId');
        this.myCloner.AddField('Transshipment1ETD');
        this.myCloner.AddField('Transshipment1ETA');
        this.myCloner.AddField('Transshipment1ATD');
        this.myCloner.AddField('Transshipment1ATA');
        // Transshipment2
        this.myCloner.AddField('Transshipment2CarrierId');
        this.myCloner.AddField('Transshipment2CarrierPrefix');
        this.myCloner.AddField('Transshipment2CarrierNumber');
        this.myCloner.AddField('Transshipment2AdditionalMAWBOBLBL');
        this.myCloner.AddField('Transshipment2VesselId');
        this.myCloner.AddField('Transshipment2ETD');
        this.myCloner.AddField('Transshipment2ETA');
        this.myCloner.AddField('Transshipment2ATD');
        this.myCloner.AddField('Transshipment2ATA');
        // Transshipment3
        this.myCloner.AddField('Transshipment3CarrierId');
        this.myCloner.AddField('Transshipment3CarrierPrefix');
        this.myCloner.AddField('Transshipment3CarrierNumber');
        this.myCloner.AddField('Transshipment3AdditionalMAWBOBLBL');
        this.myCloner.AddField('Transshipment3VesselId');
        this.myCloner.AddField('Transshipment3ETD');
        this.myCloner.AddField('Transshipment3ETA');
        this.myCloner.AddField('Transshipment3ATD');
        this.myCloner.AddField('Transshipment3ATA');
        if (this.TransportModeId == "I") {
            this.myCloner.AddField('TrailerNumber');
        }
        this.myCloner.AddEntity(this.EntityPM);
        if (this.TransportModeId == "A") {
            this.entityCloner = new Cloner_1.Cloner(this.EntityPM);
            this.entityCloner.AddField("CarrierIsCheckDigit");
            this.entityCloner.AddField("CarrierIsLimitedLength");
            this.entityCloner.AddField("CarrierIsChampRegistered");
            this.entityCloner.AddField("CarrierIsGLSHKRegistered");
            this.entityCloner.AddField("TenantZeroAirlineId");
            this.entityCloner.AddField("TenantZeroAirlineTTY");
            this.entityCloner.AddField("TenantZeroAirlinePIMA");
            this.entityCloner.AddField("TenantZeroAirlineChampFWB");
            this.entityCloner.AddField("TenantZeroAirlineChampFHL");
            this.entityCloner.AddField("TenantZeroAirlineChampFSU");
            this.entityCloner.AddField("TenantZeroAirlineChampFVRFVA");
            this.entityCloner.AddField("TenantZeroAirlineChampFSRFSA");
            this.entityCloner.AddField("TenantZeroAirlineChampNeedsRegistration");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFWB");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFHL");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFSU");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFVRFVA");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKFSRFSA");
            this.entityCloner.AddField("TenantZeroAirlineGLSHKNeedsRegistration");
        }
    };
    AddEditMainCarriageComponent.prototype.RejectChanges = function () {
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
        if (this.entityCloner) {
            this.entityCloner.RejectChanges();
        }
        this.myCloner.RejectChanges();
    };
    AddEditMainCarriageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditMainCarriageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditMainCarriageComponent);
    return AddEditMainCarriageComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditMainCarriageComponent = AddEditMainCarriageComponent;
//# sourceMappingURL=AddEditMainCarriageComponent.js.map