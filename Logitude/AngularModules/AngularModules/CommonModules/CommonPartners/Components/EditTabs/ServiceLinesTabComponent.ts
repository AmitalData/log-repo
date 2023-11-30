import { Component, OnDestroy, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ShippingLinePM } from '../../../../Common/EntityPMs/ShippingLinePM';
import { CarrierServiceLinePM } from '../../../../Common/EntityPMs/CarrierServiceLinePM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    templateUrl: './ServiceLinesTabComponent.html',
})

export class ServiceLinesTabComponent implements OnDestroy {
    public ItemsSource: ServiceLineItem[];
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTableName: string;
    public PartnerTypeId: string = null;
    public ResourcesReady: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DomainService: PartnersDomainService;

    constructor(public entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("CarrierServiceLine", 0).subscribe(response1 => {
            this.ResourcesReady = true;
            this.EntityPM = entityArgs.EntityPM;
            this.EntityId = entityArgs.EntityPM.Id;
            this.ObjectTableName = entityArgs.ObjectTableName;

            if (this.EntityPM instanceof ShippingLinePM) {
                this.PartnerTypeId = "SL";
            }

            if (this.DomainService == null) {
                this.DomainService = new PartnersDomainService();
            }

            this.Listen();
            this.LoadData();
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

    public AllServiceLines: CarrierServiceLinePM[] = [];
    public LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllCarrierServiceLinesByCarrierId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllServiceLines = myResponse.Result;
                this.BuildItemsSource();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource() {
        this.ItemsSource = [];

        this.AllServiceLines.forEach(item => {
            this.ItemsSource.push(new ServiceLineItem(item, this, false));
        });
    }

    AddServiceLine() {
        var item = new CarrierServiceLinePM();
        item.Tenant = this.EntityPM.Tenant;
        item.CardId = this.EntityPM.Id;
        item.PartnerTypeId = this.PartnerTypeId;

        var itemComponent = new ServiceLineItem(item, this, true);
        this.RunWindow(itemComponent, "New Service Line");
    }

    EditClicked(itemComponent: ServiceLineItem) {
        this.RunWindow(itemComponent, "Edit Service Line");
    }
    DeleteClicked(itemComponent: ServiceLineItem) {
        if (itemComponent) {
            var window: ConfirmWindow = new ConfirmWindow();
            window.Show("Are you sure you want to delete this service line?");
            window.WindowClosed.subscribe((event: any) => {
                if (window.Yes) {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.DomainService.RemoveServiceLineFromCarrier(itemComponent.Id).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            this.LoadData();
                        }

                        else {
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        this.CurrentSession.StopBusyIndicator();
                    });
                }
            });
        }
    }

    private RunWindow(itemComponent: ServiceLineItem, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemComponent;
        logWindow.Show("./CommonModules/CommonPartners/Components/AddEdit/AddEditServiceLineComponent");
    }
}

export class ServiceLineItem extends BaseComponent {
    public ObjectTableName = "CarrierServiceLine";
    public EntityPM: CarrierServiceLinePM;
    public IsNewEntity: boolean = false;
    constructor(entity: CarrierServiceLinePM, public FatherComponent: ServiceLinesTabComponent, isNew: boolean) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;

        this.SetUIProperties();
    }

    SetUIProperties() {
        if (this.IsNewEntity) {
            this.UIProperties.SetVisibility("Inactive", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetVisibility("Inactive", this.ObjectTableName, true);
        }
    }

    get Id() { return this.EntityPM.Id; }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get Name() { return this.EntityPM.Name }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }
}
