import { Component } from '@angular/core';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodePM } from '../../../../Infrastructure/EntityPMs/TextCodePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomizationEditComponent } from './CustomizationEditComponent';
import { QueriesPMService } from '../../../../Infrastructure/Services/StandardPMs/QueriesPMService';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { QueryPM } from '../../../../Infrastructure/EntityPMs/QueryPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ScreenLayoutComponent } from './ScreenLayoutComponent';
import { ObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/ObjectTablePMService';
declare var window: any;

@Component({
    templateUrl: './CustomizationQueriesComponent.html',
})

export class CustomizationQueriesComponent extends BaseComponent{
    public CustomQueriesCollection: ObservableCollection;
    private QueriesPMService: QueriesPMService;
    private ObjectTablePMService: ObjectTablePMService;
    private AllQueries: QueryPM[] = [];
    public IsAddButtonEnabled: boolean = true;
    public AllowQueries: boolean = true;
    public customizationEditComponent: CustomizationEditComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ScreenLayoutComponent: ScreenLayoutComponent;
    NewScreenFilterItems: ApiQueryFilters;
    private newScreenList: any[] = [];
    constructor() {
        super();
        this.CustomQueriesCollection = new ObservableCollection([]);
        this.QueriesPMService = new QueriesPMService();
        this.ObjectTablePMService = new ObjectTablePMService();
        let serviceArgs = new ServiceArgs();
        serviceArgs.http = ServiceHelper.HttpClient;
        this.QueriesPMService.setServiceArgs(serviceArgs);
    }
    private NewWizardControlNameValue;
    NewScreensSelectionChanged(selectedNewScreen: any) {
        if (!selectedNewScreen) return;
        if (this.SelectedScreen?.Code == selectedNewScreen.Code) return;
        this.NewWizardControlNameValue = selectedNewScreen ? selectedNewScreen.Code : "";
        this.customizationEditComponent.IsDirty = true;
    }

    get SelectedScreen() {
        return this.newScreenList.filter(screen => screen.Code == this.ObjectTable.NewWizardControlName)[0];
    }

    public SelectedNewScreenId: string;
    SetSelectedNewScreen() {
        this.SelectedNewScreenId = this.SelectedScreen?.Id;
    }

    InitLOVFilters() {
        this.NewScreenFilterItems = new ApiQueryFilters();
        this.NewScreenFilterItems.addAdditionalFilter("ObjectTableId", this.ObjectTableId, null, null, "Equals", true, false, false, "string");
        this.NewScreenFilterItems.Tenant = SessionLocator.Tenant;
    }

    FillNewScreenList() {
        this.newScreenList = window.Screens.filter(screen => screen.ObjectTableId == this.ObjectTableId && !screen.Inactive && screen.Tenant == SessionLocator.Tenant);
    }

    private NewWizardControlNameOldValue: string;
    private ObjectTableId: string;
    private ObjectTable: any;
    SetWindowArgs(args: any) {
        this.ObjectTableId = args['ObjectTableId'];
        this.ObjectTable = window.ObjectTables.filter(d => d.Id === this.ObjectTableId)[0];
        if (!this.ObjectTable) return;
        this.NewWizardControlNameOldValue = this.ObjectTable.NewWizardControlName;
        this.LoadCustomQueries();
        this.FillNewScreenList();
        this.InitLOVFilters();
        this.SetSelectedNewScreen();
    }

    public LoadCustomQueries() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.QueriesPMService.getAllSystemViewsByObjectTable(this.ObjectTable.Name).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError && serviceResponse.Result) {
                this.BuildItemsSource(serviceResponse.Result);
            }
            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private BuildItemsSource(queries) {
        this.AllQueries = queries;
        this.CustomQueriesCollection = new ObservableCollection(queries);
        this.IsAddButtonEnabled = true;
        this.CurrentSession.StopBusyIndicator();
    }

    GetSelectedDefaultViewName(query) {
        let DefaultViewNameTextCodeCode = "";
        if (query) {
            DefaultViewNameTextCodeCode = this.AllQueries.filter(quer => quer.IsDefault && quer.Id != query.Id)[0]?.NameTextCodeCode;
        }
        else {
            DefaultViewNameTextCodeCode = this.AllQueries.filter(quer => quer.IsDefault)[0]?.NameTextCodeCode;
        }

        if (AppTool.IsNullOrEmpty(DefaultViewNameTextCodeCode)) {
            return DefaultViewNameTextCodeCode;
        }
        return TextCodeTranslator.Translate(DefaultViewNameTextCodeCode);
    }

    AddCustomView() {
        let windowArgs: any = {};
        windowArgs.queryCode = "";
        windowArgs.queryId = "";
        windowArgs.currentObjectTable = this.ObjectTable.Name;
        windowArgs.IsNew = true;
        windowArgs.CreateWithoutOriginalQuery = true;
        windowArgs.IsFromCustomization = true;
        windowArgs.SelectedDefaultViewName = this.GetSelectedDefaultViewName(null);

        let logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.O.CreateNewView");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event != "") {
                CachedDataManager.RefreshTenantTextCodes().subscribe((response: any) => {
                    this.AddEditCustomViewWindowClosed($event);
                });
            }
        });
    }

    AddEditCustomViewWindowClosed(queryUniqueCode) {
        let query = window.Queries.filter(q => q.UniqueCode == queryUniqueCode)[0];
        if (!query.IsDefault) {
            this.LoadCustomQueries();
            return;
        }
        let oldDefaultQuery = window.Queries.filter(q => q.ObjectTableId == this.ObjectTableId && q.SystemLevel && q.IsDefault && q.UniqueCode != queryUniqueCode)[0];
        if (!oldDefaultQuery) {
            this.LoadCustomQueries();
            return;
        }
        window.Queries = window.Queries.filter(q => q.UniqueCode != oldDefaultQuery.UniqueCode);
        oldDefaultQuery.IsDefault = false;
        window.Queries.push(oldDefaultQuery);
        this.LoadCustomQueries();
    }
    RemoveIsDefaultQuery(Query: any) {
        Query.IsDefault = false;
        this.updateQuery(Query);
    }
    updateQuery(Query: any) {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.QueriesPMService.update(Query).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError || !serviceResponse.Result) {
                this.CurrentSession.StopBusyIndicator();
                return;
            }
            window.Queries = window.Queries.filter(q => (q.UniqueCode != Query.UniqueCode));
            window.Queries.push(Query);
            this.LoadCustomQueries();
        });
    }
    IsDefaultClicked(ClickedQuery: any, event: boolean) {
        if (!event) {
            this.RemoveIsDefaultQuery(ClickedQuery);
            return;
        }
        let oldDefaultQuery = window.Queries.filter(q => (q.ObjectTableId == this.ObjectTableId) && q.SystemLevel && q.IsDefault);
        if (oldDefaultQuery.length == 0) {
            ClickedQuery.IsDefault = true;
            this.updateQuery(ClickedQuery);
        }
        else {
            oldDefaultQuery = window.Queries.filter(q => q.ObjectTableId == this.ObjectTableId && q.SystemLevel && q.IsDefault)[0];
            ClickedQuery.IsDefault = true;
            oldDefaultQuery.IsDefault = false;
            this.updateQuery(ClickedQuery);
            this.updateQuery(oldDefaultQuery);
        }
    }

    EditCustomView(item) {
        let windowArgs: any = {};
        windowArgs.queryId = item.Id;
        windowArgs.queryCode = item.UniqueCode;
        windowArgs.currentObjectTable = this.ObjectTable.Name;
        windowArgs.IsNew = false;
        windowArgs.IsFromCustomization = true;
        windowArgs.QueryName = TextCodeTranslator.Translate(item.NameTextCodeCode);
        windowArgs.SelectedDefaultViewName = this.GetSelectedDefaultViewName(item);

        let logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.EditView");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == item.UniqueCode) {
                CachedDataManager.RefreshTenantTextCodes().subscribe((response: any) => {
                    this.AddEditCustomViewWindowClosed($event);
                });
            }
        });
    }

    private OkClicked() {
        if (this.NewWizardControlNameValue == this.NewWizardControlNameOldValue) return;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.ObjectTable.NewWizardControlName = this.NewWizardControlNameValue;
        this.ObjectTablePMService.update(this.ObjectTable)
            .subscribe(arg => {
                this.CurrentSession.StopBusyIndicator();
                this.customizationEditComponent.IsDirty = false;
                }, error => {
                alert("error happened!");
            });
        this.NewWizardControlNameOldValue = this.NewWizardControlNameValue;
        if (this.customizationEditComponent.NewSelectedMenu) {
            this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
        }
    }
    Save() {
        if (!this.customizationEditComponent.IsDirty) return;
        this.OkClicked();
        if (this.customizationEditComponent.IsSaveAndClose) {
            this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
            this.customizationEditComponent.IsSaveAndClose = false;
        }
    }

    Cancel() {
        if (this.customizationEditComponent.NewSelectedMenu) {
            this.UndoChanges();
            this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
        }
    }

    private UndoChanges() {
        this.customizationEditComponent.IsDirty = false;
        this.SetSelectedNewScreen();
    }
}
