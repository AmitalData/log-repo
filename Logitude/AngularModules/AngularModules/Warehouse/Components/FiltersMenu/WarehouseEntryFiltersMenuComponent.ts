import {Component, AfterViewInit, Output, EventEmitter} from '@angular/core';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    moduleId: module.id,
    templateUrl: './WarehouseEntryFiltersMenuComponent.html',
})

export class WarehouseEntryFiltersMenuComponent implements AfterViewInit {
    @Output() SelectedValueChanged = new EventEmitter();
    apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
 
    public DirectionWidth: number = 140;
    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;
    DirectionFilter_E: string;
    DirectionFilter_R: string;
    DirectionFilter_D: string;
    DirectionFilter_I: string;
    DirectionFilter_C: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.TransportFilter_A = "TransportFilter_A_-1_-1";
            this.TransportFilter_O = "TransportFilter_O_-1_-1";
            this.TransportFilter_I = "TransportFilter_I_-1_-1";
            this.DirectionFilter_E = "DirectionFilter_E_-1_-1";
            this.DirectionFilter_I = "DirectionFilter_I_-1_-1";
            this.DirectionFilter_R = "DirectionFilter_R_-1_-1";
            this.DirectionFilter_D = "DirectionFilter_D_-1_-1";
            this.DirectionFilter_C = "DirectionFilter_D_-1_-1";
        }

        else {
            var index_T = this.CurrentSession.GetNewId("WarehouseEntryTransportFilterMenu");
            var index_D = this.CurrentSession.GetNewId("WarehouseEntryDirectionFilterMenu");
            this.TransportFilter_A = "TransportFilter_A" + index_T;
            this.TransportFilter_O = "TransportFilter_O" + index_T;
            this.TransportFilter_I = "TransportFilter_I" + index_T;
            this.DirectionFilter_E = "DirectionFilter_E" + index_D;
            this.DirectionFilter_R = "DirectionFilter_R" + index_D;
            this.DirectionFilter_D = "DirectionFilter_D" + index_D;
            this.DirectionFilter_I = "DirectionFilter_I" + index_D;
            this.DirectionFilter_C = "DirectionFilter_C" + index_D;
        }
      
        this.DirectionWidth = 140;
    }

    ngAfterViewInit() {
        this.ApplyTransportSelectedStyle();
        this.ApplyDirectionSelectedStyle();
    }

    // Transport
    SetTransport(itemValue: string) {
        this.selectedValue = itemValue;
        this.ApplyTransportSelectedStyle();
    }
    private selectedValue: string = "All";
    public get SelectedValue() { return this.selectedValue; }
    public set SelectedValue(value: string) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
        }
    }

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            var RemoveFilter = false;

            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "TransportModeId");
            }

            this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            if (itemValue == "All") {
                RemoveFilter = true;
            }

            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
            this.ApplyTransportSelectedStyle();
        }
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

                case "I": {
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

                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }
    ApplyTransportSelectedStyle() {
        var itemValue = this.SelectedValue;

        var img_A = document.getElementById(this.TransportFilter_A);
        var img_O = document.getElementById(this.TransportFilter_O);
        var img_I = document.getElementById(this.TransportFilter_I);

        if (img_A) {

            this.CurrentSession.ChangeSessionHeader({ TransportId: itemValue });

            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_G.png");

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

    // Direction
    SetDirection(itemValue: string) {
        this.selectedDirection = itemValue;
        this.ApplyDirectionSelectedStyle();
    }
    private selectedDirection: string = "All";
    public get SelectedDirection() { return this.selectedDirection; }
    public set SelectedDirection(value: string) {
        if (this.selectedDirection != value) {
            this.selectedDirection = value;
        }
    }

    DirectionitemClicked(itemValue: string) {
        if (this.SelectedDirection != itemValue) {
            this.SelectedDirection = itemValue;

            var RemoveFilter = false;
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(a => a.FieldName != "DirectionId");
            }

            this.apiQueryFilters.addAdditionalFilter("DirectionId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));

            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
            this.ApplyDirectionSelectedStyle();
        }
    }
    DirectionitemMouseOver(itemValue: string) {
        if (this.SelectedDirection != itemValue) {
            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);
            var img_C: HTMLElement;
         
            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R.png");
                    break;
                }

                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D.png");
                    break;
                }

                case "C": {
                  
                    break;
                }
            }
        }
    }
    DirectionitemMouseLeave(itemValue: string) {
        if (this.SelectedDirection != itemValue) {
            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);
            var img_C: HTMLElement;
    

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E_g.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I_g.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R_g.png");
                    break;
                }

                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D_g.png");
                    break;
                }

                case "C": {
                   
                    break;
                }
            }
        }
    }
    ApplyDirectionSelectedStyle() {
        var itemValue = this.SelectedDirection;

        var img_E = document.getElementById(this.DirectionFilter_E);
        var img_I = document.getElementById(this.DirectionFilter_I);
        var img_R = document.getElementById(this.DirectionFilter_R);
        var img_D = document.getElementById(this.DirectionFilter_D);
        var img_C: HTMLElement;

        if (img_E) {

            this.CurrentSession.ChangeSessionHeader({ DirectionId: itemValue });

            //var img_C = document.getElementById("DirectionFilter_C");
            img_E.setAttribute("src", "./Images/Directions/E_g.png");
            img_I.setAttribute("src", "./Images/Directions/I_g.png");
            img_R.setAttribute("src", "./Images/Directions/R_G.png");
            img_D.setAttribute("src", "./Images/Directions/D_G.png");

            //img_C.setAttribute("src", "./Images/Directions/C_G.png");

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E_w.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I_w.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R_w.png");
                    break;
                }

                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D_w.png");
                    break;
                }

                case "C": {
                   
                    break;
                }
            }
        }
    }



}
