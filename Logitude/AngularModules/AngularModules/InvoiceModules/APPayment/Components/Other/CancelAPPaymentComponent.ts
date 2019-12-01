import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './CancelAPPaymentComponent.html',
})




export class CancelAPPaymentComponent extends BaseComponent {


    public EntityPM: APPaymentPM = null;

   
    public DataContext = this;
    public ObjectTableName: string = "APPayment";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    IsVisibile: boolean;
    PaymentDate: Date;
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.PaymentDate = args.PaymentDate;
            this.AccountingCancelationDate = args.PaymentDate;
           
        }
    }



    constructor() {
        super();
        this._entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe(response => {
            this.IsVisibile = true;
            this.SetUIProperties();
        });


    }

    SetUIProperties() {
       

    }
   
    accountingCancelationDate: Date;
    get AccountingCancelationDate() { return this.accountingCancelationDate; }
    set AccountingCancelationDate(value: Date) {
        if (this.accountingCancelationDate != value) {
            this.accountingCancelationDate = value;
            var datetocompare = DateTool.GetDateParts(this.accountingCancelationDate).DateTicks;
            var dateFromcompare = DateTool.GetDateParts(this.PaymentDate).DateTicks;
            if (value == null) {
                this.UIProperties.SetRequired("AccountingCancelationDate", this.ObjectTableName, true);

            }
            if (this.accountingCancelationDate != null && datetocompare > dateFromcompare) {
                this.UIProperties.SetValidity("AccountingCancelationDate", this.ObjectTableName, false, "Cancellation date ");
            }

        }
    }
    cancelationNotes: string;
    get CancelationNotes() { return this.cancelationNotes; }
    set CancelationNotes(value: string) {
        if (this.cancelationNotes != value) {
            this.cancelationNotes = value;
            if (value == null) {
                this.UIProperties.SetRequired("CancelationNotes", this.ObjectTableName, true);

            }
        }
    }
    dontIncludeInDeductionReport: boolean;
    get DontIncludeInDeductionReport() { return this.dontIncludeInDeductionReport; }
    set DontIncludeInDeductionReport(value: boolean) {
        if (this.dontIncludeInDeductionReport != value) {
            this.dontIncludeInDeductionReport = value;
        }
    }
    FIELD_IS_REQUIERD: string;
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.AccountingCancelationDate)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.AccountingCancelationDate")));
          
        }
        if (AppTool.IsNullOrEmpty(this.CancelationNotes)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.CancelationNotes")));

        }
        var datetocompare = DateTool.GetDateParts(this.AccountingCancelationDate).DateTicks;
        var dateFromcompare = DateTool.GetDateParts(this.PaymentDate).DateTicks;
        if (datetocompare > dateFromcompare) {
            this.ValidationErrorsList.push("xxxx");
        }

        if (this.ValidationErrorsList.length == 0) {

            this.CurrentSession.CurrentWindow.Close("Ok");
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }
}
