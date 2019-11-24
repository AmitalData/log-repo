import { Component, OnInit} from '@angular/core';
import { APPaymentGeneralTabComponent } from '../../../../../InvoiceModules/APPayment/Components/EditTabs/APPaymentGeneralTabComponent';
import { APPaymentPM } from '../../../../../Invoice/EntityPMs/APPaymentPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './InterestBasesTypeDetailsTabComponent.html',
})
export class InterestBasesTypeDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: APPaymentPM;
    public ObjectTableName: string = "APPayment";
    public DataContext: InterestBasesTypeDetailsTabComponent = this;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();
        this.Listen();
    }
    ngOnInit() {
    }
    SetUIProperties() {
        if (this.IsScreenEnabled) { 
            this.UIProperties.SetEnabled("VendorBankAddress", "APPayment", true);
            this.UIProperties.SetEnabled("VendorBankName", "APPayment", true);
            this.UIProperties.SetEnabled("VendorBankAccountNumber", "APPayment", true);
            this.UIProperties.SetEnabled("VendorSwift", "APPayment", true);
            this.UIProperties.SetEnabled("VendorIBANNumber", "APPayment", true);
        }
        else{
            this.UIProperties.SetEnabled("VendorBankAddress", "APPayment", false);
            this.UIProperties.SetEnabled("VendorBankName", "APPayment", false);
            this.UIProperties.SetEnabled("VendorBankAccountNumber", "APPayment", false);
            this.UIProperties.SetEnabled("VendorSwift", "APPayment", false);
            this.UIProperties.SetEnabled("VendorIBANNumber", "APPayment", false);
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private CurrentSession = SessionLocator.SelectedSession;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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

    get IsScreenEnabled() {
        var result = true;
        if (this.EntityPM != null) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                result = true;
            }
            else {
                result = false;
            }
        }
        return result;
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

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
