import { Component } from '@angular/core';
import { ShipmentList } from '../../../../Shipment/EntityLists/ShipmentList';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ShipmentListService } from '../../../../Shipment/Services/StandardLists/ShipmentListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';

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
    public shipmentDomainService: ShipmentDomainService;
    private myShipmentListService: ShipmentListService; 
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        this.myShipmentListService = new ShipmentListService();
        this.shipmentDomainService = new ShipmentDomainService();
    }

    SetWindowArgs(args: any) {
        this.ShipmentType = args.ShipmentType;
        this.FromPartnerId = args.FromPartnerId;
        this.ToPartnerId = args.ToPartnerId;
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
        filters.addAdditionalFilter("MainCarriageFromPartnerId", this.FromPartnerId, null, null, "Equals", false, true, false, "string");
        filters.addAdditionalFilter("MainCarriageToPartnerId", this.ToPartnerId, null, null, "Equals", false, true, false, "string");

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
        this.GetNumberOfSelectedShipmentPackages(item);
    }

    ValidateNumberOfSelectedShipmentPackeges(item: ShipmentList, numberOfSelectedShipmentPackages: number) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Height = 200;
        if (this.NumberOfPickupDeliveryPackages == 1 && numberOfSelectedShipmentPackages >= 1) {
            messageWindow.Show(" Can't connect to Shipment " + item.ShipmentNumber + " because the number of containers should be one. Please remove the containers either from Shipment "
                + item.ShipmentNumber + " or this pickup / delivery and try again. ");
        }

        else if (numberOfSelectedShipmentPackages >= 1 && this.NumberOfPickupDeliveryPackages == 0) {
            messageWindow.Show("Can't connect to Shipment " + item.ShipmentNumber + " because it has containers. Please remove the containers and try again.");
        }

        else {
            this.ConfirmSelectedShipment(item);
        }
    }

    ConfirmSelectedShipment(item: ShipmentList) {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "";
        confirmWindow.Show("Different Fields and Containers will be Updated from the shipment level when connecting the shipment to this leg");
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

    GetNumberOfSelectedShipmentPackages(shipment: ShipmentList) {
        this.CurrentSession.StartBusyIndicatorLoading();
        var shipmentId = shipment.Id;
        this.shipmentDomainService.GetNumberOfShipmentPackages(shipmentId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CurrentSession.StopBusyIndicator();
                var numberOfSelectedShipmentPackages = myResponse.Result;
                this.ValidateNumberOfSelectedShipmentPackeges(shipment, numberOfSelectedShipmentPackages)
            }
            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
