import {Component, Output, EventEmitter} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'ActivitiesFilter',
    inputs: ['SelectedValue'],
    template:
    `
    <ul class="FiltersMenu">
        <li (click)="itemClicked('All')" (mouseover)="itemMouseOver('All')" [class.SelectedFilter]="SelectedValue === 'All'">
            All
        </li>
        <li (click)="itemClicked('CL')" (mouseover)="itemMouseOver('CL')" (mouseleave)="itemMouseLeave('CL')" [class.SelectedFilter]="SelectedValue === 'CL'" title="Phone Call">
            <img [id]="ActivityFilter_CL" class="CenterCenter" src="./Images/Activities/CL_g.png" style="top: 1px;" />
        </li>
        <li (click)="itemClicked('TS')" (mouseover)="itemMouseOver('TS')" (mouseleave)="itemMouseLeave('TS')" [class.SelectedFilter]="SelectedValue === 'TS'" title="Task">
            <img [id]="ActivityFilter_TS" class="CenterCenter" src="./Images/Activities/TS_g.png" style="top: 1px;" />
        </li>
        <li (click)="itemClicked('AP')" (mouseover)="itemMouseOver('AP')" (mouseleave)="itemMouseLeave('AP')" [class.SelectedFilter]="SelectedValue === 'AP'" title="Appointment">
            <img [id]="ActivityFilter_AP" class="CenterCenter" src="./Images/Activities/AP_g.png" style="top: 1px;" />
        </li>
        <li (click)="itemClicked('EO')" (mouseover)="itemMouseOver('EO')" (mouseleave)="itemMouseLeave('EO')" [class.SelectedFilter]="SelectedValue === 'EO'" title="Email Out">
            <img [id]="ActivityFilter_EO" class="CenterCenter" src="./Images/Activities/EO_g.png" style="top: 1px;" />
        </li>
    </ul>
    `
})

export class ActivitiesFilter {
    public SelectedValue: string = "All";
    @Output() SelectedValueChanged = new EventEmitter();
    ActivityFilter_CL: string;
    ActivityFilter_TS: string;
    ActivityFilter_AP: string;
    ActivityFilter_EO: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.ActivityFilter_CL = "ActivityFilter_CL_-1_-1";
            this.ActivityFilter_TS = "ActivityFilter_TS_-1_-1";
            this.ActivityFilter_AP = "ActivityFilter_AP_-1_-1";
            this.ActivityFilter_EO = "ActivityFilter_EO_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ActivityFilter");
            this.ActivityFilter_CL = "ActivityFilter_CL_" + idIndex;
            this.ActivityFilter_TS = "ActivityFilter_TS_" + idIndex;
            this.ActivityFilter_AP = "ActivityFilter_AP_" + idIndex;
            this.ActivityFilter_EO = "ActivityFilter_EO_" + idIndex;
        }
    }

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);

            var img_CL = document.getElementById(this.ActivityFilter_CL);
            var img_TS = document.getElementById(this.ActivityFilter_TS);
            var img_AP = document.getElementById(this.ActivityFilter_AP);
            var img_EO = document.getElementById(this.ActivityFilter_EO);

            img_CL.setAttribute("src", "./Images/Activities/CL_g.png");
            img_TS.setAttribute("src", "./Images/Activities/TS_g.png");
            img_AP.setAttribute("src", "./Images/Activities/AP_g.png");
            img_EO.setAttribute("src", "./Images/Activities/EO_g.png");

            switch (itemValue) {
                case "CL": {
                    img_CL.setAttribute("src", "./Images/Activities/CL_w.png");
                    break;
                }

                case "TS": {
                    img_TS.setAttribute("src", "./Images/Activities/TS_w.png");
                    break;
                }

                case "AP": {
                    img_AP.setAttribute("src", "./Images/Activities/AP_w.png");
                    break;
                }

                case "EO": {
                    img_EO.setAttribute("src", "./Images/Activities/EO_w.png");
                    break;
                }
            }
        }
    }
    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_CL = document.getElementById(this.ActivityFilter_CL);
            var img_TS = document.getElementById(this.ActivityFilter_TS);
            var img_AP = document.getElementById(this.ActivityFilter_AP);
            var img_EO = document.getElementById(this.ActivityFilter_EO);

            switch (itemValue) {
                case "CL": {
                    img_CL.setAttribute("src", "./Images/Activities/CL.png");
                    break;
                }
                case "VM": {
                    img_TS.setAttribute("src", "./Images/Activities/VM.png");
                    break;
                }
                case "TS": {
                    img_TS.setAttribute("src", "./Images/Activities/TS.png");
                    break;
                }

                case "AP": {
                    img_AP.setAttribute("src", "./Images/Activities/AP.png");
                    break;
                }

                case "EO": {
                    img_EO.setAttribute("src", "./Images/Activities/EO.png");
                    break;
                }
                case "EI": {
                    img_EO.setAttribute("src", "./Images/Activities/EI.png");
                    break;
                }
            }
        }
    }
    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_CL = document.getElementById(this.ActivityFilter_CL);
            var img_TS = document.getElementById(this.ActivityFilter_TS);
            var img_AP = document.getElementById(this.ActivityFilter_AP);
            var img_EO = document.getElementById(this.ActivityFilter_EO);

            switch (itemValue) {
                case "CL": {
                    img_CL.setAttribute("src", "./Images/Activities/CL_g.png");
                    break;
                }

                case "TS": {
                    img_TS.setAttribute("src", "./Images/Activities/TS_g.png");
                    break;
                }

                case "AP": {
                    img_AP.setAttribute("src", "./Images/Activities/AP_g.png");
                    break;
                }

                case "EO": {
                    img_EO.setAttribute("src", "./Images/Activities/EO_g.png");
                    break;
                }
            }
        }
    }
}
