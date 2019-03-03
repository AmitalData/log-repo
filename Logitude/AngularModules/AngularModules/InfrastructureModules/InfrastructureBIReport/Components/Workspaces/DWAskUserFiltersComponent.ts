import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWObjectFieldsDetails } from '../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'DWAskUserFiltersComponent',
    moduleId: module.id,
    templateUrl: './DWAskUserFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'ShowRunButton', 'RunReportCommand', 'IsDateFilter', 'ComputeFiltersCommand','IsFirstTime']
})

export class DWAskUserFiltersComponent implements OnInit {

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
    @Output() ComputeFiltersComplete = new EventEmitter();
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    public _DWSubQueryPMService: DWSubQueryPMService;
    ValidationErrorsList: any[];
    public DWQueryData: DWQueryData;
    IsDateFilter: boolean = false;
    IsFirstTime: boolean = false;
    public ComputeFiltersCommand: EventEmitter<any>;

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
        if (this.ComputeFiltersCommand) {
            this.ComputeFiltersCommand.subscribe((QueryId) => {
                this.ComputeFilters();
            });
        }
    }

    ComputeFilters() {
        this.ComputeFiltersComplete.emit(this.DWQueryData);
    }

    private PageIndex = 0;
    private PageSize = 10000;
    private count = 0;
    private rowData = []; 

    RunReport(MyDWQueryData) {
        this.ValidationErrorsList = [];
        this.PageIndex = 0;
        this.PageSize = 10000;
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
            SessionLocator.CurrentSession.StartBusyIndicator("Loading " + this.count);
            this.GetRowDataRecursive();
        }
        else {
            if (MyDWQueryData.FirstTime == true) {
                this.ValidationErrorsList = [];
                this.RunReportComplete.emit({ Msg:"ValidationError"});
            }
            else {
                this.RunReportComplete.emit({ Msg: "ValidationError" });
            }
        }
    }
    GetRowDataRecursive() {
        if (this.count <50000) {
            var QueryData = new DWQueryData();
            QueryData.Columns = this.DWQueryData.Columns;
            QueryData.Filters = this.SelectedFiltersDataSource[0];
            QueryData.PageIndex = this.PageIndex;
            QueryData.PageSize = this.PageSize;
            QueryData.ColumnsSort = this.DWQueryData.ColumnsSort;
            this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];
            this.GetRowData(QueryData);
        }
        else {
            SessionLocator.CurrentSession.StopBusyIndicator();
            this.RunReportComplete.emit({ rowData: this.rowData, Count: this.count});
        }
    }
    GetRowData(QueryData: DWQueryData) {
        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(myResult => {
            if (!myResult.HasError) {
                this.rowData = this.rowData.concat(myResult.Result.SQLDataResult);
                this.PageIndex = this.PageIndex + 10000;
                var dataSize = myResult.Result.SQLDataResult.length;
                if (dataSize == 0) {
                    SessionLocator.CurrentSession.StopBusyIndicator();
                    this.RunReportComplete.emit({ rowData: this.rowData , Count: this.count});
                }
                else {
                    this.count = this.count + dataSize;
                    if (this.count == 50000) {
                        this.PageIndex = this.PageIndex + 1;
                        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(myResult => {
                            if (!myResult.HasError) {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                                this.RunReportComplete.emit({ rowData: this.rowData, Msg: "MT5000", Count: this.count});// more than 50000
                            }
                            else {
                                SessionLocator.CurrentSession.StopBusyIndicator();
                            }
                        });
                    }
                    else {
                        this.GetRowDataRecursive();
                    }
                }
            }
            else {
                SessionLocator.CurrentSession.StopBusyIndicator();
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

    }

    Msg: string = "";
    CheckFiltersValidationsFilters(MyFilter: DWObjectFieldsDetails) {
        if (!MyFilter) {
            return;
        }
        MyFilter.FilterItems.forEach((field) => {

            if (field.FilterItems.length == 0) {
                if (field.IsMandatoryFilter == true && AppTool.IsNullOrEmpty(field.TextValue)) {
                    this.ValidationErrorsList.push(field.DisplayName.replace('[', '').replace(']', '') + " filter is required");
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
