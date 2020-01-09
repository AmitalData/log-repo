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
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'DWAskUserFiltersComponent',
    moduleId: module.id,
    templateUrl: './DWAskUserFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'ShowRunButton', 'RunReportCommand', 'IsDateFilter', 'ComputeFiltersCommand', 'IsFirstTime', 'IsStaticFilter', 'IsStaticDateFilter', 'SelectedDynamicFiltersDataSource', 'SelectedFixedFiltersDataSource','ShowFixedFilters']
})

export class DWAskUserFiltersComponent extends BaseComponent implements OnInit {

    selectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    SelectedDynamicFiltersDataSource: DWObjectFieldsDetails[] = [];
    SelectedFixedFiltersDataSource: DWObjectFieldsDetails[] = [];
    AllFieldsWithChildrenDataSource: DWObjectFieldsDetails[];
    public AndOrOps = ["And", "Or"];
    public Types = ["Fixed Filter", "Ask User"];
    public BooleanValues = ["Yes", "No", "No Value"];
    DataContext: any = this;
    //@Output() myShowFixedFilters = new EventEmitter();
    ShowRunButton: boolean = false;
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public RunReportCommand: EventEmitter<any>;
    public ShowFixedFilters: EventEmitter<any>; 
    @Output() RunReportComplete = new EventEmitter();
    @Output() ComputeFiltersComplete = new EventEmitter();
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    public _DWSubQueryPMService: DWSubQueryPMService;
    ValidationErrorsList: any[];
    public DWQueryData: DWQueryData;
    IsDateFilter: boolean = false;
    IsStaticDateFilter: boolean = false;
    IsStaticFilter: boolean = false;
    IsFirstTime: boolean = false;
    //ShowStaticFilter: boolean = true;

    get SelectedFiltersDataSource() {
        return this.selectedFiltersDataSource;
    }
    set SelectedFiltersDataSource(value: DWObjectFieldsDetails[]) {
          
        this.selectedFiltersDataSource = value;
        this.SelectedDynamicFiltersDataSource = this.selectedFiltersDataSource.filter(a => a.FilterType == "Ask User");
        this.SelectedFixedFiltersDataSource = this.selectedFiltersDataSource.filter(a => a.FilterType == "Fixed Filter");
        //if (this.ShowStaticFilters == true) {
        //    this.SelectedFixedFiltersDataSource = this.selectedFiltersDataSource.filter(a => a.FilterType == "Fixed Filter");
        //}
        //else {
        //    this.SelectedFixedFiltersDataSource = [];
        //}
    }

    public ComputeFiltersCommand: EventEmitter<any>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        super();
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
    }

    ngOnInit() {
        var ObsList = [];
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        //this.SelectedDynamicFiltersDataSource = this.SelectedFiltersDataSource.filter(a => a.FilterType == 'Ask User');
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
        if (this.ShowFixedFilters) {
            this.ShowFixedFilters.subscribe((ShowFixed) => {
                this.ShowStaticFilters = ShowFixed;
                //this.CD.detectChanges();
            });
        }
    }

    ComputeFilters() {
        this.ComputeFiltersComplete.emit(this.DWQueryData);
    }

    private PageIndex = 0;
    private PageSize = 1000;
    private count = 0;
    private rowData = [];
    private totalDataLoaded = 10000;
    private loadingMsg = "Loading";
    private isParentTenant = false;

    get LoadingMsg() {
        return this.loadingMsg;
    }
    set LoadingMsg(value:string) {
        if (this.loadingMsg != value) {
            this.loadingMsg = value;
        }
    }

    RunReport(MyDWQueryData) {
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
                this.RunReportComplete.emit({ Msg:"ValidationError"});
            }
            else {
                this.RunReportComplete.emit({ Msg: "ValidationError" });
            }
        }
    }
    GetRowDataRecursive() {
        if (this.count < this.totalDataLoaded) {
            var QueryData = new DWQueryData();
            QueryData.Columns = this.DWQueryData.Columns;
            QueryData.Filters = this.SelectedFiltersDataSource[0];
            QueryData.PageIndex = this.PageIndex;
            QueryData.PageSize = this.PageSize;
            QueryData.ColumnsSort = this.DWQueryData.ColumnsSort;
            var myAndOr = "And";
            if (this.DWQueryData.Filters) {
                myAndOr = this.DWQueryData.Filters.AndOr;
            }
            this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];
            if (this.DWQueryData.Filters) {
                this.DWQueryData.Filters.AndOr = myAndOr;
            }
           
            this.GetRowData(QueryData);
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.RunReportComplete.emit({ rowData: this.rowData, Count: this.count, IsParentTenant: this.isParentTenant});
        }
    }
    GetRowData(QueryData: DWQueryData) {
        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(myResult => {
            if (!myResult.HasError) {
                this.rowData = this.rowData.concat(myResult.Result.SQLDataResult);
                this.PageIndex = this.PageIndex + 1000;
                var dataSize = myResult.Result.SQLDataResult.length;
                this.isParentTenant  =myResult.Result.IsParentTenant;
                if (dataSize == 0) {
                    this.CurrentSession.StopBusyIndicator();
                    this.RunReportComplete.emit({ rowData: this.rowData, Count: this.count, IsParentTenant: this.isParentTenant});
                }
                else {
                    this.count = this.count + dataSize;
                    this.LoadingMsg = "Loading " + this.count;
                    this.CurrentSession.StartBusyIndicator("Loading " + this.count);
                    if (this.count == this.totalDataLoaded) {
                        this.PageIndex = this.PageIndex + 1;
                        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe(myResult => {
                            if (!myResult.HasError) {
                                this.CurrentSession.StopBusyIndicator();
                                this.RunReportComplete.emit({ rowData: this.rowData, Msg: "MT5000", Count: this.count, IsParentTenant: this.isParentTenant});// more than 10000
                            }
                            else {
                                this.CurrentSession.StopBusyIndicator();
                            }
                        });
                    }
                    else {
                        this.GetRowDataRecursive();
                    }
                }


                

            }
            else {

                if (myResult.ErrorsArray && myResult.ErrorsArray.length > 0) {
                    this.ShowMessageWindow(myResult.ErrorsArray[0]);
                }

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }



    public ShowMessageWindow(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
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

                if (field.OperationCode != "Between") {
                    if (field.IsMandatoryFilter == true && AppTool.IsNullOrEmpty(field.TextValue)) {
                        this.ValidationErrorsList.push(field.DisplayName.replace('[', '').replace(']', '') + " filter is required");
                    }
                }
                else {
                    var messageError: string = "";
                    if (field.TextValue) {
                        var values: string = field.TextValue.split('^');
                        var valueDate1: string = values[0];
                        var valueDate2: string = values.length > 1 ? values[1] : "";
                        if (!valueDate1 || !valueDate2) {
                            messageError = "From/To is Required";
                        }
                    } else {
                        messageError = "From/To is Required";
                    }

                    if (messageError) {
                        this.ValidationErrorsList.push(field.DisplayName.replace('[', '').replace(']', '') + " " + messageError);
                    }
                    
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

    private showStaticFilters: boolean = false;
    public get ShowStaticFilters() { return this.showStaticFilters; }
    public set ShowStaticFilters(newValue: boolean) {
        this.showStaticFilters = newValue;
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
        this.list.push(this.BetweenOp);
        return this.list;
    }

    beforeOp: ObjectFieldOperator = new ObjectFieldOperator("Before", "Before");
    afterOp: ObjectFieldOperator = new ObjectFieldOperator("After", "After");
    previousOp: ObjectFieldOperator = new ObjectFieldOperator("Previous", "Previous");
    currentOp: ObjectFieldOperator = new ObjectFieldOperator("Current", "Current");
    nextOp: ObjectFieldOperator = new ObjectFieldOperator("Next", "Next");
    BetweenOp: ObjectFieldOperator = new ObjectFieldOperator("Between", "Between");
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
