import { Component } from '@angular/core';
import { AppTool,DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './APPaymentCancelationDetailsComponent.html',
})

export class APPaymentCancelationDetailsComponent extends BaseComponent {
    public EntityPM: APPaymentPM = null;


    public DataContext = this;
    public ObjectTableName: string = "APPayment";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    IsVisibile: boolean;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.Listen();
        this._entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe(response => {
            this.IsVisibile = true;
            this.EntityPM = entityArgs.EntityPM;
           this.SetUIProperties();
        });


    }
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.RefreshProperties();
                }

               
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.RefreshProperties();
                }
            });
        }
    }

    RefreshProperties() {
      // var value: Date = DateTool.GetDateFromDate(this.AccountingCancelationDate, true);
        this.AccountingCancelationDate = this.EntityPM.AccountingCancelationDate;

        this.CancelationNotes = this.EntityPM.CancelationNotes;
     


    }
    Cancelled: boolean;
    SetUIProperties() {

        this.UIProperties.SetEnabled("AccountingCancelationDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CancelationNotes", this.ObjectTableName, false);
        if (this.EntityPM.StatusCode == "VD") {
            this.Cancelled = true;
        }
        if (this.EntityPM.AccountingCancelationDate != null) {
            var AccountingCancelationDate = DateTool.GetDateFromDate(this.EntityPM.AccountingCancelationDate, true);
            var RegisterDate = DateTool.GetDateFromDate(this.EntityPM.RegisterDate, true);
            //var accountingCancellationYear = AccountingCancelationDate.getUTCFullYear();
            //var registerYear = RegisterDate.getFullYear();
            if (AccountingCancelationDate.getUTCFullYear() != RegisterDate.getUTCFullYear()) {

                this.Cancelled = false;
              //  this.EntityPM.DontIncludeInDeductionReport = true;

            }
        }



    }
    get AccountingCancelationDate() { return this.EntityPM.AccountingCancelationDate; }
    set AccountingCancelationDate(value: Date) {
        if (this.EntityPM.AccountingCancelationDate != value) {
            this.EntityPM.AccountingCancelationDate = value;
           
        }
    }
    get CancelationNotes() { return this.EntityPM.CancelationNotes; }
    set CancelationNotes(value: string) {
        if (this.EntityPM.CancelationNotes != value) {
            this.EntityPM.CancelationNotes = value;

        }
    }
    get DontIncludeInDeductionReport() { return this.EntityPM.DontIncludeInDeductionReport; }
    set DontIncludeInDeductionReport(value: boolean) {
        if (this.EntityPM.DontIncludeInDeductionReport != value) {
            this.EntityPM.DontIncludeInDeductionReport = value;
        }
    }

}
