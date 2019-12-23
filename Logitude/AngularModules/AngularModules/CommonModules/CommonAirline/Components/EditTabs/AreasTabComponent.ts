import { Component, OnDestroy} from '@angular/core';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { CarrierAreaPM } from '../../../../Common/EntityPMs/CarrierAreaPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { CarrierAreasPortPM } from '../../../../Common/EntityPMs/CarrierAreasPortPM';

@Component({
    moduleId: module.id,
    templateUrl: './AreasTabComponent.html',
})

export class AreasTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: AirlinePM;
    public ObjectTableName: string = "Airline";
    public TenantPM: TenantPM;
    public ResourcesReady: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();  
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: AreaItemClass[];
    public DomainService: PartnersDomainService;

    constructor(public entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("AirlineArea", 0).subscribe(response1 => {
            this._entityResourceService.getEntityResourceByTableName("AirlineAreasPort", 0).subscribe(response2 => {
                this.ResourcesReady = true;
                this.EntityPM = entityArgs.EntityPM;
                this.TenantPM = SessionLocator.TenantPM;
                this.DomainService = new PartnersDomainService();

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

    public LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllCarrierAreasByCarrierId(this.EntityPM.Id).subscribe((myResult: any) => {
            this.BuildItemsSource(myResult);
            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource(items: CarrierAreaPM[]) {
        this.ItemsSource = [];

        if (items == null) {
            items = [];
        }

        items.forEach(item => {
            this.ItemsSource.push(new AreaItemClass(item, this, false));
        });               
    }
    
    AddCarrierAreaClicked() {
        var item = new CarrierAreaPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CarrierId = this.EntityPM.Id;
        item.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        item.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        item.CreatedByUserId = SessionInfo.LoggedUserId;
        item.UpdatedByUserId = SessionInfo.LoggedUserId;
        item.CreatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
        item.UpdatedByUserName = SessionInfo.LoggedUserPM.EnglishName;       
        
        var itemComponent = new AreaItemClass(item, this, true);
        this.RunWindow(itemComponent, "New Area");        
    }

    EditCarrierAreaClicked(editedEntity: AreaItemClass) {
        this.RunWindow(editedEntity, "Edit Area");
    }

    DeleteCarrierAreaClicked(EditedEntity: CarrierAreaPM) {
        if (EditedEntity) {
            var window: ConfirmWindow = new ConfirmWindow();
            window.Show("Are you sure you want to delete this area?");
            window.WindowClosed.subscribe((event: any) => {
                if (window.Yes) {
                    this.DomainService.RemoveAreaFromCarrier(EditedEntity.Id).subscribe((myResult: any) => {
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
        logWindow.Show("./CommonModules/CommonAirline/Components/AddEdit/AddEditCarrierAreaComponent");
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
        logWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/ChoosePortComponent');
    }

    ChooseCountry() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;
        logWindow.DataContext = this;
        logWindow.Title = "Choose Country Ports";
        logWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/ChooseCountryPortComponent');
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
