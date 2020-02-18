import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {TasksSchedulerPM} from '../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {AppTool} from '../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SchedulerExtendedPMService} from '../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import {Component, }  from '@angular/core';
import { TaskReportSchedulerItemClass } from './TaskReportSchedulerComponent';
import { SchedulerDetails, ReportSchedulerDetails } from '../../DataContracts/SchedulerDetails';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditReportTaskSchedulerComponent.html',
})

export class AddEditReportTaskSchedulerComponent  {
    public EntityPM: TasksSchedulerPM;
    public DataContext: TaskReportSchedulerItemClass;
    public ObjectTableName: string = "TasksScheduler";
    public ValidationErrorsList: string[];
    schedulerExtendedPMService: SchedulerExtendedPMService;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
    }

    SetDataContext(DataContext: TaskReportSchedulerItemClass) {
        this.DataContext = DataContext["DataContext"];
        this.EntityPM = DataContext["DataContext"].EntityPM;
        this.EntityPM.Type = "Report";
        this.EntityPM.ProcedureCode = "Report";

        this.BuildSchedulerDetailsData();

        this.Clone();
        this.SetTigger(this.DataContext.TriggerType);
    }

    BuildSchedulerDetailsData() {
            if (this.EntityPM.SchedulerDetailsData) {
                this.SetReportSchedulerDetailsData(this.EntityPM.SchedulerDetailsData);
            }
            else if (this.EntityPM.Id) {
                //this.LoadSchedulerDetailsData();
            } else {
                var schedulerDetailsData = new SchedulerDetails();
                schedulerDetailsData.ReportDetails = new ReportSchedulerDetails();
                this.SetReportSchedulerDetailsData(schedulerDetailsData);
            }
    }

    SetReportSchedulerDetailsData(schedulerDetailsData: SchedulerDetails) {
        this.DataContext.SetReportSchedulerDetailsData(schedulerDetailsData);
        this.Clone();
    }

    StartTimeTabTitle: string = "One Time";

    private isDaily: boolean;
    get IsDaily() { return this.isDaily; }
    set IsDaily(newValue: boolean) {
        if (this.isDaily != newValue) {
            this.isDaily = newValue;
        }
    }

    private isWeekly: boolean;
    get IsWeekly() { return this.isWeekly; }
    set IsWeekly(newValue: boolean) {
        if (this.isWeekly != newValue) {
            this.isWeekly = newValue;
        }
    }

    private isMonthly: boolean;
    get IsMonthly() { return this.isMonthly; }
    set IsMonthly(newValue: boolean) {
        if (this.isMonthly != newValue) {
            this.isMonthly = newValue;
        }
    }
    
    SetTigger(triggerType: string) {
        switch (triggerType) {
            case "D":
                {
                    this.IsDaily = true;
                    this.IsWeekly = false;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "D";   
                    this.StartTimeTabTitle = "Daily";
                    break;
                }

            case "W":
                {
                    this.IsDaily = false;
                    this.IsWeekly = true;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "W";   
                    this.StartTimeTabTitle = "Weekly";
                    break;
                }

            case "M":
                {
                    this.IsDaily = false;
                    this.IsWeekly = false;
                    this.IsMonthly = true;
                    this.DataContext.TriggerType = "M";   
                    this.StartTimeTabTitle = "Monthly";
                    break;
                }

            default: {
                this.IsDaily = true;
                this.IsWeekly = false;
                this.IsMonthly = false;
                this.DataContext.TriggerType = "D";
                this.StartTimeTabTitle = "Daily";
                break;
            }
        }
    }

    NextButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);

        
        if (AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            errors.push(msg.replace("%FieldName", "Name"));
        }

        if (this.DataContext.StartDateTime == null) {
            errors.push(msg.replace("%FieldName", "Start Date Time"));
            console.log("error");
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.TriggerType)) {
            errors.push(msg.replace("%FieldName", "Trigger Type"));
        }

        else {
            switch (this.DataContext.TriggerType) {
                case "W":
                    {
                        if (!this.DataContext.Satarday && !this.DataContext.Sunday && !this.DataContext.Monday && !this.DataContext.Tuesday
                            && !this.DataContext.Wednesday && !this.DataContext.Thursday && !this.DataContext.Friday) {
                            errors.push(msg.replace("%FieldName", "Day"));
                        }
                        break;
                    }
                case "M":
                    {
                        break;
                    }
            }
        }


        if (this.EntityPM.Status == "In progress") {
            errors.push("The task is in progress. You are not allowed to edit it");
        }
  
    
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            return true;
        }
    }

    //LoadSchedulerDetailsData() {

    //    this.CurrentSession.StartBusyIndicator("Loading...");

    //    this.schedulerExtendedPMService.GetSchedulerDetailsById(this.EntityPM.Id).subscribe(myResult => {
    //        var myResponse: ServiceResponse = myResult;
    //        if (!myResponse.HasError) {
    //            this.SetReportSchedulerDetailsData(myResponse.Result);
    //        }

    //        else {
    //            this.ValidationErrorsList = myResponse.ErrorsArray;
    //            this.Clone();
    //        }

    //        this.CurrentSession.StopBusyIndicator();
    //    });
    //}

    SaveButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        if (this.DataContext.IsNew) {
            this.DataContext.ReportDetails.CreateDate = new Date();
            this.schedulerExtendedPMService.insert(this.EntityPM).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.EntityPM.IsDirty = false;
                    if (this.DataContext.fatherComponent) {
                        this.DataContext.fatherComponent.RefreshTasksSchedular(this.EntityPM);
                    }
                    return true;
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }

        else {
            if (this.EntityPM.IsDirty) {
                this.EntityPM.UpdatedBy = SessionLocator.LoggedUserPM.EnglishName;

                this.schedulerExtendedPMService.update(this.EntityPM).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        this.EntityPM = myResponse.Result;
                        this.EntityPM.IsDirty = false;
                        if (this.DataContext.fatherComponent) {
                            this.DataContext.fatherComponent.RefreshTasksSchedular(this.EntityPM);
                        }
                        return true;
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                return true;
            }
        }
    }


    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('StartDateTime');
        this.myCloner.AddField('MonthlyDay');
        this.myCloner.AddField('Satarday');
        this.myCloner.AddField('Sunday');
        this.myCloner.AddField('Monday');
        this.myCloner.AddField('Tuesday');
        this.myCloner.AddField('Wednesday');
        this.myCloner.AddField('Thursday');
        this.myCloner.AddField('Friday');
        this.myCloner.AddField('TriggerType');

        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
