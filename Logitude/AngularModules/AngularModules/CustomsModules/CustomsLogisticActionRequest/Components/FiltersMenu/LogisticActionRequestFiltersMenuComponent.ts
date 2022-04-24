import { Component, EventEmitter, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    selector: 'LogisticActionRequestFiltersMenuComponent',
    templateUrl: './LogisticActionRequestFiltersMenuComponent.html',
})


export class LogisticActionRequestFiltersMenuComponent
    extends BaseComponent {
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
    @Output() SelectedValueChanged = new EventEmitter();
    // _SelectedValue: string = 'Q';
    constructor() {
        super();
    }
    // FilterClicked(filter: any) {
    //     this._SelectedValue=filter;
    //     var RemoveFilter = false;
    //     if (this.apiQueryFilters.AdditionalFilters.length > 0) {
    //         this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportmodeId");
    //     }

    //     if (filter == "Q") {
    //         this.apiQueryFilters.addAdditionalFilter("TransportmodeId", filter, null, null, "NotEqual", false, false, false, "string");
    //     } else {
    //         this.apiQueryFilters.addAdditionalFilter("TransportmodeId", filter, null, null, "Equals", false, false, false, "string");
    //     }
    //     this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
    // }
    SelectedValue
    itemClicked(itemValue: string) {
        var RemoveFilter = false;
        this.SelectedValue = itemValue;

        if (this.apiQueryFilters.AdditionalFilters.length > 0)
            this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId")

        if (itemValue !== "All")
            this.apiQueryFilters.addAdditionalFilter("TransportmodeId", itemValue, null, null, "Equals", false, false, false, "string");

        this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
        // this.ApplyTransportSelectedStyle();
    }

    TransportFilter_A: string = "TransportFilter_A_-1_-1";
    TransportFilter_O: string = "TransportFilter_O_-1_-1";
    TransportFilter_I: string = "TransportFilter_I_-1_-1";

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
                    //img_I.style.top = "1px";
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

    // ApplyTransportSelectedStyle() {
    //     var itemValue = this.SelectedValue;
    //     var img_A = document.getElementById(this.TransportFilter_A);
    //     var img_O = document.getElementById(this.TransportFilter_O);
    //     var img_I = document.getElementById(this.TransportFilter_I);
    //     if (img_A) {
    //         SessionLocator.SelectedSession.ChangeSessionHeader({ TransportId: itemValue });
    //         img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
    //         img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
    //         img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
    //         switch (itemValue) {
    //             case "A": {
    //                 img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
    //                 break;
    //             }

    //             case "O": {
    //                 img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
    //                 break;
    //             }

    //             case "L": {
    //                 img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
    //                 break;
    //             }
    //         }
    //     }
    // }
}

