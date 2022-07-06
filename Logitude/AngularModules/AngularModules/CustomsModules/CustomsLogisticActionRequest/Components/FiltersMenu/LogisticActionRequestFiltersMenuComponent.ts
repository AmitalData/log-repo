import { Component, EventEmitter, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";

@Component({
    selector: 'LogisticActionRequestFiltersMenuComponent',
    templateUrl: './LogisticActionRequestFiltersMenuComponent.html',
})
export class LogisticActionRequestFiltersMenuComponent
extends BaseComponent {
    @Output() SelectedValueChanged = new EventEmitter();
    
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    SelectedValue: string = 'All'
    TransportFilter_A: string = "TransportFilter_A_-1_-1";
    TransportFilter_O: string = "TransportFilter_O_-1_-1";
    TransportFilter_I: string = "TransportFilter_I_-1_-1";


    constructor() {
        super();
    }


    itemClicked(itemValue: string) {
        var RemoveFilter = false;
        this.SelectedValue = itemValue;

        this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportmodeId")

        const operator: string = (itemValue !== "All") ? "Equals" : "NotEqual";
        this.apiQueryFilters.addAdditionalFilter("TransportmodeId", itemValue, null, null, operator, false, false, false, "string");

        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
    }


    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    break;
                }
            }
        }
    }
    

    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }
}

