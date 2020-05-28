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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CustomPickListListService_1 = require("../../../../Infrastructure/Services/StandardLists/CustomPickListListService");
var GroupByPipe_1 = require("../../../../Infrastructure/Pipes/GroupByPipe");
var ObjectFieldPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/ObjectFieldPMService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var LoginService_1 = require("../../../../Infrastructure/Services/LoginService");
var http_1 = require("@angular/http");
var AddEditCustomFieldComponent = /** @class */ (function (_super) {
    __extends(AddEditCustomFieldComponent, _super);
    function AddEditCustomFieldComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this._customPickListListService = new CustomPickListListService_1.CustomPickListListService();
        _this._ObjectFieldPMService = new ObjectFieldPMService_1.ObjectFieldPMService();
        _this.IsNew = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.PickListVisibile = false;
        _this.ControlField1Visibile = false;
        _this.ControlField2Visibile = false;
        _this.loginService = new LoginService_1.LoginService();
        _this.ContolFieldsList1 = [];
        _this.ContolFieldsList2 = [];
        _this.CustomPickListsList = [];
        _this.LookUpTables = window.ObjectTables.filter(function (o) { return o.IsLookUp && !Tools_1.AppTool.IsNullOrEmpty(o.LookUp1); });
        var picklistslist = [];
        _this._customPickListListService.getAll().subscribe(function (response) {
            var temp = response.Result.filter(function (p) { return p.Tenant == SessionLocator_1.SessionLocator.Tenant; });
            var list = new GroupByPipe_1.GroupByPipe().transform(temp, "Code");
            list.forEach(function (value, key) {
                _this.CustomPickListsList.push(value.key);
            });
            //this.TenantCustomPickLists = response.Result;
            //this.CurrentSession.StopBusyIndicator();
            //if (this.TenantCustomPickLists != null) {
            //    this.TenantCustomPickLists.forEach(p => {
            //        if (this.CustomPickLists.indexOf(p.Code) === - 1) {
            //            this.CustomPickLists.push(p.Code);
            //        }
            //    });
            //} 
        });
        //IEnumerable < IGrouping < string, CustomPickListList >> list = CustomPickListDataProvider.GetCachedList<CustomPickListList>().Where(t => t.Tenant == TenantContext.Current.Id).GroupBy(f => f.Code);
        //foreach(IGrouping < string, CustomPickListList > group in list)
        //{
        //    picklistslist.Add(group.Key);
        //}
        //CustomPickListsList = new ObservableCollection<string>(picklistslist);
        _this.UIProperties.SetRequired("Code", "ObjectField", true);
        return _this;
    }
    AddEditCustomFieldComponent.prototype.SetWindowArgs = function (args) {
        //this.ShipmentList = args.SelectedShipment;
        this.IsNew = args.IsNew;
        this.objectField = args.objectField;
        this.DataTypeCollection = args.DataTypeCollection;
        if (this.IsNew) {
        }
        else {
            this.DataTypeSelectionMethod({ Code: this.objectField.DataTypeCode });
            this.LookUpTablesSelectionMethod("");
            this.PickListSelectionMethod(this.objectField.CustomPickListCode);
            this.UIProperties.SetEnabled("Code", "ObjectField", false);
        }
    };
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "CustomFieldDataType", {
        get: function () {
            var _this = this;
            var fieldDataType = this.DataTypeCollection.filter(function (d) { return d.Code == _this.objectField.DataTypeCode; })[0];
            return fieldDataType;
        },
        set: function (newValue) {
            this.objectField.DataTypeCode = newValue.Code;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "FieldLable", {
        get: function () {
            if (this.IsNew) {
                return this.objectField.FullNameTextCodeId;
            }
            else {
                return this.objectField.FullNameTextCodeDefaultText;
            }
        },
        set: function (newValue) {
            if (this.IsNew) {
                this.objectField.FullNameTextCodeId = newValue;
                this.objectField.ListTextCodeId = newValue;
                //if (AppTool.IsNullOrEmpty(this.Code) && !AppTool.IsNullOrEmpty(newValue)) {
                this.Code = Tools_1.AppTool.Replace(newValue, " ", "");
                //}
            }
            else {
                this.objectField.FullNameTextCodeDefaultText = newValue;
                this.objectField.ListTextCodeDefaultText = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "Code", {
        //private code: string;
        get: function () {
            if (this.IsNew) {
                return this.objectField.Code;
            }
            else {
                return this.objectField.Code;
            }
        },
        set: function (newValue) {
            if (newValue != this.objectField.Code) {
                this.objectField.Code = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "HelpText", {
        get: function () {
            if (!this.IsNew) {
                if (this.objectField.HelpTextCodeDefaultText != null) {
                    return this.objectField.HelpTextCodeDefaultText;
                }
                else {
                    return this.objectField.HelpTextCodeId;
                }
            }
            return this.objectField.HelpTextCodeId;
        },
        set: function (value) {
            if (!this.IsNew) {
                if (this.objectField.HelpTextCodeDefaultText != null) {
                    this.objectField.HelpTextCodeDefaultText = value;
                }
                else {
                    this.objectField.HelpTextCodeId = value;
                }
            }
            else {
                this.objectField.HelpTextCodeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "LookUpTableId", {
        get: function () {
            return this.objectField.LookUpTableId;
        },
        set: function (value) {
            if (value) {
                this.objectField.LookUpTableId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "LookUpTable", {
        get: function () {
            return this.lookUpTable;
        },
        set: function (value) {
            this.lookUpTable = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "MinLength", {
        get: function () {
            return this.objectField.MinLength;
        },
        set: function (value) {
            this.objectField.MinLength = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "MaxLength", {
        get: function () {
            return this.objectField.MaxLength;
        },
        set: function (value) {
            this.objectField.MaxLength = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "IsMultiline", {
        get: function () {
            return this.objectField.MultiLine;
        },
        set: function (value) {
            this.objectField.MultiLine = value;
        },
        enumerable: true,
        configurable: true
    });
    //public get PickListItem() {
    //    if (this.objectField.DataTypeCode == "PickList") {
    //        if (!AppTool.IsNullOrEmpty(this.objectField.CustomPickListCode)) {
    //            this._customPickListListService.getAll().subscribe(response => {
    //                var temp = response.Result.filter(p => p.Code == this.objectField.CustomPickListCode);
    //                if (temp.length > 0) {
    //                    this.pickListItem = temp[0].Code;
    //                }
    //                //this.TenantCustomPickLists = response.Result;
    //                //this.CurrentSession.StopBusyIndicator();
    //                //if (this.TenantCustomPickLists != null) {
    //                //    this.TenantCustomPickLists.forEach(p => {
    //                //        if (this.CustomPickLists.indexOf(p.Code) === - 1) {
    //                //            this.CustomPickLists.push(p.Code);
    //                //        }
    //                //    });
    //                //} 
    //            }); 
    //        }
    //    }
    //    return this.pickListItem;
    //}
    //public set PickListItem(value: string) {
    //    if (value != null) {
    //        this.objectField.CustomPickListCode = value;
    //    }
    //    else {
    //        this.objectField.CustomPickListCode = null;
    //    }
    //} 
    AddEditCustomFieldComponent.prototype.PickListSelectionMethod = function (item) {
        var _this = this;
        if (this.objectField.DataTypeCode == "PickList") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.objectField.CustomPickListCode)) {
                this._customPickListListService.getAll().subscribe(function (response) {
                    var temp = response.Result.filter(function (p) { return p.Code == _this.objectField.CustomPickListCode; });
                    if (temp.length > 0) {
                        _this.PickListItem = temp[0].Code;
                    }
                });
            }
            this.objectField.CustomPickListCode = item;
        }
    };
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "ControlField1", {
        get: function () {
            var _this = this;
            if (this.objectField.ControlField1 != null) {
                this.controlField1 = this.ContolFieldsList1.filter(function (f) { return f.FieldName == _this.objectField.ControlField1; })[0];
            }
            return this.controlField1;
        },
        set: function (value) {
            this.controlField1 = value;
            if (this.controlField1 != null) {
                this.objectField.ControlField1 = this.controlField1.FieldName;
            }
            else {
                this.objectField.ControlField1 = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditCustomFieldComponent.prototype.ContolFieldsList1SelectionMethod = function (item) {
        this.objectField.ControlField1 = item.FieldName;
    };
    Object.defineProperty(AddEditCustomFieldComponent.prototype, "ControlField2", {
        get: function () {
            var _this = this;
            if (this.objectField.ControlField2 != null) {
                this.controlField2 = this.ContolFieldsList2.filter(function (f) { return f.FieldName == _this.objectField.ControlField2; })[0];
            }
            return this.controlField2;
        },
        set: function (value) {
            this.controlField2 = value;
            if (this.controlField2 != null) {
                this.objectField.ControlField2 = this.controlField2.FieldName;
            }
            else {
                this.objectField.ControlField2 = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditCustomFieldComponent.prototype.ContolFieldsList2SelectionMethod = function (item) {
        this.objectField.ControlField2 = item.FieldName;
    };
    AddEditCustomFieldComponent.prototype.onIsMultilineChange = function (event) {
        this.IsMultiline = event;
    };
    AddEditCustomFieldComponent.prototype.LookUpTablesSelectionMethod = function (item) {
        var _this = this;
        this.ContolFieldsList1 = [];
        this.ContolFieldsList2 = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.LookUpTableId)) {
            this.LookUpTableId = item.Id;
            var lookupTable = window.ObjectTables.filter(function (t) { return t.Id == _this.LookUpTableId; })[0];
            this.LookUpTable = lookupTable;
            if (lookupTable != null) {
                this.ContolFieldsList1 = window.ObjectFields.filter(function (f) { return f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == _this.objectField.ObjectTableId; });
                this.ContolFieldsList2 = window.ObjectFields.filter(function (f) { return function (f) { return f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == _this.objectField.ObjectTableId; }; });
            }
        }
        else {
            this.LookUpTableId = item.Id;
            var lookupTable = window.ObjectTables.filter(function (t) { return t.Id == item.Id; })[0];
            this.LookUpTable = lookupTable;
            if (lookupTable != null) {
                this.ContolFieldsList1 = window.ObjectFields.filter(function (f) { return f.FieldName == lookupTable.DependencyFilter1 && f.DataTypeCode == "LookUp" && f.ObjectTableId == _this.objectField.ObjectTableId; });
                this.ContolFieldsList2 = window.ObjectFields.filter(function (f) { return function (f) { return f.FieldName == lookupTable.DependencyFilter2 && f.DataTypeCode == "LookUp" && f.ObjectTableId == _this.objectField.ObjectTableId; }; });
            }
        }
    };
    AddEditCustomFieldComponent.prototype.DataTypeSelectionMethod = function (fieldDataType) {
        if (fieldDataType == null) {
        }
        else {
            this.objectField.DataTypeCode = fieldDataType.Code;
            switch (fieldDataType.Code) {
                case "LookUp":
                    {
                        this.ControlField1Visibile = true;
                        this.ControlField2Visibile = true;
                        this.PickListVisibile = false;
                        break;
                    }
                case "nText":
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = false;
                        this.LookUpTableId = null;
                        this.objectField.ControlField1 = null;
                        this.objectField.ControlField2 = null;
                        break;
                    }
                case "Text":
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = false;
                        this.LookUpTableId = null;
                        this.objectField.ControlField1 = null;
                        this.objectField.ControlField2 = null;
                        break;
                    }
                case "PickList":
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = true;
                        break;
                    }
                default:
                    {
                        this.ControlField1Visibile = false;
                        this.ControlField2Visibile = false;
                        this.PickListVisibile = false;
                        this.objectField.ControlField1 = null;
                        this.objectField.ControlField2 = null;
                        this.LookUpTableId = null;
                        break;
                    }
            }
        }
        //FirePropertyChanged("DataTypeVisibility_LookUp");
        //FirePropertyChanged("DataTypeVisibility_Text");
        if (fieldDataType.Code == "LookUp") {
        }
        else {
        }
    };
    AddEditCustomFieldComponent.prototype.SaveChanges = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.objectField.DataTypeCode)) {
            this.ValidationErrorsList.push("Data Type is Required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Code)) {
            this.ValidationErrorsList.push("Code is Required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.FieldLable)) {
            this.ValidationErrorsList.push("Field Label is Required");
        }
        if (this.objectField.DataTypeCode == "LookUp" && this.objectField.LookUpTableId == null) {
            this.ValidationErrorsList.push("LookUp table is Required");
        }
        if ((this.objectField.DataTypeCode == "Text" || this.objectField.DataTypeCode == "nText") && this.objectField.MaxLength > 2000) {
            this.ValidationErrorsList.push("Maximum length of the text is 2000");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.authHeader = new http_1.Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator_1.SessionLocator.Tenant;
            if (this.IsNew == true) {
                this._ObjectFieldPMService.insert(this.objectField).subscribe(function (Fieldresponse) {
                    if (Fieldresponse.HasError) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.ValidationErrorsList = Fieldresponse.ErrorsArray;
                    }
                    else {
                        CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                            var item = Fieldresponse.Result;
                            var oldItem = window.ObjectFields.filter(function (t) { return t.Id == item.Id; })[0];
                            if (oldItem) {
                                var index = window.ObjectFields.indexOf(oldItem);
                                window.ObjectFields.splice(index, 1);
                            }
                            window.ObjectFields.push(item);
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            _this.CurrentSession.CloseCurrentWindow();
                            //    this.loginService.GetObjectFields().subscribe(myResult => {
                            //        if (myResult != null) { 
                            //            window.ObjectFields = myResult;
                            //            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            //            this.CurrentSession.CloseCurrentWindow();
                            //        }
                            //    });  
                            //});
                        });
                    }
                });
            }
            else {
                this._ObjectFieldPMService.update(this.objectField).subscribe(function (Fieldresponse) {
                    if (Fieldresponse.HasError) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.ValidationErrorsList = Fieldresponse.ErrorsArray;
                    }
                    else {
                        CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                            var item = Fieldresponse.Result;
                            var oldItem = window.ObjectFields.filter(function (t) { return t.Id == item.Id; })[0];
                            if (oldItem) {
                                var index = window.ObjectFields.indexOf(oldItem);
                                window.ObjectFields.splice(index, 1);
                            }
                            window.ObjectFields.push(item);
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            _this.CurrentSession.CloseCurrentWindow();
                            //this.loginService.GetObjectFields().subscribe(myResult => {
                            //    if (myResult != null) {
                            //        window.ObjectFields = myResult;
                            //        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            //        this.CurrentSession.CloseCurrentWindow();
                            //    }
                            //});
                        });
                    }
                });
            }
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    };
    AddEditCustomFieldComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCustomFieldComponent.prototype.EditPickListButtonClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add Custom Pick List";
        var windowArgs = {};
        windowArgs.isNew = false;
        windowArgs.Code = this.objectField.CustomPickListCode;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditPickListComponent');
        logWindow.WindowClosed.subscribe(function (event) {
            _this._customPickListListService.getAll().subscribe(function (response) {
                var temp = response.Result.filter(function (p) { return p.Tenant == SessionLocator_1.SessionLocator.Tenant; });
                var list = new GroupByPipe_1.GroupByPipe().transform(temp, "Code");
                _this.CustomPickListsList = [];
                list.forEach(function (value, key) {
                    _this.CustomPickListsList.push(value.key);
                });
            });
        });
    };
    AddEditCustomFieldComponent.prototype.AddPickListButtonClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Add Custom Pick List";
        var windowArgs = {};
        windowArgs.isNew = true;
        windowArgs.PickListCode = null;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditPickListComponent');
        logWindow.WindowClosed.subscribe(function (event) {
            _this._customPickListListService.getAll().subscribe(function (response) {
                var temp = response.Result.filter(function (p) { return p.Tenant == SessionLocator_1.SessionLocator.Tenant; });
                var list = new GroupByPipe_1.GroupByPipe().transform(temp, "Code");
                _this.CustomPickListsList = [];
                list.forEach(function (value, key) {
                    _this.CustomPickListsList.push(value.key);
                });
            });
        });
    };
    AddEditCustomFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditCustomFieldComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditCustomFieldComponent);
    return AddEditCustomFieldComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomFieldComponent = AddEditCustomFieldComponent;
//# sourceMappingURL=AddEditCustomFieldComponent.js.map