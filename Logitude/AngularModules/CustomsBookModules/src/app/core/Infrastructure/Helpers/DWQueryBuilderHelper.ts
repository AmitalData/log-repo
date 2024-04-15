declare var window: any;
import { Component, OnInit, EventEmitter, Output} from '@angular/core';

import {AppTool, DateTool} from '../Tools';
import { SessionLocator } from '../Utilities/SessionLocator';
import { BaseComponent } from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DWObjectFieldExtendedPMService } from '../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { ServiceResponse } from '../DataContracts/ServiceResponse';

export class DWQueryBuilderHelper   {

    public FactFields: any[];
    public  _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public FilterValueChanged = new EventEmitter();

    constructor() {
        window.FactFields = [];
        this.FactFields = [];
        //this.CurrentEntityPM = currentEntityPM;
        //this.AddEditAutomationsViewModel = addEditAutomationsViewModel;
        //this.ViewModel = viewModel;
        //this.Type = type;
    }

    public RestoreFilters(Filters: any) {
        var DWObjectField = new DWObjectFieldsDetails();
        DWObjectField.IsGroup = true;
        var MyFilter = this.GetRestoreFilters(Filters,DWObjectField);
        return MyFilter;
    }

    public FillAllFactFields(FactTableCode: string) {
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        if (window.FactFields.length == 0)
            this._DWObjectFieldPMService.getDWObjectFieldsWithChildrenByDWTableId(FactTableCode).subscribe((Result: ServiceResponse) => {
            if (!Result.HasError) {
                Result.Result.forEach((field) => {
                    if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) { 
                        this.FactFields.push(field);
                    }
                });
                window.FactFields = this.FactFields;
                //return this.FactFields
                //this.DataSource = this.ObsList;
                //this.AllFieldsWithChildrenDataSource = this.ObsList;
            }

        });
    }
    private GetRestoreFilters(BaseFilter: DWObjectFieldsDetails, MyFilter: DWObjectFieldsDetails) {

        BaseFilter.FilterItems.forEach((field) => {
            var view = new DWObjectFieldsDetails(field);
            view.FilterChanged = this.FilterValueChanged;
            if (AppTool.IsNullOrEmpty(view.DimensionTableDisplayName)) {
                view.DimensionTableDisplayName = field.ParentCode;
            }
            if (field.FilterItems.length == 0) {
                if (field.DWObjectTableCode && (field.DWObjectTableCode.indexOf("DIM_") != -1 && (field.DataTypeCode != "DateTime" || field.DWObjectTableCode.indexOf("DIM_Date") != -1))) {
                    if (view.Code == '[Full Date]') {
                        view.ParentDataTypeCode = "Date";
                        view.DataTypeCode = "Date";
                    }
                    else {
                        view.ParentDataTypeCode = "LookUp";
                    }
                    //view.ParentDataTypeCode = "LookUp";
                    view.ParentDimTabelName = field.DWObjectTableCode;

                    if (view.DataTypeCode == "Boolean") view.ParentDataTypeCode = "Boolean";

                }
                else {
                    view.ParentDataTypeCode = field.DataTypeCode;
                }
            }
            
            if (field.FilterItems.length == 0) {
                view.TextValue = field.TextValue;
                view.MultiSelectedValueLists = this.MapMultiSelectedValueLists(field.MultiSelectedValueLists);
                view.Operation = new ObjectFieldOperator(field.OperationCode, field.OperationName);
                MyFilter.FilterItems.push(view);
            }
            else {
                var DWObjectField = new DWObjectFieldsDetails();
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.FilterItems.length;
                DWObjectField.AndOr = field.AndOr;
                this.GetRestoreFilters(field, DWObjectField);
                MyFilter.FilterItems.push(DWObjectField); 
            } 

        });

        return MyFilter;
         
    }

    MapMultiSelectedValueLists(lists: MultiSelectedValue[]) {
        var result: MultiSelectedValue[] = [];
        if (lists) {
            lists.forEach((field) => {
                var item: MultiSelectedValue = new MultiSelectedValue();
                var i = "";
                var j = 0;
                while (field["Value" + i]) {
                    var valueDetails: ValueDetails = new ValueDetails();
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
    }
}

export class DWObjectFieldsDetails extends BaseComponent {
   public MyParentClass: any;
    public BaseDWObjectField: any;
    public FilterChanged: EventEmitter<any>;// = new EventEmitter();
    public MultiSelectedDisplayName: string;

    constructor(DWObjectField: any = null, ParentClass: any = null) {
        super();
        this.BaseDWObjectField = DWObjectField;
        if (ParentClass != null) {
            this.MyParentClass = ParentClass;
            this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        }
        if (DWObjectField != null) {
            this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
            if (!this.LOVAdditionalColumns && ParentClass && ParentClass.AllFieldsDataSource) {
                var field = ParentClass.AllFieldsDataSource.filter(a => a.DWObjectTableCode == DWObjectField.DWObjectTableCode && a.Code == DWObjectField.Code && a.Name == DWObjectField.Name)[0];
                if (field) {
                    this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns = field.LOVAdditionalColumns;
                }

            }


            if (!AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
                this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
            }
            else {
                this.DimensionTableDisplayName = DWObjectField.ParentCode;
            }
            this.Name = DWObjectField.Name;
            this.Code = DWObjectField.Code;
            this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
            this.DimensionTableCode = DWObjectField.DimensionTableCode;
            this.DataTypeCode = DWObjectField.DataTypeCode;
            this.IsCustom = DWObjectField.IsCustom;
            this.CustomPickListCode = DWObjectField.CustomPickListCode;

            if (DWObjectField.FilterItems.length == 0) {
                this.DisplayName = this.ComputeDisplayName(DWObjectField);//(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
            }
            this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
            this.IsMeasurement = DWObjectField.IsMeasurement;
            this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
            if (DWObjectField.FilterType) {
                this.FilterType = DWObjectField.FilterType;
            }
            if (DWObjectField.IsSetDefaults) {
                this.IsSetDefaults = DWObjectField.IsSetDefaults;
            }
            if (DWObjectField.IsMandatoryFilter) {
                this.IsMandatoryFilter = DWObjectField.IsMandatoryFilter;
            }


            //this.Name = DWObjectField.Name;
        }

    }

    public ComputeDisplayName(DWObjectField: any) {
        //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        var Displayname = DWObjectField.DisplayName;
        if (AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
            if (DWObjectField.DWObjectTableCode.indexOf("DIM_") != -1) {
                Displayname = DWObjectField.Name + ' ' + DWObjectField.Code;
            }
            else {
                Displayname = DWObjectField.Code;
            }
        }
        return Displayname;
    }


    Items: any[] = [];
    FilterItems: DWObjectFieldsDetails[] = [];
    private lOVAdditionalColumns: string;
    public get LOVAdditionalColumns() { return this.lOVAdditionalColumns; }
    public set LOVAdditionalColumns(newValue: string) { this.lOVAdditionalColumns = newValue; }

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(newValue: string) { this.category1 = newValue; }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(newValue: string) { this.category2 = newValue; }

    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue; }

    private isGroup: boolean = false;
    public get IsGroup() { return this.isGroup; }
    public set IsGroup(newValue: boolean) { this.isGroup = newValue; }

    private isPrimaryKey: boolean;
    public get IsPrimaryKey() { return this.isPrimaryKey; }
    public set IsPrimaryKey(newValue: boolean) { this.isPrimaryKey = newValue; }

    private showBtns: boolean = false;
    public get ShowBtns() { return this.showBtns; }
    public set ShowBtns(newValue: boolean) { this.showBtns = newValue; }

    private isViewTree: boolean;
    public get IsViewTree() { return this.isViewTree; }
    public set IsViewTree(newValue: boolean) { this.isViewTree = newValue; }


    private hasTree: boolean;
    public get HasTree() { return this.hasTree; }
    public set HasTree(newValue: boolean) { this.hasTree = newValue; }

    

    private isCustom: boolean;
    public get IsCustom() { return this.isCustom; }
    public set IsCustom(newValue: boolean) { this.isCustom = newValue; }

    
    private customPickListCode: string;
    public get CustomPickListCode() { return this.customPickListCode; }
    public set CustomPickListCode(newValue: string) { this.customPickListCode = newValue; }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

    private displayname: string;
    public get DisplayName() { return this.displayname; }
    public set DisplayName(newValue: string) { this.displayname = newValue; }

    private dimensionTableDisplayName: string;
    public get DimensionTableDisplayName() { return this.dimensionTableDisplayName; }
    public set DimensionTableDisplayName(newValue: string) { this.dimensionTableDisplayName = newValue; }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private parentcode: string;
    public get ParentCode() { return this.parentcode; }
    public set ParentCode(newValue: string) { this.parentcode = newValue; }

    private parentDimTabelName: string;
    public get ParentDimTabelName() { return this.parentDimTabelName; }
    public set ParentDimTabelName(newValue: string) { this.parentDimTabelName = newValue; }


    private dWObjectTableCode: string;
    public get DWObjectTableCode() { return this.dWObjectTableCode; }
    public set DWObjectTableCode(newValue: string) { if (this.dWObjectTableCode != newValue) { this.dWObjectTableCode = newValue; } }

    private isMeasurement: boolean;
    public get IsMeasurement() { return this.isMeasurement; }
    public set IsMeasurement(newValue: boolean) { if (this.isMeasurement != newValue) { this.isMeasurement = newValue; } }

    private aggregationTypeCode: string;
    public get AggregationTypeCode() { return this.aggregationTypeCode; }
    public set AggregationTypeCode(newValue: string) { if (this.aggregationTypeCode != newValue) { this.aggregationTypeCode = newValue; } }

    private dataTypeCode: string;
    public get DataTypeCode() { return this.dataTypeCode; }
    public set DataTypeCode(newValue: string) {
        //if (this.dataTypeCode != newValue) {
        this.dataTypeCode = newValue;
        if (this.dataTypeCode == "Boolean") {
            this.TextValue = false;
        }
        if (this.dataTypeCode == "LookUp" || this.dataTypeCode == "Dimension") {
            this.HasTree = true;
            if (this.MyParentClass) {
                var MyTable = this.MyParentClass.AllTables.filter(a => a.Code == this.DimensionTableCode);
                if (MyTable && MyTable.length > 0) {
                    this.Code = MyTable[0].DefaultFilterBy;
                    this.DWObjectTableCode = MyTable[0].Code;
                    this.DisplayName = this.ComputeDisplayName(this);//(AppTool.IsNullOrEmpty(this.DisplayName)) ? (this.DWObjectTableCode + ' ' + this.Code) : (this.DisplayName);
                    this.ParentDimTabelName = this.DimensionTableCode;
                    if (!AppTool.IsNullOrEmpty(this.DimensionTableDisplayName)) {
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
    }

    private parentDataTypeCode: string;
    public get ParentDataTypeCode() { return this.parentDataTypeCode; }
    public set ParentDataTypeCode(newValue: string) {
        if (this.parentDataTypeCode != newValue) {
            this.parentDataTypeCode = newValue;
        }
    }


    private dimensionTableCode: string;
    public get DimensionTableCode() { return this.dimensionTableCode; }
    public set DimensionTableCode(newValue: string) { if (this.dimensionTableCode != newValue) { this.dimensionTableCode = newValue; } }

    private operators: ObjectFieldOperator[];
    public get Operators() { return this.GetFieldOperators(this); }
    public set Operators(newValue: ObjectFieldOperator[]) {
        this.operators = newValue;
    }

    private textValue: any;
    public get TextValue() {
        return this.textValue;
    }
    public set TextValue(newValue: any) {
        if (this.textValue != newValue) {
            //if (this.textValue != null && this.textValue != undefined) {
            this.textValue = newValue;
            if (this.ParentDataTypeCode == "DateTime" || this.ParentDataTypeCode == "Date") {
                var timerToken = setTimeout(() => {
                    this.ShowSampleDateCommand.emit(this);
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
    }

    private multiSelectedValueLists: MultiSelectedValue[];
    public get MultiSelectedValueLists() {
        return this.multiSelectedValueLists;
    }
    public set MultiSelectedValueLists(newValue: MultiSelectedValue[]) {
        this.multiSelectedValueLists = newValue;
        this.MultiSelectedDisplayName = this.GetMultiSelectedDisplayName();
    }

    private GetMultiSelectedDisplayName() {
        let textValue = "";
        if (this.multiSelectedValueLists && this.multiSelectedValueLists.length > 0) {
            let dimensionTableCode = this.DimensionTableCode ? this.DimensionTableCode : this.ParentDimTabelName;
            let fieldCode = (dimensionTableCode == "DIM_Partners" && this.LOVAdditionalColumns && this.LOVAdditionalColumns.indexOf('[Local Name]') != -1) ? "Local Name" : this.Code;
            let fieldName = fieldCode ? fieldCode.replace('[', '').replace(']', '') : "";
            this.multiSelectedValueLists.forEach((item) => {
                textValue += this.ResolveValue(item, fieldName);
                textValue += !this.IsLastItemInMultiSelectedValueLists(item) ? ";" : "";
            });
        }
        return textValue;
    }


    private IsLastItemInMultiSelectedValueLists(item: MultiSelectedValue) {
        return this.multiSelectedValueLists.indexOf(item) == (this.multiSelectedValueLists.length - 1);
    }


    private ResolveValue(Values: MultiSelectedValue, header: string) {
        var i = "";
        var j = 0;
        var result = "";
        while (Values["Value" + i]) {
            if (Values["Value" + i].Header == header) {
                result = Values["Value" + i].Row;
                return result;
            }

            j += 1;
            i = j.toString();
        }

        return result;
    }

    private operationName: string;
    public get OperationName() { return this.operationName; }
    public set OperationName(newValue: string) {
        if (this.operationName != newValue) {
            this.operationName = newValue;
        }
    }

    private operationCode: string;
    public get OperationCode() { return this.operationCode; }
    public set OperationCode(newValue: string) {
        if (this.operationCode != newValue) {
            this.operationCode = newValue;
        }
    }

    private operation: ObjectFieldOperator;
    public get Operation() {
        if (!this.operation) {
            if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText") && AppTool.IsNullOrEmpty(this.operation)) {
                this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                this.OperationCode = "StartsWith";
                this.OperationName = "Starts With";
                return this.operation;
            }
            else {
                if (AppTool.IsNullOrEmpty(this.operation)) {
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
    }
    public set Operation(newValue: ObjectFieldOperator) {
        this.OperationCode = newValue.Code;
        this.OperationName = newValue.Name;
        this.operation = newValue;
        this.FilterChanged.emit("FilterValueChanged");
    }

    private andOr: string;
    public get AndOr() {
        if (AppTool.IsNullOrEmpty(this.andOr)) {
            return "And";
        }
        return this.andOr;
    }
    public set AndOr(newValue: string) {
        this.andOr = newValue;
        if (this.MyParentClass) {
            this.MyParentClass.SaveChanges();
        }
    }

    private filterType: string = "Fixed Filter";
    public get FilterType() { return this.filterType; }
    public set FilterType(newValue: string) { this.filterType = newValue; }

    private isSetDefaults: boolean = false;
    public get IsSetDefaults() { return this.isSetDefaults; }
    public set IsSetDefaults(newValue: boolean) { if (this.isSetDefaults != newValue) { this.isSetDefaults = newValue; } }

    private isMandatoryFilter: boolean = false;
    public get IsMandatoryFilter() { return this.isMandatoryFilter; }
    public set IsMandatoryFilter(newValue: boolean) { if (this.isMandatoryFilter != newValue) { this.isMandatoryFilter = newValue; } }
    //IsMandatoryFilter: boolean = false;
    //IsSetDefaults: boolean = false;

    FilterTypeChanged(Value) {
        this.FilterType = Value;
    }

    OpenFilterSettings() {
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
    }

    OperationValueChanged(operation) {

        if (this.Operation.Code == this.IsNullOp.Code || this.Operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = "";
            this.MultiSelectedValueLists = [];
        }

        this.Operation = operation;
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = operation.Code;
        }
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !AppTool.IsNullOrEmpty(this.TextValue)) {
            if (this.MyParentClass) {
                this.MyParentClass.SaveChanges();
            }
        }
       
    }

    LoadItems(DWObjectField: any) {
        //if (this.IsViewTree) {
        //    this.IsViewTree = false;
        //}
        //else {
        //    if (this.Items.length == 0) {
        //        this.Load(DWObjectField);
        //    }
        //    else this.IsViewTree = true;
        //}
    }

    //Load(DWObjectField: any) {
    //    var _DWObjectTablePMService = new DWObjectTablePMService();
    //    var _DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
    //    var ObsList = [];
    //    _DWObjectTablePMService.get(DWObjectField.DimensionTableCode).subscribe((myResult:any) => {
    //        if (!myResult.HasError) {
    //            _DWObjectFieldPMService.getDWObjectFieldsByDWTableId(myResult.Result.Code).subscribe((Result:any) => {
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
    @Output() ShowSampleDateCommand = new EventEmitter();

    onTextChange(value) {
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
    }
    //onTextChange(value) {
    //    this.TextValue = value;
    //}

    AndOrOpsChanged(value) {
        this.AndOr = value;
    }

    OnMouseOver(event) {
        //console.log("Over");
        var e = event.toElement;// || event.relatedTarget;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    }

    OnMouseOut(event) {
        //console.log("Out");
        var e = event.toElement;// || event.relatedTarget;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == true) {
            this.ShowBtns = false;
        }
    }

    preventInnerHover(event) {
        event.stopPropagation();
    }

    AddFilterToGroup() {
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
    }

    AddGroup(Father: DWObjectFieldsDetails) {
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
    }

    FieldValueChanged(DWObjectField: DWObjectFieldsDetails) {
        //this.MyParentClass = ParentClass;
        this.TextValue = "";
        this.MultiSelectedValueLists = [];
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        if (!AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
            this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
        }
        else {
            this.DimensionTableDisplayName = DWObjectField.ParentCode;
        }
        this.DisplayName = this.ComputeDisplayName(DWObjectField);//(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        this.IsMeasurement = DWObjectField.IsMeasurement;
        this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
        if (this.DWObjectTableCode.indexOf("DIM_") != -1) {
            this.ParentDataTypeCode = "LookUp";
            this.ParentDimTabelName = DWObjectField.DWObjectTableCode;

            if (this.DataTypeCode == "Boolean") this.ParentDataTypeCode = "Boolean";


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
    }

    onDeleteFilterClick() {
       
    }


    list: ObjectFieldOperator[];
    private GetFieldOperators(field: DWObjectFieldsDetails) {


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
    }




    startsWithOp: ObjectFieldOperator = new ObjectFieldOperator("StartsWith", "Starts With");
    equalsOp: ObjectFieldOperator = new ObjectFieldOperator("Equals", "Equals to");
    notEqualsOp: ObjectFieldOperator = new ObjectFieldOperator("NotEqual", "Not Equal to");
    largerThanOp: ObjectFieldOperator = new ObjectFieldOperator("LargerThan", "Greater Than");
    lessThanOp: ObjectFieldOperator = new ObjectFieldOperator("LessThan", "Less Than");
    greaterThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
    lessThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
    BetweenOp: ObjectFieldOperator = new ObjectFieldOperator("Between", "Between");
    IsNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNull", "Is Empty");
    IsNotNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNotNull", "Has Value");
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

export class DWFieldsGroup {

    constructor(Key: string, FieldsList: any[]) {
        this.Key = Key;
        this.FieldsList = FieldsList;
    }

    private key: string;
    public get Key() { return this.key; }
    public set Key(newValue: string) { this.key = newValue; }

    private fieldsList: any[];
    public get FieldsList() { return this.fieldsList; }
    public set FieldsList(newValue: any[]) { this.fieldsList = newValue; }

    private detailsIcon: string = "./Images/CellIcons/Arrowup.png";
    public get DetailsIcon() { return this.detailsIcon; }
    public set DetailsIcon(newValue: string) { this.detailsIcon = newValue; }

    private isDetailesOpened: boolean = false;
    public get IsDetailesOpened() { return this.isDetailesOpened; }
    public set IsDetailesOpened(newValue: boolean) { this.isDetailesOpened = newValue; }

    GroupClicked() {
        this.IsDetailesOpened = !this.IsDetailesOpened;
        if (!this.IsDetailesOpened) {
            this.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
        }
        else {
            this.DetailsIcon = "./Images/CellIcons/Arrowup.png";
        }
    }
}


export class MultiSelectedValue {

    public Value: ValueDetails;
    public Value1: ValueDetails;
    public Value2: ValueDetails;
    public Value3: ValueDetails;
    public Value4: ValueDetails;
    public Value5: ValueDetails;
    public Value6: ValueDetails;
    public Value7: ValueDetails;
    public Value8: ValueDetails;
    public Value9: ValueDetails;
    public Value10: ValueDetails;

}

export class ValueDetails {
    public Header: string;
    public Row: string;
}
