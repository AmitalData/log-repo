declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceArgs} from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { FormGroup, FormBuilder} from '@angular/forms';

@Component({
    selector: 'LogBoxTabComponent',
    moduleId: module.id,
    templateUrl: './LogBoxTabComponent.html', 
})

export class LogBoxTabComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public myForm: FormGroup;
    constructor(public entityArgs: EntityArgs , fb: FormBuilder) {
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

      

  


    }

  







}






