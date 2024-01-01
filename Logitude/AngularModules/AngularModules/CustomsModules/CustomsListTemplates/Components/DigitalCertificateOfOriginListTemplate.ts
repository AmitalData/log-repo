import {Component, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './DigitalCertificateOfOriginListTemplate.html',
})

export class DigitalCertificateOfOriginListTemplate {

    public rowData: any;
    public fieldName: any;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.CD.detectChanges();
    }
}