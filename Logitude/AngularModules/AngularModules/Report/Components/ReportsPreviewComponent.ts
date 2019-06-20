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


    public DataContext = this;
    public ComponentId: string;
    public FiltersAreaId: string;
    public IsResourcesReady: boolean = false;
    public ComponentRef: ComponentRef<ReportsPreviewComponent>;
    StimulsoftArg: StimulsoftArg;
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
    NumberOfRequests: number = 0;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;

    IsHaveToggleFeature: boolean = false;
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

                this.StimulsoftArg = new StimulsoftArg();
                this.StimulsoftArg.Tenant = SessionLocator.Tenant;
                this.StimulsoftArg.ReportsPreviewComponent = this;
                this.StimulsoftArg.TypePage = "Report";
                this.StimulsoftArg.ShowStimulHeader = true;
                this.StimulsoftArg.ShowStimulFooter = true;
                this.StimulsoftArg.IsShowExportPrinttoPDF = true;
                this.StimulsoftArg.IsShowExportMicrosoftExcel = true;
                this.StimulsoftArg.IsShowSendButton = true;
                this.StimulsoftArg.EditableFieldLists = null;

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

    private ComputeSize(clientWidth: number, clientHeight: number) {
        var width = clientWidth;
        var height = clientHeight;

        if (width < 1024) {
            width = 1024;
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

IsRunReportSucceeded:boolean = false ;
IsRunReportFailed:boolean = false ;

    GenerateReport(filter: ReportFliter, isloading: boolean) {
       this.ReportFliter = this.FillReportFilter(filter);

       if (!this.IsHaveToggleFeature || (this.IsHaveToggleFeature && this.ReportFliter.ProcessType != "GenerateReport")) {

           this.IsRunReportSucceeded = false;
           this.IsRunReportFailed = false;

           this.ValiditySelectedTemplate();

               this.StartBusyIndicator("Generating...");
               if (this.ReportsRunUsingWR && !this.IsUsedReportsRunUsingWR && !this.IsHaveToggleFeature) {
                   this.NextRunDate = DateTool.AddSecond(DateTool.GetCurrentDateTimeAsUtc(), 50);
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
                   this.NextRunDate = null;
                  


                   this.SetReportData(myResponse);
                   this.StopBusyIndicator();
               });

           
       }

       else {
           this.StartBuildStimulReportViaWorkerRole(this.ReportFliter);
       }
}

    GenerateReportViewWorkerRole(filter: ReportFliter) {

        this.IsRunReportSucceeded= false ;
        this.IsRunReportFailed = false ;
        this.ReportFliter = this.FillReportFilter(filter);

         this.ValiditySelectedTemplate();


        if (AppTool.IsNullOrEmpty(this.ReportFliter.DefaultTemplateId)) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please select a template");
            
            return;
        }

            this.NumberOfRequests += 1;
            this._reportService.GenerateReportMethod(this.ReportFliter).subscribe((myResponse: ServiceResponse) => {
                this.IsUsedReportsRunUsingWR = false;
                this.SetReportData(myResponse);
                this.StopBusyIndicator();
            });

        
    }





    SetReportData(myResponse: ServiceResponse) {

            if (myResponse.HasError) {
                var messageWindow = new MessageWindow();
                messageWindow.Show(myResponse.ErrorsArray[0]);
                this.IsRunReportFailed = true;
      
            }

            else {
              this.IsRunReportSucceeded = true;
                var myResult = myResponse.Result;
                if (myResult) {

                    if (this.ReportFliter != null) {
                        this.StimulsoftArg.ReportFliter = this.ReportFliter;
                    }

                    this.StimulsoftArg.NumberOfPage = this.ReportFliter.NumberOfPage;
                    this.StimulsoftArg.PartnersObslist = this.PartnersObslist;
                    this.StimulsoftArg.EditableFieldLists = myResult;

                    if (this.StimulsoftArg && this.StimulsoftArg.StimulsoftViewerComponent) {
                        this.StimulsoftArg.StimulsoftViewerComponent.SetStimualData();
                    }

                    this.cd.detectChanges();
                }
            }

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

    StartBuildStimulReportViaWorkerRole(filter: ReportFliter) {

        this.StartBusyIndicator("Report generating is taking longer than expected. Please wait", 400);

         filter.ReportsRunUsingWR = this.IsUsedReportsRunUsingWR = true;

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
        return Observable.interval(2000).timeInterval();
    }
    private StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub: any = null;

    StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer() {
        this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub = this.initializeStartCheckStimulSoftSoftReportBliudViaWorkerRoleTimer().subscribe(res => {
            var isStopStimulSoftReportsub = false;
            if (this.CurrentSession && this.CurrentSession.isDestroingSession) {
                this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
                isStopStimulSoftReportsub = true;
                return;
            }

            this._reportService.GetCheckIfStimulSoftReportIsBliud(this.ReportFliter.ReportKey, SessionLocator.Tenant).subscribe(res => {
                        var pmResponse: ServiceResponse = res;
                        if (!isStopStimulSoftReportsub) {
                            if (pmResponse.HasError || (pmResponse.Result && pmResponse.Result.HasError) || (pmResponse.Result && pmResponse.Result.StatusCode == "D")) {
                                this.StartCheckStimulSoftSoftReportBliudViaWorkerRoleTimersub.unsubscribe();
                                this.StopBusyIndicator();
                                this.IsUsedReportsRunUsingWR = false;

                            }
                            if (!pmResponse.HasError) {
                                var result: ReportBuildResult = pmResponse.Result;
                                if (result) {
                                    if (result.HasError) {
                                        var messageWindow = new MessageWindow();
                                        messageWindow.Show(result.ExceptionMessage);
                                    }
                                    else if (result.StatusCode == "D") {
                                        this.ReportFliter.ProcessType = "ReportsRunUsingWR";
                                        this.GenerateReportViewWorkerRole(this.ReportFliter);
                                    }

                                }

                            }
                            else {
                                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                    var messageWindow = new MessageWindow();
                                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                                }
                            }

                        }

                    });
                
            


        });

    }


     //Wait Result Stimul Timer
    initializeStartTimerWaitingFirstStimulReportBuild() {
        return Observable.interval(100).timeInterval();
    }

    private StartTimerWaitingFirstStimulReportBuildsub: any = null;
    StartTimerWaitingFirststimulReportBuild() {
        this.StartTimerWaitingFirstStimulReportBuildsub = this.initializeStartTimerWaitingFirstStimulReportBuild().subscribe(res => {
            if (this.CurrentSession && this.CurrentSession.isDestroingSession) {
                this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                return;
            }

            if (this.NextRunDate) {
                var dateNow: Date = DateTool.GetCurrentDateTimeAsUtc();
                if (dateNow >= this.NextRunDate ) {
                    this.NextRunDate = null;
                    this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                    this.StartBuildStimulReportViaWorkerRole(this.ReportFliter);
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
