import {Component, ChangeDetectorRef} from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,

    selector: 'TaskSchedulerTemplateComponent',
    templateUrl: './TaskSchedulerTemplateComponent.html',
})

export class TaskSchedulerTemplateComponent {
    DataContext: any;
    public ObjectTableName: string = "TasksScheduler";
    constructor() {

    }


    LoadComponent(dataContext: any) {
        this.DataContext = dataContext;

    }

}
