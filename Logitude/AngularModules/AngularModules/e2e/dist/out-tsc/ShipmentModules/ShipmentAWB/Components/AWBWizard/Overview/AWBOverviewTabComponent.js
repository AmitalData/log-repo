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
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var LocationDirective_1 = require("../../../../../Infrastructure/Utilities/LocationDirective");
var ShipmentDomainService_1 = require("../../../../../Shipment/Services/ShipmentDomainService");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var AWBWizardComponent_1 = require("../AWBWizardComponent");
var FSRWizardComponent_1 = require("../../FSRWizard/FSRWizardComponent");
var SendFSRComponent_1 = require("../../FSRWizard/SendFSRComponent");
var Tools_2 = require("../../../../../Shipment/Tools");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var AWBOverviewTabComponent = /** @class */ (function () {
    function AWBOverviewTabComponent() {
        this.DataContext = this;
        this.ItemsSource = [];
        this.IsFullWizard = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.AWBWizard = null;
        this.FSRWizard = null;
        this.IsFHLStatusVisible = false;
        this.IsCargonautFWBStatusVisible = false;
        this.IsCargonautFHLStatusVisible = false;
        this.IsFNAReasonStatusVisible = false;
        this.IsDemoTenantStatusVisible = false;
        this.IsBookingStatusVisible = false;
        this.CargonautDEXXFWBLabel = null;
        this.CargonautDEXXFHLLabel = null;
        // Tabs
        this.CommunicationsPage = null;
        this.selectedTabCode = "ST";
        // Statuses
        this.IsNoStatus = false;
        // FSR
        this.isSendWindowOpen = false;
        this.IsFSRButtonVisible = false;
        this.IsFSRButtonEnabled = false;
    }
    AWBOverviewTabComponent.prototype.InitTab = function (entityPM, wizard) {
        if (wizard instanceof AWBWizardComponent_1.AWBWizardComponent) {
            this.AWBWizard = wizard;
            this.IsFullWizard = true;
        }
        if (wizard instanceof FSRWizardComponent_1.FSRWizardComponent) {
            this.FSRWizard = wizard;
            this.IsFullWizard = false;
        }
        this.EntityPM = entityPM;
        this.ObjectTableName = entityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.SetFSRStatus();
        this.SetMessagingStatus();
        this.LoadCarrierStatuses();
        this.Listen();
    };
    AWBOverviewTabComponent.prototype.RefreshTab = function () {
        this.SetFSRStatus();
        this.SetMessagingStatus();
    };
    AWBOverviewTabComponent.prototype.SetMessagingStatus = function () {
        this.SetMessagingLables();
        this.IsFHLStatusVisible = false;
        this.IsCargonautFWBStatusVisible = false;
        this.IsCargonautFHLStatusVisible = false;
        this.IsFNAReasonStatusVisible = false;
        this.IsDemoTenantStatusVisible = false;
        this.IsBookingStatusVisible = false;
        if (this.EntityPM.ShipmentLevelCode != "D") {
            this.IsFHLStatusVisible = true;
        }
        switch (this.EntityPM.MainCarriageFromPortCode) {
            case "SPL":
            case "AMS":
            case "RTM":
            case "MST":
            case "LGG":
            case "BRU":
                {
                    this.IsCargonautFWBStatusVisible = true;
                    if (this.EntityPM.ShipmentLevelCode != "D") {
                        this.IsCargonautFHLStatusVisible = true;
                    }
                    break;
                }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FNAReason)) {
            this.IsFNAReasonStatusVisible = true;
        }
        if (SessionLocator_1.SessionLocator.TenantPM.Id == 65 || SessionLocator_1.SessionLocator.TenantManagementJS.IsEAWBOnlyDemo) {
            this.IsDemoTenantStatusVisible = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
            this.IsBookingStatusVisible = true;
        }
    };
    AWBOverviewTabComponent.prototype.SetMessagingLables = function () {
        var FWBLabel = "Cargonaut FWB";
        var FHLLabel = "Cargonaut FHL";
        switch (this.EntityPM.MainCarriageFromPortCode) {
            case "LGG":
            case "BRU":
                {
                    FWBLabel = "DEXX FWB";
                    FHLLabel = "DEXX FWB";
                    break;
                }
        }
        this.CargonautDEXXFWBLabel = FWBLabel;
        this.CargonautDEXXFHLLabel = FHLLabel;
    };
    Object.defineProperty(AWBOverviewTabComponent.prototype, "FWBStatusName", {
        // Messaging Status
        get: function () { return this.EntityPM.FWBStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "FWBStatusDate", {
        get: function () { return this.EntityPM.FWBStatusDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "FHLStatusName", {
        get: function () { return this.EntityPM.FHLStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "FHLStatusDate", {
        get: function () { return this.EntityPM.FHLStatusDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CargonautFWBStatusName", {
        get: function () { return this.EntityPM.CargonautFWBStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CargonautFWBStatusDate", {
        get: function () { return this.EntityPM.CargonautFWBStatusDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CargonautFHLStatusName", {
        get: function () { return this.EntityPM.CargonautFHLStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CargonautFHLStatusDate", {
        get: function () { return this.EntityPM.CargonautFHLStatusDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CarrierLastStatusCode", {
        get: function () { return this.EntityPM.CarrierLastStatusCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CarrierLastStatusName", {
        get: function () { return this.EntityPM.CarrierLastStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "CarrierLastStatusDate", {
        get: function () { return this.EntityPM.CarrierLastStatusDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "FNAReason", {
        get: function () { return this.EntityPM.FNAReason; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "BookingId", {
        get: function () { return this.EntityPM.BookingId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "BookingNumber", {
        get: function () { return this.EntityPM.BookingNumber; },
        enumerable: true,
        configurable: true
    });
    AWBOverviewTabComponent.prototype.OpenBooking = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BookingId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = "Edit Booking Title";
            logWindow.WindowArgs = this.BookingId;
            logWindow.Show('./Booking/Components/BookingWizard/BookingWizardLoadComponent');
        }
    };
    Object.defineProperty(AWBOverviewTabComponent.prototype, "VolumeUnitCode", {
        // Cargo Information
        get: function () { return this.EntityPM.VolumeUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "Volume", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "GrossWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "Quantity", {
        get: function () {
            var myResult = 0;
            if (Tools_1.AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId)) {
                this.EntityPM.ShipmentPackages.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrZero(item.Quantity)) {
                        myResult += item.Quantity;
                    }
                });
            }
            else {
                myResult = this.EntityPM.ShipmentPackages.length;
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = 0;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBOverviewTabComponent.prototype, "SelectedTabCode", {
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
    AWBOverviewTabComponent.prototype.SelectionChanged = function () {
        var _this = this;
        switch (this.SelectedTabCode) {
            case "ST": {
                break;
            }
            case "CM": {
                if (this.CommunicationsPage == null) {
                    var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                    if (myLocation_1 != null) {
                        this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.CommunicationsPage = cmpRef.instance;
                                _this.CommunicationsPage.IsTitleHidden = true;
                                //this.CommunicationsPage.Run(this.EntityPM.Id, this.ObjectTableName);
                            });
                        });
                    }
                }
                else {
                    //this.CommunicationsPage.Load();
                }
                break;
            }
        }
    };
    AWBOverviewTabComponent.prototype.LoadCarrierStatuses = function () {
        var _this = this;
        this.ItemsSource = [];
        this.IsNoStatus = false;
        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        }
        this.myDomainService.GetShipmentCarrierStatuses(this.EntityPM.Id).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    myResponse.Result.forEach(function (item) {
                        _this.ItemsSource.push(new StatusLineItem(item));
                    });
                    if (myResponse.Result.length == 0) {
                        _this.IsNoStatus = true;
                    }
                }
            }
        });
    };
    // Refresh
    AWBOverviewTabComponent.prototype.RefreshClicked = function () {
        this.ReloadEntity();
    };
    AWBOverviewTabComponent.prototype.SetFSRStatus = function () {
        var isFSRButtonVisible = false;
        var isFSRButtonEnabled = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "SENDREQUEST")) {
            isFSRButtonVisible = true;
        }
        if (this.IsFullWizard) {
            if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
                if (this.EntityPM.TenantZeroAirlineGLSHKFSRFSA) {
                    isFSRButtonEnabled = true;
                }
            }
            else {
                if (this.EntityPM.TenantZeroAirlineChampFSRFSA) {
                    isFSRButtonEnabled = true;
                }
            }
        }
        else {
            if (SessionLocator_1.SessionLocator.TenantPM.Id == 0) {
                isFSRButtonEnabled = true;
            }
            else if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
                isFSRButtonEnabled = true;
            }
            else if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                isFSRButtonEnabled = true;
            }
        }
        this.IsFSRButtonVisible = isFSRButtonVisible;
        this.IsFSRButtonEnabled = isFSRButtonEnabled;
    };
    AWBOverviewTabComponent.prototype.SendFSRClicked = function () {
        var _this = this;
        var ShowConfirmRCS = false;
        if (this.ItemsSource) {
            if (this.ItemsSource.filter(function (d) { return d.Status.toUpperCase() == "RCS"; }).length > 0) {
                ShowConfirmRCS = true;
            }
        }
        if (ShowConfirmRCS) {
            var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
            myConfirmWindow.Width = 400;
            myConfirmWindow.Show("The airline may not receive your update after RCS status received, send anyway?");
            myConfirmWindow.WindowClosed.subscribe(function (event) {
                if (myConfirmWindow.Yes) {
                    _this.SendFSR();
                }
            });
        }
        else {
            this.SendFSR();
        }
    };
    AWBOverviewTabComponent.prototype.SendFSR = function () {
        var isValidFSR = false;
        if (this.AWBWizard != null) {
            isValidFSR = this.AWBWizard.ValidateShipment();
            if (isValidFSR) {
                isValidFSR = this.AWBWizard.ValidateFSR();
            }
        }
        else if (this.FSRWizard != null) {
            isValidFSR = this.FSRWizard.ValidateFSR();
        }
        if (isValidFSR) {
            var myCCSValidator = Tools_2.AWBHelper.ValidateAWBCCS(this.EntityPM);
            if (myCCSValidator.FSRFSA) {
                if (myCCSValidator.IsValid) {
                    if (this.AWBWizard != null) {
                        this.AWBWizard.SaveFSR();
                    }
                    else if (this.FSRWizard != null) {
                        this.RunSendWindow();
                    }
                }
                else {
                    if (myCCSValidator.TenantManagementFieldHasError) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show(myCCSValidator.TenantManagementFieldErrorMessage);
                    }
                    else if (myCCSValidator.AirlineFieldHasError) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show(myCCSValidator.AirlineFieldErrorMessage);
                    }
                }
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("This airline does not support FSR/FSA messages");
            }
        }
    };
    AWBOverviewTabComponent.prototype.RunSendWindow = function () {
        var _this = this;
        if (!this.isSendWindowOpen) {
            this.isSendWindowOpen = true;
            var args = new SendFSRComponent_1.SendFSRArgs();
            args.EntityPM = this.EntityPM;
            args.OverviewTab = this;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "FSR Request";
            logWindow.WindowArgs = args;
            logWindow.Show("./ShipmentModules/ShipmentAWB/Components/FSRWizard/SendFSRComponent");
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.isSendWindowOpen = false;
            });
        }
    };
    AWBOverviewTabComponent.prototype.ReloadEntity = function () {
        if (this.AWBWizard != null) {
            this.AWBWizard.ReloadEntity();
        }
        else if (this.FSRWizard != null) {
            this.FSRWizard.ReloadEntity();
        }
    };
    AWBOverviewTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.AWBWizard != null) {
            this.AWBWizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.AWBWizard.EntityPM;
                    _this.RefreshTab();
                    if (_this.AWBWizard.IsFSRRequestButtonClicked) {
                        _this.AWBWizard.IsFSRRequestButtonClicked = false;
                        _this.RunSendWindow();
                    }
                }
            });
            this.AWBWizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.AWBWizard.EntityPM;
                    _this.LoadCarrierStatuses();
                    _this.RefreshTab();
                    if (_this.AWBWizard.IsFSRRequestButtonClicked) {
                        _this.AWBWizard.IsFSRRequestButtonClicked = false;
                        _this.RunSendWindow();
                    }
                }
            });
        }
        else if (this.FSRWizard != null) {
            this.FSRWizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.FSRWizard.EntityPM;
                    _this.LoadCarrierStatuses();
                    _this.RefreshTab();
                }
            });
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], AWBOverviewTabComponent.prototype, "AllLocations", void 0);
    AWBOverviewTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'OverviewTabComponent',
            templateUrl: './AWBOverviewTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBOverviewTabComponent);
    return AWBOverviewTabComponent;
}());
exports.AWBOverviewTabComponent = AWBOverviewTabComponent;
var StatusLineItem = /** @class */ (function () {
    function StatusLineItem(item) {
        this.item = item;
        this.Name = "LOL";
        this.LegHeight = 50;
        this.IsDatesRowVisible = false;
        if (item.DepartureDate != null || item.ArrivalDate != null) {
            this.LegHeight = 75;
            this.IsDatesRowVisible = true;
            this.SetDatesProperties();
        }
    }
    Object.defineProperty(StatusLineItem.prototype, "Status", {
        get: function () { return this.item.Status; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "StatusName", {
        get: function () { return this.item.StatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "Details", {
        get: function () { return this.item.Details; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "EventDate", {
        get: function () { return this.item.EventDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "ReceivingDate", {
        get: function () { return this.item.ReceivingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "Weight", {
        get: function () { return this.item.Weight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "Pieces", {
        get: function () { return this.item.Pieces; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "Partial", {
        get: function () { return this.item.Partial; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "AirlineName", {
        get: function () { return this.item.AirlineName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "FlightNumber", {
        get: function () { return this.item.FlightNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "LocationCode", {
        get: function () { return this.item.LocationCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "LocationName", {
        get: function () { return this.item.LocationName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "DepartureDate", {
        get: function () { return this.item.DepartureDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatusLineItem.prototype, "ArrivalDate", {
        get: function () { return this.item.ArrivalDate; },
        enumerable: true,
        configurable: true
    });
    StatusLineItem.prototype.SetDatesProperties = function () {
        if (this.item.DepartureDate != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.item.TimeOfDepartureInfo)) {
                switch (this.item.TimeOfDepartureInfo.toUpperCase()) {
                    case "A": {
                        this.DepartureLabel = "Actual Departure: ";
                        break;
                    }
                    case "E": {
                        this.DepartureLabel = "Expected Departure: ";
                        break;
                    }
                    case "S": {
                        this.DepartureLabel = "Scheduled Departure: ";
                        break;
                    }
                    default:
                        {
                            this.DepartureLabel = "Departure: ";
                            break;
                        }
                        ;
                }
            }
        }
        if (this.item.ArrivalDate != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.item.TimeOfArrivalInfo)) {
                switch (this.item.TimeOfArrivalInfo.toUpperCase()) {
                    case "A": {
                        this.ArrivalLabel = "Actual Arrival: ";
                        break;
                    }
                    case "E": {
                        this.ArrivalLabel = "Expected Arrival: ";
                        break;
                    }
                    case "S": {
                        this.ArrivalLabel = "Scheduled Arrival: ";
                        break;
                    }
                    default:
                        {
                            this.ArrivalLabel = "Arrival: ";
                            break;
                        }
                        ;
                }
            }
        }
    };
    return StatusLineItem;
}());
//# sourceMappingURL=AWBOverviewTabComponent.js.map