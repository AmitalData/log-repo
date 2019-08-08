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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../Shipment/Tools");
var Args_1 = require("../../../../Shipment/Args");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ShipmentPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPM");
var AWBOCIPM_1 = require("../../../../Shipment/EntityPMs/AWBOCIPM");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var CCSWebService_1 = require("../../../../Infrastructure/Services/WebServices/CCSWebService");
var DocsOutDataViewModel_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel");
var DocumentTypeListExtendedService_1 = require("../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService");
var DocumentOutPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentOutPMService");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var StateListService_1 = require("../../../../Common/Services/StandardLists/StateListService");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var BranchListService_1 = require("../../../../Common/Services/StandardLists/BranchListService");
var AWBWizardComponent = /** @class */ (function () {
    function AWBWizardComponent(entityArgs, _documentTypeListExtendedService, _documentOutPMService, _documentTypePMService) {
        this.entityArgs = entityArgs;
        this._documentTypeListExtendedService = _documentTypeListExtendedService;
        this._documentOutPMService = _documentOutPMService;
        this._documentTypePMService = _documentTypePMService;
        this.LoadCompleted = new core_1.EventEmitter();
        this.SaveCompleted = new core_1.EventEmitter();
        this.DataContext = this;
        this.IsFHL = false;
        this.IsFWB = false;
        this.IsNewEntity = false;
        this.ValidationErrorsList = [];
        this.ValidationWarningsList = [];
        this.IsValidationSingleLine = false;
        this.IsImportWizard = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
        this.Retries = 0;
        this.SendAWBLabel = null;
        this.IsSendVisible = false;
        this.IsSendFHLsVisible = false;
        this.IsTabVisible_OVE = false;
        this.IsTabVisible_HAW = false;
        this.IsTabVisible_RAD = false;
        this.IsTabVisible_OTP = false;
        this.CCSTypeCode = "CHAMP";
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.PrintToggleButtonTop = "-170px";
        this.PrintDocumentsList = [];
        this.documentTypeCode = "";
        this.IsDEXXVisibile = false;
        this.IsDEXXButtonVisibile = false;
        this.IsDEXXToggleButtonVisibile = false;
        this.IsCargonautVisibile = false;
        this.IsCargonautButtonVisibile = false;
        this.IsCargonautToggleButtonVisibile = false;
        this.AllStates = [];
        this.PageChild_OVE = null;
        this.PageChild_PAR = null;
        this.PageChild_ROU = null;
        this.PageChild_HAW = null;
        this.PageChild_PAC = null;
        this.PageChild_FRE = null;
        this.PageChild_OTC = null;
        this.PageChild_RAD = null;
        this.PageChild_GEN = null;
        this.PageChild_OCI = null;
        this.PageChild_OTP = null;
        // Airline Rules
        this.AirlineRulesList = [];
        // Currency Rates
        this.AllCurrencyRates = [];
        this.TabErrors_PAR = [];
        this.TabErrors_ROU = [];
        this.TabErrors_PAC = [];
        this.TabErrors_FRE = [];
        this.TabErrors_OTC = [];
        this.TabErrors_RAD = [];
        this.TabErrors_GEN = [];
        this.TabErrors_OCI = [];
        this.TabErrors_OTP = [];
        this.TabWarnings_PAR = [];
        this.TabWarnings_ROU = [];
        this.TabWarnings_PAC = [];
        this.TabWarnings_FRE = [];
        this.TabWarnings_OTC = [];
        this.TabWarnings_RAD = [];
        this.TabWarnings_GEN = [];
        this.TabWarnings_OCI = [];
        this.TabWarnings_OTP = [];
        this.Fill_PAR = null;
        this.Fill_ROU = null;
        this.Fill_PAC = null;
        this.Fill_FRE = null;
        this.Fill_OTC = null;
        this.Fill_RAD = null;
        this.Fill_GEN = null;
        this.Fill_OCI = null;
        this.Fill_OTP = null;
        // Commands
        this.isSendingFHLs = false;
        this.isSendingDEXX = false;
        this.isSendingCargonaut = false;
        this.isSaveButtonClicked = false;
        this.isSendButtonClicked = false;
        this.isPrintButtonClicked = false;
        this.isPreviewButtonClicked = false;
        this.isConfirmCloseClicked = false;
        this.isFullDetailsButtonClicked = false;
        this.isCopyShipmentButtonClicked = false;
        this.isCancelShipmentButtonClicked = false;
        this.isReactivateShipmentButtonClicked = false;
        this.isSendToAirlineTenantButtonClicked = false;
        this.IsFSRRequestButtonClicked = false;
        this.IsCancelButtonDisabled = false;
        this.IsReactivateButtonDisabled = false;
        this.IsSendToAirlineTenantVisible = false;
        this.isSendWindowOpen = false;
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("AWBWizard", "View");
    }
    AWBWizardComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.WindowArgs = windowArgs;
        this.InitializeWizard();
        this.RunComponent();
    };
    AWBWizardComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    AWBWizardComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AWBWizardComponent.prototype.InitializeWizard = function () {
        var _this = this;
        if (this.WindowArgs != null) {
            this.IsNewEntity = this.WindowArgs.IsNewEntity;
            this.ShipmentLevelCode = this.WindowArgs.ShipmentLevelCode;
            this.ObjectTableName = this.ShipmentLevelCode == "C" ? "Master" : "Shipment";
            this.targetObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName && (x.Tenant == _this.TenantPM.Id || x.Tenant == 0); })[0].Id;
            this.IsFHL = this.ShipmentLevelCode == "H" ? true : false;
            this.IsFWB = !this.IsFHL;
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "SENDAWB")) {
                this.IsSendVisible = true;
                this.SendAWBLabel = this.ShipmentLevelCode == "H" ? "Send FHL" : "Send FWB";
                this.IsSendFHLsVisible = this.ShipmentLevelCode == "C" ? true : false;
                if (!this.IsNewEntity) {
                    this.IsTabVisible_OVE = true;
                }
            }
            this.YellowIconHelpMessage = this.ShipmentLevelCode == "H" ? "Required fields for sending FHL" : "Required fields for sending FWB";
            this.IsTabVisible_HAW = this.ShipmentLevelCode == "C" ? true : false;
            this.IsTabVisible_OTP = this.IsFWB ? true : false;
            this.IsTabVisible_RAD = this.IsFWB && this.TenantPM.RegulatedAgentRegimeActivated ? true : false;
            if (this.WindowArgs.IsNewEntity) {
                this.CreateShipment();
            }
            else {
                this.EntityPM = this.WindowArgs.EntityPM;
            }
            this.entityArgs.EntityPM = this.EntityPM;
            if (this.EntityPM.DirectionId == "I") {
                this.IsImportWizard = true;
            }
            this.entityArgs.ObjectTableName = this.ObjectTableName;
            this.BuildPrintDocuments();
            this.SetCargonautDEXXVisibility();
            this.SetSelectedTab();
            this.SetMoreButtons();
        }
    };
    AWBWizardComponent.prototype.BuildPrintDocuments = function () {
        this.PrintDocumentsList = [];
        switch (this.ShipmentLevelCode) {
            case "D": {
                this.PrintDocumentsList.push(new DocumentTypeClass("740", "Plain Paper AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("714", "Plain Paper HAWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740L", "AWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740HL", "HAWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740PP", "Neutral AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("714PP", "Neutral HAWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("785A", "Cargo Manifest"));
                this.PrintToggleButtonTop = "-170px";
                break;
            }
            case "H": {
                this.PrintDocumentsList.push(new DocumentTypeClass("714", "Plain Paper HAWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740HL", "HAWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("714PP", "Neutral HAWB"));
                this.PrintToggleButtonTop = "-80px";
                break;
            }
            case "C": {
                this.PrintDocumentsList.push(new DocumentTypeClass("740", "Plain Paper AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740L", "AWB Labels"));
                this.PrintDocumentsList.push(new DocumentTypeClass("740PP", "Neutral AWB"));
                this.PrintDocumentsList.push(new DocumentTypeClass("785A", "Cargo Manifest"));
                this.PrintToggleButtonTop = "-100px";
                break;
            }
        }
    };
    AWBWizardComponent.prototype.SetCargonautDEXXVisibility = function () {
        this.IsDEXXVisibile = false;
        this.IsDEXXButtonVisibile = false;
        this.IsDEXXToggleButtonVisibile = false;
        this.IsCargonautVisibile = false;
        this.IsCargonautButtonVisibile = false;
        this.IsCargonautToggleButtonVisibile = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "SENDAWB")) {
            switch (this.EntityPM.MainCarriageFromPortCode) {
                case "SPL":
                case "AMS":
                case "RTM":
                case "MST":
                    {
                        if (this.ShipmentLevelCode == "C") {
                            this.IsCargonautToggleButtonVisibile = true;
                        }
                        else {
                            this.IsCargonautButtonVisibile = true;
                        }
                        this.IsCargonautVisibile = true;
                        break;
                    }
                case "LGG":
                case "BRU":
                    {
                        if (this.ShipmentLevelCode == "C") {
                            this.IsDEXXToggleButtonVisibile = true;
                        }
                        else {
                            this.IsDEXXButtonVisibile = true;
                        }
                        this.IsDEXXVisibile = true;
                        break;
                    }
            }
        }
    };
    AWBWizardComponent.prototype.InitializeComponent = function () {
        if (this.WindowArgs != null && this.isViewInited) {
            var airlineCode = this.EntityPM.MainCarriageCarrierCode;
            if (this.WindowArgs.IsCreatingHouseFromMaster) {
                airlineCode = this.WindowArgs.MasterPM.MainCarriageCarrierCode;
            }
            this.SelectionChanged();
            this.ValidateAllTabs();
            this.LoadAirlineRules(airlineCode);
            this.LoadAllowedAirline();
            this.LoadCurrencyRates();
            this.LoadAllStates();
        }
    };
    AWBWizardComponent.prototype.LoadAllStates = function () {
        var _this = this;
        if (this.myStateListService == null) {
            this.myStateListService = new StateListService_1.StateListService();
        }
        this.myStateListService.getAll().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.AllStates = myResponse.Result;
                    _this.ValidateScreen_PAR();
                }
            }
        });
    };
    AWBWizardComponent.prototype.CreateShipment = function () {
        var _this = this;
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM = new ShipmentPM_1.ShipmentPM();
        this.EntityPM.DirectionId = "E";
        this.EntityPM.TransportModeId = "A";
        this.EntityPM.ShipmentLevelCode = this.ShipmentLevelCode;
        this.EntityPM.FHLStatusCode = "NSEN";
        this.EntityPM.FWBStatusCode = "NSEN";
        this.EntityPM.ManifestStatusCode = "NSEN";
        this.EntityPM.FHLStatusName = "Not Sent";
        this.EntityPM.FWBStatusName = "Not Sent";
        this.EntityPM.IsOperationalClosed = false;
        this.EntityPM.CreateDateTime = todayDate;
        this.EntityPM.LastUpdateDate = todayDate;
        this.EntityPM.StatusDate = todayDate;
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.AWBCurrencyId = this.TenantPM.FreightCurrencyId;
        this.EntityPM.ProfitCurrencyId = this.TenantPM.ProfitCurrencyId;
        this.EntityPM.VolumeUnitCode = this.TenantPM.VolumeUnitCode;
        this.EntityPM.DimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        this.EntityPM.ChargeableWeightUnitCode = this.TenantPM.ChargeableWeightUnitCode;
        this.EntityPM.OtherPrepaidCollectId = this.TenantPM.ExportOtherPrepaidCollectId;
        this.EntityPM.CASSCode = this.TenantPM.CASSCode;
        this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
        this.EntityPM.DepartmentId = SessionLocator_1.SessionLocator.LoggedUserPM.DepartmentId;
        this.EntityPM.NewConcurrencyGUID = Tools_1.AppTool.GetNewGuid();
        this.EntityPM.Ratio = 6;
        this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.EntityPM.Ratio, this.EntityPM.DimensionsUnitCode, this.EntityPM.ChargeableWeightUnitCode);
        this.EntityPM.AWBDeclaredValueForCarriage = "NVD";
        this.EntityPM.AWBDeclaredValueForCustoms = "NCV";
        this.EntityPM.AWBInsurrenceValue = "XXX";
        this.EntityPM.RateClassCode = "Q";
        this.EntityPM.FreightPrepaidCollectId = this.TenantPM.ExportFreightPrepaidCollectId;
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.EntityPM.FreightPrepaidCollectId = this.TenantPM.MasterExportFreightPrepaidCollectId;
        }
        this.GetAWBSignature();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.TenantPM.CASSCode)) {
            this.EntityPM.AWBChargesCodeCode = "PX";
        }
        else {
            Tools_2.ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
        }
        if (this.WindowArgs.IsCreatingHouseFromMaster) {
            var masterPM = this.WindowArgs.MasterPM;
            this.EntityPM.DirectionId = masterPM.DirectionId;
            this.EntityPM.TransportModeId = masterPM.TransportModeId;
            this.EntityPM.FromPortId = masterPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageFromPortId = masterPM.MainCarriageFromPortId;
            this.EntityPM.MainCarriageToPortId = masterPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.ToPortId = masterPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.FinalDistenationPortId = masterPM.FinalDistenationPortId;
            this.EntityPM.MainCarriageFinalDestinationPortId = masterPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.BranchId = masterPM.BranchId;
            this.EntityPM.DepartmentId = masterPM.DepartmentId;
            this.EntityPM.SalesmanUserId = masterPM.SalesmanUserId;
            this.EntityPM.FreightPrepaidCollectId = masterPM.FreightPrepaidCollectId;
            this.EntityPM.OtherPrepaidCollectId = masterPM.OtherPrepaidCollectId;
            this.EntityPM.MasterShipmentDataId = masterPM.Id;
            this.EntityPM.MainCarriageCarrierPrefix = masterPM.MainCarriageCarrierPrefix;
            this.EntityPM.TenantZeroAirlineId = masterPM.TenantZeroAirlineId;
            this.EntityPM.TenantZeroAirlineTTY = masterPM.TenantZeroAirlineTTY;
            this.EntityPM.TenantZeroAirlinePIMA = masterPM.TenantZeroAirlinePIMA;
            this.EntityPM.TenantZeroAirlineChampFWB = masterPM.TenantZeroAirlineChampFWB;
            this.EntityPM.TenantZeroAirlineChampFHL = masterPM.TenantZeroAirlineChampFHL;
            this.EntityPM.TenantZeroAirlineChampFSU = masterPM.TenantZeroAirlineChampFSU;
            this.EntityPM.TenantZeroAirlineChampFSRFSA = masterPM.TenantZeroAirlineChampFSRFSA;
            this.EntityPM.TenantZeroAirlineChampFVRFVA = masterPM.TenantZeroAirlineChampFVRFVA;
            this.EntityPM.CarrierIsChampRegistered = masterPM.CarrierIsChampRegistered;
            this.EntityPM.TenantZeroAirlineChampNeedsRegistration = masterPM.TenantZeroAirlineChampNeedsRegistration;
            this.EntityPM.TenantZeroAirlineGLSHKFWB = masterPM.TenantZeroAirlineGLSHKFWB;
            this.EntityPM.TenantZeroAirlineGLSHKFHL = masterPM.TenantZeroAirlineGLSHKFHL;
            this.EntityPM.TenantZeroAirlineGLSHKFSU = masterPM.TenantZeroAirlineGLSHKFSU;
            this.EntityPM.TenantZeroAirlineGLSHKFSRFSA = masterPM.TenantZeroAirlineGLSHKFSRFSA;
            this.EntityPM.TenantZeroAirlineGLSHKFVRFVA = masterPM.TenantZeroAirlineGLSHKFVRFVA;
            this.EntityPM.CarrierIsGLSHKRegistered = masterPM.CarrierIsGLSHKRegistered;
            this.EntityPM.TenantZeroAirlineGLSHKNeedsRegistration = masterPM.TenantZeroAirlineGLSHKNeedsRegistration;
            this.EntityPM.CarrierIsCheckDigit = masterPM.CarrierIsCheckDigit;
            this.EntityPM.CarrierIsLimitedLength = masterPM.CarrierIsLimitedLength;
            this.EntityPM.SCI = masterPM.SCI;
            this.GetAWBSignature();
        }
        if (this.WindowArgs.IsBuildFromBooking) {
            Tools_2.ShipmentTool.MapBookingShipment(this.EntityPM, this.WindowArgs.EntityPM);
        }
        if (this.WindowArgs.IsCopyFromShipment) {
            var oldShipment = this.WindowArgs.EntityPM;
            Tools_2.ShipmentTool.CopyShipment(this.EntityPM, oldShipment);
            Tools_2.ShipmentTool.CopyShipmentPackages(this.EntityPM, oldShipment, true);
            Tools_2.ShipmentTool.CopyFlights(this.EntityPM, oldShipment);
            // OCIs
            oldShipment.AWBOCIPMs.forEach(function (item) {
                var newItem = new AWBOCIPM_1.AWBOCIPM(_this.EntityPM);
                newItem.Tenant = _this.EntityPM.Tenant;
                newItem.ShipmentId = _this.EntityPM.Id;
                newItem.CountryId = item.CountryId;
                newItem.AWBInformationCode = item.AWBInformationCode;
                newItem.AWBCustomsInformationCode = item.AWBCustomsInformationCode;
                newItem.SupplementaryCustomsInfo = item.SupplementaryCustomsInfo;
                _this.EntityPM.AWBOCIPMs.push(newItem);
            });
            // Partners
            this.EntityPM.ShipperId = oldShipment.ShipperId;
            this.EntityPM.ShipperName = oldShipment.ShipperName;
            this.EntityPM.ShipperNote = oldShipment.ShipperNote;
            this.EntityPM.ShipperAddressId = oldShipment.ShipperAddressId;
            this.EntityPM.ShipperContactId = oldShipment.ShipperContactId;
            //this.EntityPM.ShipperReference1 = oldShipment.ShipperReference1;
            //this.EntityPM.ShipperReference2 = oldShipment.ShipperReference2;
            this.EntityPM.ShipperAddress1 = oldShipment.ShipperAddress1;
            this.EntityPM.ShipperAddress2 = oldShipment.ShipperAddress2;
            this.EntityPM.ShipperCity = oldShipment.ShipperCity;
            this.EntityPM.ShipperStateId = oldShipment.ShipperStateId;
            this.EntityPM.ShipperZipCode = oldShipment.ShipperZipCode;
            this.EntityPM.ConsigneeId = oldShipment.ConsigneeId;
            this.EntityPM.ConsigneeAddressId = oldShipment.ConsigneeAddressId;
            this.EntityPM.ConsigneeAddressId = oldShipment.ConsigneeAddressId;
            this.EntityPM.ConsigneeName = oldShipment.ConsigneeName;
            this.EntityPM.ConsigneeNote = oldShipment.ConsigneeNote;
            //this.EntityPM.ConsigneeReference1 = oldShipment.ConsigneeReference1;
            //this.EntityPM.ConsigneeReference2 = oldShipment.ConsigneeReference2;
            this.EntityPM.ConsigneeAddress1 = oldShipment.ConsigneeAddress1;
            this.EntityPM.ConsigneeAddress2 = oldShipment.ConsigneeAddress2;
            this.EntityPM.ConsigneeCity = oldShipment.ConsigneeCity;
            this.EntityPM.ConsigneeStateId = oldShipment.ConsigneeStateId;
            this.EntityPM.ConsigneeZipCode = oldShipment.ConsigneeZipCode;
            this.EntityPM.Notify1Id = oldShipment.Notify1Id;
            this.EntityPM.Notify1AddressId = oldShipment.Notify1AddressId;
            this.EntityPM.Notify1AddressId = oldShipment.Notify1AddressId;
            this.EntityPM.Notify1Name = oldShipment.Notify1Name;
            this.EntityPM.Notify1Note = oldShipment.Notify1Note;
            this.EntityPM.Notify1Address1 = oldShipment.Notify1Address1;
            this.EntityPM.Notify1Address2 = oldShipment.Notify1Address2;
            this.EntityPM.Notify1City = oldShipment.Notify1City;
            this.EntityPM.Notify1StateId = oldShipment.Notify1StateId;
            this.EntityPM.Notify1ZipCode = oldShipment.Notify1ZipCode;
            this.EntityPM.CustomerId = oldShipment.CustomerId;
            this.EntityPM.CustomerName = oldShipment.CustomerName;
            this.EntityPM.CustomerNote = oldShipment.CustomerNote;
            this.EntityPM.CustomerAddressId = oldShipment.CustomerAddressId;
            this.EntityPM.CustomerContactId = oldShipment.CustomerContactId;
            //this.EntityPM.CustomerReference1 = oldShipment.CustomerReference1;
            //this.EntityPM.CustomerReference2 = oldShipment.CustomerReference2;
            this.EntityPM.CustomerRankName = oldShipment.CustomerRankName;
            this.EntityPM.ShipmentCustomerTypeCode = oldShipment.ShipmentCustomerTypeCode;
            this.EntityPM.ViaColoader = oldShipment.ViaColoader;
            this.EntityPM.IssuingCarrierAgentId = oldShipment.IssuingCarrierAgentId;
            this.EntityPM.IssuingCarrierAddressId = oldShipment.IssuingCarrierAddressId;
            this.EntityPM.IssuingCarrierAgentName = oldShipment.IssuingCarrierAgentName;
            this.EntityPM.IssuingCarrierAgentNote = oldShipment.IssuingCarrierAgentNote;
            this.EntityPM.IssuingCarrierIATACode = oldShipment.IssuingCarrierIATACode;
            this.EntityPM.CASSCode = oldShipment.CASSCode;
            //this.EntityPM.IssuingCarrierReference1 = oldShipment.IssuingCarrierReference1;
            this.EntityPM.IssuingCarrierCity = oldShipment.IssuingCarrierCity;
            this.EntityPM.ConsolidatorId = oldShipment.ConsolidatorId;
            this.EntityPM.ConsolidatorName = oldShipment.ConsolidatorName;
            this.EntityPM.ConsolidatorNote = oldShipment.ConsolidatorNote;
            this.EntityPM.ConsolidatorAddressId = oldShipment.ConsolidatorAddressId;
            this.EntityPM.ConsolidatorContactId = oldShipment.ConsolidatorContactId;
            //this.EntityPM.ConsolidatorReference = oldShipment.ConsolidatorReference;
        }
    };
    AWBWizardComponent.prototype.GetAWBSignature = function () {
        var _this = this;
        if (this.EntityPM.BranchId) {
            var myResult = null;
            var myService = new BranchListService_1.BranchListService();
            myService.getSingleFromCache(this.EntityPM.BranchId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list) {
                        myResult = list.Signature;
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                        myResult = SessionLocator_1.SessionLocator.TenantPM.Signature;
                    }
                    _this.EntityPM.AWBSignature = myResult;
                }
            });
        }
        else {
            this.EntityPM.AWBSignature = SessionLocator_1.SessionLocator.TenantPM.Signature;
        }
    };
    // Selected Tab
    AWBWizardComponent.prototype.SetSelectedTab = function () {
        if (this.IsTabVisible_OVE) {
            this.selectedTabCode = "OVE";
        }
        else {
            this.selectedTabCode = "PAR";
        }
    };
    Object.defineProperty(AWBWizardComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.SelectedTabCode != null) {
            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response1) {
                var myLocation = _this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                if (myLocation != null) {
                    switch (_this.SelectedTabCode) {
                        case "OVE": {
                            if (_this.PageChild_OVE == null) {
                                _this._entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Overview/AWBOverviewTabComponent', myLocation.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.PageChild_OVE = cmpRef.instance;
                                        _this.PageChild_OVE.InitTab(_this.EntityPM, _this);
                                    });
                                });
                            }
                            else {
                                _this.PageChild_OVE.RefreshTab();
                            }
                            break;
                        }
                        case 'PAR': {
                            if (_this.PageChild_PAR == null) {
                                _this._entityResourceService.getEntityResourceByTableName("Card").subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("Address").subscribe(function (response2) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Partners/AWBPartnersTabComponent', myLocation.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.PageChild_PAR = cmpRef.instance;
                                            _this.PageChild_PAR.InitTab(_this);
                                        });
                                    });
                                });
                            }
                            else {
                                _this.PageChild_PAR.RefreshTab();
                            }
                            break;
                        }
                        case "ROU": {
                            if (_this.PageChild_ROU == null) {
                                if (_this.EntityPM.ShipmentLevelCode == "H") {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Routings/AWBHouseRoutingsTabComponent', myLocation.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.PageChild_ROU = cmpRef.instance;
                                        _this.PageChild_ROU.InitTab(_this);
                                    });
                                }
                                else {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Routings/AWBRoutingsTabComponent', myLocation.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.PageChild_ROU = cmpRef.instance;
                                        _this.PageChild_ROU.InitTab(_this);
                                    });
                                }
                            }
                            else {
                                _this.PageChild_ROU.RefreshTab();
                            }
                            break;
                        }
                        case "HAW": {
                            if (_this.PageChild_HAW == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/HAWB/HAWBTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_HAW = cmpRef.instance;
                                    _this.PageChild_HAW.InitTab(_this);
                                });
                            }
                            else {
                                _this.PageChild_HAW.RefreshTab();
                            }
                            break;
                        }
                        case "PAC": {
                            if (_this.PageChild_PAC == null) {
                                _this._entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBPackagesTabComponent', myLocation.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.PageChild_PAC = cmpRef.instance;
                                        _this.PageChild_PAC.InitTab(_this);
                                    });
                                });
                            }
                            else {
                                _this.PageChild_PAC.RefreshTab();
                            }
                            break;
                        }
                        case "FRE": {
                            if (_this.PageChild_FRE == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/FreightCharges/FreightChargesTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_FRE = cmpRef.instance;
                                    _this.PageChild_FRE.InitTab(_this);
                                });
                            }
                            else {
                                _this.PageChild_FRE.RefreshTab();
                            }
                            break;
                        }
                        case "OTC": {
                            if (_this.PageChild_OTC == null) {
                                _this._entityResourceService.getEntityResourceByTableName("ShipmentAWBPrintOnly").subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherCharges/OtherChargesTabComponent', myLocation.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.PageChild_OTC = cmpRef.instance;
                                        _this.PageChild_OTC.InitTab(_this);
                                    });
                                });
                            }
                            else {
                                _this.PageChild_OTC.RefreshTab();
                            }
                            break;
                        }
                        case "RAD": {
                            if (_this.PageChild_RAD == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/RADetails/RADetailsTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_RAD = cmpRef.instance;
                                    _this.PageChild_RAD.InitTab(_this);
                                });
                            }
                            else {
                                _this.PageChild_RAD.RefreshTab();
                            }
                            break;
                        }
                        case "GEN": {
                            if (_this.PageChild_GEN == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/GeneralDetails/GeneralDetailsTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_GEN = cmpRef.instance;
                                    _this.PageChild_GEN.InitTab(_this);
                                });
                            }
                            else {
                                _this.PageChild_GEN.RefreshTab();
                            }
                            break;
                        }
                        case "OCI": {
                            if (_this.PageChild_OCI == null) {
                                _this._entityResourceService.getEntityResourceByTableName("AWBOCI", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/OCI/OCITabComponent', myLocation.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.PageChild_OCI = cmpRef.instance;
                                        _this.PageChild_OCI.InitTab(_this);
                                    });
                                });
                            }
                            else {
                                _this.PageChild_OCI.RefreshTab();
                            }
                            break;
                        }
                        case "OTP": {
                            if (_this.PageChild_OTP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherPartners/OtherPartnersTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_OTP = cmpRef.instance;
                                    _this.PageChild_OTP.InitTab(_this);
                                });
                            }
                            else {
                                _this.PageChild_OTP.RefreshTab();
                            }
                            break;
                        }
                    }
                }
            });
        }
    };
    // Allowed Airline
    AWBWizardComponent.prototype.LoadAllowedAirline = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    if (this.EntityPM.TransportModeId == "A") {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            this.myPartnersDomainService.GetAllowedAirlineId().subscribe(function (myResponse) {
                                if (myResponse != null) {
                                    if (myResponse.HasError) {
                                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                                    }
                                    else {
                                        var allowedAirlineId = myResponse.Result;
                                        if (!Tools_1.AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                            _this.EntityPM.MainCarriageCarrierId = allowedAirlineId;
                                            _this.GetMainCarriageCarrier();
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
    AWBWizardComponent.prototype.GetMainCarriageCarrier = function () {
        var _this = this;
        if (this.EntityPM.MainCarriageCarrierId == null) {
            this.EntityPM.Master = null;
            this.EntityPM.LongMaster = null;
            this.EntityPM.AirlinePrefix = null;
            this.EntityPM.AccountNumber = null;
            this.EntityPM.MainCarriageCarrierPrefix = null;
            this.EntityPM.MainCarriageCarrierNumber = null;
            this.EntityPM.MainCarriageCarrierCode = null;
            this.EntityPM.MainCarriageCarrierName = null;
            this.EntityPM.CarrierIsChampRegistered = false;
            this.EntityPM.CarrierIsGLSHKRegistered = false;
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;
            Tools_2.ShipmentTool.MapTenantZeroAirline(this.EntityPM, null);
            this.OnLoadingAllowedAirlineCompleted();
        }
        else {
            if (this.myCardListService == null) {
                this.myCardListService = new CardListService_1.CardListService();
            }
            if (this.myAirlineListService == null) {
                this.myAirlineListService = new AirlineListService_1.AirlineListService();
            }
            this.myCardListService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        _this.EntityPM.MainCarriageCarrierPrefix = list.Code;
                        _this.EntityPM.AccountNumber = list.AirlineAccountNumber;
                        _this.EntityPM.MainCarriageCarrierCode = list.Code;
                        _this.EntityPM.MainCarriageCarrierName = list.EnglishName;
                    }
                }
                _this.OnLoadingAllowedAirlineCompleted();
            });
            this.myAirlineListService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    if (list != null) {
                        var myPrefix = null;
                        if (!Tools_1.AppTool.IsNullOrEmpty(list.Prefix)) {
                            myPrefix = list.Prefix.toString().trim();
                            myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, "0");
                        }
                        _this.EntityPM.AirlinePrefix = myPrefix;
                        _this.EntityPM.LongMaster = Tools_1.AppTool.GetLongMasterField(_this.EntityPM.TransportModeId, myPrefix, _this.EntityPM.Master);
                        _this.EntityPM.CarrierIsChampRegistered = list.IsChampRegistered;
                        _this.EntityPM.CarrierIsGLSHKRegistered = list.IsGLSHKRegistered;
                        _this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                        _this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;
                        _this.myPartnersDomainService.GetAirlineByCode(list.Code, 0).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                Tools_2.ShipmentTool.MapTenantZeroAirline(_this.EntityPM, myResponse.Result);
                            }
                            _this.OnLoadingAllowedAirlineCompleted();
                        });
                    }
                }
            });
        }
    };
    AWBWizardComponent.prototype.OnLoadingAllowedAirlineCompleted = function () {
        this.ValidateScreen_ROU();
    };
    AWBWizardComponent.prototype.LoadAirlineRules = function (myAirlineCode) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(myAirlineCode)) {
            this.AirlineRulesList = [];
            this.ValidateAllTabs();
            this.RefreshTab(this.SelectedTabCode);
        }
        else {
            var myMessageCode = this.IsFWB ? "FWB" : "FHL";
            this.myPartnersDomainService.GetAirlineRules(myAirlineCode, myMessageCode).subscribe(function (myResponse) {
                if (myResponse == null) {
                    _this.AirlineRulesList = [];
                    _this.ValidateAllTabs();
                    _this.RefreshTab(_this.SelectedTabCode);
                }
                else {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.AirlineRulesList = myResponse.Result;
                        _this.ValidateAllTabs();
                        _this.RefreshTab(_this.SelectedTabCode);
                    }
                }
            });
        }
    };
    AWBWizardComponent.prototype.RefreshTab = function (tabCode) {
        switch (tabCode) {
            case "OVE": {
                if (this.PageChild_OVE != null) {
                    this.PageChild_OVE.RefreshTab();
                }
                break;
            }
            case "PAR": {
                if (this.PageChild_PAR != null) {
                    this.PageChild_PAR.RefreshTab();
                }
                break;
            }
            case "ROU": {
                if (this.PageChild_ROU != null) {
                    this.PageChild_ROU.RefreshTab();
                }
                break;
            }
            case "HAW": {
                if (this.PageChild_HAW != null) {
                    this.PageChild_HAW.RefreshTab();
                }
                break;
            }
            case "PAC": {
                if (this.PageChild_PAC != null) {
                    this.PageChild_PAC.RefreshTab();
                }
                break;
            }
            case "FRE": {
                if (this.PageChild_FRE != null) {
                    this.PageChild_FRE.RefreshTab();
                }
                break;
            }
            case "OTC": {
                if (this.PageChild_OTC != null) {
                    this.PageChild_OTC.RefreshTab();
                }
                break;
            }
            case "RAD": {
                if (this.PageChild_RAD != null) {
                    this.PageChild_RAD.RefreshTab();
                }
                break;
            }
            case "GEN": {
                if (this.PageChild_GEN != null) {
                    this.PageChild_GEN.RefreshTab();
                }
                break;
            }
            case "OCI": {
                if (this.PageChild_OCI != null) {
                    this.PageChild_OCI.RefreshTab();
                }
                break;
            }
            case "OTP": {
                if (this.PageChild_OTP != null) {
                    this.PageChild_OTP.RefreshTab();
                }
                break;
            }
        }
    };
    AWBWizardComponent.prototype.LoadCurrencyRates = function () {
        var _this = this;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var myService = new CurrencyRatesService_1.CurrencyRatesService();
        myService.getAll(this.TenantPM.CurrencyId, todayDate).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.AllCurrencyRates = myResponse.Result;
                    if (_this.IsNewEntity) {
                        var myProfitRate = _this.GetCurrencyRate(_this.EntityPM.ProfitCurrencyId);
                        if (_this.EntityPM.ProfitExchangeRate == null) {
                            if (_this.EntityPM.ProfitExchangeRate != myProfitRate) {
                                _this.EntityPM.ProfitExchangeRate = myProfitRate;
                            }
                        }
                    }
                }
            }
        });
    };
    AWBWizardComponent.prototype.GetCurrencyRate = function (myCurrencyId) {
        var myExchangeRate = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(myCurrencyId)) {
            if (this.AllCurrencyRates != null) {
                if (myCurrencyId == this.TenantPM.CurrencyId) {
                    myExchangeRate = 1;
                }
                else {
                    var lastRate = this.AllCurrencyRates.filter(function (d) { return d.ForeignCurrencyId == myCurrencyId; })[0];
                    if (lastRate != null) {
                        myExchangeRate = lastRate.Rate;
                    }
                }
            }
        }
        return myExchangeRate;
    };
    AWBWizardComponent.prototype.ValidateAllTabs = function () {
        this.ValidationText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.ValidateScreen_PAR();
        this.ValidateScreen_ROU();
        this.ValidateScreen_PAC();
        this.ValidateScreen_FRE();
        this.ValidateScreen_OTC();
        this.ValidateScreen_RAD();
        this.ValidateScreen_GEN();
        this.ValidateScreen_OCI();
        this.ValidateScreen_OTP();
    };
    AWBWizardComponent.prototype.ValidateScreen_PAR = function () {
        var screenErrors = [];
        var screenWarnings = [];
        this.ValidateScreen_PAR_Shipper(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_Consignee(screenErrors, screenWarnings);
        if (!this.IsImportWizard) {
            this.ValidateScreen_PAR_Notify1(screenErrors, screenWarnings);
            this.ValidateScreen_PAR_IssuingAgent(screenErrors, screenWarnings);
            this.ValidateScreen_PAR_AirlineRules(screenErrors, screenWarnings);
        }
        this.TabErrors_PAR = screenErrors;
        this.TabWarnings_PAR = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAR");
    };
    AWBWizardComponent.prototype.ValidateScreen_PAR_Shipper = function (screenErrors, screenWarnings) {
        var _this = this;
        if (this.EntityPM.ShipperId == null) {
            if (!this.IsImportWizard) {
                screenErrors.push(this.ValidationText.replace("%FieldName", "Shipper"));
            }
        }
        else {
            if (!this.IsImportWizard) {
                if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.ShipperName)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper Name"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address"));
                }
                else {
                    var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress1) ? null : this.EntityPM.ShipperAddress1.trim();
                    var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress2) ? null : this.EntityPM.ShipperAddress2.trim();
                    var myZipCode = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperZipCode) ? null : this.EntityPM.ShipperZipCode.trim();
                    var myCity = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperCity) ? null : this.EntityPM.ShipperCity.trim();
                    var myFaxNumber = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperFaxNumber) ? null : this.EntityPM.ShipperFaxNumber.trim();
                    var myPhoneNumber = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperPhoneNumber) ? null : this.EntityPM.ShipperPhoneNumber.trim();
                    if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper Address1"));
                    }
                    if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper Address2"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address1 Or Address2"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Zip Code"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myZipCode)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper Zip Code"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper City"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper City"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperStateId)) {
                        if (this.AllStates.filter(function (d) { return d.CountryId == _this.EntityPM.ShipperCountryId; }).length > 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper address state"));
                        }
                    }
                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (Tools_1.AppTool.IsNullOrEmpty(myFaxNumber) && Tools_1.AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Phone Or Fax"));
                        }
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_PAR_Consignee = function (screenErrors, screenWarnings) {
        var _this = this;
        if (this.EntityPM.ConsigneeId == null) {
            if (!this.IsImportWizard) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee"));
            }
            else {
                screenErrors.push(this.ValidationText.replace("%FieldName", "Consignee"));
            }
        }
        else {
            if (!this.IsImportWizard) {
                if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.ConsigneeName)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Consignee Name"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address"));
                }
                else {
                    var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress1) ? null : this.EntityPM.ConsigneeAddress1.trim();
                    var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress2) ? null : this.EntityPM.ConsigneeAddress2.trim();
                    var myZipCode = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeZipCode) ? null : this.EntityPM.ConsigneeZipCode.trim();
                    var myCity = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeCity) ? null : this.EntityPM.ConsigneeCity.trim();
                    var myFaxNumber = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeFaxNumber) ? null : this.EntityPM.ConsigneeFaxNumber.trim();
                    var myPhoneNumber = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneePhoneNumber) ? null : this.EntityPM.ConsigneePhoneNumber.trim();
                    if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Consignee Address1"));
                    }
                    if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Consignee Address2"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address1 Or Address2"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Zip Code"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myZipCode)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Consignee Zip Code"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee City"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Consignee City"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeStateId)) {
                        if (this.AllStates.filter(function (d) { return d.CountryId == _this.EntityPM.ConsigneeCountryId; }).length > 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee address state"));
                        }
                    }
                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (Tools_1.AppTool.IsNullOrEmpty(myFaxNumber) && Tools_1.AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Phone Or Fax"));
                        }
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_PAR_Notify1 = function (screenErrors, screenWarnings) {
        var _this = this;
        if (this.IsFWB) {
            if (this.EntityPM.Notify1Id != null) {
                if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.Notify1Name)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Notify1 Name"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1AddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Address"));
                }
                else {
                    var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1Address1) ? null : this.EntityPM.Notify1Address1.trim();
                    var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1Address2) ? null : this.EntityPM.Notify1Address2.trim();
                    var myZipCode = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1ZipCode) ? null : this.EntityPM.Notify1ZipCode.trim();
                    var myCity = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1City) ? null : this.EntityPM.Notify1City.trim();
                    var myFaxNumber = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1FaxNumber) ? null : this.EntityPM.Notify1FaxNumber.trim();
                    var myPhoneNumber = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1PhoneNumber) ? null : this.EntityPM.Notify1PhoneNumber.trim();
                    if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Notify1 Address1"));
                    }
                    if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Notify1 Address2"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Address1 Or Address2"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Zip Code"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myZipCode)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Notify1 Zip Code"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 City"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Notify1 City"));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Notify1StateId)) {
                        if (this.AllStates.filter(function (d) { return d.CountryId == _this.EntityPM.Notify1CountryId; }).length > 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 address state"));
                        }
                    }
                    if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                        if (Tools_1.AppTool.IsNullOrEmpty(myFaxNumber) && Tools_1.AppTool.IsNullOrEmpty(myPhoneNumber)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", "Notify1 Phone Or Fax"));
                        }
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_PAR_IssuingAgent = function (screenErrors, screenWarnings) {
        if (this.IsFWB) {
            if (this.EntityPM.IssuingCarrierAgentId == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent"));
            }
            else {
                if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.IssuingCarrierAgentName)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Issuing Carrier Agent Name"));
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierIATACode)) {
                    if (!Tools_1.FormatTool.Validate_IATACode(this.EntityPM.IssuingCarrierIATACode)) {
                        var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "IssuingCarrierIATACode");
                        screenWarnings.push(fieldName + " wrong format: must be 7 numeric digits max");
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CASSCode)) {
                    if (!Tools_1.FormatTool.Validate_CASSCode(this.EntityPM.CASSCode)) {
                        var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "CASSCode");
                        screenWarnings.push(fieldName + " wrong format: must be 4 numeric digits max");
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierAddressId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent Address"));
                }
                else {
                    var myCity = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierCity) ? null : this.EntityPM.IssuingCarrierCity.trim();
                    if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent City"));
                    }
                    else if (!Tools_1.FormatTool.IsTextFormatted(myCity)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Issuing Carrier Agent City"));
                    }
                }
            }
            if (this.CCSTypeCode == "GLSHK") {
                if (this.EntityPM.ViaColoader) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierReference1)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Reference 1"));
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_PAR_AirlineRules = function (screenErrors, screenWarnings) {
        this.ValidateAirlineRule("IssuingCarrierIATACode", this.EntityPM.IssuingCarrierIATACode, screenWarnings);
        this.ValidateAirlineRule("CASSCode", this.EntityPM.CASSCode, screenWarnings);
    };
    AWBWizardComponent.prototype.ValidateScreen_ROU = function () {
        var screenErrors = [];
        var screenWarnings = [];
        if (this.EntityPM.MainCarriageFromPortId == null) {
            screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Departure")));
        }
        if (this.EntityPM.MainCarriageToPortId == null) {
            screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Destination")));
        }
        if (this.EntityPM.DirectionId.toUpperCase() == "D") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageToPortId)) {
                if (this.EntityPM.FromCountryId != this.EntityPM.ToCountryId) {
                    if (this.EntityPM.FromCountryIsEC == false || this.EntityPM.ToCountryIsEC == false) {
                        if (this.EntityPM.TransportModeId == "I") {
                            screenErrors.push("Main Carriage Addresses must be in the same country since the direction is Domestic");
                        }
                        else {
                            screenErrors.push("Main Carriage Ports must be in the same country since the direction is Domestic");
                        }
                    }
                }
            }
        }
        if (this.IsFWB) {
            if (!this.IsImportWizard) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Airline")));
                }
                else {
                    var codePrefix = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierPrefix) ? this.EntityPM.MainCarriageCarrierPrefix : this.EntityPM.MainCarriageCarrierPrefix.trim();
                    if (Tools_1.AppTool.IsNullOrEmpty(codePrefix)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Carrier Prefix"));
                    }
                    else if (codePrefix.length != 2) {
                        screenWarnings.push("Main Carriage Carrier Prefix length must be 2");
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierNumber)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.FlightNo")));
                }
                else {
                    if (this.isSendButtonClicked || this.isPrintButtonClicked || this.isPreviewButtonClicked) {
                        if (!Tools_1.FormatTool.Validate_FlightNumber(this.EntityPM.MainCarriageCarrierNumber)) {
                            var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".O." + "Routings.FlightNo");
                            screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                        }
                    }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
                var myMasterFieldError = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeId, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);
                if (!Tools_1.AppTool.IsNullOrEmpty(myMasterFieldError)) {
                    screenErrors.push(myMasterFieldError);
                }
            }
            if (!this.IsImportWizard) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MAWBStackNumber)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MAWB")));
                }
                if (this.EntityPM.MAWBOBLDate == null) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MAWBDate")));
                }
                if (this.EntityPM.MainCarriageETD == null) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage ETD"));
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1ToPortId)) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 2 carrier"));
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortId) && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2ToPortId)) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 3 carrier"));
                    }
                }
            }
        }
        else {
            if (this.EntityPM.HasPreCarriage && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PreCarriageFromPortId)) {
                screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.PreCarriageFromPortId")));
            }
            if (this.EntityPM.HasOnCarriage && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageToPortId)) {
                screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.OnCarriageToPortId")));
            }
            if (!this.IsImportWizard) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.House)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "House"));
                }
            }
        }
        this.TabErrors_ROU = screenErrors;
        this.TabWarnings_ROU = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "ROU");
    };
    AWBWizardComponent.prototype.ValidateScreen_PAC = function () {
        var screenErrors = [];
        var screenWarnings = [];
        //if (!this.TenantPM.AllowEAWBMoreThanTenPackages) {
        //    var myError = ShipmentTool.ValidateAddedPackagesCount(this.EntityPM);
        //    if (!AppTool.IsNullOrEmpty(myError)) {
        //        screenErrors.push(myError);
        //    }
        //}
        if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight)) {
            if (!this.IsImportWizard) {
                var msgField = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.GrossWeight");
                msgField = msgField.replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
                screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
            }
        }
        if (this.IsFWB) {
            this.ValidateScreen_PAC_FWB(screenErrors, screenWarnings);
        }
        else {
            if (!this.IsImportWizard) {
                var myFieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.DescriptionOfGoods");
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoods)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", myFieldName));
                }
            }
        }
        this.ValidateScreen_PAC_AirlineRules(screenErrors, screenWarnings);
        this.TabErrors_PAC = screenErrors;
        this.TabWarnings_PAC = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAC");
    };
    AWBWizardComponent.prototype.ValidateScreen_PAC_FWB = function (screenErrors, screenWarnings) {
        var _this = this;
        if (!this.IsImportWizard) {
            if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight)) {
                var msgField = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.ChargeableWeight");
                msgField = msgField.replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
                screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
            }
            if (this.EntityPM.IsMultipleCommodities) {
                var BreakException = {};
                try {
                    this.EntityPM.ShipmentCommodities.forEach(function (item) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.CommodityNumber)) {
                            if (!Tools_1.FormatTool.Validate_CommodityNo(item.CommodityNumber)) {
                                var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentCommodity.F.CommodityNumber");
                                screenWarnings.push(fieldName + " must be 4-7 numeric");
                                throw BreakException;
                            }
                        }
                        if (Tools_1.AppTool.IsNullOrEmpty(item.RateClassCode)) {
                            screenWarnings.push(_this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentCommodity.F.RateClassCode")));
                        }
                        if (!_this.EntityPM.AsAgreedFreight) {
                            var rateClassGroupCode = Tools_2.ShipmentTool.GetRateClassGroupCode(item.RateClassCode);
                            if (rateClassGroupCode != "S") {
                                if (Tools_1.AppTool.IsNullOrZero(item.ChargeRate)) {
                                    screenWarnings.push(_this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentCommodity.F.ChargeRate")));
                                }
                            }
                            if (Tools_1.AppTool.IsNullOrZero(item.ChargeAmount)) {
                                screenWarnings.push(_this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentCommodity.F.AWBChargeAmount")));
                            }
                        }
                    });
                }
                catch (e) {
                    if (e !== BreakException)
                        throw e;
                }
            }
            else {
                if (this.EntityPM.ShipmentLevelCode == "C") {
                    if (this.EntityPM.MainCarriageCarrierCode == "AR") {
                        var myFieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.DescriptionOfGoods");
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoods)) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", myFieldName));
                        }
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBCommodityItemNumber)) {
                    if (!Tools_1.FormatTool.Validate_CommodityNo(this.EntityPM.AWBCommodityItemNumber)) {
                        var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBCommodityItemNumber");
                        screenWarnings.push(fieldName + " must be 4-7 numeric");
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_PAC_AirlineRules = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            if (!this.EntityPM.IsMultipleCommodities) {
                if (this.IsFWB) {
                    this.ValidateAirlineRule("AWBCommodityItemNumber", this.EntityPM.AWBCommodityItemNumber, screenWarnings);
                }
                this.ValidateAirlineRule("DescriptionOfGoods", this.EntityPM.DescriptionOfGoods, screenWarnings);
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_FRE = function () {
        var screenErrors = [];
        var screenWarnings = [];
        if (!this.IsImportWizard) {
            if (this.IsFWB) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBCurrencyId)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBCurrencyId")));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBChargesCodeCode)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBChargesCodeCode")));
                }
                if (!this.EntityPM.IsMultipleCommodities) {
                    if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight)) {
                        var msgField = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.ChargeableWeight");
                        msgField = msgField.replace("%ChargWeightCode", this.EntityPM.ChargeableWeightUnitCode);
                        screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
                    }
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.RateClassCode)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.RateClassCode")));
                    }
                    if (!this.EntityPM.AsAgreedFreight) {
                        var rateClassGroupCode = Tools_2.ShipmentTool.GetRateClassGroupCode(this.EntityPM.RateClassCode);
                        if (rateClassGroupCode != "S") {
                            if (this.EntityPM.AWBChargeRate == null || this.EntityPM.AWBChargeRate == 0) {
                                screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBChargeRate")));
                            }
                        }
                        if (this.EntityPM.AWBChargeAmount == null || this.EntityPM.AWBChargeAmount == 0) {
                            screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBChargeAmount")));
                        }
                    }
                }
            }
            if (this.IsFHL) {
                this.ValidateAirlineRule("AWBChargeRate", this.EntityPM.AWBChargeRate, screenWarnings);
            }
        }
        this.TabErrors_FRE = screenErrors;
        this.TabWarnings_FRE = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "FRE");
    };
    AWBWizardComponent.prototype.ValidateScreen_OTC = function () {
        var _this = this;
        var screenErrors = [];
        var screenWarnings = [];
        if (this.IsFWB) {
            this.EntityPM.ShipmentAWBPrintOnlies.forEach(function (item) {
                Validator_1.Validator.TryValidateObject(item, 'ShipmentAWBPrintOnly', screenErrors);
            });
            var myCount1 = this.EntityPM.ShipmentAWBPrintOnlies.length;
            var myCount2 = this.EntityPM.ShipmentPayables.filter(function (d) { return d.ChargesGroupCode != "FRT" && d.CurrencyId == _this.EntityPM.AWBCurrencyId && d.AWBPrint == true; }).length;
            var myCount3 = this.EntityPM.ShipmentReceivables.filter(function (d) { return d.ChargesGroupCode != "FRT" && d.CurrencyId == _this.EntityPM.AWBCurrencyId && d.AWBPrint == true; }).length;
            var myCount = myCount1 + myCount2 + myCount3;
            if (!this.IsImportWizard) {
                if (myCount > 9) {
                    screenWarnings.push("You have exceeded the allowable limit of 9 lines of other charges");
                }
            }
        }
        this.TabErrors_OTC = screenErrors;
        this.TabWarnings_OTC = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "OTC");
    };
    AWBWizardComponent.prototype.ValidateScreen_RAD = function () {
        var screenErrors = [];
        var screenWarnings = [];
        this.TabErrors_RAD = screenErrors;
        this.TabWarnings_RAD = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "RAD");
    };
    AWBWizardComponent.prototype.ValidateScreen_GEN = function () {
        var screenErrors = [];
        var screenWarnings = [];
        this.ValidateScreen_GEN_FWB(screenErrors, screenWarnings);
        this.ValidateScreen_GEN_Declared(screenErrors, screenWarnings);
        this.ValidateScreen_GEN_Dangerous(screenErrors, screenWarnings);
        this.ValidateScreen_GEN_AirlineRules(screenErrors, screenWarnings);
        this.TabErrors_GEN = screenErrors;
        this.TabWarnings_GEN = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "GEN");
    };
    AWBWizardComponent.prototype.ValidateScreen_GEN_FWB = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            if (this.IsFWB) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSignature)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBSignature")));
                }
                else if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.AWBSignature)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBSignature")));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBPlace)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBPlace")));
                }
                else if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.AWBPlace)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBPlace")));
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBAccountingInformation)) {
                    if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.AWBAccountingInformation)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBAccountingInformation")));
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBHandlingInformation)) {
                    if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.AWBHandlingInformation)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBHandlingInformation")));
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBComments)) {
                    if (!Tools_1.FormatTool.IsTextFormatted(this.EntityPM.AWBComments)) {
                        screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.AWBComments")));
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainHarmonize)) {
                    var isValid = false;
                    if (this.EntityPM.MainHarmonize.length >= 6 && this.EntityPM.MainHarmonize.length <= 18) {
                        if (Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.MainHarmonize)) {
                            isValid = true;
                        }
                    }
                    if (!isValid) {
                        var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.MainHarmonize");
                        screenWarnings.push(fieldName + " must be 6-18 AlphaNumeric");
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_GEN_Declared = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            if (!Tools_1.FormatTool.Validate_DeclaredCarriage(this.EntityPM.AWBDeclaredValueForCarriage)) {
                var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBDeclaredValueForCarriage");
                screenWarnings.push(fieldName + " wrong format: must be numeric Or NVD");
            }
            if (!Tools_1.FormatTool.Validate_DeclaredCustoms(this.EntityPM.AWBDeclaredValueForCustoms)) {
                var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBDeclaredValueForCustoms");
                screenWarnings.push(fieldName + " wrong format: must be numeric Or NCV");
            }
            if (!Tools_1.FormatTool.Validate_DeclaredInsurrence(this.EntityPM.AWBInsurrenceValue)) {
                var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "AWBInsurrenceValue");
                screenWarnings.push(fieldName + " wrong format: must be numeric Or XXX");
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_GEN_Dangerous = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            if (this.EntityPM.IsDangerous) {
                var isValidSpecialHandling = false;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId1)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId2)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId3)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId4)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId5)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId6)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId7)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId8)) {
                    isValidSpecialHandling = true;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId9)) {
                    isValidSpecialHandling = true;
                }
                if (!isValidSpecialHandling) {
                    screenWarnings.push("Shipments with Dangerous packages at least one of the special handling codes is required");
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_GEN_AirlineRules = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            this.ValidateAirlineRule("SCI", this.EntityPM.SCI, screenWarnings);
            if (!Tools_2.ShipmentTool.IsAdvancedAccountingInformation(this.EntityPM)) {
                this.ValidateAirlineRule("AWBAccountingInformation", this.EntityPM.AWBAccountingInformation, screenWarnings);
            }
            this.ValidateAirlineRule("AWBHandlingInformation", this.EntityPM.AWBHandlingInformation, screenWarnings);
            this.ValidateAirlineRule("AWBSpecialHandlingCodeId1", this.EntityPM.AWBSpecialHandlingCodeId1, screenWarnings);
            this.ValidateAirlineRule("AWBSpecialHandlingCodeId2", this.EntityPM.AWBSpecialHandlingCodeId2, screenWarnings);
            if (this.IsFWB) {
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId3", this.EntityPM.AWBSpecialHandlingCodeId3, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId4", this.EntityPM.AWBSpecialHandlingCodeId4, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId5", this.EntityPM.AWBSpecialHandlingCodeId5, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId6", this.EntityPM.AWBSpecialHandlingCodeId6, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId7", this.EntityPM.AWBSpecialHandlingCodeId7, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId8", this.EntityPM.AWBSpecialHandlingCodeId8, screenWarnings);
                this.ValidateAirlineRule("AWBSpecialHandlingCodeId9", this.EntityPM.AWBSpecialHandlingCodeId9, screenWarnings);
                this.ValidateAirlineRule("ReferenceNumber", this.EntityPM.ReferenceNumber, screenWarnings);
                this.ValidateAirlineRule("SupplementaryShipmentInformation1", this.EntityPM.SupplementaryShipmentInformation1, screenWarnings);
                this.ValidateAirlineRule("SupplementaryShipmentInformation2", this.EntityPM.SupplementaryShipmentInformation2, screenWarnings);
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_OCI = function () {
        var screenErrors = [];
        var screenWarnings = [];
        this.EntityPM.AWBOCIPMs.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, 'AWBOCI', screenErrors);
            if (Tools_1.AppTool.IsNullOrEmpty(item.CountryId) && Tools_1.AppTool.IsNullOrEmpty(item.AWBCustomsInformationCode) && Tools_1.AppTool.IsNullOrEmpty(item.AWBInformationCode)) {
                screenErrors.push("You must fill one of the fields (Country or Information or CustomsInformation)");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.SupplementaryCustomsInfo)) {
                if (!Tools_1.FormatTool.IsText(item.SupplementaryCustomsInfo)) {
                    screenErrors.push(Tools_1.FormatTool.GetWrongTextFormatMessage(TextCodeTranslator_1.TextCodeTranslator.Translate("AWBOCI.F.SupplementaryCustomsInfo")));
                }
            }
        });
        if (!this.IsImportWizard) {
            if (this.EntityPM.MainCarriageFinalDestinationPortCountryCode == "CN") {
                if (this.EntityPM.AWBOCIPMs.length == 0) {
                    screenWarnings.push("Please add at least one line in the OCI tab");
                }
            }
        }
        this.TabErrors_OCI = screenErrors;
        this.TabWarnings_OCI = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "OCI");
    };
    AWBWizardComponent.prototype.ValidateScreen_OTP = function () {
        var screenErrors = [];
        var screenWarnings = [];
        if (this.IsFWB) {
            this.ValidateScreen_OTP_Participant1(screenErrors, screenWarnings);
            this.ValidateScreen_OTP_Participant2(screenErrors, screenWarnings);
            this.ValidateScreen_OTP_Participant3(screenErrors, screenWarnings);
        }
        if (this.IsFWB) {
            this.ValidateScreen_OTP_AirlineRules(screenErrors, screenWarnings);
        }
        this.TabErrors_OTP = screenErrors;
        this.TabWarnings_OTP = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "OTP");
    };
    AWBWizardComponent.prototype.ValidateScreen_OTP_Participant1 = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            var isParticipantFilled = Tools_2.ShipmentTool.IsParticipant1Filled(this.EntityPM);
            if (isParticipantFilled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantIdCode1)) {
                    screenWarnings.push("Other Partners Participant1 Id field is required");
                }
                else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode1)) {
                    screenWarnings.push("Other Partners Participant1 Id field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationCode1)) {
                    screenWarnings.push("Other Partners Participant1 Code field is required");
                }
                else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode1)) {
                    screenWarnings.push("Other Partners Participant1 Code field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationPortCode1)) {
                    screenWarnings.push("Other Partners Participant1 Port/City field is required");
                }
                else if (!Tools_1.FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode1)) {
                    screenWarnings.push("Other Partners Participant1 Port/City field invalid format");
                }
                else if (this.EntityPM.OtherParticipantInformationPortCode1.length != 3) {
                    screenWarnings.push("Other Partners Participant1 Port/City field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationName1)) {
                    screenWarnings.push("Other Partners Participant1 Name field is required");
                }
                else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationName1)) {
                    screenWarnings.push("Other Partners Participant1 Name field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationReference1)) {
                    screenWarnings.push("Other Partners Participant1 Reference field is required");
                }
                else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference1)) {
                    screenWarnings.push("Other Partners Participant1 Reference field invalid format");
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_OTP_Participant2 = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            var isParticipantFilled = Tools_2.ShipmentTool.IsParticipant2Filled(this.EntityPM);
            if (isParticipantFilled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantIdCode2)) {
                    screenWarnings.push("Other Partners Participant2 Id field is required");
                }
                else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode2)) {
                    screenWarnings.push("Other Partners Participant2 Id field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationCode2)) {
                    screenWarnings.push("Other Partners Participant2 Code field is required");
                }
                else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode2)) {
                    screenWarnings.push("Other Partners Participant2 Code field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationPortCode2)) {
                    screenWarnings.push("Other Partners Participant2 Port/City field is required");
                }
                else if (!Tools_1.FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode2)) {
                    screenWarnings.push("Other Partners Participant2 Port/City field invalid format");
                }
                else if (this.EntityPM.OtherParticipantInformationPortCode2.length != 3) {
                    screenWarnings.push("Other Partners Participant2 Port/City field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationName2)) {
                    screenWarnings.push("Other Partners Participant2 Name field is required");
                }
                else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationName2)) {
                    screenWarnings.push("Other Partners Participant2 Name field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationReference2)) {
                    screenWarnings.push("Other Partners Participant2 Reference field is required");
                }
                else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference2)) {
                    screenWarnings.push("Other Partners Participant2 Reference field invalid format");
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_OTP_Participant3 = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            var isParticipantFilled = Tools_2.ShipmentTool.IsParticipant3Filled(this.EntityPM);
            if (isParticipantFilled) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantIdCode3)) {
                    screenWarnings.push("Other Partners Participant3 Id field is required");
                }
                else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode3)) {
                    screenWarnings.push("Other Partners Participant3 Id field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationCode3)) {
                    screenWarnings.push("Other Partners Participant3 Code field is required");
                }
                else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode3)) {
                    screenWarnings.push("Other Partners Participant3 Code field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationPortCode3)) {
                    screenWarnings.push("Other Partners Participant3 Port/City field is required");
                }
                else if (!Tools_1.FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode3)) {
                    screenWarnings.push("Other Partners Participant3 Port/City field invalid format");
                }
                else if (this.EntityPM.OtherParticipantInformationPortCode3.length != 3) {
                    screenWarnings.push("Other Partners Participant3 Port/City field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationName3)) {
                    screenWarnings.push("Other Partners Participant3 Name field is required");
                }
                else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationName3)) {
                    screenWarnings.push("Other Partners Participant3 Name field invalid format");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OtherParticipantInformationReference3)) {
                    screenWarnings.push("Other Partners Participant3 Reference field is required");
                }
                else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference3)) {
                    screenWarnings.push("Other Partners Participant3 Reference field invalid format");
                }
            }
        }
    };
    AWBWizardComponent.prototype.ValidateScreen_OTP_AirlineRules = function (screenErrors, screenWarnings) {
        if (!this.IsImportWizard) {
            this.ValidateAirlineRule("NominatedHandlingPartyId", this.EntityPM.NominatedHandlingPartyId, screenWarnings);
            this.ValidateAirlineRule("OtherParticipantIdCode1", this.EntityPM.OtherParticipantIdCode1, screenWarnings);
            this.ValidateAirlineRule("OtherParticipantIdCode2", this.EntityPM.OtherParticipantIdCode2, screenWarnings);
            this.ValidateAirlineRule("OtherParticipantIdCode3", this.EntityPM.OtherParticipantIdCode3, screenWarnings);
        }
    };
    AWBWizardComponent.prototype.ValidateAirlineRule = function (myFieldName, myFieldValue, validationList) {
        if (this.AirlineRulesList != null) {
            var myRule = this.AirlineRulesList.filter(function (d) { return d.RuleFieldName == myFieldName; })[0];
            if (myRule != null) {
                var myFieldLabel = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + myFieldName);
                if (myFieldValue == null || isNaN(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }
                else if (typeof (myFieldValue) == "string") {
                    if (Tools_1.AppTool.IsNullOrEmpty(myFieldValue)) {
                        if (myRule.IsMandatoryForSending) {
                            validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                        }
                    }
                    else if (myRule.MaxSize > 0) {
                        if (myFieldValue.length > myRule.MaxSize) {
                            validationList.push(myFieldLabel + " exceeds max size (" + myRule.MaxSize + ")");
                        }
                    }
                }
                else if (typeof (myFieldValue) == "number") {
                    if (Tools_1.AppTool.IsNullOrZero(myFieldValue)) {
                        if (myRule.IsMandatoryForSending) {
                            validationList.push(this.ValidationText.replace("%FieldName", myFieldLabel));
                        }
                    }
                }
            }
        }
    };
    AWBWizardComponent.prototype.ApplyStyle = function (hasErrors, hasWarnings, screenCode) {
        if (hasErrors) {
            switch (screenCode) {
                case "PAR": {
                    this.Fill_PAR = "#E45A26";
                    break;
                }
                case "OTP": {
                    this.Fill_OTP = "#E45A26";
                    break;
                }
                case "ROU": {
                    this.Fill_ROU = "#E45A26";
                    break;
                }
                case "PAC": {
                    this.Fill_PAC = "#E45A26";
                    break;
                }
                case "FRE": {
                    this.Fill_FRE = "#E45A26";
                    break;
                }
                case "OTC": {
                    this.Fill_OTC = "#E45A26";
                    break;
                }
                case "GEN": {
                    this.Fill_GEN = "#E45A26";
                    break;
                }
                case "OCI": {
                    this.Fill_OCI = "#E45A26";
                    break;
                }
                default: {
                    break;
                }
            }
        }
        else if (hasWarnings) {
            switch (screenCode) {
                case "PAR": {
                    this.Fill_PAR = "#FFCB00";
                    break;
                }
                case "OTP": {
                    this.Fill_OTP = "#FFCB00";
                    break;
                }
                case "ROU": {
                    this.Fill_ROU = "#FFCB00";
                    break;
                }
                case "PAC": {
                    this.Fill_PAC = "#FFCB00";
                    break;
                }
                case "FRE": {
                    this.Fill_FRE = "#FFCB00";
                    break;
                }
                case "OTC": {
                    this.Fill_OTC = "#FFCB00";
                    break;
                }
                case "GEN": {
                    this.Fill_GEN = "#FFCB00";
                    break;
                }
                case "OCI": {
                    this.Fill_OCI = "#FFCB00";
                    break;
                }
                default: {
                    break;
                }
            }
        }
        else {
            switch (screenCode) {
                case "PAR": {
                    this.Fill_PAR = null;
                    break;
                }
                case "OTP": {
                    this.Fill_OTP = null;
                    break;
                }
                case "ROU": {
                    this.Fill_ROU = null;
                    break;
                }
                case "PAC": {
                    this.Fill_PAC = null;
                    break;
                }
                case "FRE": {
                    this.Fill_FRE = null;
                    break;
                }
                case "OTC": {
                    this.Fill_OTC = null;
                    break;
                }
                case "GEN": {
                    this.Fill_GEN = null;
                    break;
                }
                case "OCI": {
                    this.Fill_OCI = null;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        this.ValidateAWB();
    };
    AWBWizardComponent.prototype.ValidateAWB = function () {
        this.ValidationWarningsList = [];
        if (this.ValidationErrorsList.length == 0) {
            if (this.isSendButtonClicked) {
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAR);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_ROU);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAC);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_FRE);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_OTC);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_RAD);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_GEN);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_OCI);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_OTP);
            }
        }
        return this.ValidationWarningsList.length == 0 ? true : false;
    };
    AWBWizardComponent.prototype.ValidateFSR = function () {
        this.ValidateAllTabs();
        this.ValidationWarningsList = [];
        if (this.IsFSRRequestButtonClicked) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                this.ValidationWarningsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.Airline")));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MAWBStackNumber)) {
                this.ValidationWarningsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.MAWB")));
            }
        }
        return this.ValidationWarningsList.length == 0 ? true : false;
    };
    AWBWizardComponent.prototype.ValidateShipment = function () {
        this.ValidateAllTabs();
        this.ValidationErrorsList = [];
        this.IsValidationSingleLine = false;
        var errors = Tools_2.AWBHelper.ValidateShipment(this.EntityPM);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(errors);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAR);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_ROU);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAC);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_FRE);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_OTC);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_RAD);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_GEN);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_OCI);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_OTP);
        if (this.PageChild_ROU != null) {
            if (this.PageChild_ROU.MasterFieldValidityMessage != null) {
                var errors_ROU = [];
                errors_ROU.push(this.PageChild_ROU.MasterFieldValidityMessage);
                this.ValidationErrorsList = this.ValidationErrorsList.concat(errors_ROU);
            }
        }
        return this.ValidationErrorsList.length == 0 ? true : false;
    };
    AWBWizardComponent.prototype.InitFlags = function () {
        this.isSendingFHLs = false;
        this.isSendingDEXX = false;
        this.isSendingCargonaut = false;
        this.isSaveButtonClicked = false;
        this.isSendButtonClicked = false;
        this.isPrintButtonClicked = false;
        this.isPreviewButtonClicked = false;
        this.isConfirmCloseClicked = false;
        this.IsFSRRequestButtonClicked = false;
        this.isFullDetailsButtonClicked = false;
        this.isCopyShipmentButtonClicked = false;
        this.isCancelShipmentButtonClicked = false;
        this.isReactivateShipmentButtonClicked = false;
        this.isSendToAirlineTenantButtonClicked = false;
    };
    AWBWizardComponent.prototype.SetMoreButtons = function () {
        this.IsCancelButtonDisabled = false;
        this.IsReactivateButtonDisabled = false;
        if (this.EntityPM.IsAccountingClosed || this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || (this.EntityPM.MasterShipmentDataId != null && this.EntityPM.MasterShipmentDataId != this.EntityPM.Id)) {
            this.IsCancelButtonDisabled = true;
        }
        if (!this.EntityPM.IsCancelled) {
            this.IsReactivateButtonDisabled = true;
        }
        if (this.CurrentSession.CurrentWindow != null) {
            this.CurrentSession.CurrentWindow.ShowCancelControl(this.EntityPM.IsCancelled);
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Action.SendToAirlineTenant")) {
            this.IsSendToAirlineTenantVisible = true;
        }
    };
    AWBWizardComponent.prototype.CopyShipmentClicked = function () {
        this.InitFlags();
        this.isCopyShipmentButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.Save();
        }
    };
    AWBWizardComponent.prototype.CancelShipmentClicked = function () {
        var _this = this;
        var confirmMsg;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
            confirmMsg = "Cancelling this shipment will disconnect it from the Booking , are you sure you want to cancel?";
        }
        else {
            confirmMsg = "Are you sure you want to cancel this Shipment?";
        }
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var isValid = _this.ValidateShipment();
                if (isValid) {
                    _this.InitFlags();
                    _this.isCancelShipmentButtonClicked = true;
                    _this.EntityPM.IsCancelled = true;
                    _this.isReloadingOnSave = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.BookingId)) {
                        if (_this.EntityPM.MainCarriageIsFromStack || _this.EntityPM.MAWBTakenFromStack) {
                            _this.EntityPM.MAWBReturnedToStack = true;
                            _this.EntityPM.MAWBReturnedToStackWithCancel = true;
                            _this.EntityPM.MAWBStackNumber = _this.EntityPM.Master;
                        }
                    }
                    _this.Save();
                }
            }
        });
    };
    AWBWizardComponent.prototype.ReactivateShipmentClicked = function () {
        this.InitFlags();
        this.isReactivateShipmentButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.Save();
        }
    };
    AWBWizardComponent.prototype.SendToAirlineTenantClicked = function () {
        this.InitFlags();
        this.isSendToAirlineTenantButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.Save();
        }
    };
    AWBWizardComponent.prototype.SendClicked = function (typeCode) {
        if (typeCode === void 0) { typeCode = null; }
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isSendButtonClicked = true;
        if (typeCode == "CARG") {
            this.isSendingCargonaut = true;
        }
        else if (typeCode == "DEXX") {
            this.isSendingDEXX = true;
        }
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }
        if (isValid) {
            this.Save();
        }
        else {
            this.StopBusyIndicator();
        }
    };
    AWBWizardComponent.prototype.SendFHLsClicked = function (typeCode) {
        if (typeCode === void 0) { typeCode = null; }
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isSendingFHLs = true;
        this.isSendButtonClicked = true;
        if (typeCode == "CARG") {
            this.isSendingCargonaut = true;
        }
        else if (typeCode == "DEXX") {
            this.isSendingDEXX = true;
        }
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }
        if (isValid) {
            this.Save();
        }
        else {
            this.StopBusyIndicator();
        }
    };
    AWBWizardComponent.prototype.SaveFSR = function () {
        this.InitFlags();
        this.IsFSRRequestButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            isValid = this.ValidateFSR();
        }
        if (isValid) {
            this.Save();
        }
    };
    AWBWizardComponent.prototype.CloseClicked = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", this.ObjectTableName));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.isConfirmCloseClicked = true;
                    var isValid = _this.ValidateShipment();
                    if (isValid) {
                        _this.Save();
                    }
                }
                else if (confirmWindow.No) {
                    _this.CloseWizardWindow();
                }
            });
        }
        else {
            this.CloseWizardWindow();
        }
    };
    AWBWizardComponent.prototype.SaveClicked = function () {
        this.InitFlags();
        this.isSaveButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.Save();
        }
        else {
            this.SaveCompleted.emit(false);
        }
    };
    AWBWizardComponent.prototype.Save = function () {
        var _this = this;
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        if (this.EntityPM.IsDirty) {
            if (this.EntityPM.Id == null) {
                this.isReloadingOnSave = true;
                this.SubmitCreatingShipment();
            }
            else {
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
                    this.StopBusyIndicator();
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Title = "Ports Changed";
                    confirmWindow.Show("Updating the Master shipment ports will update the house shipment accordingly");
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            _this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
                            _this.SubmitUpdatingShipment();
                        }
                    });
                }
                else {
                    this.SubmitUpdatingShipment();
                }
            }
        }
        else {
            this.StopBusyIndicator();
            this.OnSaveCompletedSuccessfully();
            this.SaveCompleted.emit(true);
        }
    };
    AWBWizardComponent.prototype.SubmitCreatingShipment = function () {
        var _this = this;
        if (this.myService == null) {
            this.myService = new ShipmentPMService_1.ShipmentPMService();
        }
        this.myService.insert(this.EntityPM).subscribe(function (myRespone) {
            if (!myRespone.HasError) {
                _this.EntityPM = myRespone.Result;
                if (_this.isReloadingOnSave) {
                    _this.OnSaveCompletedSuccessfully();
                }
                else {
                    _this.SaveCompleted.emit(true);
                    _this.OnSaveCompletedSuccessfully();
                }
            }
            else {
                _this.IsValidationSingleLine = true;
                _this.ValidationErrorsList = myRespone.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
                _this.SaveCompleted.emit(false);
            }
        });
    };
    AWBWizardComponent.prototype.SubmitUpdatingShipment = function () {
        var _this = this;
        if (this.myService == null) {
            this.myService = new ShipmentPMService_1.ShipmentPMService();
        }
        this.myService.update(this.EntityPM).subscribe(function (myRespone) {
            if (!myRespone.HasError) {
                _this.EntityPM = myRespone.Result;
                if (_this.isReloadingOnSave) {
                    _this.OnSaveCompletedSuccessfully();
                }
                else {
                    _this.SaveCompleted.emit(true);
                    _this.OnSaveCompletedSuccessfully();
                }
            }
            else {
                _this.IsValidationSingleLine = true;
                _this.ValidationErrorsList = myRespone.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
                _this.SaveCompleted.emit(false);
            }
        });
    };
    AWBWizardComponent.prototype.OnSaveCompletedSuccessfully = function () {
        if (this.isReloadingOnSave) {
            this.isReloadingOnSave = false;
            this.isExecutingMethod = true;
            this.ReloadShipment();
        }
        else {
            this.isExecutingMethod = true;
            this.ExecuteRequestedMethod();
        }
    };
    AWBWizardComponent.prototype.ExecuteRequestedMethod = function () {
        if (this.isExecutingMethod) {
            if (this.isSaveButtonClicked) {
                this.StopBusyIndicator();
            }
            else if (this.isFullDetailsButtonClicked) {
                //this.ViewFullDetails();
            }
            else if (this.isPrintButtonClicked) {
                this.Print();
            }
            else if (this.isSendButtonClicked) {
                this.ExecuteSend();
            }
            else if (this.isPreviewButtonClicked) {
                this.Preview();
            }
            else if (this.isCopyShipmentButtonClicked) {
                this.StopBusyIndicator();
                this.CopyShipment();
            }
            else if (this.isReactivateShipmentButtonClicked) {
                this.StopBusyIndicator();
                this.ReactivateShipment();
            }
            else if (this.isConfirmCloseClicked) {
                this.CloseWindow();
            }
            else if (this.isSendToAirlineTenantButtonClicked) {
                this.StopBusyIndicator();
                this.SendToAirlineTenant();
            }
            this.isSaveButtonClicked = false;
            this.isExecutingMethod = false;
            this.isReloadingOnSave = false;
        }
    };
    AWBWizardComponent.prototype.Print = function () {
        var isRunningPrintingManager = false;
        if (!this.IsImportWizard) {
            if (FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                if (SessionLocator_1.SessionLocator.TenantManagementJS.IsAWBStockPrepaid) {
                    var isDemoTenant = false;
                    if (this.TenantPM.Id == 65 || SessionLocator_1.SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
                        isDemoTenant = true;
                    }
                    if (!isDemoTenant) {
                        if (this.EntityPM.ShipmentLevelCode != "H") {
                            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TenantZeroAirlineTTY)) {
                                if (this.EntityPM.FWBStatusCode == "NSEN") {
                                    isRunningPrintingManager = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        if (isRunningPrintingManager) {
            this.RunPrintingManager(false);
        }
        else {
            this.ExecutePrinting();
        }
    };
    AWBWizardComponent.prototype.RunPrintingManager = function (isConfirmedByUser) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myService = new CCSWebService_1.CCSWebService();
        myService.GetAWBPrintingStock(this.EntityPM.Id, this.isSendingCargonaut, this.isSendingDEXX, isConfirmedByUser).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                var myResult = myResponse.Result;
                if (myResult) {
                    if (myResult.IsConfirmedByUser) {
                        if (myResult.IsPrintingAllowed) {
                            _this.ShowPrintingStockResult(myResult);
                        }
                        else {
                            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                            logitudeWindow.Width = 370;
                            logitudeWindow.Height = 100;
                            logitudeWindow.IsShowCloseButton = true;
                            logitudeWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/Others/PurchaseStockComponent');
                        }
                    }
                    else {
                        if (myResult.IsPrintingAllowed) {
                            _this.ExecutePrinting();
                        }
                        else {
                            _this.ConfirmPrintingStock();
                        }
                    }
                }
            }
        });
    };
    AWBWizardComponent.prototype.ConfirmPrintingStock = function () {
        var _this = this;
        var confirmMsg = "Please note that printing before sending will use one messaging stock. note that when sending the message it will be sent using the same stock";
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.ShowCancelButton = false;
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.RunPrintingManager(true);
            }
        });
    };
    AWBWizardComponent.prototype.ShowPrintingStockResult = function (myResult) {
        var _this = this;
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show("Stock before the printing: " + myResult.StockRemainingBefore + ", Stock after the printing: " + myResult.StockRemainingAfter);
        messageWindow.WindowClosed.subscribe(function ($event) {
            _this.ExecutePrinting();
        });
    };
    AWBWizardComponent.prototype.ExecutePrinting = function () {
        this.isPrintButtonClicked = true;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("AWBWizard", "Print");
        var oldCode = this.documentTypeCode;
        this.documentTypeCode = this.selectedDocumentTypeClass.Code;
        this.documentTypeName = this.selectedDocumentTypeClass.Name;
        if (oldCode == this.selectedDocumentTypeClass.Code) {
            this.StartPrint();
        }
        else {
            this.GetDocstOut();
        }
    };
    AWBWizardComponent.prototype.StartPrint = function () {
        if (this.documentTypePM != null) {
            if (this.documentTypePM.DocumentTypeDefaultReportTemplateId) {
                this.LoadPrintControl();
            }
            else {
                var docType = this.documentTypeName; //(shipmentPM.ShipmentLevelCode == "H") ? "HAWB" : "AWB";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(docType + " document has no template!!");
                this.StopBusyIndicator();
            }
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Can't find document type!!");
            this.StopBusyIndicator();
        }
    };
    AWBWizardComponent.prototype.LoadPrintControl = function () {
        // this.StopBusyIndicator();
        var _this = this;
        if (!this.isPrintWindowOpened) {
            this.documentOutPmLists = new Array();
            this.isPrintWindowOpened = true;
            this.documentOutPM.NeedsRebuild = true;
            this.documentOutPmLists.push(this.documentOutPM);
            var SelectedInternalDocument = new DocsOutDataViewModel_1.DocsOutDataViewModel(this.documentTypePM, this.EntityPM.Id, this.documentOutPM.ChildEntityId, this.targetObjectTableId, "", this.documentOutPM.ChildEntityReference, this.documentOutPmLists, null, null, this.EntityPM);
            SelectedInternalDocument.IsNotFromDocsOutListOpenPrintControl = true;
            SelectedInternalDocument.EntityId = this.EntityPM.Id;
            SelectedInternalDocument.IsAWBWizard = true;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 760;
            logitudeWindow.Height = 502;
            logitudeWindow.DataContext = SelectedInternalDocument;
            logitudeWindow.Title = "Print " + this.documentTypePM.Name;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent');
            logitudeWindow.WindowClosed.subscribe(function ($event) {
                _this.isPrintWindowOpened = false;
                _this.StopBusyIndicator();
            });
        }
    };
    AWBWizardComponent.prototype.GetDocstOut = function () {
        var _this = this;
        this._documentTypeListExtendedService.getDocumentTypeListByCode(this.documentTypeCode, this.TenantPM.Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentTypeList = myResult;
                    _this.documentTypeId = _this.documentTypeList.Id;
                    _this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
                    _this._documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(_this.EntityPM.Id, _this.TenantPM.Id, "", _this.documentTypeId).subscribe(function (res) {
                        _this.StopBusyIndicator();
                        var pmResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            _this.documentOutPM = myResult;
                            if (!_this.documentOutPM) {
                                _this._documentOutPMService.getCreateDocumentOut(_this.documentTypeId, _this.EntityPM.Id, null, null, _this.targetObjectTableId, _this.EntityPM.Tenant).subscribe(function (res) {
                                    var pmResponse = res;
                                    if (!pmResponse.HasError) {
                                        var myResult = pmResponse.Result;
                                        if (myResult) {
                                            _this.documentOutPM = myResult;
                                            _this.LoadCreatedDocMethod();
                                        }
                                        else
                                            _this.StopBusyIndicator();
                                    }
                                });
                            }
                            else {
                                _this.LoadCreatedDocMethod();
                            }
                        }
                    });
                }
                else {
                    _this.documentTypePM = null;
                    _this.StopBusyIndicator();
                }
            }
            else {
                _this.documentTypePM = null;
                _this.StopBusyIndicator();
            }
        });
    };
    AWBWizardComponent.prototype.LoadCreatedDocMethod = function () {
        var _this = this;
        this._documentOutPMService.getSingleDocumentOutPM(this.documentOutPM.Id, this.TenantPM.Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentOutPM = myResult;
                    _this.LoadDocumentTypeMethod();
                }
                else
                    _this.StopBusyIndicator();
            }
        });
    };
    AWBWizardComponent.prototype.LoadDocumentTypeMethod = function () {
        var _this = this;
        this._documentTypePMService.getSingleDocumentType(this.documentTypeId, this.documentOutPM.Id, this.TenantPM.Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.documentTypePM = myResult;
                    _this.isCurrentDocsOutLoaded = true;
                    if (_this.isPrintButtonClicked) {
                        _this.StartPrint();
                    }
                    else if (_this.isPreviewButtonClicked) {
                        _this.StartPreview();
                    }
                }
                else
                    _this.StopBusyIndicator();
            }
        });
    };
    AWBWizardComponent.prototype.StartPreview = function () {
        this.StopBusyIndicator();
        if (this.documentTypePM.DocumentTypeDefaultReportTemplateId) {
            this.LoadPreviewControl();
        }
        else {
            var docType = (this.EntityPM.ShipmentLevelCode == "H") ? "HAWB" : "AWB";
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(docType + " document has no template!!");
        }
    };
    AWBWizardComponent.prototype.LoadPreviewControl = function () {
        var _this = this;
        if (!this.isPreviewWindowOpend) {
            this.isPreviewWindowOpend = true;
            var windowArgs = {};
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            windowArgs.ModePage = "Preview";
            windowArgs.PageType = "EditDocument";
            windowArgs.WindowHeight = window.innerHeight - 100;
            windowArgs.WindowWidth = window.innerWidth - 100;
            windowArgs.CurrentDocument = this.documentOutPM;
            windowArgs.DocumentTypePM = this.documentTypePM;
            windowArgs.EntityId = this.EntityPM.Id;
            windowArgs.DataViewModel = this;
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = windowArgs.WindowWidth;
            logWindow.Height = windowArgs.WindowHeight;
            logWindow.Title = this.documentTypePM.Name + " Preview";
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/EditDocumentComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.isPreviewWindowOpend = false;
                _this.StopBusyIndicator();
            });
        }
    };
    AWBWizardComponent.prototype.PrintAWBMethod = function (item) {
        this.selectedDocumentTypeClass = item;
        this.InitFlags();
        this.isPrintButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }
        if (isValid) {
            this.Save();
        }
    };
    AWBWizardComponent.prototype.PreviewAWBMethod = function () {
        this.InitFlags();
        this.isPreviewButtonClicked = true;
        var isValid = this.ValidateShipment();
        if (isValid) {
            this.ValidateAllTabs();
            isValid = this.ValidateAWB();
        }
        if (isValid) {
            this.Save();
        }
    };
    AWBWizardComponent.prototype.Preview = function () {
        if (this.EntityPM.ShipmentLevelCode == "H") {
            this.documentTypeCode = "714";
        }
        else {
            this.documentTypeCode = "740";
        }
        this.GetDocstOut();
    };
    AWBWizardComponent.prototype.ExecuteSend = function () {
        var _this = this;
        var isValidForSending = true;
        var validationErrorMessage = "";
        var isCargonautEnabled = SessionLocator_1.SessionLocator.TenantManagementJS.IsCargonautEnabled;
        var isDEXXConnectionEnabled = SessionLocator_1.SessionLocator.TenantManagementJS.IsDEXXConnectionEnabled;
        var isDemoTenantManagement = SessionLocator_1.SessionLocator.TenantManagementJS.IsEAWBOnlyDemo;
        var myCCSValidator = Tools_2.AWBHelper.ValidateAWBCCS(this.EntityPM);
        if (isValidForSending) {
            if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId == null) {
                isValidForSending = false;
                validationErrorMessage = " FHL can’t be sent, House isn't connected to a Master shipment";
            }
        }
        if (isValidForSending) {
            if (!isDemoTenantManagement) {
                if (myCCSValidator.TenantManagementFieldHasError) {
                    isValidForSending = false;
                    validationErrorMessage = myCCSValidator.TenantManagementFieldErrorMessage;
                }
            }
        }
        if (isValidForSending) {
            if (this.isSendingCargonaut && !isCargonautEnabled) {
                isValidForSending = false;
                validationErrorMessage = "Cargonaut is not connected. Please contact your account manager";
            }
            else if (this.isSendingDEXX && !isDEXXConnectionEnabled) {
                isValidForSending = false;
                validationErrorMessage = "DEXX is not connected. Please contact your account manager";
            }
        }
        if (!this.isSendingCargonaut && !this.isSendingDEXX) {
            if (isValidForSending) {
                if (myCCSValidator.AirlineFieldHasError) {
                    isValidForSending = false;
                    validationErrorMessage = myCCSValidator.AirlineFieldErrorMessage;
                }
            }
            if (isValidForSending) {
                if (this.EntityPM.ShipmentLevelCode == "H" || this.isSendingFHLs) {
                    if (!myCCSValidator.FHL) {
                        isValidForSending = false;
                        validationErrorMessage = "This airline will not receive FHL";
                    }
                }
            }
            if (isValidForSending) {
                if (!myCCSValidator.FWB) {
                    isValidForSending = false;
                    validationErrorMessage = "This airline will not receive FWB";
                }
            }
            if (isValidForSending) {
                if (!isDemoTenantManagement) {
                    if (myCCSValidator.AirlineRegistrationHasError) {
                        isValidForSending = false;
                        validationErrorMessage = myCCSValidator.AirlineRegistrationErrorMessage;
                    }
                }
            }
        }
        if (!isValidForSending) {
            this.StopBusyIndicator();
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(validationErrorMessage);
        }
        else {
            var RateDescriptionMaxOccurs = 11;
            var DimensionsLinesMaxOccurs = RateDescriptionMaxOccurs - 1;
            if (this.IsFWB) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainHarmonize)) {
                    if (this.EntityPM.MainHarmonize.length >= 6 && this.EntityPM.MainHarmonize.length <= 18) {
                        if (Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.MainHarmonize)) {
                            DimensionsLinesMaxOccurs -= 1;
                        }
                    }
                }
            }
            if (this.EntityPM.ShipmentPackages.length > DimensionsLinesMaxOccurs) {
                this.StopBusyIndicator();
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show("Due to IATA limitation, please notice that not all the packages lines full details will be sent, some lines will be sent as a part of the total commodity.");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.ExecuteSendConfirmed();
                    }
                });
            }
            else {
                this.ExecuteSendConfirmed();
            }
        }
    };
    AWBWizardComponent.prototype.ExecuteSendConfirmed = function () {
        var windowTitle = "";
        var totangoUserActivity = "";
        if (this.isSendingFHLs) {
            if (this.isSendingCargonaut) {
                windowTitle = "Send All Cargonaut FHL(s)";
                totangoUserActivity = "Sending All Cargonaut FHL(s)";
            }
            else if (this.isSendingDEXX) {
                windowTitle = "Send All DEXX FHL(s)";
                totangoUserActivity = "Sending All DEXX FHL(s)";
            }
            else {
                windowTitle = "Send All FHL(s)";
                totangoUserActivity = "Sending All FHL(s)";
            }
        }
        else if (this.EntityPM.ShipmentLevelCode == "H") {
            if (this.isSendingCargonaut) {
                windowTitle = "Send Cargonaut FHL";
                totangoUserActivity = "Sending Cargonaut FHL";
            }
            else if (this.isSendingDEXX) {
                windowTitle = "Send DEXX FHL";
                totangoUserActivity = "Sending DEXX FHL";
            }
            else {
                windowTitle = "Send FHL";
                totangoUserActivity = "Sending FHL";
            }
        }
        else {
            if (this.isSendingCargonaut) {
                windowTitle = "Send Cargonaut FWB";
                totangoUserActivity = "Sending Cargonaut FWB";
            }
            if (this.isSendingDEXX) {
                windowTitle = "Send DEXX FWB";
                totangoUserActivity = "Sending DEXX FWB";
            }
            else {
                windowTitle = "Send FWB";
                totangoUserActivity = "Sending FWB";
            }
        }
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("AWBWizard", totangoUserActivity);
        this.RunSendWindow(windowTitle);
    };
    AWBWizardComponent.prototype.RunSendWindow = function (windowTitle) {
        var _this = this;
        this.StopBusyIndicator();
        if (!this.isSendWindowOpen) {
            this.isSendWindowOpen = true;
            var args = new Args_1.SendAWBArgs();
            args.EnttiyPM = this.EntityPM;
            args.Wizard = this;
            args.IsSendingFHLs = this.isSendingFHLs;
            args.IsSendingDEXX = this.isSendingDEXX;
            args.IsSendingCargonaut = this.isSendingCargonaut;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = args;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/SendWindowComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.isSendWindowOpen = false;
                _this.StopBusyIndicator();
            });
        }
    };
    AWBWizardComponent.prototype.CloseWizardWindow = function () {
        this.CloseWindow();
    };
    AWBWizardComponent.prototype.CloseWindow = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AWBWizardComponent.prototype.CopyShipment = function () {
        var windowArgs = new Args_1.AWBWizardArgs();
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.ShipmentLevelCode = this.EntityPM.ShipmentLevelCode;
        windowArgs.IsCopyFromShipment = true;
        windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "Copy Shipment";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
    };
    AWBWizardComponent.prototype.ReactivateShipment = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Title = "Reactivate Shipment";
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnShipmentReactivate($event); });
        logWindow.Show('./Shipment/Components/Reactivate/ReactivateShipmentComponent');
    };
    AWBWizardComponent.prototype.SendToAirlineTenant = function () {
        var _this = this;
        this.StartBusyIndicator("Sending...");
        var myService = new InfrastructureDomainService_1.InfrastructureDomainService();
        myService.SendEntityToAirlineTenant(this.EntityPM.Id, "Shipment", this.EntityPM.MainCarriageCarrierCode).subscribe(function (myResponse) {
            _this.StopBusyIndicator();
            if (myResponse.HasError) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
            }
        });
    };
    AWBWizardComponent.prototype.OnShipmentReactivate = function ($event) {
        if ($event == 'OK') {
            this.SetMoreButtons();
            this.SaveCompleted.emit(true);
        }
    };
    AWBWizardComponent.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    AWBWizardComponent.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    AWBWizardComponent.prototype.ReloadShipment = function () {
        var _this = this;
        var myService = new ShipmentPMService_1.ShipmentPMService();
        myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.LoadCompleted.emit(false);
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    _this.LoadCompleted.emit(true);
                }
            }
            _this.CurrentSession.StopBusyIndicator();
            _this.SetMoreButtons();
            _this.ExecuteRequestedMethod();
        });
    };
    AWBWizardComponent.prototype.ReloadEntity = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myService == null) {
            this.myService = new ShipmentPMService_1.ShipmentPMService();
        }
        this.myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.LoadCompleted.emit(true);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.LoadCompleted.emit(false);
                }
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], AWBWizardComponent.prototype, "LoadCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], AWBWizardComponent.prototype, "SaveCompleted", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AWBWizardComponent.prototype, "AllLocations", void 0);
    AWBWizardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AWBWizardComponent',
            templateUrl: './AWBWizardComponent.html',
            providers: [EntityArgs_1.EntityArgs, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService, DocumentOutPMService_1.DocumentOutPMService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, DocumentTypeListExtendedService_1.DocumentTypeListExtendedService, DocumentOutPMService_1.DocumentOutPMService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService])
    ], AWBWizardComponent);
    return AWBWizardComponent;
}());
exports.AWBWizardComponent = AWBWizardComponent;
var DocumentTypeClass = /** @class */ (function () {
    function DocumentTypeClass(code, name) {
        this.Code = null;
        this.Name = null;
        this.Code = code;
        this.Name = name;
    }
    return DocumentTypeClass;
}());
//# sourceMappingURL=AWBWizardComponent.js.map