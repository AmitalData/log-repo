import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';

@Component({
    
    templateUrl: './ARInvoiceGeneralTabComponent.html',
})
export class ARInvoiceGeneralTabComponent extends BaseComponent implements OnInit {
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

    get GlobalTaxCalculation() { return this.EntityPM.GlobalTaxCalculation; }
    set GlobalTaxCalculation(newValue: string) {
        if (this.EntityPM.GlobalTaxCalculation != newValue) {
            this.EntityPM.GlobalTaxCalculation = newValue;
        }
    }
}
