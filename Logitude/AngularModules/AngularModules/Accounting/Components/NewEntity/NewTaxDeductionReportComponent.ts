
import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
import { TaxDeductionReportPMService } from '../../Services/StandardPMs/TaxDeductionReportPMService';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool } from '../../../Infrastructure/Tools';
import { UIProperties } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';


@Component({
    selector: 'NewTaxDeductionReportComponent',
    moduleId: module.id,

    templateUrl: './NewTaxDeductionReportComponent.html',
})

export class NewTaxDeductionReportComponent extends BaseComponent {


    ObjectTableName: string = "TaxDeductionReport";
    DataContext: any = this;
    entityPM: TaxDeductionReportPM = new TaxDeductionReportPM();
    TaxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        var date = new Date();
        this.entityPM.TaxYear = date.getFullYear();
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.entityPM.Email = SessionLocator.LoggedUserPM.Email;

    }

    get Email() { return this.entityPM.Email; }
    set Email(value: string) {
        if (this.entityPM.Email != value) {
            this.entityPM.Email = value;
        }
    }

    get TaxYear() { return this.entityPM.TaxYear; }
    set TaxYear(value: number) {
        if (this.entityPM.TaxYear != value) {
            this.entityPM.TaxYear = value;
        }
    }

    get IsAdditionalReportExist() { return this.entityPM.IsAdditionalReportExist; }
    set IsAdditionalReportExist(value: boolean) {
        if (this.entityPM.IsAdditionalReportExist != value) {
            this.entityPM.IsAdditionalReportExist = value;
        }
    }


    FIELD_IS_REQUIERD: string = null;
    ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        var errors: string[] = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.entityPM.TaxYear)) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxDeductionReport.F.TaxYear"));

            errors.push(s);
        }

        if (AppTool.IsNullOrEmpty(this.entityPM.Email)) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxDeductionReport.F.Email"));

            errors.push(s);
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.TaxDeductionReportPMService.insert(this.entityPM).subscribe(myResult => {

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
