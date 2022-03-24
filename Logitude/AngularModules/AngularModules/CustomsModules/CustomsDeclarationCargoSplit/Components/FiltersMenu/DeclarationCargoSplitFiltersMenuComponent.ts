import { Component, EventEmitter, Output } from "@angular/core";
import { AnyKindOfDictionary } from "cypress/types/lodash";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";

@Component({
    selector: 'DeclarationCargoSplitFiltersMenuComponent',
    templateUrl: './DeclarationCargoSplitFiltersMenuComponent.html',
})


export class DeclarationCargoSplitFiltersMenuComponent
    extends BaseComponent {
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    @Output() SelectedValueChanged = new EventEmitter();
    _SelectedValue: string = 'A';
    constructor() {
        super();
    }
    FilterClicked(filter: any) {
        this._SelectedValue=filter;
        var RemoveFilter = false;
        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "Direction");
        }
        
        if (filter == "A") {
            this.apiQueryFilters.addAdditionalFilter("Direction", filter, null, null, "NotEqual", false, false, false, "string");
        } else {
            this.apiQueryFilters.addAdditionalFilter("Direction", filter, null, null, "Equals", false, false, false, "string");
        }
        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
    }
}

