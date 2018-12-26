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

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIReportComponent.html',
})

export class BIReportComponent {
  
    public ItemsSource: BIReportList[] = [];
    private BIReportListService: BIReportListService;

    constructor(private entityResourceService: EntityResourceService) {
        this.LoadData();
    }

    LoadData() {
        this.BIReportListService = new BIReportListService();
        this.BIReportListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var myResult: BIReportList[] = myResponse.Result;
                this.ItemsSource = myResult;
            }
        });
    }

    public NewBIReportButtonClicked() {
        var windowTitle = "New BI eport";
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

    EditBIReportClickedViewBIReportClicked(report: BIReportList) {
        if (!AppTool.IsNullOrEmpty(report.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: report.Id, ObjectTableName: 'BIReport' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    ViewBIReportClickedViewBIReportClicked(report: BIReportList) {
       
    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
    }
}
