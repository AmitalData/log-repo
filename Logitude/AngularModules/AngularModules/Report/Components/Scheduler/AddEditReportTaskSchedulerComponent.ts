import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TasksSchedulerPM } from '../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SchedulerExtendedPMService } from '../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import { Component } from '@angular/core';
import { TaskReportSchedulerItemClass } from './TaskReportSchedulerComponent';
import { QueryFilterItem } from '../Filters/QueryFilterItem';
import {
    SchedulerDetails,
    ReportSchedulerDetails,
    ReportSchedulerRecepients,
} from '../../../Infrastructure/DataContracts/SchedulerDetails';
import { DateTimePipe } from '../../../Controls/Pipes/DateTimePipe';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { AddEditReportSchedulerComponent } from './AddEditReportSchedulerComponent';

@Component({
    templateUrl: './AddEditReportTaskSchedulerComponent.html',
   
})
export class AddEditReportTaskSchedulerComponent {
    public EntityPM: TasksSchedulerPM;
    public DataContext: TaskReportSchedulerItemClass;
    public ObjectTableName: string = 'TasksScheduler';
    public DisplayFTPOption: boolean = false;
    public SchedulerFormats: CodeNameClass[] = [];
    public SelectedFormat: CodeNameClass;
    public SelectedFormatAdvanced: string;
    schedulerExtendedPMService: SchedulerExtendedPMService;
    private parentComponent: AddEditReportSchedulerComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
        if (SessionLocator.LoggedUserPM.IsCustomerCare || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            this.DisplayFTPOption = true;
        }
    }

    SetDataContext(DataContext: any) {
        this.DataContext = DataContext['DataContext'];
        this.EntityPM = DataContext['DataContext'].EntityPM;
        this.EntityPM.EntityId = this.DataContext.fatherComponent.ReportList.Id;
        this.FillSchedulerFormats();
        this.SetSchedulerFormat();
        this.SetSchedulerResultType();
        this.EntityPM.ProcedureCode = 'ReportSchedulerTask';
        this.parentComponent = DataContext['parentComponent'];
        this.BuildSchedulerDetailsData();
        this.Clone();
        this.SetTigger(this.DataContext.TriggerType);
    }
     
    private FillSchedulerFormats() {
        this.SchedulerFormats.push(new CodeNameClass("PDF", "PDF"));
        this.SchedulerFormats.push(new CodeNameClass("EXCL", "Excel File"));
        this.SchedulerFormats.push(new CodeNameClass("EXCLA", "Excel File (Advanced)"));
    }

    private SetSchedulerFormat() {
        this.SelectedFormat = this.SchedulerFormats.filter(format => format.Code == this.EntityPM.Format)[0];
        this.SelectedFormatAdvanced = this.EntityPM.AdvancedFormat;
    }

    private SetSchedulerResultType() {
        if (this.DataContext.IsNew) {
            this.EntityPM.Type = 'Report';
            this.EntityPM.ResultType = "Email";
            this.IsEmail = true;
        }

        else {
            this.IsEmail = (this.EntityPM.ResultType === "Email") || AppTool.IsNullOrEmpty(this.EntityPM.ResultType);
            this.IsFTP = (this.EntityPM.ResultType === "FTP");
        }
    }

    BuildSchedulerDetailsData() {
        if (this.EntityPM.SchedulerDetailsData) {
            this.SetSchedulerDetailsData(this.EntityPM.SchedulerDetailsData);
        } else if (this.EntityPM.Id) {
            this.LoadReportSchedulerDetailsData();
        } else {
            var schedulerDetails = new SchedulerDetails();
            schedulerDetails.ReportDetails = new ReportSchedulerDetails();
            schedulerDetails.ReportDetails.Recepients = new ReportSchedulerRecepients();
            this.SetSchedulerDetailsData(schedulerDetails);
        }
    }

    SetSchedulerDetailsData(schedulerDetails: SchedulerDetails) {
        this.DataContext.SetReportSchedulerDetailsData(schedulerDetails);
        this.Clone();
    }

    StartTimeTabTitle: string = 'One Time';

    private isDaily: boolean;
    get IsDaily() {
        return this.isDaily;
    }
    set IsDaily(newValue: boolean) {
        if (this.isDaily != newValue) {
            this.isDaily = newValue;
        }
    }

    private isWeekly: boolean;
    get IsWeekly() {
        return this.isWeekly;
    }
    set IsWeekly(newValue: boolean) {
        if (this.isWeekly != newValue) {
            this.isWeekly = newValue;
        }
    }

    private isMonthly: boolean;
    get IsMonthly() {
        return this.isMonthly;
    }
    set IsMonthly(newValue: boolean) {
        if (this.isMonthly != newValue) {
            this.isMonthly = newValue;
        }
    }

    SetTigger(triggerType: string) {
        switch (triggerType) {
            case 'D': {
                this.IsDaily = true;
                this.IsWeekly = false;
                this.IsMonthly = false;
                this.DataContext.TriggerType = 'D';
                this.StartTimeTabTitle = 'Daily';
                break;
            }

            case 'W': {
                this.IsDaily = false;
                this.IsWeekly = true;
                this.IsMonthly = false;
                this.DataContext.TriggerType = 'W';
                this.StartTimeTabTitle = 'Weekly';
                break;
            }

            case 'M': {
                this.IsDaily = false;
                this.IsWeekly = false;
                this.IsMonthly = true;
                this.DataContext.TriggerType = 'M';
                this.StartTimeTabTitle = 'Monthly';
                break;
            }

            default: {
                this.IsDaily = true;
                this.IsWeekly = false;
                this.IsMonthly = false;
                this.DataContext.TriggerType = 'D';
                this.StartTimeTabTitle = 'Daily';
                break;
            }
        }
    }

    private isFTP: boolean;
    get IsFTP() {
        return this.isFTP;
    }
    set IsFTP(newValue: boolean) {
        if (this.isFTP != newValue) {
            this.isFTP = newValue;
            this.DataContext.IsFTP = newValue;
            if (AppTool.IsNullOrEmpty(this.SelectedFormat)) this.FormatSelectionChanged(this.SchedulerFormats.filter(format => format.Code == 'PDF')[0]);
        }
    }

    private isEmail: boolean;
    get IsEmail() {
        return this.isEmail;
    }
    set IsEmail(newValue: boolean) {
        if (this.isEmail != newValue) {
            this.isEmail = newValue;
        }
    }

    get SendIfEmpty() {
        return this.DataContext.SendIfEmpty;
    }
    set SendIfEmpty(newValue: boolean) {
        if (this.DataContext.SendIfEmpty != newValue) {
            this.DataContext.SendIfEmpty = newValue;
        }
    }

    SetFormatAdvanced(formatAdvanced: string) {
        this.SelectedFormatAdvanced = formatAdvanced;
        this.EntityPM.AdvancedFormat = formatAdvanced;
    }

    SetTaskType(taskType: string) {
        switch (taskType) {
            case 'E':
                this.EntityPM.ResultType = 'Email';
                this.IsEmail = true;
                this.IsFTP = false;
                break;
            case 'FTP':
                this.EntityPM.ResultType = 'FTP';
                this.IsEmail = false;
                this.IsFTP = true;
                break;
            default:
                this.IsEmail = true;
                this.IsFTP = false;
                this.EntityPM.ResultType = 'Email';
                break;
        }
    }

    NextButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate('General.M.FieldIsRequired');

        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");


        if (this.IsFTP) {
            

            if (AppTool.IsNullOrEmpty(this.DataContext.UserName)) errors.push(msg.replace("%FieldName", "UserName"));
            if (AppTool.IsNullOrEmpty(this.DataContext.Password)) errors.push(msg.replace("%FieldName", "Password"));
            //if (AppTool.IsNullOrEmpty(this.DataContext.Subject)) errors.push(msg.replace("%FieldName", "Subject"));
            //if (AppTool.IsNullOrEmpty(this.DataContext.From)) errors.push(msg.replace("%FieldName", "From"));
            if (AppTool.IsNullOrEmpty(this.DataContext.Host)) errors.push(msg.replace("%FieldName", "Host"));
        }

        Validator.TryValidateObject(
            this.DataContext.EntityPM,
            this.ObjectTableName,
            errors
        );

        if (AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            errors.push(msg.replace('%FieldName', 'Name'));
        }

        if (this.DataContext.StartDateTime == null) {
            errors.push(msg.replace('%FieldName', 'Start Date Time'));
            console.log('error');
        }

        if (AppTool.IsNullOrEmpty(this.DataContext.TriggerType)) {
            errors.push(msg.replace('%FieldName', 'Trigger Type'));
        } else {
            switch (this.DataContext.TriggerType) {
                case 'W': {
                    if (
                        !this.DataContext.Satarday &&
                        !this.DataContext.Sunday &&
                        !this.DataContext.Monday &&
                        !this.DataContext.Tuesday &&
                        !this.DataContext.Wednesday &&
                        !this.DataContext.Thursday &&
                        !this.DataContext.Friday
                    ) {
                        errors.push(msg.replace('%FieldName', 'Day'));
                    }
                    break;
                }
                case 'M': {
                    break;
                }
            }
        }

        if (this.EntityPM.Status == 'In progress') {
            errors.push(
                'The task is in progress. You are not allowed to edit it'
            );
        }

        if (
            this.DataContext.StartDateTime < DateTool.GetCurrentDateTimeAsUtc()
        ) {
            errors.push("You can't select a past date");
        }

        this.parentComponent.ValidationErrorsList = errors;
        if (this.parentComponent.ValidationErrorsList.length == 0) {
            return true;
        }
    }

    LoadReportSchedulerDetailsData() {
        this.CurrentSession.StartBusyIndicator('Loading...');

        this.schedulerExtendedPMService
            .GetSchedulerDetailsById(this.EntityPM.Id)
            .subscribe((myResult: ServiceResponse) => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.SetSchedulerDetailsData(myResponse.Result);
                } else {
                    this.parentComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    this.Clone();
                }
                this.CurrentSession.StopBusyIndicator();
            });
    }

    FormatSelectionChanged(selectControl: any) {
        if (selectControl) {
            this.SelectedFormat = selectControl;
            this.EntityPM.Format = selectControl.Code;
            if (AppTool.IsNullOrEmpty(this.SelectedFormatAdvanced) && selectControl.Code == 'EXCLA') {
                this.SetFormatAdvanced('DO');
            }
            else if (selectControl.Code != 'EXCLA') {
                this.SetFormatAdvanced('');
            }
        }
    }

    SaveButtonClicked(
        reportFilterItems: Array<QueryFilterItem>,
        reportTemplateId: string,
        recepients: ReportSchedulerRecepients
    ) {

        
        this.CurrentSession.StartBusyIndicatorSaving();

        this.SetReportDetails(reportFilterItems, reportTemplateId, recepients);
        if (this.DataContext.IsNew) {
            this.DataContext.SchedulerDetails.ReportDetails.CreatedByUserId =
                SessionLocator.LoggedUserId;
            this.schedulerExtendedPMService
                .insert(this.EntityPM)
                .subscribe((myResult: ServiceResponse) => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        this.EntityPM = myResponse.Result;
                        this.EntityPM.IsDirty = false;
                        if (this.DataContext.fatherComponent) {
                            this.DataContext.fatherComponent.RefreshButtonClicked();
                        }

                        this.CurrentSession.CloseCurrentWindow();
                    } else {
                        this.HandleServiceError(myResponse);
                    }
                    this.CurrentSession.StopBusyIndicator();
                });
        } else {
            if (this.EntityPM.IsDirty) {
                this.EntityPM.UpdatedBy =
                    SessionLocator.LoggedUserPM.EnglishName;
                this.schedulerExtendedPMService
                    .update(this.EntityPM)
                    .subscribe((myResult: ServiceResponse) => {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {
                            this.EntityPM = myResponse.Result;
                            this.EntityPM.IsDirty = false;
                            if (this.DataContext.fatherComponent) {
                                this.DataContext.fatherComponent.RefreshButtonClicked();
                            }

                            this.CurrentSession.CloseCurrentWindow();
                        } else {

                            this.HandleServiceError(myResponse);
                        }
                        this.CurrentSession.StopBusyIndicator();
                    });
            } else {
                this.CurrentSession.StopBusyIndicator();
            }
        }
    }



    private HandleServiceError(myResponse: ServiceResponse) {
        this.parentComponent.ValidationErrorsList = myResponse.ErrorsArray;
        if (this.isReportPreviewTab())
            this.openReportTaskTab();
    }

    private openReportTaskTab() {
        this.parentComponent.SelectedTabLocation = 0;
        this.parentComponent.SetSelectedItem("RETASK");
    }

    private isReportPreviewTab() {
        return this.parentComponent.SelectedTabLocation == 1;
    }

    SetReportDetails(
        reportFilterItems: Array<QueryFilterItem>,
        reportTemplateId: string,
        recepients: ReportSchedulerRecepients
    ) {
        this.DataContext.SchedulerDetails.ReportDetails.ReportFilterItems = reportFilterItems;
        this.DataContext.SchedulerDetails.ReportDetails.ReportTemplateId = reportTemplateId;
        this.DataContext.SchedulerDetails.ReportDetails.Recepients.To = recepients.To
            ? recepients.To.toString().split(',').join(';')
            : '';
        this.DataContext.SchedulerDetails.ReportDetails.Recepients.Cc = recepients.Cc
            ? recepients.Cc.toString().split(',').join(';')
            : '';
        this.DataContext.SchedulerDetails.ReportDetails.Recepients.Bcc = recepients.Bcc
            ? recepients.Bcc.toString().split(',').join(';')
            : '';
    }

    GetReportFilterItems() {
        return this.EntityPM.SchedulerDetailsData.ReportDetails
            .ReportFilterItems;
    }

    GetReportTemplateId() {
        return this.EntityPM.SchedulerDetailsData.ReportDetails
            .ReportTemplateId;
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
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
        this.myCloner.AddField('Format');
        this.myCloner.AddField('AdvancedFormat');

        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
