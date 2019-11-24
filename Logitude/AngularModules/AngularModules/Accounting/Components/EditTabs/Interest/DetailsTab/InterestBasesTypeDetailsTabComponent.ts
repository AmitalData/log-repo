import { Component, OnInit} from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { InterestBasesTypePM } from '../../../../EntityPMs/InterestBasesTypePM';

@Component({
    moduleId: module.id,
    templateUrl: './InterestBasesTypeDetailsTabComponent.html',
})
export class InterestBasesTypeDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: InterestBasesTypePM;
    public ObjectTableName: string = "InterestBasesType";
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
        //if (this.IsScreenEnabled) { 
        //    this.UIProperties.SetEnabled("VendorBankAddress", "APPayment", true);
        //    this.UIProperties.SetEnabled("VendorBankName", "APPayment", true);
        //    this.UIProperties.SetEnabled("VendorBankAccountNumber", "APPayment", true);
        //    this.UIProperties.SetEnabled("VendorSwift", "APPayment", true);
        //    this.UIProperties.SetEnabled("VendorIBANNumber", "APPayment", true);
        //}
        //else{
        //    this.UIProperties.SetEnabled("VendorBankAddress", "APPayment", false);
        //    this.UIProperties.SetEnabled("VendorBankName", "APPayment", false);
        //    this.UIProperties.SetEnabled("VendorBankAccountNumber", "APPayment", false);
        //    this.UIProperties.SetEnabled("VendorSwift", "APPayment", false);
        //    this.UIProperties.SetEnabled("VendorIBANNumber", "APPayment", false);
        //}
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


    get Description() {
        if (this.EntityPM != null) {
            return this.EntityPM.Description;
        }
        else
            return null;
    }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }
    get EnglishName() {
        if (this.EntityPM != null) {
            return this.EntityPM.EnglishName;
        }
        else
            return null;
    }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }
    get LocalName() {
        if (this.EntityPM != null) {
            return this.EntityPM.LocalName;
        }
        else
            return null;
    }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }
    get Code() {
        if (this.EntityPM != null) {
            return this.EntityPM.Code;
        }
        else
            return null;
    }
    set Code(newValue: string) {
        if (this.EntityPM.Code != newValue) {
            this.EntityPM.Code = newValue;
        }
    }
    get InActive() {
        if (this.EntityPM != null) {
            return this.EntityPM.InActive;
        }
        else
            return null;
    }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}
