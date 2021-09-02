import { Component } from '@angular/core';
import { ShipmentList } from '../../../../Shipment/EntityLists/ShipmentList';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ShipmentListService } from '../../../../Shipment/Services/StandardLists/ShipmentListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    templateUrl: './ChooseStandaloneShipmentComponent.html',
})

export class ChooseStandaloneShipmentComponent {
    public EntityPM: any;
    public ItemsSource: ShipmentList[] = [];
    public AllShipmentsCount: number = 0;
    public ShipmentType: string;
    public FromPartnerId: string;
    public ToPartnerId: string;
    public CarrierId: string;
    public NumberOfPickupDeliveryPackages: number;   
    public EntityId: string;
    public FromType: string;
    public ToType: string;
    public FromPortId: string;
    public ToPortId: string;
    public FromCity: string;
    public ToCity: string;
    public FromCountryId: string;
    public ToCountryId: string;
    private myShipmentListService: ShipmentListService; 
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        this.myShipmentListService = new ShipmentListService();
    }

    SetWindowArgs(args: any) {
        this.ShipmentType = args.ShipmentType;
        this.FromType = args.FromType;
        this.ToType = args.ToType;
        this.FromPartnerId = args.FromPartnerId;
        this.ToPartnerId = args.ToPartnerId;
        this.FromPortId = args.FromPortId;
        this.ToPortId = args.ToPortId;
        this.FromCity = args.FromCity;
        this.ToCity = args.ToCity;
        this.FromCountryId = args.FromCountryId;
        this.ToCountryId = args.ToCountryId;
        this.CarrierId = args.CarrierId;
        this.NumberOfPickupDeliveryPackages = args.NumberOfPackages;
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
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = false;
        filters.GetCount = true;
        var searchValue = null;

        filters.addAdditionalFilter("ShipmentLevelCode", "D", null, null, "Equals", true, true, false, "string");
        filters.addAdditionalFilter("DirectionId", "D", null, null, "Equals", false, true, false, "string");
        filters.addAdditionalFilter("TransportModeId", "I", null, null, "Equals", false, true, false, "string");
        filters.addAdditionalFilter("IsStandalonePickupDelivery", false, null, null, "Equals", true, false, false, "Boolean");        

        if (!AppTool.IsNullOrEmpty(this.CarrierId)) {
            filters.addAdditionalFilter("MainCarriageCarrierId", this.CarrierId, null, null, "Equals", false, true, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
            filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentType) && ["FCL", "FCLD"].includes(this.ShipmentType)) {
            filters.addAdditionalFilter("ShipmentTypeId", "FTL", null, null, "Equals", true, false, false, "string");
        }

        else if (!AppTool.IsNullOrEmpty(this.ShipmentType) && ["LCL", "LCLD", "Air"].includes(this.ShipmentType)) {
            filters.addAdditionalFilter("ShipmentTypeId", "LTL", null, null, "Equals", true, false, false, "string");
        }

        else {
            filters.addAdditionalFilter("ShipmentTypeId", this.ShipmentType, null, null, "Equals", true, false, false, "string");
        }

        switch (this.FromType) {
            case "PART": {
                filters.addAdditionalFilter("MainCarriageFromPartnerId", this.FromPartnerId, null, null, "Equals", false, true, false, "string");       
                break;
            }

            case "PORT": {
                filters.addAdditionalFilter("MainCarriageFromPortId", this.FromPortId, null, null, "Equals", false, true, false, "string"); 
                break;
            }

            case "CASL": {
                filters.addAdditionalFilter("InlandDomesticFromCity", this.FromCity, null, null, "Equals", false, true, false, "string");
                filters.addAdditionalFilter("InlandDomesticFromCountryId", this.FromCountryId, null, null, "Equals", false, true, false, "string"); 
                break;
            }
        }

        switch (this.ToType) {
            case "PART": {
                 filters.addAdditionalFilter("MainCarriageToPartnerId", this.ToPartnerId, null, null, "Equals", false, true, false, "string");
                break;
            }

            case "PORT": {
                filters.addAdditionalFilter("MainCarriageFinalDestinationPortId", this.ToPortId, null, null, "Equals", false, true, false, "string");
                break;
            }

            case "CASL": {
                filters.addAdditionalFilter("InlandDomesticToCity", this.ToCity, null, null, "Equals", false, true, false, "string");
                filters.addAdditionalFilter("InlandDomesticToCountryId", this.ToCountryId, null, null, "Equals", false, true, false, "string"); 
                break;
            }
        }

        this.myShipmentListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result;
                this.AllShipmentsCount = this.ItemsSource.length;
            }
        });
    }

    // Commands
    public SelectedShipment: ShipmentList = null;

    Selecting(item: ShipmentList) {
        this.ConfirmSelectedShipment(item);
    }

    ConfirmSelectedShipment(item: ShipmentList) {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "";
        confirmWindow.Show("Different Fields will be Updated and Containers will be Deleted from the shipment level when connecting the shipment to this leg");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SelectedShipment = item;
                this.Close();
            }

            else if (confirmWindow.No) {

            }
        });  
    }

    CloseButtonClicked() {
        this.Close();
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
