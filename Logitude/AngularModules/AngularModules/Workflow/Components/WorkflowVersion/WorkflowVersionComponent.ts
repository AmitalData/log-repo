import { Component, EventEmitter, Output } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { AppTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowVersionPM } from 'Workflow/EntityPMs/WorkFlowVersionPM';
import { ApiQueryFiltersBuilder } from 'Workflow/Utilities/ApiQueryFiltersBuilder';
import { WorkFlowVersionListService } from 'Workflow/Services/StandardLists/WorkFlowVersionListService';
import { WorkFlowPMService } from 'Workflow/Services/StandardPMs/WorkFlowPMService';
import { WorkFlowVersionPMService } from 'Workflow/Services/StandardPMs/WorkFlowVersionPMService';

@Component({
    templateUrl: './WorkflowVersionComponent.html',
})

export class WorkflowVersionComponent extends BaseComponent {
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    public EntityPM: WorkFlowPM;
    public ClickedVersion: WorkFlowVersionPM;
    public HasChanges = false;
    public ObjectTableName: string = "WorkFlowVersion";
    public DataContext: WorkflowVersionComponent = this;
    public AllInstancesCount: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    public WorkFlowPMService: WorkFlowPMService = new WorkFlowPMService();
    public WorkFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public BusyIndicatorWidth: number = 200;

    private TabSelectedEvent: any = null;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
    }

    ngOnInit() {
        this.BuildColumns();
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "WFVR") {
                    this.LoadData()
                }
            });
        }

        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "WorkflowVersionsUpdated") {
                        this.LoadData()
                    }
                }
            );
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    LoadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "VersionNumber",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            this.CurrentSession.StopBusyIndicator();
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.CurrentSession.StartBusyIndicatorLoading();

        var filters = ApiQueryFiltersBuilder.getWorkflowVersionApiQueryFilters(this.EntityPM.Id);

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = getCount;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        return new Promise((resolve) => {
            var service: WorkFlowVersionListService = new WorkFlowVersionListService();
            resolve(service.getByFilters(filters));
        });
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'VersionNumber',
            DataTypeCode: 'Int',
            Display: "Version",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'Description',
            DataTypeCode: 'String',
            Display: "Description",
            IsCustomTemplate: true,
            Styles: { width: '400px' }
        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'Date',
            Display: "CreateDate",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
            AdditionalDataCustom: this.ObjectTableName,
            HtmlListComponentName: 'FieldTemplateComponent',
            HtmlListComponentUrl: './Workflow/Components/Templates/FieldTemplateComponent',
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'StatusName',
            DataTypeCode: 'String',
            Display: "Status",
            AdditionalDataCustom: this.ObjectTableName,
            HtmlListComponentName: 'FieldTemplateComponent',
            HtmlListComponentUrl: './Workflow/Components/Templates/FieldTemplateComponent',
            IsCustomTemplate: true,
            Styles: { width: '400px' },
        });
        this.columns.push({
            FieldName: 'ActivatedDate',
            DataTypeCode: 'Date',
            Display: "Last Activated Date",
            IsCustomTemplate: true,
            Styles: { width: '400px' },
            AdditionalDataCustom: this.ObjectTableName,
            HtmlListComponentName: 'FieldTemplateComponent',
            HtmlListComponentUrl: './Workflow/Components/Templates/FieldTemplateComponent',
            ServerSideSortable: true
        });
    }

    onRowSelected($event) {
        this.ClickedVersion = $event.rowData
        this.HasChanges = true;
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, UpdatedVersion: null }
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, ClickedVersionRow: $event.rowData.Id }
        this.CurrentSession.CurrentEditComponent.SetSelectedTabByCode("WFFB");
    }

    startBusyIndicator(message: string) {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
    }

    stopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
}