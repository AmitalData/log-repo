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
var TextCodeTranslator_1 = require("../../../Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Tools");
var LogitudeListBoxComponent = /** @class */ (function () {
    function LogitudeListBoxComponent(CD) {
        this.CD = CD;
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.Binding = null;
        this.Sort = false;
        this.SelectedItem = null;
        this.ClassName = "ListBoxItem";
    }
    LogitudeListBoxComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.Height != null) {
            this.Style = { 'width': '100%', 'margin-top': '5px', 'height': this.Height + 'px', 'overflow': 'auto', 'background-color': 'transparent', 'max-height:': this.Height + 'px' };
        }
        else {
            this.Style = { 'width': '100%', 'margin-top': '5px', 'height': '100%', 'overflow': 'auto', 'background-color': 'transparent', 'max-height:': '494px' };
        }
        this.SourceItems = [];
        this.event.subscribe(function (res) {
            if (_this.DataSource) {
                if (_this.DataSource.length > 0) {
                    _this.SourceItems = [];
                    var iSourceItems = [];
                    _this.DataSource.forEach(function (value, key) {
                        if (value == _this.SelectedItem && res != null) {
                            iSourceItems.push(new ListBoxItem(value, _this, true));
                        }
                        else {
                            iSourceItems.push(new ListBoxItem(value, _this));
                        }
                        if (_this.Sort) {
                            _this.SourceItems = iSourceItems.sort(function (a, b) { return a.TranslatedText.toLowerCase() !== b.TranslatedText.toLowerCase() ? a.TranslatedText.toLowerCase() < b.TranslatedText.toLowerCase() ? -1 : 1 : 0; });
                        }
                        else {
                            _this.SourceItems = iSourceItems;
                        }
                        _this.CD.detectChanges();
                    });
                }
                else {
                    _this.SourceItems = [];
                }
            }
        });
        if (this.DataSourceChanged) {
            this.DataSourceChanged.subscribe(function (res) {
                if (res) {
                    if (res.length > 0) {
                        _this.DataSource = res;
                        _this.SourceItems = [];
                        _this.DataSource.forEach(function (value, key) {
                            if (value == _this.SelectedItem && res != null) {
                                _this.SourceItems.push(new ListBoxItem(value, _this, true));
                            }
                            else {
                                _this.SourceItems.push(new ListBoxItem(value, _this));
                            }
                            _this.CD.detectChanges();
                        });
                    }
                    else {
                        _this.SourceItems = [];
                    }
                }
            });
        }
    };
    LogitudeListBoxComponent.prototype.setSelectedItem = function (item) {
        this.SelectedItem = item;
        this.SourceItems.forEach(function (value, key) {
            value.ClassName = "ListBoxItem";
        });
        this.SelectedItemChanged.emit(this.SelectedItem);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogitudeListBoxComponent.prototype, "SelectedItemChanged", void 0);
    LogitudeListBoxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LogitudeListBoxComponent.html',
            inputs: ['DataSource', 'Height', 'SelectedItem', 'Binding', 'event', 'DataSourceChanged', 'Sort'],
            selector: 'LogListBox',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], LogitudeListBoxComponent);
    return LogitudeListBoxComponent;
}());
exports.LogitudeListBoxComponent = LogitudeListBoxComponent;
var ListBoxItem = /** @class */ (function () {
    function ListBoxItem(SourceItem, parentComponent, isselected) {
        if (isselected === void 0) { isselected = false; }
        this.parentComponent = parentComponent;
        this.ClassName = "ListBoxItem";
        this.TranslatedText = "";
        this.Item = SourceItem;
        if (isselected == true) {
            this.ClassName = "SelectedListBoxItem";
        }
        if (this.parentComponent.Binding == null) {
            this.Text = SourceItem;
        }
        else {
            this.Text = SourceItem[this.parentComponent.Binding];
        }
        this.TranslatedText = TextCodeTranslator_1.TextCodeTranslator.Translate(this.Text);
        if (Tools_1.AppTool.IsNullOrEmpty(this.TranslatedText)) {
            this.TranslatedText = "";
        }
    }
    Object.defineProperty(ListBoxItem.prototype, "Item", {
        get: function () { return this.item; },
        set: function (newValue) { this.item = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ListBoxItem.prototype, "Text", {
        get: function () { return this.text; },
        set: function (newValue) { this.text = newValue; },
        enumerable: true,
        configurable: true
    });
    ListBoxItem.prototype.setSelected = function () {
        this.parentComponent.setSelectedItem(this.Item);
        this.ClassName = "SelectedListBoxItem";
    };
    return ListBoxItem;
}());
exports.ListBoxItem = ListBoxItem;
//# sourceMappingURL=LogitudeListBoxComponent.js.map