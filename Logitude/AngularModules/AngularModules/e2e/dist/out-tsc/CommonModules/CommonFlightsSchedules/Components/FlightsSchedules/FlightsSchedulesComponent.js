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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var FlightsSchedulesRequestPM_1 = require("../../../../Booking/EntityPMs/FlightsSchedulesRequestPM");
var BookingPM_1 = require("../../../../Booking/EntityPMs/BookingPM");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var FVRWebService_1 = require("../../../../Infrastructure/Services/WebServices/FVRWebService");
var FlightsSchedulesRequestPMService_1 = require("../../../../Booking/Services/StandardPMs/FlightsSchedulesRequestPMService");
var Args_1 = require("../../Args");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_2 = require("../../../../Booking/Args");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var FlightsSchedulesComponent = /** @class */ (function (_super) {
    __extends(FlightsSchedulesComponent, _super);
    function FlightsSchedulesComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "FlightsSchedulesRequest";
        _this.DataContext = _this;
        _this.RequestSent = false;
        _this.FlightSelected = false;
        _this.ValidationErrorsList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsResourcesReady = false;
        _this.IsSimulateVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Load Airline
        _this.myTenantZeroAirlineTTY = null;
        _this.IsAirlinepNeedsRegistration = false;
        _this.IsAirlineRegistered = false;
        _this.ValidationText = null;
        _this.timerSeconds = 1;
        _this.Retries = 0;
        _this.IsLoading = false;
        _this.IsSimulatorEnabled = false;
        _this.IsResponseProgressVisible = false;
        _this.IsNoFlightsResult = false;
        _this.ItemsSource = [];
        _this.EntityPM = new FlightsSchedulesRequestPM_1.FlightsSchedulesRequestPM();
        _this.EntityPM.StatusCode = "W";
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.myAirlineService = new AirlineListService_1.AirlineListService();
        _this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        _this.myFVRWebService = new FVRWebService_1.FVRWebService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(_this.ObjectTableName, "FlightsSchedules.Simulator")) {
            _this.IsSimulateVisible = true;
        }
        return _this;
    }
    FlightsSchedulesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.args = args;
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(function (response) {
            _this.IsResourcesReady = true;
            if (args.BookingPM != null) {
                _this.myBookingPM = args.BookingPM;
                _this.InitFromBooking();
            }
            else if (args.ShipmentPM != null) {
                _this.myShipmentPM = args.ShipmentPM;
                _this.InitFromShipment();
            }
            _this.InitializeComponent();
        });
    };
    FlightsSchedulesComponent.prototype.InitFromBooking = function () {
        this.EntityPM.BookingId = this.myBookingPM.Id;
        this.EntityPM.AirlineId = this.myBookingPM.MainCarriageCarrierId;
        this.EntityPM.FromPortId = this.myBookingPM.MainCarriageFromPortId;
        this.EntityPM.ToPortId = this.myBookingPM.MainCarriageFinalDestinationPortId;
        this.EntityPM.ETD = this.myBookingPM.MainCarriageETD;
        this.EntityPM.ETA = null;
        this.EntityPM.Volume = this.myBookingPM.Volume;
        this.EntityPM.GrossWeight = this.myBookingPM.GrossWeight;
        this.EntityPM.VolumeUnitCode = this.myBookingPM.VolumeUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.myBookingPM.GrossWeightUnitCode;
        if (this.EntityPM.ToPortId == null) {
            this.EntityPM.ToPortId = this.myBookingPM.MainCarriageToPortId;
        }
        if (!this.args.IsMainLeg) {
            this.EntityPM.AirlineId = this.myBookingPM.Transshipment1CarrierId;
            this.EntityPM.FromPortId = this.myBookingPM.Transshipment1FromPortId;
            this.EntityPM.ToPortId = this.myBookingPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.ETD = this.myBookingPM.Transshipment1ETD;
        }
    };
    FlightsSchedulesComponent.prototype.InitFromShipment = function () {
        this.EntityPM.ShipmentId = this.myShipmentPM.Id;
        this.EntityPM.AirlineId = this.myShipmentPM.MainCarriageCarrierId;
        this.EntityPM.FromPortId = this.myShipmentPM.MainCarriageFromPortId;
        this.EntityPM.ToPortId = this.myShipmentPM.MainCarriageFinalDestinationPortId;
        this.EntityPM.ETD = this.myShipmentPM.MainCarriageETD;
        this.EntityPM.ETA = null;
        this.EntityPM.Volume = this.myShipmentPM.Volume;
        this.EntityPM.GrossWeight = this.myShipmentPM.GrossWeight;
        this.EntityPM.VolumeUnitCode = this.myShipmentPM.VolumeUnitCode;
        this.EntityPM.GrossWeightUnitCode = this.myShipmentPM.GrossWeightUnitCode;
        if (this.EntityPM.FromPortId == null) {
            this.EntityPM.FromPortId = this.myShipmentPM.FromPortId;
        }
        if (this.EntityPM.ToPortId == null) {
            this.EntityPM.ToPortId = this.myShipmentPM.ToPortId;
        }
        if (!this.args.IsMainLeg) {
            this.EntityPM.AirlineId = this.myShipmentPM.Transshipment1CarrierId;
            this.EntityPM.FromPortId = this.myShipmentPM.Transshipment1FromPortId;
            this.EntityPM.ToPortId = this.myShipmentPM.MainCarriageFinalDestinationPortId;
            this.EntityPM.ETD = this.myShipmentPM.Transshipment1ETD;
        }
    };
    FlightsSchedulesComponent.prototype.InitializeComponent = function () {
        //if (this.EntityPM.ETD != null) {
        //    this.EntityPM.ETD = DateTool.TruncateTime(this.EntityPM.ETD);
        //}
        if (Tools_1.AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
            this.GrossWeightUnitCode = InfraSettings_1.InfraSettings.TenantPM.GrossWeightUnitCode;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
            this.VolumeUnitCode = InfraSettings_1.InfraSettings.TenantPM.VolumeUnitCode;
        }
        this.SetUIProperties();
        this.SetUIProperties_Airline();
        if (Tools_1.AppTool.IsNullOrEmpty(this.AirlineId)) {
            this.LoadAllowedAirline();
        }
        else {
            this.LoadTenantZeroAirline();
        }
    };
    FlightsSchedulesComponent.prototype.OnFlightSelected = function () {
        this.args.IsFlightSelected = true;
        this.FlightSelected = true;
        this.Close();
    };
    FlightsSchedulesComponent.prototype.UpdateMissingPorts = function (myResult) {
        var _this = this;
        myResult.forEach(function (item0) {
            _this.ItemsSource.forEach(function (item1) {
                item1.ItemsSource.forEach(function (item2) {
                    item2.UpdateMissingPorts(item0);
                });
            });
        });
    };
    // SetUIProperties
    FlightsSchedulesComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("ETD", this.ObjectTableName, (this.ETD == null ? true : false));
        var isWeightRequired = false;
        if (this.GrossWeight > 0) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                isWeightRequired = true;
            }
        }
        var isVolumeRequired = false;
        if (this.Volume > 0) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
                isVolumeRequired = true;
            }
        }
        this.UIProperties.SetRequired("Volume", this.ObjectTableName, isVolumeRequired);
        this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, isWeightRequired);
    };
    FlightsSchedulesComponent.prototype.SetUIProperties_Airline = function () {
        var isFieldEnabled = true;
        if (this.Airline != null) {
            if (this.Airline.NoAvailabilityInFVAMessages) {
                isFieldEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, isFieldEnabled);
    };
    FlightsSchedulesComponent.prototype.LoadAllowedAirline = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsRestrictedByAirline) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.AirlineId)) {
                this.myPartnersDomainService.GetAllowedAirlineId().subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            var allowedAirlineId = myResponse.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(allowedAirlineId)) {
                                _this.AirlineId = allowedAirlineId;
                            }
                        }
                    }
                });
            }
        }
    };
    FlightsSchedulesComponent.prototype.LoadTenantZeroAirline = function () {
        var _this = this;
        this.myTenantZeroAirlineTTY = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AirlineId)) {
            this.myAirlineService.getSingle(this.AirlineId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.Airline = myResponse.Result;
                    _this.SetResponseHelpMessage();
                    _this.SetUIProperties_Airline();
                    if (_this.Airline != null) {
                        _this.IsAirlineRegistered = _this.Airline.IsChampRegistered;
                        _this.myPartnersDomainService.GetAirlineByCode(_this.Airline.Code, 0).subscribe(function (myResponse2) {
                            if (!myResponse2.HasError) {
                                var airlinePM = myResponse2.Result;
                                if (airlinePM != null) {
                                    _this.myTenantZeroAirlineTTY = airlinePM.TTY;
                                    _this.IsAirlinepNeedsRegistration = airlinePM.ChampNeedsRegistration;
                                }
                            }
                        });
                    }
                }
            });
        }
    };
    Object.defineProperty(FlightsSchedulesComponent.prototype, "AirlineId", {
        get: function () { return this.EntityPM.AirlineId; },
        set: function (newValue) {
            if (this.EntityPM.AirlineId != newValue) {
                this.EntityPM.AirlineId = newValue;
                this.SetUIProperties_Airline();
                this.LoadTenantZeroAirline();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (newValue) {
            if (this.EntityPM.FromPortId != newValue) {
                this.EntityPM.FromPortId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (newValue) {
            if (this.EntityPM.ToPortId != newValue) {
                this.EntityPM.ToPortId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "ETD", {
        get: function () { return this.EntityPM.ETD; },
        set: function (newValue) {
            if (this.EntityPM.ETD != newValue) {
                this.EntityPM.ETD = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "ETA", {
        get: function () { return this.EntityPM.ETA; },
        set: function (newValue) {
            if (this.EntityPM.ETA != newValue) {
                this.EntityPM.ETA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "VolumeUnitCode", {
        get: function () { return this.EntityPM.VolumeUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.VolumeUnitCode != newValue) {
                this.EntityPM.VolumeUnitCode = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightsSchedulesComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeightUnitCode != newValue) {
                this.EntityPM.GrossWeightUnitCode = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    FlightsSchedulesComponent.prototype.SetResponseHelpMessage = function () {
        var myResult = "Flights displayed are in the specific date of departure only";
        if (this.Airline != null) {
            if (this.Airline.ScheduleDays > 0) {
                var myParam = this.Airline.ScheduleDays + " days";
                if (this.Airline.ScheduleDays == 1) {
                    myParam = this.Airline.ScheduleDays + " day";
                }
                myResult = "Flights displayed are " + myParam + " from the departure date";
            }
        }
        this.ResponseHelpMessage = myResult;
    };
    // Commands
    FlightsSchedulesComponent.prototype.FindFlightsClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Sending Request...");
        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            if (Tools_1.AppTool.IsNullOrEmpty(this.myTenantZeroAirlineTTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "This Airline doesn't support transmitting messages";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Tenant communication parameter (TTY) is missing";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }
            else if (this.IsAirlinepNeedsRegistration && !this.IsAirlineRegistered) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }
            else {
                this.myPartnersDomainService.GetAirlineRules(this.Airline.Code, "FVR").subscribe(function (myResponse) {
                    var isValid = true;
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            isValid = false;
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            var items = myResponse.Result;
                            if (items != null) {
                                if (items.length > 0) {
                                    var errors = _this.ValidateRules(items);
                                    if (errors.length > 0) {
                                        isValid = false;
                                        _this.ValidationErrorsList = errors;
                                    }
                                }
                            }
                        }
                    }
                    if (isValid) {
                        _this.SendRequest();
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    };
    FlightsSchedulesComponent.prototype.BuildXMLClicked = function () {
        var myBookingId = null;
        var myShipmentId = null;
        if (this.myBookingPM != null) {
            myBookingId = this.myBookingPM.Id;
        }
        if (this.myShipmentPM != null) {
            myShipmentId = this.myShipmentPM.Id;
        }
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = { ShipmentId: myShipmentId, BookingId: myBookingId, FatherComponent: this };
        logWindow.Title = "Build FVA Response";
        logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/XMLFlightsSimulatorComponent');
    };
    FlightsSchedulesComponent.prototype.UpgradeFromXML = function (myResult) {
        this.ETD = myResult.ETD;
        this.FromPortId = myResult.FromPortId;
        this.ToPortId = myResult.ToPortId;
        this.AirlineId = myResult.AirlineId;
        this.myRequestId = myResult.RequestId;
        this.StartTimer();
        this.RequestSent = true;
    };
    FlightsSchedulesComponent.prototype.SendFVAClicked = function () {
        var isValid = this.Validate();
        if (isValid) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.myTenantZeroAirlineTTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "This Airline doesn't support transmitting messages";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantManagementJS.TTY)) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Tenant communication parameter (TTY) is missing";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }
            else if (this.IsAirlinepNeedsRegistration && !this.IsAirlineRegistered) {
                this.CurrentSession.StopBusyIndicator();
                var validationErrorMessage = "Can’t send this message, the airline needs Champ registration. Please contact your account manager";
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show(validationErrorMessage);
            }
            else {
                var args = new Args_1.FVASimulatorWindowArgs();
                args.EntityPM = this.EntityPM;
                args.AirlineList = this.Airline;
                args.Recipient = this.myTenantZeroAirlineTTY;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 920;
                logWindow.Height = 530;
                logWindow.WindowArgs = args;
                logWindow.Title = "Flights Schedules";
                logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FVASimulatorComponent');
            }
        }
    };
    FlightsSchedulesComponent.prototype.CloseClicked = function () {
        this.Close();
    };
    FlightsSchedulesComponent.prototype.Validate = function () {
        var isValid = true;
        var errors = [];
        if (this.ValidationText == null) {
            this.ValidationText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        }
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.ETD == null) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.ETD");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }
        if (this.Volume != null && Tools_1.AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.VolumeUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }
        if (this.GrossWeight != null && Tools_1.AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.GrossWeightUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }
        this.ValidationErrorsList = errors;
        isValid = errors.length == 0 ? true : false;
        return isValid;
    };
    FlightsSchedulesComponent.prototype.ValidateRules = function (items) {
        var _this = this;
        var errors = [];
        var allKeys = Object.keys(this.EntityPM);
        var list = [];
        allKeys.forEach(function (item) {
            list.push(item.toLowerCase());
        });
        items.forEach(function (item) {
            if (list.indexOf(item.RuleFieldName.toLowerCase()) > -1) {
                var value = _this.EntityPM[item.RuleFieldName];
                _this.ValidateAirlineRule(item, value, errors);
            }
        });
        return errors;
    };
    FlightsSchedulesComponent.prototype.ValidateAirlineRule = function (myRule, myFieldValue, validationList) {
        if (myRule != null) {
            var myFieldLabel = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F." + myRule.RuleFieldName);
            if (myFieldValue == null || isNaN(myFieldValue)) {
                if (myRule.IsMandatoryForSending) {
                    validationList.push("Rule: " + this.ValidationText.replace("%FieldName", myFieldLabel));
                }
            }
            else if (typeof (myFieldValue) == "string") {
                if (Tools_1.AppTool.IsNullOrEmpty(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push("Rule: " + this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }
                else if (myRule.MaxSize > 0) {
                    if (myFieldValue.length > myRule.MaxSize) {
                        validationList.push("Rule: " + myFieldLabel + " exceeds max size (" + myRule.MaxSize + ")");
                    }
                }
            }
            else if (typeof (myFieldValue) == "number") {
                if (Tools_1.AppTool.IsNullOrZero(myFieldValue)) {
                    if (myRule.IsMandatoryForSending) {
                        validationList.push("Rule: " + this.ValidationText.replace("%FieldName", myFieldLabel));
                    }
                }
            }
        }
    };
    // Send
    FlightsSchedulesComponent.prototype.SendRequest = function () {
        var _this = this;
        this.myFVRWebService.SendFVR(this.AirlineId, this.FromPortId, this.ToPortId, this.ETD, this.ETA, this.Volume, this.GrossWeight, this.VolumeUnitCode, this.GrossWeightUnitCode, this.EntityPM.ShipmentId, this.EntityPM.BookingId, this.myTenantZeroAirlineTTY).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    var myResult = myResponse.Result;
                    if (myResult.IsValid) {
                        _this.myRequestId = myResult.RequestId;
                        _this.StartTimer();
                        _this.RequestSent = true;
                    }
                    else {
                        _this.ValidationErrorsList = myResult.Errors;
                    }
                }
            }
        });
    };
    FlightsSchedulesComponent.prototype.StopTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        this.IsSimulatorEnabled = false;
        this.IsResponseProgressVisible = false;
    };
    FlightsSchedulesComponent.prototype.StartTimer = function () {
        var _this = this;
        this.Retries = 0;
        this.timerToken = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
        this.IsSimulatorEnabled = true;
        this.IsResponseProgressVisible = true;
    };
    FlightsSchedulesComponent.prototype.IncreaseTimer = function () {
        var _this = this;
        clearTimeout(this.timerToken);
        this.timerToken = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
    };
    FlightsSchedulesComponent.prototype.AdjustTimerSpeed = function () {
        if (this.Retries <= 60) {
            if (this.timerSeconds != 1) {
                this.timerSeconds = 1;
                this.IncreaseTimer();
            }
        }
        else if (this.Retries <= 120) {
            if (this.timerSeconds != 5) {
                this.timerSeconds = 5;
                this.IncreaseTimer();
            }
        }
        else if (this.Retries <= 180) {
            if (this.timerSeconds != 60) {
                this.timerSeconds = 60;
                this.IncreaseTimer();
            }
        }
        else {
            this.StopTimer();
        }
    };
    FlightsSchedulesComponent.prototype.RunTimerFunction = function () {
        if (!this.IsLoading) {
            this.Retries++;
            this.LoadData();
            this.AdjustTimerSpeed();
        }
    };
    FlightsSchedulesComponent.prototype.LoadData = function () {
        var _this = this;
        this.IsLoading = true;
        this.IsNoFlightsResult = false;
        if (this.myFlightsSchedulesRequestPMService == null) {
            this.myFlightsSchedulesRequestPMService = new FlightsSchedulesRequestPMService_1.FlightsSchedulesRequestPMService();
        }
        this.myFlightsSchedulesRequestPMService.get(this.myRequestId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var entity = myResponse.Result;
                    if (entity != null) {
                        if (entity.ResponseDate != null) {
                            _this.StopTimer();
                            _this.BuildObslist(entity);
                        }
                    }
                }
            }
            _this.IsLoading = false;
        });
    };
    FlightsSchedulesComponent.prototype.BuildObslist = function (myResult) {
        var _this = this;
        this.ItemsSource = [];
        var Responses = myResult.Responses.filter(function (f) { return f.FromPortId == _this.FromPortId; }).sort(function (a, b) { return a.ResultNumber - b.ResultNumber; });
        Responses.forEach(function (item) {
            var itemsList = [];
            if (item.ToPortId == _this.ToPortId) {
                itemsList.push(item);
            }
            else {
                var insideResponses = myResult.Responses.filter(function (f) { return f.ResultNumber == item.ResultNumber; }).sort(function (a, b) { return a.LineNumber - b.LineNumber; });
                insideResponses.forEach(function (insideItem) {
                    itemsList.push(insideItem);
                });
            }
            var args = new ResponseItemArgs();
            args.Code = "Leg";
            args.Shipment = _this.myShipmentPM;
            args.Booking = _this.myBookingPM;
            args.Items = itemsList;
            _this.ItemsSource.push(new FlightsSchedulesResponeViewModel(args, _this));
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(myResult.AnswerOSI)) {
            var args = new ResponseItemArgs();
            args.Code = "OSI";
            args.DisplayText = myResult.AnswerOSI;
            this.ItemsSource.push(new FlightsSchedulesResponeViewModel(args, this));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myResult.AnswerReasonForNoReply)) {
            var args = new ResponseItemArgs();
            args.Code = "NRP";
            args.DisplayText = myResult.AnswerReasonForNoReply;
            this.ItemsSource.push(new FlightsSchedulesResponeViewModel(args, this));
            if (myResult.AnswerReasonForNoReply.toUpperCase() == "NO FLIGHTS" || myResult.AnswerReasonForNoReply.toUpperCase() == "FLIGHT NOT FOUND") {
                this.IsNoFlightsResult = true;
            }
        }
    };
    FlightsSchedulesComponent.prototype.CancelResponseProgressClicked = function () {
        this.args.IsCancelledFomProgress = true;
        this.StopTimer();
    };
    FlightsSchedulesComponent.prototype.CloseResponseProgressClicked = function () {
        this.args.IsClosedFomProgress = true;
        this.Close();
    };
    FlightsSchedulesComponent.prototype.Close = function () {
        this.StopTimer();
        this.CurrentSession.CloseCurrentWindow();
    };
    FlightsSchedulesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FlightsSchedulesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FlightsSchedulesComponent);
    return FlightsSchedulesComponent;
}(BaseComponent_1.BaseComponent));
exports.FlightsSchedulesComponent = FlightsSchedulesComponent;
var FlightsSchedulesResponeViewModel = /** @class */ (function () {
    function FlightsSchedulesResponeViewModel(args, fatherComponent) {
        var _this = this;
        this.fatherComponent = fatherComponent;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ItemsSource = [];
        this.myBookingPM = args.Booking;
        this.myShipmentPM = args.Shipment;
        this.legTypeCode = args.Code;
        this.LegDescription = args.DisplayText;
        if (args.Booking == null && args.Shipment == null) {
            this.IsFromOutSide = true;
        }
        if (args.Code == "Leg") {
            this.IsFlightsLeg = true;
            args.Items.sort(function (a, b) { return a.LineNumber - b.LineNumber; }).forEach(function (item) {
                _this.ItemsSource.push(new FlightItemViewModel(item));
            });
        }
        else {
            if (this.legTypeCode == "OSI") {
                this.LegHeader = "Other Service Information: ";
            }
            else if (this.legTypeCode == "NRP") {
                this.LegHeader = "Reason For No Reply: ";
            }
        }
    }
    FlightsSchedulesResponeViewModel.prototype.BookButtonClicked = function () {
        this.selectedCommandCode = "B";
        if (this.ItemsSource.filter(function (f) { return f.MissingPort == true; }).length > 0) {
            this.CopyMissingPorts();
        }
        else {
            this.Book();
        }
    };
    FlightsSchedulesResponeViewModel.prototype.SelectButtonClicked = function () {
        this.selectedCommandCode = "S";
        if (this.ItemsSource.filter(function (f) { return f.MissingPort == true; }).length > 0) {
            this.CopyMissingPorts();
        }
        else {
            this.Select();
        }
    };
    FlightsSchedulesResponeViewModel.prototype.CopyMissingPorts = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Copy missing ports..");
        var myResponsesIds = null;
        this.ItemsSource.forEach(function (item) {
            if (myResponsesIds == null) {
                myResponsesIds = item.Id;
            }
            else {
                myResponsesIds += ":" + item.Id;
            }
        });
        if (myResponsesIds.length == 0) {
            this.CurrentSession.StopBusyIndicator();
            if (this.selectedCommandCode == "B") {
                this.Book;
            }
            else {
                this.Select();
            }
        }
        else {
            this.fatherComponent.myFVRWebService.GetCopyFlightsSchedulesPorts(myResponsesIds).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse != null) {
                    if (myResponse.HasError) {
                        _this.fatherComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        var myResult = myResponse.Result;
                        _this.fatherComponent.UpdateMissingPorts(myResult);
                        if (_this.selectedCommandCode == "B") {
                            _this.Book();
                        }
                        else {
                            _this.Select();
                        }
                    }
                }
            });
        }
    };
    FlightsSchedulesResponeViewModel.prototype.Book = function () {
        // NewEntityPMService(tablename)
        var newBookingPM = new BookingPM_1.BookingPM();
        newBookingPM.IsCopyMode = true;
        newBookingPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        newBookingPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newBookingPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        newBookingPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newBookingPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newBookingPM.DirectionCode = "E";
        newBookingPM.TransportModeCode = "A";
        newBookingPM.SpaceAllocationCode = "NN";
        newBookingPM.MainCarriageSpaceAllocationCode = "NN";
        newBookingPM.BookingStatusCode = "CRT";
        newBookingPM.BookingStatusName = "Created";
        newBookingPM.FFRStatusCode = "NST";
        newBookingPM.FFRStatusName = "Not Sent";
        newBookingPM.DimensionsUnitCode = InfraSettings_1.InfraSettings.TenantPM.DimensionsUnitCode;
        newBookingPM.VolumeUnitCode = InfraSettings_1.InfraSettings.TenantPM.VolumeUnitCode;
        newBookingPM.GrossWeightUnitCode = InfraSettings_1.InfraSettings.TenantPM.GrossWeightUnitCode;
        newBookingPM.ChargeableWeightUnitCode = InfraSettings_1.InfraSettings.TenantPM.ChargeableWeightUnitCode;
        newBookingPM.CASSCode = InfraSettings_1.InfraSettings.TenantPM.CASSCode;
        newBookingPM.IssuingCarrierIATACode = InfraSettings_1.InfraSettings.TenantPM.IATA;
        newBookingPM.IssuingCarrierAgentId = InfraSettings_1.InfraSettings.TenantPM.AgentId;
        newBookingPM.IssuingCarrierAddressId = InfraSettings_1.InfraSettings.TenantPM.AddressId;
        newBookingPM.Ratio = Tools_1.AppTool.GetRatio(newBookingPM.DirectionCode, newBookingPM.TransportModeCode, null, InfraSettings_1.InfraSettings.TenantPM.CountryCode);
        newBookingPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(newBookingPM.Ratio, newBookingPM.DimensionsUnitCode, newBookingPM.ChargeableWeightUnitCode);
        var myFinalDestinationPortId = null;
        for (var i = 0; i < this.ItemsSource.length; i++) {
            var item = this.ItemsSource[i];
            if (item != null) {
                var itemETD = Tools_1.DateTool.GetDateParts(item.ETD).DateObject;
                switch (i) {
                    case 0:
                        {
                            newBookingPM.MainCarriageETD = itemETD;
                            newBookingPM.MainCarriageCarrierId = item.AirlineId;
                            newBookingPM.MainCarriageCarrierCode = item.AirlineCode;
                            newBookingPM.MainCarriageCarrierName = item.AirlineName;
                            newBookingPM.MainCarriageCarrierNumber = item.FlightNumber;
                            newBookingPM.MainCarriageCarrierPrefix = item.AirlineCode;
                            newBookingPM.MainCarriageFromPortId = item.FromPortId;
                            newBookingPM.MainFromPortCode = item.FromPortCode;
                            newBookingPM.MainFromPortName = item.FromPortName;
                            newBookingPM.MainFromPortCountryCode = item.FromPortCountryCode;
                            newBookingPM.MainFromPortCountryName = item.FromPortCountryName;
                            newBookingPM.MainCarriageToPortId = item.ToPortId;
                            newBookingPM.MainToPortCode = item.ToPortCode;
                            newBookingPM.MainToPortName = item.ToPortName;
                            newBookingPM.MainToPortCountryCode = item.ToPortCountryCode;
                            newBookingPM.MainToPortCountryName = item.ToPortCountryName;
                            break;
                        }
                    case 1:
                        {
                            newBookingPM.Transshipment1ETD = itemETD;
                            newBookingPM.Transshipment1CarrierId = item.AirlineId;
                            newBookingPM.Transshipment1CarrierName = item.AirlineName;
                            newBookingPM.Transshipment1CarrierNumber = item.FlightNumber;
                            newBookingPM.Transshipment1CarrierPrefix = item.AirlineCode;
                            newBookingPM.Transshipment1FromPortId = item.FromPortId;
                            newBookingPM.Trans1FromPortCode = item.FromPortCode;
                            newBookingPM.Trans1FromPortName = item.FromPortName;
                            newBookingPM.Trans1FromPortCountryCode = item.FromPortCountryCode;
                            newBookingPM.Trans1FromPortCountryName = item.FromPortCountryName;
                            newBookingPM.Transshipment1ToPortId = item.ToPortId;
                            newBookingPM.Trans1ToPortCode = item.ToPortCode;
                            newBookingPM.Trans1ToPortName = item.ToPortName;
                            newBookingPM.Trans1ToPortCountryCode = item.ToPortCountryCode;
                            newBookingPM.Trans1ToPortCountryName = item.ToPortCountryName;
                            break;
                        }
                    case 2:
                        {
                            newBookingPM.Transshipment2ETD = itemETD;
                            newBookingPM.Transshipment2CarrierId = item.AirlineId;
                            newBookingPM.Transshipment2CarrierName = item.AirlineName;
                            newBookingPM.Transshipment2CarrierNumber = item.FlightNumber;
                            newBookingPM.Transshipment2CarrierPrefix = item.AirlineCode;
                            newBookingPM.Transshipment2FromPortId = item.FromPortId;
                            newBookingPM.Trans2FromPortCode = item.FromPortCode;
                            newBookingPM.Trans2FromPortName = item.FromPortName;
                            newBookingPM.Trans2FromPortCountryCode = item.FromPortCountryCode;
                            newBookingPM.Trans2FromPortCountryName = item.FromPortCountryName;
                            newBookingPM.Transshipment2ToPortId = item.ToPortId;
                            newBookingPM.Trans2ToPortCode = item.ToPortCode;
                            newBookingPM.Trans2ToPortName = item.ToPortName;
                            newBookingPM.Trans2ToPortCountryCode = item.ToPortCountryCode;
                            newBookingPM.Trans2ToPortCountryName = item.ToPortCountryName;
                            break;
                        }
                }
                myFinalDestinationPortId = item.ToPortId;
            }
        }
        newBookingPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
        var windowTitle = "New Booking Wizard";
        var windowArgs = new Args_2.BookingWizardArgs();
        windowArgs.IsNewEntity = true;
        windowArgs.IsBuiltFromSchedule = true;
        windowArgs.EntityPM = newBookingPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./Booking/Components/BookingWizard/BookingWizardComponent');
    };
    FlightsSchedulesResponeViewModel.prototype.Select = function () {
        var isMainLeg = this.fatherComponent.args.IsMainLeg;
        var myFinalDestinationPortId = null;
        if (this.myShipmentPM != null) {
            if (isMainLeg) {
                this.myShipmentPM.MainCarriageETD = null;
                this.myShipmentPM.MainCarriageCarrierNumber = null;
                this.myShipmentPM.MainCarriageFromPortId = null;
                this.myShipmentPM.MainCarriageFromPortCode = null;
                this.myShipmentPM.MainCarriageFromPortName = null;
                this.myShipmentPM.MainCarriageFromPortCountryCode = null;
                this.myShipmentPM.MainCarriageFromPortCountryName = null;
            }
            this.myShipmentPM.MainCarriageToPortId = null;
            this.myShipmentPM.MainCarriageToPortCode = null;
            this.myShipmentPM.MainCarriageToPortName = null;
            this.myShipmentPM.MainCarriageToPortCountryCode = null;
            this.myShipmentPM.MainCarriageToPortCountryName = null;
            //**********
            this.myShipmentPM.Transshipment1ETD = null;
            this.myShipmentPM.Transshipment1CarrierId = null;
            this.myShipmentPM.Transshipment1CarrierCode = null;
            this.myShipmentPM.Transshipment1CarrierName = null;
            this.myShipmentPM.Transshipment1CarrierNumber = null;
            this.myShipmentPM.Transshipment1CarrierPrefix = null;
            this.myShipmentPM.Transshipment1FromPortId = null;
            this.myShipmentPM.Transshipment1FromPortCode = null;
            this.myShipmentPM.Transshipment1FromPortName = null;
            this.myShipmentPM.Transshipment1FromPortCountryCode = null;
            this.myShipmentPM.Transshipment1FromPortCountryName = null;
            this.myShipmentPM.Transshipment1ToPortId = null;
            this.myShipmentPM.Transshipment1ToPortCode = null;
            this.myShipmentPM.Transshipment1ToPortName = null;
            this.myShipmentPM.Transshipment1ToPortCountryCode = null;
            this.myShipmentPM.Transshipment1ToPortCountryName = null;
            //**********
            this.myShipmentPM.Transshipment2ETD = null;
            this.myShipmentPM.Transshipment2CarrierId = null;
            this.myShipmentPM.Transshipment2CarrierCode = null;
            this.myShipmentPM.Transshipment2CarrierName = null;
            this.myShipmentPM.Transshipment2CarrierNumber = null;
            this.myShipmentPM.Transshipment2CarrierPrefix = null;
            this.myShipmentPM.Transshipment2FromPortId = null;
            this.myShipmentPM.Transshipment2FromPortCode = null;
            this.myShipmentPM.Transshipment2FromPortName = null;
            this.myShipmentPM.Transshipment2FromPortCountryCode = null;
            this.myShipmentPM.Transshipment2FromPortCountryName = null;
            this.myShipmentPM.Transshipment2ToPortId = null;
            this.myShipmentPM.Transshipment2ToPortCode = null;
            this.myShipmentPM.Transshipment2ToPortName = null;
            this.myShipmentPM.Transshipment2ToPortCountryCode = null;
            this.myShipmentPM.Transshipment2ToPortCountryName = null;
        }
        else if (this.myBookingPM != null) {
            if (isMainLeg) {
                this.myBookingPM.MainCarriageETD = null;
                this.myBookingPM.MainCarriageCarrierNumber = null;
                this.myBookingPM.MainCarriageFromPortId = null;
                this.myBookingPM.MainFromPortCode = null;
                this.myBookingPM.MainFromPortName = null;
                this.myBookingPM.MainFromPortCountryCode = null;
                this.myBookingPM.MainFromPortCountryName = null;
            }
            this.myBookingPM.MainCarriageToPortId = null;
            this.myBookingPM.MainToPortCode = null;
            this.myBookingPM.MainToPortName = null;
            this.myBookingPM.MainToPortCountryCode = null;
            this.myBookingPM.MainToPortCountryName = null;
            //**********
            this.myBookingPM.Transshipment1ETD = null;
            this.myBookingPM.Transshipment1CarrierId = null;
            this.myBookingPM.Transshipment1CarrierName = null;
            this.myBookingPM.Transshipment1CarrierNumber = null;
            this.myBookingPM.Transshipment1CarrierPrefix = null;
            this.myBookingPM.Transshipment1FromPortId = null;
            this.myBookingPM.Trans1FromPortCode = null;
            this.myBookingPM.Trans1FromPortName = null;
            this.myBookingPM.Trans1FromPortCountryCode = null;
            this.myBookingPM.Trans1FromPortCountryName = null;
            this.myBookingPM.Transshipment1ToPortId = null;
            this.myBookingPM.Trans1ToPortCode = null;
            this.myBookingPM.Trans1ToPortName = null;
            this.myBookingPM.Trans1ToPortCountryCode = null;
            this.myBookingPM.Trans1ToPortCountryName = null;
            //**********
            this.myBookingPM.Transshipment2ETD = null;
            this.myBookingPM.Transshipment2CarrierId = null;
            this.myBookingPM.Transshipment2CarrierName = null;
            this.myBookingPM.Transshipment2CarrierNumber = null;
            this.myBookingPM.Transshipment2CarrierPrefix = null;
            this.myBookingPM.Transshipment2FromPortId = null;
            this.myBookingPM.Trans2FromPortCode = null;
            this.myBookingPM.Trans2FromPortName = null;
            this.myBookingPM.Trans2FromPortCountryCode = null;
            this.myBookingPM.Trans2FromPortCountryName = null;
            this.myBookingPM.Transshipment2ToPortId = null;
            this.myBookingPM.Trans2ToPortCode = null;
            this.myBookingPM.Trans2ToPortName = null;
            this.myBookingPM.Trans2ToPortCountryCode = null;
            this.myBookingPM.Trans2ToPortCountryName = null;
        }
        for (var i = 0; i < this.ItemsSource.length; i++) {
            var item = this.ItemsSource[i];
            if (item != null) {
                switch (i) {
                    case 0: {
                        if (isMainLeg) {
                            this.MapLeg_MN(item);
                        }
                        else {
                            if (this.myShipmentPM != null) {
                                this.myShipmentPM.MainCarriageToPortId = item.FromPortId;
                                this.myShipmentPM.MainCarriageToPortCode = item.FromPortCode;
                                this.myShipmentPM.MainCarriageToPortName = item.FromPortName;
                                this.myShipmentPM.MainCarriageToPortCountryCode = item.FromPortCountryCode;
                                this.myShipmentPM.MainCarriageToPortCountryName = item.FromPortCountryName;
                            }
                            else if (this.myBookingPM != null) {
                                this.myBookingPM.MainCarriageToPortId = item.FromPortId;
                                this.myBookingPM.MainToPortCode = item.FromPortCode;
                                this.myBookingPM.MainToPortName = item.FromPortName;
                                this.myBookingPM.MainToPortCountryCode = item.FromPortCountryCode;
                                this.myBookingPM.MainToPortCountryName = item.FromPortCountryName;
                            }
                            this.MapLeg_T1(item);
                        }
                        break;
                    }
                    case 1: {
                        if (isMainLeg) {
                            this.MapLeg_T1(item);
                        }
                        else {
                            this.MapLeg_T2(item);
                        }
                        break;
                    }
                    case 2: {
                        if (isMainLeg) {
                            this.MapLeg_T2(item);
                        }
                        else {
                            //this.MapLeg_T3(item);
                        }
                        break;
                    }
                }
                myFinalDestinationPortId = item.ToPortId;
            }
        }
        if (this.myShipmentPM != null) {
            this.myShipmentPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
        }
        else if (this.myBookingPM != null) {
            this.myBookingPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
        }
        if (this.fatherComponent != null) {
            this.fatherComponent.OnFlightSelected();
        }
    };
    FlightsSchedulesResponeViewModel.prototype.MapLeg_MN = function (item) {
        var itemETD = Tools_1.DateTool.GetDateParts(item.ETD).DateObject;
        if (this.myShipmentPM != null) {
            this.myShipmentPM.MainCarriageETD = itemETD;
            this.myShipmentPM.MainCarriageCarrierNumber = item.FlightNumber;
            this.myShipmentPM.MainCarriageFromPortId = item.FromPortId;
            this.myShipmentPM.MainCarriageFromPortCode = item.FromPortCode;
            this.myShipmentPM.MainCarriageFromPortName = item.FromPortName;
            this.myShipmentPM.MainCarriageFromPortCountryCode = item.FromPortCountryCode;
            this.myShipmentPM.MainCarriageFromPortCountryName = item.FromPortCountryName;
            this.myShipmentPM.MainCarriageToPortId = item.ToPortId;
            this.myShipmentPM.MainCarriageToPortCode = item.ToPortCode;
            this.myShipmentPM.MainCarriageToPortName = item.ToPortName;
            this.myShipmentPM.MainCarriageToPortCountryCode = item.ToPortCountryCode;
            this.myShipmentPM.MainCarriageToPortCountryName = item.ToPortCountryName;
        }
        else if (this.myBookingPM != null) {
            this.myBookingPM.MainCarriageETD = itemETD;
            this.myBookingPM.MainCarriageCarrierNumber = item.FlightNumber;
            this.myBookingPM.MainCarriageFromPortId = item.FromPortId;
            this.myBookingPM.MainFromPortCode = item.FromPortCode;
            this.myBookingPM.MainFromPortName = item.FromPortName;
            this.myBookingPM.MainFromPortCountryCode = item.FromPortCountryCode;
            this.myBookingPM.MainFromPortCountryName = item.FromPortCountryName;
            this.myBookingPM.MainCarriageToPortId = item.ToPortId;
            this.myBookingPM.MainToPortCode = item.ToPortCode;
            this.myBookingPM.MainToPortName = item.ToPortName;
            this.myBookingPM.MainToPortCountryCode = item.ToPortCountryCode;
            this.myBookingPM.MainToPortCountryName = item.ToPortCountryName;
        }
    };
    FlightsSchedulesResponeViewModel.prototype.MapLeg_T1 = function (item) {
        var itemETD = Tools_1.DateTool.GetDateParts(item.ETD).DateObject;
        if (this.myShipmentPM != null) {
            this.myShipmentPM.Transshipment1ETD = Tools_1.DateTool.GetDateParts(itemETD).DateObject;
            this.myShipmentPM.Transshipment1CarrierId = item.AirlineId;
            this.myShipmentPM.Transshipment1CarrierCode = item.AirlineCode;
            this.myShipmentPM.Transshipment1CarrierName = item.AirlineName;
            this.myShipmentPM.Transshipment1CarrierNumber = item.FlightNumber;
            this.myShipmentPM.Transshipment1CarrierPrefix = item.AirlineCode;
            this.myShipmentPM.Transshipment1FromPortId = item.FromPortId;
            this.myShipmentPM.Transshipment1FromPortCode = item.FromPortCode;
            this.myShipmentPM.Transshipment1FromPortName = item.FromPortName;
            this.myShipmentPM.Transshipment1FromPortCountryCode = item.FromPortCountryCode;
            this.myShipmentPM.Transshipment1FromPortCountryName = item.FromPortCountryName;
            this.myShipmentPM.Transshipment1ToPortId = item.ToPortId;
            this.myShipmentPM.Transshipment1ToPortCode = item.ToPortCode;
            this.myShipmentPM.Transshipment1ToPortName = item.ToPortName;
            this.myShipmentPM.Transshipment1ToPortCountryCode = item.ToPortCountryCode;
            this.myShipmentPM.Transshipment1ToPortCountryName = item.ToPortCountryName;
        }
        else if (this.myBookingPM != null) {
            this.myBookingPM.Transshipment1ETD = itemETD;
            this.myBookingPM.Transshipment1CarrierId = item.AirlineId;
            this.myBookingPM.Transshipment1CarrierName = item.AirlineName;
            this.myBookingPM.Transshipment1CarrierNumber = item.FlightNumber;
            this.myBookingPM.Transshipment1CarrierPrefix = item.AirlineCode;
            this.myBookingPM.Transshipment1FromPortId = item.FromPortId;
            this.myBookingPM.Trans1FromPortCode = item.FromPortCode;
            this.myBookingPM.Trans1FromPortName = item.FromPortName;
            this.myBookingPM.Trans1FromPortCountryCode = item.FromPortCountryCode;
            this.myBookingPM.Trans1FromPortCountryName = item.FromPortCountryName;
            this.myBookingPM.Transshipment1ToPortId = item.ToPortId;
            this.myBookingPM.Trans1ToPortCode = item.ToPortCode;
            this.myBookingPM.Trans1ToPortName = item.ToPortName;
            this.myBookingPM.Trans1ToPortCountryCode = item.ToPortCountryCode;
            this.myBookingPM.Trans1ToPortCountryName = item.ToPortCountryName;
        }
    };
    FlightsSchedulesResponeViewModel.prototype.MapLeg_T2 = function (item) {
        var itemETD = Tools_1.DateTool.GetDateParts(item.ETD).DateObject;
        if (this.myShipmentPM != null) {
            this.myShipmentPM.Transshipment2ETD = itemETD;
            this.myShipmentPM.Transshipment2CarrierId = item.AirlineId;
            this.myShipmentPM.Transshipment2CarrierCode = item.AirlineCode;
            this.myShipmentPM.Transshipment2CarrierName = item.AirlineName;
            this.myShipmentPM.Transshipment2CarrierNumber = item.FlightNumber;
            this.myShipmentPM.Transshipment2CarrierPrefix = item.AirlineCode;
            this.myShipmentPM.Transshipment2FromPortId = item.FromPortId;
            this.myShipmentPM.Transshipment2FromPortCode = item.FromPortCode;
            this.myShipmentPM.Transshipment2FromPortName = item.FromPortName;
            this.myShipmentPM.Transshipment2FromPortCountryCode = item.FromPortCountryCode;
            this.myShipmentPM.Transshipment2FromPortCountryName = item.FromPortCountryName;
            this.myShipmentPM.Transshipment2ToPortId = item.ToPortId;
            this.myShipmentPM.Transshipment2ToPortCode = item.ToPortCode;
            this.myShipmentPM.Transshipment2ToPortName = item.ToPortName;
            this.myShipmentPM.Transshipment2ToPortCountryCode = item.ToPortCountryCode;
            this.myShipmentPM.Transshipment2ToPortCountryName = item.ToPortCountryName;
        }
        else if (this.myBookingPM != null) {
            this.myBookingPM.Transshipment2ETD = itemETD;
            this.myBookingPM.Transshipment2CarrierId = item.AirlineId;
            this.myBookingPM.Transshipment2CarrierName = item.AirlineName;
            this.myBookingPM.Transshipment2CarrierNumber = item.FlightNumber;
            this.myBookingPM.Transshipment2CarrierPrefix = item.AirlineCode;
            this.myBookingPM.Transshipment2FromPortId = item.FromPortId;
            this.myBookingPM.Trans2FromPortCode = item.FromPortCode;
            this.myBookingPM.Trans2FromPortName = item.FromPortName;
            this.myBookingPM.Trans2FromPortCountryCode = item.FromPortCountryCode;
            this.myBookingPM.Trans2FromPortCountryName = item.FromPortCountryName;
            this.myBookingPM.Transshipment2ToPortId = item.ToPortId;
            this.myBookingPM.Trans2ToPortCode = item.ToPortCode;
            this.myBookingPM.Trans2ToPortName = item.ToPortName;
            this.myBookingPM.Trans2ToPortCountryCode = item.ToPortCountryCode;
            this.myBookingPM.Trans2ToPortCountryName = item.ToPortCountryName;
        }
    };
    FlightsSchedulesResponeViewModel.prototype.MapLeg_T3 = function (item) {
    };
    return FlightsSchedulesResponeViewModel;
}());
exports.FlightsSchedulesResponeViewModel = FlightsSchedulesResponeViewModel;
var FlightItemViewModel = /** @class */ (function () {
    function FlightItemViewModel(entityPM) {
        this.entityPM = entityPM;
    }
    Object.defineProperty(FlightItemViewModel.prototype, "Id", {
        get: function () { return this.entityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "LineNumber", {
        get: function () { return this.entityPM.LineNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "AirlineId", {
        get: function () { return this.entityPM.AirlineId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "AirlineCode", {
        get: function () { return this.entityPM.AirlineCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "AirlineName", {
        get: function () { return this.entityPM.AirlineName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "AirplaneType", {
        get: function () { return this.entityPM.AirplaneType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "FlightNumber", {
        get: function () { return this.entityPM.FlightNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "Carrier", {
        get: function () { return this.AirlineCode + " " + this.FlightNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "NumberOfStops", {
        get: function () { return this.entityPM.NumberOfStops; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "FromPortId", {
        get: function () { return this.entityPM.FromPortId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "FromPortCode", {
        get: function () { return this.entityPM.FromPortCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "FromPortName", {
        get: function () { return this.entityPM.FromPortName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "FromPortCountryCode", {
        get: function () { return this.entityPM.FromPortCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "FromPortCountryName", {
        get: function () { return this.entityPM.FromPortCountryName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ToPortId", {
        get: function () { return this.entityPM.ToPortId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ToPortCode", {
        get: function () { return this.entityPM.ToPortCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ToPortName", {
        get: function () { return this.entityPM.ToPortName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ToPortCountryCode", {
        get: function () { return this.entityPM.ToPortCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ToPortCountryName", {
        get: function () { return this.entityPM.ToPortCountryName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "MissingPort", {
        get: function () { return this.entityPM.MissingPort; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ETD", {
        get: function () { return this.entityPM.ETD; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "ETA", {
        get: function () { return this.entityPM.ETA; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FlightItemViewModel.prototype, "IsLineVisible", {
        get: function () {
            var myResult = false;
            if (this.LineNumber > 0) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    FlightItemViewModel.prototype.UpdateMissingPorts = function (item) {
        if (this.entityPM.FromPortId == null) {
            if (this.entityPM.FromPortCode == item.PortCode) {
                this.entityPM.FromPortId = item.PortId;
                this.entityPM.FromPortCode = item.PortCode;
                this.entityPM.FromPortName = item.PortName;
                this.entityPM.FromPortCountryCode = item.PortCountryCode;
                this.entityPM.FromPortCountryName = item.PortCountryName;
            }
        }
        if (this.entityPM.ToPortId == null) {
            if (this.entityPM.ToPortCode == item.PortCode) {
                this.entityPM.ToPortId = item.PortId;
                this.entityPM.ToPortCode = item.PortCode;
                this.entityPM.ToPortName = item.PortName;
                this.entityPM.ToPortCountryCode = item.PortCountryCode;
                this.entityPM.ToPortCountryName = item.PortCountryName;
            }
        }
        if (this.entityPM.FromPortId != null && this.entityPM.ToPortId != null) {
            this.entityPM.MissingPort = false;
        }
    };
    return FlightItemViewModel;
}());
exports.FlightItemViewModel = FlightItemViewModel;
var ResponseItemArgs = /** @class */ (function () {
    function ResponseItemArgs() {
    }
    return ResponseItemArgs;
}());
//# sourceMappingURL=FlightsSchedulesComponent.js.map