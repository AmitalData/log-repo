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
//import {CORE_DIRECTIVES} from '@angular/common';
var LogColumnComponent_1 = require("./LogColumnComponent");
var LogRowDetailsComponent_1 = require("./LogRowDetailsComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
//import {VirtualRowController} from './VirtualRowController'; 
var EditGridVirtualRowController_1 = require("./EditGridVirtualRowController");
//import {ListHeaderTemplateComponent} from './app/ListHeaderTemplateComponent';
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var GroupByPipe_1 = require("../../../../Infrastructure/Pipes/GroupByPipe");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var EditableLogGridComponent = /** @class */ (function () {
    function EditableLogGridComponent(_elementRef, _renderer, cd, differs) {
        var _this = this;
        this._elementRef = _elementRef;
        this._renderer = _renderer;
        this.cd = cd;
        this.FooterTop = 0;
        this.HeaderHeight = 27;
        this.BodyTop = 27;
        this.EnableGridViewRowBackground = false;
        this.HorizantalScrollStatus = 'auto';
        this.ShowFooter = false;
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false); //true;
        this.UseVirtuallization = false;
        this.MyTimer = null;
        this.EnableExpandCollapseAll = false;
        this.ShowCollapseAllIcon = false;
        this.IsDarkHeader = false;
        this.Disabled = false;
        //rows123: IRow[];
        this.rowHeight = 27;
        this.ShowCount = false;
        this.height = 800;
        this.scrollTop = 0;
        this.pixelsPerPage = this.rowsPerPage * this.rowHeight;
        this.numberOfRowsToDraw = 0;
        this.cachedPages = [];
        this.pageIndex = 0;
        this.lastIndex = 0;
        this.inProgressRows = 0;
        this.MouseOverChanged = new core_1.EventEmitter();
        this.MouseleaveChanged = new core_1.EventEmitter();
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.RowDoubleClick = new core_1.EventEmitter();
        this.Ondblclick = new core_1.EventEmitter();
        this.RowEnded = new core_1.EventEmitter();
        this.RowDataLoaded = new core_1.EventEmitter();
        this.onparentclickEvent = new core_1.EventEmitter();
        this.scrollPosition = null;
        this.LogGridId = null;
        this.LogGridRowsId = null;
        this.LogGridColumnsId = null;
        this.show = true;
        this.Vir = false;
        this.DetailButtonVisibile = false;
        this.ColumnId = null;
        this.AllowDisableCells = false;
        this.DetailsDivId = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isResizing = false;
        this.lastDownX = 0;
        this.ColIndex = 0;
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
        this.isDraging = false;
        this.GridLeft = 0;
        this.OldWidth = 0;
        this.NewWidthFinal = 0;
        //leftPadd : number = 0;
        this.FinalWidthNew = 0;
        this.StaticDetailsHeight = 0;
        this.SelectedIndex = -1;
        this.FinishLoading = new core_1.EventEmitter();
        this.FinishLoadingGroups = new core_1.EventEmitter();
        //renderRows(res: any[]) {
        //    //if (this.SourceItems == null) {
        //    //    this.SourceItems = [];
        //    //}
        //    if (res.length < this.viewportSize || res.length < this.rowsPerPage) {
        //        this.ViewPortRowCount = res.length;
        //    }
        //    var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        //    var currentPage = Math.max(Math.floor(this.scrollTop / this.rowHeight / this.rowsPerPage), 0);
        //    res.forEach((value, key) => {
        //        //if (value.RowIndex >= this.rowCount) {
        //        //    return;
        //        //}
        //        //console.log("RowIndex", value.RowIndex);
        //        if (value.RowIndex >= this.rowCount) {
        //            return;
        //        }
        //        //if (value.RowData) {
        //        var buffered = this.rowsBuffer.filter(a => a.rowIndex === value.RowIndex)[0];
        //        if (!buffered) {
        //            ////console.log(value.RowIndex);
        //            var rowData = value.RowData;
        //            // //console.log(value.RowIndex);
        //            var row: IRow = { rowIndex: value.RowIndex, rowData: value.RowData, DetailsIcon: "+", IsDetailesOpened: false };
        //            row.styles = {
        //                'top': (value.RowIndex * this.rowHeight) + "px",
        //                'min-width': (this.ViewWidth) + 'px'
        //            };
        //            ////console.log("row.styles", row.styles);
        //            this.inProgressRows--;
        //            this.rowsBuffer.push(row);
        //        }
        //        //}
        //    });
        //    //if (this.inProgressRows < 0) {
        //    //    console.log("inProgressRows", this.inProgressRows);
        //    //}
        //    this.rowsBuffer.forEach((item) => {
        //        var existingItem = this.rows.filter(d => d.rowIndex === item.rowIndex)[0];
        //        if (!existingItem) {
        //            this.rows.push(item);
        //            //this.SourceItems.push(item.rowData); 
        //        }
        //    });
        //    this.rowsBuffer = [];
        //    if (this.rows.length > this.tripleViewport) {
        //        var diff = this.rows.length - this.tripleViewport;
        //        if (this.scrollDirection == "up") {
        //            /**
        //            Calculate lowest index of rows and delete from there
        //            */
        //            this.rows.splice(diff, diff - this.viewportSize);
        //            //console.log("roooooows.lenght up", this.rows.length);
        //        }
        //        else if (this.scrollDirection == "down") {
        //            /**
        //            Calculate lowest index of rows and delete from there
        //            */
        //            //console.log("roooooows.lenght down", this.rows.length);
        //            ////console.log("rows.length", this.rows.length);
        //            /*this.rows = */
        //            this.rows.splice(0, diff - this.viewportSize);
        //            ////console.log("rows.length", this.rows.length);
        //            //////console.log("rows.length", this.rows.length);
        //        }
        //    }
        //}
        this.mouseMove = function (ev) {
            var ss = ev;
        };
        //@ViewChildren(LogCellTemplateComponent) cellChildren;//: QueryList<LogCellTemplateComponent>; 
        this.TotalWidth = 0;
        this.sortingDir = '';
        this.sortingCol = '';
        this.rowsBuffer = [];
        this.firstRow = 0;
        this.existingFirstRow = -1;
        this.extraRows = 10;
        this.headerStyle = { left: '0px' };
        this.rowStyle = {};
        this.timer = null;
        this.scrollDirection = null;
        this.HScrollPosition = -1;
        this.countIsHere = true;
        this.editingCell = [];
        this.selectedRows = [];
        this.EnableMultiSelection = false;
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
        this.controller = new EditGridVirtualRowController_1.EditGridVirtualRowController();
        //window.onresize = this.onWindowResized.bind(this);
        this.WindowResizeSub = this.CurrentSession.WindowResizeEvent.subscribe(function (res) {
            _this.onWindowResized(res);
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
    Object.defineProperty(EditableLogGridComponent.prototype, "ItemSource", {
        get: function () { return this.itemSource; },
        set: function (newValue) {
            var _this = this;
            this.itemSource = newValue;
            newValue.Changed.subscribe(function (evt) {
                _this.CurrentSession.LogitudeGridHelper.ResetRowIndex(_this.LogGridId);
                _this.CurrentSession.LogitudeGridHelper.ResetNextRowIndex(_this.LogGridId);
                var OldCount = _this.group.length;
                if (_this.groupby) {
                    _this.group = new GroupByPipe_1.GroupByPipe().ShapeGrouping(_this.ItemSource.Collection, _this.groupby);
                    _this.rowCount = _this.group.length;
                    _this.updateDisplayListGrouping(true);
                }
                else {
                    _this.group = new GroupByPipe_1.GroupByPipe().ShapeList(_this.ItemSource.Collection);
                    _this.rowCount = _this.group.length;
                    //this.updateDisplayListGrouping(true);
                    _this.updateDisplayList(true);
                }
                _this.canvasHeight = {
                    //'top': this.HeaderHeight + 'px',
                    height: (_this.rowCount * _this.rowHeight) + 'px',
                    'width': '100%',
                    'min-width': _this.TotalWidth + 'px' ////(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                };
                _this.BodyTop = _this.HeaderHeight;
                var isDestroyed = _this.cd['destroyed'];
                if (!isDestroyed) {
                    _this.cd.detectChanges();
                }
                if (evt.IsCollection == false && !evt.RowIndex) {
                    var RowsElement = document.getElementById(_this.LogGridRowsId);
                    if (RowsElement && !evt.PreventScroll) {
                        RowsElement.scrollTop = RowsElement.scrollHeight;
                    }
                    if (_this.group.length != OldCount && _this.ItemSource.Length > 0 && evt.FocusFirstCell == true) {
                        _this.MyTimer = setTimeout(function () { return _this.NewElementInsertedEvent(evt.RowIndex); }, 400);
                    }
                }
                else if (evt.RowIndex) {
                    var RowsElement = document.getElementById(_this.LogGridRowsId);
                    if (RowsElement && !evt.PreventScroll) {
                        RowsElement.scrollTop = (evt.RowIndex * _this.rowHeight) - _this.rowHeight;
                    }
                    if (_this.group.length != OldCount && _this.ItemSource.Length > 0 && evt.FocusFirstCell == true) {
                        _this.MyTimer = setTimeout(function () { return _this.NewElementInsertedEvent(evt.RowIndex); }, 400);
                    }
                }
                else if (!evt.PreventScroll) {
                    var RowsElement = document.getElementById(_this.LogGridRowsId);
                    if (RowsElement) {
                        RowsElement.scrollTop = 0;
                    }
                }
                _this.SetSelectedRows();
            });
            if (this.groupby) {
                this.group = new GroupByPipe_1.GroupByPipe().ShapeGrouping(this.ItemSource.Collection, this.groupby);
                this.rowCount = this.group.length;
                this.updateDisplayListGrouping(true);
            }
            else {
                this.group = new GroupByPipe_1.GroupByPipe().ShapeList(this.ItemSource.Collection);
                this.rowCount = this.group.length;
                //this.updateDisplayListGrouping(true);
                this.updateDisplayList(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    EditableLogGridComponent.prototype.NewElementInsertedEvent = function (rowIndex) {
        if (rowIndex === void 0) { rowIndex = null; }
        if (this.MyTimer) {
            clearTimeout(this.MyTimer);
        }
        this.CurrentSession.ObsNewElementInsertedEvent.emit({ length: this.group.length, Id: this.LogGridId, RowIndex: rowIndex });
    };
    EditableLogGridComponent.prototype.OnRowMouseOut = function (rowIndex, BackGround, IsSelected) {
        //"linear-gradient(0deg, rgba(133, 203, 237, 1) 0%, rgba(232, 247, 255, 1) 100%)"
        var Color = "transparent";
        var elem = document.getElementById(this.LogGridId + 'row' + rowIndex);
        if (elem) {
            if (!Tools_1.AppTool.IsNullOrEmpty(BackGround)) {
                Color = BackGround;
            }
            if (IsSelected) {
                Color = "linear-gradient(0deg, rgba(133, 203, 237, 1) 0%, rgba(232, 247, 255, 1) 100%)";
            }
            elem.style.background = Color;
        }
    };
    EditableLogGridComponent.prototype.OnRowMouseOver = function (rowIndex, BackGround, IsSelected) {
        var temp = "linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(221, 232, 245, 1) 100%)";
        var Selected = "linear-gradient(0deg, rgba(133, 203, 237, 1) 0%, rgba(232, 247, 255, 1) 100%)";
        var elem = document.getElementById(this.LogGridId + 'row' + rowIndex);
        if (elem) {
            if (IsSelected) {
                elem.style.background = Selected;
            }
            else {
                elem.style.background = temp;
            }
        }
    };
    Object.defineProperty(EditableLogGridComponent.prototype, "IsReadOnly", {
        get: function () { return this.isReadOnly; },
        set: function (newValue) {
            this.isReadOnly = newValue;
            if (this.cols) {
                this.cols.toArray().forEach(function (value, key) {
                    value.IsReadOnlyGrid = newValue;
                });
            }
            //this.init();
        },
        enumerable: true,
        configurable: true
    });
    EditableLogGridComponent.prototype.OnMyMouseDown = function ($event, arg) {
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
        this.ColIndex = +($event.currentTarget.parentElement.id.split(',')[1]); //+(arg.split(',')[1]);
        var left = document.getElementById(this.ColumnId + "resizable-column-," + this.ColIndex);
        this.OldWidth = left.clientWidth;
    };
    EditableLogGridComponent.prototype.OnDragMouseDown = function ($event, arg) {
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
            this.ColIndex = +($event.currentTarget.id.split(',')[1]); //+(arg.split(',')[1]);
            this.lastDownX = $event.clientX - this.GridLeft;
            //var d = document.getElementById(this.ColumnId + 'Mask'); 
            //d.style.display = "block";
            this.ShadowTitle = $event.currentTarget.innerHTML;
        }
    };
    EditableLogGridComponent.prototype.OnMyMouseUp = function (e) {
        if (this.isResizing || this.isDraging) {
            if (this.isResizing) {
                this.TotalWidth = this.FinalWidthNew;
                this.headerStyle = {
                    'height': this.HeaderHeight + 'px',
                    'width': '100%',
                    'min-width': this.TotalWidth + 'px',
                };
                this.canvasHeight = {
                    //'top': this.HeaderHeight + 'px',
                    height: (this.rowCount * this.rowHeight) + this.StaticDetailsHeight + 'px',
                    'width': '100%',
                    'min-width': this.TotalWidth + 'px' ////(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
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
    };
    EditableLogGridComponent.prototype.workOutDirection = function (event) {
        var direction;
        if (this.RTL) {
            if (this.lastDownX < (event.clientX - this.GridLeft)) {
                direction = "Right";
            }
            else if (this.lastDownX >= (event.clientX - this.GridLeft)) {
                direction = "Left";
            }
            else {
                direction = null;
            }
        }
        else {
            if (this.lastDownX > (event.clientX - this.GridLeft)) {
                direction = "Left";
            }
            else if (this.lastDownX <= (event.clientX - this.GridLeft)) {
                direction = "Right";
            }
            else {
                direction = null;
            }
        }
        return direction;
    };
    EditableLogGridComponent.prototype.OnMyMouseMove = function (e) {
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
                    this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Style = {
                        width: left.style.width,
                        right: left.style.right,
                        'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                        'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
                        'overflow': 'hidden',
                        'white-space': 'nowrap',
                        'text-overflow': 'ellipsis',
                        'min-width': '25px'
                    };
                }
                else {
                    this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Style = {
                        width: left.style.width,
                        left: left.style.left,
                        'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                        'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                        'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
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
                        col.style.right = ((+prevcol.style.right.replace("px", "")) + ((+prevcol.style.width.replace("px", "")) < 25 ? 25 : (+prevcol.style.width.replace("px", "")))) + "px"; //leftPadd + "px";
                        this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Style = {
                            width: col.style.width,
                            right: col.style.right,
                            'background-color': this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                            'text-align': this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Alignment : 'left',
                            'display': this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
                            'overflow': 'hidden',
                            'white-space': 'nowrap',
                            'text-overflow': 'ellipsis',
                            'min-width': '25px'
                        };
                    }
                    else {
                        col.style.left = ((+prevcol.style.left.replace("px", "")) + ((+prevcol.style.width.replace("px", "")) < 25 ? 25 : (+prevcol.style.width.replace("px", "")))) + "px"; //leftPadd + "px";
                        this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Style = {
                            width: col.style.width,
                            left: col.style.left,
                            'background-color': this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                            'text-align': this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].Alignment : 'left',
                            'display': this.customcolumns.filter(function (a) { return a.ColId == col.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
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
                            this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Style = {
                                width: right.style.width,
                                right: right.style.right,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            left.style.right = temp + "px";
                            this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Style = {
                                width: left.style.width,
                                right: left.style.right,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
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
                            this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Style = {
                                width: left.style.width,
                                left: left.style.left,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            right.style.left = temp + "px";
                            this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Style = {
                                width: right.style.width,
                                left: right.style.left,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
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
                            this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Style = {
                                width: left.style.width,
                                right: left.style.right,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            right.style.right = temp + "px";
                            this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Style = {
                                width: right.style.width,
                                right: right.style.right,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
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
                            this.customcolumns.filter(function (a) { return a.ColId == right.attributes['colid'].value; })[0].Style = {
                                width: right.style.width,
                                left: right.style.left,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
                                'overflow': 'hidden',
                                'white-space': 'nowrap',
                                'text-overflow': 'ellipsis',
                                'min-width': '25px'
                            };
                            left.style.left = temp + "px";
                            this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Style = {
                                width: left.style.width,
                                left: left.style.left,
                                'background-color': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Editable ? 'transparent' : 'rgba(230, 231, 232, 0.5)',
                                'text-align': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment ? this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].Alignment : 'left',
                                'display': this.customcolumns.filter(function (a) { return a.ColId == left.attributes['colid'].value; })[0].visibility == 'hidden' ? 'none' : 'inline-block',
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
    };
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
    EditableLogGridComponent.prototype.placeDiv = function (x_pos, y_pos, width) {
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
    };
    EditableLogGridComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.ReloadDetails) {
            this.ReloadDetailsSub = this.ReloadDetails.subscribe(function (res) {
                _this.cd.detectChanges();
                var grouptop = 0;
                var top = 0;
                var Bodytop = _this.HeaderHeight;
                if (_this.groupby) {
                    _this.rows.forEach(function (value, key) {
                        if (value.IsGroupOpened) {
                            value.styles = {
                                'top': top + "px",
                                'min-width': _this.TotalWidth + 22 + 'px',
                                'width:': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px',
                                'max-width:': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px',
                            };
                            value.Detailsstyles = {
                                'top': Bodytop + "px",
                                'min-width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px',
                                'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                            };
                            var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                            var DetailsDivHeight = 0;
                            var OpenedItemsHeight = 0;
                            if (DetailsDiv) {
                                DetailsDivHeight = DetailsDiv.clientHeight;
                                OpenedItemsHeight += DetailsDivHeight;
                            }
                            top += (_this.rowHeight) + DetailsDivHeight;
                            Bodytop = top + _this.HeaderHeight; //27;
                        }
                    });
                }
                else {
                    var DetailsHeight = 0;
                    _this.StaticDetailsHeight = 0;
                    _this.rowsBuffer.forEach(function (value, key) {
                        value.styles = {
                            'top': top + "px",
                            'min-width': _this.TotalWidth + 22 + 'px',
                            'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                        };
                        value.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': _this.TotalWidth + 22 + 'px',
                            'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                        };
                        var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                        var DetailsDivHeight = 0;
                        var OpenedItemsHeight = 0;
                        if (DetailsDiv) {
                            DetailsDivHeight = DetailsDiv.clientHeight;
                            OpenedItemsHeight += DetailsDivHeight;
                        }
                        top += (_this.rowHeight) + DetailsDivHeight;
                        DetailsHeight += DetailsDivHeight;
                        _this.StaticDetailsHeight += DetailsDivHeight;
                        Bodytop = top + _this.HeaderHeight; //27;
                    });
                    _this.canvasHeight = {
                        //'top': this.HeaderHeight + 'px',
                        height: (_this.rowCount * _this.rowHeight) + DetailsHeight + 'px',
                        'width': '100%',
                        'min-width': _this.TotalWidth + 'px' //(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                    };
                    _this.BodyTop = _this.HeaderHeight;
                    _this.updateDisplayList();
                }
            });
        }
        if (this.ReRenderGrid) {
            this.ReRenderGridSub = this.ReRenderGrid.subscribe(function (res) {
                _this.cd.detectChanges();
                var index = 0;
                var left = 0;
                if (_this.groupby) {
                    left = 15;
                }
                if (_this.RowDetail) {
                    left = 25;
                }
                if (_this.RowDetail && _this.groupby) {
                    left = 40;
                }
                _this.TotalWidth = 0;
                //var i = 0;
                _this.customcolumns = [];
                //var temp = this.cols.changes;
                _this.cols.toArray().forEach(function (value, key) {
                    ////HeaderTemplateDiv
                    ////HeaderHeight
                    //var temp = document.getElementById('HeaderTemplateDiv' + i);
                    //var cHeight = temp.clientHeight;
                    //if (cHeight > this.HeaderHeight) {
                    //    this.HeaderHeight = cHeight;
                    //}
                    if (value.hastemplate) {
                        value.index = index;
                        index++;
                    }
                    if (value.Editable == undefined) {
                        value.Editable = true;
                    }
                    //if (value.required == undefined) {
                    //    value.required = false;
                    //}
                    value.Style = {
                        width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
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
                        'height': _this.HeaderHeight - 1 + 'px',
                        'line-height': _this.HeaderHeight <= 27 ? '26px' : 'normal',
                        width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                    };
                    if (value.visibility != 'hidden') {
                        _this.TotalWidth += +(value.width);
                        value.width = value.width + 'px';
                        left += (+(value.width.replace("px", "")));
                    }
                    if (value.AllowDisableCells) {
                        _this.AllowDisableCells = true;
                    }
                    _this.customcolumns.push(value);
                });
            });
        }
        if (this.ChangeScrollPosition) {
            this.ChangeScrollPosition.subscribe(function (res) {
                var RowsElement = document.getElementById(_this.LogGridRowsId);
                if (RowsElement) {
                    RowsElement.scrollTop = _this.rowHeight * res.RowIndex;
                }
            });
        }
        this.EndOfRowReachedSub = this.CurrentSession.EndOfRowReachedEvent.subscribe(function (res) {
            if (_this.LogGridId == _this.CurrentSession.CurrentLogGrid) {
                _this.RowEnded.emit(res);
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
        //if (this.ItemSource) {
        //    if (this.groupby) {
        //        var group = new GroupByPipe().transform(this.ItemSource, this.groupby);
        //        group.forEach((value, key) => {
        //            var row: IRow = { rowIndex: i, rowData: value.key, DetailsIcon: "-", IsDetailesOpened: true, Type: "Head" };
        //            row.styles = {
        //                'top': (row.rowIndex * this.rowHeight) + "px",
        //                'min-width': '100%',
        //                'width': '100%'
        //            };
        //            row.Groupstyles = {
        //                'top': (row.rowIndex * this.rowHeight) + "px",
        //                'min-width': '100%',
        //                'width': '100%'
        //            };
        //            this.rows.push(row);
        //            i++;
        //            value.value.forEach((item, key) => {
        //                var row: IRow = { rowIndex: i, rowData: item, DetailsIcon: "+", IsDetailesOpened: false, Type: "Body" };
        //                row.styles = {
        //                    'top': (row.rowIndex * this.rowHeight) + "px",
        //                    'min-width': '100%',
        //                    'width': '100%'
        //                };
        //                row.Groupstyles = {
        //                    'top': (row.rowIndex * this.rowHeight) + "px",
        //                    'min-width': '100%',
        //                    'width': '100%'
        //                };
        //                this.rows.push(row);
        //                i++;
        //            });
        //        });
        //    }
        //    else {
        //        this.ItemSource.forEach((value, key) => {
        //            //var rowData = value;
        //            var row: IRow = { rowIndex: i, rowData: value, DetailsIcon: "+", IsDetailesOpened: false, Type: "Body" };
        //            row.styles = {
        //                'top': (row.rowIndex * this.rowHeight) + (this.groupby ? 27 : 0) + "px",
        //                'min-width': '100%',
        //                'width': '100%'
        //            };
        //            row.Groupstyles = {
        //                'top': (row.rowIndex * this.rowHeight) + "px",
        //                'min-width': '100%',
        //                'width': '100%'
        //            };
        //            this.rows.push(row);
        //            i++;
        //        });
        //    }
        //}
        i = 0;
        this.FinishLoadingSub = this.FinishLoading.subscribe(function (res) {
            var grouptop = 0;
            var top = 0;
            var Bodytop = _this.HeaderHeight;
            if (_this.groupby) {
                _this.rows.forEach(function (value, key) {
                    if (value.IsGroupOpened) {
                        value.styles = {
                            'top': top + "px",
                            'min-width': _this.TotalWidth + 22 + 'px',
                            'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px',
                            'max-width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth - 18 : _this.ViewWidth - 18) + 'px',
                        };
                        value.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': _this.TotalWidth + 'px',
                            'width': '100%'
                            //'min-width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px',
                            //'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                        };
                        var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                        var DetailsDivHeight = 0;
                        var OpenedItemsHeight = 0;
                        if (DetailsDiv) {
                            DetailsDivHeight = DetailsDiv.clientHeight; // != 0 ? DetailsDiv.clientHeight : 100;
                            OpenedItemsHeight += DetailsDivHeight;
                        }
                        top += (_this.rowHeight) + DetailsDivHeight;
                        Bodytop = top + _this.HeaderHeight; //27;
                    }
                });
            }
            else {
                var DetailsHeight = 0;
                _this.StaticDetailsHeight = 0;
                _this.rows.forEach(function (value, key) {
                    value.styles = {
                        'top': top + "px",
                        'min-width': _this.TotalWidth + 22 + 'px',
                        'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                    };
                    value.Detailsstyles = {
                        'top': Bodytop + "px",
                        'min-width': _this.TotalWidth + 22 + 'px',
                        'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                    };
                    var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                    var DetailsDivHeight = 0;
                    var OpenedItemsHeight = 0;
                    if (DetailsDiv) {
                        DetailsDivHeight = DetailsDiv.clientHeight;
                        OpenedItemsHeight += DetailsDivHeight;
                    }
                    top += (_this.rowHeight) + DetailsDivHeight;
                    DetailsHeight += DetailsDivHeight;
                    _this.StaticDetailsHeight += DetailsDivHeight;
                    Bodytop = top + _this.HeaderHeight; //27;
                });
                //if (this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex).length > 0) {
                //    this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex)[0] = value;
                //} 
                _this.canvasHeight = {
                    //'top': this.HeaderHeight + 'px',
                    height: (_this.rowCount * _this.rowHeight) + DetailsHeight + 'px',
                    'width': '100%',
                    'min-width': _this.TotalWidth + 'px' //(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
                };
                _this.BodyTop = _this.HeaderHeight;
                //this.updateDisplayList();
            }
        });
        this.FinishLoadingGroupsSub = this.FinishLoadingGroups.subscribe(function (res) {
            var grouptop = 0;
            var top = 0;
            var height = 0;
            var Bodytop = _this.HeaderHeight;
            _this.rows.forEach(function (value, key) {
                if (value.IsGroupOpened) {
                    value.styles = {
                        'top': top + "px",
                        'min-width': _this.TotalWidth + 22 + 'px',
                        'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                    };
                    value.Detailsstyles = {
                        'top': Bodytop + "px",
                        'min-width': _this.TotalWidth + 22 + 'px',
                        'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
                    };
                    var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
                    var DetailsDivHeight = 0;
                    var OpenedItemsHeight = 0;
                    if (DetailsDiv) {
                        DetailsDivHeight = DetailsDiv.clientHeight;
                        OpenedItemsHeight += DetailsDivHeight;
                    }
                    top += (_this.rowHeight) + DetailsDivHeight;
                    Bodytop = top + _this.HeaderHeight; //27;
                    height = height + _this.rowHeight;
                }
            });
            //this.canvasHeight = {
            //    height: height + 'px'
            //};
            _this.canvasHeight = {
                //'top': this.HeaderHeight + 'px',
                height: _this.rowCount * _this.rowHeight + 'px',
                'width': '100%',
                'min-width': _this.TotalWidth + 'px' //(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            };
            _this.BodyTop = _this.HeaderHeight;
        });
    };
    EditableLogGridComponent.prototype.OnPlusClick = function (row) {
        var _this = this;
        this.rowsBuffer.forEach(function (value, key) {
            if (value == row) {
                value.IsDetailesOpened = !value.IsDetailesOpened;
                if (!value.IsDetailesOpened) {
                    _this.ShowCollapseAllIcon = false;
                    value.DetailsIcon = "./Images/CustomTreeIcon.png";
                }
                else {
                    _this.ShowCollapseAllIcon = true;
                    value.DetailsIcon = "./Images/CustomtreeIcon2.png";
                    //value.Detailsstyles = this.getRowDetailsStylesbyIndex(value.rowIndex);
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
    };
    EditableLogGridComponent.prototype.OnGroupClick = function (row) {
        var KeepGoing = true;
        this.rows.forEach(function (value, key) {
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
                //this.cd.detectChanges();
            }
        });
        this.cd.detectChanges();
        this.FinishLoadingGroups.emit(row);
    };
    EditableLogGridComponent.prototype.OnCollapseAllClick = function () {
        var _this = this;
        this.ShowCollapseAllIcon = false;
        this.rowsBuffer.forEach(function (value, key) {
            if (value.IsExpandable) {
                value.IsDetailesOpened = false;
                value.DetailsIcon = "./Images/CustomTreeIcon.png";
                _this.cd.detectChanges();
                if (_this.UseVirtuallization == false) {
                    _this.FinishLoading.emit(value);
                }
                else {
                    _this.updateDisplayList();
                }
            }
        });
    };
    EditableLogGridComponent.prototype.OnExpandAllClick = function () {
        var _this = this;
        this.ShowCollapseAllIcon = true;
        this.rowsBuffer.forEach(function (value, key) {
            if (value.IsExpandable) {
                value.IsDetailesOpened = true;
                value.DetailsIcon = "./Images/CustomtreeIcon2.png";
                _this.cd.detectChanges();
                if (_this.UseVirtuallization == false) {
                    _this.FinishLoading.emit(value);
                }
                else {
                    _this.updateDisplayList();
                }
            }
        });
    };
    EditableLogGridComponent.prototype.init = function () {
        this.RowClass = "Row ag-row";
        this.rowsBuffer = [];
        if (this.ViewHeight == null || this.ViewWidth == null || (this.ViewHeight <= 0 && this.ViewWidth <= 0)) {
            var RowsElement = document.getElementById(this.LogGridRowsId);
            this.ViewHeight = RowsElement.clientHeight;
            this.ViewWidth = RowsElement.clientWidth;
        }
        this.headerStyle = {
            //'width': (this.ViewWidth) + 'px',
            //'min-width': (this.ViewWidth) + 'px'
            'height': this.HeaderHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px',
        };
        //this.canvasHeight = {
        //    height: this.rowCount * this.rowHeight + 'px',
        //    'width': (this.ViewWidth) + 'px',
        //    'min-width': (this.ViewWidth) + 'px'
        //};
        this.rowStyle.minWidth = this.ViewWidth + 'px';
        this.viewportSize = Math.round(this.ViewHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
        this.rowsPerPage = 10;
        this.rows = [];
        this.cd.detectChanges();
        if (this.groupby) {
            this.group = new GroupByPipe_1.GroupByPipe().ShapeGrouping(this.ItemSource.Collection, this.groupby);
            this.rowCount = this.group.length;
        }
        else {
            this.rowCount = this.ItemSource.Length;
        }
        //this.canvasHeight = {
        //    height: this.rowCount * this.rowHeight + 'px'
        //};
        this.canvasHeight = {
            //'top': this.HeaderHeight + 'px',
            height: this.rowCount * this.rowHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px' ////(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
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
        //this.cols.toArray().forEach((value, key) => {
        //    var temp = document.getElementById('HeaderTemplateDiv' + i);
        //    var cHeight = temp.clientHeight;
        //    if (cHeight > this.HeaderHeight) {
        //        this.HeaderHeight = cHeight;
        //    }
        //});
    };
    ;
    //HeaderHeight: number = 27;
    EditableLogGridComponent.prototype.SetSelectedRows = function () {
        var _this = this;
        if (this.SelectedRows.length > 0) {
            this.SelectedRows.forEach(function (value, key) {
                if (_this.rows.filter(function (a) { return a.rowData == value; }).length > 0) {
                    _this.rows.filter(function (a) { return a.rowData == value; })[0].IsSelected = true;
                }
            });
            this.cd.detectChanges();
        }
    };
    EditableLogGridComponent.prototype.ngAfterContentInit = function () {
        var _this = this;
        // get all active tabs
        //var ss = this.cols;
        //var xx = this.contentTpl;   
        var ss = this.cols;
        var sss = this.RowDetail;
        if (this.RowDetail) {
            this.DetailButtonVisibile = true;
        }
        this.customcolumns = [];
        //this.contentTpl.toArray().forEach((value, key) => {
        //    let hostComponent = this._viewManager.getComponent(value.elementref);
        //});
        /*
          if (this.columns && this.columns.length > 0) {
            var index = 0
            var left = 0;
            this.columns.forEach((value, key) => {

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
                value.Styles = {
                    width: + (value.Styles.width.replace("px", "")) + 'px',
                    left: left + 'px',
                };
                left += (+(value.Styles.width.replace("px", "")));
                //this.customcolumns.push(value);
            });
            this.cd.detectChanges();
        }
         */
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
        this.cols.changes.subscribe(function (changes) {
            var index = 0;
            var left = 0;
            if (_this.groupby) {
                left = 15;
            }
            if (_this.RowDetail) {
                left = 25;
            }
            if (_this.RowDetail && _this.groupby) {
                left = 40;
            }
            _this.TotalWidth = 0;
            //var i = 0;
            _this.customcolumns = [];
            //var temp = this.cols.changes;
            changes.toArray().forEach(function (value, key) {
                if (value.hasFootertemplate == true) {
                    _this.ShowFooter = true;
                }
                ////HeaderTemplateDiv
                ////HeaderHeight
                //var temp = document.getElementById('HeaderTemplateDiv' + i);
                //var cHeight = temp.clientHeight;
                //if (cHeight > this.HeaderHeight) {
                //    this.HeaderHeight = cHeight;
                //}
                if (value.hastemplate) {
                    value.index = index;
                    index++;
                }
                if (value.Editable == undefined) {
                    value.Editable = true;
                }
                //if (value.required == undefined) {
                //    value.required = false;
                //}
                if (_this.RTL) {
                    value.Style = {
                        width: +(value.visibility == 'hidden' ? 0 : value.width.replace("px", "")) + 'px',
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
                        'height': _this.HeaderHeight - 1 + 'px',
                        'line-height': _this.HeaderHeight <= 27 ? '26px' : 'normal',
                        width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        right: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                    };
                }
                else {
                    value.Style = {
                        width: +(value.visibility == 'hidden' ? 0 : value.width.replace("px", "")) + 'px',
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
                        'height': _this.HeaderHeight - 1 + 'px',
                        'line-height': _this.HeaderHeight <= 27 ? '26px' : 'normal',
                        width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
                        left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                        'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                        'text-align': value.Alignment ? value.Alignment : 'left',
                    };
                }
                if (value.visibility != 'hidden') {
                    _this.TotalWidth += +(value.width.replace("px", ""));
                    value.width = value.width;
                    left += (+(value.width.replace("px", "")));
                }
                if (value.AllowDisableCells) {
                    _this.AllowDisableCells = true;
                }
                _this.customcolumns.push(value);
            });
            _this.init();
        });
        this.cols.toArray().forEach(function (value, key) {
            if (value.hasFootertemplate == true) {
                _this.ShowFooter = true;
            }
            value.LogGridId = _this.LogGridId;
            value.EditableLogGridComponent = _this;
            if (value.IgnoreColumn == false) {
                _this.CurrentSession.LogitudeGridHelper.SetColumnsCount(false, _this.LogGridId);
            }
            value.IsReadOnlyGrid = _this.IsReadOnly;
            ////HeaderTemplateDiv
            ////HeaderHeight
            //var temp = document.getElementById('HeaderTemplateDiv' + i);
            //var cHeight = temp.clientHeight;
            //if (cHeight > this.HeaderHeight) {
            //    this.HeaderHeight = cHeight;
            //}
            if (value.hastemplate) {
                value.index = index;
                index++;
            }
            if (value.Editable == undefined) {
                value.Editable = true;
            }
            //if (value.required == undefined) {
            //    value.required = false;
            //}
            if (_this.RTL) {
                value.Style = {
                    width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
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
                    'height': _this.HeaderHeight - 1 + 'px',
                    'line-height': _this.HeaderHeight <= 27 ? '26px' : 'normal',
                    width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
                    right: (value.visibility == 'hidden' ? 0 : left) + 'px',
                    'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                    'text-align': value.Alignment ? value.Alignment : 'left',
                };
            }
            else {
                value.Style = {
                    width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
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
                    'height': _this.HeaderHeight - 1 + 'px',
                    'line-height': _this.HeaderHeight <= 27 ? '26px' : 'normal',
                    width: +(value.visibility == 'hidden' ? 0 : value.width) + 'px',
                    left: (value.visibility == 'hidden' ? 0 : left) + 'px',
                    'display': value.visibility == 'hidden' ? 'none' : 'inline-block',
                    'text-align': value.Alignment ? value.Alignment : 'left',
                };
            }
            if (value.visibility != 'hidden') {
                _this.TotalWidth += +(value.width);
                value.width = value.width + 'px';
                left += (+(value.width.replace("px", "")));
            }
            if (value.AllowDisableCells) {
                _this.AllowDisableCells = true;
            }
            _this.customcolumns.push(value);
        });
        //console.log(this.DisableRowByFieldValue);
        //console.log(this.DisableRowByFieldName);
    };
    EditableLogGridComponent.prototype.ngAfterViewInit = function () {
        //var ss = this.cellChildren;
        var RowsElement = document.getElementById(this.LogGridRowsId);
        var RowsToLoad = RowsElement.clientHeight / this.rowHeight;
        var elem = document.getElementById(this.LogGridId);
        this.LogGridElement = elem;
        this.scrollPosition = RowsElement.scrollTop;
        var vscroll = elem.scrollTop;
        var hscroll = elem.scrollLeft;
        this.rowCount = this.ItemSource.Length;
        //if (this.dataSource) {
        this.init();
        //}
    };
    EditableLogGridComponent.prototype.handleonclick = function (evt, rownum) {
        var _this = this;
        //var id = "";
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
            this.customcolumns.forEach(function (item, key) {
                if (keepGoing == true) {
                    if (item.binding != undefined && item.binding != '') {
                        colindex++;
                        if (item.required && (_this.dataSource[_this.currentrow][item.binding] == undefined || _this.dataSource[_this.currentrow][item.binding] == "")) {
                            _this.allowtomove = false;
                            var id = "row" + _this.currentrow + "col" + colindex;
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
    };
    EditableLogGridComponent.prototype.keyupHandler = function (arg, evt, rownum) {
        if (arg.keyCode == 9) {
            this.handleonclick(evt, rownum);
        }
    };
    EditableLogGridComponent.prototype.handleblur = function (evt, rownum) {
        return;
        //if (!this.InClick) {
        //    if (this.currentrow == undefined) {
        //        this.currentrow = rownum;
        //    }
        //    //if (rownum == this.currentrow) {
        //    //    if (evt != undefined) {
        //    //        var element = document.getElementById(evt);
        //    //        element.style.border = "none";
        //    //    }
        //    //    this.onparentclickEvent.emit(evt);
        //    //}
        //    //else {
        //        var allowtomove = true;
        //        var keepGoing = true;
        //        var colindex = 0;
        //        this.columns.forEach((item, key) => {
        //            if (keepGoing == true) {
        //                if (item.FieldName != undefined && item.FieldName != '') {
        //                    colindex++;
        //                    if (item.required && (this.dataSource[this.currentrow][item.FieldName] == undefined || this.dataSource[this.currentrow][item.FieldName] == "")) {
        //                        allowtomove = false;
        //                        this.id = "row" + this.currentrow + "col" + colindex;
        //                        var element = document.getElementById(this.id);
        //                        element.focus();
        //                        element.style.border = "thin dotted red";
        //                        keepGoing = false;
        //                        this.DontEnter = true;
        //                    }
        //                }
        //            }
        //        });
        //        if (allowtomove) {
        //            this.currentrow = rownum;
        //            if (this.id != undefined) {
        //                var element = document.getElementById(this.id);
        //                element.style.border = "none";
        //            }
        //            //this.onparentclickEvent.emit(evt);
        //        }
        //    //}
        ////}
        ////else {
        ////    this.InClick = false;
        ////}
    };
    EditableLogGridComponent.prototype.handlerowfocus = function (rownum) {
        this.currentrow = rownum;
    };
    EditableLogGridComponent.prototype.setselectedstyle = function () {
        this.RowClass = "ag-selected-row SelectedRow";
    };
    EditableLogGridComponent.prototype.CompareFunction = function (a, b) {
        if (a < b)
            return -1;
        if (a > b)
            return 1;
        return 0;
    };
    EditableLogGridComponent.prototype.ServerSort = function (colDef, i, LogGridId) {
        if (colDef.SortFieldName) {
            this.OrigionalSortingData = this.ItemSource.Collection;
            if (colDef.SortFieldName != this.sortingCol) {
                this.sortingDir = 'Ascending';
                this.sortingCol = colDef.SortFieldName;
            }
            //this.dataSource.sortingCol = colDef.FieldName;
            switch (this.sortingDir) {
                case 'Ascending':
                    {
                        //this.dataSource.sortingDir = "Descending";
                        this.sortingDir = "Descending";
                        var temp = this.ItemSource.Collection.sort(function (a, b) { return (a[colDef.SortFieldName] < b[colDef.SortFieldName]) ? -1 : ((a[colDef.SortFieldName] > b[colDef.SortFieldName]) ? 1 : 0); });
                        this.ItemSource = new ObservableCollection_1.ObservableCollection(temp);
                        break;
                    }
                case "Descending":
                    {
                        //this.dataSource.sortingDir = "Ascending";
                        this.sortingDir = "Ascending";
                        var temp = this.ItemSource.Collection.sort(function (a, b) { return (a[colDef.SortFieldName] < b[colDef.SortFieldName]) ? 1 : ((a[colDef.SortFieldName] > b[colDef.SortFieldName]) ? -1 : 0); });
                        this.ItemSource = new ObservableCollection_1.ObservableCollection(temp);
                        break;
                    }
                //case "Ascending":
                //    {
                //        //this.dataSource.sortingDir = '';
                //        //this.ItemSource = new ObservableCollection(this.OrigionalSortingData);
                //        this.sortingDir = '';
                //        break;
                //    }
                default:
            }
            var allelems = document.getElementsByClassName("ag-header-cell");
            for (var a = 0; a < allelems.length; a++) {
                if (allelems.item(a).attributes['LogGridId'].value == this.LogGridId) {
                    allelems.item(a).style.background = 'transparent';
                }
            }
            if (this.sortingDir === 'Descending' || this.sortingDir === 'Ascending') {
                var ColumnsElements = document.getElementsByClassName("ag-header-cell");
                for (var j = 0; j < ColumnsElements.length; j++) {
                    if (ColumnsElements[j].attributes['LogGridId'].value == this.LogGridId) {
                        if (ColumnsElements[j].attributes['FieldName'] && ColumnsElements[j].attributes['FieldName'].value == colDef.SortFieldName) {
                            //(<HTMLElement>ColumnsElements[j]).style.color = 'rgb(103, 103, 103)';
                            ColumnsElements[j].style.background = '#cfcbcb';
                        }
                    }
                    //ColIndexes.push({ FieldName: ColumnsElements[i].attributes['colid'].value, Index: +(ColumnsElements[i].id.split(',')[1]), Width: ColumnsElements[i].clientWidth });
                }
                //var chosen = document.getElementById(id);//this.ColumnId + "resizable-column-," + this.ColIndex); 
                //chosen.style.color = 'rgb(103, 103, 103)';
                //chosen.style.background = '#cfcbcb';
            }
            //var allelems = document.getElementsByClassName("ag-header-cell");
            //for (var a = 0; a < allelems.length; a++) {
            //    (<HTMLScriptElement>allelems.item(a)).style.color = 'white';
            //    (<HTMLScriptElement>allelems.item(a)).style.background = '-moz-linear-gradient(50% 100% 90deg,rgba(112, 112, 112, 1) 0%,rgba(168, 168, 168, 1) 100%)';
            //    (<HTMLScriptElement>allelems.item(a)).style.background = '-webkit-linear-gradient(90deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
            //    (<HTMLScriptElement>allelems.item(a)).style.background = '-webkit-gradient(linear,50% 100%,50% 0%,color-stop(0,rgba(112, 112, 112, 1) ),color-stop(1,rgba(168, 168, 168, 1) ))';
            //    (<HTMLScriptElement>allelems.item(a)).style.background = '-o-linear-gradient(90deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
            //    (<HTMLScriptElement>allelems.item(a)).style.background = 'linear-gradient(0deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
            //}
            //if (this.sortingDir === 'Descending' || this.sortingDir === 'Ascending') {
            //    var chosen = document.getElementById('resizable-column-' + i);
            //    chosen.style.color = 'rgb(103, 103, 103)';
            //    chosen.style.background = '#cfcbcb';
            //}
            this.rows = [];
            this.updateDisplayList(true);
        }
    };
    EditableLogGridComponent.prototype.cellClicked = function (field, value, rowIndex, colIndex) {
    };
    EditableLogGridComponent.prototype.rowClicked = function (rowIndex, row) {
    };
    EditableLogGridComponent.prototype.updateDisplayList = function (reload) {
        var _this = this;
        if (reload === void 0) { reload = false; }
        if (this.UseVirtuallization == false) {
            this.updateDisplayListGrouping(reload);
        }
        else {
            this.rows = [];
            if (reload) {
                this.rowsBuffer = [];
            }
            //if (!this.rowsBuffer) {
            //    this.rowsBuffer = [];
            //}
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
                    this.ViewPortRowCount = (this.viewportSize - this.HeaderHeight) > 0 ? (this.viewportSize - this.HeaderHeight) : 30; // + this.extraRows;
                }
                var top = 0;
                var Bodytop = this.HeaderHeight;
                var DetailsHeight = 0;
                this.StaticDetailsHeight = 0;
                var OpenedItemsHeight = 0;
                var DetailsDivHeight = 0;
                this.group = this.group.sort(function (a, b) { return (a["Index"] < b["Index"]) ? -1 : ((a["Index"] > b["Index"]) ? 1 : 0); });
                for (var i = firstRow; i < firstRow + this.ViewPortRowCount; i++) {
                    if (i >= this.rowCount) {
                        //this.rowsBuffer.forEach((item) => {
                        //    var existingItem = this.rows.filter(d => d.rowIndex === item.rowIndex)[0];
                        //    if (!existingItem) {
                        //        this.rows.push(item);
                        //    }
                        //});
                        var isDestroyed = this.cd['destroyed'];
                        if (!isDestroyed) {
                            this.cd.detectChanges();
                        }
                        //this.RenderLoadedRows();
                        return;
                    }
                    top = (this.group[i].Index * this.rowHeight) + DetailsDivHeight;
                    Bodytop = top + this.rowHeight;
                    var buffered = this.rowsBuffer.filter(function (a) { return a && (a.rowIndex == _this.group[i].Index); })[0];
                    if (buffered) {
                        buffered.styles = {
                            'top': top + "px",
                            'min-width': this.TotalWidth + 22 + 'px',
                            'width': '100%'
                        };
                        buffered.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': this.TotalWidth + 22 + 'px',
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
                        var row = { rowIndex: this.group[i].Index, rowData: this.group[i].Data, DetailsIcon: (this.group[i].Type == "Head" ? "./Images/CellIcons/Arrowup.png" : "./Images/CustomTreeIcon.png"), IsDetailesOpened: (this.group[i].Type == "Head" ? true : false), Type: this.group[i].Type, IsGroupOpened: true, ChildrenFirstIndex: this.group[i].ChildrenFirstIndex, IsExpandable: false, SetExpandaple: function (isExpandable) { this.IsExpandable = isExpandable; }, ChildrensCount: this.group[i].Count, IsSelected: false };
                        row.rowData.rowIndex = (row.rowIndex);
                        row.styles = {
                            'top': top + "px",
                            'min-width': this.TotalWidth + 22 + 'px',
                            'width': '100%'
                        };
                        row.Detailsstyles = {
                            'top': Bodytop + "px",
                            'min-width': this.TotalWidth + 22 + 'px',
                            'width': '100%'
                        };
                        //row.styles = {
                        //    'top': (row.rowIndex * this.rowHeight) + "px",
                        //    'min-width': this.TotalWidth + 22 + 'px',
                        //    'width': '100%'
                        //};
                        //row.Detailsstyles = this.getRowDetailsStyles();
                        //    {
                        //    'top': (row.rowIndex * this.rowHeight) + "px",
                        //    'min-width': this.TotalWidth + 22 + 'px',
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
                var isDestroyed = this.cd['destroyed'];
                if (!isDestroyed) {
                    this.cd.detectChanges();
                }
            }
        }
    };
    ;
    EditableLogGridComponent.prototype.GetDetailsHeight = function () {
        var _this = this;
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        var StaticDetailsHeight = 0;
        this.rowsBuffer.forEach(function (value, key) {
            var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }
            StaticDetailsHeight += DetailsDivHeight;
        });
        return StaticDetailsHeight;
    };
    EditableLogGridComponent.prototype.GetPrevDetailsHeight = function (Index) {
        var _this = this;
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        var StaticDetailsHeight = 0;
        //for (var i = firstRow; i < firstRow + this.ViewPortRowCount; i++) {
        //}
        this.rowsBuffer.forEach(function (value, key) {
            var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
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
    };
    EditableLogGridComponent.prototype.RenderLoadedRows = function () {
        var _this = this;
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        this.StaticDetailsHeight = 0;
        this.rowsBuffer.forEach(function (value, key) {
            value.styles = {
                'top': top + "px",
                'min-width': _this.TotalWidth + 22 + 'px',
                'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
            };
            value.Detailsstyles = {
                'top': Bodytop + "px",
                'min-width': _this.TotalWidth + 22 + 'px',
                'width': (_this.TotalWidth > _this.ViewWidth ? _this.TotalWidth : _this.ViewWidth) + 'px'
            };
            var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }
            top += (_this.rowHeight) + DetailsDivHeight;
            DetailsHeight += DetailsDivHeight;
            _this.StaticDetailsHeight += DetailsDivHeight;
            Bodytop = top + _this.HeaderHeight; //27;
        });
        //if (this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex).length > 0) {
        //    this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex)[0] = value;
        //} 
        this.canvasHeight = {
            //'top': this.HeaderHeight + 'px',
            height: (this.rowCount * this.rowHeight) + DetailsHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px' //(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
        };
        this.BodyTop = this.HeaderHeight;
    };
    EditableLogGridComponent.prototype.getRowStyles = function () {
        var _this = this;
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        this.StaticDetailsHeight = 0;
        this.rows.forEach(function (value, key) {
            //value.styles = {
            //    'top': top + "px",
            //    'min-width': this.TotalWidth + 22 + 'px',
            //    'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            //};
            //value.Detailsstyles = {
            //    'top': Bodytop + "px",
            //    'min-width': this.TotalWidth + 22 + 'px',
            //    'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            //};
            var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }
            top += (_this.rowHeight) + DetailsDivHeight;
            DetailsHeight += DetailsDivHeight;
            _this.StaticDetailsHeight += DetailsDivHeight;
            Bodytop = top + _this.HeaderHeight; //27;
        });
        //if (this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex).length > 0) {
        //    this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex)[0] = value;
        //} 
        this.BodyTop = this.HeaderHeight;
        var styles = {
            'top': top + "px",
            'min-width': this.TotalWidth + 22 + 'px',
            'width': '100%'
        };
        return DetailsHeight;
        //this.updateDisplayList();
    };
    EditableLogGridComponent.prototype.getRowDetailsStyles = function () {
        var _this = this;
        var top = 0;
        var Bodytop = this.HeaderHeight;
        var DetailsHeight = 0;
        this.StaticDetailsHeight = 0;
        this.rows.forEach(function (value, key) {
            //value.styles = {
            //    'top': top + "px",
            //    'min-width': this.TotalWidth + 22 + 'px',
            //    'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            //};
            //value.Detailsstyles = {
            //    'top': Bodytop + "px",
            //    'min-width': this.TotalWidth + 22 + 'px',
            //    'width': (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
            //};
            var DetailsDiv = document.getElementById(_this.DetailsDivId + 'DetailsDiv' + (value.rowIndex));
            var DetailsDivHeight = 0;
            var OpenedItemsHeight = 0;
            if (DetailsDiv) {
                DetailsDivHeight = DetailsDiv.clientHeight;
                OpenedItemsHeight += DetailsDivHeight;
            }
            top += (_this.rowHeight) + DetailsDivHeight;
            DetailsHeight += DetailsDivHeight;
            _this.StaticDetailsHeight += DetailsDivHeight;
            Bodytop = top + _this.HeaderHeight; //27;
        });
        //if (this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex).length > 0) {
        //    this.rowsBuffer.filter(a => a.rowIndex == value.rowIndex)[0] = value;
        //} 
        this.BodyTop = this.HeaderHeight;
        var styles = {
            'top': Bodytop + "px",
            'min-width': this.TotalWidth + 22 + 'px',
            'width': '100%'
        };
        return styles;
        //this.updateDisplayList();
    };
    EditableLogGridComponent.prototype.getRowDetailsStylesbyIndex = function (index) {
        var styles = {
            'top': (index * this.rowHeight) + this.rowHeight + "px",
            'min-width': this.TotalWidth + 22 + 'px',
            'width': '100%'
        };
        return styles;
        //this.updateDisplayList();
    };
    EditableLogGridComponent.prototype.updateDisplayListGrouping = function (reload) {
        var _this = this;
        if (reload === void 0) { reload = false; }
        if (reload) {
            this.rowsBuffer = [];
        }
        if (!this.rowsBuffer) {
            this.rowsBuffer = [];
        }
        //var oldRow = this.firstRow;
        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        if (firstRow < this.oldRow) {
            firstRow = this.oldRow + 1;
        }
        //var oldPage; // = 0;
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
            //for (var i = firstRow; i < firstRow + this.ViewPortRowCount; i++) {
            this.group.forEach(function (value, key) {
                if (value.Index >= _this.rowCount) {
                    _this.cd.detectChanges();
                    return;
                }
                var buffered = _this.rowsBuffer.filter(function (a) { return a.rowIndex === value.Index; })[0];
                if (!buffered) {
                    var row = { rowIndex: value.Index, rowData: value.Data, DetailsIcon: (value.Type == "Head" ? "./Images/CellIcons/Arrowup.png" : "./Images/CustomTreeIcon.png"), IsDetailesOpened: (value.Type == "Head" ? true : false), Type: value.Type, IsGroupOpened: true, ChildrenFirstIndex: value.ChildrenFirstIndex, IsExpandable: false, SetExpandaple: function (isExpandable) { this.IsExpandable = isExpandable; }, ChildrensCount: value.Count, IsSelected: false };
                    /*
               row.styles = {
                   'top': (value.RowIndex * this.rowHeight) + "px",
                   'min-width': this.TotalWidth + 22 + 'px',
                   'width:': this.TotalWidth + 'px',
                   'max-width:': this.TotalWidth + 'px',
               };
               */
                    row.styles = {
                        'top': (row.rowIndex * _this.rowHeight) + "px",
                        'min-width': _this.TotalWidth + 22 + 'px',
                        'width': '100%'
                    };
                    _this.rowsBuffer.push(row); //this.rows.push(row);
                    _this.RowDataLoaded.emit(row);
                }
                //} 
            });
            this.rowsBuffer.forEach(function (item) {
                var existingItem = _this.rows.filter(function (d) { return d.rowIndex === item.rowIndex; })[0];
                if (!existingItem) {
                    _this.rows.push(item);
                }
            });
            //this.rowsBuffer = [];
            var isDestroyed = this.cd['destroyed'];
            if (!isDestroyed) {
                this.cd.detectChanges();
            }
            //this.cd.detectChanges();
            this.oldPage = currentPage;
        }
        //else if (this.rows.length < this.group.length) {
        //    alert(this.rows.length);
        //}
    };
    ;
    EditableLogGridComponent.prototype.onScroll = function () {
        var columns = document.getElementById(this.LogGridColumnsId);
        var elem = document.getElementById(this.LogGridRowsId);
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
            //this.updateDisplayList();
            //if (this.groupby) {
            //this.updateDisplayListGrouping();
            //}
            //else {
            this.updateDisplayList();
            //} 
        }
    };
    ;
    EditableLogGridComponent.prototype.onWindowResized = function (event) {
        var _this = this;
        //alert(this.LogGridId);
        var logGrid = document.getElementById(this.LogGridId);
        if (logGrid != null) {
            logGrid.style.display = "none";
            logGrid.style.display = "block";
            this.ViewHeight = logGrid.clientHeight;
            this.ViewWidth = logGrid.clientWidth;
            //console.log("ViewHeight", this.ViewHeight, "ViewWidth", this.ViewWidth);
        }
        //logGrid.style.display = "none";
        //logGrid.style.display = "block";
        //this.ViewHeight = logGrid.clientHeight;
        //this.ViewWidth = logGrid.clientWidth;
        //console.log("ViewHeight", this.ViewHeight, "ViewWidth", this.ViewWidth);
        //if (this.ViewHeight == 0 && this.ViewWidth == 0) {
        //    return;
        //}
        var rowsArray = document.getElementsByClassName("ag-row");
        for (var i = 0; i < rowsArray.length; i++) {
            //rowsArray.item(i).style.width = (this.ViewWidth - 20) + 'px';
            rowsArray.item(i).style.minWidth = (this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px';
        }
        this.rowStyle.minWidth = this.ViewWidth + 'px';
        this.viewportSize = Math.round(this.ViewHeight / this.rowHeight);
        this.tripleViewport = this.viewportSize * 3;
        //this.rowsPerPage = this.dataSource.pageSize;
        //var header = document.getElementById(this.LogGridId);
        //header.style.width = this.ViewWidth + 'px';
        //header.style.width = '1920px';
        //header.style.minWidth = this.ViewWidth + 'px';
        //header.style.minWidth = '1920px';
        //this.headerStyle = {
        //    'width': (this.ViewWidth - 20) + 'px',
        //    'min-width': (this.ViewWidth - 20) + 'px'
        //};
        //this.canvasHeight = {
        //    height: this.rowCount * this.rowHeight + 'px'
        //};
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
            MinWidth = '100%;';
        }
        else {
            MinWidth = this.ViewWidth + 'px';
        }
        if (this.TotalWidth == 0) {
            this.cols.toArray().forEach(function (value, key) {
                if (value.visibility != 'hidden') {
                    _this.TotalWidth += +(value.width);
                }
            });
        }
        this.canvasHeight = {
            //'top': this.HeaderHeight + 'px',
            height: (this.rowCount * this.rowHeight) + this.StaticDetailsHeight + 'px',
            'width': '100%',
            'min-width': this.TotalWidth + 'px' ////(this.TotalWidth > this.ViewWidth ? this.TotalWidth : this.ViewWidth) + 'px'
        };
        this.BodyTop = this.HeaderHeight;
        //this.numberOfTotalPages = (this.rowCount / this.rowsPerPage); // + (this.rowCount % this.rowsPerPage);
        //console.log("number of total pages:", this.numberOfTotalPages, "this.rowCount % this.rowsPerPage", this.rowCount % this.rowsPerPage);
        //console.log("this.rowCount / this.rowsPerPage", this.rowCount / this.rowsPerPage);
        //this.updateDisplayList();
        //this.onScroll();
    };
    EditableLogGridComponent.prototype.GridBodyResized = function () {
        //console.log("GridBodyResized()");
    };
    EditableLogGridComponent.prototype.ngOnDestroy = function () {
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
        // Removes the event listener
        //  this.func();
    };
    EditableLogGridComponent.prototype.focusCell = function ($event, rowIndex, colIndex) {
        //////console.log($event, rowIndex, colIndex);
        //this.editingCell[rowIndex, colIndex] = true;
    };
    EditableLogGridComponent.prototype.resizeColumn = function ($event, column) {
        $event.preventDefault();
        window.addEventListener("mousemove", this.mouseMove);
        //////console.log($event, column);
        this.MouseDownX = $event.clientX;
        this.ResizableColumn = document.getElementById(column);
        this.ColumnWidth = this.ResizableColumn.clientWidth;
        //console.log(this.ColumnWidth);
        //elem.style.width = '250px';
        //var rows = document.getElementsByClassName(column);
        //if (rows) {
        //    for (var i in rows) {
        //        i.style.width = '250px';
        //    }
        //}
        //console.log($event.clientX, $event.clientY, column);
    };
    EditableLogGridComponent.prototype.resizeColumn2 = function ($event, column) {
        $event.preventDefault();
        window.removeEventListener("mousemove", this.mouseMove);
        //////console.log($event, column);
        this.MouseUpX = $event.clientX;
        var diff = this.MouseUpX - this.MouseDownX;
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
    };
    EditableLogGridComponent.prototype.mousemoveevent = function (args) {
        var ss = args;
    };
    EditableLogGridComponent.prototype.divOnFocus = function (rowIndex, colIndex) {
        var inputID = 'input-row' + rowIndex + 'col' + colIndex;
        //////console.log(inputID);
        //this.editingCell[rowIndex + ' ' + colIndex] = true;
        var elem = document.getElementById(inputID);
        var span = document.getElementById('span-row' + rowIndex + 'col' + colIndex);
        if (span && elem) {
            elem.style.display = 'block';
            span.style.display = 'none';
            elem.focus();
        }
        //////console.log(elem);
        //elem.focus();
        //this.displayVisibility[rowIndex + '' + colIndex] = { display: 'none' };
        //this.editVisibility[rowIndex + '' + colIndex] = { display: 'block' };
    };
    EditableLogGridComponent.prototype.blurInput = function (rowIndex, colIndex) {
        var inputID = 'input-row' + rowIndex + 'col' + colIndex;
        //////console.log(inputID);
        //this.editingCell[rowIndex + ' ' + colIndex] = true;
        var elem = document.getElementById(inputID);
        var span = document.getElementById('span-row' + rowIndex + 'col' + colIndex);
        if (span && elem) {
            elem.style.display = 'none';
            span.style.display = 'block';
        }
        //elem.focus();
    };
    EditableLogGridComponent.prototype.inputFocus = function ($event) {
        //////console.log('event', $event);
    };
    EditableLogGridComponent.prototype.GetRowCount = function (reload) {
        if (reload === void 0) { reload = false; }
        var firstRow = Math.floor(this.scrollTop / this.rowHeight);
        this.controller.getRow(firstRow, this.sortingCol, this.sortingDir, true, "", false, null, reload);
    };
    Object.defineProperty(EditableLogGridComponent.prototype, "SelectedRow", {
        get: function () { return this.selectedRow; },
        set: function (newValue) {
            if (this.selectedRow != newValue) {
                this.selectedRow = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditableLogGridComponent.prototype, "SelectedRows", {
        get: function () { return this.selectedRows; },
        set: function (newValue) {
            var _this = this;
            if (this.selectedRows != newValue) {
                this.selectedRows = newValue;
                this.selectedRows.forEach(function (value, key) {
                    if (_this.rows.filter(function (a) { return a.rowData == value; }).length > 0) {
                        _this.rows.filter(function (a) { return a.rowData == value; })[0].IsSelected = true;
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    EditableLogGridComponent.prototype.onRowSelected = function (item) {
        if (this.Disabled) {
            return;
        }
        if (this.EnableMultiSelection == true) {
            if (this.SelectedRows.filter(function (a) { return a == item; }).length > 0) {
                this.SelectedRows = this.SelectedRows.filter(function (a) { return a != item; });
                this.rows.filter(function (a) { return a.rowData == item; })[0].IsSelected = false;
            }
            else {
                this.SelectedRows.push(item);
                this.rows.filter(function (a) { return a.rowData == item; })[0].IsSelected = true;
            }
            this.SelectedItemChanged.emit(this.SelectedRows);
        }
        else {
            this.SelectedItemChanged.emit(item);
        }
    };
    EditableLogGridComponent.prototype.OnRowDoubleClick = function (item) {
        this.RowDoubleClick.emit(item);
    };
    EditableLogGridComponent.prototype.OnMouseleave = function (item) {
        this.MouseleaveChanged.emit(item.rowData);
    };
    EditableLogGridComponent.prototype.OnMouseOver = function (item) {
        this.MouseOverChanged.emit(item.rowData);
    };
    EditableLogGridComponent.prototype.Ondblclicked = function () {
        this.Ondblclick.emit("");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "MouseOverChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "MouseleaveChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "RowDoubleClick", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "Ondblclick", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "RowEnded", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "RowDataLoaded", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "onparentclickEvent", void 0);
    __decorate([
        core_1.ContentChildren(LogColumnComponent_1.LogColumnComponent, { descendants: false }),
        __metadata("design:type", core_1.QueryList)
    ], EditableLogGridComponent.prototype, "cols", void 0);
    __decorate([
        core_1.ContentChild(LogRowDetailsComponent_1.LogRowDetailsComponent),
        __metadata("design:type", Object)
    ], EditableLogGridComponent.prototype, "RowDetail", void 0);
    __decorate([
        core_1.ViewChildren(LogColumnComponent_1.LogColumnComponent),
        __metadata("design:type", core_1.QueryList)
    ], EditableLogGridComponent.prototype, "wrapper", void 0);
    EditableLogGridComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'logitude-edit-grid',
            templateUrl: './EditableLogGridComponent.html',
            //directives: [CORE_DIRECTIVES, EditableListTemplateComponent],
            inputs: ['columns:columns', 'GridWidth', 'GridHeight', 'groupby', 'ItemSource', 'EnableLines', 'DisableRowByFieldValue', 'DisableRowByFieldName', 'SelectedRow', 'Disabled', 'HeaderHeight', 'ShowCount', 'ReloadDetails', 'ReRenderGrid', 'IsReadOnly', 'IsDarkHeader', 'EnableMultiSelection', 'SelectedRows', 'FooterTop', 'UseVirtuallization', 'EnableGridViewRowBackground', 'ChangeScrollPosition', 'EnableExpandCollapseAll'],
        }),
        __metadata("design:paramtypes", [core_1.ElementRef, core_1.Renderer, core_1.ChangeDetectorRef, core_1.IterableDiffers])
    ], EditableLogGridComponent);
    return EditableLogGridComponent;
}());
exports.EditableLogGridComponent = EditableLogGridComponent;
//# sourceMappingURL=EditableLogGridComponent.js.map