declare var System: any;
declare var window: any;
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { DocumentTypePM } from '../../../../../Common/EntityPMs/DocumentTypePM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceArgs } from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import { UIProperty, UIProperties } from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AppTool } from 'Infrastructure/Tools';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'SharedLogisticsTab',

    templateUrl: './SharedLogisticsTabComponent.html',
})

export class SharedLogisticsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public myForm: FormGroup;
    public IsCustomerViewVisible: boolean;

    constructor(public entityArgs: EntityArgs, fb: FormBuilder) {
        super();
        this.myForm = fb.group({});
        this.IsCustomerViewVisible = FeatureLocator.HasFeaturePermession("General", "SHLOGDIGITALPORTAL");
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) this.Run();
    }

    Run() {

    }


}






