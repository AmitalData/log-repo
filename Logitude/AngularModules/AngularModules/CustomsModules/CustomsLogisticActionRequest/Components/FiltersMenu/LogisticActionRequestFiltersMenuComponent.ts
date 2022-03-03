import { Component, EventEmitter, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";

@Component({
    selector: 'LogisticActionRequestFiltersMenuComponent',
    templateUrl: './LogisticActionRequestFiltersMenuComponent.html',
})


export class LogisticActionRequestFiltersMenuComponent
    extends BaseComponent {
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    @Output() SelectedValueChanged = new EventEmitter();
    _SelectedValue: string = 'Q';
    constructor() {
        super();
    }
    FilterClicked(filter: any) {
        this._SelectedValue=filter;
        var RemoveFilter = false;
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportmodeId");
        }

        if (filter == "Q") {
            this.apiQueryFilters.addAdditionalFilter("TransportmodeId", filter, null, null, "NotEqual", false, false, false, "string");
        } else {
            this.apiQueryFilters.addAdditionalFilter("TransportmodeId", filter, null, null, "Equals", false, false, false, "string");
        }
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
    }
}

