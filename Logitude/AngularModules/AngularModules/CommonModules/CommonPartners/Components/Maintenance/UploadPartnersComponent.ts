import { Component, OnDestroy } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { BatchTaskExecutionPM } from '../../../../Infrastructure/EntityPMs/BatchTaskExecutionPM';
import { BatchTaskExecutionListService } from '../../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'UploadPartnersComponent',

    templateUrl: './UploadPartnersComponent.html',
})

export class UploadPartnersComponent extends BaseComponent implements OnDestroy {
    public DataContext = this;
    private CommonDomainService: CommonDomainService;
    ValidationErrorsList = [];
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.CommonDomainService = new CommonDomainService();
    }

    // Commands
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private batchEntity: BatchTaskExecutionPM;
    public IsResponseProgressVisible: boolean = false;
    UploadPartnersClicked() {

        var errors: string[] = [];

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.Retries = 0;

        }
    }

    DownloadUploadPartnersTemplate() {
        this.CommonDomainService.DownloadUploadPartnersTemplate().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + fileName;
                {
                    window.open(url);
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
        batchTaskExecutionListService.getSingle(this.batchEntity.Id).subscribe((myResponse: ServiceResponse) => {

            if (myResponse.HasError) {
                this.StopTimer();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.bteList = myResponse.Result;

                if (this.bteList.StatusCode == "D") // D- Done
                {
                    this.StopTimer();
                    var window: MessageWindow = new MessageWindow();
                    window.Show("Get TM Projects Completed Succesfully");
                }

                else if (this.bteList.StatusCode == "F") // F- Failed
                {
                    this.StopTimer();
                    var window: MessageWindow = new MessageWindow();
                    window.Show("Faild: " + this.bteList.ErrorLog);
                }

                this.AdjustTimerSpeed();
            }
        });
    }

    CloseResponseProgressClicked() {
        this.StopTimer();
    }
}
