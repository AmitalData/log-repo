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

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIReportComponent.html',
})

export class BIReportComponent {
  

    constructor(private entityResourceService: EntityResourceService) {
        
    }

    public NewBIReportButtonClicked() {
        var windowTitle = "New BI eport";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 850;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewBIReportWindowClosed($event));
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');

    }
    OnNewBIReportWindowClosed(arg: any) {


    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
    }
}
