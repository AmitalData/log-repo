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
var DWQueryBuilderFiltersComponent = /** @class */ (function () {
    //@Output() DataSourceChanged: EventEmitter<any> = new EventEmitter();
    //public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    //get AllFieldsWithChildrenDataSource() { return this.allFieldsWithChildrenDataSource; }
    //set AllFieldsWithChildrenDataSource(value: any[]) {
    //    if (value && this.allFieldsWithChildrenDataSource != value) { 
    //        this.allFieldsWithChildrenDataSource = value;
    //        this.cd.detectChanges();
    //    }
    //}
    function DWQueryBuilderFiltersComponent(cd) {
        this.cd = cd;
        this.SelectedFiltersDataSource = [];
        //allFieldsWithChildrenDataSource: DWObjectFieldsDetails[] = [];
        this.AndOrOps = ["And", "Or"];
        this.BooleanValues = ["Yes", "No", "No Value"];
        //this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
        var ObsList = [];
    }
    DWQueryBuilderFiltersComponent.prototype.ngOnInit = function () {
        var _this = this;
        var ObsList = [];
        this._DWObjectTablePMService = new DWObjectTablePMService_1.DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService_1.DWObjectFieldExtendedPMService();
        //window.FactFields.forEach((field) => {
        //    if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) {
        //        var view = new DWObjectFieldsDetails(field, this.DataContext);
        //        view.ParentDataTypeCode = field.DataTypeCode;
        //        ObsList.push(view);
        //    }
        //});
        //this.AllFieldsWithChildrenDataSource = ObsList;
        //this.DataSourceChanged.emit(ObsList);
        //this._DWObjectTablePMService.get("Fact_Shipments").subscribe(myResult => {
        //    this._DWObjectFieldPMService.getDWObjectFieldsWithChildrenByDWTableId(myResult.Result.Code).subscribe(Result => {
        //        if (!Result.HasError) {
        //            Result.Result.forEach((field) => {
        //                if (field.DisplayInQueryBuilder == true) {
        //                    var view = new DWObjectFieldsDetails(field, this.DataContext);
        //                    view.DisplayName = field.DisplayName;
        //                    view.ParentDataTypeCode = field.DataTypeCode;
        //                    ObsList.push(view); 
        //                }
        //            });
        //            //this.DataSource = this.ObsList;
        //            this.AllFieldsWithChildrenDataSource = ObsList;
        //        }
        //    });
        //});
        if (this.SelectedFiltersDataSourceChanged) {
            this.SelectedFiltersDataSourceChanged.subscribe(function (res) {
                _this.SelectedFiltersDataSource = res;
                //alert(res.length);
            });
        }
    };
    DWQueryBuilderFiltersComponent.prototype.AddFilterToGroup = function (item) {
        var DWObjectField = new DWQueryBuilderComponent_1.DWObjectFieldsDetails(null, item.MyParentClass);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var tempData = item.FilterItems;
        tempData.push(DWObjectField);
        item.FilterItems = tempData;
    };
    DWQueryBuilderFiltersComponent.prototype.AddGroup = function (item) {
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
    DWQueryBuilderFiltersComponent.prototype.DeleteField = function (Item, ListItems) {
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
    DWQueryBuilderFiltersComponent.prototype.onDeleteFilterClick = function (item) {
        item.MyParentClass.SelectedFiltersDataSource = this.DeleteField(item, item.MyParentClass.SelectedFiltersDataSource);
        item.MyParentClass.ClearData(); //SaveChanges();
        //var temp = this.SelectedFiltersDataSource;
        //this.SelectedFiltersDataSource = this.SelectedFiltersDataSource.filter(a => a != item);
        //var temp = this.MyParentClass.SelectedFieldsDataSource;
    };
    DWQueryBuilderFiltersComponent.prototype.FieldValueChanged = function (DWObjectField) {
        ////this.MyParentClass = ParentClass;
        //this.Name = DWObjectField.Name;
        //this.Code = DWObjectField.Code;
        //this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        //this.DataTypeCode = DWObjectField.DataTypeCode;
        //this.DimensionTableCode = DWObjectField.DimensionTableCode;
        //this.DisplayName = DWObjectField.Code;
        //this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        //this.IsMeasurement = DWObjectField.IsMeasurement;
        //this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        //this.ParentDataTypeCode = DWObjectField.DataTypeCode;
        //this.ParentDimTabelName = DWObjectField.ParentDimTabelName;
        ////this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        ////var Filters = DWObjectField.MyParentClass.SelectedFiltersDataSource;
        ////DWObjectField.MyParentClass.SelectedFiltersDataSource = [];
        ////DWObjectField.MyParentClass.SelectedFiltersDataSource = Filters;
        ////this.MyParentClass.SelectedFiltersDataSource.where
    };
    DWQueryBuilderFiltersComponent = __decorate([
        core_1.Component({
            selector: 'DWQueryBuilderFilters',
            moduleId: module.id,
            templateUrl: './DWQueryBuilderFiltersComponent.html',
            inputs: ['SelectedFiltersDataSource', 'DataContext', 'SelectedFiltersDataSourceChanged']
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DWQueryBuilderFiltersComponent);
    return DWQueryBuilderFiltersComponent;
}());
exports.DWQueryBuilderFiltersComponent = DWQueryBuilderFiltersComponent;
//# sourceMappingURL=DWQueryBuilderFiltersComponent.js.map