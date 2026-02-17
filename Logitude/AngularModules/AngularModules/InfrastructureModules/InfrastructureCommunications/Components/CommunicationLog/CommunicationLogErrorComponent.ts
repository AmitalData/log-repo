
declare var System: any;
declare var window: any;


import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';

import {CommunicationLogPM} from '../../../../Common/EntityPMs/CommunicationLogPM';

import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';


import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    selector: 'CommunicationLogMessageBody',
    templateUrl: './CommunicationLogErrorComponent.html',

})

export class CommunicationLogErrorComponent extends BaseComponent implements OnInit {
    public EntityPM: CommunicationLogPM;
    public ExceptionMessage: string;
    MessageWidth: string;
    constructor(public entityArgs: EntityArgs) {
        super();

        if (window.innerWidth > 1380) {
            this.MessageWidth = "1380px";

        }
        else {
            this.MessageWidth = (window.innerWidth - 250).toString();
        }

    }

    ngOnInit() {

        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {

            this.ExceptionMessage = this.EntityPM.ExceptionMessage;
        }



    }






 



}






