
import { Component, ViewChild, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef, OnInit, Injectable, HostListener } from '@angular/core';
import { Router, ActivatedRoute, NavigationStart, NavigationEnd } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from '../../../../Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from '../../../../EntityLists/CargoTrackingShipmentList';
import { SessionInfo } from '../../../../../Infrastructure/Utilities/SessionInfo';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { ShipmentDataSource } from '../../../../DataContracts/CargoTrackingShipmentDataSource';
import { CargoTrackingShipmentSearchInput, MoreFilter } from '../../../../DataContracts/CargoTrackingShipmentFilters';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { CargoTrackingPortService } from '../../../../Services/Others/CargoTrackingPortService';
import { CargoTrackingShipmentService } from '../../../../Services/Others/CargoTrackingShipmentService';
import { MessageWindowComponent } from '../../../../../Infrastructure/Components/MessageWindow/MessageWindowComponent';
import { MatDialog } from '@angular/material/dialog';
import { RootContext } from 'src/CargoTracking/Utilities/RootContext';
import { CargoTrackingMilestoneService } from 'src/CargoTracking/Services/Others/CargoTrackingMilestoneService';
import { MultipleSelectionComponent } from 'src/Infrastructure/Components/MultipleSelection/MultipleSelectionComponent';
import { filter } from 'rxjs/operators';
import { ShipmentDirections } from '../ShipmentDetails/ShipmentDetailsComponent';
import { ReplaySubject } from 'rxjs';
import { SharedService } from 'src/CargoTracking/Services/Others/SharedService';

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

    isLoading: boolean = false;
    isFilter1Expanded: boolean = false;
    isFilter2Expanded: boolean = false;
    isMilestonesStatusFilterExpanded: boolean = false;
    isShipmentTypeFilterExpanded: boolean = false;
    isShipmentDirectionFilterExpanded: boolean = false;
    isAbdullahCompanyChecked: boolean = true;
    showMobileSortMenu: boolean = false;
    showShipmentDetailsMenu: boolean = false;
    ShipmentsDataSource;
    @ViewChild(CdkVirtualScrollViewport) virtualScroll: CdkVirtualScrollViewport;
    @ViewChild('input') searchInput: ElementRef;
    @ViewChild('mobileSearch') mobileSearchInput: ElementRef;
    @ViewChild('shipmentTypeMultipleSelection') shipmentTypeMultipleSelection: MultipleSelectionComponent;
    @ViewChild('shipmentDirectionMultipleSelection') shipmentDirectionMultipleSelection: MultipleSelectionComponent;
    @ViewChild('shipmentMoreFiltersMultipleSelection') shipmentMoreFiltersMultipleSelection: MultipleSelectionComponent;
    @ViewChild('sortByMultipleSelection') sortByMultipleSelection: MultipleSelectionComponent;
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
    ShipmenTypeForRouting: string;
    public SortOptions = SortOptions;
    MasterOrHouseLabel: string = "";
    EntityType_Customs = "C";
    isMobileView: boolean;
    get tenant() {
        return CargoTrackingBrandingData.Tenant;
    }
    FiltersSelectedInvitedCustoms: any[] = [];
    ShipmentSearchInput: CargoTrackingShipmentSearchInput = new CargoTrackingShipmentSearchInput();
    MilestonesStatus: any[] = [];
    MilestonesStatusDictionary: {} = {};

    InvitedCustomers: any[] = [];
    InvitedCustomersDictionary: {} = {};
    MoreFilterMobileValue: MoreFilter = new MoreFilter();
    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private changeDetector: ChangeDetectorRef,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService,
        public dialog: MatDialog,
        private searchService: CargoTrackingSearchService,
        private milestonesService: CargoTrackingMilestoneService,
        public sharedService: SharedService) {
        this.InitComponent();
        this.SetDefaultBackgroundColor();
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

    private GetCompanyLoginsFromCache() {
        SessionInfo.LoggedUserCompanyLogins = JSON.parse(sessionStorage.getItem("LoggedUserCompanyLogins"));
        this.GetInvitedCustomers();
    }
    private GetInvitedCustomers() {
        this.InvitedCustomers = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => (
                {
                    IsSelected: false,
                    Name: d.CompanyName.substring(0, d.CompanyName.lastIndexOf('(')),
                    ...d
                }
            ));
        this.FillInvitedCustomersDictionary(this.InvitedCustomers);
    }
    FillInvitedCustomersDictionary(InvitedCustomers: any[]) {
        InvitedCustomers.forEach(element => {
            this.InvitedCustomersDictionary[element.CardId] = element;
        });
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
    UnselectCustomer(customerId) {
        RootContext.ShipmentsScrollPosition = 0;
        this.InvitedCustomersDictionary[customerId].IsSelected = false;
        this.ShipmentSearchInput.CustomersIds = this.ShipmentSearchInput.CustomersIds.filter(e => e != customerId);
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
        const shipmentCardsContainer = document.getElementById("scrollArea");
        if (shipmentCardsContainer) {
            shipmentCardsContainer.scrollTop = RootContext.ShipmentsScrollPosition || 0;
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

            case 'I': {
                direction = "Import"
                break;
            }
        }

        this.ShipmentTypeAndDirectionTooltip = type + ' ' + direction;

    }

    SetTitileAndValueForSupplierOrClient(shipment: CargoTrackingShipmentList) {
        this.SetTitleForSupplierOrClient(shipment);
        this.SetValueForSupplierOrClient(shipment);
    }


    SetTitleForSupplierOrClient(shipment: CargoTrackingShipmentList) {
        var title;
        switch (shipment.DirectionId) {
            case ShipmentDirections.Import: {
                title = "SHIPPER"
                break;
            }

            case ShipmentDirections.Export: {
                title = "CLIENT"
                break;
            }
        }

        if (shipment.EntityType == this.EntityType_Customs) {
            title = "SHIPPER"
        }

        this.SupplierOrClientTitle = title;
    }

    SetValueForSupplierOrClient(shipment: CargoTrackingShipmentList) {
        var value;
        value = this.SetSupplierOrCleintValueByDirection(shipment, value);
        value = this.SetSupplierOrCleintValueByEntityType(shipment, value);

        this.SupplierOrClientValue = value;
    }

    private SetSupplierOrCleintValueByDirection(shipment: CargoTrackingShipmentList, value: any) {
        switch (shipment.DirectionId) {
            case 'I': {
                value = shipment.ShipperName;
                break;
            }

            case 'E': {
                value = shipment.ConsigneeName;
                break;
            }
        }
        return value;
    }

    private SetSupplierOrCleintValueByEntityType(shipment: CargoTrackingShipmentList, value: any) {
        if (shipment.EntityType == this.EntityType_Customs) {
            value = shipment.ShipperName;
        }
        return value;
    }

    SetShipmenTypeForRouting(shipment: CargoTrackingShipmentList) {

        if (shipment.ShipmentLevelCode == 'D') {
            this.ShipmenTypeForRouting = "Direct"
        }

        else if (shipment.ShipmentLevelCode == 'H') {
            this.ShipmenTypeForRouting = "House"
        }

        else if (shipment.ShipmentTypeCode == "FCL") {
            this.ShipmenTypeForRouting = "FCL"
        }

        else if (shipment.ShipmentTypeCode == "LCL") {
            this.ShipmenTypeForRouting = "LCL"
        }
    }




    private InitComponent() {
        this.InitForm();
    }



    SetMoreReferenceText(reference: string) {
        var allreferences = reference?.split(',');
        if (allreferences?.length > 4) {

            var morereferences = allreferences.slice(4, allreferences.length + 1)
            this.MoreReferenceText = morereferences.join(',');

        }
    }

    private GetMilstones() {
        this.milestonesService.getAll(this.tenant)
            .subscribe((milestones: any) => {
                this.MilestonesStatus = milestones;
                this.FillMilestoneDictionary(milestones);
            });
    }
    FillMilestoneDictionary(milestones: any[]) {
        milestones.forEach(element => {
            this.MilestonesStatusDictionary[element.Code] = element
        });

    }


    private AddDemoCustomersForTest() {
        var demoCustomer1 = {
            CardType: 'CS',
            CardId: '1-711',
            Tenant: this.tenant,
            CompanyName: 'Customer 711'
        };
        var demoCustomer2 = {
            CardType: 'CS',
            CardId: '1-749',
            Tenant: this.tenant,
            CompanyName: 'Customer 749'
        };
        SessionInfo.LoggedUserCompanyLogins.push(demoCustomer1, demoCustomer2);
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
        const shipmentCardsContainer = document.getElementById("scrollArea");
        RootContext.ShipmentsScrollPosition = shipmentCardsContainer.scrollTop;
    }

    ShipmentsCounter: CargoTrackingShipmentsCounter = new CargoTrackingShipmentsCounter();
    LoadScreenData() {
        if (this.tenant) {
            this.ShipmentSearchInput.Tenant = this.tenant;
            var shipmentFilters = this.BuildShipmentFilters();
            this.LoadShipments();
            this.LoadShipmentsCounter();
            this.SetShipmentsScrollPosition();
        }
    }
    references: string[];
    private LoadShipments() {
        if (this.ShipmentsDataSource)
            this.ReloadShipments();

        else {
            this.InitiateShipmentDataSource();
        }
    }

    private LoadShipmentsCounter() {
        this.searchService.GetUserShipmentsCounter(this.ShipmentSearchInput).subscribe((counter: any) => {
            this.ShipmentsCounter = counter;
            this.BuildToggleFilters();
            this.SetShipmentsScrollPosition();
        });
    }


    private InitiateShipmentDataSource() {
        this.ShipmentsDataSource = new ShipmentDataSource(this.changeDetector, this.searchService, this.ShipmentSearchInput, this);
    }

    private ReloadShipments() {
        this.ShipmentsDataSource.ReloadData(this.ShipmentSearchInput);
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









    SplitReference(reference: string) {
        this.references = reference != null ? reference.split(',') : null;

    }
    SetMasterOrHouseLabel(shipment: CargoTrackingShipmentList) {
        this.MasterOrHouseLabel = '';
        var isShipmentDirectOrder = shipment.ShipmentLevelCode == ShipmentLevelCodes.Direct && shipment.EntityType == ShipmentEntityTypes.Order;
        var isShipmentHouseOrder = shipment.ShipmentLevelCode == ShipmentLevelCodes.House && shipment.EntityType == ShipmentEntityTypes.Order;

        if (shipment.ForwardingShipmentLevelCode == ShipmentLevelCodes.Direct || isShipmentDirectOrder)
            this.SetMasterOrHouseLabelForDirect(shipment);

        else if (shipment.ForwardingShipmentLevelCode == ShipmentLevelCodes.House || isShipmentHouseOrder)
            this.SetMasterOrHouseLabelForHouse(shipment);

        else if (shipment.EntityType == ShipmentEntityTypes.Customs && !shipment.ForwardingShipmentHeaderId)
            this.SetMasterOrHouseLabelForCustoms(shipment);
        return this.MasterOrHouseLabel;

    }
    private SetMasterOrHouseLabelForCustoms(shipment: CargoTrackingShipmentList) {
        if (shipment.House)
            this.MasterOrHouseLabel = "House";
        else if (shipment.Master)
            this.MasterOrHouseLabel = "Master";
    }

    private SetMasterOrHouseLabelForHouse(shipment: CargoTrackingShipmentList) {
        if (shipment.House)
            this.MasterOrHouseLabel = "House";
        else if (shipment.Master)
            this.MasterOrHouseLabel = "Master";
    }

    private SetMasterOrHouseLabelForDirect(shipment: CargoTrackingShipmentList) {
        if (shipment.Master)
            this.MasterOrHouseLabel = "Master";
        else if (shipment.House != null)
            this.MasterOrHouseLabel = "House";
    }

    SetConsignmentNumber(shipment: CargoTrackingShipmentList) {
        if (shipment.ShipmentLevelCode == 'D') {
            this.ConsignmentNumber = shipment.Master != null ? shipment.Master : shipment.House;
        }

        else if (shipment.ShipmentLevelCode == 'H') {
            this.ConsignmentNumber = shipment.House != null ? shipment.House : shipment.Master;;
        }
    }

    GetConsignmentNumberForCustomsForwardingShipment(shipment: CargoTrackingShipmentList) {
        if (shipment.ForwardingShipmentLevelCode == 'D') {
            this.ConsignmentNumber = shipment.ForwardingMaster;
        }

        else if (shipment.ForwardingShipmentLevelCode == 'H') {
            this.ConsignmentNumber = shipment.ForwardingHouse;
        }
        return this.ConsignmentNumber;
    }
    SetEstimationORActualDate(shipment: CargoTrackingShipmentList) {
        if (shipment.ArrivalDate != null) {
            this.TitleOfEstimationORActualDate = 'ATA'
            this.ValueOfEstimationORActualDate = shipment.ArrivalDate;
        }

        else if (shipment.ArrivalEstimationDate != null) {
            this.TitleOfEstimationORActualDate = 'ETA'
            this.ValueOfEstimationORActualDate = shipment.ArrivalEstimationDate;
        }

        else if (shipment.DepartureDate != null) {
            this.TitleOfEstimationORActualDate = 'ATD'
            this.ValueOfEstimationORActualDate = shipment.DepartureDate;
        }

        else if (shipment.DepartureEstimationDate != null) {
            this.TitleOfEstimationORActualDate = 'ETD'
            this.ValueOfEstimationORActualDate = shipment.DepartureEstimationDate;
        }
        else {
            this.TitleOfEstimationORActualDate = 'ATA'
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

    OpenReferencesMessageWindow(references, isMobile: boolean) {
        references = references.map(x => x.trim());
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'References',
                description: isMobile ? references.join("\n") : references.slice(3, references.length + 1).join("\n"),
            }
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
        new ToggleFilter(ShipmentDirectionCodes.Drop, 'Drop'),
        new ToggleFilter(ShipmentDirectionCodes.Domestic, 'Domestic'),
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
        new ToggleFilter(SortOptions.CMD, 'Current Status Date',null),
        new ToggleFilter(SortOptions.ATA, 'ATA/ETA',null),
        new ToggleFilter(SortOptions.ATD, 'ATD/ETD',null),
        new ToggleFilter(SortOptions.ASC, 'Ascending',null),
        new ToggleFilter(SortOptions.DESC, 'Descending',null),
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


    }

    BuildToggleFilters() {
        this.shipmentTypeMultipleSelection.MultipleSelectionList.map(x => {
            x.Count = this.setCounterForshipmentTypeMultipleSelect(x);
            return x;
        });
        this.shipmentDirectionMultipleSelection.MultipleSelectionList.map(x => {
            x.Count = this.setCounterForDirectionMultipleSelect(x);
            return x;
        });
        this.shipmentMoreFiltersMultipleSelection.MultipleSelectionList.map(x => {
            x.Count = this.setCounterForMoreFiltersMultipleSelect(x);
            return x;
        });

        this.changeDetector.detectChanges();
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
        } else if (toggleFilter.Code == ShipmentDirectionCodes.Domestic) {
            return this.ShipmentsCounter.Domestic;
        }
    }
    setCounterForMoreFiltersMultipleSelect(toggleFilter: ToggleFilter) {
        if (toggleFilter.Code == MoreFilterCodes.ExceptionOnly) {
            return this.ShipmentsCounter.HasException;
        } else if (toggleFilter.Code == MoreFilterCodes.OrdersOnly) {
            return this.ShipmentsCounter.OrdersOnly;
        } else if (toggleFilter.Code == MoreFilterCodes.EstimatedArrivalOnly) {
            return this.ShipmentsCounter.EstimatedArrivalOnly;
        }else if (toggleFilter.Code == MoreFilterCodes.OperationalOpenedOnly) {
            return this.ShipmentsCounter.OperationalOpenedOnly;
        }
    }



    ClearFilters() {
        this.ClearAdvancedFilters();
        this.shipmentTypeMultipleSelection.ClearFilters();
        this.shipmentDirectionMultipleSelection.ClearFilters();
        this.shipmentMoreFiltersMultipleSelection.ClearFilters();
        this.InvitedCustomers.map(e => e.IsSelected = false);
        this.MilestonesStatus.map(e => e.IsSelected = false);
        this.ShipmentSearchInput.CustomersIds = [];
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

    ShipmentsLoadingError: string;

    FocusOnSearchInput() {
        this.searchInput.nativeElement.focus();
    }



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
    private setViews() {
        this.isMobileView = window.innerWidth <= 479;
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
    Domestic: number = 0;
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
    Direct = 'D'
}

export enum ShipmentEntityTypes {
    Order = 'O',
    Customs = 'C'
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
    Domestic = 'D',
    CustomsImport = 'C'

}
export enum shipmentTypeCodes {
    Air = 'A',
    Ocean = 'O',
    InLand = 'I'

}


