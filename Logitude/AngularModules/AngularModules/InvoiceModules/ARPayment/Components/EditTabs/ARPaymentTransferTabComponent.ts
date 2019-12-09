import {Component, ViewChild, ViewContainerRef, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARPaymentPM} from '../../../../Invoice/EntityPMs/ARPaymentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARPaymentTransferTemplate} from '../NewEntity/ARPaymentTransferTemplate';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './ARPaymentTransferTabComponent.html',
})

export class ARPaymentTransferTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: ARPaymentPM = null;
    public ObjectTableName = "ARPayment";
    public DataContext = this;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;  
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.RunComponent();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.InputTemplate) {
                        this.InputTemplate.EntityPM = this.EntityPM
                        //this.InputTemplate.BuildList();
                    }
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.InputTemplate) {
                        this.InputTemplate.EntityPM = this.EntityPM
                        // this.InputTemplate.BuildList();
                    }
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private InputTemplate: ARPaymentTransferTemplate;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./InvoiceModules/ARPayment/Components/NewEntity/ARPaymentTransferTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.InputTemplate = cmpRef.instance;
                this.InputTemplate.InitTemplate(this.EntityPM );
            });
    }
}