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
    }

    private BuildPermissionLevelsList() {
        this.PermissionLevelsList = [];

        this.PermissionLevelsList.push(new CodeNameClass("ONM", "Only Me"));
        this.PermissionLevelsList.push(new CodeNameClass("ALL", "All Users"));
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

    CancelButtonClicked() {        
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

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
