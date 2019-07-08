import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClaimsRelatedEntitiesSeizurePM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesSeizurePM';
import { ClaimsRelatedEntitiesRefundPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesRefundPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';


@Component({
    moduleId: module.id,
    templateUrl: './ClaimRelatedEntityCancelOrObjectionTabComponent.html',
})

export class ClaimRelatedEntityCancelOrObjectionTabComponent extends BaseComponent {
    public DataContext: ClaimRelatedEntityCancelOrObjectionTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(new ClaimPM());
    public ClaimPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";

    public CurrentEditComponentId: string;
    public IsControlEnabled: boolean = true;

    ValidationErrors: string[] = [];

    constructor(public entityArgs: EntityArgs) {
        super();

        this.ValidationErrors = [];
        SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.CurrentSession.CurrentEditComponent.ComponentId;

            SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            SessionLocator.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == SessionLocator.CurrentSession.CurrentEditComponent.ComponentId) {
                }
            });
        }
    }

    InitTab(entityPM: ClaimsRelatedEntityPM, claimPM: ClaimPM, isEnable: boolean) {

        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;

        this.UIProperties.SetEnabled("ContinuousMessagesTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Note", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ClaimRequestNumber", this.ObjectTableName, false);
    }

    RefreshEntity() {
        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get ContinuousRequestTypeCode() { return this.EntityPM.ContinuousRequestTypeCode; }
    public set ContinuousRequestTypeCode(newValue: string) { this.EntityPM.ContinuousRequestTypeCode = newValue; }

    public get ContinuousMessagesTypeCode() { return this.EntityPM.ContinuousMessagesTypeCode; }
    public set ContinuousMessagesTypeCode(newValue: string) { this.EntityPM.ContinuousMessagesTypeCode = newValue; }

    public get Explanation() { return this.EntityPM.Explanation; }
    public set Explanation(newValue: string) { this.EntityPM.Explanation = newValue; }

    public get Note() { return this.EntityPM.Note; }
    public set Note(newValue: string) { this.EntityPM.Note = newValue; }

    public get ClaimRequestNumber() { return this.EntityPM.ClaimRequestNumber; }
    public set ClaimRequestNumber(newValue: string) { this.EntityPM.ClaimRequestNumber = newValue; }

    //#endregion
}

