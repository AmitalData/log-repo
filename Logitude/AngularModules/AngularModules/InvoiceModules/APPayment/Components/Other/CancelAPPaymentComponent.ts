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
    private accountingCancelationDate: Date;
    get AccountingCancelationDate() { return this.accountingCancelationDate; }
    set AccountingCancelationDate(value: Date) {
        if (this.accountingCancelationDate != value) {
            this.accountingCancelationDate = value;
            var cancelationDateParts = DateTool.GetDateParts(this.accountingCancelationDate).DateTicks;
            var paymentDateParts = DateTool.GetDateParts(this.PaymentDate).DateTicks;

            if (value == null) {
                this.UIProperties.SetRequired("AccountingCancelationDate", this.ObjectTableName, true);

            }
            if (this.accountingCancelationDate != null && cancelationDateParts < paymentDateParts) {
                this.UIProperties.SetValidity("AccountingCancelationDate", this.ObjectTableName, false, TextCodeTranslator.Translate("APPAyment.O.CancellationDateValidation"));
            }
            if (value != null) {
                this.SetDontIncludeInDeductionReport();

                this.CheckClosedMonth(value);
            }
        }
    }
    closedMonth: boolean;
    CheckClosedMonth(value: Date) {

        value = DateTool.GetDateFromDate(this.AccountingCancelationDate, true);
        var accountingPeriod = this.AccountingPeriods.find(d => d.Year == value.getFullYear());

        if (accountingPeriod) {

            var month = value.getMonth() + 1;

            this.ValidationErrorsList = []; // empty errors list
            // Valid Month => (ClosedMonth < month <= OpenMonth)
            if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)

                this.closedMonth = false;

            } else { // invalid (closed month)

                // push the error to errors list
                this.closedMonth = true;


                return;

            }
        }

    }
    private cancelationNotes: string;
    get CancelationNotes() { return this.cancelationNotes; }
    set CancelationNotes(value: string) {
        if (this.cancelationNotes != value) {
            this.cancelationNotes = value;
            if (value == null) {
                this.UIProperties.SetRequired("CancelationNotes", this.ObjectTableName, true);

            }
        }
    }
    private dontIncludeInDeductionReport: boolean;
    get DontIncludeInDeductionReport() { return this.dontIncludeInDeductionReport; }
    set DontIncludeInDeductionReport(value: boolean) {
        if (this.dontIncludeInDeductionReport != value) {
            this.dontIncludeInDeductionReport = value;
        }
    }

    SetDontIncludeInDeductionReport() {
        var AccountingCancelationDate = DateTool.GetDateFromDate(this.AccountingCancelationDate, true);
        var RegisterDate = DateTool.GetDateFromDate(this.EntityPM.RegisterDate, true);
        if (AccountingCancelationDate.getUTCFullYear() != RegisterDate.getUTCFullYear()) {


            this.DontIncludeInDeductionReport = true;

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
        this.FillValidationErrorList();

        if (this.ValidationErrorsList.length == 0) {
            this.SetEntityPMFields();
            this.CurrentSession.CurrentWindow.Close("Ok");
        }
    }

    private FillValidationErrorList() {
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.AccountingCancelationDate)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.AccountingCancelationDate")));

        }
        if (AppTool.IsNullOrEmpty(this.CancelationNotes)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.CancelationNotes")));

        }
        var datetocompare = DateTool.GetDateParts(this.AccountingCancelationDate).DateTicks;
        var dateFromcompare = DateTool.GetDateParts(this.PaymentDate).DateTicks;
        if (datetocompare < dateFromcompare) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("APPAyment.O.CancellationDateValidation"));
        }

        if (this.closedMonth) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ClosedMonth"));
        }

    }

    private SetEntityPMFields() {
         var value: Date = DateTool.GetDateFromDate(this.AccountingCancelationDate, true);
        this.EntityPM.AccountingCancelationDate = value;
        this.EntityPM.CancelationNotes = this.CancelationNotes;
        this.EntityPM.DontIncludeInDeductionReport = this.DontIncludeInDeductionReport;
    }


    //private RejectChanges() {
    //    this.EntityPM.AccountingCancelationDate = null;
    //    this.EntityPM.CancelationNotes = null;
    //    this.EntityPM.DontIncludeInDeductionReport = this.DontIncludeInDeductionReport;
    //}
    CancelButtonClicked() {
        //this.RejectChanges();
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }
}
