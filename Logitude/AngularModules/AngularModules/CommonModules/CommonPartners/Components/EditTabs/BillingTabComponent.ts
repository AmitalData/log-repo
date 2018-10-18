import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    moduleId: module.id,
    templateUrl: './BillingTabComponent.html',
})

export class BillingTabComponent extends BaseComponent {
    private ScreenCode: string;
    public DisplaySATSettings: boolean = false;
    public EntityPM: any;
    public ObjectTableName: string;
    public DataContext = this;

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.ScreenCode = entityArgs.ObjectTableName + ".BillingTabScreen";
        this.RunComponent();

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
        }
    }

    RunComponent() {
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.viewContainerRef) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
                });
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

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
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

    get SATForeignRFC() { return this.EntityPM.SATForeignRFC; }
    set SATForeignRFC(newValue: string) {
        if (this.EntityPM.SATForeignRFC != newValue) {
            this.EntityPM.SATForeignRFC = newValue;
        }
    }
}