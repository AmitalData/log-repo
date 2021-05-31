import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {ShipmentList} from '../../../Shipment/EntityLists/ShipmentList';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ShipmentListService} from '../../../Shipment/Services/StandardLists/ShipmentListService';
import {ShipmentDomainService} from '../../../Shipment/Services/ShipmentDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {QuoteListService} from '../../../Quote/Services/StandardLists/QuoteListService';
import {QuoteList} from '../../../Quote/EntityLists/QuoteList'; 

@Component({
    
    templateUrl: './ChooseEntityComponent.html',
})

export class ChooseEntityComponent {
    public EntityPM: any;
    public ItemsSource: ShipmentList[] = [];
    public AllShipmentsCount: number = 0;
    public AllQuotesCount: number = 0;
    private myShipmentListService: ShipmentListService;
    private myQuoteListService: QuoteListService;
    private DomainService: ShipmentDomainService;
    public ListTitle = "";
    public EntityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.DomainService = new ShipmentDomainService();
        this.myShipmentListService = new ShipmentListService();
        this.myQuoteListService = new QuoteListService();
    }

    public IsShipment = false; 
    public IsQuote = false; 
    public EntityObjectTableName: string = "";
    public IsFromTicket = false;
    public IsStandAloneSearch = false;
    SetWindowArgs(args: any) {
        this.EntityObjectTableName = args.EntityObjectTableName;
        this.IsFromTicket = args.IsFromTicket;
        this.IsStandAloneSearch = args.IsStandAloneSearch;

        if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master") {
            this.IsShipment = true;
            this.LoadShipmentsData();
            this.ListTitle = "All Shipments";
        }
        else if (this.EntityObjectTableName == "House") {
            this.IsShipment = true;
            this.EntityId = args.EntityId;
            this.LoadShipmentsData();
            this.ListTitle = "All Houses";
        }
        else {
            this.IsQuote = true;
            this.LoadQuotesData();
            this.ListTitle = "All Quotes";
        }
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master" || this.EntityObjectTableName == "House") {
                this.LoadShipmentsData();
            }
            else {
                this.LoadQuotesData();
            }
        }
    }

    LoadShipmentsData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = false;
        filters.GetCount = true;
        var searchValue = null;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
        }

        if (!this.IsFromTicket) {
            if (this.EntityObjectTableName == "Master") {
                filters.addAdditionalFilter("ShipmentLevelCode", "D,C", null, null, "InList", true, true, false, "string");
            }
            else if (this.EntityObjectTableName == "House") {
                if (!AppTool.IsNullOrEmpty(this.EntityId)) {
                    filters.addAdditionalFilter("MasterShipmentDataId", this.EntityId, null, null, "Equals", false, false, false, "string");
                }
                filters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, true, false, "string");
                filters.addAdditionalFilter("MasterConnectedHouses", true, null, null, "Equals", true, false, false, "Boolean");
            }
            else {
                filters.addAdditionalFilter("ShipmentLevelCode", "D,H", null, null, "InList", true, true, false, "string");
            }

        }
        filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");

        if (!AppTool.IsNullOrEmpty(this.CurrentDirectionId)) {
            if (this.CurrentDirectionId == "All") this.CurrentDirectionId = "";
            else {
                filters.addAdditionalFilter("DirectionId", this.CurrentDirectionId, null, null, "Equals", false, true, false, "string");
            }
        }

        if (!AppTool.IsNullOrEmpty(this.CurrentTransportModeId)) {
            if (this.CurrentTransportModeId == "All") this.CurrentTransportModeId = "";
            else {
                filters.addAdditionalFilter("TransportModeId", this.CurrentTransportModeId, null, null, "Equals", false, true, false, "string");
            }
        }

        if (this.IsStandAloneSearch) {
            filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");
            filters.addAdditionalFilter("DirectionId", "D", null, null, "Equals", false, true, false, "string");
            filters.addAdditionalFilter("TransportModeId", "I", null, null, "Equals", false, true, false, "string");
            filters.addAdditionalFilter("IsStandalonePickupDelivery", false, null, null, "Equals", true, false, false, "Boolean");
        }
        //this.DomainService.GetShipmentFullTextSearch(filters).subscribe((myResponse: ServiceResponse) => {
        //    if (!myResponse.HasError) {
        //        this.ItemsSource = myResponse.Result;
        //        this.AllShipmentsCount = this.ItemsSource.length;
        //    }
        //});

        this.myShipmentListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result;
                this.AllShipmentsCount = this.ItemsSource.length;
            }
        });
    }
    LoadQuotesData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.AllQuotesCount = 0;
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "OpenDate";
        filters.SortDirection = "Descending";
        var searchValue = null;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
        }

        filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");

        if (!AppTool.IsNullOrEmpty(this.CurrentDirectionId)) {
            if (this.CurrentDirectionId == "All") this.CurrentDirectionId = "";
            else {
                filters.addAdditionalFilter("DirectionId", this.CurrentDirectionId, null, null, "Equals", false, true, false, "string");
            }
        }

        if (!AppTool.IsNullOrEmpty(this.CurrentTransportModeId)) {
            if (this.CurrentTransportModeId == "All") this.CurrentTransportModeId = "";
            else {
                filters.addAdditionalFilter("TransportModeId", this.CurrentTransportModeId, null, null, "Equals", false, true, false, "string");
            }
        }

        this.myQuoteListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result;
                this.AllQuotesCount = this.ItemsSource.length;
            }
        });
    }

    // Filters
    private mySelectedDirectionFilter: string = "";
    get CurrentTransportModeId() { return this.mySelectedDirectionFilter; }
    set CurrentTransportModeId(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;
            if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master" || this.EntityObjectTableName == "House" ) {
                this.LoadShipmentsData();
            }
            else {
                this.LoadQuotesData();
            }
        }
    }

    private mySelectedTransportFilter: string = "";
    get CurrentDirectionId() { return this.mySelectedTransportFilter; }
    set CurrentDirectionId(newValue: string) {
        if (this.mySelectedTransportFilter != newValue) {
            this.mySelectedTransportFilter = newValue;
            if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master" || this.EntityObjectTableName == "House") {
                this.LoadShipmentsData();
            }
            else {
                this.LoadQuotesData();
            }
        }
    }

    // Commands
    public SelectedShipment: ShipmentList = null;
    public SelectedQuote: QuoteList = null;

    Selecting(item: ShipmentList) {
        this.SelectedShipment = item;
        this.Close();
    }
    SelectingQuote(item: QuoteList) {
        this.SelectedQuote = item;
        this.Close();
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
