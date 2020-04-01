import { Component, OnDestroy } from '@angular/core';
import { AirlinePM } from '../../../../../Common/EntityPMs/AirlinePM';
import { ShippingLinePM } from '../../../../../Common/EntityPMs/ShippingLinePM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { PartnersDomainService } from '../../../../../Common/Services/PartnersDomainService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';
import { TariffCarrierTranslationPM } from '../../../../../Common/EntityPMs/TariffCarrierTranslationPM';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { PortList } from '../../../../../Common/EntityLists/PortList';
import { SessionInfo } from '../../../../../Infrastructure/Utilities/SessionInfo';

@Component({
    moduleId: module.id,
    templateUrl: './TariffTranslationsTabComponent.html',
})

export class TariffTranslationsTabComponent implements OnDestroy {
    public ItemsSource: TranslationItemClass[];
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTableName: string;
    public TransportModeCode: string = null;
    public ResourcesReady: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DomainService: PartnersDomainService;

    constructor(public entityArgs: EntityArgs) {
        this._entityResourceService.getEntityResourceByTableName("TariffCarrierTranslation", 0).subscribe(response1 => {
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

    public AllTranslations: TariffCarrierTranslationPM[] = [];
    public LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetAllTariffTranslationsByCarrierId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllTranslations = myResponse.Result;
                this.BuildItemsSource();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource() {
        this.ItemsSource = [];

        this.AllTranslations.forEach(item => {
            this.ItemsSource.push(new TranslationItemClass(item, this, false));
        });
    }

    AddTranslationClicked() {
        var item = new TariffCarrierTranslationPM();
        item.Tenant = this.EntityPM.Tenant;
        item.CarrierId = this.EntityPM.Id;
        item.CreateDate = DateTool.GetCurrentDateAsUtc();
        item.UpdateDate = DateTool.GetCurrentDateAsUtc();
        item.CreatedByUserId = SessionInfo.LoggedUserId;
        item.UpdatedByUserId = SessionInfo.LoggedUserId;

        var itemComponent = new TranslationItemClass(item, this, true);
        this.RunWindow(itemComponent, "New Tariff Translation");
    }

    EditTranslationClicked(itemComponent: TranslationItemClass) {
        this.RunWindow(itemComponent, "Edit Tariff Translation");
    }

    DeleteTranslationClicked(itemComponent: TariffCarrierTranslationPM) {
        if (itemComponent) {
            var window: ConfirmWindow = new ConfirmWindow();
            window.Show("Are you sure you want to delete this translation?");
            window.WindowClosed.subscribe((event: any) => {
                if (window.Yes) {
                    this.DomainService.RemoveTranslationFromCarrier(itemComponent.Id).subscribe((myResult: any) => {
                        this.LoadData();
                    });
                }
            });
        }
    }

    private RunWindow(itemComponent: TranslationItemClass, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        //logWindow.Width = 500;
        //logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.DataContext = itemComponent;
        logWindow.Show("./CommonModules/CommonPartners/Components/EditTabs/TariffTranslations/AddEditTariffTranslationComponent");
    }
}

export class TranslationItemClass extends BaseComponent {
    public ObjectTableName = "TariffCarrierTranslation";
    public EntityPM: TariffCarrierTranslationPM;
    public IsNewEntity: boolean = false;
    constructor(item: TariffCarrierTranslationPM, public fatherComponent: TariffTranslationsTabComponent, isNewEntity: boolean) {
        super();
        this.EntityPM = item;
        this.IsNewEntity = isNewEntity;

        this.SetUIProperties();
    }

    public SetUIProperties() {
        
    }
    
    public get PartnerCode() { return this.EntityPM.PartnerCode; }
    public set PartnerCode(value: string) {
        if (this.EntityPM.PartnerCode != value) {
            this.EntityPM.PartnerCode = value;
        }
    }

    public get PortId() { return this.EntityPM.PortId; }
    public set PortId(value: string) {
        if (this.EntityPM.PortId != value) {
            this.EntityPM.PortId = value;
        }
    }
    
    public get PortCode() { return this.EntityPM.PortCode; }
    public set PortCode(value: string) {
        if (this.EntityPM.PortCode != value) {
            this.EntityPM.PortCode = value;
        }
    }

    public get PortName() { return this.EntityPM.PortName; }
    public set PortName(value: string) {
        if (this.EntityPM.PortName != value) {
            this.EntityPM.PortName = value;
        }
    } 

    private port: PortList;
    get Port() { return this.port; }
    set Port(value: PortList) {
        if (this.port != value) {
            this.port = value;
        }

        if (!AppTool.IsNullOrEmpty(value)) {
            this.PortCode = value.Code;
            this.PortName = value.CombinedCode;
        }

        else {
            this.PortCode = null;
            this.PortName = null;
        }
    }

    get Id() { return this.EntityPM.Id; }
    get CreatedByUserName() { return this.EntityPM.CreatedByUserName; }
    get CreateDate() { return this.EntityPM.CreateDate; }
    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    get UpdateDate() { return this.EntityPM.UpdateDate; }    
}
