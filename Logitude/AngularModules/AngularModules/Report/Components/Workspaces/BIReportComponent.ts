import { Component, OnInit, ElementRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ReportList } from '../../EntityLists/ReportList';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportService } from '../../../Common/Services/ExtendedLists/ReportService';
import { ReportGroupService } from '../../../Common/Services/ExtendedLists/ReportGroupService';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { ReportsTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BIReportList } from '../../../Infrastructure/EntityLists/BIReportList';
import { BIReportListService } from '../../../Infrastructure/Services/StandardLists/BIReportListService';
import { InfrastructureDomainService } from '../../../Infrastructure/Services/InfrastructureDomainService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIReportComponent.html',
})

export class BIReportComponent {
  
    public ItemsSource: BIReportList[] = [];
    private BIReportListService: BIReportListService;
    private InfrastructureDomainService: InfrastructureDomainService;

    constructor(private entityResourceService: EntityResourceService) {
        this.LoadData();
        this.InfrastructureDomainService = new InfrastructureDomainService();
    }

    InitComponent() {

    }

    LoadData() {
        this.ItemsSource = [];
        this.BIReportListService = new BIReportListService();
        this.BIReportListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var myResult: BIReportList[] = myResponse.Result;
                //this.ItemsSource = myResult;
                if (AppTool.IsNullOrEmpty(this.mySearchText)) {
                    this.ItemsSource = myResult;
                }
                else {
                    myResult.forEach((item) => {
                        if (!AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.mySearchText.toUpperCase()) > -1
                            ||
                            !AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.mySearchText.toUpperCase()) > -1) {
                            this.ItemsSource.push(item);
                        }
                    });
                }
            }
        });
    }

    public NewBIReportButtonClicked() {
        var windowTitle = "New BI Report";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewBIReportWindowClosed($event));
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');

    }
    OnNewBIReportWindowClosed(arg: any) {
        if (arg != 'cancel') {
            this.LoadData();
        }
    }

    EditBIReportClicked(report: BIReportList) {
        this.entityResourceService.getEntityResourceByTableName("BIReport", 0).subscribe(response => {
            if (!AppTool.IsNullOrEmpty(report.Id)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: report.Id, ObjectTableName: 'BIReport' });
                        cmpRef.instance.BackCompleted.subscribe(bk => {
                        });
                    });
            }
        });
    }

    ViewBIReportClicked(report: BIReportList) {


    }

    ExportToExcelClicked(report: BIReportList) {
        var windowArgs: any = {};
        windowArgs.queryId = report.DWQueryId;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/ExportBI2ExcelControl/ExportBI2ExcelControl');
    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
        this.LoadData();
    }
}
