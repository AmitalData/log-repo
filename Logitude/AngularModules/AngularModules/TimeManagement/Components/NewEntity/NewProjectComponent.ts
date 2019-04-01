import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TMProjectPM} from '../../EntityPMs/TMProjectPM';
import {TMProjectPMService} from '../../Services/StandardPMs/TMProjectPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'NewProjectComponent',
    moduleId: module.id,
    templateUrl: './NewProjectComponent.html',
})

export class NewProjectComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMProject";
    public EntityPM: TMProjectPM;
    public SelectedLocationFilter: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new TMProjectPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
    }


    SetWindowArgs(args) {
        this.EntityPM.CustomerId = args.EntityArgs.CustomerId;
        this.EntityPM.CustomerName = args.EntityArgs.CustomerName;
        this.EntityPM.OwnerId = args.EntityArgs.OwnerId;
        this.EntityPM.OwnerName = args.EntityArgs.OwnerName;
        this.EntityPM.ProjectNumber = args.EntityArgs.ProjectNumber;
        this.EntityPM.Id = args.EntityArgs.Id;
        this.EntityPM.IsInnerProject = true;
        this.EntityPM.CategoryId = args.EntityArgs.CategoryId;
    }

    get CustomerId() {
        return this.EntityPM.CustomerId;
    }
    set CustomerId(value: string) {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;
        }
    }


    get BudgetId() {
        return this.EntityPM.BudgetId;
    }
    set BudgetId(value: string) {
        if (this.EntityPM.BudgetId != value) {
            this.EntityPM.BudgetId = value;
        }
    }

    get IsProrated() {
        return this.EntityPM.IsProrated;
    }
    set IsProrated(value: boolean) {
        if (this.EntityPM.IsProrated != value) {
            this.EntityPM.IsProrated = value;
        }
    }

    get CategoryId() {
        return this.EntityPM.CategoryId;
    }
    set CategoryId(value: string) {
        if (this.EntityPM.CategoryId != value) {
            this.EntityPM.CategoryId = value;
        }
    }

    get OwnerId() {
        return this.EntityPM.OwnerId;
    }
    set OwnerId(value: string) {
        if (this.EntityPM.OwnerId != value) {
            this.EntityPM.OwnerId = value;
        }
    }

    get ExternalProjectNumber() {
        return this.EntityPM.ExternalProjectNumber;
    }
    set ExternalProjectNumber(value: string) {
        if (this.EntityPM.ExternalProjectNumber != value) {
            this.EntityPM.ExternalProjectNumber = value;
        }
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
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
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService: TMProjectPMService = new TMProjectPMService();
        myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
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
