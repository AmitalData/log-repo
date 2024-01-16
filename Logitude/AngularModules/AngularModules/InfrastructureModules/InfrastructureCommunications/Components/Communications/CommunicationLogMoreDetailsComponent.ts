declare var window: any;
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LogTab } from '../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';

@Component({
    

    selector: 'communication-log-more-details',
    templateUrl: './CommunicationLogMoreDetailsComponent.html',
})

export class CommunicationLogMoreDetailsComponent
    implements OnInit
{
    MyTabs: LogTab[] = [];
    selectedTab: LogTab;
    resurceReady: boolean = false;

    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    constructor(
        private _entityResourceService: EntityResourceService,
        private cd: ChangeDetectorRef,
    ) {
        this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe((response: any) => {
            this.resurceReady = true;
            this.cd.detectChanges()            
        });
     }

    ngOnInit() {
    }

    OnSelectedChanged(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
            console.log("Tab selected: ", tab);
        }
    }
   
    SetWindowArgs(CustomsRequestsSheetList: any) {
        //alert();
        var tab = new LogTab();
        //this.EntityPM.Id, this.EntityPM.Tenant
        tab.EntityPM = {
            'Id': CustomsRequestsSheetList.RequestComminicationId,
            'Tenant': CustomsRequestsSheetList.Tenant,
        };
        tab.Code = "CommunicationLogSteps";
        tab.Header = TextCodeTranslator.Translate("CommunicationLogSteps.O.CommunicationLogSteps");
        //tab.ComponentPath = "./Components/Maintenance/CommunicationLog/CommunicationLogStepsComponent";
        //tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/ConsigmentTabContentComponent";
        //tab.ComponentPath = './Components/Communications/CommunicationStepsComponent';
        tab.ComponentPath = './InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationStepsComponent';
        this.MyTabs.push(tab);

        var tab2 = new LogTab();
        tab2.EntityPM = {
            'Id': CustomsRequestsSheetList.RequestComminicationId,
            'Tenant': CustomsRequestsSheetList.Tenant,
            'CorrelationId': CustomsRequestsSheetList.CorrelationId,
            'CustomsRequestsSheetId': CustomsRequestsSheetList.Id,

        };
        tab2.Code = "MoreDetails";
        tab2.Header = TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");
        tab2.ComponentPath = './InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationMoreComponent';
        this.MyTabs.push(tab2);

        //console.debug("SetWindowArgs");
    }
}
