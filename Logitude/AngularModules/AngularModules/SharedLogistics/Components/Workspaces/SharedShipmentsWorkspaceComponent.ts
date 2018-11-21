import { Component, OnInit } from '@angular/core';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ShipmentList } from '../../../Shipment/EntityLists/ShipmentList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SharedLogisticsService, ShipmentFilters } from '../../Services/Others/SharedLogisticsService';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AppTool, DateTool, DateFormats } from '../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './SharedShipmentsWorkspaceComponent.html',
})

export class SharedShipmentsWorkspaceComponent implements OnInit {
    public IsResourcesReady: boolean = false;
    private myService: SharedLogisticsService;
    constructor(private _entityResourceService: EntityResourceService) {
        this.myService = new SharedLogisticsService();
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(response => {
            this.IsResourcesReady = true;
            this.SetSelectedItem();

            this.LoadShipments();
        });
    }

    public InProgressCount: number;
    public AllShipmentsCount: number;

    private SetSelectedItem() {
        this.SelectedItem = "PROG";
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
        }
    }

    RefreshButtonClicked() {

    }

    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(value: string) {
        if (this.mySelectedTransportFilter != value) {
            this.mySelectedTransportFilter = value;

        }
    }

    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(value: string) {
        if (this.mySelectedDirectionFilter != value) {
            this.mySelectedDirectionFilter = value;

        }
    }

    public InProgressShipmentsList: ShipmentEntity[];
    LoadShipments() {
        this.InProgressShipmentsList = [];

        var filters: ShipmentFilters = new ShipmentFilters();
        filters.PartnerId = SessionInfo.LoggedUserCardId;
        filters.PartnerType = SessionInfo.LoggedUserCardType;
        filters.DirectionId = this.SelectedDirectionFilter;
        filters.TransportModeId = this.SelectedTransportFilter;
        //filters.ShipmentLevelCode = ($.trim($.SelectedShipmentLevel) == "" || $.trim($.SelectedShipmentLevel) == "All") ? null : $.SelectedShipmentLevel;
        //filters.SearchField = ($.trim($.SearchText_SHI) == "" || $.trim($.SearchText_SHI) == $.watermark_SHI) ? null : $.trim($.SearchText_SHI);
        filters.PageSize = 1000000;
        filters.PageIndex = 0

        if (this.SelectedItem == "PROG") {
            filters.IsOperationalClosed = false;
        }
        else {
            filters.IsOperationalClosed = null;
        }

        this.myService.GetSharedShipments(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myList: ShipmentList[] = myResponse.Result;

                    myList.forEach((item) => {
                        this.InProgressShipmentsList.push(new ShipmentEntity(item));
                    });

                    this.InProgressCount = this.InProgressShipmentsList.length;
                    if (this.InProgressShipmentsList.length == 0) {
                        //no data
                    }
                }
            }
        });
    }
}

export class ShipmentEntity {
    private myEntity: ShipmentList;
    constructor(entity: ShipmentList) {
        this.myEntity = entity;
    }

    get DirectionId() { return this.myEntity.DirectionId; }
    get TransportModeId() { return this.myEntity.TransportModeId; }
    get ShipmentNumber() { return this.myEntity.ShipmentNumber; }
    get StatusName() { return this.myEntity.StatusName; }
    get IncotermCode() { return this.myEntity.IncotermCode; }
    get ShipmentType() { return this.myEntity.ShipmentType; }
    get DescriptionOfGoods() { return this.myEntity.DescriptionOfGoods; }

    get PartnerName() {
        var result = "";

        if (this.myEntity.ShipmentLevelCode == "C") {
            result = this.myEntity.AgentName;
        }

        else {
            if (this.myEntity.DirectionId == "I") {
                result = this.myEntity.Shipper;
            }

            else {
                result = this.myEntity.Consignee;
            }
        }

        return result;
    }

    get FromCountyCode() {
        var result = "";

        if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
            result = this.myEntity.MainCarriageFromCountryCode;
        }

        else {
            result = this.myEntity.FromCountryCode;
        }

        return result;
    }

    get FromPortCode() {
        var result = "";

        if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
            result = "";
        }

        else {
            result = this.myEntity.FromPort;
        }

        return result;
    }

    get FromPortName() {
        var result = "";

        if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
            result = this.myEntity.MainCarriageFromCity;
        }

        else {
            result = this.myEntity.FromPortName;
        }

        return result;
    }

    get ToCountyCode() {
        var result = "";

        if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
            result = this.myEntity.MainCarriageToCountryCode;
        }

        else {
            result = this.myEntity.ToCountryCode;
        }

        return result;
    }

    get ToPortCode() {
        var result = "";

        if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
            result = "";
        }

        else {
            result = this.myEntity.ToPort;
        }

        return result;
    }

    get ToPortName() {
        var result = "";

        if (this.myEntity.DirectionId == "D" && this.myEntity.TransportModeId == "I") {
            result = this.myEntity.MainCarriageToCity;
        }

        else {
            result = this.myEntity.ToPortName;
        }

        return result;
    }

    get StatusNameAndLocation() {
        var result = "";

        if (!AppTool.IsNullOrEmpty(this.myEntity.StatusName)) {
            result = this.myEntity.StatusName;
        }

        if (!AppTool.IsNullOrEmpty(this.myEntity.StatusLocation)) {
            result += " (" + this.myEntity.StatusLocation + ")";
        }

        return result;
    }

    get MyReference() {
        var result = "";

        if (this.myEntity.ShipmentLevelCode == "C") {            
            result = this.myEntity.AgentReference1;
            if (!AppTool.IsNullOrEmpty(this.myEntity.AgentReference2)) {
                result = (result == "") ? this.myEntity.AgentReference2 : result + ", " + this.myEntity.AgentReference2;
            }
        }

        else {
            result = this.myEntity.CustomerReference1;
            if (!AppTool.IsNullOrEmpty(this.myEntity.CustomerReference2)) {
                result = (result == "") ? this.myEntity.CustomerReference2 : result + ", " + this.myEntity.CustomerReference2;
            }
        }

        return result;
    }

    get FromDate() {
        var result = "";
        var myATD: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageATD);
        var myETD: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageETD);

        if (myATD != null) {
            result = myATD.ShortDateString;           
        }

        else {
            if (myETD != null) {
                result = myETD.ShortDateString;                
            }

            else {
                result = "No Date";
            }
        }

        return result;
    }

    get FromDateColor() {
        var result = "";

        if (this.myEntity.MainCarriageATD != null) {
            result = "#282E30";
        }

        else {
            if (this.myEntity.MainCarriageETD != null) {
                result = "#282E30";
            }

            else {
                result = "Silver";
            }
        }

        return result;
    }

    get FromDateType() {
        var result = "";

        if (this.myEntity.MainCarriageATD != null) {
            result = "(Actual)";
        }

        else {
            if (this.myEntity.MainCarriageETD != null) {
                result = "(Estimate)";
            }
        }

        return result;
    }

    get FromDateTypeColor() {
        var result = "";

        if (this.myEntity.MainCarriageATD != null) {
            result = "#009161";
        }

        else {
            if (this.myEntity.MainCarriageETD != null) {
                result = "#6E7172";
            }
        }

        return result;
    }
    
    get ToDate() {
        var result = "";
        var myATA: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationATA);
        var myETA: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationETA);

        if (myATA != null) {
            result = myATA.ShortDateString;
        }

        else {
            if (myETA != null) {
                result = myETA.ShortDateString;
            }

            else {
                result = "No Date";
            }
        }

        return result;
    }

    get ToDateColor() {
        var result = "";

        if (this.myEntity.MainCarriageFinalDestinationATA != null) {
            result = "#282E30";
        }

        else {
            if (this.myEntity.MainCarriageFinalDestinationETA != null) {
                result = "#282E30";
            }

            else {
                result = "Silver";
            }
        }

        return result;
    }

    get ToDateType() {
        var result = "";

        if (this.myEntity.MainCarriageFinalDestinationATA != null) {
            result = "(Actual)";
        }

        else {
            if (this.myEntity.MainCarriageFinalDestinationETA != null) {
                result = "(Estimate)";
            }
        }

        return result;
    }

    get ToDateTypeColor() {
        var result = "";

        if (this.myEntity.MainCarriageFinalDestinationATA != null) {
            result = "#009161";
        }

        else {
            if (this.myEntity.MainCarriageFinalDestinationETA != null) {
                result = "#6E7172";
            }
        }

        return result;
    }

    get StatusDate() {
        var result = "";
        var date: DateFormats = DateTool.GetDateFormats(this.myEntity.StatusDate);

        if (date != null) {
            result = date.ShortDateString;
        }

        return result;
    }

    get ReferenceLabel() {
        var result = "";

        if (this.myEntity.ShipmentLevelCode == "H") {            
            result = "House:";
        }

        else {
            result = "Master:";
        }

        return result;
    }

    get Reference() {
        var result = "";

        if (this.myEntity.ShipmentLevelCode == "H") {
            result = this.myEntity.House;
        }

        else {
            result = this.myEntity.LongMaster;
        }

        return result;
    }

    get FromTime() {
        var result = "";
        var myATD: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageATD);
        var myETD: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageETD);

        if (myATD != null) {
            result = myATD.ShortTimeString;
        }

        else {
            if (myETD != null) {
                result = myETD.ShortTimeString;
            }
        }

        return result;
    }

    get ToTime() {
        var result = "";
        var myATA: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationATA);
        var myETA: DateFormats = DateTool.GetDateFormats(this.myEntity.MainCarriageFinalDestinationETA);

        if (myATA != null) {
            result = myATA.ShortTimeString;
        }

        else {
            if (myETA != null) {
                result = myETA.ShortTimeString;
            }
        }

        return result;
    }
}
