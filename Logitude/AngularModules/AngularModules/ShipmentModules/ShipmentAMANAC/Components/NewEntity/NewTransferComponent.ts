import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AccountingTransferHeaderPM } from '../../../../Invoice/EntityPMs/AccountingTransferHeaderPM';
import { AccountingTransferLinePM } from '../../../../Invoice/EntityPMs/AccountingTransferLinePM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ShipmentList } from '../../../../Shipment/EntityLists/ShipmentList';
import { ShipmentListService } from '../../../../Shipment/Services/StandardLists/ShipmentListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';

@Component({
    moduleId: module.id,
    templateUrl: './NewTransferComponent.html',
})

export class NewTransferComponent extends BaseComponent {
    public EntityPM: AccountingTransferHeaderPM = null;
    public ObjectTableName: string = "AccountingTransferHeader";
    public DataContext = this;
    public TransferTypeCode: string = null;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: NewTransferLine[] = [];
    public SelectedItem: NewTransferLine = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new AccountingTransferHeaderPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.UserId = SessionLocator.LoggedUserId;
        this.EntityPM.TransferDate = DateTool.GetCurrentDateTimeAsUtc();
        this.BuildShipmentDatesList();
        this.Listen();
    }

    private Listen() {
        this.CurrentSession.SessionEvent.subscribe(s => {


        });
    }

    SetWindowArgs(transferTypeCode: string) {
        this.TransferTypeCode = transferTypeCode;
        this.EntityPM.AccountingTransferTypeCode = transferTypeCode;
        this.LoadData();
    }

    private isAllChecked: boolean = true;
    get IsAllChecked() { return this.isAllChecked; }
    set IsAllChecked(value: boolean) {
        if (this.isAllChecked != value) {
            this.isAllChecked = value;

            this.ItemsSource.forEach(item => {
                item.IsChecked = value;
            });

            this.OnLinesSelected();
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
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

    private entityListService: any = null;
    LoadData() {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        filters.addAdditionalFilter("LocalCustomsTransmissionsStatusCode", "NSEN", null, null, "Equals", false, true, false, "string");

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

        if (this.TransferTypeCode == "Air") {
            filters.addAdditionalFilter("TransportModeId", "A", null, null, "Equals", false, true, false, "string");
        }
        else {
            filters.addAdditionalFilter("TransportModeId", "O", null, null, "Equals", false, true, false, "string");
        }

        if (this.entityListService == null) {
            this.entityListService = new ShipmentListService();
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
        this.SelectedItem = null;
        var myResultList: NewTransferLine[] = [];

        if (!myResponse.HasError) {
            if (myResponse.Result.length > 0) {


                myResponse.Result.forEach((item: ShipmentList) => {
                    var myResultItem = new NewTransferLine(this);
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
        this.IsFirstTimeLoading = false;
        this.OnLinesSelected();
    }


    public SelectedCount: number = 0;
    public ExportButtonIsEnabled: boolean = false;
    public IsFirstTimeLoading: boolean = true;
    OnLinesSelected() {
        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;
        this.ExportButtonIsEnabled = this.SelectedCount > 0 ? true : false;
    }

    ExportButtonClicked() {
        var errors: string[] = [];
        
        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;

        if (this.SelectedCount == 0) {
            errors.push("You must select 1 line at least");
        }

        else if (this.SelectedCount > 100) {
            errors.push("You must select 100 line max");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
           
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class NewTransferLine {
    constructor(private fatherComponent: NewTransferComponent) {
        if (fatherComponent.IsFirstTimeLoading) {
            this.isChecked = true;
        }
    }

    public Id: string;
    public Date: Date;
    public DateTicks: number;
    public ShipmentNumber: string;
    public Shipper: string;
    public Status: string;
    public Consignee: string;
    public TransferError: string;
    public ReadyForTransfer: boolean;

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            this.fatherComponent.OnLinesSelected();    
        }
    }
}
