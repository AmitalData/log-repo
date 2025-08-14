import {Component, OnDestroy, OnInit } from '@angular/core';
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
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { GLAccountPM } from 'Accounting/EntityPMs/GLAccountPM';
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
    public EntityPM: GLAccountPM = null;

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
    
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();

    }
  
    ngOnInit() {  
        this.entityResourceService.getEntityResourceByTableName("CustomerDebtNotification").subscribe((res: any) => {
            this.CurrentSession.StartBusyIndicator('Loading...');   
            if(!this.IsFromMaintenance)
            this.EntityPM.DisableMarkAsDirty = true;
            this.ApiQueryFilters = new ApiQueryFilters();      
            this.GLAccountId = !AppTool.IsNullOrEmpty(this.EntityPM ?.Id) ? this.EntityPM?.Id : null;
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
    
     private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        this.taskSchedulerSub = AccountingEventManager.TasksSchedulerId.subscribe((event: string) => {
            if (!AppTool.IsNullOrEmpty(event)) {
               this.TasksSchedulerId = event;
            }
        });
         if (this.CurrentSession.CurrentEditComponent != null) {

            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                      
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

        }
    }


   
    private LoadCustomerDebtNotification() {
        var myService: CustomerDebtNotificationExtendedPMService = new CustomerDebtNotificationExtendedPMService();
           myService.GetCustomerDebtNotificationByAccountId(this.GLAccountId).subscribe((response: ServiceResponse) => {
            this.CustomerDebtNotificationPM = response.Result as CustomerDebtNotificationPM;

           myService.GetCustomerDebtNotificationByAccountId(null).subscribe((res: ServiceResponse) => {
            this.CustomerDebtNotificationMaintenance = res.Result as CustomerDebtNotificationPM;

                const isMaintenanceNoActive =  this.CustomerDebtNotificationMaintenance?.InActive === IsActiveEnum.NotActive ||
                                               AppTool.IsNullOrEmpty(this.CustomerDebtNotificationMaintenance?.InActive);

               
            if (!this.Notification) {
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
                        this.IsActive = this.Notification.InActive == IsActiveEnum.NotActive  ? false : true;
                        this.InActive = this.Notification.InActive;
                    }
                this.TypesDebts = this.Notification.TypesDebts;
                this.DebtLevel = this.Notification.DebtLevel;
             }
            
            this.CurrentSession.StopBusyIndicator();
            this.IsVisibile = true;
             if(!this.IsFromMaintenance)
            this.EntityPM.DisableMarkAsDirty = false;
        });
        });

    }
    private createDefaultNotification() {
       this.Notification = new CustomerDebtNotificationPM();
       this.Notification.Tenant = SessionLocator.Tenant;
       this.Notification.AccountId = this.GLAccountId;
     
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
            reportService.GetReportListsByGroupId(groupList.Id, SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
              
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
    IsActiveChecked(value: boolean) {
      this.InActive = value ? IsActiveEnum.ActiveAllCustomers : IsActiveEnum.NotActive;
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

     private get Notification(): CustomerDebtNotificationPM {
       return this.IsFromMaintenance ? this.CustomerDebtNotificationPM : this.EntityPM.CustomerDebtNotification;
     }
   
     private set Notification(value: CustomerDebtNotificationPM) {
       if (this.IsFromMaintenance) {
         this.CustomerDebtNotificationPM = value;
       } else {
         this.EntityPM.CustomerDebtNotification = value;
       }
     }

    get InActive() {
    return this.Notification?.InActive;
  }

  set InActive(value: string) {
    if (this.Notification.InActive != value) {
      this.Notification.InActive = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
    }
    this.IsActiveFilter = value;
  }

  get TypesDebts() {
    return this.Notification?.TypesDebts;
  }

  set TypesDebts(value: string) {
    if (this.Notification.TypesDebts != value) {
      this.Notification.TypesDebts = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
    }
    this.TypesDebtsFilter = value;
    if (value != TypesDebtsEnum.Obligato) {
      this.DebtLevel = DebtLevelEnum.TotalAmount;
    }
  }

  get DebtLevel() {
    return this.Notification?.DebtLevel;
  }

  set DebtLevel(value: string) {
    if (this.Notification.DebtLevel != value) {
      this.Notification.DebtLevel = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
    }
    this.SelectedDebtLevelTypeItem = this.DebtLevelTypes.find(x => x.Code === value);
  }

  get DebtLevelAmount() {
    return this.Notification?.DebtLevelAmount;
  }

  set DebtLevelAmount(value: number) {
    if (this.Notification.DebtLevelAmount != value) {
      this.Notification.DebtLevelAmount = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
    }
  }

  get TasksSchedulerId() {
    return this.Notification?.TasksSchedulerId;
  }

  set TasksSchedulerId(value: string) {
    if (this.Notification.TasksSchedulerId != value) {
      this.Notification.TasksSchedulerId = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
    }
  }

  get PaymentNotes() {
    return this.Notification?.PaymentNotes;
  }

  set PaymentNotes(value: string) {
    if (this.Notification.PaymentNotes != value) {
      this.Notification.PaymentNotes = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
    }
  }

  get AccountId() {
    return this.Notification?.AccountId;
  }

  set AccountId(value: string) {
    if (this.Notification.AccountId != value) {
      this.Notification.AccountId = value;
      if (!this.IsFromMaintenance) {
        this.EntityPM.MarkAsDirty("CustomerDebtNotification");
      }
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

