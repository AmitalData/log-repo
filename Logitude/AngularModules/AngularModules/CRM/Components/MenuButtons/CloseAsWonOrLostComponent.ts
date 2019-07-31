import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {OpportunityPM} from '../../EntityPMs/OpportunityPM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {OpportunityClosingReasonListService} from '../../Services/StandardLists/OpportunityClosingReasonListService';
import {OpportunityClosingReasonList} from '../../EntityLists/OpportunityClosingReasonList';
import {StageListService} from '../../Services/StandardLists/StageListService';
import {Validator} from '../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './CloseAsWonOrLostComponent.html',
})

export class CloseAsWonOrLostComponent extends BaseComponent {
    public entityPM: OpportunityPM;
    public ObjectTableName: string="Opportunity";
    public DataContext: CloseAsWonOrLostComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }


    SetWindowArgs(args: OpportunityPM) {
        this.entityPM = args;
        this.ActualClosingDate = DateTool.GetCurrentDateTimeAsUtc();
    }


    SetRequiredClosingReason() {
        this.UIProperties.SetRequired("ClosingReasonId", "Opportunity", AppTool.IsNullOrEmpty(this.ClosingReasonId));
        this.UIProperties.SetEnabled("ClosingReasonId", this.ObjectTableName, this.IsClosedLost);

    }

    public get ClosingReasonId() { return this.entityPM.ClosingReasonId; }

    public set ClosingReasonId(value: string) {

        this.entityPM.ClosingReasonId = value;
        this.ClosedToCompetitorId = null;
        this.SetRequiredClosingReason();


        var closingListService: OpportunityClosingReasonListService = new OpportunityClosingReasonListService();
        closingListService.getAllFromCache().subscribe(result => {
            var list: OpportunityClosingReasonList = result.Result.filter(d => d.Id == value)[0];
            if (list != null) {
                this.ClosingReasonCode = list.Code;

                if (this.ClosingReasonCode == "LC") {
                    this.LostToCompetitionVisibility = true;
                }
                else {
                    this.LostToCompetitionVisibility = false;
                }

                if ((this.ClosingReasonCode == "WN" || this.ClosingReasonCode == "LC" || this.ClosingReasonCode == "LO")) {
                    this.PostToFollowersAsWon = true;
                }
            }

        });



       

    }
    public ValidationErrorsList :Array<string>=[];

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancle");
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.ClosingReasonId)) {
            this.ValidationErrorsList.push("Closing reason is required");
        }
        Validator.TryValidateObject(this.entityPM, "Opportunity", this.ValidationErrorsList);

        if (this.ValidationErrorsList.length == 0) {
            this.entityPM.IsClosed = true;
            this.entityPM.StageDueDate = null;

            if (!this.IsClosedLost) {
                this.entityPM.Probability = 100;
                this.ComputeValue();
            }
            else {
                this.entityPM.Probability = 0;
                this.ComputeValue();
            }
            this.SetStageId();
            this.CurrentSession.CurrentEditComponent.SaveChanges("Closing Opportunity...");
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(event => {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();

            });          
        }

    }
    public get SendPostVisibility() {

        return ((this.ClosingReasonCode == "WN" || this.ClosingReasonCode == "LC" || this.ClosingReasonCode == "LO" || this.IsClosedLost) ? true : false);
    
}
    

    ComputeValue() {
        var field1 = parseFloat(this.entityPM.Probability + "");
        var field2 = this.entityPM.NumberOfShipments == null ? 0 : parseFloat(this.entityPM.NumberOfShipments+"");
       var myValue = field1 * field2 / 100;
       this.entityPM.ValueField = myValue;
    }

    SetStageId() {
        this.entityPM.LastStageIdBeforeClosure = this.entityPM.StageId;
        this.entityPM.LastStageDate = DateTool.GetCurrentDateTimeAsUtc();

        if (!this.IsClosedLost) {
            var stageListService: StageListService = new StageListService();
            stageListService.getAllFromCache().subscribe(result => {                    
                var stage = result.Result.filter(s => s.Code == "CWN")[0];
                if (stage != null) {
                    this.entityPM.StageId = stage.Id;
                    this.entityPM.StageName = stage.Name;

                    }
            });
        }
        else {
            var stageListService: StageListService = new StageListService();
            stageListService.getAllFromCache().subscribe(result => {
                var stage = result.Result.filter(s => s.Code == "CLS")[0];
                if (stage != null) {
                    this.entityPM.StageId = stage.Id;
                    this.entityPM.StageName = stage.Name;

                }
            });
        }


    }
    public get ClosingDescription() { return this.entityPM.ClosingDescription; }
    public set ClosingDescription(value: string) { this.entityPM.ClosingDescription = value; }
    public get PostToFollowersAsWon() { return this.entityPM.PostToFollowersAsWon; }
    public set PostToFollowersAsWon(value: boolean) { this.entityPM.PostToFollowersAsWon = value; }
    public lostVisi: boolean = false;
    public get LostToCompetitionVisibility() { return this.lostVisi; }
    public set LostToCompetitionVisibility(value: boolean) { this.lostVisi = value; }
    public get ClosedToCompetitorId() { return this.entityPM.ClosedToCompetitorId; }
    public set ClosedToCompetitorId(value: string) { this.entityPM.ClosedToCompetitorId = value; } 
    public get ClosingReasonCode() { return this.entityPM.ClosingReasonCode; }
    public set ClosingReasonCode(value: string) { this.entityPM.ClosingReasonCode = value; }

    public get ActualClosingDate() { return this.entityPM.ActualClosingDate; }
    public set ActualClosingDate(value: Date) {
        var myValue: Date = value;
        if (myValue != null) {
          //  myValue = Date.SpecifyKind(myValue.Value, DateTimeKind.Utc);
        }
        this.entityPM.ActualClosingDate = myValue;
    }

    private isClosedLost: boolean;
    public get IsClosedLost() {
        return this.isClosedLost;
    }

    public set IsClosedLost(value: boolean) {
        this.isClosedLost = value;
        if (value) {
            this.PostToFollowersAsWon = true;
            this.SetRequiredClosingReason();
        }

    }


  
       


}
