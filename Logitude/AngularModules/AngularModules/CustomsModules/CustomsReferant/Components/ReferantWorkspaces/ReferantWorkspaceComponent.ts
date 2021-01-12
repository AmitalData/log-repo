import { Component, OnDestroy, AfterViewInit, Output, EventEmitter, ViewChild} from '@angular/core';
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
import { Dictionary } from '../../../../Infrastructure/GenericTypes/Dictionary';
import { DeclarationReferantDataFiltersMenuComponent } from '../FiltersMenu/DeclarationReferantDataFiltersMenuComponent';
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
    public FilesInAllInclusiveVisibility: boolean = true;
    public FilesRejectedByControllerVisibility: boolean = true;
    public FilesRejectedByClassificationVisibility: boolean = true;
    public AllCasesVisibility: boolean = true;
    public isRTL: boolean = false;
    public ChartID: string = null;
    public InProgressDeclarationReferantDataId: string = "InProgressDeclarationReferantDataId_";
    public InProgressDeclarationReferantDataDashboard: Array<ChartingDataClass>;
    public InProgressDeclarationReferantDataYAxis: any[] = [];
    public InProgressDeclarationReferantDataYAxisFilterd = [];
    public InProgressDeclarationReferantDataXAxis: string[] = [];
    public filterAgrs: ApiQueryFilters;
    public RefId: string;
    public DepId: string;
    public TransportModeId: string;

    dec_queries: Dictionary<string>;

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

    @ViewChild(DeclarationReferantDataFiltersMenuComponent) declarationReferantDataFiltersMenuComponent: DeclarationReferantDataFiltersMenuComponent;

    setFilters() {
        this.RefId = (SessionLocator.LoggedUserPM.Id == null || SessionLocator.LoggedUserPM.Id == "0") ? "9999999" : SessionLocator.LoggedUserPM.Id;
        this.DepId = "";
        this.TransportModeId = "ALL";
    }
    constructor(public _declarationReferantDataWebService: DeclarationReferantDataWebService) {
        this.setFilters();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationReferantData").subscribe((response: any) => {

            _declarationReferantDataWebService.GetQueriesCounts(this.RefId, this.DepId, this.TransportModeId).subscribe(
                (data: any) => {
                    this.counters = data.Result;


                    if (this.CurrentSession == null) {
                        this.ChartID = "ChartID_-1_-1";
                    }

                    else {
                        this.ChartID = "ChartID_" + this.CurrentSession.GetChartId();
                    }

                    this.InProgressDeclarationReferantDataId = this.InProgressDeclarationReferantDataId + this.CurrentSession.GetChartId();
                    this.LoadInProgressDeclarationReferantDatasDashboard();

                    this.isScreenLoaded = true;
                    this.CurrentSession.StopBusyIndicator();
                });
        });
    }

    BarClicking() {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }

    }
    i: number = 0;
    LoadInProgressDeclarationReferantDatasDashboard() {

        this.InProgressDeclarationReferantDataDashboard = new Array<ChartingDataClass>();


        this._declarationReferantDataWebService.GetDeclarationReferantDataDashBoard(SessionLocator.TenantPM.Id).subscribe((myResult: any) => {
            this.InProgressDeclarationReferantDataDashboard = new Array<ChartingDataClass>();
            var myResponse: ServiceResponse = myResult;
            this.InProgressDeclarationReferantDataDashboard = myResponse.Result;
            this.dec_queries = new Dictionary();
            this.i = 0;

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInProcess"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInProcess", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("TrackingCases"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("TrackingCases", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInOCR"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInOCR", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInSivug"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInSivug", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInReview"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInReview", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInCreditControl"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInCreditControl", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesAvailableFreeOfCharge"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesAvailableFreeOfCharge", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInAllInclusive"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInAllInclusive", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByController"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByController", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByClassification"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByClassification", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("AllCases"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("AllCases", true));

            this.FillInProgressDeclarationReferantDataDashboardData();
        });
    }


    createChartingItem(queryCode: string, withAvailabilityDate: boolean = false) {

        var ele1 = new ChartingDataClass();
        if (withAvailabilityDate) {
            this.dec_queries.Add(this.i.toString(), queryCode + "_A");
            ele1.Id = queryCode + "_A";

        }
        else {
            ele1.Id = queryCode;
            this.dec_queries.Add(this.i.toString(), queryCode);
        }
        this.i++;

        ele1.StringProperty = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O." + queryCode);
        if (withAvailabilityDate)
            ele1.IntegerProperty = this.counters[queryCode + "_A"];
        else
            ele1.IntegerProperty = this.counters[queryCode];

        ele1.DataTypeCode = queryCode;
        ele1.DateTimeProperty = new Date();
        ele1.DecimalProperty = 0;
        ele1.DoubleProperty = 0;
        ele1.GroupedId = null;
        ele1.LabelProperty = null;
        ele1.TypeIndex = 0;
        return ele1;
    }


    FillInProgressDeclarationReferantDataDashboardData() {
        var index = 0;
        this.InProgressDeclarationReferantDataXAxis = [];

        //   this.InProgressDeclarationReferantDataDashboard.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
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

        //  StringArr.sort((a, b) => { return (a === b) ? 0 : (a < b) ? -1 : 1 });
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
        if (!flag) return;
        item = e.item;
        var Key = item.index;
        var query = this.dec_queries.Item((Key * 2 + e.target.columnIndex).toString());
        this.ViewReferantQuery(query);
    }
    public filters = new ApiQueryFilters();

    FilterChange($event) {
        if ($event != null) {
            this.filters = $event.Filters;
        }
        this.RefId = this.declarationReferantDataFiltersMenuComponent.LOVListUsers.map(({ Id }) => Id).toString();
        this.DepId = this.declarationReferantDataFiltersMenuComponent.LOVListDepartment.map(({ Id }) => Id).toString();
        this.TransportModeId = this.declarationReferantDataFiltersMenuComponent.transportmodeId;
        this._declarationReferantDataWebService.GetQueriesCounts(this.RefId, this.DepId, this.TransportModeId).subscribe(
            (data: any) => {
                this.counters = data.Result;
                this.InProgressDeclarationReferantDataId = this.InProgressDeclarationReferantDataId + this.CurrentSession.GetChartId();
                this.LoadInProgressDeclarationReferantDatasDashboard();
            });
    }


    ViewReferantQuery(myQueryCode: string) {
        if (myQueryCode != null) {
            var displayTitle = "";
            switch (myQueryCode.replace("_A", "")) {
                case "FilesInProcess":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInProcess");
                        break;
                    }
                case "TrackingCases":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.TrackingCases");
                        break;
                    }
                case "FilesInOCR":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInOCR");
                        break;
                    }
                case "FilesInSivug":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInSivug");
                        break;
                    }
                case "FilesInReview":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInReview");
                        break;
                    }
                case "FilesInCreditControl":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInCreditControl");
                        break;
                    }
                case "FilesAvailableFreeOfCharge":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesAvailableFreeOfCharge");
                        break;
                    }
                case "FilesInAllInclusive":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesInAllInclusive");
                        break;
                    }
                case "FilesRejectedByController":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesRejectedByController");
                        break;
                    }
                case "FilesRejectedByClassification":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesRejectedByClassification");
                        break;
                    }
                case "AllCases":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.AllCases");
                        break;
                    }
                default: { break; }
            }
            this.BuildFiltersForQuery(this.filters);
            if (myQueryCode.endsWith("_A"))
                this.filters.addAdditionalFilter("IsAvailabilityDateNull", false, null, null, "Equals", false, false, false, "number");

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode.replace("_A", "");
            listArgs.Filters = this.filters;
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
                            this._declarationReferantDataWebService.GetQueriesCounts(this.RefId, this.DepId, this.TransportModeId).subscribe(
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


