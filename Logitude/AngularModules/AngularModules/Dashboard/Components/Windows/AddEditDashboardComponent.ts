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
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { DashboardPMExtendedService } from '../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { DashboardGlobalFilterPM } from '../../../DashboardModule/EntityPMs/DashboardGlobalFilterPM';
import { AnalyticsFactsFieldsMetaDataList } from '../../../DashboardModule/EntityLists/AnalyticsFactsFieldsMetaDataList';
import { DashboardSharedUserPM } from '../../../DashboardModule/EntityPMs/DashboardSharedUserPM';

@Component({
    templateUrl: './AddEditDashboardComponent.html',
})

export class AddEditDashboardComponent extends BaseComponent {
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
    public GlobalFilters: GlobalFilterItem[];
    private maxFiltersLineNumber = 0;
    constructor() {
        super();
        this.dashboardService = new DashboardPMService();
        this.SessionIndex = this.CurrentSession.SessionIndex;        
        this.GlobalFilters = [];
        this.BuildPermissionLevelsList();
        this.BuildFilterTypesList();
        this.BuildCommonFilterFieldsList();
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.DataContext = this;
        this.isNew = AppTool.IsNullOrEmpty(this.EntityPM.Id);

        if (!this.isNew) {
            this.CopySharedUsers();
            this.CopyGlobalFilters();
        }

        this.BuildGlobalFilters();
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
    private BuildFilterTypesList() {
        this.FilterTypes = [];

        this.FilterTypes.push(new CodeNameClass("COMN", "Common Filter"));
        this.FilterTypes.push(new CodeNameClass("DATA", "Dataset Filter"));
    }

    private BuildCommonFilterFieldsList() {
        this.CommonFilterFields = [];

        this.CommonFilterFields.push(new CodeNameClass("CreateDate", "Create Date", "Date"));
        this.CommonFilterFields.push(new CodeNameClass("Number", "Number", "String"));
    }

    private BuildGlobalFilters() {
        this.EntityPM.DashboardGlobalFilters.sort((a, b) => { return a.LineNumber - b.LineNumber }).forEach(item => {
            this.GlobalFilters.push(new GlobalFilterItem(item, false, this));
        });

        this.maxFiltersLineNumber = ArrayTool.Max(this.GlobalFilters, "LineNumber");
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

    ChooseUsersClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Choose Users";
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Show("./Dashboard/Components/Windows/ChooseUsersComponent");
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

    public savedFilters: DashboardGlobalFilterPM[] = [];
    public CopyGlobalFilters() {
        this.savedFilters = [];
        if (this.EntityPM.DashboardGlobalFilters.length > 0) {
            this.EntityPM.DashboardGlobalFilters.forEach(item => {
                this.maxFiltersLineNumber = 0;
                if (item.LineNumber > this.maxFiltersLineNumber) {
                    this.maxFiltersLineNumber = item.LineNumber;
                }

                var filterItem = new DashboardGlobalFilterPM(null);
                filterItem.DashboardId = item.DashboardId;
                filterItem.Tenant = item.Tenant;
                filterItem.CommonFilterField = item.CommonFilterField;
                filterItem.DataSetFieldId = item.DataSetFieldId;
                filterItem.DataSetId = item.DataSetId;
                filterItem.DataTypeCode = item.DataTypeCode;
                filterItem.FilterOperator = item.FilterOperator;
                filterItem.IsCommonFilter = item.IsCommonFilter;
                filterItem.Id = item.Id;
                filterItem.LineNumber = item.LineNumber;
                this.savedFilters.push(filterItem);
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
        this.ResetGlobalFilters();
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
    public ResetGlobalFilters() {
        if (this.savedFilters != null) {
            var items: DashboardGlobalFilterPM[] = this.EntityPM.DashboardGlobalFilters;
            items.forEach(item => {
                var savedItem: DashboardGlobalFilterPM = this.savedFilters.filter(d => d.Id == item.Id)[0];
                if (savedItem == null) {
                    if (this.EntityPM.DashboardGlobalFilters.indexOf(item) != -1) {
                        this.EntityPM.RemoveDashboardGlobalFilter(item);
                    }
                }

                else {
                    item.DashboardId = savedItem.DashboardId;
                    item.Tenant = savedItem.Tenant;
                    item.CommonFilterField = savedItem.CommonFilterField;
                    item.DataSetFieldId = savedItem.DataSetFieldId;
                    item.DataSetId = savedItem.DataSetId;
                    item.DataTypeCode = savedItem.DataTypeCode;
                    item.FilterOperator = savedItem.FilterOperator;
                    item.IsCommonFilter = savedItem.IsCommonFilter;
                    item.Id = savedItem.Id;
                    item.LineNumber = savedItem.LineNumber;
                }
            });

            this.savedFilters.forEach(item => {
                var list = this.EntityPM.DashboardGlobalFilters.filter(d => d.Id == item.Id);
                if (list == null) {
                    this.EntityPM.DashboardGlobalFilters.push(item);
                }
            });
        }
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.GlobalFilters.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);

            if (AppTool.IsNullOrEmpty(item.SelectedFilterType))
                errors.push("Filter type is missing");

            if (item.IsCommonFilter) {
                if (AppTool.IsNullOrEmpty(item.CommonFilterField))
                    errors.push("Filter field is missing");
            }

            else if (item.IsCommonFilter == false) {
                if (AppTool.IsNullOrEmpty(item.DataSetId))
                    errors.push("Dataset is missing");

                if (AppTool.IsNullOrEmpty(item.DataSetFieldId))
                    errors.push("Filter field is missing");
            }

            if (AppTool.IsNullOrEmpty(item.FilterOperator))
                errors.push("Operator is missing");
        });

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

    DeleteButtonClicked() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm";
        confirmWindow.Show("Are you sure you want to permanently delete this dashboard?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Deleting...");
                var service: DashboardPMExtendedService = new DashboardPMExtendedService();
                service.Delete(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                    this.OnDeleteCompleted(myResponse);
                });
            }
        });
    }
    private OnDeleteCompleted(myResponse: ServiceResponse) {
        if (!myResponse.HasError) {
            this.CurrentSession.CloseCurrentWindowEmit("OK_delete");
        }

        else {
            this.ValidationErrorsList = myResponse.ErrorsArray;
        }

        this.CurrentSession.StopBusyIndicator();
    }

    AddFilterClicked() {
        this.maxFiltersLineNumber += 1;
        var filter: DashboardGlobalFilterPM = new DashboardGlobalFilterPM(null);
        filter.Tenant = SessionInfo.LoggedUserTenant;
        filter.DashboardId = this.EntityPM.Id;
        filter.LineNumber = this.maxFiltersLineNumber;
        this.EntityPM.AddDashboardGlobalFilter(filter);

        var filterItem = new GlobalFilterItem(filter, true, this);
        this.GlobalFilters.push(filterItem);
    }
}

export class GlobalFilterItem extends BaseComponent {
    public EntityPM: DashboardGlobalFilterPM;
    public Operators: CodeNameClass[] = [];
    public ObjectTableName: string = "DashboardGlobalFilter";
    public DataContext = this;
    constructor(filter: DashboardGlobalFilterPM, public isNew: boolean, public fatherComponent: AddEditDashboardComponent) {
        super();
        this.EntityPM = filter;

        this.FillOperators(this.EntityPM.DataTypeCode);
        this.SetFilterType();
        this.SetFilterField();
        this.SetOperator();
    }

    private SetFilterType() {
        if (!this.isNew) {
            this.selectedFilterType = new CodeNameClass();
            this.selectedFilterType = this.fatherComponent.FilterTypes.filter(d => d.Code == (this.EntityPM.IsCommonFilter ? "COMN" : "DATA"))[0];
        }
    }
    private SetFilterField() {
        if (!this.isNew && this.EntityPM.IsCommonFilter) {
            this.selectedCommonFilterField = new CodeNameClass();
            this.selectedCommonFilterField = this.fatherComponent.CommonFilterFields.filter(d => d.Code == this.EntityPM.CommonFilterField)[0];
        }
    }

    private SetOperator() {
        if (!this.isNew) {
            this.selectedOperator = new CodeNameClass();
            this.selectedOperator = this.Operators.filter(d => d.Code == this.EntityPM.FilterOperator)[0];
        }
    }

    private FillOperators(dataTypeCode: string) {
        this.Operators = [];
        switch (dataTypeCode) {
            case "DateTime":
            case "Date":
                this.Operators.push(new CodeNameClass("After", "GreaterThan"));
                this.Operators.push(new CodeNameClass("Before", "LessThan"));
                this.Operators.push(new CodeNameClass("Previous", "Previous"));
                this.Operators.push(new CodeNameClass("Current", "Current"));
                this.Operators.push(new CodeNameClass("Next", "Next"));
                this.Operators.push(new CodeNameClass("Between", "Between"));
                break;

            case "Integer":
            case "Decimal":
            case "Double":
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("Does Not Equal", "NotEqual"));
                this.Operators.push(new CodeNameClass("Greater Than", "GreaterThan"));
                this.Operators.push(new CodeNameClass("Less Than", "LessThan"));
                this.Operators.push(new CodeNameClass("Greater Than Or Equal", "GreaterThanOrEqual"));
                this.Operators.push(new CodeNameClass("Less Than Or Equal", "LessThanOrEqual"));
                this.Operators.push(new CodeNameClass("Is Empty", "IsEmpty"));
                this.Operators.push(new CodeNameClass("Is not Empty", "IsNotEmpty"));
                break;

            case "Boolean":
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("Is Empty", "IsEmpty"));
                this.Operators.push(new CodeNameClass("Is not Empty", "IsNotEmpty"));
                break;

            case "LookUp":
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("Does Not Equal", "NotEqual"));
                this.Operators.push(new CodeNameClass("Is Empty", "IsEmpty"));
                this.Operators.push(new CodeNameClass("Is not Empty", "IsNotEmpty"));
                break;

            default:
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("Does Not Equal", "NotEqual"));
                this.Operators.push(new CodeNameClass("Contains", "Contains"));
                this.Operators.push(new CodeNameClass("Does Not Contain", "NotContains"));
                this.Operators.push(new CodeNameClass("Is Empty", "IsEmpty"));
                this.Operators.push(new CodeNameClass("Is not Empty", "IsNotEmpty"));
                break;
        }
    }

    private selectedFilterType: CodeNameClass;
    get SelectedFilterType() { return this.selectedFilterType; }
    set SelectedFilterType(value: CodeNameClass) {
        if (this.selectedFilterType != value) {
            this.selectedFilterType = value;

            this.IsCommonFilter = false;

            if (value != null && value.Code == "COMN")
                this.IsCommonFilter = true;
        }
    }

    private selectedCommonFilterField: CodeNameClass;
    get SelectedCommonFilterField() { return this.selectedCommonFilterField; }
    set SelectedCommonFilterField(value: CodeNameClass) {
        if (this.selectedCommonFilterField != value) {
            this.selectedCommonFilterField = value;

            this.CommonFilterField = null;
            this.DataTypeCode = null;
            this.SelectedOperator = null;

            if (value != null) {
                this.CommonFilterField = value.Code;
                this.DataTypeCode = value.LocalName;
                this.FillOperators(value.LocalName);
            }

            else {
                this.FillOperators(null);
            }
        }
    }

    private selectedOperator: CodeNameClass;
    get SelectedOperator() { return this.selectedOperator; }
    set SelectedOperator(value: CodeNameClass) {
        if (this.selectedOperator != value) {
            this.selectedOperator = value;

            this.FilterOperator = null;

            if (value != null) {
                this.FilterOperator = value.Code;
            }
        }
    }

    get IsCommonFilter() { return this.EntityPM.IsCommonFilter; }
    set IsCommonFilter(value: boolean) {
        if (this.EntityPM.IsCommonFilter != value) {
            this.EntityPM.IsCommonFilter = value;
        }
    }

    get CommonFilterField() { return this.EntityPM.CommonFilterField; }
    set CommonFilterField(value: string) {
        if (this.EntityPM.CommonFilterField != value) {
            this.EntityPM.CommonFilterField = value;            
        }
    }

    get DataSetId() { return this.EntityPM.DataSetId; }
    set DataSetId(value: string) {
        if (this.EntityPM.DataSetId != value) {
            this.EntityPM.DataSetId = value;
            this.DataSetFieldId = null;
        }
    }

    get DataSetFieldId() { return this.EntityPM.DataSetFieldId; }
    set DataSetFieldId(value: string) {
        if (this.EntityPM.DataSetFieldId != value) {
            this.EntityPM.DataSetFieldId = value;
            this.SelectedOperator = null;
        }
    }

    get FilterOperator() { return this.EntityPM.FilterOperator; }
    set FilterOperator(value: string) {
        if (this.EntityPM.FilterOperator != value) {
            this.EntityPM.FilterOperator = value;
        }
    }

    get DataTypeCode() { return this.EntityPM.DataTypeCode; }
    set DataTypeCode(value: string) {
        if (this.EntityPM.DataTypeCode != value) {
            this.EntityPM.DataTypeCode = value;
        }
    }

    get LineNumber() { return this.EntityPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.EntityPM.LineNumber != value) {
            this.EntityPM.LineNumber = value;
        }
    }

    FieldChanged(field: AnalyticsFactsFieldsMetaDataList) {
        if (field) {
            this.FillOperators(field.DataTypeCode);
            this.DataTypeCode = field.DataTypeCode;
        }
        else {
            this.FillOperators(null);
            this.DataTypeCode = null;
        }
    }

    DeleteFilterClick() {
        var index = this.fatherComponent.GlobalFilters.indexOf(this);
        if (index != -1) {
            this.fatherComponent.GlobalFilters.splice(index, 1);
        }

        index = this.fatherComponent.EntityPM.DashboardGlobalFilters.indexOf(this.EntityPM);
        if (index != -1) {
            this.fatherComponent.EntityPM.RemoveDashboardGlobalFilter(this.EntityPM);
        }
    }
}

