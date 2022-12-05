import { BaseComponent } from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Guid } from '../../Infrastructure/Utilities/Guid';
import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../Infrastructure/Tools';
import { WarehouseEntryList } from '../EntityLists/WarehouseEntryList';
import { WarehouseEntryPMService } from '../Services/StandardPMs/WarehouseEntryPMService';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { WarehouseEntryPM } from '../EntityPMs/WarehouseEntryPM';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';

@Component({
    selector: 'ChooseWarehouseEntryComponent',
    templateUrl: './ChooseWarehouseEntryComponent.html',
})

export class ChooseWarehouseEntryComponent extends BaseComponent implements OnInit {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ObjectTableName: string = "WarehouseEntry";
    public ValidationErrorsList: string[];
    WarehouseEntries: WarehouseEntryClass[] = [];
    AllWarehouseEntries: any[] = [];
    SelectedWarehouseEntry: WarehouseEntryClass;
    ShipmentPM: any;
    ShipmentPickUpPM: any;
    DataContext: any = this;
    IsLoadPage: boolean = false;
    ConnectedTo: string;
    ParentComponent: any;
    private CurrentSession = SessionLocator.SelectedSession;
    private warehouseEntryPMService: WarehouseEntryPMService;
    constructor() {
        super();
        this.warehouseEntryPMService = new WarehouseEntryPMService();
    }

    ngOnInit() {
    }

    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.Start(args);
        });
    }

    Start(args) {
        this.ShipmentPM = args.EntityPM;
        this.ShipmentPickUpPM = args.EntityChildPM;
        this.CustomerId = args.EntityPM.CustomerId;
        this.WarehouseId = args.WarehouseId;
        this.ParentComponent = args.ParentComponent;
        this.ConnectedTo = args.ConnectedTo;
        this.AllWarehouseEntries = args.WarehouseEntries;
        this.FilterWarehouseEntries();
        this.IsLoadPage = true;
    }
    
    IsShowMessageNoResult: boolean = false;
    FilterWarehouseEntries() {
        this.WarehouseEntries = [];
        this.AllWarehouseEntries.forEach(warehouseEntry => {
            this.WarehouseEntries.push(new WarehouseEntryClass(warehouseEntry));
        });
        if (!AppTool.IsNullOrEmpty(this.TransportModeId) && this.TransportModeId != "All") {
            this.WarehouseEntries = this.WarehouseEntries.filter(d => d.WarehouseEntryList.TransportModeId == this.TransportModeId);
        }
        if (!AppTool.IsNullOrEmpty(this.DirectionId) && this.DirectionId != "All") {
            this.WarehouseEntries = this.WarehouseEntries.filter(d => d.WarehouseEntryList.DirectionId == this.DirectionId);
        }
        this.IsShowMessageNoResult = this.WarehouseEntries.length == 0;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        if (!this.SelectedWarehouseEntry || !this.SelectedWarehouseEntry.WarehouseEntryList) {
            this.ValidationErrorsList.push("You must choose one entry");
        }
        if (this.ValidationErrorsList.length != 0) return;

        this.CurrentSession.StartBusyIndicator("Loading...");
        this.warehouseEntryPMService.get(this.SelectedWarehouseEntry.WarehouseEntryList.Id).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError) {
                this.ShowErrorMessage(serviceResponse);
                return;
            }

            this.UpdateWarehouseEntry(serviceResponse.Result);
        });
    }

    private ShowErrorMessage(response: ServiceResponse) {
        this.CurrentSession.StopBusyIndicator();
        const messageWindow = new MessageWindow();
        messageWindow.Show("Error found, please check browser console");
        console.error(response.ErrorsArray);
    }

    private UpdateWarehouseEntry(warehouseEntryPM: WarehouseEntryPM) {
        if (!warehouseEntryPM) return;
        this.MapwarehouseEntryPM(warehouseEntryPM);
        this.warehouseEntryPMService.update(warehouseEntryPM).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError) {
                this.ShowErrorMessage(serviceResponse);
                return;
            }
            this.CurrentSession.StopBusyIndicator();
            if (this.ParentComponent) this.ParentComponent.LoadConnectedWarehouseEntry();
            this.CurrentSession.CurrentWindow.Close("Refresh");
        });
    }

    private MapwarehouseEntryPM(warehouseEntryPM: WarehouseEntryPM) {
        warehouseEntryPM.ReceivedBy = SessionLocator.LoggedUserPM.EnglishName;
        warehouseEntryPM.ShipmentId = this.ShipmentPM.Id;
        warehouseEntryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        warehouseEntryPM.HouseNumber = this.ShipmentPM.House;
        warehouseEntryPM.MasterNumber = this.ShipmentPM.LongMaster;
        warehouseEntryPM.ShipmentLevelCode = this.ShipmentPM.ShipmentLevelCode;
        warehouseEntryPM.ShipmentTypeId = this.ShipmentPM.ShipmentTypeId;
        warehouseEntryPM.ConnectedTo = this.ConnectedTo;
        warehouseEntryPM.ConnectedToShipment = true;
        warehouseEntryPM.ConnectedToReferenceNumber = this.ShipmentPickUpPM.PickUpDeliveryNumber;
    }

    private transportModeId: string = "All";
    get TransportModeId() {
        return this.transportModeId;
    }
    set TransportModeId(newValue: string) {
        if (this.transportModeId == newValue) return;
        this.transportModeId = newValue;
        this.FilterWarehouseEntries();
    }

    private directionId: string = "All";
    get DirectionId() { return this.directionId; }
    set DirectionId(newValue: string) {
        if (this.directionId == newValue) return;
        this.directionId = newValue;
        this.FilterWarehouseEntries();
    }

    private customerId: string;
    get CustomerId() {
        return this.customerId;
    }

    set CustomerId(newValue: string) {
        if (this.customerId == newValue) return;
        this.customerId = newValue;
        this.FilterWarehouseEntries();
    }

    private warehouseId: string;
    get WarehouseId() {
        return this.warehouseId;
    }

    set WarehouseId(newValue: string) {
        if (this.warehouseId == newValue) return;
        this.warehouseId = newValue;
        this.FilterWarehouseEntries();
    }

    ViewWarehouseEntry(EntryId: any) {
        let myBackButtonLabel = "Choose Cross Dock Package";
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: EntryId, ObjectTableName: this.ObjectTableName, BackButtonLabel: myBackButtonLabel });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    //this.LoadWarehouseEntries();
                });
            });
    }
}

export class WarehouseEntryClass extends BaseComponent {
    WarehouseEntryList: WarehouseEntryList;
    IsSelectedKeyId: string = Guid.newGuid();
    constructor(warehouseEntryList: WarehouseEntryList) {
        super();
        this.WarehouseEntryList = warehouseEntryList;
    }
}
