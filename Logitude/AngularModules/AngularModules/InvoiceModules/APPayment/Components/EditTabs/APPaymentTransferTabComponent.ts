import {Component, ViewChild, ViewContainerRef, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APPaymentPM} from '../../../../Invoice/EntityPMs/APPaymentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {APPaymentTransferTemplate} from '../NewEntity/APPaymentTransferTemplate';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './APPaymentTransferTabComponent.html',
})

export class APPaymentTransferTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: APPaymentPM = null;
    public ObjectTableName = "APPayment";
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
                        this.InputTemplate.BuildList();
                    }
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.InputTemplate) {
                        this.InputTemplate.EntityPM = this.EntityPM
                        this.InputTemplate.BuildList();
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

    private InputTemplate: APPaymentTransferTemplate;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./InvoiceModules/APPayment/Components/NewEntity/APPaymentTransferTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.InputTemplate = cmpRef.instance;
                this.InputTemplate.InitTemplate(this.EntityPM);
            });
    }
}