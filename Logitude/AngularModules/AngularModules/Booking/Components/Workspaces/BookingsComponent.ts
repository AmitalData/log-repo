import {Component, Output, EventEmitter} from '@angular/core';
import {BookingDomainService, BookingsDataCounts} from '../../Services/BookingDomainService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {BookingPM} from '../../EntityPMs/BookingPM';
import {BookingList} from '../../EntityLists/BookingList';
import {BookingWizardArgs} from '../../Args';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {FlightsSchedulesArgs} from '../../../CommonModules/CommonFlightsSchedules/Args';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ChartingDataClass} from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {FormatTool} from '../../../Infrastructure/Tools';
declare var makeAmBarChart, BarClick, ResetItem: any;
@Component({
    moduleId: module.id,
    templateUrl: './BookingsComponent.html',
})

export class BookingsComponent {
    private myBookingDomainService: BookingDomainService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    public InProgressBookingYAxis: any[] = [];
    public InProgressBookingYAxisFilterd = [];
    public InProgressBookingXAxis: string[] = [];
    public InProgressBookingId: string = "InProgressBookingId_";
    public filterAgrs: ApiQueryFilters;
    private ComponentRef: any = null;
    @Output() ReloadUserQueries = new EventEmitter();
    public ChartID: string = null;
    public InProgressBookingDashboard: Array<ChartingDataClass>;
    public IsFlightsSchedulesVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myBookingDomainService = new BookingDomainService();
        if (this.CurrentSession == null) {
            this.ChartID = "ChartID_-1_-1";
        }

        else {
            this.ChartID = "ChartID_" + this.CurrentSession.GetChartId();
        }

        this.InProgressBookingId = this.InProgressBookingId + this.CurrentSession.GetChartId();

        if (FeatureLocator.HasFeaturePermession("General", "FlightsSchedules")) {
            this.IsFlightsSchedulesVisible = true;
        }
    }

    public IsQueryVisible_MyViewsGroup: boolean = false;
    InitComponent() {
        this.LoadAllScreenData();
        this.IsQueryVisible_MyViewsGroup = FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.LoadRecentBookings();
        this.LoadInProgressBookingsDashboard();
        this.ReloadUsersQuery();
    }

    ClickOnQuiery() {
        //if (this.ComponentRef != null)
        //    this.ComponentRef.destroy();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    public WaitingForTransmissionCount: string;
    public WaitingForConfirmationCount: string;
    public ConfirmedCount: string;
    public ErrorsCount: string;
    public InProgressCount: string;
    public AllBookingsCount: string;
    public CancelledBookingsCount: string;
    public ErrorsQueryColor: string = "#282E30";
    LoadQueriesCounts() {
        this.myBookingDomainService.GetBookingsCounts().subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                var myData: BookingsDataCounts = myResponse.Result;
                if (myData != null) {
                    this.WaitingForTransmissionCount = myData.CreatedBookingsCount > 1000 ? "1000+" : myData.CreatedBookingsCount.toString();
                    this.WaitingForConfirmationCount = myData.WaitingForResponseCount > 1000 ? "1000+" : myData.WaitingForResponseCount.toString();
                    this.ConfirmedCount = myData.ConfirmedBookingsCount > 1000 ? "1000+" : myData.ConfirmedBookingsCount.toString();
                    this.ErrorsCount = myData.RejectedBookingsCount > 1000 ? "1000+" : myData.RejectedBookingsCount.toString();
                    this.InProgressCount = myData.InProgressBookingsCount > 1000 ? "1000+" : myData.InProgressBookingsCount.toString();
                    this.AllBookingsCount = myData.AllBookingsCount >= 1000 ? "1000+" : myData.AllBookingsCount.toString();
                    this.CancelledBookingsCount = myData.CancelledBookingsCount > 1000 ? "1000+" : myData.CancelledBookingsCount.toString();

                    if (this.ErrorsCount != "0") {
                        this.ErrorsQueryColor = "#FF0000";
                    }
                }
            }
        });
    }

    public RecentBookingsCount: number = 0;
    public RecentBookingsList: BookingList[] = [];
    LoadRecentBookings() {
        this.myBookingDomainService.GetRecentBookings().subscribe(myResult => {
            if (myResult == null) {
                this.RecentBookingsList = [];
                this.RecentBookingsCount = 0;
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.RecentBookingsList = myResponse.Result;
                    this.RecentBookingsCount = this.RecentBookingsList.length;
                }
            }
        });
    }

    BarClicking() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }

    }
    EditBooking(entity: any) {
        if (entity != null) {
            //if (this.ComponentRef != null)
            //    this.ComponentRef.destroy();

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = "Edit Booking Wizard";
            logWindow.WindowArgs = entity.Id;
            logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
            logWindow.Show('./Booking/Components/BookingWizard/BookingWizardLoadComponent');
        }
    }

    ViewBookingQuery(myQueryCode: string) {
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

                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = "Booking";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Operations";
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                        //if (this.ComponentRef != null)
                        //   this.ComponentRef.destroy();
                    });
            });
        }
    }

    OnBackFromList() {
        this.LoadAllScreenData();
    }

    LoadInProgressBookingsDashboard() {
        this.myBookingDomainService.GetBookingsDashBoard(SessionLocator.TenantPM.Id).subscribe(myResult => {
            this.InProgressBookingDashboard = new Array<ChartingDataClass>();
            var myResponse: ServiceResponse = myResult;
            this.InProgressBookingDashboard = myResponse.Result;
            this.FillInProgressBookingDashboardData();
        });
    }

    FillInProgressBookingDashboardData() {
        var index = 0;
        this.InProgressBookingXAxis = [];

        this.InProgressBookingDashboard.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
        var StringArr: Array<string> = new Array<string>();
        var j = 0;

        this.InProgressBookingDashboard.forEach(element => {
            if (!StringArr.includes(element.StringProperty) && element.StringProperty != null) {
                StringArr.push(element.StringProperty);
                this.InProgressBookingYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [] };
                this.InProgressBookingYAxis[j].data = [];
                j++;
            }
        });

        StringArr.sort((a, b) => { return (a === b) ? 0 : (a < b) ? -1 : 1 });
        var Graphs = [];
        var index = 0;
        this.InProgressBookingDashboard.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    this.InProgressBookingYAxis[i].data.push(element.IntegerProperty);
                    this.InProgressBookingYAxis[i].label = element.DataTypeCode;
                    this.InProgressBookingYAxis[i].BindingElement.push(element.DataTypeCode);
                    this.InProgressBookingYAxis[i].OwnerIds.push(element.OwnerId);
                    if (!this.InProgressBookingXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (this.InProgressBookingXAxis[i] == null)
                            this.InProgressBookingXAxis[i] = (element.StringProperty);

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
        this.InProgressBookingYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.InProgressBookingYAxisFilterd[i] == null) {
                    this.InProgressBookingYAxisFilterd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.InProgressBookingYAxisFilterd[i].data.push(element.data[i]);
                this.InProgressBookingYAxisFilterd[i].BindingElement.push(element.BindingElement[i]);
                this.InProgressBookingYAxisFilterd[i].OwnerIds.push(element.OwnerIds[i]);
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]")+"",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [this.barChartColors[i].backgroundColor1 + "", this.barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                        //   "plotAreaFillColors": ["#ff0000", "#f1783e", "#00ff00"],
                    };
                }
                objectArray[i] = (element.data[i]);

            }
            DataProvider[index] = { "category": this.InProgressBookingXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2], "col4": objectArray[3] };
            index++;
        });

        var InProgressBookingDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();        
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
    }

    isResizing: boolean = false;
    ChartLeft: number = 0;
    lastDownX: number = 0;
    lastDownY: number = 0;

    OnMyMouseDown($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.ChartID);
        var rec = grid.getBoundingClientRect();
        this.ChartLeft = rec.left;
        this.lastDownY = ($event.clientY - rec.bottom);
        this.lastDownX = ($event.clientX - this.ChartLeft);
    }
    OnBarClick(e) {
        var flag = false;
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode: string = "";
        var displayName: string = "";
        var myTableName: string = "Booking";
        this.filterAgrs = new ApiQueryFilters();     
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

            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "Operations";

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }

    barChartColors: any[] = [
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
    ]

    RunBookingWizard() {
        var windowTitle = "New Booking Wizard";
        var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;

        logWindow.WindowClosed.subscribe(s => {
            this.LoadAllScreenData();
        });

        logWindow.Show('./Booking/Components/BookingWizard/BookingWizardComponent');
    }

    RunSchedulesControl() {
        var args = new FlightsSchedulesArgs();
        var logWindow = new LogitudeWindow();
        logWindow.Width = 920;
        logWindow.Height = 530;
        logWindow.WindowArgs = args;
        logWindow.Title = "Flight Schedules / Availability";
        this._entityResourceService.getEntityResourceByTableName("FlightsSchedulesRequest").subscribe(response => {
            logWindow.Show('./CommonModules/CommonFlightsSchedules/Components/FlightsSchedules/FlightsSchedulesComponent');
        });
    }
}

export class GraphData {

    public label: string;
    public data: Array<any> = new Array<any>();

    constructor() {


    }
}
