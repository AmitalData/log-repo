import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { DashboardPM } from '../../../../DashboardModule/EntityPMs/DashboardPM';
import { DashboardPMService } from '../../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { DashboardPMExtendedService } from '../../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { DashboardSharedUserPM } from '../../../../DashboardModule/EntityPMs/DashboardSharedUserPM';

@Component({
    templateUrl: './AddEditDashboardComponent.html',
})

export class AddEditDashboardComponent extends BaseComponent implements OnInit {
    public EntityPM: DashboardPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditDashboardComponent;
    public isNew: boolean = false;
    private dashboardService: DashboardPMService;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Dashboard";
    public SessionIndex: number;
    public PermissionLevelsList: CodeNameClass[] = [];
    public FilterTypes: CodeNameClass[] = [];
    public CommonFilterFields: CodeNameClass[] = [];
    private maxFiltersLineNumber = 0;
    public LoggedTenant: number = 0;
    constructor() {
        super();
        this.dashboardService = new DashboardPMService();
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.LoggedTenant = SessionLocator.Tenant;
        this.BuildPermissionLevelsList();
        this.BuildFilterTypesList();
        this.BuildCommonFilterFieldsList();
    }

    ngOnInit(): void {
        this.UIProperties.SetEnabled("PredefinedOrder", this.ObjectTableName, this.PinnedByDefault);
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DataContext = this;
        this.isNew = AppTool.IsNullOrEmpty(this.EntityPM.Id);
        if (!this.isNew) this.CopySharedUsers();
        this.Clone();
    }

    private BuildPermissionLevelsList() {
        this.PermissionLevelsList = [];

        this.PermissionLevelsList.push(new CodeNameClass("ONM", "Only Me"));
        this.PermissionLevelsList.push(new CodeNameClass("PUB", "Public"));
        this.PermissionLevelsList.push(new CodeNameClass("SPF", "Specific Users"));

        if (this.isNew)
            this.PermissionLevelCode = this.LoggedTenant == 0 ? "PUB" : "ONM";
    }
    private BuildFilterTypesList() {
        this.FilterTypes = [];

        this.FilterTypes.push(new CodeNameClass("COMN", "Common Filter"));
        this.FilterTypes.push(new CodeNameClass("DATA", "Dataset Filter"));
    }

    private BuildCommonFilterFieldsList() {
        this.CommonFilterFields = [];

        this.CommonFilterFields.push(new CodeNameClass("CreateDate", "Create Date", "Date"));
        this.CommonFilterFields.push(new CodeNameClass("Number", "Number", "Text"));
    }

    get Name() { return this.EntityPM.Name }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard name change", DashboardId: this.EntityPM?.Id });
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard description change", DashboardId: this.EntityPM?.Id });
        }
    }

    get PermissionLevelCode() { return this.EntityPM.PermissionLevelCode; }
    set PermissionLevelCode(value: string) {
        if (this.EntityPM.PermissionLevelCode != value) {
            this.EntityPM.PermissionLevelCode = value;
            MixPanelLocator.PostDashboardAction({ ActionName: "New Edit Dashboard permission change", DashboardId: this.EntityPM?.Id });
        }
    }

    get PinnedByDefault() { return this.EntityPM.PinnedByDefault; }
    set PinnedByDefault(value: boolean) {
        if (this.EntityPM.PinnedByDefault != value) {
            this.EntityPM.PinnedByDefault = value;
            this.UIProperties.SetEnabled("PredefinedOrder", this.ObjectTableName, this.PinnedByDefault);
        }
    }

    get PredefinedOrder() { return this.EntityPM.PredefinedOrder; }
    set PredefinedOrder(value: number) {
        if (this.EntityPM.PredefinedOrder != value) {
            this.EntityPM.PredefinedOrder = value;
        }
    }

    ChooseUsersClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Choose Users";
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Show("./DashboardModule/Components/Windows/Controls/ChooseUsersComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
        });
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    public savedUsers: DashboardSharedUserPM[] = [];
    public CopySharedUsers() {
        this.savedUsers = [];
        if (this.EntityPM.DashboardSharedUsers.length > 0) {
            this.EntityPM.DashboardSharedUsers.forEach(item => {
                var userItem = new DashboardSharedUserPM(null);
                userItem.DashboardId = item.DashboardId;
                userItem.Tenant = item.Tenant;
                userItem.UserId = item.UserId;
                userItem.Id = item.Id;
                this.savedUsers.push(item);
            });
        }
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
        this.ResetSharedUsers();
        this.myCloner.RejectChanges();
    }

    public ResetSharedUsers() {
        if (this.savedUsers != null) {
            var items: DashboardSharedUserPM[] = this.EntityPM.DashboardSharedUsers;
            items.forEach(item => {
                var savedItem: DashboardSharedUserPM = this.savedUsers.filter(d => d.Id == item.Id)[0];
                if (savedItem == null) {
                    if (this.EntityPM.DashboardSharedUsers.indexOf(item) != -1) {
                        this.EntityPM.RemoveDashboardSharedUser(item);
                    }
                }

                else {
                    item.DashboardId = savedItem.DashboardId;
                    item.Tenant = savedItem.Tenant;
                    item.UserId = savedItem.UserId;
                    item.Id = savedItem.Id;
                }
            });

            this.savedUsers.forEach(item => {
                var list = this.EntityPM.DashboardSharedUsers.filter(d => d.Id == item.Id);
                if (list == null) {
                    this.EntityPM.DashboardSharedUsers.push(item);
                }
            });
        }
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
                MixPanelLocator.PostDashboardAction({ ActionName: "New Dashboard save click", DashboardId: this.EntityPM?.Id });
                this.CreateDashboard();
            }

            else {
                MixPanelLocator.PostDashboardAction({ ActionName: "Edit Dashboard save click", DashboardId: this.EntityPM?.Id });
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