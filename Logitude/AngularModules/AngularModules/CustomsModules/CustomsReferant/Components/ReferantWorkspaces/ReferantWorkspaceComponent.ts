import { Component, OnDestroy, AfterViewInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationReferantDataWebService } from '../../../../Customs/Services/WebServices/DeclarationReferantDataWebService';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ChartingDataClass } from '../../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { FormatTool, AppTool } from '../../../../Infrastructure/Tools';
import { DeclarationReferantDataExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationReferantDataExtendedListService';
import { Dictionary } from '../../../../Infrastructure/GenericTypes/Dictionary';
import { DeclarationReferantDataFiltersMenuComponent } from '../FiltersMenu/DeclarationReferantDataFiltersMenuComponent';
import { AdvancedQueryFiltersPMService } from 'Infrastructure/Services/StandardPMs/AdvancedQueryFiltersPMService';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { ServiceArgs } from 'Infrastructure/DataContracts/ServiceArgs';
import { debounce, debounceTime } from 'rxjs/operators';
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
    public ObjectTableName: string = "Customs.DeclarationReferantData";


    // Queries Features
    public FilesInProcessVisibility: boolean = true;
    public TrackingCasesVisibility: boolean = true;
    public FilesInOCRVisibility: boolean = true;
    public FilesInSivugVisibility: boolean = true;
    public FilesInReviewVisibility: boolean = true;
    public FilesInCreditControlVisibility: boolean = true;
    public FilesAvailableFreeOfChargeVisibility: boolean = true;
    public FilesWithoutReleaseVisibility: boolean = true;
    public FilesInAllInclusiveVisibility: boolean = true;
    public FilesRejectedByControllerVisibility: boolean = true;
    public FilesRejectedByClassificationVisibility: boolean = true;
    public AllCasesVisibility: boolean = true;
    public FilesToPayVisibility: boolean = true;
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
        this.getFiltersFromDb();
    }


    getFiltersFromDb() {
        this._declarationReferantDataWebService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, "Customs.DeclarationReferantData.AllCases").subscribe((myResult: any) => {
            var listArgs = new ListComponentArgs();
            this.CurrentSession.StopBusyIndicator();
            this.isScreenLoaded = true;
            var AdditionalFilters: FilterItem[] = new Array();
            if (myResult) {
                var ReferantUserIdFilter = myResult.filter(a => a.ObjectFieldName == "ReferentUserId");
                if (ReferantUserIdFilter.length > 0) {
                    AdditionalFilters.push(new FilterItem("ReferentUserId", ReferantUserIdFilter[0].PredefinedValue, null, null, "InListExact", false, false, false, "string", null, null, null));
                }
                var DepartmentNameFilter = myResult.filter(a => a.ObjectFieldName == "DepartmentName");
                if (DepartmentNameFilter.length > 0) {
                    AdditionalFilters.push(new FilterItem("DepartmentName", DepartmentNameFilter[0].PredefinedValue, null, null, "Equal", false, false, false, "string", null, null, null));
                }
                var ReferantUserNameFilter = myResult.filter(a => a.ObjectFieldName == "ReferantUserName");
                if (ReferantUserNameFilter.length > 0) {
                    AdditionalFilters.push(new FilterItem("ReferantUserName", ReferantUserNameFilter[0].PredefinedValue, null, null, "Equal", false, false, false, "string", null, null, null));
                }
                var DepartmentIdFilter = myResult.filter(a => a.ObjectFieldName == "DepartmentId");
                if (DepartmentIdFilter.length > 0) {
                    AdditionalFilters.push(new FilterItem("DepartmentId", DepartmentIdFilter[0].PredefinedValue, null, null, "InListExact", false, false, false, "string", null, null, null));
                }
                var TransportModeIdFilter = myResult.filter(a => a.ObjectFieldName == "TransportModeId");
                if (TransportModeIdFilter.length > 0) {
                    AdditionalFilters.push(new FilterItem("TransportModeId", TransportModeIdFilter[0].PredefinedValue, null, null, "Equals", false, true, false, "string", null, null, null));
                }
            }
            if (AdditionalFilters.length > 0) {
                listArgs.QuerySection = myResult[0].QueryId;
            }else{
                this.applyQueriesCount();
            }
            listArgs.Filters = AdditionalFilters;
            this.declarationReferantDataFiltersMenuComponent.SetFiltersMenu(listArgs);
            //this.applyQueriesCount();
        });
    }

    constructor(public _declarationReferantDataWebService: DeclarationReferantDataWebService) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationReferantData").subscribe((response: any) => {
                    this.setFilters();
        });

    }

    applyQueriesCount() {
        this.GetFilterForQueriesCount();
        this._declarationReferantDataWebService.GetQueriesCounts(this.RefId, this.DepId, this.TransportModeId).subscribe(
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
    }

    public IsQueryVisible_MyViewsGroup: boolean = true;

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

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesToPay"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesToPay", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesAvailableFreeOfCharge"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesAvailableFreeOfCharge", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesWithoutRelease"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesWithoutRelease", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInAllInclusive"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesInAllInclusive", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByController"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByController", true));

            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByClassification"));
            this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("FilesRejectedByClassification", true));


            // this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("AllCases"));
            // this.InProgressDeclarationReferantDataDashboard.push(this.createChartingItem("AllCases", true));

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
    public filters: ApiQueryFilters;
    viewFilters = new ApiQueryFilters();


    FilterChange($event, IsFilterFromDbLoadd: boolean) {
        if (IsFilterFromDbLoadd) {
            this.filters = new ApiQueryFilters();
            this.filters = $event.Filters;
            this.ApplyFilters(this.filters);
        }
    }
    RefreshButtonClicked() {
        this.GetFilterForQueriesCount();
        this._declarationReferantDataWebService.GetQueriesCounts(this.RefId, this.DepId, this.TransportModeId).subscribe(
            (data: any) => {
                this.counters = data.Result;
                this.LoadAllScreenData();
                this.LoadInProgressDeclarationReferantDatasDashboard();
            });

    }

    ApplyFilters(filters) {
        this.GetFilterForQueriesCount();
        this._declarationReferantDataWebService.GetQueriesCounts(this.RefId, this.DepId, this.TransportModeId).pipe(debounceTime(1000)).subscribe(
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
                case "FilesToPay":
                    {
                        displayTitle = TextCodeTranslator.Translate("Customs.DeclarationReferantData.O.FilesToPay");
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
            if (AppTool.IsNullOrEmpty(listArgs.NewButtonLabel)) listArgs.NewButtonLabel = TextCodeTranslator.Translate("Customs.General.O.NewCustomsFile"); // "פתיחת תיק חדש";
            var _filters = ["TransportModeId", "ReferentUserId", "DepartmentName", "ReferantUserName", "DepartmentId"];

            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
               
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.UpdateOnReturnFromQuery();
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

    private UpdateOnReturnFromQuery() {
        this.filters.AdditionalFilters = this.filters.AdditionalFilters.filter(x => x.FieldName == "TransportModeId" || x.FieldName == "ReferentUserId" || x.FieldName == "DepartmentName" || x.FieldName == "ReferantUserName" || x.FieldName == "DepartmentId");
        this.LoadAllScreenData();
        this.GetFilterForQueriesCount();
    }

    public GetFilterForQueriesCount() {
        if (this.filters?.AdditionalFilters.length > 0) {
            this.viewFilters.AdditionalFilters = this.filters.AdditionalFilters.filter(a => a.FieldName == "ReferentUserId" || a.FieldName == "DepartmentId" || a.FieldName == "TransportModeId" || a.FieldName == "ReferantUserName" || a.FieldName == "DepartmentName");
            this.RefId = this.declarationReferantDataFiltersMenuComponent.LOVListUsers.map(({ Id }) => Id).toString();
            this.DepId = this.declarationReferantDataFiltersMenuComponent.LOVListDepartment.map(({ Id }) => Id).toString();
            this.TransportModeId = this.declarationReferantDataFiltersMenuComponent.SelectedValue;
        }
    }

    public LoadAllScreenData() {
        this.ReloadUsersQuery();
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.UpdateOnReturnFromQuery();
    }

    BuildFiltersForQuery(filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
    }
}


