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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var PortListService_1 = require("../../../../Common/Services/StandardLists/PortListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var AWBStackDomainService_1 = require("../../../../Common/Services/AWBStackDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../../../Common/Args");
var Args_2 = require("../../../../CommonModules/CommonFlightsSchedules/Args");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_2 = require("../../../Tools");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var BookingDomainService_1 = require("../../../Services/BookingDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BookingDetailsTabComponent = /** @class */ (function (_super) {
    __extends(BookingDetailsTabComponent, _super);
    function BookingDetailsTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsFlightSchedulesVisible = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsApplyAllEnabled = false;
        _this.IsAddStockVisible_Mn = false;
        _this.IsAddStockVisible_In = false;
        _this.IsFromStockVisible = false;
        _this.IsFromStockEnabled = false;
        _this.IsReturnStockVisible = false;
        _this.IsReturnStockEnabled = false;
        _this.IsFlightSchedulesEnabled = false;
        _this.IsAddInterlineEnabled = false;
        _this.ShowWarning_Master = false;
        _this.ShowWarning_FromPort = false;
        _this.ShowWarning_FinalPort = false;
        _this.ShowWarning_CarrierNumber = false;
        _this.ShowWarning_ETD = false;
        _this.ShowWarning_MainAllocation = false;
        _this.ShowWarning_MainAllotment = false;
        _this.ShowWarning_Trans1CarrierId = false;
        _this.ShowWarning_Trans1CarrierNumber = false;
        _this.ShowWarning_Trans1ETD = false;
        _this.ShowWarning_Trans1Allocation = false;
        _this.ShowWarning_Trans1Allotment = false;
        _this.ShowWarning_Trans2CarrierId = false;
        _this.ShowWarning_Trans2CarrierNumber = false;
        _this.ShowWarning_Trans2ETD = false;
        _this.ShowWarning_Trans2Allocation = false;
        _this.ShowWarning_Trans2Allotment = false;
        _this.isSaveRequested = false;
        _this.isReloadRequested = false;
        //Interline
        _this.IsInterlineAdded = false;
        _this.MasterErrorMessage = null;
        _this.IsMasterValid = true;
        //Get Card
        _this.mainCarriageCarrier = null;
        _this.isGetFromStock = false;
        _this.isManualDataEntryEnabled = false;
        _this.isManualDataEntryEnabled_Via1 = false;
        _this.IsManualDataEntryVisible = false;
        _this.IsManualDataEntryVisible_Via1 = false;
        _this.InitializeServices();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "FlightsSchedules")) {
            _this.IsFlightSchedulesVisible = true;
        }
        return _this;
    }
    BookingDetailsTabComponent.prototype.InitializeServices = function () {
        this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
        if (this.myPortService == null) {
            this.myPortService = new PortListService_1.PortListService();
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
    BookingDetailsTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.Validate();
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
    };
    BookingDetailsTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
        this.SetUIProperties();
    };
    BookingDetailsTabComponent.prototype.SetUIProperties = function () {
        this.isEditingEnabled_MasterPorts = Tools_2.BookingTool.IsEditingFieldsEnabled_MasterPorts(this.EntityPM);
        this.isEditingEnabled_Others = Tools_2.BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);
        this.SetUIProperties_Carriers();
        this.SetUIProperties_Ports();
        this.SetUIProperties_Others();
    };
    BookingDetailsTabComponent.prototype.SetUIProperties_Carriers = function () {
        var isInterlineEnabled = this.isEditingEnabled_MasterPorts;
        var isMainCarrierEnabled = this.isEditingEnabled_MasterPorts;
        var isMasterFieldEnabled = this.isEditingEnabled_MasterPorts;
        var isMainCarrierNumberEnabled = false;
        var isMainCarriageCarrierPrefixEnabled = false;
        var isMaincarriageETDEnabled = false;
        var isTransshipment1FieldsEnabled = false;
        var isTransshipment2FieldsEnabled = false;
        if (this.isEditingEnabled_MasterPorts) {
            isInterlineEnabled = true;
            isMainCarrierEnabled = true;
            isMasterFieldEnabled = false;
            var isTakenFromStock = (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) ? true : false;
            if (isTakenFromStock || !Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                isInterlineEnabled = false;
                isMainCarrierEnabled = false;
            }
            if (!isTakenFromStock) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) || !Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                    isMasterFieldEnabled = true;
                }
            }
        }
        if (this.isEditingEnabled_Others) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                    if (this.IsManualDataEntryEnabled) {
                        isMainCarrierNumberEnabled = true;
                        isMainCarriageCarrierPrefixEnabled = true;
                    }
                }
                else {
                    isMainCarrierNumberEnabled = true;
                    isMainCarriageCarrierPrefixEnabled = true;
                }
            }
            //if (!AppTool.IsNullOrEmpty(this.Transshipment1CarrierId)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                if (this.IsManualDataEntryEnabled_Via1) {
                    isTransshipment1FieldsEnabled = true;
                }
            }
            else {
                isTransshipment1FieldsEnabled = true;
            }
            //}
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2CarrierId)) {
                isTransshipment2FieldsEnabled = true;
            }
            if (this.EntityPM.BookingStatusCode != "AWB") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
                    if (this.IsManualDataEntryEnabled) {
                        isMaincarriageETDEnabled = true;
                    }
                }
                else {
                    isMaincarriageETDEnabled = true;
                }
            }
        }
        this.UIProperties.SetEnabled("Master", this.ObjectTableName, isMasterFieldEnabled);
        this.UIProperties.SetEnabled("InterlineId", this.ObjectTableName, isInterlineEnabled);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Master));
        }
        else {
            this.UIProperties.SetEnabled("AirlinePrefix", this.ObjectTableName, false);
        }
        this.UIProperties.SetEnabled("MainCarriageCarrierId", this.ObjectTableName, isMainCarrierEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierNumber", this.ObjectTableName, isMainCarrierNumberEnabled);
        this.UIProperties.SetEnabled("MainCarriageCarrierPrefix", this.ObjectTableName, isMainCarriageCarrierPrefixEnabled);
        this.UIProperties.SetEnabled("MainCarriageETD", this.ObjectTableName, isMaincarriageETDEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierNumber", this.ObjectTableName, isTransshipment1FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1CarrierPrefix", this.ObjectTableName, isTransshipment1FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment1ETD", this.ObjectTableName, isTransshipment1FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierNumber", this.ObjectTableName, isTransshipment2FieldsEnabled);
        this.UIProperties.SetEnabled("Transshipment2CarrierPrefix", this.ObjectTableName, isTransshipment2FieldsEnabled);
        this.UIProperties.SetEnabled("AccountNumber", this.ObjectTableName, this.isEditingEnabled_Others);
        this.SetUIProperties_StockButton();
        this.Validate_Carrier();
    };
    BookingDetailsTabComponent.prototype.SetUIProperties_Ports = function () {
        var isMainLegEnabled = this.isEditingEnabled_MasterPorts;
        var isLeg1Enabled = false;
        var isLeg2Enabled = false;
        var isLastLegEnabled = this.isEditingEnabled_MasterPorts;
        if (this.isEditingEnabled_MasterPorts) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
                isLeg1Enabled = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
                isLeg2Enabled = true;
            }
        }
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isMainLegEnabled);
        this.UIProperties.SetEnabled("Transshipment1FromPortId", this.ObjectTableName, isLeg1Enabled);
        this.UIProperties.SetEnabled("Transshipment2FromPortId", this.ObjectTableName, isLeg2Enabled);
        this.UIProperties.SetEnabled("MainCarriageFinalDestinationPortId", this.ObjectTableName, isLastLegEnabled);
        this.Validate_Ports();
    };
    BookingDetailsTabComponent.prototype.SetUIProperties_Others = function () {
        this.UIProperties.SetEnabled("SpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("MainCarriageSpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("Transshipment1SpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("Transshipment2SpaceAllocationCode", this.ObjectTableName, this.isEditingEnabled_Others);
        var isMainCarriageIdentificationEnabled = false;
        var isTransshipment1IdentificationEnabled = false;
        var isTransshipment2IdentificationEnabled = false;
        var isApplyAllEnabled = false;
        if (this.isEditingEnabled_Others) {
            if (this.MainCarriageSpaceAllocationCode == "CA") {
                isMainCarriageIdentificationEnabled = true;
            }
            if (this.Transshipment1SpaceAllocationCode == "CA") {
                isTransshipment1IdentificationEnabled = true;
            }
            if (this.Transshipment2SpaceAllocationCode == "CA") {
                isTransshipment2IdentificationEnabled = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SpaceAllocationCode)) {
                isApplyAllEnabled = true;
            }
        }
        this.IsApplyAllEnabled = isApplyAllEnabled;
        this.UIProperties.SetEnabled("MainCarriageAllotmentIdentification", this.ObjectTableName, isMainCarriageIdentificationEnabled);
        this.UIProperties.SetEnabled("Transshipment1AllotmentIdentification", this.ObjectTableName, isTransshipment1IdentificationEnabled);
        this.UIProperties.SetEnabled("Transshipment2AllotmentIdentification", this.ObjectTableName, isTransshipment2IdentificationEnabled);
        this.Validate_Others();
    };
    BookingDetailsTabComponent.prototype.SetUIProperties_StockButton = function () {
        var isAddStockVisible_Mn = false;
        var isAddStockVisible_In = false;
        var isFromStockVisible = false;
        var isReturnStockVisible = false;
        var isFromStockEnabled = this.isEditingEnabled_MasterPorts;
        var isReturnStockEnabled = this.isEditingEnabled_MasterPorts;
        var isFlightSchedulesEnabled = this.isEditingEnabled_Others;
        var isAddInterlineEnabled = this.isEditingEnabled_MasterPorts;
        isAddStockVisible_Mn = !Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId);
        isAddStockVisible_In = !Tools_1.AppTool.IsNullOrEmpty(this.InterlineId);
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            isReturnStockVisible = true;
        }
        isFromStockVisible = !isReturnStockVisible;
        //interline
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) && this.EntityPM.MainCarriageIsFromStack) {
            isAddInterlineEnabled = false;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            isAddInterlineEnabled = false;
        }
        //get from stock
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId) && Tools_1.AppTool.IsNullOrEmpty(this.InterlineId)) {
            isFromStockEnabled = false;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            isFromStockEnabled = false;
        }
        else if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            isFromStockEnabled = false;
        }
        this.IsAddStockVisible_Mn = isAddStockVisible_Mn;
        this.IsAddStockVisible_In = isAddStockVisible_In;
        this.IsFromStockVisible = isFromStockVisible;
        this.IsFromStockEnabled = isFromStockEnabled;
        this.IsReturnStockVisible = isReturnStockVisible;
        this.IsReturnStockEnabled = isReturnStockEnabled;
        this.IsFlightSchedulesEnabled = isFlightSchedulesEnabled;
        this.IsAddInterlineEnabled = isAddInterlineEnabled;
    };
    BookingDetailsTabComponent.prototype.Validate = function () {
        this.Validate_Carrier();
        this.Validate_Ports();
        this.Validate_Others();
    };
    BookingDetailsTabComponent.prototype.Validate_Carrier = function () {
        this.ShowWarning_Trans1CarrierId = !Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1CarrierId) ? false : true;
        this.ShowWarning_Trans2CarrierId = !Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2CarrierId) ? false : true;
        var isValidNumberFormat = Tools_1.FormatTool.Validate_FlightNumber(this.MainCarriageCarrierNumber);
        var codePrefix = Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierPrefix) ? this.MainCarriageCarrierPrefix : this.MainCarriageCarrierPrefix.trim();
        var isValidcodePrefix = !Tools_1.AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;
        var isValidNumberFormat1 = Tools_1.FormatTool.Validate_FlightNumber(this.Transshipment1CarrierNumber);
        var codePrefix1 = Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1CarrierPrefix) ? this.Transshipment1CarrierPrefix : this.Transshipment1CarrierPrefix.trim();
        var isValidcodePrefix1 = !Tools_1.AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;
        var isValidNumberFormat2 = Tools_1.FormatTool.Validate_FlightNumber(this.Transshipment2CarrierNumber);
        var codePrefix2 = Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2CarrierPrefix) ? this.Transshipment2CarrierPrefix : this.Transshipment2CarrierPrefix.trim();
        var isValidcodePrefix2 = !Tools_1.AppTool.IsNullOrEmpty(codePrefix) && codePrefix.length == 2;
        this.ShowWarning_Master = !Tools_1.AppTool.IsNullOrEmpty(this.Master) ? false : true;
        this.ShowWarning_CarrierNumber = isValidNumberFormat && isValidcodePrefix ? false : true;
        this.ShowWarning_Trans1CarrierNumber = isValidNumberFormat1 && isValidcodePrefix1 ? false : true;
        this.ShowWarning_Trans2CarrierNumber = isValidNumberFormat2 && isValidcodePrefix2 ? false : true;
        this.FireWizardEvent();
    };
    BookingDetailsTabComponent.prototype.Validate_Ports = function () {
        this.ShowWarning_FromPort = !Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? false : true;
        this.ShowWarning_FinalPort = !Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageFinalDestinationPortId) ? false : true;
        this.FireWizardEvent();
    };
    BookingDetailsTabComponent.prototype.Validate_Others = function () {
        this.ShowWarning_ETD = this.MainCarriageETD != null ? false : true;
        this.ShowWarning_Trans1ETD = this.Transshipment1ETD != null ? false : true;
        this.ShowWarning_Trans2ETD = this.Transshipment2ETD != null ? false : true;
        this.ShowWarning_MainAllocation = !Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageSpaceAllocationCode) ? false : true;
        this.ShowWarning_Trans1Allocation = !Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1SpaceAllocationCode) ? false : true;
        this.ShowWarning_Trans2Allocation = !Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2SpaceAllocationCode) ? false : true;
        if (this.MainCarriageSpaceAllocationCode == "CA") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageAllotmentIdentification)) {
                this.ShowWarning_MainAllotment = false;
            }
            else {
                this.ShowWarning_MainAllotment = true;
            }
        }
        else {
            this.ShowWarning_MainAllotment = false;
        }
        if (this.Transshipment1SpaceAllocationCode == "CA") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1AllotmentIdentification)) {
                this.ShowWarning_Trans1Allotment = false;
            }
            else {
                this.ShowWarning_Trans1Allotment = true;
            }
        }
        else {
            this.ShowWarning_Trans1Allotment = false;
        }
        if (this.Transshipment2SpaceAllocationCode == "CA") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2AllotmentIdentification)) {
                this.ShowWarning_Trans2Allotment = false;
            }
            else {
                this.ShowWarning_Trans2Allotment = true;
            }
        }
        else {
            this.ShowWarning_Trans2Allotment = false;
        }
        this.FireWizardEvent();
    };
    BookingDetailsTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_BKD();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
    };
    BookingDetailsTabComponent.prototype.Save = function () {
        this.isSaveRequested = true;
        this.Wizard.SaveClicked();
    };
    BookingDetailsTabComponent.prototype.Reload = function () {
        this.isReloadRequested = true;
        this.Wizard.ReloadEntity();
    };
    BookingDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
                if (_this.isSaveRequested) {
                    _this.isSaveRequested = false;
                    if (isSaveSuccess) {
                        _this.Reload();
                    }
                    else {
                        if (_this.isGetFromStock) {
                            _this.EntityPM.MAWBTakenFromStack = false;
                            _this.EntityPM.MAWBStackNumber = _this.myOldMAWBStackNumber;
                            _this.isGetFromStock = false;
                        }
                        _this.FireWizardEvent();
                        _this.SetUIProperties_Carriers();
                    }
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    if (_this.isReloadRequested) {
                        _this.isReloadRequested = false;
                        _this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(_this.InterlineId) ? false : true;
                        _this.Validate();
                        _this.FireWizardEvent();
                        _this.SetUIProperties_Carriers();
                    }
                }
            });
        }
    };
    Object.defineProperty(BookingDetailsTabComponent.prototype, "LongMaster", {
        //General Properties
        get: function () { return this.EntityPM.LongMaster; },
        set: function (newValue) {
            if (this.EntityPM.LongMaster != newValue) {
                this.EntityPM.LongMaster = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "SpaceAllocationCode", {
        get: function () { return this.EntityPM.SpaceAllocationCode; },
        set: function (newValue) {
            if (this.EntityPM.SpaceAllocationCode != newValue) {
                this.EntityPM.SpaceAllocationCode = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingDetailsTabComponent.prototype.ApplyAllClicked = function () {
        this.MainCarriageSpaceAllocationCode = this.SpaceAllocationCode;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment1FromPortId)) {
            this.Transshipment1SpaceAllocationCode = this.SpaceAllocationCode;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Transshipment2FromPortId)) {
            this.Transshipment2SpaceAllocationCode = this.SpaceAllocationCode;
        }
    };
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageCarrierId", {
        //Main Carriage
        get: function () { return this.EntityPM.MainCarriageCarrierId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierId != newValue) {
                this.EntityPM.MainCarriageCarrierId = newValue;
                this.OnMainCarriageCarrierChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingDetailsTabComponent.prototype.OnMainCarriageCarrierChanged = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
                this.EntityPM.CarrierIsCheckDigit = false;
                this.EntityPM.CarrierIsLimitedLength = false;
                this.AirlinePrefix = null;
            }
            this.Master = null;
            this.AccountNumber = null;
            this.MainCarriageCarrierPrefix = null;
            this.MainCarriageCarrierNumber = null;
            this.EntityPM.MainCarriageCarrierName = null;
            this.EntityPM.TenantZeroAirlineId = null;
            this.EntityPM.TenantZeroAirlineTTY = null;
            this.EntityPM.TenantZeroAirlinePIMA = null;
            this.EntityPM.TenantZeroAirlineChampFFR = false;
            this.EntityPM.CarrierIsChampRegistered = false;
            this.EntityPM.ZeroChampNeedsRegistration = false;
            this.EntityPM.TenantZeroAirlineGLSHKFFR = false;
            this.EntityPM.CarrierIsGLSHKRegistered = false;
            this.EntityPM.ZeroGLSHKNeedsRegistration = false;
            this.EntityPM.TenantZeroIsManagingProduct = false;
            this.EntityPM.TenantZeroIsProductMandatory = false;
            this.EntityPM.ZeroIsDescOfGoodsFromList = false;
        }
        else {
            this.GetCard("M");
            this.GetAirline();
        }
        this.SetUIProperties_Carriers();
        this.FireWizardEvent();
    };
    Object.defineProperty(BookingDetailsTabComponent.prototype, "AirlinePrefix", {
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (newValue) {
            if (this.EntityPM.AirlinePrefix != newValue) {
                this.EntityPM.AirlinePrefix = newValue;
                this.LongMaster = this.ComputeLongMaster();
                this.ValidateMasterField();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Master", {
        get: function () { return this.EntityPM.Master; },
        set: function (newValue) {
            if (this.EntityPM.Master != newValue) {
                this.EntityPM.Master = newValue;
                this.LongMaster = this.ComputeLongMaster();
                this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
                this.ValidateMasterField();
                this.FireWizardEvent();
                this.SetUIProperties_Carriers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageSpaceAllocationCode", {
        get: function () { return this.EntityPM.MainCarriageSpaceAllocationCode; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageSpaceAllocationCode != newValue) {
                this.EntityPM.MainCarriageSpaceAllocationCode = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageAllotmentIdentification", {
        get: function () { return this.EntityPM.MainCarriageAllotmentIdentification; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageAllotmentIdentification != newValue) {
                this.EntityPM.MainCarriageAllotmentIdentification = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "AccountNumber", {
        get: function () { return this.EntityPM.AccountNumber; },
        set: function (newValue) {
            if (this.EntityPM.AccountNumber != newValue) {
                this.EntityPM.AccountNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageCarrierPrefix", {
        get: function () { return this.EntityPM.MainCarriageCarrierPrefix; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierPrefix != newValue) {
                this.EntityPM.MainCarriageCarrierPrefix = newValue;
                this.SetFirstFlightFeild();
                this.SetUIProperties_Carriers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageCarrierNumber", {
        get: function () { return this.EntityPM.MainCarriageCarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageCarrierNumber != newValue) {
                this.EntityPM.MainCarriageCarrierNumber = newValue;
                this.SetFirstFlightFeild();
                this.SetUIProperties_Carriers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageETD", {
        get: function () { return this.EntityPM.MainCarriageETD; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageETD != newValue) {
                this.EntityPM.MainCarriageETD = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingDetailsTabComponent.prototype.AddInterlineClicked = function () {
        this.IsInterlineAdded = true;
    };
    Object.defineProperty(BookingDetailsTabComponent.prototype, "InterlineId", {
        get: function () { return this.EntityPM.InterlineId; },
        set: function (newValue) {
            if (this.EntityPM.InterlineId != newValue) {
                this.EntityPM.InterlineId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.IsManualDataEntryVisible = false;
                    this.IsManualDataEntryVisible_Via1 = true;
                }
                else {
                    this.IsManualDataEntryVisible_Via1 = false;
                }
                this.OnInterlineChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingDetailsTabComponent.prototype.OnInterlineLostFocus = function ($event) {
        // this.OnInterlineChanged();
        this.IsInterlineAdded = Tools_1.AppTool.IsNullOrEmpty(this.InterlineId) ? false : true;
    };
    BookingDetailsTabComponent.prototype.OnInterlineChanged = function () {
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
            this.myAirlineService.getSingleFromCache(myAirlineId).subscribe(function (myResult) {
                var myResponse = myResult;
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
                    _this.SetUIProperties_Carriers();
                }
            });
        }
    };
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1CarrierId", {
        //Transshipment 1
        get: function () { return this.EntityPM.Transshipment1CarrierId; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1CarrierId != newValue) {
                this.EntityPM.Transshipment1CarrierId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Transshipment1CarrierPrefix = null;
                    this.Transshipment1CarrierNumber = null;
                    this.SetUIProperties_Carriers();
                }
                else {
                    this.GetCard("T1");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment1CarrierPrefix; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1CarrierPrefix != newValue) {
                this.EntityPM.Transshipment1CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(newValue) ? newValue : newValue.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1CarrierNumber", {
        get: function () { return this.EntityPM.Transshipment1CarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1CarrierNumber != newValue) {
                this.EntityPM.Transshipment1CarrierNumber = newValue;
                this.SetUIProperties_Carriers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1ETD", {
        get: function () { return this.EntityPM.Transshipment1ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1ETD != newValue) {
                this.EntityPM.Transshipment1ETD = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1SpaceAllocationCode", {
        get: function () { return this.EntityPM.Transshipment1SpaceAllocationCode; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1SpaceAllocationCode != newValue) {
                this.EntityPM.Transshipment1SpaceAllocationCode = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1AllotmentIdentification", {
        get: function () { return this.EntityPM.Transshipment1AllotmentIdentification; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1AllotmentIdentification != newValue) {
                this.EntityPM.Transshipment1AllotmentIdentification = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2CarrierId", {
        //Transshipment 2
        get: function () { return this.EntityPM.Transshipment2CarrierId; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2CarrierId != newValue) {
                this.EntityPM.Transshipment2CarrierId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Transshipment2CarrierPrefix = null;
                    this.Transshipment2CarrierNumber = null;
                    this.SetUIProperties_Carriers();
                }
                else {
                    this.GetCard("T2");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2CarrierPrefix", {
        get: function () { return this.EntityPM.Transshipment2CarrierPrefix; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2CarrierPrefix != newValue) {
                this.EntityPM.Transshipment2CarrierPrefix = Tools_1.AppTool.IsNullOrEmpty(newValue) ? newValue : newValue.trim();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2CarrierNumber", {
        get: function () { return this.EntityPM.Transshipment2CarrierNumber; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2CarrierNumber != newValue) {
                this.EntityPM.Transshipment2CarrierNumber = newValue;
                this.SetUIProperties_Carriers();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2ETD", {
        get: function () { return this.EntityPM.Transshipment2ETD; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2ETD != newValue) {
                this.EntityPM.Transshipment2ETD = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2SpaceAllocationCode", {
        get: function () { return this.EntityPM.Transshipment2SpaceAllocationCode; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2SpaceAllocationCode != newValue) {
                this.EntityPM.Transshipment2SpaceAllocationCode = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2AllotmentIdentification", {
        get: function () { return this.EntityPM.Transshipment2AllotmentIdentification; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2AllotmentIdentification != newValue) {
                this.EntityPM.Transshipment2AllotmentIdentification = newValue;
                this.SetUIProperties_Others();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageFromPortId", {
        //Ports
        get: function () { return this.EntityPM.MainCarriageFromPortId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageFromPortId != newValue) {
                this.EntityPM.MainCarriageFromPortId = newValue;
                this.BuildLegs();
                this.FireWizardEvent();
                this.BuildRoutingField();
                this.SetUIProperties_Ports();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment1FromPortId", {
        get: function () { return this.EntityPM.Transshipment1FromPortId; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment1FromPortId != newValue) {
                this.EntityPM.Transshipment1FromPortId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Transshipment1SpaceAllocationCode = "NN";
                }
                this.BuildLegs();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "Transshipment2FromPortId", {
        get: function () { return this.EntityPM.Transshipment2FromPortId; },
        set: function (newValue) {
            if (this.EntityPM.Transshipment2FromPortId != newValue) {
                this.EntityPM.Transshipment2FromPortId = newValue;
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Transshipment2SpaceAllocationCode = "NN";
                }
                this.BuildLegs();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "MainCarriageFinalDestinationPortId", {
        get: function () { return this.EntityPM.MainCarriageFinalDestinationPortId; },
        set: function (newValue) {
            if (this.EntityPM.MainCarriageFinalDestinationPortId != newValue) {
                if (this.EntityPM.Transshipment2ToPortId != null) {
                    this.EntityPM.Transshipment2ToPortId = newValue;
                }
                else if (this.EntityPM.Transshipment1ToPortId != null) {
                    this.EntityPM.Transshipment1ToPortId = newValue;
                }
                else {
                    this.EntityPM.MainCarriageToPortId = newValue;
                }
                this.EntityPM.MainCarriageFinalDestinationPortId = newValue;
                this.BuildLegs();
                this.FireWizardEvent();
                this.BuildRoutingField();
                this.SetUIProperties_Ports();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingDetailsTabComponent.prototype.BuildLegs = function () {
        /*[1]*/
        if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment2FromPortId == null) {
            this.EntityPM.Transshipment1ToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.Transshipment1FromPortId;
            this.EntityPM.Transshipment2ToPortId = null;
            this.EntityPM.Transshipment2FromPortId = null;
            this.EntityPM.Transshipment2CarrierId = null;
            this.EntityPM.Transshipment2CarrierNumber = null;
            this.EntityPM.Transshipment2ETD = null;
        }
        /*[2]*/
        else if (this.EntityPM.Transshipment1FromPortId == null && this.EntityPM.Transshipment2FromPortId != null) {
            this.EntityPM.Transshipment2ToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.Transshipment2FromPortId;
            this.EntityPM.Transshipment1ToPortId = null;
            this.EntityPM.Transshipment1FromPortId = null;
            this.EntityPM.Transshipment1CarrierId = null;
            this.EntityPM.Transshipment1CarrierNumber = null;
            this.EntityPM.Transshipment1ETD = null;
        }
        /*[1:2]*/
        else if (this.EntityPM.Transshipment1FromPortId != null && this.EntityPM.Transshipment2FromPortId != null) {
            this.EntityPM.Transshipment2ToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.Transshipment1ToPortId = this.EntityPM.Transshipment2FromPortId;
            this.EntityPM.MainCarriageToPortId = this.EntityPM.Transshipment1FromPortId;
        }
        /*[0]*/
        else if (this.EntityPM.Transshipment1FromPortId == null && this.EntityPM.Transshipment2FromPortId == null) {
            this.EntityPM.MainCarriageToPortId = this.MainCarriageFinalDestinationPortId;
            this.EntityPM.Transshipment1ToPortId = null;
            this.EntityPM.Transshipment1FromPortId = null;
            this.EntityPM.Transshipment1CarrierId = null;
            this.EntityPM.Transshipment1CarrierNumber = null;
            this.EntityPM.Transshipment2ToPortId = null;
            this.EntityPM.Transshipment2FromPortId = null;
            this.EntityPM.Transshipment2CarrierId = null;
            this.EntityPM.Transshipment2CarrierNumber = null;
            this.EntityPM.Transshipment1ETD = null;
            this.EntityPM.Transshipment2ETD = null;
        }
        this.LoadPortsData();
        this.SetUIProperties_Ports();
    };
    BookingDetailsTabComponent.prototype.LoadPortsData = function () {
        this.LoadMainCarriageFromPort();
        this.LoadMainCarriageToPort();
        this.LoadTransshipment1FromPort();
        this.LoadTransshipment1ToPort();
        this.LoadTransshipment2FromPort();
        this.LoadTransshipment2ToPort();
    };
    BookingDetailsTabComponent.prototype.LoadMainCarriageFromPort = function () {
        var _this = this;
        var myPortId = this.EntityPM.MainCarriageFromPortId;
        if (myPortId == null) {
            this.EntityPM.MainFromPortCode = null;
            this.EntityPM.MainFromPortName = null;
            this.EntityPM.MainFromPortCountryCode = null;
            this.EntityPM.MainFromPortCountryName = null;
            this.BuildRoutingField();
        }
        else {
            var myService = new PortListService_1.PortListService();
            myService.getSingle(myPortId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.MainFromPortCode = list.Code;
                        _this.EntityPM.MainFromPortName = list.EnglishName;
                        _this.EntityPM.MainFromPortCountryCode = list.CountryCode;
                        _this.EntityPM.MainFromPortCountryName = list.CountryName;
                        _this.BuildRoutingField();
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.LoadMainCarriageToPort = function () {
        var _this = this;
        var myPortId = this.EntityPM.MainCarriageToPortId;
        if (myPortId == null) {
            this.EntityPM.MainToPortCode = null;
            this.EntityPM.MainToPortName = null;
            this.EntityPM.MainToPortCountryCode = null;
            this.EntityPM.MainToPortCountryName = null;
            this.BuildRoutingField();
        }
        else {
            var myService = new PortListService_1.PortListService();
            myService.getSingle(myPortId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.MainToPortCode = list.Code;
                        _this.EntityPM.MainToPortName = list.EnglishName;
                        _this.EntityPM.MainToPortCountryCode = list.CountryCode;
                        _this.EntityPM.MainToPortCountryName = list.CountryName;
                        _this.BuildRoutingField();
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.LoadTransshipment1FromPort = function () {
        var _this = this;
        var myPortId = this.EntityPM.Transshipment1FromPortId;
        if (myPortId == null) {
            this.EntityPM.Trans1FromPortCode = null;
            this.EntityPM.Trans1FromPortName = null;
            this.EntityPM.Trans1FromPortCountryCode = null;
            this.EntityPM.Trans1FromPortCountryName = null;
            this.BuildRoutingField();
        }
        else {
            var myService = new PortListService_1.PortListService();
            myService.getSingle(myPortId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.Trans1FromPortCode = list.Code;
                        _this.EntityPM.Trans1FromPortName = list.EnglishName;
                        _this.EntityPM.Trans1FromPortCountryCode = list.CountryCode;
                        _this.EntityPM.Trans1FromPortCountryName = list.CountryName;
                        _this.BuildRoutingField();
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.LoadTransshipment1ToPort = function () {
        var _this = this;
        var myPortId = this.EntityPM.Transshipment1ToPortId;
        if (myPortId == null) {
            this.EntityPM.Trans1ToPortCode = null;
            this.EntityPM.Trans1ToPortName = null;
            this.EntityPM.Trans1ToPortCountryCode = null;
            this.EntityPM.Trans1ToPortCountryName = null;
            this.BuildRoutingField();
        }
        else {
            var myService = new PortListService_1.PortListService();
            myService.getSingle(myPortId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.Trans1ToPortCode = list.Code;
                        _this.EntityPM.Trans1ToPortName = list.EnglishName;
                        _this.EntityPM.Trans1ToPortCountryCode = list.CountryCode;
                        _this.EntityPM.Trans1ToPortCountryName = list.CountryName;
                        _this.BuildRoutingField();
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.LoadTransshipment2FromPort = function () {
        var _this = this;
        var myPortId = this.EntityPM.Transshipment2FromPortId;
        if (myPortId == null) {
            this.EntityPM.Trans2FromPortCode = null;
            this.EntityPM.Trans2FromPortName = null;
            this.EntityPM.Trans2FromPortCountryCode = null;
            this.EntityPM.Trans2FromPortCountryName = null;
        }
        else {
            var myService = new PortListService_1.PortListService();
            myService.getSingle(myPortId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.Trans2FromPortCode = list.Code;
                        _this.EntityPM.Trans2FromPortName = list.EnglishName;
                        _this.EntityPM.Trans2FromPortCountryCode = list.CountryCode;
                        _this.EntityPM.Trans2FromPortCountryName = list.CountryName;
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.LoadTransshipment2ToPort = function () {
        var _this = this;
        var myPortId = this.EntityPM.Transshipment2ToPortId;
        if (myPortId == null) {
            this.EntityPM.Trans2ToPortCode = null;
            this.EntityPM.Trans2ToPortName = null;
            this.EntityPM.Trans2ToPortCountryCode = null;
            this.EntityPM.Trans2ToPortCountryName = null;
            this.BuildRoutingField();
        }
        else {
            var myService = new PortListService_1.PortListService();
            myService.getSingle(myPortId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.Trans2ToPortCode = list.Code;
                        _this.EntityPM.Trans2ToPortName = list.EnglishName;
                        _this.EntityPM.Trans2ToPortCountryCode = list.CountryCode;
                        _this.EntityPM.Trans2ToPortCountryName = list.CountryName;
                        _this.BuildRoutingField();
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.ComputeLongMaster = function () {
        var myField = null;
        if (this.EntityPM.TransportModeCode == "A") {
            myField = this.AirlinePrefix + "-";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                myField = myField + this.Master;
            }
        }
        else {
            myField = this.Master;
        }
        return myField;
    };
    BookingDetailsTabComponent.prototype.ValidateMasterField = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
        }
        else {
            this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
            var myMasterFieldError = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeCode, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);
            if (!Tools_1.AppTool.IsNullOrEmpty(myMasterFieldError)) {
                this.UIProperties.SetValidity("Master", this.ObjectTableName, false, myMasterFieldError);
                this.IsMasterValid = false;
                this.MasterErrorMessage = myMasterFieldError;
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this.AirlinePrefix)) {
                    this.UIProperties.SetValidity("Master", this.ObjectTableName, true, "");
                }
                else {
                    this.ValidateMasterStack();
                    this.ValidateMasterFieldIsUsed();
                }
            }
        }
    };
    BookingDetailsTabComponent.prototype.ValidateMasterFieldIsUsed = function () {
        var _this = this;
        var myBookingDomainService = new BookingDomainService_1.BookingDomainService();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
            var myMasterFieldError = "";
            myBookingDomainService.ValidateBookingMasterFieldExistance(this.EntityPM.Id, this.EntityPM.Master, this.EntityPM.AirlinePrefix, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode, this.EntityPM.IsCancelled, this.EntityPM.Tenant).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    myMasterFieldError = myResponse.Result;
                    if (Tools_1.AppTool.IsNullOrEmpty(myMasterFieldError)) {
                        _this.UIProperties.SetValidity("Master", _this.ObjectTableName, true, "");
                    }
                    else {
                        _this.UIProperties.SetValidity("Master", _this.ObjectTableName, false, myMasterFieldError);
                        _this.IsMasterValid = false;
                        _this.MasterErrorMessage = myMasterFieldError;
                    }
                }
            }, function (error) {
                _this.MasterErrorMessage = error;
            });
        }
    };
    BookingDetailsTabComponent.prototype.MasterLostFocus = function (input) {
        this.ValidateMasterStack();
    };
    BookingDetailsTabComponent.prototype.ValidateMasterStack = function () {
        var _this = this;
        if (this.EntityPM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Master)) {
                if (this.Master.length == 8 && !this.EntityPM.MainCarriageIsFromStack && !this.EntityPM.MAWBTakenFromStack) {
                    if (Tools_1.FormatTool.IsNumeric(this.Master)) {
                        this.StackDomainService.GetMAWBStackPMByNumber(+this.Master).subscribe(function (myResult) {
                            var myResponse = myResult;
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
                                                    _this.SetMAWBAirline();
                                                    _this.isGetFromStock = true;
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
    BookingDetailsTabComponent.prototype.BuildRoutingField = function () {
        var fromCode = "";
        var toCode = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainFromPortCode)) {
            fromCode = this.EntityPM.MainFromPortCode;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Trans2ToPortCode)) {
            toCode = this.EntityPM.Trans2ToPortCode;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Trans1ToPortCode)) {
            toCode = this.EntityPM.Trans1ToPortCode;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainToPortCode)) {
            toCode = this.EntityPM.MainToPortCode;
        }
        this.EntityPM.Routing = fromCode + " , " + toCode;
    };
    BookingDetailsTabComponent.prototype.SetFirstFlightFeild = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierNumber) && !Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierPrefix)) {
            this.EntityPM.FirstFlight = this.MainCarriageCarrierPrefix + this.MainCarriageCarrierNumber;
        }
    };
    BookingDetailsTabComponent.prototype.GetCard = function (myCode) {
        var _this = this;
        var myCardId = null;
        switch (myCode) {
            case "M":
                {
                    myCardId = this.MainCarriageCarrierId;
                    break;
                }
            case "T1":
                {
                    myCardId = this.Transshipment1CarrierId;
                    break;
                }
            case "T2":
                {
                    myCardId = this.Transshipment2CarrierId;
                    break;
                }
        }
        this.myCardService.getSingle(myCardId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    _this.mainCarriageCarrier = list;
                    if (list != null) {
                        switch (myCode) {
                            case "M":
                                {
                                    _this.MainCarriageCarrierPrefix = list.Code;
                                    _this.EntityPM.MainCarriageCarrierName = list.EnglishName;
                                    _this.EntityPM.MainCarriageCarrierCode = list.Code;
                                    _this.AccountNumber = list.AirlineAccountNumber;
                                    //this.LoadMessageRules(list.Code);
                                    break;
                                }
                            case "T1": {
                                _this.Transshipment1CarrierPrefix = list.Code;
                                break;
                            }
                            case "T2": {
                                _this.Transshipment2CarrierPrefix = list.Code;
                                break;
                            }
                            default: {
                                break;
                            }
                        }
                    }
                }
            }
            _this.SetUIProperties_Carriers();
        });
    };
    BookingDetailsTabComponent.prototype.GetAirline = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            this.myAirlineService.getSingleFromCache(this.MainCarriageCarrierId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InterlineId)) {
                            _this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                            _this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;
                            var myPrefix = null;
                            if (!Tools_1.AppTool.IsNullOrEmpty(list.Prefix)) {
                                myPrefix = list.Prefix;
                                myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, '0');
                            }
                            _this.AirlinePrefix = myPrefix;
                        }
                        _this.EntityPM.CarrierIsChampRegistered = list.IsChampRegistered;
                        _this.EntityPM.CarrierIsGLSHKRegistered = list.IsGLSHKRegistered;
                        _this.GetTenantZeroAirline(list.Code);
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.GetTenantZeroAirline = function (airlineCode) {
        var _this = this;
        this.myPartnersDomainService.GetAirlineByCode(airlineCode, 0).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var airlinePM = myResponse.Result;
                if (airlinePM != null) {
                    _this.EntityPM.TenantZeroAirlineId = airlinePM.Id;
                    _this.EntityPM.TenantZeroAirlineTTY = airlinePM.TTY;
                    _this.EntityPM.TenantZeroAirlinePIMA = airlinePM.GLSHKPIMA;
                    _this.EntityPM.TenantZeroAirlineChampFFR = airlinePM.ChampFFRFFA;
                    _this.EntityPM.TenantZeroAirlineChampFVR = airlinePM.ChampFVRFVA;
                    _this.EntityPM.ZeroChampNeedsRegistration = airlinePM.ChampNeedsRegistration;
                    _this.EntityPM.TenantZeroAirlineGLSHKFFR = airlinePM.GLSHKFFRFFA;
                    _this.EntityPM.TenantZeroAirlineGLSHKFVR = airlinePM.GLSHKFVRFVA;
                    _this.EntityPM.ZeroGLSHKNeedsRegistration = airlinePM.GLSHKNeedsRegistration;
                    _this.EntityPM.TenantZeroIsManagingProduct = airlinePM.IsManagingProduct;
                    _this.EntityPM.TenantZeroIsProductMandatory = airlinePM.IsProductMandatory;
                    _this.EntityPM.ZeroIsDescOfGoodsFromList = airlinePM.IsDescriptionOfGoodsFromList;
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DescriptionOfGoodsId)) {
                        _this.EntityPM.DescriptionOfGoodsId = null;
                        _this.EntityPM.DescriptionOfGoodsService = null;
                        _this.EntityPM.DescriptionOfGoods = null;
                    }
                }
            }
        });
    };
    //Stock
    BookingDetailsTabComponent.prototype.AddStockClicked = function (airlineId) {
        if (!Tools_1.AppTool.IsNullOrEmpty(airlineId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit Airline";
            logWindow.ShowEditComponent(airlineId, "Airline", "ALST");
        }
    };
    BookingDetailsTabComponent.prototype.GetStockClicked = function () {
        var _this = this;
        var isValid = this.Wizard.ValidateBooking();
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
                        _this.myOldMAWBStackNumber = _this.EntityPM.MAWBStackNumber;
                        _this.EntityPM.MAWBTakenFromStack = true;
                        _this.EntityPM.MAWBStackNumber = Tools_1.AppTool.PadLeft(stackNumber.toString(), 8, '0');
                        _this.isGetFromStock = true;
                        _this.Save();
                    }
                }
            });
        }
    };
    BookingDetailsTabComponent.prototype.ReturnStockClicked = function () {
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            var isValid = this.Wizard.ValidateBooking();
            if (isValid) {
                this.SetMAWBAirline();
                this.EntityPM.MAWBReturnedToStack = true;
                this.EntityPM.MAWBStackNumber = this.Master;
                this.isGetFromStock = false;
                this.Save();
            }
        }
    };
    BookingDetailsTabComponent.prototype.SetMAWBAirline = function () {
        if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.MainCarriageCarrierId) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            if (this.EntityPM.MAWBStackAirlineId != this.EntityPM.InterlineId) {
                this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
            }
        }
    };
    Object.defineProperty(BookingDetailsTabComponent.prototype, "IsManualDataEntryEnabled", {
        get: function () { return this.isManualDataEntryEnabled; },
        set: function (enabled) {
            this.isManualDataEntryEnabled = enabled;
            if (this.isManualDataEntryEnabled) {
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingDetailsTabComponent.prototype, "IsManualDataEntryEnabled_Via1", {
        get: function () { return this.isManualDataEntryEnabled_Via1; },
        set: function (enabled) {
            this.isManualDataEntryEnabled_Via1 = enabled;
            if (this.isManualDataEntryEnabled_Via1) {
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    BookingDetailsTabComponent.prototype.AllowManualDataEntry = function (leg) {
        if (leg == "M") {
            this.IsManualDataEntryEnabled = true;
        }
        else {
            this.IsManualDataEntryEnabled_Via1 = true;
        }
    };
    BookingDetailsTabComponent.prototype.FindFlightsClicked = function (isMainLeg) {
        var _this = this;
        this.IsManualDataEntryEnabled = false;
        this.IsManualDataEntryEnabled_Via1 = false;
        this.IsManualDataEntryVisible = false;
        this.IsManualDataEntryVisible_Via1 = false;
        var args = new Args_2.FlightsSchedulesArgs();
        args.BookingPM = this.EntityPM;
        args.IsMainLeg = isMainLeg;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = args;
        logWindow.Title = "Flights Schedules";
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(function (response) {
            logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FlightsSchedulesComponent');
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (event) {
                    if (args.IsFlightSelected) {
                        _this.Validate();
                        _this.FireWizardEvent();
                        _this.SetUIProperties();
                    }
                    if (logWindow.WindowArgs.IsClosedFomProgress || logWindow.WindowArgs.IsCancelledFomProgress || comp.IsNoFlightsResult) {
                        if (_this.EntityPM.BookingStatusCode != "AWB") {
                            if (Tools_1.AppTool.IsNullOrEmpty(_this.InterlineId)) {
                                _this.IsManualDataEntryVisible = true;
                            }
                            else {
                                _this.IsManualDataEntryVisible_Via1 = true;
                            }
                        }
                    }
                });
            });
        });
    };
    BookingDetailsTabComponent = __decorate([
        core_1.Component({
            selector: 'BookingDetailsTabComponent',
            moduleId: module.id,
            templateUrl: './BookingDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BookingDetailsTabComponent);
    return BookingDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.BookingDetailsTabComponent = BookingDetailsTabComponent;
//# sourceMappingURL=BookingDetailsTabComponent.js.map