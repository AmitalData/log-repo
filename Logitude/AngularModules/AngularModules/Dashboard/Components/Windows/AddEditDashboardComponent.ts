import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';

@Component({
    templateUrl: './AddEditDashboardComponent.html',
})

export class AddEditDashboardComponent extends BaseComponent {
    public EntityPM: DashboardPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditDashboardComponent;
    private isNew: boolean = false;
    private dashboardService: DashboardPMService;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Dashboard";
    public SessionIndex: number;
    public PermissionLevelsList: CodeNameClass[] = [];
    constructor() {
        super();
        this.dashboardService = new DashboardPMService();
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.BuildPermissionLevelsList();
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DataContext = this;
        this.isNew = AppTool.IsNullOrEmpty(this.EntityPM.Id);
        this.Clone();
    }

    private BuildPermissionLevelsList() {
        this.PermissionLevelsList = [];

        this.PermissionLevelsList.push(new CodeNameClass("ONM", "Only Me"));
        this.PermissionLevelsList.push(new CodeNameClass("PUB", "Public"));
        this.PermissionLevelsList.push(new CodeNameClass("SPF", "Specific Users"));

        if (this.isNew)
            this.PermissionLevelCode = "ONM";
    }

    get Name() { return this.EntityPM.Name }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get PermissionLevelCode() { return this.EntityPM.PermissionLevelCode; }
    set PermissionLevelCode(value: string) {
        if (this.EntityPM.PermissionLevelCode != value) {
            this.EntityPM.PermissionLevelCode = value;
        }
    }

    ChooseUsersClicked() {
        //var args = new ChooseUserArgs();
        //args.MyQuery = this.EntityPM;
        //args.AllUsers = this.myUsersList;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Choose Users";
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Show("./Dashboard/Components/Windows/ChooseUsersComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            //this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
            //this.FillSharedWithUsersItemsSource();
        });
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('PermissionLevelCode');

        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.PermissionLevelCode == "SPF" && this.EntityPM.DashboardSharedUsers.length == 0) {
            errors.push("You have to choose at least one user");
        }

        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.isNew) {
                this.CreateDashboard();                
            }

            else {
                this.UpdateDashboard();
            }
        }
    }
    private CreateDashboard() {
        this.EntityPM.Tenant = SessionInfo.LoggedUserTenant;
        this.dashboardService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.OnSaveCompleted(myResponse);
        });
    }   
    private UpdateDashboard() {
        this.dashboardService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.OnSaveCompleted(myResponse);
        });
    }
    private OnSaveCompleted(myResponse: ServiceResponse) {
        if (!myResponse.HasError) {
            this.EntityPM = myResponse.Result;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }

        else {
            this.ValidationErrorsList = myResponse.ErrorsArray;
        }

        this.CurrentSession.StopBusyIndicator();
    }
}
