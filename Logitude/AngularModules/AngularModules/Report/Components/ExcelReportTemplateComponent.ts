
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ExcelReportService } from 'Common/Services/ExtendedLists/ExcelReportService';
import { ReportsTemplatePM } from 'Common/EntityPMs/ReportsTemplatePM';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { DataProviderField } from 'Common/DataContracts/DataProviderField';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ExcelReportArguments } from 'Common/DataContracts/ExcelReportArguments';

@Component({

    selector: 'ExcelReportTemplateComponent',
    templateUrl: './ExcelReportTemplateComponent.html',
})


export class ExcelReportTemplateComponent implements OnInit {

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
    IsSearchIconVisible: boolean = true;
    SearchText: string;

    DataProviderFields: DataProviderField[];
    DataProviderFieldsAll: DataProviderField[];

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
        this.isNew = args.isNew;
        this.LoadReportTemplate();
    }

    LoadReportTemplate() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.excelReportService.getDataProviderFields(this.ReportTemplatePM.ReportId, this.ReportTemplatePM.Id, this.isNew).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.FillDataProviderFields(myResponse.Result)
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    FillDataProviderFields(result: any) {
        this.DataProviderFields = new Array<DataProviderField>();
        this.DataProviderFieldsAll = new Array<DataProviderField>();
        result.forEach((item) => {
            this.DataProviderFieldsAll.push(item);
            this.DataProviderFields = this.DataProviderFieldsAll;
        });
        this.TextSearchEnabled();
    }

    private TextSearchEnabled() {
        if (this.DataProviderFields.length > 0)
            this.IsTextSearchEnabled = true;
        else
            this.IsTextSearchEnabled = false;
    }

    OnFucos() {
        this.IsSearchIconVisible = false;
    }

    OnLostFucos() {
        if (this.SearchText) {
            this.IsSearchIconVisible = false;
        }
        else this.IsSearchIconVisible = true;
    }

    Search(textsearch: string) {
        this.SearchText = textsearch;
        if (textsearch) {
            this.DataProviderFields = JSON.parse(JSON.stringify(this.DataProviderFieldsAll)).filter(function f(o) {
                if (o.Text.toLowerCase().indexOf(textsearch.toLowerCase()) > -1) return true

                if (o.Fields) {
                    return (o.Fields = JSON.parse(JSON.stringify(o.Fields)).filter(f)).length
                }
            });
        }
        else {
            this.DataProviderFields = this.DataProviderFieldsAll;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    SaveButtonClicked() {
        var selectedDataProviderFields: DataProviderField[] = this.GetSelectedDataProviderFields(this.CloneList(this.DataProviderFields));

        if (selectedDataProviderFields == null || selectedDataProviderFields.length == 0) {
            return;
        }

        this.CurrentSession.StartBusyIndicatorLoading();

        var excelReportArguments: ExcelReportArguments = this.GetExcelReportArguments(selectedDataProviderFields);

        this.excelReportService.postDataProviderProperties(excelReportArguments).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("");
        });
    }

    private GetExcelReportArguments(selectedDataProviderFields: DataProviderField[]) {
        var excelReportArguments: ExcelReportArguments = new ExcelReportArguments();
        excelReportArguments.DataProviderFields = selectedDataProviderFields;
        excelReportArguments.ReportId = this.ReportTemplatePM.ReportId;
        excelReportArguments.ReportsTemplateId = this.ReportTemplatePM.Id;
        excelReportArguments.IsNew = this.isNew;
        return excelReportArguments;
    }

    CloneList(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

    GetSelectedDataProviderFields(dataProviderFields: DataProviderField[]): DataProviderField[] {
        var selectedDataProviderFields: DataProviderField[] = [];
        dataProviderFields.forEach(item => {
            if (item.IsChecked) {
                selectedDataProviderFields.push(item);
            }
            if (item.Fields != null) {
                item.Fields = this.GetSelectedDataProviderFields(this.CloneList(item.Fields));
            }
        });
        return selectedDataProviderFields;
    }



    PreviewButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }



}