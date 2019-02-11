import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWObjectFieldsDetails } from '../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent'; 
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService'; 
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    selector: 'DWAskUserFiltersComponent',
    moduleId: module.id,
    templateUrl: './DWAskUserFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'ShowRunButton','RunReportCommand','IsDateFilter']
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
    ValidationErrorsList: any[];
    public DWQueryData: DWQueryData;
    IsDateFilter: boolean = false;

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

    RunReport(MyDWQueryData) {
        this.ValidationErrorsList = [];

        if (MyDWQueryData.FirstTime == true) {
            this.DWQueryData = MyDWQueryData.MyData;
        }
        else {
            this.DWQueryData = MyDWQueryData;
        } 
        
        //this.DWQueryData.Filters = this.SelectedFiltersDataSource;
        //if (this.DWQueryData.Filters) {
            this.CheckFiltersValidationsFilters(this.SelectedFiltersDataSource[0]);
        if (this.ValidationErrorsList.length == 0) {
            var QueryData = new DWQueryData();
            QueryData.Columns = this.DWQueryData.Columns;
            QueryData.Filters = this.SelectedFiltersDataSource[0];
            QueryData.PageIndex = this.DWQueryData.PageIndex;
            QueryData.PageSize = this.DWQueryData.PageSize;
            QueryData.ColumnsSort = this.DWQueryData.ColumnsSort;

            this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];

            this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(myResult => {
                if (!myResult.HasError) {
                    //this.rowData = myResult.Result;
                    this.RunReportComplete.emit(myResult.Result.SQLDataResult);
                }
                else {
                    //this.StopBusyIndicator();
                }

                //this.LoadBIReportData();
            });
        }
        else {
            if (MyDWQueryData.FirstTime == true) {
                this.ValidationErrorsList = [];
                this.RunReportComplete.emit("ValidationError");
            }
            else {
                this.RunReportComplete.emit("ValidationError");
            } 
        }
        //} 
        
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
        
    }

    Msg : string = "";
    CheckFiltersValidationsFilters(MyFilter: DWObjectFieldsDetails) {
        if (!MyFilter) {
            return;
        }
        MyFilter.FilterItems.forEach((field) => {
          
            if (field.FilterItems.length == 0) {
                if (field.IsMandatoryFilter == true && AppTool.IsNullOrEmpty(field.TextValue)) {
                    this.ValidationErrorsList.push(field.Name + " filter is required");
                } 
            } 
            else { 
                this.CheckFiltersValidationsFilters(field);
               
            }


        });

        return MyFilter;


    }

    private operators: ObjectFieldOperator[];
    public get Operators() { return this.GetFieldOperators(); }
    public set Operators(newValue: ObjectFieldOperator[]) {
        this.operators = newValue;
    }

    OperationValueChanged(event, Item) {
        Item.Operation = new ObjectFieldOperator(event.Code, event.Name);
    }

    list: ObjectFieldOperator[];
    private GetFieldOperators() {


        this.list = [];
      
            this.list.push(this.beforeOp);
            this.list.push(this.afterOp);
            this.list.push(this.previousOp);
            this.list.push(this.currentOp);
            this.list.push(this.nextOp);
  
        return this.list;
    }

    beforeOp: ObjectFieldOperator = new ObjectFieldOperator("Before", "Before");
    afterOp: ObjectFieldOperator = new ObjectFieldOperator("After", "After");
    previousOp: ObjectFieldOperator = new ObjectFieldOperator("Previous", "Previous");
    currentOp: ObjectFieldOperator = new ObjectFieldOperator("Current", "Current");
    nextOp: ObjectFieldOperator = new ObjectFieldOperator("Next", "Next");

}

export class ObjectFieldOperator {

    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

}
