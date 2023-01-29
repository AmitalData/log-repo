import { Component, EventEmitter, Output } from '@angular/core';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { ObjectFieldPMExtendedService } from 'Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { AppTool } from 'Infrastructure/Tools';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { ApiQueryFiltersBuilder } from 'Workflow/Utilities/ApiQueryFiltersBuilder';
import { WorkFlowInstanceListService } from 'Workflow/Services/StandardLists/WorkFlowInstanceListService';

const SearchBoxDelayTime = 700;

@Component({
    templateUrl: './RunHistoryWorkflowComponent.html',
})

export class RunHistoryWorkflowComponent extends BaseComponent {
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    public DataSource: any;
    public DataContext: RunHistoryWorkflowComponent = this;
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlowInstance";
    public AllInstancesCount: number = 0;

    public QueryColumns: QueryColumnPM[] = [];
    public columns: any[] = null;

    private CurrentSession = SessionLocator.SelectedSession;
    private SearchText: string = null;
    private VersionIds: string[];

    public StartTimeObjectfield: any;
    public Filters: ApiQueryFilters;
    public FilterValue1: any;
    public FilterValue2: any;
    public FilterOperator: string;
    public IsDateFilter: boolean;
    private TabSelectedEvent: any = null;

    public LogitudeGridExportToExcelComponent: LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
    }

    ngOnInit() {
        this.BuildColumns();
        this.SetDataSource();
        this.BuildQueryColumns();
        this.GetStartTimeObjectFields();
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "WFRH") {
                    this.LoadData()
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    LoadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    RefreshButtonClicked() {
        this.LoadData();
    }

    GetStartTimeObjectFields() {
        var objectFieldPMExtendedService = new ObjectFieldPMExtendedService()
        objectFieldPMExtendedService.GetObjectFieldsByObjectTable(this.ObjectTableName).subscribe((response: any) => {
            if (!response) return;
            this.StartTimeObjectfield = response.find((e: { FieldName: string; }) => e.FieldName == 'StartTime')
        });
    }

    SetDataSource() {
        this.DataSource = {
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
    }

    private timerToken: any;
    TextChanged(searchtext: string) {
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

    getRows(skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.CurrentSession.StartBusyIndicatorLoading();
        var versionsIdList = this.EntityPM.WorkFlowVersions.map(v => v.Id);

        let businessKeyFilterValue = !AppTool.IsNullOrEmpty(this.SearchText) ? (AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText) : null;
        this.Filters = ApiQueryFiltersBuilder.getWorkflowInstancesByVersionApiQueryFilters(versionsIdList, businessKeyFilterValue);

        if (this.IsDateFilter) {
            this.Filters.addAdditionalFilter(this.StartTimeObjectfield.FieldName, this.FilterValue1, this.FilterValue2, null, this.FilterOperator, false, false, false, this.StartTimeObjectfield.dataTypeCode)
        }

        this.Filters.PageSize = take;
        this.Filters.PageIndex = skip;
        this.Filters.GetAll = false;
        this.Filters.GetCount = true;
        this.Filters.SortBy = sortingCol;
        this.Filters.SortDirection = sortingDir;
        return new Promise((resolve) => {
            var service: WorkFlowInstanceListService = new WorkFlowInstanceListService();
            resolve(service.getByFilters(this.Filters));
        });
    }

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
            FieldName: 'WorkFlowVersionNumber',
            DataTypeCode: 'String',
            Display: "Version Number",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'StartTime',
            AdditionalDataCustom: this.ObjectTableName,
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
            AdditionalDataCustom: this.ObjectTableName,
            DataTypeCode: 'String',
            Display: "Status",
            HtmlListComponentName: 'FieldTemplateComponent',
            HtmlListComponentUrl: './Workflow/Components/Templates/FieldTemplateComponent',
            IsCustomTemplate: true,
            Styles: { width: '400px' },
        });
    }

    BuildQueryColumns() {
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("BusinessKey", 'String', "Business Key"));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("StartTime", 'Date', "Start Time"));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Duration", 'Date', "Duration"));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("StatusName", 'String', "Status"));
    }

    onRowSelected($event: { rowData: { Id: any; }; }) {
        let isVariableHasPermission = FeatureLocator.HasFeaturePermession("WorkFlowInstance", "WorkFlowInstance.ShowVariables")
        if ($event != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = isVariableHasPermission ? 690 : 570;
            logWindow.Title = "Instance Activities" + (isVariableHasPermission ? " And Variables" : '');
            logWindow.IsShowCloseButton = true
            var windowArgs: any = {};
            windowArgs.EntityId = $event.rowData.Id;
            windowArgs.ObjectTableName = "WorkFlowInstanceActivity";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Workflow/Components/CreateEditWorkflow/WorkflowInstanceActivityComponent');
        }
    }

    public ExportToExcel() {
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute(this.ObjectTableName, this.Filters, this.QueryColumns);
    }

    setDateFilter(event: any) {
        if (event) {
            if (event.FromDate && event.ToDate) {
                this.FilterValue1 = event.FromDate;
                this.FilterValue2 = event.ToDate;
                this.FilterOperator = "Between";
                this.IsDateFilter = true
            } else if (event.Date) {
                this.FilterValue1 = event.Date;
                this.FilterValue2 = null;
                this.FilterOperator = event.Operation;
                this.IsDateFilter = true
            } else if (event == "NoDate") {
                this.IsDateFilter = false
            }
            this.LoadData();
        } else {
            this.IsDateFilter = false
            this.LoadData();
        }
    }
}