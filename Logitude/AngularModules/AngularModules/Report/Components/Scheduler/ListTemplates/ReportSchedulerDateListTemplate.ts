import { Component, ChangeDetectorRef } from '@angular/core';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SchedulerExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TasksSchedulerPMService } from '../../../../Infrastructure/Services/StandardPMs/TasksSchedulerPMService';
import { TaskSchedulerItemClass } from '../../../../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerComponent';
import { ReportGroupList } from '../../../../Report/EntityLists/ReportGroupList';
import { ReportList } from '../../../../Report/EntityLists/ReportList';


@Component({
    moduleId: module.id,

    selector: 'ReportSchedulerDateListTemplate',
    templateUrl: './ReportSchedulerDateListTemplate.html',
})

export class ReportSchedulerDateListTemplate {

    public rowData: any;
    public fieldName: any;
    public dateValue: any;
    public Type: string;
    public AdditionalData: any;
    SchedulerType: string = '';
    ReportGroupList: ReportGroupList;
    ReportList: ReportList;
    myTasksSchedulerPMService: TasksSchedulerPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this.myTasksSchedulerPMService = new TasksSchedulerPMService();
    }

    setVariables(rowData: any, fieldName: string, myAdditionalData: any) {
        this.rowData = rowData;
        var MyFieldName = fieldName.split(';');
        this.fieldName = MyFieldName[0];
        this.AdditionalData = myAdditionalData;
        if (MyFieldName.length > 1) {
            this.SchedulerType = MyFieldName[1];
        }
        //this.ReportGroupList = additionalData.ReportGroupList;
        //this.ReportList = additionalData.ReportList;
        if (fieldName == "Duration") {
            var startDate = new Date(rowData["StartDateTime"]);
            var endDate = new Date(rowData["EndDateTime"]);

            var seconds = Math.abs(((endDate.getTime() - startDate.getTime()) / 1000)).toFixed(2); 
            if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
                this.dateValue = seconds + " sec";
            }
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

    EditTaskClicked() {
        this.myTasksSchedulerPMService.get(this.rowData["Id"]).subscribe(myResult => {
            if (myResult.Result) {
                var MyTask = new TaskSchedulerItemClass(myResult.Result, null, false);
                //var windowArgs: any = {};
                //windowArgs.ReportGroupList = this.ReportGroupList;
                //windowArgs.ReportList = this.ReportList;
                var logWindow = new LogitudeWindow();
                logWindow.Title = this.SchedulerType + " Scheduler Details";
                logWindow.DataContext = MyTask;
                logWindow.Width = 900;
                if (this.SchedulerType == "Report") {
                    logWindow.Height = 820;
                    //logWindow.WindowArgs = windowArgs;
                    logWindow.Show('./Report/Components/Scheduler/AddEditReportSchedulerComponent');
                    logWindow.WindowClosed.subscribe(s => {

                        this.CurrentSession.FireEvent({ Name: 'ReloadTasks' });

                    });
                }
            }
        });

    }


}
