import { Component, ChangeDetectorRef } from '@angular/core';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { SchedulerExtendedPMService } from '../../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { DownloadManager } from '../../../../../Infrastructure/Utilities/DownloadManager';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { TasksSchedulerPMService } from '../../../../../Infrastructure/Services/StandardPMs/TasksSchedulerPMService';
import { TaskSchedulerItemClass } from '../../../../../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent';


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
    SchedulerType: string = '';
    schedulerExtendedPMService: SchedulerExtendedPMService;
    myTasksSchedulerPMService: TasksSchedulerPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
        this.myTasksSchedulerPMService = new TasksSchedulerPMService();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        var MyFieldName = fieldName.split(';');
        this.fieldName = MyFieldName[0];
        if (MyFieldName.length > 1) {
            this.SchedulerType = MyFieldName[1];
        }
        if (fieldName == "Duration") {
            var startDate = new Date(rowData["StartDateTime"]);
            var endDate = new Date(rowData["EndDateTime"]);

            var seconds = Math.abs(((endDate.getTime() - startDate.getTime()) / 1000)).toFixed(2); /*;*/
            if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
                this.dateValue = seconds + " sec";
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



    ViewLogFile() {
        let logDocumentId = this.rowData["LogDocumentId"];
        if (!AppTool.IsNullOrEmpty(logDocumentId)) {

            DownloadManager.DownloadPage(logDocumentId);
        }
    }

    EditTaskClicked() {
        this.myTasksSchedulerPMService.get(this.rowData["Id"]).subscribe(myResult => {
            if (myResult.Result) {
                var MyTask = new TaskSchedulerItemClass(myResult.Result, null, false);
                var logWindow = new LogitudeWindow();
                logWindow.Title = this.SchedulerType + " Scheduler Details";
                logWindow.DataContext = MyTask;
                logWindow.Height = (this.SchedulerType == "FTP" || this.SchedulerType == "SFTP") ? 820 : 750;
                logWindow.Width = 900;
                logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/AddEditTaskSchedulerComponent');
                logWindow.WindowClosed.subscribe(s => {

                    this.CurrentSession.FireEvent({ Name: 'ReloadTasks' });

                });
            }
        });

    }


}
