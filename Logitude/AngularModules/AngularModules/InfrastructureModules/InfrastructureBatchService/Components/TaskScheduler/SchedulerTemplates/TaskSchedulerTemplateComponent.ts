
import {Component, ChangeDetectorRef} from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
@Component({
    moduleId: module.id,

    selector: 'TaskSchedulerTemplateComponent',
    templateUrl: './TaskSchedulerTemplateComponent.html',
})

export class TaskSchedulerTemplateComponent {
    DataContext: any;
    public ObjectTableName: string = "TasksScheduler";
    public ProcedureCode: string = "";
    constructor(private CD: ChangeDetectorRef) {

    };

    
    LoadComponent(dataContext: any) {
        this.DataContext = dataContext;
        this.ProcedureCode = this.DataContext["ProcedureCode"];
    }

    ChooseCodeClicked() {
      
        var logitudeWindow: LogitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.Title = "Choose Procedure Code";
        logitudeWindow.Show("./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/SchedulerTemplates/ChooseProcedureCodeComponent");
        logitudeWindow.WindowClosed.subscribe((event: any) => {
            this.DataContext["ProcedureCode"] = event;
            this.ProcedureCode = event;
            var isDestroyed: boolean = this.CD['destroyed'];
            if (!isDestroyed) {
                this.CD.detectChanges();
            }
        });

    }
   
}
