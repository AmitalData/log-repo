import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { AccountingSettingPM } from '../../../../Common/EntityPMs/AccountingSettingPM';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    
    templateUrl: './AccountingAdvancedAPSettingsComponent.html',
})

export class AccountingAdvancedAPSettingsComponent extends BaseComponent {
    public EntityPM: AccountingSettingPM;
    public ObjectTableName: string = "AccountingSetting";
    public DataContext: any;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsAPPaymentExternalPaymentVisible: boolean = false;
    constructor() {
        super();

        if (FeatureLocator.HasFeaturePermession("General", "APPaymentExternalPayment")) {
            this.IsAPPaymentExternalPaymentVisible = true;
        }
    }

    SetDataContext(dataContext: any) {
        this.DataContext = dataContext;
        this.EntityPM = this.DataContext.EntityPM;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('EnableAPPaymentExternalPayment');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
