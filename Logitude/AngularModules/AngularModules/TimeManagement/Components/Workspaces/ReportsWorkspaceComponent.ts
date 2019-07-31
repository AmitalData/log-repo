import {Component, ElementRef} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ReportList} from '../../../Common/EntityLists/ReportList';
import {ReportGroupList} from '../../../Report/EntityLists/ReportGroupList';
import {ReportService} from '../../../Common/Services/ExtendedLists/ReportService';
import {ReportGroupService} from '../../../Common/Services/ExtendedLists/ReportGroupService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {ReportsTemplateListExtendedService} from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';

@Component({
    selector: 'ReportsWorkspaceComponent',
    moduleId: module.id,
    templateUrl: './ReportsWorkspaceComponent.html',
})

export class ReportsWorkspaceComponent {
    public ItemsSource: ReportsGrpupClass[] = [];
    public ItemsSourceTemp: ReportsGrpupClass[] = [];
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    ReportTemplates: any[] = [];
    IsLoadReportsTemplateListRuning: boolean = false;
    IsLoadSettingWorkerRoleRuning: boolean = false;
    IsViewReport: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
    }
    InitComponent() {
        this.LoadData();
    }

    RefreshTab() {
        this.LoadData();
    }
    LoadData() {
        var myService = new ReportGroupService();
        myService.getReportGroupListByCode('RTFS').subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSourceTemp = [];
                var myResult: ReportGroupList = myResponse.Result;
                this.ItemsSourceTemp.push(new ReportsGrpupClass(myResult, this));
            }
        });
    }
    BuildItemsSource() {
        if (this.ItemsSourceTemp.filter(f => f.IsDataLoaded == false).length == 0) {
            this.ItemsSource = this.ItemsSourceTemp.filter(f => f.ItemsSource.length > 0);
        }
    }

    ViewReportClicked(GroupList: ReportGroupList, ReportList: ReportList) {
        if (!this.IsViewReport) {
            this.IsViewReport = true;
            ServiceLocator.SendTotangoUserActivity("Reports", "Report View");
            this.LoadReportTemplate(GroupList, ReportList);
        }
    }
    LoadReportTemplate(groupList: ReportGroupList, reportList: ReportList) {

        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id, "R").subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;

            }
            this.IsLoadReportsTemplateListRuning = false;
            this.ViewReport(groupList, reportList);

        });
    }

    ViewReport(GroupList: ReportGroupList, ReportList: ReportList) {
        if (!this.IsLoadSettingWorkerRoleRuning && !this.IsLoadReportsTemplateListRuning) {
            SessionLocator.DynamicLoader.Load("./Report/Components/ReportsPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.ReportsPreview(GroupList, ReportList, this.ReportTemplates);
                });
            this.IsViewReport = false;
        }
    }
}
export class ReportsGrpupClass {
    public Name: string;
    public ItemsSource: ReportList[] = [];
    public GroupList: ReportGroupList;
    public IsDataLoaded: boolean = false;
    constructor(private list: ReportGroupList, private fatherComponent: ReportsWorkspaceComponent) {
        this.GroupList = list;
        this.Name = list.EnglishName;
        this.LoadData();
    }

    LoadData() {
        var myService = new ReportService();
        myService.GetReportListsByGroupId(this.list.Id, SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSource = [];

                var myResult: ReportList[] = myResponse.Result;

                myResult.forEach((item) => {

                    if (SessionLocator.Tenant == 1526 || SessionLocator.Tenant == 1525 || SessionLocator.Tenant == 1524 || SessionLocator.Tenant == 1523 || SessionLocator.Tenant == 1608) {
                        if (item.Code == "SHID") {
                            this.ItemsSource.push(item);
                        }
                    }

                    else {
                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                            this.ItemsSource.push(item);
                        }
                    }

                });

                this.IsDataLoaded = true;
                this.fatherComponent.BuildItemsSource();
            }
        });
    }
}
