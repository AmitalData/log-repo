import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './APPaymentGeneralTabComponent.html',
})
export class APPaymentGeneralTabComponent extends BaseComponent implements OnInit {
    public EntityPM: APPaymentPM;
    public ObjectTableName: string = "APPayment";
    public LabelColumnWidth: number = 100;
    public ControlColumnWidth: number = 200;
    public DataContext: APPaymentGeneralTabComponent = this;
    private ScreenCode: string = "APPayment.GeneralTabScreen";
    public DisplaySATSettings: boolean = false;
    public DisplayFechaPago: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();
    }

    ngOnInit() {

    }

    SetUIProperties() {
           this.UIProperties.SetEnabled("VendorBankAddress", "APPayment", false);
           this.UIProperties.SetEnabled("VendorBankName", "APPayment", false);
           this.UIProperties.SetEnabled("VendorBankAccountNumber", "APPayment", false);
           this.UIProperties.SetEnabled("VendorSwift", "APPayment", false);
           this.UIProperties.SetEnabled("VendorIBANNumber", "APPayment", false);
    }

    get VendorBankAddress() {
        if (this.EntityPM != null) {
            return this.EntityPM.VendorBankAddress;
        }
        else
            return null;
    }
    set VendorBankAddress(newValue: string) {
        if (this.EntityPM.VendorBankAddress != newValue) {
            this.EntityPM.VendorBankAddress = newValue;
        }
    }

    get VendorBankName() {
        if (this.EntityPM != null) {
            return this.EntityPM.VendorBankName;
        }
        else
            return null;
    }
    set VendorBankName(newValue: string) {
        if (this.EntityPM.VendorBankName != newValue) {
            this.EntityPM.VendorBankName = newValue;
        }
    }
    get VendorBankAccountNumber() {
        if (this.EntityPM != null) {
            return this.EntityPM.VendorBankAccountNumber;
        }
        else
            return null;
    }
    set VendorBankAccountNumber(newValue: string) {
        if (this.EntityPM.VendorBankAccountNumber != newValue) {
            this.EntityPM.VendorBankAccountNumber = newValue;
        }
    }

    get VendorSwift() {
        if (this.EntityPM != null) {
            return this.EntityPM.VendorSwift;
        }
        else
            return null;
    }
    set VendorSwift(newValue: string) {
        if (this.EntityPM.VendorSwift != newValue) {
            this.EntityPM.VendorSwift = newValue;

        }
    }

    get VendorIBANNumber() {
        if (this.EntityPM != null) {
            return this.EntityPM.VendorIBANNumber;
        }
        else
            return null;
    }
    set VendorIBANNumber(newValue: string) {
        if (this.EntityPM.VendorIBANNumber != newValue) {
            this.EntityPM.VendorIBANNumber = newValue;

        }
    }
}
