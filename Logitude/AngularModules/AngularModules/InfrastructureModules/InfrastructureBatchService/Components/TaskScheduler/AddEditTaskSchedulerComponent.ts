

import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {TasksSchedulerPM} from '../../../../Infrastructure/EntityPMs/TasksSchedulerPM';

import {TaskSchedulerItemClass} from './TaskSchedulerComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SchedulerDetails, FTPSchedulerDetails} from '../../../../Infrastructure/DataContracts/SchedulerDetails';
import {SchedulerExtendedPMService} from '../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {Component, OnInit, ChangeDetectorRef, QueryList, ViewChild, ViewContainerRef}  from '@angular/core';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

@Component({
    
    templateUrl: './AddEditTaskSchedulerComponent.html',
})

export class AddEditTaskSchedulerComponent  {
    public EntityPM: TasksSchedulerPM;
    public DataContext: TaskSchedulerItemClass;
    public ObjectTableName: string = "TasksScheduler";
    public OkBtnId: string;
    public ValidationErrorsList: string[];
    public GeneralAreaHeight: string = "200px";
    schedulerExtendedPMService: SchedulerExtendedPMService;
    IsEnableSaveButton: boolean = false;
    ShowDateFields: boolean = false;
    ShowRunTaskNowButton: boolean = true;
    @ViewChild('GeneralSectionLocation', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    private CurrentSession = SessionLocator.SelectedSession;
    private GeneralTemplateComponent: any = null;
    constructor() {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
        if (FeatureLocator.HasFeaturePermession("TasksScheduler", "UPDATE")) this.IsEnableSaveButton = true;
    }

    SetDataContext(dataContext: TaskSchedulerItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ShowDateFields = this.EntityPM?.ProcedureCode === "InvoiceApiQueryTask";
        this.ShowRunTaskNowButton = SessionLocator.TenantPM.AccountingActivated && !(this.DataContext.Type === "FTP" || this.DataContext.Type === "SFTP");
        if (this.ShowDateFields) {
            this.RunNowFromDate = DateTool.GetCurrentDateAsUtc();
            this.RunNowFromDate.setDate(this.RunNowFromDate.getDate() - 1);
            this.RunNowToDate = DateTool.GetCurrentDateAsUtc();
        }    
        this.BuildSchedulerDetailsData();

        this.Clone();
        this.SetTigger(this.DataContext.TriggerType);

        this.RunComponent();
    }




    private isLoaderReady: boolean = false;

    private RunComponent() {
        if (this.viewContainerRef) {
            var componentName: string = (this.DataContext.Type == "FTP" || this.DataContext.Type == "SFTP") ? "FTBSchedulerTemplateComponent" :"TaskSchedulerTemplateComponent";
            SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/SchedulerTemplates/" + componentName, this.viewContainerRef)
                    .then(cmpRef => {
                        this.GeneralTemplateComponent = cmpRef.instance;
                        this.GeneralTemplateComponent.LoadComponent(this.DataContext);
                        this.isLoaderReady = true;
       
                    });
            }

            else {
                this.RunComponentTimer();
            }
        
    }


    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }




    BuildSchedulerDetailsData() {
        if (this.DataContext.Type == "FTP" || this.DataContext.Type == "SFTP") {
            this.GeneralAreaHeight = "310px";
            if (this.EntityPM.SchedulerDetailsData) {
                this.SetSchedulerDetailsData(this.EntityPM.SchedulerDetailsData);
            }
            else if (this.EntityPM.Id) {
                this.LoadSchedulerDetailsData();
            } else {
                var schedulerDetailsData = new SchedulerDetails();
                schedulerDetailsData.FTPDetails = new FTPSchedulerDetails();
                schedulerDetailsData.FTPDetails.IsSFTP = (this.EntityPM.Type == "SFTP" ? true : false);
                this.SetSchedulerDetailsData(schedulerDetailsData);
            }
        }
    }

  
    LoadSchedulerDetailsData() {

        this.CurrentSession.StartBusyIndicator("Loading...");

        this.schedulerExtendedPMService.GetSchedulerDetailsById(this.EntityPM.Id).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                this.SetSchedulerDetailsData(myResponse.Result);
            }

            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.Clone();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }



    SetSchedulerDetailsData(schedulerDetailsData: SchedulerDetails) {
        this.DataContext.SetSchedulerDetailsData(schedulerDetailsData);
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
    private runNowFromDate: Date;

    public get RunNowFromDate() { return this.runNowFromDate; }
    public set RunNowFromDate(value: Date) { if (this.runNowFromDate != value) this.runNowFromDate = value; }

 
    private runNowToDate: Date;
    get RunNowToDate() { return this.runNowToDate; }
    set RunNowToDate(newValue: Date) {
        if (this.runNowToDate != newValue) {
            this.runNowToDate = newValue;
        }
    }
    SetTigger(triggerType: string) {
        switch (triggerType) {
            //case "O":
            //    {
            //        this.IsOneTime = true;
            //        this.IsDaily = false; 
            //        this.IsWeekly = false;
            //        this.IsMonthly = false;
            //        this.DataContext.TriggerType = "O";             
            //        this.StartTimeTabTitle = "One Time";
            //        break;
            //    }

            case "D":
                {
                    //this.IsOneTime = false;
                    this.IsDaily = true;
                    this.IsWeekly = false;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "D";   
                    this.StartTimeTabTitle = "Daily";
                    break;
                }

            case "W":
                {
                    //this.IsOneTime = false;
                    this.IsDaily = false;
                    this.IsWeekly = true;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "W";   
                    this.StartTimeTabTitle = "Weekly";
                    break;
                }

            case "M":
                {
                    //this.IsOneTime = false;
                    this.IsDaily = false;
                    this.IsWeekly = false;
                    this.IsMonthly = true;
                    this.DataContext.TriggerType = "M";   
                    this.StartTimeTabTitle = "Monthly";
                    break;
                }

            default: {
                //this.IsOneTime = true;
                this.IsDaily = true;
                this.IsWeekly = false;
                this.IsMonthly = false;
                //this.DataContext.TriggerType = "O";
                //this.StartTimeTabTitle = "One Time";
                this.DataContext.TriggerType = "D";
                this.StartTimeTabTitle = "Daily";
                break;
            }
        }
    }



    private ValidateHost() {
        var isValid = false;

        if (!AppTool.IsNullOrEmpty(this.DataContext.Host)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.DataContext.Host.match(ipformat)) {
                isValid = true;
            }
        }

        return isValid
    }

    OKButtonClicked() {


        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");


        if (this.DataContext.Type == "FTP" || this.DataContext.Type == "SFTP") {
            this.DataContext.ProcedureCode = (this.DataContext.Type == "FTP" ? "FTPSchedulerTask" : "SFTPSchedulerTask");


            if (AppTool.IsNullOrEmpty(this.DataContext.UserName)) errors.push(msg.replace("%FieldName", "UserName"));
            if (AppTool.IsNullOrEmpty(this.DataContext.Password)) errors.push(msg.replace("%FieldName", "Password"));
            if (AppTool.IsNullOrEmpty(this.DataContext.Subject)) errors.push(msg.replace("%FieldName", "Subject"));
            if (AppTool.IsNullOrEmpty(this.DataContext.From)) errors.push(msg.replace("%FieldName", "From"));

            if (AppTool.IsNullOrEmpty(this.DataContext.Host)) errors.push(msg.replace("%FieldName", "Host"));
           // else {
               // var isValid = this.ValidateHost();
               // if (!isValid) errors.push("Invalid Host");
            //}

        }

        Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);

        
        if (AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            errors.push(msg.replace("%FieldName", "Name"));
        }
        if (this.DataContext.RepeatInMinutes  != null) {
            if (this.DataContext.RepeatInMinutes < 5) {

                errors.push("The lowest value you can add in Repeat in Minutes field is 5");
            }
        }
        //if (AppTool.IsNullOrEmpty(this.DataContext.Description)) {
        //    errors.push(msg.replace("%FieldName", "Description"));
        //}
        
        if (this.DataContext.StartDateTime == null) {
            errors.push(msg.replace("%FieldName", "Start Date Time"));
            console.log("error");
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.ProcedureCode)) {
            errors.push(msg.replace("%FieldName", "Procedure Code"));
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
                        //if (this.DataContext.MonthlyDay == 0) {
                        //    errors.push(msg.replace("%FieldName", "Day of a month"));
                        //}
                        break;
                    }
            }
        }


        if (this.EntityPM.Status == "In progress") {
            errors.push("The task is in progress. You are not allowed to edit it");// the start time field
        }
  
    
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {

            //if (this.EntityPM.Type == "FTP" || this.EntityPM.Type == "SFTP") this.EntityPM.SchedulerDetailsData = this.DataContext.SchedulerDetailsData;

            this.CurrentSession.StartBusyIndicatorSaving();
          

            if (this.DataContext.IsNew) {

                this.schedulerExtendedPMService.insert(this.EntityPM).subscribe((myResult: ServiceResponse) => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        this.CurrentSession.CloseCurrentWindow();
                        this.EntityPM = myResponse.Result;
                        this.EntityPM.IsDirty = false;
                        if (this.DataContext.fatherComponent) {
                            this.DataContext.fatherComponent.RefreshTasksSchedular(this.EntityPM);
                        }
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

                    this.schedulerExtendedPMService.update(this.EntityPM).subscribe((myResult: ServiceResponse) => {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {
                            this.EntityPM = myResponse.Result;
                            this.EntityPM.IsDirty = false;
                            this.CurrentSession.CloseCurrentWindow();
                            if (this.DataContext.fatherComponent) {
                                this.DataContext.fatherComponent.RefreshTasksSchedular(this.EntityPM);
                            }
                        }

                        else {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        this.CurrentSession.StopBusyIndicator();
                    });
                }

                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();
                }
            }            
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    DatePickerFromValueChange(value: Date) {
        if (value != null) {
            this.RunNowFromDate = value;
        }
    }
    DatePickerToValueChange(value: Date) {
        if (value != null) {
            this.RunNowToDate = value;
        }
    }
    RunNowButtonClicked() {
        
        this.CurrentSession.StartBusyIndicatorLoading();

        this.schedulerExtendedPMService.RunNowButtonClicked(this.EntityPM.Id ,this.RunNowFromDate,this.RunNowToDate).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            var messageWindow: MessageWindow = new MessageWindow();
            if (!myResponse.HasError) {
                messageWindow.ShowSuccessIcon = true;
                messageWindow.Show("The task has been started successfully")
        
            } else {
                messageWindow.ShowErrorIcon = true;
                messageWindow.Show("The task could not be started. " + myResponse.ErrorsArray.join(", "));  
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            this.CurrentSession.StopBusyIndicator();

        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('ProcedureCode');
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

        this.myCloner.AddField('Host');
        this.myCloner.AddField('UserName');
        this.myCloner.AddField('Folder');
        this.myCloner.AddField('Password');
        this.myCloner.AddField('From');
        this.myCloner.AddField('Subject');
        this.myCloner.AddField('Prefix');
        this.myCloner.AddField('Extension');

        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
