declare var window: any;
import {Directive, ElementRef, Renderer, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit} from '@angular/core';  
import {SessionLocator} from '../../Utilities/SessionLocator';


@Component({
    moduleId: module.id,

    selector: 'ChooseDatesComponent',
    templateUrl: './ChooseDatesComponent.html',
})

export class ChooseDatesComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[] = [];
    public DateSelected: EventEmitter<any> = new EventEmitter();
    private selectedToDate: any;
    public get SelectedToDate() { return this.selectedToDate; }
    public set SelectedToDate(newValue: any) {
        this.selectedToDate = newValue; 
    }
    private selectedFromDate: any;
    public get SelectedFromDate() { return this.selectedFromDate; }
    public set SelectedFromDate(newValue: any) {
        this.selectedFromDate = newValue; 
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
        return ValidationErrors;
    }
}
