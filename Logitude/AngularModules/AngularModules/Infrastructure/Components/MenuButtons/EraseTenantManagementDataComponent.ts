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
    public ValidationErrorsList: string[] = [];
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
    
    private batchEntity: BatchTaskExecutionPM;
    private DoDelete(type: string) {
        this.Message = null;        

        this.myService.DeleteDataForTenant(this.entityId, type).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var mm: ServiceResponse = response;
                this.batchEntity = mm.Result;

                if (this.batchEntity != null) {
                    this.CurrentSession.StartBusyIndicator("Deleting...");
                    this.StopTimer();

                    this.timer = setInterval(() => {
                        this.GetBTE();
                    }, this.timerInterval);
                }
            }            
        });
    }
    
    // Timer
    timer: any;
    timerInterval: number = 1000;
    public StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }

    ngOnDestroy() {
        this.StopTimer();
    }
    
    GetBTE() {
        var batchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        batchTaskExecutionListService.getSingle(this.batchEntity.Id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var bteList: BatchTaskExecutionList = myResponse.Result;
                
                if (bteList.StatusCode == "D")
                {
                    this.ValidationErrorsList = [];

                    this.GetCounts();

                    switch (this.type) {
                        case "B": {
                            this.Message = "Erasing Business Records Completed Succesfully";
                            break;
                        }

                        case "P": {
                            this.Message = "Erasing Shippers & Consignees Completed Succesfully";
                            break;
                        }

                        case "T": {
                            this.Message = "Erasing Tickets Completed Succesfully";
                            break;
                        }

                        case "C": {
                            this.Message = "Erasing CRM Data Completed Succesfully";
                            break;
                        }
                    }

                    this.StopTimer();
                }

                else if (bteList.StatusCode == "F") {
                    this.StopTimer();
                    this.CurrentSession.StopBusyIndicator();

                    var errors: string[] = [];
                    errors.push(bteList.ErrorLog);
                    this.ValidationErrorsList = errors;
                }

                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.StartBusyIndicator("Deleting... ");
                }
            }

            else {
                this.StopTimer();
                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
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
