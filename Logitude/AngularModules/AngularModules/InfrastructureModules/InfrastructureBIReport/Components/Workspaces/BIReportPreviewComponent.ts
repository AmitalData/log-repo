import { Component, ComponentRef, OnInit, ViewChild, Output, EventEmitter} from '@angular/core';
//import { AgGridModule } from "ag-grid-angular/main";
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { DateTimePipe } from '../../../../Controls/Pipes/DateTimePipe';
import { AgGridNg2 } from 'ag-grid-angular/main';
import { InfrastructureDomainService, BIReportXMLData, BITabularViewSettings, Column } from '../../../../Infrastructure/Services/InfrastructureDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { DWObjectFieldsDetails } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';

@Component({
    moduleId: module.id,
    templateUrl: 'BIReportPreviewComponent.html',
})

export class BIReportPreviewComponent implements OnInit {
    @ViewChild('agGrid') agGrid: AgGridNg2;
    public ComponentRef: ComponentRef<BIReportPreviewComponent>;
    public EntityPM: BIReportPM = null;
    public EntityId: string;
    public DWQueryId: string;
    public DWQueryData: DWQueryData;
    SelectedFiltersDataSource: any[] = [];
    public _DWSubQueryPMService: DWSubQueryPMService;
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper
    public _BIReportPMService: BIReportPMService;
    public _InfrastructureDomainService: InfrastructureDomainService;
    public columnDefs: any[] = [];
    public rowData: any[] = [];
    public BIReportName = "";
    @Output() RunReportCommand = new EventEmitter();
    private _EntityPMService: EntityPMService = new EntityPMService();
    private timerToken: any;
    public BIReportXMLData: BIReportXMLData = null;

    constructor() {
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
    }
    ngOnInit() {
        this._DWSubQueryPMService.getByQueryId(this.DWQueryId).subscribe(myResult => {
            if (!myResult.HasError) {
                this.DWQueryData = myResult.Result;
                if (this.DWQueryData.Filters) {
                    var MyFilter = this._DWQueryBuilderHelper.RestoreFilters(this.DWQueryData.Filters);
                    var temp = [];
                    temp.push(MyFilter);
                    this.SelectedFiltersDataSource = temp;
                }
            }
        });
        this.LoadBIReportData();
    }
    public Run(args: any) {
        this.InitializeServices();
        this.DWQueryId = args['DWQueryId'];
        this.EntityId = args['EntityId'];
    }
    InitializeServices() {
        this._InfrastructureDomainService = new InfrastructureDomainService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        this._BIReportPMService = new BIReportPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService();
    }

    //#region Build ag-grid Data
    private IsSorting = false;
    private IsResizing = false;
    private ReportXML: any;
    public LoadBIReportData() {
        if (this.DWQueryId != null) {
            this._InfrastructureDomainService.GetByBIReportId(this.EntityId, this.DWQueryId).subscribe(myResult => {
                if (!myResult.HasError) {
                    var result: BIReportXMLData = myResult.Result;
                    this.ReportXML = result;
                    this.BIReportXMLData = result;
                    this.EntityPM = result.BIReportPM;
                    this.BIReportName = this.EntityPM != null ? this.EntityPM.Name : "";
                    this.BuildColumns(result);
                }
            });
        }
    }
    public UpdateAGGrid(arg: BIReportXMLData) {
        var sortsList = [];
        if (arg.BITabularViewSettings != null && arg.BITabularViewSettings.Columns != null) {
            arg.BITabularViewSettings.Columns.forEach(item => {
                if (item != null && item.Code != null) {
                    if (item != null && item.SortDirction != null) {
                        sortsList.push({
                            colId: item.Code,
                            sort: item.SortDirction,
                            order: item.SortOrder
                        });
                    }
                }
            });
            if (sortsList != null) {
                sortsList = sortsList.sort((a, b) => { return (a.order === b.order) ? 0 : (a.order < b.order) ? -1 : 1 });
                this.IsSorting = true;
                this.agGrid.api.setSortModel(sortsList);
            }
            //var params = {
            //    force: true,
            //};
            this.agGrid.api.refreshCells();
        }
    }
    public BuildColumns(arg: BIReportXMLData) {
        this.columnDefs = [];
        var columns = arg.BITabularViewSettings.Columns.sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 });
        if (columns != null) {
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].IsChecked) {
                    if (columns[i].DataTypeCode == "DateTime") {
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Name,
                            field: columns[i].Code,
                            sortable: true,
                            width: columns[i].Width,
                            resizable: true,
                            cellRenderer: this.DateCellRenderer,
                            cellClass: columns[i].DataTypeCode,
                            //filter: 'agTextColumnFilter',
                            pivotIndex: columns[i].Index,

                        });                            //sort: sortingDirction,
                    }
                    else {
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Name,
                            field: columns[i].Code,
                            sortable: true,
                            filter: true,
                            width: columns[i].Width,
                            resizable: true,
                            cellClass: columns[i].DataTypeCode,
                            pivotIndex: columns[i].Index,
                            //sort: sortingDirction,
                        });
                    }
                }
            }
        }
       
        this.BuildRows(arg);
    }
    public BuildRows(arg: BIReportXMLData) {
        this.rowData = [];
       
        if (arg.DWQueryData != null && arg.DWQueryData.SubQueryData != null && arg.DWQueryData.SubQueryData.SQLString != null) {
            this.StartBusyIndicator("Loading ..");
            this._DWQueryBuilderService.GetDWQueryData(arg.DWQueryData.SubQueryData.SQLString, "Fact_Shipments").subscribe(myResult => {
                if (!myResult.HasError) {
                    this.rowData = myResult.Result;
                    this.timerToken = setTimeout(() => this.UpdateAGGrid(arg), 500);
                    this.StopBusyIndicator();
                }
                else {
                    this.StopBusyIndicator();
                }
            });
        }
        else {
            this.StopBusyIndicator();
        }
    }
    private DateCellRenderer(params: any) {
        var DatePipe = new DateTimePipe();
        return DatePipe.transform(params.value, "SD");
    }
    //#endregion

    private hasChanged = false;
    get HasChanges() {
        if (this.EntityId == null || (this.EntityPM != null && this.EntityPM.IsDirty) || this.hasChanged) {
            return true;
        }
        return false;
    }

    get IsNewEntity() {
        var isNew = this.EntityPM != null ? false : true;
        return isNew;
    }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    WidthBusyIndicator: number;
    public StartBusyIndicator(message: string = "Generating...", width: number = 200) {
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
        this.WidthBusyIndicator = width;
    }
    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    //#region ag-grid Events 
    private gridApi;
    private gridColumnApi;
    onGridReady(params) {
        this.gridApi = params.api;
        this.gridApi.hideOverlay()
        this.gridColumnApi = params.columnApi;

    }
    onSortChanged(params) {
        if (!this.IsSorting)
            this.hasChanged = true;
        else {
            this.hasChanged = false;
            this.IsSorting = false;
        }
    }
    onColumnResized(params) {
        if (params != null && params.finished === true) {
            if (!this.IsResizing)
                this.hasChanged = true;
            else {
                this.hasChanged = false;
                this.IsResizing = false;
            }
        }
    }
    onColumnMoved(params) {
        if (!this.IsResizing)
            this.hasChanged = true;
        else {
            this.hasChanged = false;
            this.IsResizing = false;
        }
    }
    //#endregion

    //#region Export To Excel
    ExportToExcelAGGridClicked() {
        var params = {
            allColumns: true,
            columnSeparator: ',',
            skipHeader: false,
            skipFooters: true,
            skipGroups: true,
            columnGroups: true,
            sheetName: "sheetName"
        };
        this.agGrid.api.exportDataAsExcel(params);
    }
    ExportToExcelClicked() {
        var windowArgs: any = {};
        windowArgs.queryId = this.DWQueryId;
        windowArgs.reportId = this.EntityPM.Id;
        windowArgs.reportName = this.EntityPM.Name;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/ExportBI2ExcelControl/ExportBI2ExcelControl');
    }
    //#endregion

    //#region Buttons Commands
    BackButtonClicked() {
        if (this.HasChanges) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = "Don't Save";
            confirmWindow.YesButtonText = "Save ";
            confirmWindow.CancelButtonText = "Cancel";
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show("This Report has unsaved changes. Do you want to save it?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    // save
                    this.SaveBIReport();
                }
                else if (confirmWindow.No) {
                    if (this.ComponentRef) {
                        this.ComponentRef.destroy();
                    }
                }
                else if (confirmWindow.Cancel) {
                    // nth
                }
            });
        }
        else {
            if (this.ComponentRef) {
                SessionLocator.CurrentSession.FireEvent("BIRefresh");
                this.ComponentRef.destroy();
            }
        }
    }
    SaveBIReport() {
        if (this.EntityId == null) {
            this.NewBIReport();
        }
        else {
            this.UpdateBIReport(false);
        }
    }
    NewBIReport() {
        var windowTitle = "New BI Report";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        var windowArgs: any = {};
        windowArgs.DWQueryId = this.DWQueryId;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewBIReportWindowClosed($event));
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                this.EntityPM = s.EntityPM;
                this.EntityId = s.EntityPM.Id;
                this.BIReportName = this.EntityPM != null ? this.EntityPM.Name : "";
                this.UpdateBIReport(false);
            });
        });
    }
    UpdateBIReport(arg: boolean) {
        // call method in server side to build xml 
        var sorting = this.agGrid.api.getSortModel();
        var coulmns = this.agGrid.columnApi.getColumnState();
        this.StartBusyIndicator("Saving ...");
        var result = new BIReportXMLData();
        result.BITabularViewSettings = new BITabularViewSettings();
        result.BIReportId = this.EntityId;
        result.BIReportPM = this.EntityPM;
        result.BITabularViewSettings.Columns = [];
        var newColumn = new Column();

        this.BIReportXMLData.DWQueryData.Columns.forEach(item => {
            newColumn = new Column();
            var agCol = coulmns != null ? coulmns.filter(x => x.colId === item.DisplayName.replace('[', '').replace(']', ''))[0] : null;
            newColumn.Index = this.BIReportXMLData.DWQueryData.Columns.indexOf(item);
            newColumn.DataTypeCode = item.DataTypeCode;
            newColumn.IsChecked = false;
            newColumn.Name = item.Name;
            if (agCol != null) {
                newColumn.IsChecked = true;
                newColumn.Index = coulmns.indexOf(agCol);
                newColumn.Width = agCol["width"];
                newColumn.Code = agCol["colId"];
            }
            if (sorting != null) {
                var sortItem = sorting.filter(x => x.colId === item.DisplayName.replace('[', '').replace(']', ''))[0];
                if (sortItem != null) {
                    newColumn.SortDirction = sortItem["sort"];
                    newColumn.SortOrder = sorting.indexOf(sortItem);
                }
            }
            result.BITabularViewSettings.Columns.push(newColumn);
        });

        this._InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(myResult => {
            this.hasChanged = false;
            this.StopBusyIndicator();
            if (arg) {
                this.ShowQueryBuilder();
            }
        });
    }
    OnNewBIReportWindowClosed(arg: string) {
        if (arg != 'cancel') {
            this.EntityId = arg;
        }
    }
    ShowQueryBuilderClicked() {
        if (this.HasChanges) {
            this.UpdateBIReport(true);
        }
        else {
            this.ShowQueryBuilder();
        }
    }
    ShowQueryBuilder() {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DWQueryId = this.DWQueryId;
        windowArgs.IsBIReportEditScreen = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null && d != null && d != "cancel") {
                    this.DWQueryId = s.ID;
                    this.LoadBIReportData();
                }
            });
        });
    }
    EditBIReportClicked() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityPM.Id, ObjectTableName: 'BIReport' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this._EntityPMService.getSingle( "BIReport",this.EntityPM.Id).then((res: any) => {
                            res.subscribe((aa: any) => {
                                this.BIReportName = aa.Result != null ? aa.Result.Name : "";
                            })
                        });
                    });
                });
        }
    }
    RunReportButtonClicked() {
        this.RunReportCommand.emit(this.DWQueryData);
    }
    OnRunReportComplete(MyData) {
        this.rowData = MyData;
        this.timerToken = setTimeout(() => this.UpdateAGGrid(this.ReportXML), 500);
    }
    CountClicked() {
        alert("Count : " + this.agGrid.api.getDisplayedRowCount());
    }
    onBIReportColumnsClick() {
        if (!this.IsNewEntity) {
            var windowTitle = "Show/Hide/Reorder Column(s)";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 350;
            logWindow.Height = 400;
            logWindow.Title = windowTitle;
            var windowArgs: any = {};
            windowArgs.father = this;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewBIReportWindowClosed($event));
            logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/AgGridColumnsOperations');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    if (s != null && d != "cancel") {
                        this.BuildColumns(this.BIReportXMLData);
                    }
                });
            });
        }
    }
    //#endregion
}
