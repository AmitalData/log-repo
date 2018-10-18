
import {Component, OnInit } from '@angular/core';

import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

import { UIProperties, UIProperty } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';

import { AppTool} from '../../../../Infrastructure/Tools';

import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './ErrorLogExceptionComponent.html',
    selector: 'ErrorLogExceptionComponent',

})


export class ErrorLogExceptionComponent extends BaseComponent implements OnInit {
    public DataContext = this;
    public ObjectTableName: string = "ErrorLog";
    Exception: string;
    StackTrace: string;
    MessageWidth: string;
    EntityPM: any;
    constructor(public entityArgs: EntityArgs) {
        super();

        if (window.innerWidth > 1380) {
            this.MessageWidth = "1380px";

        }
        else {
            this.MessageWidth = (window.innerWidth - 300).toString();
        }

    }

    ngOnInit() {

        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {

            this.Exception = this.EntityPM.Exception;
            this.StackTrace = this.EntityPM.StackTrace;
        }



    }

 
}

