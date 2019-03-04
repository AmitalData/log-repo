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

    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.entityPM = entityArgs.EntityPM;

        // this.encodeParameters();
        // this.decodeParameters();
        this.SetUIProperty();

    }
    ngOnInit() {
    }

    ReloadScreen() {
    }


    SetUIProperty() {
        this.UIProperties.SetEnabled("ResultXML", this.ObjectTableName, false);
        if(this.entityPM.Id && this.entityPM.Id != "new"){
            this.UIProperties.SetEnabled("FromMonthInclusive", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToMonthInclusive", this.ObjectTableName, false);
        }
    }

    //#region Properties

    get Tenant() { return this._parameters.Tenant; }
    set Tenant(value: number) {
        if (this.entityPM.Tenant != value) {
            this.entityPM.Tenant = value;
        }
    }

    get FromMonthInclusive () { return this.entityPM.FromMonthInclusive ; }
    set FromMonthInclusive (value: Date) {
        if (this.entityPM.FromMonthInclusive  != value) {
            this.entityPM.FromMonthInclusive  = value;
            // this.encodeParameters();

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }

        }
    }

    get ToMonthInclusive () { return this.entityPM.ToMonthInclusive ; }
    set ToMonthInclusive (value: Date) {
        if (this.entityPM.ToMonthInclusive  != value) {
            this.entityPM.ToMonthInclusive  = value;
            // this.encodeParameters();

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }
        }
    }


    get ResultXML () { return this.entityPM.ResultXML ; }
    set ResultXML (value: string) {
        if (this.entityPM.ResultXML  != value) {
            this.entityPM.ResultXML  = value;
        }
    }

    //#endregion


    //#region Date Filters Validation
    isValidate: boolean = false;
    validateDates() {
        setTimeout(() => {
            if (this.FromMonthInclusive > this.ToMonthInclusive) {

                this.UIProperties.SetValidity("ToMonthInclusive", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                this.UIProperties.SetValidity("FromMonthInclusive", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();

            } else {
                this.UIProperties.SetValidity("ToMonthInclusive", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromMonthInclusive", this.ObjectTableName, true, "");
                this.CD.detectChanges();

            }
        }, 200);
    }

    ReloadData() {
    }


}

export class IntegrityCheckParameters {
    constructor(){}
    Tenant: number;
    FromMonthInclusive: Date;
    ToMonthInclusive: Date;
}
