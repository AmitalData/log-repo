import {Component, ViewChild, ViewContainerRef, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './BillingTabComponent.html',
})

export class BillingTabComponent extends BaseComponent implements OnDestroy {
    private ScreenCode: string;
    public DisplaySATSettings: boolean = false;
    public EntityPM: any;
    public ObjectTableName: string;
    public DataContext = this;
    public Profact4Enabled: boolean = false;

    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    @ViewChild('ARInvoiceDocumentTypeTemplateArea', { read: ViewContainerRef, static: false }) documentTemplateViewContainerRef: ViewContainerRef;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.ScreenCode = entityArgs.ObjectTableName + ".BillingTabScreen";
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.EntityPM = this.entityArgs.EntityPM;
        this.LoadGeneratedComponents();

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
            this.Profact4Enabled = SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40";
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null; 
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    LoadGeneratedComponents() {
        if (!this.viewContainerRef) {
            this.RunComponentTimer("Child");
            return;
        }
        this.LoadChildComponent(this.viewContainerRef);     
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer(componentName: String) {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = componentName == "ARInvoiceDocumentTypeTemplateComponent" ? setTimeout(() => this.LoadARInvoiceDocumentTypeTemplateComponent(), 1) : setTimeout(() => this.LoadGeneratedComponents(), 1);
        }
    }

    private LoadChildComponent(viewContainerRef) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
                if (viewContainerRef == this.viewContainerRef) this.LoadARInvoiceDocumentTypeTemplateComponent();
            });

        this.Listen();
    }

    IsARInvoiceDocumentTypeTemplateAreaLoaded: boolean = false;
    public LoadARInvoiceDocumentTypeTemplateComponent() {
        if (this.IsARInvoiceDocumentTypeTemplateAreaLoaded) return;
        this.Retries = 0;
        if (!this.documentTemplateViewContainerRef) {
            this.RunComponentTimer("ARInvoiceDocumentTypeTemplateComponent");
            return;
        }
        SessionLocator.DynamicLoader.Load('./CommonModules/CommonPartners/Components/Templates/PartnerARInvoiceDocumentTypeTemplateComponent', this.documentTemplateViewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.entityArgs.EntityPM);
            });
        this.IsARInvoiceDocumentTypeTemplateAreaLoaded = true;
    }

    get PaymentMethodCode() { return this.entityArgs.EntityPM.PaymentMethodCode; }
    set PaymentMethodCode(newValue: string) {
        if (this.entityArgs.EntityPM.PaymentMethodCode != newValue) {
            this.entityArgs.EntityPM.PaymentMethodCode = newValue;
        }
    }

    get MetodoPagoCode() { return this.EntityPM.MetodoPagoCode; }
    set MetodoPagoCode(newValue: string) {
        if (this.EntityPM.MetodoPagoCode != newValue) {
            this.EntityPM.MetodoPagoCode = newValue;
        }
    }

    get UsoCFDICode() { return this.EntityPM.UsoCFDICode; }
    set UsoCFDICode(newValue: string) {
        if (this.EntityPM.UsoCFDICode != newValue) {
            this.EntityPM.UsoCFDICode = newValue;
        }
    }

    get RegimenFiscalCode() { return this.EntityPM.RegimenFiscalCode; }
    set RegimenFiscalCode(newValue: string) {
        if (this.EntityPM.RegimenFiscalCode != newValue) {
            this.EntityPM.RegimenFiscalCode = newValue;
        }
    }

    get SATReceptorName() { return this.EntityPM.SATReceptorName; }
    set SATReceptorName(newValue: string) {
        if (this.EntityPM.SATReceptorName != newValue) {
            this.EntityPM.SATReceptorName = newValue;
        }
    }

    get SATForeignRFC() { return this.EntityPM.SATForeignRFC; }
    set SATForeignRFC(newValue: string) {
        if (this.EntityPM.SATForeignRFC != newValue) {
            this.EntityPM.SATForeignRFC = newValue;
        }
    }
}
