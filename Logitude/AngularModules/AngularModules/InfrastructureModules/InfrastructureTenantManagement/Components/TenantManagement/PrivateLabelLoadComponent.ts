import {Component, AfterViewInit, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagmentPrivateLabelsPM} from '../../../../Infrastructure/EntityPMs/TenantManagmentPrivateLabelsPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TenantManagmentPrivateLabelsPMService} from '../../../../Infrastructure/Services/StandardPMs/TenantManagmentPrivateLabelsPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './PrivateLabelLoadComponent.html',
})

export class PrivateLabelLoadComponent implements AfterViewInit {
    public EntityId: string = null;
    public EntityPM: TenantManagmentPrivateLabelsPM;
    @ViewChild('WizardView', { read: ViewContainerRef }) target: ViewContainerRef;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetWindowArgs(entityId: string) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityId = entityId;
        this.Load();
    }

    private isViewInited = false;
    ngAfterViewInit() {
        this.isViewInited = true;
        this.Load();
    }

    private Load() {
        if (this.EntityId != null && this.isViewInited) {

            var myService: TenantManagmentPrivateLabelsPMService = new TenantManagmentPrivateLabelsPMService();

            myService.get(this.EntityId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.ImportWizard();
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }

    }

    private ImportWizard() {
        var Args = { Entity: this.EntityPM };
        this._entityResourceService.getEntityResourceByTableName("TenantManagmentPrivateLabels", 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditPrivateLabelsComponent', this.target)
                .then(cmpRef => {
                    cmpRef.instance.SetWindowArgs(Args);
                    this.CurrentSession.StopBusyIndicator();
                });
        });
    }
    
}
