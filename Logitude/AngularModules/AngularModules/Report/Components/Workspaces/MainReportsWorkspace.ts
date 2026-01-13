import { Component, ViewChildren, QueryList, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import * as pbi from 'powerbi-client';
import { models } from 'powerbi-client';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ReportGroupList } from 'Report/EntityLists/ReportGroupList';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ReportList } from '../../EntityLists/ReportList';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ReportListService } from 'Common/Services/StandardLists/ReportListService';

@Component({
    
    selector: 'MainReportsWorkspace',
    templateUrl: './MainReportsWorkspace.html',
    providers: [EntityResourceService],
})

export class MainReportsWorkspace implements OnInit {
    public IsMenuVisible: boolean = false;
    public IsBIItemVisible: boolean = false;
    public IsReportItemVisible: boolean = false;
    public IsResourcesReady: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private _reportListService: ReportListService;

    constructor(private _entityResourceService: EntityResourceService, private sanitizer: DomSanitizer) {
        this._reportListService = new ReportListService();
    }
    ngOnInit() {

        this._entityResourceService.getEntityResourceByTableName("BIReportFolder", 0).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("BIReport", 0).subscribe((response:any) => {
                this._entityResourceService.getEntityResourceByTableName("Report", 0).subscribe((response:any) => {
                    this.IsResourcesReady = true;

                    if (FeatureLocator.HasFeaturePermession("BIReport", "BIReport.Menu")) {
                        this.IsBIItemVisible = true;
                        this.IsMenuVisible = true;
                    }

                    if (FeatureLocator.HasFeaturePermession("Report", "Module")) {
                        this.IsReportItemVisible = true;
                    }


                    this.RunComponent();
                });
            });
        });
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private SetSelectedItem() {
        if (FeatureLocator.HasFeaturePermession("Report", "Module")) {
            this.SelectedItem = "Report";
        }
        else {
            this.SelectedItem = "BI";
        }
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    public PowerBiReports;
    public PowerBiReportUrl = null;
    public ActiveDirectoryTenantId;
    public SelectedPowerBIReport = null;
    private _reportService: ReportService = new ReportService();
    @ViewChild('reportContainer', { static: false })
    reportContainer!: ElementRef<HTMLDivElement>;

    GetPowerBiReports() {

        if (!this.PowerBiReports) {
            this._reportService.GetPowerBIReports().subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray?.[0] || "An unexpected error occurred");
                }
                else {
                    this.ActiveDirectoryTenantId = myResponse?.Result?.ActiveDirectoryTenantId;
                    this.PowerBiReports = myResponse?.Result?.Reports || [];

                    this.PowerBiReports.forEach(report => {                        
                        var filters = new ApiQueryFilters;
                        filters.GetAll = true;
                        filters.SortBy = "CreateDate";
                        filters.SortDirection = "Descending";
                        filters.addAdditionalFilter("Name", report.Name, null, null, "Equals", false, false, false, "string");

                        this._reportListService.getByFilters(filters).subscribe((myResult: ServiceResponse) => {
                            var myResponse: ServiceResponse = myResult;
                            if (!myResponse.HasError && myResponse.Result?.length > 0) {
                                report.Report = myResponse.Result[0];
                            }
                        });
                    });
                }
            });
        }
    }

    SelectPowerBiReport(report) {
        this.SelectedPowerBIReport = report;

        setTimeout(() => this.EmbedReport());
    }

    EmbedReport() {
        const embedConfig = {
            type: 'report',
            tokenType: models.TokenType.Embed,
            accessToken: this.SelectedPowerBIReport.EmbedToken,
            embedUrl: this.SelectedPowerBIReport.EmbedUrl,
            id: this.SelectedPowerBIReport.Id,
            settings: {
                panes: {
                    filters: { visible: false },
                    pageNavigation: { visible: true }
                }
            } as any
        };
        
        const powerbiService = new pbi.service.Service(
            pbi.factories.hpmFactory,
            pbi.factories.wpmpFactory,
            pbi.factories.routerFactory
        );

        powerbiService.embed(this.reportContainer.nativeElement, embedConfig);
    }

    onReportSchedulerClick(groupList: ReportGroupList, reportList: ReportList) {
        this._entityResourceService.getEntityResourceByTableName("TasksScheduler", 0).subscribe((response:any) => {

            var windowArgs: any = {};
            windowArgs.ReportGroupList = groupList;
            windowArgs.ReportList = reportList;
            windowArgs.IsPowerBIReport = true;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1200;
            logWindow.Height = 1000;

            logWindow.Title = reportList.Name + " Scheduler";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Report/Components/Scheduler/MainReportSchedulerComponent');
        });
    }

    private Page_BI: any = null;
    private Page_Report: any = null;

    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null || this.SelectedItem == "PowerBI") {

                    switch (this.SelectedItem) {

                        case "Report": {
                            if (this.Page_Report == null) {
                                this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe((response:any) => {
                                    SessionLocator.DynamicLoader.Load('./Report/Components/Workspaces/ReportComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_Report = cmpRef.instance;
                                            this.Page_Report.InitComponent();
                                        });
                                });
                            }
                            break;
                        }

                        case "BI": {
                            if (this.Page_BI == null) {
                                SessionLocator.DynamicLoader.Load('./Report/Components/Workspaces/BIFolderReportComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_BI = cmpRef.instance;
                                        this.Page_BI.InitComponent();
                                    });
                            }
                            break;
                        }

                        case "PowerBI":
                            this.SelectedPowerBIReport = null;
                            this.GetPowerBiReports();
                            break;
                    }
                }
            }
        }
    }
}
