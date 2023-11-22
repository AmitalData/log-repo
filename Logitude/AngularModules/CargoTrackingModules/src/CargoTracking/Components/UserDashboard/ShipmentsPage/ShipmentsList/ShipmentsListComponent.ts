import {
    Component,
    ViewChild,
    AfterViewInit,
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    ElementRef,
    OnInit,
    HostListener
} from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import {Router, ActivatedRoute, NavigationStart, NavigationEnd} from '@angular/router';
import {FormBuilder} from '@angular/forms';
import {CargoTrackingSearchService} from '../../../../Services/Others/CargoTrackingSearchService';
import {CargoTrackingShipmentList} from '../../../../EntityLists/CargoTrackingShipmentList';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {CdkScrollable, CdkVirtualScrollViewport} from '@angular/cdk/scrolling';
import {Customer, ShipmentDataSource} from '../../../../DataContracts/CargoTrackingShipmentDataSource';
import {CargoTrackingShipmentSearchInput, MoreFilter} from '../../../../DataContracts/CargoTrackingShipmentFilters';
import {CargoTrackingBrandingData} from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import {CargoTrackingPortService} from '../../../../Services/Others/CargoTrackingPortService';
import {CargoTrackingShipmentService} from '../../../../Services/Others/CargoTrackingShipmentService';
import {MessageWindowComponent} from '../../../../../Infrastructure/Components/MessageWindow/MessageWindowComponent';
import {MatDialog} from '@angular/material/dialog';
import {RootContext} from 'src/CargoTracking/Utilities/RootContext';
import {CargoTrackingMilestoneService} from 'src/CargoTracking/Services/Others/CargoTrackingMilestoneService';
import {MultipleSelectionComponent} from 'src/Infrastructure/Components/MultipleSelection/MultipleSelectionComponent';
import {filter} from 'rxjs/operators';
import {ShipmentDirections} from '../ShipmentDetails/ShipmentDetailsComponent';
import {SharedService} from 'src/CargoTracking/Services/Others/SharedService';
import {QueryColumnPM} from 'src/CargoTracking/Services/Others/QueryColumnPM';
import {ApiQueryFilters} from 'src/CargoTracking/Services/Others/ApiQueryFilters';
import {LogitudeGridExportToExcelService} from 'src/CargoTracking/Services/Others/LogitudeGridExportToExcelComponent';
import { TenantManagementService } from 'src/CargoTracking/Services/Others/TenantManagementService';
import { TenantManagementPM } from 'src/CargoTracking/Services/Others/TenantManagementPM';

@Component({
    selector: 'ShipmentsListComponent',
    templateUrl: './ShipmentsListComponent.html',
    styleUrls: [
        './ShipmentsListComponent.css',
        './ShipmentsListComponent.mobile.css',
        './ShipmentsListComponent.tablet.css'
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,

})
export class ShipmentsListComponent implements AfterViewInit, OnInit {
    moreFilterCodes = MoreFilterCodes;
    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    searchForm;
    mobileSearchForm;
    Shipments: CargoTrackingShipmentList[] = [];
    public QueryColumns: QueryColumnPM[] = [];
    private filterAgrs: ApiQueryFilters = new ApiQueryFilters();
    isLoading: boolean = false;
    isFilter1Expanded: boolean = false;
    isFilter2Expanded: boolean = false;
    isMilestonesStatusFilterExpanded: boolean = false;
    isShipmentTypeFilterExpanded: boolean = false;
    isShipmentDirectionFilterExpanded: boolean = false;
    isAbdullahCompanyChecked: boolean = true;
    showMobileSortMenu: boolean = false;
    showShipmentDetailsMenu: boolean = false;
    ShipmentsDataSource: ShipmentDataSource;
    @ViewChild(CdkVirtualScrollViewport) virtualScroll: CdkVirtualScrollViewport;
    @ViewChild('input') searchInput: ElementRef;
    @ViewChild('mobileSearch') mobileSearchInput: ElementRef;
    @ViewChild('shipmentTypeMultipleSelection') shipmentTypeMultipleSelection: MultipleSelectionComponent;
    @ViewChild('shipmentDirectionMultipleSelection') shipmentDirectionMultipleSelection: MultipleSelectionComponent;
    @ViewChild('shipmentMoreFiltersMultipleSelection') shipmentMoreFiltersMultipleSelection: MultipleSelectionComponent;
    @ViewChild('sortByMultipleSelection') sortByMultipleSelection: MultipleSelectionComponent;
    @ViewChild('scrollViewport') scrollViewport: CdkScrollable;
    public MoreReferenceText: string;
    public ConsignmentNumber: string;
    public toPortCode: string;
    public fromPortCode: string;
    ShipmentPM: any;
    NumberOfPackages: number = 0;
    TitleOfEstimationORActualDate: string = "";
    ValueOfEstimationORActualDate: Date;
    ShipmentTypeAndDirectionTooltip: string;
    SupplierOrClientTitle: string;
    SupplierOrClientValue: string;
    ShowMobileSearch: boolean = false;
    public SortOptions = SortOptions;
    MasterOrHouseLabel: string = "";
    EntityType_Customs = "C";
    isMobileView: boolean;
    tempInvitedCustomers: any[] = [];
    tempMilestonesStatus: any[] = [];
    tempShipmentDirectionFilters: any[] = [];
    tempShipmentTypeFilters: any[] = [];
    MilestonesStatusNoResult: boolean;
    ShipmentDirectionFiltersNoResult: boolean;
    ShipmentTypeFiltersNoResult: boolean;
    InvitedCustomersNoResult: boolean;
    SearchText: any = "";
    backMonths: number;

    get tenant() {
        return CargoTrackingBrandingData.Tenant;
    }

    get enableExportToExcel() {
        return CargoTrackingBrandingData.EnableExportToExcel;
    }

    FiltersSelectedInvitedCustoms: any[] = [];
    ShipmentSearchInput: CargoTrackingShipmentSearchInput = new CargoTrackingShipmentSearchInput();
    MilestonesStatus: any[] = [];
    MilestonesStatusDictionary: {} = {};
    InvitedCustomers: any[] = [];
    MoreFilterMobileValue: MoreFilter = new MoreFilter();
    InvitedCustomersDictionary: {} = {};

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private changeDetector: ChangeDetectorRef,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService,
        public dialog: MatDialog,
        private searchService: CargoTrackingSearchService,
        private milestonesService: CargoTrackingMilestoneService,
        public sharedService: SharedService,
        private logitudeGridExportToExcelService: LogitudeGridExportToExcelService,
        private tenantManagementService: TenantManagementService,
        ) {
        this.InitComponent();
        this.SetDefaultBackgroundColor();

        router.events.subscribe((value) => {
            this.OnRouteChanged(value);
        });
    }

    private GetCompanyLoginsFromCache() {

        SessionInfo.LoggedUserCompanyLogins = JSON.parse(sessionStorage.getItem("LoggedUserCompanyLogins"));
        if (!SessionInfo.IsAdmin) {
            this.GetInvitedCustomers();
            // if(SessionInfo.LoggedUserCompanyLogins.filter(a => a.Tenant == this.tenant)[0].IsUser === false)
            // {

            // }
        }


    }

    GetInvitedCustomers() {

        console.log('SessionInfo.LoggedUserCompanyLogins', SessionInfo.LoggedUserCompanyLogins);
        this.InvitedCustomers = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => (
                {
                    IsSelected: false,
                    Name: d.CompanyName.substring(0, d.CompanyName.lastIndexOf('(')),
                    ...d
                }
            ));
        let StartwithSpeicalCharCustomers = this.InvitedCustomers.filter(a => this.CheckSpeicalChar(a.Name.replace(/ /g, "")));
        let StartwithoutSpeicalCharCustomers = this.InvitedCustomers.filter(a => !this.CheckSpeicalChar(a.Name.replace(/ /g, "")));
        StartwithSpeicalCharCustomers = StartwithSpeicalCharCustomers.sort((a, b) => a["Name"].toUpperCase().replace(/ /g, "") > b["Name"].toUpperCase().replace(/ /g, "") ? 1 : a["Name"].toUpperCase().replace(/ /g, "") === b["Name"].toUpperCase().replace(/ /g, "") ? 0 : -1);
        StartwithoutSpeicalCharCustomers = StartwithoutSpeicalCharCustomers.sort((a, b) => a["Name"].toUpperCase().replace(/ /g, "") > b["Name"].toUpperCase().replace(/ /g, "") ? 1 : a["Name"].toUpperCase().replace(/ /g, "") === b["Name"].toUpperCase().replace(/ /g, "") ? 0 : -1);
        this.InvitedCustomers = StartwithSpeicalCharCustomers.concat(StartwithoutSpeicalCharCustomers);
        this.FillInvitedCustomersDictionary(this.InvitedCustomers);
    }

    FillInvitedCustomersDictionary(InvitedCustomers: any[]) {
        InvitedCustomers.forEach(element => {
            this.InvitedCustomersDictionary[element.CardId] = element;
        });
        this.InvitedCustomers.forEach(val => this.tempInvitedCustomers.push(val));
    }
    private CheckSpeicalChar(s: string) {
        var format = /^[A-Za-z0-9]/;
        if (format.test(s.charAt(0))) {
            return true;
        }
        return false;
    }

    ngOnInit(): void {
        this.setMaxNumberOfCarachter(window.innerWidth);
        this.setDefaultSort();
        this.GetMilstones();
        this.getPreviousScroll();
        this.GetCompanyLoginsFromCache();
        this.fillFeltersDictionary();
        this.setViews();
    }

    OnRouteChanged(event) {
        if (event instanceof NavigationEnd) {
            this.RerenderVirtualScroll();
        }
    }

    public ExportToExcelClick() {
        this.buildFilterArgs();
        this.QueryColumns = [];
        this.buildQueryColumns();
        this.logitudeGridExportToExcelService.ExportToExcelExcute('CargoTrackingShipment', this.filterAgrs, this.QueryColumns);
    }

    private buildFilterArgs() {
        this.filterAgrs = new ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("Tenant", this.ShipmentSearchInput.Tenant, null, null, "Equals", false, false, false, "string");
        this.filterAgrs.addAdditionalFilter("HasException", this.ShipmentSearchInput.HasException, null, null, "Equals", true, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("OrdersOnly", this.ShipmentSearchInput.OrdersOnly, null, null, "Equals", true, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("EstimatedArrivalOnly", this.ShipmentSearchInput.EstimatedArrivalOnly, null, null, "Equals", true, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("OperationalOpenedOnly", this.ShipmentSearchInput.OperationalOpenedOnly, null, null, "Equals", true, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("SearchText", this.ShipmentSearchInput.SearchText, null, null, "Equals", false, false, false, "string");
        this.filterAgrs.Tenant = this.ShipmentSearchInput.Tenant;
        this.filterAgrs.SortDirection = this.ShipmentSearchInput.SortType;
        this.filterAgrs.SortBy = this.ShipmentSearchInput.SortFieldName;

        if (this.ShipmentSearchInput.CustomersIds.length > 0) {
            this.filterAgrs.addAdditionalFilter("CustomersIds", this.ShipmentSearchInput.CustomersIds.join("_"), null, null, "Equals", false, false, false, "string");
        } else if (this.InvitedCustomers.length > 0) {

            this.filterAgrs.addAdditionalFilter("CustomersIds", this.InvitedCustomers.map(d => d.CardId).join("_"), null, null, "Equals", false, false, false, "string");
        }

        if (this.ShipmentSearchInput.MilestonesCodes.length > 0) {
            this.filterAgrs.addAdditionalFilter("MilestonesCodes", this.ShipmentSearchInput.MilestonesCodes.join("_"), null, null, "Equals", false, false, false, "string");
        }

        if (this.ShipmentSearchInput.TransportModeCodes.length > 0) {
            this.filterAgrs.addAdditionalFilter("TransportModeCodes", this.ShipmentSearchInput.TransportModeCodes.join("_"), null, null, "Equals", false, false, false, "string");
        }

        if (this.ShipmentSearchInput.DirectionCodes.length > 0) {
            this.filterAgrs.addAdditionalFilter("DirectionCodes", this.ShipmentSearchInput.DirectionCodes.join("_"), null, null, "Equals", false, false, false, "string");
        }
    }

    private buildQueryColumns() {
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("ShipmentNumber", 'Text', 'Shipment Number'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("CustomerReference", 'Text', 'Reference No'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("TransportModeId", 'Text', 'Transport Mode'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("DirectionId", 'Text', 'Direction'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("House", 'Text', 'House'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("Master", 'Text', 'Master'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("Client", 'Text', 'Client'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("ConsigneeName", 'Text', 'Consignee'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("CurrentMilestoneDate", 'DateTime', 'Current Milestone Date'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("CurrentMilestoneName", 'Text', 'Current Milestone Name'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("FromPortName", 'Text', 'From Port'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("ToPortName", 'Text', 'To Port'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("ArrivalDate", 'DateTime', 'ATA'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("ArrivalEstimationDate", 'DateTime', 'ETA'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("DepartureDate", 'DateTime', 'ATD'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("DepartureEstimationDate", 'DateTime', 'ETD'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("GrossWeight", 'Double', 'Weight'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("HasException", 'Text', 'Has Exception'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("CurrentMilestoneExceptions", 'Text', 'Exception Description'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("IsOrder", 'Text', 'Is Order'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("NumberOfPackages", 'Integer', 'Quantity'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("IncotermName", 'Text', 'Incoterm'));
        this.QueryColumns.push(this.logitudeGridExportToExcelService.GetQueryColumn("ChargeableWeight", 'Double', 'Chargeable Weight', 100));

    }

    private setDefaultSort() {
        this.ShipmentSearchInput.SortFieldName = SortOptions.CMD;
        this.ShipmentSearchInput.SortType = SortOptions.DESC;
    }

    private selectDefaultSortFields() {
        this.sortByMultipleSelection.select.options.filter(d =>
            [SortOptions.CMD, SortOptions.DESC].includes(d.value.Code))
            .map(x => x.select());
    }

    private getPreviousScroll() {
        this.router.events.pipe(
            filter((e: any): e is NavigationEnd => e instanceof NavigationEnd)
        ).subscribe((e: NavigationEnd) => {
            if (e.url === '/cargo-tracking/shipments') {
                const shipmentCardsContainer = document.getElementById("scrollArea");
                if (shipmentCardsContainer && RootContext.ShipmentsScrollPosition > 0) {
                    shipmentCardsContainer.scrollTop = RootContext.ShipmentsScrollPosition || 0;
                }
            }

        });
    }

    ngAfterViewInit(): void {
        this.selectDefaultSortFields();
        this.SetShipmentsScrollPosition();
        this.LoadScreenData();
    }

    ShipmentTypeChangedHandler(code: string) {
        RootContext.ShipmentsScrollPosition = 0;
        if (this.ShipmentSearchInput.TransportModeCodes.includes(code)) {
            this.ShipmentSearchInput.TransportModeCodes = this.ShipmentSearchInput.TransportModeCodes.filter(e => e != code);
            this.ShipmentTypeFilters.filter(e => e.Code == code).map(e => e.IsSelected = false);
        } else {
            this.ShipmentSearchInput.TransportModeCodes.push(code);
            this.ShipmentTypeFilters.filter(e => e.Code == code).map(e => e.IsSelected = true);

        }
        this.LoadScreenData();

    }

    ShipmentDirectionChangedHandler(code: string) {
        RootContext.ShipmentsScrollPosition = 0;
        if (this.ShipmentSearchInput.DirectionCodes.includes(code)) {
            this.ShipmentSearchInput.DirectionCodes = this.ShipmentSearchInput.DirectionCodes.filter(e => e != code);
            this.ShipmentDirectionFilters.filter(e => e.Code == code).map(e => e.IsSelected = false);
        } else {
            this.ShipmentSearchInput.DirectionCodes.push(code);
            this.ShipmentDirectionFilters.filter(e => e.Code == code).map(e => e.IsSelected = true);

        }
        this.LoadScreenData();

    }

    SelectionMoreFilterChangedHandler(event) {
        switch (event) {
            case MoreFilterCodes.EstimatedArrivalOnly:
                this.ShipmentSearchInput.EstimatedArrivalOnly = !this.ShipmentSearchInput.EstimatedArrivalOnly;
                this.MoreFilterMobileValue.EstimatedArrivalOnly = this.ShipmentSearchInput.EstimatedArrivalOnly;
                break;
            case MoreFilterCodes.ExceptionOnly:
                this.ShipmentSearchInput.HasException = !this.ShipmentSearchInput.HasException;
                this.MoreFilterMobileValue.HasException = this.ShipmentSearchInput.HasException;
                break;
            case MoreFilterCodes.OrdersOnly:
                this.ShipmentSearchInput.OrdersOnly = !this.ShipmentSearchInput.OrdersOnly;
                this.MoreFilterMobileValue.OrdersOnly = this.ShipmentSearchInput.OrdersOnly;
                break;
            case MoreFilterCodes.OperationalOpenedOnly:
                this.ShipmentSearchInput.OperationalOpenedOnly = !this.ShipmentSearchInput.OperationalOpenedOnly;
                this.MoreFilterMobileValue.OperationalOpenedOnly = this.ShipmentSearchInput.OperationalOpenedOnly;
                break;

        }
        this.LoadScreenData();
    }
    private timerToken: any;
    onSearchChange() {

        this.timerToken = setTimeout(() => this.LoadScreenData(), 500);
        this.ShipmentSearchInput;
    }
    sortBySelectionChangedHandler(event) {
        switch (event) {
            case SortOptions.ASC:
                this.ShipmentSearchInput.SortType = event;
                this.sortByMultipleSelection.select.options.filter(d =>
                    d.value.Code == SortOptions.DESC).map(x => x.deselect());
                break;
            case SortOptions.DESC: {
                this.ShipmentSearchInput.SortType = event;
                this.sortByMultipleSelection.select.options.filter(d =>
                    d.value.Code == SortOptions.ASC).map(x => x.deselect());
                break;
            }
            case SortOptions.ATA:
                this.ShipmentSearchInput.SortFieldName = event;
                this.sortByMultipleSelection.select.options.filter(d =>
                    [SortOptions.CMD, SortOptions.ATD].includes(d.value.Code)).map(x => x.deselect());
                break;
            case SortOptions.ATD:
                this.ShipmentSearchInput.SortFieldName = event;
                this.sortByMultipleSelection.select.options.filter(d =>
                    [SortOptions.CMD, SortOptions.ATA].includes(d.value.Code)).map(x => x.deselect());
                break;
            case SortOptions.CMD: {
                this.ShipmentSearchInput.SortFieldName = event;
                this.sortByMultipleSelection.select.options.filter(d =>
                    [SortOptions.ATD, SortOptions.ATA].includes(d.value.Code)).map(x => x.deselect());
                break;
            }

        }
        this.LoadScreenData();
        this.showMobileSortMenu = false;
    }

    DeselectTransportModeFilter(code) {
        RootContext.ShipmentsScrollPosition = 0;
        this.ShipmentSearchInput.TransportModeCodes = this.ShipmentSearchInput.TransportModeCodes.filter(e => e != code)
        this.shipmentTypeMultipleSelection.DeselectFilter(code);
        this.ShipmentTypeFilters.filter(e => e.Code == code).map(e => e.IsSelected = false);
        this.LoadScreenData();

    }

    DeselectDirectionFilter(code) {
        RootContext.ShipmentsScrollPosition = 0;
        this.ShipmentSearchInput.DirectionCodes = this.ShipmentSearchInput.DirectionCodes.filter(e => e != code)
        this.shipmentDirectionMultipleSelection.DeselectFilter(code);
        this.ShipmentDirectionFilters.filter(e => e.Code == code).map(e => e.IsSelected = false);

        this.LoadScreenData();

    }

    ClearHasExceptionChanged(event) {
        RootContext.ShipmentsScrollPosition = 0;
        this.ShipmentSearchInput.HasException = this.MoreFilterMobileValue.HasException = event;
        this.shipmentMoreFiltersMultipleSelection.DeselectFilter(MoreFilterCodes.ExceptionOnly);
        this.LoadScreenData();
    }

    ClearOrdersOnlyChanged(event) {
        RootContext.ShipmentsScrollPosition = 0;
        this.ShipmentSearchInput.OrdersOnly = this.MoreFilterMobileValue.OrdersOnly = event;
        this.shipmentMoreFiltersMultipleSelection.DeselectFilter(MoreFilterCodes.OrdersOnly);
        this.LoadScreenData();
    }

    ClearEstimatedArrivalOnlyChanged(event) {
        RootContext.ShipmentsScrollPosition = 0;
        this.ShipmentSearchInput.EstimatedArrivalOnly = this.MoreFilterMobileValue.EstimatedArrivalOnly = event;
        this.shipmentMoreFiltersMultipleSelection.DeselectFilter(MoreFilterCodes.EstimatedArrivalOnly);
        this.LoadScreenData();
    }

    ClearOperationalOpenedOnlyChanged(event) {
        RootContext.ShipmentsScrollPosition = 0;
        this.ShipmentSearchInput.OperationalOpenedOnly = this.MoreFilterMobileValue.OperationalOpenedOnly = event;
        this.shipmentMoreFiltersMultipleSelection.DeselectFilter(MoreFilterCodes.OperationalOpenedOnly);
        this.LoadScreenData();
    }

    UnselectMiletone(code) {
        RootContext.ShipmentsScrollPosition = 0;
        this.MilestonesStatusDictionary[code].IsSelected = false;
        this.ShipmentSearchInput.MilestonesCodes = this.ShipmentSearchInput.MilestonesCodes.filter(e => e != code);
        this.LoadScreenData();

    }

    // Milestones filter
    OnMilestonesStatusFilterChanged(value, state) {
        state.IsSelected = value;
    }

    OnShipmentTypeFilterChanged(value, item: ToggleFilter) {
        item.IsSelected = value;
    }

    OnShipmentDirectionFilterChanged(value, item: ToggleFilter) {
        item.IsSelected = value;
    }

    private SetShipmentsScrollPosition() {
        const shipmentCardsContainer = document.getElementById('scrollArea');
        if (shipmentCardsContainer) {
            shipmentCardsContainer.scrollTop = 0;
        }

    }

    onSearchAdvancedFiltersChange(searchInput) {
        this.onSearchInvitedCustomersChange(searchInput);
        this.onSearchMilestonesStatusChange(searchInput);
        this.onSearchShipmentDirectionFiltersChange(searchInput);
        this.onSearchShipmentTypeFiltersChange(searchInput);
        this.SearchText = searchInput;
    }
    onSearchInvitedCustomersChange(searchInput) {
        if (searchInput) {
            var size = this.tempInvitedCustomers.length;
            this.InvitedCustomers = [];
            for (let i = 0; i < size; i++) {
                if (this.tempInvitedCustomers[i].Name.toUpperCase().match(searchInput.toUpperCase())) {
                    this.InvitedCustomers.push(this.tempInvitedCustomers[i]);
                }
            }
            this.InvitedCustomersNoResult = false;
            if (this.InvitedCustomers.length === 0)
                this.InvitedCustomersNoResult = true;
        }
        else {
            this.InvitedCustomers = [];
            this.tempInvitedCustomers.forEach(val => this.InvitedCustomers.push(val));
            this.InvitedCustomersNoResult = false;
        }
    }
    onSearchMilestonesStatusChange(searchInput) {
        if (searchInput) {
            var size = this.tempMilestonesStatus.length;
            this.MilestonesStatus = [];
            for (let i = 0; i < size; i++) {
                if (this.tempMilestonesStatus[i].EnglishName.toUpperCase().match(searchInput.toUpperCase())) {
                    this.MilestonesStatus.push(this.tempMilestonesStatus[i]);
                }
            }
            this.MilestonesStatusNoResult = false;
            if (this.MilestonesStatus.length === 0)
                this.MilestonesStatusNoResult = true;
        }
        else {
            this.MilestonesStatus = [];
            this.tempMilestonesStatus.forEach(val => this.MilestonesStatus.push(val));
            this.MilestonesStatusNoResult = false;
        }
    }
    onSearchShipmentDirectionFiltersChange(searchInput) {
        if (searchInput) {
            var size = this.tempShipmentDirectionFilters.length;
            this.ShipmentDirectionFilters = [];
            for (let i = 0; i < size; i++) {
                if (this.tempShipmentDirectionFilters[i].name.toUpperCase().match(searchInput.toUpperCase())) {
                    this.ShipmentDirectionFilters.push(this.tempShipmentDirectionFilters[i]);
                }
            }
            this.ShipmentDirectionFiltersNoResult = false;
            if (this.ShipmentDirectionFilters.length === 0)
                this.ShipmentDirectionFiltersNoResult = true;
        }
        else {
            this.ShipmentDirectionFilters = [];
            this.tempShipmentDirectionFilters.forEach(val => this.ShipmentDirectionFilters.push(val));
            this.ShipmentDirectionFiltersNoResult = false;
        }
    }
    onSearchShipmentTypeFiltersChange(searchInput) {
        if (searchInput) {
            var size = this.tempShipmentTypeFilters.length;
            this.ShipmentTypeFilters = [];
            for (let i = 0; i < size; i++) {
                if (this.tempShipmentTypeFilters[i].name.toUpperCase().match(searchInput.toUpperCase())) {
                    this.ShipmentTypeFilters.push(this.tempShipmentTypeFilters[i]);
                }
            }
            this.ShipmentTypeFiltersNoResult = false;
            if (this.ShipmentTypeFilters.length === 0)
                this.ShipmentTypeFiltersNoResult = true;
        }
        else {
            this.ShipmentTypeFilters = [];
            this.tempShipmentTypeFilters.forEach(val => this.ShipmentTypeFilters.push(val));
            this.ShipmentTypeFiltersNoResult = false;
        }
    }

    private SetDefaultBackgroundColor() {
        document.documentElement.style.setProperty('--BGColor', 'RGB(250,251,252)');
    }

    SetShipmentTypeAndDirectionTooltip(shipment: CargoTrackingShipmentList) {
        var type = "";
        var direction = "";
        switch (shipment.TransportModeId) {
            case 'A': {
                type = "Air"
                break;
            }

            case 'I': {
                type = "Inland"
                break;
            }

            case 'O': {
                type = "Ocean"
                break;
            }
        }

        switch (shipment.DirectionId) {
            case 'E': {
                direction = "Export "
                break;
            }

            case 'I':
            case 'C': {
                direction = "Import"
                break;
            }
        }

        this.ShipmentTypeAndDirectionTooltip = type + ' ' + direction;

    }

    SetTitleAndValueForSupplierOrClient(shipment: CargoTrackingShipmentList) {
        switch (shipment.DirectionId) {
            case ShipmentDirections.Import: {
                this.SupplierOrClientTitle = 'SHIPPER';
                this.SupplierOrClientValue = shipment.ShipperName;
                break;
            }
            case ShipmentDirections.Export: {
                this.SupplierOrClientTitle = 'CLIENT';
                this.SupplierOrClientValue = shipment.ConsigneeName;
                break;
            }
            case ShipmentDirections.Drop: {
                if (shipment.ShipmentNumber.startsWith('EF') || shipment.ShipmentNumber.startsWith('MF')) {
                    this.SupplierOrClientTitle = 'CLIENT';
                    this.SupplierOrClientValue = shipment.ConsigneeName;
                } else {
                    this.SupplierOrClientTitle = 'SHIPPER';
                    this.SupplierOrClientValue = shipment.ShipperName;
                }
                break;
            }
        }

        if (shipment.EntityType === this.EntityType_Customs) {
            this.SupplierOrClientTitle = 'SHIPPER';
            this.SupplierOrClientValue = shipment.ShipperName;
        }
    }

    QuantityAndWeight: string;

    public SetQuantityAndWeight(shipment: CargoTrackingShipmentList) {
        this.QuantityAndWeight = (shipment.NumberOfPackages ? shipment.NumberOfPackages + ' Units ' : '')
            + (shipment.NumberOfPackages && shipment.GrossWeight ? ' / ' : '')
            + (shipment.GrossWeight ? shipment.GrossWeight + ' ' + shipment.GrossWeightUnitCode : '');
    }

    private SetSupplierOrCleintValueByEntityType(shipment: CargoTrackingShipmentList, value: any) {
        if (shipment.EntityType === this.EntityType_Customs) {
            value = shipment.ShipperName;
        }
        return value;
    }

    SetShipmenTypeForRouting(shipment: CargoTrackingShipmentList): string {

        if (shipment.ShipmentLevelCode === 'D') {
            return 'Direct';
        } else if (shipment.ShipmentLevelCode === 'H') {
            return 'House';
        } else if (shipment.ShipmentTypeCode === 'FCL') {
            return 'FCL';
        } else if (shipment.ShipmentTypeCode === 'LCL') {
            return 'LCL';
        }
    }

    private InitComponent() {
        this.InitForm();
    }


    SetMoreReferenceText(reference: string) {
        var allreferences = reference?.split(',');
        allreferences = allreferences.filter((el, i, a) => i === a.indexOf(el));
        if (allreferences?.length > 4) {

            var morereferences = allreferences.slice(3, allreferences.length + 1)
            this.MoreReferenceText = morereferences.join(',');

        }
    }

    private GetMilstones() {
        this.milestonesService.getAll(this.tenant)
            .subscribe((milestones: any) => {
                this.MilestonesStatus = milestones;
                this.tempMilestonesStatus = milestones;
                this.FillMilestoneDictionary(milestones);
            });
    }

    FillMilestoneDictionary(milestones: any[]) {
        milestones.forEach(element => {
            this.MilestonesStatusDictionary[element.Code] = element
        });

    }

    private InitForm() {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });
        this.mobileSearchForm = this.formBuilder.group({
            PanelSearchText: ''
        });
    }


    Search() {
        this.LoadScreenData();
    }

    ItemClicked(item) {
        var selection = window.getSelection();
        if (selection.toString().length === 0) {
            var SecurityKey = item.SecurityKey;
            SessionInfo.ShipmentsFilters = this.BuildShipmentFilters();

            this.router.navigate(['cargo-tracking', 'shipment', SecurityKey]);
        }

        this.SaveShipmentsScrollPosition();
    }

    private SaveShipmentsScrollPosition() {
        const shipmentCardsContainer = document.getElementById('scrollArea');
        RootContext.ShipmentsScrollPosition = shipmentCardsContainer.scrollTop;
    }

    ShipmentsCounter: CargoTrackingShipmentsCounter = new CargoTrackingShipmentsCounter();

    LoadScreenData() {

        if (this.tenant) {
            this.ShipmentSearchInput.Tenant = this.tenant;
            var shipmentFilters = this.BuildShipmentFilters();
            this.LoadShipments();
            this.SetShipmentsScrollPosition();
        }
    }

    references: string[];

    private LoadShipments() {
        if (this.ShipmentsDataSource) {
            this.ReloadShipments();
        } else {
            this.InitiateShipmentDataSource();
        }
    }

    private async filterWithAllCustomersWhenCustomersNotSelected() {
        let filter: CargoTrackingShipmentSearchInput = Object.assign({}, this.ShipmentSearchInput);
        filter.CustomersIds = filter.CustomersIds.length == 0 ? this.InvitedCustomers.map(d => d.CardId) : filter.CustomersIds;        
        filter.FromDate = await this.getFromDate();

        return filter;
    }

    private async getFromDate() {
        const fromDate = new Date();
        this.backMonths = this.backMonths || await this.tenantManagementService.getShipmentBuildMonth();
        
        fromDate.setMonth(fromDate.getMonth() - this.backMonths);
        return fromDate;
    }

    private async InitiateShipmentDataSource() {
        let filter = await this.filterWithAllCustomersWhenCustomersNotSelected();
        this.ShipmentsDataSource = new ShipmentDataSource(this.changeDetector, this.searchService, filter, this);
        let s = SessionInfo.LoggedUserCompanyLogins.filter(a => a.Tenant == this.tenant)[0];
        if (SessionInfo.LoggedUserCompanyLogins.filter(a => a.Tenant == this.tenant)[0].IsUser === true) {
            this.ShipmentsDataSource.GetShipmentsCustomers(this.tenant);
        }
    }

    private async ReloadShipments() {
        let filter = await this.filterWithAllCustomersWhenCustomersNotSelected();
        this.ShipmentsDataSource.ReloadData(filter);
        this.ResetShipmentsScrollbarPosition();
    }


    private ResetShipmentsScrollbarPosition() {
        this.virtualScroll.scrollToIndex(0);
    }


    private BuildShipmentFilters() {
        var shipmentFilters = new CargoTrackingShipmentSearchInput();
        shipmentFilters.Tenant = this.tenant;
        return shipmentFilters;
    }

    BuildShipmentReferences(shipment: CargoTrackingShipmentList) {
        this.references = shipment.CustomerReference != null ? shipment.CustomerReference.split(',').filter(d => d) : [];
        this.references = this.references.map((el) => {
            return el.trim();
        });
        this.references = this.references.filter((el, i, a) => i === a.indexOf(el));
    }


    masterLabel = 'Master';
    houseLabel = 'House';

    SetMasterOrHouseLabel(shipment: CargoTrackingShipmentList) {
        if (shipment.EntityType == ShipmentEntityTypes.Customs && shipment.ForwardingShipmentHeaderId && shipment.ForwardingShipmentLevelCode == ShipmentLevelCodes.House && shipment.ForwardingHouse) {
            return this.houseLabel;
        } else if (shipment.EntityType == ShipmentEntityTypes.Customs && shipment.ForwardingShipmentHeaderId && shipment.ForwardingShipmentLevelCode == ShipmentLevelCodes.Direct && shipment.ForwardingMaster) {
            return this.masterLabel;
        } else if (shipment.EntityType == ShipmentEntityTypes.Forwarding && shipment.ShipmentLevelCode == ShipmentLevelCodes.House && shipment.House) {
            return this.houseLabel;
        } else if (shipment.EntityType == ShipmentEntityTypes.Forwarding && shipment.ShipmentLevelCode == ShipmentLevelCodes.Direct && shipment.Master) {
            return this.masterLabel;
        } else if (shipment.House) {
            return this.houseLabel;
        } else if (shipment.Master) {
            return this.masterLabel;
        }
        return null;

    }

    SetMasterOrHouseValue(shipment: CargoTrackingShipmentList) {
        if (shipment.EntityType == ShipmentEntityTypes.Customs && shipment.ForwardingShipmentHeaderId && shipment.ForwardingShipmentLevelCode == ShipmentLevelCodes.House && shipment.ForwardingHouse) {
            return shipment.ForwardingHouse;
        } else if (shipment.EntityType == ShipmentEntityTypes.Customs && shipment.ForwardingShipmentHeaderId && shipment.ForwardingShipmentLevelCode == ShipmentLevelCodes.Direct && shipment.ForwardingMaster) {
            return shipment.ForwardingMaster;
        } else if (shipment.EntityType == ShipmentEntityTypes.Forwarding && shipment.ShipmentLevelCode == ShipmentLevelCodes.House && shipment.House) {
            return shipment.House;
        } else if (shipment.EntityType == ShipmentEntityTypes.Forwarding && shipment.ShipmentLevelCode == ShipmentLevelCodes.Direct && shipment.Master) {
            return shipment.Master;
        } else if (shipment.House) {
            return shipment.House;
        } else if (shipment.Master) {
            return shipment.Master;
        }

        return null;

    }


    SetEstimationORActualDate(shipment: CargoTrackingShipmentList) {
        if (shipment.ArrivalDate != null) {
            this.TitleOfEstimationORActualDate = 'ATA';
            this.ValueOfEstimationORActualDate = shipment.ArrivalDate;
        } else if (shipment.ArrivalEstimationDate != null) {
            this.TitleOfEstimationORActualDate = 'ETA';
            this.ValueOfEstimationORActualDate = shipment.ArrivalEstimationDate;
        } else if (shipment.DepartureDate != null) {
            this.TitleOfEstimationORActualDate = 'ATD';
            this.ValueOfEstimationORActualDate = shipment.DepartureDate;
        } else if (shipment.DepartureEstimationDate != null) {
            this.TitleOfEstimationORActualDate = 'ETD';
            this.ValueOfEstimationORActualDate = shipment.DepartureEstimationDate;
        } else {
            this.TitleOfEstimationORActualDate = 'ATA';
            this.ValueOfEstimationORActualDate = null;
        }
    }

    OpenMessageWindow(references) {
        let referencesArray = references != null ? references.split(',') : null;
        referencesArray = referencesArray.slice(1, referencesArray.length + 1);
        referencesArray = referencesArray.map(x => x.trim());
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'References',

                description: referencesArray.toString().split(',').join("\n"),
            }
        });
    }

    OpenReferencesMessageWindow(references: any[], isMobile: boolean, event: MouseEvent) {
        if (!references)
            return;

        references = references.filter(d => d).map(x => x.trim());
        references = references.filter((el, i, a) => i === a.indexOf(el));
        this.dialog.open(MessageWindowComponent, {
            data: {
               
                title: 'References',
                description: isMobile ? references.join("\n") : references.slice(3, references.length + 1).join("\n"),
            },

            position: {
                top: event.clientY + 'px',
                left: event.clientX + 'px',
            },

        });
    }

    OpenExceptionMessageWindow(messageDescription) {
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'Exception',
                description: messageDescription,
            }
        });
    }

    GetModeIcon(mode: string) {
        var iconPath = "";

        switch (mode) {
            case shipmentTypeCodes.Air:
                iconPath = "./assets/images/misc/plane.svg";
                break;
            case shipmentTypeCodes.Ocean:
                iconPath = "./assets/images/misc/ship.svg";
                break;
            case shipmentTypeCodes.InLand:
                iconPath = "./assets/images/misc/Truck.svg";
                break;
            default:
                break;
        }
        return iconPath;
    }

    ShipmentDirectionFilters: ToggleFilter[] = [
        new ToggleFilter(ShipmentDirectionCodes.Import, 'Import'),
        new ToggleFilter(ShipmentDirectionCodes.Export, 'Export'),
        new ToggleFilter(ShipmentDirectionCodes.Drop, 'Drop')
    ];
    ShipmentTypeFilters: ToggleFilter[] = [
        new ToggleFilter(shipmentTypeCodes.Air, 'Air'),
        new ToggleFilter(shipmentTypeCodes.InLand, 'Land'),
        new ToggleFilter(shipmentTypeCodes.Ocean, 'Ocean'),
    ];
    ShipmentMoreFilters: ToggleFilter[] = [
        new ToggleFilter(MoreFilterCodes.ExceptionOnly, 'Exception Only'),
        new ToggleFilter(MoreFilterCodes.OrdersOnly, 'Orders Only'),
        new ToggleFilter(MoreFilterCodes.EstimatedArrivalOnly, 'Estimated Arrival Only'),
        new ToggleFilter(MoreFilterCodes.OperationalOpenedOnly, 'Operational Opened Only')
    ];


    sortByOptions: ToggleFilter[] = [
        new ToggleFilter(SortOptions.CMD, 'Current Status Date', null),
        new ToggleFilter(SortOptions.ATA, 'ATA/ETA', null),
        new ToggleFilter(SortOptions.ATD, 'ATD/ETD', null),
        new ToggleFilter(SortOptions.ASC, 'Ascending', null),
        new ToggleFilter(SortOptions.DESC, 'Descending', null),
    ];
    ShipmentDirectionFiltersDictionary: { [key: string]: ToggleFilter } = {};
    ShipmentShipmentTypeFiltersDictionary: { [key: string]: ToggleFilter } = {};

    fillFeltersDictionary() {
        this.ShipmentDirectionFilters.forEach(element => {
            this.ShipmentDirectionFiltersDictionary[element.Code] = element;
        });
        this.ShipmentTypeFilters.forEach(element => {
            this.ShipmentShipmentTypeFiltersDictionary[element.Code] = element;
        });
        this.ShipmentDirectionFilters.forEach(val => this.tempShipmentDirectionFilters.push(val));
        this.ShipmentTypeFilters.forEach(val => this.tempShipmentTypeFilters.push(val));

    }

    setCounterForshipmentTypeMultipleSelect(toggleFilter: ToggleFilter): number {
        if (toggleFilter.Code == shipmentTypeCodes.Air) {
            return this.ShipmentsCounter.Air;
        } else if (toggleFilter.Code == shipmentTypeCodes.InLand) {
            return this.ShipmentsCounter.Land;
        } else if (toggleFilter.Code == shipmentTypeCodes.Ocean) {
            return this.ShipmentsCounter.Sea;
        }
    }

    setCounterForDirectionMultipleSelect(toggleFilter: ToggleFilter) {
        if (toggleFilter.Code == ShipmentDirectionCodes.Import) {
            return this.ShipmentsCounter.Import;
        } else if (toggleFilter.Code == ShipmentDirectionCodes.Drop) {
            return this.ShipmentsCounter.Drop;
        } else if (toggleFilter.Code == ShipmentDirectionCodes.Export) {
            return this.ShipmentsCounter.Export;
        }
    }

    setCounterForMoreFiltersMultipleSelect(toggleFilter: ToggleFilter) {
        if (toggleFilter.Code == MoreFilterCodes.ExceptionOnly) {
            return this.ShipmentsCounter.HasException;
        } else if (toggleFilter.Code == MoreFilterCodes.OrdersOnly) {
            return this.ShipmentsCounter.OrdersOnly;
        } else if (toggleFilter.Code == MoreFilterCodes.EstimatedArrivalOnly) {
            return this.ShipmentsCounter.EstimatedArrivalOnly;
        } else if (toggleFilter.Code == MoreFilterCodes.OperationalOpenedOnly) {
            return this.ShipmentsCounter.OperationalOpenedOnly;
        }
    }

    UnselectCustomer(customerId) {

        RootContext.ShipmentsScrollPosition = 0;

        const index = this.InvitedCustomers.findIndex(obj => {
            return obj.CardId === customerId;
        });
        this.InvitedCustomers[index].IsSelected = false;

        this.ShipmentSearchInput.CustomersIds = this.InvitedCustomers.filter(e => e.IsSelected).map(d => d.CardId);

        this.LoadScreenData();
    }

    ClearFilters() {
        this.ClearAdvancedFilters();
        this.shipmentTypeMultipleSelection.ClearFilters();
        this.shipmentDirectionMultipleSelection.ClearFilters();
        this.shipmentMoreFiltersMultipleSelection.ClearFilters();
        this.InvitedCustomers.map(e => e.IsSelected = false);
        this.MilestonesStatus.map(e => e.IsSelected = false);
        this.ShipmentSearchInput.CustomersIds = this.InvitedCustomers.filter(e => e.IsSelected).map(d => d.CardId);
        this.ShipmentSearchInput.DirectionCodes = [];
        this.ShipmentSearchInput.MilestonesCodes = [];
        this.ShipmentSearchInput.TransportModeCodes = [];
        this.ShipmentSearchInput.EstimatedArrivalOnly = this.MoreFilterMobileValue.EstimatedArrivalOnly = false;
        this.ShipmentSearchInput.HasException = this.MoreFilterMobileValue.HasException = false;
        this.ShipmentSearchInput.OrdersOnly = this.MoreFilterMobileValue.OrdersOnly = false;
        this.ShipmentSearchInput.OperationalOpenedOnly = this.MoreFilterMobileValue.OperationalOpenedOnly = false;
        this.LoadScreenData();
    }

    ClearAdvancedFilters() {
        this.InvitedCustomers.map(e => e.IsSelected = false);
        this.MilestonesStatus.map(e => e.IsSelected = false);
        this.ShipmentDirectionFilters.map(e => e.IsSelected = false);
        this.ShipmentTypeFilters.map(e => e.IsSelected = false);
        this.MoreFilterMobileValue.EstimatedArrivalOnly = false;
        this.MoreFilterMobileValue.HasException = false;
        this.MoreFilterMobileValue.OrdersOnly = false;
        this.MoreFilterMobileValue.OperationalOpenedOnly = false;
    }

    ApplyFilterButtonClicked() {
        this.sharedService.updateValue(false);
        this.ShipmentSearchInput.CustomersIds = this.InvitedCustomers.filter(e => e.IsSelected).map(d => d.CardId);

        this.ShipmentSearchInput.MilestonesCodes = this.MilestonesStatus.filter(e => e.IsSelected).map(state => state.Code);
        if (this.isMobileView) {
            this.ShipmentSearchInput.DirectionCodes = this.ShipmentDirectionFilters.filter(e => e.IsSelected).map(e => e.Code);
            this.ShipmentSearchInput.TransportModeCodes = this.ShipmentTypeFilters.filter(e => e.IsSelected).map(e => e.Code);
            this.MapMoreMobileFilter();
        }

        RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();
    }

    MapMoreMobileFilter() {
        this.ShipmentSearchInput.EstimatedArrivalOnly = this.MoreFilterMobileValue.EstimatedArrivalOnly;
        this.ShipmentSearchInput.HasException = this.MoreFilterMobileValue.HasException;
        this.ShipmentSearchInput.OrdersOnly = this.MoreFilterMobileValue.OrdersOnly;
        this.ShipmentSearchInput.OperationalOpenedOnly = this.MoreFilterMobileValue.OperationalOpenedOnly;
    }

    CheckFiltersExists() {
        return this.ShipmentSearchInput.CustomersIds.length > 0
            || this.ShipmentSearchInput.DirectionCodes.length > 0
            || this.ShipmentSearchInput.MilestonesCodes.length > 0
            || this.ShipmentSearchInput.TransportModeCodes.length > 0
            || this.ShipmentSearchInput.EstimatedArrivalOnly
            || this.ShipmentSearchInput.HasException
            || this.ShipmentSearchInput.OrdersOnly
            || this.ShipmentSearchInput.OperationalOpenedOnly
    }

    SortMenuClicked(buttonCode: string) {
        // this.LoadScreenData();
    }

    lastClickedShipment: any;

    ShipmentMoreButtonClicked(shipment: any, event: any) {
        event.preventDefault();
        event.stopPropagation();

        this.lastClickedShipment = shipment;
        this.showShipmentDetailsMenu = true;
    }

    ShipmentDetailsMenuClicked(buttonCode: string) {

        if (buttonCode == "set")
            this.lastClickedShipment.IsFavorite = true;

    }

    ShipmentsCount: number = 0;
    ShipmentsCustomers: any;

    ShipmentsLoadingError: string;

    OpenAdvancedFiltersSidebar() {

        this.sharedService.updateValue(true);
    }

    OnCustomerValueChanged(value, customer) {
        customer.IsSelected = value;
    }


    ShowSearchPanel() {
        this.ShowMobileSearch = true;
        this.FocusOnMobileSearchBox();
    }

    HideSearchPanel() {
        this.ShowMobileSearch = false;
        if (!this.ShipmentSearchInput.SearchText) {
            this.Shipments = [];
            RootContext.LastSearchText = this.ShipmentSearchInput.SearchText;
            this.LoadScreenData();
        } else {
            this.Search();
        }
    }

    ToggleSearchPanel() {
        this.ShowMobileSearch = !this.ShowMobileSearch;

        if (this.ShowMobileSearch)
            this.FocusOnMobileSearchBox();
    }

    private FocusOnMobileSearchBox() {
        setTimeout(() => {
            this.mobileSearchInput?.nativeElement?.focus();
        }, 50);
    }

    GetSlice(text: string, numberOfCharacter) {

        var result = text
        if (text?.length > numberOfCharacter) {
            result = text.slice(0, numberOfCharacter) + "..."
        }
        return result;

    }

    maxNumberOfCarachter = 15;

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        this.setMaxNumberOfCarachter(event.srcElement.innerWidth);
        this.setViews();
    }

    RerenderVirtualScroll() {
        this.virtualScroll.checkViewportSize();
    }

    setViews() {
        if (this.IsWebView) {
            this.isMobileView = window.innerWidth <= 479;
        } else {
            const isLandscapeView = window.innerWidth > window.innerHeight;
            if (isLandscapeView) {
                this.isMobileView = window.innerHeight <= 479;
            } else {

                this.isMobileView = window.innerWidth <= 479;
            }
        }
    }

    get IsWebView() {
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))
            return false;
        else
            return true;
    }

    setMaxNumberOfCarachter(width) {
        if (width <= 1024 && width > 768) {
            this.maxNumberOfCarachter = 10
        } else if (width <= 768 && width > 425) {
            this.maxNumberOfCarachter = 9;

        } else {
            this.maxNumberOfCarachter = 15
        }
    }


    GetCustomerReferencesItems(shipment) {
        return shipment.CustomerReference?.split(',').filter(d => d);
    }

}

export class ToggleFilter {
    constructor(code: string, name: string, count: number = 0) {
        this.Code = code;
        this.Name = name;
        this.Count = count;
        this.IsSelected = false;
    }

    public IsSelected: boolean;

    private name: string;

    public get Name(): string {
        return this.name;
    }

    public set Name(v: string) {
        this.name = v;
    }


    private count: number = 0;

    public get Count(): number {
        return this.count;
    }

    public set Count(v: number) {
        this.count = v;
    }


    private code: string;

    public get Code(): string {
        return this.code;
    }

    public set Code(v: string) {
        this.code = v;
    }


}

export class CargoTrackingShipmentsCounter {

    Import: number = 0;
    Export: number = 0;
    Drop: number = 0;
    Air: number = 0;
    Land: number = 0;
    Sea: number = 0;
    HasException: number = 0;
    OrdersOnly: number = 0;
    EstimatedArrivalOnly: number = 0;
    OperationalOpenedOnly: number = 0;
}

enum Milestones {
    Booking = "1",
    Pickup = "2",
    FromWarehouse = "3",
    Departure = "4",
    Arrival = "5",
    ToWarehouse = "6",
    AssignedToCustomsBroker = "7",
    CustomsProcess = "8",
    GoodsClassification = "9",
    DocumentInspection = "10",
    CustomsPayment = "11",
    Clearance = "12",
    GatepassArrived = "13",
    AssignedtoTrucker = "14",
    DeliveryOut = "15",
    Delivered = "16",
    Invoiced = "17"
}

export enum SortOptions {
    CMD = "CMD",
    ATA = "ATA",
    ATD = "ATD",
    ASC = "ASC",
    DESC = "DESC"
}

export enum ShipmentLevelCodes {
    House = 'H',
    Direct = 'D',
    Customs = 'A'
}

export enum ShipmentEntityTypes {
    Order = 'O',
    Customs = 'C',
    Forwarding = 'F'
}

export enum MoreFilterCodes {
    ExceptionOnly = 'ExceptionOnly',
    OrdersOnly = 'OrdersOnly',
    EstimatedArrivalOnly = 'EstimatedArrivalOnly',
    OperationalOpenedOnly = 'OperationalOpenedOnly'
}

export enum ShipmentDirectionCodes {
    Import = 'I',
    Export = 'E',
    Drop = 'R',
    CustomsImport = 'C'

}

export enum shipmentTypeCodes {
    Air = 'A',
    Ocean = 'O',
    InLand = 'I'

}
