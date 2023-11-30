import {Component, OnInit, ViewChild, ViewContainerRef, OnDestroy} from '@angular/core';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './AgentBillingTabComponent.html',
})

export class AgentBillingTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: AgentPM;
    public ObjectTableName: string = "Agent";
    public DataContext = this;
    public HasCreditLimitFeature: boolean = false;
    public IsCreditLimitActivated: boolean = false;
    public DisplaySATSettings: boolean = false;
    public Profact4Enabled: boolean = false;

    @ViewChild('BillingChild', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    @ViewChild('ARInvoiceDocumentTypeTemplateArea', { read: ViewContainerRef, static: false }) documentTemplateViewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;

        this.HasCreditLimitFeature = FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");

        if (this.HasCreditLimitFeature) {
            this.IsCreditLimitActivated = ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
            this.Profact4Enabled = SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40";
        }

    }

    ngOnInit() {
        this.SetUIProperties();
        this.LoadGeneratedComponents();
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
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, "Agent.BillingTabScreen");
                if (viewContainerRef == this.viewContainerRef) this.LoadARInvoiceDocumentTypeTemplateComponent();
            });
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
                cmpRef.instance.Run(this.EntityPM);
            });
        this.IsARInvoiceDocumentTypeTemplateAreaLoaded = true;
    }

    SetUIProperties() {
        var isFieldActivated: boolean = false;

        if (this.HasCreditLimitFeature) {
            if (this.IsCreditLimitActivated) {
                if (this.IsCreditLimitEnabled) {
                    isFieldActivated = true;
                }
            }
        }

        this.UIProperties.SetEnabled("BlockNewInvoiceCreation", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("BlockNewShipmentCreation", this.ObjectTableName, isFieldActivated);
    }

    get IsCreditLimitEnabled() { return this.EntityPM.IsCreditLimitEnabled; }
    set IsCreditLimitEnabled(value: boolean) {
        if (this.EntityPM.IsCreditLimitEnabled != value) {
            this.EntityPM.IsCreditLimitEnabled = value;
            this.SetUIProperties();
        }
    }

    get BlockNewInvoiceCreation() { return this.EntityPM.BlockNewInvoiceCreation; }
    set BlockNewInvoiceCreation(value: boolean) {
        if (this.EntityPM.BlockNewInvoiceCreation != value) {
            this.EntityPM.BlockNewInvoiceCreation = value;
        }
    }

    get BlockNewShipmentCreation() { return this.EntityPM.BlockNewShipmentCreation; }
    set BlockNewShipmentCreation(value: boolean) {
        if (this.EntityPM.BlockNewShipmentCreation != value) {
            this.EntityPM.BlockNewShipmentCreation = value;
        }
    }

    get PaymentMethodCode() { return this.EntityPM.PaymentMethodCode; }
    set PaymentMethodCode(newValue: string) {
        if (this.EntityPM.PaymentMethodCode != newValue) {
            this.EntityPM.PaymentMethodCode = newValue;
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
