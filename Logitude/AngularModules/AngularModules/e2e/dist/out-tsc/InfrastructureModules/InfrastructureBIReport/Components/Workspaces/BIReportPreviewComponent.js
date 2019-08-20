"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BIReportPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/BIReportPMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DWSubQueryPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService");
var DWQueryBuilderService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService");
var DateTimePipe_1 = require("../../../../Controls/Pipes/DateTimePipe");
var NumbersPipe_1 = require("../../../../Infrastructure/Pipes/NumbersPipe");
var main_1 = require("ag-grid-angular/main");
var InfrastructureDomainService_1 = require("../../../../Infrastructure/Services/InfrastructureDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var DWQueryBuilderHelper_1 = require("../../../../Infrastructure/Helpers/DWQueryBuilderHelper");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EditShipmentLinkRendererComponent_1 = require("../TemplateRenderer/EditShipmentLinkRendererComponent");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var BIReportPreviewComponent = /** @class */ (function () {
    function BIReportPreviewComponent() {
        var _this = this;
        this.EntityPM = null;
        this.SelectedFiltersDataSource = [];
        this.columnDefs = [];
        this.rowData = [];
        this.BIReportName = "";
        this.RunReportCommand = new core_1.EventEmitter();
        this.BackCompleted = new core_1.EventEmitter();
        this._EntityPMService = new EntityPMService_1.EntityPMService();
        this.BIReportXMLData = null;
        this.HasDeletionFeature = false;
        this.IsFilterValueChanged = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Build ag-grid Data
        this.IsSorting = false;
        this.IsResizing = false;
        //#endregion
        this.hasChanged = false;
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
        this.HasValidationError = false;
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper_1.DWQueryBuilderHelper();
        this._DWQueryBuilderHelper.FilterValueChanged.subscribe(function (QueryId) {
            _this.IsFilterValueChanged = true;
            //this.timerToken = setTimeout(() => this.IsFilterValueChanged = false, 500);
        });
    }
    BIReportPreviewComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.HasDeletionFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("BIReport", "BIReportDelete");
        this._DWSubQueryPMService.getByQueryId(this.DWQueryId).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.DWQueryData = myResult.Result;
                if (_this.DWQueryData.Filters) {
                    var MyFilter = _this._DWQueryBuilderHelper.RestoreFilters(_this.DWQueryData.Filters);
                    var temp = [];
                    temp.push(MyFilter);
                    _this.SelectedFiltersDataSource = temp;
                }
            }
        });
        this.LoadBIReportData();
    };
    BIReportPreviewComponent.prototype.Run = function (args) {
        this.InitializeServices();
        this.DWQueryId = args['DWQueryId'];
        this.EntityId = args['EntityId'];
        this.FolderId = args['FolderId'];
    };
    BIReportPreviewComponent.prototype.InitializeServices = function () {
        this._InfrastructureDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        this._DWSubQueryPMService = new DWSubQueryPMService_1.DWSubQueryPMService();
        this._BIReportPMService = new BIReportPMService_1.BIReportPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService_1.DWQueryBuilderService();
        this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
    };
    BIReportPreviewComponent.prototype.LoadBIReportData = function () {
        var _this = this;
        if (this.DWQueryId != null) {
            this._InfrastructureDomainService.GetByBIReportId(this.EntityId, this.DWQueryId).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    var result = myResult.Result;
                    _this.ReportXML = result;
                    _this.BIReportXMLData = result;
                    _this.EntityPM = result.BIReportPM;
                    _this.BIReportName = _this.EntityPM != null ? _this.EntityPM.Name : "";
                    // this.BuildColumns(result);
                    _this.BuildRows(result);
                }
            });
        }
    };
    BIReportPreviewComponent.prototype.UpdateAGGrid = function (arg, msg, count) {
        if (msg === void 0) { msg = null; }
        if (count === void 0) { count = 0; }
        var sortsList = [];
        if (arg.BITabularViewSettings != null && arg.BITabularViewSettings.Columns != null) {
            arg.BITabularViewSettings.Columns.forEach(function (item) {
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
                sortsList = sortsList.sort(function (a, b) { return (a.order === b.order) ? 0 : (a.order < b.order) ? -1 : 1; });
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
    };
    BIReportPreviewComponent.prototype.BuildColumns = function (arg) {
        this.columnDefs = [];
        this.agGrid.api.setColumnDefs(this.columnDefs);
        var columns = arg.BITabularViewSettings.Columns.sort(function (a, b) { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1; });
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
                    else if (type == "integerColumn") {
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
                                var pipe = new NumbersPipe_1.NumbersPipe();
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
                            cellRenderer: function (params) {
                                if (params && params.value == "Yes") {
                                    return "<img src=\"./Images/CheckBoxIcon.png\" class=\"CenterCenter\" />";
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
                                cellRendererFramework: EditShipmentLinkRendererComponent_1.EditShipmentLinkRendererComponent,
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
    };
    BIReportPreviewComponent.prototype.GetColumnDataType = function (type) {
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
    };
    BIReportPreviewComponent.prototype.BuildRows = function (arg) {
        this.rowData = [];
        this.CountText = "";
        this.StartBusyIndicator();
        this.RunReportCommand.emit({ MyData: arg.DWQueryData, FirstTime: true });
    };
    BIReportPreviewComponent.prototype.DateCellRenderer = function (params) {
        var datepipe = new DateTimePipe_1.DateTimePipe();
        return datepipe.transform(params.value, "SD");
    };
    BIReportPreviewComponent.prototype.DateTimeCellRenderer = function (params) {
        var datepipe = new DateTimePipe_1.DateTimePipe();
        return datepipe.transform(params.value, "DT");
    };
    BIReportPreviewComponent.prototype.methodFromParent = function (cell) {
        var _this = this;
        this.StartBusyIndicator("Loading ...");
        this._ShipmentPMService.getSingleByShipmentNumber(cell).subscribe(function (myResult) {
            if (!myResult.HasError) {
                var Id = myResult.Result;
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: Id, ObjectTableName: 'Shipment', BackButtonLabel: "BI Report" });
                });
            }
            _this.StopBusyIndicator();
        });
    };
    Object.defineProperty(BIReportPreviewComponent.prototype, "HasChanges", {
        get: function () {
            if (this.EntityId == null || (this.EntityPM != null && this.EntityPM.IsDirty) || this.hasChanged) {
                return true;
            }
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BIReportPreviewComponent.prototype, "IsNewEntity", {
        get: function () {
            var isNew = this.EntityPM != null ? false : true;
            return isNew;
        },
        enumerable: true,
        configurable: true
    });
    BIReportPreviewComponent.prototype.StartBusyIndicator = function (message, width) {
        if (message === void 0) { message = "Generating..."; }
        if (width === void 0) { width = 200; }
        this.BusyIndicatorText = message;
        this.ShowBusyIndicator = true;
        this.WidthBusyIndicator = width;
    };
    BIReportPreviewComponent.prototype.StopBusyIndicator = function () {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    };
    BIReportPreviewComponent.prototype.onGridReady = function (params) {
        this.gridApi = params.api;
        this.gridApi.hideOverlay();
        this.gridColumnApi = params.columnApi;
    };
    BIReportPreviewComponent.prototype.onSortChanged = function (params) {
        if (!this.IsSorting)
            this.hasChanged = true;
        else {
            this.hasChanged = false;
            this.IsSorting = false;
        }
    };
    BIReportPreviewComponent.prototype.onColumnResized = function (params) {
        if (params != null && params.finished === true) {
            if (!this.IsResizing)
                this.hasChanged = true;
            else {
                this.hasChanged = false;
                this.IsResizing = false;
            }
        }
    };
    BIReportPreviewComponent.prototype.onColumnMoved = function (params) {
        if (!this.IsResizing)
            this.hasChanged = true;
        else {
            this.hasChanged = false;
            this.IsResizing = false;
        }
    };
    //#endregion
    //#region Export To Excel
    BIReportPreviewComponent.prototype.ExportToExcelAGGridClicked = function () {
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
    };
    BIReportPreviewComponent.prototype.ExportToExcelClicked = function () {
        var windowArgs = {};
        windowArgs.queryId = this.DWQueryId;
        windowArgs.reportId = this.EntityPM.Id;
        windowArgs.reportName = this.EntityPM.Name;
        windowArgs.BIReportXMLData = this.BIReportXMLData;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/ExportBI2ExcelControl/ExportBI2ExcelControl');
    };
    //#endregion
    //#region Buttons Commands
    BIReportPreviewComponent.prototype.BackButtonClicked = function () {
        var _this = this;
        if (this.HasChanges) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = "Don't Save";
            confirmWindow.YesButtonText = "Save ";
            confirmWindow.CancelButtonText = "Cancel";
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show("This Report has unsaved changes. Do you want to save it?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    // save
                    _this.SaveBIReport(true);
                }
                else if (confirmWindow.No) {
                    if (_this.ComponentRef) {
                        _this.BackCompleted.emit(false);
                        _this.ComponentRef.destroy();
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
    };
    BIReportPreviewComponent.prototype.SaveBIReport = function (isBackBtn) {
        if (isBackBtn === void 0) { isBackBtn = false; }
        if (this.EntityId == null) {
            this.NewBIReport();
        }
        else {
            this.UpdateBIReport(false, isBackBtn);
        }
    };
    BIReportPreviewComponent.prototype.NewBIReport = function () {
        var _this = this;
        var windowTitle = "New BI Report";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        var windowArgs = {};
        windowArgs.DWQueryId = this.DWQueryId;
        windowArgs.FolderId = this.FolderId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (d != "cancel") {
                    _this.EntityPM = s.EntityPM;
                    _this.EntityId = s.EntityPM.Id;
                    _this.BIReportName = _this.EntityPM != null ? _this.EntityPM.Name : "";
                    _this.UpdateBIReport(false);
                }
            });
        });
    };
    BIReportPreviewComponent.prototype.UpdateBIReport = function (arg, isBackBtn) {
        var _this = this;
        if (isBackBtn === void 0) { isBackBtn = false; }
        // call method in server side to build xml 
        var sorting = this.agGrid.api.getSortModel();
        var coulmns = this.agGrid.columnApi.getColumnState();
        this.StartBusyIndicator("Saving ...");
        var result = new InfrastructureDomainService_1.BIReportXMLData();
        result.BITabularViewSettings = new InfrastructureDomainService_1.BITabularViewSettings();
        result.BIReportId = this.EntityId;
        result.BIReportPM = this.EntityPM;
        result.BITabularViewSettings.Columns = [];
        result.DWQueryData = this.BIReportXMLData.DWQueryData;
        var newColumn = new InfrastructureDomainService_1.Column();
        var sortsList = [];
        this.BIReportXMLData.DWQueryData.Columns.forEach(function (item) {
            newColumn = new InfrastructureDomainService_1.Column();
            var agCol = coulmns != null ? coulmns.filter(function (x) { return x.colId === item.DisplayName.replace('[', '').replace(']', ''); })[0] : null;
            newColumn.Index = _this.BIReportXMLData.DWQueryData.Columns.indexOf(item);
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
                var sortItem = sorting.filter(function (x) { return x.colId === item.DisplayName.replace('[', '').replace(']', ''); })[0];
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
            sortsList.sort(function (a, b) { return (a.order === b.order) ? 0 : (a.order < b.order) ? -1 : 1; }).forEach(function (item) {
                result.DWQueryData.ColumnsSort += item.colId + " " + item.sort + ",";
            });
            result.DWQueryData.ColumnsSort = result.DWQueryData.ColumnsSort.replace(/,\s*$/, "");
        }
        this._InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.BIReportXMLData = myResult.Result;
                _this.EntityPM = _this.BIReportXMLData.BIReportPM;
                _this.EntityId = _this.BIReportXMLData.BIReportId;
                _this.hasChanged = false;
                _this.StopBusyIndicator();
                if (arg) {
                    _this.ShowQueryBuilder();
                }
                if (_this.ComponentRef && isBackBtn) {
                    _this.BackCompleted.emit(true);
                    _this.ComponentRef.destroy();
                }
            }
        });
    };
    BIReportPreviewComponent.prototype.ShowQueryBuilderClicked = function () {
        if (this.HasChanges) {
            this.UpdateBIReport(true);
        }
        else {
            this.ShowQueryBuilder();
        }
    };
    BIReportPreviewComponent.prototype.ShowQueryBuilder = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.DWQueryId = this.DWQueryId;
        windowArgs.IsBIReportEditScreen = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 780;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if ((d != null && d != "cancel")) {
                    _this.DWQueryData = s.QueryData;
                    if (_this.DWQueryData.Filters) {
                        var MyFilter = _this._DWQueryBuilderHelper.RestoreFilters(_this.DWQueryData.Filters);
                        var temp = [];
                        temp.push(MyFilter);
                        _this.SelectedFiltersDataSource = temp;
                    }
                    _this.LoadBIReportData();
                }
            });
        });
    };
    BIReportPreviewComponent.prototype.EditBIReportClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.EntityPM.Id, ObjectTableName: 'BIReport' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this._EntityPMService.getSingle("BIReport", _this.EntityPM.Id).then(function (res) {
                        res.subscribe(function (aa) {
                            _this.BIReportName = aa.Result != null ? aa.Result.Name : "";
                        });
                    });
                });
            });
        }
    };
    BIReportPreviewComponent.prototype.RunReportButtonClicked = function () {
        this.IsFilterValueChanged = false;
        this.RunReportCommand.emit(this.BIReportXMLData.DWQueryData); //this.DWQueryData);
    };
    BIReportPreviewComponent.prototype.OnRunReportComplete = function (MyData) {
        var _this = this;
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
            this.timerToken = setTimeout(function () { return _this.UpdateAGGrid(_this.ReportXML, MyData.Msg, MyData.Count); }, 500);
            this.StopBusyIndicator();
        }
        this.StopBusyIndicator();
    };
    BIReportPreviewComponent.prototype.OnComputeFiltersComplete = function (MyData) {
        this.BIReportXMLData.DWQueryData = MyData;
    };
    BIReportPreviewComponent.prototype.CountClicked = function () {
        alert("Count : " + this.agGrid.api.getDisplayedRowCount());
    };
    BIReportPreviewComponent.prototype.onBIReportColumnsClick = function () {
        var _this = this;
        if (!this.IsNewEntity) {
            var windowTitle = "Show/Hide/Reorder Column(s)";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            var windowArgs = {};
            windowArgs.father = this;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/AgGridColumnsOperations');
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    if (s != null && d != "cancel") {
                        _this.BuildColumns(_this.BIReportXMLData);
                        _this.BuildRows(_this.BIReportXMLData);
                    }
                });
            });
        }
    };
    BIReportPreviewComponent.prototype.onDeleteBIReportClick = function () {
        var _this = this;
        if (!this.IsNewEntity) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.YesButtonText = "Delete";
            confirmWindow.Title = "Confirm Deletion";
            confirmWindow.Show("Deleting this report will remove it from the BI reports list Once deleted it can't be restored");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    // save
                    _this._InfrastructureDomainService.DeleteBIReport(_this.EntityPM.Id).subscribe(function (myResult) {
                        if (!myResult.HasError) {
                            if (_this.ComponentRef) {
                                _this.CurrentSession.FireEvent("BIRefresh");
                                _this.ComponentRef.destroy();
                            }
                        }
                    });
                }
                else if (confirmWindow.No) {
                    //nth
                }
            });
        }
    };
    __decorate([
        core_1.ViewChild('agGrid'),
        __metadata("design:type", main_1.AgGridNg2)
    ], BIReportPreviewComponent.prototype, "agGrid", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], BIReportPreviewComponent.prototype, "RunReportCommand", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BIReportPreviewComponent.prototype, "BackCompleted", void 0);
    BIReportPreviewComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: 'BIReportPreviewComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BIReportPreviewComponent);
    return BIReportPreviewComponent;
}());
exports.BIReportPreviewComponent = BIReportPreviewComponent;
//# sourceMappingURL=BIReportPreviewComponent.js.map