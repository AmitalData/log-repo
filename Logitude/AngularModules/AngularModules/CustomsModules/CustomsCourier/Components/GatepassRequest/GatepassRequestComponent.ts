import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { GatepassRequestPM } from '../../../../Customs/EntityPMs/GatepassRequestPM';
import { GatepassRequestPMService } from '../../../../Customs/Services/StandardPMs/GatepassRequestPMService';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { GatepassRequestMessageRequestParams } from '../../../../Customs/DataContract/RequestParams/GatepassRequestMessageRequestParams';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';


@Component({
    templateUrl: './GatepassRequestComponent.html',
})

export class GatepassRequestComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.GatepassRequest";
    public EntityPM: GatepassRequestPM;
    CourierMasterPM: CourierMasterPM = new CourierMasterPM();
    ValidationErrorsList: any[] = [];
    OriginPortCode: string;
    OkButtonEnabled: boolean;
    IsNew: boolean = true;
    public HeaderScreenHeight: number = 40;

    _entityResourceService: EntityResourceService = new EntityResourceService();
    _CourierMasterService: CourierMasterService = new CourierMasterService();
    _GatepassRequestPMService: GatepassRequestPMService = new GatepassRequestPMService()

    UpdateCodeList: UpdateCodeClass[] = [{ 'EnumId': 1, 'Name': 'חדש' }, { 'EnumId': 2, 'Name': 'ביטול' } ];
    Loaded: boolean = false;
    private _MyUpdateCodeClass: UpdateCodeClass;
    public get MyUpdateCodeClass(): UpdateCodeClass {
        return this._MyUpdateCodeClass;
    }
    public set MyUpdateCodeClass(value: UpdateCodeClass) {
        this._MyUpdateCodeClass = value;
        if (this._MyUpdateCodeClass != null) {
            this.UpdateCode = this._MyUpdateCodeClass.EnumId.toString();
        }
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this.Loaded = true;
            SessionLocator.SelectedSession.StopBusyIndicator();
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CourierMasterPM = args.CourierMasterPM;
            this.EntityPM = new GatepassRequestPM();

            if (AppTool.IsNullOrEmpty(this.CourierMasterPM.MAWB)) {
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Width = 250;
                myMessageWindow.Height = 150;
                myMessageWindow.Show("לם ניתן לבצע גייטפס העברות ללם מזהה מטען");
                this.CancelButtonClicked();
            }
            this.SetGatepassRequest();
            
        }
    }

    SetGatepassRequest() {

        this._GatepassRequestPMService.get(this.CourierMasterPM.Id).subscribe(rsptPMget => {
            let entityPMResult = rsptPMget.Result;
            if (entityPMResult != null) {
                this.IsNew = false;
                this.EntityPM = entityPMResult;
                this.GatepassNumber = this.EntityPM.GatepassNumber.toString();
                this.GatepassRequestStatus = this.EntityPM.GatepassRequestStatus;
                this.SetGatepassRequestStatus();
            }
            else {
                this.EntityPM.MasterCourierId = this.CourierMasterPM.Id;
                this.EntityPM.Tenant = this.CourierMasterPM.Tenant;
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }];
                this.MyUpdateCodeClass = this.UpdateCodeList[0];//this.UpdateCode = "1";
            }
        });
    }

    SetScreenFieldsEditability(isDisplayOnly: boolean) {
        this.UIProperties.SetEnabled("OriginSiteCode", this.ObjectTableName, !isDisplayOnly);
        this.UIProperties.SetEnabled("DesignateSiteCode", this.ObjectTableName, !isDisplayOnly);
        this.UIProperties.SetEnabled("TransportationTypeCode", this.ObjectTableName, !isDisplayOnly);
    }

    //#region Properties
    public get MAWB() { return this.CourierMasterPM.MAWB; }
    public set MAWB(newValue: string) {
        this.CourierMasterPM.MAWB = newValue;
    }

    public get AirlinePrefix() { return this.CourierMasterPM.AirlinePrefix; }
    public set AirlinePrefix(newValue: string) {
        this.CourierMasterPM.AirlinePrefix = newValue;
    }

    public get HAWB() { return this.CourierMasterPM.HAWB; }
    public set HAWB(newValue: string) {
        this.CourierMasterPM.HAWB = newValue;
    }

    private _GatepassNumber: string;
    public get GatepassNumber() { return this._GatepassNumber; }
    public set GatepassNumber(newValue: string) {
        this._GatepassNumber = newValue;
    }

    private _GatepassRequestStatus: string;
    public get GatepassRequestStatus() { return this._GatepassRequestStatus; }
    public set GatepassRequestStatus(newValue: string) {
        this._GatepassRequestStatus = newValue;
    }

    private _GatepassRequestStatusName: string;
    public get GatepassRequestStatusName() { return this._GatepassRequestStatusName; }
    public set GatepassRequestStatusName(newValue: string) {
        this._GatepassRequestStatusName = newValue;
    }
    
    private _UpdateCode: string;
    public get UpdateCode() { return this._UpdateCode; }
    public set UpdateCode(newValue: string) {
        this._UpdateCode = newValue;
        if (newValue == "2") {
            this.OriginSiteCode = this.EntityPM.OriginSiteCode;
            this.DesignateSiteCode = this.EntityPM.DesignateSiteCode;
            this.TransportationTypeCode = this.EntityPM.TransportationTypeCode;
            this.SetScreenFieldsEditability(true);
        }
        else {
            this.OriginSiteCode = null;
            this.DesignateSiteCode = null;
            this.TransportationTypeCode = null;
            this.SetScreenFieldsEditability(false);
        }
    }

    private _OriginSiteCode: string;
    public get OriginSiteCode() { return this._OriginSiteCode; }
    public set OriginSiteCode(newValue: string) {
        this._OriginSiteCode = newValue;
    }

    private _DesignateSiteCode: string;
    public get DesignateSiteCode() { return this._DesignateSiteCode; }
    public set DesignateSiteCode(newValue: string) {
        this._DesignateSiteCode = newValue;
    }

    private _TransportationTypeCode: string;
    public get TransportationTypeCode() { return this._TransportationTypeCode; }
    public set TransportationTypeCode(newValue: string) {
        this._TransportationTypeCode = newValue;
    }
    //#endregion\

    _ShowUpdateCode;
    ResetUpdateCodeListChangeSelected(enumvalue) {
        this._ShowUpdateCode = enumvalue;
    }

    SetGatepassRequestStatus() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.GatepassRequestStatus)) {
            this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }];
            this.MyUpdateCodeClass = this.UpdateCodeList[0] //this.UpdateCode = "1";
        }

        switch (this.EntityPM.GatepassRequestStatus) {
            case "2":
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }];
                this.MyUpdateCodeClass = this.UpdateCodeList[0]//this.UpdateCode = "1";
                this.GatepassRequestStatusName = "בקשת העברה שגויה";
                break;
            case "4":
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }];
                //this.UpdateCode = "1";
                this.MyUpdateCodeClass = this.UpdateCodeList[0]
                this.GatepassRequestStatusName = "בקשת העברה נדחתה";
                break;
            case "7":
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }];
                //this.UpdateCode = "1";
                this.MyUpdateCodeClass = this.UpdateCodeList[0]
                this.GatepassRequestStatusName = "בקשת ביטול העברה םושרה";
                break;
            case "1":
                this.UpdateCodeList = [{ 'EnumId': 2, 'Name': 'ביטול' }];
                //this.UpdateCode = "2";
                this.MyUpdateCodeClass = this.UpdateCodeList[0]
                this.GatepassRequestStatusName = "ממתין לםישור העברה";
                break;
            case "3":
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }, { 'EnumId': 2, 'Name': 'ביטול' }];
                //this.UpdateCode = "1";
                this.MyUpdateCodeClass = this.UpdateCodeList[0]
                this.GatepassRequestStatusName = "בקשת העברה םושרה";
                break;
            case "6":
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }, { 'EnumId': 2, 'Name': 'ביטול' }];
                //this.UpdateCode = "1";
                this.MyUpdateCodeClass = this.UpdateCodeList[0]
                this.GatepassRequestStatusName = "בקשת ביטול העברה שגויה";
                break;
            case "8":
                this.UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }, { 'EnumId': 2, 'Name': 'ביטול' }];
                //this.UpdateCode = "1";
                this.MyUpdateCodeClass = this.UpdateCodeList[0]
                this.GatepassRequestStatusName = "בקשת ביטול העברה נדחתה";
                break;
            case "5":
                this.UpdateCodeList = [];
                this.MyUpdateCodeClass = null;
                this.UpdateCode = "";
                this.GatepassRequestStatusName = "ממתין לםישור ביטול העברה";
                this.UIProperties.SetEnabled("UpdateCode", this.ObjectTableName, false);
                this.SetScreenFieldsEditability(true);
                break;
        }
    }

    FillErrors() {

        var errors: string[] = [];
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.OriginSiteCode)) {
            this.ValidationErrorsList.push("חובה להזין מםתר םחסון");
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.GatepassRequest.O.OriginSiteCodeMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.DesignateSiteCode)) {
            this.ValidationErrorsList.push("חובה להזין לםתר םחסון");
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.GatepassRequest.O.DesignateSiteCodeMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.TransportationTypeCode)) {
            this.ValidationErrorsList.push("חובה להזין הובלה");
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.GatepassRequest.O.TransportationTypeCodeMandatory"));
        }


    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        SessionLocator.SelectedSession.StartBusyIndicator("");
        this.EntityPM.UpdateCode = this.UpdateCode;
        this.EntityPM.OriginSiteCode = this.OriginSiteCode;
        this.EntityPM.DesignateSiteCode = this.DesignateSiteCode;
        this.EntityPM.TransportationTypeCode = this.TransportationTypeCode;
        this.EntityPM.GatepassRequestStatus = null;
        if (this.IsNew) {
            this._GatepassRequestPMService.insert(this.EntityPM).subscribe(res => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.SendGatepassRequestMessage(customSendOptionsArgs);
            });
        }
        else {
            this._GatepassRequestPMService.update(this.EntityPM).subscribe(res => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.SendGatepassRequestMessage(customSendOptionsArgs);
            });
        }
    }

    SendGatepassRequestMessage(customSendOptionsArgs: CustomSendOptionsArgs) {

        SessionLocator.SelectedSession.StartBusyIndicator("");
        var currRequestParams = new GatepassRequestMessageRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.MasterCourierId = this.CourierMasterPM.Id;
        currRequestParams.OriginSiteCode = this.OriginSiteCode;
        currRequestParams.DesignateSiteCode = this.DesignateSiteCode;
        currRequestParams.UpdateCode = this.UpdateCode;
        currRequestParams.TransportationTypeCode = this.TransportationTypeCode;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                "שליחת בקשה העברה", true)
            .then((res) => {
                this.SetGatepassRequest();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._CourierMasterService.PostGatepassRequestMessage(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
}

export class UpdateCodeClass {
    EnumId: number;
    Name: string;
}

