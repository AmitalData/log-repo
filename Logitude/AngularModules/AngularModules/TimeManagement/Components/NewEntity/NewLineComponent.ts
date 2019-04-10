import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TMEmployeeTimePM} from '../../EntityPMs/TMEmployeeTimePM'; 
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {TimeManagementDomainService, TimeManagementAPIHelper, TimeSheetItem, TimeSheetItemDay} from '../../Services/TimeManagementDomainService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {DailyTimeSheetComponent, ItemSourceItem} from '../Workspaces/TimeSheet/DailyTimeSheetComponent';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'NewLineComponent',
    moduleId: module.id,
    templateUrl: './NewLineComponent.html',
})

export class NewLineComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMEmployeeTime";
    public EntityPM: TMEmployeeTimePM;
    private myDomainService: TimeManagementDomainService = new TimeManagementDomainService();
    Father: DailyTimeSheetComponent;

    public DateOfWorkDate: TimeSheetItemDay = new TimeSheetItemDay();
    private CurrentSession = SessionLocator.SelectedSession;
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
        this.EntityPM.NeedsProrating = true;
        this.SetUIProperties();
    }

    public EntityId = "";
    public IsNew = true;
    SetWindowArgs(args: any) {
        if (args != null) {

            if (!args.IsNew) {
                this.EntityPM = args.EntityPM;
                this.Clone();
                this.EntityId = this.EntityPM.Id;
                this.DateOfWorkDate.Date = this.EntityPM.DateOfWork;
                this.DateOfWorkMinutes = this.EntityPM.TimeInMinutes;
                this.Father = args.Father;
                this.LocationCode = args.LocationCode;
                this.SetUIProperties();
                this.IsNew = false;
            }

            else {
                this.Father = args.Father;
                this.EntityId = args.EntityId;
                this.LocationCode = args.LocationCode;
                this.EntityPM.EmployeeUserId = SessionLocator.LoggedUserId;
                this.EntityPM.LocationCode = this.LocationCode;
                this.ProjectId = args.ProjectId;
                this.TimeSheetItem.ProjectId_db = args.ProjectId;
                this.WINumber = args.WINumber;
                this.TimeSheetItem.WINumber_db = args.WINumber;
                this.TimeSheetItem.Description_db = args.Description;
                this.DateOfWorkDateFormat = this.ApplyTimeFormat(this.DateOfWorkMinutes);
                this.DateOfWork = args.DateOfWork;
                this.DateOfWorkDate.Date = this.DateOfWork;
                this.SprintId = args.SprintId;
            }
        }
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("LocationCode", this.ObjectTableName,true);
        this.UIProperties.SetRequired("DateOfWorkDateFormat", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DateOfWorkDateFormat));
        this.UIProperties.SetRequired("SprintId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.SprintId));
        this.UIProperties.SetRequired("DateOfWork", this.ObjectTableName, this.DateOfWork == null ? true : false);
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

    get EmployeeUserId() {
        if (this.EntityPM != null) {
            return this.EntityPM.EmployeeUserId;
        }
    }
    set EmployeeUserId(value: string) {
        if (this.EntityPM.EmployeeUserId != value) {
            this.EntityPM.EmployeeUserId = value;
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

    get LocationCode() {
        if (this.EntityPM != null) {
            return this.EntityPM.LocationCode;
        }
    }
    set LocationCode(value: string) {
        if (this.EntityPM.LocationCode != value) {
            this.EntityPM.LocationCode = value;
        }
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
            this.UIProperties.SetRequired("DateOfWork", this.ObjectTableName, this.DateOfWork == null ? true : false);
        }
    }

    get DateOfWorkMinutes() {
        if (this.DateOfWorkDate != null) {
            return this.DateOfWorkDate.Minuts;
        }
    }
    set DateOfWorkMinutes(value: number) {
        if (this.DateOfWorkDate != null) {
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
            this.dateOfWorkDateFormat = value;
        }
    }

    // Commands
    public ValidationErrorsList: string[];
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.DateOfWorkMinutes == null || this.DateOfWorkMinutes == 0) {
            errors.push("Please fill the Time");
        }

        if (this.DateOfWork == null) {
            errors.push("Date of work is required");
        }

        if (this.SprintId == null) {
            errors.push("Sprint is required");
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.InsertTMEmployeeTime();
        }
    }
    public TimeManagementAPIHelper = new TimeManagementAPIHelper();
    public TimeSheetItem: TimeSheetItem = new TimeSheetItem();
    InsertTMEmployeeTime() {
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.CurrentSession.StartBusyIndicatorSaving();

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
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }


    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('ProjectId');
        this.myCloner.AddField('EmployeeUserId');
        this.myCloner.AddField('WINumber');
        this.myCloner.AddField('SprintId');
        this.myCloner.AddField('DateOfWork');
        this.myCloner.AddField('LocationCode');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
