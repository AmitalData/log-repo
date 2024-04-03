declare var window: any;

import { Component, OnInit, } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ExcelReportService } from 'Common/Services/ExtendedLists/ExcelReportService';
import { ReportsTemplatePM } from 'Common/EntityPMs/ReportsTemplatePM';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DataProviderField } from 'Common/DataContracts/DataProviderField';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ExcelReportArguments } from 'Common/DataContracts/ExcelReportArguments';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { ExcelReportResult } from 'Common/DataContracts/ExcelReportResult';

@Component({
    selector: 'ExcelReportTemplateComponent',
    templateUrl: './ExcelReportTemplateComponent.html',
})


export class ExcelReportTemplateComponent implements OnInit {
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);

    public SearchTextValue: FormControl;

    private CurrentSession = SessionLocator.SelectedSession;
    TemplateData: any;
    DataViewModel: any;
    TemplateType: string;
    Area: string;
    ReportTemplatePM: ReportsTemplatePM;
    isNew: boolean;
    excelReportService: ExcelReportService;
    IsTextSearchEnabled: boolean = false;
    SearchText: string;

    DataProviderFields: DataProviderField[];
    DataProviderFieldsAll: DataProviderField[];
    SelectedHeaderDataProviderFields: DataProviderField[];
    SelectedListsDataProviderFields: DataProviderField[];
    SelectedField: DataProviderField;
    SelectedListName: any;


    public IsbtnAddDisabled: boolean = true;
    public IsbtnRemoveDisabled: boolean = true;
    public IsbtnUpDisabled: boolean = true;
    public IsbtnDownDisabled: boolean = true;
    public IsListsbtnDownDisabled: boolean = true;
    public IsListsbtnUpDisabled: boolean = true;

    constructor() {
        this.excelReportService = new ExcelReportService();
    }

    ngOnInit(): void {
        this.SetSearchView();
    }
    SetSearchView() {
        this.SearchTextValue = new FormControl();
        this.SearchTextValue.valueChanges.pipe(
            debounceTime(500),
            distinctUntilChanged())
            .subscribe((search: string): any => {
                this.Search(search);
            });

    }

    SetWindowArgs(args: any) {
        this.DataViewModel = args.DataViewModel;
        this.TemplateType = args.TemplateType;
        this.Area = args.Area;
        this.ReportTemplatePM = args.ReportTemplatePM;
        this.LoadReportTemplate();
    }

    LoadReportTemplate() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.excelReportService.getDataProviderFields(this.ReportTemplatePM.ReportId, this.ReportTemplatePM.Id)
            .subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.FillDataProviderFields(myResponse.Result)
                this.CurrentSession.StopBusyIndicator();
            });
    }

    FillDataProviderFields(result: ExcelReportResult) {
        this.DataProviderFields = new Array<DataProviderField>();
        this.DataProviderFieldsAll = new Array<DataProviderField>();
        result.DataProviderFields.forEach((item) => {
            this.DataProviderFieldsAll.push(item);
            this.DataProviderFields.push(this.Clone(item));
        });
        this.SetSelectedLists(result.SelectedDataProviderFields);
        this.TextSearchEnabled();
    }

    SetSelectedLists(selectedDataProviderFields: DataProviderField[]) {
        this.SelectedHeaderDataProviderFields = [];
        this.SelectedListsDataProviderFields = [];
        if (!selectedDataProviderFields) return;

        selectedDataProviderFields.forEach(element => {
            if ((element.Type != 'Class' && element.Type != 'List')) this.SelectedHeaderDataProviderFields.push(this.Clone(element));
            else this.SelectedListsDataProviderFields.push(this.Clone(element));
        });
    }

    private TextSearchEnabled() {
        if (this.DataProviderFields.length > 0) this.IsTextSearchEnabled = true;
        else this.IsTextSearchEnabled = false;
    }

    Search(textsearch: string) {
        this.SearchText = textsearch;
        if (textsearch) {
            this.DataProviderFields = this.FilterDataProviderFields(textsearch);
        }
        else {
            this.DataProviderFields = this.DataProviderFieldsAll;
        }
    }

    FilterDataProviderFields(textsearch: string): DataProviderField[] {
        return this.Clone(this.DataProviderFieldsAll).filter(function f(o) {
            if (o.Text.toLowerCase().indexOf(textsearch.toLowerCase()) > -1) return true

            if (o.Fields) {
                return (o.Fields = JSON.parse(JSON.stringify(o.Fields)).filter(f)).length
            }
        });
    }


    SelectChanges(selectedField: DataProviderField, selectedListName: string) {
        this.SelectedField = selectedField;
        this.SelectedListName = selectedListName;

        this.IsbtnAddDisabled = selectedListName != 'DataProviderFields';
        this.IsbtnRemoveDisabled = selectedListName == 'DataProviderFields';

        this.IsbtnUpDisabled = selectedListName != 'SelectedHeaderDataProviderFields';
        this.IsbtnDownDisabled = selectedListName != 'SelectedHeaderDataProviderFields';

        this.IsListsbtnDownDisabled = selectedListName != 'SelectedListsDataProviderFields';
        this.IsListsbtnUpDisabled = selectedListName != 'SelectedListsDataProviderFields';

        this.UnSelectAllFields();
    }

    private UnSelectAllFields() {
        this.UnselectField(this.SelectedHeaderDataProviderFields);
        this.UnselectField(this.SelectedListsDataProviderFields);
        this.UnselectField(this.DataProviderFields);
    }

    UnselectField(dataProviderFields: DataProviderField[]) {
        dataProviderFields.forEach(field => {
            field.ClassName = "ListBoxItem";
            if (field.Fields != null) this.UnselectField(field.Fields);
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    PreviewButtonClicked() {
        this.SaveButtonClicked(true);
    }

    OpenStimulsoftDesigner() {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.ProcessType = "";
        windowArgs.ReportTemplateId = this.ReportTemplatePM.Id;
        windowArgs.ReportsTemplateId = this.ReportTemplatePM.Id;
        windowArgs.Tenant = SessionLocator.Tenant;
        windowArgs.TemplateType = this.ReportTemplatePM.TemplateType;

        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var logWindow = new LogitudeWindow();

        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = this.ReportTemplatePM.Description;

        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        window.designerClosed = false;
        logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
    }


    btnAdd_Click() {
        if (this.IsHeaderField(this.SelectedField)) this.SelectedHeaderDataProviderFields.push(this.SelectedField);
        else this.AddFieldToListsDataProvider(this.SelectedField);

        this.CheckUncheckFields(this.DataProviderFields, true, true);
        this.IsbtnAddDisabled = true;
        this.SelectedField = null;
        this.UnSelectAllFields();
    }

    public HasFields(field: DataProviderField) {
        return field.Fields && field.Fields.length > 0;
    }

    AddFieldToListsDataProvider(dataProviderField: DataProviderField) {
        var expressionList = dataProviderField.Expression.split('.');
        var parentExpression = expressionList[0] + '.' + expressionList[1] + '}';

        if (!this.DataProviderFields.find(x => x.Expression == parentExpression)) {
            this.AddListToListsDataProvider(dataProviderField);
            return;
        }

        var existField = this.SelectedListsDataProviderFields.find(x => x.Expression == parentExpression);
        if (existField) {
            existField.Fields.push(this.Clone(dataProviderField));
            return;
        }

        var parentField = this.Clone(this.DataProviderFields.find(x => x.Expression == parentExpression));
        parentField.Fields = [];
        parentField.Fields.push(this.Clone(dataProviderField));
        this.SelectedListsDataProviderFields.push(parentField);
    }

    AddListToListsDataProvider(dataProviderField: DataProviderField) {
        dataProviderField.Fields.forEach(field => {
            this.AddListFieldToDataProvider(field);
        });
    }

    private AddListFieldToDataProvider(field: DataProviderField) {
        if (field.IsChecked) return;
        this.AddFieldToListsDataProvider(field);
        field.IsChecked = true;
    }

    CheckUncheckFields(dataProviderFields: DataProviderField[], checkSubFields: boolean, isChecked: boolean) {
        dataProviderFields.forEach(field => {
            if (field.Expression == this.SelectedField.Expression) field.IsChecked = isChecked;
            else if (checkSubFields && field.Fields != null) this.CheckUncheckFields(field.Fields, false, isChecked);
        });
    }

    private IsHeaderField(field: DataProviderField) {
        return field.Expression.split('.').length == 2 && (field.Fields == null || field.Fields.length == 0);
    }

    btnRemove_Click() {
        if (this.IsHeaderField(this.SelectedField)) this.SelectedHeaderDataProviderFields = this.SelectedHeaderDataProviderFields.filter(item => item !== this.SelectedField);
        else this.RemoveListField();

        this.CheckUncheckFields(this.DataProviderFields, true, false);
        this.IsbtnRemoveDisabled = true;
        this.SelectedField = null;
        this.UnSelectAllFields();
    }

    RemoveListField() {
        var expressionList = this.SelectedField.Expression.split('.');
        var parentExpression = expressionList[0] + '.' + expressionList[1] + '}';

        if (!this.DataProviderFields.find(x => x.Expression == parentExpression)) {
            this.SelectedListsDataProviderFields = this.SelectedListsDataProviderFields.filter(item => item.Expression !== this.SelectedField.Expression);
            this.DataProviderFields.find(x => x.Expression == this.SelectedField.Expression)?.Fields?.forEach(field => { field.IsChecked = false; });
            return;
        }

        var existField = this.SelectedListsDataProviderFields.find(x => x.Expression == parentExpression);
        if (existField) existField.Fields = existField.Fields.filter(item => item !== this.SelectedField);
        if (existField.Fields.length == 0) this.SelectedListsDataProviderFields = this.SelectedListsDataProviderFields.filter(item => item !== existField);
    }

    btnUp_Click() {
        var list = this.GetSelectedList();
        var i = list.indexOf(this.SelectedField);
        if (i <= 0) return;
        this.ChangeItemPosition(list, i, i - 1);
    }

    btnDown_Click() {
        var list = this.GetSelectedList();
        var i = list.indexOf(this.SelectedField);
        if (i >= list.length - 1) return;
        this.ChangeItemPosition(list, i, i + 1);
    }

    private GetSelectedList() {
        var list: DataProviderField[] = this[this.SelectedListName];
        if (this.SelectedListName == 'SelectedListsDataProviderFields' && !list.find(x => x.Expression == this.SelectedField.Expression))
            list = list.find(x => x.Name == this.SelectedField.Expression.split('.')[1]).Fields;
        return list;
    }

    ChangeItemPosition(arr: any, fromIndex: number, toIndex: number) {
        var element = arr[fromIndex];
        arr.splice(fromIndex, 1);
        arr.splice(toIndex, 0, element);
    }


    SaveButtonClicked(preview: boolean) {
        this.ReorderColumnsList(this.SelectedHeaderDataProviderFields, true);
        this.ReorderColumnsList(this.SelectedListsDataProviderFields, true);
        var selectedDataProviderFields: DataProviderField[] = this.Clone(this.SelectedHeaderDataProviderFields);
        selectedDataProviderFields = selectedDataProviderFields.concat(this.Clone(this.SelectedListsDataProviderFields));

        var excelReportArguments: ExcelReportArguments = this.GetExcelReportArguments(selectedDataProviderFields);

        this.CurrentSession.StartBusyIndicatorLoading();
        this.excelReportService.postDataProviderProperties(excelReportArguments).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            preview ? this.OpenStimulsoftDesigner() : this.CurrentSession.CurrentWindow.Close("");
        });
    }

    ReorderColumnsList(dataProviderFields: DataProviderField[], checkSubFields: boolean) {
        for (let index = 0; index < dataProviderFields.length; index++) {
            dataProviderFields[index].Sort = index + 1;
            if (checkSubFields && dataProviderFields[index].Fields != null && dataProviderFields[index].Fields.length > 0)
                this.ReorderColumnsList(dataProviderFields[index].Fields, false);
        }
    }

    private GetExcelReportArguments(selectedDataProviderFields: DataProviderField[]) {
        var excelReportArguments: ExcelReportArguments = new ExcelReportArguments();
        excelReportArguments.DataProviderFields = selectedDataProviderFields;
        excelReportArguments.ReportId = this.ReportTemplatePM.ReportId;
        excelReportArguments.ReportsTemplateId = this.ReportTemplatePM.Id;
        excelReportArguments.IsNew = this.isNew;
        return excelReportArguments;
    }

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

}