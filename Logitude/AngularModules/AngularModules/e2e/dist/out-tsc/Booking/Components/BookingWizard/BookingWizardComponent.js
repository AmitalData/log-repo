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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var AWBUtilities_1 = require("../../Utilities/AWBUtilities");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var BookingPM_1 = require("../../EntityPMs/BookingPM");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var BookingPMService_1 = require("../../Services/StandardPMs/BookingPMService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var AirlineListService_1 = require("../../../Common/Services/StandardLists/AirlineListService");
var StateListService_1 = require("../../../Common/Services/StandardLists/StateListService");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var FFRWebService_1 = require("../../../Infrastructure/Services/WebServices/FFRWebService");
var Tools_2 = require("../../Tools");
var Args_1 = require("../../Args");
var Args_2 = require("../../../Shipment/Args");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var BookingDomainService_1 = require("../../Services/BookingDomainService");
var FSRWebService_1 = require("../../../Infrastructure/Services/WebServices/FSRWebService");
var InfrastructureDomainService_1 = require("../../../Infrastructure/Services/InfrastructureDomainService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var BookingWizardComponent = /** @class */ (function () {
    function BookingWizardComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.LoadCompleted = new core_1.EventEmitter();
        this.SaveCompleted = new core_1.EventEmitter();
        this.DataContext = this;
        this.IsNewEntity = false;
        this.IsSendResponseVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
        this.Retries = 0;
        this.SendFFRContent = "";
        this.IsCopyButtonEnabled = false;
        this.IsCancelButtonEnabled = false;
        this.IsReactivateButtonEnabled = false;
        this.IsSendCancellationEnabled = false;
        this.IsSaveButtonEnabled = false;
        this.IsBuildShipmentEnabled = false;
        this.IsSendResponseEnabled = false;
        this.IsSendToAirlineTenantVisible = false;
        this.AllStates = [];
        this.IsTabVisible_OVE = false;
        this.PageChild_OVE = null;
        this.PageChild_BKD = null;
        this.PageChild_PAR = null;
        this.PageChild_PAC = null;
        this.PageChild_GEN = null;
        this.PageChild_EVE = null;
        this.AirlineRulesList = [];
        this.TabErrors_BKD = [];
        this.TabErrors_PAR = [];
        this.TabErrors_PAC = [];
        this.TabErrors_GEN = [];
        this.TabWarnings_BKD = [];
        this.TabWarnings_PAR = [];
        this.TabWarnings_PAC = [];
        this.TabWarnings_GEN = [];
        this.Fill_BKD = null;
        this.Fill_PAR = null;
        this.Fill_PAC = null;
        this.Fill_GEN = null;
        this.isSaveButtonClicked = false;
        this.isSendFFRButtonClicked = false;
        this.isSendCancellationButtonClicked = false;
        this.isConfirmCloseClicked = false;
        this.isCopyBookingButtonClicked = false;
        this.isCancelBookingButtonClicked = false;
        this.isReactivateButtonClicked = false;
        this.isBuildButtonClicked = false;
        this.isFSRRequestButtonClicked = false;
        this.isManualStatusClicked = false;
        this.isSendToAirlineTenantButtonClicked = false;
        this.isCancellationSent = false;
        this.mySendingResultClass = new FFRWebService_1.FFRResult();
        this.runOverview = false;
        this.shipmentLevelCode = null;
        this.shipperPartnerType = null;
        this.consigneePartnerType = null;
        this.IsResponseProgressVisible = false;
        this.TimerStoppedByUser = false;
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Booking", "BOOKINGSENDRESPONSE")) {
            this.IsSendResponseVisible = true;
        }
    }
    BookingWizardComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.WindowArgs = windowArgs;
        this.InitializeWizard();
        this.RunComponent();
    };
    BookingWizardComponent.prototype.ngAfterViewInit = function () {
        //this.isViewInited = true;
        //this.InitializeComponent();
    };
    BookingWizardComponent.prototype.RunComponent = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Booking", "Booking Wizard");
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
    BookingWizardComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    BookingWizardComponent.prototype.InitializeWizard = function () {
        if (this.WindowArgs != null) {
            this.IsNewEntity = this.WindowArgs.IsNewEntity;
            this.ObjectTableName = "Booking";
            if (this.IsNewEntity) {
                this.CreateBooking();
            }
            else {
                this.EntityPM = this.WindowArgs.EntityPM;
            }
            this.entityArgs.EntityPM = this.EntityPM;
            this.entityArgs.ObjectTableName = this.ObjectTableName;
            this.SetSelectedTab();
            this.SetButtonsProperties();
        }
    };
    BookingWizardComponent.prototype.SetButtonsProperties = function () {
        var isCopyEnabled = true;
        var isCancelEnabled = true;
        var isReacivateEnabled = true;
        var isSendCancellationEnabled = true;
        var isSaveEnabled = true;
        var isBuildShipmentEnabled = false;
        var isSendResponseEnabled = true;
        if (this.EntityPM.IsCancelled) {
            isCopyEnabled = false;
            isSendResponseEnabled = false;
        }
        if (this.EntityPM.BookingStatusCode == "CRT"
            || this.EntityPM.BookingStatusCode == "AWB"
            || this.EntityPM.IsCancelled
            || (this.EntityPM.WaitingForResponse && this.EntityPM.FFRStatusCode == "CRS")
            || this.EntityPM.FFRStatusCode == "RBC") {
            isSendCancellationEnabled = false;
        }
        if (this.EntityPM.BookingStatusCode == "CNF" || this.EntityPM.BookingStatusCode == "AWB" || this.EntityPM.IsCancelled) {
            isCancelEnabled = false;
        }
        if (this.EntityPM.BookingStatusCode == "CNF" || this.EntityPM.BookingStatusCode == "AWB" || !this.EntityPM.IsCancelled) {
            isReacivateEnabled = false;
        }
        if (this.EntityPM.BookingStatusCode == "AWB" || this.EntityPM.IsCancelled || this.EntityPM.BookingStatusCode == "CNF" || (this.EntityPM.WaitingForResponse && this.EntityPM.FFRStatusCode == "BRQ")) {
            isSaveEnabled = false;
        }
        if (this.EntityPM.BookingStatusCode == "CRT") {
            this.SendFFRContent = "Request Booking";
        }
        else {
            this.SendFFRContent = "Update Booking";
        }
        if (this.CurrentSession.CurrentWindow != null) {
            this.CurrentSession.CurrentWindow.ShowCancelControl(this.EntityPM.IsCancelled);
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            if ((this.EntityPM.BookingStatusCode == "CNF" || this.EntityPM.BookingStatusCode == "WCF") && !this.EntityPM.IsCancelled) {
                isBuildShipmentEnabled = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Booking", "Booking.Action.SendToAirlineTenant")) {
            this.IsSendToAirlineTenantVisible = true;
        }
        this.IsCopyButtonEnabled = isCopyEnabled;
        this.IsCancelButtonEnabled = isCancelEnabled;
        this.IsReactivateButtonEnabled = isReacivateEnabled;
        this.IsSendCancellationEnabled = isSendCancellationEnabled;
        this.IsSaveButtonEnabled = isSaveEnabled;
        this.IsBuildShipmentEnabled = isBuildShipmentEnabled;
        this.IsSendResponseEnabled = isSendResponseEnabled;
    };
    Object.defineProperty(BookingWizardComponent.prototype, "IsSaveButtonVisible", {
        get: function () {
            var myResult = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                myResult = true;
            }
            else if (this.EntityPM.BookingStatusCode == "CRT" && this.EntityPM.IsDirty) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingWizardComponent.prototype, "IsUpdateBookingVisible", {
        get: function () {
            var myResult = false;
            if (!this.EntityPM.IsCancelled) {
                if (this.EntityPM.BookingStatusCode != "CRT" && this.EntityPM.IsDirty) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingWizardComponent.prototype, "IsSendFFREnabled", {
        get: function () {
            var myResult = false;
            if (this.EntityPM.BookingStatusCode == "CRT") {
                myResult = true;
            }
            else if (this.EntityPM.BookingStatusCode == "AWB") {
                myResult = false;
            }
            else {
                if (this.EntityPM.IsDirty) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    BookingWizardComponent.prototype.InitializeComponent = function () {
        if (this.WindowArgs != null && this.isViewInited) {
            this.SelectionChanged();
            this.ValidateAllTabs();
            this.LoadAirlineRules(this.EntityPM.MainCarriageCarrierCode);
            this.LoadAllowedAirline();
            this.LoadAllStates();
        }
    };
    BookingWizardComponent.prototype.LoadAllStates = function () {
        var _this = this;
        if (this.myStateListService == null) {
            this.myStateListService = new StateListService_1.StateListService();
        }
        this.myStateListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllStates = myResponse.Result;
                _this.ValidateScreen_PAR();
            }
        });
    };
    BookingWizardComponent.prototype.CreateBooking = function () {
        var _this = this;
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM = new BookingPM_1.BookingPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        this.EntityPM.DirectionCode = "E";
        this.EntityPM.TransportModeCode = "A";
        this.EntityPM.SpaceAllocationCode = "NN";
        this.EntityPM.MainCarriageSpaceAllocationCode = "NN";
        this.EntityPM.BookingStatusCode = "CRT";
        this.EntityPM.BookingStatusName = "Created";
        this.EntityPM.FFRStatusCode = "NST";
        this.EntityPM.FFRStatusName = "Not Sent";
        this.EntityPM.DimensionsUnitCode = this.TenantPM.DimensionsUnitCode;
        this.EntityPM.VolumeUnitCode = this.TenantPM.VolumeUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.TenantPM.GrossWeightUnitCode;
        this.EntityPM.ChargeableWeightUnitCode = this.TenantPM.ChargeableWeightUnitCode;
        if (this.WindowArgs.IsCopyFromBooking || this.WindowArgs.IsBuiltFromSchedule) {
            var oldBooking = this.WindowArgs.EntityPM;
            Tools_2.BookingTool.CopyBooking(this.EntityPM, oldBooking);
            Tools_2.BookingTool.CopyBookingPackages(this.EntityPM, oldBooking);
        }
        else {
            this.EntityPM.CASSCode = this.TenantPM.CASSCode;
            this.EntityPM.IssuingCarrierIATACode = this.TenantPM.IATA;
            this.EntityPM.IssuingCarrierAgentId = this.TenantPM.AgentId;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierAgentId)) {
                var myService = new CardListService_1.CardListService();
                myService.getSingle(this.EntityPM.IssuingCarrierAgentId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list != null) {
                            _this.EntityPM.IssuingCarrierAddressId = list.MainAddressId;
                        }
                        _this.ValidateScreen_PAR();
                    }
                });
            }
        }
    };
    BookingWizardComponent.prototype.SetSelectedTab = function () {
        if (this.IsNewEntity) {
            this.selectedTabCode = "BKD";
        }
        else {
            this.IsTabVisible_OVE = true;
            this.selectedTabCode = "OVE";
        }
    };
    Object.defineProperty(BookingWizardComponent.prototype, "SelectedTabCode", {
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
    BookingWizardComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isViewInited) {
            if (this.SelectedTabCode != null) {
                var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                if (myLocation != null) {
                    switch (this.SelectedTabCode) {
                        case "OVE": {
                            if (this.PageChild_OVE == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/Overview/OverviewTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_OVE = cmpRef.instance;
                                    _this.PageChild_OVE.InitTab(_this);
                                    if (_this.runOverview) {
                                        _this.PageChild_OVE.SetDataAfterSending();
                                    }
                                });
                            }
                            else {
                                this.PageChild_OVE.RefreshTab();
                                if (this.runOverview) {
                                    this.PageChild_OVE.SetDataAfterSending();
                                }
                            }
                            break;
                        }
                        case "BKD": {
                            if (this.PageChild_BKD == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/BookingDetails/BookingDetailsTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_BKD = cmpRef.instance;
                                    _this.PageChild_BKD.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_BKD.RefreshTab();
                            }
                            break;
                        }
                        case 'PAR': {
                            if (this.PageChild_PAR == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/Partners/PartnersTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_PAR = cmpRef.instance;
                                    _this.PageChild_PAR.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_PAR.RefreshTab();
                            }
                            break;
                        }
                        case "PAC": {
                            if (this.PageChild_PAC == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/Packages/PackagesTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_PAC = cmpRef.instance;
                                    _this.PageChild_PAC.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_PAC.RefreshTab();
                            }
                            break;
                        }
                        case "GEN": {
                            if (this.PageChild_GEN == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/GeneralDetails/GeneralDetailsTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_GEN = cmpRef.instance;
                                    _this.PageChild_GEN.InitTab(_this);
                                });
                            }
                            else {
                                this.PageChild_GEN.RefreshTab();
                            }
                            break;
                        }
                        case "EVE": {
                            if (this.PageChild_EVE == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Common/Components/Events/EventsTabComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.PageChild_EVE = cmpRef.instance;
                                });
                            }
                            else {
                                this.PageChild_EVE.LoadData();
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    BookingWizardComponent.prototype.OverviewDirectiveLoaded = function () {
        if (this.SelectedTabCode == "OVE") {
            if (this.PageChild_OVE == null) {
                this.SelectionChanged();
            }
        }
    };
    // Allowed Airline
    BookingWizardComponent.prototype.LoadAllowedAirline = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) && this.EntityPM.TransportModeCode == "A") {
            if (this.WindowArgs.IsCopyFromBooking || this.WindowArgs.IsBuiltFromSchedule) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                    this.GetMainCarriageCarrier();
                }
            }
            else if (SessionLocator_1.SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
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
    };
    BookingWizardComponent.prototype.GetMainCarriageCarrier = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            this.EntityPM.AirlinePrefix = null;
            this.EntityPM.Master = null;
            this.EntityPM.AccountNumber = null;
            this.EntityPM.MainCarriageCarrierPrefix = null;
            this.EntityPM.MainCarriageCarrierNumber = null;
            this.EntityPM.MainCarriageCarrierName = null;
            this.EntityPM.LongMaster = null;
            this.EntityPM.TenantZeroAirlineId = null;
            this.EntityPM.TenantZeroAirlineTTY = null;
            this.EntityPM.TenantZeroAirlinePIMA = null;
            this.EntityPM.TenantZeroAirlineChampFFR = false;
            this.EntityPM.TenantZeroAirlineGLSHKFFR = false;
            this.EntityPM.TenantZeroAirlineChampFVR = false;
            this.EntityPM.TenantZeroAirlineGLSHKFVR = false;
            this.EntityPM.ZeroChampNeedsRegistration = false;
            this.EntityPM.ZeroGLSHKNeedsRegistration = false;
            this.EntityPM.TenantZeroIsManagingProduct = false;
            this.EntityPM.TenantZeroIsProductMandatory = false;
            this.EntityPM.ZeroIsDescOfGoodsFromList = false;
            this.EntityPM.CarrierIsChampRegistered = false;
            this.EntityPM.CarrierIsGLSHKRegistered = false;
            this.EntityPM.CarrierIsCheckDigit = false;
            this.EntityPM.CarrierIsLimitedLength = false;
            this.OnLoadingAllowedAirlineCompleted();
        }
        else {
            this.GetCarrier();
            this.GetAirline();
        }
    };
    BookingWizardComponent.prototype.GetCarrier = function () {
        var _this = this;
        var myService = new CardListService_1.CardListService();
        myService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list != null) {
                    _this.EntityPM.MainCarriageCarrierPrefix = list.Code;
                    _this.EntityPM.AccountNumber = list.AirlineAccountNumber;
                    _this.EntityPM.MainCarriageCarrierCode = list.Code;
                    _this.EntityPM.MainCarriageCarrierName = list.EnglishName;
                }
                _this.OnLoadingAllowedAirlineCompleted();
            }
        });
    };
    BookingWizardComponent.prototype.GetAirline = function () {
        var _this = this;
        var myService = new AirlineListService_1.AirlineListService();
        myService.getSingle(this.EntityPM.MainCarriageCarrierId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list != null) {
                    var myPrefix = null;
                    if (!Tools_1.AppTool.IsNullOrEmpty(list.Prefix)) {
                        myPrefix = list.Prefix.toString().trim();
                        myPrefix = Tools_1.AppTool.PadLeft(myPrefix, 3, "0");
                    }
                    _this.EntityPM.AirlinePrefix = myPrefix;
                    _this.EntityPM.LongMaster = myPrefix + "-";
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Master)) {
                        _this.EntityPM.LongMaster = _this.EntityPM.LongMaster + _this.EntityPM.Master;
                    }
                    _this.EntityPM.CarrierIsChampRegistered = list.IsChampRegistered;
                    _this.EntityPM.CarrierIsGLSHKRegistered = list.IsGLSHKRegistered;
                    _this.EntityPM.CarrierIsCheckDigit = list.CheckDigit;
                    _this.EntityPM.CarrierIsLimitedLength = list.LimitedLength;
                    _this.GetTenantZeroAirline(list.Code);
                }
            }
        });
    };
    BookingWizardComponent.prototype.GetTenantZeroAirline = function (myCode) {
        var _this = this;
        this.myPartnersDomainService.GetAirlineByCode(myCode, 0).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var airlinePM = myResponse.Result;
                if (airlinePM != null) {
                    _this.EntityPM.TenantZeroAirlineId = airlinePM.Id;
                    _this.EntityPM.TenantZeroAirlineTTY = airlinePM.TTY;
                    _this.EntityPM.TenantZeroAirlinePIMA = airlinePM.GLSHKPIMA;
                    _this.EntityPM.TenantZeroAirlineChampFFR = airlinePM.ChampFFRFFA;
                    _this.EntityPM.TenantZeroAirlineGLSHKFFR = airlinePM.GLSHKFFRFFA;
                    _this.EntityPM.TenantZeroAirlineChampFVR = airlinePM.ChampFVRFVA;
                    _this.EntityPM.TenantZeroAirlineGLSHKFVR = airlinePM.GLSHKFVRFVA;
                    _this.EntityPM.ZeroChampNeedsRegistration = airlinePM.ChampNeedsRegistration;
                    _this.EntityPM.ZeroGLSHKNeedsRegistration = airlinePM.GLSHKNeedsRegistration;
                    _this.EntityPM.TenantZeroIsManagingProduct = airlinePM.IsManagingProduct;
                    _this.EntityPM.TenantZeroIsProductMandatory = airlinePM.IsProductMandatory;
                    _this.EntityPM.ZeroIsDescOfGoodsFromList = airlinePM.IsDescriptionOfGoodsFromList;
                }
                _this.OnLoadingAllowedAirlineCompleted();
            }
        });
    };
    BookingWizardComponent.prototype.OnLoadingAllowedAirlineCompleted = function () {
        this.ValidateScreen_BKD();
        this.PageChild_BKD.RefreshTab();
    };
    BookingWizardComponent.prototype.LoadAirlineRules = function (myAirlineCode) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(myAirlineCode)) {
            this.AirlineRulesList = [];
            this.ValidateAllTabs();
            this.RefreshTab(this.SelectedTabCode);
        }
        else {
            this.myPartnersDomainService.GetAirlineRules(myAirlineCode, "FFR").subscribe(function (myResult) {
                if (myResult == null) {
                    _this.AirlineRulesList = [];
                    _this.ValidateAllTabs();
                    _this.RefreshTab(_this.SelectedTabCode);
                }
                else {
                    var myResponse = myResult;
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
    BookingWizardComponent.prototype.RefreshTab = function (tabCode) {
        switch (tabCode) {
            case "OVE": {
                if (this.PageChild_OVE != null) {
                    this.PageChild_OVE.RefreshTab();
                }
                break;
            }
            case "BKD": {
                if (this.PageChild_BKD != null) {
                    this.PageChild_BKD.RefreshTab();
                }
                break;
            }
            case "PAR": {
                if (this.PageChild_PAR != null) {
                    this.PageChild_PAR.RefreshTab();
                }
                break;
            }
            case "PAC": {
                if (this.PageChild_PAC != null) {
                    this.PageChild_PAC.RefreshTab();
                }
                break;
            }
            case "GEN": {
                if (this.PageChild_GEN != null) {
                    this.PageChild_GEN.RefreshTab();
                }
                break;
            }
            case "EVE": {
                if (this.PageChild_EVE != null) {
                    this.PageChild_EVE.LoadData();
                }
                break;
            }
        }
    };
    BookingWizardComponent.prototype.ValidateAllTabs = function () {
        this.ValidationText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.ValidateScreen_BKD();
        this.ValidateScreen_PAR();
        this.ValidateScreen_PAC();
        this.ValidateScreen_GEN();
    };
    BookingWizardComponent.prototype.ValidateScreen_BKD = function () {
        var screenErrors = [];
        var screenWarnings = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
            screenErrors.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.S.Routings.Airline")));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Master"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFromPortId)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Departure"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageFinalDestinationPortId)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Destination"));
        }
        //Main carriage
        var codePrefix = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierPrefix) ? this.EntityPM.MainCarriageCarrierPrefix : this.EntityPM.MainCarriageCarrierPrefix.trim();
        if (Tools_1.AppTool.IsNullOrEmpty(codePrefix)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Carrier Prefix"));
        }
        else if (codePrefix.length != 2) {
            screenWarnings.push("Main Carriage Carrier Prefix length must be 2");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierNumber)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.S.Routings.FlightNo")));
        }
        else {
            if (this.isSendFFRButtonClicked || this.isSendCancellationButtonClicked) {
                if (!Tools_1.FormatTool.Validate_FlightNumber(this.EntityPM.MainCarriageCarrierNumber)) {
                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".O." + "Routings.FlightNo");
                    screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                }
            }
        }
        if (this.EntityPM.MainCarriageETD == null) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage ETD"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageSpaceAllocationCode)) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Space Allocation"));
        }
        else {
            if (this.EntityPM.MainCarriageSpaceAllocationCode == "CA") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageAllotmentIdentification)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Main Carriage Allotment Identification"));
                }
            }
        }
        //Transshipment1
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 2 carrier"));
            }
            var codePrefix1 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierPrefix) ? this.EntityPM.Transshipment1CarrierPrefix : this.EntityPM.Transshipment1CarrierPrefix.trim();
            if (Tools_1.AppTool.IsNullOrEmpty(codePrefix1)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Carrier Prefix"));
            }
            else if (codePrefix1.length != 2) {
                screenWarnings.push("Transshipment1 Carrier Prefix length must be 2");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierNumber)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Flight No."));
            }
            else {
                if (!Tools_1.FormatTool.Validate_FlightNumber(this.EntityPM.Transshipment1CarrierNumber)) {
                    var fieldName = "Transshipment1 Flight No.";
                    screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                }
            }
            if (this.EntityPM.Transshipment1ETD == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 ETD"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1SpaceAllocationCode)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Space Allocation"));
            }
            else {
                if (this.EntityPM.Transshipment1SpaceAllocationCode == "CA") {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1AllotmentIdentification)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment1 Allotment Identification"));
                    }
                }
            }
        }
        //Transshipment2
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortId)) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Main carriage leg 3 carrier"));
            }
            var codePrefix2 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierPrefix) ? this.EntityPM.Transshipment2CarrierPrefix : this.EntityPM.Transshipment2CarrierPrefix.trim();
            if (Tools_1.AppTool.IsNullOrEmpty(codePrefix2)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Carrier Prefix"));
            }
            else if (codePrefix2.length != 2) {
                screenWarnings.push("Transshipment2 Carrier Prefix length must be 2");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierNumber)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Flight No."));
            }
            else {
                if (!Tools_1.FormatTool.Validate_FlightNumber(this.EntityPM.Transshipment2CarrierNumber)) {
                    var fieldName = "Transshipment2 Flight No.";
                    screenWarnings.push(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                }
            }
            if (this.EntityPM.Transshipment2ETD == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 ETD"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2SpaceAllocationCode)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Space Allocation"));
            }
            else {
                if (this.EntityPM.Transshipment2SpaceAllocationCode == "CA") {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2AllotmentIdentification)) {
                        screenWarnings.push(this.ValidationText.replace("%FieldName", "Transshipment2 Allotment Identification"));
                    }
                }
            }
        }
        this.TabErrors_BKD = screenErrors; //this.TabErrors_BKD.concat(screenErrors);
        this.TabWarnings_BKD = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "BKD");
    };
    BookingWizardComponent.prototype.ValidateScreen_PAR = function () {
        var screenErrors = [];
        var screenWarnings = [];
        this.ValidateScreen_PAR_Shipper(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_Consignee(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_IssuingAgent(screenErrors, screenWarnings);
        this.ValidateScreen_PAR_AirlineRules(screenErrors, screenWarnings);
        this.TabErrors_PAR = screenErrors;
        this.TabWarnings_PAR = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAR");
    };
    BookingWizardComponent.prototype.ValidateScreen_PAR_Shipper = function (screenErrors, screenWarnings) {
        if (this.EntityPM.ShipperId != null) {
            if (!AWBUtilities_1.AWBUtilities.IsText(this.EntityPM.ShipperName)) {
                screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Shipper Name"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddressId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address"));
            }
            else {
                var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress1) ? null : this.EntityPM.ShipperAddress1.trim();
                var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperAddress2) ? null : this.EntityPM.ShipperAddress2.trim();
                var myZipCode = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperZipCode) ? null : this.EntityPM.ShipperZipCode.trim();
                var myCity = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperCity) ? null : this.EntityPM.ShipperCity.trim();
                if (!Tools_1.FormatTool.IsTextFormatted(myAddress1)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper Address1"));
                }
                if (!Tools_1.FormatTool.IsTextFormatted(myAddress2)) {
                    screenWarnings.push(Tools_1.FormatTool.GetWrongTextFormatMessage("Shipper Address2"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper Address1 Or Address2"));
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                    if (!AWBUtilities_1.AWBUtilities.IsText(myZipCode)) {
                        screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Shipper Zip Code"));
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Shipper City"));
                }
                else if (!AWBUtilities_1.AWBUtilities.IsText(myCity)) {
                    screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Shipper City"));
                }
            }
        }
    };
    BookingWizardComponent.prototype.ValidateScreen_PAR_Consignee = function (screenErrors, screenWarnings) {
        if (this.EntityPM.ConsigneeId != null) {
            if (!AWBUtilities_1.AWBUtilities.IsText(this.EntityPM.ConsigneeName)) {
                screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Consignee Name"));
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddressId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address"));
            }
            else {
                var myAddress1 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress1) ? null : this.EntityPM.ConsigneeAddress1.trim();
                var myAddress2 = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeAddress2) ? null : this.EntityPM.ConsigneeAddress2.trim();
                var myZipCode = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeZipCode) ? null : this.EntityPM.ConsigneeZipCode.trim();
                var myCity = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeCity) ? null : this.EntityPM.ConsigneeCity.trim();
                if (!AWBUtilities_1.AWBUtilities.IsText(myAddress1)) {
                    screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Consignee Address1"));
                }
                if (!AWBUtilities_1.AWBUtilities.IsText(myAddress2)) {
                    screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Consignee Address2"));
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myAddress1) && Tools_1.AppTool.IsNullOrEmpty(myAddress2)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee Address1 Or Address2"));
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(myZipCode)) {
                    if (!AWBUtilities_1.AWBUtilities.IsText(myZipCode)) {
                        screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Consignee Zip Code"));
                    }
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myCity)) {
                    screenWarnings.push(this.ValidationText.replace("%FieldName", "Consignee City"));
                }
                else if (!AWBUtilities_1.AWBUtilities.IsText(myCity)) {
                    screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Consignee City"));
                }
            }
        }
    };
    BookingWizardComponent.prototype.ValidateScreen_PAR_IssuingAgent = function (screenErrors, screenWarnings) {
        if (this.EntityPM.IssuingCarrierAgentId == null) {
            screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent"));
        }
        else {
            if (!AWBUtilities_1.AWBUtilities.IsText(this.EntityPM.IssuingCarrierAgentName)) {
                screenWarnings.push(AWBUtilities_1.AWBUtilities.GetWrongTextFormatMessage("Issuing Carrier Agent Name"));
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierIATACode)) {
                if (!AWBUtilities_1.AWBUtilities.FormateValidate_IATACode(this.EntityPM.IssuingCarrierIATACode)) {
                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "IssuingCarrierIATACode");
                    screenWarnings.push(fieldName + " wrong format: must be 7 numeric digits max");
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CASSCode)) {
                if (!AWBUtilities_1.AWBUtilities.FormateValidate_CASSCode(this.EntityPM.CASSCode)) {
                    var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + "CASSCode");
                    screenWarnings.push(fieldName + " wrong format: must be 4 numeric digits max");
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.IssuingCarrierAddressId)) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Issuing Carrier Agent Address"));
            }
        }
    };
    BookingWizardComponent.prototype.ValidateScreen_PAR_AirlineRules = function (screenErrors, screenWarnings) {
        this.ValidateAirlineRule("ShipperId", this.EntityPM.ShipperId, screenWarnings);
        this.ValidateAirlineRule("ConsigneeId", this.EntityPM.ConsigneeId, screenWarnings);
        this.ValidateAirlineRule("IssuingCarrierIATACode", this.EntityPM.IssuingCarrierIATACode, screenWarnings);
        this.ValidateAirlineRule("CASSCode", this.EntityPM.CASSCode, screenWarnings);
    };
    BookingWizardComponent.prototype.ValidateScreen_PAC = function () {
        var screenErrors = [];
        var screenWarnings = [];
        if (this.EntityPM.BookingPackages.length > 10) {
            screenErrors.push("You have exceeded the allowable limit of 10 lines of packages");
        }
        if (Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight)) {
            var msgField = TextCodeTranslator_1.TextCodeTranslator.Translate("Booking.F.GrossWeight");
            msgField = msgField.replace("%GrossWeightCode", this.EntityPM.GrossWeightUnitCode);
            screenWarnings.push(this.ValidationText.replace("%FieldName", msgField));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DescriptionOfGoods)) {
            screenWarnings.push("Description Of Goods field is required");
        }
        this.EntityPM.BookingPackages.forEach(function (item) {
            if (Tools_1.AppTool.IsNullOrZero(item.Quantity)) {
                if (Tools_1.AppTool.IsNullOrZero(item.Height) || Tools_1.AppTool.IsNullOrZero(item.Width) || Tools_1.AppTool.IsNullOrZero(item.Length)) {
                    screenWarnings.push("Dimensions field is required");
                }
                if ((item.Weight == null || item.Weight == 0)) {
                    screenWarnings.push("Weight field is required");
                }
            }
        });
        this.TabErrors_PAC = screenErrors;
        this.TabWarnings_PAC = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "PAC");
    };
    BookingWizardComponent.prototype.ValidateScreen_GEN = function () {
        var screenErrors = [];
        var screenWarnings = [];
        if (this.EntityPM.TenantZeroIsProductMandatory) {
            if (this.EntityPM.BookingProductId == null) {
                screenWarnings.push(this.ValidationText.replace("%FieldName", "Booking Product"));
            }
        }
        if (this.EntityPM.IsTemperatureSensitive) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId1) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId2) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId3)
                && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId4) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId5) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId6)
                && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId7) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId8) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBSpecialHandlingCodeId9)) {
                screenWarnings.push("You have to select at least one handling code");
            }
        }
        this.ValidateScreen_GEN_AirlineRules(screenErrors, screenWarnings);
        this.TabErrors_GEN = screenErrors;
        this.TabWarnings_GEN = screenWarnings;
        this.ApplyStyle(screenErrors.length > 0, screenWarnings.length > 0, "GEN");
    };
    BookingWizardComponent.prototype.ValidateScreen_GEN_AirlineRules = function (screenErrors, screenWarnings) {
        this.ValidateAirlineRule("AWBCarrierTarrifReference", this.EntityPM.AWBCarrierTarrifReference, screenWarnings);
        this.ValidateAirlineRule("OtherServicesInformation", this.EntityPM.OtherServicesInformation, screenWarnings);
        this.ValidateAirlineRule("SpecialServicesRequest", this.EntityPM.SpecialServicesRequest, screenWarnings);
        this.ValidateAirlineRule("BookingProductId", this.EntityPM.BookingProductId, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId1", this.EntityPM.AWBSpecialHandlingCodeId1, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId2", this.EntityPM.AWBSpecialHandlingCodeId2, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId3", this.EntityPM.AWBSpecialHandlingCodeId3, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId4", this.EntityPM.AWBSpecialHandlingCodeId4, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId5", this.EntityPM.AWBSpecialHandlingCodeId5, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId6", this.EntityPM.AWBSpecialHandlingCodeId6, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId7", this.EntityPM.AWBSpecialHandlingCodeId7, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId8", this.EntityPM.AWBSpecialHandlingCodeId8, screenWarnings);
        this.ValidateAirlineRule("AWBSpecialHandlingCodeId9", this.EntityPM.AWBSpecialHandlingCodeId9, screenWarnings);
    };
    BookingWizardComponent.prototype.ValidateAirlineRule = function (myFieldName, myFieldValue, validationList) {
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
    BookingWizardComponent.prototype.ApplyStyle = function (hasErrors, hasWarnings, screenCode) {
        if (hasErrors) {
            switch (screenCode) {
                case "BKD": {
                    this.Fill_BKD = "#E45A26";
                    break;
                }
                case "PAR": {
                    this.Fill_PAR = "#E45A26";
                    break;
                }
                case "PAC": {
                    this.Fill_PAC = "#E45A26";
                    break;
                }
                case "GEN": {
                    this.Fill_GEN = "#E45A26";
                    break;
                }
                default: {
                    break;
                }
            }
        }
        else if (hasWarnings) {
            switch (screenCode) {
                case "BKD": {
                    this.Fill_BKD = "#FFCB00";
                    break;
                }
                case "PAR": {
                    this.Fill_PAR = "#FFCB00";
                    break;
                }
                case "PAC": {
                    this.Fill_PAC = "#FFCB00";
                    break;
                }
                case "GEN": {
                    this.Fill_GEN = "#FFCB00";
                    break;
                }
                default: {
                    break;
                }
            }
        }
        else {
            switch (screenCode) {
                case "BKD": {
                    this.Fill_BKD = null;
                    break;
                }
                case "PAR": {
                    this.Fill_PAR = null;
                    break;
                }
                case "PAC": {
                    this.Fill_PAC = null;
                    break;
                }
                case "GEN": {
                    this.Fill_GEN = null;
                    break;
                }
                default: {
                    break;
                }
            }
        }
        this.ValidateFFR();
    };
    BookingWizardComponent.prototype.ValidateFFR = function () {
        this.ValidationWarningsList = [];
        if (this.isSendFFRButtonClicked || this.isSendCancellationButtonClicked) {
            if (this.ValidationErrorsList.length == 0) {
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_BKD);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAR);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_PAC);
                this.ValidationWarningsList = this.ValidationWarningsList.concat(this.TabWarnings_GEN);
            }
        }
        return this.ValidationWarningsList.length == 0 ? true : false;
    };
    BookingWizardComponent.prototype.ValidateBooking = function () {
        this.ValidationErrorsList = [];
        var myMasterFieldError = Tools_1.AppTool.ValidateMasterField(this.EntityPM.Master, this.EntityPM.TransportModeCode, this.EntityPM.CarrierIsCheckDigit, this.EntityPM.CarrierIsLimitedLength);
        if (!Tools_1.AppTool.IsNullOrEmpty(myMasterFieldError)) {
            this.ValidationErrorsList.push(myMasterFieldError);
        }
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_BKD);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAR);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_PAC);
        this.ValidationErrorsList = this.ValidationErrorsList.concat(this.TabErrors_GEN);
        return this.ValidationErrorsList.length == 0 ? true : false;
    };
    BookingWizardComponent.prototype.CloseButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            if (this.EntityPM.BookingStatusCode != "CRT") {
                confirmWindow.YesButtonText = "Update";
                confirmWindow.NoButtonText = "Don't Update";
            }
            else {
                confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
                confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
            }
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", this.ObjectTableName));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.isConfirmCloseClicked = true;
                    var isValid = _this.ValidateBooking();
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
    BookingWizardComponent.prototype.CloseWizardWindow = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    BookingWizardComponent.prototype.InitFlags = function () {
        this.isSendFFRButtonClicked = false;
        this.isSendCancellationButtonClicked = false;
        this.isSaveButtonClicked = false;
        this.isConfirmCloseClicked = false;
        this.isCopyBookingButtonClicked = false;
        this.isCancelBookingButtonClicked = false;
        this.isReactivateButtonClicked = false;
        this.isBuildButtonClicked = false;
        this.isFSRRequestButtonClicked = false;
        this.isManualStatusClicked = false;
        this.isSendToAirlineTenantButtonClicked = false;
    };
    BookingWizardComponent.prototype.SaveManualStatus = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isManualStatusClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.Save();
        }
        else {
            this.StopBusyIndicator();
        }
    };
    BookingWizardComponent.prototype.SaveClicked = function () {
        this.InitFlags();
        this.isSaveButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.Save();
        }
    };
    BookingWizardComponent.prototype.Save = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        if (this.EntityPM.IsDirty) {
            if (this.EntityPM.Id == null) {
                this.isReloadingOnSave = true;
                this.SubmitCreatingBooking();
            }
            else {
                this.SubmitUpdatingBooking();
            }
        }
        else {
            this.StopBusyIndicator();
            this.OnSaveCompletedSuccessfully();
            this.SaveCompleted.emit(true);
        }
    };
    BookingWizardComponent.prototype.SubmitCreatingBooking = function () {
        var _this = this;
        var myService = new BookingPMService_1.BookingPMService();
        myService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.OnSaveCompletedSuccessfully();
                _this.SaveCompleted.emit(true);
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.StopBusyIndicator();
                _this.SaveCompleted.emit(false);
            }
        }, function (error) {
            _this.StopBusyIndicator();
            _this.SaveCompleted.emit(false);
        });
    };
    BookingWizardComponent.prototype.SubmitUpdatingBooking = function () {
        var _this = this;
        var myService = new BookingPMService_1.BookingPMService();
        myService.update(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.OnSaveCompletedSuccessfully();
                _this.SaveCompleted.emit(true);
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.StopBusyIndicator();
                _this.SaveCompleted.emit(false);
            }
        }, function (error) {
            _this.StopBusyIndicator();
            _this.SaveCompleted.emit(false);
        });
    };
    BookingWizardComponent.prototype.OnSaveCompletedSuccessfully = function () {
        if (this.isReloadingOnSave) {
            this.isExecutingMethod = true;
            this.ReloadBooking();
        }
        else {
            this.isExecutingMethod = true;
            this.ExecuteRequestedMethod();
        }
    };
    BookingWizardComponent.prototype.ExecuteRequestedMethod = function () {
        if (this.isExecutingMethod) {
            if (this.isSaveButtonClicked) {
                this.StopBusyIndicator();
            }
            else if (this.isSendFFRButtonClicked) {
                this.Send(false);
            }
            else if (this.isSendCancellationButtonClicked) {
                this.Send(true);
            }
            else if (this.isConfirmCloseClicked) {
                this.CloseWizardWindow();
            }
            else if (this.isCopyBookingButtonClicked) {
                this.StopBusyIndicator();
                this.CopyBooking();
            }
            else if (this.isCancelBookingButtonClicked) {
                this.StopBusyIndicator();
                this.CancelBooking();
            }
            else if (this.isReactivateButtonClicked) {
                this.StopBusyIndicator();
                this.ReactivateBooking();
            }
            else if (this.isBuildButtonClicked) {
                this.StopBusyIndicator();
                this.BuildShipment();
            }
            else if (this.isFSRRequestButtonClicked) {
                this.FSRRequest();
            }
            else if (this.isManualStatusClicked) {
                this.StopBusyIndicator();
                this.ReloadBooking();
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
    BookingWizardComponent.prototype.OnSendFFRButtonClicked = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isSendFFRButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.ValidateAllTabs();
            var isAwbValid = this.ValidateFFR();
            if (isAwbValid) {
                this.SetSendingData(false);
                this.Save();
            }
            else {
                this.StopBusyIndicator();
            }
        }
        else {
            this.StopBusyIndicator();
        }
    };
    BookingWizardComponent.prototype.OnSendCancellationButtonClicked = function () {
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        this.InitFlags();
        this.isSendCancellationButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.ValidateAllTabs();
            var isAwbValid = this.ValidateFFR();
            if (isAwbValid) {
                this.SetSendingData(true);
                this.Save();
            }
            else {
                this.StopBusyIndicator();
            }
        }
        else {
            this.StopBusyIndicator();
        }
    };
    BookingWizardComponent.prototype.SetSendingData = function (cancellationSent) {
        var _this = this;
        if (cancellationSent) {
            //this.EntityPM.IsCancellationButtonClicked = true;
        }
        else {
            //this.EntityPM.IsFFRButtonClicked = true;
        }
        this.EntityPM.FNAReason = null;
        this.EntityPM.FMAAcknowledgementReason = null;
        this.EntityPM.AnswerOtherServicesInformation = null;
        if (this.EntityPM.BookingAnswers != null) {
            if (this.EntityPM.BookingAnswers.length > 0) {
                this.EntityPM.BookingAnswers.forEach(function (item) {
                    _this.EntityPM.RemoveBookingAnswer(item);
                });
            }
        }
    };
    BookingWizardComponent.prototype.Send = function (cancellationSent) {
        this.isCancellationSent = cancellationSent;
        var isValidForSending = true;
        var validationErrorMessage = "";
        var myAWBFFRValidator = AWBUtilities_1.AWBUtilities.ValidateAWBFFR(this.EntityPM);
        if (isValidForSending) {
            if (myAWBFFRValidator.AirlineFieldHasError) {
                isValidForSending = false;
                validationErrorMessage = myAWBFFRValidator.AirlineFieldErrorMessage;
            }
        }
        if (isValidForSending) {
            if (myAWBFFRValidator.TenantManagementFieldHasError) {
                isValidForSending = false;
                validationErrorMessage = myAWBFFRValidator.TenantManagementFieldErrorMessage;
            }
        }
        if (isValidForSending) {
            if (myAWBFFRValidator.AirlineRegistrationHasError) {
                isValidForSending = false;
                validationErrorMessage = myAWBFFRValidator.AirlineRegistrationErrorMessage;
            }
        }
        if (!cancellationSent) {
            if (isValidForSending) {
                if (myAWBFFRValidator.ETDFieldHasError) {
                    isValidForSending = false;
                    validationErrorMessage = myAWBFFRValidator.ETDFieldErrorMessage;
                }
            }
        }
        if (isValidForSending) {
            if (!myAWBFFRValidator.FFR) {
                isValidForSending = false;
                validationErrorMessage = "This airline will not receive FFR";
            }
        }
        if (!isValidForSending) {
            this.StopBusyIndicator();
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show(validationErrorMessage);
        }
        else {
            this.ValidateOnline();
        }
    };
    BookingWizardComponent.prototype.ValidateOnline = function () {
        var _this = this;
        var myBookingDomainService = new BookingDomainService_1.BookingDomainService();
        myBookingDomainService.ValidateBookingForSending(this.EntityPM.Id, this.isCancellationSent).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var myResultClass = myResponse.Result;
                if (myResultClass != null) {
                    _this.ValidationErrorsList = [];
                    if (myResultClass.IsValid) {
                        _this.StopBusyIndicator();
                        _this.StartSending(_this.isCancellationSent);
                    }
                    else if (myResultClass.HasStockErrors) {
                        _this.StopBusyIndicator();
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 150;
                        logWindow.Title = "AWB Stock error";
                        logWindow.Show('./Booking/Components/BookingWizard/NoRemainingStockComponent');
                    }
                    else {
                        myResultClass.ErrorsList.forEach(function (item) {
                            _this.ValidationErrorsList.push(item);
                        });
                        _this.StopBusyIndicator();
                    }
                }
            }
            else {
                _this.StopBusyIndicator();
            }
        });
    };
    BookingWizardComponent.prototype.StartSending = function (isCancellationSent) {
        var _this = this;
        this.StartBusyIndicator("Sending...");
        if (this.myFFRWebService == null) {
            this.myFFRWebService = new FFRWebService_1.FFRWebService();
        }
        this.myFFRWebService.Send(this.EntityPM.Id, this.EntityPM.Tenant, this.EntityPM.TenantZeroAirlineTTY, isCancellationSent).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.mySendingResultClass = myResponse.Result;
                if (_this.mySendingResultClass == null) {
                    _this.StopBusyIndicator();
                }
                else {
                    if (_this.mySendingResultClass.IsValid) {
                        _this.runOverview = true;
                        _this.ReloadBookingAfterSending();
                        _this.SetDemoMessage();
                    }
                    else if (_this.mySendingResultClass.HasStockError) {
                        _this.StopBusyIndicator();
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 450;
                        logWindow.Height = 150;
                        logWindow.Title = "AWB Stock error";
                        logWindow.Show('./Booking/Components/BookingWizard/NoRemainingStockComponent');
                    }
                    else {
                        _this.StopBusyIndicator();
                    }
                }
            }
        });
    };
    BookingWizardComponent.prototype.RunOverviewTab = function () {
        this.IsTabVisible_OVE = true;
        this.SelectedTabCode = "OVE";
    };
    BookingWizardComponent.prototype.SetDemoMessage = function () {
        if (InfraSettings_1.InfraSettings.TenantPM.Id == 65 || SessionLocator_1.SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Please note that this message will not be sent to the airline since it is a demo environment. You can still review the built message");
        }
    };
    BookingWizardComponent.prototype.BuildShipmentClicked = function (shipmentLevel) {
        var _this = this;
        this.shipmentLevelCode = shipmentLevel;
        this.InitFlags();
        if (this.EntityPM.BookingStatusCode == "WCF") {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show("Please notice that the booking is not confirmed yet, confirm manually ?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.EntityPM.BookingStatusCode = "CNF";
                    _this.EntityPM.FFRStatusCode = "CFM";
                    _this.isBuildButtonClicked = true;
                    var isValid = _this.ValidateBooking();
                    if (isValid) {
                        _this.Save();
                    }
                }
            });
        }
        else {
            this.isBuildButtonClicked = true;
            var isValid = this.ValidateBooking();
            if (isValid) {
                this.Save();
            }
        }
    };
    BookingWizardComponent.prototype.BuildShipment = function () {
        var _this = this;
        var confirmMsg = "";
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipperId) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ConsigneeId)) {
            confirmMsg = null;
            this.StartBuildingShipment();
        }
        else {
            var myService = new CardListService_1.CardListService();
            myService.getSingle(this.EntityPM.ShipperId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var shipper = myResponse.Result;
                    myService.getSingle(_this.EntityPM.ConsigneeId).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            var consignee = myResponse.Result;
                            _this.SetPartnerType(shipper, consignee, confirmMsg);
                        }
                    });
                }
            });
        }
    };
    BookingWizardComponent.prototype.SetPartnerType = function (shipper, consignee, confirmMsg) {
        var _this = this;
        if (shipper != null) {
            this.shipperPartnerType = shipper == null ? "" : shipper.PartnerTypeId;
        }
        if (consignee != null) {
            this.consigneePartnerType = consignee == null ? "" : consignee.PartnerTypeId;
        }
        if (this.shipmentLevelCode == "D") {
            if (this.shipperPartnerType == "AG" || this.consigneePartnerType == "AG") {
                confirmMsg = "Please confirm creating a direct AWB, note that you'll lose the shipper/consignee details if filled";
            }
        }
        else if (this.shipmentLevelCode == "C") {
            if (this.shipperPartnerType == "CS" || this.consigneePartnerType == "CS") {
                confirmMsg = "Please confirm creating a consol AWB, note that you'll lose the shipper/consignee details if filled";
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(confirmMsg)) {
            this.StartBuildingShipment();
        }
        else {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Show(confirmMsg);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.StartBuildingShipment();
                }
            });
        }
    };
    BookingWizardComponent.prototype.StartBuildingShipment = function () {
        var _this = this;
        var shipmentPM = Tools_2.BookingTool.BuildShipment(this.EntityPM);
        shipmentPM.ShipmentLevelCode = this.shipmentLevelCode;
        if (this.shipmentLevelCode == "D") {
            if (this.shipperPartnerType == "AG") {
                shipmentPM.ShipperId = null;
                shipmentPM.ShipperAddressId = null;
                shipmentPM.ShipperName = null;
                shipmentPM.ShipperReference1 = null;
            }
            if (this.consigneePartnerType == "AG") {
                shipmentPM.ConsigneeId = null;
                shipmentPM.ConsigneeAddressId = null;
                shipmentPM.ConsigneeName = null;
                shipmentPM.ConsigneeReference1 = null;
            }
        }
        else if (this.shipmentLevelCode == "C") {
            if (this.shipperPartnerType == "CS") {
                shipmentPM.ShipperId = null;
                shipmentPM.ShipperAddressId = null;
                shipmentPM.ShipperName = null;
                shipmentPM.ShipperReference1 = null;
            }
            if (this.consigneePartnerType == "CS") {
                shipmentPM.ConsigneeId = null;
                shipmentPM.ConsigneeAddressId = null;
                shipmentPM.ConsigneeName = null;
                shipmentPM.ConsigneeReference1 = null;
            }
        }
        var windowArgs = new Args_2.AWBWizardArgs();
        windowArgs.IsNewEntity = true;
        windowArgs.ShipmentLevelCode = this.shipmentLevelCode;
        windowArgs.EntityPM = shipmentPM;
        windowArgs.IsBuildFromBooking = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 600;
        logWindow.Title = "New AWB Shipment";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardComponent');
        logWindow.WindowClosed.subscribe(function (event) {
            _this.ReloadBooking();
        });
    };
    BookingWizardComponent.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    BookingWizardComponent.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    BookingWizardComponent.prototype.ReloadBookingAfterSending = function () {
        var _this = this;
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        var myService = new BookingPMService_1.BookingPMService();
        myService.get(this.EntityPM.Id).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.EntityPM = mm.Result;
                _this.LoadCompleted.emit(true);
                _this.SetButtonsProperties();
                _this.RefreshTabs();
                if (_this.runOverview) {
                    _this.RunOverviewTab();
                }
                _this.StopBusyIndicator();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.LoadCompleted.emit(false);
                _this.StopBusyIndicator();
            }
        }, function (error) {
            _this.ValidationErrorsList = [];
            _this.ValidationErrorsList.push('server error on reload');
            _this.LoadCompleted.emit(false);
            _this.StopBusyIndicator();
        });
    };
    BookingWizardComponent.prototype.ReloadBooking = function () {
        var _this = this;
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        var myService = new BookingPMService_1.BookingPMService();
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
            _this.StopBusyIndicator();
            _this.SetButtonsProperties();
            _this.ExecuteRequestedMethod();
        });
    };
    BookingWizardComponent.prototype.ReloadEntity = function () {
        var _this = this;
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        var myService = new BookingPMService_1.BookingPMService();
        myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.LoadCompleted.emit(true);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.LoadCompleted.emit(false);
                }
                _this.StopBusyIndicator();
            }
        });
    };
    BookingWizardComponent.prototype.RefreshTabs = function () {
        if (this.PageChild_OVE != null) {
            this.PageChild_OVE.RefreshTab();
        }
        if (this.PageChild_BKD != null) {
            this.PageChild_BKD.RefreshTab();
        }
        if (this.PageChild_GEN != null) {
            this.PageChild_GEN.RefreshTab();
        }
        if (this.PageChild_PAC != null) {
            this.PageChild_PAC.RefreshTab();
        }
        if (this.PageChild_PAR != null) {
            this.PageChild_PAR.RefreshTab();
        }
    };
    //More Button
    BookingWizardComponent.prototype.CopyBookingClicked = function () {
        this.InitFlags();
        this.isCopyBookingButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.Save();
        }
    };
    BookingWizardComponent.prototype.CancelBookingClicked = function () {
        var _this = this;
        var confirmMsg = "Are you sure you want to cancel this Booking?";
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var isValid = _this.ValidateBooking();
                if (isValid) {
                    _this.InitFlags();
                    _this.isCancelBookingButtonClicked = true;
                    _this.Save();
                }
            }
        });
    };
    BookingWizardComponent.prototype.ReactivateBookingClicked = function () {
        this.InitFlags();
        this.isReactivateButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.Save();
        }
    };
    BookingWizardComponent.prototype.SendToAirlineTenantClicked = function () {
        this.InitFlags();
        this.isSendToAirlineTenantButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.Save();
        }
    };
    BookingWizardComponent.prototype.CopyBooking = function () {
        var windowArgs = new Args_1.BookingWizardArgs();
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsCopyFromBooking = true;
        windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "Copy Booking";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Booking/Components/BookingWizard/BookingWizardComponent');
    };
    BookingWizardComponent.prototype.CancelBooking = function () {
        this.EntityPM.IsCancelled = true;
        this.EntityPM.MAWBStackAirlineId = this.EntityPM.MainCarriageCarrierId;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.InterlineId)) {
            this.EntityPM.MAWBStackAirlineId = this.EntityPM.InterlineId;
        }
        if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
            this.EntityPM.MAWBReturnedToStack = true;
            this.EntityPM.MAWBReturnedToStackWithCancel = true;
            this.EntityPM.MAWBStackNumber = this.EntityPM.Master;
            this.SaveClicked();
        }
        else {
            this.isCancelBookingButtonClicked = false;
            this.Save();
        }
        this.SetButtonsProperties();
        this.RefreshTabs();
        this.StopBusyIndicator();
    };
    BookingWizardComponent.prototype.ReactivateBooking = function () {
        this.InitFlags();
        this.EntityPM.IsCancelled = false;
        this.Save();
        this.SetButtonsProperties();
        this.StopBusyIndicator();
    };
    BookingWizardComponent.prototype.SendToAirlineTenant = function () {
        var _this = this;
        this.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Sending..."));
        var myService = new InfrastructureDomainService_1.InfrastructureDomainService();
        myService.SendEntityToAirlineTenant(this.EntityPM.Id, "Booking", this.EntityPM.MainCarriageCarrierCode).subscribe(function (myResponse) {
            _this.StopBusyIndicator();
            if (myResponse.HasError) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
            }
        });
    };
    BookingWizardComponent.prototype.CloseResponseProgressClicked = function () {
        this.IsResponseProgressVisible = false;
        this.TimerStoppedByUser = true;
        if (this.PageChild_OVE != null) {
            this.PageChild_OVE.StopTimer();
        }
    };
    BookingWizardComponent.prototype.FSRRequestMethod = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.TenantZeroAirlineTTY)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("This Airline doesn't support transmitting messages");
        }
        else {
            this.SendFSR();
        }
    };
    BookingWizardComponent.prototype.SendFSR = function () {
        this.InitFlags();
        this.isFSRRequestButtonClicked = true;
        var isValid = this.ValidateBooking();
        if (isValid) {
            this.ValidateAllTabs();
            var isAwbValid = this.ValidateFFR();
            if (isAwbValid) {
                this.Save();
            }
            else {
                this.StopBusyIndicator();
            }
        }
        else {
            this.StopBusyIndicator();
        }
    };
    BookingWizardComponent.prototype.FSRRequest = function () {
        this.StopBusyIndicator();
        var myValidator = AWBUtilities_1.AWBUtilities.ValidateAWBFSR(this.EntityPM);
        if (myValidator.FSR) {
            if (myValidator.IsValid) {
                this.StartSendingFSR();
            }
            else {
                this.StopBusyIndicator();
                var errorMessage = "";
                if (myValidator.TenantManagementFieldHasError) {
                    errorMessage = myValidator.TenantManagementFieldErrorMessage;
                }
                else if (myValidator.AirlineFieldHasError) {
                    errorMessage = myValidator.AirlineFieldErrorMessage;
                }
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(errorMessage);
            }
        }
        else {
            this.ValidationErrorsList.push("This airline does not support FSR/FSA messages");
        }
    };
    BookingWizardComponent.prototype.StartSendingFSR = function () {
        var _this = this;
        this.StartBusyIndicator("Sending in Progress..");
        var myService = new FSRWebService_1.FSRWebService();
        myService.SendBookingFSR(this.EntityPM.Id, "Booking", this.EntityPM.TenantZeroAirlineTTY).subscribe(function (myResponse) {
            _this.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                if (myResponse.Result instanceof FSRWebService_1.FSRResultClass) {
                    _this.ReloadBooking();
                    _this.PageChild_OVE.SetDataAfterSending();
                    _this.SetDemoMessage();
                }
                else {
                    var errors = [];
                    errors.push("Sending FSR failed");
                    _this.ValidationErrorsList = errors;
                }
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BookingWizardComponent.prototype, "LoadCompleted", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BookingWizardComponent.prototype, "SaveCompleted", void 0);
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], BookingWizardComponent.prototype, "AllLocations", void 0);
    BookingWizardComponent = __decorate([
        core_1.Component({
            selector: 'BookingWizardComponent',
            moduleId: module.id,
            templateUrl: './BookingWizardComponent.html',
            providers: [EntityArgs_1.EntityArgs]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], BookingWizardComponent);
    return BookingWizardComponent;
}());
exports.BookingWizardComponent = BookingWizardComponent;
//# sourceMappingURL=BookingWizardComponent.js.map