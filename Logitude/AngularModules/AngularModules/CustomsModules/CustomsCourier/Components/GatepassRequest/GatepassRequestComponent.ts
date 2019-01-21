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
    CourierMasterPM: CourierMasterPM = new CourierMasterPM();
    ValidationErrorsList: any[] = [];

    //_DeclarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    UpdateCodeList;
    constructor() {
        super();

        this.UpdateCodeList =
            [
                { 'EnumId': 0, 'Name': 'No' },
                { 'EnumId': 1, 'Name': 'Yes' },
                { 'EnumId': 2, 'Name': 'All' }
            ];
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

    public get OriginSiteCode() { return this.EntityPM.OriginSiteCode; }
    public set OriginSiteCode(newValue: string) {
        this.EntityPM.OriginSiteCode = newValue;
    }

    public get DesignateSiteCode() { return this.EntityPM.DesignateSiteCode; }
    public set DesignateSiteCode(newValue: string) {
        this.EntityPM.DesignateSiteCode = newValue;
    }

    public get TransportationTypeCode() { return this.EntityPM.TransportationTypeCode; }
    public set TransportationTypeCode(newValue: string) {
        this.EntityPM.TransportationTypeCode = newValue;
    }

    //#endregion\

    _ShowUpdateCode;
    ResetUpdateCodeListChangeSelected(enumvalue) {
        this._ShowUpdateCode = enumvalue;

    }

    OkButtonClicked() {

        

    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
