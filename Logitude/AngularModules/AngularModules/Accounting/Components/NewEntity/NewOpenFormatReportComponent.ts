
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
import { AppTool } from '../../../Infrastructure/Tools';
import { UIProperties } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';




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

    constructor() {
        super();
        this.entityPM.Tenant = SessionLocator.Tenant;

      
    }





    ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
  
        var errors: string[] = [];
       // this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        //if (AppTool.IsNullOrEmpty(this.entityPM.Year)) {
        //    var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxReport.F.Year"));

        //    errors.push(s);
        //}
        //if (this.SelectedMonth == null) {
        //    var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxReport.F.TaxReportMonth"));

        //    errors.push(s);
        //}


        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            SessionLocator.CurrentSession.StartBusyIndicator("");
            this.OpenFormatReportPMService.insert(this.entityPM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;

                    SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");

                    //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    //    SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                    //    .then(cmpRef => {
                    //        cmpRef.instance.ComponentRef = cmpRef;
                    //        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                    //        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    //            this.CancelButtonClicked();
                    //        });
                    //    });
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    SessionLocator.CurrentSession.StopBusyIndicator();
                }
            });


        }



    }


    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();

    }
}
