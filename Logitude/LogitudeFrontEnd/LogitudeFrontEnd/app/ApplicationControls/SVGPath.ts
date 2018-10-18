import {Component, OnInit} from 'angular2/core';

@Component({
    selector: 'SVGPath',
    inputs: ['Name', 'Width', 'Height', 'IsSelected'],
    template: '<img [style.width.px]="Width" [style.height.px]="Height" src="{{Source}}" style="position:absolute; left: 10px; top:0; bottom:0; margin:auto;" />',
})

export class SVGPath implements OnInit {

    public Name: string;
    public Width: number;
    public Height: number;
    public Source: string;
    public IsHover: boolean = false;
    public IsSelected: boolean = false;
    constructor() {

    }

    ngOnInit() {

        if (this.Name != null && this.Name !== undefined) {


         
            switch (this.Name) {                
                case "General.MH.Operations":
                    {                        
                        this.Width = 19;
                        this.Height = 18;

                        if (this.IsSelected) {
                            //console.log('sssssssssssssssss');
                            this.Source = "SVG/White/Operations.svg";
                        }

                        else {
                            //console.log('nnnnnnnnnnnnnnnn');
                            this.Source = "SVG/Colored/Operations.svg";
                        }

                        break;
                    }

                case "General.MH.CRM": {
                    this.Width = 18.26;
                    this.Height = 17.64;
                    this.Source = "SVG/Colored/Report.svg";
                    break;
                }

                case "General.MH.Quotes": {
                    this.Width = 17.89;
                    this.Height = 17.89;
                    this.Source = "SVG/Colored/Pen.svg";
                    break;
                }

                case "S":
                case "General.MH.Accounting": {
                    this.Width = 16.86;
                    this.Height = 19;
                    this.Source = "SVG/Colored/S.svg";
                    break;
                }

                case "General.MH.Maintenance": {
                    this.Width = 17.92;
                    this.Height = 17.92;
                    this.Source = "SVG/Colored/Maintenance.svg";
                    break;
                }

                case "notes": {

                    if (this.Width == null) {
                        this.Width = 16;
                    }

                    if (this.Height == null) {
                        this.Height = 13;
                    }

                    break;
                }

                case "x": {
                    if (this.Width == null) {
                        this.Width = 10.67;
                    }

                    if (this.Height == null) {
                        this.Height = 10.68;
                    }

                    break;
                }

                default: {
                    this.Width = 19;
                    this.Height = 18.09;
                    this.Source = "SVG/Colored/Person.svg";
                    break;
                }
            }
        }

    }
}