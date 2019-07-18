import { Component } from '@angular/core';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ContinuousRequestOnClaimFileRequestParams } from '../../../../../Customs/DataContract/RequestParams/ContinuousRequestOnClaimFileRequestParams';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ClaimsRelatedEntityExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/ClaimsRelatedEntityExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';


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
    public IsNewEntity: boolean = false;

    ValidationErrors: string[] = [];
    _ClaimsRelatedEntityExtendedPMService: ClaimsRelatedEntityExtendedPMService = new ClaimsRelatedEntityExtendedPMService()

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

    InitTab(entityPM: ClaimsRelatedEntityPM, claimPM: ClaimPM, isEnable: boolean, isNew: boolean) {

        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.IsNewEntity = isNew;

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

    FillErrors() {

        var errors: string[] = [];
        this.ValidationErrors = errors;

        if (AppTool.IsNullOrEmpty(this.ContinuousRequestTypeCode)) {
            this.ValidationErrors.push("חובה להזין סוג פנייה");
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.GatepassRequest.O.OriginSiteCodeMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.Explanation)) {
            this.ValidationErrors.push("חובה להזין הסבר");
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.GatepassRequest.O.DesignateSiteCodeMandatory"));
        }

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        this.FillErrors();
        if (this.ValidationErrors.length > 0) {
            return;
        }

        SessionLocator.CurrentSession.StartBusyIndicator("");
        this.EntityPM.ContinuousRequestTypeCode = this.ContinuousRequestTypeCode;
        this.EntityPM.Explanation = this.Explanation;
        if (this.IsNewEntity) {
            this._ClaimsRelatedEntityExtendedPMService.insert(this.EntityPM).subscribe(res => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                this.SendContinuousRequestOnClaimMessage(customSendOptionsArgs);
            });
        }
        else {
            this._ClaimsRelatedEntityExtendedPMService.update(this.EntityPM).subscribe(res => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                this.SendContinuousRequestOnClaimMessage(customSendOptionsArgs);
            });
        }
    }

    SendContinuousRequestOnClaimMessage(customSendOptionsArgs: CustomSendOptionsArgs) {

        SessionLocator.CurrentSession.StartBusyIndicator("");
        var currRequestParams = new ContinuousRequestOnClaimFileRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.AppicationId = this.EntityPM.ClaimId;
        currRequestParams.ClaimRelatedEntityCounterKey = this.EntityPM.EntityCounterKey.toString();

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
                "שליחת בקשה ביטול/ערר תביעה", true)
            .then((res) => {
                //this._ClaimsRelatedEntityExtendedPMService.get(this.CourierMasterPM.Id).subscribe(rsptPMget => {
                //    let entityPMResult = rsptPMget.Result;
                //    if (entityPMResult != null) {
                //        this.IsNew = false;
                //        this.EntityPM = entityPMResult;
            }
            ).catch((err) => {
                this.ValidationErrors.push(err);
            });


        //this._CourierMasterService.PostGatepassRequestMessage(currRequestParams)
        //    .subscribe((myServiceResponse: ServiceResponse) => {
        //    });
    }
}

