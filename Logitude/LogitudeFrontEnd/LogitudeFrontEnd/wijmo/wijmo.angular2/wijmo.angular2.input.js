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
var core_4 = require('angular2/core');
var ngCore = require('angular2/core');
var wijmo_angular2_directiveBase_1 = require('./wijmo.angular2.directiveBase');
//import { wjNg2Meta } from './wijmo.angular2.MetaFactory';
var wjNg2Base = wijmo_angular2_directiveBase_1.wj.angular2;
var wj;
(function (wj) {
    var angular2;
    (function (angular2) {
        'use strict';
        // WjComboBox
        var WjComboBox = (function (_super) {
            __extends(WjComboBox, _super);
            function WjComboBox(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjComboBox = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-combo-box',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjComboBox);
            return WjComboBox;
        })(wijmo.input.ComboBox);
        angular2.WjComboBox = WjComboBox;
        // WjAutoComplete
        var WjAutoComplete = (function (_super) {
            __extends(WjAutoComplete, _super);
            function WjAutoComplete(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjAutoComplete = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-auto-complete',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjAutoComplete);
            return WjAutoComplete;
        })(wijmo.input.AutoComplete);
        angular2.WjAutoComplete = WjAutoComplete;
        // WjCalendar
        var WjCalendar = (function (_super) {
            __extends(WjCalendar, _super);
            function WjCalendar(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjCalendar = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-calendar',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjCalendar);
            return WjCalendar;
        })(wijmo.input.Calendar);
        angular2.WjCalendar = WjCalendar;
        // WjColorPicker
        var WjColorPicker = (function (_super) {
            __extends(WjColorPicker, _super);
            function WjColorPicker(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjColorPicker = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-color-picker',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjColorPicker);
            return WjColorPicker;
        })(wijmo.input.ColorPicker);
        angular2.WjColorPicker = WjColorPicker;
        // WjInputMask
        var WjInputMask = (function (_super) {
            __extends(WjInputMask, _super);
            function WjInputMask(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjInputMask = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-input-mask',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjInputMask);
            return WjInputMask;
        })(wijmo.input.InputMask);
        angular2.WjInputMask = WjInputMask;
        // WjInputColor
        var WjInputColor = (function (_super) {
            __extends(WjInputColor, _super);
            function WjInputColor(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjInputColor = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-input-color',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjInputColor);
            return WjInputColor;
        })(wijmo.input.InputColor);
        angular2.WjInputColor = WjInputColor;
        // WjMultiSelect
        var WjMultiSelect = (function (_super) {
            __extends(WjMultiSelect, _super);
            function WjMultiSelect(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjMultiSelect = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-multi-select',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjMultiSelect);
            return WjMultiSelect;
        })(wijmo.input.MultiSelect);
        angular2.WjMultiSelect = WjMultiSelect;
        // InputNumber
        var WjInputNumber = (function (_super) {
            __extends(WjInputNumber, _super);
            function WjInputNumber(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjInputNumber = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-input-number',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjInputNumber);
            return WjInputNumber;
        })(wijmo.input.InputNumber);
        angular2.WjInputNumber = WjInputNumber;
        // InputDate
        var WjInputDate = (function (_super) {
            __extends(WjInputDate, _super);
            function WjInputDate(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjInputDate = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-input-date',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjInputDate);
            return WjInputDate;
        })(wijmo.input.InputDate);
        angular2.WjInputDate = WjInputDate;
        // InputTime
        var WjInputTime = (function (_super) {
            __extends(WjInputTime, _super);
            function WjInputTime(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjInputTime = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-input-time',
                }),
                core_1.View({
                    template: "",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjInputTime);
            return WjInputTime;
        })(wijmo.input.InputTime);
        angular2.WjInputTime = WjInputTime;
        // ListBox
        var WjListBox = (function (_super) {
            __extends(WjListBox, _super);
            function WjListBox(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjListBox = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-list-box',
                }),
                core_1.View({
                    // we need a div here to supply instantiated templates with a root in shadow DOM
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjListBox);
            return WjListBox;
        })(wijmo.input.ListBox);
        angular2.WjListBox = WjListBox;
        // Menu
        var WjMenu = (function (_super) {
            __extends(WjMenu, _super);
            function WjMenu(elRef, injector, appRef, cdRef) {
                var _this = this;
                this._appRef = appRef;
                this._cdRef = cdRef;
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
                this.itemsSource = new wijmo.collections.ObservableArray();
                this.selectedIndex = 0;
                this.itemClicked.addHandler(function () {
                    _this.value = _this.selectedValue;
                });
            }
            Object.defineProperty(WjMenu.prototype, "value", {
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    //if (this._value != value) {
                    this._value = value;
                    if (value != null) {
                        this.selectedValue = value;
                        this._updateHeader();
                    }
                    //this._cdRef.markForCheck();
                    //this._appRef.tick();
                    //}
                },
                enumerable: true,
                configurable: true
            });
            WjMenu.prototype.ngOnInit = function () {
                this._attachToControl();
                this._updateHeader();
            };
            WjMenu.prototype.ngOnDestroy = function () {
                this.listBox.formatItem.removeHandler(this._fmtItem, this);
                this.listBox.loadingItems.removeHandler(this._loadingItems, this);
            };
            WjMenu.prototype.ngOnChanges = function (changes) {
                var headerChange = changes['header'];
                if (headerChange) {
                    this._definedHeader = headerChange.currentValue;
                    this._updateHeader();
                }
            };
            WjMenu.prototype.ngAfterContentInit = function () {
                // to force correct selectedValue and header update
                this.value = this.value;
                //this._updateHeader();
            };
            WjMenu.prototype.refresh = function (fullUpdate) {
                if (fullUpdate === void 0) { fullUpdate = true; }
                _super.prototype.refresh.call(this, fullUpdate);
                this._updateHeader();
            };
            WjMenu.prototype._attachToControl = function () {
                this.listBox.formatItem.addHandler(this._fmtItem, this);
                this.listBox.loadingItems.addHandler(this._loadingItems, this);
                //if (this.parent._isInitialized) {
                //    ownerControl.invalidate();
                this.invalidate();
            };
            WjMenu.prototype._loadingItems = function (s) {
                //TBD: will this destroy Wijmo directives in templates?
                //this.viewContainerRef.clear();
            };
            WjMenu.prototype._fmtItem = function (s, e) {
                if (!(e.data instanceof WjMenuItem)) {
                    return;
                }
                var itemEl = e.item;
                itemEl.textContent = '';
                var contentRoot = e.data.contentRoot;
                if (contentRoot) {
                    itemEl.appendChild(contentRoot);
                }
                //var viewRef = this._instantiateTemplate(itemEl);
                ////itemEl[WjItemTemplate._itemScopeProp] = itemScope;
                //viewRef.setLocal('control', s);
                //viewRef.setLocal('item', e.data);
                //viewRef.setLocal('itemIndex', e.index);
            };
            // if the scope has a value, show it in the header
            WjMenu.prototype._updateHeader = function () {
                this.header = this._definedHeader || '';
                var selItem = this.selectedItem;
                if (this.value != null && selItem && this.displayMemberPath) {
                    var currentValue = null;
                    if (selItem instanceof WjMenuItem) {
                        var contentRoot = selItem.contentRoot;
                        if (contentRoot) {
                            currentValue = contentRoot.innerHTML;
                        }
                        else {
                            currentValue = selItem[this.displayMemberPath];
                        }
                    }
                    if (currentValue != null) {
                        this.header += ': <b>' + currentValue + '</b>';
                    }
                }
            };
            WjMenu = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-menu',
                    changeDetection: core_4.ChangeDetectionStrategy.OnPush
                }),
                core_1.View({
                    // we need a div here to supply instantiated templates with a root in shadow DOM
                    template: "<div><ng-content></ng-content></div>",
                    directives: [core_2.forwardRef(function () { return WjMenuItem; })]
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)),
                __param(2, core_3.Inject(ngCore.ApplicationRef)),
                __param(3, core_3.Inject(ngCore.ChangeDetectorRef)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector, ngCore.ApplicationRef, ngCore.ChangeDetectorRef])
            ], WjMenu);
            return WjMenu;
        })(wijmo.input.Menu);
        angular2.WjMenu = WjMenu;
        var WjMenuItem = (function () {
            function WjMenuItem(
                //@Inject(ViewContainerRef) public viewContainerRef: ViewContainerRef,
                //@Inject(TemplateRef) @Optional() public templateRef: TemplateRef,
                elRef, injector, viewContainerRef, appViewManager, domRenderer) {
                this.elRef = elRef;
                this.viewContainerRef = viewContainerRef;
                this.appViewManager = appViewManager;
                this.domRenderer = domRenderer;
                var behavior = wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
                this._ownerMenu = behavior.parentBehavior.directive;
            }
            // patch, don't remove
            WjMenuItem.prototype.ngOnInit = function () {
            };
            WjMenuItem.prototype.wjAfterParentInit = function () {
                var ownerMenu = this._ownerMenu;
                if (ownerMenu.itemsSource.length == 1 && ownerMenu.selectedIndex < 0) {
                    ownerMenu.selectedIndex = 0;
                }
                if (!ownerMenu.displayMemberPath) {
                    ownerMenu.displayMemberPath = 'header';
                }
                if (!ownerMenu.selectedValuePath) {
                    ownerMenu.selectedValuePath = 'value';
                }
                if (!ownerMenu.commandPath) {
                    ownerMenu.commandPath = 'cmd';
                }
                if (!ownerMenu.commandParameterPath) {
                    ownerMenu.commandParameterPath = 'cmdParam';
                }
                //ownerMenu.invalidate();
            };
            WjMenuItem.prototype.ngAfterContentInit = function () {
            };
            WjMenuItem = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    //wjIsDirective: true,
                    wjMetadataId: 'MenuItem',
                    wjParentDirectives: [WjMenu],
                    wjSiblingDirectiveId: 'menuItemDir',
                    selector: 'wj-menu-item',
                }),
                core_1.View({
                    //template: '<div><ng-content></ng-content></div>',
                    template: "<template [wjMenuItemTemplateDir]><ng-content></ng-content></template>",
                    directives: [core_2.forwardRef(function () { return WjMenuItemTemplateDir; })]
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)),
                __param(2, core_3.Inject(core_2.ViewContainerRef)),
                __param(3, core_3.Inject(core_2.AppViewManager)),
                __param(4, core_3.Inject(core_2.Renderer)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector, core_2.ViewContainerRef, core_2.AppViewManager, core_2.Renderer])
            ], WjMenuItem);
            return WjMenuItem;
        })();
        angular2.WjMenuItem = WjMenuItem;
        var WjMenuSeparator = (function (_super) {
            __extends(WjMenuSeparator, _super);
            function WjMenuSeparator(elRef, injector, viewContainerRef, appViewManager, domRenderer) {
                _super.call(this, elRef, injector, viewContainerRef, appViewManager, domRenderer);
            }
            // patch, don't remove
            WjMenuSeparator.prototype.ngOnInit = function () {
            };
            WjMenuSeparator = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    //wjIsDirective: true,
                    wjMetadataId: 'MenuSeparator',
                    wjParentDirectives: [WjMenu],
                    wjSiblingDirectiveId: 'menuItemDir',
                    selector: 'wj-menu-separator',
                }),
                core_1.View({
                    //template: `<div><div class="wj-state-disabled" style="width:100%;height:1px;background-color:lightgray"></div></div>`,
                    template: "<template [wjMenuItemTemplateDir]><div class=\"wj-state-disabled\" style=\"width:100%;height:1px;background-color:lightgray\"></div></template>",
                    directives: [core_2.forwardRef(function () { return WjMenuItemTemplateDir; })]
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)),
                __param(2, core_3.Inject(core_2.ViewContainerRef)),
                __param(3, core_3.Inject(core_2.AppViewManager)),
                __param(4, core_3.Inject(core_2.Renderer)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector, core_2.ViewContainerRef, core_2.AppViewManager, core_2.Renderer])
            ], WjMenuSeparator);
            return WjMenuSeparator;
        })(WjMenuItem);
        angular2.WjMenuSeparator = WjMenuSeparator;
        var WjMenuItemTemplateDir = (function () {
            function WjMenuItemTemplateDir(viewContainerRef, templateRef, elRef, injector, domRenderer, menuItem, menuSeparator) {
                this.viewContainerRef = viewContainerRef;
                this.templateRef = templateRef;
                this.elRef = elRef;
                this.domRenderer = domRenderer;
                this.ownerItem = menuItem || menuSeparator;
                this.ownerItem.templateDir = this;
            }
            WjMenuItemTemplateDir.prototype.ngOnInit = function () {
            };
            WjMenuItemTemplateDir.prototype.ngAfterContentInit = function () {
                var self = this;
                //Without timeout, we get "LifeCycle.tick is called recursively" exception.
                setTimeout(function () {
                    var rootEl = wijmo_angular2_directiveBase_1.WjDirectiveBehavior.instantiateTemplate(null, self.viewContainerRef, self.templateRef, self.domRenderer).rootElement;
                    self.contentRoot = rootEl;
                    self.ownerItem.contentRoot = rootEl;
                    self.ownerItem._ownerMenu.listBox.invalidate();
                    self.ownerItem._ownerMenu.invalidate();
                }, 0);
            };
            WjMenuItemTemplateDir = __decorate([
                core_2.Directive({
                    selector: '[wjMenuItemTemplateDir]',
                    inputs: ['wjMenuItemTemplateDir']
                }),
                __param(0, core_3.Inject(core_2.ViewContainerRef)),
                __param(1, core_3.Inject(core_2.TemplateRef)),
                __param(1, core_2.Optional()),
                __param(2, core_3.Inject(core_2.ElementRef)),
                __param(3, core_3.Inject(core_2.Injector)),
                __param(4, core_3.Inject(core_2.Renderer)),
                __param(5, core_3.Inject(WjMenuItem)),
                __param(5, core_2.Optional()),
                __param(6, core_3.Inject(WjMenuSeparator)),
                __param(6, core_2.Optional()), 
                __metadata('design:paramtypes', [core_2.ViewContainerRef, core_2.TemplateRef, core_2.ElementRef, core_2.Injector, core_2.Renderer, WjMenuItem, WjMenuSeparator])
            ], WjMenuItemTemplateDir);
            return WjMenuItemTemplateDir;
        })();
        angular2.WjMenuItemTemplateDir = WjMenuItemTemplateDir;
        //@Directive({
        var WjItemTemplate = (function () {
            function WjItemTemplate(viewContainerRef, templateRef, elRef, injector, domRenderer) {
                this.viewContainerRef = viewContainerRef;
                this.templateRef = templateRef;
                this.elRef = elRef;
                this.domRenderer = domRenderer;
                var behavior = wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
                this.ownerControl = behavior.parentBehavior.directive;
                this.listBox = WjItemTemplate._getListBox(this.ownerControl);
            }
            WjItemTemplate.prototype.ngOnInit = function () {
                this._attachToControl();
            };
            WjItemTemplate.prototype.ngOnDestroy = function () {
                var ownerControl = this.ownerControl, listBox = this.listBox;
                if (listBox) {
                    listBox.formatItem.removeHandler(this._fmtItem, this);
                    listBox.loadingItems.removeHandler(this._loadingItems, this);
                }
                if (ownerControl) {
                    ownerControl.invalidate();
                }
            };
            WjItemTemplate.prototype._attachToControl = function () {
                this.listBox.formatItem.addHandler(this._fmtItem, this);
                this.listBox.loadingItems.addHandler(this._loadingItems, this);
                //if (this.parent._isInitialized) {
                //    ownerControl.invalidate();
                this.ownerControl.invalidate();
            };
            WjItemTemplate.prototype._loadingItems = function (s) {
                //TBD: will this destroy Wijmo directives in templates?
                this.viewContainerRef.clear();
            };
            WjItemTemplate.prototype._fmtItem = function (s, e) {
                var itemEl = e.item;
                itemEl.textContent = '';
                var viewRef = this._instantiateTemplate(itemEl);
                viewRef.setLocal('control', s);
                viewRef.setLocal('item', e.data);
                viewRef.setLocal('itemIndex', e.index);
            };
            WjItemTemplate.prototype._instantiateTemplate = function (parent) {
                return wijmo_angular2_directiveBase_1.WjDirectiveBehavior.instantiateTemplate(parent, this.viewContainerRef, this.templateRef, this.domRenderer).viewRef;
            };
            // Gets a ListBox control whose items are templated, it maybe the control itself or internal ListBox used by controls like
            // ComboBox.
            WjItemTemplate._getListBox = function (ownerControl) {
                if (ownerControl) {
                    return ownerControl instanceof wijmo.input.ListBox ? ownerControl : ownerControl.listBox;
                }
                return null;
            };
            WjItemTemplate = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    wjIsDirective: true,
                    wjMetadataId: 'ItemTemplate',
                    wjParentDirectives: [WjListBox, WjMenu],
                    selector: '[wjItemTemplate]',
                    inputs: ['wjItemTemplate']
                }),
                __param(0, core_3.Inject(core_2.ViewContainerRef)),
                __param(1, core_3.Inject(core_2.TemplateRef)),
                __param(1, core_2.Optional()),
                __param(2, core_3.Inject(core_2.ElementRef)),
                __param(3, core_3.Inject(core_2.Injector)),
                __param(4, core_3.Inject(core_2.Renderer)), 
                __metadata('design:paramtypes', [core_2.ViewContainerRef, core_2.TemplateRef, core_2.ElementRef, core_2.Injector, core_2.Renderer])
            ], WjItemTemplate);
            return WjItemTemplate;
        })();
        angular2.WjItemTemplate = WjItemTemplate;
        //function wjPopupInit() {
        //    var metaData = wjNg2Meta.MetaFactory.getMetaData(wijmo.input.Popup);
        //    wjNg2Meta.MetaFactory.findProp('owner', metaData.props).propertyType = wijmo.interop.PropertyType.Any;
        //}
        // WjPopup
        var WjPopup = (function (_super) {
            __extends(WjPopup, _super);
            function WjPopup(elRef, injector) {
                _super.call(this, wijmo_angular2_directiveBase_1.WjDirectiveBehavior.getHostElement(this, elRef));
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjPopup.prototype.ngOnChanges = function (changes) {
                var ownerChange = changes['owner'];
                if (ownerChange) {
                    if (this.modal == null) {
                        this.modal = this.owner ? false : true;
                    }
                }
            };
            WjPopup = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    selector: 'wj-popup',
                }),
                core_1.View({
                    template: "<div><ng-content></ng-content></div>",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjPopup);
            return WjPopup;
        })(wijmo.input.Popup);
        angular2.WjPopup = WjPopup;
        // WjContextMenu
        var WjContextMenu = (function () {
            function WjContextMenu(elRef) {
                this.elRef = elRef;
            }
            WjContextMenu.prototype.onContextMenu = function (e) {
                var menu = this.wjContextMenu, dropDown = menu.dropDown;
                if (menu && dropDown && !wijmo.closest(e.target, '[disabled]')) {
                    e.preventDefault();
                    menu.owner = this.elRef.nativeElement;
                    menu.selectedIndex = -1;
                    if (menu.onIsDroppedDownChanging(new wijmo.CancelEventArgs())) {
                        wijmo.showPopup(dropDown, e);
                        menu.onIsDroppedDownChanged();
                        dropDown.focus();
                    }
                }
            };
            WjContextMenu = __decorate([
                core_2.Directive({
                    selector: '[wjContextMenu]',
                    inputs: ['wjContextMenu'],
                    host: { '(contextmenu)': 'onContextMenu($event)' }
                }),
                __param(0, core_3.Inject(core_2.ElementRef)), 
                __metadata('design:paramtypes', [core_2.ElementRef])
            ], WjContextMenu);
            return WjContextMenu;
        })();
        angular2.WjContextMenu = WjContextMenu;
        // WjCollectionViewNavigator
        var WjCollectionViewNavigator = (function () {
            function WjCollectionViewNavigator(elRef, injector) {
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjCollectionViewNavigator = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    wjMetadataId: 'CollectionViewNavigator',
                    selector: 'wj-collection-view-navigator',
                }),
                core_1.View({
                    template: "\n            <div class=\"wj-control wj-content wj-pager\">\n                <div class=\"wj-input-group\">\n                    <span class=\"wj-input-group-btn\" >\n                        <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                           (click)=\"cv?.moveCurrentToFirst()\"\n                           [disabled]=\"!cv || cv?.currentPosition <= 0\">\n                            <span class=\"wj-glyph-left\" style=\"margin-right: -4px;\"></span>\n                            <span class=\"wj-glyph-left\"></span>\n                         </button>\n                    </span>\n                    <span class=\"wj-input-group-btn\" >\n                       <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                           (click)=\"cv?.moveCurrentToPrevious()\"\n                           [disabled]=\"!cv || cv?.currentPosition <= 0\">\n                            <span class=\"wj-glyph-left\"></span>\n                       </button>\n                    </span>\n                    <input type=\"text\" class=\"wj-form-control\" value=\"\n                       {{cv?.currentPosition + 1 | number}} / {{cv?.itemCount | number}}\n                       \" disabled />\n                    <span class=\"wj-input-group-btn\" >\n                        <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                           (click)=\"cv?.moveCurrentToNext()\"\n                           [disabled]=\"!cv || cv?.currentPosition >= cv?.itemCount - 1\">\n                            <span class=\"wj-glyph-right\"></span>\n                        </button>\n                    </span>\n                    <span class=\"wj-input-group-btn\" >\n                        <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                           (click)=\"cv?.moveCurrentToLast()\"\n                           [disabled]=\"!cv || cv?.currentPosition >= cv?.itemCount - 1\">\n                            <span class=\"wj-glyph-right\"></span>\n                            <span class=\"wj-glyph-right\" style=\"margin-left: -4px;\"></span>\n                        </button>\n                    </span>\n                </div>\n            </div>\n",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjCollectionViewNavigator);
            return WjCollectionViewNavigator;
        })();
        angular2.WjCollectionViewNavigator = WjCollectionViewNavigator;
        // WjCollectionViewPager
        var WjCollectionViewPager = (function () {
            function WjCollectionViewPager(elRef, injector) {
                wijmo_angular2_directiveBase_1.WjDirectiveBehavior.attach(this, elRef, injector);
            }
            WjCollectionViewPager = __decorate([
                wijmo_angular2_directiveBase_1.WjComponent({
                    wjMetadataId: 'CollectionViewPager',
                    selector: 'wj-collection-view-pager',
                }),
                core_1.View({
                    template: "\n            <div class=\"wj-control wj-content wj-pager\" >\n                <div class=\"wj-input-group\">\n                    <span class=\"wj-input-group-btn\" >\n                        <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                            (click)=\"cv?.moveToFirstPage()\"\n                            [disabled]=\"!cv || cv?.pageIndex <= 0\">\n                            <span class=\"wj-glyph-left\" style=\"margin-right: -4px;\"></span>\n                            <span class=\"wj-glyph-left\"></span>\n                        </button>\n                    </span>\n                    <span class=\"wj-input-group-btn\" >\n                    <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                            (click)=\"cv?.moveToPreviousPage()\"\n                            [disabled]=\"!cv || cv?.pageIndex <= 0\">\n                            <span class=\"wj-glyph-left\"></span>\n                        </button>\n                    </span>\n                    <input type=\"text\" class=\"wj-form-control\" value=\"\n                        {{cv?.pageIndex + 1 | number}} / {{cv?.pageCount | number}}\n                    \" disabled />\n                    <span class=\"wj-input-group-btn\" >\n                        <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                            (click)=\"cv?.moveToNextPage()\"\n                            [disabled]=\"!cv || cv?.pageIndex >= cv?.pageCount - 1\">\n                            <span class=\"wj-glyph-right\"></span>\n                        </button>\n                    </span>\n                    <span class=\"wj-input-group-btn\" >\n                        <button class=\"wj-btn wj-btn-default\" type=\"button\"\n                            (click)=\"cv?.moveToLastPage()\"\n                            [disabled]=\"!cv || cv?.pageIndex >= cv?.pageCount - 1\">\n                            <span class=\"wj-glyph-right\"></span>\n                            <span class=\"wj-glyph-right\" style=\"margin-left: -4px;\"></span>\n                        </button>\n                    </span>\n                </div>\n            </div>\n",
                }),
                __param(0, core_3.Inject(core_2.ElementRef)),
                __param(1, core_3.Inject(core_2.Injector)), 
                __metadata('design:paramtypes', [core_2.ElementRef, core_2.Injector])
            ], WjCollectionViewPager);
            return WjCollectionViewPager;
        })();
        angular2.WjCollectionViewPager = WjCollectionViewPager;
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Input = wj.angular2;
//# sourceMappingURL=wijmo.angular2.input.js.map