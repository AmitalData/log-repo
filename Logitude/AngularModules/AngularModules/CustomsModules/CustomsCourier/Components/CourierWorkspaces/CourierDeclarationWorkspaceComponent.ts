import {Component, Output, EventEmitter , OnInit , AfterViewInit} from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
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
import { DeclarationEditComponentController } from '../../../../Customs/Controller/DeclarationEditComponentController';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CourierWorksheetSharedDataService } from '../../../../Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { DeclarationCourierStatusList } from '../../../../Customs/EntityLists/DeclarationCourierStatusList';
import { CustomsRequestsSheetPM } from '../../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';

@Component({
    moduleId: module.id,
    templateUrl: './CourierDeclarationWorkspaceComponent.html',

})

export class CourierDeclarationWorkspaceComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();
    public RecentGLAccountsCount: number = 0;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _CourierMasterService: CourierMasterService = new CourierMasterService();
    _EntityListService: EntityListService = new EntityListService();

    // Queries Features
    public OpenCourierMasterVisibility: boolean = true;
    public UnReleasedFastProcessVisibility: boolean = true;
    public CourierMasterOpenIndividualVisibility: boolean = true;
    public UnReleasedIndividualVisibility: boolean = true;
    public WithoutIdVisibility: boolean = true;
    public WithoutClassificationVisibility: boolean = true;
    public PendingPaymentVisibility: boolean = true;
    public PendingCustomsVisibility: boolean = true;
    public PendingVisibility: boolean = true;
    public isRTL: boolean = false;
    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LoadAllScreenData();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response: any) => { 
                {
                    this.isScreenLoaded = true;
                    this.CurrentSession.StopBusyIndicator();
                    this.BuildColumns();
                }
            });
        });

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    ngAfterViewInit() {
        this.LoadAllScreenData();
    }
    public IsQueryVisible_MyViewsGroup: boolean = true;

    InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        //this.LoadRecentGLAccounts();
        this.ReloadUsersQuery();
    }

    SetQueriesVisibility() {
        //this.OpenCourierMasterVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ACTIVEGLACCOUNTS") ? true : false;
        this.OpenCourierMasterVisibility = true;
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

    EditCourierMaster(entity: any) {
        if (entity != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Customs.CourierMaster', BackButtonLabel: TextCodeTranslator.Translate("General.MH.CourierMaster") });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.RefreshButtonClicked();
                    });
                });
        }
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

                        break;
                    }


                default: { break; }
            }

            //filters.SortBy = this.currentSortingCol;
            //filters.SortDirection = this.currentSortingDir;

            this.BuildFiltersForQuery(filters);

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "Customs.Declaration";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.Declarations");
            listArgs.Perspective = "CourierMasterWS";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }


    BuildFiltersForQuery(filters: ApiQueryFilters = null) {

        if (filters == null) {
            filters = new ApiQueryFilters();
        }
        //filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        /*
        switch (this._SelectedTabFilter.Code) {
            //case "ACC":
            case "ALL": {
                break;
            }
            default: {
                filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }
        
        switch (this._SelectedBOLValue) {
            case "L": {
                filters.addAdditionalFilter("HighLowValue", "L", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "H": {
                filters.addAdditionalFilter("HighLowValue", "H", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedStatusValue) {
            case "O": {
                filters.addAdditionalFilter("IsClosedForFollowUp", false, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
            case "C": {
                filters.addAdditionalFilter("IsClosedForFollowUp", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }

        switch (this._SelectedMNFValue) {
            case "C": {
                filters.addAdditionalFilter("CourierManifestStatusCode", "M", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "W": {
                filters.addAdditionalFilter("CourierManifestStatusCode", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedDECValue) {
            case "C": {
                filters.addAdditionalFilter("CourierDeclarationStatusCode", "M", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "W": {
                filters.addAdditionalFilter("CourierDeclarationStatusCode", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedDOCValue) {
            case "C": {
                filters.addAdditionalFilter("DocumentStatusCode", "M", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "U": {
                filters.addAdditionalFilter("DocumentStatusCode", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedTotalInvoiceValue) {
            case "75": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 75, null, null, "LessThanOrEqual", false, false, false, "number");
                break;
            }
            case "500": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 76, 500, null, "Between", false, false, false, "number", false);
                break;
            }
            case "1000": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 501, 1000, null, "Between", false, false, false, "number", false);
                break;
            }
        }

        switch (this._SelectedAvailableValue) {
            case "AD": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "AV": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "1", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "NAV": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "null", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedACCValue) {
            case "W": {
                filters.addAdditionalFilter("StorageSiteStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "WS": {
                filters.addAdditionalFilter("SpecialActionStatus", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        if (this.SelectedPendingCodeFilter != null && this._SelectedTabFilter.Code == "HOLD") {
            switch (this.SelectedPendingCodeFilter.Key) {
                case "A": {
                    break;
                }
                default: {
                    filters.addAdditionalFilter("CourierPendingReasonList", this.SelectedPendingCodeFilter.Key, null, null, "Contains", false, false, false, "string");
                    break;
                }
            }
        }

        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            filters.addAdditionalFilter("CourierSearchFields", this.SearchFilter, null, null, "Contains", false, false, false, "string", false, true);
        }
        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "CourierHawb";
        }
        if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }

        switch (this._SelectedFastIndividualProcessValue) {
            case "F": {
                filters.addAdditionalFilter("FastIndividualProcessCode", "F", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "I": {
                filters.addAdditionalFilter("FastIndividualProcessCode", "I", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedCustomStatusValue) {
            case "H": {
                filters.addAdditionalFilter("CourierCustomStatusCode", "1", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "S": {
                filters.addAdditionalFilter("CourierCustomStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        */
    }



    /*

    public RecentGLAccountsList: GLAccountList[];
    LoadRecentGLAccounts() {
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;

        this._DeclarationExtendedListService.GetRecentGLAccounts("1").subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentGLAccountsList = myResult;
                    this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    }
    */

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: 'MAWBP',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.MAWB"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });

        this.columns.push({
            FieldName: 'EstimatedArrivalDate',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.EstimatedArrivalDate"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });

        this.columns.push({
            FieldName: 'CalcClosedForFollowUp',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcClosedForFollowUp"),
            Styles: { width: '70px' },
            IsCustomTemplate: true,
            //HtmlListComponentName: 'CourierWorksheetListTemplate',
            //HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });

        this.columns.push({
            FieldName: 'CalcMissingImporterId',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcMissingImporterId"),
            Styles: { width: '160px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });

        this.columns.push({
            FieldName: 'CalcMissingClassification',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcMissingClassification"),
            Styles: { width: '160px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });

        this.columns.push({
            FieldName: 'CalcPendingCustoms',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.CourierMaster.F.CalcPendingCustoms"),
            Styles: { width: '160px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });

    }

    DataSource = {

        pageSize: 30,
        rowCount: null,
        //sortingCol: "CourierHawb",
        //sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };

    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        if (filters == null) {
            filters = new ApiQueryFilters();
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

    BuildFiltersForCourierMasterQuery(filters: ApiQueryFilters = null) {

        if (filters == null) {
            filters = new ApiQueryFilters();
        }
        filters.addAdditionalFilter("IsOpen", true, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "EstimatedArrivalDate";
        }
        if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }
    }

    ViewInitCompleted($event) {
    }

}
