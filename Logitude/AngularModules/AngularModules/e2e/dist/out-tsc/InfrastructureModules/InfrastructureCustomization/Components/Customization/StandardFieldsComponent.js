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
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var StandardFieldsComponent = /** @class */ (function () {
    function StandardFieldsComponent(_entityListService) {
        this._entityListService = _entityListService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        this.myService = new GeneralDomainService_1.GeneralDomainService();
    }
    StandardFieldsComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.ObjecttableId = windowArgs['ObjectTableId'];
        this.BuildTabsItemsSource();
    };
    Object.defineProperty(StandardFieldsComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    StandardFieldsComponent.prototype.SearchTextChanged = function (text) {
        this.SearchText = text;
        this.SelectedTabItem.BuildItemsSource(text);
    };
    StandardFieldsComponent.prototype.BuildTabsItemsSource = function () {
        var _this = this;
        var objectTablePM;
        var tableName;
        this.Tabs = [];
        objectTablePM = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjecttableId; })[0];
        if (objectTablePM != null) {
            this.Tabs.push(new TabItem(objectTablePM, this));
        }
        var tableIds = [];
        var mulityList = window.ObjectFields.filter(function (d) { return d.ObjectTableId == _this.ObjecttableId && d.IsMulti; });
        mulityList.forEach(function (item) {
            var index = tableIds.indexOf(item.MultiTableId);
            if (index == -1) {
                tableIds.push(item.MultiTableId);
                objectTablePM = window.ObjectTables.filter(function (d) { return d.Id == item.MultiTableId; })[0];
                if (objectTablePM != null) {
                    _this.Tabs.push(new TabItem(objectTablePM, _this));
                }
            }
        });
        this.SelectedTabItem = this.Tabs[0];
    };
    Object.defineProperty(StandardFieldsComponent.prototype, "SelectedTabItem", {
        get: function () { return this.selectedTabItem; },
        set: function (newValue) {
            if (this.selectedTabItem != newValue) {
                this.selectedTabItem = newValue;
                this.selectedTabItem.LoadStandardFields();
            }
        },
        enumerable: true,
        configurable: true
    });
    StandardFieldsComponent.prototype.SelectionChanged = function (clickdTab) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;
            }
        }
    };
    StandardFieldsComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    StandardFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './StandardFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], StandardFieldsComponent);
    return StandardFieldsComponent;
}());
exports.StandardFieldsComponent = StandardFieldsComponent;
var TabItem = /** @class */ (function () {
    function TabItem(objectTablePM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ObjectTablePM = objectTablePM;
        this.ObjectTableId = objectTablePM.Id;
        this.myService = new GeneralDomainService_1.GeneralDomainService();
        this.SetTabHeader();
    }
    TabItem.prototype.SetTabHeader = function () {
        this.Header = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.ObjectTablePM.Name);
    };
    TabItem.prototype.LoadStandardFields = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myService.GetStandardFieldsByTableId(this.ObjectTableId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.loadedFields = myResponse.Result;
                if (_this.loadedFields != null) {
                    _this.LoadTranslationsForMultiEntity();
                }
            }
        });
    };
    TabItem.prototype.LoadTranslationsForMultiEntity = function () {
        var _this = this;
        this.myService.GetTranslationsByParam(null, this.ObjectTableId, SessionLocator_1.SessionLocator.TenantPM.Language).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.EntityTranslations = myResponse.Result;
                _this.BuildItemsSource();
            }
        });
    };
    TabItem.prototype.BuildItemsSource = function (searchText) {
        var _this = this;
        if (searchText === void 0) { searchText = null; }
        this.FieldsItemsSource = [];
        if (Tools_1.AppTool.IsNullOrEmpty(searchText)) {
            this.loadedFields.forEach(function (item) {
                _this.FieldsItemsSource.push(new StandardFieldItem(item, _this.loadedFields, _this.EntityTranslations));
            });
        }
        else {
            this.loadedFields.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.FullNameTextCodeDefaultText) && item.FullNameTextCodeDefaultText.toUpperCase().indexOf(searchText.toUpperCase()) > -1) {
                    _this.FieldsItemsSource.push(new StandardFieldItem(item, _this.loadedFields, _this.EntityTranslations));
                }
            });
        }
        this.CurrentSession.StopBusyIndicator();
    };
    TabItem.prototype.EditField = function (editedItem) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "Edit Standard Field";
        logitudeWindow.WindowArgs = editedItem;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/EditStandardFieldComponent');
    };
    return TabItem;
}());
exports.TabItem = TabItem;
var StandardFieldItem = /** @class */ (function () {
    function StandardFieldItem(field, loadedFields, fieldsTranslations) {
        this.loadedFields = loadedFields;
        this.fieldsTranslations = fieldsTranslations;
        this.fullLabelObject = new GeneralDomainService_1.FieldsTranslations();
        this.shortLabelObject = new GeneralDomainService_1.FieldsTranslations();
        this.listLabelObject = new GeneralDomainService_1.FieldsTranslations();
        this.helpLabelObject = new GeneralDomainService_1.FieldsTranslations();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ObjectField = field;
        this.ObjectFieldId = field.Id;
        this.fullLabelObject = this.fieldsTranslations.filter(function (f) { return f.TextCodeId == field.FullNameTextCodeId; })[0];
        this.shortLabelObject = this.fieldsTranslations.filter(function (f) { return f.TextCodeId == field.ShortNameTextCodeId; })[0];
        this.listLabelObject = this.fieldsTranslations.filter(function (f) { return f.TextCodeId == field.ListTextCodeId; })[0];
        this.helpLabelObject = this.fieldsTranslations.filter(function (f) { return f.TextCodeId == field.HelpTextCodeId; })[0];
    }
    Object.defineProperty(StandardFieldItem.prototype, "DefaultText", {
        get: function () { return this.ObjectField.FullNameTextCodeDefaultText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StandardFieldItem.prototype, "IsRequiered", {
        get: function () { return this.ObjectField.IsRequiered; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StandardFieldItem.prototype, "FullLabelText", {
        get: function () { return this.fullLabelObject == null ? "" : this.fullLabelObject.TranslatedText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StandardFieldItem.prototype, "HelpTextText", {
        get: function () { return this.helpLabelObject == null ? "" : this.helpLabelObject.TranslatedText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StandardFieldItem.prototype, "DataTypeText", {
        get: function () {
            var result = this.ObjectField.DataTypeCode;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectField.ObjectTable_LookUpTableName) && this.ObjectField.DataTypeCode.toLowerCase() == "lookup") {
                result += " (" + this.ObjectField.ObjectTable_LookUpTableName + ")";
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StandardFieldItem.prototype, "IsEdited", {
        get: function () {
            var result = false;
            if (this.ObjectField.SystemRequired || this.ObjectField.SystemMaxLength > 0) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    return StandardFieldItem;
}());
exports.StandardFieldItem = StandardFieldItem;
//# sourceMappingURL=StandardFieldsComponent.js.map