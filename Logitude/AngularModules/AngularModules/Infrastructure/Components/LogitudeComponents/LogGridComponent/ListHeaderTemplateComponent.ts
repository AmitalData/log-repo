import {SessionLocator} from '../../../Utilities/SessionLocator';
import {Component, ElementRef, OnInit, ViewContainerRef} from '@angular/core';

@Component({
    selector: 'list-header-template',
    template: `<!--<div>-->
                
               <span><span style="text-overflow: ellipsis" *ngIf="noComponent">{{col.Display}}</span></span>
            
               <!--</div>-->`,
    inputs: ['colDef']
})

export class ListHeaderTemplateComponent implements OnInit {

    public colDef: any;

    constructor(private _elementRef: ElementRef, private _ViewContainerRef: ViewContainerRef) {
        this.noComponent = false;
    }

    public noComponent: boolean;

    ngOnInit() {

        if (this.colDef.ColumnHeaderTemplateName && this.colDef.ColumnHeaderTemplateName) {
            this.noComponent = false;
            //console.log("htmlHeaderComponent is -----> ", this.htmlHeaderComponent);
            if (this.colDef.ColumnHeaderTemplateName === "TransportModeHeaderTemplate") {

                SessionLocator.DynamicLoader.Load("./Shipment/Components/ListHeaderTemplates/TransportModeListHeaderTemplate", this._ViewContainerRef);

            }
            if (this.colDef.ColumnHeaderTemplateName === "DirectionHeaderTemplate") {

                SessionLocator.DynamicLoader.Load("./Shipment/Components/ListHeaderTemplates/DirectionListHeaderTemplate", this._ViewContainerRef);

            }

            if (this.colDef.ColumnHeaderTemplateName === "ARInvoiceIsPrintedHeaderTemplate") {
                SessionLocator.DynamicLoader.Load("./Invoice/Components/ListHeaderTemplates/ARInvoiceIsPrintedHeaderTemplate", this._ViewContainerRef);
            }

            if (this.colDef.ColumnHeaderTemplateName === "ARInvoiceSentHeaderTemplate") {
                SessionLocator.DynamicLoader.Load("./Invoice/Components/ListHeaderTemplates/ARInvoiceSentHeaderTemplate", this._ViewContainerRef);
            }
        }
        else {
            this.noComponent = true;
        }

    }

}