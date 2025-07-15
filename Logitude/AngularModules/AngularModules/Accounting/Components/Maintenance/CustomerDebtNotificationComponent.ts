import { Component, OnDestroy, OnInit } from '@angular/core';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { CustomerDebtNotificationPM } from 'Accounting/EntityPMs/CustomerDebtNotificationPM';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { CustomerDebtNotificationExtendedPMService } from 'Accounting/Services/ExtendedPMs/CustomerDebtNotificationExtendedPMService';
import { CustomerDebtNotificationPMService } from 'Accounting/Services/StandardPMs/CustomerDebtNotificationPMService';
import { ReportGroupList } from 'Report/EntityLists/ReportGroupList';
import { ReportGroupService } from 'Common/Services/ExtendedLists/ReportGroupService';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';
import { AccountingEventManager } from 'Accounting/Utilities/AccountingEventManager';
import { TasksSchedulerExtendedService } from 'Infrastructure/Services/ExtendedPMs/TasksSchedulerExtendedService';
import { Subscription } from 'rxjs';
@Component({

    selector: 'CustomerDebtNotificationComponent',
    templateUrl: './CustomerDebtNotificationComponent.html',
    providers: [ServiceArgs]
})
export class CustomerDebtNotificationComponent extends BaseComponent implements OnInit, OnDestroy {
    public DataContext: any = this;
    public ObjectTableName: string = "CustomerDebtNotification";
    public CustomerDebtNotificationPM: CustomerDebtNotificationPM;
    public CustomerDebtNotificationMaintenance: CustomerDebtNotificationPM;

    public ValidationErrorsList: string[];
    public IsVisibile = false;
    private CurrentSession = SessionLocator.SelectedSession;
   
    private taskSchedulerSub: Subscription;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    ApiQueryFilters:ApiQueryFilters;
    IsFromMaintenance:boolean;
    GLAccountId:string;
    myService: CustomerDebtNotificationPMService = new CustomerDebtNotificationPMService();
    IsActiveEnum = IsActiveEnum;
    TypesDebtsEnum = TypesDebtsEnum;
    DebtLevelEnum = DebtLevelEnum;
    Percentage:string = '%';
    TotalAmount:string ='|X|';
    
    constructor() {
        super();
        this.Listen();

    }
  
    ngOnInit() {     
        this.entityResourceService.getEntityResourceByTableName("CustomerDebtNotification").subscribe((res: any) => {
            this.CurrentSession.StartBusyIndicator('Loading...');           
            this.ApiQueryFilters = new ApiQueryFilters();      
            this.GLAccountId = !AppTool.IsNullOrEmpty(this.CurrentSession?.CurrentEditComponent?.EntityPM?.Id) ? this.CurrentSession?.CurrentEditComponent?.EntityPM?.Id : null;
            this.LoadCustomerDebtNotification();
        
        });
    }
    ngOnDestroy(): void {
        this.taskSchedulerSub.unsubscribe(); 
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.IsFromMaintenance = args.IsFromMaintenance;
        }
    }
    
    Listen() {
        this.taskSchedulerSub = AccountingEventManager.TasksSchedulerId.subscribe((event: string) => {
            if (!AppTool.IsNullOrEmpty(event)) {
               this.TasksSchedulerId = event;
            }
        });
    }

   
    private LoadCustomerDebtNotification() {
        var myService: CustomerDebtNotificationExtendedPMService = new CustomerDebtNotificationExtendedPMService();
           myService.GetCustomerDebtNotificationByAccountId(this.GLAccountId).subscribe((response: ServiceResponse) => {
            this.CustomerDebtNotificationPM = response.Result as CustomerDebtNotificationPM;

           myService.GetCustomerDebtNotificationByAccountId(null).subscribe((res: ServiceResponse) => {
            this.CustomerDebtNotificationMaintenance = res.Result as CustomerDebtNotificationPM;

                const isMaintenanceNoActive =  this.CustomerDebtNotificationMaintenance?.InActive === IsActiveEnum.NotActive ||
                                               AppTool.IsNullOrEmpty(this.CustomerDebtNotificationMaintenance?.InActive);

               
            if (!this.CustomerDebtNotificationPM) {
               this.createDefaultNotification();
                
                if(!this.IsFromMaintenance) 
                {           
                    if (this.CustomerDebtNotificationMaintenance) {
                        this.InActive = isMaintenanceNoActive || this.CustomerDebtNotificationMaintenance.InActive == IsActiveEnum.ActiveSelectedCustomers? IsActiveEnum.NotActive : IsActiveEnum.ActiveAllCustomers;
                        this.IsActive = !(isMaintenanceNoActive || this.CustomerDebtNotificationMaintenance.InActive == IsActiveEnum.ActiveSelectedCustomers);
                    } else {
                        this.InActive = IsActiveEnum.NotActive;
                        this.IsActive = false;
                    }                   
                }
                else{
                    this.InActive = IsActiveEnum.NotActive;
                }
             }
             else
             {
                    if(!this.IsFromMaintenance && isMaintenanceNoActive)
                    {
                       this.IsActive =  false; 
                       this.InActive = IsActiveEnum.NotActive;    
                    }
                    else 
                    {
                        this.IsActive = this.CustomerDebtNotificationPM.InActive == IsActiveEnum.NotActive  ? false : true;
                        this.InActive = this.CustomerDebtNotificationPM.InActive;
                    }
                this.TypesDebts = this.CustomerDebtNotificationPM.TypesDebts;
                this.DebtLevel = this.CustomerDebtNotificationPM.DebtLevel;
             }
            
            this.CurrentSession.StopBusyIndicator();
            this.IsVisibile = true;
        });
        });

    }
    private createDefaultNotification() {
        this.CustomerDebtNotificationPM = new CustomerDebtNotificationPM();
        this.CustomerDebtNotificationPM.Tenant = SessionLocator.Tenant;
        this.CustomerDebtNotificationPM.AccountId = this.GLAccountId;
        this.TypesDebts = TypesDebtsEnum.Obligato;
        this.DebtLevel = DebtLevelEnum.Percentage;
      
    }
    onReportSchedulerClick() {
        var groupService = new ReportGroupService();
        var reportService = new ReportService();
        this.entityResourceService.getEntityResourceByTableName("TasksScheduler", 0).subscribe((response:any) => {
            var myService = new ReportGroupService();
            groupService.getReportGroupListByCode('RACC').subscribe((myResponse: ServiceResponse) => {
            var groupList: ReportGroupList = myResponse.Result;
            reportService.GetReportListsByGroupId(groupList.Id).subscribe((myResponse: ServiceResponse) => {
              
            var reportList = myResponse.Result.filter(x => x.Code === 'LTRP')[0];           
            var windowArgs: any = {};
            windowArgs.ReportGroupList = groupList;
            windowArgs.ReportList = reportList;
            windowArgs.IsCustomerDebNotification = true;
            windowArgs.TaskSchedulerId = this.TasksSchedulerId;
            windowArgs.GLAccountId = this.GLAccountId;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 1200;
            logWindow.Height = 1000;

            logWindow.Title = reportList.Name + " Scheduler";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Report/Components/Scheduler/MainReportSchedulerComponent');
        });
        });
        });
    }


    IsActive:boolean;

    IsActiveChecked(value: boolean)
    {
        if(value)
          this.CustomerDebtNotificationPM.InActive = IsActiveEnum.ActiveAllCustomers;
        else
          this.CustomerDebtNotificationPM.InActive = IsActiveEnum.NotActive;
    }
    public IsActiveFilter: string;
    IsActiveFilterItemClicked(itemValue: string)
    {
            this.InActive = itemValue;
    }
    public TypesDebtsFilter: string;
    TypesDebtsFilterItemClicked(itemValue: string)
    {
            this.TypesDebts = itemValue;
    }


    SelectedDebtLevelTypeItem =  { Code: '1', EnglishName: "Percentage", LocalName: "%" };
    DebtLevelTypeChanged(type){
        this.SelectedDebtLevelTypeItem = type;
        this.DebtLevel = type.Code;
    }
    SelectedBalanceTypeCode: string;
    DebtLevelTypes = [
        { Code: '1', EnglishName: "Percentage", LocalName: "%" },
        { Code: '2', EnglishName: "Total Amount", LocalName: "FIX" },
    ]; 


    get InActive() { return this.CustomerDebtNotificationPM?.InActive; }
    set InActive(value: string) {

        if (this.CustomerDebtNotificationPM.InActive != value) {
            this.CustomerDebtNotificationPM.InActive = value;
        }
        this.IsActiveFilter = value;
          
    }
    get TypesDebts() { return this.CustomerDebtNotificationPM?.TypesDebts; }
    set TypesDebts(value: string) {

        if (this.CustomerDebtNotificationPM.TypesDebts != value) {
            this.CustomerDebtNotificationPM.TypesDebts = value;
        }
        this.TypesDebtsFilter = value;
        if(value != TypesDebtsEnum.Obligato) {
            this.DebtLevel = DebtLevelEnum.TotalAmount;
        }
    }
    get DebtLevel() { return this.CustomerDebtNotificationPM?.DebtLevel; }
    set DebtLevel(value: string) {
        if (this.CustomerDebtNotificationPM.DebtLevel != value) {
            this.CustomerDebtNotificationPM.DebtLevel = value;
        }
        this.SelectedDebtLevelTypeItem = this.DebtLevelTypes.find(x=> x.Code === value);

    }

    get DebtLevelAmount() { return this.CustomerDebtNotificationPM?.DebtLevelAmount; }
    set DebtLevelAmount(value: number) {

        if (this.CustomerDebtNotificationPM.DebtLevelAmount != value) {
            this.CustomerDebtNotificationPM.DebtLevelAmount = value;
        }
    }

    get TasksSchedulerId() { return this.CustomerDebtNotificationPM?.TasksSchedulerId; }
    set TasksSchedulerId(value: string) {

        if (this.CustomerDebtNotificationPM.TasksSchedulerId != value) {
            this.CustomerDebtNotificationPM.TasksSchedulerId = value;
        }
    }

    get PaymentNotes() { return this.CustomerDebtNotificationPM?.PaymentNotes; }
    set PaymentNotes(value: string) {

        if (this.CustomerDebtNotificationPM.PaymentNotes != value) {
            this.CustomerDebtNotificationPM.PaymentNotes = value;
        }
        
    }

    get AccountId() { return this.CustomerDebtNotificationPM?.AccountId; }
    set AccountId(value: string) {

        if (this.CustomerDebtNotificationPM.AccountId != value) {
            this.CustomerDebtNotificationPM.AccountId = value;
        }
    }

    CancelButtonClicked() {
        if(AppTool.IsNullOrEmpty(this.CustomerDebtNotificationPM.Id) && !AppTool.IsNullOrEmpty(this.TasksSchedulerId)){
            var tasksSchedulerExtendedService =  new TasksSchedulerExtendedService();
            tasksSchedulerExtendedService.Delete(this.TasksSchedulerId)           
            .subscribe((myResponse: ServiceResponse) => {          
                this.CurrentSession.CloseCurrentWindow();             
            });
        }
        else { 
             this.CurrentSession.CloseCurrentWindow();
        }
    }
    OkButtonClicked() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    }
    SubmitChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

         if (this.CustomerDebtNotificationPM?.Id == null) {
            this.myService.insert(this.CustomerDebtNotificationPM).subscribe((myResponse: ServiceResponse) => {
                this.AfterSubmit(myResponse);
            });
        }
        else {
            this.myService.update(this.CustomerDebtNotificationPM).subscribe((myResponse: ServiceResponse) => {
                this.AfterSubmit(myResponse);
            });
        }
    }
    AfterSubmit(myResponse: ServiceResponse)
    {
        if (myResponse != null) {
            if (!myResponse.HasError) {
                if(this.IsFromMaintenance)
                   this.CurrentSession.CloseCurrentWindowEmit("ok");
                else
                   this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        }
    }

}



export enum IsActiveEnum {
    NotActive = '1',
    ActiveAllCustomers = '2',
    ActiveSelectedCustomers = '3'
}

export enum TypesDebtsEnum {
    Obligato = '1',
    AccountingBalance = '2',
    BalanceRegarding = '3'
}
export enum DebtLevelEnum {
    Percentage = '1',
    TotalAmount = '2',
    
}   

