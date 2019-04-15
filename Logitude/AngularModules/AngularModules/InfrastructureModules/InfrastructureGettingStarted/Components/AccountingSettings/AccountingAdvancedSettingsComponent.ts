import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AccountingSettingPM} from '../../../../Common/EntityPMs/AccountingSettingPM';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'AccountingAdvancedSettingsComponent',
    moduleId: module.id,
    templateUrl: './AccountingAdvancedSettingsComponent.html',
})

export class AccountingAdvancedSettingsComponent extends BaseComponent {
    public EntityPM: AccountingSettingPM;
    public ObjectTableName: string = "AccountingSetting";
    public DataContext: any;
    public IsEnableMultiCurrencyARPaymentsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        if (FeatureLocator.HasFeaturePermession("ARPayment", "EnableMultiCurrency")) {
            this.IsEnableMultiCurrencyARPaymentsVisible = true;
        }
    }

    SetDataContext(dataContext: any) {
        this.DataContext = dataContext;
        this.EntityPM = this.DataContext.EntityPM;
        this.Clone();
    }

    //Commands 
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
        this.myCloner.AddField('EnableMultiPercentageVATTypes');
        this.myCloner.AddField('NotifyPastDateOnInvoiceEdit');
        this.myCloner.AddField('RegistryDateTypeCode');
        this.myCloner.AddField('EnableMultiCurrencyARPayments');
        this.myCloner.AddField('EnableNegativeOffsetARPayments');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

