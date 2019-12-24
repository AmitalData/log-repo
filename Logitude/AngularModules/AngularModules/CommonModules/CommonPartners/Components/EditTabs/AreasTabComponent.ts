import { Component, OnDestroy } from '@angular/core';
import { AirlinePM } from '../../../../Common/EntityPMs/AirlinePM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { CarrierAreaPM } from '../../../../Common/EntityPMs/CarrierAreaPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { CarrierAreasPortPM } from '../../../../Common/EntityPMs/CarrierAreasPortPM';
import { ShippingLinePM } from '../../../../Common/EntityPMs/ShippingLinePM';

@Component({
    moduleId: module.id,
    templateUrl: './AreasTabComponent.html',
})

export class AreasTabComponent implements OnDestroy {
    public ItemsSource: AreaItemClass[];
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTableName: string;
    public TransportModeCode: string = null;
    public ResourcesReady: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;    
    public DomainService: PartnersDomainService;

    constructor(public entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("CarrierArea", 0).subscribe(response1 => {
            this._entityResourceService.getEntityResourceByTableName("CarrierAreasPort", 0).subscribe(response2 => {
                this.ResourcesReady = true;
                this.EntityPM = entityArgs.EntityPM;
                this.EntityId = entityArgs.EntityPM.Id;
                this.ObjectTableName = entityArgs.ObjectTableName;

                if (this.EntityPM instanceof AirlinePM) {
                    this.TransportModeCode = "A";
                }
                else if (this.EntityPM instanceof ShippingLinePM) {
                    this.TransportModeCode = "O";
                }
                
                if (this.DomainService == null) {
                    this.DomainService = new PartnersDomainService();
                }

                this.Listen();
                this.LoadData();
            });
        });
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
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
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public AllAreas: CarrierAreaPM[] = [];
    public LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllCarrierAreasByCarrierId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllAreas = myResponse.Result;
                this.BuildItemsSource();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource() {
        this.ItemsSource = [];
        
        this.AllAreas.forEach(item => {
            this.ItemsSource.push(new AreaItemClass(item, this, false));
        });
    }

    AddCarrierAreaClicked() {
        var item = new CarrierAreaPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CarrierId = this.EntityPM.Id;        
        item.TransportModeCode = this.TransportModeCode;

        var itemComponent = new AreaItemClass(item, this, true);
        this.RunWindow(itemComponent, "New Area");
    }

    EditCarrierAreaClicked(editedEntity: AreaItemClass) {
        this.RunWindow(editedEntity, "Edit Area");
    }

    DeleteCarrierAreaClicked(area: CarrierAreaPM) {
        if (area) {
            var window: ConfirmWindow = new ConfirmWindow();
            window.Show("Are you sure you want to delete this area?");
            window.WindowClosed.subscribe((event: any) => {
                if (window.Yes) {
                    this.DomainService.RemoveAreaFromCarrier(area.Id).subscribe((myResult: any) => {
                        this.LoadData();
                    });
                }
            });
        }
    }

    private RunWindow(itemComponent: AreaItemClass, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 650;
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemComponent;
        logWindow.Show("./CommonModules/CommonPartners/Components/AddEdit/AddEditAreaComponent");
    }
}

export class AreaItemClass extends BaseComponent {
    public ObjectTableName = "CarrierArea";
    public EntityPM: CarrierAreaPM;
    public IsNewEntity: boolean = false;
    public PortItemsList: CarrierAreasPortPM[];
    constructor(item: CarrierAreaPM, public fatherComponent: AreasTabComponent, isNewEntity: boolean) {
        super();
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;

        this.SetUIProperties();
        this.BuildPortItemsList();
    }

    public SetUIProperties() {
        this.UIProperties.SetRequired("Name", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Name));
    }

    public BuildPortItemsList() {
        this.PortItemsList = [];
        this.PortItemsList = this.EntityPM.CarrierAreasPorts;
        this.PortItemsList = this.PortItemsList.sort((a, b) => { return (a.CountryCode === b.CountryCode) ? 0 : (a.CountryCode < b.CountryCode) ? -1 : 1 });
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;

            this.SetUIProperties();
        }
    }

    public get Description() { return this.EntityPM.Description; }
    public set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get Id() { return this.EntityPM.Id; }
    get CreatedByUserName() { return this.EntityPM.CreatedByUserName; }
    get CreateDate() { return this.EntityPM.CreateDate; }
    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    get UpdateDate() { return this.EntityPM.UpdateDate; }
    
    ChoosePort() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;
        logWindow.DataContext = this;
        logWindow.Title = "Choose Ports";
        logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/ChoosePortComponent');
    }

    ChooseCountry() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;
        logWindow.DataContext = this;
        logWindow.Title = "Choose Country Ports";
        logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/ChooseCountryPortComponent');
    }

    DeletePort(item: CarrierAreasPortPM) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this item ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.EntityPM.CarrierAreasPorts.indexOf(item) != -1) {
                    this.EntityPM.RemoveCarrierAreasPortPM(item);
                    this.BuildPortItemsList();
                }
            }
        });
    }
}
