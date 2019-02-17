import {Component, OnInit, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteDomainService, QuoteConnectedEntity} from '../../../../Quote/Services/QuoteDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'ConnectionsTabComponent',
    moduleId: module.id,
    templateUrl: './ConnectionsTabComponent.html',
})

export class ConnectionsTabComponent implements OnInit, OnDestroy {
    public EntityPM: QuotePM;
    public ObjectTableName: string;
    private myDomainService: QuoteDomainService;
    public ItemsSource: QuoteConnectedEntityItem[] = [];
    public ShipmentsItemsSource: QuoteConnectedEntityItem[];
    public TicketsItemsSource: QuoteConnectedEntityItem[];
    public OpportunitiesItemsSource: QuoteConnectedEntityItem[];
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.myDomainService = new QuoteDomainService();
        
        this.Listen();
        this.LoadData();
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            
        }
    }

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = SessionLocator.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "LoadConnectedShipments") {
                    this.LoadData();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadData();                    
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadData();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTCE") {
                    this.LoadData();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    LoadData() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetQuoteConnectedEntities(this.EntityPM.Id, this.EntityPM.OpportunityId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list: QuoteConnectedEntity[] = myResponse.Result;
                    this.FillItemSources(list);
                }
            }
            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }

    public IsShipmentGridVisible: boolean = false;
    public IsTicketsGridVisible: boolean = false;
    public IsOpportunitiesGridVisible: boolean = false;

    public ShipmentsGridHeight: number = 90;
    public TicketsGridHeight: number = 90;
    public OpportunitiesGridHeight: number = 90;

    private FillItemSources(list: QuoteConnectedEntity[]) {
        this.ItemsSource = [];
        this.ShipmentsItemsSource = [];
        this.TicketsItemsSource = [];

        list.forEach(item => {
            this.ItemsSource.push(new QuoteConnectedEntityItem(item, this));
        });

        this.ShipmentsItemsSource = this.ItemsSource.filter(d => d.ObjectTable == "Shipment");
        this.TicketsItemsSource = this.ItemsSource.filter(d => d.ObjectTable == "Ticket");
        this.OpportunitiesItemsSource = this.ItemsSource.filter(d => d.ObjectTable == "Opportunity");
        
        this.IsShipmentGridVisible = this.ShipmentsItemsSource.length == 0 ? false : true;
        this.IsTicketsGridVisible = this.TicketsItemsSource.length == 0 ? false : true;
        this.IsOpportunitiesGridVisible = this.OpportunitiesItemsSource.length == 0 ? false : true; 

        this.ShipmentsGridHeight = this.ComputeGridHeight(this.ShipmentsItemsSource);
        this.TicketsGridHeight = this.ComputeGridHeight(this.TicketsItemsSource);
        this.OpportunitiesGridHeight = this.ComputeGridHeight(this.OpportunitiesItemsSource); 
    }

    private ComputeGridHeight(list: any[]): number {
        var height: number = 90;

        if (list.length == 0 || list.length == 1) {
            height = 90;
        }

        else if (list.length == 2) {
            height = 110;
        }

        else if (list.length == 3) {
            height = 130;
        }

        else {
            height = 250;
        }

        return height;    
    }
}

class QuoteConnectedEntityItem {
    private myEntity: QuoteConnectedEntity = new QuoteConnectedEntity();
    constructor(entity: QuoteConnectedEntity, public fatherComponent: ConnectionsTabComponent) {
        this.myEntity = entity;
    }

    get EntityId() { return this.myEntity.EntityId; }
    get ShipmentType() { return this.myEntity.ShipmentType; }
    get EntityNumber() { return this.myEntity.EntityNumber; }
    get ObjectTable() { return this.myEntity.ObjectTable; }
    get EntityStatus() { return this.myEntity.EntityStatus; }
    get OpenDate() { return this.myEntity.OpenDate; }
    get Master() { return this.myEntity.Master; }
    get House() { return this.myEntity.House; }
    get Customer() { return this.myEntity.Customer; }
    get From() { return this.myEntity.From; }
    get To() { return this.myEntity.To; }
    get GrossWeight() { return this.myEntity.GrossWeight; }
    get VolumeInKG() { return this.myEntity.VolumeInKG; }
    get EntityOwner() { return this.myEntity.EntityOwner; }
    get EntityClosingDate() { return this.myEntity.EntityClosingDate; }

    ViewEntitytClicked() {        
        if (!AppTool.IsNullOrEmpty(this.ObjectTable)) {
            var backLabel = "Quote: " + this.fatherComponent.EntityPM.QuoteNumber;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityId, ObjectTableName: this.ObjectTable, BackButtonLabel: backLabel });

                    let isEditComponentSaved = false;
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.fatherComponent.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }
    }
}
