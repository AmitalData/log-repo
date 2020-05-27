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
var BookingDomainService_1 = require("../../Services/BookingDomainService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../Args");
var Args_2 = require("../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var Args_3 = require("../../../CommonModules/CommonFlightsSchedules/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Tools_1 = require("../../../Infrastructure/Tools");
var BookingsComponent = /** @class */ (function () {
    function BookingsComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.InProgressBookingYAxis = [];
        this.InProgressBookingYAxisFilterd = [];
        this.InProgressBookingXAxis = [];
        this.InProgressBookingId = "InProgressBookingId_";
        this.ComponentRef = null;
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.ChartID = null;
        this.IsFlightsSchedulesVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsQueryVisible_MyViewsGroup = false;
        this.ErrorsQueryColor = "#282E30";
        this.RecentBookingsCount = 0;
        this.RecentBookingsList = [];
        this.isResizing = false;
        this.ChartLeft = 0;
        this.lastDownX = 0;
        this.lastDownY = 0;
        this.barChartColors = [
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',
                borderWidth: 0
            },
            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',
                borderWidth: 0,
            },
            {
                backgroundColor1: '#21782E',
                backgroundColor2: '#90bb96',
                borderWidth: 0,
            },
            {
                backgroundColor1: '#FF0000',
                backgroundColor2: '#ff7f7f',
                borderWidth: 0,
            }
        ];
        this.myBookingDomainService = new BookingDomainService_1.BookingDomainService();
        if (this.CurrentSession == null) {
            this.ChartID = "ChartID_-1_-1";
        }
        else {
            this.ChartID = "ChartID_" + this.CurrentSession.GetChartId();
        }
        this.InProgressBookingId = this.InProgressBookingId + this.CurrentSession.GetChartId();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "FlightsSchedules")) {
            this.IsFlightsSchedulesVisible = true;
        }
    }
    BookingsComponent.prototype.InitComponent = function () {
        this.LoadAllScreenData();
        this.IsQueryVisible_MyViewsGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    };
    BookingsComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    BookingsComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
        this.LoadRecentBookings();
        this.LoadInProgressBookingsDashboard();
        this.ReloadUsersQuery();
    };
    BookingsComponent.prototype.ClickOnQuiery = function () {
        //if (this.ComponentRef != null)
        //    this.ComponentRef.destroy();
    };
    BookingsComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    BookingsComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    BookingsComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myBookingDomainService.GetBookingsCounts().subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.WaitingForTransmissionCount = myData.CreatedBookingsCount > 1000 ? "1000+" : myData.CreatedBookingsCount.toString();
                    _this.WaitingForConfirmationCount = myData.WaitingForResponseCount > 1000 ? "1000+" : myData.WaitingForResponseCount.toString();
                    _this.ConfirmedCount = myData.ConfirmedBookingsCount > 1000 ? "1000+" : myData.ConfirmedBookingsCount.toString();
                    _this.ErrorsCount = myData.RejectedBookingsCount > 1000 ? "1000+" : myData.RejectedBookingsCount.toString();
                    _this.InProgressCount = myData.InProgressBookingsCount > 1000 ? "1000+" : myData.InProgressBookingsCount.toString();
                    _this.AllBookingsCount = myData.AllBookingsCount >= 1000 ? "1000+" : myData.AllBookingsCount.toString();
                    _this.CancelledBookingsCount = myData.CancelledBookingsCount > 1000 ? "1000+" : myData.CancelledBookingsCount.toString();
                    if (_this.ErrorsCount != "0") {
                        _this.ErrorsQueryColor = "#FF0000";
                    }
                }
            }
        });
    };
    BookingsComponent.prototype.LoadRecentBookings = function () {
        var _this = this;
        this.myBookingDomainService.GetRecentBookings().subscribe(function (myResult) {
            if (myResult == null) {
                _this.RecentBookingsList = [];
                _this.RecentBookingsCount = 0;
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.RecentBookingsList = myResponse.Result;
                    _this.RecentBookingsCount = _this.RecentBookingsList.length;
                }
            }
        });
    };
    BookingsComponent.prototype.BarClicking = function () {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }
    };
    BookingsComponent.prototype.EditBooking = function (entity) {
        var _this = this;
        if (entity != null) {
            //if (this.ComponentRef != null)
            //    this.ComponentRef.destroy();
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = "Edit Booking Wizard";
            logWindow.WindowArgs = entity.Id;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadAllScreenData(); });
            logWindow.Show('./Booking/Components/BookingWizard/BookingWizardLoadComponent');
        }
    };
    BookingsComponent.prototype.ViewBookingQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            var displayTitle = "";
            switch (myQueryCode) {
                case "CreatedBookings":
                    {
                        displayTitle = "Created Bookings";
                        break;
                    }
                case "WatingForResponse":
                    {
                        displayTitle = "Waiting for Response";
                        break;
                    }
                case "ConfirmedBookings":
                    {
                        displayTitle = "Confirmed without Shipments";
                        break;
                    }
                case "RejectedBookings":
                    {
                        displayTitle = "Errors and Rejections";
                        break;
                    }
                case "InProgressBookings":
                    {
                        displayTitle = "In Progress";
                        break;
                    }
                case "AllBookings":
                    {
                        displayTitle = "All Bookings";
                        break;
                    }
                case "CancelledBookings":
                    {
                        displayTitle = "Cancelled Bookings";
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_2.ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = "Booking";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Operations";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                    //if (this.ComponentRef != null)
                    //   this.ComponentRef.destroy();
                });
            });
        }
    };
    BookingsComponent.prototype.OnBackFromList = function () {
        this.LoadAllScreenData();
    };
    BookingsComponent.prototype.LoadInProgressBookingsDashboard = function () {
        var _this = this;
        this.myBookingDomainService.GetBookingsDashBoard(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (myResult) {
            _this.InProgressBookingDashboard = new Array();
            var myResponse = myResult;
            _this.InProgressBookingDashboard = myResponse.Result;
            _this.FillInProgressBookingDashboardData();
        });
    };
    BookingsComponent.prototype.FillInProgressBookingDashboardData = function () {
        var _this = this;
        var index = 0;
        this.InProgressBookingXAxis = [];
        this.InProgressBookingDashboard.sort(function (a, b) { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1; });
        var StringArr = new Array();
        var j = 0;
        this.InProgressBookingDashboard.forEach(function (element) {
            if (!StringArr.includes(element.StringProperty) && element.StringProperty != null) {
                StringArr.push(element.StringProperty);
                _this.InProgressBookingYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [] };
                _this.InProgressBookingYAxis[j].data = [];
                j++;
            }
        });
        StringArr.sort(function (a, b) { return (a === b) ? 0 : (a < b) ? -1 : 1; });
        var Graphs = [];
        var index = 0;
        this.InProgressBookingDashboard.forEach(function (element) {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    _this.InProgressBookingYAxis[i].data.push(element.IntegerProperty);
                    _this.InProgressBookingYAxis[i].label = element.DataTypeCode;
                    _this.InProgressBookingYAxis[i].BindingElement.push(element.DataTypeCode);
                    _this.InProgressBookingYAxis[i].OwnerIds.push(element.OwnerId);
                    if (!_this.InProgressBookingXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (_this.InProgressBookingXAxis[i] == null)
                            _this.InProgressBookingXAxis[i] = (element.StringProperty);
                    }
                }
            }
        });
        this.InProgressBookingYAxisFilterd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (this.InProgressBookingYAxis.length > 0)
            maximum = this.InProgressBookingYAxis[0].data[0];
        this.InProgressBookingYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.InProgressBookingYAxisFilterd[i] == null) {
                    _this.InProgressBookingYAxisFilterd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.InProgressBookingYAxisFilterd[i].data.push(element.data[i]);
                _this.InProgressBookingYAxisFilterd[i].BindingElement.push(element.BindingElement[i]);
                _this.InProgressBookingYAxisFilterd[i].OwnerIds.push(element.OwnerIds[i]);
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [_this.barChartColors[i].backgroundColor1 + "", _this.barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    };
                }
                objectArray[i] = (element.data[i]);
            }
            DataProvider[index] = { "category": _this.InProgressBookingXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2], "col4": objectArray[3] };
            index++;
        });
        var InProgressBookingDashboardFilterd = new Array();
        try {
            if (this.InProgressBookingXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.InProgressBookingId, Graphs, DataProvider, maximum);
            }
        }
        catch (e) {
        }
    };
    BookingsComponent.prototype.OnMyMouseDown = function ($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.ChartID);
        var rec = grid.getBoundingClientRect();
        this.ChartLeft = rec.left;
        this.lastDownY = ($event.clientY - rec.bottom);
        this.lastDownX = ($event.clientX - this.ChartLeft);
    };
    BookingsComponent.prototype.OnBarClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "";
        var displayName = "";
        var myTableName = "Booking";
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (flag) {
            switch (Key + "") {
                case "0":
                    {
                        displayName = "Waiting for Transmission";
                        myQueryCode = "CreatedBookings";
                        break;
                    }
                case "1":
                    {
                        displayName = "Waiting for Airline Confirmation";
                        myQueryCode = "WatingForResponse";
                        break;
                    }
                case "2":
                    {
                        displayName = "Confirmed Without Shipment";
                        myQueryCode = "ConfirmedBookings";
                        break;
                    }
                case "3":
                    {
                        displayName = "Errors and Rejections";
                        myQueryCode = "RejectedBookings";
                        break;
                    }
            }
            this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("MainCarriageCarrierId", this.InProgressBookingYAxisFilterd[e.target.columnIndex].OwnerIds[e.index], null, null, "Equals", false, false, false, "String");
            var listArgs = new Args_2.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "Operations";
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    BookingsComponent.prototype.RunBookingWizard = function () {
        var _this = this;
        var windowTitle = "New Booking Wizard";
        var windowArgs = new Args_1.BookingWizardArgs();
        windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function (s) {
            _this.LoadAllScreenData();
        });
        logWindow.Show('./Booking/Components/BookingWizard/BookingWizardComponent');
    };
    BookingsComponent.prototype.RunSchedulesControl = function () {
        var args = new Args_3.FlightsSchedulesArgs();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = args;
        logWindow.Title = "Flight Schedules / Availability";
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(function (response) {
            logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FlightsSchedulesComponent');
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], BookingsComponent.prototype, "ReloadUserQueries", void 0);
    BookingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BookingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BookingsComponent);
    return BookingsComponent;
}());
exports.BookingsComponent = BookingsComponent;
var GraphData = /** @class */ (function () {
    function GraphData() {
        this.data = new Array();
    }
    return GraphData;
}());
exports.GraphData = GraphData;
//# sourceMappingURL=BookingsComponent.js.map