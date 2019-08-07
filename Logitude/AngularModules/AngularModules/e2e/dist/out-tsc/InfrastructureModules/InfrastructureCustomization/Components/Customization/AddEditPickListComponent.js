"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var CustomPickListPM_1 = require("../../../../Infrastructure/EntityPMs/CustomPickListPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var PickListGeneralEntitiesArgs_1 = require("../../../../Infrastructure/DataContracts/PickListGeneralEntitiesArgs");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var AddEditPickListComponent = /** @class */ (function (_super) {
    __extends(AddEditPickListComponent, _super);
    function AddEditPickListComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs_1.PickListGeneralEntitiesArgs();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isNew = true;
        _this.IsMultipleChoice = false;
        _this.myService = new GeneralDomainService_1.GeneralDomainService();
        _this.PickListsList = new ObservableCollection_1.ObservableCollection([]);
        _this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs_1.PickListGeneralEntitiesArgs();
        _this.GeneralEntitiesArgs.CustomPickListPMs = [];
        _this.GeneralEntitiesArgs.RemovedCustomPickListPMs = [];
        return _this;
    }
    AddEditPickListComponent.prototype.SetWindowArgs = function (windowArgs) {
        var _this = this;
        this.PickListCode = windowArgs.Code;
        this.isNew = windowArgs.isNew;
        if (this.isNew) {
            this.PickListsList = new ObservableCollection_1.ObservableCollection([]);
        }
        else {
            this.myService.GetCustomPickListsByCode(this.PickListCode).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.loadedFields = myResponse.Result;
                    if (_this.loadedFields != null) {
                        _this.BuildItemsSource();
                    }
                }
            });
        }
    };
    AddEditPickListComponent.prototype.BuildItemsSource = function () {
        this.PickListsList = new ObservableCollection_1.ObservableCollection(this.loadedFields);
    };
    AddEditPickListComponent.prototype.AddPickListItem = function () {
        var pm = new CustomPickListPM_1.CustomPickListPM();
        pm.Code = this.PickListCode;
        pm.Tenant = SessionLocator_1.SessionLocator.Tenant;
        pm.IsMultipleChoice = this.IsMultipleChoice;
        this.PickListsList.Insert(pm);
        //this.context.CustomPickListPMs.Add(pm);
        //RefreshPickListsList();
        //var logWindow = new LogitudeWindow();
        //logWindow.Title = "Add New Custom Field";
        //var windowArgs: any = {};
        //this.myService = new GeneralDomainService();
        //this.myService.GetFieldDataTypes().subscribe(myResult => {
        //    var myResponse: ServiceResponse = myResult;
        //    if (!myResponse.HasError) {
        //        windowArgs.IsNew = true;
        //        var objectField = new ObjectFieldPM();
        //        objectField.Tenant = SessionLocator.Tenant;
        //        objectField.ObjectTableId = this.ObjecttableId;
        //        objectField.IsCustom = true;
        //        objectField.DisplayInEntityVariables = true;
        //        objectField.DisplayInList = true;
        //        objectField.CanFilter = true;
        //        windowArgs.objectField = objectField;
        //        windowArgs.DataTypeCollection = myResponse.Result;
        //        logWindow.WindowArgs = windowArgs;
        //        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
        //        logWindow.WindowClosed.subscribe((event: any) => {
        //            this.myService.GetCustomFieldsByTableId(this.ObjecttableId).subscribe(myResult => {
        //                var myResponse: ServiceResponse = myResult;
        //                if (!myResponse.HasError) {
        //                    this.loadedFields = myResponse.Result;
        //                    if (this.loadedFields != null) {
        //                        this.BuildItemsSource();
        //                    }
        //                }
        //            });
        //        });
        //    }
        //});
    };
    AddEditPickListComponent.prototype.DeleteLine = function (item) {
        this.PickListsList.Remove(item);
        if (this.GeneralEntitiesArgs == null) {
            this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs_1.PickListGeneralEntitiesArgs();
        }
        if (this.GeneralEntitiesArgs.RemovedCustomPickListPMs == null) {
            this.GeneralEntitiesArgs.RemovedCustomPickListPMs = [];
        }
        var mappedEntity;
        mappedEntity = this.MapJsonToEntityPM(item, false);
        this.GeneralEntitiesArgs.RemovedCustomPickListPMs.push(mappedEntity);
        //var logWindow = new LogitudeWindow();
        //logWindow.Title = "Add New Custom Field";
        //var windowArgs: any = {};
        //this.myService = new GeneralDomainService();
        //this.myService.GetFieldDataTypes().subscribe(myResult => {
        //    var myResponse: ServiceResponse = myResult;
        //    if (!myResponse.HasError) {
        //        windowArgs.IsNew = false;
        //        var objectField = item;
        //        //objectField.Tenant = SessionLocator.Tenant;
        //        //objectField.ObjectTableId = this.ObjecttableId;
        //        //objectField.IsCustom = true;
        //        //objectField.DisplayInEntityVariables = true;
        //        //objectField.DisplayInList = true;
        //        //objectField.CanFilter = true;
        //        windowArgs.objectField = objectField;
        //        windowArgs.DataTypeCollection = myResponse.Result;
        //        logWindow.WindowArgs = windowArgs;
        //        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditCustomFieldComponent');
        //        logWindow.WindowClosed.subscribe((event: any) => {
        //            this.myService.GetCustomFieldsByTableId(this.ObjecttableId).subscribe(myResult => {
        //                var myResponse: ServiceResponse = myResult;
        //                if (!myResponse.HasError) {
        //                    this.loadedFields = myResponse.Result;
        //                    if (this.loadedFields != null) {
        //                        this.BuildItemsSource();
        //                    }
        //                }
        //            });
        //        });
        //    }
        //});
    };
    AddEditPickListComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPickListComponent.prototype.SaveChanges = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.PickListsList != null && this.PickListsList.Length > 0) {
            this.ValidationErrorsList = [];
            this.PickListsList.Collection.forEach(function (list) {
                if (_this.PickListsList.Collection.filter(function (p) { return p.Value == list.Value && p.Id != list.Id; }).length > 0) {
                    _this.ValidationErrorsList.push("Some values are duplicated!");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(list.Value)) {
                    _this.ValidationErrorsList.push("PickList value is required");
                }
                else if (list.Value.Length > 1000) {
                    _this.ValidationErrorsList.push("PickList value length should be less than 1000 character");
                }
            });
            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving changes....");
                if (this.GeneralEntitiesArgs == null) {
                    this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs_1.PickListGeneralEntitiesArgs();
                }
                this.GeneralEntitiesArgs.CustomPickListPMs = [];
                //this.GeneralEntitiesArgs.RemovedCustomPickListPMs = [];
                this.PickListsList.Collection.forEach(function (list) {
                    var mappedEntity;
                    mappedEntity = _this.MapJsonToEntityPM(list, false);
                    _this.GeneralEntitiesArgs.CustomPickListPMs.push(mappedEntity);
                });
                if (this.isNew) {
                    this.myService.insertPickListGeneralEntities(this.GeneralEntitiesArgs).subscribe(function (myResult) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                        CachedDataManager_1.CachedDataManager.RefreshTableData("CustomPickList", true);
                        //var myResponse: ServiceResponse = myResult;
                        //if (!myResponse.HasError) {
                        //    this.loadedFields = myResponse.Result;
                        //    if (this.loadedFields != null) {
                        //        this.BuildItemsSource();
                        //    }
                        //}
                    });
                }
                else {
                    this.myService.updatePickListGeneralEntities(this.GeneralEntitiesArgs).subscribe(function (myResult) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                        CachedDataManager_1.CachedDataManager.RefreshTableData("CustomPickList", true);
                        _this.GeneralEntitiesArgs = new PickListGeneralEntitiesArgs_1.PickListGeneralEntitiesArgs();
                        //var myResponse: ServiceResponse = myResult;
                        //if (!myResponse.HasError) {
                        //    this.loadedFields = myResponse.Result;
                        //    if (this.loadedFields != null) {
                        //        this.BuildItemsSource();
                        //    }
                        //}
                    });
                }
                //SubmitOperation op = this.context.SubmitChanges();
                //op.Completed += new EventHandler(op_Completed);
            }
        }
        else {
            this.ValidationErrorsList.push("The pick list must have at least one item!");
        }
    };
    AddEditPickListComponent.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new CustomPickListPM_1.CustomPickListPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        entityPM.OldEntityPM = null;
        return entityPM;
    };
    AddEditPickListComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPickListComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPickListComponent);
    return AddEditPickListComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditPickListComponent = AddEditPickListComponent;
//# sourceMappingURL=AddEditPickListComponent.js.map