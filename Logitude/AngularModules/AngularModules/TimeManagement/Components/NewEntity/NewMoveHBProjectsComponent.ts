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
    selector: 'NewMoveHBProjectsComponent',
    moduleId: module.id,
    templateUrl: './NewMoveHBProjectsComponent.html',
})

export class NewMoveHBProjectsComponent extends BaseComponent implements OnDestroy {
    public DataContext = this;
    private myService: TimeManagementDomainService;
    ValidationErrorsList = [];
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.SetUIProperties();
        this.myService = new TimeManagementDomainService();

    }

    SetUIProperties() {
       this.UIProperties.SetRequired("FromProject", null, this.FromProject == null);
       this.UIProperties.SetRequired("ToProject", null, this.ToProject == null);
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
    private fromProject: string;
    get FromProject() {
        return this.fromProject;
    }
    set FromProject(value: string) {
        if (this.fromProject != value) {
            this.fromProject = value;
            this.SetUIProperties();
        }
    }

    private toProject: string;
    get ToProject() {
        return this.toProject;
    }
    set ToProject(value: string) {
        if (this.toProject != value) {
            this.toProject = value;
            this.SetUIProperties();
        }
    }

    private fromDate: Date;
    get FromDate() {
        return this.fromDate;
    }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
      
        }
    }

    private toDate: Date;
    get ToDate() {
        return this.toDate;
    }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
           
        }
    }


    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
  private batchEntity: BatchTaskExecutionPM;
    public IsResponseProgressVisible: boolean = false;
    MoveHoursClicked() {

        var errors: string[] = [];
        if (this.FromProject == null) {
            errors.push("From Project field is required");
        }
        if (this.ToProject == null) {
            errors.push("To Project field is required");
        }
        if (this.FromProject == this.ToProject) {
            errors.push("Can't Move Hours: From Project, To Project Are The Same");
        }
        if (this.FromDate > this.ToDate) {
            errors.push("From Date cannot be greater than To Date");
        }
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.Retries = 0;
            ////////////
            this.myService.GetTMProjectsByBatchProject(this.UserId, this.FromProject, this.ToProject, this.FromDate, this.ToDate).subscribe((myResponse: ServiceResponse) => {
           ////////////////
                if (myResponse.HasError) {
                    this.StopTimer();
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.batchEntity = myResponse.Result;

                    if (this.batchEntity != null) {
                        this.IsResponseProgressVisible = true;
                        this.timer = setInterval(() => this.RunTimerFunction(), this.timerSeconds * 1000);
                    }
                }
            });
        }
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
        this.MoveHours();

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
    MoveHours() {
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
                    window.Show("Moving Hours Between Projects Completed Succesfully");
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
