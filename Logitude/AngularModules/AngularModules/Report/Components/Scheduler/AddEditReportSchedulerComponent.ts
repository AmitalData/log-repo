import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { TaskReportSchedulerItemClass } from '../../../Report/Components/Scheduler/TaskReportSchedulerComponent';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';
import { ReportsTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ReportSchedulerRecepients, ReportSchedulerDetails, SchedulerDetails } from '../../../Infrastructure/DataContracts/SchedulerDetails';
import { SchedulerExtendedPMService } from 'Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import { AppTool } from 'Infrastructure/Tools';
import { BIReportPMService } from '../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { DWSubQueryPMService } from '../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
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
    bIReportPMService: BIReportPMService;
    dWSubQueryPMService: DWSubQueryPMService;
    private PageChild_RETASK: any = null;
    private PageChild_PRREP: any = null;
    private PageChild_OPEMA: any = null;
    public BIReportEntity: any;
    public IsBIReport: boolean;
    public IsNew: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    schedulerExtendedPMService: SchedulerExtendedPMService;
    TemplateType: any;
    
    constructor() {
        this.schedulerExtendedPMService = new SchedulerExtendedPMService();
        this.bIReportPMService = new BIReportPMService();
        this.dWSubQueryPMService = new DWSubQueryPMService();
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
        if (!windowArgs.TasksSchedulerId) {
            this.BIReportEntity = windowArgs.BIReportEntity;
        }
        if (windowArgs.BIReportEntity) {
            this.IsBIReport = true;
        }
        this.LoadReportSchedulerDetailsData(windowArgs.TasksSchedulerId);
    }

    LoadReportSchedulerDetailsData(tasksSchedulerId) {
        this.CurrentSession.StartBusyIndicator('Loading...');
        if (tasksSchedulerId) {
            this.IsNew = false;
        }
        this.schedulerExtendedPMService
            .GetSchedulerDetailsById(tasksSchedulerId)
            .subscribe((myResult: ServiceResponse) => {
                var myResponse: ServiceResponse = myResult;
                this.LoadData(myResponse);
            });
    }

    LoadData(myResponse) {
        if (this.IsBIReport && !this.IsNew) {
            this.LoadBIReport(myResponse?.Result?.ReportDetails?.BIReportEntityId);
        }
        else if (!this.IsBIReport) {
            this.LoadReportTemplate(myResponse?.Result?.ReportDetails?.ReportTemplateType);
        }
        else {
            this.RunComponent();
            this.CurrentSession.StopBusyIndicator();
        }
    }

    ReportTemplates: any = [];
    LoadReportTemplate(templateType) {
        this.TemplateType = AppTool.IsNullOrEmpty(templateType) ? "R" : templateType
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(this.ReportList.Id,this.TemplateType).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;
                this.RunComponent();
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    LoadBIReport(BIReportEntityId) {
        this.bIReportPMService.get(BIReportEntityId).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.BIReportEntity = serviceResponse.Result;
                this.RunComponent();
                this.CurrentSession.StopBusyIndicator();
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
                        this.OpenPreviewReport(myLocation);
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

    OpenPreviewReport(myLocation: LocationDirective) {
        if (!this.IsBIReport) {
            this.LoadReportsPreviewComponent(myLocation);
        }
        else {
            this.LoadBIReportPreviewComponent(myLocation);
        }
    }

    LoadBIReportPreviewComponent(myLocation: LocationDirective) {
        this.CurrentSession.StartBusyIndicator("Preview...");
        SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", myLocation.viewContainerRef)
            .then(cmpRef => {
                this.PageChild_PRREP = cmpRef.instance;
                this.PageChild_PRREP.Run({
                    DWQueryId: this.BIReportEntity['DWQueryId'],
                    Name: this.BIReportEntity['Name'] + 'Scheduler',
                    ObjectTableName: 'BIReport',
                    EntityId: this.BIReportEntity['Id'],
                    IsScheduler: true,
                    IsNewScheduler: this.IsNew
                });
                this.CurrentSession.StopBusyIndicator();
            });
    }

    LoadReportsPreviewComponent(myLocation: LocationDirective) {
        this.CurrentSession.StartBusyIndicator("Preview...");
        SessionLocator.DynamicLoader.Load('./Report/Components/ReportsPreviewComponent', myLocation.viewContainerRef)
            .then(cmpRef => {
                this.PageChild_PRREP = cmpRef.instance;
                this.SetReportDetails();
                this.CurrentSession.StopBusyIndicator();
            });
    }

    SetReportDetails() {
        var reportTemplateId = this.PageChild_RETASK.GetReportTemplateId();
        var reportFilterItems = this.PageChild_RETASK.GetReportFilterItems();
        this.PageChild_PRREP.SetReportFilterItems(reportFilterItems);
        this.PageChild_PRREP.SetReportTemplate(reportTemplateId);
        this.PageChild_PRREP.SetReportTemplateType(this.TemplateType);
        this.PageChild_PRREP.ReportsPreview(this.ReportGroupList, this.ReportList, this.ReportTemplates);
      //  this.RunBuildStimulsoftTimer();
    }

    SetRecepientsDetails(isReloaded) {
        this.SetReportRecepientsDetails(isReloaded);
    }

    SetReportRecepientsDetails(isReloaded) {
        const isPartnersChanged = this.PageChild_PRREP.IsPartnersChanged("3");
        if (isPartnersChanged) this.PageChild_OPEMA.CleanRecepientsLists();
        this.PageChild_PRREP.PrepareContactList();
        var windowArgs: any = {};
        var recepients: ReportSchedulerRecepients = this.PageChild_RETASK.DataContext.SchedulerDetails.ReportDetails.Recepients;
        windowArgs.ToEmail = this.SavedRecepients ? "" : recepients.To;
        windowArgs.Cc = this.SavedRecepients ? "" : recepients.Cc;
        windowArgs.Bcc = this.SavedRecepients ? "" : recepients.Bcc;
        windowArgs.PartnersObslist = this.PageChild_PRREP.PartnersObslist;
        windowArgs.EntityId = this.IsBIReport ? this.BIReportEntity['Id'] : this.ReportList.Id;
        windowArgs.OnCloseSendToContactsEvent = false;
        windowArgs.IsUserFromReport = this.PageChild_PRREP.PartnersObslist ? true : false;
        windowArgs.IsSchedulerReport = true;
        windowArgs.isReloaded = isReloaded;
        windowArgs.ClearRecepients = isPartnersChanged;
        windowArgs.ByCardCode = this.IsBIReport ? true : false;
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
                this.ChangeSelectedLocation("PRREP");
            }
        }
        else if (this.SelectedTabLocation == 1) {
            if (this.IsBIReport) {
                this.SelectedEmailPageForBIReport();
            }
            else if (this.PageChild_PRREP.ValidateSelectedFilters()) {
                if (this.PageChild_OPEMA != null) {
                    this.SetRecepientsDetails(true);
                }
                this.ChangeSelectedLocation("OPEMA");
            }
        }
    }

    SelectedEmailPageForBIReport() {
        if (this.PageChild_OPEMA != null) {
            this.SetRecepientsDetails(true);
        }
        this.ChangeSelectedLocation("OPEMA");
    }

    ChangeSelectedLocation(locationCode: string) {
        this.SetSelectedItem(locationCode);
        this.SelectedTabLocation += 1;
    }

    SaveButtonClicked() {
        if (this.IsBIReport && this.IsNew) {
            this.SaveNewDWQueryData();
        }
        else if (this.IsBIReport) {
            this.SaveBIReportSchedulerDetails(this.BIReportEntity['Id'], this.BIReportEntity['DWQueryId'], true);
        }
        else {
            this.SaveReportSchedulerDetails();
        }
    }



    ShowErrorWindow(error) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(error);
    }

    SaveNewDWQueryData() {
        this.dWSubQueryPMService.insertDWQueryData(this.PageChild_PRREP?.BIReportXMLData?.DWQueryData).subscribe((myResult: any) => {
            if (!myResult.HasError) {
                this.SaveNewBIReport(myResult);
            }
            else if (myResult.ErrorsArray && myResult.ErrorsArray.length > 0) {
                this.ShowErrorWindow(myResult.ErrorsArray[0]);
            }
        });
    }

    private SaveNewBIReport(myResult: any) {
        this.PageChild_PRREP.EntityPM['Name'] += 'Scheduler';
        this.PageChild_PRREP.EntityPM['IsScheduler'] = true;
        this.PageChild_PRREP.EntityPM['DWQueryId'] = myResult.Result.DWQueryId;
        this.bIReportPMService.insert(this.PageChild_PRREP.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.SubmitSavingNewBIReport(serviceResponse, myResult);
            }
            else if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) {
                this.ShowErrorWindow(serviceResponse.ErrorsArray[0]);
            }
        });
    }

    private SubmitSavingNewBIReport(serviceResponse: ServiceResponse, myResult: any) {
        this.PageChild_PRREP.EntityPM['Id'] = serviceResponse.Result.Id;
        this.PageChild_PRREP.SaveBIReport();
        this.SaveBIReportSchedulerDetails(serviceResponse.Result.Id, myResult.Result.DWQueryId, false);
    }

    private SaveReportSchedulerDetails() {
        const reportSchedulerDetails: ReportSchedulerDetails = {
            ReportFilterItems: this.PageChild_PRREP.GetReportFilterItems(),
            ReportTemplateId: this.PageChild_PRREP.GetReportTemplateId(),
            ReportTemplateType: this.PageChild_PRREP.GetReportTemplateType(),
            Recepients: this.GetAllRecepients(),
            MainCustomerFieldName: this.PageChild_PRREP.GetReportFilterMainCustomerFieldName(),
            CreatedByUserId: SessionLocator.LoggedUserId,
            BIReportEntityId: null,
            DWQueryId: null
        };
        this.PageChild_RETASK.SaveButtonClicked(reportSchedulerDetails);
    }

    private SaveBIReportSchedulerDetails(bIReportEntityId, dWQueryId, isUpdate) {
        const reportSchedulerDetails: ReportSchedulerDetails = {
            ReportFilterItems: [],
            ReportTemplateId: null,
            ReportTemplateType: null,
            Recepients: this.GetAllRecepients(),
            MainCustomerFieldName: null,
            CreatedByUserId: SessionLocator.LoggedUserId,
            BIReportEntityId: bIReportEntityId,
            DWQueryId: dWQueryId
        };
        this.PageChild_RETASK.SaveButtonClicked(reportSchedulerDetails);

        if (isUpdate) {
            this.PageChild_PRREP.SaveBIReportScheduler();
        }
    }

    GetAllRecepients() {
        var recepients: ReportSchedulerRecepients = new ReportSchedulerRecepients();
        recepients.To = this.PageChild_OPEMA?.ToEmailLists?.toString();
        recepients.Cc = this.PageChild_OPEMA?.CcEmailLists?.toString();
        recepients.Bcc = this.PageChild_OPEMA?.BccEmailLists?.toString();
        return recepients;
    }

    BackButtonClicked() {
        switch (this.SelectedTabLocation) {
            case 1:
                this.reOpenReportTaskTab();
                break;
            case 2:
                this.backFromRecepientsTab();
                break;
        }
        this.SelectedTabLocation -= 1;
    }

    private backFromRecepientsTab() {
        if (this.itHaveAnError()) {
            this.reOpenReportTaskTab();
            this.SelectedTabLocation -= 1;
        }
        else {
            this.reOpenReportPreviewTab();
        }
    }

    private itHaveAnError() {
        return this.ValidationErrorsList && this.ValidationErrorsList.length > 0;
    }

    private reOpenReportPreviewTab() {
        this.SetSelectedItem("PRREP");
        this.PageChild_PRREP.IsPartnersChanged("2");
    }

    private reOpenReportTaskTab() {
        this.IsPreviwReport = false;
        this.SetSelectedItem("RETASK");
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
