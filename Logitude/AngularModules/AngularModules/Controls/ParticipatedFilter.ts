import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'ParticipatedFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu">
        <li style=" width: 55px;" (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'" title="All">
            All
        </li>
        <li  style=" width: 80px;" (click)="itemClicked('PA')" (mouseover)="itemMouseOver('PA')" (mouseleave)="itemMouseLeave('PA')" [class.SelectedFilter]="SelectedValue === 'PA'" title="Participated">
            Participated
        </li>
        <li  style=" width: 110px;" (click)="itemClicked('DP')" (mouseover)="itemMouseOver('DP')" (mouseleave)="itemMouseLeave('DP')" [class.SelectedFilter]="SelectedValue === 'DP'" title="Didn't Participate">
           Didn't Participate
        </li>
    </ul>
    `
})

export class ParticipatedFilter {
    public FilterId_PA: string;
    public FilterId_DP: string;
    public FilterId_A: string;


    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

        if (this.CurrentSession == null) {
            this.FilterId_PA = "LocationFilter_PA_-1_-1";
            this.FilterId_DP = "LocationFilter_DP_-1_-1";
            this.FilterId_A = "LocationFilter_A_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ParticipatedFilter");
            this.FilterId_PA = "LocationFilter_PA_" + idIndex;
            this.FilterId_DP = "LocationFilter_DP_" + idIndex;
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
