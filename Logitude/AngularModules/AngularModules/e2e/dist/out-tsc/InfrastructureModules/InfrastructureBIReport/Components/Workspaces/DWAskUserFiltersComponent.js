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
var DWQueryBuilderComponent_1 = require("../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent");
var DWObjectTablePMService_1 = require("../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService");
var DWObjectFieldExtendedPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService");
var DWQueryBuilderService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService");
var DWQueryBuilderHelper_1 = require("../../../../Infrastructure/Helpers/DWQueryBuilderHelper");
var DWQueryData_1 = require("../../../../Common/DataContracts/DWQueryData");
var DWSubQueryPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DWAskUserFiltersComponent = /** @class */ (function () {
    function DWAskUserFiltersComponent() {
        this.SelectedFiltersDataSource = [];
        this.AndOrOps = ["And", "Or"];
        this.Types = ["Fixed Filter", "Ask User"];
        this.BooleanValues = ["Yes", "No", "No Value"];
        this.ShowRunButton = false;
        this.RunReportComplete = new core_1.EventEmitter();
        this.ComputeFiltersComplete = new core_1.EventEmitter();
        this.IsDateFilter = false;
        this.IsFirstTime = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.PageIndex = 0;
        this.PageSize = 1000;
        this.count = 0;
        this.rowData = [];
        this.totalDataLoaded = 10000;
        this.loadingMsg = "Loading";
        this.isParentTenant = false;
        this.Msg = "";
        this.beforeOp = new ObjectFieldOperator("Before", "Before");
        this.afterOp = new ObjectFieldOperator("After", "After");
        this.previousOp = new ObjectFieldOperator("Previous", "Previous");
        this.currentOp = new ObjectFieldOperator("Current", "Current");
        this.nextOp = new ObjectFieldOperator("Next", "Next");
        this.BetweenOp = new ObjectFieldOperator("Between", "Between");
        this._DWQueryBuilderService = new DWQueryBuilderService_1.DWQueryBuilderService();
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper_1.DWQueryBuilderHelper();
    }
    DWAskUserFiltersComponent.prototype.ngOnInit = function () {
        var _this = this;
        var ObsList = [];
        this._DWObjectTablePMService = new DWObjectTablePMService_1.DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService_1.DWObjectFieldExtendedPMService();
        this._DWSubQueryPMService = new DWSubQueryPMService_1.DWSubQueryPMService();
        if (this.RunReportCommand) {
            this.RunReportCommand.subscribe(function (QueryId) {
                //this.SelectedFiltersDataSource = selectedFilters;
                _this.RunReport(QueryId);
            });
        }
        if (this.ComputeFiltersCommand) {
            this.ComputeFiltersCommand.subscribe(function (QueryId) {
                _this.ComputeFilters();
            });
        }
    };
    DWAskUserFiltersComponent.prototype.ComputeFilters = function () {
        this.ComputeFiltersComplete.emit(this.DWQueryData);
    };
    Object.defineProperty(DWAskUserFiltersComponent.prototype, "LoadingMsg", {
        get: function () {
            return this.loadingMsg;
        },
        set: function (value) {
            if (this.loadingMsg != value) {
                this.loadingMsg = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DWAskUserFiltersComponent.prototype.RunReport = function (MyDWQueryData) {
        this.ValidationErrorsList = [];
        this.PageIndex = 0;
        this.PageSize = 1000;
        this.count = 0;
        this.rowData = [];
        if (MyDWQueryData.FirstTime == true) {
            this.DWQueryData = MyDWQueryData.MyData;
        }
        else {
            this.DWQueryData = MyDWQueryData;
        }
        this.CheckFiltersValidationsFilters(this.SelectedFiltersDataSource[0]);
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(this.LoadingMsg);
            this.GetRowDataRecursive();
        }
        else {
            if (MyDWQueryData.FirstTime == true) {
                this.ValidationErrorsList = [];
                this.RunReportComplete.emit({ Msg: "ValidationError" });
            }
            else {
                this.RunReportComplete.emit({ Msg: "ValidationError" });
            }
        }
    };
    DWAskUserFiltersComponent.prototype.GetRowDataRecursive = function () {
        if (this.count < this.totalDataLoaded) {
            var QueryData = new DWQueryData_1.DWQueryData();
            QueryData.Columns = this.DWQueryData.Columns;
            QueryData.Filters = this.SelectedFiltersDataSource[0];
            QueryData.PageIndex = this.PageIndex;
            QueryData.PageSize = this.PageSize;
            QueryData.ColumnsSort = this.DWQueryData.ColumnsSort;
            this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];
            this.GetRowData(QueryData);
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.RunReportComplete.emit({ rowData: this.rowData, Count: this.count, IsParentTenant: this.isParentTenant });
        }
    };
    DWAskUserFiltersComponent.prototype.GetRowData = function (QueryData) {
        var _this = this;
        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.rowData = _this.rowData.concat(myResult.Result.SQLDataResult);
                _this.PageIndex = _this.PageIndex + 1000;
                var dataSize = myResult.Result.SQLDataResult.length;
                _this.isParentTenant = myResult.Result.IsParentTenant;
                if (dataSize == 0) {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.RunReportComplete.emit({ rowData: _this.rowData, Count: _this.count, IsParentTenant: _this.isParentTenant });
                }
                else {
                    _this.count = _this.count + dataSize;
                    _this.LoadingMsg = "Loading " + _this.count;
                    _this.CurrentSession.StartBusyIndicator("Loading " + _this.count);
                    if (_this.count == _this.totalDataLoaded) {
                        _this.PageIndex = _this.PageIndex + 1;
                        _this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(function (myResult) {
                            if (!myResult.HasError) {
                                _this.CurrentSession.StopBusyIndicator();
                                _this.RunReportComplete.emit({ rowData: _this.rowData, Msg: "MT5000", Count: _this.count, IsParentTenant: _this.isParentTenant }); // more than 10000
                            }
                            else {
                                _this.CurrentSession.StopBusyIndicator();
                            }
                        });
                    }
                    else {
                        _this.GetRowDataRecursive();
                    }
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    DWAskUserFiltersComponent.prototype.AddFilterToGroup = function (item) {
        var DWObjectField = new DWQueryBuilderComponent_1.DWObjectFieldsDetails(null, item.MyParentClass);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var tempData = item.FilterItems;
        tempData.push(DWObjectField);
        item.FilterItems = tempData;
    };
    DWAskUserFiltersComponent.prototype.AddGroup = function (item) {
        var DWObjectField = new DWQueryBuilderComponent_1.DWObjectFieldsDetails(null, item.MyParentClass);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWQueryBuilderComponent_1.DWObjectFieldsDetails(null, this.SelectedFiltersDataSource[0].MyParentClass);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        var tempData = item.FilterItems;
        tempData.push(DWObjectField);
        item.FilterItems = tempData;
        //this.SelectedFiltersDataSource.push(DWObjectField);
    };
    DWAskUserFiltersComponent.prototype.DeleteField = function (Item, ListItems) {
        var _this = this;
        ListItems.forEach(function (Myfilter) {
            if (Myfilter.FilterItems.length > 0) { // Myfilter.FilterItems.indexOf(Item) > 
                Myfilter.FilterItems = _this.DeleteField(Item, Myfilter.FilterItems);
                if (Myfilter.FilterItems.length == 0) {
                    ListItems = ListItems.filter(function (a) { return a != Myfilter; });
                }
            }
            else {
                if (Myfilter == Item) {
                    ListItems = ListItems.filter(function (a) { return a != Item; });
                    //return temp;
                }
            }
            //return ListItems;
        });
        return ListItems;
    };
    DWAskUserFiltersComponent.prototype.onDeleteFilterClick = function (item) {
        item.MyParentClass.SelectedFiltersDataSource = this.DeleteField(item, item.MyParentClass.SelectedFiltersDataSource);
        item.MyParentClass.SaveChanges();
        //var temp = this.SelectedFiltersDataSource;
        //this.SelectedFiltersDataSource = this.SelectedFiltersDataSource.filter(a => a != item);
        //var temp = this.MyParentClass.SelectedFieldsDataSource;
    };
    DWAskUserFiltersComponent.prototype.FieldValueChanged = function (DWObjectField) {
    };
    DWAskUserFiltersComponent.prototype.CheckFiltersValidationsFilters = function (MyFilter) {
        var _this = this;
        if (!MyFilter) {
            return;
        }
        MyFilter.FilterItems.forEach(function (field) {
            if (field.FilterItems.length == 0) {
                if (field.OperationCode != "Between") {
                    if (field.IsMandatoryFilter == true && Tools_1.AppTool.IsNullOrEmpty(field.TextValue)) {
                        _this.ValidationErrorsList.push(field.DisplayName.replace('[', '').replace(']', '') + " filter is required");
                    }
                }
                else {
                    var messageError = "";
                    if (field.TextValue) {
                        var values = field.TextValue.split('^');
                        var valueDate1 = values[0];
                        var valueDate2 = values.length > 1 ? values[1] : "";
                        if (!valueDate1 || !valueDate2) {
                            messageError = "From/To is Required";
                        }
                    }
                    else {
                        messageError = "From/To is Required";
                    }
                    if (messageError) {
                        _this.ValidationErrorsList.push(field.DisplayName.replace('[', '').replace(']', '') + " " + messageError);
                    }
                }
            }
            else {
                _this.CheckFiltersValidationsFilters(field);
            }
        });
        return MyFilter;
    };
    Object.defineProperty(DWAskUserFiltersComponent.prototype, "Operators", {
        get: function () { return this.GetFieldOperators(); },
        set: function (newValue) {
            this.operators = newValue;
        },
        enumerable: true,
        configurable: true
    });
    DWAskUserFiltersComponent.prototype.OperationValueChanged = function (event, Item) {
        Item.Operation = new ObjectFieldOperator(event.Code, event.Name);
    };
    DWAskUserFiltersComponent.prototype.GetFieldOperators = function () {
        this.list = [];
        this.list.push(this.beforeOp);
        this.list.push(this.afterOp);
        this.list.push(this.previousOp);
        this.list.push(this.currentOp);
        this.list.push(this.nextOp);
        this.list.push(this.BetweenOp);
        return this.list;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWAskUserFiltersComponent.prototype, "RunReportComplete", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWAskUserFiltersComponent.prototype, "ComputeFiltersComplete", void 0);
    DWAskUserFiltersComponent = __decorate([
        core_1.Component({
            selector: 'DWAskUserFiltersComponent',
            moduleId: module.id,
            templateUrl: './DWAskUserFiltersComponent.html',
            inputs: ['SelectedFiltersDataSource', 'ShowRunButton', 'RunReportCommand', 'IsDateFilter', 'ComputeFiltersCommand', 'IsFirstTime']
        }),
        __metadata("design:paramtypes", [])
    ], DWAskUserFiltersComponent);
    return DWAskUserFiltersComponent;
}());
exports.DWAskUserFiltersComponent = DWAskUserFiltersComponent;
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
//# sourceMappingURL=DWAskUserFiltersComponent.js.map