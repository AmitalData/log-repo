import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TMProjectPM} from '../../EntityPMs/TMProjectPM'; 
import {TMProjectPMService} from '../../Services/StandardPMs/TMProjectPMService'; 
import {TMEmployeeTimePM} from '../../EntityPMs/TMEmployeeTimePM'; 
import {TMEmployeeTimePMService} from '../../Services/StandardPMs/TMEmployeeTimePMService'; 
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {TimeManagementDomainService, TimeManagementAPIHelper, TimeSheetItem, TimeSheetItemDay} from '../../Services/TimeManagementDomainService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {DailyTimeSheetComponent, ItemSourceItem} from '../Workspaces/TimeSheet/DailyTimeSheetComponent';

@Component({
    selector: 'NewLineComponent',
    moduleId: module.id,
    templateUrl: './NewLineComponent.html',
})

export class NewLineComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMEmployeeTime";
    public EntityPM: TMEmployeeTimePM;
    public LocationCode: any;
    public EmployeeUserId: string = null;
    private myDomainService: TimeManagementDomainService = new TimeManagementDomainService();
    Father: DailyTimeSheetComponent;

    public DateOfWorkDate: TimeSheetItemDay = new TimeSheetItemDay();

    constructor() {
        super();
        this.myDomainService = new TimeManagementDomainService();
        this.EntityPM = new TMEmployeeTimePM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.EmployeeUserId = SessionLocator.LoggedUserId;
        this.SetUIProperties();
    }

    public EntityId = "";
    public IsNew = true;
    SetWindowArgs(args: any) {
       
        if (args != null) {
            if (!args.IsNew == false) {
                this.EntityPM = args.EntityPM;
                this.EntityId = this.EntityPM.Id;
                this.DateOfWorkDate.Date = this.EntityPM.DateOfWork;
                this.DateOfWorkDateFormat = this.ApplyTimeFormat(this.DateOfWorkMinutes);
                this.Father = args.Father;
                this.LocationCode = args.Father.LocationCode;
                this.EmployeeUserId = args.Father.EmployeeUserId;
                this.SetUIProperties();
                this.IsNew = false;
            }
            else {
                this.Father = args.Father;
                this.EntityId = args.EntityId;
                this.LocationCode = args.LocationCode;
                this.EmployeeUserId = args.EmployeeUserId;
                this.EntityPM.LocationCode = this.LocationCode;
                this.ProjectId = args.ProjectId;
                this.TimeSheetItem.ProjectId_db = args.ProjectId;
                this.WINumber = args.WINumber;
                this.TimeSheetItem.WINumber_db = args.WINumber;
                this.TimeSheetItem.Description_db = args.Description;
                this.DateOfWorkDateFormat = this.ApplyTimeFormat(this.DateOfWorkMinutes);
                this.DateOfWork = args.DateOfWork;
                this.DateOfWorkDate.Date = this.DateOfWork;
            }
        }
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("Description", this.ObjectTableName, AppTool.IsNullOrEmpty(this.WINumber) && AppTool.IsNullOrEmpty(this.Description));
        this.UIProperties.SetRequired("WINumber", this.ObjectTableName, AppTool.IsNullOrEmpty(this.WINumber) && AppTool.IsNullOrEmpty(this.Description));
        this.UIProperties.SetRequired("DateOfWorkDateFormat", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DateOfWorkDateFormat));
        this.UIProperties.SetRequired("SprintId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.SprintId));
    }
    ApplyTimeFormat(minutes) {
        var formattedMinutes = "";
        var val = minutes;
        var h = val / 60 | 0,
            m = val % 60 | 0;
        var result = h + ":" + AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            formattedMinutes = "";
        }
        else {
            formattedMinutes = result;
        }
        return formattedMinutes;
    }

    get ProjectId() {
        if (this.EntityPM != null) {
            return this.EntityPM.ProjectId;
        }
    }
    set ProjectId(value: string) {
        if (this.EntityPM.ProjectId != value) {
            this.EntityPM.ProjectId = value;
        }
        this.SetUIProperties();
    }

    get SprintId() {
        if (this.EntityPM != null) {
            return this.EntityPM.SprintId;
        }
    }
    set SprintId(value: string) {
        if (this.EntityPM.SprintId != value) {
            this.EntityPM.SprintId = value;
        }
        this.SetUIProperties();
    }

    get WINumber() {
        return this.EntityPM.WINumber;
    }
    set WINumber(value: string) {
        if (this.EntityPM.WINumber != value) {
            this.EntityPM.WINumber = value;
        }
        this.SetUIProperties();
    }

    get Description() {
        return this.EntityPM.Description;
    }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
        this.SetUIProperties();
    }

    get DateOfWork() {
        return this.EntityPM.DateOfWork;
    }

    set DateOfWork(value: Date) {
        if (this.EntityPM.DateOfWork != value) {
            this.EntityPM.DateOfWork = value;
            this.DateOfWorkDate.Date = value;
        }
    }

    get DateOfWorkMinutes() {
        if (this.DateOfWorkDate != null) {
            return this.DateOfWorkDate.Minuts;
        }
    }
    set DateOfWorkMinutes(value: number) {
        if (this.DateOfWorkDate.Minuts != value) {
            this.DateOfWorkDate.Minuts = value;
            this.DateOfWorkDateFormat = this.ApplyTimeFormat(value);
        }
    }

    private dateOfWorkDateFormat = "";
    get DateOfWorkDateFormat() {
        return this.dateOfWorkDateFormat;
    }
    set DateOfWorkDateFormat(value: string) {
        if (this.dateOfWorkDateFormat != value) {
            this.dateOfWorkDateFormat == value;
        }
    }

    // Commands
    public ValidationErrorsList: string[];
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DateOfWorkMinutes == null || this.DateOfWorkMinutes == 0) {
            errors.push("Please fill the total minutes");
        }

        if (this.DateOfWork == null) {
            errors.push("Date of work is required");
        }

        if (this.SprintId == null) {
            errors.push("Sprint is required");
        }

        if (AppTool.IsNullOrEmpty(this.WINumber) && AppTool.IsNullOrEmpty(this.Description)) {
            errors.push("You must fill either WI Number or Description");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.InsertTMEmployeeTime();
        }
    }
    public TimeManagementAPIHelper = new TimeManagementAPIHelper();
    public TimeSheetItem: TimeSheetItem = new TimeSheetItem();
    InsertTMEmployeeTime() {
        SessionLocator.CurrentSession.StartBusyIndicator("Creating...");
        SessionLocator.CurrentSession.StartBusyIndicatorSaving();

        this.TimeManagementAPIHelper.Id = SessionLocator.Tenant;
        this.TimeManagementAPIHelper.EmployeeUserId = this.EmployeeUserId;
        this.TimeManagementAPIHelper.LocationCode = this.LocationCode;
        this.TimeManagementAPIHelper.StartDate = this.Father.StartDate;
        this.TimeManagementAPIHelper.EndDate = this.Father.EndDate;
        this.TimeSheetItem.ProjectId = this.ProjectId;

        this.TimeSheetItem.Description = this.Description;
        this.TimeSheetItem.WINumber = this.WINumber;
        this.TimeSheetItem.LocationCode = this.LocationCode;
        this.TimeSheetItem.EmployeeUserId = this.EmployeeUserId;
        this.TimeSheetItem.Days = [];
        this.TimeSheetItem.Days.push(this.DateOfWorkDate);
        this.TimeManagementAPIHelper.Items.push(this.TimeSheetItem);

        var itemPM = new TMEmployeeTimePM();
        itemPM.Id = this.EntityId;
        itemPM.ProjectId = this.ProjectId;
        itemPM.Description = this.Description;
        itemPM.WINumber = this.WINumber;
        itemPM.LocationCode = this.LocationCode;
        itemPM.EmployeeUserId = this.EmployeeUserId;
        itemPM.TimeInMinutes = this.DateOfWorkDate.Minuts;
        itemPM.DateOfWork = this.DateOfWorkDate.Date;
        itemPM.SprintId = this.SprintId;

        this.TimeManagementAPIHelper.ItemsPM.push(itemPM);

        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService();
        }

        this.myDomainService.UpdateTimeSheetList(this.TimeManagementAPIHelper).subscribe((myResponse: ServiceResponse) => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                SessionLocator.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
}
