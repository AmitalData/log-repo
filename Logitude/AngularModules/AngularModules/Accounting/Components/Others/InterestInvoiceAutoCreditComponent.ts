import { Component, OnInit } from '@angular/core';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARInvoicePM } from '../../../Invoice/EntityPMs/ARInvoicePM';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { AccountingPeriodListService } from '../../../Accounting/Services/StandardLists/AccountingPeriodListService';
import { AccountingPeriodList } from '../../../Accounting/EntityLists/AccountingPeriodList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters, FilterItem } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
@Component({

    templateUrl: './InterestInvoiceAutoCreditComponent.html',
})




export class InterestInvoiceAutoCreditComponent extends BaseComponent {


    public EntityPM: ARInvoicePM = null;
    AccountingPeriods: AccountingPeriodList[] = [];
    _AccountingPeriodListService: AccountingPeriodListService = new AccountingPeriodListService();
    interestReportPM: InterestReportPM;
    public DataContext = this;
    public ObjectTableName: string = "ARInvoice";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    InterestReportPMService: InterestReportPMService = new InterestReportPMService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    IsVisibile: boolean;
    public InvoiceDateText: string;
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.interestReportPM = args.InterestReportPM;
          
        }
    }


    constructor() {
        super();
        this.InvoiceDateText = TextCodeTranslator.Translate("InterestReport.O.EnterCreditInvoiceDate");
        // this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe((response:any) => {
        this.IsVisibile = true;
      
        //   });
    }

 
    private invoiceDate: Date;
    get InvoiceDate() { return this.invoiceDate; }
    set InvoiceDate(value: Date) {
        if (this.invoiceDate != value) {
            this.invoiceDate = value;
            if (value == null) {
                this.UIProperties.SetRequired("InvoiceDate", this.ObjectTableName, true);

            }
          
        }
    }
    closedMonth: boolean;
    CheckClosedMonth(value: Date) {

        value = DateTool.GetDateFromDate(this.InvoiceDate, true);
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
    oldStatusCode:string;
    FIELD_IS_REQUIERD: string;
    OkButtonClicked() {

        this.ValidationErrorsList = [];
        this.FillValidationErrorList();

        if (this.ValidationErrorsList.length == 0) {
            this.SetInterestReportInvoiceDate();
            this.oldStatusCode = this.interestReportPM.InterestReportStatusCode

            this.interestReportPM.InterestReportStatusCode = "3";
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            this.InterestReportPMService.update(this.interestReportPM).subscribe((result: ServiceResponse) => {
                if (!result.HasError) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.CurrentWindow.Close("Ok");
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.ValidationErrorsList = result.ErrorsArray;
                    this.interestReportPM.InterestReportStatusCode = this.oldStatusCode;

                    this.CurrentSession.StopBusyIndicator();
                }
            });

        }
    }

    private FillValidationErrorList() {
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.InvoiceDate)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("InterestReport.O.RequiedCreditInvoiceDate"));
        }
        if (this.closedMonth) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ClosedMonth"));
        }

    }

    private SetInterestReportInvoiceDate() {
        var value: Date = DateTool.GetDateFromDate(this.InvoiceDate, true);
        this.interestReportPM.InvoiceDate = value;   
    }
   
    CancelButtonClicked() {
  
        this.interestReportPM.IsDirty = false;
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }
}
