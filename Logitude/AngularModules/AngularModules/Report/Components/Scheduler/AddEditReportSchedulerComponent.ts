import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { TaskReportSchedulerItemClass } from '../../../Report/Components/Scheduler/TaskReportSchedulerComponent';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';
import { ReportsTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ReportSchedulerRecepients, ReportSchedulerDetails } from '../../../Infrastructure/DataContracts/SchedulerDetails';
import { SchedulerExtendedPMService } from 'Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService';
import { AppTool } from 'Infrastructure/Tools';
import { BIReportPMService } from '../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { DWSubQueryPMService } from '../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { QueryFilterItem } from '../Filters/QueryFilterItem';
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
    public IsPowerBIReport: boolean;
    public IsBIReport: boolean;
    public IsQueryReport: boolean;
    public IsCustomerDebNotification: boolean;
    public GLAccountId: string;
    public IsNew: boolean = true;
    public TasksSchedulerId: string;
    public TaskSchedulerIdMaintenance: string;
    public MaintenanceSchedulerDetails;
    public OldReportSchedulerDetails;
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
        this.IsQueryReport = windowArgs.IsQueryReport;
        this.IsCustomerDebNotification = windowArgs.IsCustomerDebNotification;
        this.GLAccountId = windowArgs.GLAccountId;
        this.TaskSchedulerIdMaintenance = windowArgs.TaskSchedulerIdMaintenance;

        if (!windowArgs.TasksSchedulerId) {
            this.BIReportEntity = windowArgs.BIReportEntity;
        }
        this.IsPowerBIReport = windowArgs.IsPowerBIReport;    
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
        this.TasksSchedulerId = tasksSchedulerId;
        this.schedulerExtendedPMService
            .GetSchedulerDetailsById(tasksSchedulerId)
            .subscribe((myResult: ServiceResponse) => {
                var myResponse: ServiceResponse = myResult;
                this.LoadData(myResponse);
            });
    }


    LoadData(myResponse) {
        this.OldReportSchedulerDetails = myResponse?.Result?.ReportDetails;

        if (this.IsBIReport && !this.IsNew) {
            this.LoadBIReport(myResponse?.Result?.ReportDetails);
        }
        else if (!this.IsBIReport) {
            this.LoadReportAndMessageTemplates(myResponse?.Result?.ReportDetails?.ReportTemplateType);
        }
        else {
            this.RunComponent();
            this.CurrentSession.StopBusyIndicator();
        }
    }

    ReportTemplates: any = [];
    LoadReportAndMessageTemplates(templateType) {
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(this.ReportList.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) return;
            this.TemplateType = "";
            this.ReportTemplates = myResponse.Result;
            this.RunComponent();
            this.CurrentSession.StopBusyIndicator();
        });
    }

    LoadBIReport(ReportDetails) {
        if (!ReportDetails) return;
        this.bIReportPMService.get(ReportDetails.BIReportEntityId).subscribe((serviceResponse: ServiceResponse) => {
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
                    } else if (!this.IsBIReport) {
                        this.PageChild_PRREP.SetResultType(this.DataContext.EntityPM.ResultType);
                        this.PageChild_PRREP.Refresh();
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
                    TasksSchedulerId: this.TasksSchedulerId,
                    IsScheduler: true,
                    IsNewScheduler: this.IsNew,
                    SavedFilterItemsData: this.PageChild_RETASK?.EntityPM?.SchedulerDetailsData?.ReportDetails?.DWQueryFilterData,
                    DocumentTypeTemplateId: this.PageChild_RETASK?.EntityPM?.SchedulerDetailsData?.ReportDetails?.DocumentTypeTemplateId,
                    ParentComponent: this,
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

    async SetReportDetails() {
        let reportTemplateId = this.PageChild_RETASK.GetReportTemplateId();
        let reportFilterItems = this.PageChild_RETASK.GetReportFilterItems();
        let messageTemplateId = this.PageChild_RETASK.GetMessageTemplateId();
        if (this.IsCustomerDebNotification)
        {
           var queryFilterItems = new Array<QueryFilterItem>();
            var queryFilterItem = new QueryFilterItem();

            if (!AppTool.IsNullOrEmpty(this.GLAccountId))
            {

                 if (AppTool.IsNullOrEmpty(this.TasksSchedulerId) && !AppTool.IsNullOrEmpty(this.TaskSchedulerIdMaintenance))
               {
                       await new Promise<void>((resolve, reject) => {
                    this.schedulerExtendedPMService
                        .GetSchedulerDetailsById(this.TaskSchedulerIdMaintenance)
                        .subscribe({
                            next: (myResult: ServiceResponse) => {
                                var myResponse: ServiceResponse = myResult;
                                this.MaintenanceSchedulerDetails = myResponse?.Result?.ReportDetails;
                                queryFilterItems = this.MaintenanceSchedulerDetails.ReportFilterItems;
                                resolve(); 
                            },
                            error: (err) => reject(err)
                        });
                });           
              }
               queryFilterItem.FieldName = "GLAccountId";
               queryFilterItem.FieldValue = this.GLAccountId;
               queryFilterItem.Operator = "Equals";
               queryFilterItems.push(queryFilterItem);
            }
            else
            {
               queryFilterItem.FieldName = "IsDisableGlaccountId";
               queryFilterItem.FieldValue = true;
               queryFilterItem.Operator = "Equals";
               queryFilterItem.IsCustom = true;                       
            }
            if(reportFilterItems == null)
            {
               queryFilterItems.push(queryFilterItem);
               this.PageChild_PRREP.SetReportFilterItems(queryFilterItems);
            }
            else
            {
              reportFilterItems.push(queryFilterItem);
            }
         }

         this.PageChild_PRREP.SetReportFilterItems(reportFilterItems);      
        this.PageChild_PRREP.SetReportTemplate(reportTemplateId);
        this.PageChild_PRREP.SetReportTemplateType(this.TemplateType);
        this.PageChild_PRREP.SetMessageTemplateId(messageTemplateId);
        this.PageChild_PRREP.SetResultType(this.DataContext.EntityPM.ResultType);
        this.PageChild_PRREP.SetReportEntityId(this.DataContext.EntityPM.Id);

        this.PageChild_PRREP.ReportsPreview(this.ReportGroupList, this.ReportList, this.ReportTemplates);
      //  this.RunBuildStimulsoftTimer();
    }

    SetRecepientsDetails(isReloaded) {
        this.SetReportRecepientsDetails(isReloaded);
    }

    SetReportRecepientsDetails(isReloaded) {        
        const isPartnersChanged = !this.IsQueryReport && !this.IsPowerBIReport? this.PageChild_PRREP.IsPartnersChanged("3"): false; 
        if(!this.IsQueryReport && !this.IsPowerBIReport) 
         this.PageChild_PRREP.PrepareContactList();        
        var windowArgs: any = {};
        var recepients: ReportSchedulerRecepients = this.PageChild_RETASK.DataContext.SchedulerDetails.ReportDetails.Recepients;
        windowArgs.ToEmail = this.SavedRecepients ? "" : recepients.To;
        windowArgs.Cc = this.SavedRecepients ? "" : recepients.Cc;
        windowArgs.Bcc = this.SavedRecepients ? "" : recepients.Bcc;
        windowArgs.PartnersObslist = !this.IsQueryReport && !this.IsPowerBIReport ? this.PageChild_PRREP?.PartnersObslist : [];
        windowArgs.EntityId = this.IsBIReport ? this.BIReportEntity['Id'] : this.ReportList.Id;
        windowArgs.OnCloseSendToContactsEvent = false;
        windowArgs.IsUserFromReport = this.PageChild_PRREP?.PartnersObslist || this.IsQueryReport || this.IsPowerBIReport? true : false;
        windowArgs.IsSchedulerReport = true;
        windowArgs.isReloaded = isReloaded;
        windowArgs.ClearRecepients = isPartnersChanged;
        windowArgs.ByCardCode = this.IsBIReport ? true : false;
        this.PageChild_OPEMA.SelectedPartnerItem = isPartnersChanged ? null : this.PageChild_OPEMA.SelectedPartnerItem;
        this.PageChild_OPEMA.SetWindowArgs(windowArgs);
        if (isPartnersChanged) this.PageChild_OPEMA.CleanRecepientsLists();
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
        if(this.IsQueryReport || this.IsPowerBIReport) {
            if (this.PageChild_RETASK.NextButtonClicked()) {
                if (this.PageChild_OPEMA != null) {
                    this.SetRecepientsDetails(true);
                }
                this.SelectedTabLocation = 1;
                this.ChangeSelectedLocation("OPEMA");
            }
        }
        else {
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
        if (this.PageChild_RETASK && !this.PageChild_RETASK.NextButtonClicked()) {
            this.SetSelectedItem("RETASK");
            this.SelectedTabLocation = 0;
            return;
        }
        if (!this.IsBIReport && this.PageChild_PRREP && !this.PageChild_PRREP.ValidateSelectedFilters()) {
            this.SetSelectedItem("PRREP");
            this.SelectedTabLocation = 1;
            return;
        }

        if (!this.IsCustomerDebNotification && !this.DataContext.IsFTP && !this.IsNew && AppTool.IsNullOrEmpty(this.OldReportSchedulerDetails?.Recepients?.To) && !this.PageChild_OPEMA) {
            this.PageChild_RETASK?.parentComponent?.ValidationErrorsList?.push("Please add at least one contact");
            return;
        }

        //if (this.IsBIReport && this.IsNew) { //Don't remove this //Maybe will back it
        //    this.SaveNewDWQueryData();
        //}
        //else if (this.IsBIReport) {
        if (this.IsBIReport) {
            this.SaveBIReportSchedulerDetails();
        }
        else if(this.IsQueryReport || this.IsPowerBIReport) {
            this.SaveQueryReportSchedulerDetails();
        }
        else {         
            this.SaveReportSchedulerDetails();
        }
    }



    ShowErrorWindow(error) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(error);
    }

    //SaveNewDWQueryData() {  //Don't remove this //Maybe will back it
    //    this.dWSubQueryPMService.insertDWQueryData(this.PageChild_PRREP?.BIReportXMLData?.DWQueryData).subscribe((myResult: any) => {
    //        if (!myResult.HasError) {
    //            this.SaveNewBIReport(myResult);
    //        }
    //        else if (myResult.ErrorsArray && myResult.ErrorsArray.length > 0) {
    //            this.ShowErrorWindow(myResult.ErrorsArray[0]);
    //        }
    //    });
    //}

    //private SaveNewBIReport(myResult: any) {
    //    this.PageChild_PRREP.EntityPM['Name'] += 'Scheduler';
    //    this.PageChild_PRREP.EntityPM['IsScheduler'] = true;
    //    this.PageChild_PRREP.EntityPM['DWQueryId'] = myResult.Result.DWQueryId;
    //    this.bIReportPMService.insert(this.PageChild_PRREP.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
    //        if (!serviceResponse.HasError) {
    //            this.SubmitSavingNewBIReport(serviceResponse, myResult);
    //        }
    //        else if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) {
    //            this.ShowErrorWindow(serviceResponse.ErrorsArray[0]);
    //        }
    //    });
    //}

    //private SubmitSavingNewBIReport(serviceResponse: ServiceResponse, myResult: any) {
    //    this.PageChild_PRREP.EntityPM['Id'] = serviceResponse.Result.Id;
    //    this.PageChild_PRREP.SaveBIReport();
    //    this.SaveBIReportSchedulerDetails(serviceResponse.Result.Id, myResult.Result.DWQueryId, false);
    //}

    private SaveReportSchedulerDetails() {
        const reportSchedulerDetails: ReportSchedulerDetails = {
            ReportFilterItems: this.GetReportFilterItems(),
            ReportTemplateId: this.PageChild_PRREP ? this.PageChild_PRREP.GetReportTemplateId() : this.OldReportSchedulerDetails?.ReportTemplateId,
            ReportTemplateType: this.PageChild_PRREP ? this.PageChild_PRREP.GetReportTemplateType() : this.OldReportSchedulerDetails?.ReportTemplateType,
            Recepients: this.GetAllRecepients(),
            MainCustomerFieldName: this.PageChild_PRREP ? this.PageChild_PRREP.GetReportFilterMainCustomerFieldName() : this.OldReportSchedulerDetails?.MainCustomerFieldName,
            CreatedByUserId: SessionLocator.LoggedUserId,
            BIReportEntityId: null,
            DWQueryId: null,
            DWQueryFilterData: null,
            DocumentTypeTemplateId: this.PageChild_PRREP ? this.GetDocumentTemplateMessageId() : this.OldReportSchedulerDetails.DocumentTypeTemplateId,
            DocumentTypeTemplateIds: this.PageChild_PRREP ? this.PageChild_PRREP.MessageTemplateIds : this.OldReportSchedulerDetails.DocumentTypeTemplateIds,
            MessageTemplateId: this.PageChild_PRREP ? this.PageChild_PRREP.GetMessageTemplateId() : this.OldReportSchedulerDetails?.MessageTemplateId,
            ProcedureName:null,
        };
        this.PageChild_RETASK.SaveButtonClicked(reportSchedulerDetails);
    }
    private SaveQueryReportSchedulerDetails() {
        const reportSchedulerDetails: ReportSchedulerDetails = {
            ReportFilterItems: [],//this.GetReportFilterItems(),
            ReportTemplateId: this.PageChild_PRREP ? this.PageChild_PRREP.GetReportTemplateId() : this.OldReportSchedulerDetails?.ReportTemplateId,
            ReportTemplateType: this.PageChild_PRREP ? this.PageChild_PRREP.GetReportTemplateType() : this.OldReportSchedulerDetails?.ReportTemplateType,
            Recepients: this.GetAllRecepients(),
            MainCustomerFieldName: this.PageChild_PRREP ? this.PageChild_PRREP.GetReportFilterMainCustomerFieldName() : this.OldReportSchedulerDetails?.MainCustomerFieldName,
            CreatedByUserId: SessionLocator.LoggedUserId,
            BIReportEntityId: null,
            DWQueryId: null,
            DWQueryFilterData: null,
            DocumentTypeTemplateId: null,
            DocumentTypeTemplateIds: null,
            MessageTemplateId: null,
            ProcedureName: this.IsQueryReport? this.PageChild_RETASK.SelectedReport.Code: null,

        };
        this.PageChild_RETASK.SaveButtonClicked(reportSchedulerDetails);
    }

    private GetReportFilterItems() {
        if (this.PageChild_PRREP) {
            return this.PageChild_PRREP.GetReportFilterItems()
        }

        if (!this.OldReportSchedulerDetails) {
            return [];
        }

        return this.GetOldReportFilterItems();
    }

    private GetOldReportFilterItems() {
        this.OldReportSchedulerDetails.ReportFilterItems.forEach(reportFilterItem => {
            this.SetNullObjectFieldToNullValue(reportFilterItem);
        });

        return this.OldReportSchedulerDetails.ReportFilterItems;
    }

    private SetNullObjectFieldToNullValue(reportFilterItem) {
        if (AppTool.IsNil(reportFilterItem.FieldValue))
            reportFilterItem.FieldValue = null;
        if (AppTool.IsNil(reportFilterItem.FieldValue2))
            reportFilterItem.FieldValue2 = null;
        if (AppTool.IsNil(reportFilterItem.FieldValue3))
            reportFilterItem.FieldValue3 = null;
    }

    public SaveBIReportSchedulerDetails() {
        const reportSchedulerDetails: ReportSchedulerDetails = {
            ReportFilterItems: [],
            ReportTemplateId: null,
            ReportTemplateType: null,
            Recepients: this.GetAllRecepients(),
            MainCustomerFieldName: null,
            CreatedByUserId: SessionLocator.LoggedUserId,
            BIReportEntityId: this.BIReportEntity['Id'],
            DWQueryId: this.BIReportEntity['DWQueryId'],
            DocumentTypeTemplateId: this.PageChild_PRREP ? this.GetDocumentTemplateMessageId() : this.OldReportSchedulerDetails.DocumentTypeTemplateId,
            DocumentTypeTemplateIds: this.PageChild_PRREP ? this.PageChild_PRREP.DocumentTypeTemplateIds : this.OldReportSchedulerDetails.DocumentTypeTemplateIds,
            DWQueryFilterData: this.PageChild_PRREP ? this.GetNewSelectedFilters() : this.GetOriginalSelectedFilters(),
            MessageTemplateId: null,
            ProcedureName:null,
        };
        this.PageChild_RETASK.SaveButtonClicked(reportSchedulerDetails);
    }

    GetDocumentTemplateMessageId() {
        if (!this.PageChild_PRREP) return "";
        if (!this.PageChild_PRREP.DocumentTypeTemplateSelected) return "";
       return this.PageChild_PRREP.DocumentTypeTemplateSelected.Id
    }

    private GetNewSelectedFilters(): any {
        if (this.PageChild_PRREP.SelectedFiltersDataSource[0] && this.PageChild_PRREP.DWQueryData) {
            this.PageChild_PRREP.SelectedFiltersDataSource[0].AndOr = this.PageChild_PRREP.DWQueryData.Filters?.AndOr;
        }
        return this.PageChild_PRREP.SelectedFiltersDataSource[0];
    }

    GetOriginalSelectedFilters() {
        let filterTextValue = this.PageChild_RETASK?.EntityPM?.SchedulerDetailsData?.ReportDetails?.DWQueryFilterData?.TextValue;
        if (this.PageChild_RETASK?.EntityPM?.SchedulerDetailsData?.ReportDetails?.DWQueryFilterData?.TextValue) {
            this.PageChild_RETASK.EntityPM.SchedulerDetailsData.ReportDetails.DWQueryFilterData.TextValue = AppTool.IsNil(filterTextValue) ? null : filterTextValue;
        }
        this.PageChild_RETASK?.EntityPM?.SchedulerDetailsData?.ReportDetails?.DWQueryFilterData?.FilterItems?.forEach((filter) => {
            filter.TextValue = AppTool.IsNil(filter.TextValue) ? null : filter.TextValue;
        });

        return this.PageChild_RETASK?.EntityPM?.SchedulerDetailsData?.ReportDetails?.DWQueryFilterData;
    }

    GetAllRecepients() {
        var recepients: ReportSchedulerRecepients = new ReportSchedulerRecepients();
        recepients.To = this.PageChild_OPEMA ? this.PageChild_OPEMA?.ToEmailLists?.toString() : this.OldReportSchedulerDetails?.Recepients?.To;
        recepients.Cc = this.PageChild_OPEMA ? this.PageChild_OPEMA?.CcEmailLists?.toString() : this.OldReportSchedulerDetails?.Recepients?.Cc;
        recepients.Bcc = this.PageChild_OPEMA ? this.PageChild_OPEMA?.BccEmailLists?.toString() : this.OldReportSchedulerDetails?.Recepients?.Bcc;
        return recepients;
    }

    BackButtonClicked() {
        if(!this.IsQueryReport && !this.IsPowerBIReport) {
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
        else{
            this.reOpenReportTaskTab();
            this.SelectedTabLocation = 0;
        }
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
        if (this.IsCustomerDebNotification && AppTool.IsNullOrEmpty(this.GLAccountId) && this.PageChild_PRREP) return false;
        if (this.PageChild_OPEMA && this.PageChild_OPEMA.ToEmailLists.length == 0) return true;
        if (!this.IsBIReport && this.PageChild_PRREP && !this.IsNew && !this.PageChild_PRREP.ValidateSelectedFilters()) return true;
        if((this.IsQueryReport || this.IsPowerBIReport) && this.IsNew && (this.PageChild_OPEMA)) return false;
        if (this.IsNew && this.PageChild_PRREP && this.DataContext.IsFTP) return false;
        if (this.IsNew && (!this.PageChild_PRREP || !this.PageChild_OPEMA)) return true;

        return false;
    }
    DisableNextButton() {
        if (this.IsCustomerDebNotification && AppTool.IsNullOrEmpty(this.GLAccountId) && this.PageChild_PRREP) return true;

       return this.SelectedTabLocation == 2 || (this.DataContext.IsFTP && this.SelectedTabLocation == 1);
    }
    CloseButtonClicked() {
        var confirmMsg = "Are you sure you want to leave this page without saving the report?";

        var confirmWindow = new ConfirmWindow();
        confirmWindow.ZIndex = 1000;
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (!confirmWindow.Yes) return;

            this.PageChild_RETASK.RejectChanges();
            if(this.IsCustomerDebNotification)
               this.CurrentSession.CloseCurrentWindowData(this.TasksSchedulerId);
           
               this.CurrentSession.CloseCurrentWindow();
        });
    }
}
