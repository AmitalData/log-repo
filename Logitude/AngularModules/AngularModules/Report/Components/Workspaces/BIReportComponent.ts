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

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIReportComponent.html',
})

export class BIReportComponent {
  

    constructor(private entityResourceService: EntityResourceService) {
        
    }

    public NewBIReportButtonClicked() {


    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
    }
}
