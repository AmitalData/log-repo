import {Component,ChangeDetectorRef} from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { SchedulerExtendedPMService } from '../../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,

    selector: 'SchedulerDateListTemplate',
    templateUrl: './SchedulerDateListTemplate.html',
})

export class SchedulerDateListTemplate {

    public rowData: any;
    public fieldName: any;
    public dateValue: any;
    public Type: string;
    schedulerExtendedPMService: SchedulerExtendedPMService;
    constructor(private CD: ChangeDetectorRef) {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;

        if (fieldName == "Duration") {
            var startDate = new Date(rowData["StartDateTime"]);
            var endDate = new Date(rowData["EndDateTime"]);

            var seconds = (endDate.getTime() - startDate.getTime()) / 1000;
            if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
                this.dateValue = seconds + " Seconds";
            }
        }
        else if (fieldName == "Log") {
            this.dateValue = rowData["LogFirstLine"];
            this.Type = rowData["LogType"]; 
        }
        else {
            var pmDate = new Date(rowData[fieldName]);
            if (pmDate.getFullYear() > 1970) {
                this.dateValue = pmDate;
            }
        }
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    ShowFullLog() {
        SessionLocator.CurrentSession.StartBusyIndicator("Loading...");
        this.schedulerExtendedPMService.GetSchedulerHistoryLogs(this.rowData["Id"]).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                var windowArgs: any = {};
                windowArgs.TextValue = myResponse.Result.Log;
                windowArgs.DisplayMode = true;

                var wind = new LogitudeWindow();

                wind.Width = 960;
                wind.Height = 570;
                wind.WindowArgs = windowArgs;

                wind.Title = "";

                wind.Show("./Infrastructure/Component/LogitudeComponents/MultilineTextBoxWindow");
            }

            //else {
            //    this.ValidationErrorsList = myResponse.ErrorsArray;
            //}
           
            SessionLocator.CurrentSession.StopBusyIndicator();
        });
      
        
    }

}
