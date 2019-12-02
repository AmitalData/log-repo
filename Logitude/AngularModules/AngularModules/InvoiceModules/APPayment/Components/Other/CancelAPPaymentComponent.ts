import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AccountingPeriodListService } from '../../../../Accounting/Services/StandardLists/AccountingPeriodListService';
import { AccountingPeriodList } from '../../../../Accounting/EntityLists/AccountingPeriodList';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    moduleId: module.id,
    templateUrl: './CancelAPPaymentComponent.html',
})




export class CancelAPPaymentComponent extends BaseComponent {


    public EntityPM: APPaymentPM = null;
    AccountingPeriods: AccountingPeriodList[] = [];
    _AccountingPeriodListService: AccountingPeriodListService = new AccountingPeriodListService();
   
    public DataContext = this;
    public ObjectTableName: string = "APPayment";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    IsVisibile: boolean;
    PaymentDate: Date;
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.PaymentPM;
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
   
    get AccountingCancelationDate() { return this.EntityPM.AccountingCancelationDate; }
    set AccountingCancelationDate(value: Date) {
        if (this.EntityPM.AccountingCancelationDate != value) {
            this.EntityPM.AccountingCancelationDate = value;
            var cancelationDateParts = DateTool.GetDateParts(this.EntityPM.AccountingCancelationDate).DateTicks;
            var paymentDateParts = DateTool.GetDateParts(this.PaymentDate).DateTicks;
           
            if (value == null) {
                this.UIProperties.SetRequired("AccountingCancelationDate", this.ObjectTableName, true);

            }
            if (this.EntityPM.AccountingCancelationDate != null && cancelationDateParts > paymentDateParts) {
                this.UIProperties.SetValidity("AccountingCancelationDate", this.ObjectTableName, false, "Cancellation date ");
            }
            if (value != null) {
                this.SetDontIncludeInDeductionReport();

                this.CheckClosedMonth(value);
            }
        }
    }
    closedMonth: boolean;
    CheckClosedMonth(value: Date) {

        value= DateTool.GetDateFromDate(this.EntityPM.AccountingCancelationDate, true);
        var accountingPeriod = this.AccountingPeriods.find(d => d.Year == value.getFullYear());

        if (accountingPeriod) {

            var month = value.getMonth() + 1;

            this.ValidationErrorsList = []; // empty errors list
            // Valid Month => (ClosedMonth < month <= OpenMonth)
            if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)

               this.closedMonth= false;

            } else { // invalid (closed month)

                // push the error to errors list
               this.closedMonth= true;
              
                
                return;

            }
        }

    }
    get CancelationNotes() { return this.EntityPM.CancelationNotes; }
    set CancelationNotes(value: string) {
        if (this.EntityPM.CancelationNotes != value) {
            this.EntityPM.CancelationNotes = value;
            if (value == null) {
                this.UIProperties.SetRequired("CancelationNotes", this.ObjectTableName, true);

            }
        }
    }
    get DontIncludeInDeductionReport() { return this.EntityPM.DontIncludeInDeductionReport; }
    set DontIncludeInDeductionReport(value: boolean) {
        if (this.EntityPM.DontIncludeInDeductionReport != value) {
            this.EntityPM.DontIncludeInDeductionReport = value;
        }
    }

    SetDontIncludeInDeductionReport() {
        var AccountingCancelationDate = DateTool.GetDateFromDate(this.EntityPM.AccountingCancelationDate, true);
        var RegisterDate = DateTool.GetDateFromDate(this.EntityPM.RegisterDate, true);
        if (AccountingCancelationDate.getUTCFullYear() != RegisterDate.getUTCFullYear()) {

        
             this.EntityPM.DontIncludeInDeductionReport = true;

        }

    }

    GetAccountingPeriods() {
        var filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter("PeriodTypeCode", "1", null, null, "Equals", false, false, false, "string"); // 1-Regular

        this._AccountingPeriodListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AccountingPeriods = myResponse.Result;
               
                }
            }
        });
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
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("APPAyment.O.CancellationDateValidation"));
        }
       if(this.closedMonth){
        this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ClosedMonth"));
        }
        if (this.ValidationErrorsList.length == 0) {

            this.CurrentSession.CurrentWindow.Close("Ok");
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }
}
