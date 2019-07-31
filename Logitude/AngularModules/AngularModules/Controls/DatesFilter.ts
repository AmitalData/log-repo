import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'DatesFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    template:
    `
    <ul class="FiltersMenu">
        <li style=" width: 65px;" (click)="itemClicked('T')" (mouseover)="itemMouseOver('T')" (mouseleave)="itemMouseLeave('T')" [class.SelectedFilter]="SelectedValue === 'T'">
            Today
        </li>
        <li style=" width: 65px;" (click)="itemClicked('Y')" (mouseover)="itemMouseOver('Y')" (mouseleave)="itemMouseLeave('Y')" [class.SelectedFilter]="SelectedValue === 'Y'">
            Yesterday
        </li>
        <li style=" width: 65px;" (click)="itemClicked('P')" (mouseover)="itemMouseOver('P')" (mouseleave)="itemMouseLeave('P')" [class.SelectedFilter]="SelectedValue === 'P'">
          Period
        </li>
    </ul>
    `
})

export class DatesFilter {
    public FilterId_A: string;
    public FilterId_T: string;
    public FilterId_I: string;
    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.FilterId_A = "DateFilter_T_-1_-1";
            this.FilterId_T = "DateFilter_Y_-1_-1";
            this.FilterId_I = "DateFilter_P_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("DatesFilter");
            this.FilterId_A = "DateFilter_T_" + idIndex;
            this.FilterId_T = "DateFilter_Y_" + idIndex;
            this.FilterId_I = "DateFilter_P_" + idIndex;
        }
    }

    private selectedValue: string = "T";
    public get SelectedValue() { return this.selectedValue; }
    public set SelectedValue(value: string) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
            this.ApplySelectedStyle();
        }
    }

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    }
    itemMouseOver(itemValue: string) {

    }
    itemMouseLeave(itemValue: string) {

    }
    ApplySelectedStyle() {

    }
}
