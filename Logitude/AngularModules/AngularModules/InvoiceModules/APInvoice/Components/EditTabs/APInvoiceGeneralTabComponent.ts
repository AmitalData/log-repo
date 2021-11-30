import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { APInvoicePM } from '../../../../Invoice/EntityPMs/APInvoicePM';

@Component({    
    templateUrl: './APInvoiceGeneralTabComponent.html',
})
export class APInvoiceGeneralTabComponent extends BaseComponent implements OnInit {
    public EntityPM: APInvoicePM;
    public ObjectTableName: string = "APInvoice";
    public LabelColumnWidth: number = 100;
    public ControlColumnWidth: number = 200;
    public DataContext: APInvoiceGeneralTabComponent = this;
    private ScreenCode: string = "APInvoice.GeneralTabScreen";
    public DisplayQBOSettings: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();
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

    get GlobalTaxCalculation() { return this.EntityPM.GlobalTaxCalculation; }
    set GlobalTaxCalculation(newValue: string) {
        if (this.EntityPM.GlobalTaxCalculation != newValue) {
            this.EntityPM.GlobalTaxCalculation = newValue;
        }
    }
}
