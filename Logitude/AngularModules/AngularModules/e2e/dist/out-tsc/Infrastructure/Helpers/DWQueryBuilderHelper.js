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
var Tools_1 = require("../Tools");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DWObjectFieldExtendedPMService_1 = require("../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService");
var DWQueryBuilderHelper = /** @class */ (function () {
    function DWQueryBuilderHelper() {
        this.FilterValueChanged = new core_1.EventEmitter();
        window.FactFields = [];
        this.FactFields = [];
        //this.CurrentEntityPM = currentEntityPM;
        //this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        //this.ViewModel = viewModel;
        //this.Type = type;
    }
    DWQueryBuilderHelper.prototype.RestoreFilters = function (Filters) {
        var DWObjectField = new DWObjectFieldsDetails();
        DWObjectField.IsGroup = true;
        var MyFilter = this.GetRestoreFilters(Filters, DWObjectField);
        return MyFilter;
    };
    DWQueryBuilderHelper.prototype.FillAllFactFields = function (FactTableCode) {
        var _this = this;
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService_1.DWObjectFieldExtendedPMService();
        if (window.FactFields.length == 0)
            this._DWObjectFieldPMService.getDWObjectFieldsWithChildrenByDWTableId(FactTableCode).subscribe(function (Result) {
                if (!Result.HasError) {
                    Result.Result.forEach(function (field) {
                        if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) {
                            _this.FactFields.push(field);
                        }
                    });
                    window.FactFields = _this.FactFields;
                    //return this.FactFields
                    //this.DataSource = this.ObsList;
                    //this.AllFieldsWithChildrenDataSource = this.ObsList;
                }
            });
    };
    DWQueryBuilderHelper.prototype.GetRestoreFilters = function (BaseFilter, MyFilter) {
        var _this = this;
        BaseFilter.FilterItems.forEach(function (field) {
            var view = new DWObjectFieldsDetails(field);
            view.FilterChanged = _this.FilterValueChanged;
            if (Tools_1.AppTool.IsNullOrEmpty(view.DimensionTableDisplayName)) {
                view.DimensionTableDisplayName = field.ParentCode;
            }
            if (field.FilterItems.length == 0) {
                if (field.DWObjectTableCode && field.DWObjectTableCode.indexOf("DIM_") != -1) {
                    if (view.Code == '[Full Date]') {
                        view.ParentDataTypeCode = "Date";
                        view.DataTypeCode = "Date";
                    }
                    else {
                        view.ParentDataTypeCode = "LookUp";
                    }
                    //view.ParentDataTypeCode = "LookUp";
                    view.ParentDimTabelName = field.DWObjectTableCode;
                }
                else {
                    view.ParentDataTypeCode = field.DataTypeCode;
                }
            }
            if (field.FilterItems.length == 0) {
                view.TextValue = field.TextValue;
                view.MultiSelectedValueLists = _this.MapMultiSelectedValueLists(field.MultiSelectedValueLists);
                view.Operation = new ObjectFieldOperator(field.OperationCode, field.OperationName);
                MyFilter.FilterItems.push(view);
            }
            else {
                var DWObjectField = new DWObjectFieldsDetails();
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.FilterItems.length;
                DWObjectField.AndOr = field.AndOr;
                _this.GetRestoreFilters(field, DWObjectField);
                MyFilter.FilterItems.push(DWObjectField);
            }
        });
        return MyFilter;
    };
    DWQueryBuilderHelper.prototype.MapMultiSelectedValueLists = function (lists) {
        var result = [];
        if (lists) {
            lists.forEach(function (field) {
                var item = new MultiSelectedValue();
                var i = "";
                var j = 0;
                while (field["Value" + i]) {
                    var valueDetails = new ValueDetails();
                    valueDetails.Header = field["Value" + i].Header;
                    valueDetails.Row = field["Value" + i].Row;
                    item["Value" + i] = valueDetails;
                    j += 1;
                    i = j.toString();
                }
                result.push(item);
            });
            return result;
        }
    };
    return DWQueryBuilderHelper;
}());
exports.DWQueryBuilderHelper = DWQueryBuilderHelper;
var DWObjectFieldsDetails = /** @class */ (function (_super) {
    __extends(DWObjectFieldsDetails, _super);
    function DWObjectFieldsDetails(DWObjectField, ParentClass) {
        if (DWObjectField === void 0) { DWObjectField = null; }
        if (ParentClass === void 0) { ParentClass = null; }
        var _this = _super.call(this) || this;
        _this.Items = [];
        _this.FilterItems = [];
        _this.isGroup = false;
        _this.showBtns = false;
        _this.filterType = "Fixed Filter";
        _this.isSetDefaults = false;
        _this.isMandatoryFilter = false;
        //Load(DWObjectField: any) {
        //    var _DWObjectTablePMService = new DWObjectTablePMService();
        //    var _DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        //    var ObsList = [];
        //    _DWObjectTablePMService.get(DWObjectField.DimensionTableCode).subscribe(myResult => {
        //        if (!myResult.HasError) {
        //            _DWObjectFieldPMService.getDWObjectFieldsByDWTableId(myResult.Result.Code).subscribe(Result => {
        //                if (!Result.HasError) {
        //                    Result.Result.forEach((field) => {
        //                        if (field.DisplayInQueryBuilder == true) {
        //                            var view = new DWObjectFieldsDetails(field, this.MyParentClass);
        //                            view.ParentDataTypeCode = DWObjectField.DataTypeCode;
        //                            if (!AppTool.IsNullOrEmpty(DWObjectField.Code)) {
        //                                view.DisplayName = '[' + (DWObjectField.Code.replace('[', '').replace(']', '') + view.Code.replace('[', '').replace(']', '')) + ']';//.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
        //                            }
        //                            else if (!AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
        //                                view.DisplayName = DWObjectField.DisplayName;
        //                            }
        //                            else {
        //                                view.DisplayName = '[' + (DWObjectField.Name + view.Code.replace('[', '').replace(']', '')) + ']';//.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
        //                            }
        //                            view.ParentCode = DWObjectField.Code;
        //                            view.ParentDimTabelName = DWObjectField.DimensionTableCode;
        //                            ObsList.push(view);
        //                        }
        //                        //this.ObsListAll.push(view);
        //                    });
        //                    this.Items = ObsList;
        //                    this.IsViewTree = true;
        //                }
        //            });
        //        }
        //    });
        //}
        _this.ShowSampleDateCommand = new core_1.EventEmitter();
        _this.startsWithOp = new ObjectFieldOperator("StartsWith", "Starts With");
        _this.equalsOp = new ObjectFieldOperator("Equals", "Equals to");
        _this.notEqualsOp = new ObjectFieldOperator("NotEqual", "Not Equal to");
        _this.largerThanOp = new ObjectFieldOperator("LargerThan", "Greater Than");
        _this.lessThanOp = new ObjectFieldOperator("LessThan", "Less Than");
        _this.greaterThanOrEqualOp = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
        _this.lessThanOrEqualOp = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
        _this.BetweenOp = new ObjectFieldOperator("Between", "Between");
        _this.IsNullOp = new ObjectFieldOperator("IsNull", "Is Empty");
        _this.IsNotNullOp = new ObjectFieldOperator("IsNotNull", "Has Value");
        _this.beforeOp = new ObjectFieldOperator("Before", "Before");
        _this.afterOp = new ObjectFieldOperator("After", "After");
        _this.previousOp = new ObjectFieldOperator("Previous", "Previous");
        _this.currentOp = new ObjectFieldOperator("Current", "Current");
        _this.nextOp = new ObjectFieldOperator("Next", "Next");
        _this.BaseDWObjectField = DWObjectField;
        if (ParentClass != null) {
            _this.MyParentClass = ParentClass;
            _this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        }
        if (DWObjectField != null) {
            _this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
            if (!_this.LOVAdditionalColumns && ParentClass && ParentClass.AllFieldsDataSource) {
                var field = ParentClass.AllFieldsDataSource.filter(function (a) { return a.DWObjectTableCode == DWObjectField.DWObjectTableCode && a.Code == DWObjectField.Code && a.Name == DWObjectField.Name; })[0];
                if (field) {
                    _this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns = field.LOVAdditionalColumns;
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
                _this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
            }
            else {
                _this.DimensionTableDisplayName = DWObjectField.ParentCode;
            }
            _this.Name = DWObjectField.Name;
            _this.Code = DWObjectField.Code;
            _this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
            _this.DimensionTableCode = DWObjectField.DimensionTableCode;
            _this.DataTypeCode = DWObjectField.DataTypeCode;
            _this.IsCustom = DWObjectField.IsCustom;
            if (DWObjectField.FilterItems.length == 0) {
                _this.DisplayName = _this.ComputeDisplayName(DWObjectField); //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
            }
            _this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
            _this.IsMeasurement = DWObjectField.IsMeasurement;
            _this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
            if (DWObjectField.FilterType) {
                _this.FilterType = DWObjectField.FilterType;
            }
            if (DWObjectField.IsSetDefaults) {
                _this.IsSetDefaults = DWObjectField.IsSetDefaults;
            }
            if (DWObjectField.IsMandatoryFilter) {
                _this.IsMandatoryFilter = DWObjectField.IsMandatoryFilter;
            }
            //this.Name = DWObjectField.Name;
        }
        return _this;
    }
    DWObjectFieldsDetails.prototype.ComputeDisplayName = function (DWObjectField) {
        //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        var Displayname = DWObjectField.DisplayName;
        if (Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
            if (DWObjectField.DWObjectTableCode.indexOf("DIM_") != -1) {
                Displayname = DWObjectField.Name + ' ' + DWObjectField.Code;
            }
            else {
                Displayname = DWObjectField.Code;
            }
        }
        return Displayname;
    };
    Object.defineProperty(DWObjectFieldsDetails.prototype, "LOVAdditionalColumns", {
        get: function () { return this.lOVAdditionalColumns; },
        set: function (newValue) { this.lOVAdditionalColumns = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Category1", {
        get: function () { return this.category1; },
        set: function (newValue) { this.category1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Category2", {
        get: function () { return this.category2; },
        set: function (newValue) { this.category2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { this.indexOrder = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsGroup", {
        get: function () { return this.isGroup; },
        set: function (newValue) { this.isGroup = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsPrimaryKey", {
        get: function () { return this.isPrimaryKey; },
        set: function (newValue) { this.isPrimaryKey = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ShowBtns", {
        get: function () { return this.showBtns; },
        set: function (newValue) { this.showBtns = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsViewTree", {
        get: function () { return this.isViewTree; },
        set: function (newValue) { this.isViewTree = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "HasTree", {
        get: function () { return this.hasTree; },
        set: function (newValue) { this.hasTree = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsCustom", {
        get: function () { return this.isCustom; },
        set: function (newValue) { this.isCustom = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DisplayName", {
        get: function () { return this.displayname; },
        set: function (newValue) { this.displayname = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DimensionTableDisplayName", {
        get: function () { return this.dimensionTableDisplayName; },
        set: function (newValue) { this.dimensionTableDisplayName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ParentCode", {
        get: function () { return this.parentcode; },
        set: function (newValue) { this.parentcode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ParentDimTabelName", {
        get: function () { return this.parentDimTabelName; },
        set: function (newValue) { this.parentDimTabelName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DWObjectTableCode", {
        get: function () { return this.dWObjectTableCode; },
        set: function (newValue) { if (this.dWObjectTableCode != newValue) {
            this.dWObjectTableCode = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsMeasurement", {
        get: function () { return this.isMeasurement; },
        set: function (newValue) { if (this.isMeasurement != newValue) {
            this.isMeasurement = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "AggregationTypeCode", {
        get: function () { return this.aggregationTypeCode; },
        set: function (newValue) { if (this.aggregationTypeCode != newValue) {
            this.aggregationTypeCode = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DataTypeCode", {
        get: function () { return this.dataTypeCode; },
        set: function (newValue) {
            var _this = this;
            //if (this.dataTypeCode != newValue) {
            this.dataTypeCode = newValue;
            if (this.dataTypeCode == "Boolean") {
                this.TextValue = false;
            }
            if (this.dataTypeCode == "LookUp" || this.dataTypeCode == "Dimension") {
                this.HasTree = true;
                if (this.MyParentClass) {
                    var MyTable = this.MyParentClass.AllTables.filter(function (a) { return a.Code == _this.DimensionTableCode; });
                    if (MyTable && MyTable.length > 0) {
                        this.Code = MyTable[0].DefaultFilterBy;
                        this.DWObjectTableCode = MyTable[0].Code;
                        this.DisplayName = this.ComputeDisplayName(this); //(AppTool.IsNullOrEmpty(this.DisplayName)) ? (this.DWObjectTableCode + ' ' + this.Code) : (this.DisplayName);
                        this.ParentDimTabelName = this.DimensionTableCode;
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.DimensionTableDisplayName)) {
                            this.DimensionTableDisplayName = this.DimensionTableDisplayName;
                        }
                        else {
                            this.DimensionTableDisplayName = this.ParentCode;
                        }
                    }
                }
            }
            else {
                this.HasTree = false;
            }
            //}
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ParentDataTypeCode", {
        get: function () { return this.parentDataTypeCode; },
        set: function (newValue) {
            if (this.parentDataTypeCode != newValue) {
                this.parentDataTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DimensionTableCode", {
        get: function () { return this.dimensionTableCode; },
        set: function (newValue) { if (this.dimensionTableCode != newValue) {
            this.dimensionTableCode = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Operators", {
        get: function () { return this.GetFieldOperators(this); },
        set: function (newValue) {
            this.operators = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "TextValue", {
        get: function () {
            return this.textValue;
        },
        set: function (newValue) {
            var _this = this;
            if (this.textValue != newValue) {
                //if (this.textValue != null && this.textValue != undefined) {
                this.textValue = newValue;
                if (this.ParentDataTypeCode == "DateTime" || this.ParentDataTypeCode == "Date") {
                    var timerToken = setTimeout(function () {
                        _this.ShowSampleDateCommand.emit(_this);
                    }, 0);
                }
                if (this.FilterChanged) {
                    this.FilterChanged.emit("FilterValueChanged");
                }
                //}
                //else {
                //    this.textValue = newValue;
                //}
                if (this.MyParentClass) {
                    this.MyParentClass.SaveChanges();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "MultiSelectedValueLists", {
        get: function () {
            return this.multiSelectedValueLists;
        },
        set: function (newValue) {
            this.multiSelectedValueLists = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "OperationName", {
        get: function () { return this.operationName; },
        set: function (newValue) {
            if (this.operationName != newValue) {
                this.operationName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "OperationCode", {
        get: function () { return this.operationCode; },
        set: function (newValue) {
            if (this.operationCode != newValue) {
                this.operationCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Operation", {
        get: function () {
            if (!this.operation) {
                if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText") && Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                    this.OperationCode = "StartsWith";
                    this.OperationName = "Starts With";
                    return this.operation;
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
                        this.operation = new ObjectFieldOperator("Equals", "Equals to");
                    }
                    this.OperationCode = "Equals";
                    this.OperationName = "Equals to";
                    return this.operation;
                }
            }
            else {
                return this.operation;
            }
        },
        set: function (newValue) {
            this.OperationCode = newValue.Code;
            this.OperationName = newValue.Name;
            this.operation = newValue;
            this.FilterChanged.emit("FilterValueChanged");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "AndOr", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(this.andOr)) {
                return "And";
            }
            return this.andOr;
        },
        set: function (newValue) {
            this.andOr = newValue;
            if (this.MyParentClass) {
                this.MyParentClass.SaveChanges();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "FilterType", {
        get: function () { return this.filterType; },
        set: function (newValue) { this.filterType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsSetDefaults", {
        get: function () { return this.isSetDefaults; },
        set: function (newValue) { if (this.isSetDefaults != newValue) {
            this.isSetDefaults = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsMandatoryFilter", {
        get: function () { return this.isMandatoryFilter; },
        set: function (newValue) { if (this.isMandatoryFilter != newValue) {
            this.isMandatoryFilter = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    //IsMandatoryFilter: boolean = false;
    //IsSetDefaults: boolean = false;
    DWObjectFieldsDetails.prototype.FilterTypeChanged = function (Value) {
        this.FilterType = Value;
    };
    DWObjectFieldsDetails.prototype.OpenFilterSettings = function () {
        //var windowArgs: any = {};
        //windowArgs.IsMandatoryFilter = this.IsMandatoryFilter;
        //windowArgs.IsSetDefaults = this.IsSetDefaults;
        //var logWindow = new LogitudeWindow();
        //logWindow.WindowArgs = windowArgs;
        //logWindow.Width = 500;
        //logWindow.Height = 260;
        //logWindow.Title = "Ask User Settings";
        ////logWindow.DataContext = this;
        ////logWindow.IsShowCloseButton = true;
        //logWindow.Show('./CommonModules/CommonOthers/Components/DWQueryBuilder/DWFilterSettings');
        //logWindow.WindowClosed.subscribe(($event: string) => {
        //    if ($event) {
        //        var MySettings = $event.split(',');
        //        if (MySettings[0] == "true") {
        //            this.IsMandatoryFilter = true;
        //        }
        //        else {
        //            this.IsMandatoryFilter = false;
        //        }
        //        if (MySettings[1] == "true") {
        //            this.IsSetDefaults = true;
        //        }
        //        else {
        //            this.IsSetDefaults = false;
        //        }
        //    }
        //});
    };
    DWObjectFieldsDetails.prototype.OperationValueChanged = function (operation) {
        if (this.Operation.Code == this.IsNullOp.Code || this.Operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = "";
            this.MultiSelectedValueLists = [];
        }
        this.Operation = operation;
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = operation.Code;
        }
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
            if (this.MyParentClass) {
                this.MyParentClass.SaveChanges();
            }
        }
    };
    DWObjectFieldsDetails.prototype.LoadItems = function (DWObjectField) {
        //if (this.IsViewTree) {
        //    this.IsViewTree = false;
        //}
        //else {
        //    if (this.Items.length == 0) {
        //        this.Load(DWObjectField);
        //    }
        //    else this.IsViewTree = true;
        //}
    };
    DWObjectFieldsDetails.prototype.onTextChange = function (value) {
        //this.TextValue = value;
        if (this.DataTypeCode == "Boolean") {
            if (value == "Yes") {
                this.TextValue = true;
            }
            else if (value == "No") {
                this.TextValue = false;
            }
            else {
                this.TextValue = null;
            }
        }
        else {
            this.TextValue = value;
        }
        this.ShowSampleDateCommand.emit(this);
    };
    //onTextChange(value) {
    //    this.TextValue = value;
    //}
    DWObjectFieldsDetails.prototype.AndOrOpsChanged = function (value) {
        this.AndOr = value;
    };
    DWObjectFieldsDetails.prototype.OnMouseOver = function (event) {
        //console.log("Over");
        var e = event.toElement; // || event.relatedTarget;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    };
    DWObjectFieldsDetails.prototype.OnMouseOut = function (event) {
        //console.log("Out");
        var e = event.toElement; // || event.relatedTarget;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == true) {
            this.ShowBtns = false;
        }
    };
    DWObjectFieldsDetails.prototype.preventInnerHover = function (event) {
        event.stopPropagation();
    };
    DWObjectFieldsDetails.prototype.AddFilterToGroup = function () {
        var DWObjectField = new DWObjectFieldsDetails();
        DWObjectField.IndexOrder = this.FilterItems.length;
        //this.Name = DWObjectField.Name;
        //this.Code = DWObjectField.Code;
        //this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        //this.DataTypeCode = DWObjectField.DataTypeCode;
        //this.DimensionTableCode = DWObjectField.DimensionTableCode;
        //this.DisplayName = DWObjectField.Code;
        //this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        //this.IsMeasurement = DWObjectField.IsMeasurement;
        //this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        //this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        this.FilterItems.push(DWObjectField);
    };
    DWObjectFieldsDetails.prototype.AddGroup = function (Father) {
        var DWObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWObjectField.IsGroup = true;
        if (this.MyParentClass) {
            DWObjectField.IndexOrder = Father.MyParentClass.SelectedFiltersDataSource.length;
        }
        var DWInnerObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        if (this.MyParentClass) {
            Father.MyParentClass.SelectedFiltersDataSource.push(DWObjectField);
        }
    };
    DWObjectFieldsDetails.prototype.FieldValueChanged = function (DWObjectField) {
        //this.MyParentClass = ParentClass;
        this.TextValue = "";
        this.MultiSelectedValueLists = [];
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        if (!Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
            this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
        }
        else {
            this.DimensionTableDisplayName = DWObjectField.ParentCode;
        }
        this.DisplayName = this.ComputeDisplayName(DWObjectField); //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        this.IsMeasurement = DWObjectField.IsMeasurement;
        this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
        if (this.DWObjectTableCode.indexOf("DIM_") != -1) {
            this.ParentDataTypeCode = "LookUp";
            this.ParentDimTabelName = DWObjectField.DWObjectTableCode;
        }
        else {
            this.ParentDataTypeCode = DWObjectField.DataTypeCode;
            this.ParentDimTabelName = DWObjectField.ParentDimTabelName;
        }
        //this.ParentDataTypeCode = DWObjectField.DataTypeCode;
        this.Operators = this.GetFieldOperators(this);
        if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText")) {
            this.Operation = new ObjectFieldOperator("StartsWith", "Starts With");
        }
        else {
            this.Operation = new ObjectFieldOperator("Equals", "Equals to");
        }
        //this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        //var Filters = DWObjectField.MyParentClass.SelectedFiltersDataSource;
        //DWObjectField.MyParentClass.SelectedFiltersDataSource = [];
        //DWObjectField.MyParentClass.SelectedFiltersDataSource = Filters;
        //this.MyParentClass.SelectedFiltersDataSource.where
    };
    DWObjectFieldsDetails.prototype.onDeleteFilterClick = function () {
    };
    DWObjectFieldsDetails.prototype.GetFieldOperators = function (field) {
        this.list = [];
        if (field.ParentDataTypeCode == "Text" || field.ParentDataTypeCode == "nText") {
            this.list.push(this.equalsOp);
            this.list.push(this.startsWithOp);
            this.list.push(this.IsNullOp);
            this.list.push(this.IsNotNullOp);
        }
        if (field.ParentDataTypeCode == "Integer" || field.ParentDataTypeCode == "UnsInteger"
            || field.ParentDataTypeCode == "Double" || field.ParentDataTypeCode == "SigDouble"
            || field.ParentDataTypeCode == "Decimal" || field.ParentDataTypeCode == "UnsDecimal"
            || field.ParentDataTypeCode == "DateTime" || field.ParentDataTypeCode == "Date") {
            this.list.push(this.largerThanOp);
            this.list.push(this.lessThanOp);
            this.list.push(this.equalsOp);
            this.list.push(this.greaterThanOrEqualOp);
            this.list.push(this.lessThanOrEqualOp);
        }
        if (field.ParentDataTypeCode == "LookUp" || field.ParentDataTypeCode == "Dimension" || field.ParentDataTypeCode == "PickList") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
            this.list.push(this.IsNullOp);
            this.list.push(this.IsNotNullOp);
        }
        if (field.ParentDataTypeCode == "Boolean") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
        }
        if (field.ParentDataTypeCode == "DateTime" || field.ParentDataTypeCode == "Date") {
            this.list.push(this.beforeOp);
            this.list.push(this.afterOp);
            this.list.push(this.previousOp);
            this.list.push(this.currentOp);
            this.list.push(this.nextOp);
        }
        return this.list;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWObjectFieldsDetails.prototype, "ShowSampleDateCommand", void 0);
    return DWObjectFieldsDetails;
}(BaseComponent_1.BaseComponent));
exports.DWObjectFieldsDetails = DWObjectFieldsDetails;
var ObjectFieldOperator = /** @class */ (function () {
    function ObjectFieldOperator(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(ObjectFieldOperator.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldOperator.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    return ObjectFieldOperator;
}());
exports.ObjectFieldOperator = ObjectFieldOperator;
var DWFieldsGroup = /** @class */ (function () {
    function DWFieldsGroup(Key, FieldsList) {
        this.detailsIcon = "./Images/CellIcons/Arrowup.png";
        this.isDetailesOpened = false;
        this.Key = Key;
        this.FieldsList = FieldsList;
    }
    Object.defineProperty(DWFieldsGroup.prototype, "Key", {
        get: function () { return this.key; },
        set: function (newValue) { this.key = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWFieldsGroup.prototype, "FieldsList", {
        get: function () { return this.fieldsList; },
        set: function (newValue) { this.fieldsList = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWFieldsGroup.prototype, "DetailsIcon", {
        get: function () { return this.detailsIcon; },
        set: function (newValue) { this.detailsIcon = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWFieldsGroup.prototype, "IsDetailesOpened", {
        get: function () { return this.isDetailesOpened; },
        set: function (newValue) { this.isDetailesOpened = newValue; },
        enumerable: true,
        configurable: true
    });
    DWFieldsGroup.prototype.GroupClicked = function () {
        this.IsDetailesOpened = !this.IsDetailesOpened;
        if (!this.IsDetailesOpened) {
            this.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
        }
        else {
            this.DetailsIcon = "./Images/CellIcons/Arrowup.png";
        }
    };
    return DWFieldsGroup;
}());
exports.DWFieldsGroup = DWFieldsGroup;
var MultiSelectedValue = /** @class */ (function () {
    function MultiSelectedValue() {
    }
    return MultiSelectedValue;
}());
exports.MultiSelectedValue = MultiSelectedValue;
var ValueDetails = /** @class */ (function () {
    function ValueDetails() {
    }
    return ValueDetails;
}());
exports.ValueDetails = ValueDetails;
//# sourceMappingURL=DWQueryBuilderHelper.js.map