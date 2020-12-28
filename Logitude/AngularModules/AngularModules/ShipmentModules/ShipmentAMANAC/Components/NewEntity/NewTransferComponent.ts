import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ShipmentList } from '../../../../Shipment/EntityLists/ShipmentList';
import { ShipmentListService } from '../../../../Shipment/Services/StandardLists/ShipmentListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { CustomsTransferHeaderPM } from '../../../../Shipment/EntityPMs/CustomsTransferHeaderPM';
import { CustomsTransferLinePM } from '../../../../Shipment/EntityPMs/CustomsTransferLinePM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    
    templateUrl: './NewTransferComponent.html',
})

export class NewTransferComponent extends BaseComponent {
    public EntityPM: CustomsTransferHeaderPM = null;
    public ObjectTableName: string = "CustomsTransferHeader";
    public DataContext = this;
    public TransferTypeCode: string = null;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: NewTransferLine[] = [];
    public SelectedItem: NewTransferLine = null;
    private CurrentSession = SessionLocator.SelectedSession;
    private shipmentDomainService: ShipmentDomainService;
    constructor() {
        super();
        this.InitEntityPM();
        this.shipmentDomainService = new ShipmentDomainService();
        this.BuildShipmentDatesList();
        this.Listen();
    }

    private InitEntityPM() {
        this.EntityPM = new CustomsTransferHeaderPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.TransferDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.CustomsTransferTypeCode = this.TransferTypeCode;
    }

    private Listen() {
        this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "TransferCompleted") {
                this.InitEntityPM();
                this.LoadData()
            }
        });
    }

    SetWindowArgs(transferTypeCode: string) {
        this.TransferTypeCode = transferTypeCode;
        this.EntityPM.CustomsTransferTypeCode = transferTypeCode;
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
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsAccountingClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("AMANACShipmentsFilter", true, null, null, "Equals", true, false, false, "Boolean");

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, true, false, "string");
        }

        if (this.TransferTypeCode == "AMAS") {
            filters.addAdditionalFilter("TransportModeId", "A", null, null, "Equals", false, true, false, "string");
        }

        else if (this.TransferTypeCode == "AMOS"){
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
        this.OnLinesSelected();
    }
    
    public SelectedCount: number = 0;
    public ExportButtonIsEnabled: boolean = false;
    OnLinesSelected() {
        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;
        this.ExportButtonIsEnabled = this.SelectedCount > 0 ? true : false;
    }

    ExportButtonClicked() {
        this.EntityPM.CustomsTransferLines = [];

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;

        if (this.SelectedCount == 0) {
            errors.push("You must select 1 line at least");
        }

        else if (this.SelectedCount > 100) {
            errors.push("You must select 100 line max");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var logWindow = new LogitudeWindow();
            logWindow.Title = "";
            logWindow.Width = 500;
            logWindow.Height = 200;
            logWindow.Show('./ShipmentModules/ShipmentAMANAC/Components/NewEntity/AMANACExportTransferComponent');
            logWindow.ComponentLoaded.subscribe(comp => {

                this.ItemsSource.filter(f => f.IsChecked == true).forEach(item => {
                    var TransferLinePM = new CustomsTransferLinePM(null);
                    TransferLinePM.Tenant = SessionLocator.Tenant;
                    TransferLinePM.ShipmentId = item.Id;
                    TransferLinePM.ShipmentNumber = item.ShipmentNumber;
                    this.EntityPM.AddCustomsTransferLinePM(TransferLinePM);
                });

                comp.Export(this.EntityPM);
            }); 
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    MarkAsBlockedClicked(itemId: string) {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.shipmentDomainService.MarkShipmentAsBlocked(itemId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.LoadData();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
}

export class NewTransferLine {
    constructor(private fatherComponent: NewTransferComponent) {
        this.isChecked = fatherComponent.IsAllChecked;
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
