import { Component, Output, EventEmitter, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CustomsSettingExtendedListService } from '../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterService } from '../../../../Customs/Services/Others/CourierMasterService';
import { CourierMasterValidator } from '../../../../Customs/Validators/CourierMasterValidator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationCourierStatusListService } from '../../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeclarationCourierStatusWebService } from '../../../../Customs/Services/WebServices/DeclarationCourierStatusWebService';
import { CourierWorksheetSharedDataService } from '../../../../Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { CourierDeclarationFiltersMenuComponent } from './FiltersMenu/CourierDeclarationFiltersMenuComponent';
import { CardPMService } from 'Common/Services/StandardPMs/CardPMService';
import { TabFilter } from '../CourierWorkSheet/CourierWorksheetComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { debug } from 'console';


declare var PieClick, makePieChart, ResetItemPie;

@Component({
    templateUrl: './CourierDeclarationWorkspaceComponent.html',
    providers: [DeclarationCourierStatusWebService, CourierWorksheetSharedDataService]
})

export class CourierDeclarationWorkspaceComponent implements AfterViewInit {
    public ObjectTableName: any;

    @Output() ReloadUserQueries = new EventEmitter();
    public RecentGLAccountsCount: number = 0;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _CourierMasterService: CourierMasterService = new CourierMasterService();
    _EntityListService: EntityListService = new EntityListService();
    _CardPMService: CardPMService = new CardPMService();
    @ViewChild(CourierDeclarationFiltersMenuComponent) courierDeclarationFiltersMenuComponent: CourierDeclarationFiltersMenuComponent;


    // Queries Features
    public OpenCourierMasterVisibility: boolean = true;
    public AllCourierDeclarationVisibility: boolean = true;
    public UnReleasedFastProcessVisibility: boolean = true;
    public CourierMasterOpenIndividualVisibility: boolean = true;
    public UnReleasedIndividualVisibility: boolean = true;
    public WithoutIdVisibility: boolean = true;
    public IntegratorCode :string="";
    public WithoutClassificationVisibility: boolean = true;
    public PendingPaymentVisibility: boolean = true;
    public PendingCustomsVisibility: boolean = true;
    public PendingVisibility: boolean = true;
    public isRTL: boolean = false;
    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public counters: any;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();



    public _TabFilterList: TabFilter[] = [];
    public PendingTabCount: number;
    public _SelectedTabFilter: TabFilter;
    
    set SelectedTabFilter(val: TabFilter) { this._SelectedTabFilter = val; }
    public PendingObservableList: ObservableCollection;

    constructor(public _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService, public _declarationCourierStatusWebService: DeclarationCourierStatusWebService) {

        this._TabFilterList.push(new TabFilter("OPN", "טיסות פתוחות ", null, null));
        this._TabFilterList.push(new TabFilter("PEN", "Pending ", null, null));
        this._SelectedTabFilter = this._TabFilterList[0];
        this.PendingObservableList = new ObservableCollection([]);
        
        


        //this.LoadAllScreenData();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response: any) => {
                {
                    _declarationCourierStatusWebService.GetQueriesCounts(this.IntegratorCode).subscribe(
                        (data: any) => {
                            this.counters = data.Result;
                            this.CurrentSession.StopBusyIndicator();
                            this.isScreenLoaded = true;
                            this.BuildColumns();
                            this.RefreshList();


                        });

                }
            });
        });

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    ApplyFilters(val: string) {
        this.IntegratorCode=val;
        this._declarationCourierStatusWebService.GetQueriesCounts(this.IntegratorCode).subscribe(
            (data: any) => {
                this.counters = data.Result;
                //this.BuildColumns();
                this.RefreshList();
            });

    }

    ngAfterViewInit() {
        this._CourierWorksheetSharedDataService.CurrentMessage
            .subscribe(message => {
                if (message == "DoRefresh") {
                    this.RefreshButtonClicked();

                }

                this.LoadAllScreenData();


            });


    }
    public IsQueryVisible_MyViewsGroup: boolean = true;

    RefreshButtonClicked() {
        this._declarationCourierStatusWebService.GetQueriesCounts(this.IntegratorCode).subscribe(
            (data: any) => {
                this.counters = data.Result;
                this.LoadAllScreenData();
                this.RefreshList();
            });

    }

    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        //this.LoadRecentGLAccounts();
        this.ReloadUsersQuery();
    }

    public filters: ApiQueryFilters;
    FilterChange($event) {
        this.filters = new ApiQueryFilters();
        this.filters = $event.Filters;
        var IntegratorFilter = this.filters.AdditionalFilters.filter(a => a.FieldName == "IntegratorCode");
        if(IntegratorFilter.length>0){
            this.ApplyFilters(IntegratorFilter[0].FieldValue);
        }
    }

    SetQueriesVisibility() {
        //this.OpenCourierMasterVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ACTIVEGLACCOUNTS") ? true : false;
        this.OpenCourierMasterVisibility = true;
        this.AllCourierDeclarationVisibility = true;
        this.UnReleasedFastProcessVisibility = true ;
        this.CourierMasterOpenIndividualVisibility = true;
        this.UnReleasedIndividualVisibility = true;
        this.WithoutIdVisibility = true;
        this.WithoutClassificationVisibility = true;
        this.PendingPaymentVisibility = true;
        this.PendingCustomsVisibility = true;
        this.PendingVisibility = true;
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    LoadQueriesCounts() {

        //this.RefreshList();
        /*
        this._CourierMasterService.GetSummary().subscribe(myResult => {
            if (myResult != null) {
                this.courierMasterSummary.ActiveGLAccountCount = myResult.ActiveGLAccountCount > 1000 ? "1000+" : myResult.ActiveGLAccountCount.toString();
                this.courierMasterSummary.InactiveGLAccountCount = myResult.InactiveGLAccountCount > 1000 ? "1000+" : myResult.InactiveGLAccountCount.toString();
                this.courierMasterSummary.AllGLAccountCount = myResult.AllGLAccountCount > 1000 ? "1000+" : myResult.AllGLAccountCount.toString();
                this.courierMasterSummary.OpenFilesCount = myResult.OpenFilesCount > 1000 ? "1000+" : myResult.OpenFilesCount.toString();
                this.courierMasterSummary.ClosedFilesGLAccountCount = myResult.ClosedFilesGLAccountCount > 1000 ? "1000+" : myResult.ClosedFilesGLAccountCount.toString();
                this.courierMasterSummary.AllFilesCount = myResult.AllFilesCount > 1000 ? "1000+" : myResult.AllFilesCount.toString();
                this.courierMasterSummary.AllJobsCount = myResult.AllJobsCount > 1000 ? "1000+" : myResult.AllJobsCount.toString();
            }
        });
        */
    }

    //EditCourierMaster(entity: any) {
    //    if (entity != null) {
    //        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
    //            .then(cmpRef => {
    //                cmpRef.instance.ComponentRef = cmpRef;
    //                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Customs.CourierMaster', BackButtonLabel: TextCodeTranslator.Translate("General.MH.CourierMaster") });
    //                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
    //                    this.RefreshButtonClicked();
    //                });
    //            });
    //    }
    //}
    SearchFilter: string;
    onSearchTextChangeEvent(text: string) {
        this.SearchFilter = text;
        this.RefreshList();
    }

    RunNewGLAccountWizard() {
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewAccount"); // "New Account";
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');
    }

    ViewCourierMasterQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All GLAccounts";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "AllCourierDeclarations":
                    {
                        displayTitle = "All Declarations";
                        displayTitle = TextCodeTranslator.Translate("Customs.Declaration.O.AllCourierDeclarations");

                        break;
                    }
                case "OpenCourierMaster":
                    {
                        displayTitle = "Open Courier Master";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.CourierMasterOpen");

                        break;
                    }
                case "UnReleasedFastProcess":
                    {
                        displayTitle = "UnReleased Fast Process";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.UnReleasedFastProcess");

                        break;
                    }
                case "CourierMasterOpenIndividual":
                    {
                        displayTitle = "Courier Master Open Individual";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.CourierMasterOpenIndividual");

                        break;
                    }
                case "UnReleasedIndividual":
                    {
                        displayTitle = "UnReleased Individual";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.UnReleasedIndividual");

                        break;
                    }
                case "WithoutId":
                    {
                        displayTitle = "Without Id";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.WithoutId");

                        break;
                    }
                case "WithoutClassification":
                    {
                        displayTitle = "Without Classification";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.WithoutClassification");

                        break;
                    }
                case "PendingPayment":
                    {
                        displayTitle = "Pending Payment";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.PendingPayment");

                        break;
                    }
                case "PendingCustoms":
                    {
                        displayTitle = "Pending Customs";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.PendingCustoms");

                        break;
                    }
                case "Pending":
                    {
                        displayTitle = "Pending";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.Pending");
                        if (!AppTool.IsNullOrEmpty(this.PendingCode)) {
                            filters.addAdditionalFilter("CourierPendingReasonList", this.PendingCode, null, null, "Contains", false, false, false, "String");
                        }

                        break;
                    }


                default: { break; }
            }

            //filters.SortBy = this.currentSortingCol;
            //filters.SortDirection = this.currentSortingDir;

            filters = this.BuildFiltersForQuery(filters);
            if (myQueryCode == "Pending" && !AppTool.IsNullOrEmpty(this.PendingCode)) {
                filters.addAdditionalFilter("CourierPendingReasonList", this.PendingCode, null, null, "Contains", false, false, false, "String");
            }
            
            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Customs.Declaration";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.Declarations");
            //listArgs.Perspective = "CourierMasterWS";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.PendingCode = null;
                            this.LoadAllScreenData();
                            this._declarationCourierStatusWebService.GetQueriesCounts(this.IntegratorCode).subscribe(
                                (data: any) => {
                                    
                                    this.counters = data.Result;

                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.RefreshList();
                                });
                        });
                    });
            });
        }
    }

    BuildFiltersForQuery(filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        if (this.filters?.AdditionalFilters?.length > 0) {
            var excludeIntegratorFilter = this.filters.AdditionalFilters.filter(a => a.FieldName == "IntegratorCode");
            if (excludeIntegratorFilter) {
                filters.addAdditionalFilter(excludeIntegratorFilter[0]?.FieldName, excludeIntegratorFilter[0]?.FieldValue, null, null, "Equal", false, false, false, "LookUp");
            }
        }
        
        //filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        return filters;
    }



    

    //public RecentGLAccountsList: GLAccountList[];
    //LoadRecentGLAccounts() {
    //    this.RecentGLAccountsList = [];
    //    this.RecentGLAccountsCount = 0;

    //    this._DeclarationExtendedListService.GetRecentGLAccounts("1").subscribe((myResponse: ServiceResponse) => {
    //        if (myResponse != null) {
    //            if (!myResponse.HasError) {
    //                var myResult = myResponse.Result;

    //                this.RecentGLAccountsList = myResult;
    //                this.RecentGLAccountsCount = myResult.length;
    //            }
    //        }
    //    });
    //}
    

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: 'IntegratorName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.IntegratorName"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            //ServerSideSortable: true,
            //SortByName: 'IntegratorName'
        });

        this.columns.push({
            FieldName: 'PrefixMAWB',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.MAWB"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });

        this.columns.push({
            FieldName: 'EstimatedArrivalDate',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.EstimatedArrivalDate"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });

        this.columns.push({
            FieldName: 'CalcClosedForFollowUp',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcClosedForFollowUp"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });

        this.columns.push({
            FieldName: 'CalcMissingImporterId',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcMissingImporterId"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });

        this.columns.push({
            FieldName: 'CalcMissingClassification',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcMissingClassification"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });

        this.columns.push({
            FieldName: 'CalcPending900',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcPending900"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });

        this.columns.push({
            FieldName: 'CalcSuspendedDeclarations',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcPendingCustoms"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierDeclarationWorkspaceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierDeclarationWorkspaceListTemplate',
        });
    }

    DataSource = {

        pageSize: 30,
        rowCount: null,
        //sortingCol: "CourierHawb",
        //sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, this.filters);
            return tempo;

        },
    };

    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        if (this.filters?.AdditionalFilters.length > 0) {
            filters = this.filters;
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        this.BuildFiltersForCourierMasterQuery(filters);
         var myout = this._EntityListService.getExtendedByFilters("Customs.CourierMaster", filters);
         return myout;
    }
    DeclarationCourierPendingTabRecords: DeclarationCourierPendingTabRecord[] = [];
    PindingClicking() {
        if (PieClick() != null) {
            this.OnQuoteClick(PieClick());
            ResetItemPie();
        }

    }

       RefreshList() {
        
        this._declarationCourierStatusWebService.GetWorkSpacePendingTab(this.IntegratorCode).subscribe(
            (data: any) => {
                this.PendingObservableList.InsertCollection(data.Result);
                this.DeclarationCourierPendingTabRecords = data.Result;
                let count = 0;
                for (var i = 0; i < this.DeclarationCourierPendingTabRecords.length; i++) {
                    var cur = this.DeclarationCourierPendingTabRecords[i];
                    count += cur.Count;
                }
                this.PendingTabCount = count;
                this.FillPie()
            });

        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
    }

    BuildFiltersForCourierMasterQuery(filters: ApiQueryFilters = null) {

        if (filters == null) {
            filters = new ApiQueryFilters();
        }
        filters.addAdditionalFilter("IsOpen", true, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "EstimatedArrivalDate";
        }
        if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }

        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            filters.addAdditionalFilter("SearchFields", this.SearchFilter.toLowerCase(), null, null, "Contains", false, false, false, "string", false, true);
        }
    }
    PendingClick(item) {
        //alert(item);
        
    }
    PendingCode: any;
    onCellPendingClick($event, item) {
        //alert(item);
        this.PendingCode=item.Code;
        this.ViewCourierMasterQuery('Pending')
    }

    OnQuoteClick(e) {

        var item = this.DeclarationCourierPendingTabRecords[e.index];

        this.PendingCode = item.Code;
        this.ViewCourierMasterQuery('Pending')


    }

    ViewInitCompleted($event) {
    }





    TabFilterClick(item) {
        this.SelectedTabFilter = item;
        //this._CourierWorksheetSharedDataService._SelectedItems.Collection = [];
        

        //switch (item.Code) {
            
        //    case "ACC":
        //        this._SelectedACCValue = 'W';
        //        break;
        //    case "HOLD":
        //        if (this.SelectedPendingCodeFilter == null && this._PendingCodes != null && this._PendingCodes.length > 0) this.SelectedPendingCodeFilter = this._PendingCodes[0];
        //        break;
        //}
       

    }

    public PieData: any;
    public pieChartLabels: string[] = [];
    public pieChartData: number[] = [];
    private CurrentTop10DebtorsChart: any;
    public TopFiveDashboardId: string = "TopFiveDashboardId_";
    public TopFiveDashboardLegendId: string;

    FillPie() {
        var fullData = [];
        this.pieChartLabels = [];
        this.pieChartData = [];

        this.DeclarationCourierPendingTabRecords.forEach(element => {
            var amount = AppTool.Round(element.Count, 3);
            fullData.push({ label: element.Name, data: amount })
            this.pieChartLabels.push(element.Name);
            this.pieChartData.push(element.Count);
        });

        if (this.CurrentTop10DebtorsChart != null) {
            this.CurrentTop10DebtorsChart.clear();
            this.CurrentTop10DebtorsChart = null;
        }

        this.CurrentTop10DebtorsChart = makePieChart(this.TopFiveDashboardId, fullData, false, true, this.TopFiveDashboardLegendId);
    }

}
class DeclarationCourierPendingTabRecord {
    public Code: string ;
    public Name: string;
    public Count: number;
    }
