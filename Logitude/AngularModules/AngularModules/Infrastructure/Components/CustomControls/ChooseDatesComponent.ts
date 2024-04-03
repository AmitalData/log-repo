declare var window: any;
import {Directive, ElementRef, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit} from '@angular/core';  
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../Utilities/SessionLocator';


@Component({
    

    selector: 'ChooseDatesComponent',
    templateUrl: './ChooseDatesComponent.html',
})

export class ChooseDatesComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[] = [];
    public DateSelected: EventEmitter<any> = new EventEmitter();
    private selectedToDate: any;
    private lastYearDays: number = 366;
    public get SelectedToDate() { return this.selectedToDate; }
    public set SelectedToDate(newValue: any) {
        this.selectedToDate = newValue; 
    }
    private QueryCode : string;
    private selectedFromDate: any;
    public get SelectedFromDate() { return this.selectedFromDate; }
    public set SelectedFromDate(newValue: any) {
        this.selectedFromDate = newValue; 
    }
    SetWindowArgs(args: any) {
        this.QueryCode = args.QueryCode;
        if(args.LastYearFromDate) {
            this.SelectedFromDate = args.LastYearFromDate;
        }

        if(args.LastYearToDate) {
            this.SelectedToDate = args.LastYearToDate;
        }
    }

    OnSelectedFromDateChanged(value) {
        this.SelectedFromDate = value.SelectedDate;
    }

    OnSelectedToDateChanged(value) {
        this.SelectedToDate = value.SelectedDate;
    }

    OKbtnClick() {
        this.ValidationErrorsList = this.validateDates();
        if (this.ValidationErrorsList.length == 0) {
            this.DateSelected.emit({ From: this.SelectedFromDate, To: this.SelectedToDate });
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    validateDates(): string[] {
        var ValidationErrors = [];
        if (this.SelectedFromDate != null && this.SelectedToDate != null) {
            if (this.SelectedFromDate.getTime() > this.SelectedToDate.getTime()) {
                ValidationErrors.push("ToDate should be greater than or equal to FromDate value.");
            }
        }
        else {
            ValidationErrors.push("From date and to date are required !");
        }

        if(this.QueryCode == "LedgerTransaction.LedgerTransactions" && ValidationErrors.length == 0) {
            var dateDifference = this.calculateDiff(this.SelectedFromDate, this.SelectedToDate)
            if(dateDifference > this.lastYearDays) {
                ValidationErrors.push(TextCodeTranslator.Translate("LedgerTransaction.O.OneYearValidation"));
            }
        }

        return ValidationErrors;
    }

    calculateDiff(dateFrom, dateTo){
        return Math.floor((Date.UTC(dateTo.getFullYear(), dateTo.getMonth(), dateTo.getDate()) - Date.UTC(dateFrom.getFullYear(), dateFrom.getMonth(), dateFrom.getDate()) ) /(1000 * 60 * 60 * 24));
    }
}
