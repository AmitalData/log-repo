import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({

    template: `<div style="text-indent: 10px; overflow: hidden; text-overflow: ellipsis;float:left;">
                      {{MyLabel}}
               </div>
            `
})

export class CustomReferenceListTemplate {

    public rowData: any;
    public fieldName: any;
    public MyLabel: string = "";
    constructor(private CD: ChangeDetectorRef) {

    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.MyLabel = "";
        
        if (!AppTool.IsNullOrEmpty(this.rowData['CustomerReference1'])) {
            this.MyLabel = this.rowData['CustomerReference1'];
        }

        if (AppTool.IsNullOrEmpty(this.rowData['CustomerReference1']) && !AppTool.IsNullOrEmpty(this.rowData['CustomerReference2'])) {
            this.MyLabel = this.rowData['CustomerReference2'];
        }
        if (!AppTool.IsNullOrEmpty(this.rowData['CustomerReference1']) && !AppTool.IsNullOrEmpty(this.rowData['CustomerReference2'])) {
            this.MyLabel = ' / ' + this.rowData['CustomerReference2'];
        }
        


        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}
