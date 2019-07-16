import { Component, ComponentRef, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
//import { AgGridModule } from "ag-grid-angular/main";
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { DateTimePipe } from '../../../../Controls/Pipes/DateTimePipe';
import { NumbersPipe } from '../../../../Infrastructure/Pipes/NumbersPipe';
import { AgGridNg2 } from 'ag-grid-angular/main';
import { InfrastructureDomainService, BIReportXMLData, BITabularViewSettings, Column } from '../../../../Infrastructure/Services/InfrastructureDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { DWObjectFieldsDetails } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { EditShipmentLinkRendererComponent } from "../TemplateRenderer/EditShipmentLinkRendererComponent";
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';

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
    public FolderId: string;
    public DWQueryData: DWQueryData;
    SelectedFiltersDataSource: any[] = [];
    public _DWSubQueryPMService: DWSubQueryPMService;
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper
    public _BIReportPMService: BIReportPMService;
    public _InfrastructureDomainService: InfrastructureDomainService;
    public _ShipmentPMService: ShipmentPMService;
    public columnDefs: any[] = [];
    public rowData: any[] = [];
    public BIReportName = "";
    @Output() RunReportCommand = new EventEmitter();
    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    private _EntityPMService: EntityPMService = new EntityPMService();
    private timerToken: any;
    public BIReportXMLData: BIReportXMLData = null;
    public HasDeletionFeature = false;
    public columnTypes;
    public context;
    public CountText: string;
    public IsFilterValueChanged: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
        this._DWQueryBuilderHelper.FilterValueChanged.subscribe((QueryId) => {
            this.IsFilterValueChanged = true;
            //this.timerToken = setTimeout(() => this.IsFilterValueChanged = false, 500);
        });
    }
    ngOnInit() {
        this.HasDeletionFeature = FeatureLocator.HasFeaturePermession("BIReport", "BIReportDelete");
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
        this.FolderId = args['FolderId'];
    }
    InitializeServices() {
        this._InfrastructureDomainService = new InfrastructureDomainService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        this._BIReportPMService = new BIReportPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._ShipmentPMService = new ShipmentPMService();
    }

    //#region Build ag-grid Data
    private IsSorting = false;
    private IsResizing = false;
    private ReportXML: any;
    private isParentTenant: false;
    public LoadBIReportData() {
        if (this.DWQueryId != null) {
            this._InfrastructureDomainService.GetByBIReportId(this.EntityId, this.DWQueryId).subscribe(myResult => {
                if (!myResult.HasError) {
                    var result: BIReportXMLData = myResult.Result;
                    this.ReportXML = result;
                    this.BIReportXMLData = result;
                    this.EntityPM = result.BIReportPM;
                    this.BIReportName = this.EntityPM != null ? this.EntityPM.Name : "";
                   // this.BuildColumns(result);
                    this.BuildRows(result);
                }
            });
        }
    }

    public UpdateAGGrid(arg: BIReportXMLData, msg = null, count = 0) {
      
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
                //this.agGrid.api.setSortModel(sortsList);
            }
            this.agGrid.api.refreshCells();

            if (count > 50000 || msg == "MT5000") {
                this.CountText = "Showing the first 10,000 rows, download the excel to view all.";
            }
            else {
                this.CountText = count + " rows";
            }

        }
    }
    
    public BuildColumns(arg: BIReportXMLData) {
        this.columnDefs = [];

        this.agGrid.api.setColumnDefs(this.columnDefs);
        var columns = arg.BITabularViewSettings.Columns.sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 });
        if (columns != null) {
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].IsChecked) {
                    var type = this.GetColumnDataType(columns[i].DataTypeCode);
                 
                    if (type == "dateColumn") {
                        var dataTypeCode = columns[i].DataTypeCode;
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Code,
                            field: columns[i].Code,
                            sortable: true,
                            width: columns[i].Width,
                            resizable: true,
                            //cellClass: columns[i].DataTypeCode,
                            Index: columns[i].Index,
                            type: type,
                            cellRenderer: dataTypeCode == "Date" ? this.DateCellRenderer : this.DateTimeCellRenderer,
                            filter: 'agDateColumnFilter'
                            //sort: sortingDirction,
                        });
                    }
                    else if (type == "integerColumn"){
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Code,
                            field: columns[i].Code,
                            sortable: true,
                            filter: true,
                            width: columns[i].Width,
                            resizable: true,
                            Index: columns[i].Index,
                            type: type,
                        });
                    }
                    else if (type == "numericColumn") {
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Code,
                            field: columns[i].Code,
                            sortable: true,
                            filter: true,
                            width: columns[i].Width,
                            resizable: true,
                            //cellClass: columns[i].DataTypeCode,
                            Index: columns[i].Index,
                            type: type,
                            valueFormatter: function (params) {
                                var pipe = new NumbersPipe();
                                return pipe.transform(params.value, "N2");
                            },
                           
                        });
                    }
                    else if (type == "booleanColumn") {
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Code,
                            field: columns[i].Code,
                            sortable: true,
                            filter: true,
                            width: columns[i].Width,
                            resizable: true,
                            // cellClass: columns[i].DataTypeCode,
                            Index: columns[i].Index,
                            type: type,
                            cellRenderer: params => {
                                if (params && params.value == "Yes") {
                                    return `<img src="./Images/CheckBoxIcon.png" class="CenterCenter" />`;
                                }
                            },
                           
                        });
                    }
                    else if (columns[i].Code == "Shipment Number") {
                        if (this.isParentTenant) {
                            this.columnDefs.push({
                                colId: columns[i].Code,
                                headerName: columns[i].Code,
                                field: columns[i].Code,
                                sortable: true,
                                filter: true,
                                width: columns[i].Width,
                                resizable: true,
                                Index: columns[i].Index,
                            });
                        }
                        else {
                            this.columnDefs.push({
                                colId: columns[i].Code,
                                headerName: columns[i].Code,
                                field: columns[i].Code,
                                sortable: true,
                                filter: true,
                                width: columns[i].Width,
                                resizable: true,
                                Index: columns[i].Index,
                                cellRendererFramework: EditShipmentLinkRendererComponent,
                            });
                        }
                    }
                    else {
                        this.columnDefs.push({
                            colId: columns[i].Code,
                            headerName: columns[i].Code,
                            field: columns[i].Code,
                            sortable: true,
                            filter: true,
                            width: columns[i].Width,
                            resizable: true,
                            //cellClass: columns[i].DataTypeCode,
                            Index: columns[i].Index,
                            type: type,
                           
                        });
                    }
                }
            }

            this.agGrid.api.setColumnDefs(this.columnDefs);
            this.agGrid.api.refreshHeader();
            this.context = { componentParent: this };
        }
    }

    private GetColumnDataType(type: string) {
        var datatype = "";
        switch (type) {
            case "Date":
            case "DateTime":
                {
                    datatype = "dateColumn";
                    break;
                }
            case "Integer":
                {
                    datatype = "integerColumn";
                    break;
                }
            case "UnsInteger":
            case "Double":
            case "Decimal":
            case "SigDouble":
            case "UnsDecimal":
                {
                    datatype = "numericColumn";
                    break;
                }
            case "nText":
            case "Text":
                {
                    datatype = "stringColumn";
                    break;
                }
            case "Boolean":
                {
                    datatype = "booleanColumn";
                    break;
                }
            default:
                {
                    break;
                }
        }

        return datatype;
    }

    public BuildRows(arg: BIReportXMLData) {
        this.rowData = [];
        this.CountText = "";
        this.StartBusyIndicator();
        this.RunReportCommand.emit({ MyData: arg.DWQueryData, FirstTime: true });
    }

    private DateCellRenderer(params: any) {
        var datepipe = new DateTimePipe();
        return datepipe.transform(params.value, "SD");
    }

    private DateTimeCellRenderer(params: any) {
        var datepipe = new DateTimePipe();
        return datepipe.transform(params.value, "DT");
    }

    


    public methodFromParent(cell) {
        this.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.getSingleByShipmentNumber(cell).subscribe(myResult => {
            if (!myResult.HasError) {
                var Id = myResult.Result;
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: Id, ObjectTableName: 'Shipment', BackButtonLabel: "BI Report" });
                    });
            }

            this.StopBusyIndicator();
        });
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
        windowArgs.BIReportXMLData = this.BIReportXMLData;
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
                    this.SaveBIReport(true);
                }
                else if (confirmWindow.No) {
                    if (this.ComponentRef) {
                        this.BackCompleted.emit(false);
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
                this.BackCompleted.emit(false);
                this.CurrentSession.FireEvent("BIRefresh");
                this.ComponentRef.destroy();
            }
        }
    }
    SaveBIReport(isBackBtn = false) {
        if (this.EntityId == null) {
            this.NewBIReport();
        }
        else {
            this.UpdateBIReport(false, isBackBtn);
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
        windowArgs.FolderId = this.FolderId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d != "cancel") {
                    this.EntityPM = s.EntityPM;
                    this.EntityId = s.EntityPM.Id;
                    this.BIReportName = this.EntityPM != null ? this.EntityPM.Name : "";
                    this.UpdateBIReport(false);
                }
            });
        });
    }
    UpdateBIReport(arg: boolean, isBackBtn = false) {
        // call method in server side to build xml 
        var sorting = this.agGrid.api.getSortModel();
        var coulmns = this.agGrid.columnApi.getColumnState();
        this.StartBusyIndicator("Saving ...");
        var result = new BIReportXMLData();
        result.BITabularViewSettings = new BITabularViewSettings();
        result.BIReportId = this.EntityId;
        result.BIReportPM = this.EntityPM;
        result.BITabularViewSettings.Columns = [];
        result.DWQueryData = this.BIReportXMLData.DWQueryData;

        var newColumn = new Column();
        var sortsList = [];

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

                    sortsList.push({
                        colId: item.DisplayName,
                        sort: newColumn.SortDirction,
                        order: newColumn.SortOrder
                    });
                }
            }
            result.BITabularViewSettings.Columns.push(newColumn);
        });

        if (sortsList != null && sortsList.length > 0) {
            this.BIReportXMLData.DWQueryData.ColumnsSort = "";
            sortsList.sort((a, b) => { return (a.order === b.order) ? 0 : (a.order < b.order) ? -1 : 1 }).forEach(item => {
                result.DWQueryData.ColumnsSort += item.colId + " " + item.sort + ",";
            });
            result.DWQueryData.ColumnsSort = result.DWQueryData.ColumnsSort.replace(/,\s*$/, "");
        }

        this._InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(myResult => {
            if (!myResult.HasError) {
                this.BIReportXMLData = myResult.Result;
                this.EntityPM = this.BIReportXMLData.BIReportPM;
                this.EntityId = this.BIReportXMLData.BIReportId;
                this.hasChanged = false;
                this.StopBusyIndicator();

                if (arg) {
                    this.ShowQueryBuilder();
                }

                if (this.ComponentRef && isBackBtn) {
                    this.BackCompleted.emit(true);
                    this.ComponentRef.destroy();
                }

            }
        });
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
        logWindow.Height = 780;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if ((d != null && d != "cancel")) {
                    this.DWQueryData = s.QueryData;
                    if (this.DWQueryData.Filters) {
                        var MyFilter = this._DWQueryBuilderHelper.RestoreFilters(this.DWQueryData.Filters);
                        var temp = [];
                        temp.push(MyFilter);
                        this.SelectedFiltersDataSource = temp;
                    }
                    this.LoadBIReportData();
                }
            });
        });
    }
    EditBIReportClicked() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityPM.Id, ObjectTableName: 'BIReport' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this._EntityPMService.getSingle("BIReport", this.EntityPM.Id).then((res: any) => {
                            res.subscribe((aa: any) => {
                                this.BIReportName = aa.Result != null ? aa.Result.Name : "";
                            })
                        });
                    });
                });
        }
    }
    RunReportButtonClicked() {
        this.IsFilterValueChanged = false;
        this.RunReportCommand.emit(this.BIReportXMLData.DWQueryData);//this.DWQueryData);
    }
    public HasValidationError = false;
    OnRunReportComplete(MyData) {
        if (MyData.Msg == "ValidationError") {
            this.HasValidationError = true;
            this.rowData = [];
            this.StopBusyIndicator();
        }
        else {
            this.HasValidationError = false;
            this.rowData = MyData.rowData;
            this.isParentTenant = MyData.IsParentTenant;
            this.BuildColumns(this.BIReportXMLData);
            this.timerToken = setTimeout(() => this.UpdateAGGrid(this.ReportXML, MyData.Msg, MyData.Count), 500);
            this.StopBusyIndicator();
        }

        this.StopBusyIndicator();
    }
    OnComputeFiltersComplete(MyData) {
        this.BIReportXMLData.DWQueryData = MyData;
    }
    CountClicked() {
        alert("Count : " + this.agGrid.api.getDisplayedRowCount());
    }
    onBIReportColumnsClick() {
        if (!this.IsNewEntity) {
            var windowTitle = "Show/Hide/Reorder Column(s)";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            var windowArgs: any = {};
            windowArgs.father = this;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/AgGridColumnsOperations');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    if (s != null && d != "cancel") {
                        this.BuildColumns(this.BIReportXMLData);
                        this.BuildRows(this.BIReportXMLData);
                    }
                });
            });
        }
    }
    onDeleteBIReportClick() {
        if (!this.IsNewEntity) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.YesButtonText = "Delete";
            confirmWindow.Title = "Confirm Deletion";
            confirmWindow.Show("Deleting this report will remove it from the BI reports list Once deleted it can't be restored");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    // save
                    this._InfrastructureDomainService.DeleteBIReport(this.EntityPM.Id).subscribe(myResult => {
                        if (!myResult.HasError) {
                            if (this.ComponentRef) {
                                this.CurrentSession.FireEvent("BIRefresh");
                                this.ComponentRef.destroy();
                            }
                        }
                    });
                }
                else if (confirmWindow.No) {
                    //nth
                }
            });
        }
    }
    //#endregion
}
