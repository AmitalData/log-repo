import { Component, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { SessionLocator } from '../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'InvitedFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
        `
    <ul class="FiltersMenu">
        <li style=" width: 55px;" (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'" title="All">
            All
        </li>
        <li style=" width: 50px;" (click)="itemClicked('IN')" (mouseover)="itemMouseOver('IN')" (mouseleave)="itemMouseLeave('IN')" [class.SelectedFilter]="SelectedValue === 'IN'" title="Invited">
            Invited
        </li>
        <li style=" width: 80px;" (click)="itemClicked('NI')" (mouseover)="itemMouseOver('NI')" (mouseleave)="itemMouseLeave('NI')" [class.SelectedFilter]="SelectedValue === 'NI'" title="Not Invited">
           Not Invited
        </li>
    </ul>
    `
})

export class InvitedFilter {
    public FilterId_IN: string;
    public FilterId_NI: string;
    public FilterId_A: string;

    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

        if (this.CurrentSession == null) {
            this.FilterId_IN = "LocationFilter_IN_-1_-1";
            this.FilterId_NI = "LocationFilter_NI_-1_-1";
            this.FilterId_A = "LocationFilter_A_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("InvitedFilter");
            this.FilterId_IN = "LocationFilter_IN_" + idIndex;
            this.FilterId_NI = "LocationFilter_NI_" + idIndex;
            this.FilterId_A = "LocationFilter_A_" + idIndex;
        }
    }

    private selectedValue: string = "A";
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
