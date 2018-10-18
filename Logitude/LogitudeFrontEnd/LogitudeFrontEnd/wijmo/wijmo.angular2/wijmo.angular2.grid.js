var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
var core_1 = require('angular2/core');
var core_2 = require('angular2/core');
var core_3 = require('angular2/core');
var wijmo_angular2_directiveBase_1 = require('./wijmo.angular2.directiveBase');
var wj;
(function (wj) {
    var angular2;
    (function (angular2) {
        // FlexGrid
        var WjFlexGrid = (function (_super) {
            __extends(WjFlexGrid, _super);
            function WjFlexGrid(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                new DirectiveCellFactory(this);
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexGrid = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-grid',
                }),
                core_1.View({
                    //template: `<ng-content select="wj2-flex-grid-column"></ng-content>
                    // we need a div here to supply instantiated templates with a root in shadow DOM
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_3.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_3.Injector])
            ], WjFlexGrid);
            return WjFlexGrid;
        })(wijmo.grid.FlexGrid);
        angular2.WjFlexGrid = WjFlexGrid;
        // Column
        var WjFlexGridColumn = (function (_super) {
            __extends(WjFlexGridColumn, _super);
            function WjFlexGridColumn(elRef, injector, gridCmp, viewContainerRef, templateRef) {
                _super.call(this);
                if (gridCmp.autoGenerateColumns) {
                    gridCmp.autoGenerateColumns = false;
                    gridCmp.columns.clear();
                }
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjFlexGridColumn = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-flex-grid-column',
                    wjParentDirectives: [WjFlexGrid]
                }),
                core_1.View({
                    // we need a div here to supply instantiated templates with a root in shadow DOM
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_3.Injector)),
                __param(2, core_3.Inject(WjFlexGrid)),
                __param(3, core_3.Inject(core_2.ViewContainerRef)),
                __param(4, core_3.Inject(core_2.TemplateRef)),
                __param(4, core_3.Optional()), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_3.Injector, WjFlexGrid, core_2.ViewContainerRef, core_2.TemplateRef])
            ], WjFlexGridColumn);
            return WjFlexGridColumn;
        })(wijmo.grid.Column);
        angular2.WjFlexGridColumn = WjFlexGridColumn;
        /**
        * Defines the type of cell to which to apply the template. This value is specified in the <b>cell-type</b> attribute
        * of the @see:WjFlexGridCellTemplate directive.
        */
        (function (CellTemplateType) {
            /** Defines a regular (data) cell. */
            CellTemplateType[CellTemplateType["Cell"] = 0] = "Cell";
            /** Defines a cell in edit mode. */
            CellTemplateType[CellTemplateType["CellEdit"] = 1] = "CellEdit";
            /** Defines a column header cell. */
            CellTemplateType[CellTemplateType["ColumnHeader"] = 2] = "ColumnHeader";
            /** Defines a row header cell. */
            CellTemplateType[CellTemplateType["RowHeader"] = 3] = "RowHeader";
            /** Defines a row header cell in edit mode. */
            CellTemplateType[CellTemplateType["RowHeaderEdit"] = 4] = "RowHeaderEdit";
            /** Defines a top left cell. */
            CellTemplateType[CellTemplateType["TopLeft"] = 5] = "TopLeft";
            /** Defines a group header cell in a group row. */
            CellTemplateType[CellTemplateType["GroupHeader"] = 6] = "GroupHeader";
            /** Defines a regular cell in a group row. */
            CellTemplateType[CellTemplateType["Group"] = 7] = "Group";
            /** Defines a cell in a new row template. */
            CellTemplateType[CellTemplateType["NewCellTemplate"] = 8] = "NewCellTemplate";
        })(angular2.CellTemplateType || (angular2.CellTemplateType = {}));
        var CellTemplateType = angular2.CellTemplateType;
        var WjFlexGridCellTemplate = (function () {
            function WjFlexGridCellTemplate(viewContainerRef, templateRef, elRef, domRenderer, grid, column) {
                this.viewContainerRef = viewContainerRef;
                this.templateRef = templateRef;
                this.elRef = elRef;
                this.domRenderer = domRenderer;
                this.grid = grid;
                this.column = column;
            }
            // returns the name of the property on control instance that stores info for the specified cell template type.
            WjFlexGridCellTemplate._getTemplContextProp = function (templateType) {
                return '$__cellTempl' + CellTemplateType[templateType];
            };
            WjFlexGridCellTemplate.prototype.ngOnInit = function () {
                this.ownerControl = this.column && this.column.grid === this.grid ? this.column : this.grid;
                this._attachToControl();
            };
            WjFlexGridCellTemplate.prototype.ngOnDestroy = function () {
                if (this.cellTypeStr) {
                    this.viewContainerRef.clear();
                    this.ownerControl[WjFlexGridCellTemplate._getTemplContextProp(this.cellType)] = null;
                    this.grid.invalidate();
                }
            };
            WjFlexGridCellTemplate.prototype._instantiateTemplate = function (parent) {
                console.log("aaaaaasssssaaaaaaxxxx");
                return wijmo_angular2_directiveBase_1.WjDirectiveBehavior.instantiateTemplate(parent, this.viewContainerRef, this.templateRef, this.domRenderer);
                console.log("aaaaaasssssaaaaaa33322xxxx");
            };
            WjFlexGridCellTemplate.prototype._attachToControl = function () {
                if (!this.cellTypeStr) {
                    return;
                }
                this.cellType = wijmo.asEnum(this.cellTypeStr, CellTemplateType);
                this.ownerControl[WjFlexGridCellTemplate._getTemplContextProp(this.cellType)] = this;
                this.grid.invalidate();
            };
            WjFlexGridCellTemplate = __decorate([
                core_2.Directive({
                    selector: '[wjFlexGridCellTemplate]',
                    inputs: ['wjFlexGridCellTemplate', 'cellTypeStr: cellType', 'cellOverflow']
                }),
                __param(0, core_3.Inject(core_2.ViewContainerRef)),
                __param(1, core_3.Inject(core_2.TemplateRef)),
                __param(1, core_3.Optional()),
                __param(2, core_3.Inject(core_2.ElementRef)),
                __param(3, core_3.Inject(core_3.Renderer)),
                __param(4, core_3.Inject(WjFlexGrid)),
                __param(5, core_3.Inject(WjFlexGridColumn)),
                __param(5, core_3.Optional()), 
                __metadata('design:paramtypes', [core_2.ViewContainerRef, core_2.TemplateRef, core_2.ElementRef, core_3.Renderer, WjFlexGrid, WjFlexGridColumn])
            ], WjFlexGridCellTemplate);
            return WjFlexGridCellTemplate;
        })();
        angular2.WjFlexGridCellTemplate = WjFlexGridCellTemplate;
        var DirectiveCellFactory = (function (_super) {
            __extends(DirectiveCellFactory, _super);
            function DirectiveCellFactory(grid) {
                _super.call(this);
                this._lastApplyTimeStamp = 0;
                this._noApplyLag = false;
                this._startingEditing = false;
                this.grid = grid;
                // init _templateTypes
                if (!DirectiveCellFactory._templateTypes) {
                    DirectiveCellFactory._templateTypes = [];
                    for (var templateType in CellTemplateType) {
                        if (isNaN(templateType)) {
                            DirectiveCellFactory._templateTypes.push(templateType);
                        }
                    }
                }
                var self = this;
                this._baseCf = grid.cellFactory;
                grid.cellFactory = this;
                // initialize input event dispatcher
                this._evtInput = document.createEvent('HTMLEvents');
                this._evtInput.initEvent('input', true, false);
                // initialize blur event dispatcher
                this._evtBlur = document.createEvent('HTMLEvents');
                this._evtBlur.initEvent('blur', false, false);
                // no $apply() lag while editing
                grid.prepareCellForEdit.addHandler(function (s, e) {
                    self._noApplyLag = true;
                });
                grid.cellEditEnded.addHandler(function (s, e) {
                    setTimeout(function () {
                        self._noApplyLag = false;
                    }, 300);
                });
                grid.beginningEdit.addHandler(function (s, e) {
                    self._startingEditing = true;
                });
                grid.hostElement.addEventListener('keydown', function (e) {
                    self._startingEditing = false;
                }, true);
                grid.hostElement.addEventListener('keypress', function (e) {
                    var char = e.charCode > 32 ? String.fromCharCode(e.charCode) : null;
                    if (char) {
                        // Grid's _KeyboardHandler may receive 'keypress' before or after this handler (observed at least in IE,
                        // not clear why this happens). So both grid.activeEditor and _startingEditing (the latter is initialized in
                        // beginningEdit and cleared in 'keydown') participate in detecting whether this char has initialized a cell
                        // editing.
                        if (!grid.activeEditor || self._startingEditing) {
                            self._editChar = char;
                        }
                        else if (self._editChar) {
                            self._editChar += char;
                        }
                    }
                }, true);
            }
            DirectiveCellFactory.prototype.updateCell = function (panel, rowIndex, colIndex, cell, rng) {
                // restore overflow for any cell
                if (cell.style.overflow) {
                    cell.style.overflow = '';
                }
                var self = this, grid = panel.grid, editRange = grid.editRange, templateType, row = panel.rows[rowIndex], dataItem = row.dataItem, isGridCtx = false, needCellValue = false, isEdit = false, isCvGroup = false;
                // determine template type
                switch (panel.cellType) {
                    case wijmo.grid.CellType.Cell:
                        if (row instanceof wijmo.grid.GroupRow) {
                            isCvGroup = dataItem instanceof wijmo.collections.CollectionViewGroup;
                            var isHierNonGroup = !(isCvGroup || row.hasChildren);
                            if (colIndex == panel.columns.firstVisibleIndex) {
                                templateType = isHierNonGroup ? CellTemplateType.Cell : CellTemplateType.GroupHeader;
                            }
                            else {
                                templateType = isHierNonGroup ? CellTemplateType.Cell : CellTemplateType.Group;
                                needCellValue = true;
                            }
                        }
                        else if (row instanceof wijmo.grid._NewRowTemplate) {
                            templateType = CellTemplateType.NewCellTemplate;
                        }
                        else if (editRange && editRange.row === rowIndex && editRange.col === colIndex) {
                            templateType = CellTemplateType.CellEdit;
                            needCellValue = isEdit = true;
                        }
                        else if (!(wijmo.grid['detail'] && wijmo.grid['detail'].DetailRow &&
                            (row instanceof wijmo.grid['detail'].DetailRow))) {
                            templateType = CellTemplateType.Cell;
                        }
                        break;
                    case wijmo.grid.CellType.ColumnHeader:
                        templateType = CellTemplateType.ColumnHeader;
                        break;
                    case wijmo.grid.CellType.RowHeader:
                        templateType = grid.collectionView &&
                            grid.collectionView.currentEditItem === dataItem
                            ? CellTemplateType.RowHeaderEdit
                            : CellTemplateType.RowHeader;
                        isGridCtx = true;
                        break;
                    case wijmo.grid.CellType.TopLeft:
                        templateType = CellTemplateType.TopLeft;
                        isGridCtx = true;
                        break;
                }
                var isUpdated = false;
                if (templateType != null) {
                    var col = (isCvGroup && templateType == CellTemplateType.GroupHeader ?
                        grid.columns.getColumn(dataItem.groupDescription['propertyName']) :
                        (colIndex >= 0 && colIndex < panel.columns.length ? panel.columns[colIndex] : null));
                    if (col) {
                        var templContextProp = WjFlexGridCellTemplate._getTemplContextProp(templateType), templContext = (isGridCtx ? grid : col)[templContextProp];
                        // maintain template inheritance
                        if (!templContext) {
                            if (templateType === CellTemplateType.RowHeaderEdit) {
                                templateType = CellTemplateType.RowHeader;
                                templContextProp = WjFlexGridCellTemplate._getTemplContextProp(templateType);
                                templContext = grid[templContextProp];
                            }
                            else if (templateType === CellTemplateType.Group || templateType === CellTemplateType.GroupHeader) {
                                if (!isCvGroup) {
                                    templateType = CellTemplateType.Cell;
                                    templContextProp = WjFlexGridCellTemplate._getTemplContextProp(templateType);
                                    templContext = col[templContextProp];
                                }
                            }
                        }
                        if (templContext) {
                            // apply directive template and style
                            var isTpl = true, cellValue;
                            if (needCellValue) {
                                cellValue = panel.getCellData(rowIndex, colIndex, false);
                            }
                            // apply cell template
                            if (isTpl) {
                                isUpdated = true;
                                if (isEdit) {
                                    this._baseCf.updateCell(panel, rowIndex, colIndex, cell, rng, true);
                                }
                                // if this is false then we can't reuse previously cached scope and linked tree.
                                var cellContext = (cell[templContextProp] || {}), isForeignCell = cellContext.column !== col || !cellContext.viewRef;
                                if (isForeignCell) {
                                    if (isEdit) {
                                        var rootEl = cell.firstElementChild;
                                        if (rootEl) {
                                            // set focus to cell, because hiding a focused element may move focus to a page body
                                            // that will force Grid to finish editing.
                                            cell.focus();
                                            rootEl.style.display = 'none';
                                        }
                                    }
                                    else {
                                        cell.textContent = '';
                                    }
                                    var templInstance = templContext._instantiateTemplate(cell);
                                    cellContext.column = col;
                                    cellContext.viewRef = templInstance.viewRef;
                                    cellContext.rootElement = templInstance.rootElement;
                                    cell[templContextProp] = cellContext;
                                }
                                this._setViewRefVars(cellContext.viewRef, row, col, dataItem, cellValue);
                                if (templContext.cellOverflow) {
                                    cell.style.overflow = templContext.cellOverflow;
                                }
                                // increase row height if cell doesn't fit in the current row height.
                                setTimeout(function () {
                                    var cellHeight = cell.scrollHeight, panelRows = panel.rows;
                                    if (rowIndex < panelRows.length && panelRows[rowIndex].renderHeight < cellHeight) {
                                        panelRows.defaultSize = cellHeight;
                                        if (isEdit) {
                                            grid.refresh();
                                            //grid.refreshCells(false, true, false);
                                            grid.startEditing();
                                            return;
                                        }
                                    }
                                    else if (isEdit && !wijmo.contains(cellContext.rootElement, document.activeElement)) {
                                        // Find first visible input element and focus it. Make it only if editing
                                        // was not interrupted by row height change performed above, because it may finally
                                        // results in calling setSelectionRange on detached input, which causes crash in IE.
                                        var inputs = cellContext.rootElement.querySelectorAll('input');
                                        if (inputs) {
                                            for (var i = 0; i < inputs.length; i++) {
                                                var input = inputs[i], inpSt = window.getComputedStyle(input);
                                                if (inpSt.display !== 'none' && inpSt.visibility === 'visible') {
                                                    var inpFocusEh = function () {
                                                        input.removeEventListener('focus', inpFocusEh);
                                                        setTimeout(function () {
                                                            if (self._editChar) {
                                                                input.value = self._editChar;
                                                                self._editChar = null;
                                                                wijmo.setSelectionRange(input, input.value.length);
                                                                input.dispatchEvent(self._evtInput);
                                                            }
                                                        }, 0);
                                                    };
                                                    input.addEventListener('focus', inpFocusEh);
                                                    input.focus();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }, 0);
                                if (isEdit) {
                                    var editEndingEH = function (s, e) {
                                        grid.cellEditEnding.removeHandler(editEndingEH);
                                        // Move focus out of the current input element, in order to let it to save
                                        // its value (necessary for controls like InputDate that can't update value immediately
                                        // as user typing).
                                        // We do it via event emulation, instead of moving focus to another element,
                                        // because in IE an element doesn't fit in time to receive the 'blur' event.
                                        if (document.activeElement) {
                                            document.activeElement.dispatchEvent(self._evtBlur);
                                        }
                                        // We need to move focus nevertheless, because without this grid may lose focus at all in IE.
                                        cell.focus();
                                        if (!e.cancel) {
                                            e.cancel = true;
                                        }
                                        // close all open dropdowns 
                                        var dropDowns = cell.querySelectorAll('.wj-dropdown');
                                        [].forEach.call(dropDowns, function (el) {
                                            var ctrl = wijmo.Control.getControl(el);
                                            if (ctrl && ctrl instanceof wijmo.input.DropDown) {
                                                ctrl.isDroppedDown = false;
                                            }
                                        });
                                    };
                                    // subscribe the handler to the cellEditEnding event
                                    grid.cellEditEnding.addHandler(editEndingEH);
                                }
                                else {
                                    this._baseCf.updateCell(panel, rowIndex, colIndex, cell, rng, false);
                                }
                            }
                        }
                    }
                }
                if (!isUpdated) {
                    this._baseCf.updateCell(panel, rowIndex, colIndex, cell, rng);
                }
            };
            DirectiveCellFactory.prototype.disposeCell = function (cell) {
                var ttm = DirectiveCellFactory._templateTypes;
                for (var i = 0; i < ttm.length; i++) {
                    var templContextProp = WjFlexGridCellTemplate._getTemplContextProp(CellTemplateType[ttm[i]]), cellContext = (cell[templContextProp]);
                    if (cellContext && cellContext.viewRef) {
                        var templateOwner = cellContext.column || this.grid, templateContext = templateOwner[templContextProp];
                        if (templateContext) {
                            var viewIdx = templateContext.viewContainerRef.indexOf(cellContext.viewRef);
                            if (viewIdx > -1) {
                                templateContext.viewContainerRef.remove(viewIdx);
                            }
                        }
                        cellContext.viewRef = null;
                        cellContext.rootElement = null;
                        cell[templContextProp] = null;
                    }
                }
            };
            DirectiveCellFactory.prototype._setViewRefVars = function (viewRef, row, col, dataItem, cellValue) {
                viewRef.setLocal('row', row);
                viewRef.setLocal('col', col);
                viewRef.setLocal('item', dataItem);
                viewRef.setLocal('value', cellValue);
            };
            return DirectiveCellFactory;
        })(wijmo.grid.CellFactory);
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Grid = wj.angular2;
//# sourceMappingURL=wijmo.angular2.grid.js.map