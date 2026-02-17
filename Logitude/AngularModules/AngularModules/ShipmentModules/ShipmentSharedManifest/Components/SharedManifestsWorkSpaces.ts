import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SharedAgentManifestService } from '../../../Shipment/Services/Others/SharedAgentManifestService';
import {FormatTool, DateTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs, UserArgs} from '../../../Infrastructure/Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {DashBoardFilters} from '../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GroupByClass} from '../../../Infrastructure/DataContracts/Dashboard/GroupByClass';
declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    moduleId: module.id,
    selector: 'SharedManifestsWork',
    templateUrl: './SharedManifestsWorkSpaces.html',
    providers: [SharedAgentManifestService],
})

export class SharedManifestsWorkSpaces extends BaseComponent {


    @Output() ReloadAgentSharedManifestsQueries = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    AgentSharedManifestsAirCount: number = 0;
    AgentSharedManifestsOceanCount: number = 0;
    AgentSharedManifestsInlandCount: number = 0;
    DataContext: any = this;

    IsShareDocumentsButtonVisible: boolean = false;
    AgentSharedManifestsAllVisibility: boolean = false;
    AgentSharedManifestsCancelledVisibility: boolean = false;
    AgentSharedManifestsAirVisibility: boolean = false;
    AgentSharedManifestsOceanVisibility: boolean = false;
    AgentSharedManifestsInlandVisibility: boolean = false;

    public SharedManinfestInDashboardId: string = "SharedManinfestDashboardId_";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedAgentManifestService: SharedAgentManifestService) {
        super();
        if (FeatureLocator.HasFeaturePermession("AgentSharedDocument", "NEW")) {
            this.IsShareDocumentsButtonVisible = true;
        }

        this.SharedManinfestInDashboardId = this.SharedManinfestInDashboardId + this.CurrentSession.GetChartId();


    }

    public SearchText: string = "Search";

    InitComponent() {

        this.LoadAllData();
    }

    BuildCustomQueriesList() {
        this.ReloadAgentSharedManifestsQueries.emit();

    }

    EditAgentSharedManifest(item) {

        var windowArgs: any = {};
        windowArgs.CurrentEntity = item;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 830;
        logWindow.Height = 450;
        logWindow.Title = "Shared Manifest";
        logWindow.WindowArgs = windowArgs;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestComponent");

        logWindow.WindowClosed.subscribe(($event1: any) => {



        });

    }

    ViewAgentSharedManifestQuery(myCode: string, nameTextCodeCode: string = null) {

        var queryCode = "";
        var displayTitle = "";
        if (myCode != null) {


            if (nameTextCodeCode) {
                queryCode = myCode;
                displayTitle = TextCodeTranslator.Translate(nameTextCodeCode);
            }


            else {
                switch (myCode) {
                    case "Air":
                        {
                            queryCode = "AirAgentSharedManifests";
                            displayTitle = "Air Agent Shared Manifests";
                            break;
                        }

                    case "Ocean":
                        {
                            queryCode = "OceanAgentSharedManifests";
                            displayTitle = "Ocean Agent Shared Manifests";
                            break;
                        }

                    case "Inland":
                        {
                            queryCode = "InlandAgentSharedManifests";
                            displayTitle = "Inland Agent Shared Manifests";
                            break;
                        }

                    case "Cancelled":
                        {
                            queryCode = "CancelledAgentSharedManifests";
                            displayTitle = "Cancelled Agent Shared Manifests";
                            break;
                        }



                    case "All":
                        {
                            queryCode = "Agent Shared Manifests";
                            displayTitle = "All Agent Shared Manifests";
                            break;
                        }

                }
            }


            var listArgs = new ListComponentArgs();
            listArgs.Filters = new ApiQueryFilters();
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = "AgentSharedManifest";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = "Shared Manifests";
            SessionLocator.DynamicLoader.Load("./Infrastructure/Components/ListComponent/ListComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllData());

                });

        }
    }

    LoadAllData() {

        this.LoadDataSummary();
        this.SetPropertyVisibility();
        this.BuildCustomQueriesList();
        if (!this.SelectedTimeRangeItem) {
            this.FillTimeRangeFilterList();
        } else {
            this.LoadBarQueries();
        }
        
    }

    SetPropertyVisibility() {

        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "AgentSharedManifestQ")) this.AgentSharedManifestsAllVisibility = true;
        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "AirAgentSharedManifestsQ")) this.AgentSharedManifestsAirVisibility = true;
        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "OceanAgentSharedManifestsQ")) this.AgentSharedManifestsOceanVisibility = true;
        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "InlandAgentSharedManifestsQ")) this.AgentSharedManifestsInlandVisibility = true;
        if (FeatureLocator.HasFeaturePermession("AgentSharedManifest", "CancelledAgentSharedManifestsQ")) this.AgentSharedManifestsCancelledVisibility = true;
    }


    LoadDataSummary() {

        this._sharedAgentManifestService.getAgentSharedManifestsWorkspaceSummary().subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.AgentSharedManifestsAirCount = myResult.AgentSharedManifestsAirCount > 1000 ? "1000+" : myResult.AgentSharedManifestsAirCount.toString();
                    this.AgentSharedManifestsOceanCount = myResult.AgentSharedManifestsOceanCount > 1000 ? "1000+" : myResult.AgentSharedManifestsOceanCount.toString();
                    this.AgentSharedManifestsInlandCount = myResult.AgentSharedManifestsInlandCount > 1000 ? "1000+" : myResult.AgentSharedManifestsInlandCount.toString();
                    //this.AgentSharedManifestsAllCount = myResult.AgentSharedManifestsAllCount > 1000 ? "1000+" : myResult.AgentSharedManifestsAllCount.toString();
                    //this.AgentSharedManifestsCancelledCount = myResult.AgentSharedManifestsCancelledCount > 1000 ? "1000+" : myResult.AgentSharedManifestsCancelledCount.toString();

                }
            }
        });
    }

    RefreshButtonClicked() {
        this.LoadAllData();
    }

    onAgentSharedManifestQueriesBackComplete(event) {

    }



    DocumentsPermissionsLinkClick() {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Documents Permissions";
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent");



    }



    //BarData

    public TimeRangeFilterList: DashBoardFilters[];

    private FillTimeRangeFilterList() {

        var list2 = LastFilter.ActivitymyList();
        this.TimeRangeFilterList = [];

        this.TimeRangeFilterList.push(new DashBoardFilters(list2[0].lastTitle, "0"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list2[1].lastTitle, "1"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list2[2].lastTitle, "2"));
        this.TimeRangeFilterList.push(new DashBoardFilters(list2[3].lastTitle, "3"));
        this.SelectedTimeRangeItem = this.TimeRangeFilterList[1];

    }


    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (value != this.fromDate) {
            this.fromDate = value;
            // this.LoadBarQueries();

        }
    }

    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (value != this.toDate) {
            this.toDate = value;
            //   this.LoadBarQueries();
        }
    }


    private selectedTimeRangeItem: DashBoardFilters;
    get SelectedTimeRangeItem() { return this.selectedTimeRangeItem; }
    set SelectedTimeRangeItem(value: DashBoardFilters) {
        if (this.selectedTimeRangeItem != value) {
            this.selectedTimeRangeItem = value;


            this.LoadBarQueries();
        }
    }


    private ComputeDays() {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
      
            this.fromDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
            if (this.SelectedTimeRangeItem.Index == "0") {
                days = -7;
                Todate.setDate(Todate.getDate() - 6);
                this.toDate = Todate;
            }

            else if (this.SelectedTimeRangeItem.Index == "1") {
                days = -30;
                Todate.setMonth(Todate.getMonth() - 1);
                this.toDate = Todate;
            }

            else if (this.SelectedTimeRangeItem.Index == "2") {
                days = -90;
                Todate.setMonth(Todate.getMonth() - 3);
                this.toDate = Todate;
            }

            else if (this.SelectedTimeRangeItem.Index == "3") {
                days = -365;
                Todate.setMonth(Todate.getMonth() - 12);
                this.toDate = Todate;
            }


        return days;
    }





    public BarData: any;
    public barChartLabels: string[] = [];
    public barChartData: any[] = [{ data: [], label: '', dateRange: [] }, { data: [], label: '', dateRange: [] }, { data: [], label: '', dateRange: []}];
    LoadBarQueries() {
        var days = this.ComputeDays();
        this.BarData = [];

        this._sharedAgentManifestService.GetAgentSharedManifestsForDashBoard(0, days, +this.SelectedTimeRangeItem.Index).subscribe(myResult => {

            this.BarData = myResult.Result;
            var groupedData: GroupByClass[] = [];
            //this.BarData.sort((a, b) => { return (a.Date === b.Date) ? 0 : (a.Date < b.Date) ? -1 : 1 });
            this.BarData.forEach(item => {
                var existsedItem = groupedData.filter(f => f.DateRange == item.DateRange && f.DataType == item.DataType)[0];
                if (existsedItem == null) {
                    existsedItem = new GroupByClass();
                    existsedItem.XField = item.DateRange;
                    existsedItem.DataType = item.DataType;
                    if (item.TotalAmount == null)
                        existsedItem.YField = 0;
                    else
                        existsedItem.YField = parseInt(item.TotalAmount + "");

                    existsedItem.dateRange = item.DateRange;

                    groupedData.push(existsedItem);
                }

                else {

                    if (item.TotalAmount == null)
                        existsedItem.YField += 0;
                    else
                        existsedItem.YField += parseInt(item.TotalAmount + "");

                }
            });
            this.BarData = groupedData;
            this.FillBars();
        });
    }
    

    FillBars() {
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        this.barChartData[0].data = [];
        this.barChartData[1].data = [];
        this.barChartData[2].data = []
        this.barChartLabels = [];

        var max = 0;
        var even = 0;
        this.BarData.forEach(element => {
            if (i == 0) {
                Graphs = [{
                    "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                    "fillAlphas": 1,
                    "id": "AmGraph-1" + i,

                    "title": "A",
                    "type": "column",
                    "valueField": "col1",
                    "fillColors": ["#487E9F", "#c8d8e2"],
                    "lineAlpha": 0,

                    "gradientOrientation": "horizontal",
                    "borderAlpha": 0,
                    "showHandOnHover": true,

                },
                    {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": "O",
                        "type": "column",
                        "lineAlpha": 0,

                        "valueField": "col2",
                        "fillColors": ["#DA7B38", "#ecbd9b"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                    },
                    {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-3" + i,
                        "title": "I",
                        "type": "column",
                        "lineAlpha": 0,

                        "valueField": "col3",
                        "fillColors": ["#21782E", "#90bb96"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,

                    }
                ];
            }

            if (element.DataType == 'A') {
                this.barChartData[0].label = element.DataType;
                this.barChartData[0].data[i] = element.YField;
                this.barChartData[0].dateRange[i] = element.DateRange;
           



            }
            else if (element.DataType == 'O') {
                this.barChartData[1].label = element.DataType;
                this.barChartData[1].data[i] = element.YField;
                this.barChartData[1].dateRange[i] = element.DateRange;
            }
            else if (element.DataType == 'I') {
                this.barChartData[2].label = element.DataType;
                this.barChartData[2].data[i] = element.YField;
                this.barChartData[2].dateRange[i] = element.DateRange;
            }


            if (!this.barChartLabels.includes(element.XField)) {
                this.barChartLabels[i] = element.XField;

            }
            even++;
            if (even % 3 == 0)
                i++;

            if (element.YField > max)
                max = element.YField;

        });

        var i = 0;
        this.barChartLabels.forEach(item => {
            DataProvider[i] = { "category": this.barChartLabels[i], "col1": this.barChartData[0].data[i], "col2": this.barChartData[1].data[i], "col3": this.barChartData[2].data[i] };
            i++;
        });

        makeAmBarChart(this.SharedManinfestInDashboardId, Graphs, DataProvider, max, null, null, 0);
    } 

    BarClicking() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }
    }
    OnBarClick(e) {
        var flag = false;
        var displayName: string = "";
        let item: any;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;

        if (item && item.values && item.values.value != 0) {
            this.filterAgrs = new ApiQueryFilters();
            var Key = e.target.columnIndex;
            var category: string = item.category;
            var values: string = item.values;

            if (flag) {
                var typeName = "";
                var typeCode = "";
                switch (Key + "") {
                    case "0": { typeName = "Air"; typeCode = "A"; break; }
                    case "1": { typeName = "Ocean"; typeCode = "O"; break; }
                    case "2": { typeName = "Inland"; typeCode = "I"; break; }
                }


                var date: any = this.barChartData[Key].dateRange[item.index];
                var customFilterValue: any = date + "@" + this.SelectedTimeRangeItem.Index;
                this.filterAgrs.addAdditionalFilter("BarDataCustomFilter", customFilterValue, null, null, "Equals", true, false, false, "date");
                
                var queryCode: string = "";
                var displayTitle: string = "";
                switch (typeName) {
                    case "Air":
                        {
                            queryCode = "AirAgentSharedManifests";
                            displayTitle = "Air Agent Shared Manifests";
                            break;
                        }

                    case "Ocean":
                        {
                            queryCode = "OceanAgentSharedManifests";
                            displayTitle = "Ocean Agent Shared Manifests";
                            break;
                        }

                    case "Inland":
                        {
                            queryCode = "InlandAgentSharedManifests";
                            displayTitle = "Inland Agent Shared Manifests";
                            break;
                        }

                }

                var listArgs = new ListComponentArgs();
                listArgs.Filters = this.filterAgrs;
                listArgs.QueryCode = queryCode;
                listArgs.ObjectTableName = "AgentSharedManifest";
                listArgs.DisplayTitle = displayTitle;
                listArgs.BackButtonTitle = "Shared Manifests";
                SessionLocator.DynamicLoader.Load("./Infrastructure/Components/ListComponent/ListComponent", this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllData());

                    });


            }


        }













    }

}

