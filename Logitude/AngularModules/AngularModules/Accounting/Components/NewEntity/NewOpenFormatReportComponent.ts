
import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { OpenFormatReportPM } from '../../EntityPMs/OpenFormatReportPM';
import { OpenFormatReportPMService } from '../../Services/StandardPMs/OpenFormatReportPMService';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { UIProperties } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { error } from 'util';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
declare var window: any;




@Component({
    selector: 'NewOpenFormatReportComponent',
    moduleId: module.id,

    templateUrl: './NewOpenFormatReportComponent.html',
})

export class NewOpenFormatReportComponent extends BaseComponent {


    ObjectTableName: string = "OpenFormatReport";
    DataContext: any = this;
    entityPM: OpenFormatReportPM = new OpenFormatReportPM();
    OpenFormatReportPMService: OpenFormatReportPMService = new OpenFormatReportPMService();
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    testingMode:any;
    constructor() {
        super();
        this.entityPM.Tenant = SessionLocator.Tenant;
        var table = window.ObjectTables.filter(d => d.Name === 'OpenFormatReport')[0];
        this.testingMode = FeatureLocator.Features.filter(f => (f.Code == "TestingMode") && f.ObjectTableId == table.Id)[0];
        
      
    }


    //get DateTypeCode() { return this.entityPM.DateTypeCode; }
    //set DateTypeCode(value: string) {
    //    if (this.entityPM.DateTypeCode != value) {
    //        this.entityPM.DateTypeCode = value;
    //    }
    //}


    get FromDate() { return this.entityPM.FromDate; }
    set FromDate(value: Date) {
        if (this.entityPM.FromDate != value) {
            this.entityPM.FromDate = value;
            if (this.ToDate < value) {
                this.entityPM.UIProperties.SetValidity("FromoDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
            }
        }
    }



    get ToDate() { return this.entityPM.ToDate; }
    set ToDate(value: Date) {
        if (this.entityPM.ToDate != value) {
            this.entityPM.ToDate = value;
            if (this.FromDate > value) {
                this.entityPM.UIProperties.SetValidity("ToDate", this.ObjectTableName,false, TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
            }
            if (value > DateTool.GetCurrentDateTimeAsUtc()) {
                this.entityPM.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FutureDateNotAllowed"));
            }
        }
    }
    get TestingMode() { return this.entityPM.TestingMode; }
    set TestingMode(value: boolean) {
        if (this.entityPM.TestingMode != value) {
            this.entityPM.TestingMode = value;
          
        }
    }

    ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
  
        var errors: string[] = [];
        //this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        if (this.ToDate < this.FromDate) {
            errors.push(TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.OpenFormatReportPMService.insert(this.entityPM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;

                    this.CurrentSession.CloseCurrentWindowEmit("ok");

                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                        this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                this.CancelButtonClicked();
                            });
                        });
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });


        }



    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }
}
