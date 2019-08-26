import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ShipmentList } from '../../../Shipment/EntityLists/ShipmentList';
import { ShipmentListService } from '../../../Shipment/Services/StandardLists/ShipmentListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { ShipmentDomainService } from '../../../Shipment/Services/ShipmentDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './BlockedShipmentsComponent.html',
})

export class BlockedShipmentsComponent extends BaseComponent {
    public DataContext: BlockedShipmentsComponent = this;    
    public ItemsSource: BlockedShipmentItem[] = [];    
    private CurrentSession = SessionLocator.SelectedSession;
    private shipmentDomainService: ShipmentDomainService;
    private entityListService: ShipmentListService;
    public ObjectTableName: string = "Shipment";
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();      

        this.shipmentDomainService = new ShipmentDomainService();
        this.entityListService = new ShipmentListService();
        this.BuildShipmentDatesList();
    }

    public TransportModeCode: string = null;
    SetWindowArgs(transportModeCode: string) {
        this.TransportModeCode = transportModeCode;
        this.LoadData();
    }
    
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.LoadData();
        }
    }

    private toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.LoadData();
        }
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadData();
        }
    }

    private myCurrentDirectionId: string = "";
    get CurrentDirectionId() { return this.myCurrentDirectionId; }
    set CurrentDirectionId(newValue: string) {
        if (this.myCurrentDirectionId != newValue) {
            this.myCurrentDirectionId = newValue;
            this.LoadData();
        }
    }

    public ShipmentDatesList: CodeNameClass[] = [];
    private selectedShipmentDateCode: string;
    private BuildShipmentDatesList() {
        this.ShipmentDatesList = [];
        this.ShipmentDatesList.push(new CodeNameClass("ETA", "ETA"));
        this.ShipmentDatesList.push(new CodeNameClass("ATA", "ATA"));
        this.ShipmentDateSelectedItem = this.ShipmentDatesList.filter(d => d.Code == "ETA")[0];
    }

    private shipmentDateSelectedItem: CodeNameClass;
    get ShipmentDateSelectedItem() { return this.shipmentDateSelectedItem; }
    set ShipmentDateSelectedItem(value: CodeNameClass) {
        if (this.shipmentDateSelectedItem != value) {
            this.shipmentDateSelectedItem = value;

            if (value == null) {
                this.selectedShipmentDateCode = null;
            }

            else {
                this.selectedShipmentDateCode = value.Code;
            }
        }
    }

    SelectedShipmentDateChanged(item) {
        this.ShipmentDateSelectedItem = this.ShipmentDatesList.filter(d => d.Code == item.Code)[0];
        this.selectedShipmentDateCode = item.Code;
        this.LoadData();
    }
    
    LoadData() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        filters.addAdditionalFilter("LocalCustomsTransmissionsStatusCode", "BLOK", null, null, "Equals", false, true, false, "string");

        if (!AppTool.IsNullOrEmpty(this.CurrentDirectionId)) {
            if (this.CurrentDirectionId == "All") this.CurrentDirectionId = "";
            else {
                filters.addAdditionalFilter("DirectionId", this.CurrentDirectionId, null, null, "Equals", false, true, false, "string");
            }
        }

        var date = "MainCarriageETA";
        if (this.selectedShipmentDateCode == "ATA") {
            date = "MainCarriageATA";
        }

        this.AppendDateFilter(filters, date);
        filters.addAdditionalFilter("ShipmentLevelCode", "D,H", null, null, "InList", false, true, false, "string");

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, true, false, "string");
        }

        if (this.TransportModeCode == "Air") {
            filters.addAdditionalFilter("TransportModeId", "A", null, null, "Equals", false, true, false, "string");
        }
        else {
            filters.addAdditionalFilter("TransportModeId", "O", null, null, "Equals", false, true, false, "string");
        }
        
        this.entityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            this.BuildItemsSource(myResponse);
        });
    }

    AppendDateFilter(filters: ApiQueryFilters, myFilterField: string) {
        if (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter(myFilterField, this.FromDate, this.ToDate, null, "Between", false, true, false, "date");
        }

        else if (!AppTool.IsNullOrEmpty(this.FromDate)) {
            filters.addAdditionalFilter(myFilterField, this.FromDate, null, null, "GreaterThanOrEqual", false, true, false, "date");
        }

        else if (!AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter(myFilterField, this.ToDate, null, null, "LessThanOrEqual", false, true, false, "date");
        }
    }

    BuildItemsSource(myResponse: ServiceResponse) {
        this.ItemsSource = [];        
        var myResultList: BlockedShipmentItem[] = [];

        if (!myResponse.HasError) {
            if (myResponse.Result.length > 0) {


                myResponse.Result.forEach((item: ShipmentList) => {
                    var myResultItem = new BlockedShipmentItem();
                    myResultItem.Id = item.Id;

                    if (item.MainCarriageATA != null) {
                        myResultItem.Date = item.MainCarriageATA;
                    }
                    else {
                        myResultItem.Date = item.MainCarriageETA;
                    }

                    myResultItem.DateTicks = DateTool.GetDateParts(item.FinalArrivalDate).DateTicks;
                    myResultItem.Status = item.StatusName;
                    myResultItem.Shipper = item.ShipperName;
                    myResultItem.Consignee = item.ConsigneeName;
                    myResultItem.ShipmentNumber = item.ShipmentNumber;
                    myResultList.push(myResultItem);
                });
            }
        }

        this.ItemsSource = myResultList.sort(function (a, b) { return a.DateTicks == b.DateTicks ? 0 : a.DateTicks < b.DateTicks ? -1 : 1; });
    }
    
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    UnblockedClicked(itemId: string) {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.shipmentDomainService.UnblockedShipment(itemId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LoadData();
            }

            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
}

export class BlockedShipmentItem {    
    public Id: string;
    public Date: Date;
    public DateTicks: number;
    public ShipmentNumber: string;
    public Shipper: string;
    public Status: string;
    public Consignee: string;
    public TransferError: string;
    public ReadyForTransfer: boolean;   
}
