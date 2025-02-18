import { Component, EventEmitter, Output } from "@angular/core";
import { AnyKindOfDictionary } from "cypress/types/lodash";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    selector: 'PhysicalCheckFiltersMenuComponent',
    templateUrl: './PhysicalCheckFiltersMenuComponent.html',
})


export class PhysicalCheckFiltersMenuComponent
    extends BaseComponent {
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;

    _SelectedValue: string = 'A';
    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;

    constructor() {
        super();
        if (this.CurrentSession == null) {
            this.TransportFilter_A = "TransportFilter_A_-1_-1";
            this.TransportFilter_O = "TransportFilter_O_-1_-1";
            this.TransportFilter_I = "TransportFilter_I_-1_-1";
        } else {
            var index_T = this.CurrentSession.GetNewId("ShipmentTransportFilterMenu");
            this.TransportFilter_A = "TransportFilter_A" + index_T;
            this.TransportFilter_O = "TransportFilter_O" + index_T;
            this.TransportFilter_I = "TransportFilter_I" + index_T;
        }

    }
    FilterClicked(filter: any) {
        this._SelectedValue = filter;
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

    transportmodeId: string = "All";
    itemClicked(itemValue: string) {

        

        this.transportmodeId = itemValue;
        var RemoveFilter = false;

        if (this.apiQueryFilters.AdditionalFilters.length > 0) {
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId")
        }
        if (itemValue == "All") {
            this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "NotEqual", false, false, false, "string");
        } else {
            this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "Equals", false, false, false, "string");
        }


        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        this.ApplyTransportSelectedStyle();
    }


    ApplyTransportSelectedStyle() {
        var itemValue = this.transportmodeId;
        var img_A = document.getElementById(this.TransportFilter_A);
        var img_O = document.getElementById(this.TransportFilter_O);
        var img_I = document.getElementById(this.TransportFilter_I);
        if (img_A) {
            this.CurrentSession.ChangeSessionHeader({ TransportId: itemValue });
            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
                    break;
                }
            }
        }


    }

    itemMouseOver(itemValue: string) {
        if (this.transportmodeId != itemValue) {
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

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");

                    break;
                }
            }
        }
    }


    itemMouseLeave(itemValue: string) {
        if (this.transportmodeId != itemValue) {
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

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }
}

