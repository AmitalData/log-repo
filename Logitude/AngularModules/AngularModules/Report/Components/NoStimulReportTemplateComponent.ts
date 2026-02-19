declare var window: any;

import { Component, OnInit } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ExcelReportService } from 'Common/Services/ExtendedLists/ExcelReportService';
import { ReportsTemplatePM } from 'Common/EntityPMs/ReportsTemplatePM';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DataProviderField } from 'Common/DataContracts/DataProviderField';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ExcelReportArguments } from 'Common/DataContracts/ExcelReportArguments';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { ExcelReportResult } from 'Common/DataContracts/ExcelReportResult';

@Component({
    selector: 'NoStimulReportTemplateComponent',
    templateUrl: './NoStimulReportTemplateComponent.html',
})
export class NoStimulReportTemplateComponent implements OnInit {
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

    DataProviderFields: DataProviderField[] = [];
    DataProviderFieldsAll: DataProviderField[] = [];
    SelectedDataProviderFieldsFlat: DataProviderField[] = [];
    SelectedField: DataProviderField;
    SelectedListName: any;

    public IsbtnAddDisabled: boolean = true;
    public IsbtnRemoveDisabled: boolean = true;
    public IsbtnUpDisabled: boolean = true;
    public IsbtnDownDisabled: boolean = true;

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
            distinctUntilChanged()
        ).subscribe((search: string): any => {
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
        this.excelReportService.getDataProviderFields(this.ReportTemplatePM.ReportId, this.ReportTemplatePM.Id, 10)
            .subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.FillDataProviderFields(myResponse.Result)
                this.CurrentSession.StopBusyIndicator();
            });
    }

    FillDataProviderFields(result: ExcelReportResult) {
        this.DataProviderFields = [];
        this.DataProviderFieldsAll = [];
        result.DataProviderFields.forEach((item) => {
            this.DataProviderFieldsAll.push(item);
            this.DataProviderFields.push(this.Clone(item));
        });
        this.SetSelectedLists(result.SelectedDataProviderFields);
        this.TextSearchEnabled();
    }

    SetSelectedLists(selectedDataProviderFields: DataProviderField[]) {
        this.SelectedDataProviderFieldsFlat = [];
        if (!selectedDataProviderFields) return;

        this.SelectedDataProviderFieldsFlat = this.flattenSelectedFields(selectedDataProviderFields);

    }
    

    private TextSearchEnabled() {
        this.IsTextSearchEnabled = this.DataProviderFields.length > 0;
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
        const lower = textsearch.toLowerCase();
        const filterRecursive = (fields: DataProviderField[]): DataProviderField[] => {
            const result: DataProviderField[] = [];
            for (const field of fields) {
                const match = field.Text.toLowerCase().includes(lower);
                const children = field.Fields ? filterRecursive(field.Fields) : [];
                if (match || children.length > 0) {
                    const clone = this.Clone(field);
                    clone.Fields = children;
                    result.push(clone);
                }
            }
            return result;
        };
        return filterRecursive(this.DataProviderFieldsAll);
    }
    private flattenSelectedFields(fields: DataProviderField[]): DataProviderField[] {
        let result: DataProviderField[] = [];
      
        fields.forEach(field => {
          if (field.IsChecked && field.Type !== 'List') {
            result.push({ ...field, Fields: [] });
          }
      
          if (field.Fields && field.Fields.length > 0) {
            result = result.concat(this.flattenSelectedFields(field.Fields));
          }
        });
      
        return result.sort((a, b) => a.Sort - b.Sort);
      }
   

    btnAdd_Click() {
        this.SelectedDataProviderFieldsFlat.push(this.SelectedField);
      
        this.SelectedField.IsChecked = true;
   
        this.CheckUncheckFields(this.DataProviderFields, true, true);
        this.IsbtnAddDisabled = true;
        this.SelectedField = null;
        this.UnSelectAllFields();
    }

        
    CheckUncheckFields(fields: DataProviderField[], checkSubFields: boolean, isChecked: boolean) {
        for (const field of fields) {
            if (field.Expression === this.SelectedField?.Expression) {
                field.IsChecked = isChecked;
            }
            if (checkSubFields && field.Fields?.length) {
                this.CheckUncheckFields(field.Fields, checkSubFields, isChecked);
            }
        }
    }

    private IsHeaderField(field: DataProviderField) {
        return field.Expression.split('.').length == 2 && (field.Fields == null || field.Fields.length == 0);
    }

    btnRemove_Click() {
        if (this.IsHeaderField(this.SelectedField))
            this.SelectedDataProviderFieldsFlat = this.SelectedDataProviderFieldsFlat.filter(item => item !== this.SelectedField);
      else this.RemoveListField();
      this.SelectedField.IsChecked = false;
        this.CheckUncheckFields(this.DataProviderFields, true, false);
        this.IsbtnRemoveDisabled = true;
        this.SelectedField = null;
        this.UnSelectAllFields();
    }

    RemoveListField() {
        this.SelectedDataProviderFieldsFlat = this.removeFieldRecursive(
            this.SelectedDataProviderFieldsFlat,
            this.SelectedField.Expression
        );
    }

    private removeFieldRecursive(fields: DataProviderField[], expression: string): DataProviderField[] {
        return fields
            .filter(f => f.Expression !== expression)
            .map(f => ({
                ...f,
                Fields: f.Fields ? this.removeFieldRecursive(f.Fields, expression) : []
            }))
            .filter(f => f.Fields.length > 0 || f.Type !== 'List');
    }

    btnUp_Click() {
        var list: DataProviderField[] = this[this.SelectedListName];;
        var i = list.indexOf(this.SelectedField);
        if (i <= 0) return;
        this.ChangeItemPosition(list, i, i - 1);
    }

    btnDown_Click() {
        var list: DataProviderField[] = this[this.SelectedListName];
        var i = list.indexOf(this.SelectedField);
        if (i >= list.length - 1) return;
        this.ChangeItemPosition(list, i, i + 1);
    }

    

    ChangeItemPosition(arr: any, fromIndex: number, toIndex: number) {
        var element = arr[fromIndex];
        arr.splice(fromIndex, 1);
        arr.splice(toIndex, 0, element);
    }
    filterCheckedAndUpdate(
        fields: DataProviderField[], 
        selectedFields: DataProviderField[]
      ): DataProviderField[] {
        return fields
          .map(field => {
            const filteredChildren = field.Fields ? this.filterCheckedAndUpdate(field.Fields, selectedFields) : [];
      
            const matchingField = selectedFields.find(f => f.Expression === field.Expression);
            if (matchingField) {
              field = {
                ...field,
                Translation: matchingField.Translation,
                Sort: matchingField.Sort,
                Fields: filteredChildren 
              };
            } else {
              field = {
                ...field,
                Fields: filteredChildren
              };
            }
      
            if (field.IsChecked) {
                if (field.Type === 'List') {
                  if (field.Fields && field.Fields.length > 0) {
                    return field;
                  }
                  return null;
                }
                return field;
              }
              if (field.Fields && field.Fields.length > 0) {
                return field;
              }
              return null;
          })
          .filter(f => f !== null) as DataProviderField[];
      }
      
      
    SaveButtonClicked() {
        this.ReorderColumnsList(this.SelectedDataProviderFieldsFlat, true);
        const selectedDataProviderFieldsForSave = this.filterCheckedAndUpdate(this.DataProviderFields, this.SelectedDataProviderFieldsFlat);
     
        const excelReportArguments: ExcelReportArguments = this.GetExcelReportArguments(selectedDataProviderFieldsForSave);
    
        this.CurrentSession.StartBusyIndicatorLoading();
        this.excelReportService.postDataProviderProperties(excelReportArguments).subscribe(() => {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("");
        });
    }

    
    ReorderColumnsList(dataProviderFields: DataProviderField[], checkSubFields: boolean) {
        for (let index = 0; index < dataProviderFields.length; index++) {
            dataProviderFields[index].Sort = index + 1;
            if (checkSubFields && dataProviderFields[index].Fields?.length > 0)
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
    private UnSelectAllFields() {
        this.UnselectField(this.SelectedDataProviderFieldsFlat);
        this.UnselectField(this.DataProviderFields);
    }

    UnselectField(dataProviderFields: DataProviderField[]) {
        dataProviderFields.forEach(field => {
            field.ClassName = "ListBoxItem";
            if (field.Fields != null) this.UnselectField(field.Fields);
        });
    }
    SelectChanges(selectedField: DataProviderField, selectedListName: string) {
        this.SelectedField = selectedField;
        this.SelectedListName = selectedListName;

        this.IsbtnAddDisabled = selectedListName != 'DataProviderFields';
        this.IsbtnRemoveDisabled = selectedListName == 'DataProviderFields';

        this.IsbtnUpDisabled = selectedListName != 'SelectedDataProviderFieldsFlat';
        this.IsbtnDownDisabled = selectedListName != 'SelectedDataProviderFieldsFlat';

        this.UnSelectAllFields();
    }

    
    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }


    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }
} 
