import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClientAddressPM } from '../../../../../Customs/EntityPMs/ClientAddressPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClaimsRelatedEntitiesReasonPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntitiesReasonPM';
import { ClaimsRelatedEntsReasonsExpPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntsReasonsExpPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ClientMessagesService } from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ClientPMService } from '../../../../../Customs/Services/StandardPMs/ClientPMService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ClaimRelatedEntReasonExpLineComponent } from './ClaimRelatedEntReasonExpComponent';


@Component({
    
    templateUrl: './ClaimRelatedEntityReasonsTabComponent.html',
})

export class ClaimRelatedEntityReasonsTabComponent extends BaseComponent {
  public IsDisplayOnly: boolean = false;

    public DataContext: ClaimRelatedEntityReasonsTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(new ClaimPM());
    public ClaimPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";
    FooterMethods: any;
    public ClaimsRelatedEntityReasonslist: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    ValidationErrors: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.ValidationErrors = [];
        this.ClaimsRelatedEntityReasonslist = new ObservableCollection([]);
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
                    this.BuildReasonslist();
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
        this.isControlEnabled = isEnable;

        this.BuildReasonslist();
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

    public get ClaimExplanation() { return this.EntityPM.ClaimExplanation; }
    public set ClaimExplanation(newValue: string) { this.EntityPM.ClaimExplanation = newValue; }

    BuildReasonslist() {
        this.ClaimsRelatedEntityReasonslist = new ObservableCollection([]);

        if (this.EntityPM.ClaimsRelatedEntitiesReasons != null && this.EntityPM.ClaimsRelatedEntitiesReasons.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntitiesReasons) {
                this.ClaimsRelatedEntityReasonslist.Insert(new ClaimRelatedEntityReasonComponent(item, this.EntityPM));
            }
        }
    }

    AddEntityReasonCommand() {
        if (!this.IsControlEnabled) return;

        this.ValidationErrors = [];
        if (this.ClaimsRelatedEntityReasonslist != null && this.ClaimsRelatedEntityReasonslist.Length > 0) {
            var nullVM = this.ClaimsRelatedEntityReasonslist.Collection.filter(vm => vm.ReasonListTypeCode == null);
            if (nullVM.length > 0) {
                this.ValidationErrors = [];
                this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow"));
                return;
            }
        }

        if (this.ClaimsRelatedEntityReasonslist != null && this.ClaimsRelatedEntityReasonslist.Length >= 6) {
            this.ValidationErrors = [];
            this.ValidationErrors.push("לא ניתן להוסיף יותר מ 6 שורות");
            return;
        }

        var newClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM(this.EntityPM);
        newClaimsRelatedEntitiesReasonPM.Tenant = this.EntityPM.Tenant;
        newClaimsRelatedEntitiesReasonPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntitiesReasonPM.CounterKey = this.EntityPM.EntityCounterKey;
        newClaimsRelatedEntitiesReasonPM.LineNo = (ArrayTool.Max(this.EntityPM.ClaimsRelatedEntitiesReasons, "LineNo") + 1);

        this.ClaimsRelatedEntityReasonslist.Insert(new ClaimRelatedEntityReasonComponent(newClaimsRelatedEntitiesReasonPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntitiesReason(newClaimsRelatedEntitiesReasonPM);
    }

    DeleteExportDeclarationCommand(item: ClaimRelatedEntityReasonComponent) { // to check if need to delete explanation also ?!
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntityReasonslist.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntitiesReason(item.entityPM);
        }
    }

    EditButtonClicked(item: ClaimRelatedEntityReasonComponent) {
        if (!this.IsControlEnabled) return;

        this.ValidationErrors = [];
        if (AppTool.IsNullOrEmpty(item.ReasonListTypeCode)) {
            this.ValidationErrors = [];
            this.ValidationErrors.push("חובה להזין סיבת תביעה");
            return;
        }

        var windowArgs: any = {};
        windowArgs.EntityPM = item.entityPM;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 400;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title = TextCodeTranslator.Translate("Customs.Claim.O.ReasonAndExplanations");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.SelectionCompleted($event, item));
        logitudeWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntReasonExpComponent');

    }

    SelectionCompleted(msg: string, item: ClaimRelatedEntityReasonComponent) {
        if (msg == "Ok") {
            item.BuildReasonsExplanationList();
        }
    }

    //#endregion
}

export class ClaimRelatedEntityReasonComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimsRelatedEntitiesReason";
    public DataContext: ClaimRelatedEntityReasonComponent = this;
    public ClaimsRelatedEntsReasonsExpslist: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityPM: ClaimsRelatedEntitiesReasonPM, public claimsRelatedEntity: ClaimsRelatedEntityPM) {
        super();
        this.ClaimsRelatedEntsReasonsExpslist = new ObservableCollection([]);
        this.BuildReasonsExplanationList();
    }

    public get ReasonListTypeCode() { return this.entityPM.ReasonListTypeCode; }
    public set ReasonListTypeCode(newValue: string) { this.entityPM.ReasonListTypeCode = newValue; }

    public get ReasonListTypeName() { return this.entityPM.ReasonListTypeName; }
    public set ReasonListTypeName(newValue: string) { this.entityPM.ReasonListTypeName = newValue; }

    private _ClaimExplanationTypeCode: string;
    public get ClaimExplanationTypeCode() { return this._ClaimExplanationTypeCode; }
    public set ClaimExplanationTypeCode(newValue: string) { this._ClaimExplanationTypeCode = newValue; }

    private _ClaimExplanationTypeName: string;
    public get ClaimExplanationTypeName() { return this._ClaimExplanationTypeName; }
    public set ClaimExplanationTypeName(newValue: string) { this._ClaimExplanationTypeName = newValue; }

    private _ExplanationNote: string;
    public get ExplanationNote() { return this._ExplanationNote; }
    public set ExplanationNote(newValue: string) { this._ExplanationNote = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

    BuildReasonsExplanationList() {
        this.ClaimsRelatedEntsReasonsExpslist = new ObservableCollection([]);

        if (this.entityPM.ClaimsRelatedEntsReasonsExps != null && this.entityPM.ClaimsRelatedEntsReasonsExps.length > 0) {
            for (let item of this.entityPM.ClaimsRelatedEntsReasonsExps) {
                this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpLineComponent(item, this.entityPM));

                if (this.entityPM.ClaimsRelatedEntsReasonsExps.length == 1) {
                    this.ClaimExplanationTypeCode = this.entityPM.ClaimsRelatedEntsReasonsExps[0].ClaimExplanationTypeCode;
                    this.ClaimExplanationTypeName = this.entityPM.ClaimsRelatedEntsReasonsExps[0].ClaimExplanationTypeName;
                    this.ExplanationNote = this.entityPM.ClaimsRelatedEntsReasonsExps[0].ExplanationNote;
                }
                else if (this.entityPM.ClaimsRelatedEntsReasonsExps.length > 1) {
                    this.ClaimExplanationTypeName = "List";
                    this.ExplanationNote = "List";
                }
            }
        }
    }

    ReasonsExplanationLostFocus(event) {

        if (AppTool.IsNullOrEmpty(this.ReasonListTypeCode)
            || this.ClaimExplanationTypeName == "List"
            || AppTool.IsNullOrEmpty(this.ClaimExplanationTypeName)) {
            return;
        }
        if (this.ClaimsRelatedEntsReasonsExpslist == null || this.ClaimsRelatedEntsReasonsExpslist.Length == 0) {
            this.AddNewClaimsRelatedEntsReasonsExp(this.ClaimExplanationTypeCode, this.ExplanationNote);
        }
        else {
            this.entityPM.ClaimsRelatedEntsReasonsExps[0].ClaimExplanationTypeCode = this.ClaimExplanationTypeCode;
            this.entityPM.ClaimsRelatedEntsReasonsExps[0].ExplanationNote = this.ExplanationNote;
            this.ClaimsRelatedEntsReasonsExpslist.Collection[0].entityPM.ClaimExplanationTypeCode = this.ClaimExplanationTypeCode;
            this.ClaimsRelatedEntsReasonsExpslist.Collection[0].entityPM.ExplanationNote = this.ExplanationNote;
        }
        
    }

    AddNewClaimsRelatedEntsReasonsExp(claimExplanationTypeCode: string, explanationNote: string) {

        var newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM(this.EntityPM);
        newClaimsRelatedEntsReasonsExpPM.Tenant = this.entityPM.Tenant;
        newClaimsRelatedEntsReasonsExpPM.ClaimId = this.entityPM.ClaimId;
        newClaimsRelatedEntsReasonsExpPM.CounterKey = this.entityPM.LineNo;
        newClaimsRelatedEntsReasonsExpPM.LineNo = (ArrayTool.Max(this.entityPM.ClaimsRelatedEntsReasonsExps, "LineNo") + 1);

        if (!AppTool.IsNullOrEmpty(claimExplanationTypeCode)) {
            newClaimsRelatedEntsReasonsExpPM.ClaimExplanationTypeCode = claimExplanationTypeCode;
        }
        if (!AppTool.IsNullOrEmpty(explanationNote)) {
            newClaimsRelatedEntsReasonsExpPM.ExplanationNote = explanationNote;
        }

        this.ClaimsRelatedEntsReasonsExpslist.Insert(new ClaimRelatedEntReasonExpLineComponent(newClaimsRelatedEntsReasonsExpPM, this.entityPM));
        this.entityPM.AddClaimsRelatedEntsReasonsExp(newClaimsRelatedEntsReasonsExpPM);
    }

}
