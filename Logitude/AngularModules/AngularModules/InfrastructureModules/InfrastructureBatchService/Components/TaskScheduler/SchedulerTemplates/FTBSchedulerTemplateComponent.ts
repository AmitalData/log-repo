import {Component, ChangeDetectorRef} from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../../Infrastructure/Tools';

@Component({
    

    selector: 'FTBSchedulerTemplateComponent',
    templateUrl: './FTBSchedulerTemplateComponent.html',
    inputs: ['DataContext','BasicDisplayMode']
})

export class FTBSchedulerTemplateComponent {
    public DataContext: any;
    public BasicDisplayMode: boolean = false;
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
