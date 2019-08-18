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
var ObjectFieldPM_1 = require("../../../../Infrastructure/EntityPMs/ObjectFieldPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ObjectFieldPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService");
var CustomFieldsComponent = /** @class */ (function () {
    function CustomFieldsComponent() {
        this.IsAddButtonEnabled = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.myService = new GeneralDomainService_1.GeneralDomainService();
        this.CustomFieldsCollection = new ObservableCollection_1.ObservableCollection([]);
        this._ObjectFieldPMService = new ObjectFieldPMService_1.ObjectFieldPMService();
    }
    CustomFieldsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.ObjectTableId = args['ObjectTableId'];
        this.ObjectTableName = args['ObjectTableName'];
        this.myService.GetCustomFieldsByTableId(this.ObjectTableId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.loadedFields = myResponse.Result;
                if (_this.loadedFields != null) {
                    _this.BuildItemsSource();
                }
            }
        });
        //this.BuildItemsSource();
    };
    CustomFieldsComponent.prototype.BuildItemsSource = function () {
        this.CustomFieldsCollection = new ObservableCollection_1.ObservableCollection(this.loadedFields);
        var fieldsCount = (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Master") ? 40 : 10;
        this.IsAddButtonEnabled = this.CustomFieldsCollection.Length < fieldsCount ? true : false;
        //var objectTablePM: ObjectTablePM;
        //var tableName: string;
        //this.Tabs = [];
        //objectTablePM = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
        //if (objectTablePM != null) {
        //    this.Tabs.push(new TabItem(objectTablePM));
        //}
        //var tableIds: string[] = [];
        //var mulityList: ObjectFieldPM[] = window.ObjectFields.filter(d => d.ObjectTableId == this.ObjecttableId && d.IsMulti);
        //mulityList.forEach((item) => {
        //    var index = tableIds.indexOf(item.MultiTableId);
        //    if (index == -1) {
        //        tableIds.push(item.MultiTableId);
        //        objectTablePM = window.ObjectTables.filter(d => d.Id == item.MultiTableId)[0];
        //        if (objectTablePM != null) {
        //            this.Tabs.push(new TabItem(objectTablePM));
        //        }
        //    }
        //});
        //this.SelectedTabItem = this.Tabs[0];
    };
    CustomFieldsComponent.prototype.AddCustomField = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add New Custom Field";
        var windowArgs = {};
        this.myService = new GeneralDomainService_1.GeneralDomainService();
        this.myService.GetFieldDataTypes().subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                windowArgs.IsNew = true;
                var objectField = new ObjectFieldPM_1.ObjectFieldPM();
                objectField.Tenant = SessionLocator_1.SessionLocator.Tenant;
                objectField.ObjectTableId = _this.ObjectTableId;
                objectField.IsCustom = true;
                objectField.DisplayInEntityVariables = true;
                objectField.DisplayInList = true;
                objectField.CanFilter = true;
                windowArgs.objectField = objectField;
                windowArgs.DataTypeCollection = myResponse.Result;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
                logWindow.WindowClosed.subscribe(function (event) {
                    _this.myService.GetCustomFieldsByTableId(_this.ObjectTableId).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            _this.loadedFields = myResponse.Result;
                            if (_this.loadedFields != null) {
                                _this.BuildItemsSource();
                            }
                        }
                    });
                });
            }
        });
    };
    CustomFieldsComponent.prototype.EditLine = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add New Custom Field";
        var windowArgs = {};
        this.myService = new GeneralDomainService_1.GeneralDomainService();
        this.myService.GetFieldDataTypes().subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this._ObjectFieldPMService.get(item.Id).subscribe(function (field) {
                    windowArgs.IsNew = false;
                    var objectField = field.Result;
                    //objectField.Tenant = SessionLocator.Tenant;
                    //objectField.ObjectTableId = this.ObjecttableId;
                    //objectField.IsCustom = true;
                    //objectField.DisplayInEntityVariables = true;
                    //objectField.DisplayInList = true;
                    //objectField.CanFilter = true;
                    windowArgs.objectField = objectField;
                    windowArgs.DataTypeCollection = myResponse.Result;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
                    logWindow.WindowClosed.subscribe(function (event) {
                        _this.myService.GetCustomFieldsByTableId(_this.ObjectTableId).subscribe(function (myResult) {
                            var myResponse = myResult;
                            if (!myResponse.HasError) {
                                _this.loadedFields = myResponse.Result;
                                if (_this.loadedFields != null) {
                                    _this.BuildItemsSource();
                                }
                            }
                        });
                    });
                });
            }
        });
    };
    CustomFieldsComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomFieldsComponent);
    return CustomFieldsComponent;
}());
exports.CustomFieldsComponent = CustomFieldsComponent;
//# sourceMappingURL=CustomFieldsComponent.js.map