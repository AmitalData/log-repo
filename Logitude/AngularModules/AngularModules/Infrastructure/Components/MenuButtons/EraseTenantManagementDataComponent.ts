import { Component, OnDestroy}  from '@angular/core';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {InfrastructureDomainService, BusinessRecordsSummary} from '../../Services/InfrastructureDomainService';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import { BatchTaskExecutionPM } from '../../EntityPMs/BatchTaskExecutionPM';
import { BatchTaskExecutionListService } from '../../Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../EntityLists/BatchTaskExecutionList';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
    moduleId: module.id,
    templateUrl: './EraseTenantManagementDataComponent.html',
})

export class EraseTenantManagementDataComponent implements OnDestroy {
    private myService: InfrastructureDomainService;
    private entityId: number;
    public Message: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myService = new InfrastructureDomainService();   
    }
    
    SetWindowArgs(args: number) {
        this.entityId = args;
        this.Message = null;

        this.GetCounts();
    }

    private summaryRecord: BusinessRecordsSummary;
    private GetCounts() {
        this.CurrentSession.StartBusyIndicator("Check Data Counts...");  

        this.myService.GetDataCountForTenant(this.entityId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.summaryRecord = response.Result;                
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private type: string;
    public DeleteClicked(type: string) {
        this.Message = null;
        this.type = type;

        switch (type) {
            case "B": {                
                this.DoDelete(type);
                break;
            }

            case "T": {
                if (this.summaryRecord != null) {
                    if (this.summaryRecord.ActivitiesCount == 0) {
                        this.DoDelete(type);
                    }

                    else {
                        this.Message = "You can't Tickets, since you have CRM Records";
                    }
                }
                
                break;
            }

            case "C": {
                this.DoDelete(type);
                break;
            }

            case "P": {
                if (this.summaryRecord != null) {
                    if (this.summaryRecord.ShipmentsCount == 0 && this.summaryRecord.QuotesCount == 0 && this.summaryRecord.ARInvoicesCount == 0
                        && this.summaryRecord.APInvoicesCount == 0 && this.summaryRecord.ARPaymentsCount == 0 && this.summaryRecord.APPaymentsCount == 0
                        && this.summaryRecord.TicketsCount == 0 && this.summaryRecord.ActivitiesCount == 0 && this.summaryRecord.OpportunitiesCount == 0) {

                        this.DoDelete(type);
                    }

                    else {
                        this.Message = "You can't Erase Shippers & Consignees, since you have Business Records, CRM Records or Tickets";
                    }
                }

                break;
            }
        }        
    }

    public IsResponseProgressVisible: boolean = false;
    CloseResponseProgressClicked() {      
        this.StopTimer();
    }

    private batchEntity: BatchTaskExecutionPM;
    private DoDelete(type: string) {
        this.Message = null;
        this.Retries = 0;

        this.myService.DeleteDataForTenant(this.entityId, type).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var mm: ServiceResponse = response;
                this.batchEntity = mm.Result;

                if (this.batchEntity != null) {
                    this.IsResponseProgressVisible = true;
                    this.timer = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
                }
            }            
        });
    }

    // Timer
    private timerSeconds: number = 1;
    timer: any;
    private Retries: number = 0;
    private IncreaseTimer() {
        clearTimeout(this.timer);
        this.timer = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
    }
    private AdjustTimerSpeed() {
        if (this.Retries <= 60) {
            if (this.timerSeconds != 1) {
                this.timerSeconds = 1;
                this.IncreaseTimer();
            }
        }

        else if (this.Retries <= 120) {
            if (this.timerSeconds != 5) {
                this.timerSeconds = 5;
                this.IncreaseTimer();
            }
        }

        else if (this.Retries <= 180) {
            if (this.timerSeconds != 60) {
                this.timerSeconds = 60;
                this.IncreaseTimer();
            }
        }

        else {
            this.StopTimer();
        }
    }
    private RunTimerFunction() {
        this.Retries++;
        this.GetBTE();
        this.AdjustTimerSpeed();
    }
    public StopTimer() {
        if (this.timer) {
            clearTimeout(this.timer);
        }
        
        this.IsResponseProgressVisible = false;
    }

    ngOnDestroy() {
        this.StopTimer();
    }
    
    private bteList: BatchTaskExecutionList;
    GetBTE() {
        var batchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        batchTaskExecutionListService.getSingle(this.batchEntity.Id).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.bteList = mm.Result;

                var window: MessageWindow = new MessageWindow();
                if (this.bteList.StatusCode == "D") // D- Done
                {
                    this.GetCounts();                    

                    switch (this.type) {
                        case "B": {
                            window.Show("Erasing Business Records Completed Succesfully");
                            break;
                        }

                        case "P": {
                            window.Show("Erasing Shippers & Consignees Completed Succesfully");
                            break;
                        }

                        case "T": {
                            window.Show("Erasing Tickets Completed Succesfully");
                            break;
                        }

                        case "C": {
                            window.Show("Erasing CRM Data Completed Succesfully");
                            break;
                        }
                    }

                    this.StopTimer();
                }

                else if (this.bteList.StatusCode == "F") // F- Failed
                {
                    this.StopTimer();
                    window.Show("Faild: " + this.bteList.ErrorLog);
                }
            }
        });
    }

    public ResetCountersClicked(code: string) {
        this.Message = null;

        if (this.summaryRecord != null) {
            if (code == "B") {
                if (this.summaryRecord.ShipmentsCount == 0 && this.summaryRecord.QuotesCount == 0 && this.summaryRecord.ARInvoicesCount == 0
                    && this.summaryRecord.APInvoicesCount == 0 && this.summaryRecord.ARPaymentsCount == 0 && this.summaryRecord.APPaymentsCount == 0) {

                    this.DoReset(code);
                }

                else {
                    this.Message = "You can't Reset Counters, since you have Business Records, CRM data";
                }
            }


            else if (code == "P") {
                if (this.summaryRecord.CustomersCount == 0) {

                    this.DoReset(code);
                }

                else {
                    this.Message = "You can't Reset Counters, since you have Shippers & Consignees";
                }
            }

            else if (code == "T") {
                if (this.summaryRecord.TicketsCount == 0) {

                    this.DoReset(code);
                }

                else {
                    this.Message = "You can't Reset Counters, since you have Tickets";
                }
            }
        }            
    }
    
    private DoReset(code: string) {
        this.CurrentSession.StartBusyIndicator("Reset Counters...");

        this.myService.ResetCountersForTenant(this.entityId, code).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var window: MessageWindow = new MessageWindow();
                window.Show("Reset Counters Completed Succesfully");
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    public CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }    
}
