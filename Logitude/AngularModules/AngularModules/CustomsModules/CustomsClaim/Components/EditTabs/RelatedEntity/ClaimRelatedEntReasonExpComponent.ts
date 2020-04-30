import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimsRelatedEntitiesReasonPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesReasonPM';
import { ClaimsRelatedEntsReasonsExpPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntsReasonsExpPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ClaimExplanationCodeListService } from '../../../../../Customs/Services/StandardLists/ClaimExplanationCodeListService';


@Component({
    
    templateUrl: './ClaimRelatedEntReasonExpComponent.html',
})

export class ClaimRelatedEntReasonExpComponent extends BaseComponent {
  public IsDisplayOnly: boolean = false;

    public DataContext: ClaimRelatedEntReasonExpComponent = this;
    public EntityPM: ClaimsRelatedEntitiesReasonPM;
    public ObjectTableName: string = "Customs.ClaimsRelatedEntsReasonsExp";
    public ClaimsRelatedEntsReasonsExpslist: ObservableCollection;
    FooterMethods: any;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    ValidationErrors: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.ValidationErrors = [];
        this.ClaimsRelatedEntsReasonsExpslist = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;

        if (this.EntityPM.ClaimsRelatedEntsReasonsExps != null && this.EntityPM.ClaimsRelatedEntsReasonsExps.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntsReasonsExps) {
                this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpLineComponent(item, this.EntityPM));
            }
        }

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

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    AddNewEntityReasonExplanationCommand() {
        if (!this.IsControlEnabled) return;

        if (this.ClaimsRelatedEntsReasonsExpslist != null && this.ClaimsRelatedEntsReasonsExpslist.Length > 0) {
            var nullVM = this.ClaimsRelatedEntsReasonsExpslist.Collection.filter(vm => vm.ClaimExplanationTypeCode == null || vm.ClaimExplanationTypeCode == "");
            if (nullVM.length > 0) {
                this.ValidationErrors = [];
                this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow"));
                return;
            }
        }

        var newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM(this.EntityPM);
        newClaimsRelatedEntsReasonsExpPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntsReasonsExpPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntsReasonsExpPM.CounterKey = this.EntityPM.CounterKey;
        newClaimsRelatedEntsReasonsExpPM.LineNo = (ArrayTool.Max(this.EntityPM.ClaimsRelatedEntsReasonsExps, "LineNo") + 1);

        this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpLineComponent(newClaimsRelatedEntsReasonsExpPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntsReasonsExp(newClaimsRelatedEntsReasonsExpPM);
    }

    DeleteReasonExplanationCommand(item) { 
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntsReasonsExpslist.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntsReasonsExp(item.entityPM);
        }
    }

    CancelButtonClicked() {
        this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    //#endregion
}

export class ClaimRelatedEntReasonExpLineComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimsRelatedEntsReasonsExp";
    public DataContext: ClaimRelatedEntReasonExpLineComponent = this;
    public ClaimsRelatedEntsReasonsExpslist: ObservableCollection;

    public myClaimExplanationCodeListService: ClaimExplanationCodeListService = new ClaimExplanationCodeListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityPM: ClaimsRelatedEntsReasonsExpPM, public claimsRelatedEntitiesReasonPM: ClaimsRelatedEntitiesReasonPM) {
        super();

        if (!AppTool.IsNullOrEmpty(this.ClaimExplanationTypeCode) && AppTool.IsNullOrEmpty(this.ClaimExplanationTypeName)) {
            this.myClaimExplanationCodeListService.getSingle(this.ClaimExplanationTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.ClaimExplanationTypeName = myResponse.Result.LocalName;
                    }
                }
            });

        }
    }

    public get ClaimExplanationTypeCode() { return this.entityPM.ClaimExplanationTypeCode; }
    public set ClaimExplanationTypeCode(newValue: string) { this.entityPM.ClaimExplanationTypeCode = newValue; }

    public get ClaimExplanationTypeName() { return this.entityPM.ClaimExplanationTypeName; }
    public set ClaimExplanationTypeName(newValue: string) { this.entityPM.ClaimExplanationTypeName = newValue; }

    public get ExplanationNote() { return this.entityPM.ExplanationNote; }
    public set ExplanationNote(newValue: string) { this.entityPM.ExplanationNote = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }


}
