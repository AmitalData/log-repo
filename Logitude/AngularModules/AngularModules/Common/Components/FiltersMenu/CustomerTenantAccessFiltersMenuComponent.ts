import {Component, Output, EventEmitter} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    selector: 'CustomerTenantAccessFiltersMenuComponent',
    inputs: ['SelectedValue'],
    template:
    `
    <ul class="FiltersMenu" style="float:right">
        <li  (click)="itemClicked('All')" (mouseover)="itemMouseOver('All')" [class.SelectedFilter]="SelectedValue === 'All'">
            All
        </li>
        <li style="color:green" (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'" title="Accepted">
        A
        </li>
        <li  style="color:orange"  (click)="itemClicked('W')" (mouseover)="itemMouseOver('W')" (mouseleave)="itemMouseLeave('W')" [class.SelectedFilter]="SelectedValue === 'W'" title="Waiting">
        W
        </li>
        <li style="color:red" (click)="itemClicked('IA')" (mouseover)="itemMouseOver('IA')" (mouseleave)="itemMouseLeave('IA')" style="color:'red'" [class.SelectedFilter]="SelectedValue === 'IA'"  title="InActive">
        I
        </li>
      
    </ul>
    `
})

export class CustomerTenantAccessFiltersMenuComponent {
    public SelectedValue: string = "All";
    @Output() SelectedValueChanged = new EventEmitter();
    ImpoterFilter_ALL: string;
    ImpoterFilter_A: string;
    ImpoterFilter_W: string;
    ImpoterFilter_IP: string;
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.ImpoterFilter_ALL = "ImpoterFilter_ALL-1_-1";
            this.ImpoterFilter_A = "ImpoterFilter_A-1_-1";
            this.ImpoterFilter_W = "ImpoterFilter_W-1_-1";
            this.ImpoterFilter_IP = "ImpoterFilter_IP-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ImpoterFilter");
            this.ImpoterFilter_ALL = "ImpoterFilter_ALL_" + idIndex;
            this.ImpoterFilter_A = "ImpoterFilter_A_" + idIndex;
            this.ImpoterFilter_W = "ImpoterFilter_W_" + idIndex;
            this.ImpoterFilter_IP = "ImpoterFilter_IP_" + idIndex;
        }
        this.apiQueryFilters.SortBy = "RequestDateTime";
        this.apiQueryFilters.SortDirection = "Descending";
    }

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
          
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "Status");
            }
            this.apiQueryFilters.SortBy = "RequestDateTime";
            this.apiQueryFilters.SortDirection = "Descending";
            this.apiQueryFilters.addAdditionalFilter("Status", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters});            
        }
    }
    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {

        }
    }
    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {

        }
    }
}
