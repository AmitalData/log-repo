import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TMOfficeHourPM} from '../../EntityPMs/TMOfficeHourPM';
import {TMOfficeHourPMService} from '../../Services/StandardPMs/TMOfficeHourPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {TimeManagementDomainService, TimeManagementAPIHelper, TimeSheetItem, TimeSheetItemDay} from '../../Services/TimeManagementDomainService';
import {Validator} from '../../../Infrastructure/Validators/Validator';

@Component({
    selector: 'NewOfficeHourComponent',
    moduleId: module.id,
    templateUrl: './NewOfficeHourComponent.html',
})

export class NewOfficeHourComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMOfficeHour";
    public EntityPM: TMOfficeHourPM;
    public LocationCode: any;
    public TMOfficeHourPMService: TMOfficeHourPMService;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TMOfficeHourPMService = new TMOfficeHourPMService();
        this.EntityPM = new TMOfficeHourPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
    }
    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM.UserId = args.EmployeeUserId;        
        }
    }

    get WorkDate() { return this.EntityPM.WorkDate; }
    set WorkDate(value: Date) {
        if (this.EntityPM.WorkDate != value) {
            this.EntityPM.WorkDate = value;
             }
       }
 
    set EntryTime(value: Date) {
        this.EntityPM.EntryTime = value;
        }

    get EntryTime() {
        return this.EntityPM.EntryTime;
    }

    SetEntryDateTime(value: string) {
        if (value) {
            var hours = value.split(':')[0];
            var minutes = value.split(':')[1];
            if (this.EntityPM.WorkDate != null) {
                this.EntityPM.EntryTime = DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;
                this.EntityPM.EntryTime.setUTCHours(+hours);
                this.EntityPM.EntryTime.setUTCMinutes(+minutes);

            }
            else {
                this.EntityPM.EntryTime = DateTool.GetCurrentDateAsUtc();
                this.EntityPM.EntryTime.setUTCHours(+hours);
                this.EntityPM.EntryTime.setUTCMinutes(+minutes);
            }            
        }        
    }
    
    set ExitTime(value: Date) {
        this.EntityPM.ExitTime = value;
    }

    get ExitTime() {
        return this.EntityPM.ExitTime;
    }

    SetExistDateTime(value:string) {
        if (value) {
            var hours = value.split(':')[0];
            var minutes = value.split(':')[1];
            if (this.EntityPM.WorkDate != null) {
                this.EntityPM.ExitTime = DateTool.GetDateParts(this.EntityPM.WorkDate).DateObject;
                this.EntityPM.ExitTime.setUTCHours(+hours);
                this.EntityPM.ExitTime.setUTCMinutes(+minutes);

            }
            else {
                this.EntityPM.ExitTime= DateTool.GetCurrentDateAsUtc();
                this.EntityPM.ExitTime.setUTCHours(+hours);
                this.EntityPM.ExitTime.setUTCMinutes(+minutes);
            }            
        }        
    }    

    get Description() {
        return this.EntityPM.Description;
    }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }
    

    // Commands
    public ValidationErrorsList: string[];
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.EntityPM.WorkDate == null)
            this.ValidationErrorsList.push("Work Date Field is required");



        if (this.EntityPM.ExitTime != null && this.EntityPM.EntryTime != null) {
            if (this.EntityPM.ExitTime <= this.EntityPM.EntryTime) {
                this.ValidationErrorsList.push("Exist time Field must be greater than Entry time Field");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.ExitTime != null) {
                this.SetExistDateTime(this.EntityPM.ExitTime.getUTCHours() + ":" + this.EntityPM.ExitTime.getUTCMinutes());
            }
                if(this.EntityPM.EntryTime != null) {
                this.SetEntryDateTime(this.EntityPM.EntryTime.getUTCHours() + ":" + this.EntityPM.EntryTime.getUTCMinutes());
            }

            this.InsertTMOfficeHour();
        }
    }
    InsertTMOfficeHour() {
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.CurrentSession.StartBusyIndicatorSaving();

        var myServiceHelper = new TimeManagementAPIHelper();
        myServiceHelper.Id = SessionLocator.Tenant;
     
        if (this.TMOfficeHourPMService == null) {
            this.TMOfficeHourPMService = new TMOfficeHourPMService();
        }

        this.TMOfficeHourPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
}
