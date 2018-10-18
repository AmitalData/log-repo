

import {Component, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationQueryListTemplate.html',
})

export class DeclarationQueryListTemplate {
    public rowData: any;
    public fieldName: any;

    constructor(private cd: ChangeDetectorRef) {

    }
    setVariables(rowData: any, fieldName: string) {
      
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.cd.detectChanges();
    }


}