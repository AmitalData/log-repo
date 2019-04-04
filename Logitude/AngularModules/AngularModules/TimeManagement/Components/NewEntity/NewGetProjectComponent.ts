import { Component, OnDestroy } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TMProjectPM } from '../../EntityPMs/TMProjectPM';
import { TMProjectPMService } from '../../Services/StandardPMs/TMProjectPMService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DateTool } from '../../../Infrastructure/Tools';
import { BatchTaskExecutionPM } from '../../../Infrastructure/EntityPMs/BatchTaskExecutionPM';
import { BatchTaskExecutionListService } from '../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { TimeManagementDomainService } from '../../Services/TimeManagementDomainService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';


@Component({
    selector: 'NewGetProjectComponent',
    moduleId: module.id,
    templateUrl: './NewGetProjectComponent.html',
})

export class NewGetProjectComponent extends BaseComponent implements OnDestroy {
    public DataContext = this;
    private myService: TimeManagementDomainService;

    constructor() {
        super();
        this.SetUIProperties();
        this.myService = new TimeManagementDomainService();   

    }

    SetUIProperties() {
        this.UIProperties.SetRequired("FromDate", null, this.FromDate == null);
        this.UIProperties.SetRequired("ToDate", null, this.ToDate == null);
    }

    private userId: string;
    get UserId() {
        return this.userId;
    }
    set UserId(value: string) {
        if (this.userId != value) {
            this.userId = value;
        }
    }

    private fromDate: Date;
    get FromDate() {
        return this.fromDate;
    }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.SetUIProperties();
        }
    }

    private toDate: Date;
    get ToDate() {
        return this.toDate;
    }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.SetUIProperties();
        }
    }


    // Commands
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
    private batchEntity: BatchTaskExecutionPM;
    public IsResponseProgressVisible: boolean = false;
    GetProjectsButtonClicked() {
        this.Retries = 0;
        this.myService.GetTMProjectsByBatchTask(this.UserId, this.FromDate, this.ToDate).subscribe((response: ServiceResponse) => {
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
    private timerSeconds: number = 10;
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
                    window.Show("Get TM Projects Completed Succesfully");
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

    CloseResponseProgressClicked() {
        this.StopTimer();
    }
}
