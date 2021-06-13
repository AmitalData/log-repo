import { Component, Input, OnInit, ChangeDetectorRef, OnDestroy, Directive, Output, EventEmitter } from '@angular/core';
import { DWQueryBuilderComponent } from '../../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent';
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
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { DWObjectFieldPM } from '../../../../Infrastructure/EntityPMs/DWObjectFieldPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'DWQueryBuilderBaseComponent',
    templateUrl: './DWQueryBuilderBaseComponent.html',
})

export class DWQueryBuilderBaseComponent extends BaseComponent {
    public AndOrOps = ["And", "Or"];
    public Types = ["Fixed Filter", "Ask User"];
    public BooleanValues = ["Yes", "No", "No Value"];

    RTL: boolean = false;
    ValidationErrorsList: any[];
    constructor(private CD: ChangeDetectorRef) {
        super();
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

                this.ValidateDateField(field);

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

    ValidateDateField(field: DWObjectFieldsDetails) {
        if ((field.DataTypeCode == "Date" || field.DataTypeCode == "DateTime") && AppTool.IsNullOrEmpty(field.TextValue)) {
            this.ValidationErrorsList.push(field.DisplayName.replace('[', '').replace(']', '') + " filter is required");
        }
    }

    ValidFiltersValues(MyFilter: DWObjectFieldsDetails) {
        if (MyFilter.FilterItems.length == 0) {
            return true;
        }
        var Valid = true;
        MyFilter.FilterItems.forEach((field) => {
            if (field.FilterItems.length == 0) {
                if (field.TextValue != true) {
                    if (field.DataTypeCode && field.TextValue && field.TextValue != "IsNull" && field.TextValue != "IsNotNull" && field.TextValue.indexOf(';') < 0) {
                        switch (field.DataTypeCode.toLowerCase()) {
                            case 'integer':
                            case 'double':
                            case 'decimal':
                                {
                                    var val: number;
                                    if ((field.TextValue + "").indexOf(',') == -1) {
                                        val = Number(field.TextValue);
                                    }
                                    if (isNaN(Number(val))) {
                                        Valid = false;
                                    }
                                }
                            default: {
                                break;
                            }
                        }
                    }
                }
                else {
                    this.ValidFiltersValues(field);
                }
            }
        });

        return Valid;
    }

    private showStaticFilters: boolean = false;
    public get ShowStaticFilters() { return this.showStaticFilters; }
    public set ShowStaticFilters(newValue: boolean) {
        this.showStaticFilters = newValue;
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
    }

    public GetSelectedFieldOperators(field: DWObjectFieldsDetails) {
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
        ) {
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
            this.list.push(this.afterOp);
            this.list.push(this.beforeOp);
            this.list.push(this.previousOp);
            this.list.push(this.currentOp);
            this.list.push(this.nextOp);
            this.list.push(this.BetweenOp);
        }

        return this.list;
    }

    DeleteDWField(Item: DWObjectFieldsDetails, ListItems: DWObjectFieldsDetails[]) {
        ListItems.forEach((Myfilter) => {
            if (Myfilter.FilterItems.length > 0) {
                Myfilter.FilterItems = this.DeleteDWField(Item, Myfilter.FilterItems);
                if (Myfilter.FilterItems.length == 0) {
                    ListItems = ListItems.filter(a => a != Myfilter);
                }
            }
            else {
                if (Myfilter == Item) {
                    ListItems = ListItems.filter(a => a != Item);
                }
            }
        });
        return ListItems;
    }

    onDeleteFilterClick(item) {
        item.MyParentClass.SelectedFiltersDataSource = this.DeleteDWField(item, item.MyParentClass.SelectedFiltersDataSource);
        item.MyParentClass.ClearData();
    }

    selectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    SelectedDynamicFiltersDataSource: DWObjectFieldsDetails[] = [];
    SelectedFixedFiltersDataSource: DWObjectFieldsDetails[] = [];
    get SelectedFiltersDataSource() {
        return this.selectedFiltersDataSource;
    }
    set SelectedFiltersDataSource(value: DWObjectFieldsDetails[]) {
        this.selectedFiltersDataSource = value;
        this.SelectedDynamicFiltersDataSource = this.selectedFiltersDataSource.filter(a => a.FilterType == "Ask User");
        this.SelectedFixedFiltersDataSource = this.selectedFiltersDataSource.filter(a => a.FilterType == "Fixed Filter");
    }

    list: ObjectFieldOperator[];
    beforeOp: ObjectFieldOperator = new ObjectFieldOperator("Before", "Before");
    afterOp: ObjectFieldOperator = new ObjectFieldOperator("After", "After");
    previousOp: ObjectFieldOperator = new ObjectFieldOperator("Previous", "Previous");
    currentOp: ObjectFieldOperator = new ObjectFieldOperator("Current", "Current");
    nextOp: ObjectFieldOperator = new ObjectFieldOperator("Next", "Next");
    BetweenOp: ObjectFieldOperator = new ObjectFieldOperator("Between", "Between");
    IsNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNull", "Is Empty");
    IsNotNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNotNull", "Has Value"); startsWithOp: ObjectFieldOperator = new ObjectFieldOperator("StartsWith", "Starts With");
    equalsOp: ObjectFieldOperator = new ObjectFieldOperator("Equals", "Equals to");
    notEqualsOp: ObjectFieldOperator = new ObjectFieldOperator("NotEqual", "Not Equal to");
    largerThanOp: ObjectFieldOperator = new ObjectFieldOperator("LargerThan", "Greater Than");
    lessThanOp: ObjectFieldOperator = new ObjectFieldOperator("LessThan", "Less Than");
    greaterThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
    lessThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
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

export class DWObjectFieldsDetails extends DWQueryBuilderBaseComponent
{
    public MyParentClass: DWQueryBuilderComponent;
    public BaseDWObjectField: any;
    public TooltipId: string = null;
    public TooltipContentId: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    public FilterTypes: ObjectFieldOperator[];
    public FullNameTextCodeCode: string;
    public PartnerFullNameTextCodeCode: string;
    public IsHaveTranslation: boolean = false;
    public TranslationText: string;
    public MultiSelectedDisplayName: string;

    @Output() ShowSampleDateCommand = new EventEmitter();
    constructor(DWObjectField: any = null, ParentClass: DWQueryBuilderComponent = null) {
        super(null);
        var idIndex = this.CurrentSession.GetNewId("Tooltip");
        this.TooltipId = "Tooltip_" + idIndex;
        this.TooltipContentId = "TooltipContent_" + idIndex;
        this.BaseDWObjectField = DWObjectField;
        this.FilterTypes = [];
        this.FilterTypes.push(new ObjectFieldOperator("Fixed Filter", "Fixed Filter"));
        this.FilterTypes.push(new ObjectFieldOperator("Ask User", "Dynamic Filter"));

        if (ParentClass != null) {
            this.MyParentClass = ParentClass;
            this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        }
        if (DWObjectField != null) {
            this.CustomPickListCode = DWObjectField.CustomPickListCode;
            this.HideTree = DWObjectField.HideTree;
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
            this.FullNameTextCodeCode = DWObjectField.FullNameTextCodeCode;
            this.PartnerFullNameTextCodeCode = DWObjectField.PartnerFullNameTextCodeCode;
            this.FieldCode = DWObjectField.Code;
            this.IsMultipleSelection = DWObjectField.IsMultipleSelection;
            this.ColumnName = DWObjectField.ColumnName;
            if (this.IsMultipleSelection) {
                this.MultiSelectedValueLists = DWObjectField.MultiSelectedValueLists;
            }
            this.ParentDataTypeCode = DWObjectField.ParentDataTypeCode;
            this.DataTypeCode = DWObjectField.DataTypeCode;
            this.TranslationText = this.GetTranslationText(DWObjectField);
            this.DisplayName = this.ComputeDisplayName(DWObjectField);
            this.CannotFilter = DWObjectField.CannotFilter;
            this.HelpText = DWObjectField.HelpText;
            this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
            this.IsMeasurement = DWObjectField.IsMeasurement;
            this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
            this.IsCustom = DWObjectField.IsCustom;
            if (DWObjectField.FilterType) {
                this.FilterType = DWObjectField.FilterType;
            }
            if (DWObjectField.IsSetDefaults) {
                this.IsSetDefaults = DWObjectField.IsSetDefaults;
            }
            if (DWObjectField.IsMandatoryFilter) {
                this.IsMandatoryFilter = DWObjectField.IsMandatoryFilter;
            }
        }
        this.FilterTypeSelected = this.FilterTypes.filter(d => d.Code == this.FilterType)[0];
        this.IsHaveTranslation = this.CheckIsFieldHaveTranslation(this);
    }

    // BIReportTranslate
    public ComputeDisplayName(DWObjectField: any) {
        var displayname: string = this.GetTranslationText(DWObjectField);
        if (!DWObjectField.IsCustom) {
            if (DWObjectField.DimensionTableDisplayName) {
                var partnerTranslateText = DWObjectField.PartnerFullNameTextCodeCode ? TextCodeTranslator.BIReportTranslate(DWObjectField.PartnerFullNameTextCodeCode) : "";
                displayname = (partnerTranslateText ? partnerTranslateText : DWObjectField.DimensionTableDisplayName) + " " + displayname;
            }
        }
        return displayname;
    }

    public GetTranslationText(DWObjectField: any) {
        var result: string = DWObjectField.DisplayName;
        if (!DWObjectField.IsCustom) {
            var translateText = DWObjectField.FullNameTextCodeCode ? TextCodeTranslator.BIReportTranslate(DWObjectField.FullNameTextCodeCode) : "";
            result = (translateText ? translateText : DWObjectField.Name);
        }
        return result;
    }

    CheckIsFieldHaveTranslation(DWObjectField: any) {
        var translateText = DWObjectField.FullNameTextCodeCode ? TextCodeTranslator.BIReportTranslate(DWObjectField.FullNameTextCodeCode) : "";

        return !AppTool.IsNullOrEmpty(translateText) ? true : false;
    }

    private hideTree: boolean;
    public get HideTree() { return this.hideTree; }
    public set HideTree(newValue: boolean) { this.hideTree = newValue; }

    private cannotFilter: boolean = false;
    public get CannotFilter() { return this.cannotFilter; }
    public set CannotFilter(newValue: boolean) { this.cannotFilter = newValue; }

    private helpText: string;
    public get HelpText() { return this.helpText; }
    public set HelpText(newValue: string) { this.helpText = newValue; }

    Items: any[] = [];
    FilterItems: DWObjectFieldsDetails[] = [];
    private lOVAdditionalColumns: string;
    public get LOVAdditionalColumns() { return this.lOVAdditionalColumns; }
    public set LOVAdditionalColumns(newValue: string) { this.lOVAdditionalColumns = newValue; }

    private customPickListCode: string;
    public get CustomPickListCode() { return this.customPickListCode; }
    public set CustomPickListCode(newValue: string) { this.customPickListCode = newValue; }

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(newValue: string) { this.category1 = newValue; }

    private isCustom: boolean = false;
    public get IsCustom() { return this.isCustom; }
    public set IsCustom(newValue: boolean) { this.isCustom = newValue; }

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

    private fieldCode: string;
    public get FieldCode() { return this.fieldCode; }
    public set FieldCode(newValue: string) { this.fieldCode = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

    private displayname: string;
    public get DisplayName() {

        return this.displayname;

    }
    public set DisplayName(newValue: string) {

        this.displayname = newValue;
    }

    private isMultipleSelection: boolean;
    public get IsMultipleSelection() { return this.isMultipleSelection; }
    public set IsMultipleSelection(newValue: boolean) { this.isMultipleSelection = newValue; }

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
            this.HasTree = !this.HideTree ? true : false;
            var MyTable = this.MyParentClass.AllTables.filter(a => a.Code == this.DimensionTableCode);
            if (MyTable && MyTable.length > 0) {
                this.Code = MyTable[0].DefaultFilterBy;
                if (this.code && this.MyParentClass && this.MyParentClass.AllFieldsObsList) {
                    var field = this.MyParentClass.AllFieldsObsList.filter(d => d.Code == this.code && d.DWObjectTableCode == this.DimensionTableCode)[0];
                    if (field) {
                        this.LOVAdditionalColumns = field.LOVAdditionalColumns;
                    }
                }

                this.DWObjectTableCode = MyTable[0].Code;

                this.TranslationText = this.GetTranslationText(this);
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
        else {
            if (newValue == "DateTime" && this.DWObjectTableCode.indexOf("DIM_Date") == -1) {
                this.HasTree = true;

            }
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
    public get Operators() { return this.GetSelectedFieldOperators(this); }
    public set Operators(newValue: ObjectFieldOperator[]) {
        this.operators = newValue;
    }

    private boolValue: string = "No";
    public get BoolValue() {
        if (this.TextValue == true) {
            this.boolValue = "Yes";
        }
        else if (this.TextValue == false) {
            this.boolValue = "No";
        }
        else {
            this.boolValue = "No Value";
        }
        return this.boolValue;
    }
    public set BoolValue(newValue: any) {
        if (this.boolValue != newValue) {
            this.boolValue = newValue;
        }
    }

    private textValue: any = (this.dataTypeCode == "Boolean") ? false : null;
    public get TextValue() {
        return this.textValue;
    }
    public set TextValue(newValue: any) {
        if (this.textValue != newValue) {
            this.textValue = newValue;

            if (this.ParentDataTypeCode == "DateTime" || this.ParentDataTypeCode == "Date") {
                var timerToken = setTimeout(() => {
                    this.ShowSampleDateCommand.emit(this);
                }, 0);
            }
            this.MyParentClass.ClearData();
            //if ((newValue == true || newValue == false) && this.textValue) {
            //    this.textValue = newValue;

            //    //if (!this.DontSaveChanges) {
            //    //    //this.MyParentClass.SaveChanges();
            //    //    this.MyParentClass.ClearData();
            //    //}


            //}
            //else if (newValue != true || newValue != false) {
            //    this.textValue = newValue;
            //    if (!this.DontSaveChanges) {
            //        //this.MyParentClass.SaveChanges();
            //        this.MyParentClass.ClearData();
            //    }
            //}

            this.DontSaveChanges = false;
        }
    }

    public setTextValue(Newvalue: any, ClearData: boolean = true) {
        this.textValue = Newvalue;
        if (ClearData == true) {
            this.MyParentClass.ClearData();
        }
    }

    public setAndOrOperation(Newvalue: string, ClearData: boolean = true) {
        this.AndOr = Newvalue;
        if (ClearData == true) {
            this.MyParentClass.ClearData();
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
            let headerName = (dimensionTableCode == "DIM_Partners" && this.LOVAdditionalColumns && this.LOVAdditionalColumns.indexOf('[Local Name]') != -1) ? "Local Name" : (this.Code ? this.Code.replace('[', '').replace(']', '') : "");
            this.multiSelectedValueLists.forEach((field) => {
                textValue += this.ResolveValue(field, headerName) + ";";
            });

            textValue += "@@";
            textValue = textValue.replace(";@@", "");
            textValue = textValue.replace("@@", "");
        }
        return textValue;
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

    private columnName: string;
    public get ColumnName() { return this.columnName; }
    public set ColumnName(value: string) { this.columnName = value; }

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
            else if ((this.ParentDataTypeCode == "Date" || this.ParentDataTypeCode == "DateTime") && AppTool.IsNullOrEmpty(this.operation)) {
                this.operation = new ObjectFieldOperator("Before", "Before");
                this.OperationCode = "Before";
                this.OperationName = "Before";
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
    }

    private filterType: string = "Ask User";
    public get FilterType() { return this.filterType; }
    public set FilterType(newValue: string) { this.filterType = newValue; }

    private filterTypeSelected: ObjectFieldOperator;
    public get FilterTypeSelected() { return this.filterTypeSelected; }
    public set FilterTypeSelected(newValue: ObjectFieldOperator) {

        this.filterTypeSelected = newValue;
        if (this.filterTypeSelected) {
            this.FilterType = this.filterTypeSelected.Code;
        }
    }

    private isSetDefaults: boolean = true;
    public get IsSetDefaults() { return this.isSetDefaults; }
    public set IsSetDefaults(newValue: boolean) {
        if (this.isSetDefaults != newValue) {
            this.isSetDefaults = newValue;
            if (newValue == false) {
                this.TextValue = null;
            }
        }
    }

    private isMandatoryFilter: boolean = false;
    public get IsMandatoryFilter() { return this.isMandatoryFilter; }
    public set IsMandatoryFilter(newValue: boolean) { if (this.isMandatoryFilter != newValue) { this.isMandatoryFilter = newValue; } }

    FilterTypeChanged(Value) {
        this.FilterTypeSelected = Value;
    }

    OpenFilterSettings() {
        var windowArgs: any = {};
        windowArgs.IsMandatoryFilter = this.IsMandatoryFilter;
        windowArgs.IsSetDefaults = this.IsSetDefaults;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 500;
        logWindow.Height = 260;
        logWindow.Title = "Dynamic Filter Settings";
        logWindow.Show('./CommonModules/CommonOthers/Components/DWQueryBuilder/DWFilterSettings');
        logWindow.WindowClosed.subscribe(($event: string) => {
            if ($event) {
                var MySettings = $event.split(',');
                if (MySettings[0] == "true") {
                    this.IsMandatoryFilter = true;
                }
                else {
                    this.IsMandatoryFilter = false;
                }
                if (MySettings[1] == "true") {
                    this.IsSetDefaults = true;
                }
                else {
                    this.IsSetDefaults = false;
                }
            }
        });
    }

    DontSaveChanges: boolean = false;

    OperationValueChanged(operation) {
        if (operation.Code == this.currentOp.Code || operation.Code == this.beforeOp.Code || operation.Code == this.afterOp.Code || operation.Code == this.previousOp.Code || operation.Code == this.nextOp.Code || operation.Code == this.currentOp.Code || operation.Code == this.BetweenOp.Code) {
            this.DontSaveChanges = true;
            this.Operation = operation;
        } else {
            if (this.Operation.Code == this.IsNullOp.Code || this.Operation.Code == this.IsNotNullOp.Code) {
                this.TextValue = "";
                this.MultiSelectedValueLists = [];
            }
            this.Operation = operation;
            if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
                this.TextValue = operation.Code;
            }
            if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !AppTool.IsNullOrEmpty(this.TextValue)) {
                this.MyParentClass.ClearData();
            }
        }
    }

    LoadItems(DWObjectField: any) {
        if (this.IsViewTree) {
            this.IsViewTree = false;
            this.Items = [];
        }
        else {
            if (this.Items.length == 0 || AppTool.IsNullOrEmpty(DWObjectField.MyParentClass.searchText)) {
                this.Load(DWObjectField, null);
            }
            else this.IsViewTree = true;
        }
    }

    Load(DWObjectField: any, SelectedDWObjectFields: any) {
        var ObsList = [];
        if (DWObjectField.DataTypeCode == "DateTime") {
            this.GetDataTimeTree(DWObjectField);
            return;
        }

        var parentfieldCode = !AppTool.IsNullOrEmpty(DWObjectField.FieldCode) ? (DWObjectField.FieldCode.replace("[", "").replace("]", "")) : DWObjectField.DisplayName;
        var selectedDimensionDWObjectFields = SelectedDWObjectFields;
        if (SelectedDWObjectFields == null)
            selectedDimensionDWObjectFields = DWObjectField.MyParentClass.DWObjectFields.filter(d => d.DimensionTableCode == DWObjectField.DimensionTableCode && d.DimensionTableDisplayName == DWObjectField.Name);

        selectedDimensionDWObjectFields.filter(d => AppTool.IsNullOrEmpty(d.RecordType) || (!AppTool.IsNullOrEmpty(d.RecordType) && d.RecordType.split(',').indexOf(parentfieldCode) != -1)).forEach((field) => {
            if (field.DisplayInQueryBuilder == true) {
                var view = new DWObjectFieldsDetails(field, this.MyParentClass);
                if (field.Code == '[Full Date]' || field.Code == '[Full Date US]') {
                    view.ParentDataTypeCode = field.DataTypeCode;
                    view.DataTypeCode = "Date";
                }
                else {
                    view.ParentDataTypeCode = DWObjectField.DataTypeCode;
                }
                var dwObjectFieldName: string = DWObjectField.DisplayName;
                if (AppTool.IsNullOrEmpty(DWObjectField.Code) && !AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) view.DisplayName = dwObjectFieldName;
                else view.DisplayName = view.DisplayName;

                view.ParentCode = DWObjectField.Code;
                view.ParentDimTabelName = DWObjectField.DimensionTableCode;
                view.DimensionTableDisplayName = parentfieldCode;
                ObsList.push(view);
            }
        });
        this.Items = ObsList;
        this.IsViewTree = true;
    }

    LoadWithSearchValue(DWObjectField: any, newValue: any) {
        var selectedDimensionDWObjectFields = DWObjectField.MyParentClass.DWObjectFields.filter(d => d.DimensionTableCode == DWObjectField.DimensionTableCode && d.DimensionTableDisplayName == DWObjectField.Name && d.Name.toLowerCase().indexOf(newValue.toLowerCase()) > -1);
        selectedDimensionDWObjectFields = selectedDimensionDWObjectFields.filter(
            (thing, i, arr) => arr.findIndex(t => t.Code === thing.Code) === i
        );
        this.Load(DWObjectField, selectedDimensionDWObjectFields);
    }

    GetDataTimeTree(DWObjectField: any) {
        var ObsList = [];
        let listDateFields: string[] = ['Date', 'Time'];
        listDateFields.forEach((item) => {
            var dWObjectFieldPM: DWObjectFieldPM = new DWObjectFieldPM();
            dWObjectFieldPM.Code = DWObjectField.Code;
            dWObjectFieldPM.DataTypeCode = item;
            dWObjectFieldPM.CannotFilter = true;
            dWObjectFieldPM.Name = item;
            dWObjectFieldPM.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
            dWObjectFieldPM.DWObjectTableCode = DWObjectField.DWObjectTableCode;
            var view = new DWObjectFieldsDetails(dWObjectFieldPM, this.MyParentClass);
            view.displayname = DWObjectField.DisplayName + " " + item;
            view.ParentDataTypeCode = "DateParts";
            view.parentDimTabelName = DWObjectField.ParentDimTabelName;
            dWObjectFieldPM.IsCustom = view.isCustom = DWObjectField.IsCustom;
            ObsList.push(view);
            this.Items = ObsList;
            this.IsViewTree = true;
        });
    }

    onTextChange(value) {
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

    AndOrOpsChanged(value) {
        this.AndOr = value;
    }

    OnMouseOver(event) {
        var e = event.toElement;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    }

    OnMouseOut(event) {
        var e = event.toElement;
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
        this.FilterItems.push(DWObjectField);
    }

    AddGroup(Father: DWObjectFieldsDetails) {
        var DWObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = Father.MyParentClass.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        Father.MyParentClass.SelectedFiltersDataSource.push(DWObjectField);
    }

    FieldValueChanged(DWObjectField: DWObjectFieldsDetails) {
        this.TextValue = "";
        this.MultiSelectedValueLists = [];
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        this.TranslationText = this.GetTranslationText(DWObjectField);
        this.DisplayName = this.ComputeDisplayName(DWObjectField);//(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);

        if (!AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
            this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
        }
        else {
            this.DimensionTableDisplayName = DWObjectField.ParentCode;
        }

        this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        this.IsMeasurement = DWObjectField.IsMeasurement;
        this.IsCustom = DWObjectField.IsCustom;
        this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
        if (this.DWObjectTableCode.indexOf("DIM_") != -1) {
            if (this.Code == '[Full Date]' || this.Code == '[Full Date US]') {
                this.ParentDataTypeCode = "Date";
                this.DataTypeCode = "Date";
            } else this.ParentDataTypeCode = "LookUp";

            this.ParentDimTabelName = DWObjectField.DWObjectTableCode;

            if (this.DataTypeCode == "Boolean") this.ParentDataTypeCode = "Boolean";
        }
        else {
            this.ParentDataTypeCode = DWObjectField.DataTypeCode;

            this.ParentDimTabelName = DWObjectField.ParentDimTabelName;
        }
        this.Operators = this.GetSelectedFieldOperators(this);

        if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText")) {
            this.Operation = new ObjectFieldOperator("StartsWith", "Starts With");
        }
        else if ((this.ParentDataTypeCode == "Date" || this.ParentDataTypeCode == "DateTime")) {
            this.Operation = new ObjectFieldOperator("Before", "Before");
        }
        else {
            this.Operation = new ObjectFieldOperator("Equals", "Equals to");
        }
    }

    onDeleteFilterClick() {
        this.MyParentClass.DeleteField(this, this.MyParentClass.SelectedFiltersDataSource);
    }
}
