import { Component, OnInit, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { DWQueryBuilderBaseComponent, DWObjectFieldsDetails, ObjectFieldOperator } from './DWQueryBuilderBaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'DWAskUserFiltersComponent',
    
    templateUrl: './DWAskUserFiltersComponent.html',
    inputs: ['SelectedFiltersDataSource', 'ShowRunButton', 'RunReportCommand', 'IsDateFilter', 'ComputeFiltersCommand', 'IsFirstTime', 'IsStaticFilter', 'IsStaticDateFilter', 'SelectedDynamicFiltersDataSource', 'SelectedFixedFiltersDataSource', 'ShowFixedFilters']
})

export class DWAskUserFiltersComponent extends DWQueryBuilderBaseComponent implements OnInit {
    private PageIndex = 0;
    private PageSize = 1000;
    private count = 0;
    private rowData = [];
    private totalDataLoaded = 10000;
    private isParentTenant = false;
    private loadingMsg = "Loading";
    private CurrentSession = SessionLocator.SelectedSession;

    public DWQueryData: DWQueryData;
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public RunReportCommand: EventEmitter<any>;
    public ShowFixedFilters: EventEmitter<any>;
    public _DWSubQueryPMService: DWSubQueryPMService;
    public ComputeFiltersCommand: EventEmitter<any>;

    DataContext: any = this;
    ShowRunButton: boolean = false;
    AllFieldsWithChildrenDataSource: DWObjectFieldsDetails[];
    IsDateFilter: boolean = false;
    IsStaticDateFilter: boolean = false;
    IsStaticFilter: boolean = false;
    IsFirstTime: boolean = false;

    @Output() RunReportComplete = new EventEmitter();
    @Output() ComputeFiltersComplete = new EventEmitter();
    constructor(private CDR: ChangeDetectorRef) {
        super(CDR);
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
    }

    ngOnInit() {
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        if (this.RunReportCommand) {
            this.RunReportCommand.subscribe((QueryId) => {
                this.RunReport(QueryId);
            });
        }
        if (this.ComputeFiltersCommand) {
            this.ComputeFiltersCommand.subscribe((args) => {
                this.ComputeFilters(args ? args.ExportType : "");
            });
        }
        if (this.ShowFixedFilters) {
            this.ShowFixedFilters.subscribe((ShowFixed) => {
                this.ShowStaticFilters = ShowFixed;
            });
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
                this.RunReportComplete.emit({ Msg: "ValidationError" });
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
            QueryData.FactTableName = this.DWQueryData.FactTableName;
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
            this.RunReportComplete.emit({ rowData: this.rowData, Count: this.count, IsParentTenant: this.isParentTenant });
        }
    }

    GetRowData(QueryData: DWQueryData) {
        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe((myResult: ServiceResponse) => {
            if (!myResult.HasError) {
                this.rowData = this.rowData.concat(myResult.Result.SQLDataResult);
                this.PageIndex = this.PageIndex + 1000;
                var dataSize = myResult.Result.SQLDataResult.length;
                this.isParentTenant = myResult.Result.IsParentTenant;
                if (dataSize == 0) {
                    this.CurrentSession.StopBusyIndicator();
                    this.RunReportComplete.emit({ rowData: this.rowData, Count: this.count, IsParentTenant: this.isParentTenant });
                }
                else {
                    this.count = this.count + dataSize;
                    this.LoadingMsg = "Loading " + this.count;
                    this.CurrentSession.StartBusyIndicator("Loading " + this.count);
                    if (this.count == this.totalDataLoaded) {
                        this.PageIndex = this.PageIndex + 1;
                        this._DWQueryBuilderService.GetNewDWQueryData(QueryData).subscribe((myResult: ServiceResponse) => {
                            this.CurrentSession.StopBusyIndicator();
                            if (!myResult.HasError) {
                                this.RunReportComplete.emit({ rowData: this.rowData, Msg: "MT5000", Count: this.count, IsParentTenant: this.isParentTenant });// more than 10000
                            }
                            else if (myResult.ErrorsArray && myResult.ErrorsArray.length > 0) {
                                this.ShowMessageWindow(myResult.ErrorsArray[0]);
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

    get LoadingMsg() {
        return this.loadingMsg;
    }
    set LoadingMsg(value: string) {
        if (this.loadingMsg != value) {
            this.loadingMsg = value;
        }
    }

    ComputeFilters(exportType: string) {
        this.DWQueryData.ExportType = exportType;
        this.ComputeFiltersComplete.emit(this.DWQueryData);
    }

    public ShowMessageWindow(message: string, title: string = "") {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
        if (title) {
            messageWindow.Title = title;
        }
    }

    OperationValueChanged(operation, Item) {
        Item.OperationValueChanged(operation);
        //Item.Operation = new ObjectFieldOperator(operation.Code, operation.Name);
        //Item.TextValue = operation.Code;
    }

    private operators: ObjectFieldOperator[];
    public get Operators() { return this.GetAllFieldsOperators(); }
    public set Operators(newValue: ObjectFieldOperator[]) {
        this.operators = newValue;
    }

    private GetAllFieldsOperators() {
        this.list = [];
        this.list.push(this.beforeOp);
        this.list.push(this.afterOp);
        this.list.push(this.previousOp);
        this.list.push(this.currentOp);
        this.list.push(this.nextOp);
        this.list.push(this.BetweenOp);
        this.list.push(this.IsNullOp);
        this.list.push(this.IsNotNullOp);
        this.list.push(this.equalsOp);
        this.list.push(this.notEqualsOp);
        this.list.push(this.largerThanOp);
        this.list.push(this.lessThanOp);
        this.list.push(this.greaterThanOrEqualOp);
        this.list.push(this.lessThanOrEqualOp);

        return this.list;
    }

    GetFieldOperators(Item) {
        return this.GetSelectedFieldOperators(Item);
    }
}
