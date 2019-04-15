import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'ConnectToFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu">
        <li *ngIf="FilterId_M_Feature" style=" width: 60px;" (click)="itemClicked('M')" (mouseover)="itemMouseOver('M')" (mouseleave)="itemMouseLeave('M')" [class.SelectedFilter]="SelectedValue === 'M'" title="Master">
            Master
        </li>
        <li *ngIf="FilterId_S_Feature" style=" width: 60px;" (click)="itemClicked('S')" (mouseover)="itemMouseOver('S')" (mouseleave)="itemMouseLeave('S')" [class.SelectedFilter]="SelectedValue === 'S'" title="Shipment">
            Shipment
        </li>
        <li *ngIf="FilterId_Q_Feature" style=" width: 60px;" (click)="itemClicked('Q')" (mouseover)="itemMouseOver('Q')" (mouseleave)="itemMouseLeave('Q')" [class.SelectedFilter]="SelectedValue === 'Q'" title="Quote">
          Quote
        </li>
    </ul>
    `
})

export class ConnectToFilter {
    public FilterId_M: string;
    public FilterId_S: string;
    public FilterId_Q: string;
    public FilterId_M_Feature: boolean = false;
    public FilterId_S_Feature: boolean = false;
    public FilterId_Q_Feature: boolean = false;

    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.SetVisibilityOfFilters();
        if (this.CurrentSession == null) {
            this.FilterId_M = "LocationFilter_M_-1_-1";
            this.FilterId_S = "LocationFilter_S_-1_-1";
            this.FilterId_Q = "LocationFilter_Q_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ConnectToFilter");
            this.FilterId_M = "LocationFilter_M_" + idIndex;
            this.FilterId_S = "LocationFilter_S_" + idIndex;
            this.FilterId_Q = "LocationFilter_Q_" + idIndex;
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
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "ShipmentsFilter")) {
            this.FilterId_S_Feature = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter")) {
            this.FilterId_M_Feature = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "QuotesFilter")) {
            this.FilterId_Q_Feature = true;
        }
    }
}
