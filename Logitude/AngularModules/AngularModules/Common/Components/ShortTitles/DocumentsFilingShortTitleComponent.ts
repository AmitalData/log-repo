import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentsFilingPM} from '../../EntityPMs/DocumentsFilingPM';

@Component({
    moduleId: module.id,
    templateUrl: "./DocumentsFilingShortTitleComponent.html",
})

export class DocumentsFilingShortTitleComponent {
    public EntityPM: DocumentsFilingPM;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            //this.BuildComponent();
        }
    }

    get Code() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.Code;
        }

        return myResult;
    }

    get DocumentTypeName() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.DocumentTypeName;
        }

        return myResult;
    }

    get IsDigitallySigned() {
        var myResult: boolean = false;
        // silver to lower
        if (this.EntityPM != null) {
            myResult = this.EntityPM.IsDigitallySigned;
        }

        return myResult;
    }

    get FileExtension() {
        var myResult: string = null;

        if (this.EntityPM != null) {
            myResult = this.EntityPM.FileExtension;
        }

        return myResult;
    }

  

}