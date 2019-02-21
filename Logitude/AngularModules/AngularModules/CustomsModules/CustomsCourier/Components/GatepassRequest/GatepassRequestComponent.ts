import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { GatepassRequestPM } from '../../../../Customs/EntityPMs/GatepassRequestPM';
import { GatepassRequestPMService } from '../../../../Customs/Services/StandardPMs/GatepassRequestPMService';
import { CourierMasterService } from '../../../../Customs/Services/others/CourierMasterService';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { GatepassRequestMessageRequestParams } from '../../../../Customs/DataContract/RequestParams/GatepassRequestMessageRequestParams';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
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

    _CourierMasterService: CourierMasterService = new CourierMasterService();
    UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }, { 'EnumId': 2, 'Name': 'ביטול' } ];

    constructor() {
        super();            
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CourierMasterPM = args.CourierMasterPM;
            this.EntityPM = new GatepassRequestPM();
            this.UpdateCode = "1";

            if (AppTool.IsNullOrEmpty(this.CourierMasterPM.MAWB)) {
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Width = 250;
                myMessageWindow.Height = 150;
                myMessageWindow.Show("לא ניתן לבצע גייטפס העברות ללא מזהה מטען");
                this.CancelButtonClicked();
            }

            let myGatepassRequestPMService: GatepassRequestPMService = new GatepassRequestPMService()
            myGatepassRequestPMService.get(this.CourierMasterPM.Id).subscribe(rsptPMget => {
                let entityPM = rsptPMget.Result;
                if (entityPM != null) {
                    this.EntityPM = entityPM;
                    //this.SetGatepassRequestStatus();
                }
            });
            //this.SetScreenFieldsEditability(true);
        }
    }

    SetScreenFieldsEditability(isDisplayOnly: boolean) {
        this.UIProperties.SetEnabled("OriginSiteCode", this.ObjectTableName, !isDisplayOnly);
        this.UIProperties.SetEnabled("DesignateSiteCode", this.ObjectTableName, !isDisplayOnly);
        this.UIProperties.SetEnabled("TransportationTypeCode", this.ObjectTableName, !isDisplayOnly);
    }

    //#region Properties
    private _UpdateCode: string;
    public get UpdateCode() { return this._UpdateCode; }
    public set UpdateCode(newValue: string) {
        this._UpdateCode = newValue;
    }

    public get GatepassNumber() { return this.EntityPM.GatepassNumber; }
    public set GatepassNumber(newValue: number) {
        this.EntityPM.GatepassNumber = newValue;
    }

    public get OriginSiteCode() { return this.EntityPM.OriginSiteCode; }
    public set OriginSiteCode(newValue: string) {
        this.EntityPM.OriginSiteCode = newValue;
    }

    //private _OriginSiteCode: string;
    //public get OriginSiteCode() { return this._OriginSiteCode; }
    //public set OriginSiteCode(newValue: string) {
    //    this._OriginSiteCode = newValue;
    //}

    public get DesignateSiteCode() { return this.EntityPM.DesignateSiteCode; }
    public set DesignateSiteCode(newValue: string) {
        this.EntityPM.DesignateSiteCode = newValue;
    }

    //private _DesignateSiteCode: string;
    //public get DesignateSiteCode() { return this._DesignateSiteCode; }
    //public set DesignateSiteCode(newValue: string) {
    //    this._DesignateSiteCode = newValue;
    //}

    public get TransportationTypeCode() { return this.EntityPM.TransportationTypeCode; }
    public set TransportationTypeCode(newValue: string) {
        this.EntityPM.TransportationTypeCode = newValue;
    }

    //private _TransportationTypeCode: string;
    //public get TransportationTypeCode() { return this._TransportationTypeCode; }
    //public set TransportationTypeCode(newValue: string) {
    //    this._TransportationTypeCode = newValue;
    //}

    //#endregion\

    _ShowUpdateCode;
    ResetUpdateCodeListChangeSelected(enumvalue) {
        this._ShowUpdateCode = enumvalue;

    }

    FillErrors() {

        var errors: string[] = [];
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.OriginSiteCode)) {
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.FromDateMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.DesignateSiteCode)) {
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.ToDateMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.TransportationTypeCode)) {
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.StorageSiteNumberMandatory"));
        }


    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        SessionLocator.CurrentSession.StartBusyIndicator("");
        var currRequestParams = new GatepassRequestMessageRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.MasterCourierId = this.CourierMasterPM.Id;
        currRequestParams.OriginSiteCode = this.OriginSiteCode;
        currRequestParams.DesignateSiteCode = this.DesignateSiteCode;
        //currRequestParams.UpdateCode = this.UpdateCode;
        currRequestParams.TransportationTypeCode = this.TransportationTypeCode;
        
        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
                "שליחת בקשה העברה", true)
            .then((res) => {
                //this.ResponseData = res;
                //this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._CourierMasterService.PostGatepassRequestMessage(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
