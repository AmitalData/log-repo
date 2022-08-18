import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { AppTool, ArrayTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DashboardPM } from '../../../Infrastructure/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../Infrastructure/EntityPMs/WidgetPM';
import { DashboardPMService } from '../../../Infrastructure/Services/StandardPMs/DashboardPMService';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Component({
    templateUrl: './AddEditWidgetComponent.html',
})

export class AddEditWidgetComponent extends BaseComponent {
    public EntityPM: WidgetPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditWidgetComponent;
    private isNew: boolean = false;
    private dashboardService: DashboardPMService;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Widget";
    constructor() {
        super();
        this.dashboardService = new DashboardPMService();
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DataContext = this;
        this.isNew = AppTool.IsNullOrEmpty(this.EntityPM.Id);
    }

    get Title() { return this.EntityPM.Title }
    set Title(value: string) {
        if (this.EntityPM.Title != value) {
            this.EntityPM.Title = value;
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            this.EntityPM.TypeCode = value;
        }
    }

    get GroupBy() { return this.EntityPM.GroupBy; }
    set GroupBy(value: string) {
        if (this.EntityPM.GroupBy != value) {
            this.EntityPM.GroupBy = value;
        }
    }

    get StartPotistion() { return this.EntityPM.StartPotistion; }
    set StartPotistion(value: string) {
        if (this.EntityPM.StartPotistion != value) {
            this.EntityPM.StartPotistion = value;
        }
    }

    get EndPosition() { return this.EntityPM.EndPosition; }
    set EndPosition(value: string) {
        if (this.EntityPM.EndPosition != value) {
            this.EntityPM.EndPosition = value;
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
        //this.dashboardService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
        //    this.OnSaveCompleted(myResponse);
        //});
    }
    private UpdateDashboard() {
        //this.dashboardService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
        //    this.OnSaveCompleted(myResponse);
        //});
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
