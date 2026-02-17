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
    templateUrl: './EditPaymentChequeComponent.html',
})




export class EditPaymentChequeComponent extends BaseComponent{


    public EntityPM: PaymentChequePM = null;
 
    public LocalCurrencyId: string;
    public DataContext = this;
    public ObjectTableName: string = "PaymentCheque";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    IsVisibile: boolean;
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.PayToGLAccountId = args.PayToGLAccountId;
            this.CurrencyId = args.CurrencyId;
            this.LocalAmount = args.LocalAmount;
            this.ForeignAmount = args.ForeignAmount;
            this.ValueDate = args.ValueDate;
            this.BankAccountId = args.BankAccountId;
          
        }
    }

    

    constructor() {
        super();
        this._entityResourceService.getEntityResourceByTableName("PaymentCheque", 0).subscribe(response => {
            this.IsVisibile = true;
            this.SetUIProperties();
        });
       
    
    }
    
    SetUIProperties() {
        this.UIProperties.SetEnabled("PayToGLAccountId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("LocalAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, false);

    }
    PayToGLAccountId: string;
    CurrencyId: string;
    LocalAmount: number;
    ForeignAmount: number;
    ValueDate: Date;
    BankAccountId: string;
    payToName: string;
    get PayToName() { return this.payToName; }
    set PayToName(value: string) {
        if (this.payToName != value) {
            this.payToName = value;
        }
    }
    notes: string;
    get Notes() { return this.notes; }
    set Notes(value: string) {
        if (this.notes != value) {
            this.notes = value;
        }
    }
    
    FIELD_IS_REQUIERD: string;
    OkButtonClicked() {
        this.ValidationErrorsList= [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty( this.PayToName)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("PaymentCheque.F.PayToName")));
        }
     
        else {
            this.CurrentSession.CurrentWindow.Close(this.PayToName + ',' + this.Notes);
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }
}
