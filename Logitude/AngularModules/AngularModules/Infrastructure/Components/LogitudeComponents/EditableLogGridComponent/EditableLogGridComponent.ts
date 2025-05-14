declare var System: any;
declare var window: any;
import { Component, OnDestroy, ElementRef, OnInit, AfterViewInit, AfterContentInit, OnChanges, Output, EventEmitter, ContentChildren, ContentChild, ViewChildren, QueryList, ChangeDetectorRef, TemplateRef, DoCheck, IterableDiffers } from '@angular/core';
//import {CORE_DIRECTIVES} from '@angular/common';
import { LogColumnComponent } from './LogColumnComponent';
import { LogRowDetailsComponent } from './LogRowDetailsComponent';
import { LogCellTemplateComponent } from './LogCellTemplateComponent';
import { AppTool } from '../../../../Infrastructure/Tools';
//import {VirtualRowController} from './VirtualRowController'; 
import { EditGridVirtualRowController } from './EditGridVirtualRowController';
//import {ListHeaderTemplateComponent} from './app/ListHeaderTemplateComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { GroupByPipe } from '../../../../Infrastructure/Pipes/GroupByPipe';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
declare var styleDisplay, EditgriditemStyling;

@Component({


    selector: 'logitude-edit-grid',
    templateUrl: './EditableLogGridComponent.html',
    //directives: [CORE_DIRECTIVES, EditableListTemplateComponent],
     inputs: ['columns:columns', 'GridWidth', 'GridHeight', 'groupby', 'ItemSource', 'EnableLines', 'DisableRowByFieldValue', 'DisableRowByFieldName', 'SelectedRow', 'Disabled', 'HeaderHeight', 'ShowCount', 'ReloadDetails', 'ReRenderGrid', 'IsReadOnly', 'IsDarkHeader', 'EnableMultiSelection', 'SelectedRows', 'FooterTop', 'UseVirtuallization', 'EnableGridViewRowBackground', 'ChangeScrollPosition', 'EnableExpandCollapseAll', 'LastDefaultSpace','ColumnName','Condition', 'ShowNoDataFoundText'],

})

export class EditableLogGridComponent implements OnInit, AfterViewInit, AfterContentInit {
    private itemSource: ObservableCollection;
    FooterTop: number = 0;
    HeaderHeight: number = 27;
    BodyTop: number = 27;
    EnableGridViewRowBackground = false;
    HorizantalScrollStatus: string = 'auto';
    differ: any;
    ShowFooter: boolean = false;
    LastDefaultSpace: number = 22;
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);//true;
    UseVirtuallization: boolean = false;
    ItemSourceLoaded: EventEmitter<any>;
    MyTimer = null
    EnableExpandCollapseAll = false;
    ShowNoDataFoundText = false;
    ShowCollapseAllIcon = false;
    Condition:string=""
    ColumnName:string=""
    public get ItemSource() { return this.itemSource; }
    public set ItemSource(newValue: ObservableCollection) {
        this.itemSource = newValue;
        newValue.Changed.subscribe((evt) => {
            this.CurrentSession.LogitudeGridHelper.ResetRowIndex(this.LogGridId);
            this.CurrentSession.LogitudeGridHelper.ResetNextRowIndex(this.LogGridId);
            var OldCount = this.group.length;
            this.sortingCol = null;

            this.resetHeaderCellBackGround();
            if (this.groupby) {
                this.group = new GroupByPipe().ShapeGrouping(this.ItemSource.Collection, this.groupby);
                this.rowCount = this.group.length;
                this.updateDisplayListGrouping(true);
            }
            else {
                if (evt.Items && this.ItemSource.Collection.length != evt.Items.length) {
                    this.ItemSource.Collection = evt.Items;
                }

                this.group = new GroupByPipe().ShapeList(this.ItemSource.Collection);

                this.rowCount = this.group.length;
                this.updateDisplayList(true);
            }
            this.canvasHeight = {
                //'top': this.HeaderHeight + 'px',
                height: (this.rowCount * this.rowHeight) + 'px',
                'width': '100%',//this.TotalWidth + 'px',
                'min-width': this.TotalWidth + 'px'////(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            };
            this.BodyTop = this.HeaderHeight;
            var isDestroyed: boolean = this.cd['destroyed'];
            if (!isDestroyed) {
                this.cd.detectChanges();
            }
            if (evt.IsCollection == false && !evt.RowIndex) {
                var RowsElement = document.getElementById(this.LogGridRowsId);
                if (RowsElement && !evt.PreventScroll) {
                    RowsElement.scrollTop = RowsElement.scrollHeight;
                }
                if (this.group.length != OldCount && this.ItemSource.Length > 0 && evt.FocusFirstCell == true) {
                    this.MyTimer = setTimeout(() => this.NewElementInsertedEvent(evt.RowIndex), 400);
                }

            }
            else if (evt.RowIndex) {
                var RowsElement = document.getElementById(this.LogGridRowsId);
                if (RowsElement && !evt.PreventScroll) {
                    RowsElement.scrollTop = (evt.RowIndex * this.rowHeight) - this.rowHeight;
                }
                if (this.group.length != OldCount && this.ItemSource.Length > 0 && evt.FocusFirstCell == true) {
                    this.MyTimer = setTimeout(() => this.NewElementInsertedEvent(evt.RowIndex), 400);
                }
            }
            else if (!evt.PreventScroll) {
                var RowsElement = document.getElementById(this.LogGridRowsId);
                if (RowsElement) {
                    RowsElement.scrollTop = 0;
                }

            }
            this.SetSelectedRows();
        });
        if (this.groupby) {
            this.group = new GroupByPipe().ShapeGrouping(this.ItemSource.Collection, this.groupby);
            this.rowCount = this.group.length;
            this.updateDisplayListGrouping(true);
        }
        else {
            this.group = new GroupByPipe().ShapeList(this.ItemSource.Collection);
            this.rowCount = this.group.length;
            //this.updateDisplayListGrouping(true);
            this.updateDisplayList(true);
        }

    }

    NewElementInsertedEvent(rowIndex: any = null) {
        if (this.MyTimer) {
            clearTimeout(this.MyTimer);
        }
        this.CurrentSession.ObsNewElementInsertedEvent.emit({ length: this.group.length, Id: this.LogGridId, RowIndex: rowIndex });
    }

    OnRowMouseOut(rowIndex, BackGround, IsSelected) {
        //"linear-gradient(0deg, rgba(133, 203, 237, 1) 0%, rgba(232, 247, 255, 1) 100%)"
        var Color = "transparent";
        var elem = document.getElementById(this.LogGridId + 'row' + rowIndex);
        if (elem) {
            if (!AppTool.IsNullOrEmpty(BackGround)) {
                Color = BackGround;
            }
            if (IsSelected) {
                Color = "linear-gradient(0deg, rgba(133, 203, 237, 1) 0%, rgba(232, 247, 255, 1) 100%)";
            }

            elem.style.background = Color;

        }
    }

    OnRowMouseOver(rowIndex, BackGround, IsSelected) {
        var temp = "linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%)";
        var Selected = "linear-gradient(0deg, rgba(133, 203, 237, 1) 0%, rgba(232, 247, 255, 1) 100%)"
        var elem = document.getElementById(this.LogGridId + 'row' + rowIndex);
        if (elem) {
            if (IsSelected) {
                elem.style.background = Selected;
            }
            else {
                elem.style.background = temp;
            }
        }
    }

    private isReadOnly: boolean;
    public get IsReadOnly() { return this.isReadOnly; }
    public set IsReadOnly(newValue: boolean) {
        this.isReadOnly = newValue;
        if (this.cols) {
            this.cols.toArray().forEach((value, key) => {
                value.IsReadOnlyGrid = newValue;
            });
        }
        //this.init();
    }
    IsDarkHeader: boolean = false;
    IsExpandableDataByFieldName: string;
    func: Function;
    RowClass: string;
    DisableRowByFieldValue: any;
    DisableRowByFieldName: any;
    EnableLines: boolean;
    Disabled: boolean = false;
    public customcolumns: any[];
    public columns: any[];
    GridWidth: any;
    GridHeight: any;
    rows: IRow[];
    //rows123: IRow[];
    rowHeight: number = 27;
    ShowCount: boolean = false;
    height: number = 800;
    scrollTop: number = 0;
    rowsPerPage: number;
    bufferFromRow: number;
    bufferNumberOfRows: number;
    pixelsPerPage: number = this.rowsPerPage * this.rowHeight;
    numberOfTotalPages: number;
    numberOfRowsToDraw: number = 0;
    canvasHeight: any;
    viewportSize: number;
    cachedPages: any[] = [];
    pageIndex: number = 0;
    lastIndex: number = 0;
    public dataSource: any;
    //public ItemSource: any[];
    public SourceItems: any[];
    private rowModel: any;
    inProgressRows: number = 0;
    tripleViewport: number;
    public currentrow: number;
    public currentrowid: string;




    @Output() MouseOverChanged = new EventEmitter();
    @Output() MouseleaveChanged = new EventEmitter();
    @Output() SelectedItemChanged = new EventEmitter();
    @Output() RowDoubleClick = new EventEmitter();
    @Output() Ondblclick = new EventEmitter();
    @Output() RowEnded = new EventEmitter();
    @Output() RowDataLoaded = new EventEmitter();
    @Output() onparentclickEvent = new EventEmitter();
    ReloadDetails: EventEmitter<any>;
    ReRenderGrid: EventEmitter<any>;
    ChangeScrollPosition: EventEmitter<any>;
    public ViewHeight: number;
    public ViewWidth: number;
    public ViewPortRowCount: number;
    public clickargs: any;
    public noComponent: boolean;
    public id: string;
    public groupby: any;
    public InClick: boolean;
    public DontEnter: boolean;
    private groupeddata: any[];
    public allowtomove: boolean;
    public rowCount: number;
    public GridStyle: any;
    public GridHeaderStyle: any;
    controller: EditGridVirtualRowController;
    scrollPosition: number = null;
    LogGridElement: HTMLDivElement;
    public LogGridId: string = null;
    public LogGridRowsId: string = null;
    public LogGridColumnsId: string = null;
    public show: boolean = true;
    public Vir: boolean = false;
    public DetailButtonVisibile: boolean = false;
    public ColumnId: string = null;
    AllowDisableCells: boolean = false;
    DetailsDivId: string = null;
    WindowResizeSub: any;
    EndOfRowReachedSub: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _elementRef: ElementRef, private cd: ChangeDetectorRef, differs: IterableDiffers) {
        this.differ = differs.find([]).create(null);
        if (this.CurrentSession == null) {
            this.LogGridId = "LogGrid_-1_-1";
            this.LogGridRowsId = "LogGridRows_-1_-1";
            this.LogGridColumnsId = "LogGridColumns_-1_-1";
            this.ColumnId = "ColumnId_-1_-1";
            this.DetailsDivId = "DetailsDivId_-1_-1";
        }

        else {
            this.LogGridId = "edit-log-grid_" + this.CurrentSession.LogitudeGridHelper.GetEditableLogGridIndexId();
            this.LogGridRowsId = "edit-log-gridRows_" + this.CurrentSession.LogitudeGridHelper.GetEditableLogGridRowsIndexId();
            this.LogGridColumnsId = "edit-log-gridColumns_" + this.CurrentSession.LogitudeGridHelper.GetEditableLogGridColumnsIndexId();
            this.ColumnId = "ColumnId_" + this.CurrentSession.LogitudeGridHelper.GetLogGridColumnsIndexId();
            this.DetailsDivId = "DetailsDivId_" + this.CurrentSession.LogitudeGridHelper.GetDetailsDivId();
        }
        this.controller = new EditGridVirtualRowController();
        //window.onresize = this.onWindowResized.bind(this);
        this.WindowResizeSub = this.CurrentSession.WindowResizeEvent.subscribe((res) => {
            this.onWindowResized(res);
        });
        //document.onmouseup = (e) => {
        //    if (this.isResizing || this.isDraging) {
        //        this.isResizing = false;
        //        this.isDraging = false;
        //        var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
        //        left.classList.remove("ag-header-cell-moving");
        //        var d = document.getElementById(this.ColumnId + 'Mask');
        //        //d.style.width = "0px";
        //        d.innerText = "";
        //        d.style.display = "none"; 
        //    }
        //};


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
        this.lastDownX = ($event.clientX - this.GridLeft);
        //var tr = $event.currentTarget.arentElement.id;
        this.ColIndex = +($event.currentTarget.parentElement.id.split(',')[1]);//+(arg.split(',')[1]);
        var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
        this.OldWidth = left.clientWidth;
    }
    //OnMyMouseDown($event, arg) {
    //    this.isResizing = true;
    //    var grid = document.getElementById(this.LogGridId);
    //    var rec = grid.getBoundingClientRect();
    //    this.GridLeft = rec.left;
    //    this.lastDownX = ($event.clientX - this.GridLeft);
    //    //var tr = $event.currentTarget.arentElement.id;
    //    this.ColIndex = +($event.currentTarget.parentElement.id.split(',')[1]);//+(arg.split(',')[1]);
    //    var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
    //    this.OldWidth = left.clientWidth;
    //}
    isDraging: boolean = false;
    GridLeft: number = 0;
    ShadowTitle: any;
    OnDragMouseDown($event, arg) {
        if (!this.isResizing) {
            this.isDraging = true;
            var grid = document.getElementById(this.LogGridId);
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
    OnMyMouseUp(e) {
        if (this.isResizing || this.isDraging) {
            if (this.isResizing) {
                this.TotalWidth = this.FinalWidthNew;
                this.headerStyle = {
                    'height': this.HeaderHeight + 'px',
                    'width': '100%',
                    'min-width': this.TotalWidth + 'px',//(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                };
                this.canvasHeight = {
                    //'top': this.HeaderHeight + 'px',
                    height: (this.rowCount * this.rowHeight) + this.StaticDetailsHeight + 'px',
                    'width': '100%',//this.TotalWidth + 'px',
                    'min-width': this.TotalWidth + 'px'////(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                };
                this.BodyTop = this.HeaderHeight;
            }
            this.isResizing = false;
            this.isDraging = false;
            var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
            left.classList.remove("ag-header-cell-moving");
            var d = document.getElementById(this.ColumnId + 'Mask');
            //d.style.width = "0px";
            d.innerText = "";
            styleDisplay(d);
            //if (this.timerToken) {
            //    clearTimeout(this.timerToken);
            //}
            //this.timerToken = setTimeout(() => this.FireColumnReorderComplete(), 400);

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
    OldWidth: number = 0;
    NewWidthFinal: number = 0;
    //leftPadd : number = 0;
    FinalWidthNew: number = 0;
    OnMyMouseMove(e) {
        if (this.isResizing) {
            var NewX = (e.clientX - this.GridLeft);
            var lastDelta = NewX - this.lastDownX;
            if (this.RTL) {
                lastDelta = lastDelta * -1;
            }
            var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
            var newWidth = this.OldWidth + lastDelta;
            var WidthChanged = this.OldWidth != newWidth;
            if (WidthChanged) {
                this.NewWidthFinal = newWidth;
                left.style.width = newWidth.toString() + "px";
                if (this.RTL == true) {
                    this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Style = {
                        width: left.style.width,
                        right: left.style.right,
                        'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                        'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                        'overflow': 'hidden',
                        'white-space': 'nowrap',
                        'text-overflow': 'ellipsis',
                        'min-width': '25px'
                    };
                }
                else {
                    this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Style = {
                        width: left.style.width,
                        left: left.style.left,
                        'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                        'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                        'overflow': 'hidden',
                        'white-space': 'nowrap',
                        'text-overflow': 'ellipsis',
                        'min-width': '25px'
                    };
                }

                var ColsArr = this.customcolumns;
                //var leftPadd = 0; 
                for (var i = this.ColIndex + 1; i < ColsArr.length; i++) {
                    //var tempo = document.getElementById(this.ColumnId + "resizable-column-," + 0);
                    var leftPadd = 0;
                    //if (this.RTL == true) {
                    //    leftPadd = (+tempo.style.right.replace("px", "")) + 2;//20;
                    //}
                    //else {
                    //    leftPadd = (+tempo.style.left.replace("px", "")) + 2;//20;
                    //}
                    //for (var j = 0; j < i; j++) {
                    //    var col = document.getElementById(this.ColumnId + "resizable-column-," + j);
                    //    leftPadd += col.clientWidth;
                    //}
                    //var col = document.getElementById(this.ColumnId + "resizable-column-," + i);
                    var prevcol = document.getElementById(this.ColumnId + "resizable-column-," + (i - 1));
                    var col = document.getElementById(this.ColumnId + "resizable-column-," + i);

                    if (this.RTL == true) {
                        col.style.right = ((+prevcol.style.right.replace("px", "")) + ((+prevcol.style.width.replace("px", "")) < 25 ? 25 : (+prevcol.style.width.replace("px", "")))) + "px";//leftPadd + "px";
                        this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Style = {
                            width: col.style.width,
                            right: col.style.right,
                            'background-color': this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                            'text-align': this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Alignment : 'left',
                            'display': this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                            'overflow': 'hidden',
                            'white-space': 'nowrap',
                            'text-overflow': 'ellipsis',
                            'min-width': '25px'
                        };
                    }
                    else {
                        col.style.left = ((+prevcol.style.left.replace("px", "")) + ((+prevcol.style.width.replace("px", "")) < 25 ? 25 : (+prevcol.style.width.replace("px", "")))) + "px";//leftPadd + "px";
                        this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Style = {
                            width: col.style.width,
                            left: col.style.left,
                            'background-color': this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                            'text-align': this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].Alignment : 'left',
                            'display': this.customcolumns.filter(a => a.ColId == col.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                            'overflow': 'hidden',
                            'white-space': 'nowrap',
                            'text-overflow': 'ellipsis',
                            'min-width': '25px'
                        };
                    }

                    //this.cd.detectChanges();
                }
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
                        var temp = (+right.style.right.replace("px", ""));

                        if ((this.GridLeft - e.clientX) <= (+left.style.right.replace("px", ""))) {
                            right.style.right = temp + left.clientWidth + "px";
                            this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Style = {
                                width: right.style.width,
                                right: right.style.right,
                                'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            left.style.right = temp + "px";
                            this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Style = {
                                width: left.style.width,
                                right: left.style.right,
                                'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            left.id = this.ColumnId + "resizable-column-," + (this.ColIndex - 1);
                            right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
                            this.ColIndex = this.ColIndex - 1;
                            this.lastDownX = (e.clientX - this.GridLeft);
                        }
                    }
                }
                else {
                    if ((this.ColIndex + 1) < this.customcolumns.length) {
                        var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex + 1));
                        var temp = (+left.style.left.replace("px", ""));
                        if ((e.clientX - this.GridLeft) >= (+right.style.left.replace("px", ""))) {

                            left.style.left = temp + right.clientWidth + "px";
                            this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Style = {
                                width: left.style.width,
                                left: left.style.left,
                                'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            right.style.left = temp + "px";
                            this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Style = {
                                width: right.style.width,
                                left: right.style.left,
                                'background-color': this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
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
                    if ((this.ColIndex + 1) < this.customcolumns.length) {
                        var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex + 1));
                        var temp = (+left.style.right.replace("px", ""));
                        if ((this.GridLeft - e.clientX) >= (+right.style.right.replace("px", ""))) {

                            left.style.right = temp + right.clientWidth + "px";
                            this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Style = {
                                width: left.style.width,
                                right: left.style.right,
                                'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            right.style.right = temp + "px";
                            this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Style = {
                                width: right.style.width,
                                right: right.style.right,
                                'background-color': this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
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
                        var temp = (+right.style.left.replace("px", ""));

                        if ((e.clientX - this.GridLeft) <= (+left.style.left.replace("px", ""))) {
                            right.style.left = temp + left.clientWidth + "px";
                            this.customcolumns.filter(a => a.ColId == right.attributes['colid'].value)[0].Style = {
                                width: right.style.width,
                                left: right.style.left,
                                'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            left.style.left = temp + "px";
                            this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Style = {
                                width: left.style.width,
                                left: left.style.left,
                                'background-color': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment ? this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].Alignment : 'left',
                                'display': this.customcolumns.filter(a => a.ColId == left.attributes['colid'].value)[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
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
    //OnMyMouseMove(e) {
    //    if (this.isResizing) {
    //        var NewX = (e.clientX - this.GridLeft);
    //        var lastDelta = NewX - this.lastDownX;
    //        var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
    //        var newWidth = this.OldWidth + lastDelta;
    //        var WidthChanged = this.OldWidth != newWidth;
    //        if (WidthChanged) {
    //            this.NewWidthFinal = newWidth;
    //            left.style.width = newWidth.toString() + "px";
    //            this.customcolumns.filter(a => a.binding == left.attributes['colid'].value)[0].Style = { width: left.style.width, left: left.style.left };
    //            var ColsArr = this.customcolumns;
    //            //var leftPadd = 0; 
    //            for (var i = this.ColIndex + 1; i < ColsArr.length; i++) {
    //                var leftPadd = 10;
    //                if (this.groupby) {
    //                    leftPadd = 15;
    //                }
    //                if (this.RowDetail) {
    //                    leftPadd = 25;
    //                }
    //                if (this.RowDetail && this.groupby) {
    //                    leftPadd = 40;
    //                }
    //                for (var j = 0; j < i; j++) {
    //                    var col = document.getElementById(this.ColumnId + "resizable-column-," + j);
    //                    leftPadd += col.clientWidth;
    //                }
    //                var col = document.getElementById(this.ColumnId + "resizable-column-," + i);
    //                col.style.left = leftPadd + "px";
    //                this.customcolumns.filter(a => a.binding == col.attributes['colid'].value)[0].Style = { width: col.style.width, left: col.style.left };
    //                //this.cd.detectChanges();
    //            }
    //        }
    //    }
    //    else if (this.isDraging) {
    //        var Direction = this.workOutDirection(e);
    //        if (Direction == null) {
    //            return;
    //        }
    //        if (Direction == "Right") {
    //            var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
    //            this.placeDiv((e.clientX - this.GridLeft), e.clientY, left.clientWidth);
    //            if ((this.ColIndex + 1) < this.customcolumns.length) {
    //                var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex + 1));
    //                var temp = (+left.style.left.replace("px", ""));
    //                if ((e.clientX - this.GridLeft) >= (+right.style.left.replace("px", ""))) {

    //                    left.style.left = temp + right.clientWidth + "px";
    //                    this.customcolumns.filter(a => a.binding == left.attributes['colid'].value)[0].Style = { width: left.style.width, left: left.style.left };
    //                    right.style.left = temp + "px";
    //                    this.customcolumns.filter(a => a.binding == right.attributes['colid'].value)[0].Style = { width: right.style.width, left: right.style.left };
    //                    left.id = this.ColumnId + "resizable-column-," + (this.ColIndex + 1);
    //                    right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
    //                    this.ColIndex = this.ColIndex + 1;
    //                    this.lastDownX = (e.clientX - this.GridLeft);
    //                }
    //            }
    //        }
    //        else {
    //            var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
    //            this.placeDiv((e.clientX - this.GridLeft), e.clientY, left.clientWidth);
    //            if ((this.ColIndex - 1) >= 0) {
    //                var right = document.getElementById(this.ColumnId + "resizable-column-," + (this.ColIndex - 1));
    //                var temp = (+right.style.left.replace("px", ""));

    //                if ((e.clientX - this.GridLeft) <= (+left.style.left.replace("px", ""))) {
    //                    right.style.left = temp + left.clientWidth + "px";
    //                    this.customcolumns.filter(a => a.binding == right.attributes['colid'].value)[0].Style = { width: right.style.width, left: right.style.left };
    //                    left.style.left = temp + "px";
    //                    this.customcolumns.filter(a => a.binding == left.attributes['colid'].value)[0].Style = { width: left.style.width, left: left.style.left };
    //                    left.id = this.ColumnId + "resizable-column-," + (this.ColIndex - 1);
    //                    right.id = this.ColumnId + "resizable-column-," + this.ColIndex;
    //                    this.ColIndex = this.ColIndex - 1;
    //                    this.lastDownX = (e.clientX - this.GridLeft);
    //                }
    //            }
    //        }
    //    }
    //}
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
    StaticDetailsHeight: number = 0;
    ReloadDetailsSub: any;
    ReRenderGridSub: any;
    FinishLoadingSub: any;
    FinishLoadingGroupsSub: any;
    ngOnInit() {

        if (this.ReloadDetails) {
            this.ReloadDetailsSub = this.ReloadDetails.subscribe((res) => {
                this.cd.detectChanges();
                var grouptop = 0;
                var top = 0;
                var Bodytop = this.HeaderHeight;
                if (this.groupby) {
                    this.rows.forEach((value, key) => {

                        if (value.IsGroupOpened) {
                            value.styles = {
                                'top': top + "px",
                                'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                                'width:': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px',
                                'max-width:': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px',
                            };
                            value.Detailsstyles = {
                                'top': Bodytop + "px",
                                'min-width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px',
                                'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                            };
                            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                            var DetailsDivHeight = 0;
                            var OpenedItemsHeight = 0;
                            if (DetailsDiv) {
                                DetailsDivHeight = DetailsDiv.clientHeight;
                                OpenedItemsHeight += DetailsDivHeight;
                            }

                            top += (this.rowHeight) + DetailsDivHeight;


                            Bodytop = top + this.HeaderHeight;//27;
                        }
                    });
                }
                else {
                    var DetailsHeight = 0;
                    this.StaticDetailsHeight = 0;
                    this.rowsBuffer.forEach((value, key) => {

                        value.styles = {
                            'top': top + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                        };
                        value.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                        };
                        var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                        var DetailsDivHeight = 0;
                        var OpenedItemsHeight = 0;
                        if (DetailsDiv) {
                            DetailsDivHeight = DetailsDiv.clientHeight;
                            OpenedItemsHeight += DetailsDivHeight;
                        }

                        top += (this.rowHeight) + DetailsDivHeight;
                        DetailsHeight += DetailsDivHeight;
                        this.StaticDetailsHeight += DetailsDivHeight;

                        Bodytop = top + this.HeaderHeight;//27;
                    });
                    this.canvasHeight = {
                        //'top': this.HeaderHeight + 'px',
                        height: (this.rowCount * this.rowHeight) + DetailsHeight + 'px',
                        'width': '100%',//this.TotalWidth + 'px',
                        'min-width': this.TotalWidth + 'px'//(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                    };
                    this.BodyTop = this.HeaderHeight;
                    this.updateDisplayList();
                }
            });
        }
        if (this.ReRenderGrid) {
            this.ReRenderGridSub = this.ReRenderGrid.subscribe((res) => {
                this.cd.detectChanges();
                var index = 0;
                var left = 0;
                if (this.groupby) {
                    left = 15;
                }
                if (this.RowDetail) {
                    left = 25;
                }
                if (this.RowDetail && this.groupby) {
                    left = 40;
                }
                this.TotalWidth = 0;
                this.customcolumns = [];
                this.cols.toArray().forEach((value, key) => {
                    
                    if (value.hastemplate) {
                        value.index = index;
                        index++;
                    }
                    if (value.Editable == undefined) {
                        value.Editable = true;
                    }
                    value.Style = {
                        width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'overflow': 'hidden',
                        'white-space': 'nowrap',
                        'text-overflow': 'ellipsis',
                        'min-width': '25px'
                    };
                    value.HeaderStyle = {
                        'height': this.HeaderHeight - 1 + 'px',
                        'line-height': this.HeaderHeight <= 27 ? '26px' : 'normal',
                        width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                    };
                    if (value.visibility != 'hidden') {
                        this.TotalWidth += + (value.width);
                        value.width = value.width + 'px';
                        left += (+(value.width.replace("px", "")));
                    }
                    if (value.AllowDisableCells) {
                        this.AllowDisableCells = true;
                    }
                    this.customcolumns.push(value);
                });
            });
        }
        if (this.ChangeScrollPosition) {
            this.ChangeScrollPosition.subscribe((res) => {
                var RowsElement = document.getElementById(this.LogGridRowsId);
                if (RowsElement) {
                    RowsElement.scrollTop = this.rowHeight * res.RowIndex;
                }
            });
        }
        this.EndOfRowReachedSub = this.CurrentSession.EndOfRowReachedEvent.subscribe((res) => {
            if (this.LogGridId == this.CurrentSession.CurrentLogGrid) {
                this.RowEnded.emit(res);
            }
        });
        this.CurrentSession.LogitudeGridHelper.SetColumnsCount(true, this.LogGridId);
        this.CurrentSession.LogitudeGridHelper.ResetEditCellIndex();
        this.CurrentSession.LogitudeGridHelper.ResetRowIndex(this.LogGridId);
        this.CurrentSession.LogitudeGridHelper.ResetNextRowIndex(this.LogGridId);
        if (this.dataSource) {
            this.controller.setDataSource(this.dataSource);
        }
        var xx = this.ItemSource;
        var xxx = this.dataSource;
        this.GridStyle = { width: this.GridWidth ? this.GridWidth : '100%', height: this.GridHeight ? this.GridHeight : '100%' };
        this.DontEnter = false;
        this.InClick = false;
        this.customcolumns = [];
        this.rows = [];
        this.cd.detectChanges();
        this.noComponent = true;
        var i = 0;
        this.FinishLoadingSub = this.FinishLoading.subscribe((res) => {
            var grouptop = 0;
            var top = 0;
            var Bodytop = this.HeaderHeight;
            if (this.groupby) {
                this.rows.forEach((value, key) => {
                    if (value.IsGroupOpened) {
                        value.styles = {
                            'top': top + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px',
                            'max-width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth - 18 : this.ViewWidth - 18) + 'px',
                        };
                        value.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': this.TotalWidth + 'px',
                            'width': '100%'
                        };
                        var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                        var DetailsDivHeight = 0;
                        var OpenedItemsHeight = 0;
                        if (DetailsDiv) {
                            DetailsDivHeight = DetailsDiv.clientHeight;
                            OpenedItemsHeight += DetailsDivHeight;
                        }

                        top += (this.rowHeight) + DetailsDivHeight;


                        Bodytop = top + this.HeaderHeight;
                    }
                });
            }
            else {
                var DetailsHeight = 0;
                this.StaticDetailsHeight = 0;
                this.rows.forEach((value, key) => {

                    value.styles = {
                        'top': top + "px",
                        'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                    };
                    value.Detailsstyles = {
                        'top': Bodytop + "px",
                        'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                    };
                    var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                    var DetailsDivHeight = 0;
                    var OpenedItemsHeight = 0;
                    if (DetailsDiv) {
                        DetailsDivHeight = DetailsDiv.clientHeight;
                        OpenedItemsHeight += DetailsDivHeight;
                    }

                    top += (this.rowHeight) + DetailsDivHeight;
                    DetailsHeight += DetailsDivHeight;
                    this.StaticDetailsHeight += DetailsDivHeight;

                    Bodytop = top + this.HeaderHeight;//27;

                });
                this.canvasHeight = {
                    height: (this.rowCount * this.rowHeight) + DetailsHeight + 'px',
                    'width': '100%',
                    'min-width': this.TotalWidth + 'px'
                };
                this.BodyTop = this.HeaderHeight;
            }
        });

        this.FinishLoadingGroupsSub = this.FinishLoadingGroups.subscribe((res) => {
            var grouptop = 0;
            var top = 0;
            var height = 0;
            var Bodytop = this.HeaderHeight;
            this.rows.forEach((value, key) => {
                if (value.IsGroupOpened) {
                    value.styles = {
                        'top': top + "px",
                        'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                    };
                    value.Detailsstyles = {
                        'top': Bodytop + "px",
                        'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                    };
                    var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                    var DetailsDivHeight = 0;
                    var OpenedItemsHeight = 0;
                    if (DetailsDiv) {
                        DetailsDivHeight = DetailsDiv.clientHeight;
                        OpenedItemsHeight += DetailsDivHeight;
                    }

                    top += (this.rowHeight) + DetailsDivHeight;
                    Bodytop = top + this.HeaderHeight;//27;
                    height = height + this.rowHeight;
                }
            });
            this.canvasHeight = {
                height: this.rowCount * this.rowHeight + 'px',
                'width': '100%',
                'min-width': this.TotalWidth + 'px'
            };
            this.BodyTop = this.HeaderHeight;
        });

    }
    SelectedIndex: number = -1;
    FinishLoading: EventEmitter<any> = new EventEmitter<any>();
    FinishLoadingGroups: EventEmitter<any> = new EventEmitter<any>();
    OnPlusClick(row: IRow) {
        this.rowsBuffer.forEach((value, key) => {
            if (value == row) {
                value.IsDetailesOpened = !value.IsDetailesOpened;
                if (!value.IsDetailesOpened) {
                    this.ShowCollapseAllIcon = false;
                    value.DetailsIcon = "./Images/CustomTreeIcon.png";
                }
                else {
                    this.ShowCollapseAllIcon = true;
                    value.DetailsIcon = "./Images/CustomtreeIcon2.png";
                }
            }
        });
        this.cd.detectChanges();
        if (this.UseVirtuallization == false) {
            this.FinishLoading.emit(row);
        }
        else {
            this.updateDisplayList();
        }

    }

    OnGroupClick(row: IRow) {
        var KeepGoing = true;
        this.rows.forEach((value, key) => {

            if (value.rowIndex >= row.ChildrenFirstIndex) {
                if (value.Type != "Head") {
                    value.IsDetailesOpened = false;
                }
                if (!value.IsDetailesOpened) {
                    value.DetailsIcon = "./Images/CustomTreeIcon.png";
                }
                else {
                    value.DetailsIcon = "./Images/CustomtreeIcon2.png";
                }
                if (value.Type == "Head") {
                    KeepGoing = false;
                    if (!value.IsDetailesOpened) {
                        value.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                    }
                    else {
                        value.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                    }
                }
                if (KeepGoing) {
                    value.IsGroupOpened = !value.IsGroupOpened;
                }
            }
            else if (value == row) {
                value.IsDetailesOpened = !value.IsDetailesOpened;
                if (!value.IsDetailesOpened) {
                    value.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                }
                else {
                    value.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                }
            }
        });
        this.cd.detectChanges();
        this.FinishLoadingGroups.emit(row);
    }

    OnCollapseAllClick() {
        this.ShowCollapseAllIcon = false;
        this.rowsBuffer.forEach((value, key) => {
            if (value.IsExpandable) {
                value.IsDetailesOpened = false;
                value.DetailsIcon = "./Images/CustomTreeIcon.png";
                this.cd.detectChanges();
                if (this.UseVirtuallization == false) {
                    this.FinishLoading.emit(value);
                }
                else {
                    this.updateDisplayList();
                }
            }
        });


    }

    OnExpandAllClick() {
        this.ShowCollapseAllIcon = true;
        this.rowsBuffer.forEach((value, key) => {
            if (value.IsExpandable) {
                value.IsDetailesOpened = true;
                value.DetailsIcon = "./Images/CustomtreeIcon2.png";
                this.cd.detectChanges();
                if (this.UseVirtuallization == false) {
                    this.FinishLoading.emit(value);
                }
                else {
                    this.updateDisplayList();
                }
            }
        });

    }

    init() {
        this.RowClass = "Row ag-row";
        this.rowsBuffer = [];
        if (this.ViewHeight == null || this.ViewWidth == null || (this.ViewHeight <= 0 && this.ViewWidth <= 0)) {
            var RowsElement = document.getElementById(this.LogGridRowsId);
            this.ViewHeight = RowsElement.clientHeight;
            this.ViewWidth = RowsElement.clientWidth;
        }
        this.headerStyle = {
            'height': this.HeaderHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px',
        };
        this.rowStyle.minWidth = this.ViewWidth + 'px';
        this.viewportSize = Math.round(this.ViewHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
        this.rowsPerPage = 10;
        this.rows = [];
        this.cd.detectChanges();
        if (this.groupby) {
            this.group = new GroupByPipe().ShapeGrouping(this.ItemSource.Collection, this.groupby);
            this.rowCount = this.group.length;
        }
        else {
            this.rowCount = this.ItemSource.Length;
        }
        this.canvasHeight = {
            height: this.rowCount * this.rowHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px'
        };
        this.BodyTop = this.HeaderHeight;
        this.height = this.rowCount * this.rowHeight;
        this.numberOfTotalPages = this.rowCount / this.rowsPerPage;
        if (this.groupby) {
            this.updateDisplayListGrouping();
        }
        else {
            this.updateDisplayList();
        }
        this.SetSelectedRows();
        var i = 0;
    };
    mouseMove = (ev: MouseEvent) => {
        var ss = ev;
    }
    @ContentChildren(LogColumnComponent, { descendants: false }) cols: QueryList<LogColumnComponent>;
    @ContentChild(LogRowDetailsComponent) RowDetail;
    @ViewChildren(LogColumnComponent) wrapper: QueryList<LogColumnComponent>;
    TotalWidth: number = 0;
    SetSelectedRows() {
        if (this.SelectedRows.length > 0) {
            this.SelectedRows.forEach((value, key) => {
                if (this.rows.filter(a => a.rowData == value).length > 0) {
                    this.rows.filter(a => a.rowData == value)[0].IsSelected = true;
                }

            });
            this.cd.detectChanges();
        }
    }
    ngAfterContentInit() {
        var ss = this.cols;
        var sss = this.RowDetail;
        if (this.RowDetail) {
            this.DetailButtonVisibile = true;
        }
        this.customcolumns = [];
        
        var index = 0;
        var left = 0;
        if (this.groupby) {
            left = 15;
        }
        if (this.RowDetail) {
            left = 25;
        }
        if (this.RowDetail && this.groupby) {
            left = 40;
        }
        this.TotalWidth = 0;
        this.cols.changes.subscribe((changes: any) => {

            var index = 0;
            var left = 0;
            if (this.groupby) {
                left = 15;
            }
            if (this.RowDetail) {
                left = 25;
            }
            if (this.RowDetail && this.groupby) {
                left = 40;
            }
            this.TotalWidth = 0;
            this.customcolumns = [];
            changes.toArray().forEach((value, key) => {
                if (value.hasFootertemplate == true) {
                    this.ShowFooter = true;
                }
                if (value.hastemplate) {
                    value.index = index;
                    index++;
                }
                if (value.Editable == undefined) {
                    value.Editable = true;
                }
                if (this.RTL) {
                    value.Style = {
                        width: + (value.visibility == 'hidden' ? 0 : value.width.replace("px", "")) + 'px',
                        right: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'overflow': 'hidden',
                        'white-space': 'nowrap',
                        'text-overflow': 'ellipsis',
                        'min-width': '25px'
                    };
                    value.HeaderStyle = {
                        'height': this.HeaderHeight - 1 + 'px',
                        'line-height': this.HeaderHeight <= 27 ? '26px' : 'normal',
                        width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        right: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                    };
                }
                else {
                    value.Style = {
                        width: + (value.visibility == 'hidden' ? 0 : value.width.replace("px", "")) + 'px',
                        left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'overflow': 'hidden',
                        'white-space': 'nowrap',
                        'text-overflow': 'ellipsis',
                        'min-width': '25px'
                    };
                    value.HeaderStyle = {
                        'height': this.HeaderHeight - 1 + 'px',
                        'line-height': this.HeaderHeight <= 27 ? '26px' : 'normal',
                        width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                    };
                }
                if (value.visibility != 'hidden') {
                    this.TotalWidth += + (value.width.replace("px", ""));
                    value.width = value.width;
                    left += (+(value.width.replace("px", "")));
                }
                if (value.AllowDisableCells) {
                    this.AllowDisableCells = true;
                }
                this.customcolumns.push(value);
            });
            this.init();
        });
        this.cols.toArray().forEach((value, key) => {
            if (value.hasFootertemplate == true) {
                this.ShowFooter = true;
            }
            value.LogGridId = this.LogGridId;
            value.EditableLogGridComponent = this;
            if (value.IgnoreColumn == false) {
                this.CurrentSession.LogitudeGridHelper.SetColumnsCount(false, this.LogGridId);
            }
            value.IsReadOnlyGrid = this.IsReadOnly;
            if (value.hastemplate) {
                value.index = index;
                index++;
            }
            if (value.Editable == undefined) {
                value.Editable = true;
            }
            if (this.RTL) {
                value.Style = {
                    width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                    right: (value.visibility == 'hidden' ? 0 : left) + 'px',
                    'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                    'text-align': value.Alignment ? value.Alignment : 'left',
                    'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                    'overflow': 'hidden',
                    'white-space': 'nowrap',
                    'text-overflow': 'ellipsis',
                    'min-width': '25px'
                };
                value.HeaderStyle = {
                    'height': this.HeaderHeight - 1 + 'px',
                    'line-height': this.HeaderHeight <= 27 ? '26px' : 'normal',
                    width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                    right: (value.visibility == 'hidden' ? 0 : left) + 'px',
                    'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                    'text-align': value.Alignment ? value.Alignment : 'left',
                };
            }
            else {
                value.Style = {
                    width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                    left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                    'background-color': value.Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                    'text-align': value.Alignment ? value.Alignment : 'left',
                    'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                    'overflow': 'hidden',
                    'white-space': 'nowrap',
                    'text-overflow': 'ellipsis',
                    'min-width': '25px'
                };
                value.HeaderStyle = {
                    'height': this.HeaderHeight - 1 + 'px',
                    'line-height': this.HeaderHeight <= 27 ? '26px' : 'normal',
                    width: + (value.visibility == 'hidden' ? 0 : value.width) + 'px',
                    left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                    'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                    'text-align': value.Alignment ? value.Alignment : 'left',
                };
            }
            if (value.visibility != 'hidden') {
                this.TotalWidth += + (value.width);
                value.width = value.width + 'px';
                left += (+(value.width.replace("px", "")));
            }
            if (value.AllowDisableCells) {
                this.AllowDisableCells = true;
            }
            this.customcolumns.push(value);
        });
    }

    ngAfterViewInit() {
        var RowsElement = document.getElementById(this.LogGridRowsId);
        var RowsToLoad = RowsElement.clientHeight / this.rowHeight;
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridId);
        this.LogGridElement = elem;
        this.scrollPosition = RowsElement.scrollTop;
        var vscroll = elem.scrollTop;
        var hscroll = elem.scrollLeft;
        this.rowCount = this.ItemSource.Length;
        this.init();
    }
    handleonclick(evt, rownum) {
        this.InClick = true;
        if (this.currentrow == undefined) {
            this.currentrow = rownum;
        }
        if (rownum == this.currentrow) {
            if (this.id != undefined) {
                var element = document.getElementById(this.id);
                element.style.border = "none";
            }
            this.onparentclickEvent.emit(evt);
        }
        else {
            var keepGoing = true;
            var colindex = -1;
            this.customcolumns.forEach((item, key) => {
                if (keepGoing == true) {
                    if (item.binding != undefined && item.binding != '') {
                        colindex++;
                        if (item.required && (this.dataSource[this.currentrow][item.binding] == undefined || this.dataSource[this.currentrow][item.binding] == "")) {
                            this.allowtomove = false;
                            var id = "row" + this.currentrow + "col" + colindex;
                            var element = document.getElementById(id);
                            if (element != null) {
                                element.focus();
                                element.style.border = "thin dotted red";
                            }
                            keepGoing = false;
                        }
                    }
                }
            });
            if (this.allowtomove) {
                this.currentrow = rownum;
                if (this.id != undefined) {
                    var element = document.getElementById(this.id);
                    element.style.border = "none";
                }
                this.onparentclickEvent.emit(evt);
            }
        }
    }
    keyupHandler(arg, evt, rownum) {
        if (arg.keyCode == 9) {
            this.handleonclick(evt, rownum);
        }
    }
    handleblur(evt, rownum) {
        return;
    }
    handlerowfocus(rownum) {
        this.currentrow = rownum;
    }

    public LastHeaderColumnWidth: number;

    setselectedstyle() {
        this.RowClass = "ag-selected-row SelectedRow";
    }

    sortingDir: string = '';
    sortingCol: string = '';
    CompareFunction(a, b) {
        if (a < b) return -1;
        if (a > b) return 1;
        return 0;
    }
    OrigionalSortingData: any[];
    ServerSortTimer: any;
    ServerSort(colDef, i) {
        if (this.ServerSortTimer) {
            clearTimeout(this.ServerSortTimer);
        }
        this.ServerSortTimer = setTimeout(() => this.DoServerSort(colDef, i), 200);
    }

    CompareAscending(a, b) {
        if (a == undefined) {
            a = -1;
        }
        if (b == undefined) {
            b = -1;
        }
        if (a > b) {
            return 1;
        }
        if (a < b) {
            return -1;
        }
        return 0;
    }

    CompareDecending(a, b) {
        if (a == undefined) {
            a = -1;
        }
        if (b == undefined) {
            b = -1;
        }
        if (a > b) {
            return -1;
        }
        if (a < b) {
            return 1;
        }
        return 0;
    }

    SortByColumn: any;
    DoServerSort(colDef, i) {
        this.SortByColumn = colDef;
        if (colDef.SortFieldName) {
            this.OrigionalSortingData = this.ItemSource.Collection;
            if (colDef.SortFieldName != this.sortingCol) {
                this.sortingDir = 'Ascending';
                this.sortingCol = colDef.SortFieldName;
            }
            switch (this.sortingDir) {
                case 'Ascending':
                    {
                        this.sortingDir = "Descending";
                        var temp = this.ItemSource.Collection.sort((a, b) => this.CompareAscending(a[colDef.SortFieldName], b[colDef.SortFieldName]));//(a[colDef.SortFieldName] < b[colDef.SortFieldName]) ? -1 : ((a[colDef.SortFieldName] > b[colDef.SortFieldName]) ? 1 : 0));
                        this.ItemSource = new ObservableCollection(temp);
                        break;
                    }
                case "Descending":
                    {
                        this.sortingDir = "Ascending";
                        var temp = this.ItemSource.Collection.sort((a, b) => this.CompareDecending(a[colDef.SortFieldName], b[colDef.SortFieldName]));//(a[colDef.SortFieldName] < b[colDef.SortFieldName]) ? 1 : ((a[colDef.SortFieldName] > b[colDef.SortFieldName]) ? -1 : 0));
                        this.ItemSource = new ObservableCollection(temp);
                        break;
                    }
                default:
            }
            var allelems = document.getElementsByClassName("ag-header-cell");
            for (var a = 0; a < allelems.length; a++) {
                if (allelems.item(a).attributes['LogGridId'].value == this.LogGridId) {
                    (<HTMLElement>allelems.item(a)).style.background = 'transparent';
                }
            }
            if (this.sortingDir === 'Descending' || this.sortingDir === 'Ascending') {

                var ColumnsElements = document.getElementsByClassName("ag-header-cell");
                for (var j = 0; j < ColumnsElements.length; j++) {
                    if (ColumnsElements[j].attributes['LogGridId'].value == this.LogGridId) {
                        if (ColumnsElements[j].attributes['FieldName'] && ColumnsElements[j].attributes['FieldName'].value == colDef.SortFieldName) {
                            (<HTMLElement>ColumnsElements[j]).style.background = '#cfcbcb';
                        }
                    }
                }
            }
            this.rows = [];
            this.updateDisplayList(true);

        }
    }

    resetHeaderCellBackGround() {
        var ColumnsElements = document.getElementsByClassName("ag-header-cell");
        for (var j = 0; j < ColumnsElements.length; j++) {
            if (ColumnsElements[j].attributes['LogGridId']?.value == this.LogGridId) {
                (<HTMLElement>ColumnsElements[j]).style.background = 'transparent';
            }
        }
    }

    cellClicked(field, value, rowIndex, colIndex) { }
    rowClicked(rowIndex, row) { }
    rowsBuffer: IRow[] = [];
    firstRow: number = 0;
    existingFirstRow: number = -1;
    extraRows: number = 10;
    oldPage: number;
    group: any[];
    updateDisplayList(reload: boolean = false) {
        if (this.UseVirtuallization == false) {
            this.updateDisplayListGrouping(reload);
        }
        else {
            this.rows = [];
            if (reload) {
                this.rowsBuffer = [];
            }
            var oldRow = this.firstRow;
            var firstRow = Math.floor(this.scrollTop / (this.rowHeight));
            var RabaiaVar = Math.floor(this.GetDetailsHeight() / (this.rowHeight));
            var oldPage; // = 0;
            var currentPage = Math.max(Math.floor(this.scrollTop / this.rowHeight / this.rowsPerPage), 0);
            var rowsToCreate = Math.min((this.scrollTop / (this.rowHeight)) + this.rowsPerPage, this.rowsPerPage);
            if ((firstRow == 0 || this.oldRow != firstRow)) {
                this.bufferFromRow = firstRow;
                if (this.rowCount < (this.viewportSize - this.HeaderHeight)) {
                    this.ViewPortRowCount = this.rowCount;
                }
                else {
                    this.ViewPortRowCount = (this.viewportSize - this.HeaderHeight) > 0 ? (this.viewportSize - Math.ceil(this.HeaderHeight / this.rowHeight) ): 30; // + this.extraRows;
                }
                var top = 0;
                var Bodytop = this.HeaderHeight;
                var DetailsHeight = 0;
                this.StaticDetailsHeight = 0;
                var OpenedItemsHeight = 0;
                var DetailsDivHeight = 0;
                this.group = this.group.sort((a, b) => (a["Index"] < b["Index"]) ? -1 : ((a["Index"] > b["Index"]) ? 1 : 0));
                for (var i = firstRow; i < firstRow + this.ViewPortRowCount; i++) {

                    if (i >= this.rowCount) {
                        //this.rowsBuffer.forEach((item) => {
                        //    var existingItem = this.rows.filter(d => d.rowIndex === item.rowIndex)[0];
                        //    if (!existingItem) {
                        //        this.rows.push(item);

                        //    }
                        //});
                        var isDestroyed: boolean = this.cd['destroyed'];
                        if (!isDestroyed) {
                            this.cd.detectChanges();
                        }
                        //this.RenderLoadedRows();
                        return;
                    }
                    top = (this.group[i].Index * this.rowHeight) + DetailsDivHeight;
                    Bodytop = top + this.rowHeight;
                    var buffered = this.rowsBuffer.filter(a => a && (a.rowIndex == this.group[i].Index))[0];
                    if (buffered) {
                        buffered.styles = {
                            'top': top + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': '100%'
                        };
                        buffered.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': '100%'
                        };
                        if (buffered.IsDetailesOpened == true) {
                            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (buffered.rowIndex));
                            if (DetailsDiv) {
                                DetailsDivHeight += DetailsDiv.clientHeight;
                                //OpenedItemsHeight += DetailsDivHeight;
                            }
                        }
                    }
                    else {

                        var row: IRow = { rowIndex: this.group[i].Index, rowData: this.group[i].Data, DetailsIcon: (this.group[i].Type == "Head" ? "./Images/CellIcons/Arrowup.png" : "./Images/CustomTreeIcon.png"), IsDetailesOpened: (this.group[i].Type == "Head" ? true : false), Type: this.group[i].Type, IsGroupOpened: true, ChildrenFirstIndex: this.group[i].ChildrenFirstIndex, IsExpandable: false, SetExpandaple(isExpandable: boolean) { this.IsExpandable = isExpandable; }, ChildrensCount: this.group[i].Count, IsSelected: false };
                        row.rowData.rowIndex = (row.rowIndex);
                        row.styles = {
                            'top': top + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': '100%'
                        };
                        row.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                            'width': '100%'
                        };
                        //row.styles = {
                        //    'top': (row.rowIndex * this.rowHeight) + "px",
                        //    'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        //    'width': '100%'
                        //};
                        //row.Detailsstyles = this.getRowDetailsStyles();
                        //    {
                        //    'top': (row.rowIndex * this.rowHeight) + "px",
                        //    'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        //    'width': '100%'
                        //};
                        if (row) {
                            buffered = row;
                            this.rowsBuffer.push(row); //this.rows.push(row);
                            //this.rows.push(row);
                            //this.RowDataLoaded.emit(row);
                        }
                    }

                    this.rows.push(buffered);
                    this.RowDataLoaded.emit(buffered);
                }
                //this.RenderLoadedRows();
                var isDestroyed: boolean = this.cd['destroyed'];
                if (!isDestroyed) {
                    this.cd.detectChanges();
                }


            }
        }
    };
    GetDetailsHeight() {
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        var StaticDetailsHeight = 0;
        this.rowsBuffer.forEach((value, key) => {
            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }
            StaticDetailsHeight += DetailsDivHeight;
        });

        return StaticDetailsHeight;
    }
    GetPrevDetailsHeight(Index: number) {
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        var StaticDetailsHeight = 0;
        //for (var i = firstRow; i < firstRow + this.ViewPortRowCount; i++) {
        //}
        this.rowsBuffer.forEach((value, key) => {
            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                if (value.rowIndex < Index) {
                    DetailsDivHeight = DetailsDiv.clientHeight;
                    OpenedItemsHeight += DetailsDivHeight;
                }
            }
            StaticDetailsHeight += DetailsDivHeight;
        });

        return StaticDetailsHeight;
    }
    RenderLoadedRows() {
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        this.StaticDetailsHeight = 0;
        this.rowsBuffer.forEach((value, key) => {

            value.styles = {
                'top': top + "px",
                'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            };
            value.Detailsstyles = {
                'top': Bodytop + "px",
                'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            };
            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }

            top += (this.rowHeight) + DetailsDivHeight;
            DetailsHeight += DetailsDivHeight;
            this.StaticDetailsHeight += DetailsDivHeight;

            Bodytop = top + this.HeaderHeight;//27;

        });
        //if (this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex).length > 0) {
        //    this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex)[0] = value;
        //} 
        this.canvasHeight = {
            //'top': this.HeaderHeight + 'px',
            height: (this.rowCount * this.rowHeight) + DetailsHeight + 'px',
            'width': '100%',//this.TotalWidth + 'px',
            'min-width': this.TotalWidth + 'px'//(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
        };
        this.BodyTop = this.HeaderHeight;
    }
    getRowStyles() {
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        this.StaticDetailsHeight = 0;
        this.rows.forEach((value, key) => {

            //value.styles = {
            //    'top': top + "px",
            //    'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
            //    'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            //};
            //value.Detailsstyles = {
            //    'top': Bodytop + "px",
            //    'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
            //    'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            //};
            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }

            top += (this.rowHeight) + DetailsDivHeight;
            DetailsHeight += DetailsDivHeight;
            this.StaticDetailsHeight += DetailsDivHeight;

            Bodytop = top + this.HeaderHeight;//27;

        }); 

        this.BodyTop = this.HeaderHeight;
        var styles = {
            'top': top + "px",
            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
            'width': '100%'
        };
        return DetailsHeight;
    }

    getRowDetailsStyles() {
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        this.StaticDetailsHeight = 0;
        this.rows.forEach((value, key) => {
            var DetailsDiv = document.getElementById(this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }

            top += (this.rowHeight) + DetailsDivHeight;
            DetailsHeight += DetailsDivHeight;
            this.StaticDetailsHeight += DetailsDivHeight;

            Bodytop = top + this.HeaderHeight;//27;

        });

        this.BodyTop = this.HeaderHeight;
        var styles = {
            'top': Bodytop + "px",
            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
            'width': '100%'
        };
        return styles;
    }

    getRowDetailsStylesbyIndex(index: number) {

        var styles = {
            'top': (index * this.rowHeight) + this.rowHeight + "px",
            'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
            'width': '100%'
        };
        return styles;
    }
    oldRow: number;
    updateDisplayListGrouping(reload: boolean = false) {
        if (reload) {
            this.rowsBuffer = [];
        }
        if (!this.rowsBuffer) {
            this.rowsBuffer = [];
        }
        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        if (firstRow < this.oldRow) {
            firstRow = this.oldRow + 1;
        }
        var currentPage = Math.max(Math.floor(this.scrollTop / this.rowHeight / this.rowsPerPage), 0);
        var rowsToCreate = Math.min(firstRow + this.rowsPerPage, this.rowsPerPage);

        if ((firstRow == 0 || this.oldRow != firstRow)) {
            this.rows = [];
            this.bufferFromRow = firstRow;
            if (this.rowCount < this.viewportSize) {
                this.ViewPortRowCount = this.rowCount;
            }
            else {
                this.ViewPortRowCount = this.viewportSize; // + this.extraRows;
            }
            this.group.forEach((value, key) => {
                if (value.Index >= this.rowCount) {
                    this.cd.detectChanges();
                    return;
                }
                var buffered = this.rowsBuffer.filter(a => a.rowIndex === value.Index)[0];
                if (!buffered) {
                    var row: IRow = { rowIndex: value.Index, rowData: value.Data, DetailsIcon: (value.Type == "Head" ? "./Images/CellIcons/Arrowup.png" : "./Images/CustomTreeIcon.png"), IsDetailesOpened: (value.Type == "Head" ? true : false), Type: value.Type, IsGroupOpened: true, ChildrenFirstIndex: value.ChildrenFirstIndex, IsExpandable: false, SetExpandaple(isExpandable: boolean) { this.IsExpandable = isExpandable; }, ChildrensCount: value.Count, IsSelected: false };
                    row.styles = {
                        'top': (row.rowIndex * this.rowHeight) + "px",
                        'min-width': this.TotalWidth + this.LastDefaultSpace + 'px',
                        'width': '100%'
                    };
                    this.rowsBuffer.push(row);
                    this.RowDataLoaded.emit(row);
                }
            });
            this.rowsBuffer.forEach((item) => {
                var existingItem = this.rows.filter(d => d.rowIndex === item.rowIndex)[0];
                if (!existingItem) {
                    this.rows.push(item);

                }
            });
            var isDestroyed: boolean = this.cd['destroyed'];
            if (!isDestroyed) {
                this.cd.detectChanges();
            }
            this.oldPage = currentPage;
        }
    };
    oldScrollTop: number;
    headerStyle: any = { left: '0px' };
    rowStyle: any = {};
    timer = null;
    scrollDirection: string = null;
    HScrollPosition: number = -1;
    onScroll() {
        var columns: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridColumnsId);
        var elem: HTMLDivElement = <HTMLDivElement>document.getElementById(this.LogGridRowsId);

        this.LogGridElement = elem;
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
        if (this.HScrollPosition == -1 || elem.scrollLeft > this.HScrollPosition) {
            this.HScrollPosition = elem.scrollLeft;
        }
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
        if (vscroll != this.oldScrollTop) {
            this.oldScrollTop = vscroll;
            this.updateDisplayList();
        }
    };

    private onWindowResized(event: UIEvent): void {
        var logGrid = document.getElementById(this.LogGridId);
        if (logGrid != null) {
            logGrid.style.display = "none";
            logGrid.style.display = "block";
            this.ViewHeight = logGrid.clientHeight;
            this.ViewWidth = logGrid.clientWidth;
        }
      
        var rowsArray = document.getElementsByClassName("ag-row");
        for (var i = 0; i < rowsArray.length; i++) {
            (<HTMLScriptElement>rowsArray.item(i)).style.minWidth = (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px';
        }
        this.rowStyle.minWidth = this.ViewWidth + 'px';
        this.viewportSize = Math.round(this.ViewHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
      
        this.headerStyle = {
            'height': this.HeaderHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px',
        };
        var MinWidth = '';
        if (this.TotalWidth > this.ViewWidth && this.ViewWidth != 0) {
            MinWidth = this.TotalWidth + 'px';
        }
        else if (this.ViewWidth == 0) {
            MinWidth = '100%;'
        }
        else {
            MinWidth = this.ViewWidth + 'px';
        }
        if (this.TotalWidth == 0) {
            this.cols.toArray().forEach((value, key) => {
                if (value.visibility != 'hidden') {
                    this.TotalWidth += + (value.width);
                }
            });
        }
        this.canvasHeight = {
           
            height: (this.rowCount * this.rowHeight) + this.StaticDetailsHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px'
        };
        this.BodyTop = this.HeaderHeight;
    }

    GridBodyResized() {
    }

    ngOnDestroy() {
        if (this.WindowResizeSub) {
            this.WindowResizeSub.unsubscribe();
        }
        if (this.EndOfRowReachedSub) {
            this.EndOfRowReachedSub.unsubscribe();
        }
        if (this.ReloadDetailsSub) {
            this.ReloadDetailsSub.unsubscribe();
        }
        if (this.ReRenderGridSub) {
            this.ReRenderGridSub.unsubscribe();
        }
        if (this.FinishLoadingSub) {
            this.FinishLoadingSub.unsubscribe();
        }
        if (this.FinishLoadingGroupsSub) {
            this.FinishLoadingGroupsSub.unsubscribe();
        }
        this.WindowResizeSub = null;
        this.EndOfRowReachedSub = null;
        this.ReloadDetailsSub = null;
        this.ReRenderGridSub = null;
        this.FinishLoadingSub = null;
        this.FinishLoadingGroupsSub = null;
    }
    private countIsHere = true;
    private oldSearchFields: string;
    private oldQueryId: string;
    private oldQueryCode: string;

    editingCell: any = [];
    focusCell($event, rowIndex, colIndex) {
    }

    public MouseDownX: any;
    public MouseUpX: any;
    public ResizableColumn: any;
    public ColumnWidth: number;
    resizeColumn($event, column) {
        $event.preventDefault();
        window.addEventListener("mousemove", this.mouseMove);

        this.MouseDownX = $event.clientX;
        this.ResizableColumn = document.getElementById(column);
        this.ColumnWidth = this.ResizableColumn.clientWidth;
    }
    resizeColumn2($event, column) {
        $event.preventDefault();
        window.removeEventListener("mousemove", this.mouseMove);
        this.MouseUpX = $event.clientX;
        var diff: number = this.MouseUpX - this.MouseDownX;
        if (diff < 0) {
            this.ResizableColumn.style.width = this.ColumnWidth - diff;
        }
        else {
            this.ResizableColumn.style.width = (this.ColumnWidth + diff) + 'px';
        }
    }
    mousemoveevent(args) {
        var ss = args;
    }

    divOnFocus(rowIndex, colIndex) {

        var inputID = 'input-row' + rowIndex + 'col' + colIndex;
        var elem: HTMLInputElement = <HTMLInputElement>document.getElementById(inputID);
        var span: HTMLSpanElement = document.getElementById('span-row' + rowIndex + 'col' + colIndex);
        if (span && elem) {
            elem.style.display = 'block';
            span.style.display = 'none';
            elem.focus();
        }
    }
    blurInput(rowIndex, colIndex) {
        var inputID = 'input-row' + rowIndex + 'col' + colIndex;
        var elem: HTMLInputElement = <HTMLInputElement>document.getElementById(inputID);
        var span: HTMLSpanElement = document.getElementById('span-row' + rowIndex + 'col' + colIndex);
        if (span && elem) {
            elem.style.display = 'none';
            span.style.display = 'block';
        }
    }

    displayVisibility: any;
    editVisibility: any;

    inputFocus($event) {
    }

    GetRowCount(reload: boolean = false) {
        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        this.controller.getRow(firstRow, this.sortingCol, this.sortingDir, true, "", false, null, reload);

    }

    private selectedRow: any;
    get SelectedRow() { return this.selectedRow; }
    set SelectedRow(newValue: any) {
        if (this.selectedRow != newValue) {
            this.selectedRow = newValue;
        }
    }

    public selectedRows: any[] = [];
    get SelectedRows() { return this.selectedRows; }
    set SelectedRows(newValue: any[]) {
        if (this.selectedRows != newValue) {
            this.selectedRows = newValue;
            this.selectedRows.forEach((value, key) => {
                if (this.rows.filter(a => a.rowData == value).length > 0) {
                    this.rows.filter(a => a.rowData == value)[0].IsSelected = true;
                }
            });
            if (this.EnableMultiSelection === true && this.selectedRows?.length === 0) {
                this.rows?.forEach((value) => {
                    value.IsSelected = false;
                });
            }
        }
    }
    public EnableMultiSelection: boolean = false;
    onRowSelected(item) {
        if (this.Disabled) {
            return;
        }
        if (this.EnableMultiSelection == true) {
            if (this.SelectedRows.filter(a => a == item).length > 0) {
                this.SelectedRows = this.SelectedRows.filter(a => a != item);
                this.rows.filter(a => a.rowData == item)[0].IsSelected = false;
            }
            else {
                this.SelectedRows.push(item);
                this.rows.filter(a => a.rowData == item)[0].IsSelected = true;
            }
            this.SelectedItemChanged.emit(this.SelectedRows);

        }
        else {
            this.SelectedItemChanged.emit(item);
        }
    }
    OnRowDoubleClick(item) {
        this.RowDoubleClick.emit(item);
    }


    OnMouseleave(item) {

        this.MouseleaveChanged.emit(item.rowData);
    }


    OnMouseOver(item) {

        this.MouseOverChanged.emit(item.rowData);
    }



    Ondblclicked() {
        this.Ondblclick.emit("");
    }

}

interface IRow {
    rowIndex: number;
    rowData: any;
    styles?: any;
    DetailsIcon: string;
    IsDetailesOpened: boolean;
    Detailsstyles?: any;
    Groupstyles?: any;
    Type?: string;
    IsGroupOpened: boolean;
    ChildrenFirstIndex?: number;
    IsExpandable: boolean;
    SetExpandaple(isExpandable: boolean);
    ChildrensCount: number;
    IsSelected: boolean;
}
