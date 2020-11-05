import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren } from '@angular/core';
import { AutomationPM } from '../../../../../Common/EntityPMs/AutomationPMExtended';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';


import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'SendInterfaceResult',
    templateUrl: './SendInterfaceResultComponent.html',
    inputs: [''],

})
export class SendInterfaceResultComponent extends BaseComponent implements OnInit {

   
    constructor() {
        super();

    }
    item: any;
    ngOnInit() {
      

    }


    Run(item:any) {

    }
}

