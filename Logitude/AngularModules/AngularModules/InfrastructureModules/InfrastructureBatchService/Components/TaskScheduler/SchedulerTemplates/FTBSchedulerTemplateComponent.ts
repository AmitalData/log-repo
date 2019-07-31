import {Component, ChangeDetectorRef} from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,

    selector: 'FTBSchedulerTemplateComponent',
    templateUrl: './FTBSchedulerTemplateComponent.html',
})

export class FTBSchedulerTemplateComponent {
    DataContext: any;
    public ObjectTableName: string = "TasksScheduler";
    constructor() {

    }


    LoadComponent(dataContext: any) {
        this.DataContext = dataContext;

    }

     

    ExtensionLostFocus(input: any) {

        if (this.DataContext.Extension && this.DataContext.Extension.startsWith("."))
            this.DataContext.Extension = this.DataContext.Extension.substring(1, this.DataContext.Extension.length);
        
    }

}
