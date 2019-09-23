import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, DateTool } from '../../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { DeficitDecisionPM } from '../../../../../../Customs/EntityPMs/DeficitDecisionPM';
import { DeficitPM } from '../../../../../../Customs/EntityPMs/DeficitPM';
import { EntityResourceService } from '../../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './DeficitDecisionComponent.html',
    selector: 'DeficitDecisionComponent',
})

export class DeficitDecisionComponent extends BaseComponent {
    public EntityPM: DeficitDecisionPM = new DeficitDecisionPM(new DeficitPM());
    public ObjectTableName = "Customs.DeficitDecision";
    public DataContext: DeficitDecisionComponent = this;
    IsControlEnabled: any; // html component requires this property. AOT
    IsLoaded: boolean = false;

    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.EntityResourceService.getEntityResourceByTableName("Customs.DeficitDecision").subscribe((response: any) => {
                this.IsLoaded = true;
            });
        }
        this.SetScreenFieldsEditability();
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("RequestID", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RequestTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RequestDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ApprovedProfessionCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DecisionCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DecisionNoteForLetter", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalComponentAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalEstimatedAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalFinancialPenaltyAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalInterestAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalLinkingAmount", this.ObjectTableName, false);
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.DeficitDecisionItem;
        }
    }

    public get RequestID() { return this.EntityPM.RequestID; }
    public set RequestID(newValue: string) { this.EntityPM.RequestID = newValue; }

    public get RequestTypeCode() { return this.EntityPM.RequestTypeCode; }
    public set RequestTypeCode(newValue: string) { this.EntityPM.RequestTypeCode = newValue; }

    public get RequestDate() { return this.EntityPM.RequestDate; }
    public set RequestDate(newValue: Date) { this.EntityPM.RequestDate = newValue; }

    public get ApprovedProfessionCode() { return this.EntityPM.ApprovedProfessionCode; }
    public set ApprovedProfessionCode(newValue: string) { this.EntityPM.ApprovedProfessionCode = newValue; }

    public get DecisionCode() { return this.EntityPM.DecisionCode; }
    public set DecisionCode(newValue: string) { this.EntityPM.DecisionCode = newValue; }

    public get DecisionNoteForLetter() { return this.EntityPM.DecisionNoteForLetter; }
    public set DecisionNoteForLetter(newValue: string) { this.EntityPM.DecisionNoteForLetter = newValue; }

    public get TotalComponentAmount() { return this.EntityPM.TotalComponentAmount; }
    public set TotalComponentAmount(newValue: number) { this.EntityPM.TotalComponentAmount = newValue; }

    public get TotalEstimatedAmount() { return this.EntityPM.TotalEstimatedAmount; }
    public set TotalEstimatedAmount(newValue: number) { this.EntityPM.TotalEstimatedAmount = newValue; }

    public get TotalFinancialPenaltyAmount() { return this.EntityPM.TotalFinancialPenaltyAmount; }
    public set TotalFinancialPenaltyAmount(newValue: number) { this.EntityPM.TotalFinancialPenaltyAmount = newValue; }

    public get TotalInterestAmount() { return this.EntityPM.TotalInterestAmount; }
    public set TotalInterestAmount(newValue: number) { this.EntityPM.TotalInterestAmount = newValue; }

    public get TotalLinkingAmount() { return this.EntityPM.TotalLinkingAmount; }
    public set TotalLinkingAmount(newValue: number) { this.EntityPM.TotalLinkingAmount = newValue; }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }
}
