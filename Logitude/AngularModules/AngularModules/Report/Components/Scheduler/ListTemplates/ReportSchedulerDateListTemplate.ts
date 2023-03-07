import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TasksSchedulerPMService } from '../../../../Infrastructure/Services/StandardPMs/TasksSchedulerPMService';
import { ReportGroupList } from '../../../../Report/EntityLists/ReportGroupList';
import { ReportList } from '../../../../Report/EntityLists/ReportList';
import { TaskReportSchedulerItemClass } from '../TaskReportSchedulerComponent';

@Component({
    
    selector: 'ReportSchedulerDateListTemplate',
    templateUrl: './ReportSchedulerDateListTemplate.html',
})

export class ReportSchedulerDateListTemplate {
    public rowData: any;
    public fieldName: any;
    public dateValue: any;
    public Type: string;
    public Frequency: string;
    public Recepients: string;
    SchedulerType: string = '';
    ReportGroupList: ReportGroupList;
    ReportList: ReportList;
    myTasksSchedulerPMService: TasksSchedulerPMService;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(private CD: ChangeDetectorRef) {
        this.myTasksSchedulerPMService = new TasksSchedulerPMService();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        var MyFieldName = fieldName.split(';');
        this.fieldName = MyFieldName[0];
        if (MyFieldName.length > 1) {
            this.SchedulerType = MyFieldName[1];
        }
        if (fieldName == "TriggerType") {
            this.SetTaskFrequency();
        }
        if (fieldName == "Recepients") {
            this.SetTaskRecepients();
        }
        if (fieldName == "Duration") {
            this.SetTaskDuration();
        }
        else {
            this.SetTaskDefaultDate(fieldName);
        }
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    EditTaskClicked() {
        this.myTasksSchedulerPMService.get(this.rowData["Id"]).subscribe((myResult:any) => {
            if (myResult.Result) {
                var MyTask = new TaskReportSchedulerItemClass(myResult.Result, null, false);
                this.CurrentSession.FireEvent({ Name: 'IsEditReportScheduler', DataContext: MyTask })
            }
        });
    }

    SetTaskFrequency() {
        if (this.rowData["TriggerType"] == 'D')
            this.Frequency = "Daily";
        else if (this.rowData["TriggerType"] == 'M')
            this.Frequency = "Monthly";
        else {
            var numberOfDays = 0;
            if (this.rowData["Satarday"])
                numberOfDays += 1;
            if (this.rowData["Sunday"])
                numberOfDays += 1;
            if (this.rowData["Monday"])
                numberOfDays += 1;
            if (this.rowData["Tuesday"])
                numberOfDays += 1;
            if (this.rowData["Wednesday"])
                numberOfDays += 1;
            if (this.rowData["Thursday"])
                numberOfDays += 1;
            if (this.rowData["Friday"])
                numberOfDays += 1;
            switch (numberOfDays) {
                case 1:
                    this.Frequency = "Weekly";
                    break;
                case 7:
                    this.Frequency = "Daily";
                    break;
                default:
                    this.Frequency = numberOfDays + " Days in the week";
            }
        }     
    }

    SetTaskRecepients() {
        var toRecepients = this.rowData["Recepients"];
        var allRecepients = toRecepients.split(';');

        toRecepients = "";
        allRecepients.forEach(rec => {
            toRecepients += rec.split('@')[0] + ", ";
        });

        toRecepients = toRecepients.substring(0, toRecepients.length-2); //Remove last ', '
        this.Recepients = toRecepients;
    }

    SetTaskDuration() {
        var startDate = new Date(this.rowData["StartDateTime"]);
        var endDate = new Date(this.rowData["EndDateTime"]);

        var seconds = Math.abs(((endDate.getTime() - startDate.getTime()) / 1000)).toFixed(2);
        if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
            this.dateValue = seconds + " sec";
        }
    }

    SetTaskDefaultDate(fieldName) {
        var pmDate = new Date(this.rowData[fieldName]);
        if (pmDate.getFullYear() > 1970) {
            this.dateValue = this.rowData[fieldName];
        }
    }
}
