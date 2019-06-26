import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWObjectFieldsDetails } from '../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';




declare var window: any;


@Component({
    selector: 'DWQueryBuilderFilters',
    moduleId: module.id,
    templateUrl: './DWQueryBuilderFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'DataContext', 'SelectedFiltersDataSourceChanged']
})

export class DWQueryBuilderFiltersComponent implements OnInit {

    SelectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    //allFieldsWithChildrenDataSource: DWObjectFieldsDetails[] = [];
    public AndOrOps = ["And", "Or"];

    public BooleanValues = ["Yes", "No", "No Value"];
    DataContext: any;
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public SelectedFiltersDataSourceChanged: EventEmitter<any>;
    //@Output() DataSourceChanged: EventEmitter<any> = new EventEmitter();
    //public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    //get AllFieldsWithChildrenDataSource() { return this.allFieldsWithChildrenDataSource; }
    //set AllFieldsWithChildrenDataSource(value: any[]) {
    //    if (value && this.allFieldsWithChildrenDataSource != value) { 
    //        this.allFieldsWithChildrenDataSource = value;
    //        this.cd.detectChanges();
    //    }
    //}


    constructor(private cd: ChangeDetectorRef) {
        //this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
        var ObsList = [];

    }

    ngOnInit() {
        var ObsList = [];
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
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
            this.SelectedFiltersDataSourceChanged.subscribe((res) => {
                this.SelectedFiltersDataSource = res;
                //alert(res.length);
            });
        }
    }

    AddFilterToGroup(item) {
        var DWObjectField = new DWObjectFieldsDetails(null, item.MyParentClass);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var tempData = item.FilterItems;
        tempData.push(DWObjectField);
        item.FilterItems = tempData;
    }
    AddGroup(item) {
        var DWObjectField = new DWObjectFieldsDetails(null, item.MyParentClass);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, this.SelectedFiltersDataSource[0].MyParentClass);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        var tempData = item.FilterItems;
        tempData.push(DWObjectField);
        item.FilterItems = tempData;
        //this.SelectedFiltersDataSource.push(DWObjectField);
    }

    DeleteField(Item: DWObjectFieldsDetails, ListItems: DWObjectFieldsDetails[]) {


        ListItems.forEach((Myfilter) => {
            if (Myfilter.FilterItems.length > 0) {// Myfilter.FilterItems.indexOf(Item) > 
                Myfilter.FilterItems = this.DeleteField(Item, Myfilter.FilterItems);
                if (Myfilter.FilterItems.length == 0) {
                    ListItems = ListItems.filter(a => a != Myfilter);
                }
            }
            else {
                if (Myfilter == Item) {
                    ListItems = ListItems.filter(a => a != Item);
                    //return temp;
                }
            }
            //return ListItems;
        });
        return ListItems;
    }

    onDeleteFilterClick(item) {
        item.MyParentClass.SelectedFiltersDataSource = this.DeleteField(item, item.MyParentClass.SelectedFiltersDataSource);
        item.MyParentClass.ClearData();//SaveChanges();
        //var temp = this.SelectedFiltersDataSource;
        //this.SelectedFiltersDataSource = this.SelectedFiltersDataSource.filter(a => a != item);
        //var temp = this.MyParentClass.SelectedFieldsDataSource;
    }

    FieldValueChanged(DWObjectField: DWObjectFieldsDetails) {
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
    }
}
