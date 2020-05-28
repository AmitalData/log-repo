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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_2 = require("../../../../../Shipment/Tools");
var PortListService_1 = require("../../../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../../../Common/Services/StandardLists/AirlineListService");
var PartnersDomainService_1 = require("../../../../../Common/Services/PartnersDomainService");
var AWBStackDomainService_1 = require("../../../../../Common/Services/AWBStackDomainService");
var ShipmentDomainService_1 = require("../../../../../Shipment/Services/ShipmentDomainService");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../../../Common/Args");
var Args_2 = require("../../../../../CommonModules/CommonFlightsSchedules/Args");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var AWBRoutingsTabComponent = /** @class */ (function (_super) {
    __extends(AWBRoutingsTabComponent, _super);
    function AWBRoutingsTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 110;
        _this.ControlColumnWidth = 165;
        _this.StockColumnWidth = 90;
        _this.TransportModeId = null;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.isSaveRequested = false;
        //get IsCloseMasterInfoVisible() {
        //    var myResult = false;
        //    if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
        //        myResult = true;
        //    }
        //    return myResult;
        //}
        _this.IsEditingEnabled = false;
        _this.IsMainCarrierFieldsEnabled = false;
        _this.IsCloseMasterInfoVisible = false;
        _this.IsFromStockVisible = false;
        _this.IsFromStockEnabled = false;
        _this.IsReturnStockVisible = false;
        _this.IsReturnStockEnabled = false;
        _this.IsAddInterlineEnabled = false;
        // Validate
        _this.ShowWarning_MainCarriageCarrierId = false;
        _this.ShowWarning_MainCarriageCarrierNumber = false;
        _this.ShowWarning_Master = false;
        _this.ShowWarning_MAWBOBLDate = false;
        _this.ShowWarning_MainCarriageETD = false;
        _this.ShowWarning_Transshipment1CarrierId = false;
        _this.ShowWarning_Transshipment2CarrierId = false;
        // InterlineId
        _this.IsInterlineAdded = false;
        // Transshipment1
        _this.transshipment1Carrier = null;
        // Transshipment2
        _this.transshipment2Carrier = null;
        _this.MasterFieldValidityMessage = null;
        // Flights Schedules
        _this.isMainLegFlightSchedules = false;
        _this.isFlightSchedulesRequested = false;
        _this.IsFlightSchedulesVisible = false;
        _this.InitializeServices();
        return _this;
    }
    AWBRoutingsTabComponent.prototype.InitializeServices = function () {
        this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
        if (this.myPortListService == null) {
            this.myPortListService = new PortListService_1.PortListService();
        }
        if (this.myCardService == null) {
            this.myCardService = new CardListService_1.CardListService();
        }
        if (this.myAirlineService == null) {
            this.myAirlineService = new AirlineListService_1.AirlineListService();
        }
        if (this.myPartnersDomainService == null) {
            this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        }
    };
    AWBRoutingsTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.SetLabels();
        this.Listen();
        this.Validate();
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
        this.SetFlightSchedules();
    };
    AWBRoutingsTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
        this.isFlightSchedulesRequested = false;
    };
    AWBRoutingsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                    _this.OnEntityListenSuccess();
                }
                _this.StopListenFlags();
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                    _this.OnEntityListenSuccess();
                }
                _this.StopListenFlags();
            });
        }
    };
    AWBRoutingsTabComponent.prototype.OnEntityListenSuccess = function () {
        if (this.isFlightSchedulesRequested) {
            this.isFlightSchedulesRequested = false;
            this.RunFlightSchedules();
        }
        else if (this.isSaveRequested) {
            this.isSaveRequested = false;
            //if (this.isGetFromStock) {
            //    this.isGetFromStock = false;
            //    this.EntityPM.MAWBTakenFromStack = false;
            //    this.EntityPM.MAWBStackNumber = this.myOldMAWBStackNumber;
            //    this.MAWBOBLDate = this.myOldMAWBOBLDate;
            //}
            this.FireWizardEvent();
            this.SetUIProperties_Carriers();
        }
        //else if (this.isReloadRequested) {
        //    this.isReloadRequested = false;
        //    this.IsInterlineAdded = AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        //    this.Validate();
        //    this.FireWizardEvent();
        //    this.SetUIProperties_Carriers();
        //}
    };
    AWBRoutingsTabComponent.prototype.StopListenFlags = function () {
        //this.isGetFromStock = false;
        this.isSaveRequested = false;
        this.isFlightSchedulesRequested = false;
    };
    AWBRoutingsTabComponent.prototype.Save = function () {
        this.isSaveRequested = true;
        this.Wizard.SaveClicked();
    };
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "IsCloseHouseInfoVisible", {
        // SetUIProperties
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
    AWBRoutingsTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.SetUIProperties_Ports();
        this.SetUIProperties_Carriers();
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_Ports = function () {
        var isPortsEnabled = this.IsEditingEnabled;
        var isPortVia1Enabled = false;
        var isPortVia2Enabled = false;
        this.IsCloseMasterInfoVisible = false;
        if (isPortsEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEnabled = false;
            }
        }
        if (isPortsEnabled) {
            isPortVia1Enabled = true;
            //if (this.MainCarriageFromPortId != null) {
            //    isPortVia1Enabled = true;
            //}
            if (this.Transshipment2FromPortId != null) {
                isPortVia2Enabled = true;
            }
            else if (this.Transshipment1FromPortId != null) {
                isPortVia2Enabled = true;
            }
        }
        if (isPortsEnabled) {
            if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
                if (this.EntityPM.StatusWeight >= 60) {
                    isPortsEnabled = false;
                    this.IsCloseMasterInfoVisible = true;
                }
            }
        }
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("Transshipment1FromPortId", this.ObjectTableName, isPortVia1Enabled);
        this.UIProperties.SetEnabled("Transshipment2FromPortId", this.ObjectTableName, isPortVia2Enabled);
        this.UIProperties.SetEnabled("MainCarriageFinalDestinationPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageFinalDestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFinalDestinationPortId) ? true : false);
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_Carriers = function () {
        this.SetUIProperties_CarrierM();
        this.SetUIProperties_CarrierT1();
        this.SetUIProperties_CarrierT2();
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_CarrierM = function () {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isCarrierFieldsEnabled = this.IsEditingEnabled;
        var isInterlineEnabled = this.IsEditingEnabled;
        var isMasterEnabled = this.IsEditingEnabled;
        var isETDEnabled = this.IsEditingEnabled;
        if (this.IsEditingEnabled) {
            isCarrierFieldsEnabled = false;
            isMasterEnabled = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isCarrierEnabled = false;
                isInterlineEnabled = false;
                isETDEnabled = false;
            }
            else {
                var isTakenFromStock = (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) ? true : false;
                if (isTakenFromStock || !Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                    isInterlineEnabled = false;
                    isCarrierEnabled = false;
                }
                if (!isTakenFromStock) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) || !Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                        isMasterEnabled = true;
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    isCarrierFieldsEnabled = true;
                }
            }
        }
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("InterlineId", this.ObjectTableName, isInterlineEnabled);
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isMasterEnabled);
        this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, isETDEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierPrefix", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("MAWBOBLDate", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageATD", this.ObjectTableName, this.IsEditingEnabled);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Master));
        }
        else {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, false);
        }
        this.SetUIProperties_Interline();
        this.SetUIProperties_StockButton();
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_CarrierT1 = function () {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isCarrierFieldsEnabled = (this.IsEditingEnabled && this.Transshipment1CarrierId != null) ? true : false;
        this.UIProperties.SetEnabled("Transshipment1CarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierPrefix", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierNumber", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1AdditionalMAWBOBLBL", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETD", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment1ATD", this.ObjectTableName, isCarrierEnabled);
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_CarrierT2 = function () {
        var isCarrierEnabled = this.IsEditingEnabled;
        var isCarrierFieldsEnabled = (this.IsEditingEnabled && this.Transshipment2CarrierId != null) ? true : false;
        this.UIProperties.SetEnabled("Transshipment2CarrierId", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierPrefix", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierNumber", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2AdditionalMAWBOBLBL", this.ObjectTableName, isCarrierFieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2ETD", this.ObjectTableName, isCarrierEnabled);
        this.UIProperties.SetEnabled("Transshipment2ATD", this.ObjectTableName, isCarrierEnabled);
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_Interline = function () {
        var isAddInterlineEnabled = this.IsEditingEnabled;
        if (isAddInterlineEnabled) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isAddInterlineEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId) && this.EntityPM.MainCarriageIsFromStack) {
                isAddInterlineEnabled = false;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                isAddInterlineEnabled = false;
            }
        }
        this.IsAddInterlineEnabled = isAddInterlineEnabled;
    };
    AWBRoutingsTabComponent.prototype.SetUIProperties_StockButton = function () {
        var isFromStockVisible = false;
        var isReturnStockVisible = false;
        var isFromStockEnabled = this.IsEditingEnabled;
        var isReturnStockEnabled = this.IsEditingEnabled;
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
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
            else if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
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
    AWBRoutingsTabComponent.prototype.SetLabels = function () {
        switch (this.EntityPM.TransportModeId) {
            case "A": {
                this.FromTextCode = "Shipment.O.Routings.Gateway";
                this.ToTextCode = "Shipment.O.Routings.Destination";
                this.MainCarriageTextCode = "Shipment.O.Routings.MainCarriageLeg1";
                this.Transshipment1TextCode = "Shipment.O.Routings.MainCarriageLeg2";
                this.Transshipment2TextCode = "Shipment.O.Routings.MainCarriageLeg3";
                this.Transshipment3TextCode = "Shipment.O.Routings.MainCarriageLeg4";
                this.CarrierTextCode = "Shipment.O.Routings.Airline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.FlightNo";
                this.MasterTextCode = "Shipment.O.Routings.MAWB";
                this.MasterDateTextCode = "Shipment.O.Routings.MAWBDate";
                break;
            }
            case "O": {
                this.FromTextCode = "Shipment.O.Routings.LoadingPort";
                this.ToTextCode = "Shipment.O.Routings.DischargePort";
                this.MainCarriageTextCode = "Shipment.O.Routings.MainCarriage";
                this.Transshipment1TextCode = "Shipment.O.Routings.Transshipment1";
                this.Transshipment2TextCode = "Shipment.O.Routings.Transshipment2";
                this.Transshipment3TextCode = "Shipment.O.Routings.Transshipment3";
                this.CarrierTextCode = "Shipment.O.Routings.Shippingline";
                this.CarrierNumberTextCode = "Shipment.O.Routings.VoyageNo";
                this.MasterTextCode = "Shipment.O.Routings.OBL";
                this.MasterDateTextCode = "Shipment.O.Routings.OBLDate";
                break;
            }
            case "I": {
                this.FromTextCode = "Shipment.O.Routings.From";
                this.ToTextCode = "Shipment.O.Routings.To";
                this.MainCarriageTextCode = "Shipment.O.Routings.MainCarriageLeg1";
                this.Transshipment1TextCode = "Shipment.O.Routings.MainCarriageLeg2";
                this.Transshipment2TextCode = "Shipment.O.Routings.MainCarriageLeg3";
                this.Transshipment3TextCode = "Shipment.O.Routings.MainCarriageLeg4";
                this.CarrierTextCode = "Shipment.O.Routings.Trucker";
                this.CarrierNumberTextCode = "Shipment.O.Routings.TruckNo";
                this.MasterTextCode = "Shipment.O.Routings.CMR/RWB#";
                this.MasterDateTextCode = "Shipment.O.Routings.CMR/RWBDate";
                break;
            }
        }
    };
    AWBRoutingsTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_PAR();
        this.Wizard.ValidateScreen_ROU();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
        this.Wizard.ValidateScreen_OCI();
    };
    AWBRoutingsTabComponent.prototype.Validate = function () {
        if (!this.Wizard.IsImportWizard) {
            this.ShowWarning_MainCarriageCarrierId = false;
            this.ShowWarning_MainCarriageCarrierNumber = false;
            this.ShowWarning_Master = false;
            this.ShowWarning_MAWBOBLDate = false;
            this.ShowWarning_MainCarriageETD = false;
            this.ShowWarning_Transshipment1CarrierId = false;
            this.ShowWarning_Transshipment2CarrierId = false;
            if (this.MainCarriageCarrierId == null) {
                this.ShowWarning_MainCarriageCarrierId = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                this.ShowWarning_Master = true;
            }
            if (this.MAWBOBLDate == null) {
                this.ShowWarning_MAWBOBLDate = true;
            }
            if (this.MainCarriageETD == null) {
                this.ShowWarning_MainCarriageETD = true;
            }
            if (this.Transshipment1CarrierId == null) {
                this.ShowWarning_Transshipment1CarrierId = true;
            }
            if (this.Transshipment2CarrierId == null) {
                this.ShowWarning_Transshipment2CarrierId = true;
            }
            var isCarrierNumberFormatValid = Tools_1.FormatTool.Validate_FlightNumber(this.MainCarriageCarrierNumber);
            if (this.MainCarriageCarrierId == null) {
                if (!isCarrierNumberFormatValid) {
                    this.ShowWarning_MainCarriageCarrierNumber = true;
                }
            }
            else {
                var codePrefix = Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierPrefix) ? this.MainCarriageCarrierPrefix : this.MainCarriageCarrierPrefix.trim();
                var isValidcodePrefix = !Tools_1.AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;
                if (!isValidcodePrefix || !isCarrierNumberFormatValid) {
                    this.ShowWarning_MainCarriageCarrierNumber = true;
                }
            }
        }
    };
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageFromPortId", {
        // Ports
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFromPortId != value) {
                this.EntityPM.MainCarriageFromPortId = value;
                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
                    if (this.Wizard) {
                        this.Wizard.SetCargonautDEXXVisibility();
                    }
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
                                if (_this.Wizard) {
                                    _this.Wizard.SetCargonautDEXXVisibility();
                                }
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.MainCarriageFromPortChanged(_this.EntityPM, list);
                                        if (_this.Wizard) {
                                            _this.Wizard.SetCargonautDEXXVisibility();
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
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1FromPortId", {
        get: function () { return this.EntityPM.Transshipment1FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment1FromPortId != value) {
                this.EntityPM.Transshipment1FromPortId = value;
                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.Transshipment1FromPortChanged(this.EntityPM, null);
                    Tools_2.RoutingHelper.RemoveTransshipment1Leg(this.EntityPM);
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
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2FromPortId", {
        get: function () { return this.EntityPM.Transshipment2FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment2FromPortId != value) {
                this.EntityPM.Transshipment2FromPortId = value;
                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.Transshipment2FromPortChanged(this.EntityPM, null);
                    Tools_2.RoutingHelper.RemoveTransshipment2Leg(this.EntityPM);
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
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment3FromPortId", {
        get: function () { return this.EntityPM.Transshipment3FromPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.Transshipment3FromPortId != value) {
                this.EntityPM.Transshipment3FromPortId = value;
                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_Ports();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.Transshipment3FromPortChanged(this.EntityPM, null);
                    Tools_2.RoutingHelper.RemoveTransshipment3Leg(this.EntityPM);
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
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageFinalDestinationPortId", {
        get: function () { return this.EntityPM.MainCarriageFinalDestinationPortId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.MainCarriageFinalDestinationPortId != value) {
                this.EntityPM.MainCarriageFinalDestinationPortId = value;
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    Tools_2.RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
                    this.OnFinalDestinationChanged();
                }
                else {
                    this.myPortListService.getSingleFromCache(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
                                _this.OnFinalDestinationChanged();
                            }
                            else {
                                _this.myPortListService.getSingle(value).subscribe(function (myResponse2) {
                                    if (!myResponse2.HasError) {
                                        var list = myResponse2.Result;
                                        Tools_2.RoutingHelper.FinalDestinationPortChanged(_this.EntityPM, list);
                                        _this.OnFinalDestinationChanged();
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
    AWBRoutingsTabComponent.prototype.OnFinalDestinationChanged = function () {
        this.Validate();
        this.FireWizardEvent();
        this.SetUIProperties_Ports();
    };
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageCarrierId", {
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.MainCarriageCarrierId != newValue) {
                this.EntityPM.MainCarriageCarrierId = newValue;
                this.SetUIProperties_Carriers();
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.EntityPM.MainCarriageCarrierCode = null;
                    this.EntityPM.MainCarriageCarrierName = null;
                    this.EntityPM.MainCarriageCarrierPrefix = null;
                    this.EntityPM.MainCarriageCarrierWebSite = null;
                    this.EntityPM.AccountNumber = null;
                    this.EntityPM.MainCarriageCarrierNumber = null;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                        this.EntityPM.CarrierIsCheckDigit = false;
                        this.EntityPM.CarrierIsLimitedLength = false;
                        this.AirlinePrefix = null;
                    }
                    this.Master = null;
                    this.EntityPM.CarrierIsChampRegistered = false;
                    this.EntityPM.CarrierIsGLSHKRegistered = false;
                    Tools_2.ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
                    this.Validate();
                    this.FireWizardEvent();
                    this.SetUIProperties_Carriers();
                }
                else {
                    this.myCardService.getSingle(newValue).subscribe(function (myCardListResponse) {
                        if (myCardListResponse != null) {
                            var myCardList = myCardListResponse.Result;
                            if (myCardList != null) {
                                _this.EntityPM.MainCarriageCarrierCode = myCardList.Code;
                                _this.EntityPM.MainCarriageCarrierName = myCardList.EnglishName;
                                _this.EntityPM.MainCarriageCarrierPrefix = myCardList.Code;
                                _this.EntityPM.MainCarriageCarrierWebSite = myCardList.WebSite;
                                _this.EntityPM.AccountNumber = myCardList.AirlineAccountNumber;
                                _this.Wizard.LoadAirlineRules(myCardList.Code);
                                _this.Validate();
                                _this.FireWizardEvent();
                                _this.SetUIProperties_Carriers();
                                // dont get from chach: if user choosed from tenant0 it wont get it
                                _this.myAirlineService.getSingle(newValue).subscribe(function (myAirlineListResponse) {
                                    if (myAirlineListResponse != null) {
                                        var myAirlineList = myAirlineListResponse.Result;
                                        if (myAirlineList != null) {
                                            if (Tools_1.AppTool.IsNullOrEmpty(_this.InterlineId)) {
                                                _this.EntityPM.CarrierIsCheckDigit = myAirlineList.CheckDigit;
                                                _this.EntityPM.CarrierIsLimitedLength = myAirlineList.LimitedLength;
                                                var myPrefix = null;
                                                if (!Tools_1.AppTool.IsNullOrEmpty(myAirlineList.Prefix)) {
                                                    myPrefix = myAirlineList.Prefix.toString().trim();
                                                    myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                                                }
                                                _this.AirlinePrefix = myPrefix;
                                            }
                                            _this.EntityPM.CarrierIsChampRegistered = myAirlineList.IsChampRegistered;
                                            _this.EntityPM.CarrierIsGLSHKRegistered = myAirlineList.IsGLSHKRegistered;
                                            _this.Validate();
                                            _this.FireWizardEvent();
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
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageCarrierPrefix", {
        get: function () { return this.EntityPM.MainCarriageCarrierPrefix; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierPrefix != newValue) {
                this.EntityPM.MainCarriageCarrierPrefix = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
                this.EntityPM.MainCarriageCarrierNumber = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "AirlinePrefix", {
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (newValue) {
            if (this.EntityPM.AirlinePrefix != newValue) {
                this.EntityPM.AirlinePrefix = newValue;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (newValue) {
            if (this.EntityPM.Master != newValue) {
                this.EntityPM.Master = newValue;
                this.LongMaster = Tools_2.ShipmentTool.GetLongMasterField(this.EntityPM.TransportModeId, this.EntityPM.AirlinePrefix, this.EntityPM.Master);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "LongMaster", {
        get: function () { return this.EntityPM.LongMaster; },
        set: function (newValue) {
            if (this.EntityPM.LongMaster != newValue) {
                this.EntityPM.LongMaster = newValue;
                this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
                this.Validate();
                this.FireWizardEvent();
                this.ValidateMasterField();
                this.SetUIProperties_Carriers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MAWBOBLDate", {
        get: function () { return this.EntityPM.MAWBOBLDate; },
        set: function (newValue) {
            if (this.EntityPM.MAWBOBLDate != newValue) {
                this.EntityPM.MAWBOBLDate = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) {
            if (this.EntityPM.AccountNumber != newValue) {
                this.EntityPM.AccountNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBRoutingsTabComponent.prototype.AddInterlineClicked = function () {
        this.IsInterlineAdded = true;
    };
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "InterlineId", {
        get: function () { return this.EntityPM.InterlineId; },
        set: function (newValue) {
            if (this.EntityPM.InterlineId != newValue) {
                this.EntityPM.InterlineId = newValue;
                this.OnInterlineChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBRoutingsTabComponent.prototype.OnInterlineLostFocus = function ($event) {
        // this.OnInterlineChanged();
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
    };
    AWBRoutingsTabComponent.prototype.OnInterlineChanged = function () {
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
            this.myAirlineService.getSingleFromCache(myAirlineId).subscribe(function (myResponse) {
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
        this.SetUIProperties_Carriers();
    };
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1Carrier", {
        get: function () { return this.transshipment1Carrier; },
        set: function (list) {
            if (this.transshipment1Carrier != list) {
                this.transshipment1Carrier = list;
                var Code = list == null ? null : list.Code;
                if (Code != this.EntityPM.Transshipment1CarrierCode) {
                    if (list == null) {
                        this.EntityPM.Transshipment1CarrierCode = null;
                        this.EntityPM.Transshipment1CarrierName = null;
                        this.EntityPM.Transshipment1CarrierPrefix = null;
                        this.EntityPM.Transshipment1CarrierWebSite = null;
                        this.EntityPM.Transshipment1CarrierNumber = null;
                        this.EntityPM.Transshipment1AdditionalMAWBOBLBL = null;
                    }
                    else {
                        this.EntityPM.Transshipment1CarrierCode = list.Code;
                        this.EntityPM.Transshipment1CarrierName = list.EnglishName;
                        this.EntityPM.Transshipment1CarrierPrefix = list.Code;
                        this.EntityPM.Transshipment1CarrierWebSite = list.WebSite;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1CarrierId", {
        get: function () { return this.EntityPM.Transshipment1CarrierId; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1CarrierId != newValue) {
                this.EntityPM.Transshipment1CarrierId = newValue;
                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_CarrierT1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment1CarrierPrefix; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1CarrierPrefix != newValue) {
                this.EntityPM.Transshipment1CarrierPrefix = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1CarrierNumber", {
        get: function () { return this.EntityPM.Transshipment1CarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1CarrierNumber != newValue) {
                this.EntityPM.Transshipment1CarrierNumber = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1AdditionalMAWBOBLBL", {
        get: function () { return this.EntityPM.Transshipment1AdditionalMAWBOBLBL; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1AdditionalMAWBOBLBL != newValue) {
                this.EntityPM.Transshipment1AdditionalMAWBOBLBL = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2Carrier", {
        get: function () { return this.transshipment2Carrier; },
        set: function (list) {
            if (this.transshipment2Carrier != list) {
                this.transshipment2Carrier = list;
                var Code = list == null ? null : list.Code;
                if (Code != this.EntityPM.Transshipment2CarrierCode) {
                    if (list == null) {
                        this.EntityPM.Transshipment2CarrierCode = null;
                        this.EntityPM.Transshipment2CarrierName = null;
                        this.EntityPM.Transshipment2CarrierPrefix = null;
                        this.EntityPM.Transshipment2CarrierWebSite = null;
                        this.EntityPM.Transshipment2CarrierNumber = null;
                        this.EntityPM.Transshipment2AdditionalMAWBOBLBL = null;
                    }
                    else {
                        this.EntityPM.Transshipment2CarrierCode = list.Code;
                        this.EntityPM.Transshipment2CarrierName = list.EnglishName;
                        this.EntityPM.Transshipment2CarrierPrefix = list.Code;
                        this.EntityPM.Transshipment2CarrierWebSite = list.WebSite;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2CarrierId", {
        get: function () { return this.EntityPM.Transshipment2CarrierId; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2CarrierId != newValue) {
                this.EntityPM.Transshipment2CarrierId = newValue;
                this.Validate();
                this.FireWizardEvent();
                this.SetUIProperties_CarrierT2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment2CarrierPrefix; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2CarrierPrefix != newValue) {
                this.EntityPM.Transshipment2CarrierPrefix = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2CarrierNumber", {
        get: function () { return this.EntityPM.Transshipment2CarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2CarrierNumber != newValue) {
                this.EntityPM.Transshipment2CarrierNumber = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2AdditionalMAWBOBLBL", {
        get: function () { return this.EntityPM.Transshipment2AdditionalMAWBOBLBL; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2AdditionalMAWBOBLBL != newValue) {
                this.EntityPM.Transshipment2AdditionalMAWBOBLBL = newValue;
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageETD", {
        // Dates
        get: function () { return this.EntityPM.MainCarriageETD; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageETD != newValue) {
                this.EntityPM.MainCarriageETD = newValue;
                this.Validate();
                this.FireWizardEvent();
                this.SetFlightDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "MainCarriageATD", {
        get: function () { return this.EntityPM.MainCarriageATD; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageATD != newValue) {
                this.EntityPM.MainCarriageATD = newValue;
                this.SetFlightDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBRoutingsTabComponent.prototype.SetFlightDate = function () {
        var flightDate = this.MainCarriageETD;
        var isFlightDateActual = false;
        if (this.MainCarriageATD != null) {
            flightDate = this.MainCarriageATD;
            isFlightDateActual = true;
        }
        this.EntityPM.FlightDate = flightDate;
        this.EntityPM.IsFlightDateActual = isFlightDateActual;
    };
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1ETD", {
        get: function () { return this.EntityPM.Transshipment1ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ETD != newValue) {
                this.EntityPM.Transshipment1ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment1ATD", {
        get: function () { return this.EntityPM.Transshipment1ATD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ATD != newValue) {
                this.EntityPM.Transshipment1ATD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2ETD", {
        get: function () { return this.EntityPM.Transshipment2ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ETD != newValue) {
                this.EntityPM.Transshipment2ETD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBRoutingsTabComponent.prototype, "Transshipment2ATD", {
        get: function () { return this.EntityPM.Transshipment2ATD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ATD != newValue) {
                this.EntityPM.Transshipment2ATD = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBRoutingsTabComponent.prototype.ValidateMasterField = function () {
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
    };
    AWBRoutingsTabComponent.prototype.ValidateMasterFieldIsUsed = function () {
        var _this = this;
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
    };
    AWBRoutingsTabComponent.prototype.MasterLostFocus = function (input) {
        this.ValidateMasterStack();
    };
    AWBRoutingsTabComponent.prototype.ValidateMasterStack = function () {
        var _this = this;
        if (this.EntityPM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                if (this.Master.length == 8 && !this.EntityPM.MainCarriageIsFromStack && !this.EntityPM.MAWBTakenFromStack) {
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
                                                    _this.EntityPM.MAWBTakenFromStack = true;
                                                    _this.EntityPM.MAWBStackNumber = Tools_1.AppTool.PadLeft(myStackPM.Number.toString(), 8, '0');
                                                    _this.MAWBOBLDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                                                    _this.SetMAWBAirline();
                                                    //this.isGetFromStock = true;
                                                    _this.Save();
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
    };
    // Commands
    AWBRoutingsTabComponent.prototype.AddStockClicked = function (airlineId) {
        if (!Tools_1.AppTool.IsNullOrEmpty(airlineId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit Airline";
            logWindow.ShowEditComponent(airlineId, "Airline", "ALST");
        }
    };
    //private isGetFromStock: boolean = false;
    //private myOldMAWBStackNumber: string;
    //private myOldMAWBOBLDate: Date;
    AWBRoutingsTabComponent.prototype.GetStockClicked = function () {
        var _this = this;
        var isValid = this.Wizard.ValidateShipment();
        if (isValid) {
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
                        //this.myOldMAWBStackNumber = this.EntityPM.MAWBStackNumber;
                        //this.myOldMAWBOBLDate = this.EntityPM.MAWBOBLDate;
                        _this.EntityPM.MAWBTakenFromStack = true;
                        _this.EntityPM.MAWBStackNumber = Tools_1.AppTool.PadLeft(stackNumber.toString(), 8, '0');
                        _this.MAWBOBLDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                        //this.isGetFromStock = true;
                        _this.Save();
                    }
                }
            });
        }
    };
    AWBRoutingsTabComponent.prototype.ReturnStockClicked = function () {
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            var isValid = this.Wizard.ValidateShipment();
            if (isValid) {
                this.SetMAWBAirline();
                this.EntityPM.MAWBReturnedToStack = true;
                this.EntityPM.MAWBStackNumber = this.Master;
                //this.isGetFromStock = false;
                this.Save();
            }
        }
    };
    AWBRoutingsTabComponent.prototype.SetMAWBAirline = function () {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    };
    AWBRoutingsTabComponent.prototype.SetFlightSchedules = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "FlightsSchedules")) {
            this.IsFlightSchedulesVisible = true;
        }
    };
    AWBRoutingsTabComponent.prototype.FlightSchedulesClicked = function (isMainLeg) {
        if (!this.isFlightSchedulesRequested) {
            this.isMainLegFlightSchedules = isMainLeg;
            this.isFlightSchedulesRequested = true;
            this.Wizard.SaveClicked();
        }
    };
    AWBRoutingsTabComponent.prototype.RunFlightSchedules = function () {
        var _this = this;
        var args = new Args_2.FlightsSchedulesArgs();
        args.ShipmentPM = this.EntityPM;
        args.IsMainLeg = this.isMainLegFlightSchedules;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = args;
        logWindow.Title = "Send FVA Response";
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(function (response) {
            logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FlightsSchedulesComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                if (args.IsFlightSelected) {
                    _this.Validate();
                    _this.FireWizardEvent();
                    _this.SetUIProperties();
                }
            });
        });
    };
    AWBRoutingsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AWBRoutingsTabComponent',
            templateUrl: './AWBRoutingsTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBRoutingsTabComponent);
    return AWBRoutingsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AWBRoutingsTabComponent = AWBRoutingsTabComponent;
//# sourceMappingURL=AWBRoutingsTabComponent.js.map