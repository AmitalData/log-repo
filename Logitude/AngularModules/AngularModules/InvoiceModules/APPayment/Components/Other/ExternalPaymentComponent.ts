import { Component } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { APPaymentPM } from '../../../../Invoice/EntityPMs/APPaymentPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { InvoiceTool } from '../../../../Invoice/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './ExternalPaymentComponent.html',
})


export class ExternalPaymentComponent extends BaseComponent {
    public EntityPM: APPaymentPM = null;
    public DataContext = this;
    public ObjectTableName: string = "APPayment";
    public ValidationErrorsList: string[] = [];
    public IsEditingEnabled: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();        
    }

    private externalPaymentAmount: number;
    private externalPaymentDate: Date;
    private externalPaymentNotes: string;
    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.IsEditingEnabled = InvoiceTool.IsEditingAPPaymentEnabled(this.EntityPM);
        this.externalPaymentAmount = this.ExternalPaymentAmount;
        this.externalPaymentDate = this.ExternalPaymentDate;
        this.externalPaymentNotes = this.ExternalPaymentNotes;
        this.Clone();
    }

    get ExternalPaymentAmount() { return this.EntityPM.ExternalPaymentAmount; }
    set ExternalPaymentAmount(value: number) {
        if (this.EntityPM.ExternalPaymentAmount != value) {
            this.EntityPM.ExternalPaymentAmount = value;
        }
    }

    get ExternalPaymentDate() { return this.EntityPM.ExternalPaymentDate; }
    set ExternalPaymentDate(value: Date) {
        if (this.EntityPM.ExternalPaymentDate != value) {
            this.EntityPM.ExternalPaymentDate = value;
        }
    }

    get ExternalPaymentNotes() { return this.EntityPM.ExternalPaymentNotes; }
    set ExternalPaymentNotes(value: string) {
        if (this.EntityPM.ExternalPaymentNotes != value) {
            this.EntityPM.ExternalPaymentNotes = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        if (!AppTool.IsNullOrEmpty(this.ExternalPaymentAmount) || !AppTool.IsNullOrEmpty(this.ExternalPaymentDate)) {

            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (AppTool.IsNullOrZero(this.ExternalPaymentAmount)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.ExternalPaymentAmount")));
            }

            if (AppTool.IsNullOrEmpty(this.ExternalPaymentDate)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("APPayment.F.ExternalPaymentDate")));
            }
        }

        if (!AppTool.IsNullOrEmpty(this.ExternalPaymentNotes)) {
            if (this.ExternalPaymentNotes.length > 250) {
                var maxError: string = TextCodeTranslator.Translate("General.M.Max");
                var fieldName: string = TextCodeTranslator.Translate("APPayment.F.ExternalPaymentNotes");

                var error: string = null;
                error = maxError.replace("%Maxlength", "250");
                error = error.replace("%FieldName", fieldName);
                errors.push(error);
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var isChanged: boolean = false;

            if (this.externalPaymentAmount != this.ExternalPaymentAmount) {
                isChanged = true;
            }

            else if (this.externalPaymentDate != this.ExternalPaymentDate) {
                isChanged = true;
            }

            else if (this.externalPaymentNotes != this.ExternalPaymentNotes) {
                isChanged = true;
            }

            if (isChanged) {
                this.CurrentSession.FireEvent("ExternalAPPaymentChanged");
            }

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ExternalPaymentAmount');
        this.myCloner.AddField('ExternalPaymentDate');
        this.myCloner.AddField('ExternalPaymentNotes');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
