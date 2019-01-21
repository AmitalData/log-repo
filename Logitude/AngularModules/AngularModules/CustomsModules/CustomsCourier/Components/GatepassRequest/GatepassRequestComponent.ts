import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { DeclarationCourierStatusPMService } from '../../../../Customs/Services/StandardPMs/DeclarationCourierStatusPMService';


@Component({
    moduleId: module.id,
    templateUrl: './GatepassRequestComponent.html',
})

export class GatepassRequestComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.GatepassRequest";
    //public EntityPM: GatepassRequestPM;
    CourierMasterPM: CourierMasterPM = new CourierMasterPM();
    ValidationErrorsList: any[] = [];

    //_DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    UpdateCodeList = [{ 'EnumId': 1, 'Name': 'חדש' }, { 'EnumId': 2, 'Name': 'ביטול' } ];

    constructor() {
        super();            
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.CourierMasterPM = args.CourierMasterPM;
            this.SetScreenFieldsEditability(true);
        }
    }

    SetScreenFieldsEditability(isDisplayOnly: boolean) {
        this.UIProperties.SetEnabled("OriginSiteCode", this.ObjectTableName, !isDisplayOnly);
        this.UIProperties.SetEnabled("DesignateSiteCode", this.ObjectTableName, !isDisplayOnly);
        this.UIProperties.SetEnabled("TransportationTypeCode", this.ObjectTableName, !isDisplayOnly);
    }

    //#region Properties
    //private _CourierHawb: string;
    //public get CourierHawb() { return this.CourierMasterPM.HAWB; }
    //public set CourierHawb(newValue: string) {
    //    this.CourierMasterPM.HAWB = newValue;
    //}

    public get GatepassNumber() { return this.EntityPM.GatepassNumber; }
    public set GatepassNumber(newValue: string) {
        this.EntityPM.GatepassNumber = newValue;
    }

    //public get OriginSiteCode() { return this.EntityPM.OriginSiteCode; }
    //public set OriginSiteCode(newValue: string) {
    //    this.EntityPM.OriginSiteCode = newValue;
    //}

    private _OriginSiteCode: string;
    public get OriginSiteCode() { return this._OriginSiteCode; }
    public set OriginSiteCode(newValue: string) {
        this._OriginSiteCode = newValue;
    }

    //public get DesignateSiteCode() { return this.EntityPM.DesignateSiteCode; }
    //public set DesignateSiteCode(newValue: string) {
    //    this.EntityPM.DesignateSiteCode = newValue;
    //}

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

    OkButtonClicked() {

        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
