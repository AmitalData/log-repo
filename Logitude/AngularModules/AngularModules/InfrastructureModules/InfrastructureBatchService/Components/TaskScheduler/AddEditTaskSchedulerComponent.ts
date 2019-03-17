
import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {TasksSchedulerPM} from '../../../../Infrastructure/EntityPMs/TasksSchedulerPM';

import {TaskSchedulerItemClass} from './TaskSchedulerComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SchedulerDetails, FTPSchedulerDetails} from '../../../../Infrastructure/DataContracts/SchedulerDetails';
import {SchedulerExtendedPMService} from '../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditTaskSchedulerComponent.html',
})

export class AddEditTaskSchedulerComponent  {
    public EntityPM: TasksSchedulerPM;
    public DataContext: TaskSchedulerItemClass;
    public ObjectTableName: string = "TasksScheduler";
    public OkBtnId: string;
    public ValidationErrorsList: string[];
    public GeneralAreaHeight: string = "200px";
    public SchedulerDetailsData: any;
    Type: string;

     schedulerExtendedPMService: SchedulerExtendedPMService;

    constructor() {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
    }

    SetDataContext(dataContext: TaskSchedulerItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.Type = this.EntityPM.Type;

        this.BuildSchedulerDetailsData();

        this.Clone();
        this.SetTigger(this.DataContext.TriggerType);

    }



    BuildSchedulerDetailsData() {
        if (this.Type == "FTP") {
            this.GeneralAreaHeight = "275px";
            if (this.EntityPM.SchedulerDetailsData) {
                this.SchedulerDetailsData = this.EntityPM.SchedulerDetailsData;
                this.SetSchedulerDetailsData();
            }
           else if (this.EntityPM.Id) {
                this.LoadSchedulerDetailsData();
            } else  {
                this.SchedulerDetailsData = new SchedulerDetails();
                this.SchedulerDetailsData.FTPDetails = new FTPSchedulerDetails();
                this.SetSchedulerDetailsData();
            }
        }
    }

  
    LoadSchedulerDetailsData() {

        SessionLocator.CurrentSession.StartBusyIndicator("Loading...");

        this.schedulerExtendedPMService.GetSchedulerDetailsById(this.EntityPM.Id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.SchedulerDetailsData = myResponse.Result;
                this.SetSchedulerDetailsData();
            }

            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }



    SetSchedulerDetailsData() {
        if (this.SchedulerDetailsData) {
            if (this.Type == "FTP" && this.SchedulerDetailsData.FTPDetails) {
                this.DataContext.SetFTPSchedulerDetails(this.SchedulerDetailsData.FTPDetails);
            }
        }
    }

    StartTimeTabTitle: string = "One Time";
   

    private isOneTime: boolean; 
    get IsOneTime() { return this.isOneTime; }
    set IsOneTime(newValue: boolean) {
        if (this.isOneTime != newValue) {
            this.isOneTime = newValue;
        }
    }

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
            case "O":
                {
                    this.IsOneTime = true;
                    this.IsDaily = false; 
                    this.IsWeekly = false;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "O";             
                    this.StartTimeTabTitle = "One Time";
                    break;
                }

            case "D":
                {
                    this.IsOneTime = false;
                    this.IsDaily = true;
                    this.IsWeekly = false;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "D";   
                    this.StartTimeTabTitle = "Daily";
                    break;
                }

            case "W":
                {
                    this.IsOneTime = false;
                    this.IsDaily = false;
                    this.IsWeekly = true;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "W";   
                    this.StartTimeTabTitle = "Weekly";
                    break;
                }

            case "M":
                {
                    this.IsOneTime = false;
                    this.IsDaily = false;
                    this.IsWeekly = false;
                    this.IsMonthly = true;
                    this.DataContext.TriggerType = "M";   
                    this.StartTimeTabTitle = "Monthly";
                    break;
                }

            default: {
                this.IsOneTime = true;
                this.IsDaily = false;
                this.IsWeekly = false;
                this.IsMonthly = false;
                this.DataContext.TriggerType = "O";
                this.StartTimeTabTitle = "One Time";
                break;
            }
        }
    }

    OKButtonClicked() {


        if (this.Type == "FTP") {
            this.DataContext.ServiceClassName = "ServiceClassName";
            if (this.SchedulerDetailsData.FTPDetails.Host != this.DataContext.Host) {
                this.SchedulerDetailsData.FTPDetails.Host = this.DataContext.Host;
                this.EntityPM.IsDirty = true;
            }
        }


        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        
        if (AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            errors.push(msg.replace("%FieldName", "Name"));
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.Description)) {
            errors.push(msg.replace("%FieldName", "Description"));
        }
        
        if (this.DataContext.StartDateTime == null) {
            errors.push(msg.replace("%FieldName", "Start Date Time"));
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.ServiceClassName)) {
            errors.push(msg.replace("%FieldName", "Service Class Name"));
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
                        if (this.DataContext.MonthlyDay == 0) {
                            errors.push(msg.replace("%FieldName", "Day of a month"));
                        }
                        break;
                    }
            }
        }



        this.EntityPM.SchedulerDetailsData = this.SchedulerDetailsData;
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
          

            if (this.DataContext.IsNew) {

                this.schedulerExtendedPMService.insert(this.EntityPM).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        SessionLocator.CurrentSession.CloseCurrentWindow();
                        //this.EntityPM = myResult;
                        //this.EntityPM.IsDirty = false;
                        this.DataContext.fatherComponent.GetTasksSchedular();
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    SessionLocator.CurrentSession.StopBusyIndicator();
                }); 
            }

            else {
                if (this.EntityPM.IsDirty) {
                    this.EntityPM.UpdatedBy = SessionLocator.LoggedUserId;

                    this.schedulerExtendedPMService.update(this.EntityPM).subscribe(myResult => {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {
                            //this.EntityPM.IsDirty = false;
                            SessionLocator.CurrentSession.CloseCurrentWindow();
                            this.DataContext.fatherComponent.GetTasksSchedular();
                        }

                        else {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        SessionLocator.CurrentSession.StopBusyIndicator();
                    });
                }

                else {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    SessionLocator.CurrentSession.CloseCurrentWindow();
                }
            }            
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('ServiceClassName');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('StartDateTime');
        this.myCloner.AddField('RepeatInMinutes');
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