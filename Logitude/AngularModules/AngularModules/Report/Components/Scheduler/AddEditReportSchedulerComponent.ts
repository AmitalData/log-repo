import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { TaskReportSchedulerItemClass } from '../../../Report/Components/Scheduler/TaskReportSchedulerComponent';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';
import { ReportsTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ReportSchedulerRecepients } from '../../../Infrastructure/DataContracts/SchedulerDetails';
@Component({
    
    templateUrl: './AddEditReportSchedulerComponent.html',
})

export class AddEditReportSchedulerComponent implements OnInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public ReportGroupList: ReportGroupList;
    public ReportList: ReportList;
    public ValidationErrorsList: string[] = [];
    IsPreviwReport: boolean = false;
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    private PageChild_RETASK: any = null;
    private PageChild_PRREP: any = null;
    private PageChild_OPEMA: any = null;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        this.CurrentSession.SessionEvent.subscribe($event => {
            if ($event == "RunReportEvent") {
                this.IsPreviwReport = true;
            }
        });
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
    }

    ngOnInit() {
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

    DataContext: TaskReportSchedulerItemClass;
    SetDataContext(DataContext: TaskReportSchedulerItemClass) {
        this.DataContext = DataContext;
        this.DataContext.EntityPM = DataContext.EntityPM;
    }

    SetWindowArgs(windowArgs) {
        this.ReportGroupList = windowArgs.ReportGroupList;
        this.ReportList = windowArgs.ReportList;
        this.LoadReportTemplate(windowArgs.ReportList);
        this.RunComponent();
    }

    ReportTemplates: any = [];
    LoadReportTemplate(reportList: ReportList) {
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id, "R").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;
            }
        });
    }

    RunComponent() {
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }
            else {
                var tabCode = "RETASK";
                this.SetSelectedItem(tabCode);
            }
        }
        else this.RunComponentTimer();
    }

    SetSelectedItem(tabCode: string) {
        this.SelectedTabCode = tabCode;
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    SavedRecepients: boolean = false;
    SelectionChanged() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
        if (myLocation != null) {

            switch (this.SelectedTabCode) {
                //Report Task
                case "RETASK": {
                    if (this.PageChild_RETASK == null) {
                        SessionLocator.DynamicLoader.Load('./Report/Components/Scheduler/AddEditReportTaskSchedulerComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_RETASK = cmpRef.instance;
                                this.PageChild_RETASK.SetDataContext({ DataContext: this.DataContext, parentComponent: this});
                            });
                    }
                    break;
                }
                //Preview Report
                case "PRREP": {
                    if (this.PageChild_PRREP == null) {
                        this.CurrentSession.StartBusyIndicator("Preview...");
                        SessionLocator.DynamicLoader.Load('./Report/Components/ReportsPreviewComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_PRREP = cmpRef.instance;
                                this.SetReportDetails();
                                this.CurrentSession.StopBusyIndicator();
                            });
                    }
                    break;
                }
                //Open Email
                case "OPEMA": {
                    if (this.PageChild_OPEMA == null) {
                        this.CurrentSession.StartBusyIndicator("Presend...");
                        SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_OPEMA = cmpRef.instance;
                                this.SetRecepientsDetails(false);
                                this.SavedRecepients = true;
                                this.CurrentSession.StopBusyIndicator();
                            });
                    }
                    break;
                }
            }
        }
    }

    SetReportDetails() {
        var reportTemplateId = this.PageChild_RETASK.GetReportTemplateId();
        var reportFilterItems = this.PageChild_RETASK.GetReportFilterItems();
        this.PageChild_PRREP.SetReportFilterItems(reportFilterItems);
        this.PageChild_PRREP.SetReportTemplate(reportTemplateId);
        this.PageChild_PRREP.ReportsPreview(this.ReportGroupList, this.ReportList, this.ReportTemplates);
      //  this.RunBuildStimulsoftTimer();
    }

    SetRecepientsDetails(isReloaded) {
        if (this.PageChild_PRREP.IsPartnersChanged("3")) {
            this.PageChild_OPEMA.CleanRecepientsLists();
        }
        this.PageChild_PRREP.PrepareContactList();
        var windowArgs: any = {};
        var recepients: ReportSchedulerRecepients = this.PageChild_RETASK.DataContext.SchedulerDetails.ReportDetails.Recepients;
        windowArgs.ToEmail = this.SavedRecepients ? "" : recepients.To;
        windowArgs.Cc = this.SavedRecepients ? "" : recepients.Cc;
        windowArgs.Bcc = this.SavedRecepients ? "" : recepients.Bcc;
        windowArgs.PartnersObslist = this.PageChild_PRREP.PartnersObslist;
        windowArgs.EntityId = this.ReportList.Id;
        windowArgs.OnCloseSendToContactsEvent = false;
        windowArgs.IsUserFromReport = this.PageChild_PRREP.PartnersObslist ? true : false;
        windowArgs.IsSchedulerReport = true;
        windowArgs.isReloaded = isReloaded;
        this.PageChild_OPEMA.SetWindowArgs(windowArgs);
    }

    private Retrie = 0;
    private RunBuildStimulsoftTimer() {
        this.Retrie++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retrie < 20) {
            this.timerToken = setTimeout(() => this.PageChild_PRREP.BuildStimulsoft(), 1);
        }
    }

    public SelectedTabLocation: number = 0; //0: Report Task, 1: Preview Report, 2: Open Email
    NextButtonClicked() {
        if (this.SelectedTabLocation == 0) {
            if (this.PageChild_RETASK.NextButtonClicked()) {
                this.SetSelectedItem("PRREP");
                this.SelectedTabLocation += 1;
            }
        }
        else if (this.SelectedTabLocation == 1) {
            if (this.PageChild_PRREP.ValidateSelectedFilters()) {
                if (this.PageChild_OPEMA != null) {
                    this.SetRecepientsDetails(true);
                }
                this.SetSelectedItem("OPEMA");
                this.SelectedTabLocation += 1;
            }
        }
    }

    SaveButtonClicked() {
        var reportFilterItems = this.PageChild_PRREP.GetReportFilterItems();
        var reportTemplateId = this.PageChild_PRREP.GetReportTemplateId();
        var recepients = this.GetAllRecepients();
        this.PageChild_RETASK.SaveButtonClicked(reportFilterItems, reportTemplateId, recepients);
    }

    GetAllRecepients() {
        var recepients: ReportSchedulerRecepients = new ReportSchedulerRecepients();
        recepients.To = this.PageChild_OPEMA?.ToEmailLists?.toString();
        recepients.Cc = this.PageChild_OPEMA?.CcEmailLists?.toString();
        recepients.Bcc = this.PageChild_OPEMA?.BccEmailLists?.toString();
        return recepients;
    }

    BackButtonClicked() {
        if (this.SelectedTabLocation == 1) {
            this.IsPreviwReport = false;
            //this.CurrentSession.ResizeCurrentWindow(900);
            this.SetSelectedItem("RETASK");
        }
        else if (this.SelectedTabLocation == 2) {
            if (this.ValidationErrorsList && this.ValidationErrorsList.length > 0) {
                this.IsPreviwReport = false;
                this.SetSelectedItem("RETASK");
                this.SelectedTabLocation -= 1;
            }
            else {
                this.SetSelectedItem("PRREP");
                this.PageChild_PRREP.IsPartnersChanged("2");
            }
        }
        this.SelectedTabLocation -= 1;
    }

    DisableFinishButton() {
        if ((this.PageChild_OPEMA && this.PageChild_OPEMA.ToEmailLists.length != 0)
            || this.DataContext.IsFTP)
            return false;
        return true;
    }

    CloseButtonClicked() {
        this.PageChild_RETASK.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
}
