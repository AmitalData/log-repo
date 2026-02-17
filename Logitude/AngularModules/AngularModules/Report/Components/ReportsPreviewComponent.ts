declare var window: any;
import {Component, AfterViewInit, ViewChild, ViewContainerRef, ComponentRef, ChangeDetectorRef}  from '@angular/core';
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

import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';

@Component({
    moduleId: './Report/Components/',
    selector: 'ReportsPreviewComponent',
    templateUrl: 'ReportsPreviewComponent.html',
    providers: [ReportService],
})

export class ReportsPreviewComponent implements AfterViewInit {
    public Title: string;
    public FilterControlName: string;
    public Report: ReportList;
    public ReportGroup: ReportGroupList;
    IsStopTimer: boolean = false;
    IsStopTimerWaitResult: boolean = false;
    IsStopTimerStartCheckResut: boolean = false;

    public DataContext = this;
    public ComponentId: string;
    public FiltersAreaId: string;
    public IsResourcesReady: boolean = false;
    public ComponentRef: ComponentRef<ReportsPreviewComponent>;
    stimulsoftArg: StimulsoftArg;
    ReportFliter: ReportFliter;
    heighthwindow: number;
    widthwindow: number;
    PartnersObslist: EntityPartner[];
    ReportsTemplateLists: ReportsTemplateList[];
    ReportFilterConmponent: any;
    FilterConrolHeight: number = null;
    @ViewChild('FiltersLocation', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    @ViewChild('CustomerChild', { read: ViewContainerRef }) customerViewContainerRef: ViewContainerRef;
    ReportsRunUsingWR: boolean = false;
    IsUsedReportsRunUsingWR: boolean = false;
    NextRunDate: Date = null;
    NextCheckDate: Date = null;

    IsBuildUpTimerWaitResult: boolean = false;
    IsBuildUpStimulSoftReportTimer: boolean = false;
    NumberOfRequests: number = 0;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _reportService: ReportService, private cd: ChangeDetectorRef) {
        var idIndex = this.CurrentSession.GetNewId("ReportsPreviewComponent");
        this.ComponentId = "ReportsPreview_" + idIndex;
        this.FiltersAreaId = "ReportFiltersArea_" + idIndex;

        if(ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    ReportsPreview(GroupList: ReportGroupList, ReportList: ReportList, reportTemplateLists: ReportsTemplateList[], reportsRunUsingWR: boolean) {
        this.Report = ReportList;
        this.ReportGroup = GroupList;
        this.ReportsTemplateLists = reportTemplateLists;
        this.Title = SessionLocator.LoggedUserPM.DontShowLocal ? ReportList.Name : ReportList.LocalName; 
        this.FilterControlName = ReportList.FilterControlName; 
        this.ReportsRunUsingWR = reportsRunUsingWR;
        this.RunComponent();

        //this._reportService.GetCheckIfReportsRunUsingWR().subscribe(res => {
        //    var pmResponse: ServiceResponse = res;
        //    if (!pmResponse.HasError) {
        //        this.ReportsRunUsingWR = pmResponse.Result;
        //    }
        //    this.RunComponent();
        //});


    
    }

    ngAfterViewInit() {
        this.BuildStimulsoft();
    }

    private Retries: number = 0;
    private timerToken: any;
    private isLoaderReady: boolean = false;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
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
            else {

                this.RunComponentTimer();

            }
        }

        else {
            if (this.viewContainerRef) {
                SessionLocator.DynamicLoader.Load(this.Report.FilterHtmlComponentUrl, this.viewContainerRef)
                    .then(cmpRef => {
                        this.ReportFilterConmponent = cmpRef.instance;
                        if (cmpRef.instance['InitializeComponent']) {
                            cmpRef.instance.InitializeComponent(this);
                        }

                        if (cmpRef.instance['RunReportEvent']) {
                            cmpRef.instance.RunReportEvent.subscribe(s => {
                                if (s) {
                                    this.GenerateReport(s, false);
                                }
                            });
                        }

                        this.isLoaderReady = true;
                        this.BuildStimulsoft();
                    });
            }

            else {
                this.RunComponentTimer();
            }
        }
    }
    
    private BuildStimulsoft() {
        if (this.isLoaderReady) {
            var Component = document.getElementById(this.ComponentId);
            var filtersArea = document.getElementById(this.FiltersAreaId);

            if (Component && filtersArea) {
                this.FilterConrolHeight = filtersArea.clientHeight;

                this.stimulsoftArg = new StimulsoftArg();
                this.stimulsoftArg.Tenant = SessionLocator.Tenant;
                this.stimulsoftArg.ReportsPreviewComponent = this;
                this.stimulsoftArg.TypePage = "Report";
                this.stimulsoftArg.ShowStimulHeader = true;
                this.stimulsoftArg.ShowStimulFooter = true;
                this.stimulsoftArg.IsShowExportPrinttoPDF = true;
                this.stimulsoftArg.IsShowExportMicrosoftExcel = true;
                this.stimulsoftArg.IsShowSendButton = true;
                this.stimulsoftArg.EditableFieldLists = null;

                this.stimulsoftArg.DefaultTemplateId = this.Report.DefaultTemplateId;
                this.stimulsoftArg.ReportsTemplateLists = this.ReportsTemplateLists;
                this.stimulsoftArg.ShowReportsTemlatesLists = true;
                this.stimulsoftArg.ReportFilterConmponent = this.ReportFilterConmponent;

                this.stimulsoftArg.TemplateDescription = this.Report.Name;
                if (this.ReportsTemplateLists && this.ReportsTemplateLists.filter(d => d.Id == this.Report.DefaultTemplateId)[0]) {
                    this.stimulsoftArg.TemplateDescription = this.ReportsTemplateLists.filter(d => d.Id == this.Report.DefaultTemplateId)[0].Description;
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

    private ComputeSize(clientWidth: number, clientHeight: number) {
        var width = clientWidth;
        var height = clientHeight;

        if (width < 1024) {
            width = 1024;
        }

        width = width - 20;
        height = height - 22 - 20 - this.FilterConrolHeight;

        this.stimulsoftArg.ScreenWidth = width;
        this.stimulsoftArg.ScreenHeight = height;

        if (this.stimulsoftArg && this.stimulsoftArg.StimulsoftViewerComponent) {
            this.stimulsoftArg.StimulsoftViewerComponent.SetScreenWidthAndHeight(this.stimulsoftArg.ScreenWidth, this.stimulsoftArg.ScreenHeight);
            this.cd.detectChanges();
        }
    }
    private isLoadingDate: boolean = false;
 



    GenerateReport(filter: ReportFliter, isloading: boolean) {
        this.ReportFliter = this.FillReportFilter(filter);
        if (AppTool.IsNullOrEmpty(this.ReportFliter.DefaultTemplateId)) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please select a template");
            return;
        }

        if (!this.isLoadingDate) {
            this.isLoadingDate = true;
            this.StartBusyIndicator("Generating...");
            if (this.ReportsRunUsingWR && !this.IsUsedReportsRunUsingWR) {
                this.IsStopTimerWaitResult = false;
                this.NextRunDate = DateTool.AddSecond(DateTool.GetCurrentDateTimeAsUtc(), 50);
                if (!this.IsBuildUpTimerWaitResult) {
                    this.StartWaitResultStimulTimer();
                    this.IsBuildUpTimerWaitResult = true;
                }
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
                   this.IsStopTimerWaitResult = true;
                   this.NextRunDate = null;
                    this.isLoadingDate = false;
                    this.SetReportData(myResponse);
                   this.StopBusyIndicator();
                });

        }
    }
    GenerateReportViewWorkerRole(filter: ReportFliter) {
        this.ReportFliter = this.FillReportFilter(filter);
        if (AppTool.IsNullOrEmpty(this.ReportFliter.DefaultTemplateId)) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please select a template");
            this.IsUsedReportsRunUsingWR = false;
            this.isLoadingDate = false;
            return;
        }

        if (!this.isLoadingDate) {

            this.isLoadingDate = true;
            this.NumberOfRequests += 1;
            this._reportService.GenerateReportMethod(this.ReportFliter).subscribe((myResponse: ServiceResponse) => {
                this.IsUsedReportsRunUsingWR = false;
                this.isLoadingDate = false;
                this.SetReportData(myResponse);
                this.StopBusyIndicator();
            });

        }
    }

    SetReportData(myResponse: ServiceResponse) {

            if (myResponse.HasError) {
                var messageWindow = new MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
            }

            else {
                var myResult = myResponse.Result;
                if (myResult) {

                    if (this.ReportFliter != null) {
                        this.stimulsoftArg.ReportFliter = this.ReportFliter;
                    }

                    this.stimulsoftArg.NumberOfPage = this.ReportFliter.NumberOfPage;
                    this.stimulsoftArg.PartnersObslist = this.PartnersObslist;
                    this.stimulsoftArg.EditableFieldLists = myResult;

                    if (this.stimulsoftArg && this.stimulsoftArg.StimulsoftViewerComponent) {
                        this.stimulsoftArg.StimulsoftViewerComponent.SetStimualData();
                    }

                    this.cd.detectChanges();
                }
            }

        }

    FillReportFilter(filter: ReportFliter) {

         if (this.stimulsoftArg) filter.DefaultTemplateId = this.stimulsoftArg.DefaultTemplateId;
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
       
        if (this.ReportsTemplateLists) {
            var reportTemplate: any = this.ReportsTemplateLists.filter(d => d.Id == filter.DefaultTemplateId)[0];
            if (reportTemplate) {
                filter.DefaultTemplateVsersion = reportTemplate.CurrentVersion;
                filter.ReportName = reportTemplate.Description;
            }
        }

        return filter ;

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
        var entityPartner: EntityPartner = new EntityPartner(partnerType, partnerId, false);
    
        this.PartnersObslist.push(entityPartner);

    }

    SetReportFilterConmponent(reportFilterConmponent) {
        this.ReportFilterConmponent = reportFilterConmponent;

    }

    UsingWorkerRoleToBuildStimulSoftReport(filter: ReportFliter) {

        filter.ReportsRunUsingWR = true;
        this._reportService.GenerateReportMethod(filter).subscribe((myResponse: ServiceResponse) => {
            this.isLoadingDate = false;
            if (!myResponse.HasError) {

                this.ReportFliter = myResponse.Result;
                this.IsStopTimer = false;
               
                this.IsStartCheckStimualReportIsBuilt = false;
                this.IsStopTimerStartCheckResut = false;
                if (!this.IsBuildUpStimulSoftReportTimer) {
                    this.NextCheckDate = DateTool.AddSecond(DateTool.GetCurrentDateTimeAsUtc(), 50);
                    this.StartStartCheckStimulReportReadyTimer();
                    this.IsBuildUpStimulSoftReportTimer = true;
                }
           
            } else {
                this.IsUsedReportsRunUsingWR = false;
                this.StopBusyIndicator();
                if (myResponse.HasError && myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                  
                }
            }



        });

    }


    //Stimul Soft Report Timer

    initializeStimulSoftReportTimer() {
        return Observable.interval(2000).timeInterval();
    }
    private BuildStimulSoftReportsub: any = null;
    IsStartCheckStimualReportIsBuilt: boolean = false;
    StartStimulSoftReportTimer() {
        this.IsStartCheckStimualReportIsBuilt = false;
        this.BuildStimulSoftReportsub = this.initializeStimulSoftReportTimer().subscribe(res => {
            if (!this.IsStopTimer) {
                if (!this.IsStartCheckStimualReportIsBuilt) {
                    this.IsStartCheckStimualReportIsBuilt = true;
                    this._reportService.GetCheckIfStimualReportIsBuilt(this.ReportFliter.ReportKey, SessionLocator.Tenant).subscribe(res => {
                        var pmResponse: ServiceResponse = res;
                        this.IsStartCheckStimualReportIsBuilt = false;
                        if (!this.IsStopTimer) {
                            if (!pmResponse.HasError) {
                                var result: ReportBuildResult = pmResponse.Result;
                                if (result) {
                                    if (result.HasError) {
                                        this.IsStopTimer = true;
                                        var messageWindow = new MessageWindow();
                                        messageWindow.Show(result.ExceptionMessage);
                                        this.StopBusyIndicator();
                                        this.IsUsedReportsRunUsingWR = false;
                                    }
                                    else if (result.StatusCode == "D") {
                                        this.IsStopTimer = true;
                                        this.ReportFliter.ProcessType = "ReportsRunUsingWR";
                                        this.GenerateReportViewWorkerRole(this.ReportFliter);
                                    }

                                }

                            }

                            else {
                                this.IsStopTimer = true;
                                this.IsUsedReportsRunUsingWR = false;
                                this.StopBusyIndicator();
                                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                    var messageWindow = new MessageWindow();
                                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                                }
                            }
                        }


                    });
                }
            }


        });

    }


     //Wait Result Stimul Timer
    initializeWaitResultStimulTimer() {
        return Observable.interval(100).timeInterval();
    }
    private WaitResultStimulsub: any = null;
    StartWaitResultStimulTimer() {
        this.WaitResultStimulsub = this.initializeWaitResultStimulTimer().subscribe(res => {
            if (!this.IsStopTimerWaitResult && this.NextRunDate) {
                var dateNow: Date = DateTool.GetCurrentDateTimeAsUtc();
                if (dateNow >= this.NextRunDate ) {
                    this.IsStopTimerWaitResult = true;
                    this.NextRunDate = null;
                    this.IsUsedReportsRunUsingWR = true;
                    this.isLoadingDate = true;
                    this.StartBusyIndicator("Report generating is taking longer than expected. Please wait", 400);
                    this.UsingWorkerRoleToBuildStimulSoftReport(this.ReportFliter);

                }
            }
        });
    }

    
    //StartCheckStimulReportReady
    initializeStartCheckStimulReportReadyTimer() {
        return Observable.interval(100).timeInterval();
    }
    private WaitStartCheckStimulReportReadysub: any = null;
    StartStartCheckStimulReportReadyTimer() {
        this.WaitStartCheckStimulReportReadysub = this.initializeStartCheckStimulReportReadyTimer().subscribe(res => {
            if (!this.IsStopTimerStartCheckResut && this.NextCheckDate) {
                var dateNow: Date = DateTool.GetCurrentDateTimeAsUtc();
                if (dateNow >= this.NextCheckDate) {
                    this.IsStopTimerStartCheckResut = true;
                    this.NextCheckDate = null;
                    this.StartStimulSoftReportTimer();
                }
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

        this.ShowBusyIndicator = false;
    }
}
