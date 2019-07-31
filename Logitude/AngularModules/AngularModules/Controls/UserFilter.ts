import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'UserFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu">
        <li *ngIf="FilterId_M_Feature" style=" width: 55px;" (click)="itemClicked('M')" (mouseover)="itemMouseOver('M')" (mouseleave)="itemMouseLeave('M')" [class.SelectedFilter]="SelectedValue === 'M'" title="My">
            My
        </li>
        <li *ngIf="FilterId_A_Feature" style=" width: 55px;" (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'" title="All">
            All
        </li>
    </ul>
    `
})

export class UserFilter {
    public FilterId_M: string;
    public FilterId_A: string;
    public FilterId_M_Feature: boolean = false;
    public FilterId_A_Feature: boolean = false;

    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.SetVisibilityOfFilters();
        if (this.CurrentSession == null) {
            this.FilterId_M = "LocationFilter_M_-1_-1";
            this.FilterId_A = "LocationFilter_A_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("UserFilter");
            this.FilterId_M = "LocationFilter_M_" + idIndex;
            this.FilterId_A = "LocationFilter_A_" + idIndex;
        }
    }

    private selectedValue: string = "M";
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

    SetVisibilityOfFilters() {
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "FilingInboxMyFilter")) {
            this.FilterId_M_Feature = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "FilingInboxAllFilter")) {
            this.FilterId_A_Feature = true;
        }
    }
}
