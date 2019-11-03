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
import { AirlineAreaPM } from '../../../../Common/EntityPMs/AirlineAreaPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';

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

    private LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllArilineAreasByAirlineId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BuildItemsSource(myResponse.Result);
            }
            
            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource(items: AirlineAreaPM[]) {
        this.ItemsSource = [];

        if (items == null) {
            items = [];
        }

        items.forEach(item => {
            this.ItemsSource.push(new AreaItemClass(item, this, false));
        });

        //this.SetIsNoDataVisible();
    }

    AddAirlineAreaClicked() {
        var item = new AirlineAreaPM();
        item.Tenant = this.EntityPM.Tenant;
        item.AirlineId = this.EntityPM.Id;
        item.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        item.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        item.CreatedByUserId = SessionInfo.LoggedUserId;
        item.UpdatedByUserId = SessionInfo.LoggedUserId;

        var itemComponent = new AreaItemClass(item, this, true);
        this.RunWindow(itemComponent, "New Area");        
    }

    EditAirlineAreaClicked(editedEntity: AreaItemClass) {
        this.RunWindow(editedEntity, "Edit Area");
    }

    DeleteAirlineAreaClicked(EditedEntity: AirlineAreaPM) {
        if (EditedEntity) {
            var window: ConfirmWindow = new ConfirmWindow();
            window.Show("Are you sure you want to delete this area?");
            window.WindowClosed.subscribe((event: any) => {
                if (window.Yes) {
                    //this.EntityPM.RemoveAirlineAreaPM(EditedEntity);
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
        logWindow.Show("./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAreaComponent");
    }

}

export class AreaItemClass {
    public ObjectTableName = "AirlineArea";
    public EntityPM: AirlineAreaPM;
    public IsNewEntity: boolean = false;

    constructor(item: AirlineAreaPM, public fatherComponent: AreasTabComponent, isNewEntity: boolean) {
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;
    }

    get Id() { return this.EntityPM.Id; }
    get Name() { return this.EntityPM.Name; }
    get Description() { return this.EntityPM.Description; }
    get CreatedByUserName() { return this.EntityPM.CreatedByUserName; }
    get CreateDate() { return this.EntityPM.CreateDate; }
    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    get UpdateDate() { return this.EntityPM.UpdateDate; }    
}
