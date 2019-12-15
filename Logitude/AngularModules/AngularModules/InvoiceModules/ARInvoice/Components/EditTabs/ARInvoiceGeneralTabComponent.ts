import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceGeneralTabComponent.html',
})
export class ARInvoiceGeneralTabComponent extends BaseComponent implements OnInit {
    public EntityPM: ARInvoicePM;
    public ObjectTableName: string = "ARInvoice";
   // public TenantPM: TenantPM;
    public LabelColumnWidth: number = 100;
    public ControlColumnWidth: number = 200;
    public DataContext: ARInvoiceGeneralTabComponent = this;
    private ScreenCode: string = "ARInvoice.GeneralTabScreen";
    public DisplaySATSettings: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
        }
        this.EntityPM = entityArgs.EntityPM;
        //this.TenantPM = SessionLocator.TenantPM;
        this.RunComponent();
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
     
}