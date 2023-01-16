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
declare var window: any;

@Component({
    templateUrl: './CustomizationQueriesComponent.html',
})

export class CustomizationQueriesComponent {
    public CustomQueriesCollection: ObservableCollection;
    private QueriesPMService: QueriesPMService;
    private AllQueries: QueryPM[] = [];
    public IsAddButtonEnabled: boolean = true;
    public AllowQueries: boolean = true;
    public customizationEditComponent: CustomizationEditComponent;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        this.CustomQueriesCollection = new ObservableCollection([]);
        this.QueriesPMService = new QueriesPMService();
        let serviceArgs = new ServiceArgs();
        serviceArgs.http = ServiceHelper.HttpClient;
        this.QueriesPMService.setServiceArgs(serviceArgs);
    }

    private ObjectTableId: string;
    private ObjectTable: any;
    SetWindowArgs(args: any) {
        this.ObjectTableId = args['ObjectTableId'];
        this.ObjectTable = window.ObjectTables.filter(d => d.Id === this.ObjectTableId)[0];
        if (!this.ObjectTable) return;
        this.LoadCustomQueries();
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

    Save() {
        if (!this.customizationEditComponent.IsSaveAndClose) return;
        this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
        this.customizationEditComponent.IsSaveAndClose = false;
    }

    Cancel() {

    }
}
