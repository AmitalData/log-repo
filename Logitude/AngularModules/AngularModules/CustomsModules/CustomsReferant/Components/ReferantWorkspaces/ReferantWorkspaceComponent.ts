import { Component, OnDestroy, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationReferantDataWebService } from '../../../../Customs/Services/WebServices/DeclarationReferantDataWebService';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { FormatTool } from '../../../../Infrastructure/Tools';
import { DeclarationReferantDataExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationReferantDataExtendedListService';
declare var makeAmBarChart, BarClick, ResetItem: any;

@Component({
    templateUrl: './ReferantWorkspaceComponent.html',
    providers: [DeclarationReferantDataWebService],
})

export class ReferantWorkspaceComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();

    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public counters: any;

    // Queries Features
    public FilesInProcessVisibility: boolean = true;
    public TrackingCasesVisibility: boolean = true;
    public FilesInOCRVisibility: boolean = true;
    public FilesInSivugVisibility: boolean = true;
    public FilesInReviewVisibility: boolean = true;
    public FilesInCreditControlVisibility: boolean = true;
    public FilesAvailableFreeOfChargeVisibility: boolean = true;
    public AllCasesVisibility: boolean = true;
    public isRTL: boolean = false;
    public ChartID: string = null;
    public InProgressDeclarationReferantDataId: string = "InProgressDeclarationReferantDataId_";
    public InProgressDeclarationReferantDataDashboard: Array<ChartingDataClass>;
    public InProgressDeclarationReferantDataYAxis: any[] = [];
    public InProgressDeclarationReferantDataYAxisFilterd = [];
    public InProgressDeclarationReferantDataXAxis: string[] = [];
    public filterAgrs: ApiQueryFilters;


    barChartColors: any[] = [
        {
            backgroundColor1: '#599DDB',
            backgroundColor2: '#c8d8e2',
            borderWidth: 0
        },

        {
            backgroundColor1: '#F27824',
            backgroundColor2: '#ecbd9b',
            borderWidth: 0,
        },
 
    ]


    ngAfterViewInit(): void {
    }
    constructor(public _declarationReferantDataWebService: DeclarationReferantDataWebService ) {
        this.CurrentSession.StartBusyIndicatorLoading();
        _declarationReferantDataWebService.GetQueriesCounts().subscribe(
            (data: any) => {
                this.counters = data.Result;
                this.isScreenLoaded = true;
                this.CurrentSession.StopBusyIndicator();

                if (this.CurrentSession == null) {
                    this.ChartID = "ChartID_-1_-1";
                }

                else {
                    this.ChartID = "ChartID_" + this.CurrentSession.GetChartId();
                }

                this.InProgressDeclarationReferantDataId = this.InProgressDeclarationReferantDataId + this.CurrentSession.GetChartId();
                this.LoadInProgressDeclarationReferantDatasDashboard();
            });

    }

    BarClicking() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }

    }
    LoadInProgressDeclarationReferantDatasDashboard() {

        this.InProgressDeclarationReferantDataDashboard = new Array<ChartingDataClass>();

        var ele1 = new ChartingDataClass();
        ele1.Id = "FilesInProcess";
        ele1.StringProperty = "תיקים בתהליך";
        ele1.IntegerProperty = this.counters.FilesInProcess;
        ele1.DataTypeCode = "FilesInProcess";
        this.InProgressDeclarationReferantDataDashboard.push(ele1);

        ele1 = new ChartingDataClass();
         ele1.Id = "FilesInProcess_1";
        ele1.StringProperty = "תיקים בתהליך";
        ele1.IntegerProperty = this.counters.FilesInProcess;
        ele1.DataTypeCode = "FilesInProcess1";
        this.InProgressDeclarationReferantDataDashboard.push(ele1);

        ele1 = new ChartingDataClass();
        ele1.Id = "TrackingCases";
        ele1.StringProperty = "תיקים XXX";
        ele1.IntegerProperty = this.counters.FilesInProcess;
        ele1.DataTypeCode = "TrackingCases";
        this.InProgressDeclarationReferantDataDashboard.push(ele1);

            this.FillInProgressDeclarationReferantDataDashboardData();
 

        //this._declarationReferantDataWebService.GetDeclarationReferantDataDashBoard(SessionLocator.TenantPM.Id).subscribe((myResult: any) => {
        //    this.InProgressDeclarationReferantDataDashboard = new Array<ChartingDataClass>();
        //    var myResponse: ServiceResponse = myResult;
        //    this.InProgressDeclarationReferantDataDashboard = myResponse.Result;
        //    this.FillInProgressDeclarationReferantDataDashboardData();
        //});
    }


    FillInProgressDeclarationReferantDataDashboardData() {
        var index = 0;
        this.InProgressDeclarationReferantDataXAxis = [];

        this.InProgressDeclarationReferantDataDashboard.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
        var StringArr: Array<string> = new Array<string>();
        var j = 0;

        this.InProgressDeclarationReferantDataDashboard.forEach(element => {
            if (!StringArr.includes(element.StringProperty) && element.StringProperty != null) {
                StringArr.push(element.StringProperty);
                this.InProgressDeclarationReferantDataYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [] };
                this.InProgressDeclarationReferantDataYAxis[j].data = [];
                j++;
            }
        });

        StringArr.sort((a, b) => { return (a === b) ? 0 : (a < b) ? -1 : 1 });
        var Graphs = [];
        var index = 0;
        this.InProgressDeclarationReferantDataDashboard.forEach(element => {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    this.InProgressDeclarationReferantDataYAxis[i].data.push(element.IntegerProperty);
                    this.InProgressDeclarationReferantDataYAxis[i].label = element.DataTypeCode;
                    this.InProgressDeclarationReferantDataYAxis[i].BindingElement.push(element.DataTypeCode);
                    this.InProgressDeclarationReferantDataYAxis[i].OwnerIds.push(element.OwnerId);
                    if (!this.InProgressDeclarationReferantDataXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (this.InProgressDeclarationReferantDataXAxis[i] == null)
                            this.InProgressDeclarationReferantDataXAxis[i] = (element.StringProperty);

                    }
                }
            }
        });

        this.InProgressDeclarationReferantDataYAxisFilterd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (this.InProgressDeclarationReferantDataYAxis.length > 0)
            maximum = this.InProgressDeclarationReferantDataYAxis[0].data[0];
        this.InProgressDeclarationReferantDataYAxis.forEach(element => {
            for (var i = 0; i < element.data.length; i++) {
                if (this.InProgressDeclarationReferantDataYAxisFilterd[i] == null) {
                    this.InProgressDeclarationReferantDataYAxisFilterd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                this.InProgressDeclarationReferantDataYAxisFilterd[i].data.push(element.data[i]);
                this.InProgressDeclarationReferantDataYAxisFilterd[i].BindingElement.push(element.BindingElement[i]);
                this.InProgressDeclarationReferantDataYAxisFilterd[i].OwnerIds.push(element.OwnerIds[i]);
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
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
            DataProvider[index] = { "category": this.InProgressDeclarationReferantDataXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2], "col4": objectArray[3] };
            index++;
        });

        var InProgressDeclarationReferantDataDashboardFilterd: Array<ChartingDataClass> = new Array<ChartingDataClass>();
        try {
            if (this.InProgressDeclarationReferantDataXAxis.length != 0) {

                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;

                }
                makeAmBarChart(this.InProgressDeclarationReferantDataId, Graphs, DataProvider, maximum);
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
        var myTableName: string = "DeclarationReferantData";
        this.filterAgrs = new ApiQueryFilters();
        if (flag) {
            switch (Key + "") {
                case "0":
                    {
                        displayName = "Waiting for Transmission";
                        myQueryCode = "CreatedDeclarationReferantDatas";
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
                        myQueryCode = "ConfirmedDeclarationReferantDatas";
                        break;
                    }

                case "3":
                    {
                        displayName = "Errors and Rejections";
                        myQueryCode = "RejectedDeclarationReferantDatas";
                        break;
                    }
            }

            this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
            this.filterAgrs.addAdditionalFilter("MainCarriageCarrierId", this.InProgressDeclarationReferantDataYAxisFilterd[e.target.columnIndex].OwnerIds[e.index], null, null, "Equals", false, false, false, "String");

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


    ViewReferantQuery(myQueryCode: string) {
        if (myQueryCode != null) {
            var displayTitle = "";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "FilesInProcess":
                    {
                        displayTitle = "hello";
                        break;
                    }
                case "TrackingCases":
                    {
                        displayTitle = "hello2";
                        break;
                    }
                case "FilesInOCR":
                    {
                        displayTitle = "hello";

                        break;
                    }
                case "FilesInSivug":
                    {
                        displayTitle = "hello";
                        break;
                    }
                case "FilesInReview":
                    {
                        displayTitle = "hello";

                        break;
                    }
                case "FilesInCreditControl":
                    {
                        displayTitle = "hello";

                        break;
                    }
                case "FilesAvailableFreeOfCharge":
                    {
                        displayTitle = "hello";

                        break;
                    }
                case "AllCases":
                    {
                        displayTitle = "AllCases";

                        break;
                    }
                default: { break;}
            }
            this.BuildFiltersForQuery(filters);
            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Customs.DeclarationReferantData";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.ReferantWorkspace");
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.LoadAllScreenData();
                            this._declarationReferantDataWebService.GetQueriesCounts().subscribe(
                                (data: any) => {
                                    this.counters = data.Result;
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                });
                        });
                    });
            });
        }
    }

    public LoadAllScreenData() {
        this.ReloadUsersQuery();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }
    BuildFiltersForQuery(filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
    }

}

