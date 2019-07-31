import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentList} from '../../../../Shipment/EntityLists/ShipmentList';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TicketPM} from '../../../../CRM/EntityPMs/TicketPM';
import {ShipmentListService} from '../../../../Shipment/Services/StandardLists/ShipmentListService';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './ChooseShipmentComponent.html',
})

export class ChooseShipmentComponent {
    public EntityPM: TicketPM = new TicketPM();
    public ItemsSource: ShipmentList[] = [];
    public AllShipmentsCount: number = 0;
    private myService: ShipmentListService;
    private DomainService: ShipmentDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.DomainService = new ShipmentDomainService();
        this.myService = new ShipmentListService();
        
    }

    SetWindowArgs(entityPM: TicketPM) {
        this.EntityPM = entityPM;
        this.LoadShipmentsData();
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadShipmentsData();
        }
    }

    LoadShipmentsData() {
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = true;
        filters.GetCount = true;
        var searchValue = null;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
        }

        filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");

        if (!AppTool.IsNullOrEmpty(this.EntityPM.CompanyId)) {
            filters.addAdditionalFilter("CustomerId", this.EntityPM.CompanyId, null, null, "Equals", false, false, false, "string");
        }

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

        //this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
        //    if (!myResponse.HasError) {
        //        this.ItemsSource = myResponse.Result;
        //        this.AllShipmentsCount = this.ItemsSource.length;
        //    }
        //});

        this.DomainService.GetShipmentFullTextSearch(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result;
                this.AllShipmentsCount = this.ItemsSource.length;
            }
        });
    }

    // Filters
    private mySelectedDirectionFilter: string = "";
    get CurrentTransportModeId() { return this.mySelectedDirectionFilter; }
    set CurrentTransportModeId(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;
            this.LoadShipmentsData();
        }
    }

    private mySelectedTransportFilter: string = "";
    get CurrentDirectionId() { return this.mySelectedTransportFilter; }
    set CurrentDirectionId(newValue: string) {
        if (this.mySelectedTransportFilter != newValue) {
            this.mySelectedTransportFilter = newValue;
            this.LoadShipmentsData();
        }
    }

    // Commands
    public SelectedShipment: ShipmentList = null;
    Selecting(item: ShipmentList) {
        this.SelectedShipment = item;
        this.Close();
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {

        this.CurrentSession.CloseCurrentWindow();
    }
}
