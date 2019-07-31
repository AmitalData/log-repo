import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'LocationsFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu">
        <li style=" width: 40px;" (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'" title="All">
            All
        </li>
        <li style=" width: 40px;" (click)="itemClicked('O')" (mouseover)="itemMouseOver('O')" (mouseleave)="itemMouseLeave('O')" [class.SelectedFilter]="SelectedValue === 'O'" title="Office">
            Office
        </li>
        <li style=" width: 40px;" (click)="itemClicked('H')" (mouseover)="itemMouseOver('H')" (mouseleave)="itemMouseLeave('H')" [class.SelectedFilter]="SelectedValue === 'H'" title="House">
            Home
        </li>
        <li style=" width: 40px;" (click)="itemClicked('C')" (mouseover)="itemMouseOver('C')" (mouseleave)="itemMouseLeave('C')" [class.SelectedFilter]="SelectedValue === 'C'" title="Client">
          Client
        </li>
         <li style=" width: 50px;" (click)="itemClicked('D')" (mouseover)="itemMouseOver('D')" (mouseleave)="itemMouseLeave('D')" [class.SelectedFilter]="SelectedValue === 'D'" title="Day Off">
          Day Off
        </li>

    </ul>
    `
})

export class LocationsFilter {
    public FilterId_A: string;
    public FilterId_H: string;
    public FilterId_O: string;
    public FilterId_I: string;
    public FilterId_D: string;

    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.FilterId_A = "LocationFilter_A_-1_-1";
            this.FilterId_O = "LocationFilter_O_-1_-1";
            this.FilterId_H = "LocationFilter_H_-1_-1";
            this.FilterId_I = "LocationFilter_C_-1_-1";
            this.FilterId_D = "LocationFilter_D_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("LocationsFilter");
            this.FilterId_A = "LocationFilter_A_" + idIndex;
            this.FilterId_O = "LocationFilter_O_" + idIndex;
            this.FilterId_H = "LocationFilter_H_" + idIndex;
            this.FilterId_I = "LocationFilter_C_" + idIndex;
            this.FilterId_D = "LocationFilter_D_" + idIndex;
        }
    }

    private selectedValue: string = "O";
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
