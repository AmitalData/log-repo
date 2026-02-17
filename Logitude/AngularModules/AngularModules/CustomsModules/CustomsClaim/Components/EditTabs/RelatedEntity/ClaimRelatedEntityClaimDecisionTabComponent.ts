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
    templateUrl: './ClaimRelatedEntityClaimDecisionTabComponent.html',
})

export class ClaimRelatedEntityClaimDecisionTabComponent extends BaseComponent {
    public DataContext: ClaimRelatedEntityClaimDecisionTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(new ClaimPM());
    public ClaimPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";

    public ClaimsRelatedEntitiesSeizureslist: ObservableCollection;
    public ClaimsRelatedEntitiesRefundslist: ObservableCollection;

    public CurrentEditComponentId: string;
    public IsControlEnabled: boolean = true;

    ValidationErrors: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.ValidationErrors = [];
        this.ClaimsRelatedEntitiesSeizureslist = new ObservableCollection([]);
        this.ClaimsRelatedEntitiesRefundslist = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.BuildSeizureslist();
                    this.BuildRefundslist();
                }
            });

            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                    //if (tabCode == "CLMG") {
                    //    this.RefreshEntity();
                    //    this.BuildReasonslist();
                    //}
                }
            });
        }
    }

    InitTab(entityPM: ClaimsRelatedEntityPM, claimPM: ClaimPM, isEnable: boolean) {

        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;

        this.UIProperties.SetEnabled("DecisionCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DecisionNote", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EilatVatRefoundDecision", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DepositingAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RefundAmount", this.ObjectTableName, false);

        this.BuildSeizureslist();
        this.BuildRefundslist();
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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

    public get DecisionCode() { return this.EntityPM.DecisionCode; }
    public set DecisionCode(newValue: string) { this.EntityPM.DecisionCode = newValue; }

    public get DecisionName() { return this.EntityPM.DecisionName; }
    public set DecisionName(newValue: string) { this.EntityPM.DecisionName = newValue; }

    public get DepositingAmount() { return this.EntityPM.DepositingAmount; }
    public set DepositingAmount(newValue: number) { this.EntityPM.DepositingAmount = newValue; }

    public get RefundAmount() { return this.EntityPM.RefundAmount ; }
    public set RefundAmount(newValue: number) { this.EntityPM.RefundAmount = newValue; }

    public get DecisionNote() { return this.EntityPM.DecisionNote; }
    public set DecisionNote(newValue: string) { this.EntityPM.DecisionNote = newValue; }

    public get EilatVatRefoundDecision() { return this.EntityPM.EilatVatRefoundDecision; }
    public set EilatVatRefoundDecision(newValue: string) { this.EntityPM.EilatVatRefoundDecision = newValue; }

    BuildSeizureslist() {
        this.ClaimsRelatedEntitiesSeizureslist = new ObservableCollection([]);

        if (this.EntityPM.ClaimsRelatedEntitiesSeizures != null && this.EntityPM.ClaimsRelatedEntitiesSeizures.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntitiesSeizures) {
                this.ClaimsRelatedEntitiesSeizureslist.Insert(item);
            }
        }
    }

    BuildRefundslist() {
        this.ClaimsRelatedEntitiesRefundslist = new ObservableCollection([]);

        if (this.EntityPM.ClaimsRelatedEntitiesRefunds != null && this.EntityPM.ClaimsRelatedEntitiesRefunds.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntitiesRefunds) {
                this.ClaimsRelatedEntitiesRefundslist.Insert(item);
            }
        }
    }

    //#endregion
}

