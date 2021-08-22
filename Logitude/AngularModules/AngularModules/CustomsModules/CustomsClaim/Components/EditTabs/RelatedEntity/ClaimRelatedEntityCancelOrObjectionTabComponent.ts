import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageWrapperComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ContinuousRequestOnClaimFileRequestParams } from '../../../../../Customs/DataContract/RequestParams/ContinuousRequestOnClaimFileRequestParams';
import { ContinuousResponseOnClaimFileResponseData } from '../../../../../Customs/DataContract/ResponseData/ContinuousResponseOnClaimFileResponseData';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ClaimsRelatedEntityExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/ClaimsRelatedEntityExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ClaimWebService } from '../../../../../Customs/Services/WebServices/ClaimWebService';


@Component({
    selector: 'ClaimRelatedEntityCancelOrObjectionTabComponent',
    templateUrl: './ClaimRelatedEntityCancelOrObjectionTabComponent.html',
})

export class ClaimRelatedEntityCancelOrObjectionTabComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent, OnInit{
    
    public DataContext: ClaimRelatedEntityCancelOrObjectionTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(new ClaimPM());
    public ClaimPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";

    public CurrentEditComponentId: string;
    public IsControlEnabled: boolean = true;

    ValidationErrors: string[] = [];
    _ClaimsRelatedEntityExtendedPMService: ClaimsRelatedEntityExtendedPMService = new ClaimsRelatedEntityExtendedPMService()
    _ClaimWebService: ClaimWebService = new ClaimWebService()

    SaveCompletedEvent: any;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.ValidationErrors = [];
        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
    }

    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.SelectedSession.CurrentEditComponent.ComponentId;

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                }
            });

            SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                }
            });
        }
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.ClaimsRelatedEntityPM;
            this.ClaimPM = args.ClaimPM;

            this.UIProperties.SetEnabled("ContinuousMessagesTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Note", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ClaimRequestNumber", this.ObjectTableName, false);
        }
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ContinuousRequestOnClaimFileRequestParams();
        }

        if (this.ResponseData) {
            //ContinuousResponseOnClaimFileResponseData
        }
        else {
            this.ResponseData = new ContinuousResponseOnClaimFileResponseData();
            //this.ResponseData.
        }
    }

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    }

    public get ContinuousRequestTypeCode() { return this.EntityPM.ContinuousRequestTypeCode; }
    public set ContinuousRequestTypeCode(newValue: string) { this.EntityPM.ContinuousRequestTypeCode = newValue; }

    public get Explanation() { return this.EntityPM.Explanation; }
    public set Explanation(newValue: string) { this.EntityPM.Explanation = newValue; }

    //#endregion

    //#region Response Properties
    get ContinuousMessagesTypeCode() { return this.EntityPM.ContinuousMessagesTypeCode; }
    set ContinuousMessagesTypeCode(value: string) {
        if (this.ResponseData.ContinuousMessagesTypeCode != value) {
            this.ResponseData.ContinuousMessagesTypeCode = value;
        }
    }

    get ClaimRequestNumber() { return this.EntityPM.ClaimRequestNumber; }
    set ClaimRequestNumber(value: string) {
        if (this.ResponseData.ClaimRequestNumber != value) {
            this.ResponseData.ClaimRequestNumber = value;
        }
    }

    get Note() { return this.EntityPM.Note; }
    set Note(value: string) {
        if (this.EntityPM.Note != value) {
            this.EntityPM.Note = value;
        }
    }
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

    private _IsSendContinuousRequest: boolean = false;
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        this.FillErrors();
        if (this.ValidationErrors.length > 0) {
            return;
        }

        this._IsSendContinuousRequest = true;
        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe(myResult => {
            var res: ServiceResponse = myResult;
            if (this._IsSendContinuousRequest) {
                this._IsSendContinuousRequest = false;
                if (!res.HasError) {
                    SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
                    this.SendContinuousRequestOnClaimMessage(customSendOptionsArgs);
                }
                else {
                    this.ValidationErrorsList = res.ErrorsArray;
                }
            }
            SessionLocator.SelectedSession.StopBusyIndicator();
            return false;
        });

        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
    }

    SendContinuousRequestOnClaimMessage(customSendOptionsArgs: CustomSendOptionsArgs) {

        SessionLocator.SelectedSession.StartBusyIndicator("");
        var currRequestParams = new ContinuousRequestOnClaimFileRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.AppicationId = this.EntityPM.ClaimId;
        currRequestParams.ClaimRelatedEntityCounterKey = this.EntityPM.EntityCounterKey.toString();

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                "שליחת בקשה ביטול/ערר תביעה", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrors.push(err);
            });

        this._ClaimWebService.PostSendContinuousRequestOnClaim(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
        });
    }
}

