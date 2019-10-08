import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APPaymentInvoiceArgs } from './APPaymentDetailsTabComponent';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { PaymentChequePM } from '../../../../Accounting/EntityPMs/PaymentChequePM';


@Component({
    moduleId: module.id,
    templateUrl: './PaymentChequeDetailsComponent.html',
})




export class PaymentChequeDetailsComponent extends BaseComponent{


    public EntityPM: PaymentChequePM = null;
    public LocalCurrencyId: string;
    public DataContext = this;
    public ObjectTableName: string = "PaymentCheque";
    public ValidationErrorsList: string[] = [];


    constructor() {
        super();
        this.SetUIProperties();
    }
    
    SetUIProperties() {
        this.UIProperties.SetEnabled("PayToGLAccountId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("LocalAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, false);

    }
}
