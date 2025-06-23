import { Component} from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxReportPM } from '../../../../EntityPMs/TaxReportPM';
import { TaxReportLinePM } from '../../../../EntityPMs/TaxReportLinePM';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';
import { TaxReportPMService } from '../../../../Services/StandardPMs/TaxReportPMService';
import { TaxReportLinePMService } from '../../../../Services/StandardPMs/TaxReportLinePMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { DateTimePipe } from '../../../../../Controls/Pipes/DateTimePipe';
import { TaxReportExtendedPMService } from 'Accounting/Services/ExtendedPMs/TaxReportExtendedPMService';



@Component({

    templateUrl: './EditTaxReportLineComponent.html'
})

export class EditTaxReportLineComponent extends BaseComponent {
    public TaxReportPM: TaxReportPM = null;
    public TaxReportLinePM: TaxReportLinePM = null;
    public ObjectTableName = "TaxReportLine";
    public DataContext = this;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    public TypeFilterItems: ApiQueryFilters = new ApiQueryFilters();
    public UpdateMessage: string;
    _TaxReportPMService: TaxReportPMService = new TaxReportPMService();
    _TaxReportLinePMService: TaxReportLinePMService = new TaxReportLinePMService();
    private CurrentSession = SessionLocator.SelectedSession;
    private lastUpdatedByText = TextCodeTranslator.Translate("TaxReportLine.O.LastUpdatedBy");
    private onText = TextCodeTranslator.Translate("TaxReportLine.O.On");
   
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
            this.OldConfirmationNumber = this.ConfirmationNumber;
            this.OldReferecneGroup = this.ReferecneGroup;
            this.OldReferenceDate = this.ReferenceDate;
            this.TypeFilterItems.addAdditionalFilter("Code", "I,S", null, null, "InListExact", false, false, false, "string", false, true);
            this.SetUpdatedByMessage();
            
            this.SetUIProperties();
           
        }
    }

    //#region Properties

    OldTransmitStatusCode: string;
    OldVatNumber: string;
    OldReference: string;
    OldReferecneGroup: string;
    OldReferenceDate: Date;
    OldConfirmationNumber: string;

    private SetUpdatedByMessage() {
        if (this.TaxReportLinePM.IsManuallyChanged) {
            var datePipe = new DateTimePipe();
            var formatedLastUpdatedDate = datePipe.transform(this.TaxReportLinePM.LastUpdateDateTime, "DT");
            this.UpdateMessage = `${this.lastUpdatedByText} {${this.TaxReportLinePM.UpdatedBUserName}} ${this.onText} {${formatedLastUpdatedDate}}`;
        }
    }

    //DeferredGLAccount
    get TransmitStatusCode() { return this.TaxReportLinePM.TransmitStatusCode; }
    set TransmitStatusCode(value: string) {
        if (this.TaxReportLinePM.TransmitStatusCode != value) {
            this.TaxReportLinePM.TransmitStatusCode = value;
            this.SetUIProperties();
        }
    }

    //type
    get LineTypeCode() { return this.TaxReportLinePM.LineTypeCode; }
    set LineTypeCode(value: string) {
        if (this.TaxReportLinePM.LineTypeCode != value) {
            this.TaxReportLinePM.LineTypeCode = value;
            this.SetUIProperties();
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
            this.TaxReportLinePM.OriginalReference = value;
        }
    }
    get ConfirmationNumber() { return this.TaxReportLinePM.ConfirmationNumber; }
    set ConfirmationNumber(value: string) {
        if (this.TaxReportLinePM.ConfirmationNumber != value) {
            this.TaxReportLinePM.ConfirmationNumber = value;
        }
    }
    //PreviousReference
    get PreviousReference() { return this.TaxReportLinePM.PreviousReference == null ? null : this.TaxReportLinePM.PreviousReference; }
    set PreviousReference(value: string) {
        if (this.TaxReportLinePM.PreviousReference != value) {
            this.TaxReportLinePM.PreviousReference = value;
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
    IsReferenceEditable: boolean = false;
    SetUIProperties() {
       
        this.UIProperties.SetEnabled("LineTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PreviousReference", this.ObjectTableName, false);
        this.SetEnabledForReferenceField();
        
        
        if (this.TaxReportLinePM.OutputOrInput == "O") {
            if (this.TaxReportLinePM.StatusCode == "7") {
                this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, false);
            }


            //    this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferecneGroup", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferenceDate", this.ObjectTableName, false);
        }
        else {

            this.SetEnabledForReferenceFieldForInputLines();

            this.UIProperties.SetEnabled("ReferecneGroup", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferenceDate", this.ObjectTableName, false);

        }
        this.UIProperties.SetRequired("TransmitStatusCode", this.ObjectTableName, !this.TransmitStatusCode);
        this.UIProperties.SetRequired("ConfirmationNumber", this.ObjectTableName, !this.TransmitStatusCode);

        if (this.LineTypeCode == "I" || this.LineTypeCode == "S") {
            this.UIProperties.SetEnabled("LineTypeCode", this.ObjectTableName, true);
            if (this.LineTypeCode != "I") { this.Reference = this.TaxReportLinePM.OriginalReference; }
        }
        if(this.TaxReportLinePM.IsExternalLine){

            this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LineTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);


        }
        this.UIProperties.SetEnabled("ConfirmationNumber", this.ObjectTableName, true);
    }
   
    SetEnabledForReferenceField() {

        if (!AppTool.IsNullOrEmpty(this.PreviousReference) || this.PreviousReference != " ") {
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, true);
            this.IsReferenceEditable = true;
        } else this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
    }
    SetEnabledForReferenceFieldForInputLines() {
        if (this.TaxReportLinePM.StatusCode == TaxReportLineStatuse.InvoiceNumberNotValid || this.IsReferenceEditable) {
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, true);
        } else this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
    }
    //#region Buttons
    OkButtonClicked() {

        if (!this.TaxReportLinePM.IsDirty)
            return this.CurrentSession.CloseCurrentWindowEmit("ok");
        var fieldName: string
        this.ValidationErrorsList = [];
        if(this.ConfirmationNumber?.length <9 && this.ConfirmationNumber?.length>0)
        {
            var fieldError: string = TextCodeTranslator.Translate("TaxReportLine.O.ConfirmationMinNineDigits");
            this.ValidationErrorsList.push(fieldError);
        }
        if (!this.TaxReportLinePM.TransmitStatusCode) {
            var fieldName: string = TextCodeTranslator.Translate('TaxReportLine.F.TransmitStatusCode');
            var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);

            
            this.ValidationErrorsList.push(fieldError);
            
        }
        if(this.ValidationErrorsList?.length>0)
        return;
        // update line
        this.TaxReportLinePM.IsManuallyChanged = true;

        //update report
        this.TaxReportPM.NeedsRebulid = true;

        // save(reprot)
        this.CurrentSession.StartBusyIndicatorSaving();
        this.SetPreviousReference();

        this._TaxReportLinePMService.update(this.TaxReportLinePM).subscribe((myResult: any) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                // this.CurrentSession.CloseCurrentWindowEmit("ok");
                var taxReportExtendedPMService = new TaxReportExtendedPMService();
                this.TaxReportPM.IsEdited = true;
                taxReportExtendedPMService.PutTaxReportIsEdited(this.TaxReportPM).subscribe((myResult: any) => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });



    }

    SetPreviousReference() {
        var isReferenceChanged = this.OldReference != this.Reference;
        if (isReferenceChanged) {
            this.PreviousReference = this.PreviousReference == null ? this.OldReference : this.PreviousReference;
            this.OldReference = this.Reference;
        }
    }

    CancelButtonClicked() {

        this.TransmitStatusCode = this.OldTransmitStatusCode;
        this.VatNumber = this.OldVatNumber;
        this.Reference = this.OldReference;
        this.ReferecneGroup = this.OldReferecneGroup;
        this.ReferenceDate = this.OldReferenceDate;
        this.ConfirmationNumber = this.OldConfirmationNumber;

        this.CurrentSession.CloseCurrentWindow();
    }
    //#endregion

}

enum TaxReportLineStatuse {
    InvoiceNumberNotValid = "3",

}
