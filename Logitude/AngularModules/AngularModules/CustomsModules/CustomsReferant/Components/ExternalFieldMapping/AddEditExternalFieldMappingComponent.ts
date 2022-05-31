import { Component, OnInit } from '@angular/core';
import { ExternalFieldMappingPM } from 'Customs/EntityPMs/ExternalFieldMappingPM';
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { AppTool } from 'Infrastructure/Tools';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';


@Component({
    templateUrl: './AddEditExternalFieldMappingComponent.html',
})
export class AddEditExternalFieldMappingComponent
    extends BaseComponent
    implements OnInit{

        public ObjectTableName: string = "Customs.ExternalFieldMapping";
        public EntityPM: ExternalFieldMappingPM;
        isWindowMode: boolean = false;
        isNewRecord: boolean = false;


        constructor(public entityArgs: EntityArgs) {
            super();
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM) || !(entityArgs.EntityPM instanceof ExternalFieldMappingPM)) {
                this.EntityPM = new ExternalFieldMappingPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
                this.isNewRecord = true;

            } else {
                this.EntityPM = this.entityArgs.EntityPM;
            }
        }

        ngOnInit() {
        }
    }