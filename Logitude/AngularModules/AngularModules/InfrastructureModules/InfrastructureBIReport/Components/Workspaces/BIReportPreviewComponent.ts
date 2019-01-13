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
import { LicenseManager } from "ag-grid-enterprise";
import { InfrastructureDomainService, BIReportXMLData, BIReportColumnData } from '../../../../Infrastructure/Services/InfrastructureDomainService';

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
    public excelStyles;

    constructor() {
        LicenseManager.setLicenseKey("your license key");
        this.excelStyles = [
            {
                id: "Boolean",
                dataType: "boolean"
            },
            {
                id: "Text",
                dataType: "string"
            },
            {
                id: "DateTime",
                dataType: "dateTime"
            }
        ];
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
    InitializeServices() {
        this._InfrastructureDomainService = new InfrastructureDomainService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        this._BIReportPMService = new BIReportPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService();
    }
    public Run(args: any) {
        this.InitializeServices();
        this.DWQueryId = args['DWQueryId'];
        this.EntityId = args['EntityId'];
        if (this.DWQueryId != null) {
            this._InfrastructureDomainService.GetByBIReportId(this.EntityId).subscribe(myResult => {
                if (!myResult.HasError) {
                    var result: BIReportXMLData = myResult.Result; 
                    this.EntityPM = result.BIReportPM;

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
                        filter: true,
                        checkboxSelection: true,
                        width: 170,
                        resizable: true,
                        cellRenderer: this.DateCellRenderer,
                        cellClass: columns[i].DataTypeCode,
                        //cellStyle: {'background':'red'}
                    });
                }
                else {
                    this.columnDefs.push({
                        headerName: columns[i].Name,
                        field: columns[i].Name,
                        sortable: true,
                        filter: true,
                        checkboxSelection: true,
                        width: 170,
                        resizable: true,
                        cellClass: columns[i].DataTypeCode
                    });
                }
            }
        }
    }
    private DateCellRenderer(params: any) {
        var DatePipe = new DateTimePipe();
        return DatePipe.transform(params.value, "SD");
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
            });
        }
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
    private gridApi;
    private gridColumnApi;
    onGridReady(params) {
        this.gridApi = params.api;
        this.gridApi.hideOverlay()
        this.gridColumnApi = params.columnApi;
    }
    onSortChanged(params) {
        this.hasChanged = true;
    }
    oncolumnResized(params) {
        this.hasChanged = true;
    }

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
    BackButtonClicked() {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }
    }

    private hasChanged = false;
    get HasChanged() {
        if (this.EntityId == null || (this.EntityPM != null && this.EntityPM.IsDirty) || this.hasChanged) {
            this.hasChanged = false;
            return true;
        }
        return false;
    }

    SaveBIReport() {
        if (this.EntityPM == null) {
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
        } else {
            // call method in server side to build xml 
            var sorting = this.agGrid.api.getSortModel();
            var coulmns = this.agGrid.columnApi.getColumnState();

            this.StartBusyIndicator("Saving ...");
            var result = new BIReportXMLData();
            result.BIReportId = this.EntityId;
            result.BIReportPM = this.EntityPM;
            result.Columns = [];
            this._InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(myResult => {
                this.StopBusyIndicator();
            });
        }
    }
    OnNewBIReportWindowClosed(arg: string) {
        if (arg != 'cancel') {
            this.EntityId = arg;
        }
    }
}
