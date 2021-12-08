import { Component, OnInit, OnDestroy, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { InvoiceTool } from '../../../../Invoice/Tools';

@Component({
    
    templateUrl: './ARInvoiceGeneralTabComponent.html',
})
export class ARInvoiceGeneralTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ARInvoicePM;
    public ObjectTableName: string = "ARInvoice";
    public LabelColumnWidth: number = 100;
    public ControlColumnWidth: number = 200;
    public DataContext: ARInvoiceGeneralTabComponent = this;
    private ScreenCode: string = "ARInvoice.GeneralTabScreen";
    public DisplaySATSettings: boolean = false;
    public DisplayQBOSettings: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
        }

        if (this.IsQBOAccountingSystem() && SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "QBT")[0]) {
            this.DisplayQBOSettings = true;
        }

        this.EntityPM = entityArgs.EntityPM;
        this.RunComponent();
        this.SetUIProperties();
        this.Listen();
    }

    IsQBOAccountingSystem() {
        var isQBOAccountingSystem = false;
        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            isQBOAccountingSystem = true;
        }

        return isQBOAccountingSystem;
    }

    ngOnInit() {
       
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

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
            });
    }

    // Properties 
    get SATPaymentMethodCode() { return this.EntityPM.SATPaymentMethodCode; }
    set SATPaymentMethodCode(newValue: string) {
        if (this.EntityPM.SATPaymentMethodCode != newValue) {
            this.EntityPM.SATPaymentMethodCode = newValue;
        }
    }

    get RelatedInvoice() { return this.EntityPM.RelatedInvoice; }
    set RelatedInvoice(newValue: string) {
        if (this.EntityPM.RelatedInvoice != newValue) {
            this.EntityPM.RelatedInvoice = newValue;
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

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    get GlobalTaxCalculation() { return this.EntityPM.GlobalTaxCalculation; }
    set GlobalTaxCalculation(newValue: string) {
        if (this.EntityPM.GlobalTaxCalculation != newValue) {
            this.EntityPM.GlobalTaxCalculation = newValue;
        }
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("GlobalTaxCalculation", this.ObjectTableName, false);
        var isAllowedEdit = InvoiceTool.IsEditingARInvoiceEnabled(this.EntityPM);
        if (isAllowedEdit && this.EntityPM.TransferStatusCode != "TR") {
            this.UIProperties.SetEnabled("GlobalTaxCalculation", this.ObjectTableName, true);
        }
    }
}
