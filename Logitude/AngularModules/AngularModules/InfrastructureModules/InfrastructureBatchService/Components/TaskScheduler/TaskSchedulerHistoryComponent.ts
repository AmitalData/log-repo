import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { InfrastructureDomainService } from '../../../../Infrastructure/Services/InfrastructureDomainService';
import { TaskSchedulerHistoryList } from '../../../../Infrastructure/EntityLists/TaskSchedulerHistoryList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../../Infrastructure/Tools';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';

@Component({
    selector: 'Scheduler-History',
    
    templateUrl: './TaskSchedulerHistoryComponent.html',
    inputs: ['SelectedRowChanged', 'ShowUTCTimes']
})

export class TaskSchedulerHistoryComponent implements OnInit {
    public HistoryItemsSource: TaskSchedulerHistoryList[] = [];
    public columns: any[] = null;
    @Output() CustomColumnsReady = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    public SelectedRowChanged: EventEmitter<any>;
    public ShowUTCTimes: EventEmitter<any>;
    ShowUTCTimeEnabled: boolean = false;
    public IsHistoryGridVsisible = false;
    public SelectedRow: any;
    constructor(private _entityListService: EntityListService) {
    }

    ngOnInit() {
        this.LoadTaskHistories();
        if (this.SelectedRowChanged) {
            this.SelectedRowChanged.subscribe((res) => {
                if (res == null) {
                    this.IsHistoryGridVsisible = false;
                }
                else{
                    this.SelectedRow = res;
                    this.LoadTaskHistories();
                }
            });
        }
        if (this.ShowUTCTimes) {
            this.ShowUTCTimes.subscribe((res) => {
                if (res) {
                    this.ShowUTCTimeEnabled = true;
                }
                else {
                    this.ShowUTCTimeEnabled = false;
                }
                this.LoadTaskHistories();
            });
        }
    }

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: "Log",
            DataTypeCode: 'String',
            Display: 'Log',
            Styles: { width: '280px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        if (this.ShowUTCTimeEnabled == false) {
            this.columns.push({
                FieldName: "StartDateTime",
                DataTypeCode: 'String',
                Display: 'Start Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "StartDateTime"
            });
            this.columns.push({
                FieldName: "EndDateTime",
                DataTypeCode: 'String',
                Display: 'End Date',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "EndDateTime"
            });
        }
        else {
            this.columns.push({
                FieldName: "StartDateTimeUTC",
                DataTypeCode: 'String',
                Display: 'Start Date UTC',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "StartDateTimeUTC"
            });
            this.columns.push({
                FieldName: "EndDateTimeUTC",
                DataTypeCode: 'String',
                Display: 'End Date UTC',
                Styles: { width: '200px' },
                HtmlListComponentName: 'SchedulerDateListTemplate',
                HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "EndDateTimeUTC"
            });
        }
        this.columns.push({
            FieldName: "Duration",
            DataTypeCode: 'String',
            Display: 'Duration',
            Styles: { width: '100px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        this.columns.push({
            FieldName: "ViewLog",
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '100px' },
            HtmlListComponentName: 'SchedulerDateListTemplate',
            HtmlListComponentUrl: '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/ListTemplates/SchedulerDateListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: false
        });
        /////
        //this.columns.push({
        //    FieldName: "RunResult",
        //    DataTypeCode: 'String',
        //    Display: 'Run Result',
        //    Styles: { width: '200px' },
        //    IsCustomTemplate: true,
        //    ServerSideSortable: false
        //});

        this.CustomColumnsReady.emit(this.columns);
    }

    DataSource = {
        pageSize: 20,
        rowCount: null,

        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        filters = new ApiQueryFilters();
        if (!sortingCol) {
            sortingCol = "StartDateTimeUTC";
            sortingDir = "descending";
        }
        if (!this.SelectedRow) {
            filters.addAdditionalFilter("TaskId", "0-0", null, null, "Equals", false, false, false, "String");
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
                if (filters.AdditionalFilters.filter(a => a.FieldName == "TaskId").length > 0) {
                    filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "TaskId");
                }
                filters.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Equals", false, false, false, "String");
            }
        }

        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        if (sortingCol) {
            filters.SortBy = sortingCol;
        }
        if (sortingDir) {
            filters.SortDirection = sortingDir;
        }
        filters.Tenant = SessionLocator.Tenant;

        return this._entityListService.getByFilters("TaskSchedulerHistory", filters);
    }

    private LoadTaskHistories() {
        this.IsHistoryGridVsisible = true;
        this.BuildColumns();

        var filterAgrs = new ApiQueryFilters();
        if (!this.SelectedRow) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedRow.Id)) {
            filterAgrs.addAdditionalFilter("TaskId", this.SelectedRow.Id, null, null, "Contains", true, false, false, "String");
        }

        this.MenuHeaderchangeevent.emit({ Filters: filterAgrs, IgnoreFilter: false });
    }

}
