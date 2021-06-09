import { Component, ViewChild, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, ElementRef } from '@angular/core';
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
export class ShipmentsListComponent implements AfterViewInit
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
    public MoreReferenceText: string;
    public ConsignmentNumber: string;
    public toPortCode: string;
    public fromPortCode: string;
    ShipmentPM: any;
    NumberOfPackages: number = 0;
    TitleOfEstimationORActualDate: string = "";
    ValueOfEstimationORActualDate: Date;
    ShipmentTypeAndDirectionTooltip: string;
    TitleForSupplierOrClient: string;
    ValueForSupplierOrClient: string;

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
        private searchService: CargoTrackingSearchService)
    {


        this.InitComponent();
        this.SetDefaultBackgroundColor();
    }
    ngAfterViewInit(): void
    {
        this.GetPreservedToggleFiltersFromSessionInfo();
        this.GetCompanyLoginsFromCache();

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

            case 'I': {
                title = "SUPPLIER"
                break;
            }
        }

        this.TitleForSupplierOrClient = title;
    }

    SetValueForSupplierOrClient(shipment: CargoTrackingShipmentList) {
        var value;
        const EntityType_Customs = "C";
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

        if (shipment.EntityType == EntityType_Customs)
        {
            value = shipment.ConsigneeName;
        }

        this.ValueForSupplierOrClient = value;
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
            this.SearchText = SessionInfo.ShipmentsFilters.SearchText;
            this.SelectToggleFilters(SessionInfo.ShipmentsFilters.TransportModeCodes);
            this.SelectToggleFilters(SessionInfo.ShipmentsFilters.DirectionCodes);
        }
    }

    private InitComponent()
    {
        this.InitForm();
        ''.substring(''.indexOf('('))
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
        console.log("[LoggedUserCompanyLogins]", SessionInfo.LoggedUserCompanyLogins);
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



        console.log("[Invited Customers]", this.InvitedCustomersIds);

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
        this.LoadScreenData();
    }

    Search()
    {
        if (this.tenant!=null && this.SearchText) {
            this.Shipments = [];
            this.LoadScreenData();
        }

    }

    ItemClicked(item)
    {
        var SecurityKey = item.SecurityKey;
        SessionInfo.ShipmentsFilters = this.BuildShipmentFilters();

        this.router.navigate(['cargo-tracking', 'shipment', SecurityKey]);

    }

    ShipmentsCounter: CargoTrackingShipmentsCounter = new CargoTrackingShipmentsCounter();
    LoadScreenData()
    {
        if (this.tenant)
        {
            var shipmentFilters = this.BuildShipmentFilters();
            this.LoadShipments(shipmentFilters);
            this.LoadShipmentsCounter(shipmentFilters);
        }
    }
    references: string[];
    isSortDescending: boolean = true;
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
            console.log("[GetUserShipmentsCounter]", counter);
            this.ShipmentsCounter = counter;
            this.BuildToggleFilters();
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

    SortClicked(){
        this.isSortDescending = !this.isSortDescending;
        this.LoadScreenData();
    }
    private BuildShipmentFilters()
    {
        var shipmentFilters = new CargoTrackingShipmentFilters();
        shipmentFilters.Tenant = this.tenant;
        shipmentFilters.SearchText = this._SearchText ? this._SearchText.trim().toLowerCase() : '';


        shipmentFilters.SortDescending = this.isSortDescending;

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
    private SelectToggleFilters(toggleFilterCodes: string)
    {
        if(toggleFilterCodes){
            var splitted = toggleFilterCodes.split(',');
            splitted.forEach(filterCode=>{
                this.SelectFilterByCode(filterCode);
            });
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
        new ToggleFilter('IM', 'Import'),
        new ToggleFilter('EX', 'Export'),
        new ToggleFilter('A', 'Air'),
        new ToggleFilter('I', 'Land'),
        new ToggleFilter('O', 'Sea'),
    ];

    BuildToggleFilters(){
        this.ToggleFilters = [
            new ToggleFilter('IM', 'Import',this.ShipmentsCounter.Import),
            new ToggleFilter('EX', 'Export',this.ShipmentsCounter.Export),
            new ToggleFilter('A', 'Air',this.ShipmentsCounter.Air),
            new ToggleFilter('I', 'Land',this.ShipmentsCounter.Land),
            new ToggleFilter('O', 'Sea',this.ShipmentsCounter.Sea),
        ];

        this.changeDetector.detectChanges();
    }

    SelectedFilters: ToggleFilter[] = [];
    SelectFilter(filter: ToggleFilter)
    {
        var item = this.SelectedFilters.find(d => d.Name == filter.Name);
        if (!item)
            this.SelectedFilters.push(filter);

        this.LoadScreenData();
    }
    SelectFilterByCode(filterCode: string)
    {
        var filter = this.ToggleFilters.find(d => d.Code == filterCode);
        this.SelectFilter(filter);
    }
    DeselectFilter(filter: ToggleFilter)
    {
        var index = this.SelectedFilters.findIndex(d => d.Name == filter.Name);
        this.SelectedFilters.splice(index, 1);

        this.LoadScreenData();

    }
    ClearFilters()
    {
        this.SelectedFilters = [];
        this.ClearAdvancedFilters();
        this.LoadScreenData();
    }

    ApplyFilterButtonClicked()
    {
        this.isFiltersSideBarOpened = false;

        this.SelectedInvitedCustomers = this.FiltersSelectedInvitedCustoms.map(d=>d);

        this.LoadScreenData();
    }
    ClearAdvancedFilters(){
        this.isFiltersSideBarOpened = false;
        this.SelectedInvitedCustomers = [];
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
        console.log("shipment more details", buttonCode, this.lastClickedShipment);

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
    constructor(code: string, name: string, count: number = 0)
    {
        this.Code = code;
        this.Name = name;
        this.Count = count;
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



}

export class CargoTrackingShipmentsCounter{

    Import: number = 0;
    Export: number = 0;
    Air: number = 0;
    Land: number = 0;
    Sea: number = 0;
}
