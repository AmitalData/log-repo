declare var System: any;
declare var window: any;
import {Component, OnDestroy, ElementRef, Renderer, OnInit, AfterViewInit, AfterContentInit, OnChanges, Output, SimpleChange, EventEmitter, RenderComponentType, ChangeDetectionStrategy, Input, ChangeDetectorRef } from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
//import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {VirtualRowController} from './VirtualRowController';
//import {Observable} from 'rxjs/Observable';
import {LogEvents} from '../../../../Infrastructure/Utilities/LogEvents';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {PubSubService1} from '../../../../Infrastructure/Utilities/events/ApiFiltersEvent1';
//import {ListHeaderTemplateComponent} from './ListHeaderTemplateComponent';
//import {ObjectFieldTemplate} from '../../Templates/ObjectFieldTemplate';
import {ObjectTablePM} from '../../../EntityPMs/ObjectTablePM';
import {ListTemplateComponent} from './ListTemplateComponent';
declare var styleDisplay, itemStyling, itemWidth: any;
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../Locators/ObjectsLocator';
import {ServiceLocator} from '../../../Locators/ServiceLocator';
import { filter } from 'rxjs/operators';

@Component({
    moduleId: module.id,

    selector: 'logitude-grid',
    templateUrl: './LogGridComponent.html',
    //directives: [CORE_DIRECTIVES, ObjectFieldTemplate, ListHeaderTemplateComponent, ListTemplateComponent],
    providers: [PubSubService1],
    //pipes: [TextCodeTranslationPipe], 
    inputs: ['columns', 'rowCount', 'dataSource', 'searchFields', 'queryId', 'QueryChangeEvent', 'Filterchangeevent', 'pubSubAdvanceQueryFiltersServiceRecived', 'autoLoad', 'SearchFieldchangeevent', 'MenuHeaderchangeevent', 'SelectedRow', 'ObjectTable', 'ColumnsReady', 'IsCustomTemplate', 'CustomColumnsReady', 'SelectFirstRow', 'EnableRowHoverVisibility', 'RowHoverVisibilityQueryName', 'HoverTemplateIndex', 'HasPermition', 'ShowArrow', 'IsGradiantSelectedColor', 'rowHeight', 'RowHoverColor', 'RowBackGroundColor', 'ChangeColorByPropName', 'ChangeColorByPropValue', 'IgnoreRowHoverVisibilityQueryName', 'PassAdditionalDataToTemplates', 'ShowHLineOverRow', 'EnableRowToolTip', 'ToolTipWidth', 'ToolTipHeight', 'ToolTipBinding', 'IsAllRecordsChecked', 'HighLightSelectedRow', 'SelectedRows', 'EnableMultiSelection', 'CustomBackFromEdit', 'CheckBoxFilterChanged', 'IsCheckBoxEnabled', 'FireCheckBoxChecked', 'Disabled', 'UseBusyIndecator', 'MarkIsChecked','MyScrollTop', 'MySelectedRowIndex','SortServerProp','ReloadData'],
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class LogGridComponent implements OnInit, AfterViewInit, OnChanges, OnDestroy {
    func: Function;

    public IsCheckBoxEnabled: boolean = true;
    Disabled: boolean = false;
    ReloadData: boolean = false;
    public PassAdditionalDataToTemplates: boolean = false;
    public RowBackGroundColor: string = "";
    public HighLightSelectedRow: boolean = true;
    public ChangeColorByPropName: string = "";
    public ChangeColorByPropValue: string = "";
    public RowHoverColor: string = "";
    public IgnoreRowHoverVisibilityQueryName: boolean = false;
    public EnableRowToolTip: boolean = false;
    public ToolTipWidth: number = 300;
    public ToolTipHeight: number = 120;
    public ToolTipBinding: string = "";
    //public Direction: string = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
    IsSpotLight: boolean = false;
    ShowArrow: boolean = false;
    @Input() rows: IRow[];
    public ScreenHeight: string = "";
    @Input() ReattachToDetection: boolean;
    @Output() AfterViewInitCompleted = new EventEmitter();
    public MarkIsChecked: EventEmitter<any> = new EventEmitter();
    @Output() DataLoaded = new EventEmitter();
    @Output() CountReady = new EventEmitter();
    @Output() FirstRowSelected = new EventEmitter(); 
    @Output() MenuHeaderchanged = new EventEmitter(); 
    @Output() RowOverEvent = new EventEmitter();
    @Output() RowOutEvent = new EventEmitter();
    @Output() RowUnselected = new EventEmitter();
    public BackFromEdit: EventEmitter<any> = new EventEmitter();
    public CustomBackFromEdit: EventEmitter<any> = new EventEmitter();
    public LogGridId: string = null;
    public ColumnId: string = null;
    public LogGridRowsId: string = null;
    public LogGridColumnsId: string = null;
    public ObjectTable: ObjectTablePM;
    rowHeight: number = 35;
    height: number = 800;
    columns: any[];
    scrollTop: number = 0;
    rowsPerPage: number;
    bufferFromRow: number;
    bufferNumberOfRows: number;
    pixelsPerPage: number = this.rowsPerPage * this.rowHeight;
    numberOfTotalPages: number;
    numberOfRowsToDraw: number = 0;
    canvasHeight: any;// = {};
    rowCount: number;
    public queryId: string;
    viewportSize: number;
    cachedPages: any[] = [];
    pageIndex: number = 0;
    lastIndex: number = 0;
    public dataSource: any;
    public UseBusyIndecator: boolean = false;
    private rowModel: any;
    controller: VirtualRowController;
    inProgressRows: number = 0;
    tripleViewport: number;
    @Output() rowSelectedEvent = new EventEmitter();
    @Output() CheckBoxChecked = new EventEmitter();
    public QueryChangeEvent: EventEmitter<any>;
    public FireCheckBoxChecked: EventEmitter<any>;
    public ShowHLineOverRow: EventEmitter<any>;
    public ColumnsReady: EventEmitter<any>;
    public CustomColumnsReady: EventEmitter<any>;
    public SearchFieldchangeevent: EventEmitter<any>;
    public MenuHeaderchangeevent: EventEmitter<any>;
    public Filterchangeevent: LogEvents.EventManager;
    CheckBoxFilterChanged: EventEmitter<any>;
    @Output() ColumnResisedevent = new EventEmitter();
    @Output() RowHoverevent = new EventEmitter();
    public ViewHeight: number;
    public ViewWidth: number;
    public searchFields: string;
    public ViewPortRowCount: number;
    public pubSubAdvanceQueryFiltersServiceRecived: PubSubService1;
    Filters: ApiQueryFilters;
    LogGridElement: HTMLDivElement;
    scrollPosition: number = null;
    autoLoad: boolean = false;
    IsCustomTemplate: boolean = false;
    private timerToken: any;
    ColumnsQueryId: string;
    SelectFirstRow: boolean = false;
    EnableRowHoverVisibility: boolean = false;
    RowHoverVisibilityQueryName: string;
    HoverTemplateIndex: number = 0;
    HasPermition: boolean = true;
    IsGradiantSelectedColor: boolean = false;
    MouseUpSub: any;
    EnableMultiSelection: boolean = false;
    @Output() SortInvoked = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _elementRef: ElementRef, private _renderer: Renderer, private cd: ChangeDetectorRef) {
        //setTimeout(() => this.cd.markForCheck(), 10); 
        if (this.CurrentSession == null) {
            this.LogGridId = "LogGrid_-1_-1";
            this.LogGridRowsId = "LogGridRows_-1_-1";
            this.LogGridColumnsId = "LogGridColumns_-1_-1";
            this.ColumnId = "ColumnId_-1_-1";
        }

        else {
            this.LogGridId = "LogGrid_" + this.CurrentSession.LogitudeGridHelper.GetLogGridIndexId();
            this.LogGridRowsId = "LogGridRows_" + this.CurrentSession.LogitudeGridHelper.GetLogGridRowsIndexId();
            this.LogGridColumnsId = "LogGridColumns_" + this.CurrentSession.LogitudeGridHelper.GetLogGridColumnsIndexId();
            this.ColumnId = "ColumnId_" + this.CurrentSession.LogitudeGridHelper.GetLogGridColumnsIndexId();
        }
        this.controller = new VirtualRowController();
        this.canvasHeight = { height: '800px' };
        window.onresize = this.onWindowResized.bind(this);
        this.MouseUpSub = this.CurrentSession.MouseUpEvent.subscribe((res) => {
            this.OnMyMouseUp(res);
        });
        //document.onmouseup = (e) => {
        //    if (this.isResizing == true) {
        //        this.isResizing = false;
        //        this.ColumnResisedevent.emit({ FieldName : this.columns[this.ColIndex].FieldName, Width: this.NewWidthFinal });
        //    }
        //};
        //document.onmouseup = (e) => {
        //    if (this.isResizing || this.isDraging) {
        //        if (this.isResizing) {
        //            this.TotalWidth = this.FinalWidthNew;
        //            this.headerStyle = {
        //                'width': this.TotalWidth + 'px',
        //                'min-width': (this.ViewWidth) + 'px'
        //            };
        //        } 
        //        this.isResizing = false;
        //        this.isDraging = false;
        //        var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
        //        left.classList.remove("ag-header-cell-moving");
        //        var d = document.getElementById(this.ColumnId + 'Mask');
        //        //d.style.width = "0px";
        //        d.innerText = "";
        //        styleDisplay(d); 
        //        if (this.timerToken) {
        //            clearTimeout(this.timerToken);
        //    }
        //        this.timerToken = setTimeout(() => this.FireColumnReorderComplete(), 400);

        //    }
        //};

    }
    onRowMouseOut(id, rowIndex, row) {
        //if (!AppTool.IsNullOrEmpty(this.RowHoverColor)) {
        this.RowOutEvent.emit(row.rowIndex);
        var elem = document.getElementById(this.LogGridId + 'row' + rowIndex);
        if (elem) {
            if (this.ChangeColorByPropName && this.ChangeColorByPropValue && this.RowBackGroundColor && row.rowData[this.ChangeColorByPropName] == this.ChangeColorByPropValue) {

                elem.style.background = this.RowBackGroundColor;
            }
            else {

                elem.style.background = this.SelectedRow == row.rowData ? "#DFECF7" : "transparent";
            }
        }
        //}
        if (this.EnableRowHoverVisibility && (this.RowHoverVisibilityQueryName == "My Shipments" || this.RowHoverVisibilityQueryName == "Action Required" || this.IgnoreRowHoverVisibilityQueryName == true)) {
            var template = document.getElementById(this.LogGridId + this.HoverTemplateIndex + id);
            if (template) {
                template.style.visibility = 'hidden';
                if (this.cd) {
                    this.cd.detectChanges();
                }
            }
        }
    }
    onRowMouseOver(id, rowIndex, row) {
        //this.RowHoverevent.emit(rowIndex);
        this.RowOverEvent.emit(row.rowIndex);
        var temp = "linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%)";

        var elem = document.getElementById(this.LogGridId + 'row' + rowIndex);
        if (elem) {
            if (!AppTool.IsNullOrEmpty(this.RowHoverColor)) {
                elem.style.background = this.RowHoverColor;//"#C8C8FA";
            }
            else {
                elem.style.background = temp;//"#C8C8FA";
            }

        }
        //}
        if (this.EnableRowHoverVisibility && (this.RowHoverVisibilityQueryName == "My Shipments" || this.RowHoverVisibilityQueryName == "Action Required" || (this.IgnoreRowHoverVisibilityQueryName == true && !row.rowData.ShowLineOverRow))) {
            var template = document.getElementById(this.LogGridId + this.HoverTemplateIndex + id);
            if (template) {
                template.style.visibility = 'visible';
                if (this.cd) {
                    this.cd.detectChanges();
                }
            }
        }
    }
    OnMyMouseUp(e) {
        if (this.isResizing || this.isDraging) {
            if (this.isResizing) {
                this.TotalWidth = this.FinalWidthNew;
                this.headerStyle = {
                    'width': this.TotalWidth + 43 + 'px',
                    'min-width': '100%'//(this.ViewWidth) + 'px'
                };
                this.canvasHeight = {
                    height: this.rowCount * this.rowHeight + 'px',
                    //'width': this.TotalWidth + 'px',
                    //'min-width': this.TotalWidth//(this.ViewWidth) + 'px'
                };
            }

            var AllRows = document.getElementsByClassName('Row');
            for (var i = 0; i < AllRows.length; i++) {
                (<HTMLDivElement>AllRows[i]).style.minWidth = this.TotalWidth + 22 + 'px';
            }
            this.isResizing = false;
            this.isDraging = false;
            var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
            left.classList.remove("ag-header-cell-moving");
            var d = document.getElementById(this.ColumnId + 'Mask');
            //d.style.width = "0px";
            d.innerText = "";
            styleDisplay(d);
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(() => this.FireColumnReorderComplete(), 400);

        }
    }
    FireColumnReorderComplete() {
        //this.CurrentSession.StartBusyIndicator("Saving ...");
        var ColIndexes = [];
        var ColumnsElements = document.getElementsByClassName("ag-header-cell");
        for (var i = 0; i < ColumnsElements.length; i++) {
            if (ColumnsElements[i].attributes['colid']) {
                ColIndexes.push({ FieldName: ColumnsElements[i].attributes['colid'].value, Index: +(ColumnsElements[i].id.split(',')[1]), Width: ColumnsElements[i].clientWidth });
            }
        }
        this.ColumnResisedevent.emit({ QueryId: this.ColumnsQueryId, ColIndexes: ColIndexes });
    }
    isResizing: boolean = false;
    lastDownX: number = 0;
    ColIndex: number = 0;
    OnMyMouseDown($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.LogGridColumnsId);
        var rec = grid.getBoundingClientRect();
        if (this.RTL == true) {
            this.GridLeft = rec.right;
        }
        else {
            this.GridLeft = rec.left;
        }
        //console.log("OnMyMouseDown $event.clientX : " + $event.clientX); 
        this.lastDownX = ($event.clientX - this.GridLeft);
        //var tr = $event.currentTarget.arentElement.id;
        this.ColIndex = +($event.currentTarget.parentElement.id.split(',')[1]);//+(arg.split(',')[1]);
        var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
        this.OldWidth = left.clientWidth;
    }
    isDraging: boolean = false;
    GridLeft: number = 0;
    ShadowTitle: any;
    OnDragMouseDown($event, arg) {
        if (!this.isResizing) {
            this.isDraging = true;
            var grid = document.getElementById(this.LogGridColumnsId);
            var rec = grid.getBoundingClientRect();
            if (this.RTL == true) {
                this.GridLeft = rec.right;
            }
            else {
                this.GridLeft = rec.left;
            }
            this.ColIndex = +($event.currentTarget.id.split(',')[1]);//+(arg.split(',')[1]);
            this.lastDownX = $event.clientX - this.GridLeft;
            //var d = document.getElementById(this.ColumnId + 'Mask'); 
            //d.style.display = "block";
            this.ShadowTitle = $event.currentTarget.innerHTML;
        }
    }
    public workOutDirection(event: MouseEvent): string {
        var direction: string;
        if (this.RTL) {
            if (this.lastDownX < (event.clientX - this.GridLeft)) {
                direction = "Right";
            } else if (this.lastDownX >= (event.clientX - this.GridLeft)) {
                direction = "Left";
            } else {
                direction = null;
            }
        }
        else {
            if (this.lastDownX > (event.clientX - this.GridLeft)) {
                direction = "Left";
            } else if (this.lastDownX <= (event.clientX - this.GridLeft)) {
                direction = "Right";
            } else {
                direction = null;
            }
        }
        return direction;
    }
    PreventCheckBoxSelect(event, col) {
        if (col.IsCheckBox == true) {
            event.stopPropagation();
        }
    }
    OldWidth: number = 0;
    NewWidthFinal: number = 0;
    //leftPadd : number = 0;
    FinalWidthNew: number = 0;
    OnMyMouseMove(e) {
        if (this.isResizing) {
            //console.log("this.GridLeft : " + this.GridLeft);
            //console.log("e.clientX : " + e.clientX); 
            //console.log("this.lastDownX : " + this.lastDownX); 
            var NewX = (e.clientX - this.GridLeft);
            var lastDelta = NewX - this.lastDownX;
            if (this.RTL) {
                lastDelta = lastDelta * -1;
            }
            //console.log("NewX : " + NewX);
            //console.log("lastDelta : " + lastDelta);
            var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
            var newWidth = this.OldWidth + lastDelta;
            var WidthChanged = this.OldWidth != newWidth;
            if (WidthChanged) {
                this.NewWidthFinal = newWidth;
                left.style.width = newWidth.toString() + "px";
                if (this.RTL == true) {
                    this.columns.filter(a => a.FieldName == left.attributes['colid'].value)[0].Styles = { width: left.style.width, right: left.style.right };
                }
                else {
                    this.columns.filter(a => a.FieldName == left.attributes['colid'].value)[0].Styles = { width: left.style.width, left: left.style.left };
                }

                var ColsArr = this.columns;
                //var leftPadd = 0; 
                for (var i = this.ColIndex + 1; i < ColsArr.length; i++) {
                    var leftPadd = 0;//10;
                    //for (var j = 0; j < i; j++) {
                    //    var col = document.getElementById(this.ColumnId + "resizable-column-," + j);
                    //    leftPadd += col.clientWidth;
                    //}
                    //var col = document.getElementById(this.ColumnId + "resizable-column-," + i);
                    var prevcol = document.getElementById(this.ColumnId + "resizable-column-," + (i - 1));
                    var col = document.getElementById(this.ColumnId + "resizable-column-," + i);
                    if (col.attributes['colid']) {
                        if (this.RTL == true) {
                            col.style.right = ((+prevcol.style.right.replace("px", "")) + ((+prevcol.style.width.replace("px", "")) < 25 ? 25 : (+prevcol.style.width.replace("px", "")))) + "px";////leftPadd + "px";
                            this.columns.filter(a => a.FieldName == col.attributes['colid'].value)[0].Styles = { width: col.style.width, right: col.style.right };
                        }
                        else {
                            col.style.left = ((+prevcol.style.left.replace("px", "")) + ((+prevcol.style.width.replace("px", "")) < 25 ? 25 : (+prevcol.style.width.replace("px", "")))) + "px";//leftPadd + "px";
                            this.columns.filter(a => a.FieldName == col.attributes['colid'].value)[0].Styles = { width: col.style.width, left: col.style.left };
                        }
                    }
                    //for (var j = 0; j < i; j++) {
                    //    var col = document.getElementById(this.ColumnId + "resizable-column-," + j);
                    //    leftPadd += col.clientWidth;
                    //}
                    //var col = document.getElementById(this.ColumnId + "resizable-column-," + i);
                    //if (this.RTL == true) {
                    //    col.style.right = leftPadd + "px";
                    //    this.columns.filter(a => a.FieldName == col.attributes['colid'].value)[0].Styles = { width: col.style.width, right: col.style.right };
                    //}
                    //else {
                    //    col.style.left = leftPadd + "px";
                    //    this.columns.filter(a => a.FieldName == col.attributes['colid'].value)[0].Styles = { width: col.style.width, left: col.style.left };
                    //}

                    //this.cd.detectChanges();
                }
                if (this.cd) this.cd.detectChanges();
                this.FinalWidthNew = this.TotalWidth + (newWidth - this.OldWidth);
            }
        }
        else if (this.isDraging) {
            var Direction = this.workOutDirection(e);
            if (Direction == null) {
                return;
            }
            if (Direction == "Right") {
                var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
                this.placeDiv((e.clientX - this.GridLeft), e.clientY, left.clientWidth);
                if (this.RTL == true) {
                    if ((this.ColIndex - 1) >= 0) {
                        var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex - 1));
                        var temp = 0;//(+right.style.left.replace("px", ""));
                        temp = (+right.style.right.replace("px", ""));
                        if ((this.GridLeft - e.clientX) <= (+left.style.right.replace("px", ""))) {
                            right.style.right = temp + left.clientWidth + "px";
                            this.columns.filter(a => a.FieldName == right.attributes['colid'].value)[0].Styles = { width: right.style.width, right: right.style.right };
                            left.style.right = temp + "px";
                            this.columns.filter(a => a.FieldName == left.attributes['colid'].value)[0].Styles = { width: left.style.width, right: left.style.right };
                            left.id = this.ColumnId + "resizable-column-," + (this.ColIndex - 1);
                            right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
                            this.ColIndex = this.ColIndex - 1;
                            this.lastDownX = (e.clientX - this.GridLeft);
                        }
                    }
                }
                else {
                    if ((this.ColIndex + 1) < this.columns.length) {
                        var temp = 0;//(+left.style.left.replace("px", ""));
                        var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex + 1));
                        temp = (+left.style.left.replace("px", ""));
                        if ((e.clientX - this.GridLeft) >= (+right.style.left.replace("px", ""))) {
                            left.style.left = temp + right.clientWidth + "px";
                            this.columns.filter(a => a.FieldName == left.attributes['colid'].value)[0].Styles = { width: left.style.width, left: left.style.left };
                            right.style.left = temp + "px";
                            this.columns.filter(a => a.FieldName == right.attributes['colid'].value)[0].Styles = { width: right.style.width, left: right.style.left };
                            left.id = this.ColumnId + "resizable-column-," + (this.ColIndex + 1);
                            right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
                            this.ColIndex = this.ColIndex + 1;
                            this.lastDownX = (e.clientX - this.GridLeft);
                        }
                    }
                }
            }
            else {
                var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
                this.placeDiv((e.clientX - this.GridLeft), e.clientY, left.clientWidth);
                if (this.RTL == true) {
                    if ((this.ColIndex + 1) < this.columns.length) {
                        var temp = 0;//(+left.style.left.replace("px", ""));
                        var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex + 1));
                        temp = (+left.style.right.replace("px", ""));
                        if ((this.GridLeft - e.clientX) >= (+right.style.right.replace("px", ""))) {
                            left.style.right = temp + right.clientWidth + "px";
                            this.columns.filter(a => a.FieldName == left.attributes['colid'].value)[0].Styles = { width: left.style.width, right: left.style.right };
                            right.style.right = temp + "px";
                            this.columns.filter(a => a.FieldName == right.attributes['colid'].value)[0].Styles = { width: right.style.width, right: right.style.right };
                            left.id = this.ColumnId + "resizable-column-," + (this.ColIndex + 1);
                            right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
                            this.ColIndex = this.ColIndex + 1;
                            this.lastDownX = (e.clientX - this.GridLeft);
                        }
                    }
                }
                else {
                    if ((this.ColIndex - 1) >= 0) {
                        var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex - 1));
                        var temp = 0;//(+right.style.left.replace("px", ""));
                        temp = (+right.style.left.replace("px", ""));
                        if ((e.clientX - this.GridLeft) <= (+left.style.left.replace("px", ""))) {
                            right.style.left = temp + left.clientWidth + "px";
                            this.columns.filter(a => a.FieldName == right.attributes['colid'].value)[0].Styles = { width: right.style.width, left: right.style.left };
                            left.style.left = temp + "px";
                            this.columns.filter(a => a.FieldName == left.attributes['colid'].value)[0].Styles = { width: left.style.width, left: left.style.left };
                            left.id = this.ColumnId + "resizable-column-," + (this.ColIndex - 1);
                            right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
                            this.ColIndex = this.ColIndex - 1;
                            this.lastDownX = (e.clientX - this.GridLeft);
                        }
                    }
                }
            }
        }
    }
    placeDiv(x_pos, y_pos, width) {
        if (this.RTL == true) {
            x_pos = x_pos * -1;
        }
        var d = document.getElementById(this.ColumnId + 'Mask');
        d.style.position = "absolute";
        var windowObject = d.getBoundingClientRect();
        var xPosition = (d.offsetLeft + x_pos - this.lastDownX);
        d.innerHTML = this.ShadowTitle;
        if (this.RTL == true) {
            d.style.right = (x_pos - (width / 2)) + 'px';
        }
        else {
            d.style.left = (x_pos - (width / 2)) + 'px';
        }

        d.style.width = width + 'px';
        d.style.display = "block";
    }


    SpotLightCLicked: boolean = false;
    BackFromEditSub: any;
    onRowSelected(colDef: any, colIndex: number, rowData: any, rowIndex: number) {
        if (this.Disabled) {
            return;
        }

        //if (this.MySelectedRowIndex != null) { // attempt to select the selected row when back from edit-----mohammad.
        //    if (value.RowIndex == this.MySelectedRowIndex) {
        //        this.SelectedRow = value.RowData;
        //        this.FirstRowSelected.emit({ SelectedRow: this.SelectedRow, index: value.RowIndex });
        //    }
        //}

        this.SelectedRow = rowData;
        this.MySelectedRowIndex = rowIndex;
        if (this.cd) this.cd.detectChanges();
        //this.FirstRowSelected.emit({ SelectedRow: this.SelectedRow, index: rowIndex });

        if (this.SpotLightCLicked == true) {
            this.SpotLightCLicked = false;
        }
        else {
            this.cd.detach();
            this.BackFromEdit = new EventEmitter();
            this.BackFromEditSub = this.BackFromEdit.subscribe((res) => {
                //alert("Oh Yeah !!" + res.rowIndex);

                if (this.rows.filter(a => a.rowIndex == res.rowIndex).length > 0) {
                    //this.rows.filter(a => a.rowIndex == res.rowIndex)[0].rowData = res.Data;
                    this.controller.cachedData[res.rowIndex] = res.Data;
                    this.updateDisplayList();
                }
            });
            if (this.SelectedRows && this.EnableMultiSelection == true) {
                if (this.SelectedRows.Collection.filter(a => a.rowIndex == rowIndex).length > 0) {
                    this.SelectedRows.Collection = this.SelectedRows.Collection.filter(a => a.rowIndex != rowIndex);
                    this.SelectedRows.Changed.emit(false);
                    //this.SelectedRows.Remove(this.rows.filter(a => a.rowIndex == rowIndex)[0]);
                    this.rows.filter(a => a.rowIndex == rowIndex)[0].rowData.IsSelected = false;
                    this.RowUnselected.emit(this.rows.filter(a => a.rowIndex == rowIndex)[0].rowData);
                    if (this.cd) {
                        this.cd.detectChanges();
                    }
                }
                else {
                    //this.SelectedRows.push(item);
                    this.rows.filter(a => a.rowIndex == rowIndex)[0].rowData.IsSelected = true;
                    this.SelectedRows.Insert(this.rows.filter(a => a.rowIndex == rowIndex)[0]);
                    this.controller.cachedData[rowIndex].IsSelected = true;
                    if (this.cd) {
                        this.cd.detectChanges();
                    }
                    this.rowSelectedEvent.next({ colDef, colIndex, rowData, rowIndex, BackFromEdit: this.BackFromEdit, rowHeight: this.rowHeight, selectedId: rowData.$id, scrollTop: this.scrollTop });
                }

                //this.SelectedItemChanged.emit(this.SelectedRows);

            }
            else {
                this.rowSelectedEvent.next({ colDef, colIndex, rowData, rowIndex, BackFromEdit: this.BackFromEdit, rowHeight: this.rowHeight, selectedId: rowData.$id, scrollTop: this.scrollTop });
            }
        }
    }

    //onRowSelected(colDef: any, colIndex: number, rowData: any, rowIndex: number) {
    //    if (this.SpotLightCLicked == true) {
    //        this.SpotLightCLicked = false;
    //    }
    //    else {
    //        this.cd.detach();
    //        this.rowSelectedEvent.next({ colDef, colIndex, rowData, rowIndex });
    //    }
    //}
    AdvanceFilters: ApiQueryFilters;
    processQueryFilter(filters) {
        this.SearchFieldChanged = false;
        if (this.Filters == null) {
            this.Filters = new ApiQueryFilters();
        }
        if (this.AdvanceFilters == null) {
            this.AdvanceFilters = new ApiQueryFilters();
        }

        if (filters.IsDeleted || (filters.textValue == "" && filters.textValue.toString() != "false") || filters.textValue == "No Filter") {
            this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
        }
        else if (filters.TextValue == "NoDate") {
            if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.Filters.addAdditionalFilter(filters.FieldName, null, null, null, "NoDate", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, null, null, null, "NoDate", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

        }
        else if (!AppTool.IsNullOrEmpty(filters.MyName)) {
            var TommorowDate = DateTool.AddDays((new Date()), 1);
            TommorowDate.setUTCHours(0, 0, 0, 0);
            var TodayDate = new Date();
            TodayDate.setUTCHours(0, 0, 0, 0);
            var YesterdayDate = DateTool.AddDays((new Date()), -1);
            YesterdayDate.setUTCHours(0, 0, 0, 0);
            var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
            LastSevenDaysDate.setUTCHours(0, 0, 0, 0);
            var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
            LastThirtyDaysDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
            CurrentYearFromDate.setUTCHours(0, 0, 0, 0);
            var CurrentYearToDate = DateTool.AddDays((new Date()), 1);
            CurrentYearToDate.setUTCHours(0, 0, 0, 0);
            var LastYearFromDate = DateTool.AddDays((new Date()), -365);
            LastYearFromDate.setUTCHours(0, 0, 0, 0);
            var LastYearToDate = DateTool.AddDays((new Date()), 1);
            LastYearToDate.setUTCHours(0, 0, 0, 0);

            if (filters.TextValue == "Today") {
                filters.TextValue = TodayDate;
                filters.TextValue1 = TommorowDate;
                filters.MyName = "Today";
            }
            else if (filters.TextValue == "Yesterday") {
                filters.TextValue = YesterdayDate;
                filters.TextValue1 = TodayDate;
                filters.MyName = "Yesterday";
            }
            else if (filters.TextValue == "Last 7 Days") {
                filters.TextValue = LastSevenDaysDate;
                filters.TextValue1 = TommorowDate;
                filters.MyName = "Last 7 Days";
            }
            else if (filters.TextValue == "Last 30 Days") {
                filters.TextValue = LastThirtyDaysDate;
                filters.TextValue1 = TommorowDate;
                filters.MyName = "Last 30 Days";
            }
            else if (filters.TextValue == "Current Year") {
                filters.TextValue = CurrentYearFromDate;
                filters.TextValue1 = CurrentYearToDate;
                filters.MyName = "Current Year";

            }
            else if (filters.TextValue == "Last Year") {
                filters.TextValue = LastYearFromDate;
                filters.TextValue1 = LastYearToDate;
                filters.MyName = "Last Year";

            }
            if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.Filters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, "Between", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, "Between", filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
        }
        else if (filters.TextValue1) {
            if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.Filters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, filters.TextValue1, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
        }
        else {
            if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filters.FieldName).length > 0) {
                this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filters.FieldName);
            }
            if (filters.ObjectField.DataTypeCode == 'Boolean') {
                this.Filters.addAdditionalFilter(filters.FieldName, filters.TextValue.toString().toLowerCase(), null, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
            }
            else {
                this.Filters.addAdditionalFilter(filters.FieldName, filters.TextValue, null, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);

                this.AdvanceFilters.addAdditionalFilter(filters.FieldName, filters.TextValue, null, null, filters.Operation.Code, filters.ObjectField.IsCustomFilter, filters.ObjectField.DisplayInList, filters.ObjectField.IsCustom, filters.ObjectField.DataTypeCode);
            }
        }
        // console.log("processQueryFilter");
        this.init(true);
    }

    filterChanged(filters) {
        var LogGridIdPostFex = this.LogGridId.replace('LogGrid_', '');
        //if (LogGridIdPostFex != filters.ListComponentPostFex) {
        //    return;
        //}
        this.SearchFieldChanged = false;
        if (this.Filters == null) {
            this.Filters = new ApiQueryFilters();
        }

        filters.Filters.AdditionalFilters.forEach((filter, key) => {
            if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
            }
            this.Filters.AdditionalFilters.push(filter);
            //this.Filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType);
        });
        if (filters.Filters.SortBy != null) {
            this.sortingCol = filters.Filters.SortBy;
        }
        else {
            this.sortingCol = "";
        }
        if (filters.Filters.SortDirection != null) {
            this.sortingDir = filters.Filters.SortDirection;
        }
        else {
            this.sortingDir = "";
        }
        //console.log("filterChanged");
        this.init();
    }
    TotalWidth: number = 0;
    FiltersChangedsubscription: any;
    QueryChangeEventSub: any;
    ColumnsReadySub: any;
    ColumnsReady1Sub: any;
    MenuHeaderSub: any;
    SearchFieldsSub: any;
    HLineSub: any;
    pubSubAdvanceQueryFiltersSub: any;
    SearchFieldChanged: boolean = false;
    ngOnInit() {
    
        if (this.IsCustomTemplate) {
            this.rowHeight = 27;
        }
        if (this.columns && this.columns.length > 0) {
            var index = 0
          var left = 0;
          if (this.ObjectTable) {
            var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.Id == this.queryId)[0];
            if (query && !AppTool.IsNullOrEmpty(query.SpotlightDataTemplate)) {
              this.IsSpotLight = true;
              this.SpotlightDataTemplate = query.SpotlightDataTemplate;
              if (this.cd) {
                this.cd.detectChanges();
              }
            }
          }
            if (this.IsSpotLight == true) {
                left = 25;
            }
            //if (this.ShowArrow == true) {
            //    left += 25;
            //}
            this.TotalWidth = 0;
            this.columns.forEach((value, key) => {
                this.ColumnsQueryId = value.QueryId;
                value.index = index;
                index++;
                if (value.Editable == undefined) {
                    value.Editable = true;
                }
                //value.Style = {
                //    width: + value.width + 'px',
                //    'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                //    'text-align': value.Alignment ? value.Alignment : 'right',
                //    'display': value.visibility == 'hidden' ? 'none' : 'inline-block'
                //};
                if (this.RTL) {
                    value.Styles = {
                        width: + (value.Styles.width.replace("px", "")) + 'px',
                        right: left + 'px',
                    };
                }
                else {
                    value.Styles = {
                        width: + (value.Styles.width.replace("px", "")) + 'px',
                        left: left + 'px',
                    };
                }

                left += (+(value.Styles.width.replace("px", "")));
                this.TotalWidth += + (value.Styles.width.replace("px", ""));
                //this.customcolumns.push(value);
            });
            this.headerStyle = {
                'width': this.TotalWidth + 43 + 'px',
                'min-width': '100%'//(this.ViewWidth) + 'px'
            };
            this.canvasHeight = {
                height: this.rowCount * this.rowHeight + 'px',
                //'width': this.TotalWidth + 'px',
                //'min-width': '100%'//(this.ViewWidth) + 'px'
            };
            if (this.cd) {
                this.cd.detectChanges();
            }
        }
        if (this.ColumnsReady) {
            this.ColumnsReadySub = this.ColumnsReady.subscribe((res) => {
                var index = 0
                var left = 0;
                if (this.IsSpotLight == true) {
                    left = 25;
                }
                //if (this.ShowArrow == true) {
                //    left += 25;
                //}
                this.TotalWidth = 0;
                this.columns.forEach((value, key) => {
                    this.ColumnsQueryId = value.QueryId;
                    value.index = index;
                    index++;
                    if (value.Editable == undefined) {
                        value.Editable = true;
                    }
                    //value.Style = {
                    //    width: + value.width + 'px',
                    //    'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                    //    'text-align': value.Alignment ? value.Alignment : 'right',
                    //    'display': value.visibility == 'hidden' ? 'none' : 'inline-block'
                    //};
                    if (this.RTL) {
                        value.Styles = {
                            width: + (value.Styles.width.replace("px", "")) + 'px',
                            right: left + 'px',
                        };
                    }
                    else {
                        value.Styles = {
                            width: + (value.Styles.width.replace("px", "")) + 'px',
                            left: left + 'px',
                        };
                    }

                    left += (+(value.Styles.width.replace("px", "")));
                    this.TotalWidth += + (value.Styles.width.replace("px", ""));
                    //this.customcolumns.push(value);
                });
                this.headerStyle = {
                    'width': this.TotalWidth + 43 + 'px',
                    'min-width': '100%'//(this.ViewWidth) + 'px'
                };
                this.canvasHeight = {
                    height: this.rowCount * this.rowHeight + 'px',
                    //'width': this.TotalWidth + 'px',
                    //'min-width': '100%'//(this.ViewWidth) + 'px'
                };
                //var Cols = document.getElementById(this.LogGridColumnsId);
                ////if (Cols) {
                //    Cols.classList.remove("GeneratedGridHeader");
                //    Cols.classList.add("EditableGridHeader");
                ////}
                //var RowsDiv = document.getElementById(this.LogGridRowsId);
                ////if (RowsDiv) {
                //    RowsDiv.classList.remove("GeneratedGridBody");
                //    RowsDiv.classList.add("EditableGridBody");
                ////}
                if (this.cd) {
                    this.cd.reattach();
                    this.cd.detectChanges();
                }
            });
        }
        if (this.CustomColumnsReady) {
            this.ColumnsReady1Sub = this.CustomColumnsReady.subscribe((res) => {
                var index = 0
                var left = 0;
                //if (this.ShowArrow == true) {
                //    left += 25;
                //}
                this.TotalWidth = 0;
                this.columns = res;
                this.columns.forEach((value, key) => {
                    this.ColumnsQueryId = value.QueryId;
                    value.index = index;
                    index++;
                    if (value.Editable == undefined) {
                        value.Editable = true;
                    }
                    //value.Style = {
                    //    width: + value.width + 'px',
                    //    'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                    //    'text-align': value.Alignment ? value.Alignment : 'right',
                    //    'display': value.visibility == 'hidden' ? 'none' : 'inline-block'
                    //};
                    if (this.RTL) {
                        value.Styles = {
                            width: + (value.Styles.width.replace("px", "")) + 'px',
                            right: left + 'px',
                        };
                    }
                    else {
                        value.Styles = {
                            width: + (value.Styles.width.replace("px", "")) + 'px',
                            left: left + 'px',
                        };
                    }
                    left += (+(value.Styles.width.replace("px", "")));
                    this.TotalWidth += + (value.Styles.width.replace("px", ""));
                    //this.customcolumns.push(value);
                });
                this.headerStyle = {
                    'width': this.TotalWidth + 43 + 'px',
                    'min-width': '100%'//(this.ViewWidth) + 'px'
                };
                this.canvasHeight = {
                    height: this.rowCount * this.rowHeight + 'px',
                    //'width': this.TotalWidth + 'px',
                    //'min-width': '100%'//(this.ViewWidth) + 'px'
                };
            });
        }

        var xx = this.rows;
        this.FiltersChangedsubscription = this.CurrentSession.PubSubFiltersChangeEventService.Stream.subscribe(change => this.filterChanged(change));
        if (this.pubSubAdvanceQueryFiltersServiceRecived) {
            this.pubSubAdvanceQueryFiltersSub = this.pubSubAdvanceQueryFiltersServiceRecived.Stream.subscribe(filters => this.processQueryFilter(filters));
        }
        if (this.QueryChangeEvent) {
            this.QueryChangeEventSub = this.QueryChangeEvent.subscribe((res) => {
                //console.log("QueryChangeEvent " + this.searchFields);
                //this.rows = [];
                if (res.MustIgnoreItems) {
                    this.MustIgnoreRowIndexes = res.MustIgnoreItems;
                }
                if (this.Filters == null || res.Reload) {
                    this.Filters = new ApiQueryFilters();
                    this.AdvanceFilters = new ApiQueryFilters();
                    //this.rows = [];
                    //this.cd.detectChanges();
                }
                if (res.SearchFieldChanged == true) {
                    this.SearchFieldChanged = true;
                }
                res.Filters.AdditionalFilters.forEach((filter, key) => {
                    if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                        this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                    }
                    this.Filters.AdditionalFilters.push(filter);
                    //if (this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                    //    this.AdvanceFilters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                    //}
                    //this.AdvanceFilters.AdditionalFilters.push(filter);
                    //this.Filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType);
                });

                //if (this.AdvanceFilters) {
                //    this.AdvanceFilters.AdditionalFilters.forEach((filter, key) => {
                //        if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                //            this.Filters.AdditionalFilters = this.AdvanceFilters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                //        }
                //        this.Filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType);
                //    });
                //}
                //this.Filters = res.Filters;
                this.queryId = res.QueryId;
                this.init(true);
            });
        }
        if (this.SearchFieldchangeevent) {
            this.SearchFieldsSub = this.SearchFieldchangeevent.subscribe((res) => {

                if (!this.Filters) {
                    this.Filters = new ApiQueryFilters();
                }
                if (!AppTool.IsNullOrEmpty(res)) {
                    this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
                    this.Filters.addAdditionalFilter("SearchFields", res, null, null, "Contains", false, true, false, "String");
                    this.searchFields = res;
                }
                else {
                    this.Filters.Filter1Value = "";
                    this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
                    this.searchFields = undefined;
                }
                //console.log("SearchFieldchangeevent");
                this.init(true);
            });
        }
        if (this.MenuHeaderchangeevent) {
            this.MenuHeaderSub = this.MenuHeaderchangeevent.subscribe((res) => {
                if (res.Filters != null) {
                    if (res.Filters.SortBy) {
                        this.sortingCol = res.Filters.SortBy;
                        this.dataSource.sortingCol = res.Filters.SortBy;
                    }
                    if (res.Filters.SortDirection) {
                        this.sortingDir = res.Filters.SortDirection;
                        this.dataSource.sortingDir = res.Filters.SortDirection;
                    }
                    res.Filters.AdditionalFilters.forEach((filter, key) => {
                        if (this.Filters && filter.IgnoreFilter) {
                            this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                        }
                        else {
                            if (this.Filters == null) {
                                this.Filters = new ApiQueryFilters();
                            }
                            if (this.Filters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                                this.Filters.AdditionalFilters = this.Filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                            }
                            this.Filters.AdditionalFilters.push(filter);
                            //this.Filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, false, filter.DisplayInList, false, filter.FieldDataType);
                        }
                    });
                    this.MenuHeaderchanged.emit(res.Filters);
                }
                //console.log("MenuHeaderchangeevent");
                this.init(true);
            });
        }
        if (this.ShowHLineOverRow) {
            this.HLineSub = this.ShowHLineOverRow.subscribe((RowIndex) => {
                if (this.rows.filter(a => a.rowIndex == RowIndex).length > 0) {
                    this.controller.cachedData[RowIndex].ShowLineOverRow = true;//[0].ShowLineOverRow = true;
                    //this.updateDisplayList();
                }
                //var BuffRow = this.controller.cacheBuffer.filter(a => a.rowIndex === RowIndex)[0];
                //if (BuffRow) {
                //    BuffRow.ShowLineOverRow = true;
                //}
                var Row = this.rows.filter(a => a.rowIndex === RowIndex)[0];
                if (Row) {
                    Row.rowData.ShowLineOverRow = true;
                }
                if (this.cd) {
                    this.cd.reattach();
                    this.cd.detectChanges();
                }
                //var elem = document.getElementById(this.LogGridId + 'row' + RowIndex);
                //if (elem) {
                //    elem.className = "Row ag-row HLine";
                //}
            });
        }
        this.CustomBackFromEdit.subscribe((res) => {
            //alert(res);
            res.forEach((value, key) => {
                if (this.rows.filter(a => a.rowIndex == value.rowIndex).length > 0) {
                    this.controller.cachedData[value.rowIndex] = value.rowData;
                    this.updateDisplayList();
                }
            });
        });
        if (this.CheckBoxFilterChanged) {
            this.CheckBoxFilterChanged.subscribe((res) => {
                this.UseFilteredRecordsCheckBox = res.UseFilteredCheckBox;
                if (this.rows) {
                    this.rows.forEach((item, key) => {
                        if (this.UseFilteredRecordsCheckBox == true) {
                            if (this.rows.filter(a => a.rowIndex == item.rowIndex).length > 0) {
                                var Result = false;
                                if (this.controller.cachedData[item.rowIndex].IsCustomChecked == true) {
                                    Result = this.controller.cachedData[item.rowIndex].IsChecked;
                                }
                                else if (this.MustFilterRowIndexes.filter(a => a.Id == item.rowData.Id).length > 0) {
                                    Result = this.MustFilterRowIndexes.filter(a => a.Id == item.rowData.Id)[0].IsChecked;
                                }
                                else if (res.IsAutoRecClicked == false && (this.MustIgnoreRowIndexes.filter(a => a.Id == item.rowData.Id).length > 0)) {
                                    Result = this.MustIgnoreRowIndexes.filter(a => a.Id == item.rowData.Id)[0].IsChecked;
                                }
                                else if (res.IsAutoRecClicked == false) {
                                    Result = item.rowData[res.FilteredRecordsCheckedFieldName] == res.FilteredRecordsCheckedFieldValue;
                                }
                                this.controller.cachedData[item.rowIndex].IsChecked = Result;
                                //this.controller.cachedData[item.rowIndex].IsChecked = this.controller.cachedData[item.rowIndex].IsCustomChecked == true ? this.controller.cachedData[item.rowIndex].IsChecked : (this.MustFilterRowIndexes.filter(a => a == item.rowData.Id) ? false : item.rowData[res.FilteredRecordsCheckedFieldName] == res.FilteredRecordsCheckedFieldValue);
                            }
                            var Row = this.rows.filter(a => a.rowIndex === item.rowIndex)[0];
                            if (Row) {
                                var Result = false;
                                if (Row.rowData.IsCustomChecked == true) {
                                    Result = Row.rowData.IsChecked;
                                }
                                else if (this.MustFilterRowIndexes.filter(a => a.Id == item.rowData.Id).length > 0) {
                                    Result = this.MustFilterRowIndexes.filter(a => a.Id == item.rowData.Id)[0].IsChecked;;
                                }
                                else if (res.IsAutoRecClicked == false && (this.MustIgnoreRowIndexes.filter(a => a.Id == item.rowData.Id).length > 0)) {
                                    Result = this.MustIgnoreRowIndexes.filter(a => a.Id == item.rowData.Id)[0].IsChecked;
                                }
                                else if (res.IsAutoRecClicked == false){
                                    Result = item.rowData[res.FilteredRecordsCheckedFieldName] == res.FilteredRecordsCheckedFieldValue;
                                }
                                Row.rowData.IsChecked = Result;
                                //Row.rowData.IsChecked = Row.rowData.IsCustomChecked == true ? Row.rowData.IsChecked : (this.MustFilterRowIndexes.filter(a => a == item.rowData.Id) ? false : item.rowData[res.FilteredRecordsCheckedFieldName] == res.FilteredRecordsCheckedFieldValue);
                            }
                        }
                        else {
                            if (this.rows.filter(a => a.rowIndex == item.rowIndex).length > 0) {
                                this.controller.cachedData[item.rowIndex].IsChecked = false;
                            }
                            var Row = this.rows.filter(a => a.rowIndex === item.rowIndex)[0];
                            if (Row) {
                                Row.rowData.IsChecked = false;
                            }
                        }

                    });
                    //this.updateDisplayList();
                    if (this.cd) {
                        this.cd.detectChanges();
                    }
                }
            })
        }

        if (this.FireCheckBoxChecked) {
            this.FireCheckBoxChecked.subscribe((res) => {
                this.OnCheckBoxChecked(res.rowData, res.IsChecked, res.RowIndex, true,res.ById);
                this.updateDisplayList();
            });
        }
        this.MarkIsChecked.subscribe((res) => {
            this.AllCheckedRecords = res.AllRecords;
            var Row = this.rows.filter(a => a.rowData.Id === res.MyRecord.Id)[0];
            if (Row) {
                Row.rowData.IsChecked = true;
                if (this.controller.cachedData[Row.rowIndex] && this.controller.cachedData[Row.rowIndex].IsChecked != null) {
                    this.controller.cachedData[Row.rowIndex].IsChecked = true;//this.controller.cachedData[row.rowIndex] && this.controller.cachedData[row.rowIndex].IsChecked;// == false ? false : true;
                }
            }
            this.cd.detectChanges();
            //else {
            //    row.rowData.IsChecked = true;
            //}
        });

    }
    AllCheckedRecords: any[] = [];

    ngOnChanges(changes: { [propName: string]: SimpleChange }) {
        if (changes['ReattachToDetection']) {
            this.cd.reattach();
            this.cd.detectChanges();
        }
        else {
            console.log("in else");
        }

    }

    public LastHeaderColumnWidth: number;
    ngAfterViewInit() {
        //for (var i = 0; i < this.columns.length; i++) {
        //    this.columns[i].Styles = { width: this.columns[i].Styles.width };
        //}
        //if (this.RTL == true) {
        //    var innerelem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);
        //    if (innerelem) {
        //        this.HScrollPosition = innerelem.scrollLeft;
        //    }
        //}

        var xx = this.Filters;
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridId);
        this.viewportSize = Math.round(elem.clientHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
        this.LogGridElement = elem;
        this.scrollPosition = elem.scrollTop;
        if (this.IsCustomTemplate) {
            this.rowHeight = 27;
            elem.classList.remove("GeneratedGrid");
            elem.classList.add("EditableGrid");
            var Cols = document.getElementById(this.LogGridColumnsId);
            if (Cols) {
                Cols.classList.remove("GeneratedGridHeader");
                Cols.classList.add("EditableGridHeader");
            }
            var RowsDiv = document.getElementById(this.LogGridRowsId);
            if (RowsDiv) {
                RowsDiv.classList.remove("GeneratedGridBody");
                RowsDiv.classList.add("EditableGridBody");
            }
        }
        var vscroll = elem.scrollTop;
        var hscroll = elem.scrollLeft;
        this.controller.setDataSource(this.dataSource);
        if (this.autoLoad == true) {
            //console.log("autoLoad");
            this.init();
        }
        this.AfterViewInitCompleted.emit("Complete");
    }

    sortingDir: string = ''; //'Descending';
    sortingCol: string = ''; //'CreateDateTime';
  AfterServerSort: boolean = false;
  ServerSort(colDef, id, forced: boolean = false) {
        //var div = element.parentNode.parentNode;
    //console.log(div.getAttribute('id'));
    if (forced == false) {
      this.selectedRow = null;
      this.MySelectedRowIndex = null;
      this.SelectedRow = null;
      
    }
        this.SortInvoked.emit({ colDef: colDef, id: id });
        this.SearchFieldChanged = false;
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridId);
        if (elem) {
            elem.scrollTop = 0;
            this.scrollTop = 0;
        }
        if (this.cd) {
            this.cd.reattach();
            this.cd.detectChanges();
        }
        ////console.log(colDef.ServerSideSortable);

        if (colDef.ServerSideSortable) {
            this.AfterServerSort = true;
            if (!AppTool.IsNullOrEmpty(colDef.SortByName)) {
                if (colDef.SortByName != this.sortingCol) {
                    this.sortingDir = '';
                    this.sortingCol = colDef.SortByName;
                }
                this.dataSource.sortingCol = colDef.SortByName;
            }
            else {
                if (colDef.FieldName != this.sortingCol) {
                    this.sortingDir = '';
                    this.sortingCol = colDef.FieldName;
                }
                this.dataSource.sortingCol = colDef.FieldName;
            }



            ////console.log("sortingCol", this.dataSource.sortingCol);
            ////console.log("sortingDir", this.dataSource.sortingDir);
            if (this.MyisBackFromEdit == false) {
                switch (this.sortingDir) {
                    case '':
                        {
                            ////console.log("this.dataSource.sortingDir", this.dataSource.sortingDir);
                            this.dataSource.sortingDir = "Ascending";
                            this.sortingDir = "Ascending";
                            ////console.log("this.dataSource.sortingDir", this.dataSource.sortingDir);
                            break;
                        }
                    case "Ascending":
                        {
                            ////console.log("this.dataSource.sortingDir", this.dataSource.sortingDir);
                            this.dataSource.sortingDir = "Descending";
                            this.sortingDir = "Descending";
                            // //console.log("this.dataSource.sortingDir", this.dataSource.sortingDir);
                            break;
                        }
                    case "Descending":
                        {
                            ////console.log("this.dataSource.sortingDir", this.dataSource.sortingDir);
                            this.dataSource.sortingDir = '';
                            this.sortingDir = '';
                            // //console.log("this.dataSource.sortingDir", this.dataSource.sortingDir);
                            break;
                        }
                    default:
                    //  //console.log("error sorting");
                }
            }
            else {
                this.MyisBackFromEdit = false;
            }
            var allelems = document.getElementsByClassName("ag-header-cell");
            //for (var a = 0; a < allelems.length; a++) {
            //    ////console.log(allelems.item(a));
            //    if (allelems.item(a).attributes['LogGridId'].value == this.LogGridId) {
            //        itemStyling(allelems.item(a));
            //    }
            //    //    allelems.item(a).style.color = 'white';
            //    //    allelems.item(a).style.background = '-moz-linear-gradient(50% 100% 90deg,rgba(112, 112, 112, 1) 0%,rgba(168, 168, 168, 1) 100%)';
            //    //    allelems.item(a).style.background = '-webkit-linear-gradient(90deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
            //    //    allelems.item(a).style.background = '-webkit-gradient(linear,50% 100%,50% 0%,color-stop(0,rgba(112, 112, 112, 1) ),color-stop(1,rgba(168, 168, 168, 1) ))';
            //    //    allelems.item(a).style.background = '-o-linear-gradient(90deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
            //    //    allelems.item(a).style.background = 'linear-gradient(0deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
            //}
            if (this.sortingDir === 'Descending' || this.sortingDir === 'Ascending') {

                var ColumnsElements = document.getElementsByClassName("ag-header-cell");
                for (var i = 0; i < ColumnsElements.length; i++) {
                    if (ColumnsElements[i].attributes['LogGridId'].value == this.LogGridId && ColumnsElements[i].attributes['colid']) {
                        if (ColumnsElements[i].attributes['colid'].value == colDef.FieldName) {
                            (<HTMLElement>ColumnsElements[i]).style.color = 'rgb(103, 103, 103)';
                            (<HTMLElement>ColumnsElements[i]).style.background = '#cfcbcb';
                        }
                    }
                    //ColIndexes.push({ FieldName: ColumnsElements[i].attributes['colid'].value, Index: +(ColumnsElements[i].id.split(',')[1]), Width: ColumnsElements[i].clientWidth });
                }
                //var chosen = document.getElementById(id);//this.ColumnId + "resizable-column-," + this.ColIndex); 
                //chosen.style.color = 'rgb(103, 103, 103)';
                //chosen.style.background = '#cfcbcb';
            }

            this.rows = [];
            //this.init();
            this.updateDisplayList();
            //var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
            //if (columns) {
            //    columns.style.left = (-1 * this.HScrollPosition) + "px";
            //}

        }
    }

    cellClicked(field, value, rowIndex, colIndex) {
        //////console.log(field, value, rowIndex, colIndex);
    }
    rowClicked(rowIndex, row) {
        //////console.log(rowIndex, row);
    }
    customHeight: number;
    requestedRowsReadySub: any;
    requestedRowCountSub: any;
    SpotlightDataTemplate: string;
    public MyScrollTop: number = 0;
    public MySelectedRowIndex: number = null;
    init(reload: boolean = false) {
        if (this.UseBusyIndecator)
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");

        //this.rows = [];
        if (this.ObjectTable) {
            var query = window.Queries.filter(q => q.ObjectTableId == this.ObjectTable.Id && q.Id == this.queryId)[0];
            if (query && !AppTool.IsNullOrEmpty(query.SpotlightDataTemplate)) {
                this.IsSpotLight = true;
                this.SpotlightDataTemplate = query.SpotlightDataTemplate;
                if (this.cd) {
                    this.cd.detectChanges();

                }
            }
            else {
                this.IsSpotLight = false;
                this.SpotlightDataTemplate = null;

            }
        }
        this.rowsBuffer = [];

        //this.controller.requestedRowCount.subscribe((res) => {
        //    //////console.log(res);
        //    this.rowCount = res;
        //});
        //elem[0].addEventListener('scroll', this.onScroll);
        var logGrid = document.getElementById(this.LogGridId);
        if (this.ViewHeight == null || this.ViewWidth == null || (this.ViewHeight <= 0 && this.ViewWidth <= 0) || this.ViewHeight < logGrid.clientHeight) {
            //console.log(this.LogGridId);
            //var logGrid = document.getElementById(this.LogGridId);
            //var viewHeight = logGrid.clientHeight;
            this.ViewHeight = logGrid.clientHeight;
            this.ViewWidth = logGrid.clientWidth;
            this.headerStyle = {
                'width': this.TotalWidth + 43 + 'px',
                'min-width': '100%'//(this.ViewWidth) + 'px'
            };
            this.canvasHeight = {
                height: this.rowCount * this.rowHeight + 'px',
                //'width': this.TotalWidth + 'px',
                //'min-width': '100%'//(this.ViewWidth) + 'px'
            };
        }

        //this.headerStyle.width = (this.ViewWidth - 2) + 'px';
        //this.headerStyle[min-width] = (this.ViewWidth - 2) + 'px';

        this.rowStyle.minWidth = this.ViewWidth + 'px';
        //this.rowStyle.width = this.ViewWidth + 'px';

        ////console.log("headerStyle", this.headerStyle, "ViewWidth", this.ViewWidth, "rowStyle", this.rowStyle);
        this.viewportSize = Math.round(this.ViewHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
        //////console.log("viewportSize", this.viewportSize);
        //this.numberOfCells = 3 * this.viewportSize;
        //this.rowsPerPage = 20;
        this.rowsPerPage = this.dataSource.pageSize;
        //////console.log("rowsPerPage", this.rowsPerPage);//, "numberOfCells", this.numberOfCells);
        //if (this.rows) {

        //}
        //////console.log("after canvasHeight: ", this.canvasHeight.height, this.rowCount);
        //////console.log("numberOfTotalPages", this.numberOfTotalPages, "pixelsPerPage", this.pixelsPerPage);
        //////console.log("after canvasHeight: ", this.canvasHeight.height, this.rows.length);
        this.rows = [];
        if (this.requestedRowsReadySub) {
            this.requestedRowsReadySub.unsubscribe();
        }
        this.requestedRowsReadySub = this.controller.requestedRowsReady.subscribe((res) => {
            //////console.log(res);

            this.renderRows(res);
            if (this.cd) {
                this.cd.reattach();
                this.cd.detectChanges();
                //console.log("Inside detectChanges " + res.length + " this.columns " + this.columns.length);
                //setTimeout(() => this.DoIt(), 10);
            }
            else {
                this.DetectChangesTimer();
            }

        });
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);

        if (elem) {
            elem.scrollTop = 0;
        }
        if (this.requestedRowCountSub) {
            this.requestedRowCountSub.unsubscribe();
        }
        this.requestedRowCountSub = this.controller.requestedRowCount.subscribe((res) => {

            this.rowCount = res;
            this.CountReady.emit(res);
            //this.rows = new Array(this.rowCount)
            this.dataSource.rowCount = res;
            this.canvasHeight = {
                height: this.rowCount * this.rowHeight + 'px',
                //'width': this.TotalWidth + 'px',
                //'min-width': '100%'//(this.ViewWidth) + 'px'
            };
            //var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
            var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);

            if (elem) {
                elem.scrollTop = 0;
            }
            this.customHeight = this.rowCount * this.rowHeight;
            this.height = this.rowCount * this.rowHeight;
            this.numberOfTotalPages = this.rowCount / this.rowsPerPage;
            if (this.rowCount > 0) {
                this.updateDisplayList(false);
            }
            else {
                if (this.cd) {
                    this.cd.reattach();
                    this.cd.detectChanges();
                }
            }
            if (this.MyScrollTop != 0) {
                elem.scrollTop = this.MyScrollTop;
            }
            //this.BackFromEditSub = this.CurrentSession.BackFromEdit.subscribe((res) => {
            //    this.BackFromEditSub.unsubscribe();
            //    this.BackFromEditSub = null; 
            //    this.MyScrollTop = (res.rowIndex * this.rowHeight) - this.rowHeight;
            //});
        });
        this.GetRowCount(reload);
       };
    rowsBuffer: IRow[] = [];
    SpotLightHeight: number = 116;
    DetectChangeTimerInterVal: number = 10;
    DoIt() {
        //console.log("inside DoIt");
        this.cd.detectChanges()
    }
    DetectChangesTimer() {
        //console.log("Inside DetectChangesTimer " + this.DetectChangeTimerInterVal);
        setTimeout(() => {
            if (this.cd) {
                this.cd.detectChanges();
                //console.log("Inside DetectChangesTimer Change Detected" + this.DetectChangeTimerInterVal);
                return;
            }
            else {
                if (this.DetectChangeTimerInterVal < 10000) {
                    //console.log("Inside DetectChangesTimer Still Null" + this.DetectChangeTimerInterVal);
                    this.DetectChangeTimerInterVal *= 50;
                    this.DetectChangesTimer();
                }
                else {
                    //console.log("Inside DetectChangesTimer Still Null Interval Ended" + this.DetectChangeTimerInterVal);
                    return;
                }
            }
        }, this.DetectChangeTimerInterVal);
    }
    IsDataLoaded: boolean = false;
    renderRows(res: any[]) {    

        if (res.length < this.viewportSize || res.length < this.rowsPerPage) {
            this.ViewPortRowCount = res.length;
        }

        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        var currentPage = Math.max(Math.floor(this.scrollTop / this.rowHeight / this.rowsPerPage), 0);
        var top = 0;
        var Detailstop = 0;
        res.forEach((value, key) => {
            if (value.RowIndex >= this.rowCount) {
                return;
            }

            var buffered = this.rowsBuffer.filter(a => a.rowIndex === value.RowIndex)[0];
            if (!buffered) {

                var rowData = value.RowData;

                //if (!this.SelectedRow && this.SelectFirstRow) {
                var test = this.SelectedRow ? this.SelectedRow.$id == value.RowData.$id : true;
                if ((!this.SelectedRow || this.SelectedRow.$id == value.RowData.$id) && this.SelectFirstRow) {
                    this.SelectedRow = value.RowData;
                    this.FirstRowSelected.emit({ SelectedRow: this.SelectedRow, index: value.RowIndex });
                    //console.log("InSide SelectedRow");
                }
                if (this.MySelectedRowIndex != null) { // attempt to select the selected row when back from edit-----mohammad.
                    if (value.RowIndex == this.MySelectedRowIndex) {
                        this.SelectedRow = value.RowData;
                        this.FirstRowSelected.emit({ SelectedRow: this.SelectedRow, index: value.RowIndex });
                    }
                }
                if (value.RowData) {


                    var row: IRow = { rowIndex: value.RowIndex, rowData: value.RowData, DetailsIcon: "./Images/SpotLightPlusIcon.png" };

                    row.styles = {
                        'top': (value.RowIndex * this.rowHeight) + "px",
                        'min-width': this.TotalWidth + 22 + 'px',
                        'width:': this.TotalWidth + 'px',
                        'max-width:': this.TotalWidth + 'px',
                    };
                    //if (row.rowData) {
                    if (this.IsAllRecordsChecked == true) {
                        //if (this.rows.filter(a => a.rowIndex == value.rowIndex).length > 0) {
                        if (this.controller.cachedData[row.rowIndex] && this.controller.cachedData[row.rowIndex].IsChecked != null) {
                            row.rowData.IsChecked = this.controller.cachedData[row.rowIndex] && this.controller.cachedData[row.rowIndex].IsChecked;// == false ? false : true;
                        }
                        else {
                            row.rowData.IsChecked = true;
                        }
                        //var Row = this.rows.filter(a => a.rowIndex === row.rowIndex)[0];
                        //if (Row) {
                        //    Row.rowData.IsChecked = this.controller.cachedData[value.rowIndex].IsChecked ? this.controller.cachedData[value.rowIndex].IsChecked : true;
                        //}
                        //}
                        //else {
                        //    value.RowData.IsChecked = true;
                        //}
                    }
                    //else if (this.AllCheckedRecords && this.AllCheckedRecords.length > 0) {
                    //    var Row = this.AllCheckedRecords.filter(a => a.Id === row.rowData.Id)[0];
                    //    if (Row) {
                    //        row.rowData.IsChecked = true;
                    //        if (this.controller.cachedData[row.rowIndex] && this.controller.cachedData[row.rowIndex].IsChecked != null) {
                    //            this.controller.cachedData[row.rowIndex].IsChecked = true;//this.controller.cachedData[row.rowIndex] && this.controller.cachedData[row.rowIndex].IsChecked;// == false ? false : true;
                    //        }
                    //    }
                    //}
                    else {
                        //if (this.rows.filter(a => a.rowIndex == value.rowIndex).length > 0) {
                        row.rowData.IsChecked = this.controller.cachedData[row.rowIndex] ? this.controller.cachedData[row.rowIndex].IsChecked : null;// == true ? true : false;
                        if (row.rowData.IsChecked == true) {
                            console.log("Grid Display True");
                        }
                        //var Row = this.rows.filter(a => a.rowIndex === value.rowIndex)[0];
                        //if (Row) {
                        //    Row.rowData.IsChecked = this.controller.cachedData[value.rowIndex].IsChecked ? this.controller.cachedData[value.rowIndex].IsChecked : false;
                        //}
                        //}
                        //else {
                        //    value.RowData.IsChecked = false;
                        //}
                    }


                    //row.Detailsstyles = {
                    //    'top': (value.RowIndex * this.rowHeight) + (temp.length * this.SpotLightHeight) + this.rowHeight + "px",
                    //    'width:': '100%',
                    //};

                    this.rowsBuffer.push(row);
                    //}
                }

            }
        });

        var Detect = false;
        //if (this.SelectedSpotLightIndex == null) {
        //    this.SelectedSpotLightIndex = 0;
        //}
        this.rowsBuffer.forEach((item) => {
            var existingItem = this.rows.filter(d => d.rowIndex === item.rowIndex)[0];
            if (!existingItem) {
                if (item.rowIndex > this.SelectedSpotLightIndex && this.ShowSpot == true) {
                    top = (item.rowIndex * this.rowHeight) + (this.SpotLightHeight);
                    Detailstop = (top + this.rowHeight);
                }
                else if (this.SelectedSpotLightIndex != null && item.rowIndex == this.SelectedSpotLightIndex) {
                    item.ShowDetails = true;
                    if (!item.ShowDetails) {
                        item.DetailsIcon = "./Images/SpotLightPlusIcon.png";
                    }
                    else {
                        item.DetailsIcon = "./Images/spotlightMinusIcon.png";
                    }
                    top = (item.rowIndex * this.rowHeight);
                    Detailstop = (top + this.rowHeight);
                }
                else {
                    top = (item.rowIndex * this.rowHeight);
                }
                item.styles = {
                    'top': top + "px",
                    'min-width': this.TotalWidth + 22 + 'px',//'min-width': (this.ViewWidth) + 'px',
                    'width:': this.TotalWidth + 'px',
                    'max-width:': this.TotalWidth + 'px',
                };
                item.Detailsstyles = {
                    'top': Detailstop + "px",
                    'width:': '100%',
                };
                if (this.AllCheckedRecords && this.AllCheckedRecords.length > 0) {
                    var Row = this.AllCheckedRecords.filter(a => a.Id === item.rowData.Id)[0];
                    if (Row) {
                        item.rowData.IsChecked = true;
                        if (this.controller.cachedData[item.rowIndex]) {
                            this.controller.cachedData[item.rowIndex].IsChecked = true;//this.controller.cachedData[row.rowIndex] && this.controller.cachedData[row.rowIndex].IsChecked;// == false ? false : true;
                        }
                    }
                }
                this.rows.push(item);
                if (this.cd) this.cd.detectChanges();
                Detect = true;
            }
        });
       
        this.rowsBuffer = [];
        this.DataLoaded.emit(this.rows);
        if (this.SearchFieldChanged == true) {
            this.IsDataLoaded = true;
        }
        if (this.UseBusyIndecator)
            this.CurrentSession.CurrentWindow.StopBusyIndicator();

        //console.log("this.rows.length :" + this.rows.length);
        //console.log("this.tripleViewport :" + this.tripleViewport);
        //console.log("this.viewportSize :" + this.viewportSize);
        if (this.rows.length > this.tripleViewport) {
            var diff = this.rows.length - this.tripleViewport;
            if (this.scrollDirection == "up") {
                /**
                Calculate lowest index of rows and delete from there
                */
                this.rows.splice(diff, diff - this.viewportSize);
                //console.log("roooooows.lenght up", this.rows.length);
            }
            else if (this.scrollDirection == "down") {
                /**
                Calculate lowest index of rows and delete from there
                */
                //console.log("roooooows.lenght down", this.rows.length);
                ////console.log("rows.length", this.rows.length);
                /*this.rows = */
                this.rows.splice(0, diff - this.viewportSize);
                ////console.log("rows.length", this.rows.length);
                //////console.log("rows.length", this.rows.length);
            }
        }

     

    }
    SelectedSpotLightIndex: number;
    ShowSpot: boolean = false;
    onSpotLightSelect(rowIndex) {
        if (this.SelectedSpotLightIndex == rowIndex.rowIndex) {
            this.SelectedSpotLightIndex = null;
        }
        else {
            this.SelectedSpotLightIndex = rowIndex.rowIndex;
        }
        this.SpotLightCLicked = true;
        var DetailsCount = 0;
        var i = 0;
        var DetailsDivsHeight = 0;
        this.rows.filter(a => a.rowIndex == rowIndex.rowIndex)[0].ShowDetails = !(this.rows.filter(a => a.rowIndex == rowIndex.rowIndex)[0].ShowDetails);
        this.rows.forEach((value, key) => {
            //console.log("rows value.rowIndex :" + value.rowIndex);
            var top = 0;
            var Detailstop = 0;
            if (value == rowIndex) {
                //value.ShowDetails = !value.ShowDetails;
                if (value.ShowDetails == false) {
                    DetailsCount = 0;
                }
                this.ShowSpot = value.ShowDetails;
                if (!value.ShowDetails) {
                    value.DetailsIcon = "./Images/SpotLightPlusIcon.png";

                }
                else {
                    value.DetailsIcon = "./Images/spotlightMinusIcon.png";
                    //ShowSpot = true;
                }
            }
            else {
                value.DetailsIcon = "./Images/SpotLightPlusIcon.png";
                value.ShowDetails = false;
                //ShowSpot = false;
            }
            //var DetailsDiv = 50;//document.getElementById('DetailsDiv' + (value.rowIndex));
            //DetailsDivsHeight += DetailsDiv.clientHeight;
            //if (value.ShowDetails == true) {
            //    value.ShowDetails = true;
            //    top = (value.rowIndex * this.rowHeight) + (DetailsCount * DetailsDiv);
            //    Detailstop = (top + this.rowHeight);
            //    DetailsCount++;
            //}
            //else {
            //    top = (value.rowIndex * this.rowHeight) + (DetailsCount * DetailsDiv);
            //}

            //console.log("rowIndex.rowIndex  : " + rowIndex.rowIndex);
            //console.log("value.rowIndex > rowIndex.rowIndex  : " + value.rowIndex > rowIndex.rowIndex );
            //console.log("this.rows.filter(a => a.ShowDetails == true).length > 0 : " + (this.rows.filter(a => a.ShowDetails == true).length > 0));
            //console.log("value.rowIndex : " + value.rowIndex);
            if (value.rowIndex > rowIndex.rowIndex && this.rows.filter(a => a.ShowDetails == true).length > 0) {
                //value.ShowDetails = true;
                top = (value.rowIndex * this.rowHeight) + (this.SpotLightHeight);
                //Detailstop = (top + this.rowHeight);
                //DetailsCount++;
            }
            else if (value.rowIndex == rowIndex.rowIndex) {
                //value.ShowDetails = true; 
                top = (value.rowIndex * this.rowHeight);
                Detailstop = (top + this.rowHeight);
                DetailsCount++;
            }
            else {
                //console.log("2rowIndex.rowIndex :" + rowIndex.rowIndex);
                //console.log("2value.rowIndex :" + value.rowIndex);
                top = (value.rowIndex * this.rowHeight);
            }
            //console.log("top " + top);
            //console.log("Detailstop " + Detailstop);
            value.styles = {
                'top': top + "px",
                'min-width': this.TotalWidth + 22 + 'px',//'min-width': (this.ViewWidth) + 'px',
                'width:': this.TotalWidth + 'px',
                'max-width:': this.TotalWidth + 'px',
            };
            value.Detailsstyles = {
                'top': Detailstop + "px",
                'width:': '100%',
            };
            i++;
        });

        this.canvasHeight = {
            height: this.rowCount * this.rowHeight + (this.SpotLightHeight) + 'px',
            //'width': this.TotalWidth + 'px',
            //'min-width': '100%'//(this.ViewWidth) + 'px'
        };

        if (this.cd) {
            this.cd.detectChanges();
        }
        if (this.ObjectTable) {
            ServiceLocator.SendTotangoUserActivity(this.ObjectTable.Name, "Spotlight View");
        }
    }

    firstRow: number = 0;
    existingFirstRow: number = -1;
    extraRows: number = 10;
    updateDisplayList(reload: boolean = false, LoadFromCache: boolean = false) {
     
            this.rows = [];
        //this.inProgressRows = 0;
        //this.rowsBuffer = [];
        //this.rows = [];
        var oldRow = this.firstRow;
        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        //if (this.existingFirstRow != -1) {
        //    if (firstRow < this.existingFirstRow + this.extraRows) {
        //        return;
        //    }
        //}
        //this.existingFirstRow = this.firstRow;

        //console.log("firstRow", firstRow);
        var oldPage; // = 0;
        var currentPage = Math.max(Math.floor(this.scrollTop / this.rowHeight / this.rowsPerPage), 0);
        var rowsToCreate = Math.min(firstRow + this.rowsPerPage, this.rowsPerPage);
        //var buffer = Math.round(this.scrollTop / 30) > this.rowsPerPage * 2;
        if (this.viewportSize == 0) {
            this.viewportSize = 30;
        }
        if ((firstRow == 0 || oldRow != firstRow) && (currentPage != oldPage) || currentPage == 0) {

            this.bufferFromRow = firstRow;
            if (this.rowCount < this.viewportSize || this.viewportSize == 0) {
                this.ViewPortRowCount = this.rowCount;
            }
            else {
                this.ViewPortRowCount = this.viewportSize; // + this.extraRows;
            }

            var returnCahced = false;
            if (this.rowCount == 0) {
                this.rows = [];
                this.controller.getRow(firstRow, this.dataSource.sortingCol, this.dataSource.sortingDir, false, this.searchFields, returnCahced, this.Filters, reload, false, this.viewportSize, this.SearchFieldChanged);
            }
            var temp = 0;
            if (firstRow > this.viewportSize) {
                temp = firstRow - this.viewportSize;
            }
            for (var i = temp; i < firstRow + (this.ViewPortRowCount); i++) {
                if (i >= this.rowCount) {
                    //console.log("In 2 if ...." + this.rows.length);
                    if (this.controller.cacheBuffer.length > 0) {
                        //var FirstIndex = this.controller.cacheBuffer[0].RowIndex;
                        //var diffCount = this.viewportSize - this.controller.cacheBuffer.length;
                        //var NewIndex = FirstIndex - diffCount; 
                        //for (var j = 0; j < diffCount; j++) {
                        //    if (!this.controller.cachedData[NewIndex]) {
                        //        this.controller.getRow(NewIndex, AppTool.IsNullOrEmpty(this.dataSource.sortingCol) ? "" : this.dataSource.sortingCol, AppTool.IsNullOrEmpty(this.dataSource.sortingDir) ? "" : this.dataSource.sortingDir, false, this.searchFields, returnCahced, this.Filters, reload, true, this.viewportSize);      //.subscribe((res) => { });

                        //    }
                        //    else {
                        //        this.controller.cacheBuffer.push({ RowIndex: NewIndex, RowData: this.controller.cachedData[NewIndex] });
                        //    }
                        //    NewIndex++;
                        //}
                        this.renderRows(this.controller.cacheBuffer);

                        if (this.cd) {
                            this.cd.reattach();
                            this.cd.detectChanges();
                            //console.log("Inside detectChanges " + res.length);
                            //setTimeout(() => this.cd.detectChanges(), 10);
                        }
                        else {
                            this.DetectChangesTimer();
                        }

                        return;
                    }
                }
                //this.controller.requestedRowCount.subscribe((res) => {
                //    //////console.log(res);
                //    this.rowCount = res;
                //    this.dataSource.rowCount = res;
                //});
                //if (i > this.rowCount) {
                //    break;
                //}
                //////console.log(this.controller.getRow(i).subscribe((item) => { this.testArray.push(item) }))
                //////console.log(i);
                //var indexExists = this.rows.filter(s => s.rowIndex == i)[0];
                ////console.log("i", i, "indexExists", indexExists);
                //if (indexExists == undefined) {

                //////console.log("indexExists", indexExists);
                this.inProgressRows++;
                ////console.log("searchFields", this.searchFields);
                if ((i === firstRow + this.ViewPortRowCount - 1) || LoadFromCache == true) {
                    returnCahced = true;
                }
                //console.log("row requested: ", i, returnCahced);
                this.controller.getRow(i, AppTool.IsNullOrEmpty(this.dataSource.sortingCol) ? this.sortingCol : this.dataSource.sortingCol, AppTool.IsNullOrEmpty(this.dataSource.sortingDir) ? this.sortingDir : this.dataSource.sortingDir, false, this.searchFields, returnCahced, this.Filters, reload, false, this.viewportSize, this.SearchFieldChanged);     //.subscribe((res) => { });
                // }

            }




        }

      

    };
    oldScrollTop: number;
    headerStyle: any = { left: '0px' };
    rowStyle: any = {};
    timer = null;
    scrollDirection: string = null;
    //onScroll() {
    //    var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
    //    var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridId);

    //    this.LogGridElement = elem;
    //    if (elem) {
    //        var scroll = elem.scrollTop;
    //        if (scroll > this.scrollPosition) {
    //            this.scrollDirection = "down";
    //            ////console.log("scrollPosition", this.scrollPosition);
    //            ////console.log("scrollDirection", this.scrollDirection)
    //        }
    //        else {
    //            this.scrollDirection = "up";
    //            ////console.log("scrollPosition", this.scrollPosition);
    //            ////console.log("scrollDirection", this.scrollDirection)
    //        }
    //        this.scrollPosition = scroll;
    //        var hscroll = elem.scrollLeft;
    //        //console.log("horizontal scroll", hscroll);
    //        //this.headerStyle = { left: -(hscroll) + 'px' };
    //        //if (this.timer !== null) {
    //        //    clearTimeout(this.timer);
    //        //}
    //        var vscroll = elem.scrollTop;
    //        this.scrollTop = elem.scrollTop;
    //        //console.log(this.scrollTop,"hola i'm the on scroll");

    //        if (vscroll != this.oldScrollTop) {
    //            //////console.log("vscroll entered");
    //            if (columns) {
    //                //var maxTop = columns.parentElement.scrollHeight - columns.offsetHeight;
    //                columns.style.top = elem.scrollTop + "px";//Math.min(elem.scrollTop, maxTop) + "px";
    //            }
    //            this.oldScrollTop = vscroll;
    //            this.updateDisplayList();
    //        }
    //        //this.timer = setTimeout(() => {
    //        //    //////console.log("scrolling...");
    //        //    var vscroll = elem.scrollTop;
    //        //    this.scrollTop = elem.scrollTop;
    //        //    //////console.log("v scroll...", vscroll, "h scroll...", hscroll);
    //        //    //////console.log("scrollHeight...", elem.scrollHeight, "scrollWidth...", elem.scrollWidth);
    //        //    //elem.scrollHeight = this.rowCount * this.rowHeight;
    //        //    //////console.log(elem.scrollHeight);
    //        //    if (vscroll != this.oldScrollTop) {
    //        //        //////console.log("vscroll entered");
    //        //        this.oldScrollTop = vscroll;
    //        //        this.updateDisplayList();
    //        //    }
    //        //    //this.$apply();
    //        //    //this._renderer.renderComponent(this);
    //        //}, 0);
    //    }
    //};
    scrolltimer = null;
    onScroll() {
        this.SearchFieldChanged = false;
        var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);

        this.LogGridElement = elem;
        if (elem) {
            if (this.HScrollPosition == -1 || elem.scrollLeft > this.HScrollPosition) {
                this.HScrollPosition = elem.scrollLeft;
            }
            if (columns) {
                //columns.style.top = elem.scrollTop + "px";
                if (this.RTL == true) {
                    columns.style.right = -1 * (this.HScrollPosition - elem.scrollLeft) + "px";
                }
                else {
                    columns.style.left = -1 * elem.scrollLeft + "px";
                }
            }
            //console.log("Inside Elem " + elem.scrollTop);
        }
        else {
            //console.log("Inside else ");
        }
        if (this.timer) {
            clearTimeout(this.timer);
        }
        this.timer = setTimeout(() => this.DoScroll(), 200);
    };
    HScrollPosition: number = -1;
    HorizantalScrollValue: string = "0px";
    OldHScrollPosition: number = 0;
    DoScroll() {
        var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);

        this.LogGridElement = elem;
        if (elem) {
            if (elem.scrollLeft > 0) {
                this.HorizantalScrollValue = elem.scrollLeft + "px";
            }
            if (this.HScrollPosition == -1 || elem.scrollLeft > this.HScrollPosition) {
                this.HScrollPosition = elem.scrollLeft;
            }
            var scroll = elem.scrollTop;
            if (scroll > this.scrollPosition) {
                this.scrollDirection = "down";
                ////console.log("scrollPosition", this.scrollPosition);
                ////console.log("scrollDirection", this.scrollDirection)
            }
            else {
                this.scrollDirection = "up";
                ////console.log("scrollPosition", this.scrollPosition);
                ////console.log("scrollDirection", this.scrollDirection)
            }
            this.scrollPosition = scroll;
            var hscroll = elem.scrollLeft;
            //console.log("horizontal scroll", hscroll);
            //this.headerStyle = { left: -(hscroll) + 'px' };
            //if (this.timer !== null) {
            //    clearTimeout(this.timer);
            //}
            var vscroll = elem.scrollTop;
            this.scrollTop = elem.scrollTop;
            //console.log(this.scrollTop,"hola i'm the on scroll");

            if (vscroll != this.oldScrollTop) {
                //////console.log("vscroll entered");
                if (columns) {
                    //var maxTop = columns.parentElement.scrollHeight - columns.offsetHeight;
                    //columns.style.top = elem.scrollTop + "px";//Math.min(elem.scrollTop, maxTop) + "px";
                    if (this.RTL == true) {
                        columns.style.right = -1 * (this.HScrollPosition - elem.scrollLeft) + "px";
                    }
                    else {
                        columns.style.left = -1 * elem.scrollLeft + "px";
                    }
                }
                this.oldScrollTop = vscroll;
                if (this.ReloadData == true) {
                    this.controller.ClearCache();
                }
                this.updateDisplayList();
            }
            if (this.timer) {
                clearTimeout(this.timer);
            }
            if (this.AfterServerSort == true) {

                this.timer = setTimeout(() => this.RedrowScrollBar(), 400);
            }

            //this.timer = setTimeout(() => {
            //    //////console.log("scrolling...");
            //    var vscroll = elem.scrollTop;
            //    this.scrollTop = elem.scrollTop;
            //    //////console.log("v scroll...", vscroll, "h scroll...", hscroll);
            //    //////console.log("scrollHeight...", elem.scrollHeight, "scrollWidth...", elem.scrollWidth);
            //    //elem.scrollHeight = this.rowCount * this.rowHeight;
            //    //////console.log(elem.scrollHeight);
            //    if (vscroll != this.oldScrollTop) {
            //        //////console.log("vscroll entered");
            //        this.oldScrollTop = vscroll;
            //        this.updateDisplayList();
            //    }
            //    //this.$apply();
            //    //this._renderer.renderComponent(this);
            //}, 0);
        }
    }

    private RedrowScrollBar() {
        this.AfterServerSort = false;
        var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);

        if (columns) {
            elem.scrollLeft = +(this.HorizantalScrollValue.replace("px", ""));
            columns.style.left = (-1 * (+(this.HorizantalScrollValue.replace("px", "")))) + "px";
        }
    }

    private onWindowResized(event: UIEvent): void {

        var logGrid = document.getElementById(this.LogGridId);
        if (logGrid != null) {
            logGrid.style.display = "none";
            logGrid.style.display = "block";
            this.ViewHeight = logGrid.clientHeight;
            this.ViewWidth = logGrid.clientWidth;
            //console.log("ViewHeight", this.ViewHeight, "ViewWidth", this.ViewWidth);
        }

        var rowsArray = document.getElementsByClassName("ag-row");
        for (var i = 0; i < rowsArray.length; i++) {
            //rowsArray.item(i).style.width = (this.ViewWidth - 20) + 'px';
            itemWidth(rowsArray.item(i));
            // rowsArray.item(i).style.minWidth = (this.ViewWidth) + 'px';
        }
        this.rowStyle.minWidth = this.ViewWidth + 'px';
        this.viewportSize = Math.round(this.ViewHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
        this.rowsPerPage = this.dataSource != null ? this.dataSource.pageSize : 10;
        var header = document.getElementById(this.LogGridColumnsId);
        //header.style.width = this.ViewWidth + 'px';
        //header.style.width = '1920px';
        //header.style.minWidth = this.ViewWidth + 'px';
        // header.style.minWidth = '1920px';
        //this.headerStyle = {
        //    'width': (this.ViewWidth - 20) + 'px',
        //    'min-width': (this.ViewWidth - 20) + 'px'
        //};
        this.headerStyle = {
            'width': this.TotalWidth + 43 + 'px',
            'min-width': '100%'//(this.ViewWidth) + 'px'
        };
        this.canvasHeight = {
            height: this.rowCount * this.rowHeight + 'px',
            //'width': this.TotalWidth + 'px',
            //'min-width': '100%'//(this.ViewWidth) + 'px'
        };

        this.numberOfTotalPages = (this.rowCount / this.rowsPerPage); // + (this.rowCount % this.rowsPerPage);
        //console.log("number of total pages:", this.numberOfTotalPages, "this.rowCount % this.rowsPerPage", this.rowCount % this.rowsPerPage);
        //console.log("this.rowCount / this.rowsPerPage", this.rowCount / this.rowsPerPage);
        //this.updateDisplayList();
        this.onScroll();
    }

    GridBodyResized() {
        //console.log("GridBodyResized()");
    }

    ngOnDestroy() {
        this.cd = null;
        this.FiltersChangedsubscription.unsubscribe();
        if (this.requestedRowsReadySub) {
            this.requestedRowsReadySub.unsubscribe();
        }
        if (this.requestedRowCountSub) {
            this.requestedRowCountSub.unsubscribe();
        }
        if (this.QueryChangeEventSub) {
            this.QueryChangeEventSub.unsubscribe();
        }
        if (this.MouseUpSub) {
            this.MouseUpSub.unsubscribe();
        }
        //if (this.BackFromEditSub) {
        //    this.BackFromEditSub.unsubscribe();
        //}
        if (this.ColumnsReadySub) {
            this.ColumnsReadySub.unsubscribe();
        }
        if (this.ColumnsReady1Sub) {
            this.ColumnsReady1Sub.unsubscribe();
        }
        if (this.MenuHeaderSub) {
            this.MenuHeaderSub.unsubscribe();
        }
        if (this.SearchFieldsSub) {
            this.SearchFieldsSub.unsubscribe();
        }
        if (this.HLineSub) {
            this.HLineSub.unsubscribe();
        }
        if (this.pubSubAdvanceQueryFiltersSub) {
            this.pubSubAdvanceQueryFiltersSub.unsubscribe();
        }
        this.requestedRowsReadySub = null;
        this.FiltersChangedsubscription = null;
        this.requestedRowCountSub = null;
        this.QueryChangeEventSub = null;
        this.MouseUpSub = null;
        //this.BackFromEditSub = null;
        this.ColumnsReadySub = null;
        this.ColumnsReady1Sub = null;
        this.MenuHeaderSub = null;
        this.SearchFieldsSub = null;
        this.HLineSub = null;
        this.pubSubAdvanceQueryFiltersSub = null;
        //console.log(this.Filters != null ? this.Filters.AdditionalFilters.length : "Dest");
    }
    private countIsHere = true;
    private oldSearchFields: string;
    private oldQueryId: string;

    editingCell: any = [];
    focusCell($event, rowIndex, colIndex) {
        //////console.log($event, rowIndex, colIndex);
        //this.editingCell[rowIndex, colIndex] = true;
    }

    public MouseDownX: any;
    public MouseUpX: any;
    public ResizableColumn: any;
    public ColumnWidth: number;
    public startX: any;
    public startY: any;
    public startWidth: any;
    public startHeight: any;
    resizeColumn($event, column) {
        //this.ResizableColumn = document.getElementById(column);
        //this.startX = $event.clientX;
        //this.startY = $event.clientY;
        //this.startWidth = parseInt(document.defaultView.getComputedStyle(this.ResizableColumn).width, 10);
        //this.startHeight = parseInt(document.defaultView.getComputedStyle(this.ResizableColumn).height, 10);
        //document.documentElement.addEventListener('mousemove', doDrag, false);
        //document.documentElement.addEventListener('mouseup', stopDrag, false);
        //function doDrag(e) {
        //    var ResizableColumn = document.getElementById(column);
        //    ResizableColumn.style.width = (this.startWidth + e.clientX - this.startX) + 'px';
        //    ResizableColumn.style.height = (this.startHeight + e.clientY - this.startY) + 'px';
        //}
        //function stopDrag(e) {
        //    document.documentElement.removeEventListener('mousemove', this.doDrag, false); document.documentElement.removeEventListener('mouseup', this.stopDrag, false);
        //}
        //$event.preventDefault();
        ////////console.log($event, column);
        //this.MouseDownX = $event.clientX;
        //this.ResizableColumn = document.getElementById(column);
        //this.ColumnWidth = this.ResizableColumn.clientWidth;
        ////console.log(this.ColumnWidth);
        ////elem.style.width = '250px';
        ////var rows = document.getElementsByClassName(column);
        ////if (rows) {
        ////    for (var i in rows) {
        ////        i.style.width = '250px';
        ////    }
        ////}
        ////console.log($event.clientX, $event.clientY, column);
    }




    resizeColumn2($event, column) {
        $event.preventDefault();
        //////console.log($event, column);
        this.MouseUpX = $event.clientX;
        var diff: number = this.MouseUpX - this.MouseDownX;
        //console.log(diff);
        if (diff < 0) {
            this.ResizableColumn.style.width = this.ColumnWidth - diff;
        }
        else {
            this.ResizableColumn.style.width = (this.ColumnWidth + diff) + 'px';
        }
        //var elem = document.getElementById(column);
        //elem.style.width = '250px';
        //var rows = document.getElementsByClassName(column);
        //if (rows) {
        //    for (var i in rows) {
        //        i.style.width = '250px';
        //    }
        //}
        //console.log($event.clientX, $event.clientY, column);
    }


    divOnFocus(rowIndex, colIndex) {

        var inputID = 'input-row' + rowIndex + 'col' + colIndex;
        //////console.log(inputID);
        //this.editingCell[rowIndex + ' ' + colIndex] = true;
        var elem: HTMLInputElement = <HTMLInputElement>document.getElementById(inputID);
        var span: HTMLSpanElement = document.getElementById('span-row' + rowIndex + 'col' + colIndex);
        if (span && elem) {
            elem.style.display = 'block';
            span.style.display = 'none';
            elem.focus();
        }
        //////console.log(elem);
        //elem.focus();
        //this.displayVisibility[rowIndex + '' + colIndex] = { display: 'none' };
        //this.editVisibility[rowIndex + '' + colIndex] = { display: 'block' };
    }
    blurInput(rowIndex, colIndex) {
        var inputID = 'input-row' + rowIndex + 'col' + colIndex;
        //////console.log(inputID);
        //this.editingCell[rowIndex + ' ' + colIndex] = true;
        var elem: HTMLInputElement = <HTMLInputElement>document.getElementById(inputID);
        var span: HTMLSpanElement = document.getElementById('span-row' + rowIndex + 'col' + colIndex);
        if (span && elem) {
            elem.style.display = 'none';
            span.style.display = 'block';
        }
        //elem.focus();
    }

    displayVisibility: any;
    editVisibility: any;

    inputFocus($event) {
        //////console.log('event', $event);
    }

    GetRowCount(reload: boolean = false) {
        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        if (reload == true) {
            this.controller.ClearCache();
        }
        this.controller.getRow(firstRow, AppTool.IsNullOrEmpty(this.dataSource.sortingCol) ? "" : this.dataSource.sortingCol, AppTool.IsNullOrEmpty(this.dataSource.sortingDir) ? "" : this.dataSource.sortingDir, true, this.searchFields, false, this.Filters, reload, false, this.viewportSize, this.SearchFieldChanged);

    }

    private selectedRow: any;
    get SelectedRow() { return this.selectedRow; }
    set SelectedRow(newValue: any) {
        if (this.selectedRow != newValue) {
            this.selectedRow = newValue;
        }
    }

    public selectedRows: ObservableCollection;
    get SelectedRows() { return this.selectedRows; }
    set SelectedRows(newValue: ObservableCollection) {
        // if (this.selectedRows != newValue) {
        this.selectedRows = newValue;
        newValue.Changed.subscribe((isCollection) => {
            if (this.rows && this.EnableMultiSelection == true) {
                newValue.Collection.forEach((item, key) => {
                    if (this.controller.cachedData[item.rowIndex]) {
                        this.controller.cachedData[item.rowIndex].IsSelected = true;
                    }
                    var Row = this.rows.filter(a => a.rowIndex === item.rowIndex)[0];
                    if (Row) {
                        Row.rowData.IsSelected = true;
                    }
                    //if (newValue == true && !item.rowData.IsChecked) {
                    //    this.OnCheckBoxChecked(item.rowData, newValue, item.rowIndex);
                    //}
                    //if (newValue == false && item.rowData.IsChecked) {
                    //    this.OnCheckBoxChecked(item.rowData, newValue, item.rowIndex);
                    //}
                });
                if (this.cd) {
                    this.cd.detectChanges();
                }
            }
        });

        //}
    }

    private isAllRecordsChecked: boolean;
    get IsAllRecordsChecked() { return this.isAllRecordsChecked; }
    set IsAllRecordsChecked(newValue: boolean) {
        if (this.isAllRecordsChecked != newValue) {
            this.isAllRecordsChecked = newValue;
            if (this.rows) {
                //console.log(this.controller.cachedData.length);
                var keys: string[] = Object.keys(this.controller.cachedData);
                keys.forEach((key) => {
                    this.controller.cachedData[key].IsChecked = newValue;
                });
                this.rows.forEach((item, key) => {
                    //if (this.rows.filter(a => a.rowIndex == item.rowIndex).length > 0) {
                    //    this.controller.cachedData[item.rowIndex].IsChecked = newValue;
                    //}
                    var Row = this.rows.filter(a => a.rowIndex === item.rowIndex)[0];
                    if (Row) {
                        Row.rowData.IsChecked = newValue;
                    }
                    //if (newValue == true && !item.rowData.IsChecked) {
                    //    this.OnCheckBoxChecked(item.rowData, newValue, item.rowIndex);
                    //}
                    //if (newValue == false && item.rowData.IsChecked) {
                    //    this.OnCheckBoxChecked(item.rowData, newValue, item.rowIndex);
                    //}
                });
            }
        }
    }

    private useFilteredRecordsCheckBox: boolean;
    get UseFilteredRecordsCheckBox() { return this.useFilteredRecordsCheckBox; }
    set UseFilteredRecordsCheckBox(newValue: boolean) {
        if (this.useFilteredRecordsCheckBox != newValue) {
            this.useFilteredRecordsCheckBox = newValue;
        }
    }


    //public UseFilteredRecordsCheckBox: boolean;
    //public FilteredRecordsCheckedFieldName: string = "";
    //public FilteredRecordsCheckedFieldValue: string = "";
    MustFilterRowIndexes: any[] = [];
    MustIgnoreRowIndexes: any[] = [];
    OnCheckBoxChecked(rowData, IsChecked, RowIndex, FromOutSide: boolean = false, ById: boolean = false) {
        if (ById == true) {
            var temp = this.controller.cachedData;//
            let result = [];
            for (let item in temp) {
                if (temp[item].Id == rowData.Id) {
                    RowIndex = item;
                    result.push(rowData.Id);
                }
            }

            if (result.length == 0) {
                this.MustFilterRowIndexes.push({ Id: rowData.Id, IsChecked: IsChecked });
            }
        }


        if (this.rows.filter(a => a.rowIndex == RowIndex).length > 0 || this.controller.cachedData[RowIndex] != null) {
            this.controller.cachedData[RowIndex].IsChecked = IsChecked;
            this.controller.cachedData[RowIndex].IsCustomChecked = FromOutSide;
            //this.MustIgnoreRowIndexes.push({ Id: rowData.Id, IsChecked: IsChecked });
        }
       
        var Row = this.rows.filter(a => a.rowIndex === RowIndex)[0];
        if (Row) {
            Row.rowData.IsChecked = IsChecked;
            Row.rowData.IsCustomChecked = FromOutSide;
            //if (this.MustIgnoreRowIndexes.filter(a => a.Id == Row.rowData.Id).length == 0) {
            //    this.MustIgnoreRowIndexes.push({ Id: Row.rowData.Id, IsChecked: IsChecked });
            //}
        }
        //if (Row.rowData.IsChecked == true) {
        //    console.log("OnCheckBoxChecked Display True " + this.rows.filter(a => a.rowIndex === RowIndex)[0].rowData.IsChecked);
        //}
        //this.cd.detectChanges();
        if (FromOutSide == false) {
            this.CheckBoxChecked.emit({ rowData: rowData, IsChecked: IsChecked, rowIndex: RowIndex });
        }
    }
    MyisBackFromEdit: boolean = false;  
  private sortServerProp: any;
  get SortServerProp() { return this.sortServerProp; }
  set SortServerProp(newValue: any) {
    if (newValue && newValue != this.sortServerProp) {

      this.sortServerProp = newValue;
      if (this.sortServerProp && this.sortServerProp.isBackFromEdit == true) {
          this.MyisBackFromEdit = true;
        this.sortServerProp.isBackFromEdit = false;
        newValue.isBackFromEdit = false;
        this.ForceSortServer(newValue.colDef, newValue.id);
      }
    }
  }

  ForceSortServer(colDef, id) {
    //[id]="ColumnId + 'resizable-column-,' + i"
    var indexarr = id.split("resizable-column-,");
    var indexspec = indexarr[1];
    var newColId = this.ColumnId + 'resizable-column-,' + indexspec;
    this.ServerSort(colDef, newColId, true);
  }
}

interface IRow {
    rowIndex: number;
    rowData: any;
    styles?: any;
    Detailsstyles?: any;
    ShowDetails?: boolean;
    DetailsIcon: string;
    IsArchived?: boolean;
}

class SpotLightData {
    RowIndex: number;
    IsSpotLightOpened: boolean;
}
