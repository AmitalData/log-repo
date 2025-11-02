import {Component, OnInit, ElementRef, Output, EventEmitter}  from '@angular/core';
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
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'ReportComponent.html',
})

export class ReportComponent {
    public ItemsSource: ReportsGrpupClass[] = [];
    public ItemsSourceTemp: ReportsGrpupClass[] = [];
    public IsAvailableForScheduling: boolean = false;
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    IsViewReport: boolean = false;
    showLocal: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.IsAvailableForScheduling = FeatureLocator.HasFeaturePermession("Report", "ReportsScheduler");
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

        groupService.getReportGroupLists().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.groupList = myResponse.Result;
                this.groupList = this.groupList.sort((a, b) => { return a.OrderNumber - b.OrderNumber });

                this.groupList.forEach(item => {
                    reportService.GetReportListsByGroupId(item.Id).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var myResult: ReportList[] = myResponse.Result;
                            
                            myResult.forEach((item) => {
                                if (item.Code == "AREX") {
                                    if (SessionLocator.Tenant == 1212 || FeatureLocator.IsPackage_DVMT()) {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }

                                else if (item.Code == "DSCA") {
                                    if (SessionLocator.Tenant != 1212 || FeatureLocator.IsPackage_DVMT()) {
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

                                else if (item.Code == "UNER") {
                                    var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "URT")[0];
                                    if (FeatureToggle) {
                                        this.reportList.push(item);
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
                                else if (item.Code == "SHRR") {
                                    if (SessionLocator.Tenant == 2095 || SessionLocator.Tenant == 2052 || SessionLocator.TenantManagementJS.PackageCode == "DVMT") {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }

                                else if (item.Code == "FLBM") {
                                    if (SessionLocator.Tenant == 2095 || SessionLocator.Tenant == 2052 || FeatureLocator.IsPackage_DVMT()) {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }


                                else if (item.Code == "RCRF") {
                                    if (SessionLocator.Tenant == 1326 || FeatureLocator.IsPackage_DVMT()) {
                                        if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            this.reportList.push(item);
                                        }
                                    }
                                }
                                else if (item.Code == "COO") {
                                    if (item.FeatureCode && FeatureLocator.HasFeaturePermession("Customs.Declaration", "Declaration.Tab.DigitalCertificateOfOrigin")) {
                                        this.reportList.push(item);
                                    }
                                }
                                else {
                                    if (item.Code == "COOC" && SessionLocator.Tenant != 0) {
                                        return;
                                    }

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
            var myItem: ReportsGrpupClass = new ReportsGrpupClass(item, this, this.showLocal);
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
        if (ReportList.Code == "LRBE") {
            this.onReportSchedulerClick(GroupList, ReportList,true)
            return;
        }

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

    //IsLoadSettingWorkerRoleRuning: boolean = false;
    IsLoadReportsTemplateListRuning: boolean = false;
    
    ViewReport(groupList: ReportGroupList, reportList: ReportList) {

        if (!this.IsViewReport) {
            this.IsViewReport = true;

            //this.IsLoadSettingWorkerRoleRuning = true;
            this.IsLoadReportsTemplateListRuning = true;
            this.LoadReportTemplate(groupList, reportList);
           // this.LoadReportsRunUsingWR(groupList, reportList);
        }
    }

    ReportTemplates: any[] = [];
    LoadReportTemplate(groupList: ReportGroupList, reportList: ReportList) {
     
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id).subscribe((myResponse: ServiceResponse) => {
            
            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;
             
            }
            
            this.IsLoadReportsTemplateListRuning = false;
            reportList.DefaultExcelTemplateId = this.ReportTemplates.filter(d => d.TemplateType == "E" && d.IsDefault && d.UseStimul)[0]?.Id ?? "";
            reportList.DefaultExcelNoStimId = this.ReportTemplates.filter(d => d.TemplateType == "E" && d.IsDefault && !d.UseStimul)[0]?.Id ?? "";
            reportList.DefaultTemplateId = this.ReportTemplates.filter(d => d.TemplateType == "R" && d.IsDefault)[0]?.Id ?? "";
            this.LoadComplete(groupList, reportList);

        });
    }

    //ReportsRunUsingWR: boolean = false;
    //LoadReportsRunUsingWR(groupList: ReportGroupList, reportList: ReportList) {
       
    //    var myService = new ReportService();
    //      myService.GetCheckIfReportsRunUsingWR().subscribe((res:any) => {
    //        var pmResponse: ServiceResponse = res;
    //        if (!pmResponse.HasError) {
    //            this.ReportsRunUsingWR = pmResponse.Result;
    //        }

    //        this.IsLoadSettingWorkerRoleRuning = false;
    //        this.LoadComplete(groupList, reportList);
    //    });
    //}
    
    LoadComplete(groupList: ReportGroupList, reportList: ReportList) {
        if (!this.IsLoadReportsTemplateListRuning) {
            if (true) {
                SessionLocator.DynamicLoader.Load("./Report/Components/ReportsPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.ReportsPreview(groupList, reportList, this.ReportTemplates);
                    });
            }
            else {

            }
            this.IsViewReport = false;
        }
    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {         
        this.mySearchText = text;
        this.FillTempItemsSource();
    }

    onReportSchedulerClick(groupList: ReportGroupList, reportList: ReportList,isQueryReport: boolean = false) {
        this.entityResourceService.getEntityResourceByTableName("TasksScheduler", 0).subscribe((response:any) => {

            var windowArgs: any = {};
            windowArgs.ReportGroupList = groupList;
            windowArgs.ReportList = reportList;
            windowArgs.IsQueryReport = isQueryReport;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1200;
            logWindow.Height = 1000;

            logWindow.Title = reportList.Name + " Scheduler";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Report/Components/Scheduler/MainReportSchedulerComponent');
        });
    }
}
export class ReportsGrpupClass {
    public Name: string;
    public ItemsSource: ReportList[] = [];
    public GroupList: ReportGroupList;
    public IsDataLoaded: boolean = false;
    constructor(private list: ReportGroupList, private fatherComponent: ReportComponent, showLocal:boolean) {
        this.GroupList = list;
        if (!showLocal) {
            this.Name = list.EnglishName;
        }
        else {
            this.Name = list.LocalName;
        }
     

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
