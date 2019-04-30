import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'DirectionsFilter',
    inputs: ['SelectedValue','HideCustomsImport','HideImportDomistic'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu" [style.width.px]="DirectionWidth" style="display:block">
        <li (click)="itemClicked('All')" (mouseover)="itemMouseOver('All')" [class.SelectedFilter]="SelectedValue === 'All'">
            All
        </li>
        <li (click)="itemClicked('E')" (mouseover)="itemMouseOver('E')" (mouseleave)="itemMouseLeave('E')" [class.SelectedFilter]="SelectedValue === 'E'" title="Export">
            <img [attr.id]="FilterId_E" class="CenterCenter" [attr.src]="SelectedValue === 'E' ? './_Resources/Images/Icons/Directions/E_w.png' : './_Resources/Images/Icons/Directions/E_g.png'"   />
        </li>
        <li *ngIf="!itmImportDomistic" (click)="itemClicked('I')" (mouseover)="itemMouseOver('I')" (mouseleave)="itemMouseLeave('I')" [class.SelectedFilter]="SelectedValue === 'I'" title="Import">
            <img [attr.id]="FilterId_I" class="CenterCenter"  [attr.src]="SelectedValue === 'I' ? './_Resources/Images/Icons/Directions/I_w.png' : './_Resources/Images/Icons/Directions/I_g.png'"  />
        </li>
        <li (click)="itemClicked('R')" (mouseover)="itemMouseOver('R')" (mouseleave)="itemMouseLeave('R')" [class.SelectedFilter]="SelectedValue === 'R'" title="Drop">
            <img [attr.id]="FilterId_R" class="CenterCenter" [attr.src]="SelectedValue === 'R' ? './_Resources/Images/Icons/Directions/R_w.png' : './_Resources/Images/Icons/Directions/R_g.png'"  />
        </li>
        <li *ngIf="!itmImportDomistic" (click)="itemClicked('D')" (mouseover)="itemMouseOver('D')" (mouseleave)="itemMouseLeave('D')" [class.SelectedFilter]="SelectedValue === 'D'" title="Domestic">
            <img [attr.id]="FilterId_D" class="CenterCenter"  [attr.src]="SelectedValue === 'D' ? './_Resources/Images/Icons/Directions/D_w.png' : './_Resources/Images/Icons/Directions/D_g.png'" />
        </li>
        <li *ngIf="itmImportShipments || itmImportDomistic" (click)="itemClicked('C')" (mouseover)="itemMouseOver('C')" (mouseleave)="itemMouseLeave('C')" [class.SelectedFilter]="SelectedValue === 'C'" title="Customs Import">
            <img [attr.id]="FilterId_C" class="CenterCenter" [attr.src]="SelectedValue === 'C' ? './_Resources/Images/Icons/Directions/C_w.png' : './_Resources/Images/Icons/Directions/C_g.png'" />
        </li>
    </ul>
    `
})

export class DirectionsFilter {
    public FilterId_E: string;
    public FilterId_I: string;
    public FilterId_R: string;
    public FilterId_D: string;
    public FilterId_C: string;
    public DirectionWidth: number = 140;
  
    public itmImportShipments: boolean = false;
    public itmImportDomistic: boolean = false;

    @Output() SelectedValueChanged = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.CurrentSession == null) {
            this.FilterId_E = "DirectionsFilter_E_-1_-1";
            this.FilterId_I = "DirectionsFilter_I_-1_-1";
            this.FilterId_R = "DirectionsFilter_R_-1_-1";
            this.FilterId_D = "DirectionsFilter_D_-1_-1";
            this.FilterId_C = "DirectionsFilter_C_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("DirectionsFilter");
            this.FilterId_E = "DirectionsFilter_E_" + idIndex;
            this.FilterId_I = "DirectionsFilter_I_" + idIndex;
            this.FilterId_R = "DirectionsFilter_R_" + idIndex;
            this.FilterId_D = "DirectionsFilter_D_" + idIndex;
            this.FilterId_C = "DirectionsFilter_C_" + idIndex;
        }

        this.SetVisibilityImportShipments();
        this.SetVisibilityImportDomistic();

    }


    private selectedValue: string = "All";
    public get SelectedValue() { return this.selectedValue; }
    public set SelectedValue(value: string) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
            this.ApplySelectedStyle();
        }
    }


    private hideCustomsImport: boolean = false;
    public get HideCustomsImport() { return this.hideCustomsImport; }
    public set HideCustomsImport(value: boolean) {
        if (this.hideCustomsImport != value) {
            this.hideCustomsImport = value;
            this.SetVisibilityImportShipments();
        }
    }

    private hideImportDomistic: boolean = false;
    public get HideImportDomistic() { return this.hideImportDomistic; }
    public set HideImportDomistic(value: boolean) {
        if (this.hideImportDomistic != value) {
            this.hideImportDomistic = value;
            this.SetVisibilityImportDomistic();
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
            var img_E = document.getElementById(this.FilterId_E);
            if (!this.itmImportDomistic)
            var img_I = document.getElementById(this.FilterId_I);
            var img_R = document.getElementById(this.FilterId_R);
            if (!this.itmImportDomistic)
            var img_D = document.getElementById(this.FilterId_D);
            if (this.itmImportShipments)
            var img_C = document.getElementById(this.FilterId_C);

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E.png");
                    break;
                }

                case "I": {
                    if (!this.itmImportDomistic)
                    img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R.png");
                    break;
                }

                case "D": {
                    if (!this.itmImportDomistic)
                    img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D.png");
                    break;
                }

                case "C": {
                    if (this.itmImportShipments)
                    img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C.png");
                    break;
                }
            }
        }
    }
    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_E = document.getElementById(this.FilterId_E);
            if (!this.itmImportDomistic)
            var img_I = document.getElementById(this.FilterId_I);
            var img_R = document.getElementById(this.FilterId_R);
            if (!this.itmImportDomistic)
            var img_D = document.getElementById(this.FilterId_D);
            if (this.itmImportShipments)
            var img_C = document.getElementById(this.FilterId_C);

            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E_g.png");
                    break;
                }

                case "I": {
                    if (!this.itmImportDomistic)
                    img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I_g.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R_g.png");
                    break;
                }

                case "D": {
                    if (!this.itmImportDomistic)
                    img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D_g.png");
                    break;
                }

                case "C": {
                    if (this.itmImportShipments)
                    img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C_g.png");
                    break;
                }
            }
        }
    }
    ApplySelectedStyle() {
        var img_E = document.getElementById(this.FilterId_E);
        if (!this.itmImportDomistic)
        var img_I = document.getElementById(this.FilterId_I);
        var img_R = document.getElementById(this.FilterId_R);
        if (!this.itmImportDomistic)
        var img_D = document.getElementById(this.FilterId_D);
        if (this.itmImportShipments)
        var img_C = document.getElementById(this.FilterId_C);

        if (img_E) {
            img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E_g.png");
            if (!this.itmImportDomistic)
            img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I_g.png");
            img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R_G.png");
            if (!this.itmImportDomistic)
            img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D_G.png");
            if (this.itmImportShipments)
            img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C_G.png");

            switch (this.SelectedValue) {
                case "E": {
                    img_E.setAttribute("src", "./_Resources/Images/Icons/Directions/E_w.png");
                    break;
                }

                case "I": {
                    if (!this.itmImportDomistic)
                    img_I.setAttribute("src", "./_Resources/Images/Icons/Directions/I_w.png");
                    break;
                }

                case "R": {
                    img_R.setAttribute("src", "./_Resources/Images/Icons/Directions/R_w.png");
                    break;
                }

                case "D": {
                    if (!this.itmImportDomistic)
                    img_D.setAttribute("src", "./_Resources/Images/Icons/Directions/D_w.png");
                    break;
                }

                case "C": {
                    if (this.itmImportShipments)
                    img_C.setAttribute("src", "./_Resources/Images/Icons/Directions/C_w.png");
                    break;
                }
            }
        }
    }

    SetVisibilityImportShipments() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "IMPORTSHIPMETNS") && !this.HideCustomsImport) {
            this.itmImportShipments = true;
            this.DirectionWidth = 168;
        }
        else {
            this.itmImportShipments = false;
            this.DirectionWidth = 140;
        }
    }

    SetVisibilityImportDomistic() {
        if (this.HideImportDomistic) {
            this.itmImportDomistic = true;
            this.DirectionWidth = 112;
        }
        //else {
          
        //}
    }

}
