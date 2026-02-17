import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AnalyzeQueuePM} from '../../../../Infrastructure/EntityPMs/AnalyzeQueuePM';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './AnalyzeQueueErrorsTabComponent.html',
})

export class AnalyzeQueueErrorsTabComponent extends BaseComponent {
    public EntityPM: AnalyzeQueuePM;
    public DataContext: AnalyzeQueueErrorsTabComponent = this;
    public ObjectTableName: string = "AnalyzeQueue";

    constructor(public args: EntityArgs) {
        super();
        this.EntityPM = args.EntityPM;
    }

    get ErrorMessage() {
        if (this.EntityPM != null) {
            return this.EntityPM.ErrorMessage;
        }
        return null;
    }
    set ErrorMessage(value: string) { this.EntityPM.ErrorMessage = value; }

    get StackTrace() {
        if (this.EntityPM != null) {
            return this.EntityPM.StackTrace;
        }
        return null;
    }
    set StackTrace(value: string) { this.EntityPM.StackTrace = value; }

}