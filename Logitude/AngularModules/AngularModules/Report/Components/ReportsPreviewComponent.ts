declare var window: any;
import {Component, AfterViewInit, ViewChild, ViewContainerRef, ComponentRef, ChangeDetectorRef, Output, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../Components/Filters/ReportFliter';
import {ReportService} from '../../Common/Services/ExtendedLists/ReportService';
import {StimulsoftViewerComponent} from '../../Infrastructure/Components/StimulsoftComponent/StimulsoftViewerComponent';
import {StimulsoftArg} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/StimulsoftArg';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ReportList} from '../EntityLists/ReportList';
import {ReportGroupList} from '../EntityLists/ReportGroupList';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {EntityPartner} from '../../Infrastructure/DataContracts/EntityPartner';
import {ReportsTemplateList} from '../../Common/EntityLists/ReportsTemplateList';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {ReportBuildResult} from '../DataContracts/ReportBuildResult';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';
import { ReportsTemplateListExtendedService } from '../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { QueryFilterItem } from './Filters/QueryFilterItem';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'ReportsPreviewComponent',
    templateUrl: 'ReportsPreviewComponent.html',
    providers: [ReportService],
})

export class ReportsPreviewComponent implements AfterViewInit {
    public Title: string;
    public FilterControlName: string;
    public Report: ReportList;
    public ReportGroup: ReportGroupList;

    IsRunReportSucceeded: boolean = false;
    IsRunReportFailed: boolean = false;
    public DataContext = this;
    public ComponentId: string;
    public FiltersAreaId: string;
    public IsResourcesReady: boolean = false;
    public ComponentRef: ComponentRef<ReportsPreviewComponent>;
    public IsSchedulerReport: boolean = false;
    DefaultReportTemplateId: string;
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    StimulsoftArg: StimulsoftArg;
    ReportFliter: ReportFliter;
    heighthwindow: number;
    widthwindow: number;
    PartnersObslist: EntityPartner[];
    ReportsTemplateLists: ReportsTemplateList[];
    ReportFilterConmponent: any;
    FilterConrolHeight: number = null;
    @ViewChild('FiltersLocation', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    @ViewChild('CustomerChild', { read: ViewContainerRef, static: false }) customerViewContainerRef: ViewContainerRef;
    ReportsRunUsingWR: boolean = false;
    IsUsedReportsRunUsingWR: boolean = false;

    NumberOfRequests: number = 0;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;

  IsHaveRunReportViewWorkerRoleToggleFeature: boolean = true;
    constructor(public _reportService: ReportService, private cd: ChangeDetectorRef) {
        var idIndex = this.CurrentSession.GetNewId("ReportsPreviewComponent");
        this.ComponentId = "ReportsPreview_" + idIndex;
        this.FiltersAreaId = "ReportFiltersArea_" + idIndex;
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        //ReportRunViewWorkerRole
        //var featureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "RRW" && d.TenantNumber == SessionLocator.Tenant)[0];
        //if (featureToggle) {
        //    this.IsHaveRunReportViewWorkerRoleToggleFeature = true;//
        //}

    }

    ReportsPreview(GroupList: ReportGroupList, ReportList: ReportList, reportTemplateLists: ReportsTemplateList[]) {
        this.Report = ReportList;
        this.ReportGroup = GroupList;
        this.ReportsTemplateLists = reportTemplateLists;
        this.Title = SessionLocator.LoggedUserPM.DontShowLocal ? ReportList.Name : ReportList.LocalName;
        this.FilterControlName = ReportList.FilterControlName;
        this.ReportsRunUsingWR = true;
        this.RunComponent();
    }


    ngAfterViewInit() {
        if (!this.IsSchedulerReport) {
            this.BuildStimulsoft();
        }
    }

    QueryFilterItems: Array<QueryFilterItem>;
    SetReportFilterItems(reportFilterItems: Array<QueryFilterItem>) {
      this.IsSchedulerReport = true;
        if (reportFilterItems && reportFilterItems.length!=0) {
            this.QueryFilterItems = reportFilterItems;
        }
    }

    GetReportFilterItems() {
        var reportFilterItems: Array<QueryFilterItem> = this.ReportFilterConmponent.GetQueryFilterItems();
        return reportFilterItems;
    }

    IsPartnersChanged(SelectedTab) {
        return this.ReportFilterConmponent.IsPartnersChanged(SelectedTab);
    }

    GetReportTemplateId() {
        const reportTemplateId: string = this.StimulsoftArg.DefaultTemplateId;
        return reportTemplateId;
    }

    SetReportTemplate(reportTemplateId: string) {
        if (reportTemplateId) {
            this.DefaultReportTemplateId = reportTemplateId;
        }
    }


    private Retries: number = 0;
    private timerToken: any;
    private isLoaderReady: boolean = false;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }


    private RunComponent() {
        if (this.Report.Code == "CUPA") {
            if (this.customerViewContainerRef) {
                SessionLocator.DynamicLoader.Load('./Report/Components/FilterReportComponent/CustomerPotentialActualFilterComponent', this.customerViewContainerRef)
                    .then(cmpRef => {
                    });
            }
            else this.RunComponentTimer();
        }

        else {
            if (this.viewContainerRef) this.LoadReportFilterComponent();
            else {
                this.RunComponentTimer();
            }
        }
    }

    ValidateSelectedFilters() {
        return this.ReportFilterConmponent.ValidateSelectedFilters();
    }

    PrepareContactList() {
        this.CleanPartnersObslist();
        this.ReportFilterConmponent.PrepareContactList();
    }

    LoadReportFilterComponent() {

        SessionLocator.DynamicLoader.Load(this.Report.FilterHtmlComponentUrl, this.viewContainerRef)
            .then(cmpRef => {
                this.ReportFilterConmponent = cmpRef.instance;
                if (this.IsSchedulerReport) {
                  this.ReportFilterConmponent.SetQueryFilterItems(this.QueryFilterItems);
                  this.ReportFilterConmponent.SetRunReportTitle();
                }

                if (this.ReportFilterConmponent['InitializeComponent']) {
                    this.ReportFilterConmponent.InitializeComponent(this);
                }

                if (this.ReportFilterConmponent['RunReportEvent']) {
                    this.ReportFilterConmponent.RunReportEvent.subscribe(s => {
                        if (s) {
                            if (this.IsSchedulerReport) {
                                //this.CurrentSession.ResizeCurrentWindow(1050);
                            }
                            this.GenerateReport(s, false);
                        }
                    });
                }

                this.isLoaderReady = true;
                this.BuildStimulsoft();
            });
    }

    public BuildStimulsoft() {
        if (this.isLoaderReady) {
            var Component = document.getElementById(this.ComponentId);
            var filtersArea = document.getElementById(this.FiltersAreaId);

            if (Component && filtersArea) {
                this.StimulsoftArg = new StimulsoftArg();
                if (this.IsSchedulerReport) {
                    this.StimulsoftArg.IsSchedulerReport = true;
                }
                this.FilterConrolHeight = filtersArea.clientHeight;
                this.StimulsoftArg.Tenant = SessionLocator.Tenant;
                this.StimulsoftArg.ReportsPreviewComponent = this;
                this.StimulsoftArg.TypePage = "Report";
                this.StimulsoftArg.ShowStimulHeader = true;
                this.StimulsoftArg.ShowStimulFooter = true;
                this.StimulsoftArg.IsShowExportPrinttoPDF = true;
                this.StimulsoftArg.IsShowExportMicrosoftExcel = true;
                this.StimulsoftArg.IsShowSendButton = true;
                this.StimulsoftArg.BuildStimulReportResult = null;

                if (this.DefaultReportTemplateId) {
                    this.Report.DefaultTemplateId = this.DefaultReportTemplateId;
                }
                this.StimulsoftArg.DefaultTemplateId = this.Report.DefaultTemplateId;
                this.StimulsoftArg.ReportsTemplateLists = this.ReportsTemplateLists;
                this.StimulsoftArg.ShowReportsTemlatesLists = true;
                this.StimulsoftArg.ReportFilterConmponent = this.ReportFilterConmponent;

                this.StimulsoftArg.TemplateDescription = this.Report.Name;
                if (this.ReportsTemplateLists && this.ReportsTemplateLists.filter(d => d.Id == this.Report.DefaultTemplateId)[0]) {
                    this.StimulsoftArg.TemplateDescription = this.ReportsTemplateLists.filter(d => d.Id == this.Report.DefaultTemplateId)[0].Description;
                }

                this.ComputeSize(Component.clientWidth, Component.clientHeight);

                window.onresize = (e) => {
                    this.ComputeSize(Component.clientWidth, Component.clientHeight);
                };

                this.IsResourcesReady = true;
                this.cd.detectChanges();
            }
        }
    }

    public ComputeSize(clientWidth: number, clientHeight: number) {
        var width = clientWidth;
        var height = clientHeight;

        if (width < 1024) {
            width = 1024;
        }

        if (this.IsSchedulerReport) {
            height += 30;
        }

        width = width - 20;
        height = height - 22 - 20 - this.FilterConrolHeight;

        this.StimulsoftArg.ScreenWidth = width;
        this.StimulsoftArg.ScreenHeight = height;

        if (this.StimulsoftArg && this.StimulsoftArg.StimulsoftViewerComponent) {
            this.StimulsoftArg.StimulsoftViewerComponent.SetScreenWidthAndHeight(this.StimulsoftArg.ScreenWidth, this.StimulsoftArg.ScreenHeight);
            this.cd.detectChanges();
        }
    }

    ValiditySelectedTemplate() {

        if (AppTool.IsNullOrEmpty(this.ReportFliter.DefaultTemplateId)) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please select a template");
            this.IsUsedReportsRunUsingWR = false;
            return;
        }
    }



    GenerateReport(filter: ReportFliter, isloading: boolean) {

        if (!this.ShowBusyIndicator) {
            this.ShowBusyIndicator = true;
            this.ReportFliter = this.FillReportFilter(filter);
            if (!this.IsHaveRunReportViewWorkerRoleToggleFeature || (this.IsHaveRunReportViewWorkerRoleToggleFeature && this.ReportFliter.ProcessType != "GenerateReport")) {

                this.IsRunReportSucceeded = false;
                this.IsRunReportFailed = false;

                this.ValiditySelectedTemplate();

                this.StartBusyIndicator("Generating...");

                if (this.ReportsRunUsingWR && !this.IsHaveRunReportViewWorkerRoleToggleFeature) {
                    this.StartTimerWaitingFirststimulReportBuild();
                }

                this.NumberOfRequests += 1;
                this.ReportFliter.NumberOfRequests = this.NumberOfRequests;
                this._reportService.GenerateReportMethod(this.ReportFliter).subscribe((myResponse: ServiceResponse) => {

                    var myResult = myResponse.Result;
                    if (myResult && !myResponse.HasError) {
                        if (myResult[0] && myResult[0].NumberOfRequest != this.NumberOfRequests) {
                            return;
                        }
                    }

                    if (this.IsUsedReportsRunUsingWR) return;
                    this.SetReportData(myResponse);
                    this.StopBusyIndicator();
                });


            }

            else {
                this.StartBuildStimulReportViaWorkerRole(this.ReportFliter, true);
            }
        }
    }

    GenerateReportViewWorkerRole(filter: ReportFliter) {
        this.IsRunReportSucceeded = false;
        this.IsRunReportFailed = false;

        if (!this.Report.DisablePreview) {
            this.ReportFliter = this.FillReportFilter(filter);
            this.ValiditySelectedTemplate();
            this.NumberOfRequests += 1;
            this._reportService.GenerateReportMethod(this.ReportFliter).subscribe((myResponse: ServiceResponse) => {
                this.IsUsedReportsRunUsingWR = false;
                this.SetReportData(myResponse);
                this.StopBusyIndicator();
            });

        } else {

            this.StopBusyIndicator();
            this.IsUsedReportsRunUsingWR = false;
            this.IsRunReportSucceeded = true;
            this.SetReportData();

        }
    }





    SetReportData(myResponse: ServiceResponse = null) {
        var isSetStimualData = false;
        this.IsRunReportFailed = false;
        this.IsRunReportSucceeded = false;
        this.StimulsoftArg.NumberOfPage = this.ReportFliter.NumberOfPage;
        this.StimulsoftArg.PartnersObslist = this.PartnersObslist;
        this.StimulsoftArg.ReportFliter = this.ReportFliter;
        this.StimulsoftArg.ReportKey = this.ReportFliter.ReportKey;

        if (myResponse) {
            if (myResponse.HasError) {
                var messageWindow = new MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
                this.IsRunReportFailed = true;

            } else {
                this.StimulsoftArg.BuildStimulReportResult = myResponse.Result;
                isSetStimualData = true;
            }
        } else isSetStimualData = true;


        if (isSetStimualData) {
            this.IsRunReportSucceeded = true;

            if (this.StimulsoftArg && this.StimulsoftArg.StimulsoftViewerComponent) {
                this.StimulsoftArg.StimulsoftViewerComponent.SetStimualData();
            }

        }
        this.cd.detectChanges();



    }

    FillReportFilter(filter: ReportFliter) {

        if (this.StimulsoftArg) filter.DefaultTemplateId = this.StimulsoftArg.DefaultTemplateId;
        else filter.DefaultTemplateId = this.Report.DefaultTemplateId;

        filter.ReportsRunUsingWR = false;
        filter.Tenant = SessionLocator.Tenant;
        filter.ReportName = this.Title;
        filter.FilterControlName = this.FilterControlName;
        filter.ReportDocumentId = this.Report.ReportDocumentId;
        filter.ReportCode = this.Report.Code;
        filter.DefaultTemplateVsersion = 1;
        filter.UserId = SessionLocator.LoggedUserId;
        filter.ReportId = this.Report.Id;
        filter.DisablePreview = this.Report.DisablePreview;

        if (this.ReportsTemplateLists) {
            var reportTemplate: any = this.ReportsTemplateLists.filter(d => d.Id == filter.DefaultTemplateId)[0];
            if (reportTemplate) {
                filter.DefaultTemplateVsersion = reportTemplate.CurrentVersion;
                filter.ReportName = reportTemplate.Description;
            }
        }

        return filter;

    }

    SetFilterCotrolHeight(filterConrolHeight: number) {
        // Ayman: no need for this anymore
        //this.FilterConrolHeight = filterConrolHeight;
        // this.ResizeWindow(window.innerHeight, window.innerWidth);
    }

    BackButtonClicked() {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }
    }

    CleanPartnersObslist() {

        this.PartnersObslist = [];
    }

    AddPartner(partnerType: string, partnerId: string) {
        var partnerExist: boolean = false;
        this.PartnersObslist.forEach(partner => {
            if (!AppTool.IsNullOrEmpty(partner))
                if (partner.PartnerType == partnerType) {
                    partnerExist = true;
                    partner.PartnerId += ',' + partnerId;
                }
        });
        if (!partnerExist) {
            var entityPartner: EntityPartner = new EntityPartner(partnerType, partnerId, false);

            this.PartnersObslist.push(entityPartner);
        }
    }

    SetReportFilterConmponent(reportFilterConmponent) {
        this.ReportFilterConmponent = reportFilterConmponent;

    }


    StartBuildStimulReportViaWorkerRole(filter: ReportFliter, isUsedWorkerRoleAlalways = false) {
        filter.ReportsRunUsingWR = this.IsUsedReportsRunUsingWR = true;

        this.StartBusyIndicator("Generating...");


        this._reportService.GenerateReportMethod(filter).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.ReportFliter = myResponse.Result;
                this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer();
            } else {

                filter.ReportsRunUsingWR = this.IsUsedReportsRunUsingWR = false;
                this.StopBusyIndicator();

                if (myResponse.HasError && myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
            }
        });

    }


    //Stimul Soft Report Timer

  initializeStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer() {
    return interval(2000).pipe(timeInterval());
    }



    private StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub: any = null;
    IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer: boolean = false;
    StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer() {
        if (this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
            this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
        }

        this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = true;
        this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub = this.initializeStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer().subscribe(respose => {


            if ((this.CurrentSession && this.CurrentSession.isDestroingSession) || !this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
                this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
                this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;
                return;
            }



            if (this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {

                this._reportService.GetCheckIfStimulSoftReportIsBliud(this.ReportFliter.ReportKey, SessionLocator.Tenant).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;
                    if (this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
                        if (pmResponse.HasError || (pmResponse.Result && pmResponse.Result.HasError) || (pmResponse.Result && pmResponse.Result.StatusCode == "D")) {
                            this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
                            this.IsUsedReportsRunUsingWR = false;
                            this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;

                        }

                        if (!pmResponse.HasError) {
                            var result: ReportBuildResult = pmResponse.Result;
                            if (result) {
                                if (result.HasError) {
                                    this.StopBusyIndicator();
                                    var messageWindow = new MessageWindow();
                                    messageWindow.Show(result.ExceptionMessage);
                                }

                                else if (result.StatusCode == "P") {
                                    this.StartBusyIndicator("Report is in progress");
                                }
                                else if (result.StatusCode == "D") {
                                    this.ReportFliter.ProcessType = "ReportsRunUsingWR";
                                    this.GenerateReportViewWorkerRole(this.ReportFliter);
                                }

                            }

                        }
                        else {
                            this.StopBusyIndicator();
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                var messageWindow = new MessageWindow();
                                messageWindow.Show(pmResponse.ErrorsArray[0]);
                            }
                        }

                    }

                });

            }
        });

    }



    //Wait Result Stimul Timer
    IsStartTimerWaitingFirstStimulReportBuildRunning: boolean = false;
    initializeStartTimerWaitingFirstStimulReportBuild() {
      return interval(50000).pipe(timeInterval());

    }
    private StartTimerWaitingFirstStimulReportBuildsub: any = null;
    StartTimerWaitingFirststimulReportBuild() {

        if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
            this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
        }


        this.IsStartTimerWaitingFirstStimulReportBuildRunning = true;
        this.StartTimerWaitingFirstStimulReportBuildsub = this.initializeStartTimerWaitingFirstStimulReportBuild().subscribe((res:any) => {

            if (this.CurrentSession && this.CurrentSession.isDestroingSession) {
                this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
                return;
            }



            if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
                this.StartBuildStimulReportViaWorkerRole(this.ReportFliter);
                this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
            }
        });
    }



    //Wait Result Stimul Timer
    IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning: boolean = false;
    initializeStartTimerChangeBusyIndicatorMessageAfter50Sec() {
      return interval(50000).pipe(timeInterval());

    }
    private StartTimerChangeBusyIndicatorMessageAfter50Secsub: any = null;
    StartTimerChangeBusyIndicatorMessageAfter50Sec() {

        if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
            this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
        }


        this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = true;
        this.StartTimerChangeBusyIndicatorMessageAfter50Secsub = this.initializeStartTimerChangeBusyIndicatorMessageAfter50Sec().subscribe((res:any) => {

            if (this.CurrentSession && this.CurrentSession.isDestroingSession) {
                this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
                this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
                return;
            }

            if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
                this.StartBusyIndicator("Generating...");
                this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
                this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
            }
        });
    }

    ShowBusyIndicator: boolean = false;
    BusyIndicatorText: string = "";
    WidthBusyIndicator: number;
    StartBusyIndicator(message: string = "Generating...", width: number = 200) {

        this.ShowBusyIndicator = true;
        this.BusyIndicatorText = message;
        this.WidthBusyIndicator = width;

    }


    StopBusyIndicator() {



        if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
            this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
            this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
        }
        if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
            this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
            this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
        }


        if (this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer) {
            this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
            this.IsStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer = false;
        }


        this.ShowBusyIndicator = false;

    }
}
