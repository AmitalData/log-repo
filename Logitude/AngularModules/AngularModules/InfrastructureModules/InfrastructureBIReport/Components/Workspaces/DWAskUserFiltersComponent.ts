import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWObjectFieldsDetails } from '../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent'; 
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService'; 
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';

@Component({
    selector: 'DWAskUserFiltersComponent',
    moduleId: module.id,
    templateUrl: './DWAskUserFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'ShowRunButton','RunReportCommand']
})

export class DWAskUserFiltersComponent implements OnInit{

    SelectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    AllFieldsWithChildrenDataSource: DWObjectFieldsDetails[];
    public AndOrOps = ["And", "Or"];
    public Types = ["Fixed Filter", "Ask User"];
    DataContext: any;
    ShowRunButton: boolean = false;
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public RunReportCommand: EventEmitter<any>;
    @Output() RunReportComplete = new EventEmitter();
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    public _DWSubQueryPMService: DWSubQueryPMService;
     
    public DWQueryData: DWQueryData;

    constructor() {
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
    }

    ngOnInit() {
        var ObsList = [];
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        this._DWSubQueryPMService = new DWSubQueryPMService();

        if (this.RunReportCommand) {
            this.RunReportCommand.subscribe((QueryId) => {
                //this.SelectedFiltersDataSource = selectedFilters;
                this.RunReport(QueryId);
            });
        }
    }

    RunReport(DWQueryId) {
        this._DWSubQueryPMService.getByQueryId(DWQueryId).subscribe(myResult => {
            if (!myResult.HasError) {
                this.DWQueryData = myResult.Result;

                if (this.DWQueryData.Filters) {
                    var MyFilter = this._DWQueryBuilderHelper.RestoreFilters(this.DWQueryData.Filters);
                    var temp = [];
                    temp.push(MyFilter);
                    this.SelectedFiltersDataSource = temp;
                    var QueryData = new DWQueryData();
                    QueryData.Columns = this.DWQueryData.Columns;
                    QueryData.Filters = this.DWQueryData.Filters;
                    QueryData.PageIndex = 0;
                    QueryData.PageSize = 100;
                    this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(myResult => {
                        if (!myResult.HasError) {
                            //this.rowData = myResult.Result;
                            this.RunReportComplete.emit(myResult.Result);
                        }
                        else {
                            //this.StopBusyIndicator();
                        }

                        //this.LoadBIReportData();
                    });
                } 
            }
        });
       
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
        item.MyParentClass.SaveChanges();
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
