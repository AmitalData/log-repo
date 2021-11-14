
import { Component, ViewChild, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef, OnInit, Injectable } from '@angular/core';
import { Router, ActivatedRoute, NavigationStart, NavigationEnd } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingSearchService } from '../../../../Services/Others/CargoTrackingSearchService';
import { CargoTrackingShipmentList } from '../../../../EntityLists/CargoTrackingShipmentList';
import { SessionInfo } from '../../../../../Infrastructure/Utilities/SessionInfo';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { ShipmentDataSource } from '../../../../DataContracts/CargoTrackingShipmentDataSource';
import { CargoTrackingShipmentFilters } from '../../../../DataContracts/CargoTrackingShipmentFilters';
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
export class ShipmentsListComponent implements AfterViewInit, OnInit
{


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
    InvitedCustomersIds: string[];
    InvitedCustomers: any[] = [];
    ShipmentsDataSource;
    @ViewChild(CdkVirtualScrollViewport) virtualScroll: CdkVirtualScrollViewport;
    @ViewChild('input') searchInput: ElementRef;
    @ViewChild('mobileSearch') mobileSearchInput: ElementRef;
    @ViewChild('shipmentTypeMultipleSelection') shipmentTypeMultipleSelection: MultipleSelectionComponent;
    @ViewChild('shipmentDirectionMultipleSelection') shipmentDirectionMultipleSelection: MultipleSelectionComponent;
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
    public SortOptions= SortOptions;
    PanelSearchText;
    MasterOrHouseLabel: string = "";
    EntityType_Customs = "C";
    get tenant(){
        return CargoTrackingBrandingData.Tenant;
    }

    constructor(private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private changeDetector: ChangeDetectorRef,
        private cargoTrackingPortService: CargoTrackingPortService,
        private cargoTrackingShipmentService: CargoTrackingShipmentService,
        public dialog: MatDialog,
        private searchService: CargoTrackingSearchService,
        private milestonesService: CargoTrackingMilestoneService,
        public sharedService: SharedService)
    {
        this.InitComponent();
        this.SetDefaultBackgroundColor();
    }

    ngOnInit(): void {
        this.setDefaultSort();
        this.GetPreservedToggleFiltersFromSessionInfo();
        this.GetCompanyLoginsFromCache();
        this.GetMilstones();
        this.getPreviousScroll();
        this.selectDefaultSortFields();
    }

    private setDefaultSort(){
        this.sortField = SortOptions.CMD;
        this.isSortDescending = SortOptions.DESC;
    }

    private selectDefaultSortFields(){
        this.sortByMultipleSelection.select.options.filter(d => 
            [SortOptions.CMD, SortOptions.DESC].includes(d.value.Code))
            .map(x => x.select());
    }

    private getPreviousScroll(){
        this.router.events.pipe(
            filter((e: any): e is NavigationEnd => e instanceof NavigationEnd)
         ).subscribe((e: NavigationEnd) => {
             if(e.url === '/cargo-tracking/shipments'){
                const shipmentCardsContainer = document.getElementById("scrollArea");
                if(shipmentCardsContainer && RootContext.ShipmentsScrollPosition > 0) {
                    shipmentCardsContainer.scrollTop = RootContext.ShipmentsScrollPosition || 0;
                }
             }

         });
    }
    ngAfterViewInit(): void
    {
        this.LoadScreenData();
        this.SetShipmentsScrollPosition();
    }

    private SetShipmentsScrollPosition() {
        const shipmentCardsContainer = document.getElementById("scrollArea");
        shipmentCardsContainer.scrollTop = RootContext.ShipmentsScrollPosition || 0;
    }


    private SetDefaultBackgroundColor()
    {
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

        this.ShipmentTypeAndDirectionTooltip = type +' '+ direction;

    }

    SetTitileAndValueForSupplierOrClient(shipment: CargoTrackingShipmentList)
    {
        this.SetTitleForSupplierOrClient(shipment);
        this.SetValueForSupplierOrClient(shipment);
    }


    SetTitleForSupplierOrClient(shipment: CargoTrackingShipmentList) {
        var title;
        switch (shipment.DirectionId) {
            case ShipmentDirections.Import:{
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


    private GetPreservedToggleFiltersFromSessionInfo()
    {
        if (SessionInfo.ShipmentsFilters) {
            this.appliedSelectedFilterMilestonesStatus = SessionInfo.ShipmentsFilters.SelectedMilestonesStatus;
            this.SelectedInvitedCustomers = SessionInfo.ShipmentsFilters.SelectedInvitedCustomers;
            this.SearchText = RootContext.LastSearchText ? SessionInfo.ShipmentsFilters.SearchText : '';
            this.updateShipmentTypeAndDirectionSelectedFilters();
            this.hasException = SessionInfo.ShipmentsFilters.HasException? SessionInfo.ShipmentsFilters.HasException : this.hasException;
            this.GetInvitedCustomers();
            this.updateShipmentTypeAndDirectionSelection();

            this.LoadScreenData();
        }
    }

    private InitComponent()
    {
        this.InitForm();
        ''.substring(''.indexOf('('))
    }

    private updateShipmentTypeAndDirectionSelectedFilters(){
        if(SessionInfo.ShipmentsFilters.TransportModeCodes){
            var splitted = SessionInfo.ShipmentsFilters.TransportModeCodes.split(',');
            splitted.forEach(filterCode=>{
                this.SelectFilterByCode(filterCode);
            });
        }
        if(SessionInfo.ShipmentsFilters.DirectionCodes){
            var splitted = SessionInfo.ShipmentsFilters.DirectionCodes.split(',');
            splitted.forEach(filterCode=>{
                this.SelectFilterByCode(filterCode);
            });
        }
    }

    private updateShipmentTypeAndDirectionSelection(){
        this.SelectedFilters.map(x => {
            if(x.FilterName == 'shipmentType')
            {
                this.shipmentTypeMultipleSelection.select.options.find(d => d.value.Code == x.Code).select();
            } else if (x.FilterName == 'shipmentDirection')
            {
                this.shipmentDirectionMultipleSelection.select.options.find(d => d.value.Code == x.Code).select();
            }
        });
    }

    SetMoreReferenceText(reference: string) {
        var allreferences = reference?.split(',');
        if (allreferences?.length > 4) {

            var morereferences = allreferences.slice(4, allreferences.length + 1)
            this.MoreReferenceText = morereferences.join(',');

        }
    }
    private GetCompanyLoginsFromCache()
    {
        SessionInfo.LoggedUserCompanyLogins = JSON.parse(sessionStorage.getItem("LoggedUserCompanyLogins"));
        this.GetInvitedCustomers();
    }

    FiltersSelectedInvitedCustoms: any[] = [];
    SelectedInvitedCustomers: any[] = [];
    selectedFilterMilestonesStatus: any[] = [];
    appliedSelectedFilterMilestonesStatus: any[] = [];
    selectedShipmentTypesFilter: any[] = [];
    selectedShipmentDirectionsFilter: any[] = [];
    private GetInvitedCustomers()
    {
        this.InvitedCustomersIds = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => d.CardId);

        this.InvitedCustomers = SessionInfo.LoggedUserCompanyLogins
            .filter(d => d.CardType == 'CS' && d.CardId != null && d.Tenant == this.tenant)
            .map(d => (
                {
                    IsSelected: false,
                    Name: d.CompanyName.substring(0,d.CompanyName.lastIndexOf('(')),
                    ...d }
                ));
        this.LoadScreenData();
    }



    private GetMilstones(){
        this.milestonesService.getAll(this.tenant)
            .subscribe((milestones:any) => {
                milestones.sort(function (a, b) {
                    return Number(a.Code) - Number(b.Code);
                  });
                this.MilestonesStatus  = milestones.map(s => ({ IsSelected: false, ...s}));
            });
    }


    private AddDemoCustomersForTest()
    {
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

    private InitForm()
    {
        this.searchForm = this.formBuilder.group({
            SearchText: ''
        });
        this.mobileSearchForm = this.formBuilder.group({
            PanelSearchText: ''
        });
    }




    private _SearchText: string = '';
    public get SearchText(): string
    {
        return this._SearchText;
    }
    public set SearchText(v: string)
    {
        this._SearchText = v;

        // if (this.SearchText)
        //     this.LoadShipments();
    }

    Clear()
    {
        this.SearchText = '';
        RootContext.LastSearchText = '';
        this.LoadScreenData();
    }

    Search()
    {
        if (this.tenant!=null && this.SearchText) {
            this.Shipments = [];
            RootContext.LastSearchText = this.SearchText;
            this.LoadScreenData();
        }

    }

    ItemClicked(item)
    {
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
    LoadScreenData()
    {
        if (this.tenant)
        {
            var shipmentFilters = this.BuildShipmentFilters();
            this.LoadShipments(shipmentFilters);
            this.LoadShipmentsCounter(shipmentFilters);
            this.SetShipmentsScrollPosition();
        }
    }
    references: string[];
    sortField: string = '';
    isSortDescending: string = '';
    hasException: boolean = false;
    hasExceptionAdvancedFilter: boolean = false;
    private LoadShipments(shipmentFilters: CargoTrackingShipmentFilters)
    {
        if (this.ShipmentsDataSource)
            this.ReloadShipments(shipmentFilters);

        else{
            this.InitiateShipmentDataSource(shipmentFilters);
            this.ReloadShipments(shipmentFilters);
        }
    }

    private LoadShipmentsCounter(shipmentFilters: CargoTrackingShipmentFilters)
    {
        this.searchService.GetUserShipmentsCounter(shipmentFilters).subscribe((counter: any) =>
        {
            this.ShipmentsCounter = counter;
            this.BuildToggleFilters();
            this.SetShipmentsScrollPosition();
        });
    }


    private InitiateShipmentDataSource(shipmentFilters: CargoTrackingShipmentFilters)
    {
        this.ShipmentsDataSource = new ShipmentDataSource(this.changeDetector, this.searchService, shipmentFilters, this);
    }

    private ReloadShipments(shipmentFilters: CargoTrackingShipmentFilters)
    {
        this.ShipmentsDataSource.ReloadData(shipmentFilters);
        this.ResetShipmentsScrollbarPosition();
    }

    private ResetShipmentsScrollbarPosition()
    {
        this.virtualScroll.scrollToIndex(0);
    }

    // SortClicked(){
    //     this.isSortDescending = !this.isSortDescending;
    //     this.LoadScreenData();
    // }
    private BuildShipmentFilters()
    {
        var shipmentFilters = new CargoTrackingShipmentFilters();
        shipmentFilters.Tenant = this.tenant;
        shipmentFilters.SearchText = this._SearchText? this._SearchText.trim().toLowerCase() : '';

        shipmentFilters.SortFieldName = this.sortField;
        shipmentFilters.SortDescending = this.isSortDescending;
        shipmentFilters.HasException = this.hasException;

        this.SetCustomersFilter(shipmentFilters);
        this.SetMilestonesFilter(shipmentFilters);
        this.SetTransportModeFilters(shipmentFilters);
        this.SetDirectionFilters(shipmentFilters);
        return shipmentFilters;
    }

    private SetCustomersFilter(shipmentFilters: CargoTrackingShipmentFilters)
    {
        if(this.SelectedInvitedCustomers.length > 0){
            var str = this.SelectedInvitedCustomers.map(d => d.CardId)?.join(',');

        }else{
            var str = this.InvitedCustomers.map(d => d.CardId)?.join(',');
        }
        shipmentFilters.CustomersIds = this.InvitedCustomersIds;
        shipmentFilters.CustomersIdsString = str;
        shipmentFilters.SelectedInvitedCustomers = this.SelectedInvitedCustomers;
    }

    private SetMilestonesFilter(shipmentFilters: CargoTrackingShipmentFilters)
    {
        if(this.appliedSelectedFilterMilestonesStatus.length > 0){
            var str = this.appliedSelectedFilterMilestonesStatus.map(state => state.Code)?.join(',');
            shipmentFilters.SelectedMilestonesStatus = this.appliedSelectedFilterMilestonesStatus;
            shipmentFilters.MilestonesStatus = str;
        }
    }

    private SetDirectionFilters(shipmentFilters: CargoTrackingShipmentFilters)
    {
        var direction = "";
        if (this.SelectedFilters.length > 0){
            const directionsCodes = ['IM', 'EX'];
            direction = this.SelectedFilters.filter(d => directionsCodes.includes(d.Code)).map(d => d.Code).join(',');
        }
        shipmentFilters.DirectionCodes = direction;
    }

    private SetTransportModeFilters(shipmentFilters: CargoTrackingShipmentFilters)
    {
        var transportMode = "";
        if (this.SelectedFilters.length > 0){
            const transportModeCodes = ['A', 'I', 'O'];
            transportMode = this.SelectedFilters.filter(d => transportModeCodes.includes(d.Code)).map(d => d.Code).join(',');
        }
        shipmentFilters.TransportModeCodes = transportMode;
    }

    SelectionChangedHandler(toggleFilterCodes: string) {
        RootContext.ShipmentsScrollPosition = 0;
        this.SelectToggleFilters(toggleFilterCodes);
    }

    OnHasExceptionChanged(event){
        RootContext.ShipmentsScrollPosition = 0;
        this.hasException = event;
        this.hasExceptionAdvancedFilter = event;
        this.LoadScreenData();
    }

    OnAdvancedHasExceptionChanged(event) {
        this.hasExceptionAdvancedFilter = event;
    }

    sortBySelectionChangedHandler(event) {
        const index = this.sortByOptions.findIndex(d => d.Code == event);
        switch(index) {
            case 0: {
                this.sortField = this.sortField === event ? '' : event;
                this.sortByMultipleSelection.select.options.filter(d => 
                    [SortOptions.ATA, SortOptions.ATD].includes(d.value.Code)).map(x => x.deselect());
                break;
            }
            case 1: {
                this.sortField = this.sortField === event ? '' : event;
                this.sortByMultipleSelection.select.options.filter(d => 
                    [SortOptions.CMD, SortOptions.ATD].includes(d.value.Code)).map(x => x.deselect());
                break;
            }
            case 2:{
                this.sortField = this.sortField === event ? '' : event;
                this.sortByMultipleSelection.select.options.filter(d => 
                    [SortOptions.CMD, SortOptions.ATA].includes(d.value.Code)).map(x => x.deselect());
                break;
            }
            case 3:{
                this.isSortDescending = this.isSortDescending === event ? '' : event;
                this.sortByMultipleSelection.select.options.find(d => d.value.Code == SortOptions.DESC).deselect();
                break;
            }
            case 4:{
                this.isSortDescending = this.isSortDescending === event ? '' : event;
                this.sortByMultipleSelection.select.options.find(d => d.value.Code == SortOptions.ASC).deselect();
                break;
            }
        }
        if((this.sortField && this.isSortDescending) || (!this.sortField && !this.isSortDescending))
        {
            RootContext.ShipmentsScrollPosition = 0;
            this.LoadScreenData();
        }
    }

    private SelectToggleFilters(toggleFilterCodes: string)
    {
        if(toggleFilterCodes){
            var splitted = toggleFilterCodes.split(',');
            splitted.forEach(filterCode=>{
                this.SelectFilterByCode(filterCode);
            });
            this.LoadScreenData();
        }
    }

    SplitReference(reference: string)
    {
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

    GetModeIcon(mode: string)
    {
        var iconPath = "";
        const Air = 'A';
        const Ocean = 'O';
        const InLand = 'I';
        switch (mode) {
            case Air:
                iconPath = "./assets/images/misc/plane.svg";
                break;
            case Ocean:
                iconPath = "./assets/images/misc/ship.svg";
                break;
            case InLand:
                iconPath = "./assets/images/misc/Truck.svg";
                break;
            default:
                break;
        }
        return iconPath;
    }

    ToggleFilters: ToggleFilter[] = [
        // new ToggleFilter('AL', 'ALL', ''),
        new ToggleFilter('IM', 'Import', 'shipmentDirection'),
        new ToggleFilter('EX', 'Export', 'shipmentDirection'),
        new ToggleFilter('A', 'Air', 'shipmentType'),
        new ToggleFilter('I', 'Land', 'shipmentType'),
        new ToggleFilter('O', 'Sea', 'shipmentType'),
    ];

    sortByOptions: ToggleFilter[] = [
        new ToggleFilter(SortOptions.CMD, 'Current Status Date', '', null),
        new ToggleFilter(SortOptions.ATA, 'ATA/ETA', '', null),
        new ToggleFilter(SortOptions.ATD, 'ATD/ETD', '', null),
        new ToggleFilter(SortOptions.ASC, 'Ascending', '', null),
        new ToggleFilter(SortOptions.DESC, 'Descending', '', null),
    ];

    BuildToggleFilters(){
        this.shipmentTypeMultipleSelection.MultipleSelectionList.map(x => {
            x.Count = this.setCounterForMultipleSelect(x);
            return x;
        });
        this.shipmentDirectionMultipleSelection.MultipleSelectionList.map(x => {
            x.Count = this.setCounterForMultipleSelect(x);
            return x;
        });
        this.changeDetector.detectChanges();
    }

    setCounterForMultipleSelect(toggleFilter: ToggleFilter) : number{
        if(toggleFilter.Code == 'AL')
        {
            return this.ShipmentsCount;
        } else if(toggleFilter.Code == 'IM')
        {
            return this.ShipmentsCounter.Import;
        } else if(toggleFilter.Code == 'EX')
        {
            return this.ShipmentsCounter.Export;
        } else if(toggleFilter.Code == 'A')
        {
            return this.ShipmentsCounter.Air;
        } else if(toggleFilter.Code == 'I')
        {
            return this.ShipmentsCounter.Land;
        } else if(toggleFilter.Code == 'O')
        {
            return this.ShipmentsCounter.Sea;
        }
    }

    SelectedFilters: ToggleFilter[] = [];
    SelectFilter(filter: ToggleFilter)
    {
        var item = this.SelectedFilters.find(d => d.Name == filter.Name);
        if (!item)
            this.SelectedFilters.push(filter);
            RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();
    }

    SelectFilterWithoutLoadScreenData(filter: ToggleFilter) {
        var item = this.SelectedFilters.find(d => d.Name == filter.Name);
        if (!item){
            this.SelectedFilters.push(filter);
        } else {
            var index = this.SelectedFilters.findIndex(d => d.Name == filter.Name);
            this.SelectedFilters.splice(index, 1);
        }
    }

    SelectFilterByCode(filterCode: string)
    {
        var filter = this.ToggleFilters.find(d => d.Code == filterCode);
        this.SelectFilterWithoutLoadScreenData(filter);
    }
    DeselectFilter(filter: ToggleFilter)
    {
        var index = this.SelectedFilters.findIndex(d => d.Name == filter.Name);
        this.SelectedFilters.splice(index, 1);
        if(filter.FilterName == 'shipmentType')
        {
            this.shipmentTypeMultipleSelection.DeselectFilter(filter.Code);
            var index = this.selectedShipmentTypesFilter.findIndex(d=>d.Code==filter.Code);
            if(index >= 0){
                    this.selectedShipmentTypesFilter.splice(index,1);
            };
        } else if (filter.FilterName == 'shipmentDirection')
        {
            this.shipmentDirectionMultipleSelection.DeselectFilter(filter.Code);
            var index = this.selectedShipmentDirectionsFilter.findIndex(d=>d.Code==filter.Code);
            if(index >= 0){
                    this.selectedShipmentDirectionsFilter.splice(index,1);
            };
        }
        RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();

    }
    ClearFilters()
    {
        this.SelectedFilters = [];
        this.ClearAdvancedFilters();
        this.shipmentTypeMultipleSelection.ClearFilters();
        this.shipmentDirectionMultipleSelection.ClearFilters();
        this.hasException =false;
        this.LoadScreenData();
    }

    ApplyFilterButtonClicked()
    {
        this.sharedService.updateValue(false);
        this.SelectedInvitedCustomers = this.FiltersSelectedInvitedCustoms.map(d=>d);
        this.appliedSelectedFilterMilestonesStatus= this.selectedFilterMilestonesStatus.map(state => state);
        this.toggleMobileAdvancedFilters();
        RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();
    }

    private toggleMobileAdvancedFilters(){
        this.SelectedFilters = this.SelectedFilters.filter(item => !this.ToggleFilters.includes(item));
        if(this.selectedShipmentTypesFilter.length) {
            this.SelectToggleFilters(this.selectedShipmentTypesFilter.map(x => x.Code).join(','));
        }
        if(this.selectedShipmentDirectionsFilter.length) {
            this.SelectToggleFilters(this.selectedShipmentDirectionsFilter.map(x => x.Code).join(','));
        }
        this.hasException = this.hasExceptionAdvancedFilter;
    }

    ClearAdvancedFilters(){
        this.SelectedInvitedCustomers = [];
        this.selectedFilterMilestonesStatus = [];
        this.MilestonesStatus.map((item, index) => {
            this.MilestonesStatus[index].IsSelected = false;
        });
        this.selectedShipmentTypesFilter = [];
        this.selectedShipmentDirectionsFilter = [];
        this.hasExceptionAdvancedFilter = false;
        this.appliedSelectedFilterMilestonesStatus= [];
        this.FiltersInvitedCustomers.map((item, index) => {
            item.IsSelected = false;
        });
        // RootContext.ShipmentsScrollPosition = 0;
        // this.LoadScreenData();
    }
    SortMenuClicked(buttonCode: string)
    {
        // this.LoadScreenData();
    }

    lastClickedShipment: any;
    ShipmentMoreButtonClicked(shipment: any, event: any)
    {
        event.preventDefault();
        event.stopPropagation();

        this.lastClickedShipment = shipment;
        this.showShipmentDetailsMenu = true;
    }
    ShipmentDetailsMenuClicked(buttonCode: string)
    {

        if (buttonCode == "set")
            this.lastClickedShipment.IsFavorite = true;

    }

    ShipmentsCount: number = 0;

    ShipmentsLoadingError: string;

    FocusOnSearchInput(){
        this.searchInput.nativeElement.focus();
    }


    public get SelectedCustomers() : any[] {
        return this.InvitedCustomers.filter(customer=>customer.IsSelected) || [];
    }

    UnselectCustomer(customer)
    {
        var index = this.SelectedInvitedCustomers.findIndex(d=>d==customer);
            if(index >= 0)
                this.SelectedInvitedCustomers.splice(index,1);
                RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();

    }

    UnselectMiletone(state)
    {
        var index = this.appliedSelectedFilterMilestonesStatus.findIndex(d=>d==state);
            if(index >= 0)
                this.appliedSelectedFilterMilestonesStatus.splice(index,1);
                let itemIndex = this.MilestonesStatus.findIndex(item => item.Code == state.Code);
                state.IsSelected = false;
                this.MilestonesStatus[itemIndex] = state;
                RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();

    }
    FiltersInvitedCustomers: any[] = [];
    OpenAdvancedFiltersSidebar(){
        this.sharedService.updateValue(true);
        this.mapFiltersInvitedCustomers();
    }

    mapFiltersInvitedCustomers() {

        this.FiltersSelectedInvitedCustoms = this.SelectedInvitedCustomers.map(d=>d);

        this.InvitedCustomers.forEach(d=>d.IsSelected = false);
        this.FiltersSelectedInvitedCustoms.forEach(d=>d.IsSelected = true);

        this.FiltersInvitedCustomers = this.InvitedCustomers.map(d=>{
            var selected = this.FiltersSelectedInvitedCustoms.find(g=>g==d);
            return selected || d;
        });
    }
    OnCustomerValueChanged(value,customer){
        if(value){
            var index = this.FiltersSelectedInvitedCustoms.findIndex(d=>d==customer);
            if(index < 0)
                this.FiltersSelectedInvitedCustoms.push(customer);
        }
        else{
            var index = this.FiltersSelectedInvitedCustoms.findIndex(d=>d==customer);
            if(index >= 0)
                this.FiltersSelectedInvitedCustoms.splice(index,1);
        }
    }

    // Milestones filter
    MilestonesStatus: any[] = [];
    OnMilestonesStatusFilterChanged(value,state){
        var index = this.selectedFilterMilestonesStatus.findIndex(d=>d==state);
        if(value === true && index < 0){
            this.MilestonesStatus.filter(x => x.Code === state.Code)[0].IsSelected = true;
                this.selectedFilterMilestonesStatus.push(state);
        }
        else if(value === false && index >= 0){
            this.MilestonesStatus.filter(x => x.Code === state.Code)[0].IsSelected = false;
                this.selectedFilterMilestonesStatus.splice(index,1);
        }
    }

    OnShipmentTypeFilterChanged(value,item){
        var index = this.selectedShipmentTypesFilter.findIndex(d=>d==item);
        if(value === true && index < 0){
                this.selectedShipmentTypesFilter.push(item);
        }
        else if(value === false && index >= 0){
                this.selectedShipmentTypesFilter.splice(index,1);
        };
    }

    OnShipmentDirectionFilterChanged(value,item){
        var index = this.selectedShipmentDirectionsFilter.findIndex(d=>d==item);
        if(value === true && index < 0){
                this.selectedShipmentDirectionsFilter.push(item);
        }
        else if(value === false && index >= 0){
                this.selectedShipmentDirectionsFilter.splice(index,1);
        }
    }

    ShowSearchPanel()
    {
        this.ShowMobileSearch = true;
        this.FocusOnMobileSearchBox();
    }
    HideSearchPanel()
    {
        this.ShowMobileSearch = false;
        this.SearchText = this.PanelSearchText;
        if(!this.SearchText){
            this.Shipments = [];
            RootContext.LastSearchText = this.SearchText;
            this.LoadScreenData();
        }else{
            this.Search();
        }
    }
    ToggleSearchPanel()
    {
        this.ShowMobileSearch = !this.ShowMobileSearch;

        if (this.ShowMobileSearch)
            this.FocusOnMobileSearchBox();
    }

    private FocusOnMobileSearchBox()
    {
        setTimeout(() =>
        {
            this.mobileSearchInput?.nativeElement?.focus();
        }, 50);
    }
}

export class ToggleFilter
{
    constructor(code: string, name: string, filterName: string = '', count: number = 0)
    {
        this.Code = code;
        this.Name = name;
        this.Count = count;
        this.FilterName = filterName;
    }


    private name: string;
    public get Name(): string
    {
        return this.name;
    }
    public set Name(v: string)
    {
        this.name = v;
    }



    private count: number = 0;
    public get Count(): number
    {
        return this.count;
    }
    public set Count(v: number)
    {
        this.count = v;
    }


    private code: string;
    public get Code(): string
    {
        return this.code;
    }
    public set Code(v: string)
    {
        this.code = v;
    }

    private filterName: string;
    public get FilterName(): string
    {
        return this.filterName;
    }
    public set FilterName(v: string)
    {
        this.filterName = v;
    }

}

export class CargoTrackingShipmentsCounter{

    Import: number = 0;
    Export: number = 0;
    Air: number = 0;
    Land: number = 0;
    Sea: number = 0;
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
    Direct ='D'
}

export enum ShipmentEntityTypes {
    Order = 'O',
    Customs = 'C'
}
