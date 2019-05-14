import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../EntityPMs/TicketPM';
import {TicketStageList} from '../../EntityLists/TicketStageList';
import {TicketStageListService} from '../../Services/StandardLists/TicketStageListService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {TicketValidator} from '../../Validators/TicketValidator';
import {TicketStagesArgs, TicketClosureArgs} from '../../Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TicketPMService} from '../../Services/StandardPMs/TicketPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../Services/CRMDomainService';

@Component({
    moduleId: module.id,
    templateUrl: "TicketHelperComponent.html",
})

export class TicketHelperComponent {
    public EntityPM: TicketPM;
    public StagesList: TicketStagesArgs[] = [];
    public ObjectTableName = "Ticket";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.Listen();
            this.BuildComponent();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildComponent();
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    }

    private BuildComponent() {
        this.BuildStages();
    }

    // Send Button
    private stageButtonContent: TicketStagesArgs = new TicketStagesArgs();
    get StageButtonContent() {
        return this.stageButtonContent;
    }
    set StageButtonContent(value: TicketStagesArgs) {
        this.stageButtonContent = value;
    }

    private stageButtonCode: string = "";
    get StageButtonCode() {
        return this.stageButtonCode;
    }
    set StageButtonCode(value: string) {
        this.stageButtonCode = value;
    }

    private isOpened = false;
    get IsOpened() {
        return this.isOpened;
    }
    set IsOpened(value: boolean) {
        this.isOpened = value;
    }

    private BuildStages() {
        this.StagesList = [];
        this.GetTicketStageMethod();
    }

    // Ticket Stages
    private GetTicketStageMethod() {
        var myService: TicketStageListService = new TicketStageListService();
        myService.getAll().subscribe((resp: any) => {
            if (!resp.HasError) {
                var stages: TicketStageList[] = resp.Result;
                stages.forEach(item => {
                    var IsEnabled = true;

                    if ((!FeatureLocator.HasFeaturePermession("Ticket", "TicketClosure") && (item.Code == "RE" || item.Code == "CS")) ||
                        (!FeatureLocator.HasFeaturePermession("Ticket", "SaveAsClosed") && item.Code == "CS") ||
                        (!FeatureLocator.HasFeaturePermession("Ticket", "SaveAsResolved") && item.Code == "RE") ||
                        (!FeatureLocator.HasFeaturePermession("Ticket", "SaveAsOpen") && item.Code == "OP")) {
                        IsEnabled = false;
                    }
                   
                    var stage: TicketStagesArgs = new TicketStagesArgs();
                    stage.Code = item.Code;
                    stage.Name = "Save as " + item.Name;
                    stage.IsEnabled = IsEnabled;
                    stage.ItemOpacity = IsEnabled ? 1: 0.5;
                    this.StagesList.push(stage);
                    if (this.EntityPM.StageCode == stage.Code) {
                        this.StageButtonContent = stage;
                    }

                });

                this.StageButtonContent.Name = "Save as " + this.EntityPM.StageName;
                this.StageButtonCode = this.EntityPM.StageCode;
            }
        });
    }

    public OptionSelectionChanged(option) {
        if (option != null) {
            this.StageButtonContent = option;
            this.StageButtonCode = option.Code;
            this.IsOpened = false;
            this.ChangeStageCommand();
        }
    }

    private ChangeStageCommand() {
        var validator: TicketValidator = new TicketValidator();

        var isValid = true;
        var errors = validator.ValidateCurrenctEntity(this.EntityPM);
        if (errors != null && errors.length > 0) {
            isValid = false;
            if (this.CurrentSession.CurrentEditComponent != null) {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            }
        }

        if (isValid) {
            this.CheckTicketOwnerPermission();

        }
    }
    CheckTicketOwnerPermission() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetTicketOwnerPermission(this.EntityPM.OwnerId, this.EntityPM.OwnerName).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CompleteWorking();
            }
            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }

    CompleteWorking() {
        if (this.StageButtonCode != "CS") {
            this.EntityPM.IsClosed = false;
        }

        // Resolve Case or closed
        if (this.StageButtonCode == "RE" || this.StageButtonCode == "CS") {
            var currentDateTime: Date = DateTool.GetCurrentDateTimeAsUtc();
            if (this.StageButtonCode == "RE") {
                if (this.EntityPM.FirstResolveDate == null) {
                    this.EntityPM.FirstResolveDate = currentDateTime;
                }

                if (this.EntityPM.FirstResponseTime == null) {
                    this.EntityPM.FirstResponseTime = currentDateTime;
                }
                this.EntityPM.FullResolvedTime = currentDateTime;
            }

            if (this.StageButtonCode == "CS") {
                if (this.EntityPM.FirstResolveDate == null) {
                    this.EntityPM.FirstResolveDate = currentDateTime;
                }

                if (this.EntityPM.FullResolvedTime == null) {
                    this.EntityPM.FullResolvedTime = currentDateTime;
                }

                if (this.EntityPM.FirstResponseTime == null) {
                    this.EntityPM.FirstResponseTime = currentDateTime;
                }

                if (this.EntityPM.FirstCloseDate == null) {
                    this.EntityPM.FirstCloseDate = currentDateTime;
                }

                this.EntityPM.LastCloseDate = currentDateTime;
            }

            // Validate entity 
            this.ClosuerWindow();
        }

        else {
            this.SaveChanges();
        }
    }

    ClosuerWindow() {
        var windowTitle = "Ticket Closure";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        var args = new TicketClosureArgs();
        args.Ticket = this.EntityPM;
        args.StageCode = this.StageButtonCode;
        logWindow.WindowArgs = args;
        logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/Others/TicketClosureComponent');
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (comp.IsOkClosed == true) {
                        this.SaveChanges();
                    }
                }
            });
        });
    }
    SaveChanges() {
        //Update Ticket Stage 
        var myService: TicketStageListService = new TicketStageListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var stages: TicketStageList[] = resp.Result;
                var selectedStage = stages.filter(d => d.Code == this.StageButtonCode && d.Tenant == this.EntityPM.Tenant)[0];
                if (selectedStage != null) {
                    this.EntityPM.StageId = selectedStage.Id;
                    this.EntityPM.StageCode = selectedStage.Code;
                    this.EntityPM.StageName = selectedStage.Name;
                    if (this.entityArgs.EditComponent != null) {
                        this.entityArgs.EditComponent.SaveChanges();
                    }
                }
            }
        });
    }
}

