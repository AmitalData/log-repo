import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AccountingIntegrityCheckPM } from '../../../EntityPMs/AccountingIntegrityCheckPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';

import { builder } from "xmlbuilder";

@Component({
    moduleId: module.id,
    templateUrl: './IntegrityCheckTabComponent.html',
})

export class IntegrityCheckTabComponent extends BaseComponent implements OnInit {

    public entityPM: AccountingIntegrityCheckPM = null;
    public ObjectTableName = "AccountingIntegrityCheck";
    public DataContext = this;
    public isRTL: boolean = false;
    public showLocals: boolean = false;

    public _parameters: IntegrityCheckParameters = new IntegrityCheckParameters();

    constructor(private entityArgs: EntityArgs) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.entityPM = entityArgs.EntityPM;

        this.encodeParameters();
        this.decodeParameters();
        this.SetUIProperty();

    }
    ngOnInit() {
    }

    ReloadScreen() {
    }


    SetUIProperty() {
        this.UIProperties.SetEnabled("ResultXML", this.ObjectTableName, false);
    }

    decodeParameters(){
        var xmlString = this.entityPM.ParametersXML;
        if(xmlString)
        {
            var tenant = xmlString.substring(xmlString.indexOf('<Tenant>')+8,xmlString.indexOf('</Tenant>'));
            var __FromMonthInclusive = xmlString.substring(xmlString.indexOf('<FromMonthInclusive>')+20,xmlString.indexOf('</FromMonthInclusive>'));
            var __ToMonthInclusive = xmlString.substring(xmlString.indexOf('<ToMonthInclusive>')+18,xmlString.indexOf('</ToMonthInclusive>'));

            if(__FromMonthInclusive) this.FromMonthInclusive = new Date(__FromMonthInclusive);
            if(__ToMonthInclusive) this.ToMonthInclusive = new Date(__ToMonthInclusive);
        }
        console.log(tenant,__FromMonthInclusive,__ToMonthInclusive)

    }
    encodeParameters(){

        if (this.entityPM && this.FromMonthInclusive && this.ToMonthInclusive) {
            var xmlString =
                `<?xml version="1.0" encoding="UTF-8"?>
                <AccountingIntegrityInParam>
                <Tenant>#Tenant</Tenant>
                <FromMonthInclusive>#FromMonthInclusive</FromMonthInclusive>
                <ToMonthInclusive>#ToMonthInclusive</ToMonthInclusive>
                </AccountingIntegrityInParam>`;

            xmlString = xmlString.replace('#Tenant', this.entityPM.Tenant.toString());
            xmlString = xmlString.replace('#FromMonthInclusive', this.FromMonthInclusive.toString());
            xmlString = xmlString.replace('#ToMonthInclusive', this.ToMonthInclusive.toString());

            this.entityPM.ParametersXML = xmlString;
            console.log(xmlString);
        }
    }


    //#region Properties

    get Tenant() { return this._parameters.Tenant; }
    set Tenant(value: number) {
        if (this._parameters.Tenant != value) {
            this._parameters.Tenant = value;
        }
    }

    get FromMonthInclusive () { return this._parameters.FromMonthInclusive ; }
    set FromMonthInclusive (value: Date) {
        if (this._parameters.FromMonthInclusive  != value) {
            this._parameters.FromMonthInclusive  = value;
            this.encodeParameters();
        }
    }

    get ToMonthInclusive () { return this._parameters.ToMonthInclusive ; }
    set ToMonthInclusive (value: Date) {
        if (this._parameters.ToMonthInclusive  != value) {
            this._parameters.ToMonthInclusive  = value;
            this.encodeParameters();
        }
    }

    //#endregion


    ReloadData() {
    }


}

export class IntegrityCheckParameters {
    constructor(){}
    Tenant: number;
    FromMonthInclusive: Date;
    ToMonthInclusive: Date;
}
