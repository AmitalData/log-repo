import {Component, OnInit, ElementRef}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ReportList} from '../../EntityLists/ReportList';
import {ReportGroupList} from '../../EntityLists/ReportGroupList';
import {ReportService} from '../../../Common/Services/ExtendedLists/ReportService';
import {ReportGroupService} from '../../../Common/Services/ExtendedLists/ReportGroupService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReportsTemplateListExtendedService} from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'ReportComponent.html',
})

export class ReportComponent {
    public ItemsSource: ReportsGrpupClass[] = [];
    public ItemsSourceTemp: ReportsGrpupClass[] = [];
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    IsViewReport: boolean = false;
    showLocal: boolean = false;

    constructor(private entityResourceService: EntityResourceService) {
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
        this.LoadData();
        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
    }

    InitComponent() {

    }

    private groupList: ReportGroupList[];
    public reportList: ReportList[];
    LoadData() {
        this.groupList = [];
        this.reportList = [];

        var groupService = new ReportGroupService();
        var reportService = new ReportService();

        groupService.getReportGroupLists(0).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.groupList = myResponse.Result;
                this.groupList = this.groupList.sort((a, b) => { return a.OrderNumber - b.OrderNumber });

                this.groupList.forEach(item => {
                    reportService.GetReportListsByGroupId(item.Id, SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var myResult: ReportList[] = myResponse.Result;
                            
                            myResult.forEach((item) => {
                                if (item.Code == "AREX") {
                                    if (SessionLocator.Tenant == 1212) {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }

                                else if (item.Code == "DSCA") {
                                    if (SessionLocator.Tenant != 1212) {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }

                                else if (item.Code == "VDK") {
                                    if (SessionLocator.Tenant == 1495 || SessionLocator.TenantManagementJS.PackageCode =="DVMT") {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }

                                else if (item.Code == "SHID") {
                                    if (SessionLocator.Tenant == 1526 || SessionLocator.Tenant == 1525 || SessionLocator.Tenant == 1524 || SessionLocator.Tenant == 1523 || SessionLocator.Tenant == 1608 || SessionLocator.Tenant == 1609 || SessionLocator.Tenant == 1684 || SessionLocator.TenantManagementJS.PackageCode == "DVMT"  ) {
                                        this.reportList.push(item);
                                    }

                                    else {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }

                                else {
                                    if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                        this.reportList.push(item);
                                    }
                                }
                            });

                            this.FillTempItemsSource();
                        }
                    });
                });
            }
        });
    }

    private FillTempItemsSource() {
        this.ItemsSourceTemp = [];

        this.groupList.forEach(item => {
            var myItem: ReportsGrpupClass = new ReportsGrpupClass(item, this);
            this.ItemsSourceTemp.push(myItem);
        });
    }

    BuildItemsSource() {
        this.ItemsSource = [];

        if (this.ItemsSourceTemp.filter(f => f.IsDataLoaded == false).length == 0) {
            this.ItemsSource = this.ItemsSourceTemp.filter(f => f.ItemsSource.length > 0);
        }
    }

    ViewReportClicked(GroupList: ReportGroupList, ReportList: ReportList) {
        ServiceLocator.SendTotangoUserActivity("Reports", "Report View");
        
        var isLoadingResources = false;
        var entityResourceName = "";
        switch (ReportList.FilterControlName) {
            case "QuotesFilterControl":
                {
                    isLoadingResources = true;
                    entityResourceName = "Quote";
                    break;
                }
        }

        if (isLoadingResources) {
            this.entityResourceService.getEntityResourceByTableName(entityResourceName, 0).subscribe(p => {
                this.ViewReport(GroupList, ReportList);
            });
        }

        else {
            this.ViewReport(GroupList, ReportList);
        }
    }

    IsLoadSettingWorkerRoleRuning: boolean = false;
    IsLoadReportsTemplateListRuning: boolean = false;
    
    ViewReport(groupList: ReportGroupList, reportList: ReportList) {

        if (!this.IsViewReport) {
            this.IsViewReport = true;

            this.IsLoadSettingWorkerRoleRuning = true;
            this.IsLoadReportsTemplateListRuning = true;
            this.LoadReportTemplate(groupList, reportList);
            this.LoadReportsRunUsingWR(groupList, reportList);
        }
    }

    ReportTemplates: any[] = [];
    LoadReportTemplate(groupList: ReportGroupList, reportList: ReportList) {
     
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id,"R").subscribe((myResponse: ServiceResponse) => {
            
            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;
             
            }
            this.IsLoadReportsTemplateListRuning = false;
            this.LoadComplete(groupList, reportList);

        });
    }

    ReportsRunUsingWR: boolean = false;
    LoadReportsRunUsingWR(groupList: ReportGroupList, reportList: ReportList) {
       
        var myService = new ReportService();
          myService.GetCheckIfReportsRunUsingWR().subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.ReportsRunUsingWR = pmResponse.Result;
            }

            this.IsLoadSettingWorkerRoleRuning = false;
            this.LoadComplete(groupList, reportList);
        });
    }
    
    LoadComplete(groupList: ReportGroupList, reportList: ReportList) {

        if (!this.IsLoadSettingWorkerRoleRuning && !this.IsLoadReportsTemplateListRuning) {
            SessionLocator.DynamicLoader.Load("./Report/Components/ReportsPreviewComponent", SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.ReportsPreview(groupList, reportList, this.ReportTemplates, this.ReportsRunUsingWR);
                });

            this.IsViewReport = false;
        }
    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {         
        this.mySearchText = text;
        this.FillTempItemsSource();
    }
}
export class ReportsGrpupClass {
    public Name: string;
    public ItemsSource: ReportList[] = [];
    public GroupList: ReportGroupList;
    public IsDataLoaded: boolean = false;
    constructor(private list: ReportGroupList, private fatherComponent: ReportComponent) {
        this.GroupList = list;
        this.Name = list.EnglishName;

        this.FillData();
    }

    FillData() {
        this.ItemsSource = [];

        var myReports: ReportList[] = this.fatherComponent.reportList.filter(d => d.ReportGroupId == this.GroupList.Id);

        if (AppTool.IsNullOrEmpty(this.fatherComponent.mySearchText)) {
            myReports.forEach((item) => {
                this.ItemsSource.push(item);
            });
        }

        else {
            myReports.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.fatherComponent.mySearchText.toUpperCase()) > -1
                    ||
                    !AppTool.IsNullOrEmpty(item.LocalName) && item.LocalName.toUpperCase().indexOf(this.fatherComponent.mySearchText.toUpperCase()) > -1) {
                    this.ItemsSource.push(item);
                }
            });
        }

        this.IsDataLoaded = true;
        this.fatherComponent.BuildItemsSource();
    }
}
