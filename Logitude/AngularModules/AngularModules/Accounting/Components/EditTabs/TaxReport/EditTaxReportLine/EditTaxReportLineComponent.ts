import { Validator } from './../../../../../Infrastructure/Validators/Validator';
import { Component, OnInit, Output, EventEmitter, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { TaxReportPM } from '../../../../EntityPMs/TaxReportPM';
import { TaxReportLinePM } from '../../../../EntityPMs/TaxReportLinePM';
import { ApiQueryFilters, FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';
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
import { DateTimePipe } from '../../../../../Controls/Pipes/DateTimePipe';

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
            this.TypeFilterItems.addAdditionalFilter("Code", "I,S", null, null, "InListExact", false, false, false, "string", false, true);
            var DatePipe = new DateTimePipe();
            this.UpdateMessage = TextCodeTranslator.Translate("TaxReportLine.O.LastUpdatedBy") + " {" + this.TaxReportLinePM.UpdatedBUserName + " } " + TextCodeTranslator.Translate("TaxReportLine.O.On") + " {" + DatePipe.transform(this.TaxReportLinePM.LastUpdateDateTime, "DT") + " }";
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
       this.UIProperties.SetEnabled("LineTypeCode", this.ObjectTableName, false);

        if (this.TaxReportLinePM.OutputOrInput == "O") {
            if (this.TaxReportLinePM.StatusCode == "7") {
                this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, false);
            }
    
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferecneGroup", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferenceDate", this.ObjectTableName, false);

        }
        else {
            this.SetEnabledForReferenceField();
           
            this.UIProperties.SetEnabled("ReferecneGroup", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferenceDate", this.ObjectTableName, false);

        }
        this.UIProperties.SetRequired("TransmitStatusCode", this.ObjectTableName, !this.TransmitStatusCode);

      if (this.LineTypeCode == "I" || this.LineTypeCode == "S") {
        this.UIProperties.SetEnabled("LineTypeCode", this.ObjectTableName, true);
        if (this.LineTypeCode == "I") {
          this.UIProperties.SetEnabled("Reference", this.ObjectTableName, true);
        }
        else { this.Reference = this.TaxReportLinePM.OriginalReference; }
      }
      
        


    }
    SetEnabledForReferenceField() {
        if (this.TaxReportLinePM.StatusCode == TaxReportLineStatuse.InvoiceNumberNotValid) {
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, true);
        } else this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
    }
    //#region Buttons
    OkButtonClicked() {

        if (!this.TaxReportLinePM.TransmitStatusCode) {
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
        this.CurrentSession.StartBusyIndicatorSaving();
        this._TaxReportLinePMService.update(this.TaxReportLinePM).subscribe((myResult:any) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                // this.CurrentSession.CloseCurrentWindowEmit("ok");
                this._TaxReportPMService.update(this.TaxReportPM).subscribe((myResult:any) => {

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        // this._TaxReportLinePMService.update(this.TaxReportLinePM).subscribe((myResult:any) => {

                        //     var mm: ServiceResponse = myResult;
                        //     if (!mm.HasError) {
                        //         this.CurrentSession.CloseCurrentWindowEmit("ok");
                        //     }
                        //     else {
                        //         this.ValidationErrorsList = mm.ErrorsArray;
                        //         this.CurrentSession.StopBusyIndicator();
                        //     }
                        // });
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
    CancelButtonClicked() {

        this.TransmitStatusCode = this.OldTransmitStatusCode;
        this.VatNumber = this.OldVatNumber;
        this.Reference = this.OldReference;
        this.ReferecneGroup = this.OldReferecneGroup;
        this.ReferenceDate = this.OldReferenceDate;

        this.CurrentSession.CloseCurrentWindow();
    }
    //#endregion

}

enum TaxReportLineStatuse {
    InvoiceNumberNotValid = "3",
  
}
