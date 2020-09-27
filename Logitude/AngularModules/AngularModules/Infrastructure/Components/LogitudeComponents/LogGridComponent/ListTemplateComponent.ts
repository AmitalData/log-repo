import {Component, ElementRef, OnInit, ViewContainerRef,ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Locators/ObjectsLocator';

@Component({
    selector: 'list-template',
    template: `<div style="overflow: hidden; text-overflow: ellipsis;">
                
               <span><span style="text-overflow: ellipsis" [style.float]="RTL == true ? 'right' : 'left'" *ngIf="noComponent">{{rowData[fieldName]}}</span></span>
            
               </div>

`,
    inputs: ['htmlListComponentUrl', 'htmlListComponentName', 'fieldName', 'rowData', 'PassAdditionalData', 'AdditionalData','AdditionalDataCustom']
})

export class ListTemplateComponent implements OnInit {

    public rowData: any;
    public fieldName: any;
    public htmlListComponentUrl: string;
    public htmlListComponentName: string;
    public PassAdditionalData: boolean = false;
    public AdditionalData: any;
    public AdditionalDataCustom: any;
    public RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);

    constructor(private _elementRef: ElementRef, private _ViewContainerRef: ViewContainerRef, private CD: ChangeDetectorRef) {
        this.noComponent = false;
    }

    public noComponent: boolean;

    ngOnInit() {

        if (this.htmlListComponentName && this.htmlListComponentUrl) {
            this.noComponent = false;
            if (this.PassAdditionalData == true) {
                SessionLocator.DynamicLoader.Load(this.htmlListComponentUrl, this._ViewContainerRef)
                    .then((res) => {
                        //console.log("specific response: ", res);
                        res.instance.setVariables(this.rowData, this.fieldName, this.AdditionalData, this.AdditionalDataCustom);
                    });
            }
            else {
                SessionLocator.DynamicLoader.Load(this.htmlListComponentUrl, this._ViewContainerRef)
                    .then((res) => {
                        //console.log("specific response: ", res);
                        res.instance.setVariables(this.rowData, this.fieldName, this.AdditionalDataCustom);
                    });
            }
            
        }
        else {
            this.noComponent = true;
        }

    }
    
}
