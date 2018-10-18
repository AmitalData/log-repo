declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeCopyPM} from '../../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceArgs} from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { FormGroup, FormBuilder} from '@angular/forms';

@Component({
    moduleId: module.id,
    selector: 'SharedLogisticsTab',
    templateUrl: './PrintingOptionsComponent.html',
})

export class PrintingOptionsComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public myForm: FormGroup;
    public DocumentTypeCopies: DocumentTypeCopyPM[];
     SelectedCopy: DocumentTypeCopyPM;
    constructor(public entityArgs: EntityArgs, fb: FormBuilder) {
        super();
        this.myForm = fb.group({});

    }

    ngOnInit() {

        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.Run();

        }



    }



    Run() {

        if (this.EntityPM.DocumentTypeCopies) {
            this.DocumentTypeCopies = this.EntityPM.DocumentTypeCopies;
            this.SelectedCopy = this.DocumentTypeCopies.filter(d => d.Id == this.EntityPM.LimitedPrintCopyId)[0];
        }

      


    }




    DocumentTypeCopyValueChanged(Copy:any) {

        this.EntityPM.LimitedPrintCopyId = Copy.Id;
    }


 



}






