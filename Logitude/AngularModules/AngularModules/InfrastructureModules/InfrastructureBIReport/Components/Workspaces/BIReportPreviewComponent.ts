import { Component, ComponentRef, OnInit, ViewChild} from '@angular/core';
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
    public _DWSubQueryPMService: DWSubQueryPMService;
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _BIReportPMService: BIReportPMService;
    public _InfrastructureDomainService: InfrastructureDomainService;
    public columnDefs: any[] = [];
    public rowData: any[] = [];
    public BIReportName = "";

    constructor() {

    }
    ngOnInit() {
        this._DWSubQueryPMService.getByQueryId(this.DWQueryId).subscribe(myResult => {
            if (!myResult.HasError) {
                this.DWQueryData = myResult.Result;
                this.BuildColumns();
                this.BuildRows();
            }
        });
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
    LoadBIReportData() {
        if (this.DWQueryId != null) {
            this._InfrastructureDomainService.GetByBIReportId(this.EntityId).subscribe(myResult => {
                if (!myResult.HasError) {
                    var result: BIReportXMLData = myResult.Result;
                    this.EntityPM = result.BIReportPM;
                    this.BIReportName = this.EntityPM != null ? this.EntityPM.Name : "";
                    var sortsList = [];
                    var allColumnIds = [];
                    if (result.BITabularViewSettings != null && result.BITabularViewSettings.Columns != null) {
                        result.BITabularViewSettings.Columns.forEach(item => {
                            if (item != null && item.Code != null) {
                                if (item != null && item.SortDirction != null) {
                                    sortsList.push({
                                        colId: item.Code,
                                        sort: item.SortDirction,
                                        order: item.SortOrder
                                    });
                                }
                                var col = this.agGrid.columnApi.getColumn(item.Code);
                                if (col != null) {
                                    allColumnIds.push(item.Code);
                                    var colDef = col.getColDef();
                                    colDef.width = item.Width;
                                    colDef.pivotIndex = item.Index;
                                    this.IsResizing = true;
                                    this.agGrid.columnApi.moveColumn(item.Code, item.Index);
                                    this.agGrid.columnApi.setColumnWidth(item.Code, item.Width);
                                }
                            }
                        });

                        if (sortsList != null) {
                            sortsList = sortsList.sort((a, b) => { return (a.order === b.order) ? 0 : (a.order < b.order) ? -1 : 1 });
                            this.IsSorting = true;
                            this.agGrid.api.setSortModel(sortsList);
                        }
                    }
                }
            });
        }
    }
    BuildColumns() {
        this.columnDefs = [];
        var columns = this.DWQueryData.Columns;
        if (columns != null) {
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].DataTypeCode == "DateTime") {
                    this.columnDefs.push({
                        headerName: columns[i].Name,
                        field: columns[i].Name,
                        sortable: true,
                        //checkboxSelection: true,
                        width: 170,
                        resizable: true,
                        cellRenderer: this.DateCellRenderer,
                        cellClass: columns[i].DataTypeCode,
                        filter: 'agTextColumnFilter',
                        //cellStyle: {'background':'red'}
                    });
                }
                else {
                    this.columnDefs.push({
                        headerName: columns[i].Name,
                        field: columns[i].Name,
                        sortable: true,
                        filter: true,
                        //checkboxSelection: true,
                        width: 170,
                        resizable: true,
                        cellClass: columns[i].DataTypeCode
                    });
                }
            }
        }
    }
    BuildRows() {
        this.rowData = [];
        this.StartBusyIndicator("Loading ..");
        if (this.DWQueryData != null && this.DWQueryData.SubQueryData != null && this.DWQueryData.SubQueryData.SQLString != null) {
            this._DWQueryBuilderService.GetDWQueryData(this.DWQueryData.SubQueryData.SQLString, "Fact_Shipments").subscribe(myResult => {
                if (!myResult.HasError) {
                    this.rowData = myResult.Result;
                    this.StopBusyIndicator();
                }
                else {
                    this.StopBusyIndicator();
                }

                this.LoadBIReportData();
            });
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
            confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
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
                this.ComponentRef.destroy();
            }
        }
    }
    SaveBIReport() {
        if (this.EntityId == null) {
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
                });
            });

        }
        else {
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
            //if (sorting != null) {
            //    sorting.forEach(item => {
            //        newColumn.SortDirction = item["sort"];
            //    });
            //}
            if (coulmns != null) {
                coulmns.forEach(item => {
                    newColumn = new Column();
                    if (sorting != null) {
                        var sortItem = sorting.filter(x => x.colId === item["colId"])[0];
                        if (sortItem != null) {
                            newColumn.SortDirction = sortItem["sort"];
                            newColumn.SortOrder = sorting.indexOf(sortItem);
                        }
                    }
                    newColumn.Index = coulmns.indexOf(item);
                    newColumn.Width = item["width"];
                    newColumn.Code = item["colId"];
                    result.BITabularViewSettings.Columns.push(newColumn);
                });
            }

            this._InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(myResult => {
                this.hasChanged = false;
                this.StopBusyIndicator();
            });
        }
    }
    OnNewBIReportWindowClosed(arg: string) {
        if (arg != 'cancel') {
            this.EntityId = arg;
        }
    }
    ShowQueryBuilderClicked() {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DWQueryId = this.DWQueryId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null) {
                    this.DWQueryId = s.ID;

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
                    });
                });
        }
    }
    //#endregion

}
