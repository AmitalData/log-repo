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
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BookingPMService_1 = require("../../../Services/StandardPMs/BookingPMService");
var BookingDomainService_1 = require("../../../Services/BookingDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var OverviewTabComponent = /** @class */ (function () {
    function OverviewTabComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isSaveRequested = false;
        this.isReloadRequested = false;
        this.IsConfirmManuallyVisibile = false;
        this.IsCancelManuallyVisible = false;
        this.IsConfirmManuallyEnabled = false;
        this.CommunicationsPage = null;
        this.selectedTabCode = "ST";
        this.SameDataText = "";
        this.sameAnswersData = true;
        this.AllAnswersHaveSameData = false;
        // Timer
        this.Retries = 0;
        this.timerSeconds = 1;
        this.IsLoading = false;
        this.IsFirstTimeLoaded = false;
    }
    OverviewTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.InitializeTimer();
        this.BuildAnswersData(this.EntityPM.BookingAnswers);
        this.SetUIProperties_ManualAction();
    };
    OverviewTabComponent.prototype.RefreshTab = function () {
        this.BuildAnswersData(this.EntityPM.BookingAnswers);
        this.SetUIProperties_ManualAction();
    };
    OverviewTabComponent.prototype.SetDataAfterSending = function () {
        this.IsFirstTimeLoaded = true;
        this.EntityPM = this.Wizard.EntityPM;
        this.BuildAnswersData(this.EntityPM.BookingAnswers);
        this.SetUIProperties_ManualAction();
        this.StopTimer();
        this.StartTimer();
    };
    OverviewTabComponent.prototype.Save = function () {
        this.isSaveRequested = true;
        this.Wizard.SaveClicked();
    };
    OverviewTabComponent.prototype.Reload = function () {
        this.isReloadRequested = true;
        this.Wizard.ReloadEntity();
    };
    OverviewTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties_ManualAction();
                }
                if (_this.isSaveRequested) {
                    _this.isSaveRequested = false;
                    if (isSaveSuccess) {
                        _this.Reload();
                    }
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    if (_this.isReloadRequested) {
                        _this.isReloadRequested = false;
                        _this.SetUIProperties_ManualAction();
                    }
                }
            });
        }
    };
    OverviewTabComponent.prototype.OpenShipment = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentId)) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = "Edit AWB Wizard";
            logWindow.WindowArgs = this.ShipmentId;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');
        }
    };
    OverviewTabComponent.prototype.SetUIProperties_ManualAction = function () {
        var isConfirmVisible = false;
        var isCancelVisible = false;
        var isConfirmEnabled = true;
        if (this.EntityPM.FFRStatusCode == "BRQ" || this.EntityPM.FFRStatusCode == "RBA") {
            isConfirmVisible = true;
        }
        else if (this.EntityPM.FFRStatusCode == "CRS" || this.EntityPM.FFRStatusCode == "RBC") {
            isCancelVisible = true;
        }
        if (this.EntityPM.IsCancelled) {
            isConfirmEnabled = false;
        }
        this.IsConfirmManuallyVisibile = isConfirmVisible;
        this.IsCancelManuallyVisible = isCancelVisible;
        this.IsConfirmManuallyEnabled = isConfirmEnabled;
    };
    Object.defineProperty(OverviewTabComponent.prototype, "SelectedTabCode", {
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
    OverviewTabComponent.prototype.SelectionChanged = function () {
        var _this = this;
        switch (this.SelectedTabCode) {
            case "ST": {
                break;
            }
            case "CM": {
                if (this.CommunicationsPage == null) {
                    var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
                    if (myLocation_1 != null) {
                        this._entityResourceService.getEntityResourceByTableName("CommunicationLog", 0).subscribe(function (resp) {
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
    Object.defineProperty(OverviewTabComponent.prototype, "FFRStatusName", {
        // Status
        get: function () { return this.EntityPM.FFRStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "BookingStatusName", {
        get: function () { return this.EntityPM.BookingStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ShipmentId", {
        get: function () { return this.EntityPM.ShipmentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "VolumeUnitCode", {
        // Cardo Info
        get: function () { return this.EntityPM.VolumeUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Volume", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "GrossWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Quantity", {
        get: function () {
            var myResult = 0;
            if (this.EntityPM != null) {
                this.EntityPM.BookingPackages.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrZero(item.Quantity)) {
                        myResult += item.Quantity;
                    }
                });
            }
            if (Tools_1.AppTool.IsNullOrZero(myResult)) {
                myResult = 0;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageCarrierName", {
        // Requests
        get: function () { return this.EntityPM.MainCarriageCarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageFlightNumber", {
        get: function () {
            var result = this.EntityPM.MainCarriageCarrierPrefix;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierNumber)) {
                result = result + this.EntityPM.MainCarriageCarrierNumber;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageETD", {
        get: function () { return this.EntityPM.MainCarriageETD; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageSpaceAllocationCode", {
        get: function () { return this.EntityPM.MainCarriageSpaceAllocationCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "MainCarriageAllotmentId", {
        get: function () { return this.EntityPM.MainCarriageAllotmentIdentification; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment1FromPortId", {
        get: function () { return this.EntityPM.Transshipment1FromPortId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment1CarrierName", {
        get: function () { return this.EntityPM.Transshipment1CarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment1FlightNumber", {
        get: function () {
            var result = this.EntityPM.Transshipment1CarrierPrefix;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1CarrierNumber)) {
                result = result + this.EntityPM.Transshipment1CarrierNumber;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment1ETD", {
        get: function () { return this.EntityPM.Transshipment1ETD; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment1SpaceAllocationCode", {
        get: function () { return this.EntityPM.Transshipment1SpaceAllocationCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment1AllotmentId", {
        get: function () { return this.EntityPM.Transshipment1AllotmentIdentification; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment2FromPortId", {
        get: function () { return this.EntityPM.Transshipment2FromPortId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment2CarrierName", {
        get: function () { return this.EntityPM.Transshipment2CarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment2FlightNumber", {
        get: function () {
            var result = this.EntityPM.Transshipment2CarrierPrefix;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2CarrierNumber)) {
                result = result + this.EntityPM.Transshipment2CarrierNumber;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment2ETD", {
        get: function () { return this.EntityPM.Transshipment2ETD; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment2SpaceAllocationCode", {
        get: function () { return this.EntityPM.Transshipment2SpaceAllocationCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Transshipment2AllotmentId", {
        get: function () { return this.EntityPM.Transshipment2AllotmentIdentification; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "FFRStatusDate", {
        get: function () { return this.EntityPM.FFRStatusDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "FMAAcknowledgement", {
        get: function () { return this.EntityPM.FMAAcknowledgementReason; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "FNAReason", {
        get: function () { return this.EntityPM.FNAReason; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "OSI", {
        get: function () { return this.EntityPM.AnswerOtherServicesInformation; },
        enumerable: true,
        configurable: true
    });
    OverviewTabComponent.prototype.CheckSameData = function () {
        var _this = this;
        this.AnswersList.forEach(function (item) {
            var answerDate = null;
            var date1 = null;
            var date2 = null;
            var date3 = null;
            if (item.ETD != null) {
                answerDate = Tools_1.DateTool.TruncateTime(item.ETD);
            }
            if (_this.EntityPM.MainCarriageETD != null) {
                date1 = Tools_1.DateTool.TruncateTime(_this.EntityPM.MainCarriageETD);
            }
            if (_this.EntityPM.Transshipment1ETD != null) {
                date2 = Tools_1.DateTool.TruncateTime(_this.EntityPM.Transshipment1ETD);
            }
            if (_this.EntityPM.Transshipment2ETD != null) {
                date3 = Tools_1.DateTool.TruncateTime(_this.EntityPM.Transshipment2ETD);
            }
            if (item.LegText == "Leg 1") {
                if (item.FlightNumber != _this.MainCarriageFlightNumber || answerDate.valueOf() != date1.valueOf() || item.CarrierId != _this.EntityPM.MainCarriageCarrierId) {
                    _this.sameAnswersData = false;
                    return;
                }
            }
            else if (item.LegText == "Leg 2") {
                if (_this.sameAnswersData) {
                    if (item.FlightNumber != _this.Transshipment1FlightNumber || answerDate.valueOf() != date2.valueOf() || item.CarrierId != _this.EntityPM.Transshipment1CarrierId) {
                        _this.sameAnswersData = false;
                        return;
                    }
                }
            }
            else if (item.LegText == "Leg 3") {
                if (_this.sameAnswersData) {
                    if (item.FlightNumber != _this.Transshipment2FlightNumber || answerDate.valueOf() != date3.valueOf() || item.CarrierId != _this.EntityPM.Transshipment2CarrierId) {
                        _this.sameAnswersData = false;
                        return;
                    }
                }
            }
        });
        //this.SameAnswersData = true;
        if (this.AnswersList.length > 0 && this.sameAnswersData) {
            this.AllAnswersHaveSameData = true;
            if (this.AnswersList.every(function (elem) { return elem.SpaceAllocationCode == "KK"; })) {
                this.SameDataText = "Booking Confirmed";
            }
            else if (this.AnswersList.every(function (elem) { return elem.SpaceAllocationCode == "UU"; })) {
                this.SameDataText = "Unable";
            }
            else if (this.AnswersList.every(function (elem) { return elem.SpaceAllocationCode == "CN"; })) {
                this.SameDataText = "Cancellation Noted";
            }
        }
    };
    //Answers    
    OverviewTabComponent.prototype.BuildAnswersData = function (list) {
        var _this = this;
        if (this.AnswersList == null) {
            this.AnswersList = new Array();
        }
        else {
            this.AnswersList = [];
        }
        var i = 1;
        list.sort(function (a, b) { return (a === b) ? 0 : a ? -1 : 1; }).forEach(function (item) {
            var text = "Leg " + i++;
            var itemViewModel = new BookingAnswerItem(item, _this, text);
            _this.AnswersList.push(itemViewModel);
        });
        this.CheckSameData();
    };
    OverviewTabComponent.prototype.LoadAnswers = function () {
        var _this = this;
        var myBookingDomainService = new BookingDomainService_1.BookingDomainService();
        myBookingDomainService.GetBookingAnswerPMs(this.EntityPM.Id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                _this.BuildAnswersData(list);
            }
        });
    };
    OverviewTabComponent.prototype.RefreshAnswersClicked = function () {
        this.LoadBooking();
    };
    // Manual Actions
    OverviewTabComponent.prototype.SetStatusManually = function (type) {
        this.EntityPM.WaitingForResponse = false;
        if (type == "Confirmed") {
            this.EntityPM.BookingStatusCode = "CNF";
            this.EntityPM.FFRStatusCode = "CFM";
        }
        else if (type == "Cancelled") {
            this.EntityPM.BookingStatusCode = "CRT";
            this.EntityPM.FFRStatusCode = "CNM";
        }
        this.Wizard.SaveManualStatus();
    };
    OverviewTabComponent.prototype.InitializeTimer = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            if (this.EntityPM.WaitingForResponse) {
                this.StartTimer();
            }
        }
    };
    OverviewTabComponent.prototype.StopTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        //this.Wizard.SaveCompleted.emit(false);
        this.Wizard.IsResponseProgressVisible = false;
    };
    OverviewTabComponent.prototype.StartTimer = function () {
        var _this = this;
        this.Retries = 0;
        this.timerToken = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
        this.Wizard.IsResponseProgressVisible = true;
    };
    OverviewTabComponent.prototype.IncreaseTimer = function () {
        var _this = this;
        clearTimeout(this.timerToken);
        this.timerToken = setInterval(function () { return _this.RunTimerFunction(); }, this.timerSeconds * 1000);
    };
    OverviewTabComponent.prototype.AdjustTimerSpeed = function () {
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
    OverviewTabComponent.prototype.RunTimerFunction = function () {
        if (!this.IsLoading) {
            this.Retries++;
            this.LoadBooking();
            this.AdjustTimerSpeed();
        }
    };
    OverviewTabComponent.prototype.LoadBooking = function () {
        var _this = this;
        var myService = new BookingPMService_1.BookingPMService();
        myService.get(this.EntityPM.Id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                _this.Wizard.EntityPM = myResponse.Result;
                if (!_this.IsFirstTimeLoaded) {
                    _this.IsFirstTimeLoaded = true;
                    _this.LoadAnswers();
                    _this.SetUIProperties_ManualAction();
                }
                if (_this.EntityPM.HasResponse && _this.EntityPM.WaitingForResponse) {
                    _this.StopTimer();
                    _this.LoadAnswers();
                    _this.SetUIProperties_ManualAction();
                }
                if (!_this.EntityPM.WaitingForResponse) {
                    _this.StopTimer();
                    _this.LoadAnswers();
                    _this.SetUIProperties_ManualAction();
                }
                _this.Wizard.SetButtonsProperties();
                if (_this.Wizard.PageChild_BKD != null) {
                    _this.Wizard.PageChild_BKD.RefreshTab();
                }
                if (_this.Wizard.PageChild_GEN != null) {
                    _this.Wizard.PageChild_GEN.RefreshTab();
                }
                if (_this.Wizard.PageChild_PAC != null) {
                    _this.Wizard.PageChild_PAC.RefreshTab();
                }
                if (_this.Wizard.PageChild_PAR != null) {
                    _this.Wizard.PageChild_PAR.RefreshTab();
                }
            }
        }, function (error) {
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Object.defineProperty(OverviewTabComponent.prototype, "IsSendFSREnabled", {
        //FSR
        get: function () {
            var myResult = true;
            if (this.EntityPM.IsCancelled) {
                myResult = false;
            }
            else if (this.EntityPM.BookingStatusCode == "AWB") {
                myResult = false;
            }
            else {
                if (SessionLocator_1.SessionLocator.TenantManagementJS.AWBMessagesCCSTypeCode == "GLSHK") {
                    if (!this.EntityPM.TenantZeroAirlineGLSHKFSRFSA) {
                        myResult = false;
                    }
                }
                else {
                    if (!this.EntityPM.TenantZeroAirlineChampFSRFSA) {
                        myResult = false;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "LastFSRStatusRequestDate", {
        get: function () { return this.EntityPM.LastFSRStatusRequestDate; },
        enumerable: true,
        configurable: true
    });
    OverviewTabComponent.prototype.SendFSRButtonClicked = function () {
        this.Wizard.FSRRequestMethod();
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], OverviewTabComponent.prototype, "AllLocations", void 0);
    OverviewTabComponent = __decorate([
        core_1.Component({
            selector: 'OverviewTabComponent',
            moduleId: module.id,
            templateUrl: './OverviewTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], OverviewTabComponent);
    return OverviewTabComponent;
}());
exports.OverviewTabComponent = OverviewTabComponent;
var BookingAnswerItem = /** @class */ (function () {
    function BookingAnswerItem(entityPM, fatherComponent, legText) {
        this.fatherComponent = fatherComponent;
        this.legText = legText;
        this.EntityPM = entityPM;
        this.BookingPM = fatherComponent.EntityPM;
    }
    Object.defineProperty(BookingAnswerItem.prototype, "LegText", {
        get: function () { return this.legText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "CarrierId", {
        get: function () { return this.EntityPM.CarrierId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "CarrierName", {
        get: function () { return this.EntityPM.CarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "FlightNumber", {
        get: function () { return this.EntityPM.FlightNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "ETD", {
        get: function () { return this.EntityPM.ETD; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "Origin", {
        get: function () { return this.EntityPM.Origin; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "OriginCountryName", {
        get: function () { return this.EntityPM.OriginCountryName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "OriginCountryCode", {
        get: function () { return this.EntityPM.OriginCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "Destination", {
        get: function () { return this.EntityPM.Destination; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "DestinationCountryName", {
        get: function () { return this.EntityPM.DestinationCountryName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "DestinationCountryCode", {
        get: function () { return this.EntityPM.DestinationCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BookingAnswerItem.prototype, "SpaceAllocationCode", {
        get: function () { return this.EntityPM.BookingSpaceAllocationCode; },
        enumerable: true,
        configurable: true
    });
    return BookingAnswerItem;
}());
exports.BookingAnswerItem = BookingAnswerItem;
//# sourceMappingURL=OverviewTabComponent.js.map