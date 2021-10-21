declare var window: any;

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
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';

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
        this.LoadReportTemplate();
    }

    LoadReportTemplate() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.excelReportService.getDataProviderFields(this.ReportTemplatePM.ReportId, this.ReportTemplatePM.Id)
            .subscribe((myResponse: ServiceResponse) => {
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
            this.DataProviderFields.push(this.Clone(item));
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

    CheckChanges(expression: string) {
        this.FindItem(expression, this.DataProviderFieldsAll);
    }

    FindItem(expression: string, list: DataProviderField[]) {
        list.forEach(item => {
            if (item.Expression == expression) {
                item.IsChecked = !item.IsChecked;
                return;
            }

            if (item.Fields != null) {
                this.FindItem(expression, item.Fields);
            }
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("");
    }

    SaveButtonClicked(preview: boolean) {
        var selectedDataProviderFields: DataProviderField[] = this.GetSelectedDataProviderFields(this.Clone(this.DataProviderFieldsAll));

        var excelReportArguments: ExcelReportArguments = this.GetExcelReportArguments(selectedDataProviderFields);

        this.CurrentSession.StartBusyIndicatorLoading();
        this.excelReportService.postDataProviderProperties(excelReportArguments).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            preview ? this.OpenStimulsoftDesigner() : this.CurrentSession.CurrentWindow.Close("");
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

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

    GetSelectedDataProviderFields(dataProviderFields: DataProviderField[]): DataProviderField[] {
        var selectedDataProviderFields: DataProviderField[] = [];
        dataProviderFields.forEach(item => {
            if (item.IsChecked) {
                selectedDataProviderFields.push(item);
            }
            if (item.Fields != null) {
                item.Fields = this.GetSelectedDataProviderFields(this.Clone(item.Fields));
            }
        });
        return selectedDataProviderFields;
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

}