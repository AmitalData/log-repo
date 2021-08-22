import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TapagMessagesService } from '../../../../Customs/Services/WebServices/TapagMessagesService';
import { FaultProceduralRequestParams } from '../../../../Customs/DataContract/RequestParams/FaultProceduralRequestParams';
import { FaultProceduralResponseData, FaultGeneralDetailResult } from '../../../../Customs/DataContract/ResponseData/FaultProceduralResponseData';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ClientList} from '../../../../Customs/EntityLists/ClientList';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomsSettingListService } from '../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';

import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

@Component({
    selector: 'FaultQueryComponent',
    
    templateUrl: './FaultQueryComponent.html',
})

export class FaultQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: FaultQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public _ImporterName: string;
    public _IsImporerCodeEnabled: boolean = false;

    _TapagMessagesService: TapagMessagesService = new TapagMessagesService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    _PartnersDomainService: PartnersDomainService = new PartnersDomainService();

    public FaultGeneralDetailList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.FaultGeneralDetailList = new ObservableCollection([]);
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
            this.RequestParams = new FaultProceduralRequestParams();
            this.UIProperties.SetRequired("StartDate", null, true);

            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
                .subscribe((customsSettingList: any) => {
                    if (customsSettingList) {
                        this.AgentExternalID = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                        this.UIProperties.SetRequired("AgentExternalID", null, false);
                    }
                });
        }

        if (this.ResponseData && this.ResponseData.FaultGeneralDetailList) {
            for (let item of this.ResponseData.FaultGeneralDetailList) {
                item.FaultAdittionalInformationListObs = new ObservableCollection(item.FaultAdittionalInformationList);
            }
            this.FaultGeneralDetailList.InsertCollection(this.ResponseData.FaultGeneralDetailList); 
        }
    }

    OnRowLoaded(myRow: any) {
        if (myRow) {

            myRow.SetExpandaple(true);
        }
    }

    //#region Properties
    get FaultCode() { return this.RequestParams.ProceduralFaultCode; }
    set FaultCode(value: string) {
        if (this.RequestParams.ProceduralFaultCode != value) {
            this.RequestParams.ProceduralFaultCode = value;
        }
    }

    get AgentExternalID() { return this.RequestParams.AgentExternalID; }
    set AgentExternalID(value: string) {
        if (this.RequestParams.AgentExternalID != value) {
            this.RequestParams.AgentExternalID = value;
        }
        if (value) {
            this.UIProperties.SetEnabled("AgentExternalID", null, false);
        }
        else {
            this.UIProperties.SetEnabled("AgentExternalID", null, true);
        }
    }

    get StartDate() { return this.RequestParams.StartDate; }
    set StartDate(value: Date) {
        if (this.RequestParams.StartDate != value) {
            this.RequestParams.StartDate = value;
        }
        if (value && !AppTool.IsNullOrEmpty(this.EndDate)) {
            this.UIProperties.SetRequired("StartDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("StartDate", null, true);
        }
    }

    get EndDate() { return this.RequestParams.EndDate; }
    set EndDate(value: Date) {
        if (this.RequestParams.EndDate != value) {
            this.RequestParams.EndDate = value;
        }
        if (value && !AppTool.IsNullOrEmpty(this.StartDate)) {
            this.UIProperties.SetRequired("StartDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("StartDate", null, true);
        }
    }

    get CustomFileNo() { return this.RequestParams.CustomsFile; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomsFile != value) {
            this.RequestParams.CustomsFile = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams.DeclarationNumber; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
        }
    }

    get IsImporerCodeEnabled() { return this._IsImporerCodeEnabled; }
    set IsImporerCodeEnabled(newValue: boolean) {
        if (this._IsImporerCodeEnabled != newValue) {
            this._IsImporerCodeEnabled = newValue;
        }
    }

    get CustomerId() { return this.RequestParams.CustomerId; }
    set CustomerId(value: string) {
        if (this.RequestParams.CustomerId != value) {
            this.RequestParams.CustomerId = value;
        }
        if (value) {
            this._PartnersDomainService.GetCustomerById(value)
                .subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ImporterCode = myResponse.Result.VatNumber;
                    this.IsImporerCodeEnabled = true;
                }
            });
        }
        else {
            this.ImporterCode = "";
            this.IsImporerCodeEnabled = false;
        }
    }

    get ImporterCode() { return this.RequestParams.ImporterExternalID; }
    set ImporterCode(value: string) {
        if (this.RequestParams.ImporterExternalID != value) {
            this.RequestParams.ImporterExternalID = value;
        }
    }

    get ImporterName() { return this._ImporterName; }
    set ImporterName(value: string) {
        if (this._ImporterName != value) {
            this._ImporterName = value;
        }
    }
    //#endregion


    //#region Importer Commands
    ImporterClicked(type, client: ClientList) {
        this.ImporterCode = client.Code;
        this.ImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
    }

    ImporterTextChanged(type, item) {
        if (item == "") {
            this.ImporterCode = null;
            this.ImporterName = "";
        }
    }
    //#endregion


    //#region Declaration Commands

    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {

            this.CustomFileNo = "";
        }
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }

        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);
            });
    }

    DeclarationNumberTextChanged(DeclarationNumberText: string): void {
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }

        this.DueChangeClearChildField(false);

        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                this.FetchDeclaration(myResponse, false);

            });
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        } else {

            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            } else {
                this.SetValidityDeclarationNumber();
            }

        }
    }

    SetValidityDeclarationNumber() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }

    //#endregion

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.RequestParams.StartDate)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.StartDateIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (AppTool.IsNullOrEmpty(this.RequestParams.EndDate)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.EndDateIsMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.RequestParams.AgentExternalID)) {
            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.AgentExternalIDIsMandatory");
            this.ValidationErrorsList.push(msg);
        }

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.FaultGeneralDetailList.Clear();

        var currRequestParams = new FaultProceduralRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CustomerId = this.CustomerId;
        currRequestParams.ImporterExternalID = this.ImporterCode;
        currRequestParams.CustomsFile = this.CustomFileNo;
        currRequestParams.DeclarationNumber = this.DeclarationNumber;
        currRequestParams.AgentExternalID = this.AgentExternalID;
        currRequestParams.ProceduralFaultCode = this.FaultCode;
        currRequestParams.StartDate = this.StartDate;
        currRequestParams.EndDate = this.EndDate;

        /*currRequestParams.Client = this.GuarantorID;     
        currRequestParams.ImporterId = this.GuarantorID;
        currRequestParams.ImporterCode = this.GuarantorID;
        currRequestParams.ImporterName = this.GuarantorID;
*/

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא ליקויים", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._TapagMessagesService.PostFaultQueryRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion 
}
