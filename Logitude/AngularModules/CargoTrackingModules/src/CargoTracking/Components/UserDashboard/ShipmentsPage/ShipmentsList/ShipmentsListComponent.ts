
import { Component, ViewChild, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
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
import { MultipleSelectionComponent } from 'src/Infrastructure/Components/MultipleSelection/MultipleSelectionComponent';


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
    Shipments: CargoTrackingShipmentList[] = [];

    isLoading: boolean = false;
    isFiltersSideBarOpened: boolean = false;
    isFilter1Expanded: boolean = false;
    isFilter2Expanded: boolean = false;
    isAbdullahCompanyChecked: boolean = true;
    showSortDetailsMenu: boolean = false;
    showShipmentDetailsMenu: boolean = false;
    InvitedCustomersIds: string[];
    InvitedCustomers: any[] = [];
    ShipmentsDataSource;
    @ViewChild(CdkVirtualScrollViewport) virtualScroll: CdkVirtualScrollViewport;
    @ViewChild('input') searchInput: ElementRef;
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

    ShipmenTypeForRouting: string;

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
        private searchService: CargoTrackingSearchService)
    {
        this.InitComponent();
        this.SetDefaultBackgroundColor();
    }

    ngOnInit(): void {
        this.GetPreservedToggleFiltersFromSessionInfo();
        this.GetCompanyLoginsFromCache();
    }
    ngAfterViewInit(): void
    {
        this.setSortFilterValues();
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
            case 'E': {
                title = "CLIENT"
                break;
            }

            case 'I':
            case 'C': {
                title = "SUPPLIER"
                break;
            }
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
            case 'E': {
                value = shipment.ShipperName;
                break;
            }

            case 'I': {
                value = shipment.ConsigneeName;
                break;
            }
        }
        return value;
    }

    private SetSupplierOrCleintValueByEntityType(shipment: CargoTrackingShipmentList, value: any) {
        const EntityType_Customs = "C";
        if (shipment.EntityType == EntityType_Customs) {
            value = shipment.ConsigneeName;
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
            this.SearchText = RootContext.LastSearchText ? SessionInfo.ShipmentsFilters.SearchText : '';
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
            if(SessionInfo.ShipmentsFilters.HasException){
                this.hasException = SessionInfo.ShipmentsFilters.HasException;
            }
            
            this.GetInvitedCustomers();
            this.SelectedFilters.map(x => {
                if(x.FilterName == 'shipmentType')
                {
                    this.shipmentTypeMultipleSelection.select.options.find(d => d.value.Code == x.Code).select();
                } else if (x.FilterName == 'shipmentDirection')
                {
                    this.shipmentDirectionMultipleSelection.select.options.find(d => d.value.Code == x.Code).select();
                }
            });
            this.setSortFilterValues();
            this.LoadScreenData();
        }
    }

    private InitComponent()
    {
        this.InitForm();
        ''.substring(''.indexOf('('))
    }

    private setSortFilterValues()
    {
        if(SessionInfo.ShipmentsFilters) {
            this.sortField = SessionInfo.ShipmentsFilters.SortFieldName;
            this.isSortDescending = SessionInfo.ShipmentsFilters.SortDescending;
        }
        var AtdSelectOption = this.sortByMultipleSelection.select.options.find(d => d.value.Code == 'ATD');
        var AtaSelectOption = this.sortByMultipleSelection.select.options.find(d => d.value.Code == 'ATA');
        var AscSelectOption = this.sortByMultipleSelection.select.options.find(d => d.value.Code == 'ASC');
        var DescSelectOption = this.sortByMultipleSelection.select.options.find(d => d.value.Code == 'Desc');
        
        if(this.sortField == 'ATA') {
            AtdSelectOption.deselect();
            AtaSelectOption.select();
        } else if(this.sortField == 'ATD'){
            AtdSelectOption.select();
            AtaSelectOption.deselect();
        }
        if(this.isSortDescending) {
            AscSelectOption.deselect();
            DescSelectOption.select();
        } else {
            DescSelectOption.deselect();
            AscSelectOption.select();
        }
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
    private GetInvitedCustomers()
    {

        // this.AddDemoCustomersForTest();

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
    isSortDescending?: boolean = true;
    hasException: boolean = false;
    private LoadShipments(shipmentFilters: CargoTrackingShipmentFilters)
    {
        if (this.ShipmentsDataSource)
            this.ReloadShipments(shipmentFilters);

        else{
            this.InitiateShipmentDataSource(shipmentFilters);
            this.ReloadShipments(shipmentFilters);
        }
    }

    parseCurrentMilestoneExceptionDate(){
        var date = '21/01/2021 00:00:00'
        var dateParts = date.split("/");
        var dateObject = new Date(+dateParts[2], +dateParts[1] - 1, +dateParts[0]);
        console.log(dateObject)
        return dateObject;
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
        this.SelectToggleFilters(toggleFilterCodes);
    }
    
    OnHasExceptionChanged(event){
        this.hasException = event;
        this.LoadScreenData();
    }

    sortBySelectionChangedHandler(event) {
        const index = this.sortByOptions.findIndex(d => d.Code == event);
        var selectedCode = '';
        this.sortField = '';
        switch(index) {
            case 0: {
                selectedCode = this.sortByOptions[1].Code;
                this.sortField = event;
                this.sortByMultipleSelection.select.options.find(d => d.value.Code == selectedCode).deselect();
                break;
            }
            case 1:{
                selectedCode = this.sortByOptions[0].Code;
                this.sortField = event;
                this.sortByMultipleSelection.select.options.find(d => d.value.Code == selectedCode).deselect();
                break;
            }
            case 2:{
                selectedCode = this.sortByOptions[3].Code;
                this.isSortDescending = false;
                this.sortByMultipleSelection.select.options.find(d => d.value.Code == selectedCode).deselect();
                break;
            }
            case 3:{
                selectedCode = this.sortByOptions[2].Code;
                this.isSortDescending = true;
                this.sortByMultipleSelection.select.options.find(d => d.value.Code == selectedCode).deselect();
                break;
            }
        }
        this.LoadScreenData()
        // var filter = this.sortByOptions.find(d => d.Code == event);
        // this.SelectFilterWithoutLoadScreenData(filter);
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

    SetConsignmentNumber(shipment: CargoTrackingShipmentList) {
        if (shipment.ShipmentLevelCode == 'D') {
            this.ConsignmentNumber = shipment.Master;
        }

        else if (shipment.ShipmentLevelCode == 'H') {
            this.ConsignmentNumber = shipment.House;
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
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: 'References',
                description: references.slice(1, references.length + 1).join("\n"),
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
        // new ToggleFilter('AL', 'ALL', ''),
        new ToggleFilter('ATA', 'ATA', '', null),
        new ToggleFilter('ATD', 'ATD', '', null),
        new ToggleFilter('ASC', 'ASC', '', null),
        new ToggleFilter('Desc', 'Desc', '', null),
    ];

    BuildToggleFilters(){
        // this.ToggleFilters = [
        //     new ToggleFilter('AL', 'ALL', '', this.ShipmentsCount),
        //     new ToggleFilter('IM', 'Import', 'shipmentDirection',this.ShipmentsCounter.Import),
        //     new ToggleFilter('EX', 'Export', 'shipmentDirection',this.ShipmentsCounter.Export),
        //     new ToggleFilter('A', 'Air', 'shipmentType',this.ShipmentsCounter.Air),
        //     new ToggleFilter('I', 'Land', 'shipmentType',this.ShipmentsCounter.Land),
        //     new ToggleFilter('O', 'Sea', 'shipmentType',this.ShipmentsCounter.Sea),
        // ];
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
        } else if (filter.FilterName == 'shipmentDirection')
        {
            this.shipmentDirectionMultipleSelection.DeselectFilter(filter.Code);
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
        this.isFiltersSideBarOpened = false;

        this.SelectedInvitedCustomers = this.FiltersSelectedInvitedCustoms.map(d=>d);
        RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();
    }
    ClearAdvancedFilters(){
        this.isFiltersSideBarOpened = false;
        this.SelectedInvitedCustomers = [];
        RootContext.ShipmentsScrollPosition = 0;
        this.LoadScreenData();
    }
    SortMenuClicked(buttonCode: string)
    {
        this.isSortDescending = buttonCode == "desc";
        this.LoadScreenData();
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
    FiltersInvitedCustomers: any[] = [];
    OpenAdvancedFiltersSidebar(){
        this.isFiltersSideBarOpened = true;

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
