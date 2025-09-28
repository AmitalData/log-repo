import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AccountingPeriodList} from '../../EntityLists/AccountingPeriodList';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AccountingPeriodPMService} from '../../Services/StandardPMs/AccountingPeriodPMService';
import {LedgerTransactionListService} from '../../Services/StandardLists/LedgerTransactionListService';
import {AccountingPeriodPM} from '../../EntityPMs/AccountingPeriodPM';

import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Args, PeriodTypeCode} from '../Maintenance/AccountingPeriodsComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    
    selector: 'EditAccountingPeriodComponent',
    templateUrl: './EditAccountingPeriodComponent.html',
})

export class EditAccountingPeriodComponent extends BaseComponent {
    public DataContext: EditAccountingPeriodComponent = this;
    public EntityPM: AccountingPeriodPM;
    public EntityId: string;
    accountingPeriodListPM: AccountingPeriodPM[];

    oldClosedMonth: number;

    public ObjectTableName: string = "AccountingPeriod";
    accountingPeriodPMService: AccountingPeriodPMService;
    transactionsService: LedgerTransactionListService;
    accountingPeriodList: AccountingPeriodList[];

    accountingPeriod: AccountingPeriodList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(){
        super();
        this.accountingPeriodPMService = new AccountingPeriodPMService();
        this.transactionsService = new LedgerTransactionListService();

        this.UIProperties.SetEnabled("Year", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PeriodTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OpenMonth", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ClosedMonth", this.ObjectTableName, false);
    }

    SetWindowArgs(args: Args) {

        this.EntityId = args.EntityId;
        this.accountingPeriod = args.AccountingRow;
        this.accountingPeriodList = args.AccountingRows;
       
        this.Run();
    }

    Run() {
        this.accountingPeriodPMService.get(this.EntityId).subscribe((myResult: any) => {
            var result = myResult.Result;
            if (!AppTool.IsNullOrEmpty(result)) {
                this.EntityPM = result;
                this.oldClosedMonth = this.EntityPM.ClosedMonth;
                if(this.accountingPeriodList && this.accountingPeriodList.length > 0){
                    this.accountingPeriodListPM = [];
                    for(var i=0; i<this.accountingPeriodList.length; i++){
                        if(this.accountingPeriodList[i].Id !== this.EntityPM.Id ){
                            this.accountingPeriodPMService.get(this.accountingPeriodList[i]?.Id).subscribe((myResult: any) => {
                                if(myResult && myResult.Result){
                                   this.accountingPeriodListPM.push(myResult.Result);
                                }
                            })
                        }
                    }
                }
            } else {
                console.log("cannot find the entity!!");
            }
        });

    }

    get Year() { return this.EntityPM.Year; }
    set Year(value: number) {
        if (this.EntityPM.Year != value) {
            this.EntityPM.Year = value;
        }
    }

    get PeriodTypeCode() { return this.EntityPM.PeriodTypeCode; }
    set PeriodTypeCode(value: string) {
        if (this.EntityPM.PeriodTypeCode != value) {
            this.EntityPM.PeriodTypeCode = value;
        }
    }

    get OpenMonth() { return this.EntityPM.OpenMonth; }
    set OpenMonth(value: number) {
        if (this.EntityPM.OpenMonth != value) {
                this.EntityPM.OpenMonth = value;
        }
    }

    get ClosedMonth() { return this.EntityPM.ClosedMonth; }
    set ClosedMonth(value: number) {
        if (this.EntityPM.ClosedMonth != value) {
            this.EntityPM.ClosedMonth = value;
        }
    }


    public ValidationErrorsList: string[];

    OkButtonClicked() {
        var errors: string[] = [];

        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.SubmitChanges();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges() {
       
        this.accountingPeriodPMService.update(this.EntityPM).subscribe((myResult:any) => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                if(this.accountingPeriodListPM && this.accountingPeriodListPM.length > 0 && this.EntityPM.PeriodTypeCode !== PeriodTypeCode.Accounting){
                    for(var i=0; i<this.accountingPeriodListPM.length; i++){
                        if(this.accountingPeriodListPM[i].Id != this.EntityPM.Id && this.accountingPeriodListPM[i]?.PeriodTypeCode !== PeriodTypeCode.Accounting){
                            this.accountingPeriodListPM[i].ClosedMonth = this.EntityPM.ClosedMonth;
                            this.accountingPeriodListPM[i].OpenMonth = this.EntityPM.OpenMonth;
                            this.accountingPeriodPMService.update(this.accountingPeriodListPM[i]).subscribe((myResult:any) => {
                                var mm: ServiceResponse = myResult;
                                if (mm.HasError) {
                                    this.ValidationErrorsList = mm.ErrorsArray;
                                }
                                
                            });
                        }
                       
                    }
                }
                this.CurrentSession.CloseCurrentWindowEmit("ok");

            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }


    IncrementOpen() {
        this.ValidationErrorsList = [];

        if (!AppTool.IsNullOrEmpty(this.OpenMonth)) {
            if (this.OpenMonth < 12) {

                // begin: invoice row logic
                if (this.EntityPM.PeriodTypeCode == "2" || this.EntityPM.PeriodTypeCode == "3"  ) { //2-invoice 3-Interest Invoice
                    if (this.OpenMonth+1 > this.accountingPeriod.OpenMonth) {
                        this.ValidationErrorsList = [];
                        if (this.EntityPM.PeriodTypeCode == "2")
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingPeriod.O.CantOpenInvoiceMonth"));
                        else
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingPeriod.O.CantOpenInterestInvoiceMonth"));
                        return;
                    }
                }
                //end

                this.OpenMonth++;
            }
        }
    }
    DecrementOpen() {
        this.ValidationErrorsList = [];

        if (!AppTool.IsNullOrEmpty(this.OpenMonth)) {

            if (this.OpenMonth > 0 && (this.OpenMonth > this.ClosedMonth + 1 || this.ClosedMonth == undefined)) {


                var endDayNumber: number = new Date(new Date().getFullYear(), this.OpenMonth + 1, 0).getDate();

                var filters = new ApiQueryFilters(true);

                var fromDate = new Date();
                fromDate.setFullYear(this.Year);
                fromDate.setMonth(this.OpenMonth-1);
                fromDate.setDate(1);

                var toDate = new Date();
                toDate.setFullYear(this.Year);
                toDate.setMonth(this.OpenMonth-1);
                toDate.setDate(endDayNumber);

                filters.addAdditionalFilter("AccountingDate", fromDate, toDate, null, "Between", false, false, false, "number");
                switch (this.PeriodTypeCode) {
                    case "2": { // Regular Invoice
                        filters.addAdditionalFilter("OnlyNonInterestInvoice", true, null, null, "Equal", true, false, false, "boolean");
                        break;
                    }

                    case "3": { // Interest Invoice
                        filters.addAdditionalFilter("OnlyInterestInvoice", true, null, null, "Equal", true, false, false, "boolean");
                        break;
                    }

                    default: {  
                        filters.addAdditionalFilter("OnlyNonInvoice", true, null, null, "Equal", true, false, false, "boolean");
                        break;
                    }
                }

                this.transactionsService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var myResult = myResponse.Result;

                            if (myResult.length > 0) { // transactions exist
                                this.ValidationErrorsList = [];
                                this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingPeriod.O.CantCancelOpenMonth"));
                            } else {
                                //if (this.OpenMonth > 0 && this.OpenMonth > this.ClosedMonth + 1) {
                                    this.OpenMonth--;
                                //}
                            }
                        }
                    }
                });


            }


        }
    }

    IncrementClosed() {
        this.ValidationErrorsList = [];

        if (!AppTool.IsNullOrEmpty(this.ClosedMonth)) {

            if (this.ClosedMonth == 12)
                return;

            var endDayNumber: number = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate();

            if (((this.ClosedMonth == new Date().getMonth() + 1) && (new Date()).getDay() < endDayNumber) && (this.Year == (new Date().getFullYear()))) { // current month && not ended
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.MonthNotEndedCantClosed"));  //"The month is not ended, can’t be closed");
            } else {
                if (this.ClosedMonth < 12 && this.ClosedMonth < this.OpenMonth - 1) {
                    this.ClosedMonth++;
                } else if (this.OpenMonth == 12 && this.ClosedMonth == 11) {
                    this.ClosedMonth++;
                }
            }

        } else {
            if (this.OpenMonth > 1) {
                if ( ((new Date().getMonth() + 1) == 1) && (this.Year == (new Date().getFullYear())) ) { // current month
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.MonthNotEndedCantClosed"));  //"The month is not ended, can’t be closed");
                } else {
                    this.ClosedMonth = 1;
                }
            }
        }
    }
    DecrementClosed() {
        // // incase: we want to allow cancelling before save the period - DON'T DELETE!
        // if(this.ClosedMonth){

        //     if(this.ClosedMonth == 1 && !this.oldClosedMonth)
        //         this.ClosedMonth = null

        //     if(this.ClosedMonth > 1 && (this.ClosedMonth > this.oldClosedMonth || !this.oldClosedMonth))
        //         this.ClosedMonth--;
        // }





        if(this.ClosedMonth){

             if (this.EntityPM.PeriodTypeCode == "2" || this.EntityPM.PeriodTypeCode == "3") { //2-invoice 3-Interest Invoice
                 if (this.ClosedMonth == this.accountingPeriod.ClosedMonth) {
                     this.ValidationErrorsList = [];
                     if (this.EntityPM.PeriodTypeCode == "2")
                         this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingPeriod.O.CantCancelInvoiceClosedMonth"));
                     else
                         this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingPeriod.O.CantCancelInterestInvoiceClosedMonth"));
                     //this.ValidationErrorsList.push("Cannot open an invoice's closed month which is less than accounting period's closed month.");
                     // this.ValidationErrorsList.push(TextCodeTranslator.Translate("AccountingPeriod.O.CantCancelOpenMonth"));
                     return;
                 }
             }


            if(this.ClosedMonth == 1)
                this.ClosedMonth = null
            else if(this.ClosedMonth > 1)
                this.ClosedMonth--;
        }

    }



}
