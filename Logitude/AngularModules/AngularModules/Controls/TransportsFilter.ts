import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'TransportsFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu">
        <li (click)="itemClicked('All')" (mouseover)="itemMouseOver('All')" [class.SelectedFilter]="SelectedValue === 'All'">
            All
        </li>
        <li (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'" title="Air">
            <img [attr.id]="FilterId_A" class="CenterCenter"  [attr.src]="SelectedValue === 'A' ? './_Resources/Images/Icons/TransportModes/Filters/A_w.png' : './_Resources/Images/Icons/TransportModes/Filters/A_g.png' "   style="top: 1px;" />
        </li>
        <li (click)="itemClicked('O')" (mouseover)="itemMouseOver('O')" (mouseleave)="itemMouseLeave('O')" [class.SelectedFilter]="SelectedValue === 'O'" title="Ocean">
            <img [attr.id]="FilterId_O"  [attr.src]="SelectedValue === 'O' ? './_Resources/Images/Icons/TransportModes/Filters/O_w.png' : './_Resources/Images/Icons/TransportModes/Filters/O_g.png' "    class="CenterCenter"  style="top: 1px;" />
        </li>
        <li (click)="itemClicked('I')" (mouseover)="itemMouseOver('I')" (mouseleave)="itemMouseLeave('I')" [class.SelectedFilter]="SelectedValue === 'I'" title="Inland">
            <img [attr.id]="FilterId_I" class="CenterCenter" [attr.src]="SelectedValue === 'I' ? './_Resources/Images/Icons/TransportModes/Filters/I_w.png' : './_Resources/Images/Icons/TransportModes/Filters/I_g.png' "  style="top: 1px;" />
        </li>
    </ul>
    `
})

export class TransportsFilter {
    public FilterId_A: string;
    public FilterId_O: string;
    public FilterId_I: string;
    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.FilterId_A = "TransportFilter_A_-1_-1";
            this.FilterId_O = "TransportFilter_O_-1_-1";
            this.FilterId_I = "TransportFilter_I_-1_-1"; 
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("TransportsFilter");
            this.FilterId_A = "TransportFilter_A_" + idIndex;
            this.FilterId_O = "TransportFilter_O_" + idIndex;
            this.FilterId_I = "TransportFilter_I_" + idIndex; 
        } 
    }

    private selectedValue: string = "All";
    public get SelectedValue() {
    
        return this.selectedValue;

    }
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
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    }
    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.FilterId_A);
            var img_O = document.getElementById(this.FilterId_O);
            var img_I = document.getElementById(this.FilterId_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_g.png");
                    break;
                }
            }
        }
    }
    ApplySelectedStyle() {
        var img_A = document.getElementById(this.FilterId_A);
        var img_O = document.getElementById(this.FilterId_O);
        var img_I = document.getElementById(this.FilterId_I);


        if (img_A) {

            img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_g.png");
            img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_g.png");
            img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_G.png");

            switch (this.SelectedValue) {
                case "A": {
                    img_A.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/O_w.png");
                    break;
                }

                case "I": {
                    img_I.setAttribute("src", "./_Resources/Images/Icons/TransportModes/Filters/I_w.png");
                    break;
                }
            }
        }
    }
}
