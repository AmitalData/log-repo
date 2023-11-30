 import {Component, ElementRef, OnInit, ViewContainerRef,ChangeDetectorRef, ChangeDetectionStrategy, Input} from '@angular/core';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {ObjectsLocator} from '../../../Locators/ObjectsLocator';
 

@Component({
    selector: 'list-template',
    changeDetection: ChangeDetectionStrategy.OnPush,
    template: `<div style="overflow: hidden; text-overflow: ellipsis;">
                
               <span>
                 <div [attr.data-cy]="DataCy +'_Text'" style="text-overflow: ellipsis" [style.float]="RTL == true ? 'right' : 'left'" *ngIf="noComponent" innerHTML="{{ rowData[fieldName] | highlight : SearchTerm}}">
                     <!--<span style="text-overflow: ellipsis" [style.float]="RTL == true ? 'right' : 'left'" *ngIf="noComponent"> 
                           {{rowData[fieldName]}} 
                    </span>-->
                </div>
              </span>

             </div>

`,
    inputs: ['htmlListComponentUrl', 'htmlListComponentName', 'fieldName', 'rowData', 'PassAdditionalData', 'AdditionalData', 'AdditionalDataCustom', 'SearchTerm', 'EntityChangedData', 'DataCy']
})

export class ListTemplateComponent implements OnInit {

    public DataCy: string;
    public rowData: any;
    public fieldName: any;
    public htmlListComponentUrl: string;
    public htmlListComponentName: string;
    public PassAdditionalData: boolean = false;
    public AdditionalData: any;
    public AdditionalDataCustom: any;
    public SearchTerm: string;
    public RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
    public LoadedComponent: any;
    @Input('markChecked') set markChecked(mark: boolean) {
        if(mark)
            this.CD.markForCheck();
    }

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
                        this.LoadedComponent = res.instance;
                        res.instance.setVariables(this.rowData, this.fieldName, this.AdditionalData, this.AdditionalDataCustom);
                    });
            }
            else {
                SessionLocator.DynamicLoader.Load(this.htmlListComponentUrl, this._ViewContainerRef)
                    .then((res) => {
                        //console.log("specific response: ", res);
                        this.LoadedComponent = res.instance;
                        res.instance.setVariables(this.rowData, this.fieldName, this.AdditionalDataCustom);
                    });
            }

        }
        else {
            this.noComponent = true;
        }

        //this.CD.detectChanges();
    }

    public get EntityChangedData() {
        return;// this.test;
    }
    public set EntityChangedData(newValue: any) {
        this.rowData = newValue;
        this.LoadData();
        this.CD.detectChanges();
    }

    private LoadData() {
        if (this.LoadedComponent) {
            this.LoadedComponent.setVariables(this.rowData, this.fieldName, this.AdditionalData, this.AdditionalDataCustom);
        }
    }

}
