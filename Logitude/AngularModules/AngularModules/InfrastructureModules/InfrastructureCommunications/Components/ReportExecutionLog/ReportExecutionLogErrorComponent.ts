 declare var window: any;  
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit } from '@angular/core'; 
import { CommunicationLogPM } from '../../../../Common/EntityPMs/CommunicationLogPM'; 
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ReportExecutionLogPM } from '../../../../Common/EntityPMs/ReportExecutionLogPM';
 

@Component({

    selector: 'ReportExecutionLogMessageBody',
    templateUrl: './ReportExecutionLogErrorComponent.html',

})

export class ReportExecutionLogErrorComponent extends BaseComponent implements OnInit {
    public EntityPM: ReportExecutionLogPM;
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






