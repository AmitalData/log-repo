
import {Component} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ActivityList} from '../../EntityLists/ActivityList';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../Services/CRMDomainService'; 
import {ActivityWorkspaceComponent} from './ActivityWorkspaceComponent'; 
import {OverviewWorkspaceComponent} from './OverviewWorkspaceComponent'; 
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ActivityValidator} from '../../Validators/ActivityValidator'; 
import {ActivityPMService} from '../../Services/StandardPMs/ActivityPMService';
import {ActivityPM} from '../../EntityPMs/ActivityPM';
import {CRMTool} from '../../Tools';

export class UpcomingActivityItem extends BaseComponent {
    public entityList: ActivityList;
    public EntityId: string;
    public ObjectTableName: string = "Activity";
    public ValidationErrorsList = [];
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityList: ActivityList, public ActivityWorkspaceComponent: ActivityWorkspaceComponent, public OverviewWorkspaceComponent: OverviewWorkspaceComponent ) {
        super();
        this.entityList = entityList;
        this.EntityId = entityList.Id;
        this.ImageSrc = CRMTool.GetActivityImageSrc(this.entityList.ActivityTypePathCode);
        this.SetCompleteButtonsVisibility();
        this.GetCompletedBackground();

        if (this.entityList.ActivityTypeCode == "AP") {
            this.IsCompleted = true;
        }
    }

    get PriorityCode() { return this.entityList.PriorityCode; }
    get PriorityName() { return this.entityList.PriorityName; }
    get ActivityTypeName() { return this.entityList.ActivityTypeName; }
    get Subject() { return this.entityList.Subject; }
    get UpcomingDate() { return this.entityList.UpcomingDate; }
    get CustomerName() { return this.entityList.CustomerName; }
    get OwnerName() { return this.entityList.OwnerName; }
    get OpportunityId() { return this.entityList.OpportunityId; }
    get CustomerId() { return this.entityList.CustomerId; }
    get QuoteId() { return this.entityList.QuoteId; }
    get MeetingSummary() { return this.entityList.MeetingSummary; }
    set MeetingSummary(value: string) {
        if (this.entityList.MeetingSummary != value) {
            this.entityList.MeetingSummary = value;
        }
    }
    get PostToFollowers() { return this.entityList.PostToFollowers; }
    set PostToFollowers(value: boolean) {
        if (this.entityList.PostToFollowers != value) {
            this.entityList.PostToFollowers = value;
        }
    }
    private isCompleted;
    get IsCompleted() { return this.isCompleted; }
    set IsCompleted(value: boolean) {
        this.isCompleted = value;
        if (value) {
            this.PostToFollowers = true;
        }
    }
    get IsOpen(){ return this.entityList.IsOpen; } 

    public ImageSrc: string;
  

    public IsCompleteVisible: boolean = false;
    public IsReopenVisible: boolean = false;
    public IsCompleteToggleVisible: boolean = false;
    private SetCompleteButtonsVisibility() {
        if (this.entityList.IsOpen) {
            if (this.entityList.ActivityTypeCode == "AP") {
                this.IsReopenVisible = false;
                this.IsCompleteToggleVisible = true;
                this.IsCompleteVisible = false;
            }

            else {
                this.IsReopenVisible = false;
                this.IsCompleteToggleVisible = false;
                this.IsCompleteVisible = true;
            }
        }
        else {
            this.IsReopenVisible = true;
            this.IsCompleteToggleVisible = false;
            this.IsCompleteVisible = false;
        }
    }

    public CompletedBackground: string;
    GetCompletedBackground() {
        if (this.IsOpen) {
            this.CompletedBackground = "rgb(255,255,255)";
        }

        else {
            this.CompletedBackground = "rgba(0,0,0,0.1)";
        }
    }

    ViewConnectedEntity(code: string) {
        var tableName: string = null;
        var entityId: string = null;
        switch (code) {
            case "OPP": {
                tableName = "Opportunity";
                entityId = this.entityList.OpportunityId;
                break;
            }

            case "CUS": {
                tableName = "Customer";
                entityId = this.entityList.CustomerId;
                break;
            }

            case "QUT": {
                tableName = "Quote";
                entityId = this.entityList.QuoteId;
                break;
            }
        }

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, BackButtonLabel: 'Activity' });

                let isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                });

                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
    }
    CompleteClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetCompleteActivity(this.entityList.Id, this.PostToFollowers, this.MeetingSummary).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entityList = resp.Result;
                if (this.ActivityWorkspaceComponent) {
                    this.ActivityWorkspaceComponent.LoadAllScreenData();
                }
                if (this.OverviewWorkspaceComponent) {
                    //this.OverviewWorkspaceComponent.LoadActivitiesSummary();
                    this.SetCompleteButtonsVisibility();
                    this.GetCompletedBackground();
                }
            }
        });
    }
    ReopenClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetReopenActivity(this.entityList.Id).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entityList = resp.Result;
                if (this.ActivityWorkspaceComponent) {
                    this.ActivityWorkspaceComponent.LoadAllScreenData();
                }
                if (this.OverviewWorkspaceComponent) {
                    //this.OverviewWorkspaceComponent.LoadActivitiesSummary();
                    this.SetCompleteButtonsVisibility();
                    this.GetCompletedBackground();
                }
            }
        });
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors = [];
        if (!AppTool.IsNullOrEmpty(this.MeetingSummary)) {
            if (this.MeetingSummary.length > 5000) {
                errors.push("Meeting Summary must be less\nthan 5000 char");
            }
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.LoadActivityPM();
        }
    }
    LoadActivityPM() {
        var myService: ActivityPMService = new ActivityPMService();
        myService.get(this.entityList.Id).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                var entityPM: ActivityPM = resp.Result;
                entityPM.IsOpen = false;
                entityPM.MeetingSummary = this.MeetingSummary;
                var activityValidator: ActivityValidator = new ActivityValidator();
                var validEntry = activityValidator.Validate(entityPM);
                if (validEntry && validEntry.length == 0) {
                    this.CompleteClicked();
                }
                else {
                    entityPM.IsOpen = true;
                }
            }
        });
    }
}
