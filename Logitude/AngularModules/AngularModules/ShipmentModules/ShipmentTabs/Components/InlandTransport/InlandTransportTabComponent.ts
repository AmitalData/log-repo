import {  Component, EventEmitter, OnInit, Output } from "@angular/core";
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentDeliveryPM } from "../../../../Shipment/EntityPMs/ShipmentDeliveryPM";

import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { ColumnsWidths } from "Infrastructure/Components/LogitudeComponents/LogLovV2Component";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ShipmentPMService } from "Shipment/Services/StandardPMs/ShipmentPMService";

@Component({    
    templateUrl: './InlandTransportTabComponent.html',
})

// export class InlandTransportTabComponent extends BaseRequestsSheetMassaging implements OnInit {
export class InlandTransportTabComponent extends BaseComponent implements OnInit {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;
    public ColumnsWidths: ColumnsWidths[];
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public ItemsSource: ObservableCollection;
    public IsVisible = false;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;

        this.ItemsSource = new ObservableCollection([]);

        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();    
    }

    
    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe((response: any) => {
            this.IsVisible = true;
            this.ReloadMyScreen();
        });
    }

    ReloadMyScreen() {
        this.ItemsSource.Clear();
        this.EntityPM.ShipmentDeliveries.forEach(item => {
            item.EntityParentPM = this.EntityPM;
            this.ItemsSource.Insert(item, true);
        });
        
        this.ObjectTableName = this.entityArgs.ObjectTableName;
    }

    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.ReloadMyScreen();
                }
            });
        }
    }

    @Output() MenuHeaderchangeevent = new EventEmitter();

    // public SelectedRow: ShipmentDeliveryPM = null;
    OnRowSelected(itemComponent: ShipmentDeliveryPM) {
    //     this.SelectedRow = itemComponent;
    //     this.filterAgrs = new ApiQueryFilters();

    //     this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    filterAgrs: ApiQueryFilters;
    isOpen:boolean;
    EditButtonClicked(item: ShipmentDeliveryPM) {
        this.selectedItem = item;
        this.AddNewInlandTransport(true);
    }
    
    selectedItem = new ShipmentDeliveryPM(null);
    AddNewInlandTransport(isEdit: boolean) {

        var args: any = {
            Shipment: this.EntityPM,
            InlandTransport: null,
            IsNewEntity: false
        };

        if(!isEdit){
            args.IsNewEntity = true;
        }
        else {
            args.InlandTransport = this.selectedItem;
        }

        this.OpenAddEditWindow(args);
    }

    OpenAddEditWindow(args){
        if(this.isOpen) return;

        this.isOpen = true;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.TH.InlandTransport");
        logitudeWindow.WindowArgs = args;

        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;
        logitudeWindow.Show('./ShipmentModules/ShipmentTabs/Components/InlandTransport/AddEditInlandTransportComponent');

        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            this.ReloadMyScreen();
            this.isOpen = false;
        });
    }

    DeleteButtonClicked(item: ShipmentDeliveryPM) {
        if (!item) return;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show(TextCodeTranslator.Translate("ShipmentPickUpDelivery.O.RemoveDelivery"));
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.DeleteSelected(item);
            }
        });
    }

    DeleteSelected(item: ShipmentDeliveryPM) {
        this.EntityPM.RemoveDelivery(item);
        this.ItemsSource.Remove(item);
        this.CurrentSession.StartBusyIndicator("");

        var entityPMService = new ShipmentPMService();
        entityPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private CurrentSession = SessionLocator.SelectedSession;

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
