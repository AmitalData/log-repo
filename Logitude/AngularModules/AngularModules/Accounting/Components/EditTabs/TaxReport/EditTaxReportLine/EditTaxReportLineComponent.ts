import { Validator } from './../../../../../Infrastructure/Validators/Validator';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { TaxReportPM } from '../../../../EntityPMs/TaxReportPM';
import { TaxReportLinePM } from '../../../../EntityPMs/TaxReportLinePM';
import { ApiQueryFilters, FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { ReconcileExternalPageExtendedPMService } from '../../../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService';
import { ReconcileExternalPagePMService } from '../../../../Services/StandardPMs/ReconcileExternalPagePMService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';
import { TaxReportPMService } from '../../../../Services/StandardPMs/TaxReportPMService';
import { TaxReportLinePMService } from '../../../../Services/StandardPMs/TaxReportLinePMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './EditTaxReportLineComponent.html'
})

export class EditTaxReportLineComponent extends BaseComponent {
    public TaxReportPM: TaxReportPM = null;
    public TaxReportLinePM: TaxReportLinePM = null;
    public ObjectTableName = "TaxReportLine";
    public DataContext = this;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];

    _TaxReportPMService: TaxReportPMService = new TaxReportPMService();
    _TaxReportLinePMService: TaxReportLinePMService = new TaxReportLinePMService();

    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    SetWindowArgs(args) {
        if (args != null) {
            this.TaxReportPM = args.TaxReportPM;
            this.TaxReportLinePM = args.TaxReportLinePM;

            // Copy original values
            this.OldTransmitStatusCode = this.TransmitStatusCode;
            this.OldVatNumber = this.VatNumber;
            this.OldReference = this.Reference;
            this.OldReferecneGroup = this.ReferecneGroup;
            this.OldReferenceDate = this.ReferenceDate;

            this.SetUIProperties();
        }
    }

    //#region Properties

    OldTransmitStatusCode: string;
    OldVatNumber: string;
    OldReference: string;
    OldReferecneGroup: string;
    OldReferenceDate: Date;

    //DeferredGLAccount
    get TransmitStatusCode() { return this.TaxReportLinePM.TransmitStatusCode; }
    set TransmitStatusCode(value: string) {
        if (this.TaxReportLinePM.TransmitStatusCode != value) {
            this.TaxReportLinePM.TransmitStatusCode = value;
        }
    }

    //VatNumber
    get VatNumber() { return this.TaxReportLinePM.VatNumber; }
    set VatNumber(value: string) {
        if (this.TaxReportLinePM.VatNumber != value) {
            this.TaxReportLinePM.VatNumber = value;
        }
    }

    //Reference
    get Reference() { return this.TaxReportLinePM.Reference; }
    set Reference(value: string) {
        if (this.TaxReportLinePM.Reference != value) {
            this.TaxReportLinePM.Reference = value;
        }
    }

    //ReferecneGroup
    get ReferecneGroup() { return this.TaxReportLinePM.ReferecneGroup; }
    set ReferecneGroup(value: string) {
        if (this.TaxReportLinePM.ReferecneGroup != value) {
            this.TaxReportLinePM.ReferecneGroup = value;
        }
    }

    //ReferenceDate
    get ReferenceDate() { return this.TaxReportLinePM.ReferenceDate; }
    set ReferenceDate(value: Date) {
        if (this.TaxReportLinePM.ReferenceDate != value) {
            this.TaxReportLinePM.ReferenceDate = value;
        }
    }


    //#endregion

    SetUIProperties() {
        if (this.TaxReportLinePM.OutputOrInput == "O") {
            this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, false);
        }
        this.UIProperties.SetRequired("TransmitStatusCode", this.ObjectTableName, true);
    }

    //#region Buttons
    OkButtonClicked() {

        if(!this.TaxReportLinePM.TransmitStatusCode){
            var fieldName: string = TextCodeTranslator.Translate('TaxReportLine.F.TransmitStatusCode');
            var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);

            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(fieldError);
            return;
        }

        // update line
        this.TaxReportLinePM.IsManuallyChanged = true;

        //update report
        this.TaxReportPM.NeedsRebulid = true;

        // save(reprot)
        SessionLocator.CurrentSession.StartBusyIndicatorSaving();
        this._TaxReportPMService.update(this.TaxReportPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError)
            {
                this._TaxReportLinePMService.update(this.TaxReportLinePM).subscribe(myResult => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        SessionLocator.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                SessionLocator.CurrentSession.StopBusyIndicator();
            }
        });

    }
    CancelButtonClicked() {

        this.TransmitStatusCode = this.OldTransmitStatusCode;
        this.VatNumber = this.OldVatNumber;
        this.Reference = this.OldReference;
        this.ReferecneGroup = this.OldReferecneGroup;
        this.ReferenceDate = this.OldReferenceDate;

        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    //#endregion

}
