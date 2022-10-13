import { Component, EventEmitter, Output } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { AppTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { ApiQueryFiltersBuilder } from 'Workflow/Models/ApiQueryFiltersBuilder';
import { WorkFlowInstanceListService } from 'Workflow/Services/StandardLists/WorkFlowInstanceListService';

const SearchBoxDelayTime = 700;

@Component({
    templateUrl: './RunHistoryWorkflowComponent.html',
})

export class RunHistoryWorkflowComponent extends BaseComponent {
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlowInstance";
    public DataContext: RunHistoryWorkflowComponent = this;
    public AllInstancesCount: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    private SearchText: string = null;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
    }

    ngOnInit() {
        this.BuildColumns();
    }

    LoadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    RefreshButtonClicked() {
        this.LoadData();
    }

    private timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {
            this.timerToken = setTimeout(() => {
                this.SearchText = searchtext;
                this.LoadData();
            }, SearchBoxDelayTime);

        } else {
            this.SearchText = null;
            this.LoadData();
        }
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "StartTime",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            this.CurrentSession.StopBusyIndicator();
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.CurrentSession.StartBusyIndicatorLoading();

        let businessKeyFilterValue = !AppTool.IsNullOrEmpty(this.SearchText) ? (AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText) : null;
        var filters = ApiQueryFiltersBuilder.getWorkflowInstancesApiQueryFilters(this.EntityPM.Id, businessKeyFilterValue);

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = getCount;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        return new Promise((resolve) => {
            var service: WorkFlowInstanceListService = new WorkFlowInstanceListService();
            resolve(service.getByFilters(filters));
        });
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'BusinessKey',
            DataTypeCode: 'String',
            Display: "Business Key",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
        });
        this.columns.push({
            FieldName: 'StartTime',
            DataTypeCode: 'Date',
            Display: "Start Time",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
            HtmlListComponentName: 'FieldTemplateComponent',
            HtmlListComponentUrl: './Workflow/Components/Templates/FieldTemplateComponent',
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'Duration',
            DataTypeCode: 'Date',
            Display: "Duration",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'StatusName',
            DataTypeCode: 'String',
            Display: "Status",
            HtmlListComponentName: 'FieldTemplateComponent',
            HtmlListComponentUrl: './Workflow/Components/Templates/FieldTemplateComponent',
            IsCustomTemplate: true,
            Styles: { width: '400px' },
        });
    }

    onRowSelected($event) {
        if ($event != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = "Instance Activities";
            logWindow.IsShowCloseButton = true
            var windowArgs: any = {};
            windowArgs.EntityId = $event.rowData.Id;
            windowArgs.ObjectTableName = "WorkFlowInstanceActivity";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Workflow/Components/CreateEditWorkflow/WorkflowInstanceActivityComponent');
        }
    }
}